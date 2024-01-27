using JTSkimmer;

namespace VE3NEA.PskrDxClusterService
{
  internal class DxClusterServer : TelnetServer
  {
    private readonly RecentCalls RecentCalls = new();
    readonly Context ctx;

    internal DxClusterServer(Context ctx) : base()
    {
      this.ctx = ctx;

      EnterNameMessage = "Please enter your callsign: ";
      InvalidNameMessage = "Invalid callsign";
      ServerNameMessage = "JTSkimmer DX Cluster Server";
      Port = 7309;
    }

    public override string GetPrompt(TelnetClientEntry client)
    {
      return EOL + client.UserName + " de " + ServerNameMessage + " >";
    }

    protected override bool IsValidName(string name)
    {
      // todo: check if the name is actually a valid callsign
      return base.IsValidName(name);
    }


    public void Send(IEnumerable<MessageInfo> messages)
    {
      RecentCalls.DeleteExpired(TimeSpan.FromMinutes(5));

      if (Active)
        foreach (var message in messages)
          if (RecentCalls.AddIfNew(message.Signature))
            SendTextToAll(MessageToDxClusterSpot(message));
    }

    private string MessageToDxClusterSpot(MessageInfo message)
    {
      string deCall = $"DX de {ctx.Settings.User.Call}:";
      string time = message.Message.Utc.ToString("HHmm");
      double khz = (message.Message.Frequency / 100) / 10d;
      string comment = $"{message.Message.Mode}  {message.Decode.Snr,3} dB  {message.Decode.OffsetFrequencyHz,4}";

      return $"{deCall,-16}{khz,8:0.0}  {message.Parse.DECallsign,-13}{comment,-29}{time}Z\r\n";
    }
  }
}
