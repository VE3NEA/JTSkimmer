using System.ComponentModel;
using System.Diagnostics;
using VE3NEA.HamCockpit.PluginHelpers;
using static UN7ZO.HamCockpitPlugins.SDRPlaySource.SDRplayAPI_RSP2;
using static UN7ZO.HamCockpitPlugins.SDRPlaySource.SDRplayAPI_RSPduo;
using static UN7ZO.HamCockpitPlugins.SDRPlaySource.SDRplayAPI_RSPdx;

namespace JTSkimmer
{

  public enum SdrType
  {
    RtlSdr,

    AirspyMini,
    AirspyR2,

    SdrPlayRSP1A,
    SdrPlayRSP2,
    SdrPlayRSPdx,
    SdrPlayRSPduo,
    SdrPlayOther,
  }

  public class SdrSettings
  {
    public uint SamplingRate;
    public uint CenterFrequency;
    public uint Bandwidth;
    public uint GainIndex;
    public double Ppm;
  }

  // https://groups.io/g/airspy/message/26271
  public enum AirspyGainMode
  {
    [Description("Best Sensitivity")]
    Sensitivity,
    [Description("Best Linearity")]
    Linearity,
    Custom
  }


  public class AirspySettings : SdrSettings
  {
    // see https://github.com/airspy/airspyone_host/blob/master/libairspy/src/airspy.c

    [TypeConverter(typeof(EnumDescriptionConverter))]
    [DisplayName("Gain Mode")]
    [Description("AGC and gain settings apply only in the Custom mode, otherwise they are controlled by the SDR Gain slider.")]
    [DefaultValue(AirspyGainMode.Custom)]
    public AirspyGainMode GainMode { get; set; } = AirspyGainMode.Custom;

    [DisplayName("LNA AGC")]
    [Description("Enable LNA AGC")]
    [DefaultValue(false)]
    public bool LnaAGC { get; set; }

    [DisplayName("Mixer AGC")]
    [Description("Enable Mixer AGC")]
    [DefaultValue(false)]
    public bool MixerAGC { get; set; }

    [DisplayName("VGA Gain")]
    [Description("0 to 15")]
    [DefaultValue((byte)6)]
    public byte VgaGain { get; set; } = 6;

    [DisplayName("LNA Gain")]
    [Description("0 to 14")]
    [DefaultValue((byte)14)]
    public byte LnaGain { get; set; } = 14;

    [DisplayName("Mixer Gain")]
    [Description("0 to 15")]
    [DefaultValue((byte)15)]
    public byte MixerGain { get; set; } = 15;

    [DisplayName("Bias-T")]
    [Description("Enable Bias-T voltage")]
    [DefaultValue(false)]
    public bool BiasTEnabled { get; set; }
  }

  public class RtlSdrSettings : SdrSettings
  {
    [DisplayName("RTL2832 AGC")]
    [Description("Enable RTL2832 AGC")]
    [DefaultValue(false)]
    public bool Rtl2832Agc { get; set; }
    
    [DisplayName("IF AGC")]
    [Description("Enable IF AGC")]
    [DefaultValue(false)]
    public bool IFAgc { get; set; }

    [DisplayName("Bias-T")]
    [Description("Enable Bias-T voltage output")]
    [DefaultValue(false)]
    public bool BiasTEnabled { get; set; }
  }

  public class SdrPlaySettings : SdrSettings
  {
    [DisplayName("AGC")]
    [Description("Enable AGC")]
    [DefaultValue(false)]
    public bool Agc { get; set; }
  }

  public class SdrPlayRsp1aSettings : SdrPlaySettings
  {
    [DisplayName("Notch")]
    [Description("Enable Notch")]
    [DefaultValue(false)]
    public bool NotchEnabled { get; set; }

    [DisplayName("DAB Notch")]
    [Description("Enable DAB Notch")]
    [DefaultValue(false)]
    public bool DabNotchEnabled { get; set; }

