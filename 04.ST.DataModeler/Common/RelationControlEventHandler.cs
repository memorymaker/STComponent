using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.DataModeler
{
    public delegate void RelationControlEventHandler(object sender, RelationControlEventArgs e);

    public class RelationControlEventArgs : EventArgs
    {
        public RelationControlEventArgs(RelationControl control)
        {
            Control = control;
        }

        public RelationControl Control { get; }
    }
}
