using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace FlashLearnfr
{
    public class FlatTabControl : TabControl
    {
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x1328) return;
            base.WndProc(ref m);
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                return new Rectangle(0, 0, this.Width, this.Height);
            }
        }

        public FlatTabControl()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.DoubleBuffer |
                          ControlStyles.UserPaint, true);

            this.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.ItemSize = new Size(1, 1);
            this.SizeMode = TabSizeMode.Fixed;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
        }
    }
    public class TranslucentToolStripRenderer : ToolStripProfessionalRenderer
    {
        private Form parentForm;

    public TranslucentToolStripRenderer(Form form)
    {
        this.parentForm = form;
    }
     protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
    {
    }
    
   }

}
    class ui
    {
    }

