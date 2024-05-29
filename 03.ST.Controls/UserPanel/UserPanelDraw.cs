using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.Controls
{
    public partial class UserPanel
    {
        private Form TopForm = null;
        public bool BlockDrawing = false;

        private Color DisableColor = Color.FromArgb(60, 0, 0, 0);

        private Color TitleBarColor = Color.FromArgb(93, 107, 153);
        private Dictionary<UserPanelTitleStateType, Color> TitleBackColor = new Dictionary<UserPanelTitleStateType, Color>()
        {
            { UserPanelTitleStateType.None, Color.FromArgb(59, 79, 129) }
            , { UserPanelTitleStateType.Over, Color.FromArgb(187, 198, 241) }
            , { UserPanelTitleStateType.Selected, Color.FromArgb(204, 213, 240) }
            , { UserPanelTitleStateType.FocusedSelected, Color.FromArgb(245, 204, 132) }
        };
        private Dictionary<UserPanelTitleStateType, Color> TitleFontColor = new Dictionary<UserPanelTitleStateType, Color>()
        {
            { UserPanelTitleStateType.None, Color.FromArgb(255, 255, 255) }
            , { UserPanelTitleStateType.Over, Color.FromArgb(1, 36, 96) }
            , { UserPanelTitleStateType.Selected, Color.FromArgb(1, 36, 96) }
            , { UserPanelTitleStateType.FocusedSelected, Color.FromArgb(30, 30, 30) }
        };
        private Color UserDefaultBackColor = Color.White;

        private const int TitleSliderWidth = 44;

        protected bool BorderTopDrawing { get; set; } = true;

        protected bool BorderBottomDrawing { get; set; } = true;

        protected bool BorderLeftDrawing { get; set; } = true;

        protected bool BorderRightDrawing { get; set; } = true;

        private void LoadDraw()
        {
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            Draw(e);
        }

        private void Draw(PaintEventArgs e, bool isMouseLeave = false)
        {
            bool isFirstChild = true;
            if (Parent != null && Parent is UserPanel)
            {
                isFirstChild = Parent.Controls.IndexOf(this) == 0;
            }

            if (isFirstChild && 0 < ClientRectangle.Width && 0 < ClientRectangle.Height)
            {
                SetTopForm();

                BufferedGraphicsContext context = new BufferedGraphicsContext();
                using (BufferedGraphics bufferedGraphics = BufferedGraphicsManager.Current.Allocate(e.Graphics, ClientRectangle))
                {
                    //bufferedGraphics.Graphics.Clear(Color.Silver);
                    //bufferedGraphics.Graphics.InterpolationMode = InterpolationMode.High;
                    //bufferedGraphics.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    var cursorPoint = PointToClient(Cursor.Position);

                    Graphics g = bufferedGraphics.Graphics;
                    g.Clear(BackColor);
                    g.FillRectangle(new SolidBrush(BackColor), new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height));

                    if (TitleVisable)
                    {
                        // Title Bar
                        Rectangle rectBar = new Rectangle(TitlePadding.Left, TitlePadding.Top, Width - TitlePadding.Horizontal, TitleHeight - TitlePadding.Vertical);
                        g.FillRectangle(new SolidBrush(TitleBarColor), rectBar);

                        // Slider
                        Rectangle sliderRec = new Rectangle(Width - TitleSliderWidth, 0, TitleSliderWidth, 23);

                        int addedWidth = 0;
                        int index = 0;
                        var sortedTitles = (from title in TitleList orderby title.Sort select title).ToList();
                        for(int k = 0 + TitleStartIndex; k < sortedTitles.Count; k++)
                        {
                            UserPanelTitleInfo titleNode = sortedTitles[k];

                            Size textSize = TextRenderer.MeasureText(titleNode.Title, Font);
                            string title = titleNode.Title.Trim();

                            Rectangle titleRect = new Rectangle(
                                TitlePadding.Left + addedWidth, TitlePadding.Top
                                , textSize.Width + TitleWidthRevision, TitleHeight - TitlePadding.Vertical
                            );

                            int titleRight = addedWidth + textSize.Width + TitleWidthRevision + TitleDistance;

                            if (UsingTitleSlider)
                            {
                                if (addedWidth >= Width)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                if (titleRight + TitleAreaRightPadding > Width && index > 0)
                                {
                                    break;
                                }
                                else if (titleRight + TitleAreaRightPadding - TitleDistance > Width && index == 0)
                                {
                                    titleRect.Width = Width - TitleAreaRightPadding;
                                    var targetTextSize = titleRect.Width - TitleWidthRevision;
                                    for (int i = title.Length - 2; 1 <= i; i--)
                                    {
                                        Size newTitleSize3 = TextRenderer.MeasureText(title.Substring(0, i) + "...", Font);
                                        if (newTitleSize3.Width <= targetTextSize)
                                        {
                                            title = title.Substring(0, i) + "...";
                                            break;
                                        }

                                        if (i == 1)
                                        {
                                            Size newTitleSize2 = TextRenderer.MeasureText(title.Substring(0, i) + "..", Font);
                                            Size newTitleSize0 = TextRenderer.MeasureText(title.Substring(0, i), Font);
                                            if (newTitleSize2.Width <= targetTextSize)
                                            {
                                                title = title.Substring(0, i) + "..";
                                                break;
                                            }
                                            else if (newTitleSize0.Width <= targetTextSize)
                                            {
                                                title = title.Substring(0, i);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            // Get titleState
                            UserPanelTitleStateType titleState;
                            if (titleNode.Selected)
                            {
                                titleState = (ContainsFocus || ContextMenu.Focused)
                                    ? UserPanelTitleStateType.FocusedSelected
                                    : UserPanelTitleStateType.Selected;
                            }
                            else
                            {
                                if (isMouseLeave)
                                {
                                    titleState = UserPanelTitleStateType.None;
                                }
                                else
                                {
                                    titleState = titleRect.Contains(cursorPoint) && !sliderRec.Contains(cursorPoint)
                                        ? UserPanelTitleStateType.Over
                                        : UserPanelTitleStateType.None;
                                }
                            }

                            // Draw Title Area
                            SolidBrush blueBrush = new SolidBrush(TitleBackColor[titleState]);
                            g.FillRectangle(blueBrush, titleRect);

                            g.DrawString(title, Font, new SolidBrush(TitleFontColor[titleState]), TitleTextPosition.X + addedWidth, TitleTextPosition.Y);

                            // Draw Close
                            if ((titleRect.Contains(cursorPoint) && MouseDownArea == UserPanelMouseDownArea.None) || titleState == UserPanelTitleStateType.FocusedSelected)
                            {
                                var closeRec = new Rectangle(
                                      titleRect.Right + TitleCloseButtonRectangle.Left, TitleCloseButtonRectangle.Top
                                    , TitleCloseButtonRectangle.Width, TitleCloseButtonRectangle.Height);

                                var closeRecArea = new Rectangle(
                                      closeRec.Left - TitleCloseButtonAreaRevision, closeRec.Top - TitleCloseButtonAreaRevision
                                    , closeRec.Width + TitleCloseButtonAreaRevision * 2, closeRec.Height + TitleCloseButtonAreaRevision * 2
                                );
                                var closePenColor = closeRecArea.Contains(cursorPoint)
                                    ? Color.FromArgb(200, 1, 36, 96)
                                    : Color.FromArgb(128, 1, 36, 96);

                                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                g.DrawLine(new Pen(closePenColor, 1.6f), closeRec.Left, closeRec.Top, closeRec.Right, closeRec.Bottom);
                                g.DrawLine(new Pen(closePenColor, 1.6f), closeRec.Left, closeRec.Bottom, closeRec.Right, closeRec.Top);
                                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                            }
                    
                            addedWidth = titleRight;
                            index++;
                        }

                        // Draw Right Buttons
                        ViewContextMenuButton.Draw(g);
                        AwaysOnTopMenuButton.Draw(g);

                        // Draw Title Slider
                        if (UsingTitleSlider)
                        {
                            if (!(TitleStartIndex == 0 && addedWidth < Width - TitleSliderWidth))
                            {
                                g.FillRectangle(new SolidBrush(TitleBarColor), sliderRec);
                                SlideLeftMenuButton.Draw(g);
                                SlideRightMenuButton.Draw(g);
                                SlideLeftMenuButton.Enabled = true;
                                SlideRightMenuButton.Enabled = true;
                            }
                            else
                            {
                                SlideLeftMenuButton.Enabled = false;
                                SlideRightMenuButton.Enabled = false;
                            }
                        }

                        // Border
                        Rectangle rectBorder = new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height);
                        //Color BorderColor = Color.FromArgb(50, TitleBarColor.R, TitleBarColor.G, TitleBarColor.B);
                        Color BorderColor = TitleBarColor;
                        Pen borderPen = new Pen(BorderColor);
                        borderPen.Width = UserBorderWidth;
                        if (BorderTopDrawing) { g.DrawLine(borderPen, rectBorder.Left, rectBorder.Top + Convert.ToInt32(Math.Floor(borderPen.Width / 2.0)), rectBorder.Right, rectBorder.Top + Convert.ToInt32(Math.Floor(borderPen.Width / 2.0))); }
                        if (BorderBottomDrawing) { g.DrawLine(borderPen, rectBorder.Left, rectBorder.Bottom - Convert.ToInt32(Math.Ceiling(borderPen.Width / 2.0)), rectBorder.Right, rectBorder.Bottom - Convert.ToInt32(Math.Ceiling(borderPen.Width / 2.0))); }
                        if (BorderLeftDrawing) { g.DrawLine(borderPen, rectBorder.Left + Convert.ToInt32(Math.Floor(borderPen.Width / 2.0)), rectBorder.Top, rectBorder.Left + Convert.ToInt32(Math.Floor(borderPen.Width / 2.0)), rectBorder.Bottom); }
                        if (BorderRightDrawing) { g.DrawLine(borderPen, rectBorder.Right - Convert.ToInt32(Math.Ceiling(borderPen.Width / 2.0)), rectBorder.Top, rectBorder.Right - Convert.ToInt32(Math.Ceiling(borderPen.Width / 2.0)), rectBorder.Bottom); }

                        // Sizenig Border
                        if (Parent != null)
                        {
                            Rectangle rectBorderSizing = new Rectangle(0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
                            Pen BorderSizingPen = new Pen(BorderSizingColor, 3);
                            if ((Bounds.Contains(Parent.PointToClient(Cursor.Position)) || MouseDownArea != UserPanelMouseDownArea.None) && Cursor != Cursors.Default)
                            {
                                // Top
                                if (MouseAction.ActionType == MouseActionType.SizeTop || MouseAction.ActionType == MouseActionType.SizeTopRight || MouseAction.ActionType == MouseActionType.SizeTopLeft)
                                {
                                    g.DrawLine(BorderSizingPen, rectBorderSizing.Left, rectBorderSizing.Top + Convert.ToInt32(Math.Floor(BorderSizingPen.Width / 2.0)), rectBorderSizing.Right, rectBorderSizing.Top + Convert.ToInt32(Math.Floor(BorderSizingPen.Width / 2.0)));
                                }
                                // Bottom
                                else if (MouseAction.ActionType == MouseActionType.SizeBottom || MouseAction.ActionType == MouseActionType.SizeBottomRight || MouseAction.ActionType == MouseActionType.SizeBottomLeft)
                                {
                                    g.DrawLine(BorderSizingPen, rectBorderSizing.Left, rectBorderSizing.Bottom - Convert.ToInt32(Math.Floor(BorderSizingPen.Width / 2.0)), rectBorderSizing.Right, rectBorderSizing.Bottom - Convert.ToInt32(Math.Floor(BorderSizingPen.Width / 2.0)));
                                }

                                // Left
                                if (MouseAction.ActionType == MouseActionType.SizeLeft || MouseAction.ActionType == MouseActionType.SizeTopLeft || MouseAction.ActionType == MouseActionType.SizeBottomLeft)
                                {
                                    g.DrawLine(BorderSizingPen, rectBorderSizing.Left + Convert.ToInt32(Math.Floor(BorderSizingPen.Width / 2.0)), rectBorderSizing.Top, rectBorderSizing.Left + Convert.ToInt32(Math.Floor(BorderSizingPen.Width / 2.0)), rectBorderSizing.Bottom);
                                }
                                // Right
                                else if (MouseAction.ActionType == MouseActionType.SizeRight || MouseAction.ActionType == MouseActionType.SizeTopRight || MouseAction.ActionType == MouseActionType.SizeBottomRight)
                                {
                                    g.DrawLine(BorderSizingPen, rectBorderSizing.Right - Convert.ToInt32(Math.Floor(BorderSizingPen.Width / 2.0)), rectBorderSizing.Top, rectBorderSizing.Right - Convert.ToInt32(Math.Floor(BorderSizingPen.Width / 2.0)), rectBorderSizing.Bottom);
                                }
                            }
                        }
                    }

                    if (!Enabled)
                    {
                        g.FillRectangle(new SolidBrush(DisableColor), 0, 0, Width, Height);
                    }

                    // ------------ Draw
                    bufferedGraphics.Render(e.Graphics);
                }
            }
        }

        public void Draw(bool isMouseLeave = false)
        {
            if (!IsDisposed && !BlockDrawing)
            {
                Draw(new PaintEventArgs(CreateGraphics(), ClientRectangle), isMouseLeave);
            }
        }

        private void SetTopForm()
        {
            if (TopForm == null)
            {
                Control topForm = Parent;

                if (topForm != null)
                {
                    while(topForm.Parent != null)
                    {
                        topForm = topForm.Parent;
                    }
                }

                if (topForm is Form)
                {
                    TopForm = topForm as Form;
                    TopForm.Activated += (object sender, EventArgs e) =>
                    {
                        DrawThisNParentUserPanel();
                    };
                    TopForm.Deactivate += (object sender, EventArgs e) =>
                    {
                        DrawThisNParentUserPanel();
                    };
                }
            }
        }

        private void DrawThisNParentUserPanel()
        {
            Control userPanel = this;
            do
            {
                if (userPanel is UserPanel)
                {
                    ((UserPanel)userPanel).Draw();
                }
                userPanel = userPanel.Parent;
            }
            while (userPanel != null);
        }
    }
}
