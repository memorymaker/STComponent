using ST.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.Controls
{
    public partial class ModalBase : Form
    {
        new public string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (base.Text != value)
                {
                    base.Text = value;
                    OnPaint(new PaintEventArgs(CreateGraphics(), Bounds));
                }
            }
        }

        private ST.Controls.GraphicControl BtClose;

        public Color CaptionForeColor
        {
            get
            {
                return _CaptionForeColor;
            }
            set
            {
                if (_CaptionForeColor != value)
                {
                    _CaptionForeColor = value;
                    OnPaint(new PaintEventArgs(CreateGraphics(), Bounds));
                }
            }
        }
        private Color _CaptionForeColor = Color.White;

        public Color CaptionBackColor { get; set; } = Color.FromArgb(52, 122, 182);

        public ModalBase()
        {
            InitializeComponent();
            LoadBorderlessWindow();
            LoadThis();
        }

        private void LoadThis()
        {
            KeyPreview = true;
            KeyDown += ModalBase_KeyDown;
            Paint += ModalBase_Paint;
            SizeChanged += ModalBase_SizeChanged;

            int marginTotal = 4;
            int areaSideLength = Padding.Top - marginTotal;

            BtClose = new ST.Controls.GraphicControl(this, "BtDelete");
            BtClose
                .SetArea(new Rectangle(-(areaSideLength + (marginTotal / 2)), (marginTotal / 2) - 1, areaSideLength, areaSideLength))
                .SetDrawType(ST.Controls.GraphicControl.DrawTypeEnum.DrawLines)
                .SetDrawPositionPercent(new PointF[] {
                      new PointF(0.25f, 0.25f), new PointF(0.75f, 0.75f) // /
                    , new PointF(0.75f, 0.25f), new PointF(0.25f, 0.75f) // \
                })
                .SetDrawColor(ST.Controls.GraphicControl.StateType.Default, Color.FromArgb(220, 231, 241))
                .SetDrawColor(ST.Controls.GraphicControl.StateType.Over, Color.FromArgb(255, 255, 255))
                .SetDrawWeight(1.6f);
            BtClose.Click += (object sender, MouseEventArgs e) =>
            {
                DialogResult = DialogResult.Cancel;
            };
            BtClose.Enabled = true;
        }

        private void ModalBase_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void ModalBase_Paint(object sender, PaintEventArgs e)
        {
            if (Visible && Width > 0 && Height > 0)
            {
                Rectangle ClipRect = new Rectangle(0, 0, Width, Height);
                BufferedGraphicsContext context = new BufferedGraphicsContext();
                context.MaximumBuffer = new Size(Width, Height);
                BufferedGraphics bufferedGraphics = context.Allocate(e.Graphics, ClipRect);

                using (Graphics g = bufferedGraphics.Graphics)
                {
                    var _size = g.MeasureString(Text, Font);
                    int _textHeight = (_size.Height / 2).ToInt();
                    int positionValue = Padding.Top / 2 - _textHeight;

                    g.Clear(BackColor);

                    g.FillRectangle(new SolidBrush(CaptionBackColor), new RectangleF(0, 0, Width, Padding.Top));
                    g.DrawString(Text, Font, new SolidBrush(CaptionForeColor), new Point(positionValue, positionValue));

                    BtClose.Draw(g);

                    bufferedGraphics.Render(e.Graphics);
                }

                bufferedGraphics.Dispose();
                context.Dispose();
            }
        }

        private void ModalBase_SizeChanged(object sender, EventArgs e)
        {
            OnPaint(new PaintEventArgs(CreateGraphics(), new Rectangle(0, 0, Width, Height)));
        }
    }
}