using System.Runtime.InteropServices;
using System.Text;

// see https://github.com/steve-m/librtlsdr/blob/master/include/rtl-sdr.h
// dependency: libusb-1.0.dll

namespace JTSkimmer
{
  public static unsafe class NativeRtlSdr
  {
    private const string RTL_SDR = "rtlsdr";
    private const CallingConvention cdecl = CallingConvention.Cdecl;


    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern uint rtlsdr_get_device_count();

    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern IntPtr rtlsdr_get_device_name(uint index);

    /*!
     * Get USB device strings.
     *
     * NOTE: The string arguments must provide space for up to 256 bytes.
     *
     * \param index the device index
     * \param manufact manufacturer name, may be NULL
     * \param product product name, may be NULL
     * \param serial serial number, may be NULL
     * \return 0 on success
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_get_device_usb_strings(uint index,
                   StringBuilder? manufact,
                   StringBuilder? product,
                   StringBuilder? serial);

    /*!
     * Get device index by USB serial string descriptor.
     *
     * \param serial serial string of the device
     * \return device index of first device where the name matched
     * \return -1 if name is NULL
     * \return -2 if no devices were found at all
     * \return -3 if devices were found, but none with matching name
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_get_index_by_serial(string serial);

    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_open(out IntPtr dev, uint index);

    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_close(IntPtr dev);

    /* configuration functions */

    /*!
     * Set crystal oscillator frequencies used for the RTL2832 and the tuner IC.
     *
     * Usually both ICs use the same clock. Changing the clock may make sense if
     * you are applying an external clock to the tuner or to compensate the
     * frequency (and samplerate) error caused by the original (cheap) crystal.
     *
     * NOTE: Call this function only if you fully understand the implications.
     *
     * \param dev the device handle given by rtlsdr_open()
     * \param rtl_freq frequency value used to clock the RTL2832 in Hz
     * \param tuner_freq frequency value used to clock the tuner IC in Hz
     * \return 0 on success
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_set_xtal_freq(IntPtr dev, uint rtl_freq,
                uint tuner_freq);

    /*!
     * Get crystal oscillator frequencies used for the RTL2832 and the tuner IC.
     *
     * Usually both ICs use the same clock.
     *
     * \param dev the device handle given by rtlsdr_open()
     * \param rtl_freq frequency value used to clock the RTL2832 in Hz
     * \param tuner_freq frequency value used to clock the tuner IC in Hz
     * \return 0 on success
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_get_xtal_freq(IntPtr dev, out uint rtl_freq,
                out uint tuner_freq);

    /*!
     * Get USB device strings.
     *
     * NOTE: The string arguments must provide space for up to 256 bytes.
     *
     * \param dev the device handle given by rtlsdr_open()
     * \param manufact manufacturer name, may be NULL
     * \param product product name, may be NULL
     * \param serial serial number, may be NULL
     * \return 0 on success
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_get_usb_strings(IntPtr dev, 
      StringBuilder? manufact,
      StringBuilder? product, 
      StringBuilder? serial
      );

    /*!
     * Write the device EEPROM
     *
     * \param dev the device handle given by rtlsdr_open()
     * \param data buffer of data to be written
     * \param offset address where the data should be written
     * \param len length of the data
     * \return 0 on success
     * \return -1 if device handle is invalid
     * \return -2 if EEPROM size is exceeded
     * \return -3 if no EEPROM was found
     */

    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_write_eeprom(IntPtr dev, byte[] data,
              byte offset, UInt16 len);

    /*!
     * Read the device EEPROM
     *
     * \param dev the device handle given by rtlsdr_open()
     * \param data buffer where the data should be written
     * \param offset address where the data should be read from
     * \param len length of the data
     * \return 0 on success
     * \return -1 if device handle is invalid
     * \return -2 if EEPROM size is exceeded
     * \return -3 if no EEPROM was found
     */

    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_read_eeprom(IntPtr dev, IntPtr data,
              byte offset, UInt16 len);

    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_set_center_freq(IntPtr dev, uint freq);

