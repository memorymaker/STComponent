using ST.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ST.Controls.ModalMessageBox;

namespace ST.Controls.Modal.MessageBox
{
    public partial class ModalMessageBoxForm : ModalBase
    {
        private int ButtonsDistance = 10;
        private int ButtonWidth = 80;
        private int MaxLabelWidth = 600;
        private MessageBoxIcon MessageBoxIcon = MessageBoxIcon.None;
        
        public ModalMessageBoxForm(string text, string caption, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1, MessageBoxIcon messageBoxIcon = MessageBoxIcon.None, StartPosition position = ModalMessageBox.StartPosition.Cursor)
        {
            InitializeComponent();
            LoadThis(text, caption, buttons, defaultButton, messageBoxIcon, position);
        }

        private void LoadThis(string text, string caption, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1, MessageBoxIcon messageBoxIcon = MessageBoxIcon.None, StartPosition position = ModalMessageBox.StartPosition.Cursor)
        {
            // BasePanel
            Paint += ModalMessageBoxForm_Paint;

            ModalButton focusedButton = null;
            MessageBoxIcon = messageBoxIcon;

            Text = caption;
            LabelText.Text = text;
            if (LabelText.Width > MaxLabelWidth)
            {
                int height = LabelText.Height;
                int width = LabelText.Width;
                LabelText.AutoSize = false;
                LabelText.Size = new Size(MaxLabelWidth, Math.Ceiling(width.ToFloat() / MaxLabelWidth).ToInt() * height);
            }

            if (messageBoxIcon == MessageBoxIcon.None)
            {
                Width = Math.Max(LabelText.Width + 20 * 2, ButtonWidth * 3 + ButtonsDistance * 4);
                Height = Padding.Vertical + PanelBottom.Height + LabelText.Height + 20 * 2;
                LabelText.Location = new Point(20, (Height - Padding.Top - PanelBottom.Height - LabelText.Height) / 2 + Padding.Top);
            }
            else
            {
                Width = Math.Max(LabelText.Width + 40 + 20 * 2, ButtonWidth * 3 + ButtonsDistance * 4);
                Height = Padding.Vertical + PanelBottom.Height + Math.Max(30, LabelText.Height) + 20 * 2;
                LabelText.Location = new Point(40 + 20, (Height - Padding.Top - PanelBottom.Height - LabelText.Height) / 2 + Padding.Top);
            }

            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    {
                        ModalButton buttonOK = CreateModalButton(Color.FromArgb(52, 122, 182), "OK", 1, 1);

                        if (defaultButton == MessageBoxDefaultButton.Button1)
                        {
                            buttonOK.Focus();
                            focusedButton = buttonOK;
                        }
                    }
                    break;
                case MessageBoxButtons.OKCancel:
                    {
                        ModalButton buttonOK = CreateModalButton(Color.FromArgb(52, 122, 182), "OK", 1, 2);
                        ModalButton buttonCancel = CreateModalButton(Color.FromArgb(105, 105, 105), "Cancel", 2, 2);

                        switch (defaultButton)
                        {
                            case MessageBoxDefaultButton.Button1:
                                buttonOK.Focus();
                                focusedButton = buttonOK;
                                break;
                            case MessageBoxDefaultButton.Button2:
                                buttonCancel.Focus();
                                focusedButton = buttonCancel;
                                break;
                        }
                    }
                    break;
                case MessageBoxButtons.YesNo:
                    {
                        ModalButton buttonYes = CreateModalButton(Color.FromArgb(52, 122, 182), "Yes", 1, 2);
                        ModalButton buttonNo = CreateModalButton(Color.FromArgb(52, 122, 182), "No", 2, 2);

                        switch (defaultButton)
                        {
                            case MessageBoxDefaultButton.Button1:
                                buttonYes.Focus();
                                focusedButton = buttonYes;
                                break;
                            case MessageBoxDefaultButton.Button2:
                                buttonNo.Focus();
                                focusedButton = buttonNo;
                                break;
                        }
                    }
                    break;
                case MessageBoxButtons.YesNoCancel:
                    {
                        ModalButton buttonYes = CreateModalButton(Color.FromArgb(52, 122, 182), "Yes", 1, 3);
                        ModalButton buttonNo = CreateModalButton(Color.FromArgb(52, 122, 182), "No", 2, 3);
                        ModalButton buttonCancel = CreateModalButton(Color.FromArgb(105, 105, 105), "Cancel", 3, 3);

                        switch (defaultButton)
                        {
                            case MessageBoxDefaultButton.Button1:
                                buttonYes.Focus();
                                focusedButton = buttonYes;
                                break;
                            case MessageBoxDefaultButton.Button2:
                                buttonNo.Focus();
                                focusedButton = buttonNo;
                                break;
                            case MessageBoxDefaultButton.Button3:
                                buttonCancel.Focus();
                                focusedButton = buttonCancel;
                                break;
                        }
                    }
                    break;
                case MessageBoxButtons.RetryCancel:
                    {
                        ModalButton buttonRetry = CreateModalButton(Color.FromArgb(52, 122, 182), "Retry", 1, 2);
                        ModalButton buttonCancel = CreateModalButton(Color.FromArgb(105, 105, 105), "Cancel", 2, 2);

                        switch (defaultButton)
                        {
                            case MessageBoxDefaultButton.Button1:
                                buttonRetry.Focus();
                                focusedButton = buttonRetry;
                                break;
                            case MessageBoxDefaultButton.Button2:
                                buttonCancel.Focus();
                                focusedButton = buttonCancel;
                                break;
                        }
                    }
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    {
                        ModalButton buttonAbort = CreateModalButton(Color.FromArgb(52, 122, 182), "Abort", 1, 3);
                        ModalButton buttonRetry = CreateModalButton(Color.FromArgb(52, 122, 182), "Retry", 2, 3);
                        ModalButton buttonIgnore = CreateModalButton(Color.FromArgb(52, 122, 182), "Ignore", 3, 3);

                        switch (defaultButton)
                        {
                            case MessageBoxDefaultButton.Button1:
                                buttonAbort.Focus();
                                focusedButton = buttonAbort;
                                break;
                            case MessageBoxDefaultButton.Button2:
                                buttonRetry.Focus();
                                focusedButton = buttonRetry;
                                break;
                            case MessageBoxDefaultButton.Button3:
                                buttonIgnore.Focus();
                                focusedButton = buttonIgnore;
                                break;
                        }
                    }
                    break;
            }

