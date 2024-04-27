using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ST.Controls;

namespace ST.DataModeler
{
    public partial class RelationControl
    {
        private ContextMenu ContextMenu = new ContextMenu();

        private ToolTip ToolTip;

        private void LoadRelationControlInput()
        {
            // Events
            KeyDown += RelationControl_KeyDown;
            MouseDown += RelationControl_MouseDown;
            MouseMove += RelationControl_MouseMove;
            MouseLeave += RelationControl_MouseLeave;
            GotFocus += RelationControl_GotFocus;
            LostFocus += RelationControl_LostFocus;

            // Context
            ContextMenu.MenuItems.Add(new MenuItem("Edit(&E)", (sender, e) =>
            {
                ShowEditor();
            }));
            ContextMenu.MenuItems.Add(new MenuItem("Delete(&D)", (sender, e) =>
            {
                ShowDeleteMessageBox();
            }));
        }

        private void RelationControl_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                ShowDeleteMessageBox();
            }
            else if (e.KeyCode == Keys.F2)
            {
                ShowEditor();
            }
        }

        private void RelationControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (CanFocus)
                {
                    if (ModifierKeys == Keys.Control)
                    {
                        Focused = !Focused;
                    }
                    else
                    {
                        SetFocusingOutOtherRelationControls(this);
                        Focused = true;
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (CanFocus)
                {
                    if (!Focused)
                    {
                        SetFocusingOutOtherRelationControls(this);
                        Focused = true;
                    }
                    ContextMenu.Show(Target, e.Location);
                }
            }
            HideToolTip();
        }

        private void RelationControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!Focused)
            {
                Status = RelationControlStatus.MouseOver;
            }
            ShowToolTip();
        }

        private void RelationControl_MouseLeave(object sender, EventArgs e)
        {
            if (!Focused)
            {
                Status = RelationControlStatus.None;
            }
            HideToolTip();
        }

        private void RelationControl_GotFocus(object sender, EventArgs e)
        {
            if (ModifierKeys != Keys.Control)
            {
                SetFocusingOutOtherRelationControls(this);
                SetFocusingOutGraphicControls(Target.Controls);
            }

            Status = RelationControlStatus.Selected;
        }

        private void SetFocusingOutGraphicControls(GraphicControlCollection controls)
        {
            foreach (GraphicControl child in controls)
            {
                child.Focused = false;
                if (child.Controls.Count > 0)
                {
                    SetFocusingOutGraphicControls(child.Controls);
                }
            }
        }

        private void RelationControl_LostFocus(object sender, EventArgs e)
        {
            Status = RelationControlStatus.None;
        }

        #region Functions
        private void ShowToolTip()
        {
            if (ToolTip == null)
            {
                string text;
                // Column To Column
                if (Model.NODE_DETAIL_ID2 != string.Empty)
                {
                    text = $"{Model.NODE_ID1}({Model.NODE_SEQ1}).{Model.NODE_DETAIL_ID1} -> " +
                           $"{Model.NODE_ID2}({Model.NODE_SEQ2}).{Model.NODE_DETAIL_ID2}";
                }
                // Column To Table
                else
                {
                    text = $"{Model.NODE_ID1}({Model.NODE_SEQ1}).{Model.NODE_DETAIL_ID1} " +
                           $"{Model.RELATION_OPERATOR} {Model.RELATION_VALUE}" +
                           $" -> {Model.NODE_ID2}({Model.NODE_SEQ2})";
                }

                ToolTip = new ToolTip();
                ToolTip.InitialDelay = 0;
                ToolTip.AutomaticDelay = 0;
                ToolTip.ReshowDelay = 0;
                ToolTip.UseAnimation = false;
                ToolTip.Show(text, Target);
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

        private void ShowEditor()
        {
            string originText;
            string destinationText;
            GetSelectedRelationsOriginTextNDestinationText(out originText, out destinationText);

            ModalBase modal;

            // Column To Column
            if (Model.NODE_DETAIL_ID2 != string.Empty)
            {
                modal = new ModalRelationEditor(Model, originText, destinationText);
            }
            // Column To Table
            else
            {
                modal = new ModalColumnToTableRelationEditor(Model);
            }

            if (modal.ShowDialog() == DialogResult.OK)
            {
                List<RelationModel> targetList = new List<RelationModel>();
                foreach (RelationControl control in Target.Relations)
                {
                    if (control.Status == RelationControlStatus.Selected)
                    {
                        bool hasTarget = false;
                        for(int i = 0; i < targetList.Count; i++)
                        {
                            if (control.Model.NODE_ID1 == targetList[i].NODE_ID1
                            && control.Model.NODE_SEQ1 == targetList[i].NODE_SEQ1
                            && control.Model.NODE_ID2 == targetList[i].NODE_ID2
                            && control.Model.NODE_SEQ2 == targetList[i].NODE_SEQ2)
                            {
                                hasTarget = true;
                                break;
                            }
                        }

                        if (!hasTarget)
                        {
                            targetList.Add(new RelationModel() {
                                NODE_ID1 = control.Model.NODE_ID1
                                , NODE_SEQ1 = control.Model.NODE_SEQ1
                                , NODE_ID2 = control.Model.NODE_ID2
                                , NODE_SEQ2 = control.Model.NODE_SEQ2
                            });
                        }
                    }
                }

                foreach (RelationControl control in Target.Relations)
                {
                    for (int i = 0; i < targetList.Count; i++)
                    {
                        if (control.Model.NODE_ID1 == targetList[i].NODE_ID1
                        && control.Model.NODE_SEQ1 == targetList[i].NODE_SEQ1
                        && control.Model.NODE_ID2 == targetList[i].NODE_ID2
                        && control.Model.NODE_SEQ2 == targetList[i].NODE_SEQ2)
                        {
                            control.Model.RELATION_TYPE = Model.RELATION_TYPE;
                            break;
                        }
                    }
                }
            }
        }

        private void GetSelectedRelationsOriginTextNDestinationText(out string originText, out string destinationText)
        {
            List<string> originList = new List<string>();
            List<string> destinationList = new List<string>();
            foreach (RelationControl control in Target.Relations)
            {
                if (control.Status == RelationControlStatus.Selected)
                {
                    string originNode = string.Format("{0}({1})", control.Model.NODE_ID1, control.Model.NODE_SEQ1);
                    string destinationNode = string.Format("{0}({1})", control.Model.NODE_ID2, control.Model.NODE_SEQ2);
                    if (!(originList.Contains(originNode) && destinationList.Contains(destinationNode)))
                    {
                        originList.Add(originNode);
                        destinationList.Add(destinationNode);
                    }
                }
            }

            for (int i = 0; i < originList.Count; i++)
            {
                int nodeLength = Math.Max(originList[i].Length, destinationList[i].Length);
                originList[i] = originList[i].PadRight(nodeLength, ' ');
                destinationList[i] = destinationList[i].PadRight(nodeLength, ' ');
            }
            originText = string.Join(" ", originList);
            destinationText = string.Join(" ", destinationList);
        }

        private void ShowDeleteMessageBox()
        {
            if (ModalMessageBox.Show("Are you sure you want to delete the selected relations?", "Relation", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                for(int i = Target.Relations.Count - 1; 0 <= i; i--)
                {
                    RelationControl control = Target.Relations[i];
                    if (control.Status == RelationControlStatus.Selected)
                    {
                        Target.Relations.Remove(control);
                    }
                }
                Target.Refresh();
            }
        }

        private void SetFocusingOutOtherRelationControls(RelationControl thisControl)
        {
            foreach (RelationControl control in Target.Relations)
            {
                if (!thisControl.Equals(control))
                {
                    control.Focused = false;
                }
            }
        }
        #endregion
    }
}
