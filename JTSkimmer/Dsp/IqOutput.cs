using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CSCore;
using CSCore.Tags.ID3.Frames;
using CSCore.XAudio2;
using MathNet.Numerics;
using Newtonsoft.Json.Linq;
using Serilog;
using VE3NEA;

namespace JTSkimmer
{
  internal unsafe class IqOutput : ThreadedProcessor<Complex32>
  {
    const int STOPBAND_REJECTION_DB = 90;

    private readonly Context ctx;
    private NativeLiquidDsp.nco_crcf* nco;
    private NativeLiquidDsp.msresamp_crcf* resamp;
    private UdpClient? UdpClient;
    private float ResamplingFactor;

    private Complex32[] OutputBuffer = Array.Empty<Complex32>();
    private uint OutputCount;
    private LinradDatagram Datagram = new();
    private readonly byte[] DatagramBytes = new byte[Marshal.SizeOf<LinradDatagram>()];
    private IntPtr MemoryPtr = IntPtr.Zero;

    public bool Active { get => UdpClient != null; }
    public string LastError { get; protected set; }


    public IqOutput(Context ctx)
    {
      this.ctx = ctx;
    }

    public void Start()
    {
      Stop();

      if (ctx.Sdr == null || ctx.Downsampler == null) return;

      var sett = ctx.Settings.IqOutput;

      int inputRate = (int)ctx.Downsampler.OutputSamplingRate;
      int outputRate = LinradDatagram.SAMPLING_RATE;
      float offset = (float)(sett.CenterFrequencykHz * 1e3 - ctx.Sdr.Frequency);

      if (Math.Abs(offset) > inputRate / 2 - outputRate / 2)
      {
        Stop();
        Enabled = true; // enabled but not active, show error
        LastError = "Center frequency is out of band";
        return;
      }

      nco = NativeLiquidDsp.nco_crcf_create(NativeLiquidDsp.LiquidNcoType.LIQUID_NCO);
      NativeLiquidDsp.nco_crcf_set_frequency(nco, SdrConst.TWO_PI * offset / inputRate);

      ResamplingFactor = outputRate / (float)inputRate;
      resamp = NativeLiquidDsp.msresamp_crcf_create(ResamplingFactor, STOPBAND_REJECTION_DB);

      MemoryPtr = Marshal.AllocHGlobal(DatagramBytes.Length);

      UdpClient = new();
      try
      {
        var ipAddress = IPAddress.Parse(sett.Host);
        if (Utils.IsAddressMulticast(ipAddress)) UdpClient.JoinMulticastGroup(ipAddress);
        UdpClient.Connect(new IPEndPoint(ipAddress, sett.Port));
      }
      catch (Exception e)
      {
        Stop();
        LastError = e.Message;
        Log.Error(e,"Error starting IqOutput");
      }

      Enabled = true;
    }

    public void Stop()
    {
      Enabled = false;

      if (nco != null) NativeLiquidDsp.nco_crcf_destroy(nco);
      if (resamp != null) NativeLiquidDsp.msresamp_crcf_destroy(resamp);
      UdpClient?.Dispose();

      if (MemoryPtr != IntPtr.Zero)
      {
        Marshal.FreeHGlobal(MemoryPtr);
        MemoryPtr = IntPtr.Zero;
      }

      nco = null;
      resamp = null;
      UdpClient = null;
    }

    public void ApplySettings()
    {
      if (ctx.Settings.IqOutput.Enabled) Start(); else Stop();
      ctx.BandViewPanel?.ScalePanel?.Invalidate();
    }


    protected override void Process(DataEventArgs<Complex32> args)
    {
      if (!Active) return;

      uint inputCount = (uint)args.Data.Length;
      uint newOutputCount = (uint)Math.Ceiling(inputCount * ResamplingFactor) + 1;

      if (OutputBuffer.Length < OutputCount + newOutputCount)
        OutputBuffer = new Complex32[OutputCount + newOutputCount];

      fixed (Complex32* pInput = args.Data)
      fixed (Complex32* pOutput = OutputBuffer)
      {
        // mix down
        NativeLiquidDsp.nco_crcf_mix_block_down(nco, pInput, pInput, inputCount);

        // downsample
        NativeLiquidDsp.msresamp_crcf_execute(resamp, pInput, inputCount, pOutput + OutputCount, out newOutputCount);
        OutputCount += newOutputCount;
      }

      Datagram.time = (int)args.ReceivedAt.TimeOfDay.TotalMilliseconds;
      SendDatagrams();
    }

    private void SendDatagrams()
    {
      Datagram.passband_center = ctx.Settings.IqOutput.CenterFrequencykHz / 1e3;

      float scale = Dsp.FromDb2(ctx.Settings.IqOutput.Gain) * 70000f;
      uint blockStart = 0;
      uint blockSize = LinradDatagram.SAMPLES_PER_DATAGRAM;
      while (blockStart + blockSize <= OutputCount)
      {
        for (int i = 0; i < LinradDatagram.SAMPLES_PER_DATAGRAM; i++)
        {
          Datagram.buf[i] = OutputBuffer[blockStart + i] * scale;

          // this is what qmap.exe expects
          Datagram.buf[i] = new Complex32(Datagram.buf[i].Real, -Datagram.buf[i].Imaginary);
          if ((i & 1) == 0) Datagram.buf[i] *= -1;
        }

        Datagram.block_no++;
        blockStart += LinradDatagram.SAMPLES_PER_DATAGRAM;

        StructToBytes();
        UdpClient.Send(DatagramBytes);
      }

      if (blockStart < OutputCount) Array.Copy(OutputBuffer, blockStart, OutputBuffer, 0, OutputCount - blockStart);
      OutputCount -= blockStart;
    }

    private void StructToBytes()
    {
        Marshal.StructureToPtr(Datagram, MemoryPtr, true);
        Marshal.Copy(MemoryPtr, DatagramBytes, 0, DatagramBytes.Length);
    }

    public override void Dispose()
    {
      Enabled = false;
      base.Dispose();
      Stop();
    }
  }
}
