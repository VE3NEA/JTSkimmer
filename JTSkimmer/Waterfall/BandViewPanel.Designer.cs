namespace JTSkimmer
{
  partial class BandViewPanel
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
      WaterfallControl = new WaterfallControl();
      contextMenuStrip1 = new ContextMenuStrip(components);
      ScalePanel = new ScalePanel();
      button1 = new Button();
      SuspendLayout();
      // 
      // WaterfallControl
      // 
      WaterfallControl.Dock = DockStyle.Fill;
      WaterfallControl.Location = new Point(0, 32);
      WaterfallControl.Name = "WaterfallControl";
      WaterfallControl.ScrollSpeed = 0;
      WaterfallControl.Size = new Size(950, 234);
      WaterfallControl.TabIndex = 1;
      // 
      // contextMenuStrip1
      // 
      contextMenuStrip1.Name = "contextMenuStrip1";
      contextMenuStrip1.Size = new Size(61, 4);
      // 
      // ScalePanel
      // 
      ScalePanel.Dock = DockStyle.Top;
      ScalePanel.Location = new Point(0, 0);
      ScalePanel.Name = "ScalePanel";
      ScalePanel.Size = new Size(950, 32);
      ScalePanel.TabIndex = 2;
      ScalePanel.Paint += ScalePanel_Paint;
      ScalePanel.MouseDown += ScalePanel_MouseDown;
      ScalePanel.MouseLeave += ScalePanel_MouseLeave;
      ScalePanel.MouseMove += ScalePanel_MouseMove;
      ScalePanel.Resize += ScalePanel_Resize;
      // 
      // button1
      // 
      button1.FlatStyle = FlatStyle.Popup;
      button1.ForeColor = Color.SteelBlue;
      button1.Image = Properties.Resources.equalizer;
      button1.Location = new Point(5, 2);
      button1.Name = "button1";
      button1.Size = new Size(31, 28);
      button1.TabIndex = 3;
      button1.UseVisualStyleBackColor = false;
      button1.Click += SlidersBtn_Click;
      // 
      // BandViewPanel
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(950, 266);
      Controls.Add(WaterfallControl);
      Controls.Add(button1);
      Controls.Add(ScalePanel);
      Name = "BandViewPanel";
      ShowInTaskbar = false;
      Text = "Band View";
      FormClosing += BandViewPanel_FormClosing;
      ResumeLayout(false);
    }

    #endregion
    public WaterfallControl WaterfallControl;
    private ContextMenuStrip contextMenuStrip1;
    private Button button1;
    internal ScalePanel ScalePanel;
  }
}