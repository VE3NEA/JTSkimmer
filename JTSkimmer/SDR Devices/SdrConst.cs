namespace JTSkimmer
{
  internal static class SdrConst
  {
    internal const int AUDIO_SAMPLING_RATE = 12000;
    internal const float TWO_PI = (float)(2 * Math.PI);
    internal const uint DEFAULT_FREQUENCY = 28_100_000;

    internal static readonly uint[] DeviceRates = new uint[] { 1536_000, 3_000_000, 2_500_000, 2_000_000, 2_000_000, 2_000_000 };

    internal static readonly Dictionary<string, uint>[] Bandwidths = new Dictionary<string, uint>[] {
      new() {
        { "96",   DeviceRates[0] / 16 },
        { "192",  DeviceRates[0] / 8  },
        { "384",  DeviceRates[0] / 4  },
        { "768",  DeviceRates[0] / 2  },
        { "1536", DeviceRates[0] / 1  },

      },
      new() {
        { "90",   DeviceRates[1] / 32 },
        { "180",  DeviceRates[1] / 16 },
        { "360",  DeviceRates[1] / 8  },
        { "720",  DeviceRates[1] / 4  },
        { "1440", DeviceRates[1] / 2  },
      },
      new() {
        { "75",   DeviceRates[2] / 32 },
        { "150",  DeviceRates[2] / 16 },
        { "300",  DeviceRates[2] / 8  },
        { "600",  DeviceRates[2] / 4  },
        { "1200", DeviceRates[2] / 2  },
      },
      new() {
        { "125",  DeviceRates[3] / 16 },
        { "250",  DeviceRates[3] / 8  },
        { "500",  DeviceRates[3] / 4  },
        { "1000", DeviceRates[3] / 2  },
      },
      new() {
        { "125",  DeviceRates[3] / 16 },
        { "250",  DeviceRates[3] / 8  },
        { "500",  DeviceRates[3] / 4  },
        { "1000", DeviceRates[3] / 2  },
      },
      new() {
        { "125",  DeviceRates[3] / 16 },
        { "250",  DeviceRates[3] / 8  },
        { "500",  DeviceRates[3] / 4  },
        { "1000", DeviceRates[3] / 2  },
      },
     };
  }
}
