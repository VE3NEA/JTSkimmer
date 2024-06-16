using System.Diagnostics;

namespace JTSkimmer
{
  public class FifoBuffer<T>
  {
    public T[] Data = Array.Empty<T>();
    public int Count;
    
    public void Append(FifoBuffer<T> buffer) { Append(buffer.Data, buffer.Count); }

    public void Append(T[] data, int? count = null)
    {
      int addCount = count ?? data.Length;
      if (addCount == 0) return;
      int totalCount = Count + addCount;
      EnsureCapacity(totalCount);
      Array.Copy(data, 0, Data, Count, addCount);
      Count = totalCount;
    }

    public FifoBuffer() { }

    public FifoBuffer(T[] data)
    {
      Data = data;
      Count = data.Length;
    }

    public FifoBuffer(T[] data, int count)
    {
      Data = data;
      Count = count;
    }

    public void Dump(int count)
    {
      if (count == 0) return;
      Debug.Assert(count <= Count);
      int keepCount = Count - count;
      if (keepCount > 0) Array.Copy(Data, count, Data, 0, keepCount);
      Count = keepCount;
    }

    public void EnsureCapacity(int count)
    {
      if (count > Data.Length) Array.Resize(ref Data, count);
    }

    public void EnsureExtraSpace(int extraCount)
    {
      EnsureCapacity(Count + extraCount);
    }
  }
}
