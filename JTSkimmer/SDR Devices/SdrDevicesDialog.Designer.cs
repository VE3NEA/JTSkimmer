namespace JTSkimmer
{
  partial class SdrDevicesDialog
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SdrDevicesDialog));
      Grid = new PropertyGridEx();
      PropertyGridMenu = new ContextMenuStrip(components);
      resetToolStripMenuItem = new ToolStripMenuItem();
      panel3 = new Panel();
      CalibrateBtn = new Button();
      label5 = new Label();
      label6 = new Label();
      PpmUpDown = new NumericUpDown();
      label4 = new Label();
      label3 = new Label();
      label2 = new Label();
      label1 = new Label();
      BandwidthCombobox = new ComboBox();
      CenterFrequencyUpDown = new NumericUpDown();
      DeleteBtn = new Button();
      refreshBtn = new Button();
      cancelBtn = new Button();
      okBtn = new Button();
      panel2 = new Panel();
      ListView = new ListView();
      columnHeader2 = new ColumnHeader();
      columnHeader3 = new ColumnHeader();
      imageList1 = new ImageList(components);
      panel1 = new Panel();
      toolTip = new ToolTip(components);
      PropertyGridMenu.SuspendLayout();
      panel3.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)PpmUpDown).BeginInit();
      ((System.ComponentModel.ISupportInitialize)CenterFrequencyUpDown).BeginInit();
      panel2.SuspendLayout();
      panel1.SuspendLayout();
      SuspendLayout();
      // 
      // Grid
      // 
      Grid.ContextMenuStrip = PropertyGridMenu;
      Grid.Dock = DockStyle.Fill;
      Grid.Location = new Point(4, 4);
      Grid.Margin = new Padding(4, 3, 4, 3);
      Grid.Name = "Grid";
      Grid.PropertySort = PropertySort.NoSort;
      Grid.Size = new Size(287, 248);
      Grid.TabIndex = 1;
      Grid.ToolbarVisible = false;
      Grid.PropertyValueChanged += Grid_PropertyValueChanged;
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
      resetToolStripMenuItem.Click += ResetToolStripMenuItem_Click;
      // 
      // panel3
      // 
      panel3.Controls.Add(CalibrateBtn);
      panel3.Controls.Add(label5);
      panel3.Controls.Add(label6);
      panel3.Controls.Add(PpmUpDown);
      panel3.Controls.Add(label4);
      panel3.Controls.Add(label3);
      panel3.Controls.Add(label2);
      panel3.Controls.Add(label1);
      panel3.Controls.Add(BandwidthCombobox);
      panel3.Controls.Add(CenterFrequencyUpDown);
      panel3.Controls.Add(DeleteBtn);
      panel3.Controls.Add(refreshBtn);
      panel3.Controls.Add(cancelBtn);
      panel3.Controls.Add(okBtn);
      panel3.Dock = DockStyle.Right;
      panel3.Location = new Point(291, 4);
      panel3.Margin = new Padding(4, 3, 4, 3);
      panel3.Name = "panel3";
      panel3.Size = new Size(202, 248);
      panel3.TabIndex = 2;
      // 
      // CalibrateBtn
      // 
      CalibrateBtn.Location = new Point(155, 175);
      CalibrateBtn.Name = "CalibrateBtn";
      CalibrateBtn.Size = new Size(28, 27);
      CalibrateBtn.TabIndex = 14;
      CalibrateBtn.Text = "...";
      toolTip.SetToolTip(CalibrateBtn, "Compute Calibration Factor");
      CalibrateBtn.UseVisualStyleBackColor = true;
      CalibrateBtn.Click += CalibrateBtn_Click;
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new Point(117, 181);
      label5.Name = "label5";
      label5.Size = new Size(32, 15);
      label5.TabIndex = 13;
      label5.Text = "PPM";
      // 
      // label6
      // 
      label6.AutoSize = true;
      label6.Location = new Point(11, 157);
      label6.Name = "label6";
      label6.Size = new Size(123, 15);
      label6.TabIndex = 12;
      label6.Text = "Frequency Calibration";
      // 
      // PpmUpDown
      // 
      PpmUpDown.DecimalPlaces = 4;
      PpmUpDown.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
      PpmUpDown.Location = new Point(11, 175);
      PpmUpDown.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
      PpmUpDown.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
      PpmUpDown.Name = "PpmUpDown";
      PpmUpDown.Size = new Size(100, 27);
      PpmUpDown.TabIndex = 11;
      PpmUpDown.ValueChanged += PpmUpDown_ValueChanged;
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new Point(117, 128);
      label4.Name = "label4";
      label4.Size = new Size(27, 15);
      label4.TabIndex = 10;
      label4.Text = "kHz";
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(117, 72);
      label3.Name = "label3";
      label3.Size = new Size(27, 15);
      label3.TabIndex = 9;
      label3.Text = "kHz";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(11, 104);
      label2.Name = "label2";
      label2.Size = new Size(100, 15);
      label2.TabIndex = 8;
      label2.Text = "Center Frequency";
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(11, 47);
      label1.Name = "label1";
      label1.Size = new Size(64, 15);
      label1.TabIndex = 7;
      label1.Text = "Bandwidth";
      // 
      // BandwidthCombobox
      // 
      BandwidthCombobox.DropDownStyle = ComboBoxStyle.DropDownList;
      BandwidthCombobox.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
      BandwidthCombobox.FormattingEnabled = true;
      BandwidthCombobox.Location = new Point(11, 65);
      BandwidthCombobox.Name = "BandwidthCombobox";
      BandwidthCombobox.Size = new Size(100, 28);
      BandwidthCombobox.TabIndex = 3;
      BandwidthCombobox.SelectedIndexChanged += BandwidthCombobox_SelectedIndexChanged;
      // 
      // CenterFrequencyUpDown
      // 
      CenterFrequencyUpDown.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
      CenterFrequencyUpDown.Location = new Point(11, 122);
      CenterFrequencyUpDown.Maximum = new decimal(new int[] { 2000000, 0, 0, 0 });
      CenterFrequencyUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
      CenterFrequencyUpDown.Name = "CenterFrequencyUpDown";
      CenterFrequencyUpDown.Size = new Size(100, 27);
      CenterFrequencyUpDown.TabIndex = 4;
      CenterFrequencyUpDown.Value = new decimal(new int[] { 50150, 0, 0, 0 });
      CenterFrequencyUpDown.ValueChanged += CenterFrequencyUpDown_ValueChanged;
      // 
      // DeleteBtn
      // 
      DeleteBtn.Location = new Point(107, 3);
      DeleteBtn.Name = "DeleteBtn";
      DeleteBtn.Size = new Size(88, 27);
      DeleteBtn.TabIndex = 2;
      DeleteBtn.Text = "Delete";
      DeleteBtn.UseVisualStyleBackColor = true;
      DeleteBtn.Click += DeleteBtn_Click;
      // 
      // refreshBtn
      // 
      refreshBtn.Location = new Point(11, 3);
      refreshBtn.Name = "refreshBtn";
      refreshBtn.Size = new Size(88, 27);
      refreshBtn.TabIndex = 1;
      refreshBtn.Text = "Refresh";
      refreshBtn.UseVisualStyleBackColor = true;
      refreshBtn.Click += refreshBtn_Click;
      // 
      // cancelBtn
      // 
      cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      cancelBtn.DialogResult = DialogResult.Cancel;
      cancelBtn.Location = new Point(107, 217);
      cancelBtn.Margin = new Padding(4, 3, 4, 3);
      cancelBtn.Name = "cancelBtn";
      cancelBtn.Size = new Size(88, 27);
      cancelBtn.TabIndex = 6;
      cancelBtn.Text = "Cancel";
      cancelBtn.UseVisualStyleBackColor = true;
      // 
      // okBtn
      // 
      okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      okBtn.DialogResult = DialogResult.OK;
      okBtn.Location = new Point(11, 217);
      okBtn.Margin = new Padding(4, 3, 4, 3);
      okBtn.Name = "okBtn";
      okBtn.Size = new Size(88, 27);
      okBtn.TabIndex = 5;
      okBtn.Text = "OK";
      okBtn.UseVisualStyleBackColor = true;
      okBtn.Click += okBtn_Click;
      // 
      // panel2
      // 
      panel2.Controls.Add(Grid);
      panel2.Controls.Add(panel3);
      panel2.Dock = DockStyle.Fill;
      panel2.Location = new Point(0, 118);
      panel2.Name = "panel2";
      panel2.Padding = new Padding(4);
      panel2.Size = new Size(497, 256);
      panel2.TabIndex = 2;
      // 
      // ListView
      // 
      ListView.CheckBoxes = true;
      ListView.Columns.AddRange(new ColumnHeader[] { columnHeader2, columnHeader3 });
      ListView.Dock = DockStyle.Fill;
      ListView.Location = new Point(4, 4);
      ListView.MultiSelect = false;
      ListView.Name = "ListView";
      ListView.Size = new Size(489, 110);
      ListView.StateImageList = imageList1;
      ListView.TabIndex = 1;
      ListView.UseCompatibleStateImageBehavior = false;
      ListView.View = View.Details;
      ListView.ItemChecked += ListView_ItemChecked;
      ListView.SelectedIndexChanged += ListView_SelectedIndexChanged;
      // 
      // columnHeader2
      // 
      columnHeader2.Text = "Name";
      columnHeader2.Width = 300;
      // 
      // columnHeader3
      // 
      columnHeader3.Text = "Serial Number";
      columnHeader3.Width = 150;
      // 
      // imageList1
      // 
      imageList1.ColorDepth = ColorDepth.Depth32Bit;
      imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
      imageList1.TransparentColor = Color.Transparent;
      imageList1.Images.SetKeyName(0, "RadioButtonOn");
      imageList1.Images.SetKeyName(1, "RadioButtonOff");
      // 
      // panel1
      // 
      panel1.Controls.Add(ListView);
      panel1.Dock = DockStyle.Top;
      panel1.Location = new Point(0, 0);
      panel1.Name = "panel1";
      panel1.Padding = new Padding(4);
      panel1.Size = new Size(497, 118);
      panel1.TabIndex = 1;
      // 
      // SdrDevicesDialog
      // 
      AcceptButton = okBtn;
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      CancelButton = cancelBtn;
      ClientSize = new Size(497, 374);
      Controls.Add(panel2);
      Controls.Add(panel1);
      FormBorderStyle = FormBorderStyle.FixedDialog;
      Name = "SdrDevicesDialog";
      ShowInTaskbar = false;
      StartPosition = FormStartPosition.CenterParent;
      Text = "SDR Devices";
      Load += SdrsDialog_Load;
      PropertyGridMenu.ResumeLayout(false);
      panel3.ResumeLayout(false);
      panel3.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)PpmUpDown).EndInit();
      ((System.ComponentModel.ISupportInitialize)CenterFrequencyUpDown).EndInit();
      panel2.ResumeLayout(false);
      panel1.ResumeLayout(false);
      ResumeLayout(false);
    }

    #endregion

    private PropertyGridEx Grid;
    private Panel panel3;
    private Button cancelBtn;
    private Button okBtn;
    private Panel panel2;
    private ListView ListView;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private ImageList imageList1;
    private Panel panel1;
    private Button refreshBtn;
    private Button DeleteBtn;
    private ComboBox BandwidthCombobox;
    private NumericUpDown CenterFrequencyUpDown;
    private Label label2;
    private Label label1;
    private Label label4;
    private Label label3;
    private Label label5;
    private Label label6;
    private NumericUpDown PpmUpDown;
    private Button CalibrateBtn;
    private ToolTip toolTip;
    private ContextMenuStrip PropertyGridMenu;
    private ToolStripMenuItem resetToolStripMenuItem;
  }
}