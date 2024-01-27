namespace VE3NEA
{
  public class DataEventArgs<T> : EventArgs
  {
    public T[] Data;
    public DateTime ReceivedAt;
    public DataEventArgs() 
    {
      SetValues(Array.Empty<T>());
    }

    public DataEventArgs(T[] data, DateTime? receivedAt = null)
    {
      SetValues(data, receivedAt); 
    }

    public void SetValues(T[] data, DateTime? receivedAt = null)
    {
      Data = data;
      ReceivedAt = receivedAt ?? DateTime.UtcNow;
    }
  }
}