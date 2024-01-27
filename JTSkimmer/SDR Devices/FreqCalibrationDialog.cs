namespace JTSkimmer
{
  public partial class FreqCalibrationDialog : Form
  {
    public static FreqCalibrationDialog? dlg;
    public double currentPpm, newPpm;

    public FreqCalibrationDialog()
    {
      InitializeComponent();
    }

    internal static double ComputePpm(double ppm)
    {
      dlg ??= new FreqCalibrationDialog();
      dlg.DecoderFreqUpDown.Value = dlg.TrueFreqUpDown.Value;
      dlg.currentPpm = ppm;
      dlg.CurrentPpmLabel.Text = $"{ppm:F4}";
      dlg.ComputePpm();

      return dlg.ShowDialog() == DialogResult.OK ? dlg.newPpm : dlg.currentPpm;
    }

    private void UpDown_ValueChanged(object sender, EventArgs e)
    {
      ComputePpm();
    }

    private void ComputePpm()
    {
      double freq = (double)RxFrequencyUpDown.Value * 1e3;
      double decTone = (double)DecoderFreqUpDown.Value;
      double trueTone = (double)TrueFreqUpDown.Value;

      double currentCorrFactor = 1 + currentPpm * 1e-6;

      double newCorrFactor = currentCorrFactor * (freq + decTone) / (freq + trueTone);
      newPpm = (newCorrFactor - 1) * 1e6;
      NewPpmLabel.Text = $"{newPpm:F4}";
    }
  }
}
