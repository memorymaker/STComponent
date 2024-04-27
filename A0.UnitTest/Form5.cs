using ST.CodeGenerator;
using ST.Controls;
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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            Load += Form5_Load;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            codeGenerator1.NodeData = GetTestData();
            codeGenerator1.AddNewTab();


            codeGenerator1.ExecuteButton_Click += CodeGenerator1_ExecuteButton_Click;
            codeGenerator1.HistoryButton_Click += CodeGenerator1_HistoryButton_Click;
            return;

            UserPanel panel1 = new UserPanel();
            panel1.Size = new Size(200, 100);
            panel1.Location = new Point(10, 10);
            panel1.Title = "panel1";
            panel1.UsingMaximize = false;
            panel1.UsingViewContextMenuButton = false;
            panel1.UsingAwaysOnTopMenuButton = false;
            panel1.UsingTitleSlider = true;
            //panel1.Closing += Panel_Closing;

            this.Controls.Add(panel1);
            panel1.Dock = DockStyle.Fill;

            for (int i = 2; i <= 8; i ++)
            {
                UserPanel panelSub = new UserPanel();
                panelSub.Size = new Size(200, 100);
                panelSub.Location = new Point(210, 10);
                panelSub.Title = $"panel{i}";
                panelSub.UsingMaximize = false;
                panelSub.UsingViewContextMenuButton = false;
                panelSub.UsingAwaysOnTopMenuButton = false;
                panelSub.UsingTitleSlider = true;
                //panel.Closing += Panel_Closing;

                panel1.AddPanel(panelSub);
            }
        }

        private void CodeGenerator1_ExecuteButton_Click(object sender, EventArgs e)
        {
            var tab = sender as Tab;
            ModalMessageBox.Show(tab.Title);
        }

        private void CodeGenerator1_HistoryButton_Click(object sender, EventArgs e)
        {
            var tab = sender as Tab;
            ModalMessageBox.Show(tab.Title);
        }

        private Dictionary<string, DataTable> GetTestData()
        {
            DataTable dt = new DataTable("C1");
            dt.Columns.Add("NODE_ID");
            dt.Columns.Add("NODE_SEQ", typeof(int));
            dt.Columns.Add("NODE_DETAIL_ID");
            dt.Columns.Add("NODE_DETAIL_SEQ", typeof(int));
            dt.Columns.Add("NODE_DETAIL_ORDER", typeof(int));
            dt.Columns.Add("NODE_DETAIL_NOTE");
            dt.Columns.Add("NODE_DETAIL_VIEW_COLUMN1");
            dt.Columns.Add("NODE_DETAIL_VIEW_COLUMN2");
            dt.Columns.Add("NODE_ID_REF");
            dt.Rows.Add(new object[] { "T1", 0, "GOODS_TM_NO1", 1, 1, "", "GOODS_TM_NO1(varchar)", "설명1", "GOODS_TM_NO1" });
            dt.Rows.Add(new object[] { "T1", 0, "MM_RECV_YN1", 2, 2, "", "MM_RECV_YN1(varchar)", "설명2", "MM_RECV_YN1" });
            dt.Rows.Add(new object[] { "T1", 0, "MM_RECV_DATE1", 3, 3, "", "MM_RECV_DATE1(varchar)", "설명3", "MM_RECV_DATE1" });
            dt.Rows.Add(new object[] { "T1", 0, "MM_RECV_USER1", 4, 4, "", "MM_RECV_USER1(varchar)", "설명4", "MM_RECV_USER1" });
            dt.Rows.Add(new object[] { "T1", 0, "WC_CD1", 5, 5, "", "WC_CD1(varchar)", "설명5", "WC_CD1" });

            DataTable dt2 = new DataTable("C2");
            dt2.Columns.Add("NODE_ID");
            dt2.Columns.Add("NODE_SEQ", typeof(int));
            dt2.Columns.Add("NODE_DETAIL_ID");
            dt2.Columns.Add("NODE_DETAIL_SEQ", typeof(int));
            dt2.Columns.Add("NODE_DETAIL_ORDER", typeof(int));
            dt2.Columns.Add("NODE_DETAIL_NOTE");
            dt2.Columns.Add("NODE_DETAIL_VIEW_COLUMN1");
            dt2.Columns.Add("NODE_DETAIL_VIEW_COLUMN2");
            dt2.Columns.Add("NODE_ID_REF");
            dt2.Rows.Add(new object[] { "T2", 0, "GOODS_TM_NO2", 1, 1, "", "GOODS_TM_NO2(varchar)", "설명1", "GOODS_TM_NO2" });
            dt2.Rows.Add(new object[] { "T2", 0, "MM_RECV_YN2", 2, 2, "", "MM_RECV_YN2(varchar)", "설명2", "MM_RECV_YN2" });
            dt2.Rows.Add(new object[] { "T2", 0, "MM_RECV_DATE2", 3, 3, "", "MM_RECV_DATE2(varchar)", "설명3", "MM_RECV_DATE2" });
            dt2.Rows.Add(new object[] { "T2", 0, "MM_RECV_USER2", 4, 4, "", "MM_RECV_USER2(varchar)", "설명4", "MM_RECV_USER2" });
            dt2.Rows.Add(new object[] { "T2", 0, "WC_CD2", 5, 5, "", "WC_CD2(varchar)", "설명5", "WC_CD2" });
            dt2.Rows.Add(new object[] { "T2", 0, "ITEM_CD2", 6, 6, "", "ITEM_CD2(varchar)", "설명6", "ITEM_CD2" });
            dt2.Rows.Add(new object[] { "T2", 0, "ITEM_CD21", 7, 7, "", "ITEM_CD21(varchar)", "설명6", "ITEM_CD21" });
            dt2.Rows.Add(new object[] { "T2", 0, "ITEM_CD24", 8, 8, "", "ITEM_CD24(varchar)", "설명6", "ITEM_CD24" });
            dt2.Rows.Add(new object[] { "T2", 0, "ITEM_CD25", 9, 9, "", "ITEM_CD25(varchar)", "설명6", "ITEM_CD25" });

            Dictionary<string, DataTable> rs = new Dictionary<string, DataTable>
            {
                { "c1", dt },
                { "c2", dt2 }
            };
            return rs;
        }

        private void Panel_Closing(object sender, UserPanelClosingEventArgs e)
        {
            if (ModalMessageBox.Show("delete?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            codeGenerator1.Enabled = !codeGenerator1.Enabled;

            return;
            var t = new List<TabModel>()
            {
                new TabModel()
                {
                    TEMPLATE_SEQ = 1,
                    TEMPLATE_TITLE = "Title1",
                    TEMPLATE_CONTENT = "c1",
                    TEMPLATE_RESULT = "r1",
                    TEMPLATE_SORT = 0,
                    TEMPLATE_NOTE = "",
                    TEMPLATE_SELECTED = false
                },
                new TabModel()
                {
                    TEMPLATE_SEQ = 2,
                    TEMPLATE_TITLE = "Title2",
                    TEMPLATE_CONTENT = "c2",
                    TEMPLATE_RESULT = "r2",
                    TEMPLATE_SORT = 1,
                    TEMPLATE_NOTE = "",
                    TEMPLATE_SELECTED = true
                },
                new TabModel()
                {
                    TEMPLATE_SEQ = 3,
                    TEMPLATE_TITLE = "Title3",
                    TEMPLATE_CONTENT = "c3",
                    TEMPLATE_RESULT = "r3",
                    TEMPLATE_SORT = 2,
                    TEMPLATE_NOTE = "",
                    TEMPLATE_SELECTED = false
                }
            };
            codeGenerator1.SetData(t);

            //codeGenerator1.Clear();
            //var asdf = codeGenerator1.GetDataTable();
        }
    }
}
