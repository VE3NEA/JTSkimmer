using System.Text.RegularExpressions;

namespace JTSkimmer
{
  internal partial class SettingsDialog : Form
  {
    private readonly Context ctx;
    private readonly List<string> ChangedFields = new();

    internal SettingsDialog()
    {
      InitializeComponent();
    }

    internal SettingsDialog(Context ctx, string section = null)
    {
      InitializeComponent();
      this.ctx = ctx;
      grid.SelectedObject = Utils.DeepClone(ctx.Settings);
      grid.ExpandAllGridItems();
      SelectSection(section);
    }

    private void SelectSection(string section)
    {
      if (section == null) return;

      var gridItem = grid.GetItemByFullName(section);
      grid.ExpandAndSelect(gridItem);
    }

    private void resetToolStripMenuItem_Click(object sender, EventArgs e)
    {
      grid.ResetSelectedProperty();

      string label = PropertyGridEx.GetItemProperty(grid.SelectedGridItem, "HelpKeyword");
      ChangedFields.Add(label);
    }

    private void applyBtn_Click(object sender, EventArgs e)
    {
      ctx.Settings = (Settings)Utils.DeepClone((Settings)grid.SelectedObject);
      ApplyChangedSettings();
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      ctx.Settings = (Settings)grid.SelectedObject;
      ApplyChangedSettings();
    }




    //--------------------------------------------------------------------------------------------------------------
    //                                        validate changes
    //--------------------------------------------------------------------------------------------------------------
    private void grid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
    {
      bool changed = true;
      string label = PropertyGridEx.GetItemProperty(e.ChangedItem, "HelpKeyword");

      switch (label)
      {
        case "JTSkimmer.UserSettings.Call":
          changed = ValidateField(e, Utils.CallsignRegex, "callsign", CharacterCasing.Upper);
          break;

        case "JTSkimmer.UserSettings.Square":
          changed = ValidateField(e, Utils.GridSquare6Regex, "grid square", CharacterCasing.Upper);
          break;

        case "JTSkimmer.DxClusterSettings.Enabled":
          var sett = (Settings)grid.SelectedObject;
          changed = !sett.Distributor.DxCluster.Enabled || (!string.IsNullOrEmpty(sett.User.Call) && !string.IsNullOrEmpty(sett.User.Square));
          if (!changed)
          {
            e.ChangedItem.PropertyDescriptor.SetValue(e.ChangedItem.Parent.Value, e.OldValue);
            MessageBox.Show($"Please enter your callsign and grid square before you enable DX Cluster", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          break;

        case "JTSkimmer.PskReporterSettings.Enabled":
          sett = (Settings)grid.SelectedObject;
          changed = !sett.Distributor.PskReporter.Enabled || (!string.IsNullOrEmpty(sett.User.Call) && !string.IsNullOrEmpty(sett.User.Square));
          if (!changed)
          {
            e.ChangedItem.PropertyDescriptor.SetValue(e.ChangedItem.Parent.Value, e.OldValue);
            MessageBox.Show($"Please enter your callsign and grid square before you enable submission to PSK Reporter", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          break;
      }

      if (changed) ChangedFields.Add(label);
    }

    private bool ValidateField(PropertyValueChangedEventArgs e, Regex regEx, string fieldName, CharacterCasing casing)
    {
      string newValue = e.ChangedItem.Value.ToString();
      string cleanValue = Utils.SetCasing(newValue.Trim(), casing);

      if (!regEx.IsMatch(cleanValue))
      {
        e.ChangedItem.PropertyDescriptor.SetValue(e.ChangedItem.Parent.Value, e.OldValue);
        MessageBox.Show($"Invalid {fieldName}: \"{newValue}\"", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }

      if (cleanValue != newValue) e.ChangedItem.PropertyDescriptor.SetValue(e.ChangedItem.Parent.Value, cleanValue);
      return true;
    }




    //--------------------------------------------------------------------------------------------------------------
    //                                            apply changes
    //--------------------------------------------------------------------------------------------------------------
    private void ApplyChangedSettings()
    {
      if (ChangedFields.Exists(s => s.StartsWith("JTSkimmer.AudioSettings.")))
        ctx.MainForm.ApplyAudioSettings();

      if (ChangedFields.Exists(s => s.StartsWith("JTSkimmer.DecodingSettings.")))
        ctx.MainForm.ApplyDecodingSettings();

      if (ChangedFields.Exists(s => s.StartsWith("JTSkimmer.DxClusterSettings.")))
        ctx.MessageDistributor.ApplyDxClusterSettings();

      if (ChangedFields.Exists(s => s.StartsWith("JTSkimmer.UdpSenderSettings.")))
        ctx.MessageDistributor.ApplyUdpSenderSettings();

      if (ChangedFields.Exists(s => s.StartsWith("JTSkimmer.PskReporterSettings.")) ||
        ChangedFields.Exists(s => s.StartsWith("JTSkimmer.UserSettings.")))
        ctx.MessageDistributor.ApplyPskReporterSettings();

      if (ChangedFields.Exists(s => s.StartsWith("JTSkimmer.IqOutputSettings.")))
        ctx.IqOutput.ApplySettings();

      if (ChangedFields.Exists(s => s.StartsWith("JTSkimmer.OmniRigSettings.")))
        ctx.MainForm.ApplyOmnirigSettings();

      ChangedFields.Clear();
    }
  }
}
