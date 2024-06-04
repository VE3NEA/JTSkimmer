using JTSkimmer.Distributors;
using WsjtxUtils.WsjtxMessages.QsoParsing;

namespace JTSkimmer
{
  public class DisplayToken
  {
    internal string text;
    internal Brush bgBrush = Brushes.Transparent;
    internal Brush fgBrush;

    public DisplayToken(string text) { this.text = text; }
  }


  public class MessageInfo
  {
    internal DecodedMessage Message;
    internal WritableDecode Decode;
    internal WsjtxQso Parse;
    internal List<DisplayToken> Tokens = new();

    internal MessageInfo(DecodedMessage message)
    {
      Message = message;
      Decode = MessageDistributor.MessageToDecode(message.Message, message.Utc);
      Parse = WsjtxQsoParser.ParseDecode(Decode);
    }

    public string Signature { get => $"{Parse.DECallsign}{Message.Frequency / 1000}{Message.Mode}"; }
  }
}