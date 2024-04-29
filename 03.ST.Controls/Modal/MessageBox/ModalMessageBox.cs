using ST.Controls.Modal.MessageBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.Controls
{
    public static class ModalMessageBox
    {
        public static DialogResult Show(string text, string caption = "", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
        {
            var modal = new ModalMessageBoxForm(text, caption, buttons, defaultButton, MessageBoxIcon.None);
            return modal.ShowDialog();
        }

        public static DialogResult ShowError(string text, string caption = "", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
        {
            var modal = new ModalMessageBoxForm(text, caption, buttons, defaultButton, MessageBoxIcon.Error);
            return modal.ShowDialog();
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption = "", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1, StartPosition position = StartPosition.CenterParent)
        {
            var modal = new ModalMessageBoxForm(text, caption, buttons, defaultButton, MessageBoxIcon.None, position);
            return modal.ShowDialog(owner);
        }

        public static DialogResult ShowError(IWin32Window owner, string text, string caption = "", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1, StartPosition position = StartPosition.CenterParent)
        {
            var modal = new ModalMessageBoxForm(text, caption, buttons, defaultButton, MessageBoxIcon.Error, position);
            return modal.ShowDialog(owner);
        }

        public enum StartPosition
        {
            Cursor = 0,
            CenterParent
        }
    }
}