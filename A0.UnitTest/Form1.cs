using ST.Controls;
using ST.DAC;
using ST.DataModeler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnitTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadThis();
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            BeginContainerRectangle(e);
        }

        private void BeginContainerRectangle(PaintEventArgs e)
        {
            // Fill untransformed rectangle with green.
            //e.Graphics.FillRectangle(new SolidBrush(Color.Green), 0, 0, 200, 200);

            Rectangle bounds = new Rectangle(10, 50, 100, 100);

            // Define transformation for container.
            Rectangle srcRect = new Rectangle(0, 0, 100, 100);
            Rectangle destRect = bounds;

            // Clip #1
            e.Graphics.Clip = new Region(bounds);

            // Container #1
            GraphicsContainer containerState = e.Graphics.BeginContainer(
                destRect, srcRect,
                GraphicsUnit.Pixel);

            // Draw
            //e.Graphics.FillRectangle(new SolidBrush(Color.Red), 0, 0, 15, 30);
            e.Graphics.Clear(Color.Red);

            // Container #2
            e.Graphics.EndContainer(containerState);

            // Clip #2
            e.Graphics.ResetClip();
        }

        private void LoadThis()
        {
            //dataModeler1.Enabled = false;

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




            DataTable dt = new DataTable("T1");
            dt.Columns.Add("NODE_ID");
            dt.Columns.Add("NODE_SEQ", typeof(int));
			dt.Columns.Add("NODE_DETAIL_ID");
            dt.Columns.Add("NODE_DETAIL_SEQ", typeof(int));
			dt.Columns.Add("NODE_DETAIL_ORDER", typeof(int));
			dt.Columns.Add("NODE_DETAIL_NOTE");
			dt.Columns.Add("NODE_DETAIL_TYPE");
            dt.Columns.Add("NODE_DETAIL_DATA_TYPE");
            dt.Columns.Add("NODE_DETAIL_COMMENT");
            dt.Columns.Add("NODE_DETAIL_TABLE_ALIAS");
            dt.Columns.Add("NODE_DETAIL_VIEW_COLUMN1");
            dt.Columns.Add("NODE_DETAIL_VIEW_COLUMN2");
            dt.Columns.Add("NODE_ID_REF");
            dt.Rows.Add(new object[] { "T1", 0, "GOODS_TM_NO1" , 1, 1, "C", "varchar", "설명1", "", "note", "GOODS_TM_NO1(varchar)" , "설명1", "GOODS_TM_NO1" });
            dt.Rows.Add(new object[] { "T1", 0, "MM_RECV_YN1"  , 2, 2, "C", "varchar", "설명2", "", "note", "MM_RECV_YN1(varchar)"  , "설명2", "MM_RECV_YN1"   });
            dt.Rows.Add(new object[] { "T1", 0, "MM_RECV_DATE1", 3, 3, "C", "varchar", "설명3", "", "note", "MM_RECV_DATE1(varchar)", "설명3", "MM_RECV_DATE1" });
            dt.Rows.Add(new object[] { "T1", 0, "MM_RECV_USER1", 4, 4, "C", "varchar", "설명4", "", "note", "MM_RECV_USER1(varchar)", "설명4", "MM_RECV_USER1" });
            dt.Rows.Add(new object[] { "T1", 0, "WC_CD1"       , 5, 5, "C", "varchar", "설명5", "", "note", "WC_CD1(varchar)"       , "설명5", "WC_CD1"        });



            
            

            DataTable dt2 = new DataTable("NODE_ETC_TABLE");
            dt2.Columns.Add("NODE_ID");
            dt2.Columns.Add("NODE_SEQ", typeof(int));
			dt2.Columns.Add("NODE_DETAIL_ID");
            dt2.Columns.Add("NODE_DETAIL_SEQ", typeof(int));
			dt2.Columns.Add("NODE_DETAIL_ORDER", typeof(int));
            dt2.Columns.Add("NODE_DETAIL_TYPE");
            dt2.Columns.Add("NODE_DETAIL_DATA_TYPE");
            dt2.Columns.Add("NODE_DETAIL_COMMENT");
            dt2.Columns.Add("NODE_DETAIL_TABLE_ALIAS"); 
            dt2.Columns.Add("NODE_DETAIL_NOTE");
            dt2.Columns.Add("NODE_DETAIL_VIEW_COLUMN1");
            dt2.Columns.Add("NODE_DETAIL_VIEW_COLUMN2");
            dt2.Columns.Add("NODE_ID_REF");
            dt2.Columns.Add("NODE_SEQ_REF", typeof(int));
            dt2.Rows.Add(new object[] { "NODE_ETC_TABLE", 0, "GOODS_TM_NO2" , 1, 1, "C", "varchar", "설명1", "", "note", "GOODS_TM_NO2(varchar)" , "설명1", "NODE_ETC_TABLE", 0 });
            dt2.Rows.Add(new object[] { "NODE_ETC_TABLE", 0, "MM_RECV_YN2"  , 2, 2, "C", "varchar", "설명2", "", "note", "MM_RECV_YN2(varchar)"  , "설명2", "NODE_ETC_TABLE", 0 });
            dt2.Rows.Add(new object[] { "NODE_ETC_TABLE", 0, "MM_RECV_DATE2", 3, 3, "C", "varchar", "설명3", "", "note", "MM_RECV_DATE2(varchar)", "설명3", "NODE_ETC_TABLE", 0 });
            dt2.Rows.Add(new object[] { "NODE_ETC_TABLE", 0, "MM_RECV_USER2", 4, 4, "C", "varchar", "설명4", "", "note", "MM_RECV_USER2(varchar)", "설명4", "NODE_ETC_TABLE", 0 });
            dt2.Rows.Add(new object[] { "NODE_ETC_TABLE", 0, "WC_CD2"       , 5, 5, "C", "varchar", "설명5", "", "note", "WC_CD2(varchar)"       , "설명5", "NODE_ETC_TABLE", 0 });
            dt2.Rows.Add(new object[] { "NODE_ETC_TABLE", 0, "ITEM_CD2"     , 6, 6, "C", "varchar", "설명6", "", "note", "ITEM_CD2(varchar)"     , "설명6", "NODE_ETC_TABLE", 0 });
            dt2.Rows.Add(new object[] { "NODE_ETC_TABLE", 0, "ITEM_CD21"    , 7, 7, "C", "varchar", "설명6", "", "note", "ITEM_CD21(varchar)"    , "설명6", "NODE_ETC_TABLE", 0 });
            dt2.Rows.Add(new object[] { "NODE_ETC_TABLE", 0, "ITEM_CD24"    , 8, 8, "C", "varchar", "설명6", "", "note", "ITEM_CD24(varchar)"    , "설명6", "NODE_ETC_TABLE", 0 });
            dt2.Rows.Add(new object[] { "NODE_ETC_TABLE", 0, "ITEM_CD25"    , 9, 9, "C", "varchar", "설명6", "", "note", "ITEM_CD25(varchar)"    , "설명6", "NODE_ETC_TABLE", 0 });


            DataTable dt3 = new DataTable("TABLE_TEST_NODE");
            dt3.Columns.Add("NODE_ID");
            dt3.Columns.Add("NODE_SEQ", typeof(int));
			dt3.Columns.Add("NODE_DETAIL_ID");
            dt3.Columns.Add("NODE_DETAIL_SEQ", typeof(int));
			dt3.Columns.Add("NODE_DETAIL_ORDER", typeof(int));
            dt3.Columns.Add("NODE_DETAIL_TYPE");
            dt3.Columns.Add("NODE_DETAIL_DATA_TYPE");
            dt3.Columns.Add("NODE_DETAIL_COMMENT");
            dt3.Columns.Add("NODE_DETAIL_TABLE_ALIAS");
            dt3.Columns.Add("NODE_DETAIL_NOTE");
            dt3.Columns.Add("NODE_DETAIL_VIEW_COLUMN1");
            dt3.Columns.Add("NODE_DETAIL_VIEW_COLUMN2");
            dt3.Columns.Add("NODE_ID_REF");
            dt3.Columns.Add("NODE_SEQ_REF", typeof(int));
            dt3.Rows.Add(new object[] { "TABLE_TEST_NODE", 0, "GOODS_TM_NO_TEST1234", 1, 1, "C", "varchar", "설명1", "", "note", "GOODS_TM_NO_TEST1234(varchar)", "설명1", "TABLE_TEST_NODE", 0 });
            dt3.Rows.Add(new object[] { "TABLE_TEST_NODE", 0, "MM_RECV_YN2"         , 2, 2, "C", "varchar", "설명2", "", "note", "MM_RECV_YN2(varchar)"         , "설명2", "TABLE_TEST_NODE", 0 });
            dt3.Rows.Add(new object[] { "TABLE_TEST_NODE", 0, "MM_RECV_DATE2"       , 3, 3, "C", "varchar", "설명3", "", "note", "MM_RECV_DATE2(varchar)"       , "설명3", "TABLE_TEST_NODE", 0 });
            dt3.Rows.Add(new object[] { "TABLE_TEST_NODE", 0, "MM_RECV_USER2"       , 4, 4, "C", "varchar", "설명4", "", "note", "MM_RECV_USER2(varchar)"       , "설명4", "TABLE_TEST_NODE", 0 });
            dt3.Rows.Add(new object[] { "TABLE_TEST_NODE", 0, "WC_CD2"              , 5, 5, "C", "varchar", "설명5", "", "note", "WC_CD2(varchar)"              , "설명5", "TABLE_TEST_NODE", 0 });
            dt3.Rows.Add(new object[] { "TABLE_TEST_NODE", 0, "ITEM_CD2"            , 6, 6, "C", "varchar", "설명6", "", "note", "ITEM_CD2(varchar)"            , "설명6", "TABLE_TEST_NODE", 0 });
            dt3.Rows.Add(new object[] { "TABLE_TEST_NODE", 0, "ITEM_CD21"           , 7, 7, "C", "varchar", "설명7", "", "note", "ITEM_CD21(varchar)"           , "설명6", "TABLE_TEST_NODE", 0 });


            

            DataTable dt4 = new DataTable("TABLE_TEST_NODE");
            dt4.Columns.Add("NODE_ID");
            dt4.Columns.Add("NODE_SEQ", typeof(int));
			dt4.Columns.Add("NODE_DETAIL_ID");
            dt4.Columns.Add("NODE_DETAIL_SEQ", typeof(int));
			dt4.Columns.Add("NODE_DETAIL_ORDER", typeof(int));
            dt4.Columns.Add("NODE_DETAIL_TYPE");
            dt4.Columns.Add("NODE_DETAIL_DATA_TYPE");
            dt4.Columns.Add("NODE_DETAIL_COMMENT");
            dt4.Columns.Add("NODE_DETAIL_TABLE_ALIAS");
            dt4.Columns.Add("NODE_DETAIL_NOTE");
            dt4.Columns.Add("NODE_DETAIL_VIEW_COLUMN1");
            dt4.Columns.Add("NODE_DETAIL_VIEW_COLUMN2");
            dt4.Columns.Add("NODE_ID_REF");
            dt4.Columns.Add("NODE_SEQ_REF", typeof(int)); // 
            dt4.Rows.Add(new object[] { "TABLE_TEST_NODE", 1, "GOODS_TM_NO_TEST1234"  , 1, 1, "C", "varchar", "설명1", "", "note", "GOODS_TM_NO_TEST1234(varchar)"  , "설명1", "TABLE_TEST_NODE", 1 });
            dt4.Rows.Add(new object[] { "TABLE_TEST_NODE", 1, "GOODS_TM_NO22_TEST1234", 2, 2, "C", "varchar", "설명1", "", "note", "GOODS_TM_NO22_TEST1234(varchar)", "설명1", "TABLE_TEST_NODE", 1 });
            dt4.Rows.Add(new object[] { "TABLE_TEST_NODE", 1, "MM_RECV_YN2"           , 3, 3, "C", "varchar", "설명2", "", "note", "MM_RECV_YN2(varchar)"           , "설명2", "TABLE_TEST_NODE", 1 });
            dt4.Rows.Add(new object[] { "TABLE_TEST_NODE", 1, "MM_RECV_DATE2"         , 4, 4, "C", "varchar", "설명3", "", "note", "MM_RECV_DATE2(varchar)"         , "설명3", "TABLE_TEST_NODE", 1 });
            dt4.Rows.Add(new object[] { "TABLE_TEST_NODE", 1, "MM_RECV_USER2"         , 5, 5, "C", "varchar", "설명4", "", "note", "MM_RECV_USER2(varchar)"         , "설명4", "TABLE_TEST_NODE", 1 });
            dt4.Rows.Add(new object[] { "TABLE_TEST_NODE", 1, "WC_CD2"                , 6, 6, "C", "varchar", "설명5", "", "note", "WC_CD2(varchar)"                , "설명5", "TABLE_TEST_NODE", 1 });
            dt4.Rows.Add(new object[] { "TABLE_TEST_NODE", 1, "ITEM_CD2"              , 7, 7, "C", "varchar", "설명6", "", "note", "ITEM_CD2(varchar)"              , "설명6", "TABLE_TEST_NODE", 1 });
            dt4.Rows.Add(new object[] { "TABLE_TEST_NODE", 1, "ITEM_CD21"             , 8, 8, "C", "varchar", "설명7", "", "note", "ITEM_CD21(varchar)"             , "설명6", "TABLE_TEST_NODE", 1 });

            //GraphicPanel panel = new GraphicPanel(dataModeler1);
            //panel.Size = new Size(200, 100);
            //panel.Location = new Point(20, 20);
            //panel.BackColor = Color.FromArgb(245, 245, 245);
            //dataModeler1.Controls.Add(panel);


            //GraphicScrollBar scrollBar = new GraphicScrollBar(dataModeler1);
            //scrollBar.Size = new Size(15, 100);
            //scrollBar.Location = new Point(300, 20);
            //dataModeler1.Nodes.Add(scrollBar);


            //GraphicListView glistView = new GraphicListView(dataModeler1);
            //glistView.Size = new Size(200, 200);
            //glistView.Location = new Point(60, 150);
            //glistView.Columns.Add(new GraphicListViewColumn("Field Name", "FieldInfo"));
            //glistView.Columns.Add(new GraphicListViewColumn("Data Type", "Comment"));
            //glistView.Columns.Add(new GraphicListViewColumn("Field3", "field3"));
            //glistView.Bind(dt);
            //dataModeler1.Nodes.Add(glistView);


            //TableNode tableNode = new TableNode(dataModeler1);
            //tableNode.Size = new Size(200, 200);
            //tableNode.Location = new Point(60, 60);
            //tableNode.ListView.Bind(dt);
            //dataModeler1.Nodes.Add(tableNode);

            //for (int i = 0; i < 10; i++)
            //{
            //    TableNode tableNode = new TableNode(dataModeler1);
            //    tableNode.Size = new Size(200, 200);
            //    tableNode.Location = new Point(40 * i, 40 * i);
            //    tableNode.ListView.Bind(dt);
            //    dataModeler1.Controls.Add(tableNode);
            //}

            //TableNode tableNode = new TableNode(dataModeler1);
            //tableNode.Size = new Size(200, 200);
            //tableNode.Location = new Point(40, 40);
            //tableNode.ID = "T1";
            //tableNode.SEQ = 0;
            //tableNode.Bind(dt);
            //dataModeler1.Controls.Add(tableNode);

            TableNode tableNode2 = new TableNode(dataModeler1);
            tableNode2.Size = new Size(200, 200);
            tableNode2.Location = new Point(350, 40);
            tableNode2.ID = "NODE_ETC_TABLE";
            tableNode2.SEQ = 0;
            tableNode2.Bind(dt2);
            dataModeler1.Controls.Add(tableNode2);

            TableNode tableNode3 = new TableNode(dataModeler1);
            tableNode3.Size = new Size(200, 200);
            tableNode3.Location = new Point(250, 300);
            tableNode3.ID = "TABLE_TEST_NODE";
            tableNode3.SEQ = 0;
            tableNode3.Bind(dt3);
            dataModeler1.Controls.Add(tableNode3);

            TableNode tableNode4 = new TableNode(dataModeler1);
            tableNode4.Size = new Size(200, 200);
            tableNode4.Location = new Point(550, 300);
            tableNode4.ID = "TABLE_TEST_NODE";
            tableNode4.SEQ = 1;
            tableNode4.Bind(dt4);
            dataModeler1.Controls.Add(tableNode4);
            tableNode4.BringToFront();


            //ColumnNode columnNode = new ColumnNode(dataModeler1);
            //columnNode.Size = new Size(200, 200);
            //columnNode.Location = new Point(250, 40);
            //columnNode.ID = "C1";
            //columnNode.ListView.Bind(dt2);
            //dataModeler1.Controls.Add(columnNode);

            //ColumnNode columnNode1 = new ColumnNode(dataModeler1);
            //var t = columnNode1.Size;
            //columnNode1.Location = new Point(40, 300);
            //columnNode1.ID = "C1";
            //columnNode1.SEQ = 0;
            //dataModeler1.Controls.Add(columnNode1);

            MemoNode memoNode1 = new MemoNode(dataModeler1);
            memoNode1.Size = new Size(200, 200);
            memoNode1.Location = new Point(40, 40);
            memoNode1.ID = "_MEMO";
            memoNode1.SEQ = 0;
            dataModeler1.Controls.Add(memoNode1);
            memoNode1.BringToFront();
            t = memoNode1;
            //memoNode1.NodeNote = "가나다라마바사아자차아자차12345678911 카타파하 갸냐댜랴 먀뱌샤";


            //GraphicEditor editor = new GraphicEditor(dataModeler1);
            //editor.Width = 200;
            //editor.Height = 100;
            //editor.Left = 100;
            //editor.Top = 100;
            //dataModeler1.Controls.Add(editor);

            //GmemoNode1 = memoNode1;

            //userEditor1.ScrollBars = ScrollBars.None;
            //userEditor1.WordWrap = true;


        }
        MemoNode t;

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //var t1 = dataModeler1.GetNodeModels();
            //var t2 = dataModeler1.GetRelationModels();
            //// glistViewt.Width += 1;

            //Console.WriteLine($"{t1[0].NODE_LEFT} {dataModeler1.InnerLocation.X} {dataModeler1.InnerLocation.X + t1[0].NODE_LEFT}" );

            var t1 = dataModeler1.GetRelationDataTable();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataModeler1.Enabled = !dataModeler1.Enabled;
            //dataModeler1.test1();
        }

        int cnt = 0;
        private void button5_Click(object sender, EventArgs e)
        {
            if (cnt == 0)
            {
                cnt = 1;
                t.NodeNote = "가나다라마ffffffffff77777666\r\n자차12345678911카타파하갸냐댜랴먀뱌샤";
                //t.NodeNote = "가나다라마바사아자차아자차12345678911 카타파하 갸냐댜랴 먀뱌샤";
                //t.NodeNote = "가나다\r\n라마바사\r\n아자차아자\r\n차12345678\r\n 911 \r\n카타파하 갸\r\n냐댜랴 먀뱌샤\r\n1234\r\nsdfe\r\nfew\r\nfd\r\nrewk 234jk \r\n 423jkl \r\nfdwejk\r\n432jk \r\nkl32 v\r\n t4";
                //t.NodeNote = "카타파하 갸\r\n냐댜랴 먀뱌샤\r\n1234\r\nsdfe\r\nfew\r\nfd\r\nrewk 234jk \r\n 423jkl \r\nfdwejk\r\n432jk \r\nkl32 v\r\n t4";
                //t.NodeNote = "";
            }
            else
            {
                cnt = 0;
                //t.NodeNote = "가\r\n나";
                //t.NodeNote = "카타파하 갸\r\n냐댜랴 먀뱌샤\r\n1234\r\nsdfe\r\nfew\r\nfd\r\nrewk 234jk \r\n 423jkl \r\nfdwejk\r\n432jk \r\nkl32 v\r\n t4";
                //t.NodeNote = "카타파하 갸냐댜랴 먀뱌샤 1234\r\nsdfefewfdrewk 234jk 423jkl fdwejk 432jk kl32 vt4";
                t.NodeNote = "";
            }
        }
    }
}