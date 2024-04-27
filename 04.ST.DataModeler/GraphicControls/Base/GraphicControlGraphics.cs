using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace ST.DataModeler
{
    public class GraphicControlGraphics : IDisposable
    {
        private GraphicControl GraphicControl;
        private Graphics TargetGraphics;

        public GraphicControlGraphics(GraphicControl graphicControl)
        {
            if (graphicControl == null)
            {
                throw new ArgumentNullException("graphicControl is null.");
            }
            GraphicControl = graphicControl;
            TargetGraphics = GraphicControl.Target.CreateGraphics();
        }

        public void DrawImage(Image image, int x, int y)
        {
            Point drawLocation = GraphicControl.GetLocationFromTarget();
            drawLocation.Offset(x, y);

            TargetGraphics.DrawImage(image, drawLocation);
        }

        public void DrawImage(Image image)
        {
            DrawImage(image, 0, 0);
        }

        public SizeF MeasureString(string text, Font font)
        {
            return TargetGraphics.MeasureString(text, font);
        }

        public void Dispose()
        {
            TargetGraphics.Dispose();
        }
    }
}
