using System.Runtime.InteropServices;

namespace JTSkimmer
{
  // struct expected by jtdx_jt9.exe
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
  public class JtdxDecData
  {
    private const int NSMAX = 6827;
    private const int NTMAX = 120;
    private const int RX_SAMPLE_RATE = 12000;

    //private const int NMAX = 160 * 12000;//8 * 1024 * 1024;//30 * 60 * 12000;


    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 184 * NSMAX)]
    public float[] ss = new float[184 * NSMAX];

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = NSMAX)]
    public float[] savg = new float[NSMAX];

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = NTMAX * RX_SAMPLE_RATE)]
    public Int32[] d2 = new Int32[NTMAX * RX_SAMPLE_RATE];

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = NTMAX * RX_SAMPLE_RATE)]
    public float[] dd2 = new float[NTMAX * RX_SAMPLE_RATE];

    public JtdxParamsBlock Params = new();
  }


  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
  public struct JtdxParamsBlock
  {
    private const int MAX_CALL_LENGTH = 12;
    private const int MAX_GRID_LENGTH = 6;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_CALL_LENGTH)]
    public char[] mycall = StringToChars("", MAX_CALL_LENGTH);
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_CALL_LENGTH)]
    public char[] mybcall = StringToChars("", MAX_CALL_LENGTH);
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_CALL_LENGTH)]
    public char[] hiscall = StringToChars("", MAX_CALL_LENGTH);
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_CALL_LENGTH)]
    public char[] hisbcall = StringToChars("", MAX_CALL_LENGTH);
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_GRID_LENGTH)]
    public char[] hisgrid = StringToChars("", MAX_GRID_LENGTH);
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
    public int[] listutc = new int[10];
    public int napwid = 15;
    public int nQSOProgress = 0;
    public int nutc;                  // UTC as integer, HHMMSS
    public int nftx = 4245;
    public int ntrperiod;
    public int nfqso = 1500;
    public int npts8 = 22032;          //npts for c0() array
    public int nfa = 0;
    public int nfSplit = 2400;         //JT65 | JT9 split frequency
    public int nfb = 4000;
    public int ntol = 50;
    public int kin;
    public int nzhsym;
    public int ndepth;
    public int ncandthin = 100;  // "Candidate list thinning"
    public int ndtcenter = 0; // "DT Weighting"
    public int nft8cycles = 1; 
    public int nft8swlcycles = 3; 
    public int ntxmode = 65;
    public int nmode;
    public int nlist;
    public int nranera = 3;
    public int ntrials10 = 1; // T10 mode
    public int ntrialsrxf10 = 1; // T10 mode
    public int naggressive = 1;
    public int nharmonicsdepth = 0;
    public int ntopfreq65 = 2700; 
    public int nprepass = 4;
    public int nsdecatt = 1;
    public int nlasttx = 6;
    public int ndelay = 0;
    public int nmt = 0;
    public int nft8rxfsens = 1; // "QSO RX freq sensitivity" 1..3
    public int nft4depth = 3;
    public int nsecbandchanged = 0;

    [MarshalAs(UnmanagedType.U1)]
    public bool ndiskdat = false;
    [MarshalAs(UnmanagedType.U1)]
    public bool newdat = true;
    [MarshalAs(UnmanagedType.U1)]
    public bool nagain = false;
    [MarshalAs(UnmanagedType.U1)]
    public bool nagainfil = false;
    [MarshalAs(UnmanagedType.U1)]
    public bool nswl = true;
    [MarshalAs(UnmanagedType.U1)]
    public bool nfilter = false;
    [MarshalAs(UnmanagedType.U1)]
    public bool nstophint = true;
    [MarshalAs(UnmanagedType.U1)]
    public bool nagcc = false;
    [MarshalAs(UnmanagedType.U1)]
    public bool nhint = true;
    [MarshalAs(UnmanagedType.U1)]
    public bool fmaskact = false;
    [MarshalAs(UnmanagedType.U1)]
    public bool showharmonics = false;
    [MarshalAs(UnmanagedType.U1)]
    public bool lft8lowth;
    [MarshalAs(UnmanagedType.U1)]
    public bool lft8subpass;
    [MarshalAs(UnmanagedType.U1)]
    public bool ltxing;
    [MarshalAs(UnmanagedType.U1)]
    public bool lhidetest;
    [MarshalAs(UnmanagedType.U1)]
    public bool lhidetelemetry;
    [MarshalAs(UnmanagedType.U1)]
    public bool lhideft8dupes= true; 
    [MarshalAs(UnmanagedType.U1)]
    public bool lhound = false;
    [MarshalAs(UnmanagedType.U1)]
    public bool lhidehash;
    [MarshalAs(UnmanagedType.U1)]
    public bool lcommonft8b = true;
    [MarshalAs(UnmanagedType.U1)]
    public bool lmycallstd = true;
    [MarshalAs(UnmanagedType.U1)]
    public bool lhiscallstd;
    [MarshalAs(UnmanagedType.U1)]
    public bool lapmyc;
    [MarshalAs(UnmanagedType.U1)]
    public bool lmodechanged = false;
    [MarshalAs(UnmanagedType.U1)]
    public bool lbandchanged = false; 
    [MarshalAs(UnmanagedType.U1)]
    public bool lenabledxcsearch;
    [MarshalAs(UnmanagedType.U1)]
    public bool lwidedxcsearch;
    [MarshalAs(UnmanagedType.U1)]
    public bool lmultinst;
    [MarshalAs(UnmanagedType.U1)]
    public bool lskiptx1 = false;
    [MarshalAs(UnmanagedType.U1)]
    public bool lforcesync = false;
    [MarshalAs(UnmanagedType.U1)]
    public bool learlystart = false;


    public JtdxParamsBlock() { }

    public void Populate(WsjtxMode mode, JtdxDecoderSettings decoderSettings, DateTime utc)
    {
      nmode = mode.nmode;
      if (nmode == 5) nmode = 4; // JTDX has a different mode code
      ntrperiod = mode.ntrperiod;

      mycall = StringToChars(decoderSettings.MyCall, MAX_CALL_LENGTH);
      mybcall = StringToChars(decoderSettings.MyCall, MAX_CALL_LENGTH);
      hiscall = StringToChars(decoderSettings.HisCall, MAX_CALL_LENGTH);
      hisbcall = StringToChars(decoderSettings.HisCall, MAX_CALL_LENGTH);
      hisgrid = StringToChars(decoderSettings.HisGrid, MAX_GRID_LENGTH);

      ndepth = (int)decoderSettings.DecodingDepth;
      nQSOProgress = (int)decoderSettings.QsoProgress;
      nfqso = decoderSettings.RxOffsetHz;
      nftx = decoderSettings.TxOffsetHz;
      nfa = decoderSettings.LowDecodeHz;
      nfb = decoderSettings.HighDecodeHz;
      ntol = decoderSettings.RxToleranceHz;
      //lft8apon = decoderSettings.EnableAP;
      //emedelay = decoderSettings.EmeDelay;

      nft8cycles = decoderSettings.Ft8DecodingCycles; //  1..3
      nft8swlcycles = decoderSettings.Ft8DecodingCycles; //  1..3
      naggressive = decoderSettings.Jt65DtRange; // 1..5
      ntopfreq65 = decoderSettings.Jt65TopDecodingFreq;
      nprepass = decoderSettings.Jt65DecodingPasses; // 2..4
      nsdecatt = decoderSettings.SingleDecodeAttempts;// 1..3
      nmt = decoderSettings.Ft8Threads; // 0..24
      nft4depth = (int)decoderSettings.DecodingDepth;
      fmaskact = decoderSettings.FrequencyMaskDecoding;

      nutc = int.Parse($"{utc:HHmmss}");
    }

    private static char[] StringToChars(string str, int length)
    {
      return (str + new string(' ', length)).Substring(0, length).ToCharArray();
    }
  }
}


