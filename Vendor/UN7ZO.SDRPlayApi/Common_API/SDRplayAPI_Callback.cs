using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UN7ZO.HamCockpitPlugins.SDRPlaySource {
    public class SDRplayAPI_Callback
    {
        // Event callback enums
        public enum sdrplay_api_PowerOverloadCbEventIdT
        {
            sdrplay_api_Overload_Detected = 0,
            sdrplay_api_Overload_Corrected = 1,
        }

        public enum sdrplay_api_RspDuoModeCbEventIdT
        {
            sdrplay_api_MasterInitialised = 0,
            sdrplay_api_SlaveAttached = 1,
            sdrplay_api_SlaveDetached = 2,
            sdrplay_api_SlaveInitialised = 3,
            sdrplay_api_SlaveUninitialised = 4,
            sdrplay_api_MasterDllDisappeared = 5,
            sdrplay_api_SlaveDllDisappeared = 6,
        }

        public enum sdrplay_api_EventT
        {
            sdrplay_api_GainChange = 0,
            sdrplay_api_PowerOverloadChange = 1,
            sdrplay_api_DeviceRemoved = 2,
            sdrplay_api_RspDuoModeChange = 3,
        }


        // Event callback parameter structs
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_GainCbParamT
        {
            public uint gRdB;
            public uint lnaGRdB;
            public double currGain;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_PowerOverloadCbParamT {
            public sdrplay_api_PowerOverloadCbEventIdT powerOverloadChangeType;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_RspDuoModeCbParamT {
            public sdrplay_api_RspDuoModeCbEventIdT modeChangeType;
        }

        // Event parameters overlay
        //union sdrplay_api_EventParamsT
        //NOTE: not sure if this works... needs testing (I added Pack=0 out of intuition)
        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        public struct sdrplay_api_EventParamsT {
            [FieldOffset(0)]
            public sdrplay_api_GainCbParamT gainParams;
            [FieldOffset(0)]
            public sdrplay_api_PowerOverloadCbParamT powerOverloadParams;
            [FieldOffset(0)]
            public sdrplay_api_RspDuoModeCbParamT rspDuoModeParams;
        }


        // Stream callback parameter structs
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_StreamCbParamsT {
            public uint firstSampleNum;
            public int grChanged;
            public int rfChanged;
            public int fsChanged;
            public uint numSamples;
        }

        // Callback function prototypes
        //typedef void (* sdrplay_api_StreamCallback_t) (short* xi, short* xq, sdrplay_api_StreamCbParamsT* params, uint numSamples, uint reset, void* cbContext);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate void sdrplay_api_StreamCallback_t(IntPtr xi, IntPtr xq, ref sdrplay_api_StreamCbParamsT Params, uint numSamples, uint reset, IntPtr cbContext);

        //typedef void (* sdrplay_api_EventCallback_t) (sdrplay_api_EventT eventId, sdrplay_api_TunerSelectT tuner, sdrplay_api_EventParamsT*params, void* cbContext);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate void sdrplay_api_EventCallback_t(sdrplay_api_EventT eventId, SDRplayAPI_Tuner.sdrplay_api_TunerSelectT tuner, ref sdrplay_api_EventParamsT Params, IntPtr cbContext);

        // Callback function struct
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_CallbackFnsT
        {
            public sdrplay_api_StreamCallback_t StreamACbFn;
            public sdrplay_api_StreamCallback_t StreamBCbFn;
            public sdrplay_api_EventCallback_t EventCbFn;
        }
    }
}
