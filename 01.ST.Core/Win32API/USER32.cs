using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.Core
{
    public partial class USER32
    {
        [DllImport("user32")]
        public static extern int IsWindowVisible(IntPtr hwnd);

        #region ShowInactiveTopmost
        private const int SW_SHOWNOACTIVATE = 4;
        private const int HWND_TOPMOST = -1;
        private const uint SWP_NOACTIVATE = 0x0010;

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern bool SetWindowPos(
             int hWnd,             // Window handle
             int hWndInsertAfter,  // Placement-order handle
             int X,                // Horizontal position
             int Y,                // Vertical position
             int cx,               // Width
             int cy,               // Height
             uint uFlags);         // Window positioning flags

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static void ShowInactiveTopmost(Form form)
        {
            ShowWindow(form.Handle, SW_SHOWNOACTIVATE);
            SetWindowPos(form.Handle.ToInt32(), HWND_TOPMOST,
            form.Left, form.Top, form.Width, form.Height,
            SWP_NOACTIVATE);
        }
        #endregion
    }
}
