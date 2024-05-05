using System.ComponentModel;

namespace JTSkimmer
{
  public enum WsjtxDecodeDepth
  {
    Fast = 1,   // ndepth=1: 1 pass, no subtraction
    Normal = 2, // ndepth=2: 3 passes, bp only
    Deep = 3    // ndepth=3: 3 passes, bp+osd
  }

  public enum WsjtxQsoProgress
  {
    CALLING,
    REPLYING,
    REPORT,
    ROGER_REPORT,
    ROGERS,
    SIGNOFF
  }

  public class WsjtxDecoderSettings
  {
    [DisplayName("Input Amplitude")]
    [Description("Input signal amplitude, dBFS")]
    [DefaultValue(-10)]
    public int InputAmplitude { get; set; } = -10;

    [DefaultValue(WsjtxDecodeDepth.Deep)]
    public WsjtxDecodeDepth DecodingDepth { get; set; } = WsjtxDecodeDepth.Deep;

    [DefaultValue(true)]
    public bool EnableAP { get; set; } = true;


    [Description("Q65 enable averagins")]
    [DefaultValue(true)]
    public bool Q65Average { get; set; } = true;

    [Description("Q65 auto clear average after decode")]
    [DefaultValue(true)]
    public bool Q65ClearAfter{ get; set; } = true;

    [DefaultValue(WsjtxQsoProgress.CALLING)]
    public WsjtxQsoProgress QsoProgress { get; set; } = WsjtxQsoProgress.CALLING;

    [DefaultValue(1500)]
    public int RxOffsetHz { get; set; } = 1500;

    [DefaultValue(1500)]
    public int TxOffsetHz { get; set; } = 1500;

    [DefaultValue(200)]
    public int LowDecodeHz { get; set; } = 200;

    [DefaultValue(4000)]
    public int HighDecodeHz { get; set; } = 4000;

    [DefaultValue(50)]
    public int RxToleranceHz { get; set; } = 50;

    [DefaultValue(0f)]
    public float EmeDelay { get; set; } = 0f;

    public string MyCall { get; set; } = string.Empty;

    public string MyGrid { get; set; } = string.Empty;

    public string HisCall { get; set; } = string.Empty;

    public string HisGrid { get; set; } = string.Empty;
  }
}