using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public partial class GraphicPanel
    {
        private bool TitleOverFlow = false;

        private void LoadDraw()
        {
            Paint += UserNodeBase_Paint;
        }

        private void UserNodeBase_Paint(object sender, PaintEventArgs e)
        {
            if (BtDelete != null)
            {
                Graphics g = e.Graphics;
                g.Clear(BackColor);
                //g.FillRectangle(new SolidBrush(Color.Gray), new Rectangle(0, 0, Width, Height));
                
                if (g == null)
                {
                    // 임시
                    ((DataModeler)Target).Refresh();
                    return;
                    //g = Target.CreateGraphics();
                }

                // Draw Title - Get scaleFont, leftNtop
                int _titleHeightAPaddingTop = TitleHeight + Padding.Top;
                Font scaleFont = new Font(Font.FontFamily, Font.Size * ScaleValue
                    , (TitleBold ? FontStyle.Bold : FontStyle.Regular)
                );
                var scaleFontSize = g.MeasureString(Title, scaleFont);
                float leftNtop = (_titleHeightAPaddingTop / 2) - (scaleFontSize.Height / 2);

                // Draw Title - Get viewTitle
                string viewTitle = Title;

                var width = Width;
                if (ShowBtContextMenu && BtContextMenu.Enabled)
                {
                    width = Width + BtContextMenu.Area.Left;
                }
                else if (BtDelete.Enabled)
                {
                    width = Width + BtDelete.Area.Left; 
                }
                SizeF titlePixelSize = scaleFontSize;
                if (width < titlePixelSize.Width)
                {
                    viewTitle.Substring(0, viewTitle.Length - 1);
                    do
                    {
                        viewTitle = viewTitle.Substring(0, viewTitle.Length - 1);
                        titlePixelSize = g.MeasureString(viewTitle + "...", scaleFont);
                    } while (width < titlePixelSize.Width);
                    viewTitle = viewTitle + "...";
                    TitleOverFlow = true;
                }
                else
                {
                    TitleOverFlow = false;
                }

                // Draw Title - Draw
                g.DrawString(viewTitle, scaleFont, new SolidBrush(ForeColor), leftNtop, leftNtop);

                // Draw DeleteButton
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                //int marginTotal = (4 * ScaleValue).ToInt();
                //int scaleAreaSideLength = TitleHeight + Padding.Top - marginTotal;
                //BtDelete.SetArea(new Rectangle(-(scaleAreaSideLength + (marginTotal / 2)), (marginTotal / 2), scaleAreaSideLength, scaleAreaSideLength));

                BtDelete.Draw(g);
                if (ShowBtContextMenu)
                {
                    BtContextMenu.Draw(g);
                }

                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            }
        }
    }
}
