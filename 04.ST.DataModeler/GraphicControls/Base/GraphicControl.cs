using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ST.Core;
using System.Configuration;
using System.Drawing.Drawing2D;

namespace ST.DataModeler
{
    public abstract class GraphicControl: IGraphicControlParent, IDisposable
    {
        #region Values
        #endregion

        #region Propertise
        public DataModeler Target => _Target;
        private DataModeler _Target;

        public IGraphicControlParent Parent { get; set; }

        public bool Visible
        {
            get
            {
                return _Visible;
            }
            set
            {
                if (_Visible != value)
                {
                    _Visible = value;
                    VisibleChanged?.Invoke(this, new EventArgs());
                    RefreshAll();
                }
            }
        }
        private bool _Visible = true;

        public bool Focused
        {
            get
            {
                return _Focused;
            }
            set
            {
                
                if (_Focused != value)
                {
                    _Focused = value;
                    if (_Focused)
                    {
                        Event.CallEvent(this, GotFocus);
                    }
                    else
                    {
                        Event.CallEvent(this, LostFocus);
                    }
                    Refresh();
                }
            }
        }
        private bool _Focused = false;

        public bool CanFocus
        {
            get
            {
                return _CanFocus;
            }
            set
            {
                if (_CanFocus != value)
                {
                    _CanFocus = value;
                    if (!_CanFocus)
                    {
                        Focused = false;
                    }
                }
            }
        }
        private bool _CanFocus = true;

        public bool ContainsFocus => ContainsFocus_Proc(this);

        private bool ContainsFocus_Proc(GraphicControl parentControl)
        {
            bool rs = false;
            if (parentControl.Focused)
            {
                rs = true;
            }
            else
            {
                foreach(GraphicControl control in parentControl.Controls)
                {
                    rs = ContainsFocus_Proc(control);
                    if (rs)
                    {
                        break;
                    }
                }
            }
            return rs;
        }

        public IntPtr Handle => Target.Handle;

        public bool TabStop { get; set; } = false;

        public bool BlockRefresh { get; set; } = false;

        public int Left
        {
            get
            {
                return _Left;
            }
            set
            {
                if (_Left != value)
                {
                    _Left = value;
                    Event.CallEvent(this, LocationChanged);
                    RefreshAll();
                }
            }
        }
        private int _Left;

        public int Top
        {
            get
            {
                return _Top;
            }
            set
            {
                if (_Top != value)
                {
                    _Top = value;
                    Event.CallEvent(this, LocationChanged);
                    RefreshAll();
                }
            }
        }
        private int _Top;

        public int Right => _Left + _Width;

        public int Bottom => _Top + _Height;

        public int Width
        {
            get
            {
                return _Width;
            }
            set
            {
                if (_Width != value)
                {
                    Event.CallEvent(this, SizeChanging);
                    _Width = value;
                    Event.CallEvent(this, SizeChanged);
                    Event.CallEvent(this, Resize);
                    RefreshAll();
                }
            }
        }
        private int _Width = 200;

        public int Height
        {
            get
            {
                return _Height;
            }
            set
            {
                if (_Height != value)
                {
                    Event.CallEvent(this, SizeChanging);
                    _Height = value;
                    Event.CallEvent(this, SizeChanged);
                    Event.CallEvent(this, Resize);
                    RefreshAll();
                }
            }
        }
        private int _Height = 200;

        public Point Location
        {
            get
            {
                return new Point(_Left, _Top);
            }
            set
            {
                if (value.X != _Left || value.Y != _Top)
                {
                    _Left = value.X;
                    _Top = value.Y;
                    Event.CallEvent(this, LocationChanged);
                    RefreshAll();
                }
            }
        }

        public Size Size
        {
            get
            {
                return new Size(_Width, _Height);
            }
            set
            {
                if (value.Width != _Width || value.Height != _Height)
                {
                    Event.CallEvent(this, SizeChanging);
                    _Width = value.Width;
                    _Height = value.Height;
                    Event.CallEvent(this, SizeChanged);
                    Event.CallEvent(this, Resize);
                    RefreshAll();
                }
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(_Left, _Top, _Width, _Height);
            }
            set
            {
                bool doDraw = false;
                if (value.X != _Left || value.Y != _Top)
                {
                    _Left = value.X;
                    _Top = value.Y;
                    Event.CallEvent(this, LocationChanged);
                    doDraw = true;
                }

                if (value.Width != _Width || value.Height != _Height)
                {
                    Event.CallEvent(this, SizeChanging);
                    _Width = value.Width;
                    _Height = value.Height;
                    Event.CallEvent(this, SizeChanged);
                    Event.CallEvent(this, Resize);
                    doDraw = true;
                }

                if (doDraw)
                {
                    RefreshAll();
                }
            }
        }

