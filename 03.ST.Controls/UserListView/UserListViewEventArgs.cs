using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Controls
{
    public partial class UserListView
    {
        public delegate void ItemDeleteEventHandeler(object sender, GraphicListViewEventArgs e);

        public class GraphicListViewEventArgs : EventArgs
        {
            public UserListViewItem Item;

            public GraphicListViewEventArgs(UserListViewItem item)
            {
                Item = item;
            }
        }
    }
}
