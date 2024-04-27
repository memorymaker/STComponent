using ST.Controls;
using ST.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public partial class TableNode
    {
        private Color DragBackColor = Color.FromArgb(238, 163, 113);
        private Color DragBorderColor = Color.FromArgb(170, 218, 254);
        private DashStyle DragBorderDashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
        private DragDropEffects DragDropEffectsState = DragDropEffects.None;

        private void LoadInput()
        {
            AllowDrop = true;
            DragOver += TableNode_DragOver;
            DragDrop += TableNode_DragDrop;
            DragLeave += TableNode_DragLeave;
            Paint += TableNode_Paint;
            ScaleValueChanged += TableNode_ScaleValueChanged;

            InnerListView.KeyDown += InnerListView_KeyDown;
            InnerListView.MouseDown += InnerListView_MouseDown;
            InnerListView.Resize += InnerListView_Resize;

            InnerListView.DragOver += InnerListView_DragOver;
            InnerListView.DragDrop += InnerListView_DragDrop;

            InnerListView.ItemDrag += InnerListView_ItemDrag;

            BtDeleteMouseDown += TableNode_BtDeleteMouseDown;
            BtDelete.SetDrawColor(SimpleGraphicControl.StateType.Default, Color.FromArgb(251, 231, 217));
        }

        private void TableNode_DragLeave(object sender, EventArgs e)
        {
            DragDropEffectsState = DragDropEffects.None;
            Refresh();
        }

        private void TableNode_ScaleValueChanged(object sender, IScaleControlScaleValueChangedEventArgs e)
        {
            if (AutoSize)
            {
                SetAutoSize();
            }
        }

        private void TableNode_DragOver(object sender, DragEventArgs e)
        {
            GraphicListView _this = ((TableNode)sender).InnerListView;
            Dictionary<string, object> dic = e.Data.GetData(typeof(Dictionary<string, object>)) as Dictionary<string, object>;

            if (dic != null && !dic["Sender"].Equals(_this))
            {
                var pos = PointToClient(new Point(e.X, e.Y));
                var itemsOrigin = dic["Items"] as List<GraphicListViewItem>;
                if (pos.Y < TitleHeight && itemsOrigin != null && itemsOrigin.Count == 1)
                {
                    e.Effect = DragDropEffects.Copy;
                    DragDropEffectsState = DragDropEffects.Copy;
                    Refresh();
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                    DragDropEffectsState = DragDropEffects.None;
                    Refresh();
                }
            }
        }

        private void TableNode_DragDrop(object sender, DragEventArgs e)
        {
            Dictionary<string, object> dic = e.Data.GetData(typeof(Dictionary<string, object>)) as Dictionary<string, object>;
            var itemsOrigin = dic["Items"] as List<GraphicListViewItem>;
            if (itemsOrigin != null && itemsOrigin.Count == 1)
            {
                RelationModel model = new RelationModel()
                {
                    RELATION_TYPE = Target.GetRelationType(
                        itemsOrigin[0].Row["NODE_ID"].ToString()
                        , Convert.ToInt32(itemsOrigin[0].Row["NODE_SEQ"])
                        , ID
                        , SEQ
                    ),
                    RELATION_OPERATOR = "=",
                    RELATION_VALUE = "",
                    NODE_ID1 = itemsOrigin[0].Row["NODE_ID"].ToString(),
                    NODE_SEQ1 = Convert.ToInt32(itemsOrigin[0].Row["NODE_SEQ"]),
                    NODE_DETAIL_ID1 = itemsOrigin[0].Row["NODE_DETAIL_ID"].ToString(),
                    NODE_DETAIL_SEQ1 = Convert.ToInt32(itemsOrigin[0].Row["NODE_DETAIL_SEQ"]),
                    NODE_ID2 = ID,
                    NODE_SEQ2 = SEQ,
                    NODE_DETAIL_ID2 = "",
                    NODE_DETAIL_SEQ2 = 0,
                };

                if (!Target.RelationContains(model))
                {
                    var modal = new ModalColumnToTableRelationEditor(model);
                    if (modal.ShowDialog() == DialogResult.OK)
                    {
                        Dictionary<string, object> relationsData = new Dictionary<string, object>
                        {
                              {"ItemsOrigin", dic["Items"] }
                            , {"ItemsDestination", model }
                        };
                        Target.AddRelations(relationsData);

                        // Call Event
                        Event.CallEvent(sender, DragDropItems, new UserEventArgs(relationsData));
                    }
                }
            }

            e.Effect = DragDropEffects.None;
            DragDropEffectsState = DragDropEffects.None;
        }

        private void TableNode_Paint(object sender, PaintEventArgs e)
        {
            if (DragDropEffectsState == DragDropEffects.Copy)
            {
                var g = e.Graphics;

                // Draw Back
                Brush brush = new SolidBrush(DragBackColor);
                g.FillRectangle(brush, 1, 1, Width - 3, TitleHeight - 2);

                // Draw Border
                Pen pen = new Pen(DragBorderColor);
                pen.DashStyle = DragBorderDashStyle;
                g.DrawRectangle(pen, 1, 1, Width - 3, TitleHeight - 2);

                // Draw Title
                int titleHeightAPaddingTop = TitleHeight + Padding.Top;
                Font scaleFont = new Font(Font.FontFamily, Font.Size * ScaleValue
                    , (TitleBold ? FontStyle.Bold : FontStyle.Regular)
                );
                var scaleFontSize = g.MeasureString(Title, scaleFont);
                float leftNtop = (titleHeightAPaddingTop / 2) - (scaleFontSize.Height / 2);
                g.DrawString(Title, scaleFont, new SolidBrush(ForeColor), leftNtop, leftNtop);
            }
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
                    Clipboard.SetText(string.Join("\r\n", items));
                }
            }
        }

        private void InnerListView_MouseDown(object sender, MouseEventArgs e)
        {
            BringToFront();
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

                if (!dic["Sender"].Equals(sender))
                {
                    GraphicListViewItem target = GetItem(InnerListView, e.X, e.Y);

                    if (dic["Items"].GetType() == typeof(List<GraphicListViewItem>) && target != null)
                    {
                        int recivedItemsCount = ((List<GraphicListViewItem>)dic["Items"]).Count;
                        for (int i = 0; i < _this.Items.Count; i++)
                        {
                            if (target.Index <= i && i <= target.Index + (recivedItemsCount - 1))
                            {
                                _this.Items[i].Selected = true;
                            }
                            else
                            {
                                _this.Items[i].Selected = false;
                            }
                        }
                        e.Effect = DragDropEffects.Copy;
                        Refresh();
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
            }
        }

        private void InnerListView_DragDrop(object sender, DragEventArgs e)
        {
            Dictionary<string, object> dic = e.Data.GetData(typeof(Dictionary<string, object>)) as Dictionary<string, object>;
            if (dic["Items"].GetType() == typeof(List<GraphicListViewItem>))
            {
                Dictionary<string, object> relationsData = new Dictionary<string, object>
                {
                      {"ItemsOrigin", dic["Items"] }
                    , {"ItemsDestination", ((GraphicListView)sender).SelectedItems }
                };
                Target.AddRelations(relationsData);

                Refresh();

                // Call Event
                Event.CallEvent(sender, DragDropItems, new UserEventArgs(relationsData));
            }
        }

        private void InnerListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(new Dictionary<string, object>()
            {
                  { "Sender", sender }
                , { "Items", ((GraphicListView)sender).SelectedItems }

            }, DragDropEffects.Copy);
        }

        private void TableNode_BtDeleteMouseDown(object sender, UserEventArgs e)
        {
            if (ModalMessageBox.Show("Are you sure you want to delete this table node?", "Table Node", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                Parent.Controls.Remove(this);
                Target.Refresh();
                Target.MimimapRefresh();
            }
        }

        private void SetAutoSize()
        {
            Height = TitleHeight + Padding.Vertical + ListHeight.ToInt();

            var maxColumnsWidth = InnerListView.GetMaxColumnsTextWidth(1);
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
            Width = (OriginalWidth * ScaleValue).ToInt();
        }
    }
}