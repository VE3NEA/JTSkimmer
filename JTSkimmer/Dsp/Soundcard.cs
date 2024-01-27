using CSCore.CoreAudioAPI;
using CSCore.SoundOut;

namespace JTSkimmer
{
  internal class Soundcard : IDisposable
  {
    private WaveSource waveSource;
    private MMDevice? mmDevice;
    private WasapiOut? wasapiOut;
    private bool enabled;
    private float volume;
    public event EventHandler? StateChanged;


    public bool Enabled { get => enabled; set => SetEnabled(value); }
    public float Volume { get => Volume; set => SetVolume(value); }


    public Soundcard(string? audioDeviceId = null)
    {
      waveSource = new WaveSource();
      SetDeviceId(audioDeviceId);
    }

    public void SetDeviceId(string? deviceId)
    {
      if (deviceId == mmDevice?.DeviceID) return;

      bool wasEnabled = enabled;
      Enabled = false;
      if (deviceId == null) return;

      using (var deviceEnumerator = new MMDeviceEnumerator())
        try { mmDevice = deviceEnumerator.GetDevice(deviceId); } catch { return; }

      Enabled = wasEnabled;
    }


    private void SetEnabled(bool value)
    {
      if (value == Enabled) return;
      if (value) Start(); else Stop();
    }

    public bool IsPlaying()
    {
      return wasapiOut?.PlaybackState == PlaybackState.Playing;
    }

    private void Start()
    {
      if (mmDevice == null) return;

      waveSource.ClearBuffer();

      try
      {
        wasapiOut = new WasapiOut(false, AudioClientShareMode.Shared, 100);
        wasapiOut.Device = mmDevice;
        wasapiOut.Initialize(waveSource);
        wasapiOut.Volume = volume;
        wasapiOut.Stopped += WasapiOut_Stopped;
        wasapiOut.Play();

        enabled = true;
        StateChanged?.Invoke(this, EventArgs.Empty);
      }
      catch
      {
        Stop();
      }
    }

    private void Stop()
    {
      wasapiOut?.Stop();
      wasapiOut?.Dispose();
      wasapiOut = null;
      enabled = false;

      StateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SetVolume(float value)
    {
      volume = value;
      if (wasapiOut != null) wasapiOut.Volume = value;
    }

    private void WasapiOut_Stopped(object? sender, PlaybackStoppedEventArgs e)
    {
      StateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Dispose()
    {
      Enabled = false;
    }

    public void AddSamples(float[] samples)
    {
      waveSource.AddSamples(samples);
    }

    public static string? GetDefaultSoundcardId()
    {
      using (var deviceEnumerator = new MMDeviceEnumerator())
        return deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia)?.DeviceID;
    }

    public static string? GetFirstVacId()
    {
      using (var deviceEnumerator = new MMDeviceEnumerator())
      using (var deviceCollection = deviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
        return deviceCollection.FirstOrDefault(d => d.FriendlyName.Contains("Virtual"))?.DeviceID;
    }

    internal string GetDisplayName()
    {
      return mmDevice?.FriendlyName ?? "None Selected";
    }
  }
}