using ST.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.Controls.Modal
{
    public class ModalButton : Button
    {
        public ModalButton()
        {
            Size = new Size(80, 26);
            Padding = new Padding(0);
            Margin = new Padding(3, 4, 3, 4);
            UseVisualStyleBackColor = false;
            FlatStyle = FlatStyle.Flat;

            BackColor = Color.FromArgb(52, 122, 182);
            Font = new Font("맑은 고딕", 9f);
            ForeColor = Color.FromArgb(255, 255, 255);

            GotFocus += ModalButton_GotFocus;
            LostFocus += ModalButton_LostFocus;
        }

        private void ModalButton_LostFocus(object sender, EventArgs e)
        {
            using (Graphics g = CreateGraphics())
            {
                g.FillRectangle(new SolidBrush(Color.Red), 0, 0, 0, 0);

                g.DrawRectangle(new Pen(Color.Red), 1, 1, Width, Height);
            }
        }

        private void ModalButton_GotFocus(object sender, EventArgs e)
        {
            using (Graphics g = CreateGraphics())
            {
                g.FillRectangle(new SolidBrush(Color.Red), 0, 0, 0, 0);

                g.DrawRectangle(new Pen(Color.Red), 1, 1, Width, Height);
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Point point = Parent.PointToClient(Cursor.Position);

            // Draw Back
            float colorBrightnessRevision = Bounds.Contains(point) ? 0.1f : 0f;
            if (Control.MouseButtons == MouseButtons.Left)
            {
                colorBrightnessRevision += 0.05f;
            }
            pevent.Graphics.FillRectangle(new SolidBrush(BackColor.GetColor(colorBrightnessRevision)), new Rectangle(0, 0, Width, Height));

            // Draw Focus Line
            if (Focused)
            {
                pevent.Graphics.DrawRectangle(new Pen(ForeColor), new Rectangle(2, 2, Width - 5, Height - 5));
            }

            SizeF size = pevent.Graphics.MeasureString(Text, Font);
            int x = 0, y = 0;

            // Get x
            if (TextAlign == ContentAlignment.TopLeft || TextAlign == ContentAlignment.MiddleLeft || TextAlign == ContentAlignment.BottomLeft)
            {
                x = Padding.Left;
            }
            else if (TextAlign == ContentAlignment.TopCenter || TextAlign == ContentAlignment.MiddleCenter || TextAlign == ContentAlignment.BottomCenter)
            {
                x = (Width - Padding.Horizontal - size.Width.ToInt()) / 2 + Padding.Left;
            }
            else if (TextAlign == ContentAlignment.TopRight || TextAlign == ContentAlignment.MiddleRight || TextAlign == ContentAlignment.BottomRight)
            {
                x = Width - size.Width.ToInt() - Padding.Right;
            }

            // Get y
            if (TextAlign == ContentAlignment.TopLeft || TextAlign == ContentAlignment.TopCenter || TextAlign == ContentAlignment.TopRight)
            {
                y = Padding.Top;
            }
            else if (TextAlign == ContentAlignment.MiddleLeft || TextAlign == ContentAlignment.MiddleCenter || TextAlign == ContentAlignment.MiddleRight)
            {
                y = (Height - Padding.Vertical - size.Height.ToInt()) / 2 + Padding.Top;
            }
            else if (TextAlign == ContentAlignment.BottomLeft || TextAlign == ContentAlignment.BottomCenter || TextAlign == ContentAlignment.BottomRight)
            {
                y = Height - size.Height.ToInt() - Padding.Bottom;
            }

            // Draw Text
            pevent.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), x, y);
        }
    }
}
