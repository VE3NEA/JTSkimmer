using System.IO.MemoryMappedFiles;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace JTSkimmer
{
  public abstract class SharedMemory : IDisposable
  {
    public readonly string Key;

    protected readonly MemoryMappedFile MemoryMappedFile;
    protected readonly Semaphore Semaphore;

    public SharedMemory(string? key = null)
    {
      bool existing = key != null;

      Key = key ?? MakeRandomKey(10);
      string keyHash = Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(Key))).ToLower();
      Key = Regex.Replace(Key, "[^A-Za-z]", "");
      string semaphoreKey = $"qipc_systemsem_{Key}{keyHash}";
      string sharedMemoryKey = $"qipc_sharedmemory_{Key}{keyHash}";

      if (existing)
      {
        Semaphore = Semaphore.OpenExisting(semaphoreKey);
        MemoryMappedFile = MemoryMappedFile.OpenExisting(sharedMemoryKey);
      }
      else
      {
        Semaphore = new(1, 1, semaphoreKey, out bool ok);
        if (!ok) throw new Exception($"Unable to create semaphore {Key}");
        MemoryMappedFile = MemoryMappedFile.CreateNew(sharedMemoryKey, GetSharedMemorySize());
      }
    }

    protected abstract int GetSharedMemorySize();

    public void Dispose()
    {
      MemoryMappedFile.Dispose();
      Semaphore.Dispose();
    }

    private static Random random = new Random();
    const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static string MakeRandomKey(int length)
    {
      var chars = Enumerable.Range(1, length).Select(_ => Letters[random.Next(Letters.Length)]);
      return new string(chars.ToArray());
    }

    internal byte[] ReadAllBytes()
    {
      var stream = MemoryMappedFile.CreateViewStream();
      byte[] bytes = new byte[stream.Length];
      stream.Read(bytes);
      return bytes;
    }

    internal abstract void SetSamples(float[] samples);
    public abstract void ReadParams();
    public abstract void Write();
  }
}
