using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ST.DataModeler
{
    public partial class GraphicPanel
    {
        public delegate void TitleChangingEventHandeler(object sender, TitleChangingEventArgs e);

        public class TitleChangingEventArgs : EventArgs
        {
            public string OldTitle;
            public string NewTitle;
            public bool Cancel = false;

            public TitleChangingEventArgs(string oldTitle, string newTitle)
            {
                OldTitle = oldTitle;
                NewTitle = newTitle;
            }
        }
    }
}
