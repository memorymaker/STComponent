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

namespace Sample
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
            LoadThis();
        }

        private void LoadThis()
        {
            userListView1.AllowDrag = false;
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            userListView1.Clear();
        }

        private void btAddColumn_Click(object sender, EventArgs e)
        {
            userListView1.Columns.Clear();
            userListView1.AddColumn(new ST.Controls.UserListViewColumn("ID"   , "CODE_ID"   ));
            userListView1.AddColumn(new ST.Controls.UserListViewColumn("Name" , "CODE_NAME" ));
            userListView1.AddColumn(new ST.Controls.UserListViewColumn("Order", "CODE_ORDER"));
            userListView1.AddColumn(new ST.Controls.UserListViewColumn("Note" , "CODE_NOTE" ));
            userListView1.AutoSizeType = ST.Controls.UserListAutoSizeType.LeftFirst;
        }

        private void btBindData_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.AddColumns("{S}CODE_ID {S}CODE_NAME {I}CODE_ORDER {S}CODE_NOTE");

            dt.Rows.Add(new object[] { "POSI0001", "사원"  , 1, "Note 1" });
            dt.Rows.Add(new object[] { "POSI0002", "대리"  , 2, "Note 2" });
            dt.Rows.Add(new object[] { "POSI0003", "과장"  , 3, "Note 3" });
            dt.Rows.Add(new object[] { "POSI0004", "차장"  , 4, "Note 4" });
            dt.Rows.Add(new object[] { "POSI0005", "부장"  , 5, "Note 5" });
            dt.Rows.Add(new object[] { "RESP0001", "파트장", 1, "Note 6" });
            dt.Rows.Add(new object[] { "RESP0002", "팀장"  , 2, "Note 7" });
            dt.Rows.Add(new object[] { "RESP0003", "본부장", 3, "Note 8" });

            userListView1.Bind(dt);
        }

        private void btSetStyle_Click(object sender, EventArgs e)
        {
            try
            {
                // Column
                userListView1.Columns[0].Font = new Font("맑은 고딕", 9f, FontStyle.Bold | FontStyle.Italic);
                userListView1.Columns[1].BackColor = Color.FromArgb(255, 200, 200);
                userListView1.Columns[2].ForeColor = Color.FromArgb(0, 0, 240);

                // Items(Row)
                userListView1.Items[1].BackColor = Color.FromArgb(230, 230, 255);
                userListView1.Items[2].ForeColor = Color.FromArgb(255, 0, 0);
                userListView1.Items[3].Font = new Font("맑은 고딕", 9f, FontStyle.Bold | FontStyle.Italic);

                // Items(SubItem)
                userListView1.Items[5].SubItems[0].BackColor = Color.FromArgb(230, 255, 230);
                userListView1.Items[5].SubItems[1].ForeColor = Color.FromArgb(0, 0, 255);
                userListView1.Items[5].SubItems["CODE_NOTE"].Font = new Font("맑은 고딕", 9f, FontStyle.Bold | FontStyle.Italic);

                userListView1.Draw();
            }
            catch(Exception ex)
            {
                ModalMessageBox.Show(ex.Message, "Set Style");
            }
        }

        private void btScalePlus_Click(object sender, EventArgs e)
        {
            userListView1.ScaleValue += 0.1f;
        }

        private void btScaleMinus_Click(object sender, EventArgs e)
        {
            userListView1.ScaleValue -= 0.1f;
        }

        private void btSetColor_Click(object sender, EventArgs e)
        {

        }

    }
}
