using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace ST.DataModeler
{
    public partial class GraphicScrollBar : GraphicControl
    {
        public delegate void UserScrollBarEventHandler(object sender, UserScrollBarEventArgs e);
        public event UserScrollBarEventHandler ValueChanged;

        public UserScrollBarType Type
        {
            get
            {
                return _Type;
            }
            set
            {
                if (value != _Type)
                {
                    _Type = value;
                    SetButtonsRectangle();
                }
            }
        }
        private UserScrollBarType _Type = UserScrollBarType.Vertical;

        public int Maximum
        {
            get
            {
                return _Maximum;
            }
            set
            {
                if (_Maximum != value)
                {
                    if (_Minimum > value)
                    {
                        _Minimum = value;
                    }

                    if (value < this._Value)
                    {
                        Value = value;
                    }

                    _Maximum = value;
                    SetScrollButtonRectangle();
                    Refresh();
                }
            }
        }
        private int _Maximum = 100;

        public int Minimum
        {
            get
            {
                return _Minimum;
            }
            set
            {
                if (_Minimum != value)
                {
                    if (value < 0)
                    {
                        throw new Exception(string.Format("Minimum InvalidLowBoundArgumentEx {0} {1}", value.ToString(CultureInfo.CurrentCulture), 0.ToString(CultureInfo.CurrentCulture)));
                    }

                    if (_Maximum < value)
                    {
                        _Maximum = value;
                    }

                    if (value > this._Value)
                    {
                        this._Value = value;
                    }

                    _Minimum = value;
                    SetScrollButtonRectangle();
                    Refresh();
                }
            }
        }
        private int _Minimum = 0;

        public int LargeChange
        {
            get
            {
                return Math.Min(_LargeChange, _Maximum - _Minimum + 1);
            }
            set
            {
                if (_LargeChange != value)
                {
                    if (value < 0)
                    {
                        value = 0;
                        //throw new Exception(string.Format("LargeChange InvalidLowBoundArgumentEx {0} {1}", value.ToString(CultureInfo.CurrentCulture), 0.ToString(CultureInfo.CurrentCulture)));
                    }
                    _LargeChange = value;
                    SetScrollButtonRectangle();
                    Refresh();
                }
            }
        }
        private int _LargeChange = 10;

        public int SmallChange
        {
            get
            {
                return Math.Min(_SmallChange, LargeChange);
            }
            set
            {
                if (_SmallChange != value)
                {
                    if (value < 0)
                    {
                        throw new Exception(string.Format("SmallChange InvalidLowBoundArgumentEx {0} {1}", value.ToString(CultureInfo.CurrentCulture), 0.ToString(CultureInfo.CurrentCulture)));
                    }

                    _SmallChange = value;
                    SetScrollButtonRectangle();
                    Refresh();
                }
            }
        }
        private int _SmallChange = 1;

        public int Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (_Value != value)
                {
                    int oldValue = _Value;
                    _Value = Math.Min(Math.Max(value, Minimum), Maximum);
                    SetScrollButtonRectangle();
                    Refresh();

                    ValueChanged?.Invoke(this, new UserScrollBarEventArgs(_Value, oldValue));
                }
            }
        }
        private int _Value = 0;

        public GraphicScrollBar(DataModeler target) : base(target)
        {
            LoadThis();
            LoadObject();
            LoadInput();
            LoadDraw();
        }

        private void LoadThis()
        {
            // Default
            Width = 17;
            Height = 100;
            Focused = false;
        }
    }

    public enum UserScrollBarType
    {
        Vertical, Horizontal
    }
}