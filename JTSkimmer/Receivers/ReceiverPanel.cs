using System.Globalization;
using VE3NEA;
using VE3NEA.HamCockpit.DspFun;


namespace JTSkimmer
{
  internal partial class ReceiverPanel : UserControl
  {
    internal readonly Receiver Receiver;
    private readonly Context ctx;
    private DecoderStatus DecoderStatus = DecoderStatus.Finished;
    private int DecodedMessageCount;

    internal ReceiverPanel()
    {
      InitializeComponent();
    }

    internal ReceiverPanel(Receiver receiver, Context ctx)
    {
      this.ctx = ctx;
      InitializeComponent();
      Receiver = receiver;
      Receiver.SpectrumAvailable += Receiver_SpectrumAvailable;
      Receiver.DecoderEvent += Receiver_DecoderStatusChanged;
      Receiver.SpectrumNeeded = true;
      Indexlabel.Text = $"{receiver.index:D2}";
      UpdateLabels();
    }

    private void Receiver_DecoderStatusChanged(object? sender, DecoderEventArgs e)
    {
      DecoderStatus = e.Status;
      DecodedMessageCount = e.Data.Length;
      if (IsHandleCreated) BeginInvoke(UpdateLabels);
    }

    internal new void Dispose()
    {
      Receiver.SpectrumAvailable -= Receiver_SpectrumAvailable;
      Receiver.DecoderEvent -= Receiver_DecoderStatusChanged;
      Receiver.SpectrumNeeded = false;
      base.Dispose();
    }

    private void CloseBtn_Click(object sender, EventArgs e)
    {
      ctx.Receivers.Remove(Receiver);
      Receiver.Dispose();
      Dispose();
      ctx.ReceiversPanel?.DeleteReceiverPanel(this);
      ctx.BandViewPanel?.ScalePanel?.Invalidate();
    }

    internal void EnableDisable()
    {
      bool mustEnable = ctx.Settings.Waterfall.Enabled && ctx.Sdr != null && ctx.Sdr.Running && Receiver.IsValidOffset() && !ctx.MainForm.MustPause();
      Receiver.EnableDisable(mustEnable);

      if (WaterfallControl.Enabled != mustEnable)
      {
        WaterfallControl.Enabled = mustEnable;
        WaterfallControl.LastDrawTime = DateTime.MinValue; // reset PLL
      }

      WaterfallControl.Visible = ctx.Settings.Waterfall.Enabled;

      UpdateLabels();
    }

    private void Receiver_SpectrumAvailable(object? sender, DataEventArgs<float> e)
    {
      WaterfallControl.AppendSpectrum(e.Data);
    }

    public void UpdateLabels()
    {
      ShowFrequency();

      SpeakerBtn.Checked = Receiver.Settings.AudioToSpeaker;
      VacBtn.Checked = Receiver.Settings.AudioToVac;

      ModeLabel.Font = new Font(ModeLabel.Font, FontStyle.Bold);
      ModeLabel.Text = Receiver.Settings.DecoderMode;
      ToolTip.SetToolTip(ModeLabel, "");

      if (Receiver.Settings.DecoderMode == ReceiverSettings.NO_DECODING)
      {
        ModeLabel.ForeColor = Color.Black;
        ModeLabel.Font = new Font(ModeLabel.Font, FontStyle.Regular);
      }
      else if (Receiver.Runner == null)
      {
        ModeLabel.ForeColor = Color.Red;
        ToolTip.SetToolTip(ModeLabel, "Decoding not configured");
      }
      else if (DecoderStatus == DecoderStatus.Overload)
      {
        ModeLabel.ForeColor = Color.Magenta;
        ModeLabel.Text += " (missed)";
      }
      else if (DecoderStatus == DecoderStatus.Finished)
      {
        ModeLabel.ForeColor = Color.Green;
        ModeLabel.Text += $" ({DecodedMessageCount})";
      }
      else  // decoding started
      {
        ModeLabel.ForeColor = Color.Blue;
        ModeLabel.Text += " (busy)";
      }
    }