    [DisplayName("Bias-T")]
    [Description("Enable Bias-T voltage")]
    [DefaultValue(false)]
    public bool BiasTEnabled { get; set; }
  }

  public class SdrPlayRsp2Settings : SdrPlaySettings
  {
    [DisplayName("Ext. Ref. Output")]
    [Description("External Reference Output")]
    [DefaultValue(false)]          
    public bool ExtRefOutput { get; set; }

    [DisplayName("Bias-T")]
    [Description("Enable Bias-T voltage")]
    [DefaultValue(false)]
    public bool BiasTEnabled { get; set; }

    [DisplayName("Antenna Port")]
    [Description("Antenna PortSelection")]
    [DefaultValue(sdrplay_api_Rsp2_AmPortSelectT.sdrplay_api_Rsp2_AMPORT_2)]
    [TypeConverter(typeof(EnumDescriptionConverter))]
    public sdrplay_api_Rsp2_AmPortSelectT AntennaPortSelection { get; set; } = sdrplay_api_Rsp2_AmPortSelectT.sdrplay_api_Rsp2_AMPORT_2;

    [DisplayName("Antenna")]
    [Description("Antenna Selection")]
    [DefaultValue(sdrplay_api_Rsp2_AntennaSelectT.sdrplay_api_Rsp2_ANTENNA_A)]
    [TypeConverter(typeof(EnumDescriptionConverter))]
    public sdrplay_api_Rsp2_AntennaSelectT AntennaSelection { get; set; } = sdrplay_api_Rsp2_AntennaSelectT.sdrplay_api_Rsp2_ANTENNA_A;

    [DisplayName("Notch")]
    [Description("Enable Notch")]
    [DefaultValue(false)]
    public bool NotchEnabled { get; set; }
  }

  public class SdrPlayRspDuoSettings : SdrPlaySettings
  {
    [DisplayName("Ext. Ref. Output")]
    [Description("External Reference Output")]
    [DefaultValue(false)]
    public bool ExtRefOutput { get; set; }

    [DisplayName("Bias-T")]
    [Description("Enable Bias-T voltage")]
    [DefaultValue(false)]
    public bool BiasTEnabled { get; set; }

    [DisplayName("AM Port Selection")]
    [TypeConverter(typeof(EnumDescriptionConverter))]
    [DefaultValue(sdrplay_api_RspDuo_AmPortSelectT.sdrplay_api_RspDuo_AMPORT_2)]
    public sdrplay_api_RspDuo_AmPortSelectT AmPort { get; set; } = sdrplay_api_RspDuo_AmPortSelectT.sdrplay_api_RspDuo_AMPORT_2;

    [DisplayName("AM Notch Enabled")]
    [DefaultValue(false)]
    public bool AmNotchEnabled { get; set; }

    [DisplayName("Notch")]
    [Description("Enable Notch")]
    [DefaultValue(false)]
    public bool NotchEnabled { get; set; }

    [DisplayName("DAB Notch")]
    [Description("Enable DAB Notch")]
    [DefaultValue(false)]
    public bool DabNotchEnabled { get; set; }
  }

  public class SdrPlayRspDxSettings : SdrPlaySettings
  {
    [DisplayName("Antenna")]
    [Description("Antenna Selection")]
    [DefaultValue(sdrplay_api_RspDx_AntennaSelectT.sdrplay_api_RspDx_ANTENNA_A)]
    [TypeConverter(typeof(EnumDescriptionConverter))]
    public sdrplay_api_RspDx_AntennaSelectT AntennaSelection { get; set; } = sdrplay_api_RspDx_AntennaSelectT.sdrplay_api_RspDx_ANTENNA_A;

    [DisplayName("Notch")]
    [Description("Enable Notch")]
    [DefaultValue(false)]
    public bool NotchEnabled { get; set; }

    [DisplayName("DAB Notch")]
    [Description("Enable DAB Notch")]
    [DefaultValue(false)]
    public bool DabNotchEnabled { get; set; }

