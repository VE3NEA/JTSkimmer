using System.Diagnostics;

namespace JTSkimmer
{
  public partial class AboutBox : Form
  {
    public AboutBox()
    {
      InitializeComponent();
    }

    private void WebsiteLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      WebsiteLabel.LinkVisited = true;
      Process.Start(new ProcessStartInfo("https://ve3nea.github.io/JTSkimmer") { UseShellExecute = true });
    }
  }
}
