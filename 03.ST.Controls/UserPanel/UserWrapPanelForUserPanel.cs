using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.Controls
{
    public class UserWrapPanelForUserPanel : Panel
    {
        public bool IsParentFormResizeBegin = false;
        public Size OldSizeForParentFormResize = new Size(0, 0);
        public Size OldSizeForThis = new Size(0, 0);

        public List<UserPanel> HiddenPanels => _HiddenPanels;
		private List<UserPanel> _HiddenPanels = new List<UserPanel>();

        public UserWrapPanelForUserPanel()
        {
            LoadThis();
        }

        private void LoadThis()
        {
            SizeChanged += UserWrapPanelForUserPanel_SizeChanged;
            ParentChanged += UserWrapPanelForUserPanel_ParentChanged;
        }

        private void UserWrapPanelForUserPanel_SizeChanged(object sender, EventArgs e)
        {
            var _this = (Panel)sender;
            if (IsParentFormResizeBegin)
            {
                ResizeControls(OldSizeForParentFormResize, IsParentFormResizeBegin);
            }
            else
            {
                ResizeControls(OldSizeForThis, IsParentFormResizeBegin);
            }
            OldSizeForThis = Size;
        }

        private void ResizeControls(Size oldSize, bool parentFormResize)
        {
            var _this = this;
            if (oldSize.Width != 0 && oldSize.Height != 0 && _this.Width != 0 && _this.Height != 0)
            {
                {
                    // ------------ Scaling
                    decimal xScale = Convert.ToDecimal(_this.Width) / oldSize.Width;
                    decimal yScale = Convert.ToDecimal(_this.Height) / oldSize.Height;
                    foreach (Control control in _this.Controls)
                    {
                        UserPanel panel = control as UserPanel;
                        if (panel != null)
                        {
                            if (parentFormResize)
                            {
                                panel.Bounds = new Rectangle(
                                    Convert.ToInt32(Math.Round(panel.NBounds.Left * xScale))
                                    , Convert.ToInt32(Math.Round(panel.NBounds.Top * yScale))
                                    , Convert.ToInt32(Math.Round(panel.NBounds.Width * xScale))
                                    , Convert.ToInt32(Math.Round(panel.NBounds.Height * yScale))
                                );
                            }
                            else
                            {
                                panel.Bounds = new Rectangle(
                                    Convert.ToInt32(Math.Round(panel.Bounds.Left * xScale))
                                    , Convert.ToInt32(Math.Round(panel.Bounds.Top * yScale))
                                    , Convert.ToInt32(Math.Round(panel.Bounds.Width * xScale))
                                    , Convert.ToInt32(Math.Round(panel.Bounds.Height * yScale))
                                );
                            }
                        }
                    }

                    // ------------ Revision
                    foreach (Control control in _this.Controls)
                    {
                        UserPanel panel = control as UserPanel;
                        if (panel != null && panel.PositionInfo != null)
                        {
                            if (panel.PositionInfo.SiblingLeft.Count > 0 && panel.PositionInfo.SiblingLeft[0].Right != panel.Left)
                            {
                                var revision = panel.PositionInfo.SiblingLeft[0].Right - panel.Left;
                                panel.Left += revision;
                                panel.Width -= revision;
                            }

                            if (panel.PositionInfo.SiblingTop.Count > 0 && panel.PositionInfo.SiblingTop[0].Bottom != panel.Top)
                            {
                                var revision = panel.PositionInfo.SiblingTop[0].Bottom - panel.Top;
                                panel.Top += revision;
                                panel.Height -= revision;
                            }
                        }
                    }
                }
            }
        }

        private void UserWrapPanelForUserPanel_ParentChanged(object sender, EventArgs e)
        {
            SetParentForm();
        }

        public void SetParentForm()
        {
            Form parentForm = GetTopParentForm(this);
            if (parentForm != null)
            {
                SetParentFormEvent(parentForm);
            }
        }

        private Form GetTopParentForm(Control control)
        {
            if (control.Parent != null)
            {
                if (control.Parent is Form)
                {
                    if (control.Parent.Parent == null)
                    {
                        return control.Parent as Form;
                    }
                    else
                    {
                        return GetTopParentForm(control.Parent.Parent) as Form;
                    }
                }
                else
                {
                    return GetTopParentForm(control.Parent) as Form;
                }
            }
            else
            {
                return null;
            }
        }

        private void SetParentFormEvent(Form parentForm)
        {
            parentForm.ResizeBegin -= ParentForm_ResizeBegin;
            parentForm.ResizeEnd -= ParentForm_ResizeEnd;
            parentForm.ResizeBegin += ParentForm_ResizeBegin;
            parentForm.ResizeEnd += ParentForm_ResizeEnd;
        }

        private void ParentForm_ResizeBegin(object sender, EventArgs e)
        {
            foreach (Control control in Controls)
            {
                UserPanel panel = control as UserPanel;
                if (panel != null)
                {
                    panel.SetNboundsNPosotionInfo(Size, this);
                }
            }
            OldSizeForParentFormResize = Size;
            IsParentFormResizeBegin = true;
        }

        private void ParentForm_ResizeEnd(object sender, EventArgs e)
        {
            OldSizeForParentFormResize = new Size(0, 0);
            IsParentFormResizeBegin = false;
        }

        public void AddPanel(UserPanel newPanel)
        {
            newPanel.WrapPanel = this;
			Controls.Add(newPanel);
            newPanel.BringToFront();
        }

        public void AddHiddenPanel(UserPanel panel)
        {
            if (!_HiddenPanels.Contains(panel))
            {
                _HiddenPanels.Add(panel);
            }
		}

        public void RemoveHiddenPanel(UserPanel panel){
            if (_HiddenPanels.Contains(panel))
            {
                _HiddenPanels.Remove(panel);
            }
        }

        public UserPanel GetUserPanel(string guid)
        {
            UserPanel rs = GetUserPanel_GetPanel(Controls, guid);
            if (rs == null)
            {
                for (int i = 0; i < _HiddenPanels.Count; i++)
                {
                    if (_HiddenPanels[i].GUID == guid)
                    {
                        rs = _HiddenPanels[i];
                        break;
                    }
                }
            }
            return rs;
        }

        private UserPanel GetUserPanel_GetPanel(ControlCollection controls, string guid)
        {
            UserPanel rs = null;
            foreach(Control control in controls)
            {
                UserPanel panel = control as UserPanel;
                if (panel != null)
                {
                    if (panel.GUID == guid)
                    {
                        rs = panel;
                        break;
                    }
                    else
                    {
                        rs = GetUserPanel_GetPanel(panel.Controls, guid);
                        if (rs != null)
                        {
                            break;
                        }
                    }
                }
            }
            return rs;
        }
    }
}
