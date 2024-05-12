using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.Core
{
    public static partial class Extensions
    {
        // Original Code
        // https://stackoverflow.com/questions/13711812/parallel-generation-of-ui

        /// <summary>
        /// An application sends the WM_SETREDRAW message to a window to allow changes in that 
        /// window to be redrawn or to prevent changes in that window from being redrawn.
        /// </summary>
        private const int WM_SETREDRAW = 11;

        /// <summary>
        /// Suspends painting for the target control. Do NOT forget to call EndControlUpdate!!!
        /// </summary>
        /// <param name="control">visual control</param>
        public static void BeginControlUpdate(this Control control)
        {
            if (!control.IsDisposed)
            {
                if (!BeginControlUpdateList.Contains(control))
                {
                    BeginControlUpdateList.Add(control);
                }

                Message msgSuspendUpdate = Message.Create(control.Handle, WM_SETREDRAW, IntPtr.Zero,
                      IntPtr.Zero);

                NativeWindow window = NativeWindow.FromHandle(control.Handle);
                window.DefWndProc(ref msgSuspendUpdate);
            }
        }

        /// <summary>
        /// Resumes painting for the target control. Intended to be called following a call to BeginControlUpdate()
        /// </summary>
        /// <param name="control">visual control</param>
        public static void EndControlUpdate(this Control control, bool refresh = true)
        {
            if (!control.IsDisposed)
            {
                if (BeginControlUpdateList.Contains(control))
                {
                    BeginControlUpdateList.Remove(control);
                }

                // Create a C "true" boolean as an IntPtr
                IntPtr wparam = new IntPtr(1);
                Message msgResumeUpdate = Message.Create(control.Handle, WM_SETREDRAW, wparam,
                      IntPtr.Zero);

                NativeWindow window = NativeWindow.FromHandle(control.Handle);
                window.DefWndProc(ref msgResumeUpdate);
                control.Invalidate();
                if (refresh)
                {
                    control.Refresh();
                }
            }
        }

        private static List<Control> BeginControlUpdateList = new List<Control>();

        public static bool IsBeginControlUpdate(this Control control)
        {
            return BeginControlUpdateList.Contains(control);
        }
    }
}
