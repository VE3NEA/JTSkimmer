using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MathNet.Numerics;
using Serilog;
using UN7ZO.HamCockpitPlugins.SDRPlaySource;
using VE3NEA.HamCockpit.PluginHelpers;
using static UN7ZO.HamCockpitPlugins.SDRPlaySource.NativeMethods;
using static UN7ZO.HamCockpitPlugins.SDRPlaySource.SDRplayAPI_Callback;
using static UN7ZO.HamCockpitPlugins.SDRPlaySource.SDRplayAPI_RSP2;
using static UN7ZO.HamCockpitPlugins.SDRPlaySource.SDRplayAPI_RSPduo;


namespace JTSkimmer
{
  internal class SdrPlayDevice : BaseSdrDevice
  {
    private const float SDRPLAY_API_VERSION = 3.15f;
    public const int SDRPLAY_RSP1B_ID = 6;
    public const int SDRPLAY_RSPdxR2_ID = 7;
    

    private static readonly Dictionary<int, string> SdrplayModels = new Dictionary<int, string>()
    {
      {SDRPLAY_RSP1_ID, "SDRplay RSP1" },
      {SDRPLAY_RSP1A_ID, "SDRplay RSP1a" },
      {SDRPLAY_RSP2_ID, "SDRplay RSP2" },
      {SDRPLAY_RSPduo_ID, "SDRplay RSP Duo" },
      {SDRPLAY_RSPdx_ID, "SDRplay RSP DX" },
      {SDRPLAY_RSP1B_ID, "SDRplay RSP1b" },
      {SDRPLAY_RSPdxR2_ID, "SDRplay RSPdxR2" },
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

    private SdrPlaySettings Settings { get => (SdrPlaySettings) Info.Settings; }

    protected override void Start()
    {
      if (!ApiOK) throw new Exception("API could not be opened");

      if (Settings.Agc)
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

        // RSPduo is always opened in single tuner mode, with tuner A selected
        if (Device.hwVer == SDRPLAY_RSPduo_ID)
        {
          Device.tuner = SDRplayAPI_Tuner.sdrplay_api_TunerSelectT.sdrplay_api_Tuner_A;
          Device.rspDuoMode = sdrplay_api_RspDuoModeT.sdrplay_api_RspDuoMode_Single_Tuner;
        }

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

      DevParams.fsFreq.fsHz = Settings.SamplingRate * 3; // sdrplay divides SR by 3 - not documented
      RxChannel.tunerParams.rfFreq.rfHz = Settings.CenterFrequency * (1 + Info.Settings.Ppm * 1e-6);
      RxChannel.tunerParams.bwType = SDRplayAPI_Tuner.sdrplay_api_Bw_MHzT.sdrplay_api_BW_1_536;
      RxChannel.tunerParams.ifType = SDRplayAPI_Tuner.sdrplay_api_If_kHzT.sdrplay_api_IF_1_620;

      if (!Settings.Agc) RxChannel.tunerParams.gain.gRdB = (int)Gains[GainIndex]; //20..59
      RxChannel.ctrlParams.agc.enable = Settings.Agc ? 
        sdrplay_api_AgcControlT.sdrplay_api_AGC_50HZ : sdrplay_api_AgcControlT.sdrplay_api_AGC_DISABLE;

      SetModelSpecificParameters();

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

    private void SetModelSpecificParameters()
    {
      switch (Info.SdrType)
      {
        case SdrType.SdrPlayRSP1A:
          DevParams.rsp1aParams.rfNotchEnable = (byte)(Info.SdrPlayRsp1aSettings.NotchEnabled ? 1 : 0);
          DevParams.rsp1aParams.rfDabNotchEnable = (byte)(Info.SdrPlayRsp1aSettings.DabNotchEnabled ? 1 : 0);
          RxChannel.rsp1aTunerParams.biasTEnable = (byte)(Info.SdrPlayRsp1aSettings.BiasTEnabled ? 1 : 0);          
          break;

        case SdrType.SdrPlayRSP2:
          DevParams.rsp2Params.extRefOutputEn = (byte)(Info.SdrPlayRsp2Settings.ExtRefOutput ? 1 : 0);
          RxChannel.rsp2TunerParams.biasTEnable = (byte)(Info.SdrPlayRsp2Settings.BiasTEnabled? 1 : 0);
          RxChannel.rsp2TunerParams.amPortSel = Info.SdrPlayRsp2Settings.AntennaPortSelection;
          RxChannel.rsp2TunerParams.antennaSel = Info.SdrPlayRsp2Settings.AntennaSelection;
          break;

        case SdrType.SdrPlayRSPdx:
          DevParams.rspDxParams.antennaSel = Info.SdrPlayRspDxSettings.AntennaSelection;
          DevParams.rspDxParams.rfNotchEnable = (byte)(Info.SdrPlayRspDxSettings.NotchEnabled ? 1 : 0);
          DevParams.rspDxParams.rfDabNotchEnable = (byte)(Info.SdrPlayRspDxSettings.DabNotchEnabled ? 1 : 0);
          //DevParams.rspDxParams.hdrEnable = (byte)(Info.SdrPlayRspDxSettings.HdrEnabled ? 1 : 0);
          //RxChannel.rspDxTunerParams.hdrBw = Info.SdrPlayRspDxSettings.HdrBandwidth;
          DevParams.rspDxParams.biasTEnable= (byte)(Info.SdrPlayRspDxSettings.BiasTEnabled ? 1 : 0);
          break;

        case SdrType.SdrPlayRSPduo:
          DevParams.rspDuoParams.extRefOutputEn = (byte)(Info.SdrPlayRspDuoSettings.ExtRefOutput ? 1 : 0);
          RxChannel.rspDuoTunerParams.biasTEnable = (byte)(Info.SdrPlayRspDuoSettings.BiasTEnabled ? 1 : 0);
          RxChannel.rspDuoTunerParams.tuner1AmPortSel = Info.SdrPlayRspDuoSettings.AmPort;
          RxChannel.rspDuoTunerParams.tuner1AmNotchEnable = (byte)(Info.SdrPlayRspDuoSettings.AmNotchEnabled ? 1 : 0);
          RxChannel.rspDuoTunerParams.rfNotchEnable = (byte)(Info.SdrPlayRspDuoSettings.NotchEnabled ? 1 : 0);
          RxChannel.rspDuoTunerParams.rfDabNotchEnable = (byte)(Info.SdrPlayRspDuoSettings.DabNotchEnabled ? 1 : 0);
          break;
      }
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
        // todo: check for not null
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
      RxChannel.tunerParams.rfFreq.rfHz = Settings.CenterFrequency;
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
      Settings.GainIndex = gainIndex;

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

      try
      {
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
              result.Add(new(
                GetSdrPlayType(devices[i].hwVer),
                SdrplayModels[devices[i].hwVer],
                GetSerialNumber(devices[i])
                ));
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
      }
      catch (Exception e)
      {
        Log.Error(e, "Error listing SDRplay devices");
      }

      return result;
    }

    private static SdrType GetSdrPlayType(byte hwVer)
    {
      switch (hwVer)
      {
        case SDRPLAY_RSP1A_ID:
        case SDRPLAY_RSP1B_ID:
          return SdrType.SdrPlayRSP1A;

        case SDRPLAY_RSP2_ID:
          return SdrType.SdrPlayRSP2;

        case SDRPLAY_RSPdx_ID:
        case SDRPLAY_RSPdxR2_ID:
          return SdrType.SdrPlayRSPdx;

        case SDRPLAY_RSPduo_ID:
          return SdrType.SdrPlayRSPduo;

        default: return SdrType.SdrPlayOther;
      }
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
        Log.Warning($"SDRplay API version mismatch: service={version}, dll={SDRPLAY_API_VERSION}");

      return true;
    }

    private static string GetSerialNumber(sdrplay_api_DeviceT device)
    {
      return new string(device.SerNo).Trim((char)0);
    }
  }
}
