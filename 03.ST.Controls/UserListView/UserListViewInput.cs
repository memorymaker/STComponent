using Newtonsoft.Json.Linq;
using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.Controls
{
    public partial class UserListView
    {
        // System Option
        private int DragStartPixel = 4;

        // Ref
        private bool IsMouseDown = false;
        private int MouseDownIndex;
        private Point MouseDownLocation;
        private bool IsDragStarted = false;

        private void LoadInput()
        {
            SetInputEvents();
        }

        private void SetInputEvents()
        {
            MouseDown += UserListView_MouseDown;
            MouseMove += UserListView_MouseMove;
            MouseUp += UserListView_MouseUp;
            MouseWheel += UserListView_MouseWheel;
            DoubleClick += UserListView_DoubleClick;
            DragEnter += UserListView_DragEnter;
            DragDrop += UserListView_DragDrop;
        }

        private void UserListView_MouseDown(object sender, MouseEventArgs e)
        {
            // Scale
            int scaleItemHeight = (int)Math.Round(ItemHeight * ScaleValue);
            int scaleItemVerticalDistance = (int)Math.Round(ItemVerticalDistance * ScaleValue);
            int scaleColumnHeight = (int)Math.Round(ColumnHeight * ScaleValue);

            int _itemHeightBlock = scaleItemHeight + scaleItemVerticalDistance;
            int newIndex = (e.Y + ScrollTop - scaleColumnHeight) / _itemHeightBlock;
            if (SelectedItemIndexList.Count <= 1)
            {
                SelectedItemIndex = Math.Max(newIndex, 0);
            }
            else
            {
                _SelectedItemIndex = newIndex;
                Draw();
            }

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
                    if (0 <= MouseDownIndex && MouseDownIndex < Items.Count)
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
        }

        private void UserListView_MouseUp(object sender, MouseEventArgs e)
        {
            // Scale
            int scaleItemHeight = (int)Math.Round(ItemHeight * ScaleValue);
            int scaleItemVerticalDistance = (int)Math.Round(ItemVerticalDistance * ScaleValue);
            int scaleColumnHeight = (int)Math.Round(ColumnHeight * ScaleValue);

            int _itemHeightBlock = scaleItemHeight + scaleItemVerticalDistance;
            int newIndex = (e.Y + ScrollTop - scaleColumnHeight) / _itemHeightBlock;
            if (SelectedItemIndexList.Count > 1)
            {
                SelectedItemIndex = Math.Max(newIndex, 0);
            }

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

        private void UserListView_DoubleClick(object sender, EventArgs e)
        {
            if (SelectedItemIndex >= 0)
            {
                ItemDoubleClick?.Invoke(this, new GraphicListViewEventArgs(Items[SelectedItemIndex]));
            }
        }

        private void UserListView_DragEnter(object sender, DragEventArgs e)
        {
            Dictionary<string, object> dic = e.Data.GetData(typeof(Dictionary<string, object>)) as Dictionary<string, object>;
            if (dic != null && !dic["Sender"].Equals(sender) && dic["Items"].GetType() == typeof(UserListViewItem))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void UserListView_DragDrop(object sender, DragEventArgs e)
        {

        }

        #region Override
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (Enabled)
            {
                KeyEventArgs e = new KeyEventArgs(keyData);
                this.OnKeyDown(e);

                if (ModifierKeys == Keys.Control)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Up: _SelectedItemIndex = Math.Max(_SelectedItemIndex - 1, 0); break;
                        case Keys.Down: _SelectedItemIndex = Math.Min(_SelectedItemIndex + 1, Items.Count - 1); break;
                        case Keys.Space:
                            if (!SelectedItemIndexList.Contains(SelectedItemIndex))
                            {
                                SelectedItemIndexList.Add(SelectedItemIndex);
                            }
                            else
                            {
                                SelectedItemIndexList.Remove(SelectedItemIndex);
                            }
                            break;
                    }
                    Draw();
                }
                else
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Up: SelectedItemIndex = Math.Max(SelectedItemIndex - 1, 0); break;
                        case Keys.Down: SelectedItemIndex = Math.Min(SelectedItemIndex + 1, Items.Count - 1); break;
                        case Keys.PageUp: SelectedItemIndex = Math.Max(SelectedItemIndex - NumberOfVisibleItems, 0); break;
                        case Keys.PageDown: SelectedItemIndex = Math.Min(SelectedItemIndex + NumberOfVisibleItems, Items.Count - 1); break;
                    }
                }
            }
            // msg.Msg가 0으로 넘어오는 값이 있는지 확인 필요
            return msg.Msg == 0 ? true : base.ProcessCmdKey(ref msg, keyData);
        }

        public void OnProcessCmdKey(Keys keyData)
        {
            if (Enabled)
            {
                Message msg = new Message();
                ProcessCmdKey(ref msg, keyData);
            }
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            if (Enabled)
            {
                base.OnPreviewKeyDown(e);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (Enabled)
            {
                base.OnKeyDown(e);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (Enabled)
            {
                base.OnKeyUp(e);
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (Enabled)
            {
                base.OnKeyPress(e);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (Enabled)
            {
                base.OnMouseDown(e);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Enabled)
            {
                base.OnMouseMove(e);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (Enabled)
            {
                base.OnMouseUp(e);
            }
        }
        #endregion

        #region Function
        private double GetDistance(Point pt1, Point pt2)
        {
            var distance = Math.Sqrt((Math.Pow(pt1.X - pt2.X, 2) + Math.Pow(pt1.Y - pt2.Y, 2)));
            return distance;
        }
        #endregion
    }
}