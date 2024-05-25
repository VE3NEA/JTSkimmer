using System.ComponentModel;
using System.Drawing.Design;

namespace JTSkimmer
{
  public class MessagePanelSettings
  {
    [DisplayName("Background Color")]
    public Color BackColor { get; set; } = SystemColors.Window;

    [DisplayName("Text Color")]
    public Color TextColor { get; set; } = SystemColors.WindowText;

    [DisplayName("My Call Color")]
    public Color MyCallColor { get; set; } = Color.FromArgb(255, 175, 175);

    [DisplayName("Font Size")]
    [DefaultValue(9f)]
    public float FontSize { get; set; } = 9f;

    [DisplayName("CQ Color")]
    public Color CqColor { get; set; } = Color.Yellow;

    [DisplayName("Grid Color")]
    public Color GridColor { get; set; } = Color.Cyan;
  }

  public class HighlightSettings
  {
    public Color Color { get; set; }
    [DefaultValue(0.5f)]
    public float Opacity { get; set; } = 0.5f;

    internal HighlightSettings(Color color, float opacity) { Color = color; Opacity = opacity; }
    public override string ToString() { return ""; }
  }
}