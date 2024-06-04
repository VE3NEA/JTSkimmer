using System.ComponentModel;
using System.Runtime.InteropServices;

namespace UN7ZO.HamCockpitPlugins.SDRPlaySource
{
  public class SDRplayAPI_RSP2
  {
    public const int RSPII_NUM_LNA_STATES = 9;
    public const int RSPII_NUM_LNA_STATES_AMPORT = 5;
    public const int RSPII_NUM_LNA_STATES_420MHZ = 6;

    // RSP2 parameter public enums
    public enum sdrplay_api_Rsp2_AntennaSelectT : int
    {
      [Description("Antenna A")] // desctiptions by VE3NEA
      sdrplay_api_Rsp2_ANTENNA_A = 5,
      [Description("Antenna B")]
      sdrplay_api_Rsp2_ANTENNA_B = 6,
    }


    public enum sdrplay_api_Rsp2_AmPortSelectT
    {
      [Description("AM Port 1")]
      sdrplay_api_Rsp2_AMPORT_1 = 1,
      [Description("AM Port 2")]
      sdrplay_api_Rsp2_AMPORT_2 = 0,
    }


    // RSP2 parameter public structs
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct sdrplay_api_Rsp2ParamsT
    {
      public byte extRefOutputEn;                // default: 0
    }

    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct sdrplay_api_Rsp2TunerParamsT
    {
      public byte biasTEnable;                   // default: 0
      public sdrplay_api_Rsp2_AmPortSelectT amPortSel;    // default: sdrplay_api_Rsp2_AMPORT_2
      public sdrplay_api_Rsp2_AntennaSelectT antennaSel;  // default: sdrplay_api_Rsp2_ANTENNA_A
      public byte rfNotchEnable;                 // default: 0
    }
  }
}