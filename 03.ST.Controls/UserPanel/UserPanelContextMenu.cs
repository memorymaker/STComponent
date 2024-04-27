using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.TextFormatting;

namespace ST.Controls
{
    public partial class UserPanel
    {
        public class UserPanelContextMenu : Control
        {
            // Option
            public int MainBorderWidth = 1;
            public Color MainBorderColor = Color.FromArgb(148, 166, 202);
            public Padding MainPadding = new Padding(2);
            public Color DefaultBackgroundColor = Color.FromArgb(233, 238, 255);

            public Padding TitlePadding = new Padding(4, 4, 40, 2);
            public int TitleMinWidth = 50;
            public Color TitleBackgroundColor = Color.FromArgb(233, 238, 255);
            public Color SelectedTitleBackgroundColor = Color.FromArgb(177, 197, 255);
            public Color TitleFontColor = Color.FromArgb(60, 60, 60);
            public Color SelectedTitleFontColor = Color.FromArgb(60, 60, 60);

            // Public
            public List<UserPanelContextMenuTitleInfo> TitleList;

            // Events
            public delegate void UserPanelContextMenuItemEventHandler(object sender, UserPanelContextMenuItemEventArgs e);
            public event UserPanelContextMenuItemEventHandler ItemClick;
            public event UserPanelContextMenuItemEventHandler ItemCloseClick;

            // System Option
            private Rectangle TitleCloseButtonRectangle = new Rectangle(-15, 5, 9, 9);

            // Ref
            private Control Owner;
            private int MouseDownSelectedIndex = -1;
            private bool MouseDownIsCloseButton = false;

            // Etc
            GlobalMouseHandler GlobalMouseDown;

            private int _SelectedIndex;
            public int SelectedIndex
            {
                get
                {
                    return _SelectedIndex;
                }
                set
                {
                    _SelectedIndex = value;
                    OnPaint();
                }
            }

            private int InnerLeft
            {
                get => MainBorderWidth + MainPadding.Left;
            
            }

            private int InnerTop
            {
                get => MainBorderWidth + MainPadding.Top;
            }

            public void Show(Control parent, Point position, List<UserPanelContextMenuTitleInfo> titleList, UserPanelContextShowPosition showPosition = UserPanelContextShowPosition.TopLeft)
            {
                TitleList = titleList;
                Visible = false;
                var parentForm = GetParentForm(parent);
                parentForm.Controls.Add(this);

                SetSize();
                Location = ShowProcGetPosition(parentForm.PointToClient(parent.PointToScreen(position)), Size, showPosition);
                BringToFront();
                Visible = true;
                Focus();
            }

            private Point ShowProcGetPosition(Point position, Size size, UserPanelContextShowPosition showPosition)
            {
                var showPositionValue = ((int)showPosition).ToString();
                int rsX = position.X;
                int rsY = position.Y;

                switch (showPositionValue[0])
                {
                    case '2': rsY -= size.Height / 2; break;
                    case '3': rsY -= size.Height; break;
                }

                switch (showPositionValue[1])
                {
                    case '2': rsX -= size.Width / 2; break;
                    case '3': rsX -= size.Width; break;
                }

                return new Point(rsX, rsY);
            }

            public UserPanelContextMenu(Control owner)
            {
                Owner = owner;
                LoadThis();
            }

            private void LoadThis()
            {
                SetDefault();
                SetEvents();
            }

            private void SetDefault()
            {
                TabStop = true;
                Visible = false;
                GlobalMouseDown = new GlobalMouseHandler(this);
                Application.AddMessageFilter(GlobalMouseDown);
                BackColor = DefaultBackgroundColor;
            }

            private void SetEvents()
            {
                KeyDown += UserPanelContextMenu_KeyDown;
                MouseDown += UserPanelContextMenu_MouseDown;
                MouseMove += UserPanelContextMenu_MouseMove;
                MouseUp += UserPanelContextMenu_MouseUp;
                MouseLeave += UserPanelContextMenu_MouseLeave;
                LostFocus += UserPanelContextMenu_LostFocus;
                Paint += UserPanelContextMenu_Paint;
            }

