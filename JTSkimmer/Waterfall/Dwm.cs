using System.Runtime.InteropServices;

namespace VE3NEA.HamCockpitPlugins.Waterfall
{
  class Dwm
  {
    public static (float Rate, UInt64 RefreshCount) GetTimingInfo()
    {
      DWM_TIMING_INFO t = new DWM_TIMING_INFO();
      t.cbSize = (uint)Marshal.SizeOf(t);
      var error = DwmGetCompositionTimingInfo(IntPtr.Zero, out t);
      return (t.rateRefresh.uiNumerator / (float)t.rateRefresh.uiDenominator, t.cRefresh);
    }

    public static float RefreshRate()
    {
      return GetTimingInfo().Rate;
    }

    public static ulong RefreshCount()
    {
      return GetTimingInfo().RefreshCount;
    }






    //----------------------------------------------------------------------------------------------
    //                    derived from Win32Interop, struct packing bug fixed
    //----------------------------------------------------------------------------------------------

    [DllImport("dwmapi.dll", EntryPoint = "DwmFlush")]
    public static extern int DwmFlush();

    [DllImport("dwmapi.dll", EntryPoint = "DwmGetCompositionTimingInfo")]
    public static extern int DwmGetCompositionTimingInfo(IntPtr hwnd, out DWM_TIMING_INFO pTimingInfo);

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct UNSIGNED_RATIO
    {
      public UInt32 uiNumerator;
      public UInt32 uiDenominator;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DWM_TIMING_INFO
    {
      public UInt32 cbSize;
      public UNSIGNED_RATIO rateRefresh;
      public UInt64 qpcRefreshPeriod;
      public UNSIGNED_RATIO rateCompose;
      public UInt64 qpcVBlank;
      public UInt64 cRefresh;
      public uint cDXRefresh;
      public UInt64 qpcCompose;
      public UInt64 cFrame;
      public uint cDXPresent;
      public UInt64 cRefreshFrame;
      public UInt64 cFrameSubmitted;
      public uint cDXPresentSubmitted;
      public UInt64 cFrameConfirmed;
      public uint cDXPresentConfirmed;
      public UInt64 cRefreshConfirmed;
      public uint cDXRefreshConfirmed;
      public UInt64 cFramesLate;
      public uint cFramesOutstanding;
      public UInt64 cFrameDisplayed;
      public UInt64 qpcFrameDisplayed;
      public UInt64 cRefreshFrameDisplayed;
      public UInt64 cFrameComplete;
      public UInt64 qpcFrameComplete;
      public UInt64 cFramePending;
      public UInt64 qpcFramePending;
      public UInt64 cFramesDisplayed;
      public UInt64 cFramesComplete;
      public UInt64 cFramesPending;
      public UInt64 cFramesAvailable;
      public UInt64 cFramesDropped;
      public UInt64 cFramesMissed;
      public UInt64 cRefreshNextDisplayed;
      public UInt64 cRefreshNextPresented;
      public UInt64 cRefreshesDisplayed;
      public UInt64 cRefreshesPresented;
      public UInt64 cRefreshStarted;
      public UInt64 cPixelsReceived;
      public UInt64 cPixelsDrawn;
      public UInt64 cBuffersEmpty;
    }
  }
}