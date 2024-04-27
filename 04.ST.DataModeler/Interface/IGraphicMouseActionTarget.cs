using System;
using System.Drawing;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public interface IGraphicMouseActionTarget
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

        IGraphicControlParent Parent { get; set; }

        DataModeler Target { get; }

        event MouseEventHandler MouseDown;

        event MouseEventHandler MouseMove;

        event MouseEventHandler MouseUp;

        event EventHandler MouseLeave;
        #endregion
    }
}