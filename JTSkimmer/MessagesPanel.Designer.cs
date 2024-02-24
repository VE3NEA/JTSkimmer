namespace JTSkimmer
{
  partial class MessagesPanel
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessagesPanel));
      toolStripEx1 = new ToolStripEx();
      ClearBtn = new ToolStripButton();
      ScrollDownBtn = new ToolStripButton();
      ViewArchiveBtn = new ToolStripButton();
      CountsLabel = new ToolStripLabel();
      listBox = new ListBoxEx();
      freezeTimer = new System.Windows.Forms.Timer(components);
      toolStripEx1.SuspendLayout();
      SuspendLayout();
      // 
      // toolStripEx1
      // 
      toolStripEx1.Items.AddRange(new ToolStripItem[] { ClearBtn, ScrollDownBtn, ViewArchiveBtn, CountsLabel });
      toolStripEx1.Location = new Point(0, 0);
      toolStripEx1.Name = "toolStripEx1";
      toolStripEx1.Size = new Size(800, 25);
      toolStripEx1.TabIndex = 0;
      toolStripEx1.Text = "toolStripEx1";
      // 
      // ClearBtn
      // 
      ClearBtn.DisplayStyle = ToolStripItemDisplayStyle.Text;
      ClearBtn.ForeColor = Color.SteelBlue;
      ClearBtn.Image = (Image)resources.GetObject("ClearBtn.Image");
      ClearBtn.ImageTransparentColor = Color.Magenta;
      ClearBtn.Name = "ClearBtn";
      ClearBtn.Size = new Size(23, 22);
      ClearBtn.Text = "=";
      ClearBtn.ToolTipText = "Clear";
      ClearBtn.Click += ClearBtn_Click;
      // 
      // ScrollDownBtn
      // 
      ScrollDownBtn.DisplayStyle = ToolStripItemDisplayStyle.Text;
      ScrollDownBtn.ForeColor = Color.SteelBlue;
      ScrollDownBtn.Image = (Image)resources.GetObject("ScrollDownBtn.Image");
      ScrollDownBtn.ImageTransparentColor = Color.Magenta;
      ScrollDownBtn.Name = "ScrollDownBtn";
      ScrollDownBtn.Size = new Size(23, 22);
      ScrollDownBtn.Text = "=";
      ScrollDownBtn.ToolTipText = "Scroll to Bottom";
      ScrollDownBtn.Click += ScrollDownBtn_Click;
      // 
      // ViewArchiveBtn
      // 
      ViewArchiveBtn.DisplayStyle = ToolStripItemDisplayStyle.Text;
      ViewArchiveBtn.ForeColor = Color.SteelBlue;
      ViewArchiveBtn.Image = (Image)resources.GetObject("ViewArchiveBtn.Image");
      ViewArchiveBtn.ImageTransparentColor = Color.Magenta;
      ViewArchiveBtn.Name = "ViewArchiveBtn";
      ViewArchiveBtn.Size = new Size(23, 22);
      ViewArchiveBtn.Text = "=";
      ViewArchiveBtn.ToolTipText = "View Archive";
      ViewArchiveBtn.Click += ViewArchiveBtn_Click;
      // 
      // CountsLabel
      // 
      CountsLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
      CountsLabel.Name = "CountsLabel";
      CountsLabel.Size = new Size(0, 22);
      // 
      // listBox
      // 
      listBox.Dock = DockStyle.Fill;
      listBox.DrawMode = DrawMode.OwnerDrawFixed;
      listBox.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point);
      listBox.IntegralHeight = false;
      listBox.ItemHeight = 16;
      listBox.Location = new Point(0, 25);
      listBox.Name = "listBox";
      listBox.SelectionMode = SelectionMode.None;
      listBox.Size = new Size(800, 425);
      listBox.TabIndex = 1;
      listBox.Scroll += listBox_Scroll;
      listBox.DrawItem += listBox_DrawItem;
      listBox.MouseLeave += listBox_MouseLeave;
      listBox.MouseMove += listBox_MouseMove;
      listBox.MouseUp += listBox_MouseUp;
      listBox.MouseWheel += listBox_MouseWheel;
      // 
      // freezeTimer
      // 
      freezeTimer.Interval = 1000;
      freezeTimer.Tick += freezeTimer_Tick;
      // 
      // MessagesPanel
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(800, 450);
      Controls.Add(listBox);
      Controls.Add(toolStripEx1);
      Name = "MessagesPanel";
      ShowInTaskbar = false;
      Text = "Decoded Messages";
      FormClosing += MessagesPanel_FormClosing;
      toolStripEx1.ResumeLayout(false);
      toolStripEx1.PerformLayout();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private ToolStripEx toolStripEx1;
    private ToolStripButton ClearBtn;
    private ToolStripButton ScrollDownBtn;
    private ListBoxEx listBox;
    private System.Windows.Forms.Timer freezeTimer;
    private ToolStripLabel CountsLabel;
    private ToolStripButton ViewArchiveBtn;
  }
}