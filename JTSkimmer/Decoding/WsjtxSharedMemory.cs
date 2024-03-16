using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace JTSkimmer
{
  [SupportedOSPlatform("windows")]
  public class WsjtxSharedMemory : SharedMemory
  {
    internal const int IPC_FLAG_OFF = 0;
    internal const int IPC_FLAG_ON = 1;
    internal const int IPC_MUST_EXIT = 999;


    public int[] Ipc = new int[3];
    public WsjtxParamsBlock ParamsBlock;
    public Int16[] Samples = Array.Empty<Int16>();


    public WsjtxSharedMemory(string? key = null) : base(key)
    {
      ParamsBlock = new WsjtxParamsBlock();
    }

    public override unsafe void Write()
    {
      Semaphore.WaitOne();
      try
      {
        using (var accessor = MemoryMappedFile.CreateViewAccessor())
          fixed (short* pSamples = Samples)
          fixed (int* pIpc = Ipc)
          {
            byte* sharedMemoryBytes = null;
            accessor.SafeMemoryMappedViewHandle.AcquirePointer(ref sharedMemoryBytes);

            // params
            void* ptr = sharedMemoryBytes + Marshal.OffsetOf<WsjtxDecData>("Params");
            Marshal.StructureToPtr(ParamsBlock, new IntPtr(ptr), false);

            // samples
            int byteCount = Samples.Length * sizeof(short);
            ptr = sharedMemoryBytes + Marshal.OffsetOf<WsjtxDecData>("id2");
            Buffer.MemoryCopy(pSamples, ptr, byteCount, byteCount);

            // flags
            byteCount = Ipc.Length * sizeof(int);
            ptr = sharedMemoryBytes + Marshal.OffsetOf<WsjtxDecData>("ipc");
            Buffer.MemoryCopy(pIpc, ptr, byteCount, byteCount);
            accessor.SafeMemoryMappedViewHandle.ReleasePointer();
          }
      }
      finally
      {
        Semaphore.Release();
      }
    }

    public unsafe override void ReadParams()
    {
      Semaphore.WaitOne();
      try
      {
        using (var accessor = MemoryMappedFile.CreateViewAccessor())
        {
          byte* sharedMemoryBytes = null;
          accessor.SafeMemoryMappedViewHandle.AcquirePointer(ref sharedMemoryBytes);
          void* ptr = sharedMemoryBytes + Marshal.OffsetOf<WsjtxDecData>("Params");
          ParamsBlock = (WsjtxParamsBlock)(Marshal.PtrToStructure(new IntPtr(ptr), typeof(WsjtxParamsBlock)));
        }
      }
      finally
      {
        Semaphore.Release();
      }
    }

    public int ReadIpcFlag(int index)
    {
      Semaphore.WaitOne();
      var accessor = MemoryMappedFile.CreateViewAccessor();
      accessor.ReadArray(0, Ipc, 0, Ipc.Length);
      Semaphore.Release();

      // fortran indices are 1-based
      return Ipc[index - 1];
    }

    public void WriteIpcFlags(int? ipc1, int? ipc2, int? ipc3)
    {
      if (ipc1 != null) Ipc[0] = (int)ipc1;
      if (ipc2 != null) Ipc[1] = (int)ipc2;
      if (ipc3 != null) Ipc[2] = (int)ipc3;

      Semaphore.WaitOne();
      using (var accessor = MemoryMappedFile.CreateViewAccessor())
        accessor.WriteArray(0, Ipc, 0, Ipc.Length);
      Semaphore.Release();
    }

    internal override void SetSamples(float[] samples, float amplitude)
    {
      int sampleCount = samples.Length;
      if (Samples.Length != sampleCount) Samples = new short[sampleCount];
      float scale = amplitude * short.MinValue / Math.Max(-samples.Min(), samples.Max());
      for (int i = 0; i < sampleCount; i++) Samples[i] = (short)(samples[i] * scale);
    }

    protected override int GetSharedMemorySize()
    {
      return Marshal.SizeOf<WsjtxDecData>();
    }
  }
}