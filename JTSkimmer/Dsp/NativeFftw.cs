using System.Runtime.InteropServices;

namespace VE3NEA
{
  public class NativeFftw
  {
    // download dll's:
    // https://fftw.org/pub/fftw/fftw-3.3.5-dll32.zip
    // https://fftw.org/pub/fftw/fftw-3.3.5-dll64.zip


    [Flags]
    public enum FftwFlags : uint
    {
      Measure = 0,
      DestroyInput = 1,
      Unaligned = 2,
      ConserveMemory = 4,
      Exhaustive = 8,
      PreserveInput = 16,
      Patient = 32,
      Estimate = 64
    }

    public enum FftwDirection : int
    {
      Forward = -1,
      Backward = 1
    }

    private const string LIBFFTW = "libfftw3f-3";
    private const CallingConvention cdecl = CallingConvention.Cdecl;


    [DllImport(LIBFFTW, EntryPoint = "fftwf_malloc", ExactSpelling = true, CallingConvention = cdecl)]
    public static extern IntPtr malloc(int length);

    [DllImport(LIBFFTW, EntryPoint = "fftwf_free", ExactSpelling = true, CallingConvention = cdecl)]
    public static extern void free(IntPtr mem);

    [DllImport(LIBFFTW, EntryPoint = "fftwf_make_planner_thread_safe", ExactSpelling = true, CallingConvention = cdecl)]
    public static extern void make_planner_thread_safe();

    [DllImport(LIBFFTW, EntryPoint = "fftwf_plan_dft_1d", ExactSpelling = true, CallingConvention = cdecl)]
    public static extern IntPtr dft_1d(int n, IntPtr input, IntPtr output, FftwDirection direction, FftwFlags flags);

    [DllImport(LIBFFTW, EntryPoint = "fftwf_plan_dft_r2c_1d", ExactSpelling = true, CallingConvention = cdecl)]
    public static extern IntPtr dft_r2c_1d(int n, IntPtr input, IntPtr output, FftwFlags flags);

    [DllImport(LIBFFTW, EntryPoint = "fftwf_destroy_plan", ExactSpelling = true, CallingConvention = cdecl)]
    public static extern void destroy_plan(IntPtr plan);

    [DllImport(LIBFFTW, EntryPoint = "fftwf_import_wisdom_from_filename", ExactSpelling = true, CallingConvention = cdecl)]
    public static extern void import_wisdom_from_filename(string filename);

    [DllImport(LIBFFTW, EntryPoint = "fftwf_export_wisdom_to_filename", ExactSpelling = true, CallingConvention = cdecl)]
    public static extern void export_wisdom_to_filename(string filename);

    [DllImport(LIBFFTW, EntryPoint = "fftwf_execute", ExactSpelling = true, CallingConvention = cdecl)]
    public static extern void execute(IntPtr plan);
  }
}
