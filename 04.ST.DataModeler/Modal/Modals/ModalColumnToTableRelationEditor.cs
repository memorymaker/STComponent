using ST.Controls;
using ST.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public partial class ModalColumnToTableRelationEditor : ModalBase
    {
        public RelationModel Model;

        public ModalColumnToTableRelationEditor(RelationModel model)
        {
            Model = model;

            InitializeComponent();
            LoadThis();
        }

        private void LoadThis()
        {
            // ------ Events
            Shown += ModalColumnToTableRelationEditor_Shown;
            // KeyDown - Enter
            KeyDown += ControlsEnter_KeyDown;
            txtOrigin.KeyDown += ControlsEnter_KeyDown;
            txtDestination.KeyDown += ControlsEnter_KeyDown;
            cboRelationType.KeyDown += ControlsEnter_KeyDown;
            txtOriginColumn.KeyDown += ControlsEnter_KeyDown;
            cboComparisonOperators.KeyDown += ControlsEnter_KeyDown;
            txtValue.KeyDown += ControlsEnter_KeyDown;

            // This
            Location = new Point(
                  Cursor.Position.X - Width / 2
                , Cursor.Position.Y - Padding.Top - cboRelationType.Top - cboRelationType.Height / 2
            );

            // Set cboRelationType
            DataTable dtRelationType = new DataTable();
            dtRelationType.AddColumns("{S}ValueMember {S}DisplayMember");
            foreach (RelationControlType type in Enum.GetValues(typeof(RelationControlType)))
            {
                string stringType = type.GetStringValue();
                if (stringType != "")
                {
                    dtRelationType.Rows.Add(new object[] { stringType, type.ToString() });
                }
            }
            cboRelationType.DataSource = dtRelationType;
            cboRelationType.ValueMember = "ValueMember";
            cboRelationType.DisplayMember = "DisplayMember";
            cboRelationType.DrawItem += CboRelationType_DrawItem;

            // Set cboComparisonOperators
            DataTable dtComparisonOperators = new DataTable();
            dtComparisonOperators.AddColumns("{S}ValueMember {S}DisplayMember");
            dtComparisonOperators.Rows.Add(new object[] { "=", "=" });
            dtComparisonOperators.Rows.Add(new object[] { ">", ">" });
            dtComparisonOperators.Rows.Add(new object[] { "<", "<" });
            dtComparisonOperators.Rows.Add(new object[] { ">=", ">=" });
            dtComparisonOperators.Rows.Add(new object[] { "<=", "<=" });
            dtComparisonOperators.Rows.Add(new object[] { "<>", "<>" });
            dtComparisonOperators.Rows.Add(new object[] { "LIKE", "LIKE" });
            dtComparisonOperators.Rows.Add(new object[] { "IN", "IN" });
            dtComparisonOperators.Rows.Add(new object[] { "BETWEEN", "BETWEEN" });
            dtComparisonOperators.Rows.Add(new object[] { "NOT LIKE", "NOT LIKE" });
            dtComparisonOperators.Rows.Add(new object[] { "NOT IN", "NOT IN" });
            dtComparisonOperators.Rows.Add(new object[] { "NOT BETWEEN", "NOT BETWEEN" });
            cboComparisonOperators.DataSource = dtComparisonOperators;
            cboComparisonOperators.ValueMember = "ValueMember";
            cboComparisonOperators.DisplayMember = "DisplayMember";
            cboComparisonOperators.DrawItem += CboRelationType_DrawItem;

            // Load Data
            txtOrigin.Text = string.Format("{0}({1})", Model.NODE_ID1, Model.NODE_SEQ1);
            txtDestination.Text = string.Format("{0}({1})", Model.NODE_ID2, Model.NODE_SEQ2);
            txtOriginColumn.Text = Model.NODE_DETAIL_ID1;

            cboRelationType.SelectedValue = Model.RELATION_TYPE;
            cboComparisonOperators.SelectedValue = Model.RELATION_OPERATOR;
            txtValue.Text = Model.RELATION_VALUE;
        }

        private void ModalColumnToTableRelationEditor_Shown(object sender, EventArgs e)
        {
            txtValue.Focus();
        }

        private void CboRelationType_DrawItem(object sender, DrawItemEventArgs e)
        {
            int index = e.Index >= 0 ? e.Index : 0;
            var brush = Brushes.Black;
            e.DrawBackground();
            e.Graphics.DrawString(cboRelationType.Items[index].ToString(), e.Font, brush, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
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
            if (txtValue.Text.Trim() == "")
            {
                ModalMessageBox.Show("Plaese enter the value.", "Relation");
                txtValue.Focus();
            }
            else
            {
                Model.RELATION_TYPE = cboRelationType.SelectedValue.ToString();
                Model.RELATION_OPERATOR = cboComparisonOperators.SelectedValue.ToString();
                Model.RELATION_VALUE = txtValue.Text.Trim();
                DialogResult = DialogResult.OK;
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
