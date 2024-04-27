using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST.Core;
using ST.Controls;
using System.Drawing;
using System.Windows.Forms;

namespace Common
{
    public partial class UserNodeBase : IScaleControl
    {
        protected float MaximumScaleValue = 2f;
        protected float MinimumScaleValue = 0.2f;

        // For this
        private Padding OriginalPadding;
        private int OriginalTitleHeight;

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
                    Bounds = GetNewBoundsNSetOriginalValues(value, new Point(0, 0), out _ScaleValue);
                    Draw();
                }
            }
        }
        protected float _ScaleValue = 1f;



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
        protected int _OriginalWidth = 0;

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
        protected int _OriginalHeight = 0;

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
        protected int _OriginalLeft = 0;

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
        protected int _OriginalTop = 0;

        virtual public Color MinimapColor { get; set; } = Color.Blue;

        public void SetScaleValueNMovePoint(float scaleValue, Point movePoint)
        {
            Bounds = GetNewBoundsNSetOriginalValues(scaleValue, movePoint, out _ScaleValue);
            Draw();
        }

        #region Function for this
        private Rectangle GetNewBoundsNSetOriginalValues(float scaleValue, Point movePoint, out float _newScaleValue)
        {
            float oldScaleValue = _ScaleValue;
            float newScaleValue = Math.Min(Math.Max(scaleValue, MinimumScaleValue), MaximumScaleValue);

            if (oldScaleValue == 1)
            {
                // Common
                _OriginalWidth = Width;
                _OriginalHeight = Height;
                _OriginalLeft = Left;
                _OriginalTop = Top;

                // For this
                OriginalPadding = Padding;
                OriginalTitleHeight = TitleHeight;
            }

            Size newSize = new Size((int)Math.Round(_OriginalWidth * newScaleValue), (int)Math.Round(_OriginalHeight * newScaleValue));
            Point newLocation;
            if (Parent != null && Parent is UserScaleControlWarpPanel)
            {
                Point parentInnerLocation = ((UserScaleControlWarpPanel)Parent).InnerLocation;

                newLocation = new Point(
                      (int)Math.Round(((Location.X + parentInnerLocation.X) * (1 / oldScaleValue)) * newScaleValue) - parentInnerLocation.X
                    , (int)Math.Round(((Location.Y + parentInnerLocation.Y) * (1 / oldScaleValue)) * newScaleValue) - parentInnerLocation.Y
                );
            }
            else
            {
                newLocation = new Point((int)Math.Round(_OriginalLeft * newScaleValue), (int)Math.Round(_OriginalTop * newScaleValue));
            }

            newLocation.X += movePoint.X;
            newLocation.Y += movePoint.Y;

            // For this
            Padding = new Padding(
                  (OriginalPadding.Left * newScaleValue).ToInt()
                , (OriginalPadding.Top * newScaleValue).ToInt()
                , (OriginalPadding.Right * newScaleValue).ToInt()
                , (OriginalPadding.Bottom * newScaleValue).ToInt()
            );
            TitleHeight = (OriginalTitleHeight * newScaleValue).ToInt();

            SetChildrenSize(newScaleValue);
            _newScaleValue = newScaleValue;
            return new Rectangle(newLocation, newSize);
        }

        private void SetChildrenSize(float scaleValue)
        {
            foreach(Control control in Controls)
            {
                IScaleControl sizeControl = control as IScaleControl;
                if (sizeControl != null)
                {
                    sizeControl.ScaleValue = scaleValue;
                }
            }
        }
        #endregion
    }
}
