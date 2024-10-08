﻿namespace JTSkimmer
{
  partial class NoiseBlankerDialog
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
      SnapshotBtn = new Button();
      label2 = new Label();
      label1 = new Label();
      trackBar1 = new TrackBar();
      radioButton2 = new RadioButton();
      radioButton1 = new RadioButton();
      timer1 = new System.Windows.Forms.Timer(components);
      panel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
      SuspendLayout();
      // 
      // panel1
      // 
      panel1.Controls.Add(SnapshotBtn);
      panel1.Controls.Add(label2);
      panel1.Controls.Add(label1);
      panel1.Controls.Add(trackBar1);
      panel1.Controls.Add(radioButton2);
      panel1.Controls.Add(radioButton1);
      panel1.Dock = DockStyle.Fill;
      panel1.Location = new Point(0, 0);
      panel1.Name = "panel1";
      panel1.Size = new Size(242, 119);
      panel1.TabIndex = 0;
      // 
      // SnapshotBtn
      // 
      SnapshotBtn.Location = new Point(148, 8);
      SnapshotBtn.Name = "SnapshotBtn";
      SnapshotBtn.Size = new Size(75, 23);
      SnapshotBtn.TabIndex = 7;
      SnapshotBtn.Text = "Snapshot";
      SnapshotBtn.UseVisualStyleBackColor = true;
      SnapshotBtn.Visible = false;
      SnapshotBtn.Click += SnapshotBtn_Click;
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(148, 89);
      label2.Name = "label2";
      label2.Size = new Size(38, 15);
      label2.TabIndex = 6;
      label2.Text = "label2";
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(14, 89);
      label1.Name = "label1";
      label1.Size = new Size(38, 15);
      label1.TabIndex = 5;
      label1.Text = "label1";
      // 
      // trackBar1
      // 
      trackBar1.LargeChange = 1;
      trackBar1.Location = new Point(14, 41);
      trackBar1.Maximum = 100;
      trackBar1.Name = "trackBar1";
      trackBar1.Size = new Size(209, 45);
      trackBar1.TabIndex = 3;
      trackBar1.TickFrequency = 10;
      trackBar1.ValueChanged += trackBar1_ValueChanged;
      // 
      // radioButton2
      // 
      radioButton2.AutoSize = true;
      radioButton2.Location = new Point(71, 12);
      radioButton2.Name = "radioButton2";
      radioButton2.Size = new Size(54, 19);
      radioButton2.TabIndex = 1;
      radioButton2.Text = "NR0V";
      radioButton2.UseVisualStyleBackColor = true;
      radioButton2.CheckedChanged += radioButton_CheckedChanged;
      // 
      // radioButton1
      // 
      radioButton1.AutoSize = true;
      radioButton1.Checked = true;
      radioButton1.Location = new Point(23, 12);
      radioButton1.Name = "radioButton1";
      radioButton1.Size = new Size(42, 19);
      radioButton1.TabIndex = 0;
      radioButton1.TabStop = true;
      radioButton1.Text = "Off";
      radioButton1.UseVisualStyleBackColor = true;
      radioButton1.CheckedChanged += radioButton_CheckedChanged;
      // 
      // timer1
      // 
      timer1.Enabled = true;
      timer1.Interval = 300;
      timer1.Tick += timer1_Tick;
      // 
      // NoiseBlankerDialog
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(242, 119);
      Controls.Add(panel1);
      FormBorderStyle = FormBorderStyle.FixedToolWindow;
      KeyPreview = true;
      Name = "NoiseBlankerDialog";
      ShowInTaskbar = false;
      StartPosition = FormStartPosition.CenterParent;
      Text = "Noise Blanker";
      KeyDown += NoiseBlankerDialog_KeyDown;
      panel1.ResumeLayout(false);
      panel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
      ResumeLayout(false);
    }

    #endregion

    private Panel panel1;
    private TrackBar trackBar1;
    private RadioButton radioButton2;
    private RadioButton radioButton1;
    private Label label2;
    private Label label1;
    private System.Windows.Forms.Timer timer1;
    private Button SnapshotBtn;
  }
}