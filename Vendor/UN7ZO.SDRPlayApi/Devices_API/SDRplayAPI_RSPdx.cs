using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UN7ZO.HamCockpitPlugins.SDRPlaySource {
    public class SDRplayAPI_RSPdx
    {


        public const int RSPDX_NUM_LNA_STATES = 28;   // Number of LNA states in all bands (except where defined differently below)
        public const int RSPDX_NUM_LNA_STATES_AMPORT2_0_12 = 19;   // Number of LNA states when using AM Port 2 between 0 and 12MHz
        public const int RSPDX_NUM_LNA_STATES_AMPORT2_12_60 = 20;   // Number of LNA states when using AM Port 2 between 12 and 60MHz
        public const int RSPDX_NUM_LNA_STATES_VHF_BAND3 = 27;   // Number of LNA states in VHF and Band3
        public const int RSPDX_NUM_LNA_STATES_420MHZ = 21;   // Number of LNA states in 420MHz band
        public const int RSPDX_NUM_LNA_STATES_LBAND = 19;   // Number of LNA states in L-band
        public const int RSPDX_NUM_LNA_STATES_DX = 26;   // Number of LNA states in DX path

        // RSPdx parameter public enums
        public enum sdrplay_api_RspDx_AntennaSelectT
        {
            sdrplay_api_RspDx_ANTENNA_A = 0,
            sdrplay_api_RspDx_ANTENNA_B = 1,
            sdrplay_api_RspDx_ANTENNA_C = 2,
        }


        public enum sdrplay_api_RspDx_HdrModeBwT
        {
            sdrplay_api_RspDx_HDRMODE_BW_0_200 = 0,
            sdrplay_api_RspDx_HDRMODE_BW_0_500 = 1,
            sdrplay_api_RspDx_HDRMODE_BW_1_200 = 2,
            sdrplay_api_RspDx_HDRMODE_BW_1_700 = 3,
        }


        // RSPdx parameter public structs
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_RspDxParamsT
        {
            public byte hdrEnable;                            // default: 0
            public byte biasTEnable;                          // default: 0
            public sdrplay_api_RspDx_AntennaSelectT antennaSel;        // default: sdrplay_api_RspDx_ANTENNA_A
            public byte rfNotchEnable;                        // default: 0
            public byte rfDabNotchEnable;                     // default: 0
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_RspDxTunerParamsT
        {
            public sdrplay_api_RspDx_HdrModeBwT hdrBw;                 // default: sdrplay_api_RspDx_HDRMODE_BW_1_700
        }



    }
}
