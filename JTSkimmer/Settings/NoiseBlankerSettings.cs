using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTSkimmer
{
  internal class NoiseBlankerSettings
  {
    public bool Enabled;
    public int ThresholdTrackbarPosition = 10; // 0..100

    public bool Nonlinear;
  }
}
