using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;

namespace ST.DataModeler
{
    public partial class DataModeler
    {
        #region Values
        // Ref
        private GraphicControl MouseDownNode = null;
        private GraphicControl MouseMoveNode = null;
        private GraphicControl DragOverNode = null;
        private bool OldMouseMoveExecuted = true; // For DataModeler_MouseLeave

        private RelationControl MouseDownRelation = null;
        private RelationControl MouseMoveRelation = null;

        private bool IsMainMouseDown = false;
        #endregion

        #region ContextMenu
        new private ContextMenu ContextMenu = new ContextMenu();
        private Point ContextMenuLocation;
        #endregion

        #region Load
        private void LoadInput()
        {
            AllowDrop = true;
            
            PreviewKeyDown += Main_PreviewKeyDown;
            KeyDown += Main_KeyDown;
            KeyUp += Main_KeyUp;

            MouseDown += Main_MouseDown;
            MouseMove += Main_MouseMove;
            MouseUp += Main_MouseUp;
            MouseWheel += Main_MouseWheel;
            MouseLeave += Main_MouseLeave;

            // Need code
            Click += Main_Click;
            DoubleClick += Main_DoubleClick;

            DragOver += Main_DragOver;
            DragEnter += Main_DragEnter;
            DragDrop += Main_DragDrop;
            DragLeave += Main_DragLeave;
            QueryContinueDrag += Main_QueryContinueDrag;

            // Context
            ContextMenu = new ContextMenu();
            ContextMenu.MenuItems.Add(new MenuItem("Add New ColumnNode(&C)", (sender, e) =>
            {
                AddNewColumnNode(ContextMenuLocation);
                MimimapRefresh();
            }));
            ContextMenu.MenuItems.Add(new MenuItem("Add New MemoNode(&M)", (sender, e) =>
            {
                AddNewMemoNode(ContextMenuLocation);
                MimimapRefresh();
            }));
            ContextMenu.MenuItems.Add("-");
            ContextMenu.MenuItems.Add(new MenuItem("Save Image(&S)", (sender, e) =>
            {
                SaveImage();
            }));
        }
        #endregion

        #region Override(for focus & Enabled)
        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            if (Enabled && !ReadOnly)
            {
                base.OnPreviewKeyDown(e);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (Enabled && !ReadOnly)
            {
                base.OnKeyDown(e);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (Enabled && !ReadOnly)
            {
                base.OnKeyUp(e);
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (Enabled && !ReadOnly)
            {
                base.OnKeyPress(e);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
            if (Enabled && !ReadOnly)
            {
                base.OnMouseDown(e);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Enabled && !ReadOnly)
            {
                base.OnMouseMove(e);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (Enabled && !ReadOnly)
            {
                base.OnMouseUp(e);
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down) return true;
            if (keyData == Keys.Left || keyData == Keys.Right) return true;
            return base.IsInputKey(keyData);
        }
        #endregion

        #region Events
        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            bool executed = Node_KeyDown(sender, e);
            if (!executed)
            {
                executed = Relation_KeyDown(sender, e);
            }

            if (!executed)
            {
                // No code
                // DataModeler_KeyDown(sender, e);
            }
        }

        private void Main_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bool executed = Node_PreviewKeyDown(sender, e);
            if (!executed)
            {

            }
        }

        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            bool executed = Node_KeyUp(sender, e);
            if (!executed)
            {
                executed = Relation_KeyUp(sender, e);
            }

            if (!executed)
            {
                // No code
                // DataModeler_KeyDown(sender, e);
            }
        }

        private void Main_MouseDown(object sender, MouseEventArgs e)
        {
            bool executed = Node_MouseDown(sender, e);
            if (!executed)
            {
                executed = Relation_MouseDown(sender, e);
            }

            if (!executed)
            {
                DataModeler_MouseDown(sender, e);
                IsMainMouseDown = true;
            }
        }

        private void Main_MouseMove(object sender, MouseEventArgs e)
        {
            // Todo: MouseHover 추가?
            bool executed = false;
            executed = Node_MouseMove(sender, e);
            if (!executed)
            {
                executed = Relation_MouseMove(sender, e);
            }

            if (!executed || IsMainMouseDown)
            {
                DataModeler_MouseMove(sender, e);
            }

            if (executed && !OldMouseMoveExecuted && !IsMainMouseDown)
            {
                OnMouseLeave(e);
            }
            OldMouseMoveExecuted = executed;
        }

        private void Main_MouseUp(object sender, MouseEventArgs e)
        {
            bool executed = Node_MouseUp(sender, e);
            if (!executed)
            {

            }

            IsMainMouseDown = false;
        }

        private void Main_MouseWheel(object sender, MouseEventArgs e)
        {
            bool executed = Node_MouseWheel(sender, e);
            if (!executed)
            {
                DataModeler_MouseWheel(sender, e);
            }
        }

        private void Main_MouseHover(object sender, EventArgs e)
        {
            bool executed = Node_MouseHover(sender, e);
            if (!executed)
            {
            }
        }

        private void Main_MouseLeave(object sender, EventArgs e)
        {
            DataModeler_MouseLeave(sender, e);
            IsMainMouseDown = false;
        }

        private void Main_Click(object sender, EventArgs e)
        {
            // Todo: 구현 필요
        }

        private void Main_DoubleClick(object sender, EventArgs e)
        {
            // Todo: 구현 필요
        }

        private void Main_DragEnter(object sender, DragEventArgs e)
        {
        }

        private void Main_DragOver(object sender, DragEventArgs e)
        {
            bool executed = Node_DragOver(sender, e);
            if (!executed)
            {
            }
        }

        private void Main_DragDrop(object sender, DragEventArgs e)
        {
            bool executed = Node_DragDrop(sender, e);
            if (!executed)
            {
            }
        }

        private void Main_DragLeave(object sender, EventArgs e)
        {
        }

        private void Main_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            bool executed = Node_QueryContinueDrag(sender, e);
            if (!executed)
            {
            }
        }
        #endregion