        public int ZIndex
        {
            get
            {
                return Parent.Controls.IndexOf(this);
            }
            set
            {
                Parent.Controls.SetChildIndex(this, value);
            }
        }

        public Cursor Cursor
        {
            get
            {
                return _Cursor;
            }
            set
            {
                _Cursor = value;
                Target.Cursor = _Cursor;
            }
        }
        private Cursor _Cursor = Cursors.Default;

        public Padding Padding
        {
            get
            {
                return _Padding;
            }
            set
            {
                if (value != _Padding)
                {
                    _Padding = value;
                }
            }
        }
        private Padding _Padding = Padding.Empty;

        public Color ForeColor
        {
            get
            {
                return _ForeColor;
            }
            set
            {
                if (value != _ForeColor)
                {
                    _ForeColor = value;
                }
            }
        }
        private Color _ForeColor = Color.Black;

        public Color BackColor
        {
            get
            {
                return _BackColor;
            }
            set
            {
                if (value != _BackColor)
                {
                    _BackColor = value;
                }
            }
        }
        private Color _BackColor = Color.White;

        public Font Font
        {
            get
            {
                return _Font;
            }
            set
            {
                if (value != _Font)
                {
                    _Font = value;
                }
            }
        }
        private Font _Font = new Font("맑은 고딕", 9F);

        public bool AllowDrop { get; set; } = false;

        public DockStyle Dock
        { 
            get
            {
                return _Dock;
            }
            set
            {
                if (_Dock != value)
                {
                    if (this.GetType().Name == "GraphicScrollBar")
                    {
                    }

                    if (_Dock == DockStyle.None)
                    {
                        DockNoneBounds = Bounds;
                    }
                    else if (value == DockStyle.None)
                    {
                        Bounds = DockNoneBounds;
                        DockNoneBounds = Rectangle.Empty;
                    }

                    _Dock = value;
                    if (_Dock == DockStyle.None)
                    {
                        if (!DockNoneBounds.Equals(Rectangle.Empty))
                        {
                            Bounds = DockNoneBounds;
                            DockNoneBounds = Rectangle.Empty;
                        }
                    }
                    else
                    {
                        SetDock();
                    }
                }
            }
        }
        public DockStyle _Dock = DockStyle.None;
        private Rectangle DockNoneBounds = Rectangle.Empty;

        public Keys ModifierKeys => Control.ModifierKeys;

        public GraphicControlCollection Controls => _Controls;
        private GraphicControlCollection _Controls;
        #endregion

        #region EventHandler
        // Key
        public event PreviewKeyDownEventHandler PreviewKeyDown;
        public event KeyEventHandler KeyDown;
        public event KeyEventHandler KeyUp;

        // Mouse
        public event MouseEventHandler MouseDown;
        public event MouseEventHandler MouseMove;
        public event MouseEventHandler MouseUp;
        public event EventHandler MouseLeave;
        public event MouseEventHandler MouseWheel;
        public event EventHandler MouseHover;
        public event MouseEventHandler Click;
        public event MouseEventHandler DoubleClick;

        // Bounds
        public event EventHandler SizeChanging;
        public event EventHandler SizeChanged;
        public event EventHandler LocationChanged;
        public event EventHandler Resize;

        // Drag
        // Todo : 드래그 이벤트 구현 필요
        public event DragEventHandler DragEnter;
        public event DragEventHandler DragDrop;
        public event DragEventHandler DragOver;
        public event EventHandler DragLeave;
        public event QueryContinueDragEventHandler QueryContinueDrag;

        // Etc
        public event EventHandler GotFocus;
        public event EventHandler LostFocus;
        public event PaintEventHandler Paint;
        public event EventHandler VisibleChanged;
        public event EventHandler Disposed;

        // Control
        public event GraphicControlEventHandler ControlAdded;
        public event GraphicControlEventHandler ControlRemoved;
        #endregion

        #region Load
        public GraphicControl(DataModeler target)
        {
            _Target = target;
            _Controls = new GraphicControlCollection(this);
            SetEvents();
        }
        #endregion

