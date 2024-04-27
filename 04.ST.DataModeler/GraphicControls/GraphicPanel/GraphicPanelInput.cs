using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public partial class GraphicPanel
    {
        private ToolTip ToolTip;

        private void LoadInput()
        {
            PreviewKeyDown += GraphicPanel_PreviewKeyDown;

            MouseDown += GraphicPanel_MouseDown;
            MouseMove += GraphicPanel_MouseMove;
            MouseUp += GraphicPanel_MouseUp;
            MouseLeave += GraphicPanel_MouseLeave;
            Click += GraphicPanel_Click;

            TitleEditor.KeyDown += TitleEditor_KeyDown;
            TitleEditor.LostFocus += TitleEditor_LostFocus;
        }

        private void GraphicPanel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // Ctrn + C
            if (e.Control == true && e.KeyCode == Keys.C)
            {
                Clipboard.SetText(this.ID);
            }
            else if (e.KeyCode == Keys.F2)
            {
                if (EnableCaptionEdit)
                {
                    ShowTitleEditor();
                }
            }
        }

        private void GraphicPanel_MouseDown(object sender, MouseEventArgs e)
        {
            HideToolTip();
            BringToFront();
        }

        private void GraphicPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (!TitleEditor.Visible)
            {
                BtDelete.Enabled = e.Y <= TitleHeight;
                BtContextMenu.Enabled = ShowBtContextMenu && e.Y <= TitleHeight;

                if (e.Y <= TitleHeight && TitleOverFlow)
                {
                    ShowToolTip(Title);
                }

                Refresh();
            }
        }

        private void GraphicPanel_MouseUp(object sender, MouseEventArgs e)
        {
            Event.CallEvent(this, this.SizeLocationChanged
                , new string[] { "Left", "Top", "Width", "Height" }
                , new object[] { this.Left, this.Top, this.Width, this.Height }
            );
        }

        private void GraphicPanel_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            BtDelete.Enabled = false;
            BtContextMenu.Enabled = ShowBtContextMenu && false;
            HideToolTip();
            Refresh();
        }

        private void GraphicPanel_Click(object sender, EventArgs e)
        {
            ((GraphicControl)sender).Focus();
        }

        private void TitleEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                TitleEditor.Visible = false;

                Target.Focus();
                Focus();
                e.SuppressKeyPress = true;
                Refresh();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                string oldTitle = TitleEditor.Text;

                TitleEditor.LostFocus -= TitleEditor_LostFocus;
                var _e = new TitleChangingEventArgs(oldTitle, TitleEditor.Text.Trim());
                TitleChanging?.Invoke(this, _e);
                TitleEditor.LostFocus += TitleEditor_LostFocus;

                if (!_e.Cancel)
                {
                    Title = _e.NewTitle;
                    TitleEditor.Visible = false;

                    Target.Focus();
                    Focus();
                    e.SuppressKeyPress = true;
                    Refresh();
                }
            }
        }

        private void TitleEditor_LostFocus(object sender, EventArgs e)
        {
            TitleEditor.Visible = false;
        }

        public void ShowTitleEditor()
        {
            SetTitleEditorBoundsNFont();
            TitleEditor.Text = Title;
            TitleEditor.Visible = true;
            TitleEditor.Focus();
            TitleEditor.SelectionStart = 0;
            TitleEditor.SelectionLength = TitleEditor.Text.Length;
        }

        private void SetTitleEditorBoundsNFont()
        {
            using (var g = Target.CreateGraphics())
            {
                int titleHeightAPaddingTop = TitleHeight + Padding.Top;
                Font scaleFont = new Font(Font.FontFamily, Font.Size * ScaleValue
                    , (TitleBold ? FontStyle.Bold : FontStyle.Regular)
                );
                var scaleFontSize = g.MeasureString(Title, scaleFont);
                int top = ((titleHeightAPaddingTop / 2) - (scaleFontSize.Height / 2) + 1 * ScaleValue).ToInt();
                int left = ((titleHeightAPaddingTop / 2) - (scaleFontSize.Height / 2) + 2 * ScaleValue).ToInt();

                TitleEditor.Font = scaleFont;
                TitleEditor.ForeColor = BackColor;
                TitleEditor.BackColor = ForeColor;
                TitleEditor.Left = Left + left;
                TitleEditor.Top = Top + top;
                TitleEditor.Width = Width - left * 2;
            }
        }

        private void ShowToolTip(string text)
        {
            if (TitleOverFlow)
            {
                if (ToolTip == null)
                {
                    ToolTip = new ToolTip();
                    ToolTip.InitialDelay = 0;
                    ToolTip.AutomaticDelay = 0;
                    ToolTip.ReshowDelay = 0;
                    ToolTip.UseAnimation = false;
                    ToolTip.Show(text, Target);
                }
            }
        }

        private void HideToolTip()
        {
            if (ToolTip != null)
            {
                ToolTip.Dispose();
                ToolTip = null;
            }
        }
    }
}