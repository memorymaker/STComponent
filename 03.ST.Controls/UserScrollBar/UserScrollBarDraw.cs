using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace ST.Controls
{
    public partial class UserScrollBar
    {
        // Values
        public bool BlockDrawing = false;

        // Options
        public float DisableBrightnessColorPoint = -0.1f;

        // Buttons
        private Color DecrementButtonColor = Color.FromArgb(134, 137, 153);
        private Color EncrementButtonColor = Color.FromArgb(134, 137, 153);
        private Color ScrollButtonColor = Color.FromArgb(194, 195, 201);

        // Buttons Over
        private Color DecrementButtonOverColor = Color.FromArgb(73, 113, 185);
        private Color EncrementButtonOverColor = Color.FromArgb(73, 113, 185);
        private Color ScrollButtonOverColor = Color.FromArgb(104, 104, 104);

        // Buttons MouseDown
        private Color DecrementButtonMouseDownColor = Color.FromArgb(30, 79, 151);
        private Color EncrementButtonMouseDownColor = Color.FromArgb(30, 79, 151);
        private Color ScrollButtonMouseDownColor = Color.FromArgb(91, 91, 91);

        // Buttons Disabled
        private Color DecrementButtonDisabledColor = Color.FromArgb(190, 192, 201);
        private Color EncrementButtonDisabledColor = Color.FromArgb(190, 192, 201);
        private Color ScrollButtonDisabledColor = Color.FromArgb(220, 221, 224);

        public void Draw(Color scrollBarColor = default)
        {
            if (!BlockDrawing && ClientRectangle.Width > 0 && ClientRectangle.Height > 0)
            {
                Draw(CreateGraphics(), scrollBarColor);
            }
        }

        private void Draw(Graphics graphics, Color scrollBarColor = default)
        {
            if (!BlockDrawing && ClientRectangle.Width > 0 && ClientRectangle.Height > 0)
            {
                BufferedGraphicsContext context = new BufferedGraphicsContext();
                context.MaximumBuffer = new Size(ClientRectangle.Width, ClientRectangle.Height);
                BufferedGraphics bufferedGraphics = context.Allocate(graphics, new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height));
                Graphics g = bufferedGraphics.Graphics;

                Color backColor = SystemColors.Control.GetColor(!Parent.Enabled, DisableBrightnessColorPoint);
                g.Clear(backColor);

                if (_IncreaseDecreaseButtonVisible)
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    DrawDecrementButton(g);
                    DrawEncrementButton(g);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                }
                DrawScrollButton(g, scrollBarColor);

                bufferedGraphics.Render(graphics);

                g.Dispose();
                bufferedGraphics.Dispose();
                context.Dispose();
            }
        }

        private void DrawDecrementButton(Graphics g)
        {
            // ▲ Or ◀
            // Get Brush
            Brush brush;
            if (Maximum - Minimum > 0)
            {
                if (IsMouseDown)
                {
                    brush = new SolidBrush(ActionType == MouseActionType.DecrementButton
                        ? DecrementButtonMouseDownColor
                        : DecrementButtonColor
                    );
                }
                else
                {
                    Point point = PointToClient(Cursor.Position);
                    brush = new SolidBrush(DecrementButtonRectangle.Contains(point)
                        ? DecrementButtonOverColor
                        : DecrementButtonColor
                    );
                }
            }
            else
            {
                brush = new SolidBrush(DecrementButtonDisabledColor);
            }

            // Get Points
            Point[] points = null;
            // ▲
            if (Type == UserScrollBarType.Vertical)
            {
                int horizonCenter = DecrementButtonRectangle.Left + (int)Math.Floor(DecrementButtonRectangle.Width * 0.5f);
                int horizonMargin = (int)Math.Round(EncrementButtonRectangle.Width * 0.3f);
                points = new Point[] {
                      new Point(horizonCenter, DecrementButtonRectangle.Top + horizonMargin)
                    , new Point(horizonCenter + horizonMargin, DecrementButtonRectangle.Top + horizonMargin * 2)
                    , new Point(horizonCenter - horizonMargin, DecrementButtonRectangle.Top + horizonMargin * 2)
                };
            }
            // ◀
            else if (Type == UserScrollBarType.Horizontal)
            {
                int verticalCenter = DecrementButtonRectangle.Top + (int)Math.Floor(DecrementButtonRectangle.Height * 0.5f);
                int verticalMargin = (int)Math.Round(EncrementButtonRectangle.Height * 0.3f);
                points = new Point[] {
                      new Point(DecrementButtonRectangle.Left + verticalMargin, verticalCenter)
                    , new Point(DecrementButtonRectangle.Left + verticalMargin * 2, verticalCenter + verticalMargin)
                    , new Point(DecrementButtonRectangle.Left + verticalMargin * 2, verticalCenter - verticalMargin)
                };
            }

            g.FillPolygon(brush, points);
        }

        private void DrawEncrementButton(Graphics g)
        {
            // ▼ Or ▶
            // Get Brush
            Brush brush;
            if (Maximum - Minimum > 0)
            {
                if (IsMouseDown)
                {
                    brush = new SolidBrush(ActionType == MouseActionType.EncrementButton
                        ? EncrementButtonMouseDownColor
                        : EncrementButtonColor
                    );
                }
                else
                {
                    Point point = PointToClient(Cursor.Position);
                    brush = new SolidBrush(EncrementButtonRectangle.Contains(point)
                        ? EncrementButtonOverColor
                        : EncrementButtonColor
                    );
                }
            }
            else
            {
                brush = new SolidBrush(EncrementButtonDisabledColor);
            }

            // Get Points
            Point[] points = null;
            // ▼
            if (Type == UserScrollBarType.Vertical)
            {
                int horizonCenter = EncrementButtonRectangle.Left + (int)Math.Floor(EncrementButtonRectangle.Width * 0.5f);
                int horizonMargin = (int)Math.Round(EncrementButtonRectangle.Width * 0.3f);
                points = new Point[] {
                    new Point(horizonCenter, EncrementButtonRectangle.Bottom - horizonMargin)
                    , new Point(horizonCenter + horizonMargin, EncrementButtonRectangle.Bottom - horizonMargin * 2)
                    , new Point(horizonCenter - horizonMargin, EncrementButtonRectangle.Bottom - horizonMargin * 2)
                };
            }
            // ▶
            else if (Type == UserScrollBarType.Horizontal)
            {
                int horizonCenter = EncrementButtonRectangle.Top + (int)Math.Floor(EncrementButtonRectangle.Height * 0.5f);
                int horizonMargin = (int)Math.Round(EncrementButtonRectangle.Height * 0.3f);
                points = new Point[] {
                    new Point(EncrementButtonRectangle.Right - horizonMargin, horizonCenter)
                    , new Point(EncrementButtonRectangle.Right - horizonMargin * 2, horizonCenter + horizonMargin)
                    , new Point(EncrementButtonRectangle.Right - horizonMargin * 2, horizonCenter - horizonMargin)
                };
            }

            g.FillPolygon(brush, points);
        }

        private void DrawScrollButton(Graphics g, Color scrollBarColor = default)
        {
            // ■
            if (Maximum - Minimum > 0)
            {
                // Get Brush
                Brush brush;
                if (IsMouseDown)
                {
                    brush = new SolidBrush(ActionType == MouseActionType.ScrollButton
                        ? ScrollButtonMouseDownColor
                        : ScrollButtonColor
                    );
                }
                else
                {
                    if (scrollBarColor != default)
                    {
                        brush = new SolidBrush(scrollBarColor);
                    }
                    else
                    {
                        Point point = PointToClient(Cursor.Position);
                        brush = new SolidBrush(ScrollButtonRectangle.Contains(point)
                            ? ScrollButtonOverColor
                            : ScrollButtonColor
                        );

                    }
                }

                if (Type == UserScrollBarType.Vertical)
                {
                    int horizontalMargin = (int)Math.Round(ScrollButtonRectangle.Width * 0.2);
                    g.FillRectangle(brush, new Rectangle(
                          ScrollButtonRectangle.X + horizontalMargin, ScrollButtonRectangle.Y
                        , ScrollButtonRectangle.Width - horizontalMargin * 2, ScrollButtonRectangle.Height));
                }
                else if (Type == UserScrollBarType.Horizontal)
                {
                    int verticalMargin = (int)Math.Round(ScrollButtonRectangle.Height * 0.2);
                    g.FillRectangle(brush, new Rectangle(
                          ScrollButtonRectangle.X, ScrollButtonRectangle.Y + verticalMargin
                        , ScrollButtonRectangle.Width, ScrollButtonRectangle.Height - verticalMargin * 2));
                }
            }
        }

        new public void SuspendLayout()
        {
            BlockDrawing = true;
        }

        new public void ResumeLayout()
        {
            BlockDrawing = false;
        }
    }
}
