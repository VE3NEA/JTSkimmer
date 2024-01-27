using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using Serilog;
using WsjtxUtils.WsjtxMessages;

namespace JTSkimmer
{
  //----------------------------------------------------------------------------------------------
  //                               PskReporterUdpSender
  //----------------------------------------------------------------------------------------------
  internal class PskReporterUdpSender : IDisposable
  {
    private const int FLASH_INTERVAL_MS = 120 * 1000;
    private const int MAX_DATAGRAM_SIZE = 10000;
    private const int REQUIRED_DESCRIPTORS_COUNT = 3;

    private Int32 SequenceNumber;
    private Int32 Identifier = new Random().Next();
    public string Call;
    public string Square;
    public string Antenna;
    private byte[] Buffer = new byte[MAX_DATAGRAM_SIZE + 256];
    private int SentDescriptorsCount = 3;
    private DateTime TimeToSendDescriptors = DateTime.MinValue;
    private ConcurrentBag<MessageInfo> Messages = new();
    private PskReporterMessageWriter Writer;
    private RecentCalls RecentCalls = new();


    private System.Timers.Timer Timer = new(FLASH_INTERVAL_MS);
    private UdpClient? UdpClient;
    private bool enabled;

    public bool Enabled { get => enabled; set => SetEnabled(value); }
    public bool Active { get => UdpClient != null; }
    public string LastError { get; protected set; }

    public PskReporterUdpSender()
    {
      Writer = new(Buffer);
      Timer.Elapsed += Timer_Elapsed;
    }

    public void Dispose()
    {
      Send();
      Stop();
      Timer.Dispose();
    }

    private void SetEnabled(bool value)
    {
      if (value) Start(); else Stop();
    }

    // prod: report.pskreporter.info:4739
    // test: report.pskreporter.info:14739
    private void Start()
    {
      enabled = true;

      try
      {
        Stop();
        UdpClient = new("report.pskreporter.info", 4739);
        Send();
        Timer.Start();
      }
      catch (Exception e)
      {
        LastError = e.Message;
        Log.Error(e, "PSKReporter start failed");
      }
    }

    private void Stop()
    {
      Timer.Stop();
      UdpClient?.Dispose();
      UdpClient = null;
      Messages.Clear();

      enabled = false;
    }

    public void AddMessages(IEnumerable<MessageInfo> messages)
    {
      if (Active)
        foreach (var message in messages)
          Messages.Add(message);
    }

    private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
      Send();
    }

    private void Send()
    {
      RecentCalls.DeleteExpired(TimeSpan.FromMinutes(5));

      if (!Active) return;

      do
      {
        var datagram = BuildDatagram();
        UdpClient.Send(datagram);
      }
      while (Messages.Any());
    }

    private static uint UnixTime(DateTime? utc = null)
    {
      utc ??= DateTime.UtcNow;
      return (uint)((DateTimeOffset)utc).ToUnixTimeSeconds();
    }




    //----------------------------------------------------------------------------------------------
    //                                    datagram
    //----------------------------------------------------------------------------------------------
    public ReadOnlySpan<byte> BuildDatagram()
    {
      Writer.Position = 0;

      WriteHeader();
      WriteDescriptors();
      WriteReceiverInfo();
      WriteSpots();

      Writer.WriteSizeTo(sizeof(short));

      return new ReadOnlySpan<byte>(Buffer, 0, Writer.Position);
    }

    private void WriteHeader()
    {
      Writer.WriteInt16(0x000a);
      Writer.WriteInt16(0); // total datagram size
      Writer.WriteUInt32(UnixTime());
      Writer.WriteInt32(SequenceNumber++);
      Writer.WriteInt32(Identifier);
    }

    //  receiverCallsign, receiverLocator, decodingSoftware, anntennaInformation
    private readonly byte[] ReceiverFormatDescriptor =  {
      0x00, 0x03, 0x00, 0x2C, 0x99, 0x92, 0x00, 0x04, 0x00, 0x00,
      0x80, 0x02, 0xFF, 0xFF, 0x00, 0x00, 0x76, 0x8F,
      0x80, 0x04, 0xFF, 0xFF, 0x00, 0x00, 0x76, 0x8F,
      0x80, 0x08, 0xFF, 0xFF, 0x00, 0x00, 0x76, 0x8F,
      0x80, 0x09, 0xFF, 0xFF, 0x00, 0x00, 0x76, 0x8F,
      0x00, 0x00 };

