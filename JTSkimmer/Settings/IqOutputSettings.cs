using System.ComponentModel;

namespace JTSkimmer
{
  internal class IqOutputSettings
  {
    [DefaultValue(false)]
    [Description("Send I/Q data in the TIMF2 format")]
    public bool Enabled { get; set; }

    [DefaultValue("127.0.0.1")]
    public string Host { get; set; } = "127.0.0.1";

    [DisplayName("UDP Port")]
    [DefaultValue((ushort)50004)]
    public ushort Port { get; set; } = 50004;

    [DisplayName("Center Frequency")]
    [Description("Center frequency, kHz")]
    [DefaultValue((double)144115)]
    public double CenterFrequencykHz { get; set; } = 144115;

    [DisplayName("Gain")]
    [Description("Gain, dB")]
    [DefaultValue(0)]
    public int Gain { get; set; }


    public override string ToString() { return string.Empty; }
  }
}
