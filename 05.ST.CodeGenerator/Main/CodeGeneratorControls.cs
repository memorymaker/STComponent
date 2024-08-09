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
        // Controls
        private TabMainSpllit MainSplit;
        private GraphicControl CollapseButton;
        private UserEditor CommonVariablesEditor;
        private Color MainSplitterColor = Color.FromArgb(93, 107, 153);
        private DrawingPanel ErrorListWrapPanel;
        private UserListView ErrorList;

        // Constans
        private int MainSplitPanel1WithErrorListMinSize = 121;
        private int MainSplitPanel1MinSize = 59;
        
        // Ref
        private int OldMainSplitSplitterDistance = 0;

        /// <summary>
        /// Common Variables 영역과 Tab 영역 사이의 간격의 가져오거나 설정합니다.
        /// </summary>
        public int MainSplitterDistance
        {
            get
            {
                return MainSplit.SplitterDistance;
            }
            set
            {
                MainSplit.SplitterDistance = value;
            }
        }

        private void LoadControls()
        {
            LoadControls_SetControls();
            LoadControls_SetEvents();
        }

        private void LoadControls_SetControls()
        {
            MainSplit = new TabMainSpllit();
            MainSplit.Orientation = Orientation.Horizontal;
            MainSplit.Dock = DockStyle.Fill;
            MainSplit.SplitterColor = MainSplitterColor;
            MainSplit.Panel1.Padding = new Padding(0, CommonVariablesTitleHeight, 0, 0);
            MainSplit.FixedPanel = FixedPanel.Panel1;
            MainSplit.Panel1MinSize = CommonVariablesTitleHeight;
            MainSplit.SplitterDistance = 150;
            MainSplit.SplitterDistanceChanging += MainSplit_SplitterDistanceChanging;

            // PanTopLeft - CollapseButton
            CollapseButton = new ST.Controls.GraphicControl(MainSplit.Panel1, "CollapseButton");
            CollapseButton
                .SetArea(new Rectangle(3, 3, CommonVariablesTitleHeight - 5, CommonVariablesTitleHeight - 6))
                .SetDrawType(ST.Controls.GraphicControl.DrawTypeEnum.Image)
                .SetDrawFont(new Font("맑은 고딕", 9))
                .SetDrawInfo(ST.Controls.GraphicControl.StateType.Default, new ST.Controls.GraphicControl.DrawInfo()
                {
                      DrawBackColor = CommonVariablesTitleBackColor
                    , DrawTextColor = Color.FromArgb(90, 90, 90)
                    , DrawImage = Properties.Resources.CollapseCloseWhite
                    , DrawBorderColor = Color.FromArgb(230, 230, 230)
                    , LeftRevision = 1
                    , TopRevision = 1
                })
                .SetDrawInfo(ST.Controls.GraphicControl.StateType.Over, new ST.Controls.GraphicControl.DrawInfo()
                {
                      DrawBackColor = CommonVariablesTitleBackColor.GetColor(0.2f)
                    , DrawTextColor = Color.FromArgb(60, 60, 60)
                    , DrawImage = Properties.Resources.CollapseCloseWhite
                    , DrawBorderColor = Color.FromArgb(255, 255, 255)
                    , LeftRevision = 1
                    , TopRevision = 1
                });

            CommonVariablesEditor = new UserEditor();
            CommonVariablesEditor.Dock = DockStyle.Fill;
            MainSplit.Panel1.Controls.Add(CommonVariablesEditor);

            ErrorListWrapPanel = new DrawingPanel();
            ErrorListWrapPanel.Visible = false;
            ErrorListWrapPanel.Dock = DockStyle.Bottom;
            ErrorListWrapPanel.Height = 64;
            ErrorListWrapPanel.BorderTopWidth = 1;
            ErrorListWrapPanel.BorderTopColor = Color.FromArgb(230, 230, 230);
            MainSplit.Panel1.Controls.Add(ErrorListWrapPanel);

            ErrorList = new UserListView();
            ErrorList.Dock = DockStyle.Fill;
            ErrorList.Height = 63;
            ErrorList.ColumnHeight = 20;
            ErrorList.BackColor = Color.FromArgb(249, 249, 249);
            ErrorList.AddColumn(new UserListViewColumn("Error Description", "DESCRIPTON"));
            ErrorList.AddColumn(new UserListViewColumn("Line", "LINE"));
            ErrorList.AddColumn(new UserListViewColumn("Column", "COLUMN"));
            ErrorList.Columns[1].ItemAlign = UserListAlignType.Center;
            ErrorList.Columns[2].ItemAlign = UserListAlignType.Center;
            ErrorListWrapPanel.Controls.Add(ErrorList);

            Controls.Add(MainSplit);
        }

        private void MainSplit_SplitterDistanceChanging(object sender, UserSplitContainer.SplitterDistanceChangingEventArgs e)
        {
            int minDistance = ErrorList.Visible ? MainSplitPanel1WithErrorListMinSize : MainSplitPanel1MinSize;
            if (e.SplitterDistance < minDistance)
            {
                e.SplitterDistance = MainSplit.SplitterDistance == CommonVariablesTitleHeight
                    ? minDistance : CommonVariablesTitleHeight;
            }
        }

        private void LoadControls_SetEvents()
        {
            MainSplit.Panel1.SizeChanged += Panel1_SizeChanged;

            CollapseButton.Click += CollapseButton_Click;

            SizeChanged += CodeGenerator_SizeChanged1;
        }

        private void Panel1_SizeChanged(object sender, EventArgs e)
        {
            int minDistance = ErrorList.Visible ? MainSplitPanel1WithErrorListMinSize : MainSplitPanel1MinSize;
            if (MainSplit.SplitterDistance < minDistance)
            {
                CollapseButton.SetDrawImage(ST.Controls.GraphicControl.StateType.Default, Properties.Resources.CollapseOpenWhite);
                CollapseButton.SetDrawImage(ST.Controls.GraphicControl.StateType.Over, Properties.Resources.CollapseOpenWhite);

            }
            else
            {
                CollapseButton.SetDrawImage(ST.Controls.GraphicControl.StateType.Default, Properties.Resources.CollapseCloseWhite);
                CollapseButton.SetDrawImage(ST.Controls.GraphicControl.StateType.Over, Properties.Resources.CollapseCloseWhite);
            }
        }

        private void CollapseButton_Click(object sender, MouseEventArgs e)
        {
            int minDistance = ErrorList.Visible ? MainSplitPanel1WithErrorListMinSize : MainSplitPanel1MinSize;
            if (MainSplit.SplitterDistance > minDistance)
            {
                OldMainSplitSplitterDistance = MainSplit.SplitterDistance;
                MainSplit.SplitterDistance = CommonVariablesTitleHeight;
            }
            else
            {
                MainSplit.SplitterDistance = OldMainSplitSplitterDistance > 0
                    ? OldMainSplitSplitterDistance : MainSplitPanel1WithErrorListMinSize;
                OldMainSplitSplitterDistance = 0;
            }
        }

        private void CodeGenerator_SizeChanged1(object sender, EventArgs e)
        {
            ErrorList.Columns[0].Width = Width - 160 - ErrorList.VScrollBarWidth;
            ErrorList.Columns[1].Width = 80;
            ErrorList.Columns[2].Width = 80;
        }
    }
}
