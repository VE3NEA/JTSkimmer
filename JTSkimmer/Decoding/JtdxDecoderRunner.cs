using VE3NEA;
using Serilog;

namespace JTSkimmer
{
  internal class JtdxDecoderRunner : DecoderRunner
  {
    public JtdxDecoderSettings DecoderSettings;

    public JtdxDecoderRunner(string exePath, ReceiverSettings settings) : base(exePath, settings.DecoderMode)
    {
      InputAmplitude = Dsp.FromDb2(settings.JtdxDecoder.InputAmplitude); // 0..1
      DecoderType = DecoderType.JTDX;
      SharedMemory = new JtdxSharedMemory();
      DecoderSettings = settings.JtdxDecoder;

      Thread = StartThread();
    }

    protected override string BuildCommandline()
    {
      string dataSourceDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "JTDX");
      string exeDir = Path.GetDirectoryName(ExePath);
      string refDir = Path.Combine(Path.GetDirectoryName(exeDir), "share", "jtdx");

      // copy data from JTDX data folder
      string srcPath = Path.Combine(dataSourceDir, refDir, "CALL3.TXT");
      string dstPath = Path.Combine(DataDir, "CALL3.TXT");
      if (File.Exists(srcPath)) File.Copy(srcPath, dstPath);

      // lock file in the temp folder
      CreateFlagFile(".lock");

      srcPath = Path.Combine(dataSourceDir, "jt9_wisdom.dat");
      dstPath = Path.Combine(DataDir, "jt9_wisdom.dat");
      if (File.Exists(srcPath)) File.Copy(srcPath, dstPath);

      return $"-s {SharedMemory.Key} -w 1 -m {Environment.ProcessorCount-1} -e \"{exeDir}\" -a \"{DataDir}\" -t \"{DataDir}\" -r \"{refDir}\"";
    }

    protected override void Process(DataEventArgs<float> args)
    {
      base.Process(args);
      DeleteFlagFile(".lock");
    }

    protected override void SettingsToSharedMemory(DateTime utc)
    {
      ((JtdxSharedMemory)SharedMemory).ParamsBlock.Populate(Mode, DecoderSettings, utc);
    }

    protected override void SignalDecodingEnd()
    {
      CreateFlagFile(".lock");
      if (Stopping)
      {
        Thread.Sleep(200);
        DeleteFlagFile(".lock");
      }
    }

    protected override void SignalStopExe()
    {
      CreateFlagFile(".quit");
      DeleteFlagFile(".lock");
    }

    internal static string? FindExe()
    {
      return DecoderRunner.FindExe("jtdx ", "jtdxjt9.exe");
    }




    //--------------------------------------------------------------------------------------------------------------
    //                                          start / stop
    //--------------------------------------------------------------------------------------------------------------
    private void CreateFlagFile(string fileName)
    {
      try
      {
        File.CreateText(Path.Combine(DataDir, fileName)).Close();
      }
      catch (Exception e)
      {
        Log.Error(e, $"Error creating file {fileName}");
      }
    }

    private void DeleteFlagFile(string fileName)
    {
      try
      {
        File.Delete(Path.Combine(DataDir, fileName));
      }
      catch (Exception e)
      {
        Log.Error(e, $"Error deleting file {fileName}");
      }
    }
  }
}
