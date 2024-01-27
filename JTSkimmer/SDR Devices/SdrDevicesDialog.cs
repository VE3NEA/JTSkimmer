namespace JTSkimmer
{
  internal partial class SdrDevicesDialog : Form
  {
    private Context ctx;
    private List<SdrInfo> Sdrs;
    bool radioBtnChanging, valuesChanging;


    //----------------------------------------------------------------------------------------------
    //                                      init
    //----------------------------------------------------------------------------------------------

    // design time 
    internal SdrDevicesDialog() { InitializeComponent(); }

    internal SdrDevicesDialog(Context ctx)
    {
      InitializeComponent();
      this.ctx = ctx;
      Sdrs = Utils.DeepClone(ctx.Settings.Sdrs) ?? new();
    }

    private void SdrsDialog_Load(object sender, EventArgs e)
    {
      UpdateSdrList();
    }

    private void UpdateSdrList()
    {
      // list sdr's
      foreach (var sdr in Sdrs) sdr.Present = false;

      var presentSdrs = AirspyDevice.ListDevices();
      presentSdrs.AddRange(RtlSdrDevice.ListDevices());
      presentSdrs.AddRange(SdrPlayDevice.ListDevices());

      foreach (var sdr in presentSdrs)
      {
        var existing = Sdrs.Find(s => s.SerialNumber == sdr.SerialNumber);
        if (existing == null) Sdrs.Add(sdr);
        else existing.Present = true;
      }

      if (Sdrs.Count > 0 && Sdrs.Find(s => s.Selected) == null)
        Sdrs.First().Selected = true;

      // sdr's to listview
      radioBtnChanging = true;
      ListView.Items.Clear();
      foreach (var sdr in Sdrs)
      {
        var item = new ListViewItem(sdr.Name);
        item.SubItems.Add(sdr.SerialNumber);
        item.Checked = sdr.Selected;
        item.Selected = sdr.Selected;
        item.ForeColor = sdr.Present ? Color.Black : Color.Silver;
        item.UseItemStyleForSubItems = false;
        item.SubItems[1].ForeColor = item.ForeColor;
        item.Tag = sdr;
        ListView.Items.Add(item);
      }
      radioBtnChanging = false;

      ListView.Focus();
    }

    private void PopuldateBwDropdown(SdrInfo? info)
    {
      if (info == null)
        BandwidthCombobox.DataSource = null;
      else
      {
        BandwidthCombobox.DisplayMember = "Key";
        BandwidthCombobox.ValueMember = "Value";
        BandwidthCombobox.DataSource = new BindingSource(SdrConst.Bandwidths[(int)info.SdrType], null);
      }
    }




    //----------------------------------------------------------------------------------------------
    //                                  get / set params
    //----------------------------------------------------------------------------------------------
    private SdrInfo? GetSelectedInfo()
    {
      if (ListView.SelectedItems.Count == 0) return null;
      return (SdrInfo)ListView.SelectedItems[0].Tag;
    }

    private SdrSettings? GetEditedSettings()
    {
      return Grid.SelectedObject as SdrSettings;
    }

    private void ValuesToControls()
    {
      var sett = GetEditedSettings();
      if (sett == null) return;

      CenterFrequencyUpDown.Value = sett.CenterFrequency / 1000;

      if (sett.Bandwidth > 0) BandwidthCombobox.SelectedValue = sett.Bandwidth;
      else
      {
        BandwidthCombobox.SelectedIndex = 0;
        sett.Bandwidth = (uint)BandwidthCombobox.SelectedValue;
      }

      PpmUpDown.Value = (decimal)sett.Ppm;
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      ctx.Settings.Sdrs = Sdrs;
    }

    private void refreshBtn_Click(object sender, EventArgs e)
    {
      UpdateSdrList();
    }

    private void DeleteBtn_Click(object sender, EventArgs e)
    {
      var item = ListView.SelectedItems[0];
      Sdrs.Remove((SdrInfo)item.Tag);
      ListView.Items.Remove(item);
      if (ListView.Items.Count > 0)
      {
        item = ListView.Items[0];
        item.Selected = true;
        item.Checked = true;
        ((SdrInfo)item.Tag).Selected = true;
      }
    }




    //----------------------------------------------------------------------------------------------
    //                                      change events
    //----------------------------------------------------------------------------------------------
    private void ListView_SelectedIndexChanged(object sender, EventArgs e)
    {
      var info = GetSelectedInfo();

      if (info != null)
      {
        Grid.SelectedObject = info.Settings;
        DeleteBtn.Enabled = !info.Present;
      }
      else
      {
        Grid.SelectedObject = null;
        DeleteBtn.Enabled = false;
      }

      valuesChanging = true;
      PopuldateBwDropdown(info);
      ValuesToControls();
      valuesChanging = false;
    }

    private void ListView_ItemChecked(object sender, ItemCheckedEventArgs e)
    {
      if (radioBtnChanging) return;
      radioBtnChanging = true;

      foreach (ListViewItem item in ListView.Items)
      {
        item.Checked = item == e.Item;
        item.Selected = item.Checked;
        ((SdrInfo)item.Tag).Selected = item.Checked;
      }

      radioBtnChanging = false;
    }

    private void BandwidthCombobox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (valuesChanging) return;

      var sett = GetEditedSettings();
      if (sett == null) return;

      string? selection = BandwidthCombobox.SelectedValue?.ToString();
      if (selection != null && uint.TryParse(selection, out uint bandwidth))
        sett.Bandwidth = bandwidth;
    }

    private void CenterFrequencyUpDown_ValueChanged(object sender, EventArgs e)
    {
      if (valuesChanging) return;

      var sett = GetEditedSettings();
      if (sett != null) sett.CenterFrequency = (uint)CenterFrequencyUpDown.Value * 1000;
    }

    private void PpmUpDown_ValueChanged(object sender, EventArgs e)
    {
      if (valuesChanging) return;

      var sett = GetEditedSettings();
      if (sett != null) sett.Ppm = (double)PpmUpDown.Value;
    }

    private void CalibrateBtn_Click(object sender, EventArgs e)
    {
      var sett = GetEditedSettings();
      sett.Ppm = FreqCalibrationDialog.ComputePpm(sett.Ppm);
      PpmUpDown.Value = (decimal)sett.Ppm;
    }

    private void ResetToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Grid.ResetSelectedProperty();
    }

    private void Grid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
    {
      string helpKeyword = PropertyGridEx.GetItemProperty(e.ChangedItem, "HelpKeyword");

      switch (helpKeyword)
      {
        case "JTSkimmer.AirspySettings.VgaGain":
        case "JTSkimmer.AirspySettings.MixerGain":
          ValidateGain(e, 15);
          break;

        case "JTSkimmer.AirspySettings.LnaGain":
          ValidateGain(e, 14);
          break;
     }
    }

    private void ValidateGain(PropertyValueChangedEventArgs e, byte maxValue)
    {
      // ensure that the value is in range
      if ((byte)e.ChangedItem.Value > maxValue)
        e.ChangedItem.PropertyDescriptor.SetValue(e.ChangedItem.Parent.Value, maxValue);

      // only in custom gain mode
      var sett = (AirspySettings)Grid.SelectedObject;
      if (sett.GainMode != AirspyGainMode.Custom)
      {
        e.ChangedItem.PropertyDescriptor.SetValue(e.ChangedItem.Parent.Value, e.OldValue);
        MessageBox.Show($"This setting has effect only when Gain Mode is set to Custom", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }
  }
}