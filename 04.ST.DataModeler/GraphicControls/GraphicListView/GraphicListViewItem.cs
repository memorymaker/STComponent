using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public class GraphicListViewItem
    {
        public GraphicListView Parent
        {
            get
            {
                return _Parent;
            }
        }
        private GraphicListView _Parent = null;

        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                Parent.Refresh();
            }
        }
        private bool _IsSelected = false;

        public Color BackColor
        {
            get
            {
                return _BackColor;
            }
            set
            {
                _BackColor = value;
                Parent?.Refresh();
            }
        }
        private Color _BackColor = Color.White;

        public Font Font
        {
            get
            {
                return _Font;
            }
            set
            {
                _Font = value;
                Parent?.Refresh();
            }
        }
        private Font _Font = new Font("맑은 고딕", 8F);

        public Color ForeColor
        {
            get
            {
                return _ForeColor;
            }
            set
            {
                _ForeColor = value;
                Parent?.Refresh();
            }
        }
        private Color _ForeColor = Color.Black;

        public GraphicListAlignType Align
        {
            get
            {
                return _Align;
            }
            set
            {
                _Align = value;
                Parent?.Refresh();
            }
        }
        private GraphicListAlignType _Align = GraphicListAlignType.None;

        public DataRow Row
        {
            get
            {
                return _Row;
            }
            set
            {
                if (value != _Row)
                {
                    if (value != null && !value.Table.Equals(Parent.Data))
                    {
                        throw new Exception("The row's table does not match the parent table.");
                    }
                    _Row = value;
                }
            }
        }
        private DataRow _Row = null;

        public object[] RowItemArray
        {
            get
            {
                return _Row != null ? _Row.ItemArray : _RowItemArray;
            }
        }
        private object[] _RowItemArray = null;

        public GraphicListViewItem(GraphicListView parent, DataRow row)
        {
            _Parent = parent;
            _Row = row;
        }

        public GraphicListViewItem(GraphicListView parent, object[] rowItemArray)
        {
            _Parent = parent;
            _RowItemArray = rowItemArray;
        }

        public object Tag;

        public string Text
        {
            get
            {
                return Row[0].ToString();
            }
        }

        public Rectangle Bounds
        {
            get
            {
                // Scale
                int _scaleItemHeight = (int)Math.Round(Parent.ItemHeight * Parent.ScaleValue);
                int _scaleItemVerticalDistance = (int)Math.Round(Parent.ItemVerticalDistance * Parent.ScaleValue);
                int scaleTop = (int)Math.Round(Parent.ItemRelativeTop * Parent.ScaleValue);
                int drawHeightRef = _scaleItemHeight + _scaleItemVerticalDistance;
                int index = Parent.Items.IndexOf(this);

                // Get
                Rectangle rowBounds = new Rectangle(
                    Parent.ItemRelativeLeft
                    , Parent.ColumnHeight + scaleTop + (drawHeightRef * index) - Parent.ScrollTop
                    , Parent.Width
                    , _scaleItemHeight
                );

                return rowBounds;
            }
        }

        public int Index
        {
            get
            {
                return Parent.Items.IndexOf(this);
            }
        }

        public bool Selected
        {
            get
            {
                return Parent.SelectedIndexes.Contains(this.Index);
            }
            set
            {
                if (value)
                {
                    if (!Parent.SelectedIndexes.Contains(this.Index))
                    {
                        Parent.SelectedIndexes.Add(this.Index);
                    }
                }
                else
                {
                    if (Parent.SelectedIndexes.Contains(this.Index))
                    {
                        Parent.SelectedIndexes.Remove(this.Index);
                    }
                }
            }
        }

        public List<int> TextOverFlowList = new List<int>();

        public Dictionary<string, string> ToolTipFormat = new Dictionary<string, string>();
    }
}