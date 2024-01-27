using System.Runtime.InteropServices;
using MathNet.Numerics;
using VE3NEA;

namespace JTSkimmer
{
  internal class Fft<T> : IDisposable
  {
    private readonly int Size;
    private IntPtr InputPtr;
    private IntPtr OutputPtr;
    private IntPtr Plan;

    public T[] InputData;
    public Complex32[] OutputData;


    public Fft(int size, NativeFftw.FftwFlags flags = NativeFftw.FftwFlags.Patient)
    {
      Size = size;

      // managed buffers visible to the calling code
      if (typeof(T) == typeof(float))
      {
        InputData = new T[size * 2];
        OutputData = new Complex32[size + 1];
      }
      else if (typeof(T) == typeof(Complex32))
      {
        InputData = new T[size];
        OutputData = new Complex32[size];
      }
      else throw new ArgumentException($"Invalid FFT data type: {typeof(T)}");


      // native buffers
      int inputSampleSize = Marshal.SizeOf(typeof(T));
      int outputSampleSize = Marshal.SizeOf(typeof(Complex32));
      InputPtr = NativeFftw.malloc(InputData.Length * inputSampleSize);
      OutputPtr = NativeFftw.malloc(OutputData.Length * outputSampleSize);


      // FFT plan
      NativeFftw.make_planner_thread_safe();
      Plan = typeof(T) == typeof(float) ?
        NativeFftw.dft_r2c_1d(InputData.Length, InputPtr, OutputPtr, flags)
        :
        NativeFftw.dft_1d(InputData.Length, InputPtr, OutputPtr, NativeFftw.FftwDirection.Forward, flags);
    }

    public void Dispose()
    {
      if (Plan != IntPtr.Zero) NativeFftw.destroy_plan(Plan);
      if (InputPtr != IntPtr.Zero) NativeFftw.free(InputPtr);
      if (OutputPtr != IntPtr.Zero) NativeFftw.free(OutputPtr);

      Plan = IntPtr.Zero;
      InputPtr = IntPtr.Zero;
      OutputPtr = IntPtr.Zero;
    }

    public unsafe void Execute()
    {
      var floatSpan = new Span<T>((void*)InputPtr, InputData.Length);
      InputData.CopyTo(floatSpan);

      NativeFftw.execute(Plan);

      var complexSpan = new Span<Complex32>((void*)OutputPtr, OutputData.Length);
      complexSpan.CopyTo(OutputData);
    }


    private static string? WisdomPath;

    public static void LoadWisdom(string path)
    {
      WisdomPath = path;
      if (File.Exists(path)) NativeFftw.import_wisdom_from_filename(path);
    }

    public static void SaveWisdom()
    {
      if (string.IsNullOrEmpty(WisdomPath)) return;
      Directory.CreateDirectory(Path.GetDirectoryName(WisdomPath));
      NativeFftw.export_wisdom_to_filename(WisdomPath);
    }
  }
}
