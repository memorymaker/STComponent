using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ST.Core;

namespace ST.DataModeler
{
    public partial class GraphicPanel : IScaleControl
    {
        protected event IScaleControlScaleValueEventHandler ScaleValueChanged;

        protected float MaximumScaleValue = 2f;
        protected float MinimumScaleValue = 0.2f;

        // For this
        protected int OriginalTitleHeight => _OriginalTitleHeight;
        private int _OriginalTitleHeight = 25; // _TitleHeight Default Value
        protected Padding OriginalPadding => _OriginalPadding;
        private Padding _OriginalPadding = new Padding(3, 3, 3, 3); // _Padding Default Value

        /// <summary>
        /// GraphicPanel과 모든 자식 컨트롤의 배율을 지정한 배율 인수로 조정합니다.
        /// </summary>
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
                    float oldScaleValue = _ScaleValue;
                    Bounds = GetNewBoundsNSetOriginalValues(value, new Point(0, 0), out _ScaleValue);

                    // Call Events
                    ScaleValueChanged?.Invoke(this, new IScaleControlScaleValueChangedEventArgs()
                    {
                        OldScaleValue = oldScaleValue,
                        ScaleValue = _ScaleValue
                    });

                    RefreshAll();
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

            // Call Events
            ScaleValueChanged?.Invoke(this, new IScaleControlScaleValueChangedEventArgs()
            {
                OldScaleValue = scaleValue,
                ScaleValue = _ScaleValue
            });

            RefreshAll();
        }

        #region New Proertise
        //new public int Left
        //{
        //    get
        //    {
        //        return base.Left;
        //    }
        //    set
        //    {
        //        base.Left = value;
        //        OriginalLeft = (value * (1 / ScaleValue)).ToInt();
        //    }
        //}

        //new public int Top
        //{
        //    get
        //    {
        //        return base.Top;
        //    }
        //    set
        //    {
        //        base.Top = value;
        //        OriginalTop = (value * (1 / ScaleValue)).ToInt();
        //    }
        //}

        //new public Point Location
        //{
        //    get
        //    {
        //        return new Point(Left, Top);
        //    }
        //    set
        //    {
        //        if (value.X != Left || value.Y != Top)
        //        {
        //            base.Location = value;
        //            OriginalLeft = (value.X * (1 / ScaleValue)).ToInt();
        //            OriginalTop = (value.Y * (1 / ScaleValue)).ToInt();
        //        }
        //    }
        //}
        #endregion

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
                _OriginalPadding = Padding;
                _OriginalTitleHeight = TitleHeight;
            }

            Size newSize = new Size((int)Math.Round(_OriginalWidth * newScaleValue), (int)Math.Round(_OriginalHeight * newScaleValue));
            Point newLocation;
            if (Parent is DataModeler && Target is DataModeler)
            {
                Point parentInnerLocation = Target.InnerLocation;

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
                  (_OriginalPadding.Left * newScaleValue).ToInt()
                , (_OriginalPadding.Top * newScaleValue).ToInt()
                , (_OriginalPadding.Right * newScaleValue).ToInt()
                , (_OriginalPadding.Bottom * newScaleValue).ToInt()
            );
            TitleHeight = (_OriginalTitleHeight * newScaleValue).ToInt();

            SetChildrenSize(newScaleValue);
            _newScaleValue = newScaleValue;
            return new Rectangle(newLocation, newSize);
        }

        private void SetChildrenSize(float scaleValue)
        {
            foreach (GraphicControl control in Controls)
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