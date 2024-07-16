using ST.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public partial class TableNode : NodeBase
    {
        #region Values
        private GraphicListView InnerListView;
        private int AutoMinWidth = 200;
        private int AutoMaxWidth = 500;
        private int AutoMinColumnWidth = 50;
        private int[] ColumnWidthRevision = new int[] { 10, 15 };

        private Color PKColumnForeColor = Color.FromArgb(188, 88, 22);
        string PKColumnName = DataModeler.NODE.NODE_DETAIL_IS_PRIMARY_KEY;
        #endregion

        #region EventHandler
        public event UserEventHandler DragDropItems;
        #endregion

        #region Propertise
        public override NodeType NodeType
        {
            get
            { 
                return NodeType.TableNode;
            }
        }

        public override string ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    Title = ID + "(" + SEQ.ToString() + ")";
                }
            }
        }

        public override int SEQ
        {
            get
            {
                return _SEQ;
            }
            set
            {
                if (_SEQ != value)
                {
                    _SEQ = value;
                    Title = ID + "(" + SEQ.ToString() + ")";
                }
            }
        }

        public float ListHeight
        {
            get
            {
                return InnerListView.FullHeight;
            }
        }

        public ObservableCollection<GraphicListViewItem> Items => InnerListView.Items;
        #endregion

        #region Load
        public TableNode(DataModeler target) : base(target)
        {
            LoadThis();
            LoadInput();
        }

        private void LoadThis()
        {
            SetDefault();
        }

        private void SetDefault()
        {
            // Base Propertise
            _AutoSize = true;

            // GraphicMouseAction
            GraphicMouseAction.UseSizing = false;

            // This Propertise
            BackColor = Color.FromArgb(230, 121, 47);

            // GraphicListView
            InnerListView = new GraphicListView(Target);
            InnerListView.Dock = DockStyle.Fill;
            InnerListView.VerticalScrollBarVisible = false;
            InnerListView.ColumnHeight = 0;
            InnerListView.Font = Font;
            InnerListView.AddColumn(new GraphicListViewColumn("Column", DataModeler.NODE.NODE_DETAIL_VIEW_COLUMN1));
            InnerListView.AddColumn(new GraphicListViewColumn("Comment", DataModeler.NODE.NODE_DETAIL_VIEW_COLUMN2));
            InnerListView.Columns[0].Width = 150 - Padding.Left;
            InnerListView.Columns[1].Width = 100 - Padding.Right;
            Controls.Add(InnerListView);

            // UserPanelControl
            EnableCaptionEdit = false;
        }
        #endregion
         
        #region Public
        public GraphicListViewItem GetItemFromRowValue(string column, object value)
        {
            GraphicListViewItem rs = null;
            foreach (GraphicListViewItem item in Items)
            {
                if (item.Row[column].Equals(value))
                {
                    rs = item;
                    break;
                }
            }
            return rs;
        }

		public GraphicListViewItem GetItemFromRowValue(string[] columns, object[] values)
		{
			GraphicListViewItem rs = null;
            if (columns.Length != values.Length)
            {
                // todo: exception 문구 수정 필요
                throw new Exception("columns, values count is not equals.");
            }
            else
            {
			    foreach (GraphicListViewItem item in Items)
			    {
                    bool equals = true;
                    for(int i = 0; i < columns.Length; i++)
                    {
                        if (!(
                            (item.Row[columns[i]] == null && values[i] == null)
                            ||
                            (item.Row[columns[i]] != null && item.Row[columns[i]].ToString() == values[i].ToString())
                        ))
                        {
                            equals = false;
                            break;
                        }
				    }
                    if (equals)
                    {
				    	rs = item;
                    }
				}
            }
			return rs;
		}
		#endregion

		#region Function
		public void Bind(DataTable data)
        {
            InnerListView.BlockRefresh = true;
            InnerListView.Bind(data);
            for(int i = 0; i < InnerListView.Items.Count; i++)
            {
                if (InnerListView.Data.Columns.Contains(PKColumnName) && InnerListView.Items[i].Row[PKColumnName].ToString() == "Y")
                {
                    InnerListView.Items[i].ForeColor = PKColumnForeColor;
                }
            }
            InnerListView.BlockRefresh = false;

            if (AutoSize)
            {
                SetAutoSize();
            }
        }

        public Rectangle GetItemRectangle(GraphicListViewItem item)
        {
            Rectangle rs = new Rectangle(
                Bounds.Left + item.Bounds.Left + InnerListView.Left
                , Bounds.Top + item.Bounds.Top + InnerListView.Top
                , item.Bounds.Width
                , item.Bounds.Height
            );
            return rs;
        }

        public Rectangle GetColumnRectangle(ListViewItem item)
        {
            Rectangle rs = new Rectangle(
                Bounds.Left + item.Bounds.Left + InnerListView.Left
                , Bounds.Top + item.Bounds.Top + InnerListView.Top
                , item.Bounds.Width
                , item.Bounds.Height
            );
            return rs;
        }

        private GraphicListViewItem GetItem(GraphicListView lst, int x, int y)
        {
            Rectangle lstScreenRectangle = RectangleToScreen(lst.Bounds);
            x -= lst.Padding.Left + lstScreenRectangle.Left;
            y -= lst.Padding.Top + lstScreenRectangle.Top;

            foreach (GraphicListViewItem item in lst.Items)
            {
                if (item.Bounds.X <= x && x <= item.Bounds.X + item.Bounds.Width
                    && item.Bounds.Y <= y && y <= item.Bounds.Y + item.Bounds.Height)
                {
                    return item;
                }
            }
            return null;
        }
        #endregion

        #region Override
        public override NodeModel GetNodeModel()
        {
            var rs = new NodeModel
            {
                NODE_ID = ID,
                NODE_SEQ = SEQ,
                NODE_TYPE = NodeType.GetStringValue(),
                NODE_LEFT = AbsoluteLeft,
                NODE_TOP = AbsoluteTop,
                NODE_WIDTH = OriginalWidth == 0 ? Width : OriginalWidth,
                NODE_HEIGHT = OriginalHeight == 0 ? Height : _OriginalHeight,
                NODE_Z_INDEX = ZIndex,
                NODE_OPTION = NodeOption,
                NODE_NOTE = NodeNote,
                NODE_DETAIL = NodeDetailModelList()
            };
            return rs;
        }

        private List<NodeDetailModel> NodeDetailModelList()
        {
            var rs = new List<NodeDetailModel>();
			for (int i = 0; i < Items.Count; i++)
            {
				GraphicListViewItem item = Items[i];
                var model = new NodeDetailModel
                {
                    NODE_ID                    = item.Row[DataModeler.NODE.NODE_ID].ToString(),
                    NODE_SEQ                   = Convert.ToInt32(item.Row[DataModeler.NODE.NODE_SEQ]),
					NODE_DETAIL_ID             = item.Row[DataModeler.NODE.NODE_DETAIL_ID].ToString(),
                    NODE_DETAIL_SEQ            = Convert.ToInt32(item.Row[DataModeler.NODE.NODE_DETAIL_SEQ]),
					NODE_DETAIL_ORDER          = i + 1,
                    NODE_DETAIL_TYPE           = item.Row[DataModeler.NODE.NODE_DETAIL_TYPE].ToString(),
                    NODE_DETAIL_DATA_TYPE      = item.Row[DataModeler.NODE.NODE_DETAIL_DATA_TYPE].ToString(),
                    NODE_DETAIL_DATA_TYPE_FULL = item.Row[DataModeler.NODE.NODE_DETAIL_DATA_TYPE_FULL].ToString(),
                    NODE_DETAIL_COMMENT        = item.Row[DataModeler.NODE.NODE_DETAIL_COMMENT].ToString(),
                    NODE_DETAIL_TABLE_ALIAS    = item.Row[DataModeler.NODE.NODE_DETAIL_TABLE_ALIAS].ToString(),
                    NODE_DETAIL_NOTE           = item.Row[DataModeler.NODE.NODE_DETAIL_NOTE].ToString(),
                    NODE_DETAIL_VIEW_COLUMN1   = item.Row[DataModeler.NODE.NODE_DETAIL_VIEW_COLUMN1].ToString(),
                    NODE_DETAIL_VIEW_COLUMN2   = item.Row[DataModeler.NODE.NODE_DETAIL_VIEW_COLUMN2].ToString(),
                    NODE_ID_REF                = null,
                    NODE_SEQ_REF               = 0,
                };
                rs.Add(model);
			}
            return rs;
        }
        #endregion

        #region IScaleControl
        public override Color MinimapColor { get; set; } = Color.FromArgb(230, 121, 47);
        #endregion
    }
}