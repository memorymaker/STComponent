using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Automation;
using System.Collections;

namespace ST.DataModeler
{
    public class GraphicMouseAction
    {
        // User Option
        public bool Enable = true;
        public bool UseLocationCorrection = true;
        public bool UseSizing = true;

        // System Option
        public const int AutoMovePixelInSide = 10;
        public const int SizeArea = 5;
        public const int TopSizeArea = 5;
        public const int MinWidth = 80;
        public const int MinHeight = 80;
        public MouseActionType ActionType = MouseActionType.None;
        private System.Windows.Input.Key NoneAutoMoveKey = System.Windows.Input.Key.LeftCtrl;

        // Referece Mouse
        private Rectangle MouseDownTargetBounds = Rectangle.Empty;
        private Point MouseDownPoint = Point.Empty;
        private bool MouseDown = false;
        public bool IsMouseDown => MouseDown;

        // Default Refernce
        private IGraphicMouseActionTarget Target = null;

        public GraphicMouseAction(IGraphicMouseActionTarget target)
        {
            Target = target;
            SetEvents();
        }

        private void SetEvents()
        {
            Target.MouseDown += Target_MouseDown;
            Target.MouseMove += Target_MouseMove;
            Target.MouseUp += Target_MouseUp;
            Target.MouseLeave += Target_MouseLeave;
        }

        private void Target_MouseDown(object sender, MouseEventArgs e)
        {
            if (Enable)
            {
                SetActionTypeNMouseCursor(e.X, e.Y);

                MouseDownTargetBounds = Target.Bounds;
                MouseDownPoint = e.Location;
                MouseDown = true;
            }
        }

        private void Target_MouseMove(object sender, MouseEventArgs e)
        {
            if (Enable)
            {
                if (e.Button == MouseButtons.Left && MouseDown)
                {
                    switch (ActionType)
                    {
                        case MouseActionType.Move:
                            Target.Location = GetLocation(e.X, e.Y);
                            break;
                        case MouseActionType.SizeTop:
                        case MouseActionType.SizeTopRight:
                        case MouseActionType.SizeRight:
                        case MouseActionType.SizeBottomRight:
                        case MouseActionType.SizeBottom:
                        case MouseActionType.SizeBottomLeft:
                        case MouseActionType.SizeLeft:
                        case MouseActionType.SizeTopLeft:
                            Target.Bounds = GetBounds(ActionType, e.X, e.Y);
                            break;
                    }
                }
                else if (e.Button == MouseButtons.None)
                {
                    SetActionTypeNMouseCursor(e.X, e.Y);
                    MouseDown = false;
                }
            }
        }

        private void Target_MouseUp(object sender, MouseEventArgs e)
        {
            MouseDown = false;
        }

        private void SetActionTypeNMouseCursor(int eX, int eY)
        {
            if ((TopSizeArea < eY && eY < Target.TitleHeight) || !UseSizing)
            {
                ActionType = MouseActionType.Move;
                Target.Cursor = Cursors.Default;
            }
            else
            {
                ActionType = GetMouseActionType(eX, eY);
                switch (ActionType)
                {
                    case MouseActionType.SizeTopRight:    Target.Cursor = Cursors.SizeNESW; break;
                    case MouseActionType.SizeBottomLeft:  Target.Cursor = Cursors.SizeNESW; break;
                    case MouseActionType.SizeTopLeft:     Target.Cursor = Cursors.SizeNWSE; break;
                    case MouseActionType.SizeBottomRight: Target.Cursor = Cursors.SizeNWSE; break;
                    case MouseActionType.SizeTop:         Target.Cursor = Cursors.SizeNS; break;
                    case MouseActionType.SizeBottom:      Target.Cursor = Cursors.SizeNS; break;
                    case MouseActionType.SizeRight:       Target.Cursor = Cursors.SizeWE; break;
                    case MouseActionType.SizeLeft:        Target.Cursor = Cursors.SizeWE; break;
                    case MouseActionType.Move:            Target.Cursor = Cursors.Default; break;
                    default:                              Target.Cursor = Cursors.Default; break;
                }
            }
        }

        private void Target_MouseLeave(object sender, EventArgs e)
        {
            if (Enable)
            {
                MouseDown = false;
            }
        }

