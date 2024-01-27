using System.Diagnostics;
using System.Runtime.InteropServices;
using MathNet.Numerics;
using Serilog;
using UN7ZO.HamCockpitPlugins.SDRPlaySource;
using static UN7ZO.HamCockpitPlugins.SDRPlaySource.NativeMethods;
using static UN7ZO.HamCockpitPlugins.SDRPlaySource.SDRplayAPI_Callback;


namespace JTSkimmer
{
  internal class SdrPlayDevice : BaseSdrDevice
  {
    private const float SDRPLAY_API_VERSION = 3.11f;

    private static readonly Dictionary<int, string> SdrplayModels = new Dictionary<int, string>()
    {
      {SDRPLAY_RSP1_ID, "SDRplay RSP1" },
      {SDRPLAY_RSP1A_ID, "SDRplay RSP1a" },
      {SDRPLAY_RSP2_ID, "SDRplay RSP2" },
      {SDRPLAY_RSPduo_ID, "SDRplay RSP Duo" },
      {SDRPLAY_RSPdx_ID, "SDRplay RSP DX" },
    };

    private static bool ApiOK;
    private sdrplay_api_DeviceT Device;
    private sdrplay_api_DeviceParamsT DeviceParams;
    private sdrplay_api_DevParamsT DevParams;
    private sdrplay_api_RxChannelParamsT RxChannel;

    internal bool running;


    public SdrPlayDevice(SdrInfo info) : base(info)
    {
      ApiOK = OpenApi();
    }

    public override void Dispose()
    {
      base.Dispose();
      if (ApiOK) sdrplay_api_Close();
    }

    protected override void Start()
    {
      if (!ApiOK) throw new Exception("API could not be opened");

      if (Info.SdrPlaySettings.Agc)
        Gains = Array.Empty<float>();
      else
        Gains = Enumerable.Range(0, 40).Select(index=> 59f - index).ToArray();

      var rc = sdrplay_api_LockDeviceApi();
      CheckSuccess(rc);

      try
      {
        // find device by SN
        var devices = new sdrplay_api_DeviceT[SDRPLAY_MAX_DEVICES];
        rc = sdrplay_api_GetDevices(devices, out uint deviceCount, (uint)devices.Length);
        CheckSuccess(rc);

        // throw exception if sn not found
        Device = devices.First(d => GetSerialNumber(d) == Info.SerialNumber);

        // open device
        rc = sdrplay_api_SelectDevice(ref Device);
        CheckSuccess(rc);
      }
      finally
      {
        sdrplay_api_UnlockDeviceApi();
      }

      // device parameters
      rc = sdrplay_api_GetDeviceParams(Device.dev, out IntPtr deviceParamsPtr);
      CheckSuccess(rc);
      DeviceParams = Marshal.PtrToStructure<sdrplay_api_DeviceParamsT>(deviceParamsPtr);
      DevParams = Marshal.PtrToStructure<sdrplay_api_DevParamsT>(DeviceParams.devParams);
      RxChannel = Marshal.PtrToStructure<sdrplay_api_RxChannelParamsT>(DeviceParams.rxChannelA);

      DevParams.fsFreq.fsHz = Info.SdrPlaySettings.SamplingRate * 3; // sdrplay divides SR by 3 - not documented
      RxChannel.tunerParams.rfFreq.rfHz = Info.SdrPlaySettings.CenterFrequency * (1 + Info.Settings.Ppm * 1e-6);
      RxChannel.tunerParams.bwType = SDRplayAPI_Tuner.sdrplay_api_Bw_MHzT.sdrplay_api_BW_1_536;
      RxChannel.tunerParams.ifType = SDRplayAPI_Tuner.sdrplay_api_If_kHzT.sdrplay_api_IF_1_620;

      if (!Info.SdrPlaySettings.Agc) RxChannel.tunerParams.gain.gRdB = (int)Gains[GainIndex]; //20..59
      RxChannel.ctrlParams.agc.enable = Info.SdrPlaySettings.Agc ? 
        sdrplay_api_AgcControlT.sdrplay_api_AGC_50HZ : sdrplay_api_AgcControlT.sdrplay_api_AGC_DISABLE;

      Marshal.StructureToPtr(DevParams, DeviceParams.devParams, false);
      Marshal.StructureToPtr(RxChannel, DeviceParams.rxChannelA, false);


      // start
      sdrplay_api_CallbackFnsT cbFns = new();
      cbFns.StreamACbFn = StreamCallback;
      cbFns.StreamBCbFn = StreamCallback;
      cbFns.EventCbFn = EventCallback;
      rc = sdrplay_api_Init(Device.dev, ref cbFns, (IntPtr)gcHandle);
      CheckSuccess(rc);

      running = true;
    }

    protected override void Stop()
    {
      if (!running) return;

      running = false;

      sdrplay_api_Uninit(Device.dev);
      sdrplay_api_ReleaseDevice(ref Device);
    }

    protected override bool IsRunning()
    {
      return running;
    }

