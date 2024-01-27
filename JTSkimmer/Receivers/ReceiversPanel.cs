using System.Data;
using System.Runtime.InteropServices;
using WeifenLuo.WinFormsUI.Docking;

namespace JTSkimmer
{
  internal partial class ReceiversPanel : DockContent
  {
    private readonly Context ctx;

    // for design time only
    internal ReceiversPanel() { InitializeComponent(); }

    internal ReceiversPanel(Context ctx)
    {
      InitializeComponent();
      this.ctx = ctx;
      ctx.ReceiversPanel = this;
      ctx.MainForm.ViewReceiversMNU.Checked = true;

      var panels = ctx.Receivers.Select(rx => new ReceiverPanel(rx, ctx)).ToList();
      ArrangePanels(panels);

      foreach (var panel in panels)
      {
        panel.Dock = DockStyle.Fill;
        panel.BackColor = SystemColors.Control;
      }
    }

    private void ReceiversPanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      foreach (ReceiverPanel panel in TableLayoutPanel.Controls) panel.Dispose();

      ctx.ReceiversPanel = null;
      ctx.MainForm.ViewReceiversMNU.Checked = false;
    }

    internal ReceiverPanel AddReceiverPanel(Receiver receiver)
    {
      var rxPanel = new ReceiverPanel(receiver, ctx);
      rxPanel.Dock = DockStyle.Fill;
      rxPanel.BackColor = SystemColors.Control;

      var panels = TableLayoutPanel.Controls.OfType<ReceiverPanel>().ToList();
      panels.Add(rxPanel);
      ArrangePanels(panels);

      return rxPanel;
    }

    internal void DeleteReceiverPanel(ReceiverPanel panel)
    {
      ArrangePanels();
    }

    internal void UpdateLabels()
    {
      foreach (ReceiverPanel panel in TableLayoutPanel.Controls) panel.UpdateLabels();
    }

    internal void UpdateAudioSelection(Receiver receiver)
    {
      if (receiver.Settings.AudioToSpeaker)
        foreach (var rx in ctx.Receivers)
          if (rx != receiver)
            rx.Settings.AudioToSpeaker = false;

      if (receiver.Settings.AudioToVac)
        foreach (var rx in ctx.Receivers)
          if (rx != receiver)
            rx.Settings.AudioToVac = false;

      UpdateLabels();
      ctx.MainForm.EnableDisableSoundcards();
    }

    internal void ReorderPanels(ReceiverPanel droppedPanel, ReceiverPanel targetPanel)
    {
      ctx.MainForm.MoveReceiverInList(droppedPanel.Receiver, targetPanel.Receiver);

      var panels = TableLayoutPanel.Controls.OfType<ReceiverPanel>().ToList();
      int pos = panels.IndexOf(targetPanel);
      panels.Remove(droppedPanel);
      panels.Insert(pos, droppedPanel);
      ArrangePanels(panels);
    }


    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

    private const int WM_SETREDRAW = 11;

    private void ArrangePanels(List<ReceiverPanel>? panels = null)
    {
      SendMessage(TableLayoutPanel.Handle, WM_SETREDRAW, false, 0);

      panels ??= TableLayoutPanel.Controls.OfType<ReceiverPanel>().ToList();

      (TableLayoutPanel.RowCount, TableLayoutPanel.ColumnCount) = GetTableDimensions();

      TableLayoutPanel.Controls.Clear();
      TableLayoutPanel.ColumnStyles.Clear();
      TableLayoutPanel.RowStyles.Clear();

      var cellWidth = 100f / TableLayoutPanel.ColumnCount;
      for (int x = 0; x < TableLayoutPanel.ColumnCount; x++)
        TableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, cellWidth));

      var cellHeight = 100f / TableLayoutPanel.RowCount;
      for (int y = 0; y < TableLayoutPanel.RowCount; y++)
        TableLayoutPanel.RowStyles.Add(new ColumnStyle(SizeType.Percent, cellHeight));

      for (int i = 0; i < panels.Count; i++)
        TableLayoutPanel.Controls.Add(panels[i],
          i % TableLayoutPanel.ColumnCount,
          i / TableLayoutPanel.ColumnCount);

      SendMessage(TableLayoutPanel.Handle, WM_SETREDRAW, true, 0);
      TableLayoutPanel.Refresh();
    }

    private (int, int) GetTableDimensions()
    {
      int panelCount = ctx.Receivers.Count;
      int colCount = panelCount;
      int rowCount = 1;

      while (colCount > 1 && TableLayoutPanel.Width / colCount < 200)
      {
        int newColCount = colCount - 1;
        int newRowCount = (int)Math.Ceiling(panelCount / (float)newColCount);

        if (TableLayoutPanel.Height / newRowCount < 120) break;

        colCount = newColCount;
        rowCount = newRowCount;
      }

      colCount = (int)Math.Ceiling(panelCount / (float)rowCount);
      return (rowCount, colCount);
    }

    private void TableLayoutPanel_Resize(object sender, EventArgs e)
    {
      ArrangePanels();
    }

    public Receiver? GetReceiverUnderMouse()
    {
      return TableLayoutPanel.Controls.OfType<ReceiverPanel>()
        .FirstOrDefault(p => p.ClientRectangle.Contains(p.PointToClient(Cursor.Position)))?
        .Receiver;
    }

    internal void HighlightReceiverPanel(uint? freq)
    {
      foreach (ReceiverPanel panel in TableLayoutPanel.Controls) panel.HighlightIfFrequency(freq);
    }
  }
}