        #region DataModeler Events
        private void DataModeler_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseDownPoint = e.Location;
                MouseDownInnerLocation = InnerLocation;
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (Enabled && !ReadOnly)
                {
                    foreach(MenuItem item in ContextMenu.MenuItems)
                    {
                        item.Enabled = true;
                    }
                }
                else
                {
                    foreach (MenuItem item in ContextMenu.MenuItems)
                    {
                        item.Enabled = false;
                    }
                }
                ContextMenu.Show(this, e.Location);
                ContextMenuLocation = e.Location;
            }
        }

        private void DataModeler_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Set InnerLocation
                int reverseInnerPointX = -MouseDownInnerLocation.X + e.X - MouseDownPoint.X;
                int reverseInnerPointY = -MouseDownInnerLocation.Y + e.Y - MouseDownPoint.Y;

                SetInnerLocation(new Point(-reverseInnerPointX, -reverseInnerPointY));
            }
            else if (e.Button == MouseButtons.None)
            {
                
            }
        }

        private void DataModeler_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) == Keys.Control)
            {
                float oldScaleValue = ScaleValue;
                float newScaleValue = Math.Min(Math.Max(ScaleValue + (e.Delta < 0 ? -ScaleValueUnit : ScaleValueUnit), MinimumScaleValue), MaximumScaleValue);

                if (oldScaleValue != newScaleValue)
                {
                    // Get newInnerLocationX
                    var oldPointX = InnerLocation.X + e.X;
                    var newPointX = (InnerLocation.X + e.X) * (newScaleValue / oldScaleValue);
                    var newInnerLocationX = InnerLocation.X - (int)Math.Round(oldPointX - newPointX);
                    newInnerLocationX = Convert.ToInt32(Math.Min(Math.Max(newInnerLocationX, 0), (InnerSize.Width - Width * (1 / newScaleValue)) * newScaleValue));

                    // Get newInnerLocationY
                    var oldPointY = InnerLocation.Y + e.Y;
                    var newPointY = (InnerLocation.Y + e.Y) * (newScaleValue / oldScaleValue);
                    var newInnerLocationY = InnerLocation.Y - (int)Math.Round(oldPointY - newPointY);
                    newInnerLocationY = Convert.ToInt32(Math.Min(Math.Max(newInnerLocationY, 0), (InnerSize.Height - Height * (1 / newScaleValue)) * newScaleValue));

                    SetScaleValueNInnerLocation(newScaleValue, new Point(newInnerLocationX, newInnerLocationY));
                }
            }
            else
            {
                SetInnerLocation(new Point(InnerLocation.X, Math.Min(Math.Max(InnerLocation.Y - e.Delta, 0), InnerSize.Height - Height)));
            }
        }

        private void DataModeler_MouseLeave(object sender, EventArgs e)
        {
            // Code to prevent redundant MouseLeave event execution
            OldMouseMoveExecuted = true;
        }
        #endregion

        #region Node Events
        private bool Node_KeyDown(object sender, KeyEventArgs e)
        {
            bool rsExecuted = true;

            GraphicControl node = GetFocusedNode();
            if (node != null)
            {
                node.OnKeyDown(e);
            }
            else
            {
                rsExecuted = false;
            }

            return rsExecuted;
        }

        private bool Node_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            bool rsExecuted = true;

            GraphicControl node = GetFocusedNode();
            if (node != null)
            {
                node.OnPreviewKeyDown(e);
            }
            else
            {
                rsExecuted = false;
            }

            return rsExecuted;
        }

        private bool Node_KeyUp(object sender, KeyEventArgs e)
        {
            bool rsExecuted = true;

            GraphicControl node = GetFocusedNode();
            if (node != null)
            {
                node.OnKeyUp(e);
            }
            else
            {
                rsExecuted = false;
            }

            return rsExecuted;
        }

        private bool Node_MouseDown(object sender, MouseEventArgs e)
        {
            bool rsExecuted = true;

            GraphicControl node = GetMouseOverNode(e.Location);
            if (node != null)
            {

                node.OnMouseDown(GetPointToClientEventArgs(node, e));
                MouseDownNode = node;
            }
            else
            {
                rsExecuted = false;
            }

            return rsExecuted;
        }

        private bool Node_MouseMove(object sender, MouseEventArgs e)
        {
            bool rsExecuted = true;

            // Todo: MouseDownNode 초기화 안 됨. 확인 필요
            if (MouseDownNode != null && e.Button != MouseButtons.None)
            {
                MouseDownNode.OnMouseMove(GetPointToClientEventArgs(MouseDownNode, e));
                MouseMoveNode = MouseDownNode;
            }
            else
            {
                // MouseDownNode 초기화 이슈 / 임시?
                if (MouseDownNode != null)
                {
                    MouseDownNode = null;
                }

                GraphicControl node = GetMouseOverNode(e.Location);
                if (node != null)
                {
                    if (MouseMoveNode != null && !MouseMoveNode.Equals(node))
                    {
                        MouseMoveNode.OnMouseLeave();
                    }
                    node.OnMouseMove(GetPointToClientEventArgs(node, e));
                    MouseMoveNode = node;
                }
                else
                {
                    if (MouseMoveNode != null)
                    {
                        MouseMoveNode.OnMouseLeave();
                    }
                    MouseMoveNode = null;

                    rsExecuted = false;
                }
            }

            return rsExecuted;
        }

        private bool Node_MouseUp(object sender, MouseEventArgs e)
        {
            bool rsExecuted = true;

            if (MouseDownNode != null)
            {
                MouseDownNode.OnMouseUp(GetPointToClientEventArgs(MouseDownNode, e));
                MouseDownNode = null;
            }
            else
            {
                rsExecuted = false;
            }

            return rsExecuted;
        }

        private bool Node_MouseWheel(object sender, MouseEventArgs e)
        {
            bool rsExecuted = true;

            GraphicControl node = GetMouseOverNode(e.Location);
            if (node != null)
            {
                node.OnMouseWheel(GetPointToClientEventArgs(node, e));
            }
            else
            {
                rsExecuted = false;
            }

            return rsExecuted;
        }

        private bool Node_MouseHover(object sender, EventArgs e)
        {
            bool rsExecuted = true;

            GraphicControl node = GetMouseOverNode(PointToClient(Cursor.Position));
            if (node != null)
            {
                node.OnMouseHover();
            }
            else
            {
                rsExecuted = false;
            }

            return rsExecuted;
        }

        private bool Node_DragOver(object sender, DragEventArgs e)
        {
            bool rsExecuted = true;

            GraphicControl node = GetMouseOverNodeByScreenPoint(e.X, e.Y);
            if (node != null && node.AllowDrop)
            {
                if (DragOverNode == null)
                {
                    node.OnDragEnter(e);
                }
                else if (DragOverNode != null && !node.Equals(DragOverNode))
                {
                    DragOverNode.OnDragLeave(new EventArgs());
                    node.OnDragEnter(e);
                }

                node.OnDragOver(e);
                DragOverNode = node;
            }
            else
            {
                if (DragOverNode != null)
                {
                    DragOverNode.OnDragLeave(new EventArgs());
                    e.Effect = DragDropEffects.None;
                }

                DragOverNode = null;
                rsExecuted = false;
            }

            return rsExecuted;
        }

        private bool Node_DragDrop(object sender, DragEventArgs e)
        {
            bool rsExecuted = true;

            GraphicControl node = GetMouseOverNodeByScreenPoint(e.X, e.Y);
            if (node != null && node.AllowDrop)
            {
                node.OnDragDrop(GetPointToClientEventArgs(node, e));
                MouseDownNode = node;
            }
            else
            {
                DragOverNode = null;
                rsExecuted = false;
            }

            return rsExecuted;
        }

        private bool Node_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            bool rsExecuted = true;

            GraphicControl node = MouseDownNode;
            if (node != null)
            {
                node.OnQueryContinueDrag(e);
                MouseDownNode = node;
            }
            else
            {
                rsExecuted = false;
            }

            return rsExecuted;
        }
        #endregion

        #region Relation Events
        private bool Relation_KeyDown(object sender, KeyEventArgs e)
        {
            bool rsExecuted = true;

            RelationControl relation = GetFocusedRelation();
            if (relation != null)
            {
                relation.OnKeyDown(e);
            }
            else
            {
                rsExecuted = false;
            }

            return rsExecuted;
        }

        private bool Relation_KeyUp(object sender, KeyEventArgs e)
        {
            bool rsExecuted = true;

            RelationControl relation = GetFocusedRelation();
            if (relation != null)
            {
                relation.OnKeyUp(e);
            }
            else
            {
                rsExecuted = false;
            }

            return rsExecuted;
        }

        private bool Relation_MouseDown(object sender, MouseEventArgs e)
        {
            bool rsExecuted = true;

            RelationControl relation = GetMouseOverRelation(e.Location);
            if (relation != null)
            {
                relation.OnMouseDown(e);
                MouseDownRelation = relation;
            }
            else
            {
                rsExecuted = false;
            }

            return rsExecuted;
        }

        private bool Relation_MouseMove(object sender, MouseEventArgs e)
        {
            bool rsExecuted = true;

            // Todo: MouseDownRelation 초기화 안 됨. 확인 필요
            if (MouseDownRelation != null && e.Button != MouseButtons.None)
            {
                MouseDownRelation.OnMouseMove(e);
                MouseMoveRelation = MouseDownRelation;
            }
            else
            {
                // MouseDownRelation 초기화 이슈 / 임시?
                if (MouseDownRelation != null)
                {
                    MouseDownRelation = null;
                }

                RelationControl Relation = GetMouseOverRelation(e.Location);
                if (Relation != null)
                {
                    if (MouseMoveRelation != null && !MouseMoveRelation.Equals(Relation))
                    {
                        MouseMoveRelation.OnMouseLeave();
                    }
                    Relation.OnMouseMove(e);
                    MouseMoveRelation = Relation;
                }
                else
                {
                    if (MouseMoveRelation != null)
                    {
                        MouseMoveRelation.OnMouseLeave();
                    }
                    MouseMoveRelation = null;

                    rsExecuted = false;
                }
            }

            return rsExecuted;
        }

        private bool Relation_MouseUp(object sender, MouseEventArgs e)
        {
            bool rsExecuted = true;

            if (MouseDownRelation != null)
            {
                MouseDownRelation.OnMouseUp(e);
                MouseDownRelation = null;
            }
            else
            {
                rsExecuted = false;
            }

            return rsExecuted;
        }
        #endregion

        #region Functions
        private GraphicControl GetFocusedNode()
        {
            GraphicControl rs = null;
            for (int i = 0; i < Controls.Count; i++)
            {
                // 상위 컨트롤이 포커스가 없을 때 처리
                GraphicControl node = Controls[i];
                while (node.Controls.Count > 0)
                {
                    bool found = false;
                    foreach (GraphicControl childControl in node.Controls)
                    {
                        if (childControl.Visible && childControl.Focused)
                        {
                            found = true;
                            node = childControl;
                            rs = childControl;
                            break;
                        }
                    }
                    if (!found)
                    {
                        break;
                    }
                }

                if (rs != null)
                {
                    break;
                }
                else if (rs == null && Controls[i].Visible && Controls[i].Focused)
                {
                    rs = Controls[i];
                    break;
                }
            }
            return rs;
        }

        private RelationControl GetFocusedRelation()
        {
            RelationControl rs = null;
            for (int i = 0; i < Relations.Count; i++)
            {
                if (Relations[i].Focused)
                {
                    rs = Relations[i];
                    break;
                }
            }
            return rs;
        }

        private GraphicControl GetMouseOverNodeByScreenPoint(int x, int y)
        {
            Point clientPoint = PointToClient(new Point(x, y));
            return GetMouseOverNode(clientPoint);
        }

        private GraphicControl GetMouseOverNode(Point mouseLocation)
        {
            GraphicControl rs = null;
            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i].Visible && Controls[i].Bounds.Contains(mouseLocation))
                {
                    rs = Controls[i];
                    while(rs.Controls.Count > 0)
                    {
                        bool found = false;
                        foreach(GraphicControl childControl in rs.Controls)
                        {
                            Rectangle childControlBounds = new Rectangle(childControl.GetLocationFromTarget(), childControl.Size);
                            if (childControl.Visible && childControlBounds.Contains(mouseLocation))
                            {
                                found = true;
                                rs = childControl;
                                break;
                            }
                        }
                        if (!found)
                        {
                            break;
                        }
                    }

                    break;
                }
            }
            return rs;
        }

        private RelationControl GetMouseOverRelation(Point mouseLocation)
        {
            RelationControl rs = null;
            for (int i = 0; i < Relations.Count; i++)
            {
                foreach(Rectangle area in Relations[i].Area)
                {
                    if (area.Contains(mouseLocation))
                    {
                        rs = Relations[i];
                    }
                }
            }
            return rs;
        }

        private MouseEventArgs GetPointToClientEventArgs(GraphicControl target, MouseEventArgs e)
        {
            Point targetLocation = target.GetLocationFromTarget();
            MouseEventArgs rs = new MouseEventArgs(e.Button, e.Clicks, e.X - targetLocation.X, e.Y - targetLocation.Y, e.Delta);
            return rs;
        }

        private DragEventArgs GetPointToClientEventArgs(GraphicControl target, DragEventArgs e)
        {
            Point targetLocation = target.GetLocationFromTarget();
            DragEventArgs rs = new DragEventArgs(e.Data, e.KeyState, e.X - targetLocation.X, e.Y - targetLocation.Y, e.AllowedEffect, e.Effect);
            return rs;
        }

        public void SetInnerLocation(Point newInnerLocation, bool callOnPaint = true)
        {
            AllowDrawRequest = false;
            if (!CanMoveInnerLocationToDisableArea)
            {
                newInnerLocation.X = Math.Min(Math.Max(newInnerLocation.X, 0), (InnerSize.Width * ScaleValue - Width).ToInt());
                newInnerLocation.Y = Math.Min(Math.Max(newInnerLocation.Y, 0), (InnerSize.Height * ScaleValue - Height).ToInt());
            }

            int toBeMovedX = InnerLocation.X - newInnerLocation.X;
            int toBeMovedY = InnerLocation.Y - newInnerLocation.Y;

            foreach (NodeBase node in Controls)
            {
                node.Location = new Point(node.Location.X + toBeMovedX, node.Location.Y + toBeMovedY);
            }
            
            InnerLocation = newInnerLocation;
            if (callOnPaint && Enabled && !ReadOnly)
            {
                DrawStatusPanel();
                Minimap.Draw();
            }
            SetNodeRelationDrawInfoDic();

            if (callOnPaint && Enabled && !ReadOnly)
            {
                OnPaint(new PaintEventArgs(CreateGraphics(), Bounds));
            }

            AllowDrawRequest = true;
        }

        private void SetScaleValueNInnerLocation(float scaleValue, Point innerLocation)
        {
            AllowDrawRequest = false;

            int toBeMovedX = InnerLocation.X - innerLocation.X;
            int toBeMovedY = InnerLocation.Y - innerLocation.Y;

            _ScaleValue = Math.Min(Math.Max(scaleValue, MinimumScaleValue), MaximumScaleValue);

            foreach (GraphicControl control in Controls)
            {
                IScaleControl scaleControl = control as IScaleControl;
                if (scaleControl != null)
                {
                    scaleControl.SetScaleValueNMovePoint(_ScaleValue, new Point(toBeMovedX, toBeMovedY));
                }
            }

            InnerLocation = innerLocation;
            DrawStatusPanel();
            Minimap.Draw();
            SetNodeRelationDrawInfoDic();

            OnPaint(new PaintEventArgs(CreateGraphics(), Bounds));

            AllowDrawRequest = true;
        }
        #endregion

        #region Context Function
        private void AddNewColumnNode(Point location)
        {
            ColumnNode columnNode = new ColumnNode(this);
            columnNode.ID = GetNewNodeID("C");
            columnNode.SEQ = 0;
            columnNode.NodeType = NodeType.ColumnNode;
            columnNode.ScaleValue = ScaleValue;
            columnNode.Location = location;
            Controls.Add(columnNode);

            columnNode.BringToFront();
        }

        private void AddNewMemoNode(Point location)
        {
            MemoNode memoNode = new MemoNode(this);
            memoNode.Size = new Size(200, 200);
            memoNode.ID = GetNewNodeID("M");
            memoNode.SEQ = 0;
            memoNode.NodeType = NodeType.MemoNode;
            memoNode.ScaleValue = ScaleValue;
            memoNode.Location = location;
            Controls.Add(memoNode);

            memoNode.BringToFront();
        }

        private string GetNewNodeID(string prefix)
        {
            string rsID;
            int i = 1;
            while (true)
            {
                bool isDuplicated = false;
                rsID = prefix + i.ToString();
                foreach (GraphicControl control in Controls)
                {
                    var node = control as NodeBase;
                    if (node != null && node.ID == rsID)
                    {
                        isDuplicated = true;
                        break;
                    }
                }
                if (!isDuplicated)
                {
                    break;
                }
                i++;
            }
            return rsID;
        }

        private void SaveImage()
        {
            var tempScaleValue = ScaleValue;
            var tempInnerLocation = InnerLocation;
            AllowDrawRequest = false;
            ScaleValue = 1f;
            // This method changes the AllowDrawRequest value of a variable to true.
            SetInnerLocation(new Point(0, 0), false);

            Bitmap image = GetSaveImage();
            Rectangle area = GetSaveImageObjectsArea();
            ModalImageSave modal = new ModalImageSave(image, area, BackColor);

            AllowDrawRequest = false;
            ScaleValue = tempScaleValue;
            // This method changes the AllowDrawRequest value of a variable to true.
            SetInnerLocation(tempInnerLocation);

            modal.ShowDialog(this);
        }

        private Bitmap GetSaveImage()
        {
            var bitmap = new Bitmap(InnerSize.Width, InnerSize.Height);
            var gBitmap = Graphics.FromImage(bitmap);
            gBitmap.Clear(BackColor);

            // Draw Relations
            DrawRelations(gBitmap);

            // Draw Controls
            DrawControls(gBitmap, true);

            return bitmap;
        }

        private Rectangle GetSaveImageObjectsArea()
        {
            Rectangle rsArea = new Rectangle(-1, -1, 0, 0);

            int right = 0;
            int bottom = 0;
            foreach (GraphicControl control in Controls)
            {
                if (rsArea.X < 0 || control.Left < rsArea.X)
                {
                    rsArea.X = control.Left;
                }

                if (rsArea.Y < 0 || control.Top < rsArea.Y)
                {
                    rsArea.Y = control.Top;
                }

                if (right < control.Right)
                {
                    right = control.Right;
                }

                if (bottom < control.Bottom)
                {
                    bottom = control.Bottom;
                }
            }

            foreach(RelationControl relation in Relations)
            {
                foreach(Point point in relation.Points)
                {
                    if (rsArea.X < 0 || point.X < rsArea.X)
                    {
                        rsArea.X = point.X;
                    }

                    if (rsArea.Y < 0 || point.Y < rsArea.Y)
                    {
                        rsArea.Y = point.Y;
                    }

                    if (right < point.X)
                    {
                        right = point.X;
                    }

                    if (bottom < point.Y)
                    {
                        bottom = point.Y;
                    }
                }
            }

            rsArea.Width = Math.Max(right - rsArea.Left, 0);
            rsArea.Height = Math.Max(bottom - rsArea.Top, 0);

            return rsArea;
        }
        #endregion

        #region Override
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool rsExecuted;

            GraphicControl node = GetFocusedNode();
            if (node != null)
            {
                rsExecuted = node.ProcessCmdKey(ref msg, keyData);
            }
            else
            {
                rsExecuted = base.ProcessCmdKey(ref msg, keyData);
            }

            return rsExecuted;
        }

        protected override void WndProc(ref Message m)
        {
            GraphicControl node = GetFocusedNode();
            if (node != null)
            {
                if (node.WndProc(m))
                {
                    base.WndProc(ref m);
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        public void OnDefWndProc(ref Message m)
        {
            DefWndProc(ref m);
        }
        #endregion
    }
}