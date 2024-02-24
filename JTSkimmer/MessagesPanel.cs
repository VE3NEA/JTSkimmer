using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Devcorp.Controls.Design;
using FontAwesome;
using Newtonsoft.Json.Linq;
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
    private MessageInfo? HotItem;

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
      ctx.MessageDistributor.WsjtxUdpSender.HighlightCallsignReceived += WsjtxUdpSender_HighlightCallsignReceived;

      ClearBtn.Font = ctx.AwesomeFont14;
      ClearBtn.Text = FontAwesomeIcons.Trash;
      ScrollDownBtn.Font = ctx.AwesomeFont14;
      ScrollDownBtn.Text = FontAwesomeIcons.ArrowDown;
      ViewArchiveBtn.Font = ctx.AwesomeFont14;
      ViewArchiveBtn.Text = FontAwesomeIcons.Folder;
    }

    private void MessagesPanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      ctx.MessageDistributor.WsjtxUdpSender.HighlightCallsignReceived -= WsjtxUdpSender_HighlightCallsignReceived;
      ctx.MessagesPanel = null;
      ctx.MainForm.ViewMessagesMNU.Checked = false;
    }




    //--------------------------------------------------------------------------------------------------------------
    //                                                items
    //--------------------------------------------------------------------------------------------------------------
    internal void AddMessages(MessageInfo[] messages)
    {
      BeginInvoke(() =>
      {
        listBox.BeginUpdate();

        foreach (MessageInfo msg in messages)
        {
          TokenizeMessage(msg);
          listBox.Items.Add(msg);
        }


        while (listBox.Items.Count > MAX_LINE_COUNT) listBox.Items.RemoveAt(0);

        CheckAndScrollToBottom();
        listBox.EndUpdate();

        UpdateCounts(messages);
      });
    }

    private readonly char[] space = new char[] { ' ' };
    private readonly string[] CqWords = new string[] { "CQ", "73", "RR73" };

    private void TokenizeMessage(MessageInfo msg)
    {
      string text = msg.Message.ToString();

      msg.Tokens.Add(new DisplayToken(text[0..35]));
      msg.Tokens.Add(new DisplayToken(text[35..38]));
      msg.Tokens.AddRange(text[38..].Split(space).Select(s => new DisplayToken(s)).ToList());

      msg.Tokens[1].bgBrush = BrushFromSnr(msg.Tokens[1].text, Color.Red);

      for (int i = 2; i < msg.Tokens.Count; i++)
        if (CqWords.Contains(msg.Tokens[i].text))
          msg.Tokens[i].bgBrush = Brushes.Yellow;
    }

    private static Brush BrushFromSnr(string snrString, Color color)
    {
      if (!int.TryParse(snrString, out int snr)) return Brushes.Transparent;

      float snrFraction = 0.2f * Math.Min(1, (24 + snr) / 24f);
      snrFraction = 0.99f - snrFraction;

      var hsl = ColorSpaceHelper.RGBtoHSL(color);
      hsl = new HSL(hsl.Hue, hsl.Saturation, snrFraction);
      return new SolidBrush(ColorSpaceHelper.HSLtoColor(hsl));
    }

    private void WsjtxUdpSender_HighlightCallsignReceived(object? sender, HighlightCallsignEventArgs e)
    {
      if (listBox.Items.Count == 0) return;
      var info = (MessageInfo)listBox.Items[listBox.Items.Count - 1];

      string slot = info.Message.ToString()[5..13];

      for (int i = listBox.Items.Count - 1; i >= 0; i--)
      {
        info = (MessageInfo)listBox.Items[i];
        if (info.Message.ToString()[5..13] != slot) break;
        if (info.Parse.DECallsign != e.Callsign) continue;

        var token = info.Tokens.FirstOrDefault(t => t.text == e.Callsign || t.text == $"<{e.Callsign}>");
        if (token == null) continue;
        token.bgBrush = new SolidBrush(e.BackColor);
        token.fgBrush = new SolidBrush(e.ForeColor);
      }

      listBox.Invalidate();
    }

    private MessageInfo? GetItemUnderCursor()
    {
      Point p = listBox.PointToClient(Cursor.Position);
      int index = listBox.IndexFromPoint(p);
      if (index < 0) return null;
      return (MessageInfo)listBox.Items[index];
    }




    //--------------------------------------------------------------------------------------------------------------
    //                                                counts
    //--------------------------------------------------------------------------------------------------------------
    private Dictionary<string, int> Counts = new();

    private void UpdateCounts(MessageInfo[] messages)
    {
      foreach (MessageInfo msg in messages)
        Counts[msg.Message.Mode] = Counts.GetValueOrDefault(msg.Message.Mode) + 1;

      ShowCounts();
    }

    private void ShowCounts()
    {
      CountsLabel.Text = string.Join("", Counts.Select(m => $"   {m.Key} ({m.Value:N0})"));
    }




    //--------------------------------------------------------------------------------------------------------------
    //                                          scroll and freeze
    //--------------------------------------------------------------------------------------------------------------
    private void listBox_MouseMove(object sender, MouseEventArgs e)
    {
      if (e.Location == lastMouseLocation) return;
      lastMouseLocation = e.Location;

      Freeze(true);

      // hot tracking
      var newHotItem = GetItemUnderCursor();
      if (newHotItem != HotItem) listBox.Invalidate();
      HotItem = newHotItem;
    }

    private void listBox_MouseLeave(object sender, EventArgs e)
    {
      Freeze(false);
      HotItem = null;
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

    private void ViewArchiveBtn_Click(object sender, EventArgs e)
    {
      Process.Start("explorer.exe", Path.Combine(Utils.GetUserDataFolder(), "Messages"));
    }




    //--------------------------------------------------------------------------------------------------------------
    //                                                paint
    //--------------------------------------------------------------------------------------------------------------
    private static Brush RxBkBrush = new SolidBrush(Color.White);
    private static Brush ToMeBkBrush = new SolidBrush(Color.FromArgb(255, 175, 175));
    private static Brush HotBkBrush = new SolidBrush(Color.FromArgb(20, 0, 0, 255));

    private void listBox_DrawItem(object sender, DrawItemEventArgs e)
    {
      if (e.Index < 0) return;

      var info = (MessageInfo)listBox.Items[e.Index];
      bool toMe = info.Parse.DXCallsign == ctx.Settings.User.Call;

      var spaceWidth = e.Graphics.MeasureString("__", e.Font).Width - e.Graphics.MeasureString("_", e.Font).Width;
      PointF p = new PointF(e.Bounds.Location.X, e.Bounds.Location.Y);

      Brush bgBrush = RxBkBrush;
      if (toMe) bgBrush = ToMeBkBrush;
      e.Graphics.FillRectangle(bgBrush, e.Bounds);
      if (info == HotItem) e.Graphics.FillRectangle(HotBkBrush, e.Bounds);

      foreach (var token in info.Tokens)
      {
        SizeF size = e.Graphics.MeasureString(token.text, e.Font);
        RectangleF rect = new RectangleF(p, size);

        e.Graphics.FillRectangle(token.bgBrush, rect);
        e.Graphics.DrawString(token.text, e.Font, token.fgBrush, p);

        SizeF sizeWithSpaces = e.Graphics.MeasureString(token.text.Replace(' ', '_'), e.Font);
        p.X += sizeWithSpaces.Width + spaceWidth;
      }
    }
  }
}
