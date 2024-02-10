using System.Diagnostics;
using MathNet.Numerics;
using VE3NEA;
using VE3NEA.HamCockpit.DspFun;
using WeifenLuo.WinFormsUI.Docking;

namespace JTSkimmer
{
  public partial class MainForm : Form
  {
    private Context ctx;
    internal WidebandSpectrumAnalyzer SpectrumAnalyzer;


    //----------------------------------------------------------------------------------------------
    //                                       init
    //----------------------------------------------------------------------------------------------
    public MainForm()
    {
      InitializeComponent();

      Text = Utils.GetVersionString();

      DecoderRunner.TryDeleteAllTempFolders();

      ctx = new(this);
      ctx.Settings.LoadFromFile();

      Fft<float>.LoadWisdom(Path.Combine(Utils.GetUserDataFolder(), "wsjtx_wisdom.dat"));
      SpectrumAnalyzer = new(16384, 2048);
      SpectrumAnalyzer.SpectrumAvailable += Spect_SpectrumAvailable;

      ctx.IqOutput = new(ctx);
      ctx.IqOutput.Enabled = false;

      CreateReceivers();

      ctx.SpeakerSoundcard.StateChanged += Soundcard_StateChanged;
      ctx.VacSoundcard.StateChanged += Soundcard_StateChanged;
      ApplyAudioSettings();
      EnableDisableSoundcards();

      ctx.OmniRig.StatusChanged += OmniRig_StatusChanged;
      ctx.OmniRig.TransmitChanged += OmniRig_TransmitChanged;
      ApplyOmnirigSettings();

      ctx.MessageDistributor.ApplySettings();

      Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      ctx.Settings.Ui.RestoreWindowPosition(this);
      if (!ctx.Settings.Ui.RestoreDockingLayout(this))
        SetDefaultDockingLayout();

      StartSdr();
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      StopSdr();

      ctx.Settings.Ui.StoreDockingLayout(DockHost);
      ctx.Settings.Ui.StoreWindowPosition(this);

      ctx.Settings.Receivers = ctx.Receivers.Select(r => r.Settings).ToList();
      ctx.Settings.SaveToFile();

      foreach (var rx in ctx.Receivers) rx.Dispose();
      ctx.Sdr?.Dispose();
      ctx.SpeakerSoundcard.Dispose();
      ctx.VacSoundcard?.Dispose();
      ctx.BandViewPanel?.Dispose();
      ctx.MessageDistributor?.Dispose();
      ctx.OmniRig.Active = false;

      Fft<float>.SaveWisdom();
    }




    //----------------------------------------------------------------------------------------------
    //                                         Sdr
    //----------------------------------------------------------------------------------------------
    private void StartSdr()
    {
      ctx.Sdr = BaseSdrDevice.CreateFromSettings(ctx.Settings.SelectedSdr());

      if (ctx.Sdr != null)
      {
        ctx.Downsampler = new(ctx.Sdr.SamplingRate, ctx.Sdr.Info.Settings.Bandwidth);
        ctx.Downsampler.NoiseBlanker.ApplySettings(ctx.Settings.NoiseBlanker);
        ctx.Downsampler.DataAvailable += Downsampler_DataAvailable;
        ctx.Downsampler.Enabled = !MustPause();

        ctx.Sdr.StateChanged += Sdr_StateChanged;
        ctx.Sdr.DataAvailable += Sdr_DataAvailable;
        ctx.Sdr.Enabled = true;
      }
    }

    private void StopSdr()
    {
      ctx.Sdr?.Dispose();
      ctx.Sdr = null;

      ctx.Downsampler?.Dispose();
      ctx.Downsampler = null;

      ctx.IqOutput.Stop();

      EnableDisableWaterfalls();
    }

    private void Sdr_StateChanged(object? sender, EventArgs e)
    {
      SetUpGainTrackbar();
      ShowSdrLabel();

      ApplySettingsToAllWaterfalls(); // ???
      SetWaterfallSpeed();
      EnableDisableWaterfalls();
      ctx.IqOutput.ApplySettings();
    }

    private void Sdr_DataAvailable(object? sender, DataEventArgs<Complex32> e)
    {
      ctx.Downsampler?.StartProcessing(e);
    }

    private void Downsampler_DataAvailable(object? sender, DataEventArgs<Complex32> e)
    {
      foreach (var rx in ctx.Receivers) rx.StartProcessing(e);

      ctx.IqOutput.StartProcessing(e);

      // compute wide spectrum to track noise floor, even if BandView is not open
      SpectrumAnalyzer.StartProcessing(e);
    }

