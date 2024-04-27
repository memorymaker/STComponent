using Newtonsoft.Json.Linq;
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

namespace ST.Controls
{
    public partial class UserScrollBar : UserControl
    {
        public delegate void UserScrollBarEventHandler(object sender, UserScrollBarEventArgs e);
        public event UserScrollBarEventHandler ValueChanged;

        public UserScrollBarType Type { 
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
                    Draw();
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
                    _Minimum = Math.Max(value, 0);

                    if (_Maximum < _Minimum)
                    {
                        _Maximum = _Minimum;
                    }

                    if (_Minimum > _Value || _Value == _Minimum)
                    {
                        _Value = _Minimum;
                    }

                    SetScrollButtonRectangle();
                    Draw();
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
                    _LargeChange = Math.Max(value, 0);
                    SetScrollButtonRectangle();
                    Draw();
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
                    _SmallChange = Math.Max(value, 0);
                    SetScrollButtonRectangle();
                    Draw();
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
                    Draw();

                    ValueChanged?.Invoke(this, new UserScrollBarEventArgs(_Value, oldValue));
                }
            }
        }
        private int _Value = 0;

        new public bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                if (base.Enabled != value)
                {
                    base.Enabled = value;
                }
                Draw();
            }
        }

        public UserScrollBar()
        {
            InitializeComponent();
            LoadThis();
        }

        private void LoadThis()
        {
            SetDefault();
            SetEvents();
            LoadObject();
            LoadInput();
        }

        private void SetDefault()
        {
            Width = 17;
            Height = 100;
        }

        private void SetEvents()
        {
            // Paint += UserScrollBar_Paint;
        }

        private void UserScrollBar_Paint(object sender, PaintEventArgs e)
        {
            Draw(e.Graphics);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            Draw(e.Graphics);
        }
    }

    public enum UserScrollBarType
    {
        Vertical, Horizontal
    }
}