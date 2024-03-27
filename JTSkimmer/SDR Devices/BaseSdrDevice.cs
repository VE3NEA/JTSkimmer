using System.Diagnostics;
using System.Runtime.InteropServices;
using MathNet.Numerics;
using Serilog;
using VE3NEA;

namespace JTSkimmer
{
  public abstract class BaseSdrDevice : IDisposable
  {
    private readonly System.Windows.Forms.Timer timer = new();
    private bool enabled;
    protected readonly GCHandle gcHandle;
    protected Complex32[] Data = Array.Empty<Complex32>();

    public readonly SdrInfo Info;
    public float[] Gains = new float[0];
    public uint GainIndex { get => Info.Settings.GainIndex; set => SetGainIndex(value); }
    public bool Enabled { get => enabled; set => SetEnabled(value); }
    public bool Running { get; private set; }
    public uint SamplingRate { get => Info.Settings.SamplingRate; }
    public uint Frequency { get => Info.Settings.CenterFrequency; set => SetOrStoreFrequency(value); }
        
    
    public event EventHandler<DataEventArgs<Complex32>>? DataAvailable;
    public event EventHandler? StateChanged;


    internal static BaseSdrDevice? CreateFromSettings(SdrInfo? sdrInfo)
    {
      if (sdrInfo == null) return null;

      BaseSdrDevice device;
      switch (sdrInfo.SdrType)
      {
        case SdrType.RtlSdr: return new RtlSdrDevice(sdrInfo);

        case SdrType.SdrPlayRSP1A:
        case SdrType.SdrPlayRSPDX:
        case SdrType.SdrPlayOther:
          return new SdrPlayDevice(sdrInfo);

        default: return new AirspyDevice(sdrInfo);
      }
    }

    public BaseSdrDevice(SdrInfo info) : base()
    {
      Info = info;

      gcHandle = GCHandle.Alloc(this);

      timer.Interval = 300;
      timer.Tick += Timer_Tick;
    }

    public string GetDesctiption()
    {
      return $"{Info.Name}\nSN: {Info.SerialNumber}";
    }

    private void SetEnabled(bool value)
    {
      if (value == enabled) return;
      enabled = value;

      if (value)
      {
        TryStart(true);
        if (!Running) 
          Debug.WriteLine($"Failed to start {Info.Name}, will keep trying");
      }
      else
      {
        Stop();
        Debug.WriteLine($"{GetType().Name} stopped");
        Running = false;
        StateChanged?.Invoke(this, EventArgs.Empty);
      }

      timer.Enabled = value;
    }

    private void TryStart(bool logErrors)
    {
      try
      {
        Start();

        Running = true; 
        Debug.WriteLine($"{GetType().Name} started");
        StateChanged?.Invoke(this, EventArgs.Empty);
      }
      catch (Exception ex) 
      {
        Stop();
        Running = false;
        if (logErrors) Log.Error(ex, $"Error starting {GetType().Name}");
      }
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
      if (Running && !IsRunning())
      {
        Running = false;
        Debug.WriteLine($"{GetType().Name} failed, restarting");
        StateChanged?.Invoke(this, EventArgs.Empty);
      }

      if (enabled && !Running) TryStart(false);
    }

    private void SetOrStoreFrequency(uint frequency)
    {
      Info.Settings.CenterFrequency = frequency;
      var correctedFrequency = frequency * (1 + Info.Settings.Ppm * 1e-6);

      if (IsRunning()) SetFrequency((uint)(correctedFrequency));
    }


    DataEventArgs<Complex32> Args = new();

    protected void OnDataAvailable()
    {
      Args.SetValues(Data);
      DataAvailable?.Invoke(this, Args);
    }




    //----------------------------------------------------------------------------------------------
    //                                     override these
    //----------------------------------------------------------------------------------------------
    public virtual void Dispose()
    {
      Debug.WriteLine($"{GetType().Name} dispose");
      Stop();
      if (gcHandle.IsAllocated) gcHandle.Free();
      timer.Dispose();
    }

    // throw an exception if cannot start
    protected abstract void Start();

    // never throw exceptions
    protected abstract void Stop();

    protected abstract bool IsRunning();
    
    protected abstract void SetFrequency(uint frequency);

    protected abstract void SetGainIndex(uint gainIndex);
  }
}