    private void Spect_SpectrumAvailable(object? sender, DataEventArgs<float> e)
    {
      ctx.BandViewPanel?.AppendSpectrum(e.Data);
    }

    private void SetUpGainTrackbar()
    {
      ChangingSdrGain = true;

      SdrGainTrackbar.Enabled = ctx.Sdr != null && ctx.Sdr.Running && ctx.Sdr.Gains.Length > 0;

      if (SdrGainTrackbar.Enabled)
      {
        SdrGainTrackbar.Maximum = ctx.Sdr.Gains.Length - 1;
        SdrGainTrackbar.Value = (int)ctx.Sdr.GainIndex;
      }
      else
        SdrGainTrackbar.Value = 0;

      ShowSdrGain();

      ChangingSdrGain = false;
    }

    private void ShowSdrLabel()
    {
      SdrStatusLabel.ToolTipText = ctx.Sdr?.GetDesctiption() ?? "SDR not selected.\n\nClick here to select";

      if (ctx.Sdr == null) SdrLedLabel.ForeColor = Color.Silver;
      else if (ctx.Sdr.Running) SdrLedLabel.ForeColor = Color.Lime;
      else SdrLedLabel.ForeColor = Color.Red;
    }

    private void ShowSdrGain()
    {
      if (ctx.Sdr != null && ctx.Sdr.Gains.Length > ctx.Sdr.GainIndex)
        SdrGainLabel.Text = ctx.Sdr.GainIndex.ToString();
      else
        SdrGainLabel.Text = "N/A";
    }




    //----------------------------------------------------------------------------------------------
    //                                    receiver
    //----------------------------------------------------------------------------------------------
    private void CreateReceivers()
    {
      Receiver.SdrSettings = ctx.Settings.SelectedSdr()?.Settings;
      Receiver.DecodingSettings = ctx.Settings.Decoding;
      ctx.Receivers = ctx.Settings.Receivers.Select(sett => new Receiver(sett)).ToList();
      foreach (var rx in ctx.Receivers)
      {
        rx.DataAvailable += Receiver_DataAvailable;
        rx.DecoderEvent += ctx.MessageDistributor.Receiver_DecoderEvent;
      }
    }

    internal void AddReceiver(uint? frequency = null)
    {
      var receiver = frequency == null ? new Receiver() : new Receiver((uint)frequency);
      receiver.DataAvailable += Receiver_DataAvailable;
      receiver.DecoderEvent += ctx.MessageDistributor.Receiver_DecoderEvent;
      ctx.Receivers.Add(receiver);
      if (ctx.ReceiversPanel != null)
      {
        var panel = ctx.ReceiversPanel.AddReceiverPanel(receiver);
        panel.EnableDisable();
        panel.SetWaterfallSpeed();
        ApplySettingsToWaterfall(panel.WaterfallControl, ctx.Settings.Waterfall);
      }

      ctx.BandViewPanel?.ScalePanel?.Invalidate();
    }

    // called from settings dialog
    internal void ApplyDecodingSettings()
    {
      Receiver.DecodingSettings = ctx.Settings.Decoding;
      foreach (Receiver rx in ctx.Receivers) rx.SetUpDecoding();
      ctx.ReceiversPanel?.UpdateLabels();
    }


    private void Receiver_DataAvailable(object? sender, DataEventArgs<float> e)
    {
      var receiver = (Receiver?)sender;
      if (receiver == null) return;

      // audio output
      if (receiver.Settings.AudioToSpeaker) ctx.SpeakerSoundcard.AddSamples(e.Data);
      if (receiver.Settings.AudioToVac) ctx.VacSoundcard.AddSamples(e.Data);
    }

    internal void MoveReceiverInList(Receiver fromReceiver, Receiver toReceiver)
    {
      int pos = ctx.Receivers.IndexOf(toReceiver);
      ctx.Receivers.Remove(fromReceiver);
      ctx.Receivers.Insert(pos, fromReceiver);
    }





    //----------------------------------------------------------------------------------------------
    //                                    waterfall
    //----------------------------------------------------------------------------------------------
    internal void ApplySettingsToAllWaterfalls()
    {
      if (ctx.BandViewPanel != null)
      {
        ApplySettingsToWaterfall(ctx.BandViewPanel.WaterfallControl, ctx.Settings.Waterfall);
        ctx.BandViewPanel.ScalePanel.Invalidate();
      }

      if (ctx.ReceiversPanel != null) foreach (ReceiverPanel panel in ctx.ReceiversPanel.TableLayoutPanel.Controls)
          ApplySettingsToWaterfall(panel.WaterfallControl, ctx.Settings.Waterfall);
    }

