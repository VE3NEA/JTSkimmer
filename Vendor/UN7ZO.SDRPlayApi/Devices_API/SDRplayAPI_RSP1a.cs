using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UN7ZO.HamCockpitPlugins.SDRPlaySource {
    public class SDRplayAPI_RSP1a {
        public const int RSPIA_NUM_LNA_STATES = 10;
        public const int RSPIA_NUM_LNA_STATES_AM = 7;
        public const int RSPIA_NUM_LNA_STATES_LBAND = 9;

        // RSP1A parameter enums

        // RSP1A parameter public structs
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_Rsp1aParamsT {
            public byte rfNotchEnable;                              // default: 0
            public byte rfDabNotchEnable;                           // default: 0
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_Rsp1aTunerParamsT {
            public byte biasTEnable;                   // default: 0
        }

    }
}
