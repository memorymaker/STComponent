using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core
{
    public class GDI32
    {
        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern bool TextOut(IntPtr hdc, int nXStart, int nYStart, string lpString, int cbString);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern int SetTextColor(IntPtr hdc, int crColor);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern int SetBkColor(IntPtr hdc, int crColor);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetTextExtentPoint32(IntPtr hdc, string lpString, int cbString, out Size lpSize);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern bool Rectangle(IntPtr hdc, int ulCornerX, int ulCornerY, int lrCornerX, int lrCornerY);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern int SetDCPenColor(IntPtr hdc, int crColor);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern int SetDCBrushColor(IntPtr hdc, int crColor);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetStockObject(int fnObject);

        [DllImport("gdi32.dll")]
        public static extern bool MoveToEx(IntPtr hdc, int X, int Y, IntPtr lpPoint);

        [DllImport("gdi32.dll")]
        public static extern bool LineTo(IntPtr hdc, int nXEnd, int nYEnd);

        public static void DrawRactangle(IntPtr hdc, int left, int top, int width, int height)
        {
            IntPtr intPtrRef = IntPtr.Zero;
            GDI32.MoveToEx(hdc, left, top, intPtrRef);
            GDI32.LineTo(hdc, left + width, top);
            GDI32.LineTo(hdc, left + width, top + height);
            GDI32.LineTo(hdc, left, top + height);
            GDI32.LineTo(hdc, left, top);
        }

        public enum StockObjects
        {
            WHITE_BRUSH = 0,
            LTGRAY_BRUSH = 1,
            GRAY_BRUSH = 2,
            DKGRAY_BRUSH = 3,
            BLACK_BRUSH = 4,
            NULL_BRUSH = 5,
            HOLLOW_BRUSH = NULL_BRUSH,
            WHITE_PEN = 6,
            BLACK_PEN = 7,
            NULL_PEN = 8,
            OEM_FIXED_FONT = 10,
            ANSI_FIXED_FONT = 11,
            ANSI_VAR_FONT = 12,
            SYSTEM_FONT = 13,
            DEVICE_DEFAULT_FONT = 14,
            DEFAULT_PALETTE = 15,
            SYSTEM_FIXED_FONT = 16,
            DEFAULT_GUI_FONT = 17,
            DC_BRUSH = 18,
            DC_PEN = 19,
        }

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern int SetBkMode(IntPtr hdc, int iBkMode);

        public enum BkModes
        {
            MODE_TRANSPARENT = 1,
            MODE_OPAQUE = 2
        }
    }
}
