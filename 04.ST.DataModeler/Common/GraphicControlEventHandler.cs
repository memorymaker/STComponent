using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.DataModeler
{
    public delegate void GraphicControlEventHandler(object sender, GraphicControlEventArgs e);

    public class GraphicControlEventArgs : EventArgs
    {
        public GraphicControlEventArgs(GraphicControl control)
        {
            Control = control;
        }

        public GraphicControl Control { get; }
    }
}
