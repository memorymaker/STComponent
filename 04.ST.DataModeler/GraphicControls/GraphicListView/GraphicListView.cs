using ST.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public partial class GraphicListView : GraphicControl
    {
        #region User Options
        public bool AllowDrag = true;
        #endregion

        #region System Options
        #endregion

        #region Events
        // todo : ItemDrag, ItemDelete 구현 필요
        public event ItemDragEventHandler ItemDrag;
        public event ItemDeleteEventHandeler ItemDelete;
        #endregion

        #region Classes
        private GraphicScrollBar ScrollBarVertical;
        private int ScrollBarVerticalDefaultWidth = 14;
        public int VScrollBarWidth => ScrollBarVertical.Width;

        //private GraphicScrollBar ScrollBarHorizontal = new GraphicScrollBar();
        //private int ScrollBarHorizontalDefaultHeight = 7;
        #endregion

        #region Propertise
        public int ScrollTop
        {
            get
            {
                return _ScrollTop;
            }
            set
            {
                if (_ScrollTop != value)
                {
                    if (value < 0)
                    {
                        _ScrollTop = 0;
                    }
                    else
                    {
                        int scrollTopMax = Math.Max(Items.Count * (ItemHeight + ItemVerticalDistance) - (Height - ColumnHeight) + ItemRelativeTop, 0);
                        _ScrollTop = Math.Min(value, scrollTopMax);
                    }

                    if (!ScrollTopDrawStopOnce)
                    {
                        Refresh();
                    }
                    else
                    {
                        ScrollTopDrawStopOnce = false;
                    }
                }
            }
        }
        private int _ScrollTop = 0;
        private bool ScrollTopDrawStopOnce = false;

        public bool VerticalScrollBarVisible
        {
            get
            {
                return ScrollBarVertical.Visible;
            }
            set
            {
                ScrollBarVertical.Visible = value;
            }
        }

        // Todo : 스크롤 Horizontal 적용 필요
        public int ScrollLeft
        {
            get
            {
                return _ScrollLeft;
            }
            set
            {
                if (_ScrollLeft != value)
                {
                    if (value < 0)
                    {
                        _ScrollLeft = 0;
                    }
                    else
                    {
                        // Todo : 스크롤 최대값 적용 필요
                        _ScrollLeft = value;
                    }
                    Refresh();
                }
            }
        }
        private int _ScrollLeft = 0;

        public int FocusedIndex
        {
            get
            {
                return FocusedIndexOnly;
            }
            set
            {
                if (!(value == FocusedIndex && SelectedIndexes.Count == 1 && SelectedIndexes.Contains(FocusedIndex)))
                {
                    FocusedIndexOnly = value;
                    SelectedIndexes.Clear();
                    SelectedIndexes.Add(FocusedIndexOnly);
                    Refresh();
                }
            }
        }

        private int FocusedIndexOnly
        {
            get
            {
                return _FocusedIndexOnly;
            }
            set
            {
                int oldValue = _FocusedIndexOnly;

                if (value < 0)
                {
                    _FocusedIndexOnly = -1;
                }
                else
                {
                    if (Items.Count - 1 < value)
                    {
                        _FocusedIndexOnly = Items.Count - 1;
                    }
                    else
                    {
                        _FocusedIndexOnly = value;
                    }
                }

                if (oldValue != _FocusedIndexOnly)
                {
                    int scaleColumnHeight = (int)Math.Round(ColumnHeight * ScaleValue);

                    Rectangle rowRec = GetRowRectangle(FocusedIndexOnly);
                    if (rowRec.Top < scaleColumnHeight)
                    {
                        ScrollTopDrawStopOnce = true;
                        ScrollTop -= scaleColumnHeight - rowRec.Top;
                        ScrollBarVertical.Value = ScrollBarVertical.Minimum + ScrollTop;
                    }
                    else if (Height < rowRec.Bottom)
                    {
                        ScrollTopDrawStopOnce = true;
                        ScrollTop += rowRec.Bottom - Height + ItemRelativeTop;
                        ScrollBarVertical.Value = ScrollBarVertical.Minimum + ScrollTop;
                    }
                    Refresh();
                }
            }
        }
        private int _FocusedIndexOnly = -1;

        public GraphicListViewItem FocusedItem
        {
            get
            {
                return _FocusedIndexOnly >= 0
                    ? Items[FocusedIndexOnly]
                    : null;
            }
        }

        public List<int> SelectedIndexes
        {
            get
            {
                return _SelectedIndexes;
            }
        }
        private List<int> _SelectedIndexes = new List<int>();

        public List<GraphicListViewItem> SelectedItems
        {
            get
            {
                if (_SelectedIndexes.Count == 0)
                {
                    return null;
                }
                else
                {
                    List<GraphicListViewItem> rs = new List<GraphicListViewItem>();
                    var sortedSelectedItemIndexList = _SelectedIndexes;
                    sortedSelectedItemIndexList.Sort();
                    for(int i = 0; i < sortedSelectedItemIndexList.Count; i++)
                    {
                        rs.Add(Items[sortedSelectedItemIndexList[i]]);
                    }
                    return rs;
                }
            }
        }

        private bool ShiftProcess = false;
        private int ShiftProcessBaseIndex = 0;

        public float FullHeight
        {
            get
            {
                int scaleColumnHeight = (ColumnHeight * ScaleValue).ToInt();
                int scaleItemHeight = (ItemHeight * ScaleValue).ToInt();
                int scaleItemVerticalDistance = (ItemVerticalDistance * ScaleValue).ToInt();
                int scaleItemRelativeTop = (ItemRelativeTop * ScaleValue).ToInt();

                // todo: FullHeight min size 적용?
                float rs = scaleColumnHeight + _Items.Count * (scaleItemHeight + scaleItemVerticalDistance) + scaleItemRelativeTop;
                return rs;
            }
        }
        #endregion

        #region Element
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<GraphicListViewColumn> Columns
        {
            get
            {
                return _Columns;
            }
            set
            {
                _Columns.Clear();
                if (value != null)
                {
                    foreach (GraphicListViewColumn column in value)
                    {
                        _Columns.Add(new GraphicListViewColumn(column));
                    }
                }
                Refresh();
            }
        }
        private List<GraphicListViewColumn> _Columns = new List<GraphicListViewColumn>();
        private List<GraphicListViewColumn> AutoColumns = new List<GraphicListViewColumn>();

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ObservableCollection<GraphicListViewItem> Items { get { return _Items; } }
        public ObservableCollection<GraphicListViewItem> _Items = new ObservableCollection<GraphicListViewItem>();

        public DataTable Data { get; set; }
        #endregion

        #region Load
        public GraphicListView(DataModeler target) : base(target)
        {
            LoadInput();
            LoadThis();
            LoadDraw();
        }

        private void LoadThis()
        {
            SetDefault();
            SetScrollBar();
            SetEvents();
        }

        private void SetDefault()
        {
            TabStop = true;
            AllowDrop = true;
            BackColor = Color.White;
        }

        private void SetScrollBar()
        {
            // ScrollBarVertical
            ScrollBarVertical = new GraphicScrollBar(Target);
            Controls.Add(ScrollBarVertical);
            ScrollBarVertical.Visible = true;
            ScrollBarVertical.Width = ScrollBarVerticalDefaultWidth;
            ScrollBarVertical.Dock = DockStyle.Right;
            ScrollBarVertical.SmallChange = ItemHeight + ItemVerticalDistance;
            
            SizeChanged += (object sender, EventArgs e) =>
            {
                SetScrollBarMinMaxLargeSize();
            };

            ScrollBarVertical.ValueChanged += (object sender, UserScrollBarEventArgs e) =>
            {
                ScrollTop = e.Value - ScrollBarVertical.Minimum;
            };

            // ScrollBarVertical
            //Controls.Add(ScrollBarHorizontal);
            //ScrollBarHorizontal.Visible = true;
            //ScrollBarHorizontal.Height= ScrollBarVerticalDefaultWidth;
            //ScrollBarHorizontal.Dock = DockStyle.Bottom;
            //ScrollBarHorizontal.SmallChange = 100;

            //SizeChanged += (object sender, EventArgs e) =>
            //{
            //    SetScrollBarMinMaxLargeSize();
            //};

            //ScrollBarHorizontal.GraphicScrollBarValueChanged += (object sender, GraphicScrollBarEventArgs e) =>
            //{
            //    ScrollLeft = e.Value - ScrollBarHorizontal.Minimum;
            //};
        }
        #endregion

        #region Events
        private void SetEvents()
        {
            _Items.CollectionChanged += _Items_CollectionChanged;
            SizeChanged += UserListView_SizeChanged;
        }

        private void _Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    _Items.CollectionChanged -= _Items_CollectionChanged;
                    if (Data != null) {
                        Data.BeginLoadData();
                    }

                    foreach (var _newItem in e.NewItems)
                    {
                        GraphicListViewItem newItem = _newItem as GraphicListViewItem;
                        if (newItem.Row != null)
                        {
                            if (Data == null)
                            {
                                Data = newItem.Row.Table.Clone();
                            }

                            if (Data.Equals(newItem.Row.Table))
                            {
                                if (Data.PrimaryKey.Length > 0 && Data.Rows.Contains(newItem.Row))
                                {
                                    bool hasRow = false;
                                    for (int i = 0; i < Items.Count; i++)
                                    {
                                        if (Items[i].Row.Equals(newItem.Row))
                                        {
                                            hasRow = true;
                                            break;
                                        }
                                    }

                                    if (hasRow)
                                    {
                                        Items.Remove(newItem);
                                        throw new Exception("The row of the item is duplicated and cannot be added.");
                                    }
                                }
                                else
                                {
                                    Data.Rows.InsertAt(newItem.Row, newItem.Index);
                                }
                            }
                            else
                            {
                                _Items_CollectionChanged_ChangeRowNInsertData(newItem);
                            }
                        }
                        else if (newItem.RowItemArray != null)
                        {
                            _Items_CollectionChanged_CreateAutoData(newItem);
                            _Items_CollectionChanged_ChangeRowNInsertData(newItem);
                        }
                        else
                        {
                            throw new Exception("The Row and RowItemArray are empty and cannot be added.");
                        }
                    }

                    if (Data != null)
                    {
                        Data.EndLoadData();
                    }
                    _Items.CollectionChanged += _Items_CollectionChanged;

                    SetScrollBarMinMaxLargeSize();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var oldItem in e.OldItems)
                    {
                        Data.Rows.Remove(((GraphicListViewItem)oldItem).Row);
                    }

                    SetScrollBarMinMaxLargeSize();
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Data = null;

                    SetScrollBarMinMaxLargeSize();
                    break;
            }
        }

        private void _Items_CollectionChanged_CreateAutoData(GraphicListViewItem newItem)
        {
            if (Data == null)
            {
                Data = new DataTable();
                for (int i = 0; i < newItem.RowItemArray.Length; i++)
                {
                    Data.Columns.Add("Column" + i.ToString(), newItem.RowItemArray[i].GetType());
                }
            }
        }

        private void _Items_CollectionChanged_ChangeRowNInsertData(GraphicListViewItem newItem)
        {
            DataRow newRow = Data.NewRow();
            newRow.ItemArray = newItem.RowItemArray;
            
            Data.Rows.InsertAt(newRow, newItem.Index);
            newItem.Row = newRow;
        }

        private void UserListView_SizeChanged(object sender, EventArgs e)
        {
            RefreshAll();
        }
        #endregion

        #region Public Function
        public void AddColumn(GraphicListViewColumn userListViewColumn)
        {
            Columns.Add(new GraphicListViewColumn(userListViewColumn));
            Refresh();
        }

        public void RemoveColumn(GraphicListViewColumn userListViewColumn)
        {
            Columns.Remove(userListViewColumn);
            Refresh();
        }

        public int GetMaxColumnLength(string columnName)
        {
            int rs = 0;
            if (!Data.Columns.Contains(columnName))
            {
                throw new Exception($"Can not found the column. columnName : {columnName}");
            }
            else
            {
                foreach(DataRow row in Data.Rows)
                {
                    int nodeLength = row[columnName].ToString().Length;
                    if (rs < nodeLength)
                    {
                        rs = nodeLength;
                    }
                }
            }
            return rs;
        }

        public int GetMaxColumnLength(string[] columnNames)
        {
            int rs = 0;
            foreach (DataRow row in Data.Rows)
            {
                int nodeLength = 0;
                foreach (string columnName in columnNames)
                {
                    if (!Data.Columns.Contains(columnName))
                    {
                        throw new Exception($"Can not found the column. columnName : {columnNames}");
                    }
                    nodeLength += row[columnName].ToString().Length;
                }

                if (rs < nodeLength)
                {
                    rs = nodeLength;
                }
            }
            return rs;
        }

        public float[] GetMaxColumnsTextWidth(float scaleValue)
        {
            float _scaleValue = Math.Min(Math.Max(MinimumScaleValue, scaleValue), MaximumScaleValue);
            float[] rs = _GetMaxColumnsTextWidth(_scaleValue);
            return rs;
        }

        public float[] GetMaxColumnsTextWidth()
        {
            float[] rs = _GetMaxColumnsTextWidth(ScaleValue);
            return rs;
        }

        private float[] _GetMaxColumnsTextWidth(float scaleValue)
        {
            float[] rs = new float[Columns.Count];
            var g = CreateGraphics(this);
            for (int i = 0; i < Items.Count; i++)
            {
                for (int k = 0; k < Columns.Count; k++)
                {
                    var node = Items[i].Row[Columns[k].FieldName].ToString();
                    Font scaleFont = new Font(Items[i].Font.FontFamily, Items[i].Font.Size * scaleValue);
                    var size = g.MeasureString(node, scaleFont);
                    if (rs[k] < size.Width)
                    {
                        rs[k] = size.Width;
                    }
                }
            }
            return rs;
        }

        public void Bind(DataTable data)
        {
            if (Data != null)
            {
                Data.RowChanged -= BindProc_Data_RowChanged;
            }

            Data = data;
            Data.RowChanged += BindProc_Data_RowChanged;

            BindProcSetItems(Data, _Items);

            SetScrollBarMinMaxLargeSize();

            if (Columns == null || Columns.Count == 0)
            {
                foreach(DataColumn column in data.Columns)
                {
                    GraphicListViewColumn autoColumn = new GraphicListViewColumn(column.ColumnName, column.ColumnName);
                    AutoColumns.Add(autoColumn);
                }
            }
        }

        private void BindProc_Data_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            Refresh();
        }

        private void BindProcSetItems(DataTable data, ObservableCollection<GraphicListViewItem> items)
        {
            _Items.CollectionChanged -= _Items_CollectionChanged;

            items.Clear();
            foreach (DataRow row in data.Rows)
            {
                GraphicListViewItem item = new GraphicListViewItem(this, row);
                items.Add(item);
            }

            _Items.CollectionChanged += _Items_CollectionChanged;
        }
        #endregion

        #region UserListView Functions
        private void SetScrollBarMinMaxLargeSize()
        {
            if (Width > 0 && Height > 0)
            {
                // ------------ ScrollBarVertical
                // Scale
                int scaleColumnHeight = (int)Math.Round(ColumnHeight * ScaleValue);
                int scaleItemHeight = (int)Math.Round(ItemHeight * ScaleValue);
                int scaleItemVerticalDistance = (int)Math.Round(ItemVerticalDistance * ScaleValue);
                int scaleItemRelativeTop = (int)Math.Round(ItemRelativeTop * ScaleValue);

                // Minimum Maximum LargeChange
                ScrollBarVertical.Minimum = Height - scaleColumnHeight;
                ScrollBarVertical.Maximum = _Items.Count * (scaleItemHeight + scaleItemVerticalDistance) + scaleItemRelativeTop;
                if (ScrollBarVertical.Minimum > 0)
                {
                    ScrollBarVertical.LargeChange = Math.Max(
                          ScrollBarVertical.Minimum - (ScrollBarVertical.Minimum % ScrollBarVertical.SmallChange) - ScrollBarVertical.SmallChange
                        , scaleItemHeight + scaleItemVerticalDistance
                    );
                }

                // Size
                ScrollBarVertical.Width = (int)Math.Round(ScrollBarVerticalDefaultWidth * ScaleValue);
            }
        }

        private Rectangle GetRowRectangle(int index)
        {
            // Scale
            int scaleItemHeight = (int)Math.Round(ItemHeight * ScaleValue);
            int scaleItemVerticalDistance = (int)Math.Round(ItemVerticalDistance * ScaleValue);
            int scaleColumnHeight = (int)Math.Round(ColumnHeight * ScaleValue);

            int _itemHeightBlock = scaleItemHeight + scaleItemVerticalDistance;
            int top = (scaleColumnHeight + index * _itemHeightBlock) - ScrollTop;
            int bottom = top + _itemHeightBlock;
            int width = (from c in Columns
                         select c).Sum(t => (int)Math.Round(t.Width * ScaleValue));
            return new Rectangle(0, top, width, _itemHeightBlock);
        }
        #endregion
    }
}
