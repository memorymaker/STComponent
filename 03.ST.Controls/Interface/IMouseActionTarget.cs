using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace ST.Controls
{
    public interface IMouseActionTarget
    {
        #region This
        int TitleHeight { get; }
        #endregion

        #region Control
        int Width { get; set; }

        int Height { get; set; }

        int Left { get; set; }

        int Top { get; set; }

        int Right { get; }

        int Bottom { get; }

        bool Visible { get; set; }

        Rectangle Bounds { get; set; }

        Point Location { get; set; }

        Cursor Cursor { get; set; }

        Control Parent { get; set; }

        event MouseEventHandler MouseDown;

        event MouseEventHandler MouseMove;

        event MouseEventHandler MouseUp;

        event EventHandler MouseLeave;
        #endregion
    }
}