using ST.Controls;
using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.CodeGenerator
{
    public partial class CodeGenerator
    {
        // Option
        public Color DisableColor = Color.FromArgb(60, 0, 0, 0);

        // Common Variables
        private int CommonVariablesTitleHeight = 26;
        private Color CommonVariablesTitleBackColor = Color.FromArgb(93, 107, 153);
        private Color CommonVariablesTitleBottomBorderColor = Color.FromArgb(142, 152, 187);
        private Point CommonVariablesTitlePoint = new Point(30, 4);
        private Color CommonVariablesTitleFontColor = Color.FromArgb(255, 255, 255);
        private Font CommonVariablesTitleFont = new Font("맑은 고딕", 9f);
        private string CommonVariablesTitle = "Common Variables";

        private void LoadDraw()
        {
            // This
            MouseLeave += CodeGenerator_MouseLeave;

            // Common Variables
            MainSplit.Panel1.Paint += DrawMainSplitPanel1_Paint;
            MainSplit.Panel1.MouseMove += DrawMainSplitPanel1_MouseMove;
            MainSplit.Panel1.MouseLeave += DrawMainSplitPanel1_MouseLeave;
        }

        #region This
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Draw(e.Graphics);
        }

        private void CodeGenerator_MouseLeave(object sender, EventArgs e)
        {
            Draw(CreateGraphics(), GraphicControl.StateType.Default);
        }

        private void Draw(Graphics graphics = null, GraphicControl.StateType stateType = GraphicControl.StateType.None)
        {
            if (Visible && 0 < ClientRectangle.Width && 0 < ClientRectangle.Height)
            {
                if (graphics == null)
                {
                    graphics = CreateGraphics();
                }

                BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
                using (BufferedGraphics buffer = currentContext.Allocate(graphics, new Rectangle(0, 0, Width, Height)))
                {
                    var g = buffer.Graphics;
                    g.Clear(BackColor);
                    g.FillRectangle(new SolidBrush(MenuBackColor), new Rectangle(0, 0, Width, MenuHeight));

                    foreach(var info in GraphicButtonInfoList)
                    {
                        if (info.Visible)
                        {
                            info.Button.Draw(g, stateType);
                        }
                    }

                    if (stateType != GraphicControl.StateType.None)
                    {
                        ButtonStyle.Draw(g, stateType);
                    }
                    else
                    {
                        ButtonStyle.Draw(g);
                    }

                    if (!Enabled)
                    {
                        g.FillRectangle(new SolidBrush(DisableColor), new Rectangle(0, 0, Width, Height));
                    }

                    buffer.Render(graphics);
                }
            }
        }
        #endregion

        #region Common Variables
        private void DrawMainSplitPanel1_Paint(object sender, PaintEventArgs e)
        {
            DrawMainSplitPanel1(e.Graphics);
        }

        private void DrawMainSplitPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            DrawMainSplitPanel1();
        }

        private void DrawMainSplitPanel1_MouseLeave(object sender, EventArgs e)
        {
            DrawMainSplitPanel1(MainSplit.Panel1.CreateGraphics(), GraphicControl.StateType.Default);
        }

        private void DrawMainSplitPanel1(Graphics graphics = null, GraphicControl.StateType stateType = GraphicControl.StateType.None)
        {
            if (graphics == null)
            {
                graphics = MainSplit.Panel1.CreateGraphics();
            }

            if (Visible && MainSplit.Visible && 0 < MainSplit.Panel1.Width && 0 < MainSplit.Panel1.Height)
            {
                BufferedGraphicsContext currentContext = new BufferedGraphicsContext();

                using (BufferedGraphics buffer = currentContext.Allocate(graphics, new Rectangle(0, 0, MainSplit.Panel1.Width, CommonVariablesTitleHeight)))
                {
                    var g = buffer.Graphics;
                    g.Clear(BackColor);
                    g.FillRectangle(new SolidBrush(CommonVariablesTitleBackColor), new Rectangle(0, 0, MainSplit.Panel1.Width, CommonVariablesTitleHeight));
                    g.DrawLine(new Pen(CommonVariablesTitleBottomBorderColor), 0, CommonVariablesTitleHeight - 1, Width, CommonVariablesTitleHeight - 1);

                    CollapseButton.Draw(g);

                    // Get drawString
                    string drawString = CommonVariablesTitle;
                    if (MainSplit.SplitterDistance == CommonVariablesTitleHeight && CommonVariablesAbstact.Length > 0)
                    {
                        drawString += ": " + CommonVariablesAbstact;
                    }

                    StringFormat format = new StringFormat();
                    format.Trimming = StringTrimming.EllipsisCharacter;
                    g.DrawString(drawString, CommonVariablesTitleFont, new SolidBrush(CommonVariablesTitleFontColor), new Rectangle(30, 4, MainSplit.Panel1.Width - 40, 19), format);
                    format.Dispose();

                    if (!Enabled)
                    {
                        g.FillRectangle(new SolidBrush(DisableColor), 0, 0, Width, Height);
                    }

                    buffer.Render(graphics);
                }
            }
        }
        #endregion
    }
}
