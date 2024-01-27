using System.Drawing.Drawing2D;
using FontAwesome;

namespace JTSkimmer
{
  public partial class WaterfallSildersDlg : Form
  {
    private Context ctx;
    private bool changing;

    public WaterfallSildersDlg()
    {
      InitializeComponent();
    }

    internal WaterfallSildersDlg(Context ctx)
    {
      InitializeComponent();
      this.ctx = ctx;

      label1.Font = ctx.AwesomeFont14;
      label1.Text = FontAwesomeIcons.Sun;
      label2.Font = ctx.AwesomeFont14;
      label2.Text = FontAwesomeIcons.CircleHalfStroke;
      label3.Font = ctx.AwesomeFont14;
      label3.Text = FontAwesomeIcons.DownLong;
      label4.Font = ctx.AwesomeFont14;
      label4.Text = FontAwesomeIcons.Palette;

      SettingsToDialog();
    }

    private void WaterfallSildersDlg_Deactivate(object sender, EventArgs e)
    {
      Close();
    }

    private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
    {
      var rect = e.Bounds;

      if (!e.State.HasFlag(DrawItemState.ComboBoxEdit))
      {
        e.Graphics.FillRectangle(Brushes.White, e.Bounds);
        rect.Inflate(-2, -1);
        rect.Width -= 17; // make dropdown entries the same width as combo's edit box
      }

      if (e.Index >= 0)
      {
        e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
        e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
        e.Graphics.DrawImage(ctx.PaletteManager.Palettes[e.Index].ToBitmap(), rect);
      }
    }


    private readonly int[] Speeds = new int[] { 1, 2, 4, 8, 15, 30, 60, 120 };

    private void Trackbar_ValueChanged(object sender, EventArgs e)
    {
      BrightnessLabel.Text = BrightnessTrackbar.Value.ToString();
      ContrastLabel.Text = ContrastTrackbar.Value.ToString();
      SpeedLabel.Text = Speeds[SpeedTrackbar.Value].ToString();

      if (changing) return;
      DialogToSettings();
      ctx.MainForm.ApplySettingsToAllWaterfalls();
      ctx.MainForm.SetWaterfallSpeed();
    }

    private void SettingsToDialog()
    {
      changing = true;
      var sett = ctx.Settings.Waterfall;

      BrightnessTrackbar.Value = (int)(50 * (1 + sett.Brightness));
      ContrastTrackbar.Value = (int)(100 * sett.Contrast);

      SpeedTrackbar.Maximum = Speeds.Length - 1;
      SpeedTrackbar.Value = Array.IndexOf(Speeds, sett.Speed);

      PaletteComboBox.DataSource = ctx.PaletteManager.Palettes;
      PaletteComboBox.SelectedIndex = Math.Max(0, Math.Min(ctx.PaletteManager.Palettes.Count() - 1, sett.PaletteIndex));

      changing = false;
    }

    private void DialogToSettings()
    {
      var sett = ctx.Settings.Waterfall;

      sett.Brightness = BrightnessTrackbar.Value / 50f - 1; // 0..100 -> -1..1
      sett.Contrast = ContrastTrackbar.Value / 100f;        // 0..100 -> 0..1
      sett.Speed = Speeds[SpeedTrackbar.Value];
      sett.PaletteIndex = PaletteComboBox.SelectedIndex;
    }
  }
}
