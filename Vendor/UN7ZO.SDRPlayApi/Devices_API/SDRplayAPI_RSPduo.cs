using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UN7ZO.HamCockpitPlugins.SDRPlaySource {
    public class SDRplayAPI_RSPduo
    {

        public const int RSPDUO_NUM_LNA_STATES = 10;
        public const int RSPDUO_NUM_LNA_STATES_AMPORT = 5;
        public const int RSPDUO_NUM_LNA_STATES_AM = 7;
        public const int RSPDUO_NUM_LNA_STATES_LBAND = 9;

        // RSPduo parameter public enums
        public enum sdrplay_api_RspDuoModeT
        {
            sdrplay_api_RspDuoMode_Unknown = 0,
            sdrplay_api_RspDuoMode_Single_Tuner = 1,
            sdrplay_api_RspDuoMode_Dual_Tuner = 2,
            sdrplay_api_RspDuoMode_Master = 4,
            sdrplay_api_RspDuoMode_Slave = 8,
        }


        public enum sdrplay_api_RspDuo_AmPortSelectT
        {
            sdrplay_api_RspDuo_AMPORT_1 = 1,
            sdrplay_api_RspDuo_AMPORT_2 = 0,
        }


        // RSPduo parameter public structs
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_RspDuoParamsT
        {
            public int extRefOutputEn;                             // default: 0
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_RspDuoTunerParamsT
        {
            public byte biasTEnable;                      // default: 0
            public sdrplay_api_RspDuo_AmPortSelectT tuner1AmPortSel; // default: sdrplay_api_RspDuo_AMPORT_2
            public byte tuner1AmNotchEnable;              // default: 0
            public byte rfNotchEnable;                    // default: 0
            public byte rfDabNotchEnable;                 // default: 0
        }


    }
}
