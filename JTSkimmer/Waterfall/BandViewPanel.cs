using WeifenLuo.WinFormsUI.Docking;

namespace JTSkimmer
{
  internal partial class BandViewPanel : DockContent
  {
    private readonly Context ctx;
    private Receiver Receiver;

    internal BandViewPanel() { InitializeComponent(); }

    internal BandViewPanel(Context ctx)
    {
      InitializeComponent();
      this.ctx = ctx;
      ctx.BandViewPanel = this;
      ctx.MainForm.ViewBandViewMNU.Checked = true;

      WaterfallControl.SetTextureWidth(16384);

      ctx.MainForm.ApplySettingsToWaterfall(WaterfallControl, ctx.Settings.Waterfall);
    }

    private void BandViewPanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      ctx.BandViewPanel = null;
      ctx.MainForm.ViewBandViewMNU.Checked = false;
    }

    internal void EnableDisable()
    {
      bool enable = ctx.Settings.Waterfall.Enabled && (ctx.Sdr?.Running ?? false) && !ctx.MainForm.MustPause();
      WaterfallControl.Enabled = enable;
      WaterfallControl.LastDrawTime = DateTime.MinValue; // reset PLL

      WaterfallControl.Visible = ctx.Settings.Waterfall.Enabled;
    }

    internal void SetWaterfallSpeed()
    {
      if (ctx.Downsampler != null)
        ctx.MainForm.SpectrumAnalyzer.Spectrum.Step =
          ctx.Downsampler.OutputSamplingRate / ctx.Settings.Waterfall.Speed;

      WaterfallControl.ScrollSpeed = ctx.Settings.Waterfall.Speed;
    }

    public void AppendSpectrum(float[] spectrum)
    {
      WaterfallControl.AppendSpectrum(spectrum);
    }




    //----------------------------------------------------------------------------------------------
    //                                         draw scale
    //----------------------------------------------------------------------------------------------
    private void ScalePanel_Paint(object sender, PaintEventArgs e)
    {
      var g = e.Graphics;
      using (var brush = new SolidBrush(BackColor)) g.FillRectangle(brush, ClientRectangle);
      DrawTicksAndReceivers(g);
    }

    private static readonly float[] TickMults = { 2f, 2.5f, 2f };

    private void DrawTicksAndReceivers(Graphics g)
    {
      var sett = ctx.Settings.Sdrs.Find(s => s.Selected)?.Settings;
      if (sett == null) return;

      int width = ScalePanel.ClientSize.Width;
      int height = ScalePanel.ClientSize.Height;

      double bandwidth = sett.Bandwidth;
      double pixPerHz = ScalePanel.ClientSize.Width / bandwidth;
      double leftFreq = sett.CenterFrequency - width / 2d / pixPerHz;

      //tick steps
      float TickStep = 200;
      float LabelStep = 1000;
      for (int i = 0; i <= 24; ++i)
      {
        if (LabelStep * pixPerHz > 60) break;
        LabelStep *= TickMults[i % 3];
        TickStep *= TickMults[(i + 1) % 3];
      }

      //first label's frequency
      double absLeftPixel = Math.Round(leftFreq * pixPerHz);
      double freq0 = Math.Round(absLeftPixel / pixPerHz);
      freq0 = Math.Round(Math.Truncate(freq0 / LabelStep) * LabelStep);
      double freq = freq0;


      // draw receivers
      if (ctx.IqOutput.Enabled)
      {
        double f = ctx.Settings.IqOutput.CenterFrequencykHz * 1e3 - LinradDatagram.SAMPLING_RATE / 2;
        double absPixel = Math.Round(f * pixPerHz);
        float x = (float)(absPixel - absLeftPixel);
        float w = (float)(LinradDatagram.SAMPLING_RATE * pixPerHz);
        var rect = new RectangleF(x, height * 0.75f, w, height * 0.25f);
        g.FillRectangle(Brushes.SkyBlue, rect);
      }

      double rxBandwidth = SdrConst.AUDIO_SAMPLING_RATE / 2;

      foreach (Receiver rx in ctx.Receivers)
      {
        var f = (double)rx.Settings.Frequency;
        if (f >= leftFreq && f < leftFreq + bandwidth)
        {
          double absPixel = Math.Round(f * pixPerHz);
          float x = (float)(absPixel - absLeftPixel);
          float w = (float)(rxBandwidth * pixPerHz);
          var rect = new RectangleF(x, height / 2, w, height / 2);
          var brush = rx == Receiver ? Brushes.Green : Brushes.LightGreen;
          g.FillRectangle(brush, rect);
        }
      }

      //draw lagre ticks and labels
      while (true)
      {
        double absPixel = Math.Round(freq * pixPerHz);
        float x = (float)(absPixel - absLeftPixel);
        if (x > width) break;

        g.DrawLine(Pens.Black, x, height - 12, x, height);

        string freqText = (freq * 1e-6).ToString("F3");
        x -= g.MeasureString(freqText, Font).Width / 2;
        g.DrawString(freqText, Font, Brushes.Black, x, 0);
        freq += LabelStep;
      }

      //draw small ticks 
      leftFreq = freq0;
      while (true)
      {
        double absPixel = Math.Round(leftFreq * pixPerHz);
        float x = (float)(absPixel - absLeftPixel);
        if (x > width) break;
        g.DrawLine(Pens.Black, x, height - 6, x, height);
        leftFreq += TickStep;
      }
    }

    internal void ShowReceiver(Receiver receiver)
    {
      Receiver = receiver;
      ScalePanel.Invalidate();
    }




    //----------------------------------------------------------------------------------------------
    //                                      events
    //----------------------------------------------------------------------------------------------
    private void SlidersBtn_Click(object sender, EventArgs e)
    {
      var dlg = new WaterfallSildersDlg(ctx);
      dlg.Location = WaterfallControl.PointToScreen(new Point(2, 2));
      dlg.Show();
    }

    private void ScalePanel_Resize(object sender, EventArgs e)
    {
      ScalePanel.Invalidate();
    }

    private void ScalePanel_MouseDown(object sender, MouseEventArgs e)
    {
      //if (e.Button != MouseButtons.Left || ModifierKeys != Keys.Control) return;

      uint? frequency = FrequencyUnderMouse(e.X);
      if (frequency == null) return;

      frequency = 1000 * (uint)(frequency / 1000); // round to 1 kHz
      ctx.MainForm.AddReceiver(frequency);
    }

    private void ScalePanel_MouseMove(object sender, MouseEventArgs e)
    {
      uint? frequency = FrequencyUnderMouse(e.X);
      ctx.ReceiversPanel?.HighlightReceiverPanel(frequency);
    }

    private void ScalePanel_MouseLeave(object sender, EventArgs e)
    {
      ctx.ReceiversPanel?.HighlightReceiverPanel(null);
    }

    private uint? FrequencyUnderMouse(int x)
    {
      var sett = ctx.Settings.Sdrs.Find(s => s.Selected)?.Settings;
      if (sett == null) return null;

      int width = ScalePanel.ClientSize.Width;
      double centerX = width / 2d;
      double pixPerHz = width / (double)sett.Bandwidth;

      return (uint)(sett.CenterFrequency + (x - centerX) / pixPerHz);
    }
  }
}
