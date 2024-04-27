using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.Controls
{
    /*
    Original Code
    https://jailbreakvideo.ru/shadow-and-mouse-move-for-borderless-windows-forms-application
    */

    public partial class ModalBase
    {
        public bool UseWindowStateMaximized = false;

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        private bool AeroEnabled = true;

        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;
        private const int WM_NCLBUTTONDBLCLK = 0xA3;

        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]

        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
            );

        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                AeroEnabled = CheckAeroEnabled();
                CreateParams cp = base.CreateParams;
                if (!AeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        private void LoadBorderlessWindow()
        {
            AeroEnabled = true;
            FormBorderStyle = FormBorderStyle.None;
        }

        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0;
                DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }

        protected override void WndProc(ref Message m)
        {
            Point pos = PointToClient(Cursor.Position);

            if (!BtClose.Bounds.Contains(pos))
            {

                switch (m.Msg)
                {
                    case WM_NCPAINT:
                        if (AeroEnabled)
                        {
                            var v = 2;
                            DwmSetWindowAttribute(Handle, 2, ref v, 4);
                            MARGINS margins = new MARGINS()
                            {
                                bottomHeight = 1,
                                leftWidth = 0,
                                rightWidth = 0,
                                topHeight = 0
                            };
                            DwmExtendFrameIntoClientArea(Handle, ref margins);

                            using (var g = CreateGraphics())
                            {
                                g.DrawString(Text, Font, new SolidBrush(Color.White), new Point(0, 0));
                            }
                        }
                        break;
                    case WM_NCLBUTTONDBLCLK:
                        if (!UseWindowStateMaximized)
                        {
                            return;
                        }
                        break;
                    default: break;
                }
            }

            base.WndProc(ref m);

            if (!BtClose.Bounds.Contains(pos))
            {
                

                if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT)
                {
                    m.Result = (IntPtr)HTCAPTION;
                }

                if (BtClose.State != ST.Controls.GraphicControl.StateType.Over)
                {
                    BtClose.State = ST.Controls.GraphicControl.StateType.Over;
                    OnPaint(new PaintEventArgs(CreateGraphics(), new Rectangle(0, 0, Width, Height)));
                }
            }
            else
            {
                if (BtClose.State != ST.Controls.GraphicControl.StateType.Default)
                {
                    BtClose.State = ST.Controls.GraphicControl.StateType.Default;
                    OnPaint(new PaintEventArgs(CreateGraphics(), new Rectangle(0, 0, Width, Height)));
                }
            }
        }
    }
}
