using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ST.Controls
{
    public partial class UserScrollBar
    {
        private UserRectangle VerticalDecrementButtonLayout = new UserRectangle(-1, 20,  0,  0, -1,  0); // Height To Width(Code) -1: No Value
        private UserRectangle VerticalEncrementButtonLayout = new UserRectangle(-1, 20, -1,  0,  0,  0); // Height To Width(Code) -1: No Value
        private UserRectangle VerticalScrollButtonLayout    = new UserRectangle(-1, 10, 50,  0, -1,  0); // 10:Default

        private UserRectangle HorizontalDecrementButtonLayout = new UserRectangle(20, -1, 0, -1, 0,  0); // Height To Width(Code) -1: No Value
        private UserRectangle HorizontalEncrementButtonLayout = new UserRectangle(20, -1, 0,  0, 0, -1); // Height To Width(Code) -1: No Value
        private UserRectangle HorizontalScrollButtonLayout    = new UserRectangle(10, -1, 0, -1, 0, 50); // 10:Default

        private Rectangle DecrementButtonRectangle = new Rectangle();
        private Rectangle EncrementButtonRectangle = new Rectangle();
        private Rectangle ScrollButtonRectangle    = new Rectangle();

        private readonly int ScrollButtonMinHeightOrWidth = 4;

        public bool IncreaseDecreaseButtonVisible
        {
            get
            {
                return _IncreaseDecreaseButtonVisible;
            }
            set
            {
                if (value != _IncreaseDecreaseButtonVisible)
                {
                    _IncreaseDecreaseButtonVisible = value;
                    SetButtonsRectangle();
                }
            }
        }
        private bool _IncreaseDecreaseButtonVisible = true;

        public void LoadObject()
        {
            SetObjectEvents();
            SetButtonsRectangle();
        }

        private void SetObjectEvents()
        {
            Resize += UserScrollBar_Resize;
        }

        private void UserScrollBar_Resize(object sender, EventArgs e)
        {
            SetButtonsRectangle();
        }

        private void SetButtonsRectangle()
        {
            if (_IncreaseDecreaseButtonVisible)
            {
                if (Type == UserScrollBarType.Vertical)
                {
                    DecrementButtonRectangle = GetRectangle(VerticalDecrementButtonLayout);
                    DecrementButtonRectangle.Height = DecrementButtonRectangle.Width;

                    EncrementButtonRectangle = GetRectangle(VerticalEncrementButtonLayout);
                    EncrementButtonRectangle.Y -= EncrementButtonRectangle.Width - EncrementButtonRectangle.Height;
                    EncrementButtonRectangle.Height = EncrementButtonRectangle.Width;
                }
                else if (Type == UserScrollBarType.Horizontal)
                {
                    DecrementButtonRectangle = GetRectangle(HorizontalDecrementButtonLayout);
                    DecrementButtonRectangle.Width = DecrementButtonRectangle.Height;

                    EncrementButtonRectangle = GetRectangle(HorizontalEncrementButtonLayout);
                    EncrementButtonRectangle.X -= EncrementButtonRectangle.Height - EncrementButtonRectangle.Width;
                    EncrementButtonRectangle.Width = EncrementButtonRectangle.Height;
                }
            }
            else
            {
                if (Type == UserScrollBarType.Vertical)
                {
                    DecrementButtonRectangle = GetRectangle(VerticalDecrementButtonLayout);
                    DecrementButtonRectangle.Height = DecrementButtonRectangle.Width;
                    DecrementButtonRectangle.Y -= (DecrementButtonRectangle.Height * 0.7f).ToInt();

                    EncrementButtonRectangle = GetRectangle(VerticalEncrementButtonLayout);
                    EncrementButtonRectangle.Y -= EncrementButtonRectangle.Width - EncrementButtonRectangle.Height;
                    EncrementButtonRectangle.Height = EncrementButtonRectangle.Width;
                    EncrementButtonRectangle.Y += (EncrementButtonRectangle.Height * 0.7f).ToInt();
                }
                else if (Type == UserScrollBarType.Horizontal)
                {
                    DecrementButtonRectangle = GetRectangle(HorizontalDecrementButtonLayout);
                    DecrementButtonRectangle.Width = DecrementButtonRectangle.Height;
                    DecrementButtonRectangle.X -= (DecrementButtonRectangle.Width * 0.7f).ToInt();

                    EncrementButtonRectangle = GetRectangle(HorizontalEncrementButtonLayout);
                    EncrementButtonRectangle.X -= EncrementButtonRectangle.Height - EncrementButtonRectangle.Width;
                    EncrementButtonRectangle.Width = EncrementButtonRectangle.Height;
                    EncrementButtonRectangle.X += (EncrementButtonRectangle.Width * 0.7f).ToInt();
                }
            }

            SetScrollButtonRectangle();
        }

        private void SetScrollButtonRectangle()
        {
            if (Type == UserScrollBarType.Vertical)
            {
                ScrollButtonRectangle = GetScrollButtonRectangle(VerticalScrollButtonLayout);
            }
            else if (Type == UserScrollBarType.Horizontal)
            {
                ScrollButtonRectangle = GetScrollButtonRectangle(HorizontalScrollButtonLayout);
            }
        }

        private Rectangle GetScrollButtonRectangle(UserRectangle userRectangle)
        {
            // ---- Using
            // DecrementButtonRectangle
            // EncrementButtonRectangle
            // ScrollButtonMinHeight

            Rectangle rs = Rectangle.Empty;
            if (Maximum - Minimum > 0)
            {
                if (Type == UserScrollBarType.Vertical)
                {
                    // Get areaVerticalHeight, scrollButtonHeight
                    int areaTop = DecrementButtonRectangle.Bottom;
                    int _areaBottom = EncrementButtonRectangle.Top;
                    int areaVerticalHeight = _areaBottom - areaTop;
                    int scrollButtonHeight = Math.Max(
                          // (int)Math.Round(areaVerticalHeight * (Convert.ToDecimal(SmallChange) / (Maximum + SmallChange - Minimum)))
                          (int)Math.Round(areaVerticalHeight * (Convert.ToDecimal(100) / (100 + Maximum - Minimum)))
                        , ScrollButtonMinHeightOrWidth
                    );

                    // Get scrollButtonTop
                    decimal _valuePercent = (Convert.ToDecimal(Value) - Minimum) / (Maximum - Minimum);
                    decimal scrollButtonTop = areaTop + (_valuePercent * (areaVerticalHeight - scrollButtonHeight));

                    rs = GetRectangle(VerticalScrollButtonLayout);
                    rs.Height = scrollButtonHeight;
                    rs.Y = (int)(Math.Round(scrollButtonTop));
                }
                else if (Type == UserScrollBarType.Horizontal)
                {
                    // Get areaVerticalHeight, scrollButtonHeight
                    int areaLeft = DecrementButtonRectangle.Right;
                    int _areaRight = EncrementButtonRectangle.Left;
                    int areaVerticalWidth = _areaRight - areaLeft;
                    int scrollButtonWidth = Math.Max(
                          (int)Math.Round(areaVerticalWidth * (Convert.ToDecimal(100) / (100 + Maximum - Minimum)))
                        , ScrollButtonMinHeightOrWidth
                    );

                    // Get scrollButtonTop
                    decimal _valuePercent = (Convert.ToDecimal(Value) - Minimum) / (Maximum - Minimum);
                    decimal scrollButtonLeft = areaLeft + (_valuePercent * (areaVerticalWidth - scrollButtonWidth));

                    rs = GetRectangle(HorizontalScrollButtonLayout);
                    rs.Width = scrollButtonWidth;
                    rs.X = (int)(Math.Round(scrollButtonLeft));
                }
            }
            else
            {
                rs = Rectangle.Empty;
            }

            return rs;
        }

        private Rectangle GetRectangle(UserRectangle userRectangle)
        {
            Rectangle rs = new Rectangle();

            if (userRectangle.Width == -1)
            {
                if (userRectangle.Left >= 0 && userRectangle.Right >= 0)
                {
                    rs.X = userRectangle.Left;
                    rs.Width = this.Width - userRectangle.Left - userRectangle.Right;
                }
                else
                {
                    throw new Exception(string.Format("GetRectangle Error userRectangle.Width : {0} userRectangle.Left : {1} userRectangle.Right : {2}", userRectangle.Width, userRectangle.Left, userRectangle.Right));
                }
            }
            else
            {
                rs.Width = userRectangle.Width;
                if (userRectangle.Left == -1 && userRectangle.Right >= 0)
                {
                    rs.X = this.Width - userRectangle.Width - userRectangle.Right;
                }
                else if (userRectangle.Left >= 0 && userRectangle.Right == -1)
                {
                    rs.X = userRectangle.Left;
                }
                else
                {
                    throw new Exception(string.Format("GetRectangle Error userRectangle.Width : {0} userRectangle.Left : {1} userRectangle.Right : {2}", userRectangle.Width, userRectangle.Left, userRectangle.Right));
                }
            }

            if (userRectangle.Height == -1)
            {
                if (userRectangle.Top >= 0 && userRectangle.Bottom >= 0)
                {
                    rs.Y = userRectangle.Top;
                    rs.Height = this.Height - userRectangle.Top - userRectangle.Bottom;
                }
                else
                {
                    throw new Exception(string.Format("GetRectangle Error userRectangle.Height : {0} userRectangle.Top : {1} userRectangle.Bottom : {2}", userRectangle.Height, userRectangle.Top, userRectangle.Bottom));
                }
            }
            else
            {
                rs.Height = userRectangle.Height;
                if (userRectangle.Top == -1 && userRectangle.Bottom >= 0)
                {
                    rs.Y = this.Height - userRectangle.Height - userRectangle.Bottom;
                }
                else if (userRectangle.Top >= 0 && userRectangle.Bottom == -1)
                {
                    rs.Y = userRectangle.Top;
                }
                else
                {
                    throw new Exception(string.Format("GetRectangle Error userRectangle.Height : {0} userRectangle.Top : {1} userRectangle.Bottom : {2}", userRectangle.Height, userRectangle.Top, userRectangle.Bottom));
                }
            }

            return rs;
        }
    }

    public struct UserRectangle
    {
        public int Width;
        public int Height;
        public int Top;
        public int Right;
        public int Bottom;
        public int Left;

        public UserRectangle(int width = -1, int height = -1, int top = -1, int right = - 1, int bottom = - 1, int left = -1)
        {
            Width = width;
            Height = height;
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }
    }
}
