using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CSCore.Codecs.WAV;
using CSCore.XAudio2;
using MathNet.Numerics;
using VE3NEA;


namespace JTSkimmer
{
  public unsafe class Slicer : ThreadedProcessor<Complex32>
  {
    private const int STOPBAND_REJECTION_DB = 80;
    private const float USEFUL_BANDWIDTH = 0.99f;
    private const int FILTER_DELAY = 15; // this is the default in LiquidDsp, adjust if needed
    private static readonly Complex32[] quarterFcSinusoid = { new(1, 0), new(0, 1), new(-1, 0), new(0, -1) };

    // typed pointers do not work in ths static fields, use IntPtr instead
    private static IntPtr msresamp2Prototype;
    private static IntPtr rresampPrototype;
    private static bool prototypesCreated;

    private static double InputRate;
    private static int  OctaveDecimationFactor, RationalInterpolationFactor, RationalDecimationFactor;

    private NativeLiquidDsp.nco_crcf* nco;
    private NativeLiquidDsp.msresamp2_crcf* msresamp2;
    private NativeLiquidDsp.rresamp_cccf* rresamp;

    private FifoBuffer<Complex32> InputBuffer = new();
    private FifoBuffer<Complex32> OctaveResamplerInputBuffer = new();
    private FifoBuffer<Complex32> RationalResamplerInputBuffer = new();
    private FifoBuffer<Complex32> RationalResamplerOutputBuffer = new();
    private DataEventArgsPool<float> ArgsPool = new();

    public event EventHandler<DataEventArgs<float>>? DataAvailable;

    public Slicer(double inputRate, double frequencyOffset)
    {
      InputRate = inputRate;
      SetUpMixer(frequencyOffset);

      // resampler creation is slow, create prototypes once and copy when needed
      EnsurePrototypes(inputRate);
      if (msresamp2Prototype == IntPtr.Zero)
        msresamp2 = null;
      else
        msresamp2 = NativeLiquidDsp.msresamp2_crcf_copy((NativeLiquidDsp.msresamp2_crcf*)msresamp2Prototype);

      rresamp = NativeLiquidDsp.rresamp_cccf_copy((NativeLiquidDsp.rresamp_cccf*)rresampPrototype);
    }

    private static void EnsurePrototypes(double inputRate)
    {
      if (prototypesCreated) return;

      double octaveResamplerOutputRate = CreateOctaveResampler(inputRate);
      CreateRationalResampler(octaveResamplerOutputRate);

      prototypesCreated = true;
    }

    private static double CreateOctaveResampler(double inputRate)
    {
      int octaveStageCount = (int)Math.Truncate(Math.Log2(inputRate / SdrConst.AUDIO_SAMPLING_RATE));
      OctaveDecimationFactor = 1 << octaveStageCount;
      double octaveResamplerOutputRate = inputRate / OctaveDecimationFactor;

      // do not downsample directly to the audio rate, allow the rational resampler to do its work
      if (octaveResamplerOutputRate == SdrConst.AUDIO_SAMPLING_RATE)
      {
        OctaveDecimationFactor /= 2;
        octaveResamplerOutputRate *= 2;
      }

      if (OctaveDecimationFactor == 1)
      {
        msresamp2Prototype = IntPtr.Zero;
        return octaveResamplerOutputRate;
      }

      msresamp2Prototype = (IntPtr)NativeLiquidDsp.msresamp2_crcf_create(
        NativeLiquidDsp.LiquidResampType.LIQUID_RESAMP_DECIM,
        (uint)octaveStageCount,
        0.5f * USEFUL_BANDWIDTH,
        0,
        STOPBAND_REJECTION_DB
        );

      return octaveResamplerOutputRate;
    }

    private static void CreateRationalResampler(double inputRate)
    {
      double rationalResamplingFactor = SdrConst.AUDIO_SAMPLING_RATE / inputRate;
      Debug.Assert(rationalResamplingFactor >= 0.5 && rationalResamplingFactor < 1);

      // float to rational
      (RationalInterpolationFactor, RationalDecimationFactor) = Dsp.ApproximateRatio(rationalResamplingFactor, 1e-4);

      // design lowpass filter, -3..3 kHz passband
      uint filterLength = (uint)(2 * FILTER_DELAY * RationalDecimationFactor + 1);
      var filter = NativeLiquidDsp.firfilt_cccf_create_kaiser(filterLength, 0.25f, STOPBAND_REJECTION_DB, 0);
      var filterCoeffs = NativeLiquidDsp.firfilt_cccf_get_coefficients(filter);

      // shift filter passband in frequency to 0..6 kHz
      // this filter not only limits the signal bandwidth before decimation
      // but also demodulates SSB by suppressing -6..0 kHz
      for (int i = 0; i < filterLength; i++) filterCoeffs[i] *= quarterFcSinusoid[i % 4];

      // create resampler/demodulator
      rresampPrototype = (IntPtr)NativeLiquidDsp.rresamp_cccf_create(
        (uint)RationalInterpolationFactor,
        (uint)RationalDecimationFactor,
        FILTER_DELAY,
        filterCoeffs
        );

      // the filter itself is no longer needed
      NativeLiquidDsp.firfilt_cccf_destroy(filter);
    }

