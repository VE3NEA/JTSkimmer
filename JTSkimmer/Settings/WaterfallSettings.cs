using System.ComponentModel;

namespace JTSkimmer
{
  internal class WaterfallSettings
  {
    public float Brightness = -0.2f;
    public float Contrast = 0.15f;
    public int Speed = 15;
    public int PaletteIndex = 0;

    [Description("Disable all waterfall displays to reduce CPU load")]
    [DefaultValue(true)]
    public bool Enabled { get; set; } = true;

    public override string ToString() { return string.Empty; }
  }
}