    //senderCallsign, frequency, sNR, iMD, mode, informationSource, senderLocator, flowStartSeconds 
    private readonly byte[] SenderFormatDescriptor =  {
      0x00, 0x02, 0x00, 0x44, 0x99, 0x93, 0x00, 0x08,
      0x80, 0x01, 0xFF, 0xFF, 0x00, 0x00, 0x76, 0x8F,
      0x80, 0x05, 0x00, 0x04, 0x00, 0x00, 0x76, 0x8F,
      0x80, 0x06, 0x00, 0x01, 0x00, 0x00, 0x76, 0x8F,
      0x80, 0x07, 0x00, 0x01, 0x00, 0x00, 0x76, 0x8F,
      0x80, 0x0A, 0xFF, 0xFF, 0x00, 0x00, 0x76, 0x8F,
      0x80, 0x0B, 0x00, 0x01, 0x00, 0x00, 0x76, 0x8F,
      0x80, 0x03, 0xFF, 0xFF, 0x00, 0x00, 0x76, 0x8F,
      0x00, 0x96, 0x00, 0x04 };

    // include format descriptors in the first 3 packets and repeat every hour
    private void WriteDescriptors()
    {
      if (DateTime.UtcNow > TimeToSendDescriptors)
      {
        SentDescriptorsCount = 0;
        TimeToSendDescriptors = DateTime.UtcNow.AddHours(1);
      }

      if (SentDescriptorsCount < REQUIRED_DESCRIPTORS_COUNT)
      {
        Writer.WriteBytes(ReceiverFormatDescriptor);
        Writer.WriteBytes(SenderFormatDescriptor);
        SentDescriptorsCount++;
      }
    }

    private void WriteReceiverInfo()
    {
      string version = $"{AppDomain.CurrentDomain.FriendlyName} {Application.ProductVersion}";

      Writer.WriteUInt16(0x9992);
      int sizePosition = Writer.Position;
      Writer.WriteInt16(0); // receiver size
      Writer.WriteStringWithSizeByte(Call);
      Writer.WriteStringWithSizeByte(Square);
      Writer.WriteStringWithSizeByte(version);
      Writer.WriteStringWithSizeByte(Antenna);
      Writer.Pad();
      Writer.WriteSizeTo(sizePosition);
    }

    private void WriteSpots()
    {
      if (!Messages.Any()) return;

      Writer.WriteUInt16(0x9993);
      int sizePosition = Writer.Position;
      Writer.WriteInt16(0); // senders size

      while (Messages.Any() && Writer.Position < MAX_DATAGRAM_SIZE)
      {
        Messages.TryTake(out var message);
        if (!RecentCalls.AddIfNew(message.Signature)) continue;

        Writer.WriteStringWithSizeByte(message.Parse.DECallsign);
        Writer.WriteUInt32(message.Message.Frequency);
        Writer.WriteByte((byte)(message.Decode.Snr + 256));
        Writer.WriteByte(0);
        Writer.WriteStringWithSizeByte(message.Message.Mode);
        Writer.WriteByte(0x01);
        Writer.WriteStringWithSizeByte(message.Parse.GridSquare);
        Writer.WriteUInt32(UnixTime(message.Message.Utc));
      }

      Writer.Pad();
      Writer.WriteSizeTo(sizePosition);
    }
  }



  //----------------------------------------------------------------------------------------------
  //                              PskReporterMessageWriter
  //----------------------------------------------------------------------------------------------
  internal class PskReporterMessageWriter : WsjtxMessageWriter
  {
    public PskReporterMessageWriter(Memory<byte> source) : base(source) { }

    public void Pad()
    {
      while ((Position & 3) != 0) WriteByte(0);
    }

    public void WriteBytes(byte[] value)
    {
      foreach (byte b in value) WriteByte(b);
    }

    public void WriteStringWithSizeByte(string value)
    {
      byte length = (byte)(string.IsNullOrEmpty(value) ? 0 : Encoding.UTF8.GetByteCount(value));
      WriteByte(length);
      if (length == 0) return;

      if (!MemoryMarshal.TryGetArray((ReadOnlyMemory<byte>)buffer.Slice(Position), out ArraySegment<byte> segment) && segment.Array != null)
        throw new InsufficientMemoryException("Unable to allocate the array from the underlying buffer.");

      Encoding.UTF8.GetBytes(value, 0, value.Length, segment.Array, segment.Offset);
      Position += length;
    }

    internal void WriteSizeTo(int sizePosition)
    {
      short size = (short)(Position - sizePosition + sizeof(short));

      int oldPosition = Position;
      Position = sizePosition;
      WriteInt16(size);
      Position = oldPosition;
    }
  }
}