    private static void EventCallback(sdrplay_api_EventT eventId, SDRplayAPI_Tuner.sdrplay_api_TunerSelectT tuner, ref sdrplay_api_EventParamsT Params, nint cbContext)
    {
      var device = (SdrPlayDevice)((GCHandle)cbContext).Target;
      if (eventId == sdrplay_api_EventT.sdrplay_api_DeviceRemoved) device?.Stop();
    }

    internal short[] RawDataI = Array.Empty<short>();
    internal short[] RawDataQ = Array.Empty<short>();

    private static void StreamCallback(nint xi, nint xq, ref sdrplay_api_StreamCbParamsT Params, uint numSamples, uint reset, nint cbContext)
    {
      try
      {
        var device = (SdrPlayDevice)((GCHandle)cbContext).Target;

        if (device.RawDataI.Length < numSamples)
        {
          device.RawDataI = new short[numSamples];
          device.RawDataQ = new short[numSamples];
        }

        Marshal.Copy(xi, device.RawDataI, 0, (int)numSamples);
        Marshal.Copy(xq, device.RawDataQ, 0, (int)numSamples);

        if (numSamples != device.Data.Length) device.Data = new Complex32[numSamples];
        float scale = 1f / short.MaxValue;

        for (int i = 0; i < numSamples; i++)
          device.Data[i] = new Complex32(device.RawDataI[i], device.RawDataQ[i]) * scale;

        device.OnDataAvailable();
      }
      catch (Exception e)
      {
        Log.Error(e, $"Exception in the SDRplay callback function");
      }
    }

    protected override void SetFrequency(uint frequency)
    {
      RxChannel.tunerParams.rfFreq.rfHz = Info.SdrPlaySettings.CenterFrequency;
      Marshal.StructureToPtr(RxChannel, DeviceParams.rxChannelA, false);

      var rc = sdrplay_api_Update(Device.dev, Device.tuner, 
        sdrplay_api_ReasonForUpdateT.sdrplay_api_Update_Tuner_Frf, 
        sdrplay_api_ReasonForUpdateExtension1T.sdrplay_api_Update_Ext1_None);
      CheckSuccess(rc);
    }

    protected override void SetGainIndex(uint gainIndex)
    {
      if (Gains.Length == 0) return;

      gainIndex = (uint)Math.Min(Gains.Length - 1, Math.Max(0, gainIndex));
      Info.SdrPlaySettings.GainIndex = gainIndex;

      if (Running)
      {
        RxChannel.tunerParams.gain.gRdB = (int)Gains[gainIndex];
        Marshal.StructureToPtr(RxChannel, DeviceParams.rxChannelA, false);

        var rc = sdrplay_api_Update(Device.dev, Device.tuner,
          sdrplay_api_ReasonForUpdateT.sdrplay_api_Update_Tuner_Gr,
          sdrplay_api_ReasonForUpdateExtension1T.sdrplay_api_Update_Ext1_None);
        CheckSuccess(rc);
      }
    }

    internal static IEnumerable<SdrInfo> ListDevices()
    {
      List<SdrInfo> result = new();
      if (!OpenApi()) return result;

      try
      {
        var rc = sdrplay_api_LockDeviceApi();
        if (!CheckSuccess(rc, true)) return result;

        try
        {
          var devices = new sdrplay_api_DeviceT[SDRPLAY_MAX_DEVICES];
          rc = sdrplay_api_GetDevices(devices, out uint deviceCount, (uint)devices.Length);
          if (!CheckSuccess(rc, true)) return result;

          for (int i = 0; i < deviceCount; i++)
            if (devices[i].hwVer != SDRPLAY_RSPduo_ID) // {!} no support for duo yet
              result.Add(new(SdrType.SdrPlay, SdrplayModels[devices[i].hwVer], GetSerialNumber(devices[i])));
        }
        finally
        {
          sdrplay_api_UnlockDeviceApi();
        }
      }
      finally
      {
        sdrplay_api_Close();
      }
      return result;
    }

    private static bool CheckSuccess(sdrplay_api_ErrT rc, bool silent = false)
    {
      if (rc == sdrplay_api_ErrT.sdrplay_api_Success) return true;

      string error;

      try
      {
        IntPtr errorPtr = sdrplay_api_GetErrorString(rc);
        error = Marshal.PtrToStringAnsi(errorPtr);
      }
      catch
      {
        error = $"SDRplay api error {rc}";
      }

      if (silent)
      {
        try { Log.Error($"API call error: {error}\n{new StackTrace(true)}"); } catch { }
        return false;
      }

      else throw new Exception(error);
    }

    private static bool OpenApi()
    {
      var rc = sdrplay_api_Open();
      if (!CheckSuccess(rc, true)) return false;

      rc = sdrplay_api_ApiVersion(out float version);
      if (!CheckSuccess(rc, true))
      {
        sdrplay_api_Close();
        return false;
      }

      if (version != SDRPLAY_API_VERSION)
        Log.Warning($"SDRplay API version mismatch: service={SDRPLAY_API_VERSION}, dll={version}");

      return true;
    }

    private static string GetSerialNumber(sdrplay_api_DeviceT device)
    {
      return new string(device.SerNo).Trim((char)0);
    }
  }
}
