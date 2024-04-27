using ST.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public partial class ColumnNode : NodeBase
    {
        #region Values
        private GraphicListView InnerListView;

        private Color UserColumnForeColor = Color.FromArgb(56, 99, 141);
        private string ColumnTypeName = "NODE_DETAIL_TYPE";
        #endregion

        #region EventHandler
        public event UserEventHandler DragDropItems;
        #endregion

        #region Propertise
        public override NodeType NodeType { get; set; } = NodeType.ColumnNode;

        override public string ID
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
                    Title = ID;
                }
            }
        }

        override public int SEQ
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

        public ObservableCollection<GraphicListViewItem> Items
        {
            get
            {
                return InnerListView.Items;
            }
        }

        override public string NodeOption
        {
            get
            {
                return _NodeOption;
            }
            set
            {
                if (!_NodeOptionList.Contains(value)) {
                    throw new Exception($"ViewOption is not vaild. value : {value}");
                }
                else
                {
                    if (_NodeOptionList.Contains(value))
                    {
                        _NodeOption = value;
                    }
                    else
                    {
                        _NodeOption = DefaultNodeOption;
                    }

                    NodeOptionShowTableAlias = _NodeOption[1] == 'T';
                    NodeOptionShowComment = _NodeOption[2] == 'C';

                    SetViewColumn();
                    SetAutoSize();
                }
            }
        }
        private string _NodeOption = "CTC"; // C: ColumnNode, T: Table Alias, C: Comment
        private string[] _NodeOptionList = new string[] { "CTC", "CT_", "C_C", "C__" };
        private bool NodeOptionShowTableAlias = true;
        private bool NodeOptionShowComment = true;
        private string DefaultNodeOption = "CTC";
        #endregion

        #region Load
        public ColumnNode(DataModeler target) : base(target)
        {
            LoadThis();
            LoadInput();
            LoadDraw();
        }

        private void LoadThis()
        {
            SetDefault();
        }

        private void SetDefault()
        {
            // Base Propertise
            _AutoSize = true;
            EnableCaptionEdit = true;

            // GraphicMouseAction
            GraphicMouseAction.UseSizing = false;

            // This Propertise
            BackColor = Color.FromArgb(77, 130, 184);

            // GraphicListView
            InnerListView = new GraphicListView(Target);
            InnerListView.Dock = DockStyle.Fill;
            InnerListView.VerticalScrollBarVisible = false;
            InnerListView.ColumnHeight = 0;
            InnerListView.Font = Font;
            InnerListView.AddColumn(new GraphicListViewColumn("Column", "NODE_DETAIL_VIEW_COLUMN1"));
            InnerListView.AddColumn(new GraphicListViewColumn("Comment", "NODE_DETAIL_VIEW_COLUMN2"));
            InnerListView.Columns[0].Width = 150 - Padding.Left;
            InnerListView.Columns[1].Width = 100 - Padding.Right;
            Controls.Add(InnerListView);

            // UserPanelControl
            EnableCaptionEdit = true;

            // Set ViewOption
            NodeOption = _NodeOption;

            // Default
            SetAutoSize();
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
        #endregion

        #region Function
        public void Bind(DataTable data)
        {
            InnerListView.BlockRefresh = false;
            InnerListView.Bind(data);
            for (int i = 0; i < InnerListView.Items.Count; i++)
            {
                if (InnerListView.Data.Columns.Contains(ColumnTypeName) && InnerListView.Items[i].Row[ColumnTypeName].ToString() == "U")
                {
                    InnerListView.Items[i].ForeColor = UserColumnForeColor;
                }
            }
            InnerListView.BlockRefresh = false;

            if (!string.IsNullOrWhiteSpace(NodeOption))
            {
                SetViewColumn();
            }
            SetAutoSize();
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

        private GraphicListViewItem GetItem(GraphicListView lst, int screenX, int screenY)
        {
            Rectangle lstScreenRectangle = RectangleToScreen(lst.Bounds);
            screenX -= lst.Padding.Left + lstScreenRectangle.Left;
            screenY -= lst.Padding.Top + lstScreenRectangle.Top;

            int itemVerticalDistance = (InnerListView.ItemVerticalDistance * InnerListView.ScaleValue).ToInt();
            foreach (GraphicListViewItem item in lst.Items)
            {
                if (item.Bounds.X <= screenX && screenX <= item.Bounds.X + item.Bounds.Width
                    && item.Bounds.Y - itemVerticalDistance <= screenY && screenY <= item.Bounds.Y + item.Bounds.Height + itemVerticalDistance)
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
                NODE_HEIGHT = OriginalHeight == 0 ? Height : OriginalHeight,
                NODE_Z_INDEX = ZIndex,
                NODE_OPTION = NodeOption,
                NODE_NOTE = base.NodeNote,
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
					NODE_ID                    = item.Row["NODE_ID"].ToString(),
					NODE_SEQ                   = item.Row["NODE_SEQ"].ToInt(),
					NODE_DETAIL_ID             = item.Row["NODE_DETAIL_ID"].ToString(),
					NODE_DETAIL_SEQ            = item.Row["NODE_DETAIL_SEQ"].ToInt(),
					NODE_DETAIL_ORDER          = i + 1,
					NODE_DETAIL_TYPE           = item.Row["NODE_DETAIL_TYPE"].ToString(),
					NODE_DETAIL_DATA_TYPE      = item.Row["NODE_DETAIL_DATA_TYPE"].ToString(),
					NODE_DETAIL_DATA_TYPE_FULL = item.Row["NODE_DETAIL_DATA_TYPE_FULL"].ToString(),
					NODE_DETAIL_COMMENT        = item.Row["NODE_DETAIL_COMMENT"].ToString(),
					NODE_DETAIL_TABLE_ALIAS    = item.Row["NODE_DETAIL_TABLE_ALIAS"].ToString(),
					NODE_DETAIL_NOTE           = item.Row["NODE_DETAIL_NOTE"].ToString(),
                    NODE_DETAIL_VIEW_COLUMN1   = item.Row["NODE_DETAIL_VIEW_COLUMN1"].ToString(),
					NODE_DETAIL_VIEW_COLUMN2   = item.Row["NODE_DETAIL_VIEW_COLUMN2"].ToString(),
                    NODE_ID_REF                = item.Row["NODE_ID_REF"].ToString(),
                    NODE_SEQ_REF               = item.Row["NODE_SEQ_REF"].ToInt()
                };
				rs.Add(model);
			}
            return rs;
        }
        #endregion

        #region IScaleControl
        public override Color MinimapColor { get; set; } = Color.FromArgb(77, 130, 184);
        #endregion
    }
}