    internal void ApplySettingsToWaterfall(WaterfallControl control, WaterfallSettings sett)
    {
      control.Brightness = sett.Brightness;
      control.Contrast = sett.Contrast;
      int paletteIndex = Math.Min(ctx.PaletteManager.Palettes.Count() - 1, sett.PaletteIndex);
      control.SetPalette(ctx.PaletteManager.Palettes[paletteIndex]);
      control.Refresh();
    }

    public void EnableDisableWaterfalls()
    {
      ctx.BandViewPanel?.EnableDisable();

      if (ctx.ReceiversPanel != null)
        foreach (ReceiverPanel panel in ctx.ReceiversPanel.TableLayoutPanel.Controls)
          panel.EnableDisable();
    }

    public void SetWaterfallSpeed()
    {
      ctx.BandViewPanel?.SetWaterfallSpeed();

      if (ctx.ReceiversPanel != null) foreach (ReceiverPanel panel in ctx.ReceiversPanel.TableLayoutPanel.Controls)
          panel.SetWaterfallSpeed();
    }




    //----------------------------------------------------------------------------------------------
    //                                    omnirig
    //----------------------------------------------------------------------------------------------
    public void ApplyOmnirigSettings()
    {
      ctx.OmniRig.Active = false;
      ctx.OmniRig.RigNo = (int)ctx.Settings.OmniRig.RigNo;
      ctx.OmniRig.Active = ctx.Settings.OmniRig.RigNo != OmniRigRig.None;
      ShowOmnirigStatus();
      PauseResume();
    }

    private void OmniRig_StatusChanged(object? sender, EventArgs e)
    {
      ShowOmnirigStatus();
    }

    private void OmniRig_TransmitChanged(object? sender, EventArgs e)
    {
      PauseResume();
    }

    public void PauseResume()
    {
      if (ctx.Downsampler != null) ctx.Downsampler.Enabled = !MustPause();

      EnableDisableWaterfalls();
    }

    private void ShowOmnirigStatus()
    {
      switch (ctx.OmniRig.Status)
      {
        case RigStatus.Disabled:
          OmniRigLedLabel.ForeColor = Color.Gray; break;
        case RigStatus.Online:
          OmniRigLedLabel.ForeColor = Color.Lime; break;
        default:
          OmniRigLedLabel.ForeColor = Color.Red; break;
      }

      OmniRigStatusLabel.ToolTipText = ctx.OmniRig.GetStatusText();
    }

    public bool MustPause()
    {
      return ctx.Settings.OmniRig.PauseWhenTx && ctx.OmniRig.Transmit;
    }




    //----------------------------------------------------------------------------------------------
    //                                     soundcards
    //----------------------------------------------------------------------------------------------
    internal void ApplyAudioSettings()
    {
      var sett = ctx.Settings.Audio;

      ctx.SpeakerSoundcard.SetDeviceId(sett.SpeakerSoundcard);
      VolumeTrackbar.Value = sett.SoundcardVolume;
      SetSoundcardVolume();

      ctx.VacSoundcard.SetDeviceId(sett.Vac);
      ctx.VacSoundcard.Volume = Dsp.FromDb2(sett.VacVolume);

      EnableDisableSoundcards();
    }

    internal void EnableDisableSoundcards()
    {
      ctx.SpeakerSoundcard.Enabled = ctx.Receivers.Any(r => r.Settings.AudioToSpeaker);
      ctx.VacSoundcard.Enabled = ctx.Receivers.Any(r => r.Settings.AudioToVac);
    }

    private void Soundcard_StateChanged(object? sender, EventArgs e)
    {
      ShowSoundcardLabels();
    }

    private void VolumeTrackbar_ValueChanged(object sender, EventArgs e)
    {
      SetSoundcardVolume();
    }

    private void SetSoundcardVolume()
    {
      VolumeLabel.Text = $"{VolumeTrackbar.Value} dB";
      ctx.Settings.Audio.SoundcardVolume = VolumeTrackbar.Value;
      ctx.SpeakerSoundcard.Volume = Dsp.FromDb2(ctx.Settings.Audio.SoundcardVolume);
    }

