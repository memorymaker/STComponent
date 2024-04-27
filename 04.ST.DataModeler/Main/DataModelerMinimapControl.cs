using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public partial class DataModeler
    {
        public class MinimapControl : Control
        {
            public DataModeler Target;

            // Option
            public PositionType Position = PositionType.BottomRight;
            new public Color DefaultBackColor = Color.FromArgb(242, 242, 242);
            //new public Color DefaultBackColor = Color.FromArgb(232, 232, 234);
            public Color DisplayAreaBackColor = Color.FromArgb(255, 255, 255);
            public Color DisplayAreaBorderColor = Color.FromArgb(90, 0, 0, 0);
            public Color BorderColor = Color.FromArgb(160, 160, 160);
            public Color DisableColor = Color.FromArgb(60, 0, 0, 0);

            public MinimapControl(DataModeler target)
            {
                SetThis();
                SetTarget(target);
            }

            private void SetThis()
            {
                // Default
                BackColor = DefaultBackColor;
                
                // Events
                MouseDown += MinimapControl_MouseDownNMove;
                MouseMove += MinimapControl_MouseDownNMove;
                Paint += MinimapControl_Paint;
            }

            private void MinimapControl_MouseDownNMove(object sender, MouseEventArgs e)
            {
                if (Target.Enabled && !Target.ReadOnly)
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
            }

            private void MinimapControl_Paint(object sender, PaintEventArgs e)
            {
                Draw(e.Graphics);
            }

            private void SetTarget(DataModeler target)
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

            public void Draw()
            {
                Draw(CreateGraphics());
            }

            private void Draw(Graphics graphics)
            {
                if (Target != null)
                {
                    // Initialize
                    BufferedGraphicsContext context = new BufferedGraphicsContext();
                    context.MaximumBuffer = Size;
                    BufferedGraphics bufferedGraphics = context.Allocate(graphics, new Rectangle(0, 0, Width, Height));
                    Graphics g = bufferedGraphics.Graphics;
                    g.Clear(BackColor);

                    // Set targetInnerSize, targetInnerLocation
                    Size targetInnerSize = Target.InnerSize;
                    Point targetInnerLocation = Target.InnerLocation;

                    // Set xPercent, yPercent / -1: Revision for out size
                    float xPercent = (float)(Width - 2) / targetInnerSize.Width;
                    float yPercent = (float)(Height - 2) / targetInnerSize.Height;

                    // Draw Display Area Back
                    Rectangle displayAreaRectange = new Rectangle(
                          Convert.ToInt32(targetInnerLocation.X * (1 / Target.ScaleValue) * xPercent) + 1
                        , Convert.ToInt32(targetInnerLocation.Y * (1 / Target.ScaleValue) * yPercent) + 1
                        , Convert.ToInt32(Target.Width * (1 / Target.ScaleValue) * xPercent)
                        , Convert.ToInt32(Target.Height * (1 / Target.ScaleValue) * yPercent)
                    );
                    g.FillRectangle(new SolidBrush(DisplayAreaBackColor), displayAreaRectange);

                    // Draw Controls
                    foreach (GraphicControl control in Target.Controls)
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
                                  Convert.ToInt32(((control.Left + targetInnerLocation.X) * (1 / scaleControl.ScaleValue)) * xPercent) + 1
                                , Convert.ToInt32(((control.Top + targetInnerLocation.Y) * (1 / scaleControl.ScaleValue)) * yPercent) + 1
                            );

                            // 뒤에 숨겨진오브젝트 표시 되도록 사용하지 않음?
                            // bitmapGraphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(location, size));
                            Pen pen = new Pen(scaleControl.MinimapColor, 1);
                            g.DrawRectangle(new Pen(scaleControl.MinimapColor, 1), new Rectangle(location, size));
                            g.DrawLine(pen, location.X, location.Y + 1, size.Width + location.X, location.Y + 1);
                        }
                    }

                    // Draw Display Area Border
                    Pen displayAreaBorderPen = new Pen(DisplayAreaBorderColor);
                    displayAreaBorderPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    g.DrawRectangle(displayAreaBorderPen, displayAreaRectange);

                    // Draw Border(Left, Top)
                    Pen borderPen = new Pen(BorderColor);
                    g.DrawLine(borderPen, 0, 0, Width, 0);
                    g.DrawLine(borderPen, 0, 0, 0, Height);

                    // Enabled
                    if (!Target.Enabled)
                    {
                        g.FillRectangle(new SolidBrush(DisableColor), new Rectangle(0, 0, Width, Height));
                    }

                    // Draw
                    bufferedGraphics.Render(Graphics.FromHwnd(Handle));

                    // Dispose
                    g.Dispose();
                    bufferedGraphics.Dispose();
                    context.Dispose();
                }
            }

            public enum PositionType
            {
                Top = 1, TopRight, Right, BottomRight, Bottom, BottomLeft, Left, TopLeft
            }
        }
    }
}