        private Rectangle GetBounds(MouseActionType mouseActionType, int eX, int eY)
        {
            Rectangle rs = Target.Bounds;

            // Top
            if (mouseActionType == MouseActionType.SizeTop || mouseActionType == MouseActionType.SizeTopLeft || mouseActionType == MouseActionType.SizeTopRight)
            {
                rs.Y = Target.Top + (eY - MouseDownPoint.Y);

                // Parent Side - Top
                if (rs.Y <= AutoMovePixelInSide)
                {
                    rs.Y = 0;
                }

                rs.Height = MouseDownTargetBounds.Top + MouseDownTargetBounds.Height - rs.Y;

                // Min Height
                if (rs.Height < MinHeight)
                {
                    rs.Y -= MinHeight - rs.Height;
                    rs.Height = MinHeight;
                }
            }
            // Bottom
            else if (mouseActionType == MouseActionType.SizeBottom || mouseActionType == MouseActionType.SizeBottomLeft || mouseActionType == MouseActionType.SizeBottomRight)
            {
                rs.Height = MouseDownTargetBounds.Height + (eY - MouseDownPoint.Y);

                // Parent Side - Bottom
                if (Target.Parent != null && (Target.Parent.Height - AutoMovePixelInSide <= rs.Y + rs.Height))
                {
                    rs.Height = Target.Parent.Height - Target.Top;
                }
                else if (Target.Target.Height - AutoMovePixelInSide <= rs.Y + rs.Height)
                {
                    rs.Height = Target.Target.Height - Target.Top;
                }

                // Min Height
                if (rs.Height < MinHeight)
                {
                    rs.Height = MinHeight;
                }
            }

            // Left
            if (mouseActionType == MouseActionType.SizeLeft || mouseActionType == MouseActionType.SizeTopLeft || mouseActionType == MouseActionType.SizeBottomLeft)
            {
                rs.X = Target.Left + (eX - MouseDownPoint.X);

                // Parent Side - Left
                if (rs.X <= AutoMovePixelInSide)
                {
                    rs.X = 0;
                }

                rs.Width = MouseDownTargetBounds.Left + MouseDownTargetBounds.Width - rs.X;

                // Min Width
                if (rs.Width < MinWidth)
                {
                    rs.X -= MinWidth - rs.Width;
                    rs.Width = MinWidth;
                }
            }
            // Right
            else if (mouseActionType == MouseActionType.SizeRight || mouseActionType == MouseActionType.SizeTopRight || mouseActionType == MouseActionType.SizeBottomRight)
            {
                rs.Width = MouseDownTargetBounds.Width + (eX - MouseDownPoint.X);

                // Parent Side - Right
                if (Target.Parent != null && (Target.Parent.Width - AutoMovePixelInSide <= rs.X + rs.Width))
                {
                    rs.Width = Target.Parent.Width - Target.Left;
                }
                else if (Target.Target.Width - AutoMovePixelInSide <= rs.X + rs.Width)
                {
                    rs.Width = Target.Target.Width - Target.Left;

                }

                // Min Width
                if (rs.Width < MinWidth)
                {
                    rs.Width = MinWidth;
                }
            }

            return rs;
        }

        private Point GetLocation(int eX, int eY)
        {
            return new Point(GetLocation_GetLeft(eX), GetLocation_GetTop(eY));
        }

        private int GetLocation_GetLeft(int eX)
        {
            int left = Target.Left + eX - MouseDownPoint.X;
            //int left = MouseDownTargetBounds.Left + eX - MouseDownPoint.X;

            if (UseLocationCorrection)
            {
                // Parent Side
                if (left <= AutoMovePixelInSide || left < 0)
                {
                    left = 0;
                    return left;
                }
                else if (Target.Parent != null && (Target.Width + left >= Target.Parent.Width - AutoMovePixelInSide))
                {
                    left = Target.Parent.Width - Target.Width;
                    return left;
                }
                else if (Target.Width + left >= Target.Target.Width - AutoMovePixelInSide)
                {
                    left = Target.Target.Width - Target.Width;
                    return left;
                }
            }

            return left;
        }

        private int GetLocation_GetTop(int eY)
        {
            int top = Target.Top + eY - MouseDownPoint.Y;
            // int top = MouseDownTargetBounds.Top + eY - MouseDownPoint.Y;

            if (UseLocationCorrection)
            {
                // Parent Edge
                if (top <= AutoMovePixelInSide || top < 0)
                {
                    top = 0;
                    return top;
                }
                else if (Target.Parent != null && (Target.Height + top >= Target.Parent.Height - AutoMovePixelInSide))
                {
                    top = Target.Parent.Height - Target.Height;
                    return top;
                }
                else if (Target.Height + top >= Target.Target.Height - AutoMovePixelInSide)
                {
                    top = Target.Target.Height - Target.Height;
                    return top;
                }
            }

            return top;
        }

        private MouseActionType GetMouseActionType(int x, int y)
        {
            MouseActionType rs = MouseActionType.Move;

            // Left
            if (x <= SizeArea)
            {
                rs = MouseActionType.SizeLeft;
            }
            // Right
            else if (Target.Width - SizeArea <= x)
            {
                rs = MouseActionType.SizeRight;
            }

            // Top
            if (y <= SizeArea)
            {
                switch (rs)
                {
                    case MouseActionType.SizeRight: rs = MouseActionType.SizeTopRight; break;
                    case MouseActionType.SizeLeft: rs = MouseActionType.SizeTopLeft; break;
                    default: rs = MouseActionType.SizeTop; break;
                }
            }
            // Bottom
            else if (Target.Height - SizeArea <= y)
            {
                switch (rs)
                {
                    case MouseActionType.SizeRight: rs = MouseActionType.SizeBottomRight; break;
                    case MouseActionType.SizeLeft: rs = MouseActionType.SizeBottomLeft; break;
                    default: rs = MouseActionType.SizeBottom; break;
                }
            }

            return rs;
        }

        private bool IsAutoMove
        {
            get
            {
                return !System.Windows.Input.Keyboard.IsKeyDown(NoneAutoMoveKey);
            }
        }
    }

    public enum MouseActionType
    {
        None = 0,
        Move = 1,
        SizeTop,
        SizeTopRight,
        SizeRight,
        SizeBottomRight,
        SizeBottom,
        SizeBottomLeft,
        SizeLeft,
        SizeTopLeft,
    }

    public enum MouseActionDirectionType
    {
        Top = 1,
        Right,
        Bottom,
        Left
    }
}
