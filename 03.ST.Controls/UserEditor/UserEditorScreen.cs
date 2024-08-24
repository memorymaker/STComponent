using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ST.Controls
{
    public partial class UserEditor
    {
        #region VScroll
        public new UserScrollBar VScroll;
        public readonly int DefaultVScrollWidth = 17;
        private int VScrollValue = 0;
        private int VScrollMaxValue = 0;
        #endregion

        #region HScroll
        public new UserScrollBar HScroll;
        public readonly int DefaultHScrollHeight = 17;
        private int HScrollValue = 0;
        private int HScrollMaxValue = 0;
        #endregion

        public ScrollBars ScrollBars
        {
            get
            {
                return _ScrollBars;
            }
            set
            {
                switch(_ScrollBars)
                {
                    case ScrollBars.Both:
                        HScroll.Visible = true;
                        VScroll.Visible = true;
                        break;
                    case ScrollBars.Horizontal:
                        HScroll.Visible = true;
                        VScroll.Visible = false;
                        break;
                    case ScrollBars.Vertical:
                        HScroll.Visible = false;
                        VScroll.Visible = true;
                        break;
                    case ScrollBars.None:
                        HScroll.Visible = false;
                        VScroll.Visible = false;
                        break;
                }

                _ScrollBars = value;
            }
        }
        private ScrollBars _ScrollBars = ScrollBars.Both;

        /// <summary>
        /// 커서 정보 표시 여부를 가져오거나 설정합니다.
        /// </summary>
        public bool ShowSelectoinInfo
        {
            get
            {
                return _ShowSelectoinInfo;
            }
            set
            {
                if (_ShowSelectoinInfo != value)
                {
                    _ShowSelectoinInfo = value;
                    HScroll.Width = _ShowSelectoinInfo
                        ? Width - Draw.SelectionInfoWidth
                        : Width - VScroll.Width;

                    Draw.Draw();
                    HScroll.Draw();
                }
            }
        }
        private bool _ShowSelectoinInfo = true;

        private int PageLineSize
        {
            get
            {
                return (Height - (HScroll.Visible ? HScroll.Height : 0) - Draw.PaddingTop) / Draw.FontPixelSize.Height;
            }
        }

        private int PagePixelSize
        {
            get
            {
                return Height - (HScroll.Visible ? HScroll.Height : 0) - Draw.PaddingTop;
            }
        }

        private int PageMoveLineSize
        {
            get
            {
                return PageLineSize - 1;
            }
        }

        private int PageMovePixelSize
        {
            get
            {
                return PagePixelSize - Draw.FontPixelSize.Height - 1; // - 1 : Revision
            }
        }

        private int PageHorizonLineSize
        {
            get
            {
                return (Width - (ScrollBars == ScrollBars.Both || ScrollBars == ScrollBars.Vertical ? VScroll.Width : 0) - Draw.PaddingLeft) / Draw.FontPixelSize.Width;
            }
        }

        private int PageHorizonPixelSize
        {
            get
            {
                return Width - (ScrollBars == ScrollBars.Both || ScrollBars == ScrollBars.Vertical ? VScroll.Width : 0) - Draw.PaddingLeft;
            }
        }

        private int PageHorizonMoveLineSize
        {
            get
            {
                return PageHorizonLineSize - 1;
            }
        }

        private int PageHorizonMovePixelSize
        {
            get
            {
                return PageHorizonPixelSize - Draw.FontPixelSize.Width;
            }
        }

        private void LoadScreen()
        {
            VScroll = new UserScrollBar();
            VScroll.BlockDrawing = true;
            VScroll.Type = UserScrollBarType.Vertical;
            VScroll.Cursor = Cursors.Default;
            VScroll.Maximum = 0;
            VScroll.Minimum = 0;
            VScroll.SmallChange = 1;
            VScroll.Value = 0;
            VScroll.Width = DefaultVScrollWidth;
            VScroll.BackColor = SystemColors.Control;
            VScroll.TabStop = false;
            VScroll.DisableBrightnessColorPoint = -0.1f;
            Controls.Add(VScroll);
            VScroll.BringToFront();
            VScroll.BlockDrawing = false;

            HScroll = new UserScrollBar();
            HScroll.BlockDrawing = true;
            HScroll.Type = UserScrollBarType.Horizontal;
            HScroll.Cursor = Cursors.Default;
            HScroll.Maximum = 0;
            HScroll.Minimum = 0;
            HScroll.SmallChange = 1;
            HScroll.Value = 0;
            HScroll.Height = DefaultHScrollHeight;
            HScroll.BackColor = SystemColors.Control;
            HScroll.TabStop = false;
            HScroll.DisableBrightnessColorPoint = -0.1f;
            Controls.Add(HScroll);
            HScroll.BringToFront();
            HScroll.BlockDrawing = false;

            VScroll.GotFocus += Scroll_GotFocus; 
            HScroll.GotFocus += Scroll_GotFocus;
            VScroll.ValueChanged += VScroll_ValueChanged;
            HScroll.ValueChanged += HScroll_ValueChanged;
            VScroll.MouseDown += Scroll_MouseDown;
            HScroll.MouseDown += Scroll_MouseDown;
        }

        private void Scroll_MouseDown(object sender, MouseEventArgs e)
        {
            ActiveControl = null;
        }

        private void Scroll_GotFocus(object sender, EventArgs e)
        {
            ActiveControl = null;
        }

        private void SetScroll()
        {
            this.VScrollMaxValue = ((Data.LineLength == 0 ? 1 : Data.LineLength) - 1) * Draw.FontPixelSize.Height;
            this.VScroll.Maximum = (this.VScrollMaxValue / Draw.FontPixelSize.Height);
            this.VScroll.LargeChange = this.PageMoveLineSize;

            var maxTextWidth = Data.GetMaxLineTextByteLength() * Draw.FontPixelSize.Width + 1; // + 1 : Selection Width
            this.HScrollMaxValue = maxTextWidth <= this.PageHorizonPixelSize ? 0 : maxTextWidth - this.PageHorizonPixelSize;
            this.HScroll.Maximum = (int)Math.Ceiling((double)this.HScrollMaxValue / Draw.FontPixelSize.Width);
            this.HScroll.LargeChange = this.PageHorizonMoveLineSize;
        }

        private void SetVScrollValue(int value)
        {
            this.VScrollValue = value > 0 ? 0 : ((this.VScrollMaxValue * -1) > value ? (this.VScrollMaxValue * -1) : value);
            this.VScroll.Value = (int)Math.Ceiling(((double)this.VScrollValue / Draw.FontPixelSize.Height) * -1);
            this.VScroll_ValueChanged(null, null);
        }

        private void SetHScrollValue(int value)
        {
            this.HScrollValue = value > 0 ? 0 : ((this.HScrollMaxValue * -1) > value ? (this.HScrollMaxValue * -1) : value);
            this.HScroll.Value = (int)Math.Ceiling(((double)this.HScrollValue / Draw.FontPixelSize.Width) * -1);
            this.HScroll_ValueChanged(null, null);
        }
        
        public void SetScrollToCursor()
        {
            this.SetVScrollToCusror();
            this.SetHScrollToCusror();
        }

        private void SetVScrollToCusror()
        {
            int lineIndex = Data.GetLineFromCharIndex(Selection.Start);

            if (Selection.IndexReference >= 0 && Selection.IndexReference <= Selection.Start && Selection.Length > 0)
            {
                lineIndex = Data.GetLineFromCharIndex(Selection.Start + Selection.Length);
            }
            else if (IsMouseDown && Selection.IndexReference >= 0 && Selection.IndexReference <= Selection.Start)
            {
                lineIndex = Data.GetLineFromCharIndex(Selection.Start + Selection.Length);
            }

            int selectionStartTop = (lineIndex * Draw.FontPixelSize.Height);
            int selectionStartBottom = selectionStartTop + Draw.FontPixelSize.Height;

            if (this.VScrollValue > this.PagePixelSize - selectionStartBottom)
            {
                this.SetVScrollValue(this.PagePixelSize - selectionStartBottom);
            }
            else if (this.VScrollValue < selectionStartTop * -1)
            {
                this.SetVScrollValue(selectionStartTop * -1);
            }
        }

        private void SetHScrollToCusror()
        {
            int targetIndex, sp;

            if ((Selection.IndexReference < 0) || Selection.Start < Selection.IndexReference)
            {
                targetIndex = Selection.Start;
                sp = Data.GetLineStartIndexFromIndex(targetIndex);
            }
            else
            {
                targetIndex = Selection.Start + Selection.Length;
                sp = Data.GetLineStartIndexFromIndex(targetIndex);
            }
            
            if (Data.SB.Length < targetIndex)
            {
                targetIndex = Data.SB.Length;
            }

            int selectionLeft = Selection.GetTextWidth(Data.SB.ToString(sp, targetIndex - sp), this.Font);
            if (selectionLeft >= PageHorizonPixelSize)
            {
                selectionLeft = selectionLeft + 1; // + 1 : Selection Width
            }

            if (this.HScrollValue > this.PageHorizonPixelSize - selectionLeft)
            {
                this.SetHScrollValue(this.PageHorizonPixelSize - selectionLeft);
            }
            else if (this.HScrollValue < selectionLeft * -1)
            {
                this.SetHScrollValue(selectionLeft * -1);
            }
        }
    }
}
