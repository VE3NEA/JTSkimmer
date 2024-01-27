namespace JTSkimmer
{
  public partial class ReceiverSettingsDlg : Form
  {
    private readonly ReceiverPanel ReceiverPanel;

    internal ReceiverSettingsDlg()
    {
      InitializeComponent();
    }

    internal ReceiverSettingsDlg(ReceiverPanel receiverPanel)
    {
      InitializeComponent();

      ReceiverPanel = receiverPanel;
      SettingsToDialog();
    }

    private void ReceiverSettingsDlg_Shown(object sender, EventArgs e)
    {
      FrequencyUpDown.Focus();
      FrequencyUpDown.Select(0, FrequencyUpDown.Text.Length);
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      DialogToSettings();
    }

    private void SettingsToDialog()
    {
      var hz = ReceiverPanel.Receiver.Settings.Frequency ?? Receiver.SdrSettings?.CenterFrequency ?? 0;
      FrequencyUpDown.Value = (decimal)(hz / 1000f);

      ModeLabel.Text = ReceiverPanel.Receiver.Settings.DecoderMode;

      PopulateDecoderTypeCombo();
      DecoderTypeComboBox.SelectedItem = ReceiverPanel.Receiver.Settings.DecoderType;
    }

    private void DialogToSettings()
    {
      ReceiverPanel.Receiver.SetFrequency((int)(FrequencyUpDown.Value * 1000));

      ReceiverPanel.Receiver.Settings.DecoderMode = ModeLabel.Text;
      ReceiverPanel.Receiver.Settings.DecoderType = (DecoderType)(DecoderTypeComboBox.SelectedItem ?? DecoderType.WSJTX);

      ReceiverPanel.Receiver.SetUpDecoding();

      ReceiverPanel.EnableDisable();
      ReceiverPanel.UpdateLabels();
    }

    private void ModeBtn_Click(object sender, EventArgs e)
    {
      ModeMenu.Show(ModeBtn, new Point(ModeBtn.Width, ModeBtn.Height), ToolStripDropDownDirection.BelowLeft);
    }

    private void Q65SubMenu_Click(object sender, EventArgs e)
    {
      var menuItem = (ToolStripMenuItem)sender;
      string modeName = $"Q65-{menuItem.OwnerItem.Text}{menuItem.Text}";

      ModeLabel.Text = modeName;
      PopulateDecoderTypeCombo();
    }

    private static DecoderType[] WsjtxDecoder = { DecoderType.WSJTX };
    private static DecoderType[] JtdxDecoders = { DecoderType.WSJTX, DecoderType.JTDX };

    private void Jt65SubMenu_Click(object sender, EventArgs e)
    {
      var menuItem = (ToolStripMenuItem)sender;
      string modeName = $"JT65{menuItem.Text}";

      ModeLabel.Text = modeName;
      PopulateDecoderTypeCombo();
    }

    private void ModeMenu_Click(object sender, EventArgs e)
    {
      var menuItem = (ToolStripMenuItem)sender;
      string modeName = menuItem.Text;
      ModeLabel.Text = modeName;
      PopulateDecoderTypeCombo();
    }

    private void DecoderSettingsBtn_Click(object sender, EventArgs e)
    {
      DecoderSettingsDialog.EditSettings(ReceiverPanel.Receiver.Settings, (DecoderType)DecoderTypeComboBox.SelectedItem);
    }

    private void PopulateDecoderTypeCombo()
    {
      string mode = ModeLabel.Text;

      if (mode == "FT4" || mode == "FT8" || mode == "JT65A")
        DecoderTypeComboBox.DataSource = JtdxDecoders;
      else
        DecoderTypeComboBox.DataSource = WsjtxDecoder;
    }
  }
}
