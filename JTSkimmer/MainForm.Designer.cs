namespace JTSkimmer
{
  partial class MainForm
  {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      MainMenu = new MenuStrip();
      fileToolStripMenuItem = new ToolStripMenuItem();
      toolStripMenuItem1 = new ToolStripMenuItem();
      viewToolStripMenuItem = new ToolStripMenuItem();
      ViewReceiversMNU = new ToolStripMenuItem();
      ViewBandViewMNU = new ToolStripMenuItem();
      ViewMessagesMNU = new ToolStripMenuItem();
      toolsToolStripMenuItem = new ToolStripMenuItem();
      SettingsMNU = new ToolStripMenuItem();
      SdrDevicesMNU = new ToolStripMenuItem();
      helpToolStripMenuItem = new ToolStripMenuItem();
      toolStripMenuItem2 = new ToolStripSeparator();
      UserDataFolderMNU = new ToolStripMenuItem();
      ReferenceDataFolderMNU = new ToolStripMenuItem();
      toolStripSeparator2 = new ToolStripSeparator();
      WebsitelMNU = new ToolStripMenuItem();
      EmailTheAuthorMNU = new ToolStripMenuItem();
      AboutMNU = new ToolStripMenuItem();
      StatusStrip = new StatusStrip();
      toolStripStatusLabel2 = new ToolStripStatusLabel();
      SdrLedLabel = new ToolStripStatusLabel();
      SdrStatusLabel = new ToolStripStatusLabel();
      SoundcardLedLabel = new ToolStripStatusLabel();
      SoundcardStatusLabel = new ToolStripStatusLabel();
      VacLedLabel = new ToolStripStatusLabel();
      VacStatusLabel = new ToolStripStatusLabel();
      OmniRigLedLabel = new ToolStripStatusLabel();
      OmniRigStatusLabel = new ToolStripStatusLabel();
      NetworkLedLabel = new ToolStripStatusLabel();
      NetworkStatusLabel = new ToolStripStatusLabel();
      IqOutputLedLabel = new ToolStripStatusLabel();
      IqOutputStatusLabel = new ToolStripStatusLabel();
      NoiseFloorLabel = new ToolStripStatusLabel();
      CpuLoadlabel = new ToolStripStatusLabel();
      SetNfCalibrationMNU = new ToolStripMenuItem();
      ClearNfCalibrationMNU = new ToolStripMenuItem();
      SystrayIcon = new NotifyIcon(components);
      SystrayMenu = new ContextMenuStrip(components);
      ShowWindowMNU = new ToolStripMenuItem();
      toolStripSeparator1 = new ToolStripSeparator();
      ExitMNU = new ToolStripMenuItem();
      DockHost = new WeifenLuo.WinFormsUI.Docking.DockPanel();
      vS2015LightTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015LightTheme();
      SdrGainLabel = new Label();
      SdrGainTrackbar = new TrackBar();
      ToolTip = new ToolTip(components);
      VolumeTrackbar = new TrackBar();
      panel1 = new Panel();
      NoiseBlankerBtn = new Button();
      label2 = new Label();
      label1 = new Label();
      AddReceiverBtn = new Button();
      VolumeLabel = new Label();
      Timer = new System.Windows.Forms.Timer(components);
      NoiseFloorMenu = new ContextMenuStrip(components);
      MainMenu.SuspendLayout();
      StatusStrip.SuspendLayout();
      SystrayMenu.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)SdrGainTrackbar).BeginInit();
      ((System.ComponentModel.ISupportInitialize)VolumeTrackbar).BeginInit();
      panel1.SuspendLayout();
      NoiseFloorMenu.SuspendLayout();
      SuspendLayout();
      // 
      // MainMenu
      // 
      MainMenu.ImageScalingSize = new Size(24, 24);
      MainMenu.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, viewToolStripMenuItem, toolsToolStripMenuItem, helpToolStripMenuItem });
      MainMenu.Location = new Point(0, 0);
      MainMenu.Name = "MainMenu";
      MainMenu.Size = new Size(1537, 24);
      MainMenu.TabIndex = 0;
      MainMenu.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem1 });
      fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      fileToolStripMenuItem.Size = new Size(37, 20);
      fileToolStripMenuItem.Text = "File";
      // 
      // toolStripMenuItem1
      // 
      toolStripMenuItem1.Name = "toolStripMenuItem1";
      toolStripMenuItem1.Size = new Size(93, 22);
      toolStripMenuItem1.Text = "Exit";
      toolStripMenuItem1.Click += ExitMNU_Click;
      // 
      // viewToolStripMenuItem
      // 
      viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ViewReceiversMNU, ViewBandViewMNU, ViewMessagesMNU });
      viewToolStripMenuItem.Name = "viewToolStripMenuItem";
      viewToolStripMenuItem.Size = new Size(44, 20);
      viewToolStripMenuItem.Text = "View";
      // 
      // ViewReceiversMNU
      // 
      ViewReceiversMNU.Name = "ViewReceiversMNU";
      ViewReceiversMNU.Size = new Size(175, 22);
      ViewReceiversMNU.Text = "Receivers";
      ViewReceiversMNU.Click += ViewReceiversMNU_Click;
      // 
      // ViewBandViewMNU
      // 
      ViewBandViewMNU.Name = "ViewBandViewMNU";
      ViewBandViewMNU.Size = new Size(175, 22);
      ViewBandViewMNU.Text = "Band View";
      ViewBandViewMNU.Click += ViewBandViewMNU_Click;
      // 
      // ViewMessagesMNU
      // 
      ViewMessagesMNU.Name = "ViewMessagesMNU";
      ViewMessagesMNU.Size = new Size(175, 22);
      ViewMessagesMNU.Text = "Decoded Messages";
      ViewMessagesMNU.Click += ViewMessagesMNU_Click;
      // 
      // toolsToolStripMenuItem
      // 
      toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { SettingsMNU, SdrDevicesMNU });
      toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
      toolsToolStripMenuItem.Size = new Size(46, 20);
      toolsToolStripMenuItem.Text = "Tools";
      // 
      // SettingsMNU
      // 
      SettingsMNU.Name = "SettingsMNU";
      SettingsMNU.Size = new Size(147, 22);
      SettingsMNU.Text = "Settings...";
      SettingsMNU.Click += SettingsMNU_Click;
      // 
      // SdrDevicesMNU
      // 
      SdrDevicesMNU.Name = "SdrDevicesMNU";
      SdrDevicesMNU.Size = new Size(147, 22);
      SdrDevicesMNU.Text = "SDR Devices...";
      SdrDevicesMNU.Click += SdrDevicesMNU_Click;
      // 
      // helpToolStripMenuItem
      // 
      helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem2, UserDataFolderMNU, ReferenceDataFolderMNU, toolStripSeparator2, WebsitelMNU, EmailTheAuthorMNU, AboutMNU });
      helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      helpToolStripMenuItem.Size = new Size(44, 20);
      helpToolStripMenuItem.Text = "Help";
      // 
      // toolStripMenuItem2
      // 
      toolStripMenuItem2.Name = "toolStripMenuItem2";
      toolStripMenuItem2.Size = new Size(195, 6);
      // 
      // UserDataFolderMNU
      // 
      UserDataFolderMNU.Name = "UserDataFolderMNU";
      UserDataFolderMNU.Size = new Size(198, 22);
      UserDataFolderMNU.Text = "User Data Folder...";
      UserDataFolderMNU.Click += UserDataFolderMNU_Click;
      // 
      // ReferenceDataFolderMNU
      // 
      ReferenceDataFolderMNU.Name = "ReferenceDataFolderMNU";
      ReferenceDataFolderMNU.Size = new Size(198, 22);
      ReferenceDataFolderMNU.Text = "Reference Data Folder...";
      ReferenceDataFolderMNU.Click += ReferenceDataFolderMNU_Click;
      // 
      // toolStripSeparator2
      // 
      toolStripSeparator2.Name = "toolStripSeparator2";
      toolStripSeparator2.Size = new Size(195, 6);
      // 
      // WebsitelMNU
      // 
      WebsitelMNU.Name = "WebsitelMNU";
      WebsitelMNU.Size = new Size(198, 22);
      WebsitelMNU.Text = "Web Site...";
      WebsitelMNU.Click += WebsiteMNU_Click;
      // 
      // EmailTheAuthorMNU
      // 
      EmailTheAuthorMNU.Name = "EmailTheAuthorMNU";
      EmailTheAuthorMNU.Size = new Size(198, 22);
      EmailTheAuthorMNU.Text = "Email the Author...";
      EmailTheAuthorMNU.Click += EmailTheAuthorMNU_Click;
      // 
      // AboutMNU
      // 
      AboutMNU.Name = "AboutMNU";
      AboutMNU.Size = new Size(198, 22);
      AboutMNU.Text = "About...";
      AboutMNU.Click += AboutMNU_Click;
      // 
      // StatusStrip
      // 
      StatusStrip.ImageScalingSize = new Size(24, 24);
      StatusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel2, SdrLedLabel, SdrStatusLabel, SoundcardLedLabel, SoundcardStatusLabel, VacLedLabel, VacStatusLabel, OmniRigLedLabel, OmniRigStatusLabel, NetworkLedLabel, NetworkStatusLabel, IqOutputLedLabel, IqOutputStatusLabel, NoiseFloorLabel, CpuLoadlabel });
      StatusStrip.Location = new Point(0, 916);
      StatusStrip.Name = "StatusStrip";
      StatusStrip.ShowItemToolTips = true;
      StatusStrip.Size = new Size(1537, 35);
      StatusStrip.TabIndex = 1;
      StatusStrip.Text = "statusStrip1";
      // 
      // toolStripStatusLabel2
      // 
      toolStripStatusLabel2.AutoSize = false;
      toolStripStatusLabel2.Name = "toolStripStatusLabel2";
      toolStripStatusLabel2.Size = new Size(10, 30);
      // 
      // SdrLedLabel
      // 
      SdrLedLabel.Font = new Font("Webdings", 9F, FontStyle.Regular, GraphicsUnit.Point);
      SdrLedLabel.ForeColor = Color.Gray;
      SdrLedLabel.Name = "SdrLedLabel";
      SdrLedLabel.Size = new Size(21, 30);
      SdrLedLabel.Text = "n";
      SdrLedLabel.Click += SdrDevicesMNU_Click;
      SdrLedLabel.MouseEnter += StatusLabel_MouseEnter;
      SdrLedLabel.MouseLeave += StatusLabel_MouseLeave;
      // 
      // SdrStatusLabel
      // 
      SdrStatusLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
      SdrStatusLabel.Name = "SdrStatusLabel";
      SdrStatusLabel.Size = new Size(42, 30);
      SdrStatusLabel.Text = "SDR  ";
      SdrStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
      SdrStatusLabel.Click += SdrDevicesMNU_Click;
      SdrStatusLabel.MouseEnter += StatusLabel_MouseEnter;
      SdrStatusLabel.MouseLeave += StatusLabel_MouseLeave;
      // 
      // SoundcardLedLabel
      // 
      SoundcardLedLabel.Font = new Font("Webdings", 9F, FontStyle.Regular, GraphicsUnit.Point);
      SoundcardLedLabel.ForeColor = Color.Gray;
      SoundcardLedLabel.Name = "SoundcardLedLabel";
      SoundcardLedLabel.Size = new Size(21, 30);
      SoundcardLedLabel.Text = "n";
      // 
      // SoundcardStatusLabel
      // 
      SoundcardStatusLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
      SoundcardStatusLabel.Name = "SoundcardStatusLabel";
      SoundcardStatusLabel.Size = new Size(82, 30);
      SoundcardStatusLabel.Text = "Soundcard  ";
      SoundcardStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // VacLedLabel
      // 
      VacLedLabel.Font = new Font("Webdings", 9F, FontStyle.Regular, GraphicsUnit.Point);
      VacLedLabel.ForeColor = Color.Gray;
      VacLedLabel.Name = "VacLedLabel";
      VacLedLabel.Size = new Size(21, 30);
      VacLedLabel.Text = "n";
      // 
      // VacStatusLabel
      // 
      VacStatusLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
      VacStatusLabel.Name = "VacStatusLabel";
      VacStatusLabel.Size = new Size(43, 30);
      VacStatusLabel.Text = "VAC  ";
      VacStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // OmniRigLedLabel
      // 
      OmniRigLedLabel.Font = new Font("Webdings", 9F, FontStyle.Regular, GraphicsUnit.Point);
      OmniRigLedLabel.ForeColor = Color.Gray;
      OmniRigLedLabel.Name = "OmniRigLedLabel";
      OmniRigLedLabel.Size = new Size(21, 30);
      OmniRigLedLabel.Text = "n";
      OmniRigLedLabel.Click += OmniRigStatusLabel_Click;
      OmniRigLedLabel.MouseEnter += StatusLabel_MouseEnter;
      OmniRigLedLabel.MouseLeave += StatusLabel_MouseLeave;
      // 
      // OmniRigStatusLabel
      // 
      OmniRigStatusLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
      OmniRigStatusLabel.Name = "OmniRigStatusLabel";
      OmniRigStatusLabel.Size = new Size(70, 30);
      OmniRigStatusLabel.Text = "OmniRig  ";
      OmniRigStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
      OmniRigStatusLabel.Click += OmniRigStatusLabel_Click;
      OmniRigStatusLabel.MouseEnter += StatusLabel_MouseEnter;
      OmniRigStatusLabel.MouseLeave += StatusLabel_MouseLeave;
      // 
      // NetworkLedLabel
      // 
      NetworkLedLabel.Font = new Font("Webdings", 9F, FontStyle.Regular, GraphicsUnit.Point);
      NetworkLedLabel.ForeColor = Color.Gray;
      NetworkLedLabel.Name = "NetworkLedLabel";
      NetworkLedLabel.Size = new Size(21, 30);
      NetworkLedLabel.Text = "n";
      // 
      // NetworkStatusLabel
      // 
      NetworkStatusLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
      NetworkStatusLabel.Name = "NetworkStatusLabel";
      NetworkStatusLabel.Size = new Size(69, 30);
      NetworkStatusLabel.Text = "Network  ";
      NetworkStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
      NetworkStatusLabel.ToolTipText = "Network";
      // 
      // IqOutputLedLabel
      // 
      IqOutputLedLabel.Font = new Font("Webdings", 9F, FontStyle.Regular, GraphicsUnit.Point);
      IqOutputLedLabel.ForeColor = Color.Gray;
      IqOutputLedLabel.Name = "IqOutputLedLabel";
      IqOutputLedLabel.Size = new Size(21, 30);
      IqOutputLedLabel.Text = "n";
      // 
      // IqOutputStatusLabel
      // 
      IqOutputStatusLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
      IqOutputStatusLabel.Name = "IqOutputStatusLabel";
      IqOutputStatusLabel.Size = new Size(86, 30);
      IqOutputStatusLabel.Text = "I/Q Output  ";
      IqOutputStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // NoiseFloorLabel
      // 
      NoiseFloorLabel.Name = "NoiseFloorLabel";
      NoiseFloorLabel.Size = new Size(119, 30);
      NoiseFloorLabel.Text = "Noise Floor: -100 dB  ";
      NoiseFloorLabel.TextAlign = ContentAlignment.MiddleLeft;
      NoiseFloorLabel.Click += NoiseFloorLabel_Click;
      NoiseFloorLabel.MouseEnter += StatusLabel_MouseEnter;
      NoiseFloorLabel.MouseLeave += StatusLabel_MouseLeave;
      // 
      // CpuLoadlabel
      // 
      CpuLoadlabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
      CpuLoadlabel.Name = "CpuLoadlabel";
      CpuLoadlabel.Size = new Size(96, 30);
      CpuLoadlabel.Text = "CPU Load: 00.0%";
      CpuLoadlabel.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // SetNfCalibrationMNU
      // 
      SetNfCalibrationMNU.Name = "SetNfCalibrationMNU";
      SetNfCalibrationMNU.Size = new Size(174, 22);
      SetNfCalibrationMNU.Text = "Set Current as Zero";
      SetNfCalibrationMNU.Click += SetNfCalibrationMNU_Click;
      // 
      // ClearNfCalibrationMNU
      // 
      ClearNfCalibrationMNU.Name = "ClearNfCalibrationMNU";
      ClearNfCalibrationMNU.Size = new Size(174, 22);
      ClearNfCalibrationMNU.Text = "Clear Calibration";
      ClearNfCalibrationMNU.Click += ClearNfCalibrationMNU_Click;
      // 
      // SystrayIcon
      // 
      SystrayIcon.BalloonTipText = "fghgh";
      SystrayIcon.BalloonTipTitle = "utyutyu";
      SystrayIcon.ContextMenuStrip = SystrayMenu;
      SystrayIcon.Icon = (Icon)resources.GetObject("SystrayIcon.Icon");
      SystrayIcon.Text = "JT Skimmer";
      SystrayIcon.Visible = true;
      SystrayIcon.MouseDown += SystrayIcon_MouseDown;
      // 
      // SystrayMenu
      // 
      SystrayMenu.ImageScalingSize = new Size(24, 24);
      SystrayMenu.Items.AddRange(new ToolStripItem[] { ShowWindowMNU, toolStripSeparator1, ExitMNU });
      SystrayMenu.Name = "SystrayMenu";
      SystrayMenu.Size = new Size(151, 54);
      // 
      // ShowWindowMNU
      // 
      ShowWindowMNU.Name = "ShowWindowMNU";
      ShowWindowMNU.Size = new Size(150, 22);
      ShowWindowMNU.Text = "Show Window";
      ShowWindowMNU.Click += ShowWindowMNU_Click;
      // 
      // toolStripSeparator1
      // 
      toolStripSeparator1.Name = "toolStripSeparator1";
      toolStripSeparator1.Size = new Size(147, 6);
      // 
      // ExitMNU
      // 
      ExitMNU.Name = "ExitMNU";
      ExitMNU.Size = new Size(150, 22);
      ExitMNU.Text = "Exit";
      ExitMNU.Click += ExitMNU_Click;
      // 
      // DockHost
      // 
      DockHost.Dock = DockStyle.Fill;
      DockHost.DockBackColor = Color.FromArgb(238, 238, 242);
      DockHost.Location = new Point(0, 60);
      DockHost.Name = "DockHost";
      DockHost.Padding = new Padding(6);
      DockHost.ShowAutoHideContentOnHover = false;
      DockHost.Size = new Size(1537, 856);
      DockHost.TabIndex = 3;
      DockHost.Theme = vS2015LightTheme1;
      DockHost.ActiveContentChanged += DockHost_ActiveContentChanged;
      // 
      // SdrGainLabel
      // 
      SdrGainLabel.BackColor = SystemColors.Control;
      SdrGainLabel.Location = new Point(224, 2);
      SdrGainLabel.MinimumSize = new Size(40, 0);
      SdrGainLabel.Name = "SdrGainLabel";
      SdrGainLabel.Size = new Size(40, 32);
      SdrGainLabel.TabIndex = 8;
      SdrGainLabel.Text = "0";
      SdrGainLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // SdrGainTrackbar
      // 
      SdrGainTrackbar.AutoSize = false;
      SdrGainTrackbar.Enabled = false;
      SdrGainTrackbar.LargeChange = 1;
      SdrGainTrackbar.Location = new Point(68, 4);
      SdrGainTrackbar.Name = "SdrGainTrackbar";
      SdrGainTrackbar.Size = new Size(150, 32);
      SdrGainTrackbar.TabIndex = 6;
      ToolTip.SetToolTip(SdrGainTrackbar, "SDR Gain");
      SdrGainTrackbar.ValueChanged += SdrGainTrackbar_ValueChanged;
      // 
      // ToolTip
      // 
      ToolTip.ShowAlways = true;
      // 
      // VolumeTrackbar
      // 
      VolumeTrackbar.AutoSize = false;
      VolumeTrackbar.LargeChange = 1;
      VolumeTrackbar.Location = new Point(343, 4);
      VolumeTrackbar.Maximum = 0;
      VolumeTrackbar.Minimum = -50;
      VolumeTrackbar.Name = "VolumeTrackbar";
      VolumeTrackbar.Size = new Size(150, 32);
      VolumeTrackbar.TabIndex = 11;
      VolumeTrackbar.TickFrequency = 10;
      ToolTip.SetToolTip(VolumeTrackbar, "Volume");
      VolumeTrackbar.Value = -25;
      VolumeTrackbar.ValueChanged += VolumeTrackbar_ValueChanged;
      // 
      // panel1
      // 
      panel1.BorderStyle = BorderStyle.FixedSingle;
      panel1.Controls.Add(NoiseBlankerBtn);
      panel1.Controls.Add(label2);
      panel1.Controls.Add(label1);
      panel1.Controls.Add(AddReceiverBtn);
      panel1.Controls.Add(SdrGainTrackbar);
      panel1.Controls.Add(SdrGainLabel);
      panel1.Controls.Add(VolumeTrackbar);
      panel1.Controls.Add(VolumeLabel);
      panel1.Dock = DockStyle.Top;
      panel1.Location = new Point(0, 24);
      panel1.Name = "panel1";
      panel1.Size = new Size(1537, 36);
      panel1.TabIndex = 13;
      ToolTip.SetToolTip(panel1, "Add Receiver");
      // 
      // NoiseBlankerBtn
      // 
      NoiseBlankerBtn.BackColor = Color.Transparent;
      NoiseBlankerBtn.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
      NoiseBlankerBtn.Location = new Point(715, 1);
      NoiseBlankerBtn.Name = "NoiseBlankerBtn";
      NoiseBlankerBtn.Size = new Size(109, 32);
      NoiseBlankerBtn.TabIndex = 16;
      NoiseBlankerBtn.Text = "Noise Blanker";
      ToolTip.SetToolTip(NoiseBlankerBtn, "Add Receiver");
      NoiseBlankerBtn.UseVisualStyleBackColor = false;
      NoiseBlankerBtn.Click += NoiseBlankerBtn_Click;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(290, 9);
      label2.Name = "label2";
      label2.Size = new Size(47, 15);
      label2.TabIndex = 15;
      label2.Text = "Volume";
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(11, 9);
      label1.Name = "label1";
      label1.Size = new Size(55, 15);
      label1.TabIndex = 14;
      label1.Text = "SDR Gain";
      // 
      // AddReceiverBtn
      // 
      AddReceiverBtn.BackColor = Color.Transparent;
      AddReceiverBtn.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
      AddReceiverBtn.Location = new Point(580, 1);
      AddReceiverBtn.Name = "AddReceiverBtn";
      AddReceiverBtn.Size = new Size(109, 32);
      AddReceiverBtn.TabIndex = 13;
      AddReceiverBtn.Text = "Add Receiver";
      ToolTip.SetToolTip(AddReceiverBtn, "Add Receiver");
      AddReceiverBtn.UseVisualStyleBackColor = false;
      AddReceiverBtn.Click += AddReceiverBtn_Click;
      // 
      // VolumeLabel
      // 
      VolumeLabel.BackColor = SystemColors.Control;
      VolumeLabel.Location = new Point(499, 2);
      VolumeLabel.MinimumSize = new Size(40, 0);
      VolumeLabel.Name = "VolumeLabel";
      VolumeLabel.Size = new Size(55, 32);
      VolumeLabel.TabIndex = 12;
      VolumeLabel.Text = "0";
      VolumeLabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // Timer
      // 
      Timer.Enabled = true;
      Timer.Interval = 1000;
      Timer.Tick += Timer_Tick;
      // 
      // NoiseFloorMenu
      // 
      NoiseFloorMenu.ImageScalingSize = new Size(24, 24);
      NoiseFloorMenu.Items.AddRange(new ToolStripItem[] { SetNfCalibrationMNU, ClearNfCalibrationMNU });
      NoiseFloorMenu.Name = "NoiseFloorMenu";
      NoiseFloorMenu.Size = new Size(175, 48);
      // 
      // MainForm
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(1537, 951);
      Controls.Add(DockHost);
      Controls.Add(panel1);
      Controls.Add(StatusStrip);
      Controls.Add(MainMenu);
      Icon = (Icon)resources.GetObject("$this.Icon");
      MainMenuStrip = MainMenu;
      Name = "MainForm";
      Text = "JT Skimmer";
      FormClosing += MainForm_FormClosing;
      Load += MainForm_Load;
      Resize += Form1_Resize;
      MainMenu.ResumeLayout(false);
      MainMenu.PerformLayout();
      StatusStrip.ResumeLayout(false);
      StatusStrip.PerformLayout();
      SystrayMenu.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)SdrGainTrackbar).EndInit();
      ((System.ComponentModel.ISupportInitialize)VolumeTrackbar).EndInit();
      panel1.ResumeLayout(false);
      panel1.PerformLayout();
      NoiseFloorMenu.ResumeLayout(false);
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private MenuStrip MainMenu;
    private StatusStrip StatusStrip;
    private ToolStrip toolStrip1;
    private ToolStripMenuItem fileToolStripMenuItem;
    private NotifyIcon SystrayIcon;
    private ContextMenuStrip SystrayMenu;
    private ToolStripMenuItem ShowWindowMNU;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem ExitMNU;
    private ToolStripMenuItem toolStripMenuItem1;
    private ToolStripMenuItem viewToolStripMenuItem;
    public WeifenLuo.WinFormsUI.Docking.DockPanel DockHost;
    private WeifenLuo.WinFormsUI.Docking.VS2015LightTheme vS2015LightTheme1;
    private ToolStripMenuItem toolsToolStripMenuItem;
    private ToolStripMenuItem SettingsMNU;
    private ToolStripMenuItem helpToolStripMenuItem;
    internal ToolStripMenuItem ViewReceiversMNU;
    internal ToolStripMenuItem ViewBandViewMNU;
    internal ToolStripMenuItem ViewMessagesMNU;
    private ToolStripMenuItem OnlineManualMNU;
    private ToolStripMenuItem UserDataFolderMNU;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripMenuItem AboutMNU;
    private ToolStripStatusLabel VacLedLabel;
    private ToolStripStatusLabel VacStatusLabel;
    private Label SdrGainLabel;
    public TrackBar SdrGainTrackbar;
    private ToolStripEx ToolStrip;
    private ToolTip ToolTip;
    private ToolStripMenuItem SdrDevicesMNU;
    private System.Windows.Forms.Timer Timer;
    private ToolStripMenuItem SetNfCalibrationMNU;
    private ToolStripMenuItem ClearNfCalibrationMNU;
    private ToolStripStatusLabel CpuLoadlabel;
    private Label VolumeLabel;
    public TrackBar VolumeTrackbar;
    private ToolStripStatusLabel SdrLedLabel;
    private ToolStripStatusLabel SdrStatusLabel;
    private ToolStripStatusLabel SoundcardLedLabel;
    private ToolStripStatusLabel SoundcardStatusLabel;
    private ToolStripStatusLabel OmniRigLedLabel;
    private ToolStripStatusLabel OmniRigStatusLabel;
    private ToolStripStatusLabel toolStripStatusLabel2;
    private ToolStripStatusLabel NoiseFloorLabel;
    private ContextMenuStrip NoiseFloorMenu;
    private ToolStripMenuItem ReferenceDataFolderMNU;
    private ToolStripStatusLabel NetworkLedLabel;
    private ToolStripStatusLabel NetworkStatusLabel;
    private Panel panel1;
    private Button sssssss;
    private Button button1;
    private Button AddReceiverBtn;
    private ToolStripStatusLabel IqOutputLedLabel;
    private ToolStripStatusLabel IqOutputStatusLabel;
    private Label label2;
    private Label label1;
    private ToolStripMenuItem WebsitelMNU;
    private Button NoiseBlankerBtn;
    private ToolStripSeparator toolStripMenuItem2;
    private ToolStripMenuItem EmailTheAuthorMNU;
  }
}