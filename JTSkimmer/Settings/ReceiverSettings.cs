namespace JTSkimmer
{
  internal class ReceiverSettings
  {
    public const string NO_DECODING = "No Decoding";

    public uint? Frequency;
    public bool AudioToSpeaker;
    public bool AudioToVac;

    public string DecoderMode { get; set; } = NO_DECODING;
    public DecoderType DecoderType = DecoderType.WSJTX;

    public WsjtxDecoderSettings WsjtxDecoder { get; set; } = new();
    public JtdxDecoderSettings JtdxDecoder { get; set; } = new();
  }
}
