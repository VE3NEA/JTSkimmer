namespace JTSkimmer
{
  partial class FreqCalibrationDialog
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
      panel1 = new Panel();
      cancelBtn = new Button();
      okBtn = new Button();
      RxFrequencyUpDown = new NumericUpDown();
      label1 = new Label();
      label2 = new Label();
      label3 = new Label();
      DecoderFreqUpDown = new NumericUpDown();
      label4 = new Label();
      label5 = new Label();
      TrueFreqUpDown = new NumericUpDown();
      label6 = new Label();
      label7 = new Label();
      CurrentPpmLabel = new Label();
      NewPpmLabel = new Label();
      label10 = new Label();
      panel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)RxFrequencyUpDown).BeginInit();
      ((System.ComponentModel.ISupportInitialize)DecoderFreqUpDown).BeginInit();
      ((System.ComponentModel.ISupportInitialize)TrueFreqUpDown).BeginInit();
      SuspendLayout();
      // 
      // panel1
      // 
      panel1.Controls.Add(cancelBtn);
      panel1.Controls.Add(okBtn);
      panel1.Dock = DockStyle.Bottom;
      panel1.Location = new Point(0, 237);
      panel1.Margin = new Padding(4, 3, 4, 3);
      panel1.Name = "panel1";
      panel1.Size = new Size(198, 35);
      panel1.TabIndex = 6;
      // 
      // cancelBtn
      // 
      cancelBtn.DialogResult = DialogResult.Cancel;
      cancelBtn.Location = new Point(100, 5);
      cancelBtn.Margin = new Padding(4, 3, 4, 3);
      cancelBtn.Name = "cancelBtn";
      cancelBtn.Size = new Size(88, 27);
      cancelBtn.TabIndex = 2;
      cancelBtn.Text = "Cancel";
      cancelBtn.UseVisualStyleBackColor = true;
      // 
      // okBtn
      // 
      okBtn.DialogResult = DialogResult.OK;
      okBtn.Location = new Point(4, 5);
      okBtn.Margin = new Padding(4, 3, 4, 3);
      okBtn.Name = "okBtn";
      okBtn.Size = new Size(88, 27);
      okBtn.TabIndex = 0;
      okBtn.Text = "Apply";
      okBtn.UseVisualStyleBackColor = true;
      // 
      // RxFrequencyUpDown
      // 
      RxFrequencyUpDown.DecimalPlaces = 1;
      RxFrequencyUpDown.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
      RxFrequencyUpDown.Location = new Point(13, 29);
      RxFrequencyUpDown.Margin = new Padding(4, 3, 4, 3);
      RxFrequencyUpDown.Maximum = new decimal(new int[] { 2000000, 0, 0, 0 });
      RxFrequencyUpDown.Minimum = new decimal(new int[] { 24000, 0, 0, 0 });
      RxFrequencyUpDown.Name = "RxFrequencyUpDown";
      RxFrequencyUpDown.Size = new Size(112, 26);
      RxFrequencyUpDown.TabIndex = 10;
      RxFrequencyUpDown.ThousandsSeparator = true;
      RxFrequencyUpDown.Value = new decimal(new int[] { 50313, 0, 0, 0 });
      RxFrequencyUpDown.ValueChanged += UpDown_ValueChanged;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
      label1.Location = new Point(133, 31);
      label1.Margin = new Padding(4, 0, 4, 0);
      label1.Name = "label1";
      label1.Size = new Size(37, 20);
      label1.TabIndex = 11;
      label1.Text = "kHz";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(13, 11);
      label2.Name = "label2";
      label2.Size = new Size(109, 15);
      label2.TabIndex = 12;
      label2.Text = "Receiver Frequency";
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(12, 69);
      label3.Name = "label3";
      label3.Size = new Size(144, 15);
      label3.TabIndex = 15;
      label3.Text = "Decoder Audio Frequency";
      // 
      // DecoderFreqUpDown
      // 
      DecoderFreqUpDown.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
      DecoderFreqUpDown.Location = new Point(13, 87);
      DecoderFreqUpDown.Margin = new Padding(4, 3, 4, 3);
      DecoderFreqUpDown.Maximum = new decimal(new int[] { 6000, 0, 0, 0 });
      DecoderFreqUpDown.Name = "DecoderFreqUpDown";
      DecoderFreqUpDown.Size = new Size(112, 26);
      DecoderFreqUpDown.TabIndex = 13;
      DecoderFreqUpDown.ThousandsSeparator = true;
      DecoderFreqUpDown.Value = new decimal(new int[] { 1500, 0, 0, 0 });
      DecoderFreqUpDown.ValueChanged += UpDown_ValueChanged;
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
      label4.Location = new Point(133, 89);
      label4.Margin = new Padding(4, 0, 4, 0);
      label4.Name = "label4";
      label4.Size = new Size(29, 20);
      label4.TabIndex = 14;
      label4.Text = "Hz";
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new Point(13, 127);
      label5.Name = "label5";
      label5.Size = new Size(122, 15);
      label5.TabIndex = 18;
      label5.Text = "True Audio Frequency";
      // 
      // TrueFreqUpDown
      // 
      TrueFreqUpDown.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
      TrueFreqUpDown.Location = new Point(13, 145);
      TrueFreqUpDown.Margin = new Padding(4, 3, 4, 3);
      TrueFreqUpDown.Maximum = new decimal(new int[] { 6000, 0, 0, 0 });
      TrueFreqUpDown.Name = "TrueFreqUpDown";
      TrueFreqUpDown.Size = new Size(112, 26);
      TrueFreqUpDown.TabIndex = 16;
      TrueFreqUpDown.ThousandsSeparator = true;
      TrueFreqUpDown.Value = new decimal(new int[] { 1500, 0, 0, 0 });
      TrueFreqUpDown.ValueChanged += UpDown_ValueChanged;
      // 
      // label6
      // 
      label6.AutoSize = true;
      label6.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point);
      label6.Location = new Point(133, 147);
      label6.Margin = new Padding(4, 0, 4, 0);
      label6.Name = "label6";
      label6.Size = new Size(29, 20);
      label6.TabIndex = 17;
      label6.Text = "Hz";
      // 
      // label7
      // 
      label7.AutoSize = true;
      label7.Location = new Point(13, 186);
      label7.Name = "label7";
      label7.Size = new Size(75, 15);
      label7.TabIndex = 19;
      label7.Text = "Current PPM";
      // 
      // CurrentPpmLabel
      // 
      CurrentPpmLabel.AutoSize = true;
      CurrentPpmLabel.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
      CurrentPpmLabel.Location = new Point(13, 203);
      CurrentPpmLabel.Name = "CurrentPpmLabel";
      CurrentPpmLabel.Size = new Size(52, 20);
      CurrentPpmLabel.TabIndex = 20;
      CurrentPpmLabel.Text = "0.0000";
      // 
      // NewPpmLabel
      // 
      NewPpmLabel.AutoSize = true;
      NewPpmLabel.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
      NewPpmLabel.Location = new Point(100, 203);
      NewPpmLabel.Name = "NewPpmLabel";
      NewPpmLabel.Size = new Size(52, 20);
      NewPpmLabel.TabIndex = 22;
      NewPpmLabel.Text = "0.0000";
      // 
      // label10
      // 
      label10.AutoSize = true;
      label10.Location = new Point(100, 186);
      label10.Name = "label10";
      label10.Size = new Size(59, 15);
      label10.TabIndex = 21;
      label10.Text = "New PPM";
      // 
      // FreqCalibrationDialog
      // 
      AcceptButton = okBtn;
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      CancelButton = cancelBtn;
      ClientSize = new Size(198, 272);
      Controls.Add(NewPpmLabel);
      Controls.Add(label10);
      Controls.Add(CurrentPpmLabel);
      Controls.Add(label7);
      Controls.Add(label5);
      Controls.Add(TrueFreqUpDown);
      Controls.Add(label6);
      Controls.Add(label3);
      Controls.Add(DecoderFreqUpDown);
      Controls.Add(label4);
      Controls.Add(label2);
      Controls.Add(RxFrequencyUpDown);
      Controls.Add(label1);
      Controls.Add(panel1);
      FormBorderStyle = FormBorderStyle.FixedToolWindow;
      Name = "FreqCalibrationDialog";
      ShowInTaskbar = false;
      StartPosition = FormStartPosition.CenterParent;
      Text = "Frequency Calibration";
      panel1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)RxFrequencyUpDown).EndInit();
      ((System.ComponentModel.ISupportInitialize)DecoderFreqUpDown).EndInit();
      ((System.ComponentModel.ISupportInitialize)TrueFreqUpDown).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Panel panel1;
    private Button cancelBtn;
    private Button okBtn;
    internal NumericUpDown RxFrequencyUpDown;
    private Label label1;
    private Label label2;
    private Label label3;
    internal NumericUpDown DecoderFreqUpDown;
    private Label label4;
    private Label label5;
    internal NumericUpDown TrueFreqUpDown;
    private Label label6;
    private Label label7;
    private Label CurrentPpmLabel;
    private Label NewPpmLabel;
    private Label label10;
  }
}