    public static void DeletePrototypes()
    {
      if (msresamp2Prototype != IntPtr.Zero) NativeLiquidDsp.msresamp2_crcf_destroy((NativeLiquidDsp.msresamp2_crcf*)msresamp2Prototype);
      if (rresampPrototype != IntPtr.Zero) NativeLiquidDsp.rresamp_cccf_destroy((NativeLiquidDsp.rresamp_cccf*)rresampPrototype);
      prototypesCreated = false;
    }

    public void SetUpMixer(double offset)
    {
      if (nco != null) NativeLiquidDsp.nco_crcf_destroy(nco);
      nco = NativeLiquidDsp.nco_crcf_create(NativeLiquidDsp.LiquidNcoType.LIQUID_NCO);
      NativeLiquidDsp.nco_crcf_set_frequency(nco, (float)(SdrConst.TWO_PI * offset / InputRate));
    }

    protected override void Process(DataEventArgs<Complex32> args)
    {
      InputBuffer.Data = args.Data;
      InputBuffer.Count = args.Data.Length;

      // mix down to baseband
      fixed (Complex32* pData = InputBuffer.Data)
      {
        NativeLiquidDsp.nco_crcf_mix_block_down(nco, pData, pData, (uint)InputBuffer.Count);
      }

      if (msresamp2 == null)
        ApplyRationalResampler(InputBuffer);
      else
      {
        ApplyOctaveResampler(InputBuffer);
        ApplyRationalResampler(RationalResamplerInputBuffer);
      }


      // return the real part
      int outputCount = RationalResamplerOutputBuffer.Count;
      var outputArgs = ArgsPool.Rent(outputCount);
      for (int i = 0; i < outputCount; i++) outputArgs.Data[i] = RationalResamplerOutputBuffer.Data[i].Real * 4000;
      RationalResamplerOutputBuffer.Count = 0;
      outputArgs.ReceivedAt = args.ReceivedAt;
      DataAvailable?.Invoke(this, outputArgs);
      ArgsPool.Return(outputArgs);

    }

    public void ApplyOctaveResampler(FifoBuffer<Complex32> buffer)
    {
      OctaveResamplerInputBuffer.Append(buffer);
      int blockCount = OctaveResamplerInputBuffer.Count / OctaveDecimationFactor;
      RationalResamplerInputBuffer.EnsureExtraSpace(blockCount);

      fixed (Complex32* pInBuffer = OctaveResamplerInputBuffer.Data)
      fixed (Complex32* pOutBuffer = RationalResamplerInputBuffer.Data)
      {
        Complex32* pBlock = pInBuffer;
        Complex32* pOut = pOutBuffer + RationalResamplerInputBuffer.Count;

        for (int blockNo = 0; blockNo < blockCount; blockNo++)
        {
          int rc = NativeLiquidDsp.msresamp2_crcf_execute(msresamp2, pBlock, out pOut[blockNo]);
          if (rc != 0) throw new Exception($"LiquidDsp error {rc}");
          pBlock += OctaveDecimationFactor;
        }
      }

      OctaveResamplerInputBuffer.Dump(blockCount * OctaveDecimationFactor);
      RationalResamplerInputBuffer.Count += blockCount;
    }


    private void ApplyRationalResampler(FifoBuffer<Complex32> buffer)
    {
      int blockCount = buffer.Count / RationalDecimationFactor;
      if (blockCount == 0) return;

      int consumedCount = blockCount * RationalDecimationFactor;
      int outputCount = blockCount * RationalInterpolationFactor;
      RationalResamplerOutputBuffer.EnsureExtraSpace(outputCount);

      fixed (Complex32* pInBuffer = buffer.Data)
      fixed (Complex32* pOutBuffer = RationalResamplerOutputBuffer.Data)
      {
        Complex32* pIn = pInBuffer;
        Complex32* pOut = pOutBuffer;

        for (int blockNo = 0; blockNo < blockCount; blockNo++)
        {
          int rc = NativeLiquidDsp.rresamp_cccf_execute(rresamp, pIn, pOut);
          //if (rc != 0) throw new Exception($"LiquidDsp error {rc}");
          pIn += RationalDecimationFactor;
          pOut += RationalInterpolationFactor;
        }
      }

      buffer.Dump(consumedCount);
      RationalResamplerOutputBuffer.Count += outputCount;
    }

    public override void Dispose()
    {
      base.Dispose();

      if (nco != null) NativeLiquidDsp.nco_crcf_destroy(nco);
      if (msresamp2 != null) NativeLiquidDsp.msresamp2_crcf_destroy(msresamp2);
      if (rresamp != null) NativeLiquidDsp.rresamp_cccf_destroy(rresamp);

      nco = null;
      msresamp2 = null;
      rresamp = null;
    }
  }
}