    public void ShowSoundcardLabels()
    {
      if (ctx.SpeakerSoundcard.Enabled)
        SoundcardLedLabel.ForeColor = Color.Lime;
      else if (ctx.Receivers.Any(rx => rx.Settings.AudioToSpeaker))
        SoundcardLedLabel.ForeColor = Color.Red;
      else
        SoundcardLedLabel.ForeColor = Color.Gray;

      if (ctx.VacSoundcard.Enabled)
        VacLedLabel.ForeColor = Color.Lime;
      else if (ctx.Receivers.Any(rx => rx.Settings.AudioToVac))
        VacLedLabel.ForeColor = Color.Red;
      else
        VacLedLabel.ForeColor = Color.Gray;

      SoundcardStatusLabel.ToolTipText = ctx.SpeakerSoundcard.GetDisplayName();
      VacStatusLabel.ToolTipText = ctx.VacSoundcard.GetDisplayName();
    }




    //----------------------------------------------------------------------------------------------
    //                                         docking
    //----------------------------------------------------------------------------------------------
    private void SetDefaultDockingLayout()
    {
      ViewReceiversMNU_Click(null, null);
      ViewBandViewMNU_Click(null, null);
      ViewMessagesMNU_Click(null, null);
    }

    public IDockContent MakeDockContentFromPersistString(string persistString)
    {
      switch (persistString)
      {
        case "JTSkimmer.ReceiversPanel": return new ReceiversPanel(ctx);
        case "JTSkimmer.BandViewPanel": return new BandViewPanel(ctx);
        case "JTSkimmer.MessagesPanel": return new MessagesPanel(ctx);
        default: return null;
      }
    }




    //----------------------------------------------------------------------------------------------
    //                                        menu
    //----------------------------------------------------------------------------------------------
    private void ExitMNU_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void ViewReceiversMNU_Click(object sender, EventArgs e)
    {
      if (ctx.ReceiversPanel == null)
      {
        new ReceiversPanel(ctx).Show(DockHost, DockState.Document);

        ApplySettingsToAllWaterfalls();
        SetWaterfallSpeed();
        EnableDisableWaterfalls();
      }
      else
        ctx.ReceiversPanel.Close();
    }

    private void ViewBandViewMNU_Click(object sender, EventArgs e)
    {
      if (ctx.BandViewPanel == null)
      {
        new BandViewPanel(ctx).Show(DockHost, DockState.DockBottom);
        ctx.BandViewPanel.EnableDisable();
        SetWaterfallSpeed();
      }
      else
        ctx.BandViewPanel.Close();
    }

    private void ViewMessagesMNU_Click(object sender, EventArgs e)
    {
      if (ctx.MessagesPanel == null)
        new MessagesPanel(ctx).Show(DockHost, DockState.DockRight);
      else
        ctx.MessagesPanel.Close();
    }

    private void SettingsMNU_Click(object sender, EventArgs e)
    {
      new SettingsDialog(ctx).ShowDialog();
    }

    private void SdrDevicesMNU_Click(object sender, EventArgs e)
    {
      StopSdr();
      ShowSdrLabel();
      var rc = new SdrDevicesDialog(ctx).ShowDialog();
      Receiver.SdrSettings = ctx.Settings.SelectedSdr()?.Settings;
      StartSdr();
    }

    private void WebsiteMNU_Click(object sender, EventArgs e)
    {
      Process.Start(new ProcessStartInfo("https://ve3nea.github.io/JTSkimmer") { UseShellExecute = true });
    }

    private void UserDataFolderMNU_Click(object sender, EventArgs e)
    {
      Process.Start("explorer.exe", Utils.GetUserDataFolder());
    }

    private void ReferenceDataFolderMNU_Click(object sender, EventArgs e)
    {
      Process.Start("explorer.exe", Utils.GetReferenceDataFolder());
    }

    private void AboutMNU_Click(object sender, EventArgs e)
    {
      new AboutBox().ShowDialog();
    }

    private void SetNfCalibrationMNU_Click(object sender, EventArgs e)
    {
      ctx.Settings.NoiseFloorZero = SpectrumAnalyzer.Median;
    }

    private void ClearNfCalibrationMNU_Click(object sender, EventArgs e)
    {
      ctx.Settings.NoiseFloorZero = null;
    }

    private void AddReceiverBtn_Click(object sender, EventArgs e)
    {
      if (ctx.ReceiversPanel == null) ViewReceiversMNU_Click(sender, e);
      AddReceiver();
    }

    private void NoiseBlankerBtn_Click(object sender, EventArgs e)
    {
      new NoiseBlankerDialog(ctx).ShowDialog();
    }