            private void UserPanelContextMenu_KeyDown(object sender, KeyEventArgs e)
            {
                switch(e.KeyCode)
                {
                    case Keys.Return:
                        SelectItem();
                        break;
                    case Keys.Up: IncreaseSelectedIndex(); break;
                    case Keys.Down: DecreaseSelectedIndex(); break;
                    case Keys.Escape:
                        Close();
                        break;
                }
            }

            private void UserPanelContextMenu_MouseDown(object sender, MouseEventArgs e)
            {
                MouseDownSelectedIndex = GetCurrentSelectedIndex(new Point(e.X, e.Y));
                MouseDownIsCloseButton = IsCloseButtonArea(MouseDownSelectedIndex, new Point(e.X, e.Y));
            }

            private void UserPanelContextMenu_MouseMove(object sender, MouseEventArgs e)
            {
                int newSelectedIndex = GetCurrentSelectedIndex(new Point(e.X, e.Y));
                if (newSelectedIndex != SelectedIndex)
                {
                    SelectedIndex = newSelectedIndex;
                    OnPaint();
                }
            }

            private void UserPanelContextMenu_MouseUp(object sender, MouseEventArgs e)
            {
                int mouseUpSelectedIndex = GetCurrentSelectedIndex(new Point(e.X, e.Y));
                bool mouseDownIsCloseButton = IsCloseButtonArea(MouseDownSelectedIndex, new Point(e.X, e.Y));
                if (MouseDownSelectedIndex == mouseUpSelectedIndex && MouseDownIsCloseButton == mouseDownIsCloseButton)
                {
                    if (!mouseDownIsCloseButton)
                    {
                        SelectItem();
                    }
                    else
                    {
                        CloseItem();
                    }
                }
            }

            private void SelectItem()
            {
                var eventArgs = new UserPanelContextMenuItemEventArgs(SelectedIndex, TitleList[SelectedIndex].Title, TitleList[SelectedIndex].Data, false, true);
                ItemClick?.Invoke(this, eventArgs);
                SelectItemOrCloseItemAfterEvents(eventArgs);
            }

            private void CloseItem()
            {
                var eventArgs = new UserPanelContextMenuItemEventArgs(SelectedIndex, TitleList[SelectedIndex].Title, TitleList[SelectedIndex].Data, true, false);
                ItemCloseClick?.Invoke(this, eventArgs);
                SelectItemOrCloseItemAfterEvents(eventArgs);
            }

            private void SelectItemOrCloseItemAfterEvents(UserPanelContextMenuItemEventArgs eventArgs)
            {
                if (eventArgs.RemoveThisTitle)
                {
                    TitleList.RemoveAt(eventArgs.SelectedIndex);
                    SetSize();
                    OnPaint();
                }

                if (eventArgs.CloseContextMenu)
                {
                    Close();
                }

                if (eventArgs.DisposeContextMenu)
                {
                    Dispose();
                }
            }

            public int GetCurrentSelectedIndex(Point mousePoint)
            {
                var rsSelectedIndex = -1;
                var g = CreateGraphics();
                int maxWidth = 0, maxHeight = 0;
                PushMaxWidhtNMaxHeight(g, TitleList, TitleMinWidth, ref maxWidth, ref maxHeight);

                int titleHeight = maxHeight + TitlePadding.Vertical;
                int titleWidth = maxWidth + TitlePadding.Horizontal;

                for (int i = 0; i < TitleList.Count; i++)
                {
                    Rectangle titleRec = new Rectangle(InnerLeft, InnerTop + i * titleHeight, titleWidth, titleHeight);
                    if (titleRec.Contains(mousePoint))
                    {
                        rsSelectedIndex = i;
                    }
                }
                return rsSelectedIndex;
            }

            public bool IsCloseButtonArea(int index, Point mousePoint)
            {
                if (index >= 0)
                {
                    var g = CreateGraphics();
                    int maxWidth = 0, maxHeight = 0;
                    PushMaxWidhtNMaxHeight(g, TitleList, TitleMinWidth, ref maxWidth, ref maxHeight);

                    int titleHeight = maxHeight + TitlePadding.Vertical;
                    int titleWidth = maxWidth + TitlePadding.Horizontal;

                    Point cursorPoint = PointToClient(Cursor.Position);
                    Rectangle titleRec = new Rectangle(InnerLeft, InnerTop + (index * titleHeight), titleWidth, titleHeight);
                    Rectangle closeRec = new Rectangle(
                        titleRec.Right + TitleCloseButtonRectangle.Left, titleRec.Top + TitleCloseButtonRectangle.Top
                        , TitleCloseButtonRectangle.Width, TitleCloseButtonRectangle.Height);

                    if (closeRec.Contains(cursorPoint))
                    {
                        return true;
                    }
                }
                return false;
            }

