using System.ComponentModel;
using VE3NEA.HamCockpit.PluginHelpers;

namespace JTSkimmer
{
  internal class AudioSettings
  {
    // non-browsable
    public int SoundcardVolume = -25;


    [DisplayName("Speaker Audio Device")]
    [Description("Soundcard to be used for audio output")]
    [TypeConverter(typeof(OutputSoundcardNameConverter))]
    public string? SpeakerSoundcard { get; set; } = Soundcard.GetDefaultSoundcardId();

    [DisplayName("VAC Device")]
    [Description("Virtual Audio Cable to feed audio to WSJT-X")]
    [TypeConverter(typeof(OutputSoundcardNameConverter))]
    public string? Vac { get; set; } = Soundcard.GetFirstVacId();

    [DisplayName("VAC Gain")]
    [Description("Virtual Audio Cable Gain, in dB")]
    [DefaultValue(-40)]
    public int VacVolume { get; set; } = -40;


    public override string ToString() { return string.Empty; }
  }
}
