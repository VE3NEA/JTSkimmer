using System.ComponentModel;

namespace JTSkimmer
{
  public class DistributorSettings
  {
    [DisplayName("Save to File")]
    [Description("Save decoded messages to the files")]
    [DefaultValue(false)]
    public bool ArchiveToFile { get; set; }

    [DisplayName("DX Cluster")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public DxClusterSettings DxCluster { get; set; } = new();

    [DisplayName("WSJT-X UDP Packets")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public UdpSenderSettings UdpSender { get; set; } = new();

    [DisplayName("PSK Reporter")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public PskReporterSettings PskReporter { get; set; } = new();


    public override string ToString() { return string.Empty; }
  }

  public class DxClusterSettings
  {
    [DefaultValue(false)]
    [Description("Serve decoded messages as spots using the built-in DX cluster")]
    public bool Enabled { get; set; }

    [DisplayName("TCP Port")]
    [DefaultValue((ushort)7310)]
    public ushort Port { get; set; } = 7310;


    public override string ToString() { return string.Empty; }
  }

  public class UdpSenderSettings
  {
    [DefaultValue(false)]
    [Description("Send decoded messages as UDP packets in the WSJT-X format")]
    public bool Enabled { get; set; }

    [DisplayName("UDP Port")]
    [DefaultValue((ushort)7311)]
    public ushort Port { get; set; } = 7311;

    [DefaultValue("127.0.0.1")]
    public string Host { get; set; } = "127.0.0.1";

    public override string ToString() { return string.Empty; }
  }

  public class PskReporterSettings
  {
    [DefaultValue(true)]
    [Description("Send decoded messages to PSK Reporter")]
    public bool Enabled { get; set; }

    [Description("Antenna description")]
    public string Antenna{ get; set; } = string.Empty;

    public override string ToString() { return string.Empty; }
  }
}