    /*!
     * Get actual frequency the device is tuned to.
     *
     * \param dev the device handle given by rtlsdr_open()
     * \return 0 on error, frequency in Hz otherwise
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern uint rtlsdr_get_center_freq(IntPtr dev);

    /*!
     * Set the frequency correction value for the device.
     *
     * \param dev the device handle given by rtlsdr_open()
     * \param ppm correction value in parts per million (ppm)
     * \return 0 on success
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_set_freq_correction(IntPtr dev, int ppm);

    /*!
     * Get actual frequency correction value of the device.
     *
     * \param dev the device handle given by rtlsdr_open()
     * \return correction value in parts per million (ppm)
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_get_freq_correction(IntPtr dev);

    public enum TunerType
    {
      RTLSDR_TUNER_UNKNOWN,
      RTLSDR_TUNER_E4000,
      RTLSDR_TUNER_FC0012,
      RTLSDR_TUNER_FC0013,
      RTLSDR_TUNER_FC2580,
      RTLSDR_TUNER_R820T,
      RTLSDR_TUNER_R828D
    };

    /*!
     * Get the tuner type.
     *
     * \param dev the device handle given by rtlsdr_open()
     * \return RTLSDR_TUNER_UNKNOWN on error, tuner type otherwise
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern TunerType rtlsdr_get_tuner_type(IntPtr dev);

    /*!
     * Get a list of gains supported by the tuner.
     *
     * NOTE: The gains argument must be preallocated by the caller. If NULL is
     * being given instead, the number of available gain values will be returned.
     *
     * \param dev the device handle given by rtlsdr_open()
     * \param gains array of gain values. In tenths of a dB, 115 means 11.5 dB.
     * \return <= 0 on error, number of available (returned) gain values otherwise
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_get_tuner_gains(IntPtr dev, [In, Out] int[]? gains); 

    /*!
     * Set the gain for the device.
     * Manual gain mode must be enabled for this to work.
     *
     * Valid gain values (in tenths of a dB) for the E4000 tuner:
     * -10, 15, 40, 65, 90, 115, 140, 165, 190,
     * 215, 240, 290, 340, 420, 430, 450, 470, 490
     *
     * Valid gain values may be queried with \ref rtlsdr_get_tuner_gains function.
     *
     * \param dev the device handle given by rtlsdr_open()
     * \param gain in tenths of a dB, 115 means 11.5 dB.
     * \return 0 on success
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_set_tuner_gain(IntPtr dev, int gain);

    /*!
     * Get actual gain the device is configured to.
     *
     * \param dev the device handle given by rtlsdr_open()
     * \return 0 on error, gain in tenths of a dB, 115 means 11.5 dB.
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_get_tuner_gain(IntPtr dev);

    /*!
     * Set the intermediate frequency gain for the device.
     *
     * \param dev the device handle given by rtlsdr_open()
     * \param stage intermediate frequency gain stage number (1 to 6 for E4000)
     * \param gain in tenths of a dB, -30 means -3.0 dB.
     * \return 0 on success
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_set_tuner_if_gain(IntPtr dev, int stage, int gain);

    /*!
     * Set the gain mode (automatic/manual) for the device.
     * Manual gain mode must be enabled for the gain setter function to work.
     *
     * \param dev the device handle given by rtlsdr_open()
     * \param manual gain mode, 1 means manual gain mode shall be enabled.
     * \return 0 on success
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_set_tuner_gain_mode(IntPtr dev, bool manual);

    /*!
     * Set the sample rate for the device, also selects the baseband filters
     * according to the requested sample rate for tuners where this is possible.
     *
     * \param dev the device handle given by rtlsdr_open()
     * \param samp_rate the sample rate to be set, possible values are:
     * 		    225001 - 300000 Hz
     * 		    900001 - 3200000 Hz
     * 		    sample loss is to be expected for rates > 2400000
     * \return 0 on success, -EINVAL on invalid rate
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_set_sample_rate(IntPtr dev, uint rate);

    /*!
     * Get actual sample rate the device is configured to.
     *
     * \param dev the device handle given by rtlsdr_open()
     * \return 0 on error, sample rate in Hz otherwise
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern uint rtlsdr_get_sample_rate(IntPtr dev);

    /*!
     * Enable test mode that returns an 8 bit counter instead of the samples.
     * The counter is generated inside the RTL2832.
     *
     * \param dev the device handle given by rtlsdr_open()
     * \param test mode, 1 means enabled, 0 disabled
     * \return 0 on success
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_set_testmode(IntPtr dev, bool on);

    /*!
     * Enable or disable the internal digital AGC of the RTL2832.
     *
     * \param dev the device handle given by rtlsdr_open()
     * \param digital AGC mode, 1 means enabled, 0 disabled
     * \return 0 on success
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_set_agc_mode(IntPtr dev, bool on);

    /*!
     * Enable or disable the direct sampling mode. When enabled, the IF mode
     * of the RTL2832 is activated, and rtlsdr_set_center_freq() will control
     * the IF-frequency of the DDC, which can be used to tune from 0 to 28.8 MHz
     * (xtal frequency of the RTL2832).
     *
     * \param dev the device handle given by rtlsdr_open()
     * \param on 0 means disabled, 1 I-ADC input enabled, 2 Q-ADC input enabled
     * \return 0 on success
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_set_direct_sampling(IntPtr dev, bool on);

    /*!
     * Get state of the direct sampling mode
     *
     * \param dev the device handle given by rtlsdr_open()
     * \return -1 on error, 0 means disabled, 1 I-ADC input enabled
     *	    2 Q-ADC input enabled
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_get_direct_sampling(IntPtr dev);

    /*!
     * Enable or disable offset tuning for zero-IF tuners, which allows to avoid
     * problems caused by the DC offset of the ADCs and 1/f noise.
     *
     * \param dev the device handle given by rtlsdr_open()
     * \param on 0 means disabled, 1 enabled
     * \return 0 on success
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_set_offset_tuning(IntPtr dev, bool on);

    /*!
     * Get state of the offset tuning mode
     *
     * \param dev the device handle given by rtlsdr_open()
     * \return -1 on error, 0 means disabled, 1 enabled
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_get_offset_tuning(IntPtr dev);

    /* streaming functions */

    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_reset_buffer(IntPtr dev);

    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_read_sync(IntPtr dev, byte[] buf, int len, out int n_read);

