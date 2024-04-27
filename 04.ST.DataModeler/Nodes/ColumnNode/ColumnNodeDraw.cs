using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public partial class ColumnNode
    {
        private void LoadDraw()
        {
            Paint += ColumnNode_Paint;
        }

        private void ColumnNode_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
