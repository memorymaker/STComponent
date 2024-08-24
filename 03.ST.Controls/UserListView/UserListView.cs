using ST.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace ST.Controls
{
    public partial class UserListView : UserControl
    {
        #region User Options
        /// <summary>
        /// 사용자가 리스트의 항목을 다른 컨트롤로 드래그 할 수 있는지 여부를 가져오거나 설정합니다.
        /// </summary>
        public bool AllowDrag = true;
        #endregion

        #region System Options
        #endregion

        #region Events
        public event ItemDragEventHandler ItemDrag;
        public event ItemDeleteEventHandeler ItemDoubleClick;
        #endregion

        #region Classes
        private UserScrollBar ScrollBarVertical = new UserScrollBar();
        private int ScrollBarVerticalDefaultWidth = SystemInformation.VerticalScrollBarWidth;
        //private int ScrollBarVerticalDefaultWidth = 14;

        //private UserScrollBar ScrollBarHorizontal = new UserScrollBar();
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
                        int scrollTopMax = Items.Count * (ItemHeight + ItemVerticalDistance) - (Height - ColumnHeight) + ItemRelativeTop;
                        _ScrollTop = Math.Min(value, scrollTopMax);
                    }

                    if (!ScrollTopDrawStopOnce)
                    {
                        Draw();
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
                    Draw();
                }
            }
        }
        private int _ScrollLeft = 0;

        public int SelectedItemIndex
        {
            get
            {
                return _SelectedItemIndex;
            }
            set
            {
                int oldValue = _SelectedItemIndex;

                if (value < 0)
                {
                    _SelectedItemIndex = -1;
                }
                else
                {
                    if (Items.Count - 1 < value)
                    {
                        _SelectedItemIndex = Items.Count - 1;
                    }
                    else
                    {
                        _SelectedItemIndex = value;
                    }
                }

                if (oldValue != _SelectedItemIndex)
                {
                    int scaleColumnHeight = (int)Math.Round(ColumnHeight * ScaleValue);

                    Rectangle rowRec = GetRowRectangle(SelectedItemIndex);
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
                }

                if (ModifierKeys == Keys.Shift)
                {
                    if (!ShiftProcess)
                    {
                        ShiftProcess = true;
                        ShiftProcessBaseIndex = oldValue;
                    }

                    SelectedItemIndexList.Clear();
                    for (int i = Math.Min(ShiftProcessBaseIndex, _SelectedItemIndex); i <= Math.Max(ShiftProcessBaseIndex, _SelectedItemIndex); i++)
                    {
                        SelectedItemIndexList.Add(i);
                    }
                }
                else if (ModifierKeys == Keys.Control)
                {
                    // todo : 포커스만 움직이도록 수정 필요(키보드 입력일 때 다른 처리 필요)

                    ShiftProcess = false;

                    if (!SelectedItemIndexList.Contains(_SelectedItemIndex))
                    {
                        SelectedItemIndexList.Add(_SelectedItemIndex);
                    }
                    else
                    {
                        SelectedItemIndexList.Remove(_SelectedItemIndex);
                    }
                }
                else
                {
                    ShiftProcess = false;

                    SelectedItemIndexList.Clear();
                    if (_SelectedItemIndex >= 0)
                    {
                        SelectedItemIndexList.Add(_SelectedItemIndex);
                    }
                }

                Draw();
            }
        }
        private int _SelectedItemIndex = -1;

        public int VScrollBarWidth => ScrollBarVertical.Width;

        public UserListViewItem SelectedItem
        {
            get
            {
                return _SelectedItemIndex >= 0
                    ? Items[SelectedItemIndex]
                    : null;
            }
        }

        public List<int> SelectedItemIndexList
        {
            get
            {
                return _SelectedItemIndexList;
            }
        }
        private List<int> _SelectedItemIndexList = new List<int>();

        public List<UserListViewItem> SelectedItems
        {
            get
            {
                if (_SelectedItemIndexList.Count == 0)
                {
                    return null;
                }
                else
                {
                    List<UserListViewItem> rs = new List<UserListViewItem>();
                    var sortedSelectedItemIndexList = _SelectedItemIndexList;
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

        public int FullHeight
        {
            get
            {
                int scaleColumnHeight = (int)Math.Round(ColumnHeight * ScaleValue);
                int scaleItemHeight = (int)Math.Round(ItemHeight * ScaleValue);
                int scaleItemVerticalDistance = (int)Math.Round(ItemVerticalDistance * ScaleValue);
                int scaleItemRelativeTop = (int)Math.Round(ItemRelativeTop * ScaleValue);

                // todo: FullHeight min size 적용?
                int rs = scaleColumnHeight + _Items.Count * (scaleItemHeight + scaleItemVerticalDistance) + scaleItemRelativeTop;
                return rs;
            }
        }

        public int NumberOfVisibleItems
        {
            get
            {
                int scaleColumnHeight = (int)Math.Round(ColumnHeight * ScaleValue);
                int scaleItemHeight = (int)Math.Round(ItemHeight * ScaleValue);
                int scaleItemVerticalDistance = (int)Math.Round(ItemVerticalDistance * ScaleValue);
                int scaleItemRelativeTop = (int)Math.Round(ItemRelativeTop * ScaleValue);

                int rs = (int)Math.Ceiling(((float)Height - scaleColumnHeight - scaleItemRelativeTop) / (scaleItemHeight + scaleItemVerticalDistance));
                return rs;
            }
        }

        /// <summary>
        /// 컬럼의 크기가 자동으로 조정되는 방법을 가져오거나 설정합니다.
        /// </summary>
        public UserListAutoSizeType AutoSizeType
        {
            get
            {
                return _AutoSizingType;
            }
            set
            {
                _AutoSizingType = value;
                SetAutoSize(_AutoSizingType);
                Draw();
            }
        }
        private UserListAutoSizeType _AutoSizingType = UserListAutoSizeType.LeftFirst;

        /// <summary>
        /// 컨트롤이 사용자 상호 작용에 응답할 수 있는지를 나타내는 값을 가져오거나 설정합니다.
        /// </summary>
        /// <returns>컨트롤이 사용자 상호 작용에 응답할 수 있으면 <see langword="true"/>이고, 그렇지 않으면 <see langword="false"/>입니다. 기본값은 <see langword="true"/>입니다.</returns>
        new public bool Enabled
        {
            get
            {
                return _Enabled;
            }
            set
            {
                this.BeginControlUpdate();
                _Enabled = value;
                AllowDrop = _Enabled;
                ScrollBarVertical.Enabled = _Enabled;
                this.EndControlUpdate();
            }
        }
        private bool _Enabled = true;
        #endregion

        #region Element
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<UserListViewColumn> Columns
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
                    foreach (UserListViewColumn column in value)
                    {
                        _Columns.Add(new UserListViewColumn(column));
                    }
                }
                Draw();
            }
        }
        private List<UserListViewColumn> _Columns = new List<UserListViewColumn>();
        private List<UserListViewColumn> AutoColumns = new List<UserListViewColumn>();

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ObservableCollection<UserListViewItem> Items { get { return _Items; } }

        public ObservableCollection<UserListViewItem> _Items = new ObservableCollection<UserListViewItem>();

        private DataTable Data;
        #endregion

        #region Load
        public UserListView()
        {
            InitializeComponent();
            LoadInput();
            LoadThis();
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
            Cursor = Cursors.Default;
        }

        private void SetScrollBar()
        {
            // ScrollBarVertical
            ScrollBarVertical.BlockDrawing = true;
            ScrollBarVertical.Visible = true;
            ScrollBarVertical.Width = ScrollBarVerticalDefaultWidth;
            ScrollBarVertical.Dock = DockStyle.Right;
            ScrollBarVertical.SmallChange = ItemHeight + ItemVerticalDistance;
            ScrollBarVertical.BackColor = Color.FromArgb(245, 245, 245);
            ScrollBarVertical.DisableBrightnessColorPoint = -0.1f;
            Controls.Add(ScrollBarVertical);
            ScrollBarVertical.BlockDrawing = false;

            SizeChanged += (object sender, EventArgs e) =>
            {
                SetScrollBarMinMaxLargeSize();
            };

            ScrollBarVertical.ValueChanged += (object sender, UserScrollBarEventArgs e) =>
            {
                ScrollTop = e.Value - ScrollBarVertical.Minimum;
            };

            ScrollBarVertical.TabStop = false;
            ScrollBarVertical.MouseDown += (object sender, MouseEventArgs e) =>
            {
                ActiveControl = null;
            };
            ScrollBarVertical.GotFocus += (object sender, EventArgs e) =>
            {
                ActiveControl = null;
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

            //ScrollBarHorizontal.UserScrollBarValueChanged += (object sender, UserScrollBarEventArgs e) =>
            //{
            //    ScrollLeft = e.Value - ScrollBarHorizontal.Minimum;
            //};
        }
        #endregion

        #region Events
        private void SetEvents()
        {
            //_Items.CollectionChanged += _Items_CollectionChanged;
            SizeChanged += UserListView_SizeChanged;
            //Paint += UserListView_Paint;
        }

        private void _Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newItem in e.NewItems)
                    {
                        Data.Rows.Add(((UserListViewItem)newItem).Row);
                    }

                    SetScrollBarMinMaxLargeSize();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var oldItem in e.OldItems)
                    {
                        Data.Rows.Remove(((UserListViewItem)oldItem).Row);
                    }

                    SetScrollBarMinMaxLargeSize();
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Data.Clear();

                    SetScrollBarMinMaxLargeSize();
                    break;
            }
        }

        private void UserListView_SizeChanged(object sender, EventArgs e)
        {
            SetAutoSize(AutoSizeType);
            Draw();
        }

        private void UserListView_Paint(object sender, PaintEventArgs e)
        {
            Draw();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            Draw();
        }
        #endregion

        #region Public Function
        /// <summary>
        /// UserList의 모든 항목을 삭제합니다.
        /// </summary>
        public void Clear()
        {
            if (Data != null)
            {
                Data.RowChanged -= BindProc_Data_RowChanged;
            }
            Data = null;
            _Items = new ObservableCollection<UserListViewItem>();

            SelectedItemIndexList.Clear();
            SelectedItemIndex = -1;
            
            Draw();
        }

        /// <summary>
        /// 컬럼을 추가합니다.
        /// </summary>
        /// <param name="userListViewColumn"></param>
        public void AddColumn(UserListViewColumn userListViewColumn)
        {
            Columns.Add(new UserListViewColumn(userListViewColumn));
            Draw();
        }

        /// <summary>
        /// 컬럼을 제거합니다.
        /// </summary>
        /// <param name="userListViewColumn"></param>
        public void RemoveColumn(UserListViewColumn userListViewColumn)
        {
            Columns.Remove(userListViewColumn);
            Draw();
        }

        /// <summary>
        /// 데이터를 바인딩합니다.
        /// </summary>
        /// <param name="data"></param>
        public void Bind(DataTable data)
        {
            BlockDrawing = true;

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
                AutoColumns.Clear();
                foreach (DataColumn column in data.Columns)
                {
                    UserListViewColumn autoColumn = new UserListViewColumn(column.ColumnName, column.ColumnName);
                    AutoColumns.Add(autoColumn);
                }
            }

            SelectedItemIndex = -1;
            SetAutoSize(AutoSizeType);

            BlockDrawing = false;
            Draw();
        }

        private void BindProc_Data_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            Draw();
        }

        private void BindProcSetItems(DataTable data, ObservableCollection<UserListViewItem> items)
        {
            items.Clear();
            foreach (DataRow row in data.Rows)
            {
                UserListViewItem item = new UserListViewItem(this, row);
                item.Font = Font;
                items.Add(item);
            }
        }
        #endregion

        #region UserListView Functions
        private void SetScrollBarMinMaxLargeSize()
        {
            // ------------ ScrollBarVertical
            ScrollBarVertical.SuspendLayout();

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
            
            ScrollBarVertical.ResumeLayout();
            ScrollBarVertical.Draw();
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

        private void SetAutoSize(UserListAutoSizeType type)
        {
            var t1 = this.Width;
            int widthRevision = 6;

            switch (type)
            {
                case UserListAutoSizeType.LeftFirst:
                    {
                        List<UserListViewColumn> targetColumns;
                        if (Columns.Count == 0 && AutoColumns.Count > 0)
                        {
                            targetColumns = AutoColumns;
                        }
                        else
                        {
                            targetColumns = Columns;
                        }

                        int columnsCount = targetColumns.Count;
                        int[] widthArr = new int[columnsCount];

                        // Set Items, widthArr
                        using (var g = CreateGraphics())
                        {
                            // Captions Default Width
                            for (int i = 0; i < columnsCount; i++)
                            {
                                // widthArr
                                SizeF _textSize = g.MeasureString(targetColumns[i].Text.ToString(), Font);
                                int textWidth = (int)Math.Ceiling(_textSize.Width);
                                if (widthArr[i] < textWidth)
                                {
                                    widthArr[i] = textWidth;
                                }
                            }

                            if (Data != null)
                            {
                                foreach (DataRow dr in Data.Rows)
                                {
                                    for (int i = 0; i < columnsCount; i++)
                                    {
                                        // widthArr
                                        SizeF _textSize = g.MeasureString(dr[targetColumns[i].FieldName].ToString(), Font);
                                        int textWidth = (int)Math.Ceiling(_textSize.Width);
                                        if (widthArr[i] < textWidth)
                                        {
                                            widthArr[i] = textWidth;
                                        }
                                    }
                                }
                            }
                        }

                        // Add Columns & Set Size
                        int widthAppend = 0;
                        for (int i = 0; i < columnsCount; i++)
                        {
                            int nodeWidth = widthArr[i] + widthRevision;
                            int originalScrollBarVerticalWidth = (ScrollBarVertical.Width * (1 / ScaleValue)).ToInt();
                            int originalWidth = ScaleValue == 1f && OriginalWidth == 0 && Width != 0
                                ? Width
                                : OriginalWidth;

                            if (originalWidth - originalScrollBarVerticalWidth < nodeWidth + widthAppend)
                            {
                                nodeWidth = Math.Max(0, originalWidth - originalScrollBarVerticalWidth - widthAppend);
                            }

                            if (i == columnsCount - 1 && nodeWidth + widthAppend < originalWidth - originalScrollBarVerticalWidth)
                            {
                                nodeWidth = originalWidth - originalScrollBarVerticalWidth - widthAppend;
                            }

                            targetColumns[i].Width = nodeWidth;
                            widthAppend += nodeWidth;
                        }
                    }
                    break;
                case UserListAutoSizeType.Fairly:
                    // Need code
                    break;
            }
        }
        #endregion
    }
}