            private void UserPanelContextMenu_MouseLeave(object sender, EventArgs e)
            {
                SelectedIndex = -1;
            }

            private void UserPanelContextMenu_LostFocus(object sender, EventArgs e)
            {
                Close();
            }

            private void IncreaseSelectedIndex()
            {
                if (SelectedIndex < 0)
                {
                    SelectedIndex = 0;
                }
                else if (SelectedIndex > 0)
                {
                    SelectedIndex--;
                }
            }

            private void DecreaseSelectedIndex()
            {
                if (SelectedIndex < 0)
                {
                    SelectedIndex = TitleList.Count - 1;
                }
                else if (SelectedIndex < TitleList.Count - 1)
                {
                    SelectedIndex++;
                }
            }

            private void OnPaint()
            {
                UserPanelContextMenu_Paint(this, new PaintEventArgs(CreateGraphics(), Bounds));
            }

            private void UserPanelContextMenu_Paint(object sender, PaintEventArgs e)
            {
                if (Visible)
                {
                    var g = e.Graphics;

                    // MainPadding Bottom(For Delete)
                    g.FillRectangle(new SolidBrush(BackColor)
                        , new Rectangle(
                            MainBorderWidth, Bounds.Height - MainPadding.Bottom - MainBorderWidth
                            , Bounds.Width - (MainBorderWidth * 2), MainPadding.Bottom
                        )
                    );

                    // Border
                    g.DrawRectangle(new Pen(MainBorderColor, MainBorderWidth)
                        , new Rectangle(
                              new Point(MainBorderWidth - (MainBorderWidth < 3 ? 1 : 2), MainBorderWidth - (MainBorderWidth < 3 ? 1 : 2))
                            , new Size(Bounds.Width - MainBorderWidth, Bounds.Height - MainBorderWidth)
                        )
                    );

                    // Etc
                    int maxWidth = 0, maxHeight = 0;
                    PushMaxWidhtNMaxHeight(g, TitleList, TitleMinWidth, ref maxWidth, ref maxHeight);

                    Brush titleBackgroundBrush = new SolidBrush(TitleBackgroundColor);
                    Brush selectedTitleBackgroundColor = new SolidBrush(SelectedTitleBackgroundColor);
                    Brush titleFontBrush = new SolidBrush(TitleFontColor);
                    Brush selectedTitleFontBrush = new SolidBrush(SelectedTitleFontColor);

                    int titleHeight = maxHeight + TitlePadding.Vertical;
                    int titleWidth = maxWidth + TitlePadding.Horizontal;

                    Point cursorPoint = PointToClient(Cursor.Position);

                    for (int i = 0; i < TitleList.Count; i++)
                    {
                        Rectangle titleRec = new Rectangle(InnerLeft, InnerTop + i * titleHeight, titleWidth, titleHeight);
                        if (SelectedIndex == i)
                        {
                            g.FillRectangle(selectedTitleBackgroundColor, titleRec);
                            g.DrawString(TitleList[i].Title, Font, selectedTitleFontBrush, new Point(titleRec.Left + TitlePadding.Left, titleRec.Top + TitlePadding.Top));

                            // ---- Draw Close
                            Rectangle closeRec = new Rectangle(
                                titleRec.Right + TitleCloseButtonRectangle.Left, titleRec.Top + TitleCloseButtonRectangle.Top
                                , TitleCloseButtonRectangle.Width, TitleCloseButtonRectangle.Height);
                            Color closePenColor = closeRec.Contains(cursorPoint)
                                ? Color.FromArgb(200, 1, 36, 96)
                                : Color.FromArgb(128, 1, 36, 96);
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            g.DrawLine(new Pen(closePenColor, 1.6f), closeRec.Left, closeRec.Top, closeRec.Right, closeRec.Bottom);
                            g.DrawLine(new Pen(closePenColor, 1.6f), closeRec.Left, closeRec.Bottom, closeRec.Right, closeRec.Top);
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                        }
                        else
                        {
                            g.FillRectangle(titleBackgroundBrush, titleRec);
                            g.DrawString(TitleList[i].Title, Font, titleFontBrush, new Point(titleRec.Left + TitlePadding.Left, titleRec.Top + TitlePadding.Top));
                        }
                    }
                }
            }

