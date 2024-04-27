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

namespace UnitTest
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
            Load += Form6_Load;
        }

        DataTable dt = new DataTable();

        private void Form6_Load(object sender, EventArgs e)
        {
            dt.AddColumns("{S}NODE_ID {I}NODE_DETAIL_COUNT {S}NODE_REF_TABLE");

            dt.Rows.Add(new object[] { "C1", 11, "TABLE1, TABLE2, TABLE3, TABLE4, TABLE5, TABLE6" });
            dt.Rows.Add(new object[] { "C2", 12, "TABLE1, TABLE2, TABLE3, TABLE4, TABLE5, TABLE6" });
            dt.Rows.Add(new object[] { "C3", 13, "TABLE1, TABLE2, TABLE3, TABLE4, TABLE5, TABLE6" });
            dt.Rows.Add(new object[] { "C4", 14, "TABLE1, TABLE2, TABLE3, TABLE4, TABLE5, TABLE6" });

            userListView1.AddColumn(new ST.Controls.UserListViewColumn("C_NODE_ID111111", "NODE_ID"));
            userListView1.AddColumn(new ST.Controls.UserListViewColumn("C_NODE_DETAIL_COUNT", "NODE_DETAIL_COUNT"));
            userListView1.AddColumn(new ST.Controls.UserListViewColumn("C_NODE_REF_TABLE", "NODE_REF_TABLE"));

            //userListView1.Columns[0].Width = 20;

            userListView1.AutoSizeType = ST.Controls.UserListAutoSizeType.LeftFirst;
            userListView1.Bind(dt);

            //userListView1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            userListView1.Enabled = !userListView1.Enabled;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            userListView1.Bind(dt);
        }
    }
}
