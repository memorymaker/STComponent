using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.Controls
{
    public partial class UserScaleControlWarpPanelMinimap
    {
        public class MinimapPanel : Panel
        {
            public UserScaleControlWarpPanel Target;

            // Option
            public PositionType Position = PositionType.BottomRight;
            new public Color DefaultBackColor = Color.FromArgb(232, 232, 234);
            public Color DisplayAreaBackColor = Color.FromArgb(255, 255, 255);
            public Color DisplayAreaBorderColor = Color.FromArgb(90, 0, 0, 0);

            public MinimapPanel(UserScaleControlWarpPanel target)
            {
                SetThis();
                SetTarget(target);
            }

            private void SetThis()
            {
                BackColor = DefaultBackColor;
                Paint += (object sender, PaintEventArgs e) =>
                {
                    Draw(e.Graphics);
                };

                MouseDown += MinimapPanel_MouseDownNMove;
                MouseMove += MinimapPanel_MouseDownNMove;
            }

            private void MinimapPanel_MouseDownNMove(object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                {
                    int x = e.X, y = e.Y;
                    Size targetInnerSize = Target.InnerSize;

                    // Set xPercent, yPercent
                    float xPercent = (float)targetInnerSize.Width / Width;
                    float yPercent = (float)targetInnerSize.Height / Height;

                    // Get Display Area Size
                    Size displayAreaSize = new Size(
                          Convert.ToInt32(Target.Width * (1 / Target.ScaleValue) * ((float)Width / targetInnerSize.Width)) - 1
                        , Convert.ToInt32(Target.Height * (1 / Target.ScaleValue) * ((float)Height / targetInnerSize.Height)) - 1
                    );

                    // Revise x, y
                    x -= displayAreaSize.Width / 2;
                    y -= displayAreaSize.Height / 2;

                    // Get newInnerLocation
                    Point newInnerLocation = new Point(
                          Convert.ToInt32(x * xPercent * Target.ScaleValue)
                        , Convert.ToInt32(y * yPercent * Target.ScaleValue)
                    );
                    newInnerLocation.X = Convert.ToInt32(Math.Min(Math.Max(newInnerLocation.X, 0), (targetInnerSize.Width - Target.Width * (1 / Target.ScaleValue)) * Target.ScaleValue));
                    newInnerLocation.Y = Convert.ToInt32(Math.Min(Math.Max(newInnerLocation.Y, 0), (targetInnerSize.Height - Target.Height * (1 / Target.ScaleValue)) * Target.ScaleValue));

                    // Set Target
                    Target.SetInnerLocation(newInnerLocation);
                }
            }

            private void SetTarget(UserScaleControlWarpPanel target)
            {
                Target = target;

                Target.SizeChanged += (object sender, EventArgs e) =>
                {
                    int x = 0;
                    if (Position == PositionType.Right || Position == PositionType.TopRight || Position == PositionType.BottomRight)
                    {
                        x = Target.Width - Width;
                    }

                    int y = 0;
                    if (Position == PositionType.Bottom || Position == PositionType.BottomRight || Position == PositionType.BottomLeft)
                    {
                        y = Target.Height - Height;
                    }

                    Location = new Point(x, y);
                };
            }

            public void Draw(Graphics g = null)
            {
                if (Target != null)
                {
                    if (g == null)
                    {
                        g = CreateGraphics();
                    }

                    // Set bitmapGraphics
                    var bitmap = new Bitmap(Width, Height);
                    Graphics bitmapGraphics = Graphics.FromImage(bitmap);
                    bitmapGraphics.Clear(BackColor);

                    // Set targetInnerSize, targetInnerLocation
                    Size targetInnerSize = Target.InnerSize;
                    Point targetInnerLocation = Target.InnerLocation;

                    // Set xPercent, yPercent / -1: Revision for out size
                    float xPercent = (float)(Width - 1) / targetInnerSize.Width;
                    float yPercent = (float)(Height - 1) / targetInnerSize.Height;

                    // Draw Display Area Back
                    Rectangle displayAreaRectange = new Rectangle(
                          Convert.ToInt32(targetInnerLocation.X * (1 / Target.ScaleValue) * xPercent)
                        , Convert.ToInt32(targetInnerLocation.Y * (1 / Target.ScaleValue) * yPercent)
                        , Convert.ToInt32(Target.Width * (1 / Target.ScaleValue) * xPercent)
                        , Convert.ToInt32(Target.Height * (1 / Target.ScaleValue) * yPercent)
                    );
                    bitmapGraphics.FillRectangle(new SolidBrush(DisplayAreaBackColor), displayAreaRectange);

                    // Draw Controls
                    foreach (Control control in Target.Controls)
                    {
                        // Zindex 순서로 수정 필요
                        IScaleControl scaleControl = control as IScaleControl;
                        if (scaleControl != null)
                        {
                            Size size = new Size(
                                  Convert.ToInt32(scaleControl.Width * (1 / scaleControl.ScaleValue) * xPercent)
                                , Convert.ToInt32(scaleControl.Height * (1 / scaleControl.ScaleValue) * yPercent)
                            );

                            Point location = new Point(
                                  Convert.ToInt32(((control.Left + targetInnerLocation.X) * (1 / scaleControl.ScaleValue)) * xPercent)
                                , Convert.ToInt32(((control.Top + targetInnerLocation.Y) * (1 / scaleControl.ScaleValue)) * yPercent)
                            );

                            // 뒤에 숨겨진오브젝트 표시 되도록 사용하지 않음?
                            // bitmapGraphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(location, size));
                            bitmapGraphics.DrawRectangle(new Pen(scaleControl.MinimapColor, 1), new Rectangle(location, size));
                        }
                    }

                    // Draw Display Area Border
                    Pen displayAreaBorderPan = new Pen(DisplayAreaBorderColor);
                    displayAreaBorderPan.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    bitmapGraphics.DrawRectangle(displayAreaBorderPan, displayAreaRectange);

                    // Draw
                    g.DrawImage(bitmap, 0, 0);
                    bitmapGraphics.Dispose();
                    bitmap.Dispose();
                }
            }

            public enum PositionType
            {
                Top = 1, TopRight, Right, BottomRight, Bottom, BottomLeft, Left, TopLeft
            }
        }
    }
}