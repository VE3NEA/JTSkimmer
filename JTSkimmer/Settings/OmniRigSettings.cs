using System.ComponentModel;
using VE3NEA.HamCockpit.DspFun;

namespace JTSkimmer
{
  public class OmniRigSettings
  {
    [DisplayName("OmniRig Radio")]
    [Description("Radio selection in OmniRig")]
    [DefaultValue(OmniRigRig.Rig1)]
    public OmniRigRig RigNo { get; set; } = OmniRigRig.Rig1;

    [DisplayName("Pause when transmitting")]
    [Description("Pause the waterfalls and decoding when XCVR is transmitting")]
    [DefaultValue(false)]
    public bool PauseWhenTx { get; set; } = false;

    public override string ToString() { return string.Empty; }
  }
}