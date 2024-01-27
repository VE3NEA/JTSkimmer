using System.ComponentModel;
using VE3NEA.HamCockpit.PluginHelpers;

namespace JTSkimmer
{

  public enum SdrType
  {
    RtlSdr,
    AirspyMini,
    AirspyR2,
    SdrPlay,
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
    public SdrPlaySettings? SdrPlaySettings { get; set; }
    internal SdrSettings Settings { get => GetSettings(); }

    public SdrInfo(SdrType sdrType, string name, string serial)
    {
      SdrType = sdrType;
      Name = name;
      SerialNumber = serial;

      if (sdrType == SdrType.RtlSdr) RtlSdrSettings = new();
      else if (sdrType == SdrType.SdrPlay) SdrPlaySettings = new();
      else AirspySettings = new();
      SdrSettings sett = GetSettings();

      sett.SamplingRate = SdrConst.DeviceRate[(int)sdrType];
      sett.Bandwidth = SdrConst.Bandwidths[(int)sdrType].Values.Min();
      sett.CenterFrequency = 28100000;
    }

    public override string ToString() { return string.Empty; }

    private SdrSettings GetSettings()
    {
      switch (SdrType)
      {
        case SdrType.RtlSdr: return RtlSdrSettings;
        case SdrType.SdrPlay: return SdrPlaySettings;
        default: return AirspySettings;
      }
    }
  }
}
