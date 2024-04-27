using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public partial class DataModeler : IGraphicControlParent
    {
        public new GraphicControlCollection Controls => _Controls;
        private GraphicControlCollection _Controls;

        public new event GraphicControlEventHandler ControlAdded;
        public new event GraphicControlEventHandler ControlRemoved;

        private void LoadIGraphicControlParent()
        {
            _Controls = new GraphicControlCollection(this);
        }

        public void OnControlAdded(GraphicControlEventArgs e)
        {
            ControlAdded?.Invoke(this, e);
        }

        public void OnControlRemoved(GraphicControlEventArgs e)
        {
            ControlRemoved?.Invoke(this, e);
        }
    }
}