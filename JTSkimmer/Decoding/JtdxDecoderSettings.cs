using System.ComponentModel;

namespace JTSkimmer
{
  public class JtdxDecoderSettings
  {
    [DisplayName("Decoding Depth")]
    [DefaultValue(WsjtxDecodeDepth.Deep)]
    public WsjtxDecodeDepth DecodingDepth { get; set; } = WsjtxDecodeDepth.Deep;

    [DisplayName("QSO Progress")]
    [DefaultValue(WsjtxQsoProgress.CALLING)]
    public WsjtxQsoProgress QsoProgress { get; set; } = WsjtxQsoProgress.CALLING;

    [DisplayName("RX Offset")]
    [DefaultValue(1500)]
    public int RxOffsetHz { get; set; } = 1500;

    [DisplayName("TX Offset")]
    [DefaultValue(1500)]
    public int TxOffsetHz { get; set; } = 1500;

    [DisplayName("Low Decoded Tone")]
    [DefaultValue(0)]
    public int LowDecodeHz { get; set; } = 0;

    [DisplayName("High Decoded Tone")]
    [DefaultValue(4000)]
    public int HighDecodeHz { get; set; } = 4000;

    [DisplayName("RX Tolerance")]
    [DefaultValue(50)]
    public int RxToleranceHz { get; set; } = 50;

    [DisplayName("FT8 Decoding Cycles")]
    //[Description("")]
    [DefaultValue(1)]
    public int Ft8DecodingCycles { get; set; } = 1;

    [DisplayName("JT65 DT Range")]
    [DefaultValue(1)]
    public int Jt65DtRange { get; set; } = 1;

    [DisplayName("JT65 Top Decoding Frequency")]
    [DefaultValue(2700)]
    public int Jt65TopDecodingFreq { get; set; } = 2700;

    [DisplayName("JT65 Top Decoding Passes")]
    [DefaultValue(4)]
    public int Jt65DecodingPasses { get; set; } = 4;

    [DisplayName("Single Decode Attempts")]
    [DefaultValue(1)]
    public int SingleDecodeAttempts { get; set; } = 1;

    [DisplayName("FT8 Threads")]
    [DefaultValue(0)]
    public int Ft8Threads { get; set; } = 0;

    [DisplayName("FT4 Depth")]
    [DefaultValue(WsjtxDecodeDepth.Deep)]
    public WsjtxDecodeDepth FT4Depth { get; set; } = WsjtxDecodeDepth.Deep;

    [DisplayName("Frequency Mask Decoding")]
    [DefaultValue(false)]
    public bool FrequencyMaskDecoding { get; set; } = false;

    [DisplayName("My Call")]
    public string MyCall { get; set; } = string.Empty;

    [DisplayName("His Call")]
    public string HisCall { get; set; } = string.Empty;

    [DisplayName("His Grid")]
    public string HisGrid { get; set; } = string.Empty;
  }
}