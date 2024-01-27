using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UN7ZO.HamCockpitPlugins.SDRPlaySource {
    public class SDRplayAPI_Tuner {
        const int MAX_BB_GR = 59;

        // Tuner parameter public enums
        public enum sdrplay_api_Bw_MHzT {
            sdrplay_api_BW_Undefined = 0,
            sdrplay_api_BW_0_200 = 200,
            sdrplay_api_BW_0_300 = 300,
            sdrplay_api_BW_0_600 = 600,
            sdrplay_api_BW_1_536 = 1536,
            sdrplay_api_BW_5_000 = 5000,
            sdrplay_api_BW_6_000 = 6000,
            sdrplay_api_BW_7_000 = 7000,
            sdrplay_api_BW_8_000 = 8000
        }

        public enum sdrplay_api_If_kHzT {
            sdrplay_api_IF_Undefined = -1,
            sdrplay_api_IF_Zero = 0,
            sdrplay_api_IF_0_450 = 450,
            sdrplay_api_IF_1_620 = 1620,
            sdrplay_api_IF_2_048 = 2048
        }

        public enum sdrplay_api_LoModeT {
            sdrplay_api_LO_Undefined = 0,
            sdrplay_api_LO_Auto = 1,
            sdrplay_api_LO_120MHz = 2,
            sdrplay_api_LO_144MHz = 3,
            sdrplay_api_LO_168MHz = 4
        }

        public enum sdrplay_api_MinGainReductionT {
            sdrplay_api_EXTENDED_MIN_GR = 0,
            sdrplay_api_NORMAL_MIN_GR = 20
        }

        public enum sdrplay_api_TunerSelectT {
            sdrplay_api_Tuner_Neither = 0,
            sdrplay_api_Tuner_A = 1,
            sdrplay_api_Tuner_B = 2,
            sdrplay_api_Tuner_Both = 3,
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        // Tuner parameter public structs
        public struct sdrplay_api_GainValuesT {
            public float curr;
            public float max;
            public float min;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_GainT {
            public int gRdB;                            // default: 50
            public byte LNAstate;                       // default: 0
            public byte syncUpdate;                     // default: 0
            public sdrplay_api_MinGainReductionT minGr; // default: sdrplay_api_NORMAL_MIN_GR
            public sdrplay_api_GainValuesT gainVals;    // output parameter
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_RfFreqT {
            public double rfHz;                // default: 200000000.0
            public byte syncUpdate;            // default: 0
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_DcOffsetTunerT {
            public byte dcCal;              // default: 3 (Periodic mode)
            public byte speedUp;            // default: 0 (No speedup)
            public int trackTime;           // default: 1    (=> time in uSec = (dcCal * 3 * trackTime)       = 9uSec)
            public int refreshRateTime;     // default: 2048 (=> time in uSec = (dcCal * 3 * refreshRateTime) = 18432uSec)
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_TunerParamsT {
            public sdrplay_api_Bw_MHzT bwType;          // default: sdrplay_api_BW_0_200
            public sdrplay_api_If_kHzT ifType;          // default: sdrplay_api_IF_Zero
            public sdrplay_api_LoModeT loMode;          // default: sdrplay_api_LO_Auto
            public sdrplay_api_GainT gain;
            public sdrplay_api_RfFreqT rfFreq;
            public sdrplay_api_DcOffsetTunerT dcOffsetTuner;
        }


    }
}
