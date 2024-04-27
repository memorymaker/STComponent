using ST.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public partial class GraphicListView
    {
        // System Option
        private int DragStartPixel = 4;

        // Ref
        private bool IsMouseDown = false;
        private int MouseDownIndex;
        private Point MouseDownLocation;
        private bool IsDragStarted = false;

        // ToolTip
        private ToolTip ToolTip;
        private int OldItemIndex = -1;
        private int OldColumnIndex = -1;

        private void LoadInput()
        {
            KeyDown += GraphicListView_KeyDown;

            MouseDown += UserListView_MouseDown;
            MouseMove += UserListView_MouseMove;
            MouseUp += UserListView_MouseUp;
            MouseWheel += UserListView_MouseWheel;
            MouseLeave += GraphicListView_MouseLeave;

            DragEnter += UserListView_DragEnter;
            DragDrop += UserListView_DragDrop;
        }

        private void GraphicListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (ModifierKeys == Keys.Shift)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up: SetSelectedItemIndexShift(Math.Max(FocusedIndexOnly - 1, 0)); break;
                    case Keys.Down: SetSelectedItemIndexShift(Math.Min(FocusedIndexOnly + 1, Items.Count - 1)); break;
                }
            }
            else if (ModifierKeys == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up: FocusedIndexOnly = Math.Max(FocusedIndexOnly - 1, 0); break;
                    case Keys.Down: FocusedIndexOnly = Math.Min(FocusedIndexOnly + 1, Items.Count - 1); break;
                    case Keys.Space: ToggleSelectedItemIndexList(FocusedIndexOnly); break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Up: FocusedIndex = Math.Max(FocusedIndexOnly - 1, 0); break;
                    case Keys.Down: FocusedIndex = Math.Min(FocusedIndexOnly + 1, Items.Count - 1); break;
                }
            }
            SetShiftProcessFalse();
        }

        private void UserListView_MouseDown(object sender, MouseEventArgs e)
        {
            int newIndex = GetItemIndex(e.Location);
            if (ModifierKeys == Keys.Shift)
            {
                SetSelectedItemIndexShift(newIndex);
            }
            else if (ModifierKeys == Keys.Control)
            {
                FocusedIndexOnly = newIndex;
                ToggleSelectedItemIndexList(FocusedIndexOnly);
            }
            else
            {
                if (SelectedIndexes.Count > 1 && SelectedIndexes.Contains(newIndex))
                {
                    FocusedIndexOnly = newIndex;
                }
                else
                {
                    FocusedIndex = newIndex;
                }
            }
            SetShiftProcessFalse();

            HideToolTip();

            IsMouseDown = true;
            MouseDownIndex = newIndex;
            MouseDownLocation = e.Location;
            IsDragStarted = false;
        }

        private void UserListView_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseDown && e.Button == MouseButtons.Left)
            {
                if (AllowDrag && GetDistance(MouseDownLocation, e.Location) > DragStartPixel && !IsDragStarted)
                {
                    if (MouseDownIndex < Items.Count)
                    {
                        if (ItemDrag == null)
                        {
                            DoDragDrop(new Dictionary<string, object>()
                            {
                                {"Sender", sender }
                                , {"Items", Items[MouseDownIndex] }
                            }, DragDropEffects.Copy);
                        }
                        else
                        {
                            ItemDrag.Invoke(sender, new ItemDragEventArgs(e.Button));
                        }
                        IsDragStarted = true;
                    }
                }
            }
            else if (e.Button == MouseButtons.None)
            {
                int itemIndex = GetItemIndex(e.Location);
                if (0 <= itemIndex && itemIndex < Items.Count)
                {
                    int columnIndex = GetColumnIndex(e.Location);
                    if (columnIndex >= 0)
                    {
                        string fieldName = Columns[columnIndex].FieldName.Trim();

                        if (Items[itemIndex].ToolTipFormat.ContainsKey(fieldName) && Items[itemIndex].ToolTipFormat[fieldName].Length > 0)
                        {
                            string tooltipFormat = Items[itemIndex].ToolTipFormat[fieldName].Trim();

                            if (tooltipFormat.Length > 0)
                            {
                                if ((itemIndex != OldItemIndex || columnIndex != OldColumnIndex) || !IsToolTipShowing())
                                {
                                    string value = tooltipFormat;
                                    foreach (DataColumn column in Items[itemIndex].Row.Table.Columns)
                                    {
                                        value = value.Replace("{" + column.ColumnName + "}", Items[itemIndex].Row[column.ColumnName].ToString());
                                    }
                                    ShowToolTip(value);
                                }
                            }
                        }
                        else if (Items[itemIndex].TextOverFlowList.Contains(columnIndex))
                        {
                            if ((itemIndex != OldItemIndex || columnIndex != OldColumnIndex) || !IsToolTipShowing())
                            {
                                string value = Items[itemIndex].Row[fieldName].ToString();
                                ShowToolTip(value);
                            }
                        }
                        else
                        {
                            HideToolTip();
                        }
                    }
                    else
                    {
                        HideToolTip();
                    }
                    OldColumnIndex = columnIndex;
                }
                else
                {
                    HideToolTip();
                }
                OldItemIndex = itemIndex;
            }
        }

        private void UserListView_MouseUp(object sender, MouseEventArgs e)
        {
            int newIndex = GetItemIndex(e.Location);
            if (ModifierKeys != Keys.Shift && ModifierKeys != Keys.Control && e.Button != MouseButtons.Right)
            {
                if (SelectedIndexes.Count > 1)
                {
                    FocusedIndex = newIndex;
                }
            }
            SetShiftProcessFalse();

            IsMouseDown = false;
            IsDragStarted = false;
        }

        private void UserListView_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) != Keys.Control)
            {
                var point = -(e.Delta * 3 / 120) * ScrollBarVertical.SmallChange;
                ScrollBarVertical.Value += point;
            }
        }

        private void GraphicListView_MouseLeave(object sender, EventArgs e)
        {
            HideToolTip();
        }

        private void UserListView_DragEnter(object sender, DragEventArgs e)
        {
            Dictionary<string, object> dic = e.Data.GetData(typeof(Dictionary<string, object>)) as Dictionary<string, object>;

            if (dic != null && !dic["Sender"].Equals(sender) && dic["Items"].GetType() == typeof(List<GraphicListViewItem>))
            {
                e.Effect = DragDropEffects.Copy;
                Console.WriteLine("ListView Copy");
            }
            else
            {
                e.Effect = DragDropEffects.None;
                Console.WriteLine("ListView None");
            }
        }

        private void UserListView_DragDrop(object sender, DragEventArgs e)
        {
        }

        #region Function
        private void ShowToolTip(string text)
        {
            if (ToolTip == null)
            {
                ToolTip = new ToolTip();
                ToolTip.InitialDelay = 0;
                ToolTip.AutomaticDelay = 0;
                ToolTip.ReshowDelay = 0;
                ToolTip.UseAnimation = false;
                ToolTip.Show(text, Target);
            }
            else
            {
                ToolTip.Show(text, Target);
            }
        }

        private void HideToolTip()
        {
            if (ToolTip != null)
            {
                ToolTip.Dispose();
                ToolTip = null;
            }
        }

        private bool IsToolTipShowing()
        {
            return ToolTip != null;
        }

        private int GetItemIndex(Point mouseLocation)
        {
            // Scale
            int scaleItemHeight = (int)Math.Round(ItemHeight * ScaleValue);
            int scaleItemVerticalDistance = (int)Math.Round(ItemVerticalDistance * ScaleValue);
            int scaleColumnHeight = (int)Math.Round(ColumnHeight * ScaleValue);

            int _itemHeightBlock = scaleItemHeight + scaleItemVerticalDistance;
            int newIndex = Math.Max((mouseLocation.Y + ScrollTop - scaleColumnHeight) / _itemHeightBlock, 0);

            return newIndex;
        }

        private int GetColumnIndex(Point mouseLocation)
        {
            int rs = -1;
            int left = 0;
            for(int i = 0; i < Columns.Count; i++)
            {
                if (left <= mouseLocation.X && mouseLocation.X <= left + Columns[i].Width)
                {
                    rs = i;
                    break;
                }
                left += Columns[i].Width;
            }
            return rs;
        }

        private void SetSelectedItemIndexShift(int index)
        {
            if (!ShiftProcess)
            {
                ShiftProcess = true;
                ShiftProcessBaseIndex = FocusedIndexOnly;
            }

            FocusedIndexOnly = index;
            SelectedIndexes.Clear();
            for (int i = Math.Min(ShiftProcessBaseIndex, FocusedIndexOnly); i <= Math.Max(ShiftProcessBaseIndex, FocusedIndexOnly); i++)
            {
                SelectedIndexes.Add(i);
            }

            Refresh();
        }

        private void ToggleSelectedItemIndexList(int index)
        {
            if (!SelectedIndexes.Contains(index))
            {
                SelectedIndexes.Add(index);
            }
            else
            {
                SelectedIndexes.Remove(index);
            }

            Refresh();
        }

        private void SetShiftProcessFalse()
        {
            if (ModifierKeys != Keys.Shift)
            {
                ShiftProcess = false;
            }
        }

        private double GetDistance(Point pt1, Point pt2)
        {
            var distance = Math.Sqrt((Math.Pow(pt1.X - pt2.X, 2) + Math.Pow(pt1.Y - pt2.Y, 2)));
            return distance;
        }
        #endregion
    }
}