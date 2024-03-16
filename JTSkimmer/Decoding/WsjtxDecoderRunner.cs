using VE3NEA;

namespace JTSkimmer
{
  internal class WsjtxDecoderRunner : DecoderRunner
  {
    public WsjtxDecoderSettings DecoderSettings;

    internal WsjtxDecoderRunner(string exePath, ReceiverSettings settings) : base(exePath, settings.DecoderMode)
    {
      InputAmplitude = Dsp.FromDb2(settings.WsjtxDecoder.InputAmplitude); // 0..1
      DecoderType = DecoderType.WSJTX;
      SharedMemory = new WsjtxSharedMemory();
      DecoderSettings = settings.WsjtxDecoder;
      Thread = StartThread();
    }

    protected override string BuildCommandline()
    {
      return $"-s {SharedMemory.Key} -a \"{DataDir}\" -t \"{DataDir}\"";
    }

    protected override void SettingsToSharedMemory(DateTime utc)
    {
      ((WsjtxSharedMemory)SharedMemory).ParamsBlock.Populate(Mode, DecoderSettings, utc);
      ((WsjtxSharedMemory)SharedMemory).Ipc[1] = WsjtxSharedMemory.IPC_FLAG_ON;
      ((WsjtxSharedMemory)SharedMemory).Ipc[2] = WsjtxSharedMemory.IPC_FLAG_OFF;
    }

    protected override void SignalDecodingEnd()
    {
      if (!Stopping)
        ((WsjtxSharedMemory)SharedMemory).WriteIpcFlags(null, WsjtxSharedMemory.IPC_FLAG_OFF, WsjtxSharedMemory.IPC_FLAG_ON);
    }

    protected override void SignalStopExe()
    {
      ((WsjtxSharedMemory)SharedMemory).WriteIpcFlags(null, WsjtxSharedMemory.IPC_MUST_EXIT, WsjtxSharedMemory.IPC_FLAG_ON);
    }

    internal static string? FindExe()
    {
      return DecoderRunner.FindExe("wsjtx ", "jt9.exe");      
    }
  }
}
