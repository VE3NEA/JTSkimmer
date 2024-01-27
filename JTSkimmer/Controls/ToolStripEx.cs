namespace JTSkimmer
{
  // react to the first click even if ToolStrip is not active. 
  public class ToolStripEx : ToolStrip
  {
    private const Int32 WM_MOUSEACTIVE = 0x21;

    public ToolStripEx() : base() { }

    protected override void WndProc(ref Message m)
    {
      if (m.Msg == WM_MOUSEACTIVE && CanFocus && !Focused) Focus();
      base.WndProc(ref m);
    }
  }
}
