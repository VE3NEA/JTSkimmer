using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using VE3NEA.HamCockpit.DspFun;

namespace JTSkimmer
{
  internal class NoiseBlanker
  {
    private Complex32[] DelayLine;
    private int DelayLineIdx;

    public bool Enabled;
    private bool UseVe3neaBlanker;
    public double Threshold = 10;

    // debug
    private float[] SamplesBefore, SamplesAfter, SamplesThreshold;

    public int TotalCount, BlankedCount;

    public NoiseBlanker(double samplingRate)
    {
      Setup(samplingRate);
    }

    private void Setup(double samplingRate)
    {
      int halfSamplesInRamp = (int)Math.Round(RAMP_LENGTH * samplingRate / 2);
      int samplesInRamp = 2 * halfSamplesInRamp + 1; // must be odd
      int samplesInSlidingMin = 3 * samplesInRamp;
      int slidingMinDelay = (samplesInSlidingMin - 1) / 2;
      SlidingMin = new(samplesInSlidingMin);

      int halfSamplesInAvg = halfSamplesInRamp / GAIN_FILTER_PASS_COUNT;
      int samplesInAvergingPass = 2 * halfSamplesInAvg + 1;
      int movingAverageDelay = halfSamplesInAvg * GAIN_FILTER_PASS_COUNT;
      MultipassMovingAverage = new(samplesInAvergingPass, GAIN_FILTER_PASS_COUNT);

      DelayLine = new Complex32[slidingMinDelay + movingAverageDelay];

      double avgTimeConstantSamples = MAGNITUDE_FILTER_TIME_CONSTANT * samplingRate;
      Alpha = 1 - Math.Exp(-1 / avgTimeConstantSamples);
    }

    public void ApplySettings(NoiseBlankerSettings settings)
    {
      Enabled = settings.Enabled;
      UseVe3neaBlanker = settings.Nonlinear;
      Threshold = ThresholdFromTrackbarPosition(settings.ThresholdTrackbarPosition);
      DelayLineIdx = 0;
    }

    // 0..100 -> 1..100, exponential scale
    public static double ThresholdFromTrackbarPosition(int position)
    {
      return Math.Exp(position / 100d * Math.Log(100));
    }

    public void Process(Complex32[] data)
    {
      if (!Enabled) return;

#if DEBUG
      if (SamplesBefore?.Length != data.Length)
      {
        SamplesBefore = new float[data.Length];
        SamplesAfter = new float[data.Length];
        SamplesThreshold = new float[data.Length];
      } 
#endif

      if (UseVe3neaBlanker) Process2(data); else Process1(data);
    }

    private void TakeSnapshot()
    {
      SnapshotRequested = false;
      string stringToInspect = string.Join('\n', SamplesBefore.Select((v, i) => $"{i}  {v}"));
      stringToInspect = string.Join('\n', SamplesAfter.Select((v, i) => $"{i}  {v}"));
      stringToInspect = string.Join('\n', SamplesThreshold.Select((v, i) => $"{i}  {v}"));
    }




    //----------------------------------------------------------------------------------------------
    //                                  NR0V noise blanker
    //----------------------------------------------------------------------------------------------
    private const double MAGNITUDE_FILTER_TIME_CONSTANT = 300e-3; // seconds
    private const double RAMP_LENGTH = 100e-6; // seconds
    private const int GAIN_FILTER_PASS_COUNT = 3;

    private SlidingMin SlidingMin;
    private MultipassAverage MultipassMovingAverage;

    private double? FilteredMagnitude;
    private double Alpha; // power averaging parameter


    private void Process1(Complex32[] data)
    {
      for (int i = 0; i < data.Length; i++)
      {
        // gain = 0 if (instant magnitude / average magnitude) exceeds threshold
        double magnitude = data[i].Magnitude;
        FilteredMagnitude ??= magnitude;
        FilteredMagnitude = FilteredMagnitude * (1 - Alpha) + magnitude * Alpha;
        if (FilteredMagnitude > 0) magnitude /= (double)(FilteredMagnitude + 1e-33);
        float gain = magnitude > Threshold ? 0 : 1;

        // shape the gain function:
        // 1. if the input is a single spike, the output is a rectangle
        gain = SlidingMin.Process(gain);
        // 2. smooth the edges of the rectangle
        gain = MultipassMovingAverage.Process(gain);

        TotalCount++;
        if (gain < 1) BlankedCount++;

#if DEBUG
        SamplesBefore[i] = DelayLine[DelayLineIdx].Magnitude;
        SamplesAfter[i] = SamplesBefore[i] * gain;
        SamplesThreshold[i] = (float)(FilteredMagnitude * Threshold);
#endif

        // delayed input signal times the gain
        Complex32 processedSample = DelayLine[DelayLineIdx] * gain;
        DelayLine[DelayLineIdx] = data[i];
        data[i] = processedSample;
        if (++DelayLineIdx == DelayLine.Length) DelayLineIdx = 0;
      }

      if (SnapshotRequested) TakeSnapshot();
    }




    //----------------------------------------------------------------------------------------------
    //                                 VE3NEA noise blanker
    //----------------------------------------------------------------------------------------------
    private const int WID1 = 2;
    private const int WID2 = 16;
    private const int DELAY = 2 * (WID1 + WID2);

    SlidingMax SlidMax1 = new(2 * WID1 + 1);
    SlidingMin SlidMin = new(2 * (WID1 + WID2) + 1);
    SlidingMax SlidMax2 = new(2 * WID2 + 1);
    internal bool SnapshotRequested;

    private void Process2(Complex32[] data)
    {
      for (int i = 0; i < data.Length; i++)
      {
        float magnitude = data[i].Magnitude;

        // apply closing and opening operations to remove peaks and dips
        float filteredMagnitude = SlidMax1.Process(magnitude);
        filteredMagnitude = SlidMin.Process(filteredMagnitude);
        filteredMagnitude = SlidMax2.Process(filteredMagnitude);

        Complex32 delayedSample = DelayLine[DelayLineIdx];
        double delayedMagnitude = delayedSample.Magnitude;

        double scaledMagnitude = delayedMagnitude / (filteredMagnitude + 1e-33f);

        DelayLine[DelayLineIdx] = data[i];

        TotalCount++;

        double magOverThreshold = scaledMagnitude / Threshold;

#if DEBUG
        SamplesBefore[i] = (float)delayedMagnitude;
        SamplesAfter[i] = (float)delayedMagnitude;
        SamplesThreshold[i] = filteredMagnitude * (float)Threshold; 
#endif

        if (magOverThreshold > 1)
        {
          double gain = (1.5 - 0.5 * Math.Exp(-(magOverThreshold - 1))) / magOverThreshold;
          data[i] = delayedSample * (float)gain;

#if DEBUG
          SamplesAfter[i] = (float)(delayedMagnitude * gain); 
#endif

          BlankedCount++;
        }
        else
          data[i] = delayedSample;

        if (++DelayLineIdx >= DELAY) DelayLineIdx = 0;
      }

      if (SnapshotRequested) TakeSnapshot();
    }
  }
}
