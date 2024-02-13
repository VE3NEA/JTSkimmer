namespace JTSkimmer
{
  partial class ReceiverPanel
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReceiverPanel));
      imageList1 = new ImageList(components);
      ToolTip = new ToolTip(components);
      CloseLabel = new Label();
      panel1 = new Panel();
      ToolStrip = new ToolStripEx();
      SettingsBtn = new ToolStripButton();
      SpeakerBtn = new ToolStripButton();
      VacBtn = new ToolStripButton();
      TuneBtn = new ToolStripButton();
      panel9 = new Panel();
      Indexlabel = new Label();
      FrequencyLabel = new Label();
      ModeLabel = new Label();
      SlidersGlyph = new PictureBox();
      panel10 = new Panel();
      WaterfallControl = new WaterfallControl();
      panel1.SuspendLayout();
      ToolStrip.SuspendLayout();
      panel9.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)SlidersGlyph).BeginInit();
      panel10.SuspendLayout();
      SuspendLayout();
      // 
      // imageList1
      // 
      imageList1.ColorDepth = ColorDepth.Depth8Bit;
      imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
      imageList1.TransparentColor = Color.Fuchsia;
      imageList1.Images.SetKeyName(0, "OmniRig24x24.png");
      imageList1.Images.SetKeyName(1, "Vac24x24a.png");
      imageList1.Images.SetKeyName(2, "Oxygen-Icons.org-Oxygen-Status-audio-volume-high.24.png");
      imageList1.Images.SetKeyName(3, "Gears24x24.png");
      imageList1.Images.SetKeyName(4, "equalizer.png");
      // 
      // CloseLabel
      // 
      CloseLabel.Cursor = Cursors.Hand;
      CloseLabel.Font = new Font("Webdings", 12F, FontStyle.Regular, GraphicsUnit.Point);
      CloseLabel.ForeColor = Color.Maroon;
      CloseLabel.Location = new Point(-1, 3);
      CloseLabel.Name = "CloseLabel";
      CloseLabel.Size = new Size(18, 17);
      CloseLabel.TabIndex = 9;
      CloseLabel.Text = "r";
      ToolTip.SetToolTip(CloseLabel, "Close Receiver");
      CloseLabel.Click += CloseBtn_Click;
      // 
      // panel1
      // 
      panel1.BackColor = SystemColors.ControlLightLight;
      panel1.Controls.Add(ToolStrip);
      panel1.Controls.Add(panel9);
      panel1.Dock = DockStyle.Top;
      panel1.Location = new Point(0, 0);
      panel1.Name = "panel1";
      panel1.Size = new Size(188, 34);
      panel1.TabIndex = 16;
      // 
      // ToolStrip
      // 
      ToolStrip.BackColor = SystemColors.ControlLightLight;
      ToolStrip.Dock = DockStyle.Fill;
      ToolStrip.Items.AddRange(new ToolStripItem[] { SettingsBtn, SpeakerBtn, VacBtn, TuneBtn });
      ToolStrip.Location = new Point(0, 0);
      ToolStrip.Name = "ToolStrip";
      ToolStrip.Size = new Size(167, 34);
      ToolStrip.TabIndex = 16;
      ToolStrip.Text = "toolStripEx1";
      ToolStrip.MouseDown += ToolStrip_MouseDown;
      ToolStrip.MouseEnter += ReceiverPanel_MouseEnter;
      ToolStrip.MouseLeave += ReceiverPanel_MouseLeave;
      // 
      // SettingsBtn
      // 
      SettingsBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
      SettingsBtn.Image = Properties.Resources.gear_1_;
      SettingsBtn.ImageScaling = ToolStripItemImageScaling.None;
      SettingsBtn.ImageTransparentColor = Color.Magenta;
      SettingsBtn.Margin = new Padding(3);
      SettingsBtn.Name = "SettingsBtn";
      SettingsBtn.Size = new Size(28, 28);
      SettingsBtn.Text = "toolStripButton1";
      SettingsBtn.ToolTipText = "Settings...";
      SettingsBtn.Click += SettingsBtn_Click;
      // 
      // SpeakerBtn
      // 
      SpeakerBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
      SpeakerBtn.Image = (Image)resources.GetObject("SpeakerBtn.Image");
      SpeakerBtn.ImageScaling = ToolStripItemImageScaling.None;
      SpeakerBtn.ImageTransparentColor = Color.Magenta;
      SpeakerBtn.Margin = new Padding(3);
      SpeakerBtn.Name = "SpeakerBtn";
      SpeakerBtn.Size = new Size(28, 28);
      SpeakerBtn.Text = "toolStripButton2";
      SpeakerBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
      SpeakerBtn.ToolTipText = "Audio to Speaker";
      SpeakerBtn.Click += SpeakerBtn_Click;
      // 
      // VacBtn
      // 
      VacBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
      VacBtn.Image = Properties.Resources.usb_cable;
      VacBtn.ImageScaling = ToolStripItemImageScaling.None;
      VacBtn.ImageTransparentColor = Color.Magenta;
      VacBtn.Margin = new Padding(3);
      VacBtn.Name = "VacBtn";
      VacBtn.Size = new Size(28, 28);
      VacBtn.Text = "toolStripButton3";
      VacBtn.ToolTipText = "Audio to VAC";
      VacBtn.Click += VacBtn_Click;
      // 
      // TuneBtn
      // 
      TuneBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
      TuneBtn.Image = Properties.Resources.OmniRig24x24;
      TuneBtn.ImageScaling = ToolStripItemImageScaling.None;
      TuneBtn.ImageTransparentColor = Color.Magenta;
      TuneBtn.Margin = new Padding(3);
      TuneBtn.Name = "TuneBtn";
      TuneBtn.Size = new Size(28, 28);
      TuneBtn.Text = "Tune Transceiver to This Frequency";
      TuneBtn.Click += TuneBtn_Click;
      // 
      // panel9
      // 
      panel9.BackColor = SystemColors.ControlLightLight;
      panel9.Controls.Add(CloseLabel);
      panel9.Controls.Add(Indexlabel);
      panel9.Dock = DockStyle.Right;
      panel9.Location = new Point(167, 0);
      panel9.Name = "panel9";
      panel9.Size = new Size(21, 34);
      panel9.TabIndex = 0;
      // 
      // Indexlabel
      // 
      Indexlabel.BackColor = Color.Transparent;
      Indexlabel.Dock = DockStyle.Bottom;
      Indexlabel.Location = new Point(0, 21);
      Indexlabel.Name = "Indexlabel";
      Indexlabel.Size = new Size(21, 13);
      Indexlabel.TabIndex = 10;
      Indexlabel.Text = "00";
      Indexlabel.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // FrequencyLabel
      // 
      FrequencyLabel.AutoEllipsis = true;
      FrequencyLabel.Dock = DockStyle.Top;
      FrequencyLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
      FrequencyLabel.ForeColor = Color.Green;
      FrequencyLabel.Location = new Point(0, 34);
      FrequencyLabel.Name = "FrequencyLabel";
      FrequencyLabel.Size = new Size(188, 19);
      FrequencyLabel.TabIndex = 17;
      FrequencyLabel.TextAlign = ContentAlignment.MiddleCenter;
      FrequencyLabel.MouseDown += FineTune_MouseDown;
      FrequencyLabel.MouseEnter += ReceiverPanel_MouseEnter;
      FrequencyLabel.MouseLeave += ReceiverPanel_MouseLeave;
      // 
      // ModeLabel
      // 
      ModeLabel.AutoEllipsis = true;
      ModeLabel.Dock = DockStyle.Top;
      ModeLabel.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
      ModeLabel.Location = new Point(0, 53);
      ModeLabel.Name = "ModeLabel";
      ModeLabel.Size = new Size(188, 19);
      ModeLabel.TabIndex = 18;
      ModeLabel.TextAlign = ContentAlignment.MiddleCenter;
      ModeLabel.MouseDown += FineTune_MouseDown;
      ModeLabel.MouseEnter += ReceiverPanel_MouseEnter;
      ModeLabel.MouseLeave += ReceiverPanel_MouseLeave;
      // 
      // SlidersGlyph
      // 
      SlidersGlyph.Cursor = Cursors.Hand;
      SlidersGlyph.Image = Properties.Resources.equalizer16x16;
      SlidersGlyph.InitialImage = null;
      SlidersGlyph.Location = new Point(4, 50);
      SlidersGlyph.Name = "SlidersGlyph";
      SlidersGlyph.Size = new Size(20, 20);
      SlidersGlyph.TabIndex = 19;
      SlidersGlyph.TabStop = false;
      SlidersGlyph.MouseDown += SlidersGlyph_MouseDown;
      // 
      // panel10
      // 
      panel10.BackColor = Color.Gainsboro;
      panel10.Controls.Add(WaterfallControl);
      panel10.Dock = DockStyle.Fill;
      panel10.Location = new Point(0, 72);
      panel10.Name = "panel10";
      panel10.Size = new Size(188, 192);
      panel10.TabIndex = 20;
      // 
      // WaterfallControl
      // 
      WaterfallControl.AllowDrop = true;
      WaterfallControl.Dock = DockStyle.Fill;
      WaterfallControl.Location = new Point(0, 0);
      WaterfallControl.Name = "WaterfallControl";
      WaterfallControl.ScrollSpeed = 0;
      WaterfallControl.Size = new Size(188, 192);
      WaterfallControl.TabIndex = 2;
      WaterfallControl.Visible = false;
      WaterfallControl.DragDrop += ReceiverPanel_DragDrop;
      WaterfallControl.DragEnter += ReceiverPanel_DragEnter;
      // 
      // ReceiverPanel
      // 
      AllowDrop = true;
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      BorderStyle = BorderStyle.FixedSingle;
      Controls.Add(panel10);
      Controls.Add(SlidersGlyph);
      Controls.Add(ModeLabel);
      Controls.Add(FrequencyLabel);
      Controls.Add(panel1);
      Margin = new Padding(1);
      Name = "ReceiverPanel";
      Size = new Size(188, 264);
      DragDrop += ReceiverPanel_DragDrop;
      DragEnter += ReceiverPanel_DragEnter;
      panel1.ResumeLayout(false);
      panel1.PerformLayout();
      ToolStrip.ResumeLayout(false);
      ToolStrip.PerformLayout();
      panel9.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)SlidersGlyph).EndInit();
      panel10.ResumeLayout(false);
      ResumeLayout(false);
    }

    #endregion
    private Panel panel2;
    private Button button4;
    private Button button3;
    private Button button1;
    private Label label1;
    private Panel panel3;
    private Panel panel5;
    private Button FrequencyBtn;
    private Panel panel6;
    private Panel panel7;
    private Button DecodingBtn;
    private Button button7;
    private Panel panel4;
    private Label AudioLabel;
    private Panel panel8;
    private Button button8;
    private Button button9;
    private Button AudioBtn;
    private ImageList imageList1;
    private ToolTip ToolTip;
    private Button button2;
    private Button button5;
    private Panel panel1;
    private ToolStripEx ToolStrip;
    private ToolStripButton SettingsBtn;
    private ToolStripButton SpeakerBtn;
    private ToolStripButton VacBtn;
    private ToolStripButton TuneBtn;
    private Panel panel9;
    private Label CloseLabel;
    private Label FrequencyLabel;
    private Label ModeLabel;
    private PictureBox SlidersGlyph;
    private Label Indexlabel;
    private Panel panel10;
    public WaterfallControl WaterfallControl;
  }
}