    private void ShowFrequency()
    {
      // value
      float kHz = (Receiver.Settings.Frequency ?? 0) / 1000f;
      uint frac = Receiver.Settings.Frequency ?? 0 % 1000;
      string format = frac == 0 ? "N0" : "N1";
      FrequencyLabel.Text = kHz.ToString(format, new CultureInfo("en-US"));

      // color
      if (Receiver.IsValidOffset())
      {
        FrequencyLabel.ForeColor = Color.Green;
        ToolTip.SetToolTip(FrequencyLabel, "");
      }
      else
      {
        FrequencyLabel.ForeColor = Color.Red;
        ToolTip.SetToolTip(FrequencyLabel, "Frequency is not in the passband");
      }

      ctx.BandViewPanel?.ScalePanel?.Invalidate();
    }

    private void SettingsBtn_Click(object sender, EventArgs e)
    {
      var dlg = new ReceiverSettingsDlg(this);
      dlg.Location = ModeLabel.PointToScreen(new Point((ModeLabel.Width - dlg.Width) / 2, ModeLabel.Height));
      var rc = dlg.ShowDialog(this);
    }

    private void SpeakerBtn_Click(object sender, EventArgs e)
    {
      Receiver.Settings.AudioToSpeaker = !Receiver.Settings.AudioToSpeaker;
      ctx.ReceiversPanel?.UpdateAudioSelection(Receiver);
    }

    private void VacBtn_Click(object sender, EventArgs e)
    {
      Receiver.Settings.AudioToVac = !Receiver.Settings.AudioToVac;
      ctx.ReceiversPanel?.UpdateAudioSelection(Receiver);
    }

    private void TuneBtn_Click(object sender, EventArgs e)
    {
      if (ctx.OmniRig.Status != RigStatus.Online)
        MessageBox.Show(this, ctx.OmniRig.GetStatusText(), "OmniRig", MessageBoxButtons.OK, MessageBoxIcon.Error);
      else
        ctx.OmniRig.RxFrequency = (int)Receiver.Settings.Frequency;
    }

    internal void SetWaterfallSpeed()
    {
      Receiver.Spectrum.Step = SdrConst.AUDIO_SAMPLING_RATE / ctx.Settings.Waterfall.Speed;
      WaterfallControl.ScrollSpeed = ctx.Settings.Waterfall.Speed;
    }

    private void SlidersGlyph_MouseDown(object sender, MouseEventArgs e)
    {
      var dlg = new WaterfallSildersDlg(ctx);
      dlg.Location = WaterfallControl.PointToScreen(new Point(SlidersGlyph.Width, -dlg.Height));
      dlg.Show();
    }

    private void ReceiverPanel_DragEnter(object sender, DragEventArgs e)
    {
      e.Effect = e.Data?.GetData(typeof(ReceiverPanel)) == this ? DragDropEffects.None : DragDropEffects.Move;
    }

    private void ToolStrip_MouseDown(object sender, MouseEventArgs e)
    {
      DoDragDrop(this, DragDropEffects.Move);
    }

    private void ReceiverPanel_DragDrop(object sender, DragEventArgs e)
    {
      ReceiverPanel droppedPanel = (ReceiverPanel)e.Data.GetData(typeof(ReceiverPanel));
      ctx.ReceiversPanel.ReorderPanels(droppedPanel, this);
    }

    private void ReceiverPanel_MouseLeave(object sender, EventArgs e)
    {
      ctx.BandViewPanel?.ShowReceiver(null);
    }

    private void ReceiverPanel_MouseEnter(object sender, EventArgs e)
    {
      ctx.BandViewPanel?.ShowReceiver(Receiver);
    }

    internal void HighlightIfFrequency(uint? freq)
    {
      var f = Receiver.Settings.Frequency;
      bool highlighted = freq.HasValue && f <= freq && f + SdrConst.AUDIO_SAMPLING_RATE / 2 > freq;
      FrequencyLabel.BackColor = ModeLabel.BackColor = highlighted ? Color.LightGreen : SystemColors.Control;
      Invalidate();
    }

    private void FineTune_MouseDown(object sender, MouseEventArgs e)
    {
      int dF = e.X > ClientSize.Width / 2 ? 100 : -100;
      Receiver.SetFrequency((int)(Receiver.Settings.Frequency + dF));
      UpdateLabels();
    }
  }
}
