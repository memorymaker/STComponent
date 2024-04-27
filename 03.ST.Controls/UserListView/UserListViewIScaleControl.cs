using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.Controls
{
    public partial class UserListView : IScaleControl
    {
        private float MaximumScaleValue = 2f;
        private float MinimumScaleValue = 0.2f;

        public float ScaleValue
        {
            get
            {
                return _ScaleValue;
            }
            set
            {
                if (value != _ScaleValue)
                {
                    if (_ScaleValue == 1)
                    {
                        _OriginalWidth = Width;
                        _OriginalHeight = Height;
                        _OriginalLeft = Left;
                        _OriginalTop = Top;
                    }

                    float oldScaleValue = _ScaleValue;
                    int oldScrollBarValue = ScrollBarVertical.Value - ScrollBarVertical.Minimum;

                    _ScaleValue = Math.Min(Math.Max(value, MinimumScaleValue), MaximumScaleValue);

                    Size = new Size((int)Math.Round(_OriginalWidth * _ScaleValue), (int)Math.Round(_OriginalHeight * _ScaleValue));
                    if (Parent != null && Parent is UserScaleControlWarpPanel)
                    {
                        Point parentInnerLocation = ((UserScaleControlWarpPanel)Parent).InnerLocation;

                        Location = new Point(
                              (int)Math.Round(((Location.X + parentInnerLocation.X) * (1 / oldScaleValue)) * _ScaleValue) - parentInnerLocation.X
                            , (int)Math.Round(((Location.Y + parentInnerLocation.Y) * (1 / oldScaleValue)) * _ScaleValue) - parentInnerLocation.Y
                        );
                    }
                    else
                    {
                        Location = new Point((int)Math.Round(_OriginalLeft * _ScaleValue), (int)Math.Round(_OriginalTop * _ScaleValue));
                    }

                    
                    SetScrollBarMinMaxLargeSize();
                    ScrollBarVertical.Value = (int)Math.Round(oldScrollBarValue * (_ScaleValue / oldScaleValue)) + ScrollBarVertical.Minimum;

                    Draw();
                }
            }
        }
        private float _ScaleValue = 1f;

        public int OriginalWidth
        {
            get
            {
                return _OriginalWidth;
            }
            set
            {
                if (value != _OriginalWidth)
                {
                    _OriginalWidth = value;
                }
            }
        }
        private int _OriginalWidth = 0;

        public int OriginalHeight
        {
            get
            {
                return _OriginalHeight;
            }
            set
            {
                if (value != _OriginalHeight)
                {
                    _OriginalHeight = value;
                }
            }
        }
        private int _OriginalHeight = 0;

        public int OriginalLeft
        {
            get
            {
                return _OriginalLeft;
            }
            set
            {
                if (value != _OriginalLeft)
                {
                    _OriginalLeft = value;
                }
            }
        }
        private int _OriginalLeft = 0;

        public int OriginalTop
        {
            get
            {
                return _OriginalTop;
            }
            set
            {
                if (value != _OriginalTop)
                {
                    _OriginalTop = value;
                }
            }
        }
        private int _OriginalTop = 0;

        public Color MinimapColor { get; set; } = Color.Blue;

        public void SetScaleValueNMovePoint(float scaleValue, Point movePoint)
        {
            if (_ScaleValue == 1)
            {
                _OriginalWidth = Width;
                _OriginalHeight = Height;
                _OriginalLeft = Left;
                _OriginalTop = Top;
            }

            float oldScaleValue = _ScaleValue;
            int oldScrollBarValue = ScrollBarVertical.Value - ScrollBarVertical.Minimum;

            _ScaleValue = Math.Min(Math.Max(scaleValue, MinimumScaleValue), MaximumScaleValue);

            Size newSize = new Size((int)Math.Round(_OriginalWidth * _ScaleValue), (int)Math.Round(_OriginalHeight * _ScaleValue));
            Point newLocation;
            if (Parent != null && Parent is UserScaleControlWarpPanel)
            {
                Point parentInnerLocation = ((UserScaleControlWarpPanel)Parent).InnerLocation;

                newLocation = new Point(
                      (int)Math.Round(((Location.X + parentInnerLocation.X) * (1 / oldScaleValue)) * _ScaleValue) - parentInnerLocation.X
                    , (int)Math.Round(((Location.Y + parentInnerLocation.Y) * (1 / oldScaleValue)) * _ScaleValue) - parentInnerLocation.Y
                );
            }
            else
            {
                newLocation = new Point((int)Math.Round(_OriginalLeft * _ScaleValue), (int)Math.Round(_OriginalTop * _ScaleValue));
            }

            newLocation.X += movePoint.X;
            newLocation.Y += movePoint.Y;

            Bounds = new Rectangle(newLocation, newSize);

            SetScrollBarMinMaxLargeSize();
            ScrollBarVertical.Value = (int)Math.Round(oldScrollBarValue * (_ScaleValue / oldScaleValue)) + ScrollBarVertical.Minimum;

            Draw();
        }
    }
}