    [UnmanagedFunctionPointer(cdecl)]
    public unsafe delegate void RtlSdrReadAsyncCb(IntPtr buf, uint len, GCHandle ctx);

    /*!
     * Read samples from the device asynchronously. This function will block until
     * it is being canceled using rtlsdr_cancel_async()
     *
     * NOTE: This function is deprecated and is subject for removal.
     *
     * \param dev the device handle given by rtlsdr_open()
     * \param cb callback function to return received samples
     * \param ctx user specific context to pass via the callback function
     * \return 0 on success
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_wait_async(IntPtr dev, RtlSdrReadAsyncCb cb, GCHandle ctx);

    /*!
     * Read samples from the device asynchronously. This function will block until
     * it is being canceled using rtlsdr_cancel_async()
     *
     * \param dev the device handle given by rtlsdr_open()
     * \param cb callback function to return received samples
     * \param ctx user specific context to pass via the callback function
     * \param buf_num optional buffer count, buf_num * buf_len = overall buffer size
     *		  set to 0 for default buffer count (32)
     * \param buf_len optional buffer length, must be multiple of 512,
     *		  set to 0 for default buffer length (16 * 32 * 512)
     * \return 0 on success
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_read_async(IntPtr dev,
             RtlSdrReadAsyncCb cb,
             GCHandle ctx,
             uint buf_num,
             uint buf_len);

    /*!
     * Cancel all pending asynchronous operations on the device.
     *
     * \param dev the device handle given by rtlsdr_open()
     * \return 0 on success
     */
    [DllImport(RTL_SDR, CallingConvention = cdecl)]
    public static extern int rtlsdr_cancel_async(IntPtr dev);

  }
}
