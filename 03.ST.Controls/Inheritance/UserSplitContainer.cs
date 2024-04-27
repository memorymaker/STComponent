using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using ST.Core;

namespace ST.Controls
{
    public class UserSplitContainer : SplitContainer
    {
        public event SplitterDistanceChangingEventHandeler SplitterDistanceChanging;

        private SplitterForm Splitter;

        private SplitterArea SplitterArea1;
        private SplitterArea SplitterArea2;

        private Point MouseDownLocation;
        private Rectangle MouseDownBouds;

        private bool IsBeginControlUpdate = false;
        private bool SplitterMoveCanceled = false;

        public Color SplitterColor
        {
            get; set;
        } = Color.Blue;

        public Color SplitterMouseOverColor
        {
            get; set;
        } = Color.Red;

        public bool IsMouseMove => _IsMouseMove;
        private bool _IsMouseMove = false;

        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }

        public int SplitterWidthRevision
        {
            get
            {
                return _SplitterWidthRevision;
            }
            set
            {
                if (_SplitterWidthRevision != value)
                {
                    _SplitterWidthRevision = value;
                    SetSplitterAreaBounds();
                }
            }
        }
        private int _SplitterWidthRevision = 4;

        new public int SplitterDistance
        {
            get
            {
                return base.SplitterDistance;
            }
            set
            {
                if (base.SplitterDistance != value)
                {
                    ThisBeginControlUpdate(); Console.WriteLine("BeginControlUpdate 1 SplitterDistance");
                    base.SplitterDistance = value;
                    ThisEndControlUpdate();  Console.WriteLine("EndControlUpdate 1 SplitterDistance");
                    SetSplitterAreaBounds();
                }
            }
        }

        public UserSplitContainer()
        {
            // Styles
            SetStyle(ControlStyles.Selectable, false);
            TabStop = false;

            // This
            Paint += UserSplitContainer_Paint;
            MouseDown += UserSplitContainer_MouseDown;
            MouseMove += UserSplitContainer_MouseMove;
            MouseUp += UserSplitContainer_MouseUp;
            MouseLeave += UserSplitContainerInner_MouseLeave;
            KeyDown += UserSplitContainerInner_KeyDown;
            SplitterMoved += UserSplitContainer_SplitterMoved;
            SizeChanged += UserSplitContainer_SizeChanged;

            // Splitter
            Splitter = new SplitterForm(this);
            Splitter.FormBorderStyle = FormBorderStyle.None;
            Splitter.Visible = false;
            Splitter.BackColor = Color.FromArgb(100, 100, 100);
            Splitter.ShowInTaskbar = false;
            Splitter.StartPosition = FormStartPosition.Manual;
            Splitter.Width = SplitterWidth;

            // SplitterArea
            SplitterArea1 = new SplitterArea(this);
            SplitterArea2 = new SplitterArea(this);
            SplitterArea1.BackColor = Color.Red;
            SplitterArea1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            SplitterArea1.MouseDown += SplitterArea_MouseDown;
            SplitterArea1.MouseMove += SplitterArea_MouseMove;
            SplitterArea1.MouseUp += SplitterArea_MouseUp;
            SplitterArea1.MouseLeave += SplitterArea_MouseLeave;

            SplitterArea2.BackColor = Color.Blue;
            SplitterArea2.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            SplitterArea2.MouseDown += SplitterArea_MouseDown;
            SplitterArea2.MouseMove += SplitterArea_MouseMove;
            SplitterArea2.MouseUp += SplitterArea_MouseUp;
            SplitterArea2.MouseLeave += SplitterArea_MouseLeave;

            Panel1.Controls.Add(SplitterArea1);
            Panel2.Controls.Add(SplitterArea2);

            SetSplitterAreaBounds();
        }

        #region This Events
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            var g = e.Graphics;
            Rectangle splitterRec = Rectangle.Empty;

            if (Orientation == Orientation.Horizontal)
            {
                splitterRec.X = 0;
                splitterRec.Width = Width;
                splitterRec.Y = SplitterDistance - SplitterWidthRevision;
                splitterRec.Height = SplitterWidth + SplitterWidthRevision * 2;
            }
            else if (Orientation == Orientation.Vertical)
            {
                splitterRec.Y = 0;
                splitterRec.Height = Height;
                splitterRec.X = SplitterDistance - SplitterWidthRevision;
                splitterRec.Width = SplitterWidth + SplitterWidthRevision * 2;
            }

