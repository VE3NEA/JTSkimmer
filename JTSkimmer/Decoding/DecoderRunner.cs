using System.Diagnostics;
using Microsoft.Win32;
using Serilog;
using VE3NEA;

namespace JTSkimmer
{
  internal enum DecoderType  {WSJTX, JTDX };

  internal abstract class DecoderRunner : ThreadedProcessor<float>
  {
    protected string DataDir;
    protected readonly DataEventArgs<string> Args = new();
    protected readonly List<string> DecodedMessages = new();
    protected bool Stopping;
    protected Thread Thread;
    protected SharedMemory SharedMemory;

    public float InputAmplitude = 1;
    public DecoderType DecoderType;
    public readonly string ExePath;
    public readonly Stopwatch StopWatch = new Stopwatch();
    public bool Busy { get; protected set; }
    public WsjtxMode Mode { get; protected set; }
    public int index;
    public float SecondsPerSlot { get => Math.Max(7.5f, Mode.ntrperiod); }// Params stores "7" for 7.5 s
    public event EventHandler<DataEventArgs<string>>? MessagesDecoded;


    internal DecoderRunner(string exePath, string modeName) : base()
    {
      ExePath = exePath;
      Mode = new WsjtxMode(modeName);
    }

    public override void Dispose()
    {
      base.Dispose();

      Stopping = true;
      SignalStopExe();
      SharedMemory?.Dispose();
    }

    protected Thread StartThread()
    {
      DataDir = Path.Combine(GetTempDir(), SharedMemory.Key);
      Directory.CreateDirectory(DataDir);
      CreateRunningFlag(true);

      var thread = new Thread(new ThreadStart(() =>
      {

        StartExe().WaitForExit();
        CreateRunningFlag(false);
        TryDeleteDirectory(DataDir);
      }));

      thread.Name = $"{GetType().Name} {SharedMemory.Key}";
      thread.Start();
      return thread;
    }

    protected Process StartExe()
    {
      string Arguments = BuildCommandline();

    var process = new Process
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = ExePath,
          Arguments = Arguments,
          UseShellExecute = false,
          RedirectStandardOutput = true,
          RedirectStandardError = true,
          CreateNoWindow = true
        }
      };

      process.OutputDataReceived += Process_OutputDataReceived;
      process.ErrorDataReceived += Process_ErrorDataReceived;
      process.Start();
      process.PriorityClass = ProcessPriorityClass.BelowNormal;
      process.BeginOutputReadLine();
      process.BeginErrorReadLine();

      return process;
    }

    protected override void Process(DataEventArgs<float> args)
    {
      if (Mode.DisplayName == ReceiverSettings.NO_DECODING) return;

      Args.ReceivedAt = args.ReceivedAt;
      Busy = true;
      StopWatch.Restart();

      SettingsToSharedMemory(args.ReceivedAt);
      SharedMemory.SetSamples(args.Data, InputAmplitude);
      SharedMemory.Write();
    }

    protected void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
      if (string.IsNullOrEmpty(e.Data)) return;

      Log.Error($"STDERR in {Mode} runner: '{e.Data}");
    }

    protected void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
      if (e.Data == null) return;

      if (e.Data.Contains("Finished>"))
      {
        // confirm end of decoding
        SignalDecodingEnd();
        Busy = false;
        StopWatch.Stop();

        // notify
        Args.Data = DecodedMessages.ToArray();
        DecodedMessages.Clear();
        MessagesDecoded?.Invoke(this, Args);
      }

      else if (e.Data.StartsWith("ALLCALL") || e.Data.Contains("low rms"))
        return;

      else
        DecodedMessages.Add(e.Data.Replace('\0', ' ').Trim());
    }


    StreamWriter? RunningFlag;

    private void CreateRunningFlag(bool value)
    {
      if (value)
        try
        {
          RunningFlag = File.CreateText(Path.Combine(DataDir, ".running"));
        }
        catch (Exception e)
        {
          Log.Error(e, "Error creating .running flag");
        }
      else try
        {
          RunningFlag?.Close();
          RunningFlag = null;
        }
        catch (Exception e)
        {
          Log.Error(e, "Error removing .running flag");
        }
    }

    protected static string GetTempDir()
    {
      return Path.Combine(Path.GetTempPath(), "JTSkimmerDecoders");
    }

    // delete any orphan folders 
    public static void TryDeleteAllTempFolders()
    {
      string TempDir = GetTempDir();
      if (TempDir == null || !Directory.Exists(TempDir)) return;
      var dirs = Directory.EnumerateDirectories(TempDir);
      foreach (string dir in dirs) TryDeleteDirectory(dir);
    }

    public static void TryDeleteDirectory(string directory)
    {
      try
      {
        if (Directory.Exists(directory)) Directory.Delete(directory, true);
      }
      catch { }
    }

    internal static string? FindExe(string label, string exeName)
    {
      // read all paths to the wsjtx.exe file from the Uninstall section in the Registry
      string UninstallKeyPath = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
      var uninstallKey = Registry.LocalMachine.OpenSubKey(UninstallKeyPath);
      if (uninstallKey == null) return null;
      var subkeys = uninstallKey.GetSubKeyNames().Where(k => k.StartsWith(label));

      var paths = subkeys
        .Select(k => uninstallKey.OpenSubKey(k)?.GetValue("DisplayIcon"))
        .Where(value => value != null) // This filters out null values  
        .Select(nonNullValue => nonNullValue.ToString());

      // replace "wsjtx.exe" with "jt9.exe"
      paths = paths.Select(p => Path.Combine(Path.GetDirectoryName(p), exeName));

      // reject non-existing files
      paths = paths.Distinct().Where(File.Exists);
      
      //take the path to the latest exe
      return paths.OrderBy(File.GetCreationTime).LastOrDefault();
    }

    protected abstract void SignalStopExe();
    protected abstract void SignalDecodingEnd();
    protected abstract string BuildCommandline();
    protected abstract void SettingsToSharedMemory(DateTime utc);
  }
}
