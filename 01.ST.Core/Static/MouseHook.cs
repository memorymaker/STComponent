using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ST.Core
{   
    public static class MouseHook
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

        private delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        public delegate void HookMethod(Point point);

        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public Point pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }

        public static Point PointRef = Point.Empty;
        private const int WH_MOUSE_LL = 14;
        private static int HookHandle = 0;
        private static HookProc CallbackDelegate;
        private static HookMethod CallBackMethod;

        public static void StartHook(HookMethod callbackMethod)
        {
            CallBackMethod = callbackMethod;

            CallbackDelegate = new HookProc(CallBack);
            HookHandle = SetWindowsHookEx(WH_MOUSE_LL, CallbackDelegate, IntPtr.Zero, 0);
            if (HookHandle != 0)
            {
                return;
            }
        }

        public static void StopHook()
        {
            UnhookWindowsHookEx(HookHandle);
        }

        private static int CallBack(int nCode, IntPtr wParam, IntPtr lParam)
        {
            MouseHookStruct mouseInput = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
            CallBackMethod(mouseInput.pt);
            return CallNextHookEx(HookHandle, nCode, wParam, lParam);
        }
    }
}
