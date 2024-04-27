using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.CodeGenerator
{
    public delegate void TabEventEventHandler(object sender, TabEventArgs e);

    public class TabEventArgs : EventArgs
    {
        public Tab Tab { get; set; }
    }
}
