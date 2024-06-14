using MathNet.Numerics;
using Serilog;
using VE3NEA;

namespace JTSkimmer
{
  public struct DecodedMessage
  {
    public int ReceiverIndex;
    public DateTime Utc;
    public uint Frequency;
    public string Mode;
    public string Message;

    public DecodedMessage(int index, DateTime receivedAt, uint frequency, string decoderMode, string msg)
    {
      ReceiverIndex = index;
      Utc = receivedAt;
      Frequency = frequency;
      Mode = decoderMode;
      Message = msg;
    }

    public override string ToString()
    {
      return
        $"#{ReceiverIndex:D2}  " +
        $"{Utc:HH:mm:ss}  " +
        $"{Frequency / 1000,9:N0}  " +
        $"{Mode,-8} " +
        $"{Message.Substring(7)}";
    }
  }

  public enum DecoderStatus { Started, Finished, Overload }
  public class DecoderEventArgs : DataEventArgs<DecodedMessage>
  {
    public DecoderStatus Status;
  }


  internal class Receiver : IDisposable
  {
    private static int NextIndex;
    private readonly DataEventArgs<float> Args = new();
    private readonly DecoderEventArgs DecoderEventArgs = new();

    internal int index = ++NextIndex;
    internal static SdrSettings? SdrSettings;
    internal ReceiverSettings Settings;
    internal Slicer? Slicer;
    internal Spectrum<float> Spectrum = new(512,256);
    internal TimeSlotBuffer<float>? TimeSlot;
    internal DecoderRunner? Runner;
    internal static DecodingSettings? DecodingSettings;
    internal bool SpectrumNeeded;
    internal bool Active { get => Slicer != null; }

    internal event EventHandler<DataEventArgs<float>>? DataAvailable;
    internal event EventHandler<DataEventArgs<float>>? SpectrumAvailable;
    internal event EventHandler<DecoderEventArgs>? DecoderEvent;


    // when settings != null, re-create receiver from previous session
    public Receiver(ReceiverSettings? settings = null)
    {
      Settings = settings ?? new();
      Settings.Frequency ??= SdrSettings?.CenterFrequency ?? SdrConst.DEFAULT_FREQUENCY;
      SetUpDecoding();
    }

    // create brand new receiver
    public Receiver(uint frequency)
    {
      Settings = new();
      Settings.Frequency = frequency;
    }

    // called on creation, or when SdrSettings change, or T/R changes
    public void EnableDisable(bool value = true)
    {
      Settings.Frequency ??= SdrSettings?.CenterFrequency;
      value = value && IsValidOffset();

      if (value == Active) return;
      Stop();
      if (value) Start();
    }

    private void Start()
    {
      Slicer = new Slicer(SdrSettings.Bandwidth, GetFrequencyOffset());
      Slicer.DataAvailable += Slicer_DataAvailable;
      Spectrum.SpectrumAvailable += Spectrum_SpectrumAvailable;
    }

    private void Stop()
    {
      if (!Active) return;

      Spectrum.SpectrumAvailable -= Spectrum_SpectrumAvailable;
      Slicer?.Dispose();
      Slicer = null;

      TimeSlot?.Reset();
    }

    public void Dispose()
    {
      Stop();
      DestroyDecoder();
    }

    internal void SetUpDecoding()
    {
      DestroyDecoder();

      if (Settings.DecoderMode == ReceiverSettings.NO_DECODING) return;

      if (Settings.DecoderType == DecoderType.WSJTX)
        SetUpWsjtxDecoding();
      else
        SetUpJtdxDecoding();

      if (Runner == null) return;

      Runner.index = index;
      Runner.MessagesDecoded += Runner_MessagesDecoded;
      TimeSlot = new TimeSlotBuffer<float>(Runner.SecondsPerSlot);
      TimeSlot.index = index;
    }

    private void SetUpWsjtxDecoding()
    {
      string? exePath = DecodingSettings?.Jt9ExePath;
      if (!File.Exists(exePath)) return;

      Runner = new WsjtxDecoderRunner(exePath, Settings);
    }

    private void SetUpJtdxDecoding()
    {
      string? exePath = DecodingSettings?.JtdxJt9ExePath;
      if (!File.Exists(exePath)) return;

      Runner = new JtdxDecoderRunner(exePath, Settings);
    }

    private void DestroyDecoder()
    {
      if (Runner == null) return;

      TimeSlot = null;
      Runner.Dispose();
      Runner = null;
    }

    public bool IsValidOffset()
    {
      if (!IsConfigured()) return false;

      float frequencyOffset = GetFrequencyOffset();
      return Math.Abs((float)frequencyOffset) <= SdrSettings.Bandwidth / 2;
    }

    private bool IsConfigured()
    {
      return Settings?.Frequency != null && SdrSettings?.CenterFrequency != null;
    }

    private float GetFrequencyOffset()
    {
      return (int)Settings.Frequency - (int)SdrSettings.CenterFrequency;
    }

    internal void SetNoiseFloor(float value) 
    { 
      //{!}
      //if (Slicer != null) Slicer.NoiseFloor = value; 
    }
    
    internal void StartProcessing(DataEventArgs<Complex32> e)
    {
      Slicer?.StartProcessing(e);
    }

    private void Slicer_DataAvailable(object? sender, DataEventArgs<float> e)
    {
      // play audio
      DataAvailable?.Invoke(this, e);

      // decode
      if (TimeSlot != null)
      {
        TimeSlot.AddSamples(e.Data, e.ReceivedAt);

        if (TimeSlot.IsComplete && Runner != null)
        {
          if (Runner.Busy)
          {
            Log.Warning($"RX #{index} {Settings.DecoderMode} at {Settings.Frequency/1000f} kHz BUSY");
            OnDecoderStatusChanged(DecoderStatus.Overload);
          }
          else
          {
            Args.Data = TimeSlot.SlotData;
            Args.ReceivedAt = TimeSlot.SlotStartTime;
            Runner.StartProcessing(Args);
            OnDecoderStatusChanged(DecoderStatus.Started);
          }
        }
      }

      // compute spectrum, update waterfall
      if (SpectrumNeeded) 
        Spectrum.Process(e.Data);
    }

    private void Runner_MessagesDecoded(object? sender, DataEventArgs<string> e)
    {
      if (Runner == null) return;
      OnDecoderStatusChanged(DecoderStatus.Finished, e);
    }

    private void OnDecoderStatusChanged(DecoderStatus status, DataEventArgs<string>? e = null)
    {
      DecoderEventArgs.Status = status;

      if (e != null)
      {
        DecoderEventArgs.Data = e.Data.Select(msg => new DecodedMessage(
          index,
          e.ReceivedAt,
          (uint)Settings.Frequency,
          Settings.DecoderMode,
          msg
          )).ToArray();

        DecoderEventArgs.ReceivedAt = e.ReceivedAt;
      }

      DecoderEvent?.Invoke(this, DecoderEventArgs);
    }

    internal void SetFrequency(int frequency)
    {
      Settings.Frequency = (uint)frequency;
      Slicer?.SetUpMixer(frequency - SdrSettings.CenterFrequency);
    }

    private void Spectrum_SpectrumAvailable(object? sender,   DataEventArgs<float> e)
    {
      SpectrumAvailable?.Invoke(this, e);
    }
  }
}
