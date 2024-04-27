using Newtonsoft.Json.Linq;
using ST.Controls;
using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.CodeGenerator
{
    public class TabMainSpllit : UserSplitContainer
    {
        new public ChildPanel Panel1 => _Panel1;
        private ChildPanel _Panel1;

        new public ChildPanel Panel2 => _Panel2;
        private ChildPanel _Panel2;

        public TabMainSpllit()
        {
            SetStyle(ControlStyles.Selectable, false);
            TabStop = false;
            SplitterWidth = 1;

            _Panel1 = new ChildPanel();
            _Panel1.Dock = DockStyle.Fill;
            _Panel1.Cursor = Cursors.Default;
            base.Panel1.Controls.Add(_Panel1);

            _Panel2 = new ChildPanel();
            _Panel2.Dock = DockStyle.Fill;
            _Panel2.Cursor = Cursors.Default;
            base.Panel2.Controls.Add(_Panel2);
        }

        public class ChildPanel : Panel
        {
            protected override void OnPaintBackground(PaintEventArgs e)
            {
                base.OnPaintBackground(e);
                OnPaint(new PaintEventArgs(CreateGraphics(), new Rectangle(0, 0, Width, Height)));
            }

            public void ReDraw()
            {
                OnPaintBackground(new PaintEventArgs(CreateGraphics(), new Rectangle(0, 0, Width, Height)));
            }
        }
    }
}