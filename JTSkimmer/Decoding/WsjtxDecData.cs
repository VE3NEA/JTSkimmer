using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JTSkimmer
{
  // struct expected by jt9.exe
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
  public class WsjtxDecData
  {
    private const int NSMAX = 6827;
    private const int NMAX = 30 * 60 * 12000;
    private const int IPC_SIZE = 3;
    private const int SS_SIZE = 184 * NSMAX;
    private const int SRED_SIZE = 5760;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = IPC_SIZE)]
    public int[] ipc = new int[3];

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SS_SIZE)]
    public float[] ss = new float[SS_SIZE];

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = NSMAX)]
    public float[] savg = new float[NSMAX];

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = SRED_SIZE)]
    public float[] sred = new float[SRED_SIZE];

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = NMAX)]
    public Int16[] id2 = new Int16[NMAX];

    public WsjtxParamsBlock Params = new();

    public WsjtxDecData() { }
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
  public struct WsjtxParamsBlock
  {
    private const int MAX_CALL_LENGTH = 12;
    private const int MAX_GRID_LENGTH = 6;

    public int nutc;                  // UTC as integer, HHMMSS
    [MarshalAs(UnmanagedType.U1)]
    public bool ndiskdat = true;      // do both eraly and full decoding
    public int ntrperiod;
    public int nQSOProgress;
    public int nfqso = 1500;
    public int nftx;
    [MarshalAs(UnmanagedType.U1)]
    public bool newda = true;          //true ==> new data, must do long FFT
    public int npts8 = 74736;          //npts for c0() array
    public int nfa = 200;
    public int nfSplit = 2700;         //JT65 | JT9 split frequency
    public int nfb = 4000;
    public int ntol = 20;
    public int kin = 64800;
    public int nzhsym;
    public int nsubmode;
    [MarshalAs(UnmanagedType.U1)]
    public bool nagain = false;
    public int ndepth = 1;
    [MarshalAs(UnmanagedType.U1)]
    public bool lft8apon = true;
    [MarshalAs(UnmanagedType.U1)]
    public bool lapcqonly = false;
    [MarshalAs(UnmanagedType.U1)]
    public bool ljt65apon = true;
    public int napwid = 75;
    public int ntxmode = 65;
    public int nmode;
    public int minw;
    [MarshalAs(UnmanagedType.U1)]
    public bool nclearave = false;
    public int minSync;
    public float emedelay;
    public float dttol = 3;
    public int nlist;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
    public int[] listutc = new int[10];
    public int n2pass = 2;
    public int nranera = 6;
    public int naggressive = 0;
    [MarshalAs(UnmanagedType.U1)]
    public bool nrobust = false;
    public int nexp_decode;
    public int max_drift;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
    public char[] datetime = new char[20];
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_CALL_LENGTH)]
    public char[] mycall = StringToChars("", MAX_CALL_LENGTH);
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_GRID_LENGTH)]
    public char[] mygrid = StringToChars("", MAX_GRID_LENGTH);
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_CALL_LENGTH)]
    public char[] hiscall = StringToChars("", MAX_CALL_LENGTH);
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_GRID_LENGTH)]
    public char[] hisgrid = StringToChars("", MAX_GRID_LENGTH);


    public WsjtxParamsBlock() { }


    private const int BLOCK_SIZE = 3456;
    // put decoding parameters in the WSJTX structure
    internal void Populate(WsjtxMode mode, WsjtxDecoderSettings decoderSettings, DateTime utc)
    {
      ntxmode = nmode = mode.nmode;
      nsubmode = mode.nsubmode;
      ntrperiod = mode.ntrperiod;

      nzhsym = mode.nzhsym;
      kin = mode.kin;
      npts8 = nzhsym * BLOCK_SIZE / 8;

      // Include_averaging    16;
      // Include_correlation  32;
      // Enable_AP_DXcall     64;
      // Auto_Clear_Avg      128;
      ndepth = (int)decoderSettings.DecodingDepth;
      if (nmode == 66)
      {
        if (decoderSettings.Q65Average) ndepth += 16;
        if (decoderSettings.Q65ClearAfter) ndepth += 128;
      }

      lft8apon = decoderSettings.EnableAP;
      nQSOProgress = (int)decoderSettings.QsoProgress;
      nfqso = decoderSettings.RxOffsetHz;
      nftx = decoderSettings.TxOffsetHz;
      nfa = decoderSettings.LowDecodeHz;
      nfb = decoderSettings.HighDecodeHz;
      ntol = decoderSettings.RxToleranceHz;
      emedelay = decoderSettings.EmeDelay;

      mycall = StringToChars(decoderSettings.MyCall, MAX_CALL_LENGTH);
      mygrid = StringToChars(decoderSettings.MyGrid, MAX_GRID_LENGTH);
      hiscall = StringToChars(decoderSettings.HisCall, MAX_CALL_LENGTH);
      hisgrid = StringToChars(decoderSettings.HisGrid, MAX_GRID_LENGTH);

      nutc = int.Parse($"{utc:HHmmss}");

      // pretend that data comes from a file so that jt9 runs both early and full decoding
      ndiskdat = nmode == 8;
    }

    private static char[] StringToChars(string str, int length)
    {
      return (str + new string(' ', length)).Substring(0, length).ToCharArray();
    }
  }
}