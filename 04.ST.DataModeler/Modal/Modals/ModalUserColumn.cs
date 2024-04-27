using ST.Controls;
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
using System.Xml.Linq;

namespace ST.DataModeler
{
    public partial class ModalUserColumn : ModalBase
    {
        public string ColumnName = string.Empty;
        public string DataTypeFull = string.Empty;
        public string Comment = string.Empty;
        public string TableAlias = string.Empty;

        public ModalUserColumn(string columnName = "", string dataTypeFull = "", string comment = "", string tableAlias = "")
        {
            ColumnName = columnName;
            DataTypeFull = dataTypeFull;
            Comment = comment;
            TableAlias = tableAlias;

            InitializeComponent();
            LoadThis();
        }

        private void LoadThis()
        {
            // ------ Events
            // KeyDown - Enter 
            KeyDown += ControlsEnter_KeyDown;
            txtColumnName.KeyDown += ControlsEnter_KeyDown;
            txtDataTypeFull.KeyDown += ControlsEnter_KeyDown;
            txtComment.KeyDown += ControlsEnter_KeyDown;
            txtTableAlias.KeyDown += ControlsEnter_KeyDown;

            // This
            Location = new Point(
                  Cursor.Position.X - Width / 2
                , Cursor.Position.Y - Padding.Top - txtColumnName.Top - txtColumnName.Height / 2
            );

            // Load Data
            txtColumnName.Text = ColumnName;
            txtDataTypeFull.Text = DataTypeFull;
            txtComment.Text = Comment;
            txtTableAlias.Text = TableAlias;
        }

        private void ControlsEnter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btApply_Click(null, null);
            }
        }

        private void btApply_Click(object sender, EventArgs e)
        {
            ColumnName = txtColumnName.Text.Trim();
            DataTypeFull = txtDataTypeFull.Text.Trim();
            Comment = txtComment.Text.Trim();
            TableAlias = txtTableAlias.Text.Trim();

            if (ColumnName.Length == 0)
            {
                ModalMessageBox.Show("Please enter the column name.", "User Column");
                txtColumnName.Focus();
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
