using System.ComponentModel;
using Newtonsoft.Json;

namespace JTSkimmer
{
  internal class Settings
  {
    // non-browsable settings

    public UiSettings Ui = new();
    public List<SdrInfo> Sdrs = new();
    public List<ReceiverSettings> Receivers = new();
    public float? NoiseFloorZero;
    public NoiseBlankerSettings NoiseBlanker = new();


    // browsable settings

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public UserSettings User { get; set; } = new();

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public AudioSettings Audio { get; set; } = new();

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public WaterfallSettings Waterfall { get; set; } = new();

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public DecodingSettings Decoding { get; set; } = new();

    [DisplayName("OmniRig")]
    [Description("OmniRig settings")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public OmniRigSettings OmniRig { get; set; } = new();

    [DisplayName("Messages Panel")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public MessagePanelSettings MessagesPanel { get; set; } = new();

    [DisplayName("Message Distribution")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public DistributorSettings Distributor { get; set; } = new();

    [DisplayName("I/Q Output")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public IqOutputSettings IqOutput { get; set; } = new();


    // functions

    public SdrInfo? SelectedSdr() 
    { 
      return Sdrs.Find(s => s.Selected); 
    }

    private static string GetFileName()
    {
      return Path.Combine(Utils.GetUserDataFolder(), "Settings.json");
    }

    public void LoadFromFile()
    {
      if (File.Exists(GetFileName()))
        JsonConvert.PopulateObject(File.ReadAllText(GetFileName()), this);
      SetDefaults();
    }

    public void SaveToFile()
    {
      File.WriteAllText(GetFileName(), JsonConvert.SerializeObject(this));
    }

    private void SetDefaults()
    {
      if (string.IsNullOrEmpty(Decoding.Jt9ExePath))
        Decoding.Jt9ExePath = WsjtxDecoderRunner.FindExe();

      if (string.IsNullOrEmpty(Decoding.JtdxJt9ExePath))
        Decoding.JtdxJt9ExePath = JtdxDecoderRunner.FindExe();
    }
  }
}