using MathNet.Numerics;
using static VE3NEA.NativeAirspy;
using System.Text;
using Serilog;

namespace JTSkimmer
{
  public unsafe class AirspyDevice : BaseSdrDevice
  {

    private IntPtr Handle = IntPtr.Zero;
    private readonly AirspySampleBlockCbFn CallbackFunctionDelegate = new (CallbackFunction);

    public AirspyDevice(SdrInfo info) : base(info) {}

    protected override void Start()
    {
      AirspySettings sett = Info.AirspySettings;

      if (sett.GainMode == AirspyGainMode.Custom)
        Gains = Array.Empty<float>();
      else
        Gains = Enumerable.Range(0, 21).Select(gain => (float)gain).ToArray();


      AirspyError rc;
      rc = airspy_open_sn(out Handle, long.Parse(Info.SerialNumber));
      if (rc != AirspyError.AIRSPY_SUCCESS) throw new Exception();

      switch (sett.GainMode)
      {
        case AirspyGainMode.Linearity:
        case AirspyGainMode.Sensitivity:
          rc = airspy_set_lna_agc(Handle, false);
          if (rc != AirspyError.AIRSPY_SUCCESS) throw new Exception();
          rc = airspy_set_mixer_agc(Handle, false);
          if (rc != AirspyError.AIRSPY_SUCCESS) throw new Exception();
          SetGainIndex(sett.GainIndex);
          break;

        case AirspyGainMode.Custom:
          rc = airspy_set_lna_agc(Handle, sett.LnaAGC);
          if (rc != AirspyError.AIRSPY_SUCCESS) throw new Exception();
          rc = airspy_set_mixer_agc(Handle, sett.MixerAGC);
          if (rc != AirspyError.AIRSPY_SUCCESS) throw new Exception();
          rc = airspy_set_vga_gain(Handle, sett.VgaGain);
          if (rc != AirspyError.AIRSPY_SUCCESS) throw new Exception();

          if (!sett.LnaAGC) rc = airspy_set_lna_gain(Handle, sett.LnaGain);
          if (rc != AirspyError.AIRSPY_SUCCESS) throw new Exception();
          if (!sett.MixerAGC) rc = airspy_set_mixer_gain(Handle, sett.MixerGain);
          if (rc != AirspyError.AIRSPY_SUCCESS) throw new Exception();
          break;
      }

      rc = airspy_set_sample_type(Handle, AirspySampleType.AIRSPY_SAMPLE_FLOAT32_IQ);
      if (rc != AirspyError.AIRSPY_SUCCESS) throw new Exception();
      rc = airspy_set_samplerate(Handle, SamplingRate);
      if (rc != AirspyError.AIRSPY_SUCCESS) throw new Exception();
      rc = airspy_set_rf_bias(Handle, sett.BiasTEnabled);
      if (rc != AirspyError.AIRSPY_SUCCESS) throw new Exception();

      rc = airspy_start_rx(Handle, CallbackFunctionDelegate, gcHandle);
      if (rc != AirspyError.AIRSPY_SUCCESS) throw new Exception();

      if (Frequency > 0) Frequency = Frequency;
    }

    protected override void Stop()
    {
      if (Handle != IntPtr.Zero)
      {
        airspy_stop_rx(Handle);
        airspy_close(Handle);
        Handle = IntPtr.Zero;
      }
    }

    protected override void SetFrequency(uint frequency)
    {
      var rc = airspy_set_freq(Handle, frequency);
      if (rc != AirspyError.AIRSPY_SUCCESS) throw new Exception("Unable to set Airspy frequency");
    }

    private static AirspyError CallbackFunction(ref AirspyTransfer transfer)
    {
      try
      {
        // get device 
        if (transfer.sample_type != AirspySampleType.AIRSPY_SAMPLE_FLOAT32_IQ) throw new Exception("Wrong data format");
        if (transfer.ctx.Target == null) throw new Exception("Device not available");
        AirspyDevice device = (AirspyDevice)transfer.ctx.Target;

        // create buffer
        int sampleCount = transfer.sample_count;
        if (sampleCount != device.Data.Length) device.Data = new Complex32[sampleCount];

        // copy data
        int byteCount = sampleCount * sizeof(Complex32);
        fixed (Complex32* pData = device.Data)
          Buffer.MemoryCopy(transfer.samples, pData, byteCount, byteCount);

        device.OnDataAvailable();
      }
      catch (Exception e)
      {
        Log.Error(e, "Exception in the Airspy callback function");
      }

      return AirspyError.AIRSPY_SUCCESS; // means continue
    }

    protected override bool IsRunning()
    {
      return Handle != IntPtr.Zero && airspy_is_streaming(Handle) == AirspyError.AIRSPY_TRUE;
    }

    internal static List<SdrInfo> ListDevices()
    {
      var result = new List<SdrInfo>();

      try
      {
        int count = airspy_list_devices(null, 0);
        long[] serials = new long[count];
        count = airspy_list_devices(serials, count);
        if (count != serials.Length) Array.Resize(ref serials, count);

        foreach (long serial in serials)
        {
          AirspyError rc = airspy_open_sn(out IntPtr handle, serial);
          if (rc != AirspyError.AIRSPY_SUCCESS) continue;
          try
          {
            StringBuilder name = new(255);
            rc = airspy_version_string_read(handle, name, 255);
            if (rc != AirspyError.AIRSPY_SUCCESS) continue;
            var type = name.ToString().ToUpper().Contains("MINI") ? SdrType.AirspyMini : SdrType.AirspyR2;
            result.Add(new SdrInfo(type, name.ToString(), serial.ToString()));
          }
          finally
          {
            airspy_close(handle);
          }
        }
      }
      catch (Exception e)
      {
        Log.Error(e, "Error listing Airspy devices");
      }

      return result;
    }

    protected override void SetGainIndex(uint gainIndex)
    {
      if (Gains.Length == 0) return;

      var sett = Info.AirspySettings;
      sett.GainIndex = (uint)Math.Min(Gains.Length-1, Math.Max(0, gainIndex));

      AirspyError rc;

      if (sett.GainMode == AirspyGainMode.Sensitivity)
        rc = airspy_set_sensitivity_gain(Handle, (byte)sett.GainIndex);
      else if (sett.GainMode == AirspyGainMode.Linearity)
        rc = airspy_set_linearity_gain(Handle, (byte)sett.GainIndex);
      else
        rc = AirspyError.AIRSPY_SUCCESS;

      if (rc != AirspyError.AIRSPY_SUCCESS) throw new Exception("Unable to set Airspy gain");
    }
  }
}