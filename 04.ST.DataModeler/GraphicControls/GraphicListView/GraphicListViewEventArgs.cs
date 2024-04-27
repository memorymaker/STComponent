using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.DataModeler
{
    public partial class GraphicListView
    {
        public delegate void ItemDeleteEventHandeler(object sender, GraphicListViewEventArgs e);

        public class GraphicListViewEventArgs : EventArgs
        {
            public GraphicListViewItem Item;

            public GraphicListViewEventArgs(GraphicListViewItem item)
            {
                Item = item;
            }
        }
    }
}
