using FontAwesome;
using WeifenLuo.WinFormsUI.Docking;

namespace JTSkimmer
{
  internal partial class MessagesPanel : DockContent
  {
    public const int MAX_LINE_COUNT = 5000;

    private readonly Context ctx;
    private Point lastMouseLocation;
    private bool AutoScrollMode = true;
    private bool Frozen;

    internal MessagesPanel()
    {
      InitializeComponent();
    }

    internal MessagesPanel(Context ctx)
    {
      InitializeComponent();
      this.ctx = ctx;
      ctx.MessagesPanel = this;
      ctx.MainForm.ViewMessagesMNU.Checked = true;

      ClearBtn.Font = ctx.AwesomeFont14;
      ClearBtn.Text = FontAwesomeIcons.Trash;
      ScrollDownBtn.Font = ctx.AwesomeFont14;
      ScrollDownBtn.Text = FontAwesomeIcons.ArrowDown;
    }

    private void MessagesPanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      ctx.MessagesPanel = null;
      ctx.MainForm.ViewMessagesMNU.Checked = false;
    }




    //--------------------------------------------------------------------------------------------------------------
    //                                                items
    //--------------------------------------------------------------------------------------------------------------
    internal void AddMessages(DecodedMessage[] messages)
    {
      BeginInvoke(() =>
      {
        listBox.BeginUpdate();

        foreach (DecodedMessage msg in messages) listBox.Items.Add(msg.ToString());


        while (listBox.Items.Count > MAX_LINE_COUNT) listBox.Items.RemoveAt(0);

        CheckAndScrollToBottom();
        listBox.EndUpdate();

        UpdateCounts(messages);
      });
    }

    private Dictionary<string, int> Counts = new();

    private void UpdateCounts(DecodedMessage[] messages)
    {
      foreach (DecodedMessage msg in messages)
        Counts[msg.Mode] = Counts.GetValueOrDefault(msg.Mode) + 1;

      ShowCounts();
    }

    private void ShowCounts()
    {
      CountsLabel.Text = string.Join("", Counts.Select(m => $"   {m.Key} ({m.Value:N0})"));
    }

    private void listBox_DrawItem(object sender, DrawItemEventArgs e)
    {
      if (listBox.Items.Count > 0)
      {
        e.DrawBackground();
        e.Graphics.DrawString(listBox.Items[e.Index].ToString(), e.Font, new SolidBrush(listBox.ForeColor), new PointF(e.Bounds.X, e.Bounds.Y));
      }
    }



    //--------------------------------------------------------------------------------------------------------------
    //                                          scroll and freeze
    //--------------------------------------------------------------------------------------------------------------
    private void listBox_MouseMove(object sender, MouseEventArgs e)
    {
      if (e.Location == lastMouseLocation) return;
      lastMouseLocation = e.Location;

      Freeze(true);
    }

    private void listBox_MouseLeave(object sender, EventArgs e)
    {
      Freeze(false);
      listBox.Invalidate();
    }

    private void listBox_MouseUp(object sender, MouseEventArgs e)
    {
      Freeze(false);
    }
    private void listBox_MouseWheel(object sender, MouseEventArgs e)
    {
      if (listBox.Items.Count == 0) return;

      // suppress animated scrolling
      int maxTop = listBox.Items.Count - listBox.ClientSize.Height / listBox.ItemHeight;
      int delta = 0;
      if (e.Delta < 0 && listBox.TopIndex < maxTop) delta = Math.Min(SystemInformation.MouseWheelScrollLines, maxTop - listBox.TopIndex);
      else if (e.Delta > 0 && listBox.TopIndex > 0) delta = -Math.Min(SystemInformation.MouseWheelScrollLines, listBox.TopIndex);

      if (delta != 0)
      {
        listBox.TopIndex += delta;
        UpdateAutoScrollMode();
      }

    ((HandledMouseEventArgs)e).Handled = true;
    }

    private void listBox_Scroll(object sender, EventArgs e)
    {
      UpdateAutoScrollMode();
    }

    // auto scroll mode is updated when the user scrolls the listbox manually,
    // using a mouse wheel or a scroll bar
    private void UpdateAutoScrollMode()
    {
      int visibleItemCount = listBox.ClientSize.Height / listBox.ItemHeight;
      AutoScrollMode = listBox.TopIndex >= listBox.Items.Count - visibleItemCount;
    }

    private void Freeze(bool value)
    {
      Frozen = value;

      freezeTimer.Stop();

      if (Frozen) freezeTimer.Start();
      else CheckAndScrollToBottom();

      //listBox.BackColor = Frozen ? Color.FromArgb(245, 255, 245) : Color.White;
    }

    internal void CheckAndScrollToBottom()
    {
      if (AutoScrollMode && !Frozen) listBox.ScrollToBottom();
    }

    private void freezeTimer_Tick(object sender, EventArgs e)
    {
      Freeze(false);
    }




    //--------------------------------------------------------------------------------------------------------------
    //                                                toolbar
    //--------------------------------------------------------------------------------------------------------------
    private void ClearBtn_Click(object sender, EventArgs e)
    {
      listBox.Items.Clear();

      Counts.Clear();
      ShowCounts();
    }

    private void ScrollDownBtn_Click(object sender, EventArgs e)
    {
      listBox.ScrollToBottom();
      UpdateAutoScrollMode();
    }
  }
}
