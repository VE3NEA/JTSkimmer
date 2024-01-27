using System.Runtime.InteropServices;
using System.Text;


// see https://github.com/airspy/airspyone_host/blob/master/libairspy/src/airspy.h
// dependencies: libusb-1.0.dll, pthreadVC2.dll


namespace VE3NEA
{
  public static unsafe class NativeAirspy
  {
    private const string AIRSPY = "airspy";
    private const CallingConvention cdecl = CallingConvention.Cdecl;

    public enum AirspyError
    {
      AIRSPY_SUCCESS = 0,
      AIRSPY_TRUE = 1,
      AIRSPY_ERROR_INVALID_PARAM = -2,
      AIRSPY_ERROR_NOT_FOUND = -5,
      AIRSPY_ERROR_BUSY = -6,
      AIRSPY_ERROR_NO_MEM = -11,
      AIRSPY_ERROR_UNSUPPORTED = -12,
      AIRSPY_ERROR_LIBUSB = -1000,
      AIRSPY_ERROR_THREAD = -1001,
      AIRSPY_ERROR_STREAMING_THREAD_ERR = -1002,
      AIRSPY_ERROR_STREAMING_STOPPED = -1003,
      AIRSPY_ERROR_OTHER = -9999,
    };

    public enum AirspySampleType
    {
      AIRSPY_SAMPLE_FLOAT32_IQ,
      AIRSPY_SAMPLE_FLOAT32_REAL,
      AIRSPY_SAMPLE_INT16_IQ,
      AIRSPY_SAMPLE_INT16_REAL,
      AIRSPY_SAMPLE_UINT16_REAL,
      AIRSPY_SAMPLE_RAW,
      AIRSPY_SAMPLE_END
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct AirspyTransfer
    {
      public IntPtr device;
      public GCHandle ctx;
      public void* samples;
      public int sample_count;
      public UInt64 dropped_samples;
      public AirspySampleType sample_type;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct AirspyLibVersion
    {
      public UInt32 major_version;
      public UInt32 minor_version;
      public UInt32 revision;
    }
  
  [UnmanagedFunctionPointer(cdecl)]
    public delegate AirspyError AirspySampleBlockCbFn(ref AirspyTransfer transfer);



    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern int airspy_list_devices([In, Out] long[]? serials, int count);

    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern AirspyError airspy_open_sn(out IntPtr device, long serial_number);

    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern AirspyError airspy_open(out IntPtr dev);

    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern AirspyError airspy_close(IntPtr dev);

    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern AirspyError airspy_get_samplerates(IntPtr device, [In, Out] uint[] buffer, uint len);

    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern AirspyError airspy_set_samplerate(IntPtr dev, uint samplerate);

    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern AirspyError airspy_start_rx(IntPtr dev, [MarshalAs(UnmanagedType.FunctionPtr)] AirspySampleBlockCbFn cb, GCHandle ctx);

    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern AirspyError airspy_stop_rx(IntPtr dev);

    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern AirspyError airspy_set_sample_type(IntPtr dev, AirspySampleType sample_type);

    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern AirspyError airspy_set_rf_bias(IntPtr dev, bool value);

    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern AirspyError airspy_set_packing(IntPtr dev, bool value);

    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern AirspyError airspy_set_vga_gain(IntPtr dev, byte value);

    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern AirspyError airspy_set_mixer_gain(IntPtr dev, byte value);

    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern AirspyError airspy_set_lna_gain(IntPtr dev, byte value);

    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern AirspyError airspy_set_linearity_gain(IntPtr dev, byte value);

    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern AirspyError airspy_set_sensitivity_gain(IntPtr dev, byte value);

    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern AirspyError airspy_set_lna_agc(IntPtr dev, bool value);

    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern AirspyError airspy_set_mixer_agc(IntPtr dev, bool value);

    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern AirspyError airspy_set_freq(IntPtr dev, uint freq_hz);

    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern AirspyError airspy_is_streaming(IntPtr dev);

    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern AirspyError airspy_version_string_read(IntPtr device, StringBuilder version, byte length);

    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern AirspyError airspy_config_read(IntPtr device, byte page_index, short length, StringBuilder data);

    [DllImport(AIRSPY, CallingConvention = cdecl)]
    public static extern AirspyError airspy_lib_version(out IntPtr lib_version);
  }
}
