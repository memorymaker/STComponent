using ST.Controls;
using ST.Core;
using ST.Core.Extension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public partial class ColumnNode
    {
        #region ContextMenu
        private ContextMenu ListViewContextMenu = new ContextMenu();
        private ContextMenu TitleContextMenu = new ContextMenu();
        private Point ContextMenuLocation;
        #endregion

        #region Option
        private int AutoMinWidth = 200;
        private int AutoMaxWidth = 500;
        private int AutoMinColumnWidth = 50;
        private int[] ColumnWidthRevision = new int[] { 10, 15 };
        #endregion

        private void LoadInput()
        {
            MouseUp += ColumnNode_MouseUp;
            ScaleValueChanged += ColumnNode_ScaleValueChanged;

            InnerListView.KeyDown += InnerListView_KeyDown;
            InnerListView.MouseDown += InnerListView_MouseDown;
            InnerListView.MouseUp += InnerListView_MouseUp;
            InnerListView.Resize += InnerListView_Resize;
            InnerListView.DragOver += InnerListView_DragOver;
            InnerListView.DragDrop += InnerListView_DragDrop;
            InnerListView.DragLeave += InnerListView_DragLeave;
            InnerListView.ItemDrag += InnerListView_ItemDrag;
            InnerListView.QueryContinueDrag += InnerListView_QueryContinueDrag;

            BtDeleteMouseDown += ColumnNode_BtDeleteMouseDown;
            BtDelete.SetDrawColor(SimpleGraphicControl.StateType.Default, Color.FromArgb(227, 236, 244));

            BtContextMenuClick += ColumnNode_BtContextMenuClick;
            BtContextMenu.SetDrawColor(SimpleGraphicControl.StateType.Default, Color.FromArgb(227, 236, 244));
            ShowBtContextMenu = true;

            TitleChanging += ColumnNode_TitleChanging;

            // List Context
            ListViewContextMenu = new ContextMenu();
            ListViewContextMenu.MenuItems.Add(new MenuItem("Add User Column(&A)", (sender, e) =>
            {
                AddUserColumn(ContextMenuLocation);
            }));
            ListViewContextMenu.MenuItems.Add(new MenuItem("Edit User Column(&E)", (sender, e) =>
            {
                EditUserColumn(ContextMenuLocation);
            }));
            ListViewContextMenu.MenuItems.Add(new MenuItem("Delete Column(&D)", (sender, e) =>
            {
                DeleteColumn(ContextMenuLocation);
            }));
            ListViewContextMenu.MenuItems.Add("-");
            ListViewContextMenu.MenuItems.Add(new MenuItem("Change View Option(&C)", new MenuItem[] {
                  new MenuItem("[Table Alias].[Column Info] [Comment] (&1)", (sender, e) => { NodeOption = "CTC"; })
                , new MenuItem("[Table Alias].[Column Info] (&2)"          , (sender, e) => { NodeOption = "CT_"; })
                , new MenuItem("[Column Info] [Comment] (&3)"              , (sender, e) => { NodeOption = "C_C"; })
                , new MenuItem("[Column Info] (&4)"                        , (sender, e) => { NodeOption = "C__"; })
            }));

            // Node Context
            TitleContextMenu = new ContextMenu();
            TitleContextMenu.MenuItems.Add(new MenuItem("Change View Option(&C)", new MenuItem[] {
                  new MenuItem("[Table Alias].[Column Info] [Comment] (&1)", (sender, e) => { NodeOption = "CTC"; })
                , new MenuItem("[Table Alias].[Column Info] (&2)"          , (sender, e) => { NodeOption = "CT_"; })
                , new MenuItem("[Column Info] [Comment] (&3)"              , (sender, e) => { NodeOption = "C_C"; })
                , new MenuItem("[Column Info] (&4)"                        , (sender, e) => { NodeOption = "C__"; })
            }));
            TitleContextMenu.MenuItems.Add("-");
            TitleContextMenu.MenuItems.Add(new MenuItem("Rename ID(&R)", (sender, e) => { ShowTitleEditor(); }));
            TitleContextMenu.MenuItems.Add(new MenuItem("Delete This Node(&D)", (sender, e) => { DeleteThisNode(); }));
        }

        private void ColumnNode_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.Y <= TitleHeight)
            {
                ContextMenuLocation = Cursor.Position;
                Point point = this.GetLocationFromTarget();
                point.Offset(e.Location);

                TitleContextMenu.Show(Parent as Control, point);
            }
        }

        private void ColumnNode_ScaleValueChanged(object sender, IScaleControlScaleValueChangedEventArgs e)
        {
            SetAutoSize();
        }

        private void InnerListView_KeyDown(object sender, KeyEventArgs e)
        {
            GraphicListView _this = (GraphicListView)sender;
            // Ctrl + C
            if (e.Control == true && e.KeyCode == Keys.C)
            {
                if (_this.SelectedItems?.Count > 0)
                {
                    List<string> items = new List<string>();
                    foreach (GraphicListViewItem item in _this.SelectedItems)
                    {
                        items.Add(item.Row["NODE_DETAIL_ID"].ToString());
                    }
                    Clipboard.SetText(String.Join("\r\n", items));
                }
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (_this.SelectedItems?.Count > 0)
                {
                    if (ModalMessageBox.Show("Are you sure you want to delete selected columns?", "Column Delete", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                    {
                        foreach (GraphicListViewItem item in _this.SelectedItems)
                        {
                            Items.Remove(item);
                        }
                        SetTableAliseNViewColumns();
                        SetAutoSize();
                        Refresh();
                    }
                }
            }
            else if (e.KeyCode == Keys.F2)
            {
                ShowTitleEditor();
            }
        }

        private void OnInnerListView_KeyDown(Keys keycode)
        {
            InnerListView_KeyDown(InnerListView, new KeyEventArgs(keycode));
        }

        private void InnerListView_MouseDown(object sender, MouseEventArgs e)
        {
            BringToFront();
        }

        private void InnerListView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuLocation = Cursor.Position;

                Point point = this.GetLocationFromTarget();
                point.Offset(InnerListView.Location);
                point.Offset(e.Location);

                GraphicListViewItem target = GetItem(InnerListView, ContextMenuLocation.X, ContextMenuLocation.Y);
                ListViewContextMenu.MenuItems[0].Enabled = true;
                if (target == null)
                {
                    ListViewContextMenu.MenuItems[1].Enabled = false;
                    ListViewContextMenu.MenuItems[2].Enabled = false;
                }
                else
                {
                    ListViewContextMenu.MenuItems[1].Enabled = target.Row["NODE_DETAIL_TYPE"].ToString() == "U";
                    ListViewContextMenu.MenuItems[2].Enabled = true;
                }

                ListViewContextMenu.Show(Parent as Control, point);
            }
        }

        private void InnerListView_Resize(object sender, EventArgs e)
        {
            GraphicListView _this = (GraphicListView)sender;
        }

        private void InnerListView_DragOver(object sender, DragEventArgs e)
        {
            GraphicListView _this = (GraphicListView)sender;
            Dictionary<string, object> dic = e.Data.GetData(typeof(Dictionary<string, object>)) as Dictionary<string, object>;
            if (dic != null)
            {
                if (!_this.Focused) _this.Focus();

                if (dic["Items"].GetType() == typeof(List<GraphicListViewItem>))
                {
                    GraphicListViewItem target = GetItem(InnerListView, e.X, e.Y);
                    if (target != null)
                    {
                        Rectangle lstScreenRectangle = RectangleToScreen(InnerListView.Bounds);
                        if (target.Bounds.Y + target.Bounds.Height / 2 < e.Y - (InnerListView.Padding.Top + lstScreenRectangle.Top))
                        {
                            InnerListView.InsertLineIndex = target.Index + 1;
                        }
                        else
                        {
                            InnerListView.InsertLineIndex = target.Index;
                        }
                    }
                    else
                    {
                        InnerListView.InsertLineIndex = -1;
                    }

                    e.Effect = dic["Sender"].Equals(sender) ? DragDropEffects.Move: DragDropEffects.Copy;
                    Refresh();
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
        }

        private void InnerListView_DragDrop(object sender, DragEventArgs e)
        {
            Dictionary<string, object> dic = e.Data.GetData(typeof(Dictionary<string, object>)) as Dictionary<string, object>;
            if (dic["Items"].GetType() == typeof(List<GraphicListViewItem>))
            {
                var newItems = new List<GraphicListViewItem>();
                var items = dic["Items"] as List<GraphicListViewItem>;

                // Sort (this to this)
                if (dic["Sender"].Equals(sender))
                {
                    if (items.Count > 0)
                    {
                        var sortedItems = items.OrderBy(item => item.Index).ToList();
                        int targetIndex = sortedItems[0].Index < InnerListView.InsertLineIndex
                                    ? InnerListView.InsertLineIndex - 1
                                    : InnerListView.InsertLineIndex;

                        if (targetIndex != sortedItems[0].Index)
                        {
                            InnerListView.Items.Move(items[0].Index, targetIndex);
                            for (int i = 1; i < sortedItems.Count; i++)
                            {
                                if (items[0].Index < items[i].Index)
                                {
                                    InnerListView.Items.Move(items[i].Index, items[0].Index + i);
                                }
                                else
                                {
                                    InnerListView.Items.Move(items[i].Index, items[0].Index + i - 1);
                                }
                            }
                        }

                        // Set Selection
                        InnerListView.FocusedIndex = targetIndex;
                        for (int i = 0; i < sortedItems.Count; i++)
                        {
                            items[i].Selected = true;
                        }
                    }
                }
                // Insert (other to this)
                else
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        if (InnerListView.Data == null)
                        {
                            InnerListView.Data = GetEmptyNodeDetailModelDataTable();
                        }

                        var newRow = InnerListView.Data.NewRow();
                        foreach(DataColumn column in items[i].Row.Table.Columns)
                        {
                            if (newRow.Table.Columns.Contains(column.ColumnName))
                            {
                                newRow[column.ColumnName] = items[i].Row[column.ColumnName];
                            }
                        }
                        GraphicListViewItem newItem = new GraphicListViewItem(InnerListView, newRow);
                        newItem.Row["NODE_ID"] = ID;
                        newItem.Row["NODE_SEQ"] = SEQ;
                        newItem.Row["NODE_ID_REF"] = items[i].Row["NODE_ID"];
                        newItem.Row["NODE_SEQ_REF"] = items[i].Row["NODE_SEQ"];

                        int newItemSeq = 0;
                        foreach(GraphicListViewItem thisItem in InnerListView.Items)
                        {
                            if (thisItem.Row["NODE_ID_REF"].Equals(newItem.Row["NODE_ID_REF"])
                             && thisItem.Row["NODE_DETAIL_ID"].Equals(newItem.Row["NODE_DETAIL_ID"])
                             && newItemSeq <= Convert.ToInt32(thisItem.Row["NODE_DETAIL_SEQ"])
                            )
                            {
                                newItemSeq = Convert.ToInt32(thisItem.Row["NODE_DETAIL_SEQ"]) + 1;
                            }
                        }
                        newItem.Row["NODE_DETAIL_SEQ"] = newItemSeq;

                        if (InnerListView.InsertLineIndex >= 0)
                        {
                            InnerListView.Items.Insert(InnerListView.InsertLineIndex + i, newItem);
                        }
                        else
                        {
                            InnerListView.Items.Add(newItem);
                        }
                        newItems.Add(newItem);
                    }

                    SetTableAliseNViewColumns();
                }
                
                SetAutoSize();
                Refresh();

                // Call Event
                Dictionary<string, object> newItemsData = new Dictionary<string, object>
                {
                      {"Items", newItems }
                };
                Event.CallEvent(sender, DragDropItems, new UserEventArgs(newItemsData));

                InnerListView.InsertLineIndex = -1;
            }
        }

        private void InnerListView_DragLeave(object sender, EventArgs e)
        {
            InnerListView.InsertLineIndex = -1;
        }

        private void InnerListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(new Dictionary<string, object>()
            {
                  { "Sender", sender }
                , { "Items", ((GraphicListView)sender).SelectedItems }

            }, DragDropEffects.Move);
        }

        private void InnerListView_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (e.EscapePressed)
            {
                InnerListView.InsertLineIndex = -1;
            }
        }

        private void ColumnNode_BtDeleteMouseDown(object sender, UserEventArgs e)
        {
            DeleteThisNode();
        }

        private void ColumnNode_BtContextMenuClick(object sender, UserEventArgs e)
        {
            ContextMenuLocation = Cursor.Position;
            Point point = this.GetLocationFromTarget();
            point.Offset(((MouseEventArgs)e.Data["e"]).Location);

            TitleContextMenu.Show(Parent as Control, point);
        }

        private void ColumnNode_TitleChanging(object sender, TitleChangingEventArgs e)
        {
            bool duplicated = false;
            foreach(GraphicControl control in Target.Controls)
            {
                NodeBase node = control as NodeBase;
                if (node != null && !node.Equals(this))
                {
                    if (node.ID == e.NewTitle)
                    {
                        duplicated = true;
                        break;
                    }
                }
            }

            if (duplicated)
            {
                ModalMessageBox.Show("ID is duplicated.", "Column Node");
                e.Cancel = true;
                Target.Focus();
                TitleEditor.Focus();
                TitleEditor.SelectionStart = 0;
                TitleEditor.SelectionLength = TitleEditor.Text.Length;
            }
            else
            {
                ID = e.NewTitle;
                if (InnerListView.Data != null)
                {
                    foreach(DataRow row in InnerListView.Data.Rows)
                    {
                        row["NODE_ID"] = ID;
                    }
                }
            }
        }

        private void SetTableAliseNViewColumns()
        {
            // 0: NODE_ID_REF, 1:NODE_SEQ_REF, 2:Alias, 3:Alias Sort
            List<object[]> refData = new List<object[]>();

            // Set refData
            foreach (GraphicListViewItem thisItem in InnerListView.Items)
            {
                if (!string.IsNullOrEmpty(thisItem.Row["NODE_ID_REF"].ToString()))
                {
                    bool duplicated = false;
                    for (int i = 0; i < refData.Count; i++)
                    {
                        if (!string.IsNullOrEmpty((string)refData[i][0]))
                        {
                            if (refData[i][0].ToString() == thisItem.Row["NODE_ID_REF"].ToString()
                             && refData[i][1].ToString() == thisItem.Row["NODE_SEQ_REF"].ToString()
                            )
                            {
                                duplicated = true;
                            }
                        }
                    }

                    if (!duplicated)
                    {
                        refData.Add(new object[] { thisItem.Row["NODE_ID_REF"], thisItem.Row["NODE_SEQ_REF"], null, 0 });
                    }
                }
            }

            // Set refData[2] Alias
            SetTableAliasNViewColumns_SetAlias(ref refData);

            // Set refData[3] Alias Sort
            for (int i = 0; i < refData.Count; i++)
            {
                for (int k = 0; k < refData.Count; k++)
                {
                    if (i != k && refData[i][0] == refData[k][0] && refData[k][1].ToInt() < refData[i][1].ToInt())
                    {
                        refData[i][3] = refData[i][3].ToInt() + 1;
                    }
                }
            }

            // Set Group refData[2]
            for (int i = 0; i < refData.Count; i++)
            {
                for (int k = 0; k < refData.Count; k++)
                {
                    if (i != k && refData[i][0] == refData[k][0])
                    {
                        refData[i][3] = refData[i][3].ToInt() + 1;
                        break;
                    }
                }
            }

            // Set NODE_DETAIL_TABLE_ALIAS, NODE_DETAIL_VIEW_COLUMN1
            foreach (GraphicListViewItem thisItem in InnerListView.Items)
            {
                string talbeAlias = "";
                string tableAliasCount = "";
                if (!string.IsNullOrEmpty(thisItem.Row["NODE_ID_REF"].ToString()))
                {
                    DataRow node = thisItem.Row;
                    for (int i = 0; i < refData.Count; i++)
                    {
                        if (refData[i][0].ToString() == node["NODE_ID_REF"].ToString()
                        && refData[i][1].ToString() == node["NODE_SEQ_REF"].ToString())
                        {
                            talbeAlias = refData[i][2].ToString();
                            tableAliasCount = refData[i][3].ToString();
                            if (tableAliasCount == "0")
                            {
                                tableAliasCount = "";
                            }
                        }
                    }

                    thisItem.Row["NODE_DETAIL_TABLE_ALIAS"] = talbeAlias.ToString() + tableAliasCount;

                    if (NodeOptionShowTableAlias)
                    {
                        thisItem.Row["NODE_DETAIL_VIEW_COLUMN1"] =
                              thisItem.Row["NODE_DETAIL_TABLE_ALIAS"]
                            + "."
                            + thisItem.Row["NODE_DETAIL_ID"].ToString()
                            + $"({thisItem.Row["NODE_DETAIL_DATA_TYPE"]})";
                    }
                    else
                    {
                        thisItem.Row["NODE_DETAIL_VIEW_COLUMN1"] =
                              thisItem.Row["NODE_DETAIL_ID"].ToString()
                            + $"({thisItem.Row["NODE_DETAIL_DATA_TYPE"]})";
                    }

                    thisItem.ToolTipFormat["NODE_DETAIL_VIEW_COLUMN1"] = "{NODE_ID_REF}({NODE_SEQ_REF}) {NODE_DETAIL_VIEW_COLUMN1} {NODE_DETAIL_COMMENT}";
                }
            }
        }

        private void SetTableAliasNViewColumns_SetAlias(ref List<object[]> refData)
        {
            // 0: NODE_ID_REF, 1:NODE_SEQ_REF, 2:Alias, 3:Alias Sort
            ST.Core.Extensions.SetAlias(ref refData, 0, 1, 2);
        }

        private void SetAutoSize()
        {
            if (AutoSize)
            {
                OriginalHeight = OriginalTitleHeight + OriginalPadding.Vertical + Math.Max((ListHeight * (1 / ScaleValue)).ToInt(), InnerListView.ItemHeight + InnerListView.ItemVerticalDistance * 2);
                Height = TitleHeight + Padding.Vertical + Math.Max(ListHeight.ToInt(), (InnerListView.ItemHeight * ScaleValue).ToInt() + (InnerListView.ItemVerticalDistance * ScaleValue).ToInt() * 2);

                if (Width == 0)
                {
                    Width = ((AutoMinWidth + Padding.Horizontal) * ScaleValue).ToInt();
                }
                else
                {
                    var maxColumnsWidth = InnerListView.GetMaxColumnsTextWidth(1);
                    if (NodeOptionShowComment)
                    {
                        if (maxColumnsWidth[0] + maxColumnsWidth[1] > AutoMaxWidth)
                        {
                            InnerListView.Columns[0].Width = (Math.Min(Math.Max(AutoMinColumnWidth, maxColumnsWidth[0].ToInt()), AutoMaxWidth - AutoMinColumnWidth) * ScaleValue).ToInt();
                            InnerListView.Columns[1].Width = (AutoMaxWidth - InnerListView.Columns[0].Width * ScaleValue).ToInt();
                        }
                        else if (maxColumnsWidth[0] + maxColumnsWidth[1] + ColumnWidthRevision[0] + ColumnWidthRevision[1] < AutoMinWidth)
                        {
                            InnerListView.Columns[0].Width = (AutoMinWidth - maxColumnsWidth[1].ToInt() + ColumnWidthRevision[0] * ScaleValue).ToInt();
                            InnerListView.Columns[1].Width = (maxColumnsWidth[1].ToInt() + ColumnWidthRevision[1] * ScaleValue).ToInt();
                        }
                        else
                        {
                            InnerListView.Columns[0].Width = (maxColumnsWidth[0] + ColumnWidthRevision[0]).ToInt();
                            InnerListView.Columns[1].Width = (maxColumnsWidth[1] + ColumnWidthRevision[1]).ToInt();
                        }

                        OriginalWidth = InnerListView.Columns[0].Width + InnerListView.Columns[1].Width + OriginalPadding.Horizontal;
                        //Width = (OriginalWidth * ScaleValue).ToInt() + 1;
                        Width = (InnerListView.Columns[0].Width * ScaleValue).ToInt()
                                + (InnerListView.Columns[1].Width * ScaleValue).ToInt()
                                + Padding.Horizontal;
                        //Width = InnerListView.Width + Padding.Horizontal;
                    }
                    else
                    {
                        InnerListView.Columns[0].Width = Math.Min(
                              Math.Max(maxColumnsWidth[0].ToInt() + ColumnWidthRevision[0], AutoMinWidth)
                            , AutoMaxWidth
                        );

                        OriginalWidth = InnerListView.Columns[0].Width + OriginalPadding.Horizontal;
                        Width = (OriginalWidth * ScaleValue).ToInt();
                    }
                }
            }
        }

        private void SetViewColumn()
        {
            foreach(var item in InnerListView.Items)
            {
                if (NodeOptionShowTableAlias)
                {
                    item.Row["NODE_DETAIL_VIEW_COLUMN1"] =
                          item.Row["NODE_DETAIL_TABLE_ALIAS"]
                        + "."
                        + item.Row["NODE_DETAIL_ID"].ToString()
                        + $" [{item.Row["NODE_DETAIL_DATA_TYPE_FULL"].ToString().ToLower()}]";
                }
                else
                {
                    item.Row["NODE_DETAIL_VIEW_COLUMN1"] =
                          item.Row["NODE_DETAIL_ID"].ToString()
                        + $" [{item.Row["NODE_DETAIL_DATA_TYPE_FULL"].ToString().ToLower()}]";
                }
            }
        }

        #region Context Function
        private void DeleteThisNode()
        {
            if (ModalMessageBox.Show("Are you sure you want to delete this column node?", "Memo Node", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                Parent.Controls.Remove(this);
                Target.Refresh();
                Target.MimimapRefresh();
            }
        }

        private void AddUserColumn(Point screenCursorPosition)
        {
            ModalUserColumn modal = new ModalUserColumn();
            if (modal.ShowDialog() == DialogResult.OK)
            {
                if (InnerListView.Data == null)
                {
                    InnerListView.Data = GetEmptyNodeDetailModelDataTable();
                }

                // Set newItem
                var newRow = InnerListView.Data.NewRow();
                GraphicListViewItem newItem = new GraphicListViewItem(InnerListView, newRow);
                newItem.ForeColor = UserColumnForeColor;
                newItem.Row["NODE_ID"] = ID;
                newItem.Row["NODE_SEQ"] = SEQ;
                newItem.Row["NODE_DETAIL_ID"] = modal.ColumnName;
                newItem.Row["NODE_DETAIL_TYPE"] = "U";
                newItem.Row["NODE_DETAIL_DATA_TYPE"] = GetDataType(modal.DataTypeFull).ToUpper();
                newItem.Row["NODE_DETAIL_DATA_TYPE_FULL"] = modal.DataTypeFull.ToUpper();
                newItem.Row["NODE_DETAIL_COMMENT"] = modal.Comment;
                newItem.Row["NODE_DETAIL_TABLE_ALIAS"] = modal.TableAlias;
                newItem.Row["NODE_ID_REF"] = DBNull.Value;
                newItem.Row["NODE_SEQ_REF"] = DBNull.Value;

                string viewColumn1 = modal.ColumnName;
                if (NodeOptionShowTableAlias && modal.TableAlias.Length > 0) { viewColumn1 = $"{modal.TableAlias}.{viewColumn1}"; }
                if (modal.DataTypeFull.Length > 0) { viewColumn1 = $"{viewColumn1} [{modal.DataTypeFull}]"; }
                newItem.Row["NODE_DETAIL_VIEW_COLUMN1"] = viewColumn1;


                newItem.Row["NODE_DETAIL_VIEW_COLUMN2"] = modal.Comment;

                int newItemSeq = 0;
                foreach (GraphicListViewItem thisItem in InnerListView.Items)
                {
                    if (thisItem.Row["NODE_ID_REF"].Equals(DBNull.Value)
                     && thisItem.Row["NODE_DETAIL_ID"].Equals(newItem.Row["NODE_DETAIL_ID"])
                     && newItemSeq <= Convert.ToInt32(thisItem.Row["NODE_DETAIL_SEQ"])
                    )
                    {
                        newItemSeq = Convert.ToInt32(thisItem.Row["NODE_DETAIL_SEQ"]) + 1;
                    }
                }
                newItem.Row["NODE_DETAIL_SEQ"] = newItemSeq;

                // Append Item
                GraphicListViewItem target = GetItem(InnerListView, screenCursorPosition.X, screenCursorPosition.Y);
                if (target != null)
                {
                    Rectangle lstScreenRectangle = RectangleToScreen(InnerListView.Bounds);
                    if (target.Bounds.Y + target.Bounds.Height / 2 < screenCursorPosition.Y - (InnerListView.Padding.Top + lstScreenRectangle.Top))
                    {
                        InnerListView.Items.Insert(target.Index + 1, newItem);
                    }
                    else
                    {
                        InnerListView.Items.Insert(target.Index, newItem);
                    }
                }
                else
                {
                    InnerListView.Items.Add(newItem);
                }

                SetAutoSize();
                Refresh();
            }
        }

        private string GetDataType(string dataTypeFull)
        {
            string rs;
            int sp = dataTypeFull.IndexOf("(");
            if (sp >= 0)
            {
                rs = dataTypeFull.Substring(0, sp);
            }
            else
            {
                rs = dataTypeFull;
            }
            return rs;
        }

        private void EditUserColumn(Point location)
        {
            GraphicListViewItem target = GetItem(InnerListView, ContextMenuLocation.X, ContextMenuLocation.Y);
            if (target == null)
            {
                ModalMessageBox.Show("Can not found the column.", "User Column", MessageBoxButtons.OK);
            }
            else
            {
                ModalUserColumn modal = new ModalUserColumn(
                      target.Row["NODE_DETAIL_ID"].ToString()
                    , target.Row["NODE_DETAIL_DATA_TYPE_FULL"].ToString().ToUpper()
                    , target.Row["NODE_DETAIL_COMMENT"].ToString()
                    , target.Row["NODE_DETAIL_TABLE_ALIAS"].ToString()
                );

                if (modal.ShowDialog() == DialogResult.OK)
                {
                    target.Row["NODE_DETAIL_ID"] = modal.ColumnName;
                    target.Row["NODE_DETAIL_DATA_TYPE"] = modal.DataTypeFull.ToUpper();
                    target.Row["NODE_DETAIL_DATA_TYPE_FULL"] = modal.DataTypeFull.ToUpper();
                    target.Row["NODE_DETAIL_COMMENT"] = modal.Comment;
                    target.Row["NODE_DETAIL_TABLE_ALIAS"] = modal.TableAlias;

                    string viewColumn1 = modal.ColumnName;
                    if (NodeOptionShowTableAlias && modal.TableAlias.Length > 0) { viewColumn1 = $"{modal.TableAlias}.{viewColumn1}"; }
                    if (modal.DataTypeFull.Length > 0) { viewColumn1 = $"{viewColumn1} [{modal.DataTypeFull}]"; }
                    target.Row["NODE_DETAIL_VIEW_COLUMN1"] = viewColumn1;
                    target.Row["NODE_DETAIL_VIEW_COLUMN2"] = modal.Comment;

                    SetAutoSize();
                    Refresh();
                }
            }
        }

        private void DeleteColumn(Point location)
        {
            OnInnerListView_KeyDown(Keys.Delete);
        }

        private DataTable GetEmptyNodeDetailModelDataTable()
        {
            var temp = new List<NodeDetailModel>();
            DataTable rs = temp.ToDataTable();
            return rs;
        }
        #endregion
    }
}