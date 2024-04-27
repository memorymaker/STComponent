using ST.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.CodeGenerator
{
    public partial class Tab
    {
        public Color DisableColor = Color.FromArgb(60, 0, 0, 0);

        private void LoadDraw()
        {
            MainSplit.Panel2.Paint += Panel2_Paint;
            MainSplit.Panel2.MouseLeave += Panel2_MouseLeave;
        }

        private void Panel2_Paint(object sender, PaintEventArgs e)
        {
            DrawPanel2(e.Graphics);
        }

        private void Panel2_MouseLeave(object sender, EventArgs e)
        {
            DrawPanel2(MainSplit.Panel2.CreateGraphics(), GraphicControl.StateType.Default);
        }

        private void DrawPanel2(Graphics graphics = null, GraphicControl.StateType stateType = GraphicControl.StateType.None)
        {
            if (graphics == null)
            {
                graphics = MainSplit.Panel2.CreateGraphics();
            }

            if (Visible && MainSplit.Visible && 0 < MainSplit.Panel2.Width && 0 < MainSplit.Panel2.Height)
            {
                BufferedGraphicsContext currentContext = new BufferedGraphicsContext();
                
                using (BufferedGraphics buffer = currentContext.Allocate(graphics, new Rectangle(0, 0, MainSplit.Panel2.Width, ResultMenuHeight)))
                {
                    var g = buffer.Graphics;
                    g.Clear(BackColor);
                    g.FillRectangle(new SolidBrush(ResultMenuBackColor), new Rectangle(0, 0, MainSplit.Panel2.Width, ResultMenuHeight));

                    if (stateType != GraphicControl.StateType.None)
                    {
                        CopyButton.Draw(g, stateType);
                    }
                    else
                    {
                        CopyButton.Draw(g);
                    }
                    
                    if (!Enabled)
                    {
                        g.FillRectangle(new SolidBrush(DisableColor), 0, 0, Width, Height);
                    }

                    buffer.Render(graphics);
                }
            }
        }
    }
}
