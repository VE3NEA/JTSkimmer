namespace JTSkimmer
{
  partial class AboutBox
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
      CloseBtn = new Button();
      label1 = new Label();
      label2 = new Label();
      WebsiteLabel = new LinkLabel();
      SuspendLayout();
      // 
      // CloseBtn
      // 
      CloseBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      CloseBtn.Location = new Point(139, 112);
      CloseBtn.Name = "CloseBtn";
      CloseBtn.Size = new Size(75, 23);
      CloseBtn.TabIndex = 0;
      CloseBtn.Text = "Close";
      CloseBtn.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point);
      label1.ForeColor = Color.Blue;
      label1.Location = new Point(28, 9);
      label1.Name = "label1";
      label1.Size = new Size(293, 37);
      label1.TabIndex = 1;
      label1.Text = "JT Skimmer 0.92 Beta";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(57, 55);
      label2.Name = "label2";
      label2.Size = new Size(244, 15);
      label2.TabIndex = 2;
      label2.Text = "Copyright (c) 2024 Alex Shovkoplyas VE3NEA";
      // 
      // WebsiteLabel
      // 
      WebsiteLabel.AutoSize = true;
      WebsiteLabel.Location = new Point(154, 79);
      WebsiteLabel.Name = "WebsiteLabel";
      WebsiteLabel.Size = new Size(50, 15);
      WebsiteLabel.TabIndex = 3;
      WebsiteLabel.TabStop = true;
      WebsiteLabel.Text = "web site";
      WebsiteLabel.LinkClicked += WebsiteLabel_LinkClicked;
      // 
      // AboutBox
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      CancelButton = CloseBtn;
      ClientSize = new Size(378, 147);
      Controls.Add(WebsiteLabel);
      Controls.Add(label2);
      Controls.Add(label1);
      Controls.Add(CloseBtn);
      FormBorderStyle = FormBorderStyle.Fixed3D;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "AboutBox";
      ShowInTaskbar = false;
      StartPosition = FormStartPosition.CenterParent;
      Text = "About JT Skimmer";
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Button CloseBtn;
    private Label label1;
    private Label label2;
    private LinkLabel linkLabel1;
    private LinkLabel WebsiteLabel;
  }
}