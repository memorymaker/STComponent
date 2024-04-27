using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using ST.Core;
using ST.Controls;
using System.Collections.ObjectModel;

namespace Common
{
    public partial class UserTableNode : UserNodeBase
    {
        #region Values
        // new public string SEQ = "0";
        // new public int SEQ = 0;
        #endregion

        #region UserEventHandler, DragDropItems, MouseUp, GetSet
        public event UserEventHandler DragDropItems;
        public int ListHeight
        { 
            get
            {
                return (int)Math.Ceiling((double)lst.Font.Height) + 3;
            }
        }
        #endregion

        #region Load
        public UserTableNode()
        {
            InitializeComponent();
            this.LoadThis();
        }
        
        private void LoadThis()
        {
            this.SetDefault();
            this.SetEvent();
        }

        private void SetDefault()
        {
            // MouseAction
            MouseAction.UseSizing = false;

            this.BackColor = Color.FromArgb(230, 121, 47);

            lst.Dock = DockStyle.Fill;
            lst.Font = this.Font;
            lst.Columns.Add(new UserListViewColumn("FieldInfo", "FieldInfo"));
            lst.Columns.Add(new UserListViewColumn("Comment", "Comment"));

            // UserPanelControl
            BtDelete.Enabled = false; 
            EnableCaptionEdit = false;
        }
        #endregion

        #region Public
        public UserListViewItem GetItemFromTag(string tagKey, string value)
        {
            foreach (UserListViewItem item in this.Items)
            {
                if (item.Tag.GetType() == typeof(Dictionary<string, string>))
                {
                    Dictionary<string, string> dic = (Dictionary<string, string>)item.Tag;
                    if (dic[tagKey] == value)
                    {
                        return item;
                    }
                }
            }
            return null;
        }
        #endregion

        #region Events
        private void SetEvent()
        {
            lst.Resize += Lst_Resize;
            lst.ItemDrag += Lst_ItemDrag;
            lst.DragOver += Lst_DragOver;
            lst.DragDrop += Lst_DragDrop;
            lst.KeyDown += Lst_KeyDown;
            lst.MouseDown += Lst_MouseDown;
        }

        private void Lst_MouseDown(object sender, MouseEventArgs e)
        {
            this.BringToFront();
        }

        private void Lst_KeyDown(object sender, KeyEventArgs e)
        {
            UserListView  _this = (UserListView)sender;
            // Ctrn + C
            if (e.Control == true && e.KeyCode == Keys.C)
            {
                if (_this.SelectedItems.Count > 0)
                {
                    List<string> items = new List<string>();
                    foreach (UserListViewItem item in _this.SelectedItems)
                    {
                        Dictionary<string, string> dic = (Dictionary<string, string>)item.Tag;
                        items.Add(dic["COLUMN_ID"].ToString());
                    }
                    Clipboard.SetText(String.Join("\r\n", items));
                }
            }
        }

        public ObservableCollection<UserListViewItem> Items
        {
            get
            {
                return lst.Items;
            }
        }
        
        public UserListView ListView
        {
            get
            {
                return this.lst;
            }
        }

        public string TABLE_ID { get; set; }

        public Rectangle GetColumnRectangle(string columnName)
        {
            foreach(UserListViewItem item in lst.Items)
            {
                if (item.Text == columnName)
                {
                    Rectangle rs = new Rectangle(
                        this.Bounds.Left + item.Bounds.Left + lst.Left
                        , this.Bounds.Top + item.Bounds.Top + lst.Top
                        , item.Bounds.Width
                        , item.Bounds.Height
                    );
                    return rs;
                }
            }
            return new Rectangle();
        }

        public Rectangle GetColumnRectangle(ListViewItem item)
        {
            Rectangle rs = new Rectangle(
                this.Bounds.Left + item.Bounds.Left + lst.Left
                , this.Bounds.Top + item.Bounds.Top + lst.Top
                , item.Bounds.Width
                , item.Bounds.Height
            );
            return rs;
        }
        
        private void Lst_DragOver(object sender, DragEventArgs e)
        {
            UserListView _this = (UserListView)sender;
            Dictionary<string, object> dic = e.Data.GetData(typeof(Dictionary<string, object>)) as Dictionary<string, object>;
            if (dic != null)
            {
                if (!_this.Focused) _this.Focus();

                if (!dic["Sender"].Equals(sender))
                {
                    UserListViewItem target = this.GetItem(lst, e.X, e.Y);

                    if (dic["Items"].GetType() == typeof(List<UserListViewItem>) && target != null)
                    {
                        int recive_items_cnt = ((List<UserListViewItem>)dic["Items"]).Count;
                        for (int i = 0; i < _this.Items.Count; i++)
                        {
                            if (target.Index <= i && i <= target.Index + (recive_items_cnt - 1))
                            {
                                _this.Items[i].Selected = true;
                            }
                            else
                            {
                                _this.Items[i].Selected = false;
                            }
                        }
                        e.Effect = DragDropEffects.Copy;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
            }

        }

        private void Lst_DragDrop(object sender, DragEventArgs e)
        {
            Dictionary<string, object> dic = e.Data.GetData(typeof(Dictionary<string, object>)) as Dictionary<string, object>;
            if (dic["Items"].GetType() == typeof(List<UserListViewItem>))
            {
                List<UserListViewItem> items = dic["Items"] as List<UserListViewItem>;

                // Set Event Args
                UserEventArgs ue = new UserEventArgs(new Dictionary<string, object>
                {
                    {"TableName1", dic["TableName"] }
                    ,{"Items1", dic["Items"] }
                    ,{"TableName2", this.TABLE_ID }
                    ,{"Items2", ((UserListView)sender).SelectedItems }
                });

                // Call Event
                if (DragDropItems != null)
                {
                    this.DragDropItems(sender, ue);
                }
            }
        }

        private UserListViewItem GetItem(UserListView lst, int x, int y)
        {
            Rectangle lstScreenRectangle = RectangleToScreen(lst.ClientRectangle);
            x -= lst.Left + lstScreenRectangle.Left;
            y -= lst.Top + lstScreenRectangle.Top;

            foreach(UserListViewItem item in lst.Items)
            {
                if (item.Bounds.X <= x && x <= item.Bounds.X + item.Bounds.Width
                    && item.Bounds.Y <= y && y <= item.Bounds.Y + item.Bounds.Height)
                {
                    return item;
                }
            }
            return null;
        }

        private void Lst_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(new Dictionary<string, object>()
            {
                {"TableName", this.TABLE_ID }
                ,{"Sender", sender }
                ,{"Items", ((UserListView)sender).SelectedItems }
            }, DragDropEffects.Copy);
        }
        
        private void Lst_Resize(object sender, EventArgs e)
        {
            UserListView _this = (UserListView)sender;
            
            if (lst.Height < lst.Items.Count * this.ListHeight)
            {
                _this.Columns[0].Width = _this.Width - SystemInformation.VerticalScrollBarWidth;
            }
            else
            {
                _this.Columns[0].Width = _this.Width;
            }
        }
        #endregion

        #region IScaleControl
        override public Color MinimapColor { get; set; } = Color.FromArgb(230, 121, 47);
        #endregion
    }
}
