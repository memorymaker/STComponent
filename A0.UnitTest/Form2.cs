using ST.Controls;
using ST.DAC;
using ST.DataModeler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnitTest
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            LoadThis();
        }

        private void LoadThis()
        {
            // SoundC01();

            // TestExecuteNoneQuery();
            //TestExecuteNoneQueryDelete();
            //TestExecuteQuery1();
            //TestExecuteQuery2();
            //TestExecuteQuery3();
            

            //userScrollBar1.SmallChange = 5;
            //userScrollBar1.LargeChange = 10;
            //userScrollBar1.Maximum = 30;
            //userScrollBar1.Minimum = 0;
            //userScrollBar1.Width = 18;


            listView.Columns.Add(new UserListViewColumn("Field Name", "FieldInfo"));
            listView.Columns.Add(new UserListViewColumn("Data Type", "Comment"));
            listView.Columns.Add(new UserListViewColumn("Field3", "field3"));

            listView.Columns[0].Width = 200;
            listView.Columns[0].ItemAlign = UserListAlignType.Left;
            listView.Columns[1].Width = 100;

            DataTable dt = new DataTable("T_Name");
            dt.Columns.Add("FieldInfo");
            dt.Columns.Add("Comment");
            dt.Columns.Add("field3");
            dt.Columns.Add("field4");
            dt.Rows.Add(new object[] { "GOODS_TM_NO", "d12", "d13", "d14" });
            dt.Rows.Add(new object[] { "MM_RECV_YN", "d22", "d23", "d24" });
            dt.Rows.Add(new object[] { "MM_RECV_DATE", "d32", "d33", "d34" });
            dt.Rows.Add(new object[] { "MM_RECV_USER", "d42", "d43", "d44" });
            dt.Rows.Add(new object[] { "WC_CD", "d52", "d53", "d54" });
            dt.Rows.Add(new object[] { "ITEM_CD", "d62", "d63", "d64" });
            dt.Rows.Add(new object[] { "d71", "d72", "d73", "d74" });
            dt.Rows.Add(new object[] { "d81", "d82", "d83", "d84" });
            dt.Rows.Add(new object[] { "d91", "d92", "d93", "d94" });
            dt.Rows.Add(new object[] { "d101", "d102", "d103", "d104" });
            dt.Rows.Add(new object[] { "d111", "d112", "d113", "d114" });
            dt.Rows.Add(new object[] { "d121", "d122", "d123", "d124" });
            dt.Rows.Add(new object[] { "d81", "d82", "d83", "d84" });
            dt.Rows.Add(new object[] { "d91", "d92", "d93", "d94" });
            dt.Rows.Add(new object[] { "d101", "d102", "d103", "d104" });
            dt.Rows.Add(new object[] { "d111", "d112", "d113", "d114" });
            dt.Rows.Add(new object[] { "d121", "d122", "d123", "d124" });
            dt.Rows.Add(new object[] { "d81", "d82", "d83", "d84" });
            dt.Rows.Add(new object[] { "d91", "d92", "d93", "d94" });
            dt.Rows.Add(new object[] { "d101", "d102", "d103", "d104" });
            dt.Rows.Add(new object[] { "d111", "d112", "d113", "d114" });
            dt.Rows.Add(new object[] { "d121", "d122", "d123", "d124" });
            dt.Rows.Add(new object[] { "d81", "d82", "d83", "d84" });
            dt.Rows.Add(new object[] { "d91", "d92", "d93", "d94" });
            dt.Rows.Add(new object[] { "d101", "d102", "d103", "d104" });
            dt.Rows.Add(new object[] { "d111", "d112", "d113", "d114" });
            dt.Rows.Add(new object[] { "d121", "d122", "d123", "d124" });
            dt.Rows.Add(new object[] { "d81", "d82", "d83", "d84" });
            dt.Rows.Add(new object[] { "d91", "d92", "d93", "d94" });
            dt.Rows.Add(new object[] { "d101", "d102", "d103", "d104" });
            dt.Rows.Add(new object[] { "d111", "d112", "d113", "d114" });
            dt.Rows.Add(new object[] { "d121", "d122", "d123", "d124" });
            dt.Rows.Add(new object[] { "d81", "d82", "d83", "d84" });
            dt.Rows.Add(new object[] { "d91", "d92", "d93", "d94" });
            dt.Rows.Add(new object[] { "d101", "d102", "d103", "d104" });
            dt.Rows.Add(new object[] { "d111", "d112", "d113", "d114" });
            dt.Rows.Add(new object[] { "d121", "d122", "d123", "d124" });


            
            



            listView2.Columns.Add(new UserListViewColumn("Field Name", "FieldInfo"));
            listView2.Columns.Add(new UserListViewColumn("Data Type", "Comment"));

            listView2.Columns[0].Width = 100;
            listView2.Columns[0].ItemAlign = UserListAlignType.Left;
            listView2.Columns[1].Width = 50;

            DataTable dt2 = new DataTable("T_Name");
            dt2.Columns.Add("FieldInfo");
            dt2.Columns.Add("Comment");
            dt2.Columns.Add("field3");
            dt2.Columns.Add("field4");
            dt2.Rows.Add(new object[] { "GOODS_TM_NO", "d12", "d13", "d14" });
            dt2.Rows.Add(new object[] { "MM_RECV_YN", "d22", "d23", "d24" });
            dt2.Rows.Add(new object[] { "MM_RECV_DATE", "d32", "d33", "d34" });
            dt2.Rows.Add(new object[] { "MM_RECV_USER", "d42", "d43", "d44" });
            dt2.Rows.Add(new object[] { "WC_CD", "d52", "d53", "d54" });
            dt2.Rows.Add(new object[] { "ITEM_CD", "d62", "d63", "d64" });
            dt2.Rows.Add(new object[] { "d71", "d72", "d73", "d74" });





            //listView.Bind(dt);
            //listView2.Bind(dt2);

            userTableNode1.ListView.Bind(dt);
            userTableNode2.ListView.Bind(dt2);


            

        }

        //GraphicListView glistViewt = null;

        private void button2_Click(object sender, EventArgs e)
        {
            listView.ScaleValue -= 0.1f;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //glistViewt.Width += 1;
        }
    }
}