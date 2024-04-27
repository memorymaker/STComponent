using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace ST.Controls
{
    public class DrawingPanel : Panel
    {
        public event PaintEventHandler PaintBackground;

        public int BorderTopWidth
        {
            get
            {
                return _BorderTopWidth;
            }
            set
            {
                if (_BorderTopWidth != value)
                {
                    _BorderTopWidth = value;
                    DrawBackgound();
                    SetBasePadding();
                }
            }
        }
        private int _BorderTopWidth = 0;

        public Color BorderTopColor
        {
            get
            {
                return _BorderTopColor;
            }
            set
            {
                if (_BorderTopColor != value)
                {
                    _BorderTopColor = value;
                    DrawBackgound();
                }
            }
        }
        private Color _BorderTopColor = Color.Gray;

        public int BorderBottomWidth
        {
            get
            {
                return _BorderBottomWidth;
            }
            set
            {
                if (_BorderBottomWidth != value)
                {
                    _BorderBottomWidth = value;
                    DrawBackgound();
                    SetBasePadding();
                }
            }
        }
        private int _BorderBottomWidth = 0;

        public Color BorderBottomColor
        {
            get
            {
                return _BorderBottomColor;
            }
            set
            {
                if (_BorderBottomColor != value)
                {
                    _BorderBottomColor = value;
                    DrawBackgound();
                }
            }
        }
        private Color _BorderBottomColor = Color.Gray;

        public int BorderLeftWidth
        {
            get
            {
                return _BorderLeftWidth;
            }
            set
            {
                if (_BorderLeftWidth != value)
                {
                    _BorderLeftWidth = value;
                    DrawBackgound();
                    SetBasePadding();
                }
            }
        }
        private int _BorderLeftWidth = 0;

        public Color BorderLeftColor
        {
            get
            {
                return _BorderLeftColor;
            }
            set
            {
                if (_BorderLeftColor != value)
                {
                    _BorderLeftColor = value;
                    DrawBackgound();
                }
            }
        }
        private Color _BorderLeftColor = Color.Gray;

        public int BorderRightWidth
        {
            get
            {
                return _BorderRightWidth;
            }
            set
            {
                if (_BorderRightWidth != value)
                {
                    _BorderRightWidth = value;
                    DrawBackgound();
                    SetBasePadding();
                }
            }
        }
        private int _BorderRightWidth = 0;

        public Color BorderRightColor
        {
            get
            {
                return _BorderRightColor;
            }
            set
            {
                if (_BorderRightColor != value)
                {
                    _BorderRightColor = value;
                    DrawBackgound();
                }
            }
        }
        private Color _BorderRightColor = Color.Gray;

        new public Padding Padding
        {
            get
            {
                return _Padding;
            }
            set
            {
                if (_Padding.Top != value.Top
                || _Padding.Bottom != value.Bottom
                || _Padding.Left != value.Left
                || _Padding.Right != value.Right)
                {
                    _Padding = value;
                    SetBasePadding();
                }
            }
        }
        private Padding _Padding = new Padding(0, 0, 0, 0);

        public bool UsingPaintBackground
        {
            get; set;
        } = true;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= 0x00000020;
                return createParams;
            }
        }

        private void SetBasePadding()
        {
            base.Padding = new Padding(
                  Padding.Left + BorderLeftWidth
                , Padding.Top + BorderTopWidth
                , Padding.Right + BorderRightWidth
                , Padding.Bottom + BorderBottomWidth
            );
        }

        public void DrawBackgound()
        {
            if (Visible && Width > 0 && Height > 0)
            {
                OnPaintBackground(new PaintEventArgs(CreateGraphics(), new Rectangle(0, 0, Width, Height)));
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (UsingPaintBackground && Visible && Width > 0 && Height > 0)
            {
                Rectangle ClipRect = new Rectangle(0, 0, Width, Height);
                BufferedGraphicsContext context = new BufferedGraphicsContext();
                context.MaximumBuffer = new Size(Width, Height);
                BufferedGraphics bufferedGraphics = context.Allocate(e.Graphics, ClipRect);

                using (Graphics g = bufferedGraphics.Graphics)
                {
                    g.Clear(BackColor);

                    PaintBackground?.Invoke(this, new PaintEventArgs(g, ClipRect));
                    OnPaintBackground_DrawBorder(g);

                    bufferedGraphics.Render(e.Graphics);
                }

                bufferedGraphics.Dispose();
                context.Dispose();
            }
        }

        private void OnPaintBackground_DrawBorder(Graphics g)
        {
            if (BorderTopWidth > 0)
            {
                g.FillRectangle(new SolidBrush(BorderTopColor), new Rectangle(0, 0, Width, BorderTopWidth));
            }

            if (BorderBottomWidth > 0)
            {
                g.FillRectangle(new SolidBrush(BorderBottomColor), new Rectangle(0, Height - BorderBottomWidth, Width, BorderBottomWidth));
            }

            if (BorderLeftWidth > 0)
            {
                g.FillRectangle(new SolidBrush(BorderLeftColor), new Rectangle(0, 0, BorderLeftWidth, Height));
            }

            if (BorderRightWidth > 0)
            {
                g.FillRectangle(new SolidBrush(BorderRightColor), new Rectangle(Width - BorderRightWidth, 0, BorderRightWidth, Height));
            }
        }
    }
}