        #region Events
        private void SetEvents()
        {
            GotFocus += GraphicControl_GotFocus;
            ControlAdded += GraphicControl_ControlAdded;
            SizeChanged += GraphicControl_SizeChanged;
        }

        private void GraphicControl_GotFocus(object sender, EventArgs e)
        {
            Focused = true;
            foreach(GraphicControl control in Target.Controls)
            {
                GraphicControl_GotFocus_SetFocus(control, this);
            }

            foreach (RelationControl control in Target.Relations)
            {
                control.Focused = false;
            }
        }

        private void GraphicControl_GotFocus_SetFocus(GraphicControl control, GraphicControl focusControl)
        {
            if (!control.Equals(focusControl))
            {
                control.Focused = false;
            } 

            foreach(GraphicControl child in control.Controls)
            {
                GraphicControl_GotFocus_SetFocus(child, focusControl);
            }
        }

        private void GraphicControl_ControlAdded(object sender, GraphicControlEventArgs e)
        {
            if (e.Control.Dock != DockStyle.None)
            {
                e.Control.SetDock();
            }
        }

        private void GraphicControl_SizeChanged(object sender, EventArgs e)
        {
            SetChildrenDock(this);
        }
        #endregion

        #region OnEvents Methods
        public void OnKeyDown(KeyEventArgs e)
        {
            Event.CallEvent(this, KeyDown, e);
        }

        public void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            Event.CallEvent(this, PreviewKeyDown, e);
        }

        public void OnKeyUp(KeyEventArgs e)
        {
            Event.CallEvent(this, KeyUp, e);
        }

        public void OnMouseDown(MouseEventArgs e)
        {
            Focused = true;
            Event.CallEvent(this, MouseDown, e);
        }

        public void OnMouseMove(MouseEventArgs e)
        {
            Event.CallEvent(this, MouseMove, e);
        }

        public void OnMouseUp(MouseEventArgs e)
        {
            Event.CallEvent(this, MouseUp, e);
        }

        public void OnMouseLeave()
        {
            Event.CallEvent(this, MouseLeave);
        }

        public void OnMouseWheel(MouseEventArgs e)
        {
            Event.CallEvent(this, MouseWheel, e);
        }

        public void OnMouseHover()
        {
            Event.CallEvent(this, MouseHover);
        }

        public void OnClick(MouseEventArgs e)
        {
            Event.CallEvent(this, Click, e);
        }

        public void OnDoubleClick(MouseEventArgs e)
        {
            Event.CallEvent(this, DoubleClick, e);
        }

        public void OnPaint(PaintEventArgs e)
        {
            if (Visible)
            {
                Event.CallEvent(this, Paint, e);
                OnPaint_Children(e);
            }
        }

        private void OnPaint_Children(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle thisBounds = GetBoundsFromTarget();

            for (int i = Controls.Count - 1; 0 <= i; i--)
            {
                GraphicControl control = Controls[i];
                if (control.Width > 0 && control.Height > 0)
                {
                    Rectangle controlBounds = control.GetBoundsFromTarget();
                    controlBounds.Offset(-thisBounds.X, -thisBounds.Y);
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
            }
        }

        public void OnControlAdded(GraphicControlEventArgs e)
        {
            ControlAdded?.Invoke(this, e);
        }

        public void OnControlRemoved(GraphicControlEventArgs e)
        {
            ControlRemoved?.Invoke(this, e);
        }

        public void OnDragEnter(DragEventArgs e)
        {
            Event.CallEvent(this, DragEnter, e);
        }

        public void OnDragOver(DragEventArgs e)
        {
            Event.CallEvent(this, DragOver, e);
        }

        public void OnDragDrop(DragEventArgs e)
        {
            Event.CallEvent(this, DragDrop, e);
        }

        public void OnDragLeave(EventArgs e)
        {
            Event.CallEvent(this, DragLeave);
        }

        public void OnQueryContinueDrag(QueryContinueDragEventArgs e)
        {
            QueryContinueDrag?.Invoke(this, e);
        }

        public void OnSizeChanged()
        {
            Event.CallEvent(this, SizeChanged);
        }

        public void OnSizeChanging()
        {
            Event.CallEvent(this, SizeChanging);
        }
        #endregion

        #region Control Methods
        public void RefreshAll()
        {
            if (Visible && !BlockRefresh)
            {
                Target.RequestRefresh(this, RefreshType.All);
            }
        }

        public void Refresh()
        {
            if (Visible && !BlockRefresh)
            {
                Target.RequestRefresh(this, RefreshType.This);
            }
        }

        public GraphicControlGraphics CreateGraphics(GraphicControl graphicControl)
        {
            GraphicControlGraphics graphics = new GraphicControlGraphics(graphicControl);
            return graphics;
        }

        public Point PointToClient(Point p)
        {
            Point rsPoint = Target.PointToClient(p);
            Point thisLocation = GetLocationFromTarget();
            rsPoint.Offset(-thisLocation.X, -thisLocation.Y);
            return rsPoint;
        }

        public Rectangle RectangleToScreen(Rectangle childBounds)
        {
            Point thisLocation = GetLocationFromTarget();
            childBounds.Offset(thisLocation.X, thisLocation.Y);
            Rectangle rsRectangle = Target.RectangleToScreen(childBounds);
            return rsRectangle;
        }

        public DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects)
        {
            return Target.DoDragDrop(data, allowedEffects);
        }

