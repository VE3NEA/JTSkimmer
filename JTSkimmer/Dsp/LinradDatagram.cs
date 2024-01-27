using System.Runtime.InteropServices;
using MathNet.Numerics;

namespace JTSkimmer
{
  // defined in https://sourceforge.net/p/linrad/code/HEAD/tree/trunk/globdef.h#l1212

  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct LinradDatagram
  {
    public const int SAMPLES_PER_DATAGRAM = 174;
    public const int SAMPLING_RATE = 96000;

    public double passband_center;        // MHz
    public int time;                      // ms since midnight     
    public float userx_freq = SAMPLING_RATE;
    public int ptr;                       // unused    
    public ushort block_no;               // auto increment
    public sbyte userx_no = -1;           // 1 rx, float data
    public sbyte passband_direction = 1;  // const

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SAMPLES_PER_DATAGRAM)]
    public Complex32[] buf = new Complex32[SAMPLES_PER_DATAGRAM];

    public LinradDatagram() {}
  }
}

