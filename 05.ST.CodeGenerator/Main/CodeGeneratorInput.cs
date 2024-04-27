using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.CodeGenerator
{
    public partial class CodeGenerator
    {
        private void LoadInput()
        {
            MouseMove += CodeGenerator_MouseMove;

            CommonVariablesEditor.KeyDown += CommonVariablesEditor_KeyDown;
            CommonVariablesEditor.DelayedDataChanged += CommonVariablesEditor_DelayedDataChanged;

            ErrorList.ItemDoubleClick += ErrorList_ItemDoubleClick;
        }

        private void CodeGenerator_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Draw();
        }

        private void CommonVariablesEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                var tab = GetActiveTab();
                if (tab != null)
                {
                    tab.Generate();
                }
            }
        }

        private void CommonVariablesEditor_DataChanged(object sender, Controls.UserEditor.DataEventArgs e)
        {
            var tab = GetActiveTab();
            if (tab != null)
            {
                tab.Generate();
            }
        }

        private void CommonVariablesEditor_DelayedDataChanged(object sender, EventArgs e)
        {
            var tab = GetActiveTab();
            if (tab != null)
            {
                tab.Generate();
            }
        }

        private void ErrorList_ItemDoubleClick(object sender, Controls.UserListView.GraphicListViewEventArgs e)
        {
            int lineIndex = Convert.ToInt32(e.Item.Row["LINE"]) - 1;
            int charIndexOfLine = Convert.ToInt32(e.Item.Row["COLUMN"]) - 1;
            if (lineIndex >= 0)
            {
                int selectionStart = CommonVariablesEditor.GetFirstCharIndexFromLine(lineIndex) + charIndexOfLine;
                CommonVariablesEditor.SelectionStart = selectionStart;
                CommonVariablesEditor.SelectionLength = 0;
                CommonVariablesEditor.Focus();
            }
        }

        #region Override
        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            if (Enabled && !ReadOnly)
            {
                base.OnPreviewKeyDown(e);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (Enabled && !ReadOnly)
            {
                base.OnKeyDown(e);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (Enabled && !ReadOnly)
            {
                base.OnKeyUp(e);
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (Enabled && !ReadOnly)
            {
                base.OnKeyPress(e);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (Enabled && !ReadOnly)
            {
                base.OnMouseDown(e);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Enabled && !ReadOnly)
            {
                base.OnMouseMove(e);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (Enabled && !ReadOnly)
            {
                base.OnMouseUp(e);
            }
        }
        #endregion
    }
}
