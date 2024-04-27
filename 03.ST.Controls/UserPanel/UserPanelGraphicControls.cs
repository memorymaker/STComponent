using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.Controls
{
    public partial class UserPanel
    {
        private void LoadGraphicControls()
        {
            ViewContextMenuButton = new GraphicControl(this, "Details");
            ViewContextMenuButton
                .SetArea(new Rectangle(-12 - UserPadding.Right, 3 + UserPadding.Top, 9, 9))
                .SetDrawType(GraphicControl.DrawTypeEnum.FillPolygon)
                .SetDrawPositionPercent(new PointF[] { new PointF(0.1f, 0.1f), new PointF(0.5f, 0.9f), new PointF(0.9f, 0.1f) })
                .SetDrawColor(GraphicControl.StateType.Default, Color.FromArgb(160, 172, 210))
                .SetDrawColor(GraphicControl.StateType.Over, Color.FromArgb(255, 255, 255));
            ViewContextMenuButton.Click += ViewContextMenuButton_Click;
            ViewContextMenuButton.MouseDown += ViewContextMenuButton_MouseDown;

            AwaysOnTopMenuButton = new GraphicControl(this, "Aways On Top");
            AwaysOnTopMenuButton
                .SetArea(new Rectangle(-27 - UserPadding.Right, 3 + UserPadding.Top, 9, 9))
                .SetDrawType(GraphicControl.DrawTypeEnum.DrawLines)
                .SetDrawPositionPercent(new PointF[] {
                      new PointF(0.2f, 0.0f), new PointF(0.8f, 0.0f) // -
                    , new PointF(0.1f, 0.6f), new PointF(0.9f, 0.6f) // -
                    , new PointF(0.2f, 0.0f), new PointF(0.2f, 0.6f) // |
                    , new PointF(0.8f, 0.0f), new PointF(0.8f, 0.6f) // |
                    , new PointF(0.7f, 0.0f), new PointF(0.7f, 0.6f) // |
                    , new PointF(0.5f, 0.6f), new PointF(0.5f, 1.0f) // |
                })
                .SetDrawColor(GraphicControl.StateType.Default, Color.FromArgb(110, 132, 180))
                .SetDrawColor(GraphicControl.StateType.Over, Color.FromArgb(255, 255, 255));
            AwaysOnTopMenuButton.Click += AwaysOnTopMenuButton_Click;
            AwaysOnTopMenuButton.MouseDown += AwaysOnTopMenuButton_MouseDown;

            SlideLeftMenuButton = new GraphicControl(this, "Slide Left");
            SlideLeftMenuButton
                .SetArea(new Rectangle(-42, 2, 20, 20))
                .SetDrawType(GraphicControl.DrawTypeEnum.FillPolygon)
                .SetDrawPositionPercent(new PointF[] {
                      new PointF(0.2f, 0.5f) // ◀
                    , new PointF(0.8f, 0.2f)
                    , new PointF(0.8f, 0.8f)
                })
                .SetDrawColor(GraphicControl.StateType.Default, Color.FromArgb(232, 234, 240))
                .SetDrawColor(GraphicControl.StateType.Over, Color.FromArgb(255, 255, 255))
                .SetDrawBackColor(GraphicControl.StateType.Default, Color.FromArgb(115, 128, 170))
                .SetDrawBackColor(GraphicControl.StateType.Over, Color.FromArgb(105, 119, 165));
            SlideLeftMenuButton.MouseDown += SlideLeftMenuButton_MouseDown;

            SlideRightMenuButton = new GraphicControl(this, "Slide Right");
            SlideRightMenuButton
                .SetArea(new Rectangle(-21, 2, 20, 20))
                .SetDrawType(GraphicControl.DrawTypeEnum.FillPolygon)
                .SetDrawPositionPercent(new PointF[] {
                      new PointF(0.2f, 0.2f) // ▶
                    , new PointF(0.2f, 0.8f)
                    , new PointF(0.8f, 0.5f)
                })
                .SetDrawColor(GraphicControl.StateType.Default, Color.FromArgb(232, 234, 240))
                .SetDrawColor(GraphicControl.StateType.Over, Color.FromArgb(255, 255, 255))
                .SetDrawBackColor(GraphicControl.StateType.Default, Color.FromArgb(115, 128, 170))
                .SetDrawBackColor(GraphicControl.StateType.Over, Color.FromArgb(105, 119, 165));
            SlideRightMenuButton.MouseDown += SlideRightMenuButton_MouseDown;
        }

        private void ViewContextMenuButton_Click(object sender, MouseEventArgs e)
        {
            if (ContextMenu.Visible)
            {
                ContextMenu.Close();
            }
            else
            {
                var titleList = new List<UserPanelContextMenuTitleInfo>();
                var sortedTitles = from title in TitleList orderby title.Sort select title;
                foreach (UserPanelTitleInfo titleNode in sortedTitles)
                {
                    titleList.Add(new UserPanelContextMenuTitleInfo(titleNode.Title, titleNode));
                }
                ContextMenu.Show(((GraphicControl)sender).Parent, new Point(e.X, e.Y), titleList, UserPanelContextShowPosition.TopRight);
            }
        }

        private void ViewContextMenuButton_MouseDown(object sender, MouseEventArgs e)
        {
            MouseAction.Enable = false;
        }

        private void AwaysOnTopMenuButton_Click(object sender, MouseEventArgs e)
        {
            AwaysOnTop = !AwaysOnTop;
            BringToFrontCustom();
            BringToFrontCustom();
        }

        private void AwaysOnTopMenuButton_MouseDown(object sender, MouseEventArgs e)
        {
            MouseAction.Enable = false;
        }

        private void SlideLeftMenuButton_MouseDown(object sender, MouseEventArgs e)
        {
            TitleStartIndex--;
            Draw();
        }

        private void SlideRightMenuButton_MouseDown(object sender, MouseEventArgs e)
        {
            TitleStartIndex++;
            Draw();
        }
    }
}