            int maxX = SystemInformation.VirtualScreen.Width - Width;
            int maxY = SystemInformation.VirtualScreen.Height - Height;

            Point point = Point.Empty;
            if (position == ModalMessageBox.StartPosition.Cursor)
            {
                point = focusedButton == null 
                    ? new Point(Cursor.Position.X - Width / 2, Cursor.Position.Y - Height - Padding.Top + PanelBottom.Height / 2 + Padding.Top)
                    : new Point(Cursor.Position.X - focusedButton.Left - focusedButton.Width / 2, Cursor.Position.Y - Height - Padding.Top + PanelBottom.Height / 2 + Padding.Top);
            }
            else if (position == ModalMessageBox.StartPosition.CenterParent)
            {
                StartPosition = FormStartPosition.CenterParent;
            }

            Location = new Point(Math.Min(Math.Max(point.X, 0), maxX), Math.Min(Math.Max(point.Y, 0), maxY));
        }

        private void ModalMessageBoxForm_Paint(object sender, PaintEventArgs e)
        {
            if (MessageBoxIcon == MessageBoxIcon.Error)
            {
                Point point = new Point(20, (Height - Padding.Top - PanelBottom.Height - Properties.Resource.ErrorIcon.Height) / 2);
                e.Graphics.DrawImage(Properties.Resource.ErrorIcon, new Rectangle(point, Properties.Resource.ErrorIcon.Size));
            }
        }

        private ModalButton CreateModalButton(Color backColor, string text, int buttonSort, int buttonTotalCount)
        {
            ModalButton button = new ModalButton();
            button.Top = 8;
            button.Left = (Width - ButtonWidth * buttonTotalCount - ButtonsDistance * (buttonTotalCount - 1)) / 2 + (buttonSort - 1) * (ButtonWidth + ButtonsDistance);
            button.Size = new Size(80, 26);
            button.Margin = new Padding(3, 4, 3, 4);
            button.BackColor = backColor;
            button.Text = text;
            button.Visible = true;
            button.Click += Button_Click;

            PanelBottom.Controls.Add(button);
            return button;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            ModalButton _this = (ModalButton)sender;
            switch(_this.Text)
            {
                case "OK": DialogResult = DialogResult.OK; break;
                case "Yes": DialogResult = DialogResult.Yes; break;
                case "No": DialogResult = DialogResult.No; break;
                case "Abort": DialogResult = DialogResult.Abort; break;
                case "Retry": DialogResult = DialogResult.Retry; break;
                case "Ignore": DialogResult = DialogResult.Ignore; break;
                case "Cancel": DialogResult = DialogResult.Cancel; break;
            }
            Close();
        }
    }
}
