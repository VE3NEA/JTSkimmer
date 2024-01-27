using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using JTSkimmer.Distributors;
using Serilog;
using WsjtxUtils.WsjtxMessages;
using WsjtxUtils.WsjtxMessages.Messages;

namespace JTSkimmer
{
  internal class WsjtxUdpSender : IDisposable
  {
    private UdpClient? UdpClient;
    private readonly System.Timers.Timer Timer = new(15000);
    private byte[] HeartbeatBytes;

    public ushort Port = 7310;
    public string Host = "127.0.0.1";
    public bool Active { get => UdpClient != null; }
    public string LastError { get; protected set; }

    public WsjtxUdpSender()
    {
      HeartbeatBytes = MessageToBytes(new Heartbeat(Utils.GetAppName(), "1.0", "0.0"));
      Timer.Elapsed += Timer_Elapsed;
    }

    public void SetEnabled(bool value)
    {
      if (value) Start(); else Stop();
    }

    public void Start()
    {
      Stop();
      UdpClient = new();
      try
      {
        var ipAddress = IPAddress.Parse(Host);
        if (Utils.IsAddressMulticast(ipAddress)) UdpClient.JoinMulticastGroup(ipAddress);
        UdpClient.Connect(new IPEndPoint(ipAddress, Port));
        SendHeartbeatMessage();
        Timer.Start();
      }
      catch (Exception e) 
      {
        Stop();
        LastError = e.Message;
        Log.Error(e, "Error starting WsjtxUdpSender");
      }
    }

    public void Stop()
    {
      Timer.Stop();
      UdpClient?.Dispose();
      UdpClient = null;
    }

    private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
      SendHeartbeatMessage();
    }

    private void SendHeartbeatMessage()
    {
      UdpClient?.Send(HeartbeatBytes);
    }

    public void SendDecodedMessages(IEnumerable<MessageInfo> messages)
    {
      if (!Active) return;

      // status datagram with receiver frequency
      var status = new WritableStatus();
      status.Id = Utils.GetAppName();
      status.DialFrequencyInHz = messages.First().Message.Frequency;
      UdpClient.Send(MessageToBytes(status));

      // decoded messages
      foreach (var message in messages)
        UdpClient.Send(MessageToBytes(message.Decode));
    }

    private byte[] MessageToBytes(IWsjtxDirectionIn message)
    {
      byte[] bytes = new byte[1000];
      Memory<byte> buffer = new(bytes);
      WsjtxMessageWriter writer = new(buffer);
      message.WriteMessage(writer);
      Array.Resize(ref bytes, writer.Position);
      return bytes;
    }

    public void Dispose()
    {
      Stop();
    }
  }
}