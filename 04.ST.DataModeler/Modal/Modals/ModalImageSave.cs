using ST.Controls;
using ST.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ST.DataModeler
{
    public partial class ModalImageSave : ModalBase
    {
        public int MaxMargin = 100;
        public Bitmap Image;
        public Color ImageBackColor;
        public Rectangle ObjectsArea;

        public ModalImageSave(Bitmap image, Rectangle objectsArea, Color imageBackColor)
        {
            Image = image;
            ObjectsArea = objectsArea;
            ImageBackColor = imageBackColor;

            InitializeComponent();
            LoadThis();
        }

        private void LoadThis()
        {
            // ------ Events
            // KeyDown - Enter 
            KeyDown += ControlsEnter_KeyDown;
            txtSize.KeyDown += ControlsEnter_KeyDown;

            txtMargin.KeyDown += ControlsEnter_KeyDown;
            txtMargin.KeyDown += TxtMargin_KeyDown;
            txtMargin.KeyPress += TxtMargin_KeyPress;
            txtMargin.TextChanged += TxtMargin_TextChanged;

            // This
            Location = new Point(
                  Cursor.Position.X - Width / 2
                , Cursor.Position.Y - Padding.Top - txtSize.Top - txtSize.Height / 2
            );

            // Set picMain
            picMain.BackColor = Color.Black;
            picMain.SizeMode = PictureBoxSizeMode.Zoom;

            txtMargin.Text = Properties.ST.Default.ModalImageSaveMargin.ToString();
        }

        private void TxtMargin_KeyDown(object sender, KeyEventArgs e)
        {
            int number;
            int.TryParse(txtMargin.Text, out number);
            switch (e.KeyCode)
            {
                case Keys.Up:
                    txtMargin.Text = (number + 1).ToString();
                    break;
                case Keys.Down:
                    txtMargin.Text = (number - 1).ToString();
                    break;
                case Keys.PageUp:
                    txtMargin.Text = (number + 10).ToString();
                    break;
                case Keys.PageDown:
                    txtMargin.Text = (number - 10).ToString();
                    break;
            }
        }

        private void TxtMargin_TextChanged(object sender, EventArgs e)
        {
            int selectionStart = txtMargin.SelectionStart;
            int rs;
            int.TryParse(txtMargin.Text, out rs);
            rs = Math.Max(Math.Min(rs, MaxMargin), 0);

            txtMargin.Text = rs.ToString();
            txtMargin.SelectionStart = selectionStart;

            Properties.ST.Default.ModalImageSaveMargin = rs;
            Properties.ST.Default.Save();

            SetImage(rs);
        }

        private void TxtMargin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void ControlsEnter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btSave_Click(null, null);
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            string fileName;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Image Save";
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.Filter = "PNG File(*.png)|*.png|JPEG File(*.jpg)|*.jpg|Bitmap File(*.bmp)|*.bmp";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog.FileName;
                picMain.Image.Save(fileName);
                DialogResult = DialogResult.OK;
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void SetImage(int margin = 20)
        {
            txtSize.Text = $"{ObjectsArea.Width + margin * 2} X {ObjectsArea.Height + margin * 2}";

            var tempImage = new Bitmap(ObjectsArea.Width + margin * 2, ObjectsArea.Height + margin * 2);
            var g = Graphics.FromImage(tempImage);

            g.Clear(ImageBackColor);
            g.DrawImage(Image, -ObjectsArea.Left + margin, -ObjectsArea.Top + margin);
            picMain.Image = tempImage;
        }
    }
}