    private void NoiseFloorLabel_Click(object sender, EventArgs e)
    {
      NoiseFloorMenu.Show(StatusStrip, NoiseFloorLabel.Bounds.Location, ToolStripDropDownDirection.AboveRight);
    }

    private void OmniRigStatusLabel_Click(object sender, EventArgs e)
    {
      ctx.OmniRig.ShowDialog();
    }




    //----------------------------------------------------------------------------------------------
    //                                      events
    //----------------------------------------------------------------------------------------------
    bool ChangingSdrGain;
    private void SdrGainTrackbar_ValueChanged(object sender, EventArgs e)
    {
      if (ctx.Sdr == null || ChangingSdrGain) return;

      ctx.Sdr.GainIndex = (uint)((TrackBar)sender).Value;
      ShowSdrGain();
    }

    private void StatusLabel_MouseEnter(object sender, EventArgs e)
    {
      Cursor = Cursors.Hand;
    }

    private void StatusLabel_MouseLeave(object sender, EventArgs e)
    {
      Cursor = Cursors.Default;
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
      // noise floor
      if (SpectrumAnalyzer.Median <= 0)
        NoiseFloorLabel.Text = "";
      else
      {
        var baseNf = ctx.Settings.NoiseFloorZero ?? 1;
        var nf = (int)Math.Round(10 * Math.Log10(SpectrumAnalyzer.Median / baseNf));
        NoiseFloorLabel.Text = $"Noise Floor: {nf} dB  ";
      }

      // cpu
      CpuLoadlabel.Text = $"CPU Load: {GetCpuUsage():F1}%";


      // distributor
      bool allDisabled =
        !ctx.Settings.Distributor.UdpSender.Enabled &&
        !ctx.Settings.Distributor.DxCluster.Enabled &&
        !ctx.Settings.Distributor.PskReporter.Enabled;

      bool allOk = ctx.MessageDistributor.WsjtxUdpSender.Active == ctx.Settings.Distributor.UdpSender.Enabled &&
        ctx.MessageDistributor.DxClusterServer.Active == ctx.Settings.Distributor.DxCluster.Enabled &&
        ctx.MessageDistributor.PskReporterUdpSender.Active == ctx.Settings.Distributor.PskReporter.Enabled;

      if (allDisabled) NetworkLedLabel.ForeColor = Color.Gray;
      else if (allOk) NetworkLedLabel.ForeColor = Color.Lime;
      else NetworkLedLabel.ForeColor = Color.Red;

      NetworkStatusLabel.ToolTipText = ctx.MessageDistributor.GetStatusString();

      // I/Q Output
      if (!ctx.IqOutput.Enabled)
      {
        IqOutputLedLabel.ForeColor = Color.Gray;
        IqOutputStatusLabel.ToolTipText = "Disabled";
      }
      else if (ctx.IqOutput.Active)
      {
        IqOutputLedLabel.ForeColor = Color.Lime;
        IqOutputStatusLabel.ToolTipText = "Running";
      }
      else
      {
        IqOutputLedLabel.ForeColor = Color.Red;
        IqOutputStatusLabel.ToolTipText = ctx.IqOutput.LastError;
      }
    }

    DateTime StartTime;
    double StartSeconds;

    private float GetCpuUsage()
    {
      var time = DateTime.UtcNow;
      var seconds = AppDomain.CurrentDomain.MonitoringTotalProcessorTime.TotalSeconds;
      var result = (seconds - StartSeconds) / (time - StartTime).TotalSeconds * 100d / Environment.ProcessorCount;
      StartTime = time;
      StartSeconds = seconds;

      return (float)result;
    }




    //----------------------------------------------------------------------------------------------
    //                                      systray
    //----------------------------------------------------------------------------------------------
    private void ShowWindowMNU_Click(object sender, EventArgs e)
    {
      Show();
      ShowInTaskbar = true;
    }

    private void Form1_Resize(object sender, EventArgs e)
    {
      bool minimized = WindowState == FormWindowState.Minimized;
      ShowInTaskbar = !minimized;
    }

    private void SystrayIcon_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Left) return;

      bool minimized = WindowState == FormWindowState.Minimized;
      bool show = !Visible || minimized;

      ShowInTaskbar = show;
      Visible = show;
      if (show && minimized) WindowState = FormWindowState.Normal;
    }

    private void DockHost_ActiveContentChanged(object sender, EventArgs e)
    {

    }
  }
}