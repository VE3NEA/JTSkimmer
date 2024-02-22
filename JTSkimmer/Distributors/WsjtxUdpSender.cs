using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using JTSkimmer.Distributors;
using Serilog;
using VE3NEA;
using WsjtxUtils.WsjtxMessages;
using WsjtxUtils.WsjtxMessages.Messages;

namespace JTSkimmer
{
  internal class WsjtxUdpSender : IDisposable
  {
    private UdpClient? UdpClient;
    private readonly System.Timers.Timer Timer = new(15000);
    private byte[] HeartbeatBytes;
    private IPEndPoint EndPoint;
    private CancellationTokenSource CancellationTokenSource;

    public ushort Port = 7310;
    public string Host = "127.0.0.1";
    public event EventHandler<HighlightCallsignEventArgs>? HighlightCallsignReceived;

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
        EndPoint = new IPEndPoint(ipAddress, Port);
        SendHeartbeatMessage();

        CancellationTokenSource = new CancellationTokenSource();
        StartReceiveLoop(CancellationTokenSource.Token).DoNotAwait();

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
      CancellationTokenSource?.Cancel();
      UdpClient?.Dispose();
      UdpClient = null;
    }

    private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
      SendHeartbeatMessage();
    }

    private void SendHeartbeatMessage()
    {
      UdpClient?.Send(HeartbeatBytes, EndPoint);
    }

    public void SendDecodedMessages(IEnumerable<MessageInfo> messages)
    {
      if (!Active) return;

      // status datagram with receiver frequency
      var status = new WritableStatus();
      status.Id = Utils.GetAppName();
      status.DialFrequencyInHz = messages.First().Message.Frequency;
      UdpClient.Send(MessageToBytes(status), EndPoint);

      // decoded messages
      foreach (var message in messages)
        UdpClient.Send(MessageToBytes(message.Decode), EndPoint);
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

    public async Task StartReceiveLoop(CancellationToken token)
    {
      try
      {
        while (!token.IsCancellationRequested)
        {
          var result = await UdpClient.ReceiveAsync(token);

          var memory = result.Buffer.AsMemory();
          var reader = new WsjtxMessageReader(memory);
          MessageType messageType = reader.PeekMessageType();
          if (messageType != MessageType.HighlightCallsign) continue;

          var message = new ReadableHighlightCallsign();
          message.ReadMessage(reader);

          HighlightCallsignReceived?.Invoke(this, new HighlightCallsignEventArgs(message));
        }
      }
      catch (OperationCanceledException)
      {
        return;
      }
      catch (Exception e)
      {
        Log.Error(e, "Error in UDP listener");
      }
    }
  }

  internal class HighlightCallsignEventArgs
  {
    public string Callsign { get; private set; }
    public Color BackColor { get; private set; }
    public Color ForeColor { get; private set; }

    public HighlightCallsignEventArgs(ReadableHighlightCallsign message)
    {
      Callsign = message.Callsign;
      BackColor = message.BackColor;
      ForeColor = message.ForeColor;
    }
  }
}
