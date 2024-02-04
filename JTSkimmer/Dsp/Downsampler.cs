using System.Diagnostics;
using MathNet.Numerics;
using VE3NEA;


namespace JTSkimmer
{
  internal unsafe class Downsampler : ThreadedProcessor<Complex32>
  {
    private const float STOPBAND_REJECTION_DB = 80;
    private const float USEFUL_BANDWIDTH = 0.97f;

    private NativeLiquidDsp.msresamp2_crcf* resamp;
    private Complex32[] InBuffer = Array.Empty<Complex32>();
    private Complex32[] OutBuffer = Array.Empty<Complex32>();
    private int InCount;

    internal NoiseBlanker NoiseBlanker;
    internal int DecimationFactor { get; private set; }
    internal double OutputSamplingRate { get; private set; }

    public event EventHandler<DataEventArgs<Complex32>>? DataAvailable;


    internal Downsampler(double inputSamplingRate, double outputSamplingRate)
    {
      uint stageCount = (uint)Math.Truncate(Math.Log2(inputSamplingRate / outputSamplingRate));
      DecimationFactor = 1 << (int)stageCount;
      OutputSamplingRate = inputSamplingRate / DecimationFactor;
      Debug.Assert(OutputSamplingRate == inputSamplingRate / DecimationFactor);

      if (DecimationFactor > 1)
        resamp = NativeLiquidDsp.msresamp2_crcf_create(NativeLiquidDsp.LiquidResampType.LIQUID_RESAMP_DECIM,
          stageCount, 0.5f * USEFUL_BANDWIDTH, 0, STOPBAND_REJECTION_DB);

      NoiseBlanker = new NoiseBlanker(inputSamplingRate);
    }


    private readonly DataEventArgs<Complex32> Args = new ();

    protected override void Process(DataEventArgs<Complex32> args)
    {
      NoiseBlanker.Process(args.Data);

      // no decimation is needed
      if (DecimationFactor == 1)
      {
        DataAvailable?.Invoke(this, args);
        return;
      }

      // this may happen only in the first few calls
      int spaceNeeded = InCount + args.Data.Length;
      if (spaceNeeded > InBuffer.Length) Array.Resize(ref InBuffer, spaceNeeded);

      Array.Copy(args.Data, 0, InBuffer, InCount, args.Data.Length);
      InCount += args.Data.Length;

      int outCount = InCount / DecimationFactor;
      if (outCount == 0) return;
      if (outCount != OutBuffer.Length) OutBuffer = new Complex32[outCount];

      fixed (Complex32* pInBuffer = InBuffer)
      fixed (Complex32* pOutBuffer = OutBuffer)
        for (int i = 0; i < OutBuffer.Length; i++)
        {
          Complex32* pBlock = pInBuffer + i * DecimationFactor;
          int rc = NativeLiquidDsp.msresamp2_crcf_execute(resamp, pBlock, out pOutBuffer[i]);
          if (rc != 0) throw new Exception($"{rc}");
        }

      int usedCount = outCount * DecimationFactor;
      InCount -= usedCount;
      if (InCount > 0) Array.Copy(InBuffer, usedCount, InBuffer, 0, InCount);

      Args.SetValues(OutBuffer, args.ReceivedAt);
      DataAvailable?.Invoke(this, Args);
    }

    public override void Dispose()
    {
      base.Dispose();

      if (resamp != null) NativeLiquidDsp.msresamp2_crcf_destroy(resamp);
      resamp = null;
    }
  }
}