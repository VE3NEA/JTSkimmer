using System.Runtime.InteropServices;

namespace JTSkimmer
{
  internal class JtdxSharedMemory : SharedMemory
  {
    public JtdxParamsBlock ParamsBlock = new();
    public Int32[] Samples = Array.Empty<Int32>();

    public JtdxSharedMemory(string? key = null) : base(key) { }

    public unsafe override void ReadParams()
    {
      Semaphore.WaitOne();
      try
      {
        using (var accessor = MemoryMappedFile.CreateViewAccessor())
        {
          byte* sharedMemoryBytes = null;
          accessor.SafeMemoryMappedViewHandle.AcquirePointer(ref sharedMemoryBytes);
          void* ptr = sharedMemoryBytes + Marshal.OffsetOf<JtdxDecData>("Params");
          ParamsBlock = (JtdxParamsBlock)(Marshal.PtrToStructure(new IntPtr(ptr), typeof(JtdxParamsBlock)));
        }
      }
      finally
      {
        Semaphore.Release();
      }
    }

    public override unsafe void Write()
    {
      ParamsBlock.kin = Samples.Length;

      Semaphore.WaitOne();
      try
      {
        using (var accessor = MemoryMappedFile.CreateViewAccessor())
          fixed (int* pSamples = Samples)
          {
            byte* sharedMemoryBytes = null;
            accessor.SafeMemoryMappedViewHandle.AcquirePointer(ref sharedMemoryBytes);

            // params
            void* ptr = sharedMemoryBytes + Marshal.OffsetOf<JtdxDecData>("Params");
            Marshal.StructureToPtr(ParamsBlock, new IntPtr(ptr), false);

            // samples
            int byteCount = Samples.Length * sizeof(int);
            ptr = sharedMemoryBytes + Marshal.OffsetOf<JtdxDecData>("d2");
            Buffer.MemoryCopy(pSamples, ptr, byteCount, byteCount);
          }
      }
      finally
      {
        Semaphore.Release();
      }
    }

    internal override void SetSamples(float[] samples)
    {
      int sampleCount = samples.Length;
      if (Samples.Length != sampleCount) Samples = new Int32[sampleCount];
      float scale = 100000 / Math.Max(-samples.Min(), samples.Max());
      for (int i = 0; i < sampleCount; i++) Samples[i] = (Int32)(samples[i] * scale);
    }


    protected override int GetSharedMemorySize()
    {
      return Marshal.SizeOf<JtdxDecData>();
    }
  }
}
