namespace JTSkimmer
{
  partial class DecoderSettingsDialog
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
      ResetBtn = new Button();
      CancelBtn = new Button();
      OkBtn = new Button();
      Grid = new PropertyGrid();
      OptionsPropertyGridMenuStrip = new ContextMenuStrip(components);
      ResetMenuItem = new ToolStripMenuItem();
      panel1.SuspendLayout();
      OptionsPropertyGridMenuStrip.SuspendLayout();
      SuspendLayout();
      // 
      // panel1
      // 
      panel1.Controls.Add(ResetBtn);
      panel1.Controls.Add(CancelBtn);
      panel1.Controls.Add(OkBtn);
      panel1.Dock = DockStyle.Bottom;
      panel1.Location = new Point(0, 436);
      panel1.Margin = new Padding(4, 3, 4, 3);
      panel1.Name = "panel1";
      panel1.Size = new Size(362, 50);
      panel1.TabIndex = 1;
      // 
      // ResetBtn
      // 
      ResetBtn.Location = new Point(109, 11);
      ResetBtn.Margin = new Padding(4, 3, 4, 3);
      ResetBtn.Name = "ResetBtn";
      ResetBtn.Size = new Size(88, 27);
      ResetBtn.TabIndex = 2;
      ResetBtn.Text = "Reset All";
      ResetBtn.UseVisualStyleBackColor = true;
      ResetBtn.Click += ResetBtn_Click;
      // 
      // CancelBtn
      // 
      CancelBtn.DialogResult = DialogResult.Cancel;
      CancelBtn.Location = new Point(205, 11);
      CancelBtn.Margin = new Padding(4, 3, 4, 3);
      CancelBtn.Name = "CancelBtn";
      CancelBtn.Size = new Size(88, 27);
      CancelBtn.TabIndex = 1;
      CancelBtn.Text = "Cancel";
      CancelBtn.UseVisualStyleBackColor = true;
      // 
      // OkBtn
      // 
      OkBtn.Location = new Point(13, 11);
      OkBtn.Margin = new Padding(4, 3, 4, 3);
      OkBtn.Name = "OkBtn";
      OkBtn.Size = new Size(88, 27);
      OkBtn.TabIndex = 0;
      OkBtn.Text = "OK";
      OkBtn.UseVisualStyleBackColor = true;
      OkBtn.Click += OkBtn_Click;
      // 
      // Grid
      // 
      Grid.ContextMenuStrip = OptionsPropertyGridMenuStrip;
      Grid.Dock = DockStyle.Fill;
      Grid.Location = new Point(0, 0);
      Grid.Margin = new Padding(4, 3, 4, 3);
      Grid.Name = "Grid";
      Grid.Size = new Size(362, 436);
      Grid.TabIndex = 2;
      Grid.ToolbarVisible = false;
      // 
      // OptionsPropertyGridMenuStrip
      // 
      OptionsPropertyGridMenuStrip.Items.AddRange(new ToolStripItem[] { ResetMenuItem });
      OptionsPropertyGridMenuStrip.Name = "contextMenuStrip1";
      OptionsPropertyGridMenuStrip.Size = new Size(189, 26);
      // 
      // ResetMenuItem
      // 
      ResetMenuItem.Name = "ResetMenuItem";
      ResetMenuItem.Size = new Size(188, 22);
      ResetMenuItem.Text = "Reset to Default Value";
      ResetMenuItem.Click += ResetMenuItem_Click;
      // 
      // DecoderSettingsDialog
      // 
      AcceptButton = OkBtn;
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      CancelButton = CancelBtn;
      ClientSize = new Size(362, 486);
      Controls.Add(Grid);
      Controls.Add(panel1);
      Margin = new Padding(4, 3, 4, 3);
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "DecoderSettingsDialog";
      ShowInTaskbar = false;
      StartPosition = FormStartPosition.CenterParent;
      Text = "Decoder Options";
      panel1.ResumeLayout(false);
      OptionsPropertyGridMenuStrip.ResumeLayout(false);
      ResumeLayout(false);
    }

    #endregion

    private Panel panel1;
    private Button CancelBtn;
    private Button OkBtn;
    private PropertyGrid Grid;
    private ContextMenuStrip OptionsPropertyGridMenuStrip;
    private ToolStripMenuItem ResetMenuItem;
    private Button ResetBtn;
  }
}