        public void Focus()
        {
            Focused = true;
            Event.CallEvent(this, GotFocus);
        }

        public void BringToFront()
        {
            var dataModeler = Target as DataModeler;
            dataModeler.Controls.SetChildIndex(this, 0);
            RefreshAll();
        }

        public void SendToBack()
        {
            var dataModeler = Target as DataModeler;
            dataModeler.Controls.SetChildIndex(this, dataModeler.Controls.Count - 1);
            RefreshAll();
        }

        public IAsyncResult BeginInvoke(Delegate method)
        {
            return Target.BeginInvoke(method);
        }
        #endregion

        #region This Methods
        public void SetChildrenDock(GraphicControl parent)
        {
            foreach (GraphicControl control in parent.Controls)
            {
                control.SetDock();
                if (control.Controls.Count > 0)
                {
                    SetChildrenDock(control);
                }
            }
        }

        public void SetDock()
        {
            if (Parent != null)
            {
                switch (Dock)
                {
                    case DockStyle.Top:    Bounds = new Rectangle(Parent.Padding.Left , Parent.Padding.Top    , Parent.Width - Parent.Padding.Horizontal, Height - Parent.Padding.Vertical); break;
                    case DockStyle.Bottom: Bounds = new Rectangle(Parent.Padding.Left , Parent.Height - Height, Parent.Width - Parent.Padding.Horizontal, Height - Parent.Padding.Bottom); break;
                    case DockStyle.Left:   Bounds = new Rectangle(Parent.Padding.Left , Parent.Padding.Top    , Width - Parent.Padding.Horizontal       , Parent.Height - Parent.Padding.Vertical); break;
                    case DockStyle.Right:  Bounds = new Rectangle(Parent.Width - Width, Parent.Padding.Top    , Width - Parent.Padding.Right            , Parent.Height - Parent.Padding.Vertical); break;
                    case DockStyle.Fill:   Bounds = new Rectangle(Parent.Padding.Left , Parent.Padding.Top    , Parent.Width - Parent.Padding.Horizontal, Parent.Height - Parent.Padding.Vertical); break;
                    case DockStyle.None:
                        // Processing in [public DockStyle Dock]
                        break;
                }
            }
        }

        public Point GetLocationFromTarget()
        {
            Point rs = new Point(Left, Top);

            GraphicControl parent = Parent as GraphicControl;
            while (parent != null)
            {
                rs.Offset(parent.Left, parent.Top);
                parent = parent.Parent as GraphicControl;
            }

            return rs;
        }

        public Rectangle GetBoundsFromTarget()
        {
            Point location = GetLocationFromTarget();
            return new Rectangle(location.X, location.Y, Width, Height);
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            Disposed?.Invoke(this, new EventArgs());
        }
        #endregion

        #region Override
        /// <summary>
        /// [GraphicControl] 컨트롤이 문자를 처리하면 true이고, 그렇지 않으면 false입니다.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        public virtual bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return false;
        }

        /// <summary>
        /// [GraphicControl] Windows 메시지를 처리합니다. true를 리턴하면 base.WndProc(ref m);를 호출하고 false이면 실행하지 않습니다.
        /// </summary>
        /// <param name="m"></param>
        public virtual bool WndProc(Message m)
        {
            return true;
        }

        /// <summary>
        /// [GraphicControl] 지정된 메시지를 기본 창 프로지서로 보냅니다.
        /// </summary>
        /// <param name="m"></param>
        public void DefWndProc(ref Message m)
        {
            Target.OnDefWndProc(ref m);
        }
        #endregion
    }
}
