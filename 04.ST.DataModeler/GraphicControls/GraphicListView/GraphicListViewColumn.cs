using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public class GraphicListViewColumn
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
                    Parent?.Refresh();
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
                    Parent?.Refresh();
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
                    Parent?.Refresh();
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
                    Parent?.Refresh();
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
                    Parent?.Refresh();
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
                    Parent?.Refresh();
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
                    Parent?.Refresh();
                }
            }
        }
        private string _FieldName = string.Empty;

        public GraphicListAlignType Align
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
                    Parent?.Refresh();
                }
            }
        }
        private GraphicListAlignType _Align = GraphicListAlignType.None;

        public GraphicListAlignType ItemAlign
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
                    Parent?.Refresh();
                }
            }
        }
        private GraphicListAlignType _ItemAlign = GraphicListAlignType.None;

        public GraphicListView Parent
        {
            get
            {
                return _Parent;
            }
        }
        private GraphicListView _Parent;

        public GraphicListViewColumn()
        {
        }

        public GraphicListViewColumn(string Text, string FieldName)
        {
            _Text = Text;
            _FieldName = FieldName;
        }

        public GraphicListViewColumn(GraphicListViewColumn userListViewColumn)
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