using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.Controls
{
    public partial class UserScaleControlWarpPanel : Panel
    {
        #region Values, Propertise
        // Option
        public Size InnerSize = new Size(4000, 3000);
        public Point InnerLocation = Point.Empty;
        private Point MouseDownInnerLocation= Point.Empty;

        // Ref
        private Point MouseDownPoint = Point.Empty;

        // Controls
        // ---- StatusPanel
        private Panel StatusPanel = new Panel();
        // ---- ---- Options
        private int StatusPanelHeight = 20;
        private Point StatusPanelStringPosition = new Point(2, 2);
        private Font StatusPanelFont = new Font("맑은 고딕", 9F);
        private Brush StatusPanelBrush = new SolidBrush(Color.FromArgb(60, 60, 60));
        private Color StatusPanelBackColor = Color.White;
        // ---- ---- Ref

        // ---- Minimap
        private UserScaleControlWarpPanelMinimap.MinimapPanel Minimap = null;
        // ---- ----
        private Size MinimapSize = new Size(200, 150);

        // System Optoins
        private float MaximumScaleValue = 2f;
        private float MinimumScaleValue = 0.2f;
        private float ScaleValueUnit = 0.1f;

        // Properties
        #endregion

        public float ScaleValue
        {
            get
            {
                return _ScaleValue;
            }
            set
            {
                float newValue = Math.Min(Math.Max(value, MinimumScaleValue), MaximumScaleValue);

                if (_ScaleValue != newValue)
                {
                    _ScaleValue = newValue;

                    foreach (Control control in Controls)
                    {
                        IScaleControl scaleControl = control as IScaleControl;
                        if (scaleControl != null) {
                            scaleControl.ScaleValue = _ScaleValue;
                        }
                    }
                }
            }
        }
        private float _ScaleValue = 1f;

        #region Load
        public UserScaleControlWarpPanel()
        {
            SetThis();
            SetControls();
            SetEvents();
        }

        private void SetThis()
        {
            // SetDoubleBuffering(this, true);
        }
        #endregion

        #region Controls
        private void SetControls()
        {
            SetStatusPanel();
            SetMinimap();
        }

        private void SetStatusPanel()
        {
            Controls.Add(StatusPanel);
            StatusPanel.Dock = DockStyle.Bottom;
            StatusPanel.Height = StatusPanelHeight;
            StatusPanel.Visible = true;
            StatusPanel.Paint += (object sender, PaintEventArgs e) =>
            {
                DrawStatusPanel(e.Graphics);
            };
            StatusPanel.BackColor = StatusPanelBackColor;
        }

        private void SetMinimap()
        {
            Minimap = new UserScaleControlWarpPanelMinimap.MinimapPanel(this);
            Minimap.Size = MinimapSize;

            Controls.Add(Minimap);
            Minimap.BringToFront();
        }
        #endregion

        #region This Events
        private void SetEvents()
        {
            ControlAdded += UserScaleControlWarpPanel_ControlAdded;
            ControlRemoved += UserScaleControlWarpPanel_ControlRemoved;
            MouseDown += UserScaleControlWarpPanel_MouseDown;
            MouseMove += UserScaleControlWarpPanel_MouseMove;
            MouseWheel += UserScaleControlWarpPanel_MouseWheel;
        }

        private void UserScaleControlWarpPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            IScaleControl scaleControl = e.Control as IScaleControl;
            if (scaleControl != null)
            {
                Controls.SetChildIndex(e.Control, GetFirstZIndex());
            }

            e.Control.LocationChanged += Control_LocationChanged;
            e.Control.SizeChanged += Control_SizeChanged;
        }

        private void UserScaleControlWarpPanel_ControlRemoved(object sender, ControlEventArgs e)
        {
            e.Control.LocationChanged -= Control_LocationChanged;
            e.Control.SizeChanged -= Control_SizeChanged;
        }

        private void Control_SizeChanged(object sender, EventArgs e)
        {
            Minimap.Draw();
        }

        private void Control_LocationChanged(object sender, EventArgs e)
        {
            Minimap.Draw();
        }

        private void UserScaleControlWarpPanel_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDownPoint = e.Location;
            MouseDownInnerLocation = InnerLocation;
        }

        private void UserScaleControlWarpPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Set InnerLocation
                int reverseInnerPointX = -MouseDownInnerLocation.X + e.X - MouseDownPoint.X;
                int reverseInnerPointY = -MouseDownInnerLocation.Y + e.Y - MouseDownPoint.Y;

                SetInnerLocation(new Point(-reverseInnerPointX, -reverseInnerPointY));
            }
        }

        private void UserScaleControlWarpPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) == Keys.Control)
            {
                float oldScaleValue = ScaleValue;
                float newScaleValue = Math.Min(Math.Max(ScaleValue + (e.Delta < 0 ? -ScaleValueUnit : ScaleValueUnit), MinimumScaleValue), MaximumScaleValue);

                if (oldScaleValue != newScaleValue)
                {
                    // Get newInnerLocationX
                    var oldPointX = InnerLocation.X + e.X;
                    var newPointX = (InnerLocation.X + e.X) * (newScaleValue / oldScaleValue);
                    var newInnerLocationX = InnerLocation.X - (int)Math.Round(oldPointX - newPointX);
                    newInnerLocationX = Convert.ToInt32(Math.Min(Math.Max(newInnerLocationX, 0), (InnerSize.Width - Width * (1 / newScaleValue)) * newScaleValue));

                    // Get newInnerLocationY
                    var oldPointY = InnerLocation.Y + e.Y;
                    var newPointY = (InnerLocation.Y + e.Y) * (newScaleValue / oldScaleValue);
                    var newInnerLocationY = InnerLocation.Y - (int)Math.Round(oldPointY - newPointY);
                    newInnerLocationY = Convert.ToInt32(Math.Min(Math.Max(newInnerLocationY, 0), (InnerSize.Height - Height * (1 / newScaleValue)) * newScaleValue));

                    SetScaleValueNInnerLocation(newScaleValue, new Point(newInnerLocationX, newInnerLocationY));
                }
            }
            else
            {
                if (!HasControlsInPoint(e.Location))
                {
                    SetInnerLocation(new Point(InnerLocation.X, Math.Min(Math.Max(InnerLocation.Y - e.Delta, 0), InnerSize.Height - Height)));
                }
            }
        }
        #endregion

        #region Function
        public void SetInnerLocation(Point newInnerLocation)
        {
            int toBeMovedX = InnerLocation.X - newInnerLocation.X;
            int toBeMovedY = InnerLocation.Y - newInnerLocation.Y;

            foreach (Control control in Controls)
            {
                IScaleControl scaleControl = control as IScaleControl;
                if (scaleControl != null)
                {
                    control.Location = new Point(control.Location.X + toBeMovedX, control.Location.Y + toBeMovedY);
                }
            }

            InnerLocation = newInnerLocation;
            DrawStatusPanel();
            Minimap.Draw();
        }

        public void BringToFrontChild(Control control)
        {
            Controls.SetChildIndex(control, GetFirstZIndex());
        }

        private int GetFirstZIndex()
        {
            int notScaleControlCount = 0;
            for (int i = 0; i < Controls.Count; i++)
            {
                if (Controls[i] as IScaleControl == null)
                {
                    notScaleControlCount++;
                }
            }
            return notScaleControlCount;
        }

        private void DrawStatusPanel(Graphics graphics = null)
        {
            if (graphics == null)
            {
                graphics = StatusPanel.CreateGraphics();
            }

            var bitmap = new Bitmap(StatusPanel.Width, StatusPanel.Height);
            Graphics bitmapGraphics = Graphics.FromImage(bitmap);
            bitmapGraphics.FillRectangle(new SolidBrush(StatusPanel.BackColor), new Rectangle(0, 0, StatusPanel.Width, StatusPanel.Height));

            string bottomString = string.Format(
                "Scale: {0} %    Position: {1}, {2}"
                , Math.Round(ScaleValue * 100)
                , Math.Floor(InnerLocation.X * (1 / ScaleValue)), Math.Floor(InnerLocation.Y * (1 / ScaleValue))
            );
            bitmapGraphics.DrawString(bottomString, StatusPanelFont, StatusPanelBrush, StatusPanelStringPosition);

            graphics.DrawImage(bitmap, 0, 0);
            bitmapGraphics.Dispose();
        }

        private void SetScaleValueNInnerLocation(float scaleValue, Point innerLocation)
        {
            int toBeMovedX = InnerLocation.X - innerLocation.X;
            int toBeMovedY = InnerLocation.Y - innerLocation.Y;
            
            _ScaleValue = Math.Min(Math.Max(scaleValue, MinimumScaleValue), MaximumScaleValue);

            foreach (Control control in Controls)
            {
                IScaleControl scaleControl = control as IScaleControl;
                if (scaleControl != null)
                {
                    scaleControl.SetScaleValueNMovePoint(_ScaleValue, new Point(toBeMovedX, toBeMovedY));
                }
            }

            InnerLocation = innerLocation;
            DrawStatusPanel();
            Minimap.Draw();

        }

        private bool HasControlsInPoint(Point point)
        {
            bool rs = false;
            foreach (Control control in Controls)
            {
                IScaleControl scaleControl = control as IScaleControl;
                if (scaleControl != null)
                {
                    if (control.Bounds.Contains(point))
                    {
                        rs = true;
                        break;
                    }
                }
            }
            return rs;
        }
        #endregion

        #region Common Function(To be moved later)
        private void SuspendLayoutChildren()
        {
            foreach(Control control in Controls)
            {
                control.SuspendLayout();
            }
        }

        private void ResumeLayoutChildren(bool performLayout)
        {
            foreach (Control control in Controls)
            {
                control.ResumeLayout(performLayout);
            }
        }

        private void SetDoubleBuffering(System.Windows.Forms.Control control, bool value)
        {
            System.Reflection.PropertyInfo controlProperty = typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            controlProperty.SetValue(control, value, null);
        }
        #endregion
    }
}