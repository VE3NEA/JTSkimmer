using System.ComponentModel;

namespace JTSkimmer
{
  public class UserSettings
  {
    [DisplayName("Callsign")]
    [Description("Your callsign")]
    public string Call { get; set; }

    [DisplayName("Grid Square")]
    [Description("Your 6-character grid square")]
    public string Square { get; set; }


    public override string ToString() { return ""; }
  }
}
