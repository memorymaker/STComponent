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

namespace ST.DataModeler
{
    public partial class ModalRelationEditor : ModalBase
    {
        public RelationModel Model;
        private string OriginText = "";
        private string DestinationText = "";

        public ModalRelationEditor(RelationModel model, string originText = "", string destinationText = "")
        {
            Model = model;
            OriginText = originText;
            DestinationText = destinationText;

            InitializeComponent();
            LoadThis();
        }

        private void LoadThis()
        {
            // ------ Events
            // KeyDown - Enter 
            KeyDown += ControlsEnter_KeyDown;
            txtOrigin.KeyDown += ControlsEnter_KeyDown;
            txtDestination.KeyDown += ControlsEnter_KeyDown;
            cboRelationType.KeyDown += ControlsEnter_KeyDown;

            // This
            Location = new Point(
                  Cursor.Position.X - Width / 2
                , Cursor.Position.Y - Padding.Top - cboRelationType.Top - cboRelationType.Height / 2
            );

            // Default
            DataTable dt = new DataTable();
            dt.AddColumns("{S}ValueMember {S}DisplayMember");
            foreach (RelationControlType type in Enum.GetValues(typeof(RelationControlType)))
            {
                string stringType = type.GetStringValue();
                if (stringType != "")
                {
                    dt.Rows.Add(new object[] { stringType, type.ToString() });
                }
            }
            cboRelationType.DataSource = dt;
            cboRelationType.ValueMember = "ValueMember";
            cboRelationType.DisplayMember = "DisplayMember";
            cboRelationType.DrawItem += CboRelationType_DrawItem;

            // Load Data
            txtOrigin.Text = OriginText != ""
                ? OriginText
                : string.Format("{0}({1})", Model.NODE_ID1, Model.NODE_SEQ1);
            txtDestination.Text = DestinationText != ""
                ? DestinationText
                : string.Format("{0}({1})", Model.NODE_ID2, Model.NODE_SEQ2);
            cboRelationType.SelectedValue = Model.RELATION_TYPE;
        }

        private void CboRelationType_DrawItem(object sender, DrawItemEventArgs e)
        {
            int index = e.Index >= 0 ? e.Index : 0;
            var brush = Brushes.Black;
            e.DrawBackground();
            e.Graphics.DrawString(cboRelationType.Items[index].ToString(), e.Font, brush, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
        }

        private void TxtOrigin_KeyDown(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
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
            Model.RELATION_TYPE = cboRelationType.SelectedValue.ToString();
            DialogResult = DialogResult.OK;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
