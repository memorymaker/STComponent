using ST.Controls;
using ST.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace ST.DataModeler
{
    public partial class DataModeler : Control
    {
        #region Values
        // Option
        public Size InnerSize = new Size(4000, 3000);
        public Point InnerLocation = Point.Empty;
        private Point MouseDownInnerLocation= Point.Empty;
        public bool CanMoveInnerLocationToDisableArea = false;

        // Ref
        private Point MouseDownPoint = Point.Empty;

        // Controls
        // ---- StatusPanel
        private Panel StatusPanel = new Panel();
        // ---- ---- Options
        private int StatusPanelHeight = 20;
        private Point StatusPanelStringPosition = new Point(2, 2);
        private Font StatusPanelFont = new Font("맑은 고딕", 9F);
        // ---- ---- Ref

        // ---- Minimap
        private MinimapControl Minimap = null;
        // ---- ----
        private Size MinimapSize = new Size(200, 150);

        // System Optoins
        private float MaximumScaleValue = 2f;
        private float MinimumScaleValue = 0.2f;
        private float ScaleValueUnit = 0.1f;
        #endregion

        #region Propertise
        public string ID { get; set; }

        /// <summary>
        /// DataModeler의 자식 컨트롤(Node, Relation) 배율을 지정한 배율 인수로 조정합니다.
        /// </summary>
        public float ScaleValue
        {
            get
            {
                return _ScaleValue;
            }
            set
            {
                float oldScaleValue = ScaleValue;
                float newScaleValue = Math.Min(Math.Max(value, MinimumScaleValue), MaximumScaleValue);

                if (oldScaleValue != newScaleValue)
                {
                    int centerX = Width / 2;
                    int centerY = Height / 2;

                    // Get newInnerLocationX
                    var oldPointX = InnerLocation.X + centerX;
                    var newPointX = (InnerLocation.X + centerX) * (newScaleValue / oldScaleValue);
                    var newInnerLocationX = InnerLocation.X - (int)Math.Round(oldPointX - newPointX);
                    newInnerLocationX = Convert.ToInt32(Math.Min(Math.Max(newInnerLocationX, 0), (InnerSize.Width - Width * (1 / newScaleValue)) * newScaleValue));

                    // Get newInnerLocationY
                    var oldPointY = InnerLocation.Y + centerY;
                    var newPointY = (InnerLocation.Y + centerY) * (newScaleValue / oldScaleValue);
                    var newInnerLocationY = InnerLocation.Y - (int)Math.Round(oldPointY - newPointY);
                    newInnerLocationY = Convert.ToInt32(Math.Min(Math.Max(newInnerLocationY, 0), (InnerSize.Height - Height * (1 / newScaleValue)) * newScaleValue));

                    SetScaleValueNInnerLocation(newScaleValue, new Point(newInnerLocationX, newInnerLocationY));
                }
            }
        }
        private float _ScaleValue = 1f;

        public ControlCollection BaseControls
        {
            get
            {
                return base.Controls;
            }
        }

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
                _Enabled = value;
                AllowDrop = _Enabled;
                OnPaint(new PaintEventArgs(CreateGraphics(), new Rectangle(0, 0, Width, Height)));
                Minimap.Draw();
                DrawStatusPanel(StatusPanel.CreateGraphics());
            }
        }
        private bool _Enabled = true;

        public bool ReadOnly { get; set; } = false;

        public  bool ShowPerformanceTestLabel
        {
            get
            {
                return PerformanceTestLabel.Visible;
            }
            set
            {
                PerformanceTestLabel.Visible = value;
            }
        }

        public Brush StatusPanelBrush { get; set; } = new SolidBrush(Color.FromArgb(60, 60, 60));

        public Color StatusPanelBackColor { get; set; } = Color.FromArgb(250, 250, 250);

        public Color StatusPanelBorderColor { get; set; } = Color.FromArgb(160, 160, 160);
        #endregion

        #region Load
        public DataModeler()
        {
            LoadIGraphicControlParent();
            LoadIRelationControlParent();
            LoadThis();
            LoadInput();
            LoadDraw();
        }

        private void LoadThis()
        {
            TabStop = true;
            BackColor = SystemDefaultBackColor;

            SetControls();
            SetEvents();
        }
        #endregion

        #region Controls
        private void SetControls()
        {
            SetMinimap();
            SetStatusPanel();
        }

        private void SetMinimap()
        {
            Minimap = new MinimapControl(this);
            Minimap.Size = MinimapSize;

            base.Controls.Add(Minimap);
            Minimap.BringToFront();
        }

        private void SetStatusPanel()
        {
            base.Controls.Add(StatusPanel);
            StatusPanel.Height = StatusPanelHeight;
            StatusPanel.Visible = true;
            StatusPanel.BackColor = StatusPanelBackColor;
            StatusPanel.Paint += (object sender, PaintEventArgs e) =>
            {
                DrawStatusPanel(e.Graphics);
            };
            SizeChanged += (object sender, EventArgs e) =>
            {
                StatusPanel.Width = Minimap.Width;
                StatusPanel.Location = new Point(Minimap.Left, Minimap.Top - StatusPanel.Height);
            };
        }
        #endregion

        #region This Events
        private void SetEvents()
        {
            ControlAdded += DataModeler_ControlAdded;
            ControlRemoved += DataModeler_ControlRemoved;
        }

        private void DataModeler_ControlAdded(object sender, GraphicControlEventArgs e)
        {
            e.Control.LocationChanged += Control_LocationChanged;
            e.Control.SizeChanged += Control_SizeChanged;
        }

        private void DataModeler_ControlRemoved(object sender, GraphicControlEventArgs e)
        {
            e.Control.LocationChanged -= Control_LocationChanged;
            e.Control.SizeChanged -= Control_SizeChanged;
            DataModeler_ControlRemoved_RemoveRelations(e.Control);
        }

        private void DataModeler_ControlRemoved_RemoveRelations(GraphicControl control)
        {
            TableNode tableNode = control as TableNode;
            if (tableNode != null)
            {
                for(int i = Relations.Count - 1; 0 <= i; i--)
                {
                    if ((Relations[i].Model.NODE_ID1 == tableNode.ID
                        &&
                        Relations[i].Model.NODE_SEQ1 == tableNode.SEQ)
                    ||
                        (Relations[i].Model.NODE_ID2 == tableNode.ID
                        &&
                        Relations[i].Model.NODE_SEQ2 == tableNode.SEQ))
                    {
                        Relations.Remove(Relations[i]);
                    }
                }
            }
        }

        private void Control_SizeChanged(object sender, EventArgs e)
        {
            if (AllowDrawRequest)
            {
                Minimap.Draw();
            }
        }

        private void Control_LocationChanged(object sender, EventArgs e)
        {
            if (AllowDrawRequest)
            {
                Minimap.Draw();
            }
        }
        #endregion

        #region Function
        private int GetFirstZIndex()
        {
            int notScaleControlCount = 0;
            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i] as IScaleControl == null)
                {
                    notScaleControlCount++;
                }
            }
            return notScaleControlCount;
        }

        private void DrawStatusPanel(Graphics graphics = null)
        {
            if (graphics == null)
            {
                graphics = StatusPanel.CreateGraphics();
            }

            var bitmap = new Bitmap(StatusPanel.Width, StatusPanel.Height);
            Graphics bitmapGraphics = Graphics.FromImage(bitmap);
            bitmapGraphics.FillRectangle(new SolidBrush(StatusPanel.BackColor), new Rectangle(0, 0, StatusPanel.Width, StatusPanel.Height));

            string bottomString = string.Format(
                "Scale: {0}%  Position: {1}, {2}"
                , Math.Round(ScaleValue * 100)
                , Math.Floor(InnerLocation.X * (1 / ScaleValue)), Math.Floor(InnerLocation.Y * (1 / ScaleValue))
            );
            bitmapGraphics.DrawString(bottomString, StatusPanelFont, StatusPanelBrush, StatusPanelStringPosition);

            // Draw Border(Left, Top)
            Pen borderPen = new Pen(StatusPanelBorderColor);
            bitmapGraphics.DrawLine(borderPen, 0, 0, Width, 0);
            bitmapGraphics.DrawLine(borderPen, 0, 0, 0, Height);

            // Enabled
            if (!Enabled)
            {
                bitmapGraphics.FillRectangle(new SolidBrush(DisableColor), new Rectangle(0, 0, Width, Height));
            }

            graphics.DrawImage(bitmap, 0, 0);
            bitmapGraphics.Dispose();
        }

        public List<NodeModel> GetNodeModels()
        {
            List<NodeModel> rs = new List<NodeModel>();
            foreach (GraphicControl control in Controls)
            {
                NodeBase node = control as NodeBase;
                if (node != null)
                {
                    rs.Add(node.GetNodeModel());
                }
            }
            return rs;
        }

        /// <summary>
        /// DataModeler 내부 Nodes 데이터를 DataTable 형태로 반환합니다.
        /// </summary>
        /// <returns> <see langword="DataModelerNodeDataTables"/> 클래스 내부에 Node(DataTable), NodeDetail(DataTable) 필드로 각각의 데이터를 반환합니다.
        /// </returns>
        public DataModelerNodeDataTables GetNodeDataTables()
        {
            var nodeModels = GetNodeModels();

            // Get node
            DataTable node = nodeModels.ToDataTable();
            node.Columns.Remove("NODE_DETAIL");

            // Get nodeDetilas
            List<NodeDetailModel> detils = new List<NodeDetailModel>();
            for(int i = 0; i < nodeModels.Count; i++)
            {
                if (nodeModels[i].NODE_DETAIL != null)
                {
                    detils = detils.Union(nodeModels[i].NODE_DETAIL).ToList();
                }
            }
            DataTable nodeDetail = detils.ToDataTable();

            var rs = new DataModelerNodeDataTables(node, nodeDetail);
            return rs;
        }

        public List<RelationModel> GetRelationModels()
        {
            List<RelationModel> rs = new List<RelationModel>();
            foreach(RelationControl control in Relations)
            {
                rs.Add(control.GetRelationModel());
            }
            return rs;
        }

        /// <summary>
        /// DataModeler 내부 Relations 데이터를 DataTable 형태로 반환합니다.
        /// </summary>
        /// <returns><see langword="DataTable"></see></returns>
        public DataTable GetRelationDataTable()
        {
            var relationModels = GetRelationModels();
            DataTable rs = relationModels.ToDataTable();
            rs.AddColumns("{S}NODE_DETAIL_TABLE_ALIAS1 {S}NODE_DETAIL_TABLE_ALIAS2");
            
            List<object[]> refData = new List<object[]>();
            string[] numberTexts = new string[] { "1", "2" };

            // Get refData
            foreach (string numberText in numberTexts)
            {
                foreach(DataRow dr in rs.Rows)
                {
                    object[] newNode = new object[]
                    {
                          dr[$"NODE_ID{numberText}"].ToString()
                        , dr[$"NODE_SEQ{numberText}"].ToInt()
                        , null
                    };

                    bool duplicated = false;
                    foreach (object[] refDataNode in refData)
                    {
                        if (refDataNode[0].ToString() == newNode[0].ToString() && refDataNode[1].ToInt() == newNode[1].ToInt())
                        {
                            duplicated = true;
                            break;
                        }
                    }

                    if (!duplicated)
                    {
                        refData.Add(newNode);
                    }
                }
            }

            // Set refData Alias
            Extensions.SetAlias(ref refData, 0, 1, 2);

            // Set rs NODE_DETAIL_TABLE_ALIAS1, NODE_DETAIL_TABLE_ALIAS2
            foreach (string numberText in numberTexts)
            {
                foreach(DataRow dr in rs.Rows)
                {
                    string idField = $"NODE_ID{numberText}";
                    string seqField = $"NODE_SEQ{numberText}";

                    foreach (object[] refDataNode in refData)
                    {
                        if (refDataNode[0].ToString() == dr[idField].ToString() && refDataNode[1].ToInt() == dr[seqField].ToInt())
                        {
                            dr[DataModeler.NODE.NODE_DETAIL_TABLE_ALIAS + numberText] = refDataNode[2].ToString();
                            break;
                        }
                    }
                }
            }

            return rs;
        }

        /// <summary>
        /// DataModeler의 모든 Node와 Relation을 삭제합니다.
        /// </summary>
        public void Clear()
        {
            Controls.Clear();
            Relations.Clear();
            SetInnerLocation(new Point(0, 0));
            if (Enabled && !ReadOnly)
            {
                Refresh();
            }
        }

        public bool IsEmptyLocation(Point point)
        {
            bool rs = true;

            // Check Nodes
            foreach (GraphicControl control in Controls)
            {
                if (control.Bounds.Contains(point))
                {
                    rs = false;
                    break;
                }
            }

            // Check Controls
            if (rs)
            {
                foreach(Control control in base.Controls)
                {
                    if (control.Bounds.Contains(point))
                    {
                        rs = false;
                        break;
                    }
                }
            }

            return rs;
        }

        public int GetNewSeq(string nodeID)
        {
            int rsSeq = 0;
            foreach(GraphicControl control in Controls)
            {
                NodeBase node = control as NodeBase;
                if (node.ID == nodeID)
                {
                    if (rsSeq <= node.SEQ)
                    {
                        rsSeq = node.SEQ + 1;
                    }
                }
            }
            return rsSeq;
        }

        public void MimimapRefresh()
        {
            Minimap.Draw();
        }

        /// <summary>
        /// DataModeler 인스턴스의 Controls에 요소가 있는지 여부를 확인합니다.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool ContainsNode(NodeBase node)
        {
            bool rs = false;
            foreach (GraphicControl control in Controls)
            {
                NodeBase contolsNode = control as NodeBase;
                if (node != null && contolsNode.ID == node.ID && contolsNode.SEQ == node.SEQ)
                {
                    rs = true;
                    break;
                }
            }
            return rs;
        }

        public bool CanBeAddedRelation(RelationControl relation)
        {
            bool[] rs = new bool[2] { false, false };

            foreach (GraphicControl control in Controls)
            {
                TableNode node = control as TableNode;
                if (node != null)
                {
                    if (node.ID == relation.Model.NODE_ID1 && node.SEQ == relation.Model.NODE_DETAIL_SEQ1)
                    {
                        foreach (GraphicListViewItem item in node.Items)
                        {
                            if (item.Row[DataModeler.NODE.NODE_DETAIL_ID].ToString() == relation.Model.NODE_DETAIL_ID1
                            && item.Row[DataModeler.NODE.NODE_DETAIL_SEQ].ToInt() == relation.Model.NODE_DETAIL_SEQ1)
                            {
                                rs[0] = true;
                                break;
                            }
                        }
                    }
                    else if (node.ID == relation.Model.NODE_ID2 && node.SEQ == relation.Model.NODE_DETAIL_SEQ2)
                    {
                        if (string.IsNullOrWhiteSpace(relation.Model.NODE_DETAIL_ID2))
                        {
                            rs[1] = true;
                        }
                        else
                        {
                            foreach (GraphicListViewItem item in node.Items)
                            {
                                if (item.Row[DataModeler.NODE.NODE_DETAIL_ID].ToString() == relation.Model.NODE_DETAIL_ID2
                                && item.Row[DataModeler.NODE.NODE_DETAIL_SEQ].ToInt() == relation.Model.NODE_DETAIL_SEQ2)
                                {
                                    rs[1] = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (rs[0] && rs[1])
                    {
                        break;
                    }
                }
            }

            return rs[0] && rs[1];
        }
        #endregion

        #region Common Function(To be moved later)
        private void SuspendLayoutChildren()
        {
            foreach(Control control in Controls)
            {
                control.SuspendLayout();
            }
        }

        private void ResumeLayoutChildren(bool performLayout)
        {
            foreach (Control control in Controls)
            {
                control.ResumeLayout(performLayout);
            }
        }

        private void SetDoubleBuffering(System.Windows.Forms.Control control, bool value)
        {
            System.Reflection.PropertyInfo controlProperty = typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            controlProperty.SetValue(control, value, null);
        }
        #endregion
    }
}