using ST.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ST.CodeGenerator
{
    public partial class CodeGenerator : IGraphicControlParent
    {
        public void Redraw()
        {
            OnPaint(new System.Windows.Forms.PaintEventArgs(CreateGraphics()
                , new System.Drawing.Rectangle(0, 0, Width, Height)));
        }
    }
}
