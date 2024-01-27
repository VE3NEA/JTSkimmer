using JTSkimmer;
using Serilog;

namespace VE3NEA.HamCockpit
{
  public class ExceptionLogger
  {
    public static void Initialize()
    {
      string fileName = Path.Combine(Utils.GetUserDataFolder(), "Logs", "log_.txt");
      Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
          .WriteTo.File(fileName, rollingInterval: RollingInterval.Day).CreateLogger();

      Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
      AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
      Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
      var exception = e.ExceptionObject as Exception;
      Log.Error(exception, "Worker thread exception occurred:");
    }

    private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
      Log.Error(e.Exception, "UI thread exception occurred:");
      if (ShowDialog(e.Exception) == DialogResult.Abort) Application.Exit();
    }

    private static DialogResult ShowDialog(Exception e)
    {
      string errorMessage = $"Exception: {e.Message}\nModule: {e.Source} \nFunction: {e.TargetSite}"; // + "\n\nStack Trace:\n" + e.StackTrace;
      return MessageBox.Show(errorMessage, AppDomain.CurrentDomain.FriendlyName, MessageBoxButtons.AbortRetryIgnore,
          MessageBoxIcon.Stop);
    }
  }
}
