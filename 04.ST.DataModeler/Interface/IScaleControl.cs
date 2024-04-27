using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.DataModeler
{
    public interface IScaleControl
    {
        float ScaleValue { get; set; }

        int Width { get; set; }

        int Height { get; set; }

        int Left { get; set; }

        int Top { get; set; }

        int OriginalWidth  { get; set; }

        int OriginalHeight { get; set; }

        int OriginalLeft { get; set; }

        int OriginalTop { get; set; }

        Color MinimapColor { get; set; }

        void SetScaleValueNMovePoint(float newScaleValue, Point newLocation);
    }

    public delegate void IScaleControlScaleValueEventHandler(object sender, IScaleControlScaleValueChangedEventArgs e);

    public class IScaleControlScaleValueChangedEventArgs : EventArgs
    {
        public float OldScaleValue { get; set; }
        public float ScaleValue { get; set; }
    }
}