            private void SetSize()
            {
                var g = CreateGraphics();

                int maxWidth = 0, maxHeight = 0;
                PushMaxWidhtNMaxHeight(g, TitleList, TitleMinWidth, ref maxWidth, ref maxHeight);

                Size = new Size(
                      (MainBorderWidth * 2) + MainPadding.Horizontal + maxWidth + TitlePadding.Horizontal
                    , (MainBorderWidth * 2) + MainPadding.Vertical + (maxHeight + TitlePadding.Vertical) * TitleList.Count
                );
            }

            public void Close()
            {
                Visible = false;
                if (Parent != null)
                {
                    Parent.Controls.Remove(this);
                }
                Owner.Focus();
            }

            #region Functions For This
            private void PushMaxWidhtNMaxHeight(Graphics g, List<UserPanelContextMenuTitleInfo> list, int minWidth, ref int maxWidth, ref int maxHeight)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var titleSize = g.MeasureString(TitleList[i].Title, Font);

                    if (maxWidth < titleSize.Width)
                    {
                        maxWidth = Convert.ToInt32(Math.Ceiling(titleSize.Width));
                    }

                    if (maxHeight < titleSize.Height)
                    {
                        maxHeight = Convert.ToInt32(Math.Ceiling(titleSize.Height));
                    }
                }

                if (maxWidth < minWidth)
                {
                    maxWidth = minWidth;
                }
            }
            #endregion

            #region Common Functions
            private Control GetParentForm(Control control)
            {
                Control rs = null;
                if (control.Parent != null)
                {
                    if (control.Parent is Form)
                    {
                        rs = control.Parent;
                    }
                    else
                    {
                        rs = GetParentForm(control.Parent);
                    }
                }
                return rs;
            }
            #endregion

            #region Private Class
            private class GlobalMouseHandler : IMessageFilter
            {
                private const int WM_LBUTTONDOWN = 0x201;
                public UserPanelContextMenu TargetUserPanelContextMenu;

                public GlobalMouseHandler(UserPanelContextMenu _TargetUserPanelContextMenu)
                {
                    TargetUserPanelContextMenu = _TargetUserPanelContextMenu;
                }

                public bool PreFilterMessage(ref Message m)
                {
                    if (m.Msg == WM_LBUTTONDOWN)
                    {
                        if (TargetUserPanelContextMenu.Parent != null)
                        {
                            var cursorPoint = TargetUserPanelContextMenu.Parent.PointToClient(Cursor.Position);
                            if (TargetUserPanelContextMenu.Visible &&!TargetUserPanelContextMenu.Bounds.Contains(cursorPoint))
                            {
                                TargetUserPanelContextMenu.Close();
                            }
                        }
                    }
                    return false;
                }
            }
            #endregion
        }

        public struct UserPanelContextMenuTitleInfo
        {
            public string Title;
            public object Data;
            public UserPanelContextMenuTitleInfo(string title, object data)
            {
                Title = title;
                Data = data;
            }
        }

        public class UserPanelContextMenuItemEventArgs : EventArgs
        {
            public int SelectedIndex;
            public string TitleName;
            public object Data;
            public bool RemoveThisTitle;
            public bool CloseContextMenu;
            public bool DisposeContextMenu;
            public UserPanelContextMenuItemEventArgs(int selectedIndex, string titleName, object data, bool removeThisTitle, bool closeContextMenu, bool disposeContextMenu = false)
            {
                SelectedIndex = selectedIndex;
                TitleName = titleName;
                Data = data;
                RemoveThisTitle = removeThisTitle;
                CloseContextMenu = closeContextMenu;
                DisposeContextMenu = disposeContextMenu;
            }
        }

        public enum UserPanelContextShowPosition
        {
            Auto = 1, // Auto Not Used
            TopLeft = 11,
            TopCenter = 12,
            TopRight = 13,
            MiddleLeft = 21,
            MiddleCenter = 22,
            MiddleRight = 23,
            BottomLeft = 31,
            BottomCenter = 32,
            BottomRight = 33,
        }

    }
}