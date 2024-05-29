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

namespace ST.Controls
{
    public class UserListViewItem
    {
        public UserListView Parent
        {
            get
            {
                return _Parent;
            }
        }
        private UserListView _Parent = null;

        public Dictionary<object, UserListViewSubItem> SubItems
        {
            get
            {
                return _SubItems;
            }
        }
        private Dictionary<object, UserListViewSubItem> _SubItems = new Dictionary<object, UserListViewSubItem>();

        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                Parent.Draw();
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
                Parent?.Draw();
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
                Parent?.Draw();
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
                Parent?.Draw();
            }
        }
        private Color _ForeColor = Color.Black;

        public UserListAlignType Align
        {
            get
            {
                return _Align;
            }
            set
            {
                _Align = value;
                Parent?.Draw();
            }
        }
        private UserListAlignType _Align = UserListAlignType.None;

        public DataRow Row
        {
            get
            {
                return _Row;
            }
        }
        private DataRow _Row = null;

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
                return Parent.SelectedItemIndexList.Contains(this.Index);
            }
            set
            {
                if (value)
                {
                    if (!Parent.SelectedItemIndexList.Contains(this.Index))
                    {
                        Parent.SelectedItemIndexList.Add(this.Index);
                    }
                }
                else
                {
                    if (Parent.SelectedItemIndexList.Contains(this.Index))
                    {
                        Parent.SelectedItemIndexList.Remove(this.Index);
                    }
                }
            }
        }

        public UserListViewItem(UserListView parent, DataRow row)
        {
            _Parent = parent;
            _Row = row;
            for(int i = 0; i < row.Table.Columns.Count; i++)
            {
                UserListViewSubItem subItem = new UserListViewSubItem(this);
                _SubItems.Add(i, subItem);
                _SubItems.Add(row.Table.Columns[i].ColumnName, subItem);
            }
        }
    }

    public class UserListViewSubItem
    {
        public UserListViewItem Parent
        {
            get
            {
                return _Parent;
            }
        }
        private UserListViewItem _Parent = null;

        public Color BackColor
        {
            get
            {
                return _BackColor;
            }
            set
            {
                _BackColor = value;
                Parent?.Parent?.Draw();
            }
        }
        private Color _BackColor = Color.Empty;

        public Font Font
        {
            get
            {
                return _Font;
            }
            set
            {
                _Font = value;
                Parent?.Parent?.Draw();
            }
        }
        private Font _Font = null;

        public Color ForeColor
        {
            get
            {
                return _ForeColor;
            }
            set
            {
                _ForeColor = value;
                Parent?.Parent?.Draw();
            }
        }
        private Color _ForeColor = Color.Empty;

        public UserListAlignType Align
        {
            get
            {
                return _Align;
            }
            set
            {
                _Align = value;
                Parent?.Parent?.Draw();
            }
        }
        private UserListAlignType _Align = UserListAlignType.None;

        public UserListViewSubItem(UserListViewItem parent)
        {
            _Parent = parent;
        }
    }
}