    /*
        // HDR works only at a sampling rate of 6 MHz which we do not use
       
        [DisplayName("HDR")]
        [Description("Enable HDR Mode")]
        [DefaultValue(false)]
        public bool HdrEnabled { get; set; }

        [DisplayName("HDR Bandwidth")]
        [DefaultValue(sdrplay_api_RspDx_HdrModeBwT.sdrplay_api_RspDx_HDRMODE_BW_1_700)]
        [TypeConverter(typeof(EnumDescriptionConverter))]
        public sdrplay_api_RspDx_HdrModeBwT HdrBandwidth { get; set; } = sdrplay_api_RspDx_HdrModeBwT.sdrplay_api_RspDx_HDRMODE_BW_1_700;
    */

    [DisplayName("Bias-T")]
    [Description("Enable Bias-T voltage")]
    [DefaultValue(false)]
    public bool BiasTEnabled { get; set; }
  }


  public class SdrInfo
  {
    [TypeConverter(typeof(EnumDescriptionConverter))] 
    public SdrType SdrType {get; set; }
    public string Name {get; set; }
    public string SerialNumber {get; set; }
    public bool Present { get; set; } = true;
    public bool Selected {get; set; }

    public AirspySettings? AirspySettings { get; set; }
    public RtlSdrSettings? RtlSdrSettings { get; set; }


    public SdrPlayRsp1aSettings? SdrPlayRsp1aSettings { get; set; }
    public SdrPlayRsp2Settings? SdrPlayRsp2Settings { get; set; }
    public SdrPlayRspDxSettings? SdrPlayRspDxSettings { get; set; }
    public SdrPlayRspDuoSettings? SdrPlayRspDuoSettings { get; set; }
    public SdrPlaySettings? SdrPlayOtherSettings { get; set; }

    
    internal SdrSettings Settings { get => GetSettings(); }

    public SdrInfo(SdrType sdrType, string name, string serial)
    {
      SdrType = sdrType;
      Name = name;
      SerialNumber = serial;

      if (sdrType == SdrType.RtlSdr) RtlSdrSettings = new();


      else if (sdrType == SdrType.SdrPlayRSP1A) SdrPlayRsp1aSettings = new();
      else if (sdrType == SdrType.SdrPlayRSP2) SdrPlayRsp2Settings = new();
      else if (sdrType == SdrType.SdrPlayRSPdx) SdrPlayRspDxSettings = new();
      else if (sdrType == SdrType.SdrPlayRSPduo) SdrPlayRspDuoSettings = new();
      else if (sdrType == SdrType.SdrPlayOther) SdrPlayOtherSettings = new();
      else AirspySettings = new();
      SdrSettings sett = GetSettings();

      Debug.Assert(SdrConst.DeviceRates.Length == Enum.GetNames(typeof(SdrType)).Length);
      Debug.Assert(SdrConst.Bandwidths.Length == Enum.GetNames(typeof(SdrType)).Length);

      sett.SamplingRate = SdrConst.DeviceRates[(int)sdrType];
      sett.Bandwidth = SdrConst.Bandwidths[(int)sdrType].Values.Min();
      sett.CenterFrequency = 28100000;
    }

    public override string ToString() { return string.Empty; }

    private SdrSettings GetSettings()
    {
      switch (SdrType)
      {
        case SdrType.RtlSdr: return RtlSdrSettings;

        case SdrType.SdrPlayRSP1A: return SdrPlayRsp1aSettings;
        case SdrType.SdrPlayRSP2: return SdrPlayRsp2Settings;
        case SdrType.SdrPlayRSPdx: return SdrPlayRspDxSettings;
        case SdrType.SdrPlayRSPduo: return SdrPlayRspDuoSettings;
        case SdrType.SdrPlayOther: return SdrPlayOtherSettings;

        default: return AirspySettings;
      }
    }
  }
}
