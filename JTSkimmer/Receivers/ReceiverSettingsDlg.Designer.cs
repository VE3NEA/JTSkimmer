namespace JTSkimmer
{
  partial class ReceiverSettingsDlg
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      components = new System.ComponentModel.Container();
      panel1 = new Panel();
      cancelBtn = new Button();
      okBtn = new Button();
      groupBox1 = new GroupBox();
      FrequencyUpDown = new NumericUpDown();
      label1 = new Label();
      ModeMenu = new ContextMenuStrip(components);
      NoDecodingMNU = new ToolStripMenuItem();
      Ft8MNU = new ToolStripMenuItem();
      Ft4MNU = new ToolStripMenuItem();
      Msk144MNU = new ToolStripMenuItem();
      Q65MNU = new ToolStripMenuItem();
      toolStripMenuItem1 = new ToolStripMenuItem();
      toolStripMenuItem6 = new ToolStripMenuItem();
      toolStripMenuItem7 = new ToolStripMenuItem();
      toolStripMenuItem8 = new ToolStripMenuItem();
      toolStripMenuItem2 = new ToolStripMenuItem();
      toolStripMenuItem11 = new ToolStripMenuItem();
      toolStripMenuItem12 = new ToolStripMenuItem();
      toolStripMenuItem13 = new ToolStripMenuItem();
      toolStripMenuItem14 = new ToolStripMenuItem();
      toolStripMenuItem3 = new ToolStripMenuItem();
      toolStripMenuItem15 = new ToolStripMenuItem();
      toolStripMenuItem17 = new ToolStripMenuItem();
      toolStripMenuItem18 = new ToolStripMenuItem();
      toolStripMenuItem19 = new ToolStripMenuItem();
      toolStripMenuItem20 = new ToolStripMenuItem();
      toolStripMenuItem4 = new ToolStripMenuItem();
      toolStripMenuItem21 = new ToolStripMenuItem();
      toolStripMenuItem22 = new ToolStripMenuItem();
      toolStripMenuItem23 = new ToolStripMenuItem();
      toolStripMenuItem24 = new ToolStripMenuItem();
      toolStripMenuItem25 = new ToolStripMenuItem();
      toolStripMenuItem5 = new ToolStripMenuItem();
      toolStripMenuItem26 = new ToolStripMenuItem();
      toolStripMenuItem27 = new ToolStripMenuItem();
      toolStripMenuItem28 = new ToolStripMenuItem();
      toolStripMenuItem29 = new ToolStripMenuItem();
      toolStripMenuItem30 = new ToolStripMenuItem();
      Jt65MNU = new ToolStripMenuItem();
      toolStripMenuItem9 = new ToolStripMenuItem();
      toolStripMenuItem10 = new ToolStripMenuItem();
      toolStripMenuItem16 = new ToolStripMenuItem();
      groupBox2 = new GroupBox();
      label3 = new Label();
      DecoderTypeComboBox = new ComboBox();
      DecoderSettingsBtn = new Button();
      label2 = new Label();
      ModeLabel = new Label();
      ModeBtn = new Button();
      ToolTip = new ToolTip(components);
      groupBox3 = new GroupBox();
      panel1.SuspendLayout();
      groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)FrequencyUpDown).BeginInit();
      ModeMenu.SuspendLayout();
      groupBox2.SuspendLayout();
      SuspendLayout();
      // 
      // panel1
      // 
      panel1.Controls.Add(cancelBtn);
      panel1.Controls.Add(okBtn);
      panel1.Dock = DockStyle.Bottom;
      panel1.Location = new Point(0, 150);
      panel1.Margin = new Padding(4, 3, 4, 3);
      panel1.Name = "panel1";
      panel1.Size = new Size(406, 35);
      panel1.TabIndex = 5;
      // 
      // cancelBtn
      // 
      cancelBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      cancelBtn.DialogResult = DialogResult.Cancel;
      cancelBtn.Location = new Point(309, 4);
      cancelBtn.Margin = new Padding(4, 3, 4, 3);
      cancelBtn.Name = "cancelBtn";
      cancelBtn.Size = new Size(88, 27);
      cancelBtn.TabIndex = 2;
      cancelBtn.Text = "Cancel";
      cancelBtn.UseVisualStyleBackColor = true;
      // 
      // okBtn
      // 
      okBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      okBtn.DialogResult = DialogResult.OK;
      okBtn.Location = new Point(213, 5);
      okBtn.Margin = new Padding(4, 3, 4, 3);
      okBtn.Name = "okBtn";
      okBtn.Size = new Size(88, 27);
      okBtn.TabIndex = 0;
      okBtn.Text = "OK";
      okBtn.UseVisualStyleBackColor = true;
      okBtn.Click += okBtn_Click;
      // 
      // groupBox1
      // 
      groupBox1.Controls.Add(FrequencyUpDown);
      groupBox1.Controls.Add(label1);
      groupBox1.Location = new Point(8, 3);
      groupBox1.Name = "groupBox1";
      groupBox1.Size = new Size(174, 57);
      groupBox1.TabIndex = 8;
      groupBox1.TabStop = false;
      groupBox1.Text = "Frequency";
      // 
      // FrequencyUpDown
      // 
      FrequencyUpDown.DecimalPlaces = 1;
      FrequencyUpDown.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
      FrequencyUpDown.Increment = new decimal(new int[] { 10, 0, 0, 0 });
      FrequencyUpDown.Location = new Point(7, 20);
      FrequencyUpDown.Margin = new Padding(4, 3, 4, 3);
      FrequencyUpDown.Maximum = new decimal(new int[] { 2000000, 0, 0, 0 });
      FrequencyUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
      FrequencyUpDown.Name = "FrequencyUpDown";
      FrequencyUpDown.Size = new Size(112, 26);
      FrequencyUpDown.TabIndex = 1;
      FrequencyUpDown.ThousandsSeparator = true;
      FrequencyUpDown.Value = new decimal(new int[] { 50180, 0, 0, 0 });
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
      label1.Location = new Point(127, 22);
      label1.Margin = new Padding(4, 0, 4, 0);
      label1.Name = "label1";
      label1.Size = new Size(37, 20);
      label1.TabIndex = 9;
      label1.Text = "kHz";
      // 
      // ModeMenu
      // 
      ModeMenu.Items.AddRange(new ToolStripItem[] { NoDecodingMNU, Ft8MNU, Ft4MNU, Msk144MNU, Q65MNU, Jt65MNU });
      ModeMenu.Name = "DecodingMenu";
      ModeMenu.Size = new Size(145, 136);
      // 
      // NoDecodingMNU
      // 
      NoDecodingMNU.Name = "NoDecodingMNU";
      NoDecodingMNU.Size = new Size(144, 22);
      NoDecodingMNU.Text = "No Decoding";
      NoDecodingMNU.Click += ModeMenu_Click;
      // 
      // Ft8MNU
      // 
      Ft8MNU.Name = "Ft8MNU";
      Ft8MNU.Size = new Size(144, 22);
      Ft8MNU.Text = "FT8";
      Ft8MNU.Click += ModeMenu_Click;
      // 
      // Ft4MNU
      // 
      Ft4MNU.Name = "Ft4MNU";
      Ft4MNU.Size = new Size(144, 22);
      Ft4MNU.Text = "FT4";
      Ft4MNU.Click += ModeMenu_Click;
      // 
      // Msk144MNU
      // 
      Msk144MNU.Name = "Msk144MNU";
      Msk144MNU.Size = new Size(144, 22);
      Msk144MNU.Text = "MSK144";
      Msk144MNU.Click += ModeMenu_Click;
      // 
      // Q65MNU
      // 
      Q65MNU.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem1, toolStripMenuItem2, toolStripMenuItem3, toolStripMenuItem4, toolStripMenuItem5 });
      Q65MNU.Name = "Q65MNU";
      Q65MNU.Size = new Size(144, 22);
      Q65MNU.Text = "Q65";
      // 
      // toolStripMenuItem1
      // 
      toolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem6, toolStripMenuItem7, toolStripMenuItem8 });
      toolStripMenuItem1.Name = "toolStripMenuItem1";
      toolStripMenuItem1.Size = new Size(92, 22);
      toolStripMenuItem1.Text = "15";
      // 
      // toolStripMenuItem6
      // 
      toolStripMenuItem6.Name = "toolStripMenuItem6";
      toolStripMenuItem6.Size = new Size(82, 22);
      toolStripMenuItem6.Text = "A";
      toolStripMenuItem6.Click += Q65SubMenu_Click;
      // 
      // toolStripMenuItem7
      // 
      toolStripMenuItem7.Name = "toolStripMenuItem7";
      toolStripMenuItem7.Size = new Size(82, 22);
      toolStripMenuItem7.Text = "B";
      toolStripMenuItem7.Click += Q65SubMenu_Click;
      // 
      // toolStripMenuItem8
      // 
      toolStripMenuItem8.Name = "toolStripMenuItem8";
      toolStripMenuItem8.Size = new Size(82, 22);
      toolStripMenuItem8.Text = "C";
      toolStripMenuItem8.Click += Q65SubMenu_Click;
      // 
      // toolStripMenuItem2
      // 
      toolStripMenuItem2.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem11, toolStripMenuItem12, toolStripMenuItem13, toolStripMenuItem14 });
      toolStripMenuItem2.Name = "toolStripMenuItem2";
      toolStripMenuItem2.Size = new Size(92, 22);
      toolStripMenuItem2.Text = "30";
      // 
      // toolStripMenuItem11
      // 
      toolStripMenuItem11.Name = "toolStripMenuItem11";
      toolStripMenuItem11.Size = new Size(82, 22);
      toolStripMenuItem11.Text = "A";
      toolStripMenuItem11.Click += Q65SubMenu_Click;
      // 
      // toolStripMenuItem12
      // 
      toolStripMenuItem12.Name = "toolStripMenuItem12";
      toolStripMenuItem12.Size = new Size(82, 22);
      toolStripMenuItem12.Text = "B";
      toolStripMenuItem12.Click += Q65SubMenu_Click;
      // 
      // toolStripMenuItem13
      // 
      toolStripMenuItem13.Name = "toolStripMenuItem13";
      toolStripMenuItem13.Size = new Size(82, 22);
      toolStripMenuItem13.Text = "C";
      toolStripMenuItem13.Click += Q65SubMenu_Click;
      // 
      // toolStripMenuItem14
      // 
      toolStripMenuItem14.Name = "toolStripMenuItem14";
      toolStripMenuItem14.Size = new Size(82, 22);
      toolStripMenuItem14.Text = "D";
      toolStripMenuItem14.Click += Q65SubMenu_Click;
      // 
      // toolStripMenuItem3
      // 
      toolStripMenuItem3.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem15, toolStripMenuItem17, toolStripMenuItem18, toolStripMenuItem19, toolStripMenuItem20 });
      toolStripMenuItem3.Name = "toolStripMenuItem3";
      toolStripMenuItem3.Size = new Size(92, 22);
      toolStripMenuItem3.Text = "60";
      // 
      // toolStripMenuItem15
      // 
      toolStripMenuItem15.Name = "toolStripMenuItem15";
      toolStripMenuItem15.Size = new Size(82, 22);
      toolStripMenuItem15.Text = "A";
      toolStripMenuItem15.Click += Q65SubMenu_Click;
      // 
      // toolStripMenuItem17
      // 
      toolStripMenuItem17.Name = "toolStripMenuItem17";
      toolStripMenuItem17.Size = new Size(82, 22);
      toolStripMenuItem17.Text = "B";
      toolStripMenuItem17.Click += Q65SubMenu_Click;
      // 
      // toolStripMenuItem18
      // 
      toolStripMenuItem18.Name = "toolStripMenuItem18";
      toolStripMenuItem18.Size = new Size(82, 22);
      toolStripMenuItem18.Text = "C";
      toolStripMenuItem18.Click += Q65SubMenu_Click;
      // 
      // toolStripMenuItem19
      // 
      toolStripMenuItem19.Name = "toolStripMenuItem19";
      toolStripMenuItem19.Size = new Size(82, 22);
      toolStripMenuItem19.Text = "D";
      toolStripMenuItem19.Click += Q65SubMenu_Click;
      // 
      // toolStripMenuItem20
      // 
      toolStripMenuItem20.Name = "toolStripMenuItem20";
      toolStripMenuItem20.Size = new Size(82, 22);
      toolStripMenuItem20.Text = "E";
      toolStripMenuItem20.Click += Q65SubMenu_Click;
      // 
      // toolStripMenuItem4
      // 
      toolStripMenuItem4.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem21, toolStripMenuItem22, toolStripMenuItem23, toolStripMenuItem24, toolStripMenuItem25 });
      toolStripMenuItem4.Name = "toolStripMenuItem4";
      toolStripMenuItem4.Size = new Size(92, 22);
      toolStripMenuItem4.Text = "120";
      // 
      // toolStripMenuItem21
      // 
      toolStripMenuItem21.Name = "toolStripMenuItem21";
      toolStripMenuItem21.Size = new Size(82, 22);
      toolStripMenuItem21.Text = "A";
      toolStripMenuItem21.Click += Q65SubMenu_Click;
      // 
      // toolStripMenuItem22
      // 
      toolStripMenuItem22.Name = "toolStripMenuItem22";
      toolStripMenuItem22.Size = new Size(82, 22);
      toolStripMenuItem22.Text = "B";
      toolStripMenuItem22.Click += Q65SubMenu_Click;
      // 
      // toolStripMenuItem23
      // 
      toolStripMenuItem23.Name = "toolStripMenuItem23";
      toolStripMenuItem23.Size = new Size(82, 22);
      toolStripMenuItem23.Text = "C";
      toolStripMenuItem23.Click += Q65SubMenu_Click;
      // 
      // toolStripMenuItem24
      // 
      toolStripMenuItem24.Name = "toolStripMenuItem24";
      toolStripMenuItem24.Size = new Size(82, 22);
      toolStripMenuItem24.Text = "D";
      toolStripMenuItem24.Click += Q65SubMenu_Click;
      // 
      // toolStripMenuItem25
      // 
      toolStripMenuItem25.Name = "toolStripMenuItem25";
      toolStripMenuItem25.Size = new Size(82, 22);
      toolStripMenuItem25.Text = "E";
      toolStripMenuItem25.Click += Q65SubMenu_Click;
      // 
      // toolStripMenuItem5
      // 
      toolStripMenuItem5.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem26, toolStripMenuItem27, toolStripMenuItem28, toolStripMenuItem29, toolStripMenuItem30 });
      toolStripMenuItem5.Name = "toolStripMenuItem5";
      toolStripMenuItem5.Size = new Size(92, 22);
      toolStripMenuItem5.Text = "300";
      // 
      // toolStripMenuItem26
      // 
      toolStripMenuItem26.Name = "toolStripMenuItem26";
      toolStripMenuItem26.Size = new Size(82, 22);
      toolStripMenuItem26.Text = "A";
      toolStripMenuItem26.Click += Q65SubMenu_Click;
      // 
      // toolStripMenuItem27
      // 
      toolStripMenuItem27.Name = "toolStripMenuItem27";
      toolStripMenuItem27.Size = new Size(82, 22);
      toolStripMenuItem27.Text = "B";
      toolStripMenuItem27.Click += Q65SubMenu_Click;
      // 
      // toolStripMenuItem28
      // 
      toolStripMenuItem28.Name = "toolStripMenuItem28";
      toolStripMenuItem28.Size = new Size(82, 22);
      toolStripMenuItem28.Text = "C";
      toolStripMenuItem28.Click += Q65SubMenu_Click;
      // 
      // toolStripMenuItem29
      // 
      toolStripMenuItem29.Name = "toolStripMenuItem29";
      toolStripMenuItem29.Size = new Size(82, 22);
      toolStripMenuItem29.Text = "D";
      toolStripMenuItem29.Click += Q65SubMenu_Click;
      // 
      // toolStripMenuItem30
      // 
      toolStripMenuItem30.Name = "toolStripMenuItem30";
      toolStripMenuItem30.Size = new Size(82, 22);
      toolStripMenuItem30.Text = "E";
      toolStripMenuItem30.Click += Q65SubMenu_Click;
      // 
      // Jt65MNU
      // 
      Jt65MNU.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem9, toolStripMenuItem10, toolStripMenuItem16 });
      Jt65MNU.Name = "Jt65MNU";
      Jt65MNU.Size = new Size(144, 22);
      Jt65MNU.Text = "JT65";
      // 
      // toolStripMenuItem9
      // 
      toolStripMenuItem9.Name = "toolStripMenuItem9";
      toolStripMenuItem9.Size = new Size(82, 22);
      toolStripMenuItem9.Text = "A";
      toolStripMenuItem9.Click += Jt65SubMenu_Click;
      // 
      // toolStripMenuItem10
      // 
      toolStripMenuItem10.Name = "toolStripMenuItem10";
      toolStripMenuItem10.Size = new Size(82, 22);
      toolStripMenuItem10.Text = "B";
      toolStripMenuItem10.Click += Jt65SubMenu_Click;
      // 
      // toolStripMenuItem16
      // 
      toolStripMenuItem16.Name = "toolStripMenuItem16";
      toolStripMenuItem16.Size = new Size(82, 22);
      toolStripMenuItem16.Text = "C";
      toolStripMenuItem16.Click += Jt65SubMenu_Click;
      // 
      // groupBox2
      // 
      groupBox2.Controls.Add(label3);
      groupBox2.Controls.Add(DecoderTypeComboBox);
      groupBox2.Controls.Add(DecoderSettingsBtn);
      groupBox2.Controls.Add(label2);
      groupBox2.Controls.Add(ModeLabel);
      groupBox2.Controls.Add(ModeBtn);
      groupBox2.Location = new Point(192, 3);
      groupBox2.Name = "groupBox2";
      groupBox2.Size = new Size(202, 140);
      groupBox2.TabIndex = 9;
      groupBox2.TabStop = false;
      groupBox2.Text = "Decoding";
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(10, 66);
      label3.Name = "label3";
      label3.Size = new Size(51, 15);
      label3.TabIndex = 17;
      label3.Text = "Decoder";
      // 
      // DecoderTypeComboBox
      // 
      DecoderTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      DecoderTypeComboBox.FormattingEnabled = true;
      DecoderTypeComboBox.Location = new Point(67, 63);
      DecoderTypeComboBox.Name = "DecoderTypeComboBox";
      DecoderTypeComboBox.Size = new Size(129, 23);
      DecoderTypeComboBox.TabIndex = 16;
      ToolTip.SetToolTip(DecoderTypeComboBox, "View thectual settings sent to decoder");
      // 
      // DecoderSettingsBtn
      // 
      DecoderSettingsBtn.Location = new Point(37, 100);
      DecoderSettingsBtn.Name = "DecoderSettingsBtn";
      DecoderSettingsBtn.Size = new Size(128, 28);
      DecoderSettingsBtn.TabIndex = 15;
      DecoderSettingsBtn.Text = "Advanced Settings";
      DecoderSettingsBtn.UseVisualStyleBackColor = true;
      DecoderSettingsBtn.Click += DecoderSettingsBtn_Click;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(10, 26);
      label2.Name = "label2";
      label2.Size = new Size(38, 15);
      label2.TabIndex = 13;
      label2.Text = "Mode";
      // 
      // ModeLabel
      // 
      ModeLabel.BackColor = SystemColors.Window;
      ModeLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
      ModeLabel.Location = new Point(54, 20);
      ModeLabel.Name = "ModeLabel";
      ModeLabel.Size = new Size(111, 25);
      ModeLabel.TabIndex = 12;
      ModeLabel.Text = "Q65-300E";
      ModeLabel.TextAlign = ContentAlignment.MiddleLeft;
      ModeLabel.Click += ModeBtn_Click;
      // 
      // ModeBtn
      // 
      ModeBtn.Font = new Font("Webdings", 12F, FontStyle.Regular, GraphicsUnit.Point);
      ModeBtn.Location = new Point(167, 18);
      ModeBtn.Name = "ModeBtn";
      ModeBtn.Size = new Size(29, 29);
      ModeBtn.TabIndex = 11;
      ModeBtn.Text = "6";
      ModeBtn.UseVisualStyleBackColor = true;
      ModeBtn.Click += ModeBtn_Click;
      // 
      // groupBox3
      // 
      groupBox3.Location = new Point(9, 69);
      groupBox3.Name = "groupBox3";
      groupBox3.Size = new Size(173, 70);
      groupBox3.TabIndex = 10;
      groupBox3.TabStop = false;
      groupBox3.Text = "Denoising";
      groupBox3.Visible = false;
      // 
      // ReceiverSettingsDlg
      // 
      AcceptButton = okBtn;
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      CancelButton = cancelBtn;
      ClientSize = new Size(406, 185);
      Controls.Add(groupBox3);
      Controls.Add(groupBox2);
      Controls.Add(groupBox1);
      Controls.Add(panel1);
      FormBorderStyle = FormBorderStyle.FixedToolWindow;
      Name = "ReceiverSettingsDlg";
      ShowInTaskbar = false;
      StartPosition = FormStartPosition.Manual;
      Text = "Receiver Settings";
      Shown += ReceiverSettingsDlg_Shown;
      panel1.ResumeLayout(false);
      groupBox1.ResumeLayout(false);
      groupBox1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)FrequencyUpDown).EndInit();
      ModeMenu.ResumeLayout(false);
      groupBox2.ResumeLayout(false);
      groupBox2.PerformLayout();
      ResumeLayout(false);
    }

    #endregion

    private Panel panel1;
    private Button cancelBtn;
    private Button okBtn;
    private GroupBox groupBox1;
    private Label label1;
    internal NumericUpDown FrequencyUpDown;
    private ContextMenuStrip ModeMenu;
    private ToolStripMenuItem NoDecodingMNU;
    private ToolStripMenuItem Ft8MNU;
    private ToolStripMenuItem Ft4MNU;
    private ToolStripMenuItem Msk144MNU;
    private ToolStripMenuItem Q65MNU;
    private ToolStripMenuItem toolStripMenuItem1;
    private ToolStripMenuItem toolStripMenuItem6;
    private ToolStripMenuItem toolStripMenuItem7;
    private ToolStripMenuItem toolStripMenuItem8;
    private ToolStripMenuItem toolStripMenuItem2;
    private ToolStripMenuItem toolStripMenuItem11;
    private ToolStripMenuItem toolStripMenuItem12;
    private ToolStripMenuItem toolStripMenuItem13;
    private ToolStripMenuItem toolStripMenuItem14;
    private ToolStripMenuItem toolStripMenuItem3;
    private ToolStripMenuItem toolStripMenuItem15;
    private ToolStripMenuItem toolStripMenuItem17;
    private ToolStripMenuItem toolStripMenuItem18;
    private ToolStripMenuItem toolStripMenuItem19;
    private ToolStripMenuItem toolStripMenuItem20;
    private ToolStripMenuItem toolStripMenuItem4;
    private ToolStripMenuItem toolStripMenuItem21;
    private ToolStripMenuItem toolStripMenuItem22;
    private ToolStripMenuItem toolStripMenuItem23;
    private ToolStripMenuItem toolStripMenuItem24;
    private ToolStripMenuItem toolStripMenuItem25;
    private ToolStripMenuItem toolStripMenuItem5;
    private ToolStripMenuItem toolStripMenuItem26;
    private ToolStripMenuItem toolStripMenuItem27;
    private ToolStripMenuItem toolStripMenuItem28;
    private ToolStripMenuItem toolStripMenuItem29;
    private ToolStripMenuItem toolStripMenuItem30;
    private ToolStripMenuItem Jt65MNU;
    private ToolStripMenuItem toolStripMenuItem9;
    private ToolStripMenuItem toolStripMenuItem10;
    private ToolStripMenuItem toolStripMenuItem16;
    private GroupBox groupBox2;
    private Label label2;
    private Label ModeLabel;
    private Button ModeBtn;
    private Button DecoderSettingsBtn;
    private Label label3;
    private ComboBox DecoderTypeComboBox;
    private ToolTip ToolTip;
    private GroupBox groupBox3;
  }
}