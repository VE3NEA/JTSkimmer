using System.ComponentModel;

namespace JTSkimmer
{
  public class DecodingSettings
  {
    [DisplayName("Path to jt9.exe")]
    [Description("Path to the jt9.exe file that comes with WSJT-X")]
    public string? Jt9ExePath { get; set; }

    [DisplayName("Path to jtdxjt9.exe")]
    [Description("Path to the jtdxjt9.exe file that comes with JTDX")]
    public string? JtdxJt9ExePath { get; set; }


    public override string ToString() { return string.Empty; }
  }
}