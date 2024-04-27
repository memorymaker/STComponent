using ST.Controls;
using ST.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace ST.DataModeler
{
    public partial class DataModeler
    {
        // Ref
        private BufferedGraphics Grafx;
        private BufferedGraphicsContext Context = BufferedGraphicsManager.Current;

        // Used in SetInnerLocation, SetScaleValueNInnerLocation, Control_SizeChanged, Control_LocationChanged
        private bool AllowDrawRequest = true;
        private bool IsSuspendDraw = false;

        // Option
        public Color DisableColor = Color.FromArgb(60, 0, 0, 0);

        // Test
        private Label PerformanceTestLabel = new Label();
        private int MinMs;
        private int MaxMs;

        private Color SystemDefaultBackColor = Color.FromArgb(247, 247, 247);

        public Color DisableBackColor
        {
            get
            {
                return _DisableBackColor;
            }
            set
            {
                _DisableBackColor = value;
                Refresh();
            }
        }
        private Color _DisableBackColor = Color.Gray;

        private void LoadDraw()
        {
            Paint += DataModeler_Paint;
            SizeChanged += DataModeler_SizeChanged;
            base.Controls.Add(PerformanceTestLabel);

            // Test Code
            PerformanceTestLabel.AutoSize = false;
            PerformanceTestLabel.Location = new Point(0, 0);
            PerformanceTestLabel.Size = new Size(216, 13);
            PerformanceTestLabel.Font = new Font("맑은 고딕", 8f);
            PerformanceTestLabel.BackColor = Color.White;
            PerformanceTestLabel.Visible = true;
            PerformanceTestLabel.Click += (object sender, EventArgs e) =>
            {
                MinMs = 0;
                MaxMs = 0;
                PerformanceTestLabel.Text = string.Format(
                      "Min:{0}  Max:{1}  Cur:{2}  (Click to clear.)"
                    , MinMs.ToString().PadLeft(3), MaxMs.ToString().PadLeft(3), 0.ToString().PadLeft(3)
                );
            };
        }

        private void DataModeler_SizeChanged(object sender, EventArgs e)
        {
            if (Width > 0 && Height > 0)
            {
                Context.MaximumBuffer = new Size(Width, Height);
            }
        }

        private void DataModeler_Paint(object sender, PaintEventArgs e)
        {
            if (Width > 0 && Height > 0)
            {
                if (!IsSuspendDraw)
                {
                    // try for test
                    try
                    {
                        // Test Perfomance #1 Start
                        var msStart = DateTime.Now.Millisecond;

                        // Get Graphics - g & Clear
                        Context = new BufferedGraphicsContext();
                        Context.MaximumBuffer = Size;

                        Grafx = Context.Allocate(e.Graphics, new Rectangle(0, 0, Width, Height));
                        Graphics g = Grafx.Graphics;
                        g.Clear(BackColor);

                        // Draw Disable Width
                        if (InnerLocation.X < 0)
                        {
                            g.FillRectangle(new SolidBrush(DisableBackColor), new Rectangle(0, 0, -InnerLocation.X, Height));
                        }
                        else if (InnerSize.Width < (InnerLocation.X + Width) * (1 / ScaleValue))
                        {
                            g.FillRectangle(new SolidBrush(DisableBackColor), new Rectangle(
                                  (InnerSize.Width * ScaleValue - InnerLocation.X).ToInt()
                                , 0
                                , (InnerLocation.X + Width - InnerSize.Width * ScaleValue).ToInt()
                                , Height
                            ));
                        }

                        // Draw Disable Height
                        if (InnerLocation.Y < 0)
                        {
                            g.FillRectangle(new SolidBrush(DisableBackColor), new Rectangle(0, 0, Width , -InnerLocation.Y));
                        }
                        else if (InnerSize.Height < (InnerLocation.Y + Height) * (1 / ScaleValue))
                        {
                            g.FillRectangle(new SolidBrush(DisableBackColor), new Rectangle(
                                  0
                                , (InnerSize.Height * ScaleValue - InnerLocation.Y).ToInt()
                                , Width
                                , (InnerLocation.Y + Height - InnerSize.Height * ScaleValue).ToInt()
                            ));
                        }

                        // Draw Relations
                        DrawRelations(g);

                        // Draw Controls
                        DrawControls(g);

                        // Enabled
                        if (!Enabled)
                        {
                            g.FillRectangle(new SolidBrush(DisableColor), new Rectangle(0, 0, Width, Height));
                        }

                        // Render
                        Grafx.Render(Graphics.FromHwnd(Handle));

                        // Test Perfomance #2 End
                        int msResult = DateTime.Now.Millisecond - msStart;
                        if (msResult > 0)
                        {
                            if (msResult < MinMs || MinMs == 0) { MinMs = msResult; }
                            if (MaxMs < msResult || MaxMs == 0) { MaxMs = msResult; }

                            PerformanceTestLabel.Text = string.Format(
                                  "Min:{0}  Max:{1}  Cur:{2}  (Click to clear.)"
                                , MinMs.ToString().PadLeft(3), MaxMs.ToString().PadLeft(3), msResult.ToString().PadLeft(3)
                            );
                        }

                        // Dispose
                        g.Dispose();
                        Grafx.Dispose();
                        Context.Dispose();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"DataModeler_Paint - {ex.Message}");
                    }
                }
            }
        }

        private void DrawRelations(Graphics g)
        {
            // Draw Relations
            foreach (RelationControl relation in Relations)
            {
                relation.OnPaint(new PaintEventArgs(g, Bounds));
            }
        }

        private void DrawControls(Graphics g, bool drawAllControls = false)
        {
            for (int i = Controls.Count - 1; 0 <= i; i--)
            {
                if ((!drawAllControls && !(Controls[i].Right <= 0 || Controls[i].Bottom <= 0 || Width <= Controls[i].Left || Height <= Controls[i].Top))
                    ||
                    drawAllControls)
                {
                    DrawControl(g, Controls[i]);
                }
            }
        }

        private void DrawControl(Graphics g, GraphicControl control)
        {
            // Todo : RequestRefresh 메서드 내부처럼 Clip을 사용하지 않는
            //        객체 렌더가 더 속도가 빠른지 테스트 필요

            Rectangle controlBounds = control.GetBoundsFromTarget();
            Rectangle srcRect = new Rectangle(0, 0, controlBounds.Width, controlBounds.Height);

            // Clip #1 Start
            g.Clip = new Region(controlBounds);

            // GraphicsContainer #1 Start
            GraphicsContainer containerState = g.BeginContainer(controlBounds, srcRect, GraphicsUnit.Pixel);

            // Call Paint
            control.OnPaint(new PaintEventArgs(g, srcRect));

            // GraphicsContainer #2 End
            g.EndContainer(containerState);

            // Clip #2 End
            g.ResetClip();
        }

        public void RequestRefresh(GraphicControl control, RefreshType refreshType)
        {
            // Todo: 계속 사용할지 적용 필요
            if (AllowDrawRequest)
            {
                if (refreshType == RefreshType.All)
                {
                    SetNodeRelationDrawInfoDic(control);
                }

                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(delegate ()
                    {
                        OnPaint(new PaintEventArgs(CreateGraphics(), Bounds));
                    }));
                }
                else
                {
                    OnPaint(new PaintEventArgs(CreateGraphics(), Bounds));
                }
                

                return;

                if (refreshType == RefreshType.All)
                {
                    OnPaint(new PaintEventArgs(CreateGraphics(), Bounds));
                }
                else
                {
                    // Test Perfomance #1 Start
                    var msStart = DateTime.Now.Millisecond;

                    if (!(control.Right <= 0 || control.Bottom <= 0 || Width <= control.Left || Height <= control.Height))
                    {
                        if (0 < control.Width && 0 < control.Height)
                        {
                            Rectangle controlBounds = control.GetBoundsFromTarget();
                            Rectangle srcRect = new Rectangle(0, 0, controlBounds.Width, controlBounds.Height);

                            // Get g
                            Grafx = Context.Allocate(CreateGraphics(), controlBounds);
                            Graphics g = Grafx.Graphics;

                            // GraphicsContainer #1 Start
                            GraphicsContainer containerState = g.BeginContainer(controlBounds, srcRect, GraphicsUnit.Pixel);

                            // Call Paint
                            control.OnPaint(new PaintEventArgs(g, srcRect));

                            // GraphicsContainer #2 End
                            g.EndContainer(containerState);

                            // Render
                            Grafx.Render(Graphics.FromHwnd(Handle));
                        }
                    }

                    // Test Perfomance #2 End
                    int msResult = DateTime.Now.Millisecond - msStart;
                    if (msResult > 0)
                    {
                        if (msResult < MinMs || MinMs == 0) { MinMs = msResult; }
                        if (MaxMs < msResult || MaxMs == 0) { MaxMs = msResult; }

                        PerformanceTestLabel.Text = string.Format(
                              "Min:{0}  Max:{1}  Cur:{2}  (Click to clear.)"
                            , MinMs.ToString().PadLeft(3), MaxMs.ToString().PadLeft(3), msResult.ToString().PadLeft(3)
                        );
                    }
                }
            }
        }

        public void RequestRefresh(RelationControl control, RefreshType refreshType)
        {
            // Todo: 계속 사용할지 적용 필요
            if (AllowDrawRequest)
            {
                if (refreshType == RefreshType.All)
                {
                    SetNodeRelationDrawInfoDic();
                }

                OnPaint(new PaintEventArgs(CreateGraphics(), Bounds));
            }
        }

        new public void Refresh()
        {
            OnPaint(new PaintEventArgs(CreateGraphics(), Bounds));
        }

        public void SuspendDraw()
        {
            IsSuspendDraw = true;
        }

        public void ResumeDraw(bool draw = true)
        {
            IsSuspendDraw = false;
        }
    }
}