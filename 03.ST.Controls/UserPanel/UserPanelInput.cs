using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.Controls
{
    public partial class UserPanel
    {
        public event EventHandler TitleChanged;

        private ContextMenuStrip TitleMenuStrip = new ContextMenuStrip();
        private TextBox TitleEditor = new TextBox();

        private void LoadInput()
        {
            TitleEditor.BorderStyle = BorderStyle.None;
            TitleEditor.BackColor = TitleBackColor[UserPanelTitleStateType.FocusedSelected];
            TitleEditor.Font = Font;
            TitleEditor.KeyDown += TitleEditor_KeyDown;
            TitleEditor.LostFocus += TitleEditor_LostFocus;
            TitleEditor.Visible = false;
            Controls.Add(TitleEditor);

            MouseDown += UserPanel_MouseDown;
            MouseMove += UserPanel_MouseMove;
            MouseUp += UserPanel_MouseUp;
            MouseLeave += UserPanel_MouseLeave;
            MouseDoubleClick += UserPanel_MouseDoubleClick;

            TitleMenuStrip.Items.Add("Rename(&R)", null, TitleMenuStripRename_Click);
            TitleMenuStrip.Items[TitleMenuStrip.Items.Count - 1].Name = "TitleMenuStripRename";
            TitleMenuStrip.Items.Add("Close(&C)", null, TitleMenuStripClose_Click);
            TitleMenuStrip.Items[TitleMenuStrip.Items.Count - 1].Name = "TitleMenuStripClose";
        }

        private void TitleEditor_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox _this = (TextBox)sender;
            switch(e.KeyCode)
            {
                case Keys.Return:
                    // validation 필요
                    UserPanelTitleInfo info = GetSelectedTitleInfo();
                    string value = _this.Text.Trim();
                    if (value.Length == 0)
                    {
                        ModalMessageBox.Show("Please enter the tab title.", "Panel");
                    }
                    else
                    {
                        bool isTitleChanged = info.Title != value;

                        info.Title = value;
                        if (info.Own)
                        {
                            Title = value;
                            TitleChanged?.Invoke(this, new EventArgs());
                        }
                        else
                        {
                            var child = GetChildPanel(info.GUID);
                            child.Title = value;
                            child.TitleChanged?.Invoke(this, new EventArgs());
                        }
                        _this.Visible = false;
                    }
                    e.SuppressKeyPress = true;
                    Refresh();
                    break;
                case Keys.Escape:
                    _this.Text = string.Empty;
                    _this.Visible = false;
                    e.SuppressKeyPress = true;
                    break;
            }
        }

        private void TitleEditor_LostFocus(object sender, EventArgs e)
        {
            TextBox _this = (TextBox)sender;
            _this.Text = string.Empty;
            _this.Visible = false;
        }

        private void UserPanel_MouseDown(object sender, MouseEventArgs e)
        {
            // Clear
            ClearMouseEventRef();

            // Title
            if (TitleVisable && e.Y < TitleHeight)
            {
                var sortedTitles = (from title in TitleList orderby title.Sort select title).ToList();
                int addedWidth = 0;
                int index = 0;
                for(int i = 0 + TitleStartIndex; i < sortedTitles.Count; i++)
                {
                    UserPanelTitleInfo titleNode = sortedTitles[i];

                    Size textSize = TextRenderer.MeasureText(titleNode.Title, Font);
                    int titleRight = addedWidth + textSize.Width + TitleWidthRevision + TitleDistance;

                    Rectangle TitleRect = new Rectangle(TitlePadding.Left + addedWidth, TitlePadding.Top
                        , textSize.Width + TitleWidthRevision, TitleHeight - TitlePadding.Vertical);

                    if (titleRight + TitleAreaRightPadding > Width && index > 0)
                    {
                        if (!UsingTitleSlider)
                        {
                            break;
                        }
                    }
                    else if (titleRight + TitleAreaRightPadding - TitleDistance > Width && index == 0)
                    {
                        TitleRect.Width = Width - TitleAreaRightPadding;
                        var targetTextSize = TitleRect.Width - TitleWidthRevision;
                    }

                    if (addedWidth <= e.X && e.X <= titleRight)
                    {
                        var closeRecArea = new Rectangle(
                            titleRight + TitleCloseButtonRectangle.Left - TitleCloseButtonAreaRevision, TitleCloseButtonRectangle.Top - TitleCloseButtonAreaRevision
                            , TitleCloseButtonRectangle.Width + TitleCloseButtonAreaRevision * 2, TitleCloseButtonRectangle.Height + TitleCloseButtonAreaRevision * 2);

                        // Slider
                        Rectangle sliderRec = new Rectangle(Width - TitleSliderWidth, 0, TitleSliderWidth, 23);
                        if (!UsingTitleSlider || (UsingTitleSlider && !sliderRec.Contains(e.Location)))
                        {
                            // Title Close Area
                            if (closeRecArea.Contains(e.Location))
                            {
                                TitleGUIDToClose = titleNode.GUID;
                                MouseDownArea = UserPanelMouseDownArea.TitleClose;
                                MouseAction.Enable = false;
                            }
                            // Title Area
                            else
                            {
                                MouseDownArea = UserPanelMouseDownArea.Title;
                                SelectTitle(titleNode.GUID);
                                if (TitleList.Count > 1)
                                {
                                    MouseAction.Enable = false;
                                }
                            }
                        }

                        break;
                    }

                    addedWidth = titleRight;
                    index++;
                }

                if (Name == "userPanel1")
                {
                    Console.WriteLine(String.Format("{0} {1} {2}", MethodBase.GetCurrentMethod().Name, Name, cnt++));
                }
            }
            // Body
            else
            {
                if (Dock != DockStyle.Fill)
                {
                    MouseDownArea = UserPanelMouseDownArea.Body;
                    // Call Parent Event
                    UserPanel parent = Parent as UserPanel;
                    if (Dock != DockStyle.None && parent != null && parent.Dock == DockStyle.None)
                    {
                        MouseAction.Enable = false;
                        parent.MouseAction.Enable = true;
                        parent.OnMouseDown(new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y + TitleHeight, e.Delta));
                    }
                }
            }
        }

        private void UserPanel_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (e.Button == MouseButtons.Left)
            {
                // Title Area
                switch (MouseDownArea)
                {
                    case UserPanelMouseDownArea.Title:
                        if (ExitingPanel != null)
                        {
                            Point exitingPanelPoint = ExitingPanel.PointToClient(Cursor.Position);
                            ExitingPanel.OnMouseMove(new MouseEventArgs(MouseButtons.Left, 0, exitingPanelPoint.X, exitingPanelPoint.Y, 0));
                        }
                        else if (BecomingParentPanel != null)
                        {
                            Point becomingParentPanelPoint = BecomingParentPanel.PointToClient(Cursor.Position);
                            BecomingParentPanel.OnMouseMove(new MouseEventArgs(MouseButtons.Left, 0, becomingParentPanelPoint.X, becomingParentPanelPoint.Y, 0));
                        }
                        else
                        {
                            var selectedTitleInfo = GetSelectedTitleInfo();
                            var titleRec = GetSelectedTitleRectangle();

                            if (titleRec.Width > 0 && titleRec.Height > 0)
                            {
                                if (TitleList.Count > 1)
                                {
                                    if
                                    (
                                        (e.Y < 0 - SplitRevisionY || TitleHeight + SplitRevisionY < e.Y)
                                        || (selectedTitleInfo.Sort == 0 && e.X < titleRec.Left)
                                        || (selectedTitleInfo.Sort == TitleList.Count - 1 && titleRec.Right < e.X)
                                    )
                                    {
                                        if (UsingPanelMerge && Dock != DockStyle.Fill)
                                        {
                                            ExitPanel();
                                        }
                                    }
                                    else if (e.X < titleRec.Left)
                                    {
                                        AddSelectedTitleSort(-1);
                                        titleRec = GetSelectedTitleRectangle();
                                        if (titleRec.Right < e.X)
                                        {
                                            AddSelectedTitleSort(1);
                                        }
                                    }
                                    else if (titleRec.Right < e.X)
                                    {
                                        AddSelectedTitleSort(1);
                                        titleRec = GetSelectedTitleRectangle();
                                        if (e.X < titleRec.Left)
                                        {
                                            AddSelectedTitleSort(-1);
                                        }
                                        else
                                        {
                                            if (!IsTitleDisplayedNotUsingTitleSlider(selectedTitleInfo.GUID))
                                            {
                                                AddSelectedTitleSort(-1);
                                            }
                                        }
                                    }

                                    if (selectedTitleInfo.Sort != GetSelectedTitleSort())
                                    {
                                        Draw();
                                    }
                                }
                                else
                                {
                                    if (UsingPanelMerge)
                                    {
                                        int revisionTitleWidth = Convert.ToInt32(GetTitleRectangle().Width * MregeRevisionTitleWidth);
                                        foreach (Control control in Parent.Controls)
                                        {
                                            UserPanel panel = control as UserPanel;
                                            if (panel != null && !panel.Equals(this))
                                            {
                                                Point panelMousePoint = panel.PointToClient(Cursor.Position);
                                                if (0 < panelMousePoint.Y + MergeRevisionY && panelMousePoint.Y < panel.TitleHeight + MergeRevisionY
                                                    && 0 < panelMousePoint.X + MergeRevisionX && panelMousePoint.X < panel.GetTitleRectangle().Right + revisionTitleWidth)
                                                {
                                                    int sort = GetTitleSortToBeAdded(panelMousePoint.X, revisionTitleWidth);
                                                    panel.AddPanel(this, sort);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case UserPanelMouseDownArea.TitleClose:
                        break;
                    case UserPanelMouseDownArea.Body:
                        // Call Parent Event
                        UserPanel parent = Parent as UserPanel;
                        if (Dock != DockStyle.None && parent != null && parent.Dock == DockStyle.None)
                        {
                            parent.OnMouseMove(new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y + TitleHeight, e.Delta));
                        }
                        break;
                }
            }
            
            Draw();
        }

        private int GetSelectedTitleSort()
        {
            int rs = -1;
            for (int i = 0; i < TitleList.Count; i++)
            {
                if (TitleList[i].Selected)
                {
                    rs = TitleList[i].Sort;
                }
            }
            return rs;
        }

        private void UserPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Title Close
                if (MouseDownArea == UserPanelMouseDownArea.TitleClose)
                {
                    UserPanelTitleInfo titleInfo;
                    Rectangle? titleRec = GetTitleInfoRectangle(e.Location, out titleInfo);
                    if (titleRec != null)
                    {
                        var closeRecArea = new Rectangle(
                                    ((Rectangle)titleRec).Right + TitleCloseButtonRectangle.Left - TitleCloseButtonAreaRevision, TitleCloseButtonRectangle.Top - TitleCloseButtonAreaRevision
                                    , TitleCloseButtonRectangle.Width + TitleCloseButtonAreaRevision * 2, TitleCloseButtonRectangle.Height + TitleCloseButtonAreaRevision * 2);
                        // Title Close Area
                        if (closeRecArea.Contains(new Point(e.X, e.Y)))
                        {
                            CloseTitle(titleInfo.GUID);
                        }
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (MouseDownArea == UserPanelMouseDownArea.Title || MouseDownArea == UserPanelMouseDownArea.TitleClose)
                {
                    UserPanelTitleInfo titleInfo = GetTitleInfo(e.Location);
                    if (titleInfo != null)
                    {
                        TitleMenuStrip.Items["TitleMenuStripRename"].Visible = UsingTitleRename;
                        TitleMenuStrip.Show(Cursor.Position);
                    }
                }
            }

            ClearMouseEventRef();
        }

        private void UserPanel_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            ClearMouseEventRef();
            Draw(isMouseLeave: true);
        }

        private void UserPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseDownArea == UserPanelMouseDownArea.Title && UsingMaximize)
            {
                IsFill = !IsFill;
                if (IsFill)
                {
                    BringToFront();
                    Dock = DockStyle.Fill;
                }
                else
                {
                    Dock = DockStyle.None;
                }
            }
        }

        private void TitleMenuStripRename_Click(object sender, EventArgs e)
        {
            ShowTitleEditor();
        }

        private void TitleMenuStripClose_Click(object sender, EventArgs e)
        {
            CloseTitle(GetSelectedTitleInfo().GUID);
        }

        #region Function Main
        public void ShowTitleEditor()
        {
            int revisionX = 6, revisionY = 7, revisionWidth = -10;
            Rectangle rec = GetSelectedTitleRectangle();
            TitleEditor.Text = GetSelectedTitleInfo().Title;
            TitleEditor.Location = new Point(rec.Location.X + revisionX, rec.Location.Y + revisionY);
            TitleEditor.Size = new Size(rec.Size.Width + revisionWidth, rec.Size.Height);
            TitleEditor.Visible = true;
            TitleEditor.SelectionStart = 0;
            TitleEditor.SelectionLength = TitleEditor.TextLength;
            TitleEditor.Focus();
        }
        #endregion

        #region Function
        private void ClearMouseEventRef()
        {
            if (Dock != DockStyle.Fill)
            {
                MouseAction.Enable = true;
            }

            MouseDownArea = UserPanelMouseDownArea.None;
            TitleGUIDToClose = string.Empty;

            if (ExitingPanel != null && ExitingPanel.Dock != DockStyle.Fill)
            {
                ExitingPanel.MouseAction.Enable = true;
            }
            ExitingPanel = null;

            if (BecomingParentPanel != null && BecomingParentPanel.Dock != DockStyle.Fill)
            {
                BecomingParentPanel.MouseAction.Enable = true;
            }
            BecomingParentPanel = null;
        }

        private UserPanelTitleInfo GetTitleInfo(Point location)
        {
            UserPanelTitleInfo rs = null;
            GetTitleInfoRectangle(location, out rs);
            return rs;
        }

        private Rectangle? GetTitleInfoRectangle(Point location, out UserPanelTitleInfo titleInfo)
        {
            Rectangle? rs = null;
            titleInfo = null;

            if (TitleVisable && location.Y < TitleHeight)
            {
                var sortedTitles = (from title in TitleList orderby title.Sort select title).ToList();
                int addedWidth = 0;
                for (int i = 0 + TitleStartIndex; i < sortedTitles.Count; i++)
                {
                    UserPanelTitleInfo titleNode = sortedTitles[i];

                    Size textSize = TextRenderer.MeasureText(titleNode.Title, Font);
                    int titleRight = addedWidth + textSize.Width + TitleWidthRevision + TitleDistance;
                    if (addedWidth <= location.X && location.X <= titleRight)
                    {
                        titleInfo = titleNode;
                        rs = new Rectangle(addedWidth, 0, textSize.Width + TitleWidthRevision, TitleHeight);
                        break;
                    }
                    
                    addedWidth = titleRight;
                }
            }

            return rs;
        }
        #endregion
    }
}
