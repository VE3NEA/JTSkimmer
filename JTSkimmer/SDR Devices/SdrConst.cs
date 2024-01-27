namespace JTSkimmer
{
  internal static class SdrConst
  {
    internal const int AUDIO_SAMPLING_RATE = 12000;
    internal const float TWO_PI = (float)(2 * Math.PI);
    internal const uint DEFAULT_FREQUENCY = 28_100_000;

    internal static readonly uint[] DeviceRate = new uint[] { 1536_000, 3_000_000, 2_500_000, 2_000_000 };

    internal static readonly Dictionary<string, uint>[] Bandwidths = new Dictionary<string, uint>[] {
      new() {
        { "96",   DeviceRate[0] / 16 },
        { "192",  DeviceRate[0] / 8  },
        { "384",  DeviceRate[0] / 4  },
        { "768",  DeviceRate[0] / 2  },
        { "1536", DeviceRate[0] / 1  },

      },
      new() {
        { "90",   DeviceRate[1] / 32 },
        { "180",  DeviceRate[1] / 16 },
        { "360",  DeviceRate[1] / 8  },
        { "720",  DeviceRate[1] / 4  },
        { "1440", DeviceRate[1] / 2  },
      },
      new() {
        { "75",   DeviceRate[2] / 32 },
        { "150",  DeviceRate[2] / 16 },
        { "300",  DeviceRate[2] / 8  },
        { "600",  DeviceRate[2] / 4  },
        { "1200", DeviceRate[2] / 2  },
      },
      new() {
        { "125",  DeviceRate[3] / 16 },
        { "250",  DeviceRate[3] / 8  },
        { "500",  DeviceRate[3] / 4  },
        { "1000", DeviceRate[3] / 2  },
      },
     };
  }
}
