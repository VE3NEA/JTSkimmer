using VE3NEA.HamCockpit;

namespace JTSkimmer
{
  internal static class Program
  {
    [STAThread]
    static void Main()
    {
      using (new SingleInstanceEnforcer())
      {
        ExceptionLogger.Initialize();

        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
      }
    }
  }
}