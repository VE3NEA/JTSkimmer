using System.Diagnostics;

namespace JTSkimmer
{
  class TimeSlotBuffer<T>
  {
    private readonly float SecondsPerSlot;
    private readonly int SamplesPerSlot;
    private T[] buffer;
    private int Count;
    private DateTime FirstSampleTime;
    private DateTime LastSampleTime;
    
    public int index, blockCount;

    public bool IsComplete { get; private set; }
    public T[] SlotData { get; private set; }
    public DateTime SlotStartTime { get; private set; }


    public TimeSlotBuffer(float secondsPerSlot) 
    {
      SecondsPerSlot = secondsPerSlot;
      SamplesPerSlot = (int)(secondsPerSlot * SdrConst.AUDIO_SAMPLING_RATE);
      buffer = new T[2 * SamplesPerSlot];
      SlotData = new T[SamplesPerSlot];
    }

    public void AddSamples(T[] samples, DateTime receivedAt)
    {
      blockCount++;

      // keep track of the stored samples time range
      LastSampleTime = receivedAt;
      if (Count == 0) FirstSampleTime = LastSampleTime.AddSeconds(-samples.Length / SdrConst.AUDIO_SAMPLING_RATE);

      // write samples to buffer
      int newCount = Count + samples.Length;
      if (newCount > buffer.Length)
        Array.Resize(ref buffer, newCount);
      Array.Copy(samples, 0, buffer, Count, samples.Length);
      Count = newCount;

      // find slot boundaries
      double lastSampleSeconds = LastSampleTime.TimeOfDay.TotalSeconds;
      double slotEndSeconds = SecondsPerSlot * Math.Truncate(lastSampleSeconds / SecondsPerSlot);
      DateTime slotEndTime = LastSampleTime.Date.AddSeconds(slotEndSeconds);
      var slotEndindex = Count - (int)(SdrConst.AUDIO_SAMPLING_RATE * (LastSampleTime - slotEndTime).TotalSeconds);
      var slotStartIndex = slotEndindex - SamplesPerSlot;
      IsComplete = slotStartIndex >= 0;

      if (IsComplete)
      {
        SlotStartTime = slotEndTime.AddSeconds(-SecondsPerSlot);
        //CheckForGaps();

        Array.Copy(buffer, slotStartIndex, SlotData, 0, SamplesPerSlot);

        // dump used data
        int countToDump = slotEndindex - 2;
        Count -= countToDump;
        Array.Copy(buffer, countToDump, buffer, 0, Count);
        FirstSampleTime = LastSampleTime.AddSeconds(-Count / SdrConst.AUDIO_SAMPLING_RATE);

        blockCount = 0;
      }
    }

    public void CheckForGaps()
    {
      var expectedSeconds = (LastSampleTime - FirstSampleTime).TotalSeconds;
      var storedSeconds = Count / (float)SdrConst.AUDIO_SAMPLING_RATE;
      var missing = expectedSeconds - storedSeconds;

      Debug.WriteLine($"#{index} {DateTime.Now: HH:mm:ss.fff} stored {storedSeconds:F3} s, {missing * 1000:F2} ms missing ({blockCount} blocks received at {LastSampleTime:HH:mm:ss.fff}");
    }

    internal void Reset()
    {
      Count = 0;
    }
  }
}
