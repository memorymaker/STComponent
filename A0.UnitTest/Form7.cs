using ST.CodeGenerator;
using ST.Controls;
using ST.Core;
using ST.Core.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace UnitTest
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
            Load += Form7_Load;
        }

        //test
        private void T1()
        {
            var t0 = "ABC_DEF_GHIJ".ToAlias();

            List<object[]> t = new List<object[]>()
            {
                  new object[]{ "ABC_DEF_GHIJ1", "ADG" }
            };
            var t1 = "ABC_DEF_GHIJ".ToAlias(t, 0, 1);

            t = new List<object[]>()
            {
                  new object[]{ "ABC_DEF_GHIJ1", "ADG" }
                , new object[]{ "ABC_DEF_GHIJ1", "ADGJ" }
            };
            var t2 = "ABC_DEF_GHIJ".ToAlias(t, 0, 1);

            t = new List<object[]>()
            {
                  new object[]{ "ABC_DEF_GHIJ1", "ADG" }
                , new object[]{ "ABC_DEF_GHIJ1", "ADGJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ADFGJ" }
            };
            var t3 = "ABC_DEF_GHIJ".ToAlias(t, 0, 1);

            t = new List<object[]>()
            {
                  new object[]{ "ABC_DEF_GHIJ1", "ADG" }
                , new object[]{ "ABC_DEF_GHIJ1", "ADGJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ADFGJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ACDFGJ" }
            };
            var t4 = "ABC_DEF_GHIJ".ToAlias(t, 0, 1);

            t = new List<object[]>()
            {
                  new object[]{ "ABC_DEF_GHIJ1", "ADG" }
                , new object[]{ "ABC_DEF_GHIJ1", "ADGJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ADFGJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ACDFGJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ACDFGIJ" }
            };
            t4 = "ABC_DEF_GHIJ".ToAlias(t, 0, 1);

            t = new List<object[]>()
            {
                  new object[]{ "ABC_DEF_GHIJ1", "ADG" }
                , new object[]{ "ABC_DEF_GHIJ1", "ADGJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ADFGJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ACDFGJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ACDFGIJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ACDEFGIJ" }
            };
            t4 = "ABC_DEF_GHIJ".ToAlias(t, 0, 1);

            t = new List<object[]>()
            {
                  new object[]{ "ABC_DEF_GHIJ1", "ADG" }
                , new object[]{ "ABC_DEF_GHIJ1", "ADGJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ADFGJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ACDFGJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ACDFGIJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ACDEFGIJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ABCDEFGIJ" }
            };
            t4 = "ABC_DEF_GHIJ".ToAlias(t, 0, 1);

            t = new List<object[]>()
            {
                  new object[]{ "ABC_DEF_GHIJ1", "ADG" }
                , new object[]{ "ABC_DEF_GHIJ1", "ADGJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ADFGJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ACDFGJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ACDFGIJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ACDEFGIJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ABCDEFGIJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ABCDEFGHIJ" }
            };
            t4 = "ABC_DEF_GHIJ".ToAlias(t, 0, 1);

            t = new List<object[]>()
            {
                  new object[]{ "ABC_DEF_GHIJ1", "ADG" }
                , new object[]{ "ABC_DEF_GHIJ1", "ADGJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ADFGJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ACDFGJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ACDFGIJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ACDEFGIJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ABCDEFGIJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ABCDEFGHIJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ABCDEFGHIJ1" }
            };
            t4 = "ABC_DEF_GHIJ".ToAlias(t, 0, 1);

            t = new List<object[]>()
            {
                  new object[]{ "ABC_DEF_GHIJ1", "ADG" }
                , new object[]{ "ABC_DEF_GHIJ1", "ADGJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ADFGJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ACDFGJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ACDFGIJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ACDEFGIJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ABCDEFGIJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ABCDEFGHIJ" }
                , new object[]{ "ABC_DEF_GHIJ1", "ABCDEFGHIJ1" }
                , new object[]{ "ABC_DEF_GHIJ1", "ABCDEFGHIJ2" }
            };
            t4 = "ABC_DEF_GHIJ".ToAlias(t, 0, 1);
        }

        // Test
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }
        protected override void OnPaint(PaintEventArgs e)
        {
        }

        UserSplitContainer split = null;
        CodeGenerator codeGenerator;

        private void Form7_Load(object sender, EventArgs e)
        {
            split = new UserSplitContainer();
            split.Orientation = Orientation.Vertical;
            split.Dock = DockStyle.Fill;
            split.MouseLeave += Split_MouseLeave;
            Controls.Add(split);

            Button bt2 = new Button();
            split.Panel1.Controls.Add(bt2);
            bt2.Location = new Point(10, 10);
            bt2.Visible = true;
            bt2.Click += Bt2_Click;


            UserPanel userPanel1 = new UserPanel();
            userPanel1.Title = "Test1";
            userPanel1.Dock = DockStyle.Fill;
            split.Panel2.Controls.Add(userPanel1);


            Button bt1 = new Button();
            bt1.Visible = true;
            userPanel1.Controls.Add(bt1);
            bt1.Location = new Point(100, 100);


            UserPanel panel1 = new UserPanel();
            panel1.Title = "Test2";

            codeGenerator = new CodeGenerator();
            codeGenerator.Dock = DockStyle.Fill;
            panel1.Controls.Add(codeGenerator);

            userPanel1.AddPanel(panel1);

            //for(int i = 0; i < 20; i++)
            //{
            //    var t = new UserListView();
            //}

            //var t1 = 1;
            codeGenerator.Clear();
            //codeGenerator.AddNewTab();
            //codeGenerator.AddNewTab();
            //codeGenerator.AddNewTab();
            //codeGenerator.AddNewTab();
            //codeGenerator.AddNewTab();
            //codeGenerator.AddNewTab();
            //codeGenerator.AddNewTab();
            //codeGenerator.AddNewTab();
            //codeGenerator.AddNewTab();
            //codeGenerator.AddNewTab();
            //codeGenerator.AddNewTab();
            codeGenerator.AddNewTab();
            codeGenerator.AddNewTab();
            var t2 = 2;


            DataTable dt1 = new DataTable();
            dt1.AddColumns("{s}TableID {i}TableSeq {s}TableAlias {s}ColumnID {s}DataType {s}Comment");
            dt1.Rows.Add(new object[] {"Table1", 0, "T1_1", "CODE_GROUP", "NVARCHAR(10)", "그룹코드" });
            dt1.Rows.Add(new object[] {"Table1", 0, "T1_1", "CODE_NAME", "NVARCHAR(50)", "그룹명" });
            dt1.Rows.Add(new object[] {"Table1", 0, "T1_1", "CODE_REF", "NVARCHAR(50)", "그룹 참조 값" });
            dt1.Rows.Add(new object[] {"Table1", 0, "T1_1", "INSERT_USER", "NVARCHAR(20)", "등록자" });
            //dt1.Rows.Add(new object[] { "Table2", 0, "T2", "INSERT_DATE2", "DATETIME", "등록일자" });
            //dt1.Rows.Add(new object[] { "Table21", 0, "T21", "INSERT_DATE21", "DATETIME", "등록일자" });
            dt1.Rows.Add(new object[] { "Table3", 0, "T3_1", "INSERT_DATE3", "DATETIME", "등록일자" });
            //dt1.Rows.Add(new object[] { "Table3", 1, "T3_2", "INSERT_DATE3", "DATETIME", "등록일자" });
            dt1.Rows.Add(new object[] { "Table3", 0, "T3_1", "INSERT_DATE3", "DATETIME", "등록일자" });
            //dt1.Rows.Add(new object[] { "Table3", 0, "T3", "INSERT_DATE3", "DATETIME", "등록일자" });
            //dt1.Rows.Add(new object[] { "Table4", 0, "T4", "INSERT_DATE4", "DATETIME", "등록일자" });
            //dt1.Rows.Add(new object[] { "Table41", 0, "T41", "INSERT_DATE41", "DATETIME", "등록일자" });
            //dt1.Rows.Add(new object[] { "Table51", 0, "T51", "INSERT_DATE51", "DATETIME", "등록일자" });

            codeGenerator.NodeData.Add("list", dt1);


            DataTable dtRelation = new DataTable();
            dtRelation.AddColumns("{s}TableID1 {i}TableSeq1 {s}TableAlias1 {s}ColumnID1 {s}TableID2 {s}TableSeq2 {s}TableAlias2 {s}ColumnID2 {i}ColumnOrder2 {s}JoinType {s}JoinOperator {s}JoinValue");
            dtRelation.Rows.Add(new object[] { "Table1", 0, "T1", "Table1KeyColumn1", "Table2", 0, "T1_1", "Table2KeyColumn1", 0, "I", "=", "" });
            dtRelation.Rows.Add(new object[] { "Table1", 0, "T1", "Table1KeyColumn2", "Table2", 0, "T1_1", "Table2KeyColumn2", 0, "I", "=", "" });

            dtRelation.Rows.Add(new object[] { "Table1", 0, "T1", "", "Table2", 0, "T1_1", "Table2KeyColumn3", 0, "I", "=", "Value1" });

            dtRelation.Rows.Add(new object[] { "Table1", 0, "T1", "Table1KeyColumn1", "Table21", 0, "T21", "Table21KeyColumn1", 0, "I", "=", "" });
            dtRelation.Rows.Add(new object[] { "Table1", 0, "T1", "Table1KeyColumn2", "Table21", 0, "T21", "Table21KeyColumn2", 0, "I", "=", "" });
            dtRelation.Rows.Add(new object[] { "Table2", 0, "T2", "Table2KeyColumn1", "Table3", 0, "T3", "Table3KeyColumn1", 0, "L", "=", "" });
            dtRelation.Rows.Add(new object[] { "Table2", 0, "T2", "Table2KeyColumn2", "Table3", 0, "T3", "Table3KeyColumn2", 0, "L", "=", "" });
            dtRelation.Rows.Add(new object[] { "Table3", 0, "T3", "Table3KeyColumn1", "Table4", 0, "T4", "Table4KeyColumn1", 0, "L", "=", "" });
            dtRelation.Rows.Add(new object[] { "Table3", 0, "T3", "Table3KeyColumn2", "Table4", 0, "T4", "Table4KeyColumn2", 0, "L", "=", "" });
            dtRelation.Rows.Add(new object[] { "Table21", 0, "T21", "Table21KeyColumn1", "Table41", 0, "T41", "Table3KeyColumn1", 0, "L", "=", "" });
            dtRelation.Rows.Add(new object[] { "Table21", 0, "T21", "Table21KeyColumn2", "Table41", 0, "T41", "Table3KeyColumn2", 0, "L", "=", "" });
            dtRelation.Rows.Add(new object[] { "Table3", 0, "T3", "Table3KeyColumn1", "Table41", 0, "T41", "Table41KeyColumn3", 0, "L", "=", "" });
            dtRelation.Rows.Add(new object[] { "Table3", 0, "T3", "Table3KeyColumn2", "Table41", 0, "T41", "Table41KeyColumn4", 0, "L", "=", "" });
            dtRelation.Rows.Add(new object[] { "Table41", 0, "T41", "Table41KeyColumn1", "Table51", 0, "T51", "Table51KeyColumn1", 0, "L", "=", "" });
            dtRelation.Rows.Add(new object[] { "Table41", 0, "T41", "Table41KeyColumn2", "Table51", 0, "T51", "Table51KeyColumn2", 0, "L", "=", "" });

            dtRelation.Rows.Add(new object[] { "Table1", 0, "", "Table1KeyColumn1", "Table3", 1, "", "Table3KeyColumn1", 0, "I", "=", "" });
            dtRelation.Rows.Add(new object[] { "Table1", 0, "", "Table1KeyColumn2", "Table3", 1, "", "Table3KeyColumn2", 0, "I", "=", "" });


            codeGenerator.RelationData = dtRelation;


            CodeGenerator.NODE.NODE_ID_REF = "TableID";
            CodeGenerator.NODE.NODE_SEQ_REF = "TableSeq";
            CodeGenerator.NODE.NODE_DETAIL_TABLE_ALIAS = "TableAlias";
            CodeGenerator.NODE.NODE_DETAIL_ID = "ColumnID";

            CodeGenerator.RELATION.NODE_ID2 = "TableID1";
            CodeGenerator.RELATION.NODE_SEQ2 = "TableSeq1";
            CodeGenerator.RELATION.NODE_DETAIL_TABLE_ALIAS2 = "TableAlias1";
            CodeGenerator.RELATION.NODE_DETAIL_ID2 = "ColumnID1";
            CodeGenerator.RELATION.NODE_ID1 = "TableID2";
            CodeGenerator.RELATION.NODE_SEQ1 = "TableSeq2";
            CodeGenerator.RELATION.NODE_DETAIL_TABLE_ALIAS1 = "TableAlias2";
            CodeGenerator.RELATION.NODE_DETAIL_ID1 = "ColumnID2";
            CodeGenerator.RELATION.NODE_DETAIL_ORDER1 = "ColumnOrder2";
            CodeGenerator.RELATION.RELATION_TYPE = "JoinType";
            CodeGenerator.RELATION.RELATION_OPERATOR = "JoinOperator";
            CodeGenerator.RELATION.RELATION_VALUE = "JoinValue";


            //SizeChanged += Form7_SizeChanged;
        }

        private void Split_MouseLeave(object sender, EventArgs e)
        {
            Console.WriteLine("mouse leave");
        }

        private void Bt2_Click(object sender, EventArgs e)
        {
            //codeGenerator.Enabled = !codeGenerator.Enabled;
            codeGenerator.BeginControlUpdate();
            codeGenerator.Clear();
            codeGenerator.AddNewTab();
            codeGenerator.AddNewTab();
            codeGenerator.AddNewTab();
            codeGenerator.AddNewTab();
            codeGenerator.EndControlUpdate();
        }

        private void Form7_SizeChanged(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}
