using System.Drawing.Text;

namespace JTSkimmer
{
  internal static class FontAwesomeFactory
  {
    private static readonly PrivateFontCollection collection = new PrivateFontCollection();
    public static FontFamily Family { get => collection.Families[0]; }

    static FontAwesomeFactory()
    {
      string path = Path.Combine(Application.StartupPath, "fa-solid-900.ttf");
      collection.AddFontFile(path);
    }

    public static Font Create(float size, FontStyle style = FontStyle.Regular)
    {
      return new Font(Family, size, style);
    }
  }
}
