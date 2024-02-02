using System.Runtime.InteropServices;
using System.Text;
using MathNet.Numerics;
using Serilog;

namespace JTSkimmer
{
  public class RtlSdrDevice : BaseSdrDevice
  {
    private IntPtr Handle;
    private NativeRtlSdr.RtlSdrReadAsyncCb CallbackFunctionDelegate = new(CallbackFunction);
    private Thread? Thread;
    
    public RtlSdrDevice(SdrInfo info) : base(info) { }

    protected override void Start()
    {
      var sett = Info.RtlSdrSettings;

      int index = NativeRtlSdr.rtlsdr_get_index_by_serial(Info.SerialNumber);
      if (index < 0) throw new Exception();

      int rc = NativeRtlSdr.rtlsdr_open(out Handle, (uint)index);
      if (rc != 0) throw new Exception();

      rc = NativeRtlSdr.rtlsdr_set_agc_mode(Handle, sett.Rtl2832Agc);
      if (rc != 0) throw new Exception();
      rc = NativeRtlSdr.rtlsdr_set_tuner_gain_mode(Handle, !sett.IFAgc);
      if (rc != 0) throw new Exception();

      GetGains();
      if (!sett.IFAgc) SetGainIndex(sett.GainIndex);

      rc = NativeRtlSdr.rtlsdr_set_sample_rate(Handle, sett.SamplingRate);
      if (rc != 0) throw new Exception();

      rc = NativeRtlSdr.rtlsdr_reset_buffer(Handle);
      if (rc != 0) throw new Exception();

      Thread = new Thread(new ThreadStart(ThreadProcedure));
      Thread.Name = "RTL-SDR";
      Thread.IsBackground = true;
      Thread.Start();

      if (Frequency > 0) Frequency = Frequency;
    }

    private void ThreadProcedure()
    {
      int rc = NativeRtlSdr.rtlsdr_read_async(Handle, CallbackFunctionDelegate, gcHandle, 0, 0);
    }

    protected override void Stop()
    {
      if (Handle == IntPtr.Zero) return;

      NativeRtlSdr.rtlsdr_cancel_async(Handle);
      Thread?.Join();
      Thread = null;

      NativeRtlSdr.rtlsdr_close(Handle);
      Handle = IntPtr.Zero;
    }

    protected override bool IsRunning()
    {
      return Thread?.ThreadState == System.Threading.ThreadState.Background;
    }

    protected override void SetFrequency(uint frequency)
    {
      if (Handle == IntPtr.Zero) return;
      var rc = NativeRtlSdr.rtlsdr_set_center_freq(Handle, frequency);
      if (rc != 0) throw new Exception("Unable to set RTL-SDR frequency");
    }

    private byte[] bytes = Array.Empty<byte>();

    private static void CallbackFunction(IntPtr buf, uint len, GCHandle ctx)
    {
      try
      {
        if (ctx.Target == null) throw new Exception("device is null");
        RtlSdrDevice device = (RtlSdrDevice)ctx.Target;

        if (len != device.bytes.Length) device.bytes = new byte[len];
        Marshal.Copy(buf, device.bytes, 0, (int)len);

        uint count = len / 2;
        if (count != device.Data.Length) device.Data = new Complex32[count];

        for (int i = 0; i < count; i++)
          device.Data[i] = new Complex32(
            (device.bytes[i * 2] - 127) / 128f, 
            (device.bytes[i * 2 + 1] - 127) / 128f
            );

        device.OnDataAvailable();
      }
      catch (Exception e)
      {
        Log.Error(e, $"Exception in the RTL-SDR callback function");
      }
    }

    private void GetGains()
    {
      if (Info.RtlSdrSettings.IFAgc)
      {
        Gains = Array.Empty<float>();
        return;
      }

      int count = NativeRtlSdr.rtlsdr_get_tuner_gains(Handle, null);
      var gains = new int[count];
      int rc = NativeRtlSdr.rtlsdr_get_tuner_gains(Handle, gains);
      Gains = gains.Select(g => g * 0.1f).ToArray();
    }

    protected override void SetGainIndex(uint gainIndex)
    {
      var sett = Info.RtlSdrSettings;
      if (sett.IFAgc) return;

      sett.GainIndex = (uint)Math.Min(Gains.Length-1, Math.Max(0, gainIndex));
      int intGain = (int)(Gains[sett.GainIndex] * 10);

      if (Handle != IntPtr.Zero) NativeRtlSdr.rtlsdr_set_tuner_gain(Handle, intGain);
    }
 
    internal static List<SdrInfo> ListDevices()
    {
      var result = new List<SdrInfo> ();

      try
      {
        uint count = NativeRtlSdr.rtlsdr_get_device_count();

        for (uint i = 0; i < count; i++)
        {
          StringBuilder manufact = new(256);
          StringBuilder product = new(256);
          StringBuilder serial = new(256);
          int rc = NativeRtlSdr.rtlsdr_get_device_usb_strings(i, manufact, product, serial);
          if (rc != 0) continue;
          result.Add(new SdrInfo(SdrType.RtlSdr, $"{manufact} {product}", serial.ToString()));
        }
      }
      catch (Exception e)
      {
        Log.Error(e, "Error listing RTL-SDR devices");
      }

      return result;
    }
  }
}
