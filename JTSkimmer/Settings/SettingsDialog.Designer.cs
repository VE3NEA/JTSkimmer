namespace JTSkimmer
{
  partial class SettingsDialog
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
      applyBtn = new Button();
      cancelBtn = new Button();
      okBtn = new Button();
      grid = new PropertyGridEx();
      PropertyGridMenu = new ContextMenuStrip(components);
      resetToolStripMenuItem = new ToolStripMenuItem();
      panel1.SuspendLayout();
      PropertyGridMenu.SuspendLayout();
      SuspendLayout();
      // 
      // panel1
      // 
      panel1.Controls.Add(applyBtn);
      panel1.Controls.Add(cancelBtn);
      panel1.Controls.Add(okBtn);
      panel1.Dock = DockStyle.Bottom;
      panel1.Location = new Point(0, 660);
      panel1.Margin = new Padding(4, 3, 4, 3);
      panel1.Name = "panel1";
      panel1.Size = new Size(538, 35);
      panel1.TabIndex = 4;
      // 
      // applyBtn
      // 
      applyBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      applyBtn.Location = new Point(351, 3);
      applyBtn.Margin = new Padding(4, 3, 4, 3);
      applyBtn.Name = "applyBtn";
      applyBtn.Size = new Size(88, 27);
      applyBtn.TabIndex = 1;
      applyBtn.Text = "Apply";
      applyBtn.UseVisualStyleBackColor = true;
      applyBtn.Click += applyBtn_Click;
      // 
      // cancelBtn
      // 
      cancelBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      cancelBtn.DialogResult = DialogResult.Cancel;
      cancelBtn.Location = new Point(445, 4);
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
      okBtn.Location = new Point(257, 3);
      okBtn.Margin = new Padding(4, 3, 4, 3);
      okBtn.Name = "okBtn";
      okBtn.Size = new Size(88, 27);
      okBtn.TabIndex = 0;
      okBtn.Text = "OK";
      okBtn.UseVisualStyleBackColor = true;
      okBtn.Click += okBtn_Click;
      // 
      // grid
      // 
      grid.ContextMenuStrip = PropertyGridMenu;
      grid.Dock = DockStyle.Fill;
      grid.Location = new Point(0, 0);
      grid.Margin = new Padding(4, 3, 4, 3);
      grid.Name = "grid";
      grid.PropertySort = PropertySort.NoSort;
      grid.Size = new Size(538, 660);
      grid.TabIndex = 3;
      grid.ToolbarVisible = false;
      grid.PropertyValueChanged += grid_PropertyValueChanged;
      // 
      // PropertyGridMenu
      // 
      PropertyGridMenu.Items.AddRange(new ToolStripItem[] { resetToolStripMenuItem });
      PropertyGridMenu.Name = "PropertyGridMenu";
      PropertyGridMenu.Size = new Size(103, 26);
      // 
      // resetToolStripMenuItem
      // 
      resetToolStripMenuItem.Name = "resetToolStripMenuItem";
      resetToolStripMenuItem.Size = new Size(102, 22);
      resetToolStripMenuItem.Text = "Reset";
      resetToolStripMenuItem.Click += resetToolStripMenuItem_Click;
      // 
      // SettingsDialog
      // 
      AcceptButton = okBtn;
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      CancelButton = cancelBtn;
      ClientSize = new Size(538, 695);
      Controls.Add(grid);
      Controls.Add(panel1);
      Margin = new Padding(4, 3, 4, 3);
      Name = "SettingsDialog";
      ShowInTaskbar = false;
      StartPosition = FormStartPosition.CenterParent;
      Text = "Settings";
      panel1.ResumeLayout(false);
      PropertyGridMenu.ResumeLayout(false);
      ResumeLayout(false);
    }

    #endregion

    private Panel panel1;
    private Button cancelBtn;
    private Button okBtn;
    private PropertyGridEx grid;
    private Button applyBtn;
    private ContextMenuStrip PropertyGridMenu;
    private ToolStripMenuItem resetToolStripMenuItem;
  }
}