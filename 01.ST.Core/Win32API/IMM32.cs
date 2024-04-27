using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core
{
    public static class IMM32
    {
        public const int GCS_RESULTREADSTR = 0x0200;
        public const int GCS_COMPSTR = 0x0008;

        [Flags]
        public enum ImmAssociateContextExFlags : uint
        {
            IACE_CHILDREN = 0x0001,
            IACE_DEFAULT = 0x0010,
            IACE_IGNORENOCONTEXT = 0x0020
        }

        [DllImport("Imm32.dll")]
        public static extern bool ImmAssociateContextEx(IntPtr hWnd, IntPtr hIMC, ImmAssociateContextExFlags dwFlags);

        [DllImport("Imm32.dll")]
        public static extern int ImmGetContext(IntPtr hWnd);

        [DllImport("Imm32.dll")]
        public static extern int ImmGetCompositionString(int hIMC, int dwIndex, StringBuilder lpBuf, int dwBufLen);

        [DllImport("Imm32.dll")]
        public static extern bool ImmReleaseContext(IntPtr hWnd, int hIMC);
    }
}
