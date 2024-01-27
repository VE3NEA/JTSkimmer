namespace JTSkimmer
{
  internal class SingleInstanceEnforcer : IDisposable
  {
    Mutex Mutex; 

    public SingleInstanceEnforcer() 
    {
      string appName = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
      Mutex = new Mutex(true, appName);

      if (!Mutex.WaitOne(TimeSpan.Zero, true))
      {
        MessageBox.Show($"{appName} is already running", appName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        Environment.Exit(1);
      }
    }

    public void Dispose()
    {
      Mutex.ReleaseMutex();
    }
  }
}
