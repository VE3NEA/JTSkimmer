using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UN7ZO.HamCockpitPlugins.SDRPlaySource {
    class NativeMethods {

        public const float SDRPLAY_API_VERSION_307 = 3.07f;
        public const float SDRPLAY_API_VERSION_306 = 3.06f;

        public const int  SDRPLAY_MAX_DEVICES = 16; // Maximum devices supported by the API
        public const int  SDRPLAY_MAX_TUNERS_PER_DEVICE = 2; // Maximum number of tuners available on one device
        public const int  SDRPLAY_MAX_SER_NO_LEN = 64; // Maximum length of device serial numbers
        public const int  SDRPLAY_MAX_ROOT_NM_LEN = 32; // Maximum length of device names
        
        // Supported device IDs
        public const int  SDRPLAY_RSP1_ID = 1;
        public const int  SDRPLAY_RSP1A_ID = 255;
        public const int  SDRPLAY_RSP2_ID = 2;
        public const int  SDRPLAY_RSPduo_ID = 3;
        public const int  SDRPLAY_RSPdx_ID = 4;

        public enum sdrplay_api_ErrT {
            sdrplay_api_Success = 0,
            sdrplay_api_Fail = 1,
            sdrplay_api_InvalidParam = 2,
            sdrplay_api_OutOfRange = 3,
            sdrplay_api_GainUpdateError = 4,
            sdrplay_api_RfUpdateError = 5,
            sdrplay_api_FsUpdateError = 6,
            sdrplay_api_HwError = 7,
            sdrplay_api_AliasingError = 8,
            sdrplay_api_AlreadyInitialised = 9,
            sdrplay_api_NotInitialised = 10,
            sdrplay_api_NotEnabled = 11,
            sdrplay_api_HwVerError = 12,
            sdrplay_api_OutOfMemError = 13,
            sdrplay_api_ServiceNotResponding = 14,
            sdrplay_api_StartPending = 15,
            sdrplay_api_StopPending = 16,
            sdrplay_api_InvalidMode = 17,
            sdrplay_api_FailedVerification1 = 18,
            sdrplay_api_FailedVerification2 = 19,
            sdrplay_api_FailedVerification3 = 20,
            sdrplay_api_FailedVerification4 = 21,
            sdrplay_api_FailedVerification5 = 22,
            sdrplay_api_FailedVerification6 = 23,
            sdrplay_api_InvalidServiceVersion = 24
        }

        public enum sdrplay_api_Bw_MHzT {
            sdrplay_api_BW_Undefined = 0,
            sdrplay_api_BW_0_200 = 200,
            sdrplay_api_BW_0_300 = 300,
            sdrplay_api_BW_0_600 = 600,
            sdrplay_api_BW_1_536 = 1536,
            sdrplay_api_BW_5_000 = 5000,
            sdrplay_api_BW_6_000 = 6000,
            sdrplay_api_BW_7_000 = 7000,
        }

        public enum sdrplay_api_TransferModeT {
            sdrplay_api_ISOCH = 0,
            sdrplay_api_BULK = 1
        }
        

        public enum sdrplay_api_If_kHzT : int {
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

        public enum sdrplay_api_TunerSelectT {
            sdrplay_api_Tuner_Neither = 0,
            sdrplay_api_Tuner_A = 1,
            sdrplay_api_Tuner_B = 2,
            sdrplay_api_Tuner_Both = 3,
        }

        public enum sdrplay_api_AgcControlT {
            sdrplay_api_AGC_DISABLE = 0,
            sdrplay_api_AGC_100HZ = 1,
            sdrplay_api_AGC_50HZ = 2,
            sdrplay_api_AGC_5HZ = 3,
            sdrplay_api_AGC_CTRL_EN = 4
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_RspDxTunerParamsT {
            public SDRplayAPI_RSPdx.sdrplay_api_RspDx_HdrModeBwT hdrBw;                 // default: sdrplay_api_RspDx_HDRMODE_BW_1_700
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_DcOffsetT {
            public byte DCenable;          // default: 1
            public byte IQenable;          // default: 1
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_DecimationT {
            public byte enable;            // default: 0
            public byte decimationFactor;  // default: 1
            public byte wideBandSignal;    // default: 0
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_AgcT {
            public sdrplay_api_AgcControlT enable;    // default: sdrplay_api_AGC_50HZ
            public int setPoint_dBfs;                 // default: -60
            public ushort attack_ms;          // default: 0
            public ushort decay_ms;           // default: 0
            public ushort decay_delay_ms;     // default: 0
            public ushort decay_threshold_dB; // default: 0
            public int syncUpdate;                    // default: 0
        }

        public enum sdrplay_api_AdsbModeT {
            sdrplay_api_ADSB_DECIMATION = 0,
            sdrplay_api_ADSB_NO_DECIMATION_LOWPASS = 1,
            sdrplay_api_ADSB_NO_DECIMATION_BANDPASS_2MHZ = 2,
            sdrplay_api_ADSB_NO_DECIMATION_BANDPASS_3MHZ = 3
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_ControlParamsT {
            public sdrplay_api_DcOffsetT dcOffset;
            public sdrplay_api_DecimationT decimation;
            public sdrplay_api_AgcT agc;
            public sdrplay_api_AdsbModeT adsbMode;  //default: sdrplay_api_ADSB_DECIMATION
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_RxChannelParamsT {
            public SDRplayAPI_Tuner.sdrplay_api_TunerParamsT tunerParams;
            public sdrplay_api_ControlParamsT ctrlParams;
            public SDRplayAPI_RSP1a.sdrplay_api_Rsp1aTunerParamsT rsp1aTunerParams;
            public SDRplayAPI_RSP2.sdrplay_api_Rsp2TunerParamsT rsp2TunerParams;
            public SDRplayAPI_RSPduo.sdrplay_api_RspDuoTunerParamsT rspDuoTunerParams;
            public SDRplayAPI_RSPdx.sdrplay_api_RspDxTunerParamsT rspDxTunerParams;
        }

        // Device parameter public structure
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_DeviceParamsT {
            public /*sdrplay_api_DevParamsT**/ IntPtr devParams;
            public /*sdrplay_api_RxChannelParamsT**/ IntPtr rxChannelA;
            public /*sdrplay_api_RxChannelParamsT**/ IntPtr rxChannelB;
        }

        // Device public structure 
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_DeviceT {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = SDRPLAY_MAX_SER_NO_LEN)]
            public char[] SerNo;
            public byte hwVer;
            public SDRplayAPI_Tuner.sdrplay_api_TunerSelectT tuner;
            public SDRplayAPI_RSPduo.sdrplay_api_RspDuoModeT rspDuoMode;
            //public byte valid; // VE3NEA
            public double rspDuoSampleFreq;
            public /*HANDLE*/ IntPtr dev;
        }

        // Dev parameter public structs
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_FsFreqT {
            public double fsHz;               // default: 2000000.0
            public byte syncUpdate;           // default: 0
            public byte reCal;                // default: 0
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_SyncUpdateT {
            public uint sampleNum;             // default: 0
            public uint period;                // default: 0
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_ResetFlagsT {
            public byte resetGainUpdate;      // default: 0
            public byte resetRfUpdate;        // default: 0
            public byte resetFsUpdate;        // default: 0
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_DevParamsT {
            public double ppm;                         // default: 0.0
            public sdrplay_api_FsFreqT fsFreq;
            public sdrplay_api_SyncUpdateT syncUpdate;
            public sdrplay_api_ResetFlagsT resetFlags;
            public sdrplay_api_TransferModeT mode;     // default: sdrplay_api_ISOCH
            public uint samplesPerPkt;         // default: 0 (output param)
            public SDRplayAPI_RSP1a.sdrplay_api_Rsp1aParamsT rsp1aParams;
            public SDRplayAPI_RSP2.sdrplay_api_Rsp2ParamsT rsp2Params;
            public SDRplayAPI_RSPduo.sdrplay_api_RspDuoParamsT rspDuoParams;
            public SDRplayAPI_RSPdx.sdrplay_api_RspDxParamsT rspDxParams;
        }

        // Extended error message public structure
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct sdrplay_api_ErrorInfoT {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] file;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] function;

            public int line;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
            public char[] message;
        }

        public enum sdrplay_api_ReasonForUpdateT {
            sdrplay_api_Update_None = 0x00000000,

            // Reasons for master only mode 
            sdrplay_api_Update_Dev_Fs = 0x00000001,
            sdrplay_api_Update_Dev_Ppm = 0x00000002,
            sdrplay_api_Update_Dev_SyncUpdate = 0x00000004,
            sdrplay_api_Update_Dev_ResetFlags = 0x00000008,

            sdrplay_api_Update_Rsp1a_BiasTControl = 0x00000010,
            sdrplay_api_Update_Rsp1a_RfNotchControl = 0x00000020,
            sdrplay_api_Update_Rsp1a_RfDabNotchControl = 0x00000040,

            sdrplay_api_Update_Rsp2_BiasTControl = 0x00000080,
            sdrplay_api_Update_Rsp2_AmPortSelect = 0x00000100,
            sdrplay_api_Update_Rsp2_AntennaControl = 0x00000200,
            sdrplay_api_Update_Rsp2_RfNotchControl = 0x00000400,
            sdrplay_api_Update_Rsp2_ExtRefControl = 0x00000800,

            sdrplay_api_Update_RspDuo_ExtRefControl = 0x00001000,

            sdrplay_api_Update_Master_Spare_1 = 0x00002000,
            sdrplay_api_Update_Master_Spare_2 = 0x00004000,

            // Reasons for master and slave mode
            // Note: sdrplay_api_Update_Tuner_Gr MUST be the first value defined in this section!
            sdrplay_api_Update_Tuner_Gr = 0x00008000,
            sdrplay_api_Update_Tuner_GrLimits = 0x00010000,
            sdrplay_api_Update_Tuner_Frf = 0x00020000,
            sdrplay_api_Update_Tuner_BwType = 0x00040000,
            sdrplay_api_Update_Tuner_IfType = 0x00080000,
            sdrplay_api_Update_Tuner_DcOffset = 0x00100000,
            sdrplay_api_Update_Tuner_LoMode = 0x00200000,

            sdrplay_api_Update_Ctrl_DCoffsetIQimbalance = 0x00400000,
            sdrplay_api_Update_Ctrl_Decimation = 0x00800000,
            sdrplay_api_Update_Ctrl_Agc = 0x01000000,
            sdrplay_api_Update_Ctrl_AdsbMode = 0x02000000,
            sdrplay_api_Update_Ctrl_OverloadMsgAck = 0x04000000,

            sdrplay_api_Update_RspDuo_BiasTControl = 0x08000000,
            sdrplay_api_Update_RspDuo_AmPortSelect = 0x10000000,
            sdrplay_api_Update_RspDuo_Tuner1AmNotchControl = 0x20000000,
            sdrplay_api_Update_RspDuo_RfNotchControl = 0x40000000,
            sdrplay_api_Update_RspDuo_RfDabNotchControl = unchecked((int)0x80000000)//TODO: see if this actually works
        }

        public enum sdrplay_api_ReasonForUpdateExtension1T {
            sdrplay_api_Update_Ext1_None = 0x00000000,

            // Reasons for master only mode 
            sdrplay_api_Update_RspDx_HdrEnable = 0x00000001,
            sdrplay_api_Update_RspDx_BiasTControl = 0x00000002,
            sdrplay_api_Update_RspDx_AntennaControl = 0x00000004,
            sdrplay_api_Update_RspDx_RfNotchControl = 0x00000008,
            sdrplay_api_Update_RspDx_RfDabNotchControl = 0x00000010,
            sdrplay_api_Update_RspDx_HdrBw = 0x00000020,

            // Reasons for master and slave mode
        }

        public struct lightDeviceInfo {
            public string deviceName;
            public string deviceSN;
        }

        #region Native Methods
        private const string DLL_FILENAME = "sdrplay_api.dll";

        [DllImport(DLL_FILENAME, EntryPoint = "sdrplay_api_ApiVersion", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe sdrplay_api_ErrT sdrplay_api_ApiVersion(out float version);

        [DllImport(DLL_FILENAME, EntryPoint = "sdrplay_api_Open", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe sdrplay_api_ErrT sdrplay_api_Open();

        [DllImport(DLL_FILENAME, EntryPoint = "sdrplay_api_Close", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe sdrplay_api_ErrT sdrplay_api_Close();

        [DllImport(DLL_FILENAME, EntryPoint = "sdrplay_api_GetDevices", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe sdrplay_api_ErrT sdrplay_api_GetDevices([Out] sdrplay_api_DeviceT[] devices, out uint numDevs, uint maxDevs);

        [DllImport(DLL_FILENAME, EntryPoint = "sdrplay_api_LockDeviceApi", CallingConvention = CallingConvention.Cdecl)]
        public static extern sdrplay_api_ErrT sdrplay_api_LockDeviceApi();

        [DllImport(DLL_FILENAME, EntryPoint = "sdrplay_api_UnlockDeviceApi", CallingConvention = CallingConvention.Cdecl)]
        public static extern sdrplay_api_ErrT sdrplay_api_UnlockDeviceApi();

        [DllImport(DLL_FILENAME, EntryPoint = "sdrplay_api_SelectDevice", CallingConvention = CallingConvention.Cdecl)]
        public static extern sdrplay_api_ErrT sdrplay_api_SelectDevice(ref sdrplay_api_DeviceT device);

        [DllImport(DLL_FILENAME, EntryPoint = "sdrplay_api_ReleaseDevice", CallingConvention = CallingConvention.Cdecl)]
        public static extern sdrplay_api_ErrT sdrplay_api_ReleaseDevice(ref sdrplay_api_DeviceT device);

        [DllImport(DLL_FILENAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern sdrplay_api_ErrT sdrplay_api_Init(IntPtr dev, ref SDRplayAPI_Callback.sdrplay_api_CallbackFnsT callbackFns, IntPtr cbContext);

        [DllImport(DLL_FILENAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern sdrplay_api_ErrT sdrplay_api_Uninit(IntPtr dev);

        [DllImport(DLL_FILENAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern sdrplay_api_ErrT sdrplay_api_DebugEnable(IntPtr dev, uint enable);

        [DllImport(DLL_FILENAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr sdrplay_api_GetErrorString(sdrplay_api_ErrT err);

        [DllImport(DLL_FILENAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern sdrplay_api_ErrT sdrplay_api_GetDeviceParams(IntPtr dev, out IntPtr deviceParams);

        [DllImport(DLL_FILENAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern sdrplay_api_ErrT sdrplay_api_Update(IntPtr dev, SDRplayAPI_Tuner.sdrplay_api_TunerSelectT tuner, sdrplay_api_ReasonForUpdateT reasonForUpdate, sdrplay_api_ReasonForUpdateExtension1T reasonForUpdateExt1);

        [DllImport(DLL_FILENAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr sdrplay_api_GetLastError(IntPtr device);
        #endregion
    }
}
