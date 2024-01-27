using MathNet.Numerics;
using VE3NEA;


namespace JTSkimmer
{
  public unsafe class Slicer : ThreadedProcessor<Complex32>
  {
    const int STOPBAND_REJECTION_DB = 60;

    private NativeLiquidDsp.nco_crcf* nco;
    private NativeLiquidDsp.msresamp_crcf* resamp;
    private NativeLiquidDsp.ampmodem* modem;

    private Complex32[] iqBuffer1 = Array.Empty<Complex32>();
    private Complex32[] iqBuffer2 = Array.Empty<Complex32>();
    private float[] audioBuffer = Array.Empty<float>();
    private DataEventArgsPool<float> ArgsPool = new();
    private float InputRate;
    internal float NoiseFloor;

    public event EventHandler<DataEventArgs<float>>? DataAvailable;


    public Slicer(float inputRate, float frequencyOffset)
    {
      InputRate = inputRate;
      SetUpMixer(frequencyOffset);

      resamp = NativeLiquidDsp.msresamp_crcf_create(SdrConst.AUDIO_SAMPLING_RATE / inputRate, STOPBAND_REJECTION_DB);
      modem = NativeLiquidDsp.ampmodem_create(1, NativeLiquidDsp.LiquidAmpmodemType.LIQUID_AMPMODEM_USB, 1);
    }

    protected override void Process(DataEventArgs<Complex32> args)
    {
      
      //ensure buffers
      uint inputCount = (uint)args.Data.Length;
      uint outputCount = (uint)Math.Ceiling(inputCount / InputRate * SdrConst.AUDIO_SAMPLING_RATE) + 1;
      if (iqBuffer1.Length < inputCount) iqBuffer1 = new Complex32[inputCount];
      if (iqBuffer2.Length < outputCount) iqBuffer2 = new Complex32[outputCount];
      if (audioBuffer.Length < outputCount) audioBuffer = new float[outputCount];

      // process
      fixed (Complex32* pdata = args.Data)
      fixed (Complex32* pbuffer1 = iqBuffer1)
      fixed (Complex32* pbuffer2 = iqBuffer2)
      fixed (float* paudioBuffer = audioBuffer)
      {
        // mix down
        NativeLiquidDsp.nco_crcf_mix_block_down(nco, pdata, pbuffer1, inputCount);

        // downsample
        NativeLiquidDsp.msresamp_crcf_execute(resamp, pbuffer1, inputCount, pbuffer2, out outputCount);

        // demodulate ssb
        NativeLiquidDsp.ampmodem_demodulate_block(modem, pbuffer2, outputCount, paudioBuffer);
      }

      // outputCount may be different in each call, get a buffer of the right size
      var outputArgs = ArgsPool.Rent((int)outputCount);
      for (int i = 0; i < outputCount; i++) outputArgs.Data[i] = audioBuffer[i] * 1000;
      outputArgs.ReceivedAt = args.ReceivedAt;
      DataAvailable?.Invoke(this, outputArgs);
      ArgsPool.Return(outputArgs);
    }

    public override void Dispose()
    {
      base.Dispose();

      if (nco != null) NativeLiquidDsp.nco_crcf_destroy(nco);
      if (resamp != null) NativeLiquidDsp.msresamp_crcf_destroy(resamp);
      if (modem != null) NativeLiquidDsp.ampmodem_destroy(modem);

      nco = null;
      resamp = null;
      modem = null;
    }

    internal void SetUpMixer(float offset)
    {
      if (nco != null) NativeLiquidDsp.nco_crcf_destroy(nco);
      nco = NativeLiquidDsp.nco_crcf_create(NativeLiquidDsp.LiquidNcoType.LIQUID_NCO);
      NativeLiquidDsp.nco_crcf_set_frequency(nco, SdrConst.TWO_PI * offset / InputRate);
    }
  }
}
