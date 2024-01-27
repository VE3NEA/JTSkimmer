using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JTSkimmer
{
    public class ListBoxEx : System.Windows.Forms.ListBox
    {
        public event EventHandler Scroll;




        //--------------------------------------------------------------------------------------------------------------
        //                                          suppress flicker
        //--------------------------------------------------------------------------------------------------------------

        // https://yacsharpblog.blogspot.com/2008/07/listbox-flicker.html

        public ListBoxEx()
        {
            this.SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint,
                true);
            this.DrawMode = DrawMode.OwnerDrawFixed;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Region iRegion = new Region(e.ClipRectangle);
            e.Graphics.FillRegion(new SolidBrush(this.BackColor), iRegion);

            if (this.Items.Count > 0)
            {
                int lastIndex = Math.Min(Items.Count - 1, TopIndex + ClientSize.Height / ItemHeight + 1);

                for (int itemIndex = TopIndex; itemIndex <= lastIndex; ++itemIndex)
                {
                    Rectangle itemRect = GetItemRectangle(itemIndex);
                    if (e.ClipRectangle.IntersectsWith(itemRect))
                    {
                        OnDrawItem(new DrawItemEventArgs(e.Graphics, Font, itemRect, itemIndex, 
                            DrawItemState.Default, ForeColor, BackColor));
                        iRegion.Complement(itemRect);
                    }
                }
            }
            base.OnPaint(e);
        }




        //--------------------------------------------------------------------------------------------------------------
        //                                          auto scroll down
        //--------------------------------------------------------------------------------------------------------------
        public bool IsLastItemVisible()
        {
            int visibleItemCount = ClientSize.Height / ItemHeight;
            return Items.Count > 0 && TopIndex >= Items.Count - visibleItemCount;
        }

        public void ScrollToBottom()
        {
            int visibleItemCount = ClientSize.Height / ItemHeight;
            TopIndex = Items.Count - visibleItemCount;
        }




        //--------------------------------------------------------------------------------------------------------------
        //                                    vertical scroll event
        //--------------------------------------------------------------------------------------------------------------
        private const int WM_VSCROLL = 0x115;

        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == WM_VSCROLL) Scroll?.Invoke(this, new EventArgs());
  
            base.WndProc(ref msg);
        }
    }
}
