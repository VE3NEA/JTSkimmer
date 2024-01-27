using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VE3NEA.HamCockpit.DspFun;

namespace JTSkimmer
{
    internal class Context
  {
    internal Settings Settings = new();
    internal readonly Font AwesomeFont8 = FontAwesomeFactory.Create(8);
    internal readonly Font AwesomeFont14 = FontAwesomeFactory.Create(14);
    internal readonly PaletteManager PaletteManager = new PaletteManager();

    internal MainForm MainForm { get; }
    internal ReceiversPanel? ReceiversPanel;
    internal MessagesPanel? MessagesPanel;
    internal BandViewPanel? BandViewPanel;

    internal BaseSdrDevice? Sdr;
    internal List<Receiver> Receivers = new();

    internal Downsampler? Downsampler;
    internal readonly Soundcard SpeakerSoundcard = new();
    internal readonly Soundcard VacSoundcard = new();
    internal readonly OmniRigClient OmniRig = new();
    internal readonly MessageDistributor MessageDistributor;
    internal IqOutput IqOutput;

    public Context(MainForm mainForm)
    {
      MainForm = mainForm;
      MessageDistributor = new(this);
    }
  }
}
