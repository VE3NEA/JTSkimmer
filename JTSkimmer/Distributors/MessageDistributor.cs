using JTSkimmer.Distributors;
using VE3NEA;
using VE3NEA.PskrDxClusterService;

namespace JTSkimmer
{

  internal class MessageDistributor : ThreadedProcessor<DecodedMessage>
  {
    private readonly Context ctx;
    private Dictionary<string, DateTime> Cache = new();

    public readonly DxClusterServer DxClusterServer;
    public readonly WsjtxUdpSender WsjtxUdpSender = new();
    public readonly PskReporterUdpSender PskReporterUdpSender = new();


    public MessageDistributor(Context ctx)
    {
      this.ctx = ctx;
      DxClusterServer = new(ctx);
      processingThread.Priority = ThreadPriority.BelowNormal;
  }

    public void ApplySettings()
    {
      ApplyDxClusterSettings();
      ApplyUdpSenderSettings();
      ApplyPskReporterSettings();
    }

    public void ApplyPskReporterSettings()
    {
      PskReporterUdpSender.Call = ctx.Settings.User.Call;
      PskReporterUdpSender.Square = ctx.Settings.User.Square;
      PskReporterUdpSender.Antenna = ctx.Settings.Distributor.PskReporter.Antenna;
      PskReporterUdpSender.Enabled = ctx.Settings.Distributor.PskReporter.Enabled;
    }

    public void ApplyUdpSenderSettings()
    {
      WsjtxUdpSender.Port = ctx.Settings.Distributor.UdpSender.Port;
      WsjtxUdpSender.Host = ctx.Settings.Distributor.UdpSender.Host;
      WsjtxUdpSender.SetEnabled(ctx.Settings.Distributor.UdpSender.Enabled);
    }

    public void ApplyDxClusterSettings()
    {
      DxClusterServer.Stop();
      DxClusterServer.Port = ctx.Settings.Distributor.DxCluster.Port;
      if (ctx.Settings.Distributor.DxCluster.Enabled) DxClusterServer.Start();
    }

    internal void Receiver_DecoderEvent(object? sender, DecoderEventArgs e)
    {
      if (e.Status == DecoderStatus.Finished && e.Data.Any()) StartProcessing(e);
    }

    protected override void Process(DataEventArgs<DecodedMessage> args)
    {
      var messages = args.Data.Select(m => new MessageInfo(m)).ToArray();

      // show on screen
      ctx.MessagesPanel?.AddMessages(messages);

      // save to file
      SaveToFile(args);

      // send all decodes as wsjtx udp packets
      WsjtxUdpSender.SendDecodedMessages(messages);

      // skip bad decodes
      messages = messages.Where(m => 
        !m.Decode.LowConfidence
        && !string.IsNullOrEmpty(m.Parse.DECallsign) 
        && m.Parse.DECallsign != ctx.Settings.User.Call
        ).ToArray();

      // send to pskreporter
      PskReporterUdpSender.AddMessages(messages);

      // send to dx cluster
      DxClusterServer.Send(messages);
    }

    private void SaveToFile(DataEventArgs<DecodedMessage> args)
    {
      if (!ctx.Settings.Distributor.ArchiveToFile) return;

      string archiveFolder = Path.Combine(Utils.GetUserDataFolder(), "Messages");
      Directory.CreateDirectory(archiveFolder);
      string filePath = Path.Combine(archiveFolder, args.ReceivedAt.ToString("yyyy-MM-dd") + ".txt");

      string text = string.Join('\n', args.Data.Select(msg => msg.ToString()).ToArray());

      using (StreamWriter writer = File.AppendText(filePath)) writer.WriteLine(text);
    }

    public static WritableDecode MessageToDecode(string message, DateTime receivedAt)
    {
      var decode = new WritableDecode();
      decode.Id = Utils.GetAppName();
      decode.New = true;
      decode.Time = (uint)((DateTimeOffset)receivedAt).ToUnixTimeSeconds();
      decode.Snr = int.Parse(message.Substring(7, 3));
      decode.OffsetTimeSeconds = float.Parse(message.Substring(11, 4));
      decode.OffsetFrequencyHz = uint.Parse(message.Substring(16, 4));
      decode.Mode = message[21].ToString();
      if (message.Length > 24) decode.Message = message.Substring(24, message.Length-24).Trim();
      decode.LowConfidence = message.Length > 42 && message[42] == '?';
      decode.OffAir = false;
      
      return decode;
    }

    public override void Dispose()
    {
      base.Dispose();
      WsjtxUdpSender.Dispose();
      DxClusterServer.Dispose();
      PskReporterUdpSender.Dispose();
    }

    internal string GetStatusString()
    {
      string udpSenderStatus = "Disabled";
      if (WsjtxUdpSender.Active) udpSenderStatus = "Running";
      else if (ctx.Settings.Distributor.UdpSender.Enabled) udpSenderStatus = $"Error: {WsjtxUdpSender.LastError}";

      string dxClusterStatus = "Disabled";
      if (DxClusterServer.Active) dxClusterStatus = "Running";
      else if (ctx.Settings.Distributor.DxCluster.Enabled) dxClusterStatus = $"Error: {DxClusterServer.LastError}";

      string pskReporterStatus = "Disabled";
      if (PskReporterUdpSender.Active) pskReporterStatus = "Running";
      else if (ctx.Settings.Distributor.PskReporter.Enabled) pskReporterStatus = $"Error: {PskReporterUdpSender.LastError}";


      return $"Network:\n" +
        $"  WSJTX UDP:      {udpSenderStatus}\n" +
        $"  DX Cluster:        {dxClusterStatus}\n" +
        $"  PSK Reporter:    {pskReporterStatus}";
    }
  }
}