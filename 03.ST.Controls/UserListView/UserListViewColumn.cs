using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.Controls
{
    public class UserListViewColumn
    {
        public int Index
        {
            get
            {
                return _Index;
            }
            set
            {
                if (_Index != value)
                {
                    _Index = value;
                    Parent?.Draw();
                }
            }
        }
        private int _Index = 0; // 임시

        public int Width
        {
            get
            {
                return _Width;
            }
            set
            {
                if (_Width != value)
                {
                    _Width = value;
                    Parent?.Draw();
                }
            }
        }
        private int _Width = 100; // 임시

        public Color BackColor
        {
            get
            {
                return _BackColor;
            }
            set
            {
                if (_BackColor != value)
                {
                    _BackColor = value;
                    Parent?.Draw();
                }
            }
        }
        private Color _BackColor = Color.FromArgb(244, 244, 244);

        public Font Font
        {
            get
            {
                return _Font;
            }
            set
            {
                if (_Font != value)
                {
                    _Font = value;
                    Parent?.Draw();
                }
            }
        }
        private Font _Font = new Font("맑은 고딕", 9F);

        public Color ForeColor
        {
            get
            {
                return _ForeColor;
            }
            set
            {
                if (_ForeColor != value)
                {
                    _ForeColor = value;
                    Parent?.Draw();
                }
            }
        }
        private Color _ForeColor = Color.Black;

        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                if (_Text != value)
                {
                    _Text = value;
                    Parent?.Draw();
                }
            }
        }
        private string _Text = string.Empty;

        public string FieldName
        {
            get
            {
                return _FieldName;
            }
            set
            {
                if (_FieldName != value)
                {
                    _FieldName = value;
                    Parent?.Draw();
                }
            }
        }
        private string _FieldName = string.Empty;

        public UserListAlignType Align
        {
            get
            {
                return _Align;
            }
            set
            {
                if (_Align != value)
                {
                    _Align = value;
                    Parent?.Draw();
                }
            }
        }
        private UserListAlignType _Align = UserListAlignType.None;

        public UserListAlignType ItemAlign
        {
            get
            {
                return _ItemAlign;
            }
            set
            {
                if (_ItemAlign != value)
                {
                    _ItemAlign = value;
                    Parent?.Draw();
                }
            }
        }
        private UserListAlignType _ItemAlign = UserListAlignType.None;

        public UserListView Parent
        {
            get
            {
                return _Parent;
            }
        }
        private UserListView _Parent;

        public UserListViewColumn()
        {
        }

        public UserListViewColumn(string Text, string FieldName)
        {
            _Text = Text;
            _FieldName = FieldName;
        }

        public UserListViewColumn(UserListViewColumn userListViewColumn)
        {
            Index = userListViewColumn.Index;
            Width = userListViewColumn.Width;
            BackColor = userListViewColumn.BackColor;
            Font = userListViewColumn.Font;
            ForeColor = userListViewColumn.ForeColor;
            Text = userListViewColumn.Text;
            FieldName = userListViewColumn.FieldName;
            _Parent = userListViewColumn.Parent;
        }
    }
}