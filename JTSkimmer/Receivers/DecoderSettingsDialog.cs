namespace JTSkimmer
{
  public partial class DecoderSettingsDialog : Form
  {
    private ReceiverSettings receiverSettings;

    public DecoderSettingsDialog()
    {
      InitializeComponent();
      Grid.PropertySort = PropertySort.NoSort;
    }

    internal static DialogResult EditSettings(ReceiverSettings receiverSettings, DecoderType decoderType)
    {
      var dlg = new DecoderSettingsDialog();
      dlg.receiverSettings = receiverSettings;

      dlg.Grid.SelectedObject = decoderType == DecoderType.WSJTX ?
        Utils.DeepClone(receiverSettings.WsjtxDecoder) :
        Utils.DeepClone(receiverSettings.JtdxDecoder);

      return dlg.ShowDialog();
    }

    private void OkBtn_Click(object sender, EventArgs e)
    {
      if (Grid.SelectedObject is WsjtxDecoderSettings)
        receiverSettings.WsjtxDecoder = (WsjtxDecoderSettings)Grid.SelectedObject;
      else
        receiverSettings.JtdxDecoder = (JtdxDecoderSettings)Grid.SelectedObject;

      DialogResult = DialogResult.OK;
    }

    private void ResetMenuItem_Click(object sender, EventArgs e)
    {
      Grid.ResetSelectedProperty();
    }

    private void ResetBtn_Click(object sender, EventArgs e)
    {
      Grid.SelectedObject = Activator.CreateInstance(Grid.SelectedObject.GetType());
    }
  }
}
