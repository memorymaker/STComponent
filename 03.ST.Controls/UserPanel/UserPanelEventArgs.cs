using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Controls
{
    public delegate void UserPanelClosingEventHandler(object sender, UserPanelClosingEventArgs e);

    public class UserPanelClosingEventArgs : EventArgs
    {
        public bool Cancel { get; set; } = false;

        public UserPanelClosingEventArgs()
        {
        }
    }

    public delegate void UserPanelShownEventHandler(object sender, EventArgs e);

    //public delegate void UserPanelTitleChangedEventHandler(object sender, UserPanelTitleChangedArgs e);

    //public class UserPanelTitleChangedArgs : EventArgs
    //{
    //    public UserPanel UserPanel;

    //    public UserPanelTitleChangedArgs()
    //    {
    //    }
    //}
}
