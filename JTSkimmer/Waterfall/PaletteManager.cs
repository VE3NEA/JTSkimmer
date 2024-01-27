using VE3NEA;

namespace JTSkimmer
{
  internal class PaletteManager
  {
    private readonly string PaletteFolder;
    public Palette[] Palettes;

    public PaletteManager()
    {
      PaletteFolder = Path.Combine(Utils.GetReferenceDataFolder(), "Palettes");
      Directory.CreateDirectory(PaletteFolder);
      LoadPalettes();
    }

    private void LoadPalettes()
    {
      var paletteFiles = ListOrCreatePaletteFiles().Order();
      Palettes = paletteFiles.Select(f => Palette.CreateFromFile(f)).ToArray();
    }

    private string[] ListOrCreatePaletteFiles()
    {
      string[] paletteFiles = Directory.EnumerateFiles(PaletteFolder).ToArray();
      if (paletteFiles.Length > 0) return paletteFiles;

      var entries = Properties.Resources.DefaultPalettes.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
      foreach (var entry in entries)
      {
        var fields = entry.Split('=');
        File.WriteAllText(Path.Combine(PaletteFolder, fields[0] + ".txt"), fields[1].Replace(',', '\n'));
      }

      return Directory.EnumerateFiles(PaletteFolder).ToArray();
    }
  }
}

