using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;

namespace VE3NEA
{
  public class NativeLiquidDsp
  {
    private const string LIBLIQUID = "libliquid";
    private const CallingConvention cdecl = CallingConvention.Cdecl;

    public enum LiquidNcoType
    {
      LIQUID_NCO = 0,
      LIQUID_VCO = 1
    }

    public enum LiquidAmpmodemType
    {
      LIQUID_AMPMODEM_DSB = 0,
      LIQUID_AMPMODEM_USB,
      LIQUID_AMPMODEM_LSB
    }

    public enum LiquidResampType
    {
      LIQUID_RESAMP_INTERP = 0,
      LIQUID_RESAMP_DECIM
    }

    public unsafe struct nco_crcf { };
    public unsafe struct msresamp_crcf { };
    public unsafe struct ampmodem { };
    public unsafe struct msresamp2_crcf { };
    public unsafe struct rresamp_cccf { };
    public unsafe struct firfilt_cccf { };


  // NCO

  [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe nco_crcf* nco_crcf_create(LiquidNcoType type);

    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe nco_crcf* nco_crcf_copy(nco_crcf* q);

    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe int nco_crcf_destroy(nco_crcf* q);

    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe int nco_crcf_set_frequency(nco_crcf* nco, float frequency);

    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe int nco_crcf_mix_block_up(nco_crcf* q, Complex32* x, Complex32* y, uint n);

    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe int nco_crcf_mix_block_down(nco_crcf* q, Complex32* x, Complex32* y, uint n);


    // resampler

    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe msresamp_crcf* msresamp_crcf_create(float r, float As);

    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe int msresamp_crcf_destroy(msresamp_crcf* q);

    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe int msresamp_crcf_execute(msresamp_crcf* q, Complex32* x, uint nx, Complex32* y, out uint ny);


    // modem

    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe ampmodem* ampmodem_create(float mod_index, LiquidAmpmodemType ampmodem_type, int suppressed_carrier);

    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe int ampmodem_destroy(ampmodem* q);

    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe int ampmodem_demodulate_block(ampmodem* q, Complex32* r, uint n, float* m);


    // octave resampler    

    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe msresamp2_crcf* msresamp2_crcf_create(LiquidResampType type, uint num_stages, float fc, float f0, float As);

    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe msresamp2_crcf* msresamp2_crcf_copy(msresamp2_crcf* q);
    
    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe int msresamp2_crcf_destroy(msresamp2_crcf* q);

    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe int msresamp2_crcf_execute(msresamp2_crcf* q, Complex32* x, out Complex32 y);


    // rational resampler

    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe rresamp_cccf* rresamp_cccf_create(uint interp, uint decim, uint m, Complex32* h);

    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe int rresamp_cccf_print(rresamp_cccf* q);

    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe rresamp_cccf* rresamp_cccf_copy(rresamp_cccf* q);

    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe int rresamp_cccf_destroy(rresamp_cccf* q);

    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe int rresamp_cccf_execute(rresamp_cccf* q, Complex32* x, Complex32* y);


    // filter

    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe firfilt_cccf* firfilt_cccf_create_kaiser(uint n, float fc, float As, float mu);

    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe Complex32* firfilt_cccf_get_coefficients(firfilt_cccf* q);


    [DllImport(LIBLIQUID, CallingConvention = cdecl)]
    public static extern unsafe int firfilt_cccf_destroy(firfilt_cccf* q);
  }
}
