namespace JTSkimmer
{
  public partial class NoiseBlankerDialog : Form
  {
    private readonly Context ctx;

    public NoiseBlankerDialog()
    {
      InitializeComponent();
    }

    internal NoiseBlankerDialog(Context ctx)
    {
      InitializeComponent();

#if DEBUG
      SnapshotBtn.Visible = true;
#endif

      this.ctx = ctx;

      if (!ctx.Settings.NoiseBlanker.Enabled) radioButton1.Checked = true;
      else if (ctx.Settings.NoiseBlanker.Nonlinear) radioButton3.Checked = true;
      else radioButton2.Checked = true;

      trackBar1.Value = ctx.Settings.NoiseBlanker.ThresholdTrackbarPosition;
      ShowSettings();
    }

    private void ShowSettings()
    {
      double threshold = NoiseBlanker.ThresholdFromTrackbarPosition(trackBar1.Value);
      label1.Text = $"Threshold: {threshold:F2}";
    }

    private void radioButton_CheckedChanged(object sender, EventArgs e)
    {
      ctx.Settings.NoiseBlanker.Enabled = !radioButton1.Checked;
      ctx.Settings.NoiseBlanker.Nonlinear = radioButton3.Checked;
      ctx.Downsampler?.NoiseBlanker?.ApplySettings(ctx.Settings.NoiseBlanker);
    }

    private void trackBar1_ValueChanged(object sender, EventArgs e)
    {
      ctx.Settings.NoiseBlanker.ThresholdTrackbarPosition = trackBar1.Value;
      ctx.Downsampler?.NoiseBlanker?.ApplySettings(ctx.Settings.NoiseBlanker);

      ShowSettings();
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      if (ctx.Downsampler?.NoiseBlanker != null)
      {
        double percent = 100 * ctx.Downsampler.NoiseBlanker.BlankedCount / (ctx.Downsampler.NoiseBlanker.TotalCount + 1e-30);
        ctx.Downsampler.NoiseBlanker.TotalCount = ctx.Downsampler.NoiseBlanker.BlankedCount = 0;

        label2.Text = $"Blanked: {percent:F2}%";
      }
    }

    private void SnapshotBtn_Click(object sender, EventArgs e)
    {
      if (ctx.Downsampler?.NoiseBlanker != null)
        ctx.Downsampler.NoiseBlanker.SnapshotRequested = true;
    }

    private void NoiseBlankerDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Escape) Close();
    }
  }
}