            if (splitterRec != Rectangle.Empty)
            {
                g.FillRectangle(
                    IsMouseMove || SplitterArea1.IsMouseMove || SplitterArea2.IsMouseMove
                        ? new SolidBrush(SplitterMouseOverColor)
                        : new SolidBrush(SplitterColor)
                , splitterRec);
            }
        }

        private void UserSplitContainer_Paint(object sender, PaintEventArgs e)
        {
            OnPaintBackground(e);
        }

        private void UserSplitContainer_MouseDown(object sender, MouseEventArgs e)
        {
            SetSplitterBounds();
            Splitter.Visible = true;
            USER32.ShowInactiveTopmost(Splitter);

            MouseDownLocation = e.Location;
            MouseDownBouds = Splitter.Bounds;
            SplitterMoveCanceled = false;
            IsBeginControlUpdate = false;
        }

        private void UserSplitContainer_MouseMove(object sender, MouseEventArgs e)
        {
            if (Orientation == Orientation.Vertical)
            {
                Cursor = Cursors.SizeWE;
            }
            else if (Orientation == Orientation.Horizontal)
            {
                Cursor = Cursors.SizeNS;
            }

            _IsMouseMove = true;

            if (Splitter.Visible)
            {
                Point location = PointToScreen(MouseDownLocation);
                Point pointScreen =  Parent.PointToScreen(Location);
                if (Orientation == Orientation.Vertical)
                {
                    Splitter.Left = Math.Min(
                        Math.Max(MouseDownBouds.Left - location.X + Cursor.Position.X, pointScreen.X + Panel1MinSize)
                    , pointScreen.X + Width - Panel2MinSize - SplitterWidth);
                }
                else if (Orientation == Orientation.Horizontal)
                {
                    Splitter.Top = Math.Min
                        (Math.Max(MouseDownBouds.Top - location.Y + Cursor.Position.Y, pointScreen.Y + Panel1MinSize)
                    , pointScreen.Y + Height - Panel2MinSize - SplitterWidth);
                }
            }

            OnRefresh();
        }

        private void UserSplitContainer_MouseUp(object sender, MouseEventArgs e)
        {
            SplitterArea_MouseUp(null, null);
        }

        private void UserSplitContainerInner_MouseLeave(object sender, EventArgs e)
        {
            _IsMouseMove = false;
            Cursor = Cursors.Default;
            OnRefresh();
        }

        private void UserSplitContainerInner_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Splitter.Visible = false;
                OnRefresh();
            }
        }

        private void UserSplitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            //ThisEndControlUpdate();  Console.WriteLine("EndControlUpdate - UserSplitContainer_SplitterMoved");
            Splitter.Visible = false;
        }

        private void UserSplitContainer_SizeChanged(object sender, EventArgs e)
        {
            SetSplitterAreaBounds();
        }
        #endregion

        #region WndProc
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 257 && m.WParam.ToInt64() == 27)
            {
                // WndProc 257 27
                SplitterMoveCanceled = true;
                Splitter.Visible = false;
            }
            base.WndProc(ref m);
        }
        #endregion

        #region SplitterArea
        private void SplitterArea_MouseDown(object sender, MouseEventArgs e)
        {
            SetSplitterBounds();
            Splitter.Visible = true;
            USER32.ShowInactiveTopmost(Splitter);

            MouseDownLocation = Cursor.Position;
            MouseDownBouds = Splitter.Bounds;

            if (Orientation == Orientation.Horizontal)
            {
                MouseDownLocation.Y -= SplitterWidthRevision;
            }
            else if (Orientation == Orientation.Vertical)
            {
                MouseDownLocation.X -= SplitterWidthRevision;
            }
        }

        private void SplitterArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (Orientation == Orientation.Vertical)
            {
                Cursor = Cursors.SizeWE;
            }
            else if (Orientation == Orientation.Horizontal)
            {
                Cursor = Cursors.SizeNS;
            }

            if (e.Button == MouseButtons.Left && Splitter.Visible)
            {
                SetSplitterBounds(Cursor.Position);
            }
        }

        private void SplitterArea_MouseUp(object sender, MouseEventArgs e)
        {
            if (Splitter.Visible)
            {
                Splitter.Visible = false;
                if (Orientation == Orientation.Vertical)
                {
                    int splitterDistance = Math.Max(SplitterDistance + Splitter.Bounds.X - MouseDownBouds.X, Panel1MinSize);
                    SplitterDistanceChangingEventArgs args = new SplitterDistanceChangingEventArgs(splitterDistance);
                    SplitterDistanceChanging?.Invoke(this, args);
                    if (!args.Cancel)
                    {
                        SplitterDistance = args.SplitterDistance;
                    }
                }
                else if (Orientation == Orientation.Horizontal)
                {
                    int splitterDistance = Math.Max(SplitterDistance + Splitter.Bounds.Y - MouseDownBouds.Y, Panel1MinSize);
                    SplitterDistanceChangingEventArgs args = new SplitterDistanceChangingEventArgs(splitterDistance);
                    SplitterDistanceChanging?.Invoke(this, args);
                    if (!args.Cancel)
                    {
                        SplitterDistance = args.SplitterDistance;
                    }
                }
            }
        }

        private void SplitterArea_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }
        #endregion

        #region Function
        private void SetSplitterBounds(Point? location = null)
        {
            Point pointScreen = Parent.PointToScreen(Location);
            if (Orientation == Orientation.Vertical)
            {
                Splitter.Top = pointScreen.Y;
                Splitter.Height = Height;
                Splitter.Width = SplitterWidth;
                Splitter.MaximumSize = new Size(SplitterWidth, Height);
                if (location != null)
                {
                    Splitter.Left = Math.Min(
                        Math.Max(MouseDownBouds.Left + (((Point)location).X - MouseDownLocation.X - SplitterWidthRevision), pointScreen.X + Panel1MinSize)
                    , pointScreen.X + Width - Panel2MinSize - SplitterWidth);
                }
                else
                {
                    Splitter.Left = pointScreen.X + SplitterDistance;
                }
            }
            else if (Orientation == Orientation.Horizontal)
            {
                Splitter.Left = pointScreen.X;
                Splitter.Width = Width;
                Splitter.Height = SplitterWidth;
                Splitter.MaximumSize = new Size(Width, SplitterWidth);
                if (location != null)
                {
                    Splitter.Top = Math.Min
                        (Math.Max(MouseDownBouds.Top + (((Point)location).Y - MouseDownLocation.Y - SplitterWidthRevision), pointScreen.Y + Panel1MinSize)
                    , pointScreen.Y + Height - Panel2MinSize - SplitterWidth);
                }
                else
                {
                    Splitter.Top = pointScreen.Y + SplitterDistance;
                }
            }
        }

        private void SetSplitterAreaBounds()
        {
            if (Orientation == Orientation.Vertical)
            {
                SplitterArea1.Bounds = new Rectangle(Panel1.Width - SplitterWidthRevision, 0, SplitterWidthRevision, Panel1.Height);
                SplitterArea2.Bounds = new Rectangle(0, 0, SplitterWidthRevision, Panel2.Height);
            }
            else if (Orientation == Orientation.Horizontal)
            {
                SplitterArea1.Bounds = new Rectangle(0, Panel1.Height - SplitterWidthRevision, Panel1.Width, SplitterWidthRevision);
                SplitterArea2.Bounds = new Rectangle(0, 0, Panel2.Width, SplitterWidthRevision);
            }
        }

        public void OnRefresh()
        {
            OnPaintBackground(new PaintEventArgs(CreateGraphics(), new Rectangle(0, 0, Width, Height)));
        }

        private void ThisBeginControlUpdate()
        {
            if (!IsBeginControlUpdate)
            {
                IsBeginControlUpdate = true;
                 this.BeginControlUpdate();
            }
        }

        private void ThisEndControlUpdate(bool refresh = true)
        {
            if (IsBeginControlUpdate)
            {
                IsBeginControlUpdate = false;
                this.EndControlUpdate(refresh);
            }
        }
        #endregion

        #region SplitterDistanceChangingEventArgs
        public delegate void SplitterDistanceChangingEventHandeler(object sender, SplitterDistanceChangingEventArgs e);

        public class SplitterDistanceChangingEventArgs : EventArgs
        {
            public int SplitterDistance;
            public bool Cancel = false;

            public SplitterDistanceChangingEventArgs(int _SplitterDistance)
            {
                SplitterDistance = _SplitterDistance;
            }
        }
        #endregion

        #region SplitterForm
        private class SplitterForm : Form
        {
            private UserSplitContainer Target;

            protected override bool ShowWithoutActivation
            {
                get
                {
                    return true;
                }
            }

            public SplitterForm(UserSplitContainer target)
            {
                SetStyle(ControlStyles.Selectable, false);
                TabStop = false;
                Load += SplitterForm_Load;
                Target = target;
            }

            private void SplitterForm_Load(object sender, EventArgs e)
            {
                Size = MaximumSize;
                Target.Focus();
            }
        }
        #endregion

        #region SplitterArea
        private class SplitterArea : Panel
        {
            private UserSplitContainer Target;

            public bool IsMouseDown => _IsMouseDown;
            private bool _IsMouseDown = false;

            public bool IsMouseMove => _IsMouseMove;
            private bool _IsMouseMove = false;

            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams createParams = base.CreateParams;
                    createParams.ExStyle |= 0x00000020;
                    return createParams;
                }
            }

            public SplitterArea(UserSplitContainer target)
            {
                Target = target;
                SetEvets();
            }

            private void SetEvets()
            {
                MouseDown += UserSplitContainerSplitter_MouseDown;
                MouseMove += UserSplitContainerSplitter_MouseMove;
                MouseUp += UserSplitContainerSplitter_MouseUp;
                MouseLeave += UserSplitContainerSplitter_MouseLeave;
            }

            private void UserSplitContainerSplitter_MouseDown(object sender, MouseEventArgs e)
            {
                _IsMouseDown = true;
            }

            private void UserSplitContainerSplitter_MouseMove(object sender, MouseEventArgs e)
            {
                OnPaintBackground(new PaintEventArgs(CreateGraphics(), Bounds));
                if (!_IsMouseMove)
                {
                    _IsMouseMove = true;
                    Target.OnRefresh();
                }
            }

            private void UserSplitContainerSplitter_MouseUp(object sender, MouseEventArgs e)
            {
                _IsMouseDown = false;
            }

            private void UserSplitContainerSplitter_MouseLeave(object sender, EventArgs e)
            {
                _IsMouseDown = false;
                _IsMouseMove = false;
                Target.OnRefresh();
            }

            protected override void OnPaintBackground(PaintEventArgs e)
            {
            }

            protected override void OnPaint(PaintEventArgs e)
            {
            }
        }
        #endregion
    }
}