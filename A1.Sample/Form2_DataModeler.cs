using ST.DataModeler;
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
using ST.Controls;
using GraphicControl = ST.DataModeler.GraphicControl;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Xml.Linq;

namespace Sample
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            dataModeler.Clear();
        }

        private void btAddTableNode_Click(object sender, EventArgs e)
        {
            List<string> duplicatedNodeList = new List<string>();

            // Modeler
            dataModeler.BeginControlUpdate();

            // ------------ SYS_USER
            string nodeID1 = "SYS_USER";

            // SYS_USER - Data
            DataTable tableNode1Data = GetEmptyNodeDataTable();
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "USER_ID"       , 0, 1 , "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(30)" , "사용자 ID"      , "", 1 , "Y", "USER_ID [varchar2(30)]"       , "사용자 ID"       });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "USER_NAME"     , 0, 2 , "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(50)" , "사용자명"       , "", 2 , "N", "USER_NAME [varchar2(50)]"     , "사용자명"        });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "USER_GROUP"    , 0, 3 , "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(30)" , "사용자 그룹"    , "", 3 , "N", "USER_GROUP [varchar2(30)]"    , "사용자 그룹"     });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "USER_PASSWD"   , 0, 4 , "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(200)", "사용자 비밀번호", "", 4 , "N", "USER_PASSWD [varchar2(200)]"  , "사용자 비밀번호" });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "USER_POSITION" , 0, 5 , "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(30)" , "사용자 직위"    , "", 5 , "N", "USER_POSITION [varchar2(30)]" , "사용자 직위"     });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "USER_RESPONS"  , 0, 6 , "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(30)" , "사용자 직책"    , "", 6 , "N", "USER_RESPONS [varchar2(30)]"  , "사용자 직책"     });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "EMP_NO"        , 0, 7 , "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(10)" , "사원 번호"      , "", 7 , "N", "EMP_NO [varchar2(10)]"        , "사원 번호"       });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "EMAIL_ADDRESS" , 0, 8 , "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(100)", "이메일 주소"    , "", 8 , "N", "EMAIL_ADDRESS [varchar2(100)]", "이메일 주소"     });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "MOBILE_PHONE"  , 0, 9 , "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(50)" , "휴대폰 번호"    , "", 9 , "N", "MOBILE_PHONE [varchar2(50)]"  , "휴대폰 번호"     });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "USE_YN"        , 0, 10, "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(1)"  , "사용 여부"      , "", 10, "N", "USE_YN [varchar2(1)]"         , "사용 여부"       });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "NOTE"          , 0, 11, "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(500)", "비고"           , "", 11, "N", "NOTE [varchar2(500)]"         , "비고"            });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "INSERT_USER_ID", 0, 12, "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(30)" , "등록자 ID"      , "", 12, "N", "INSERT_USER_ID [varchar2(30)]", "등록자 ID"       });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "INSERT_DATE"   , 0, 13, "C", "", "SYS_USER", "DATE"    , "DATE"         , "등록 일시"      , "", 13, "N", "INSERT_DATE [date]"           , "등록 일시"       });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "UPDATE_USER_ID", 0, 14, "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(30)" , "수정자 ID"      , "", 14, "N", "UPDATE_USER_ID [varchar2(30)]", "수정자 ID"       });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "UPDATE_DATE"   , 0, 15, "C", "", "SYS_USER", "DATE"    , "DATE"         , "수정 일시"      , "", 15, "N", "UPDATE_DATE [date]"           , "수정 일시"       });

            // SYS_USER - Node
            TableNode tableNode1 = new TableNode(dataModeler);
            tableNode1.Location = new Point(200, 100);
            tableNode1.ID = nodeID1;
            tableNode1.SEQ = 0;
            tableNode1.Bind(tableNode1Data);
            if (!dataModeler.ContainsNode(tableNode1))
            {
                dataModeler.Controls.Add(tableNode1);
            }
            else
            {
                duplicatedNodeList.Add($"[{nodeID1}]");
            }

            // ------------ SYS_CODE
            string nodeID2 = "SYS_CODE";

            // SYS_CODE - Data
            DataTable tableNode2Data = GetEmptyNodeDataTable();
            tableNode2Data.Rows.Add(new object[] { nodeID2, 0, "CODE_ID"       , 0, 1, "C", "", "SYS_CODE", "VARCHAR2", "VARCHAR2(30)" , "코드 ID"  , "", 1 , "Y", "CODE_ID [varchar2(30)]"       , "코드 ID"   });
            tableNode2Data.Rows.Add(new object[] { nodeID2, 0, "CODE_GROUP"    , 0, 2, "C", "", "SYS_CODE", "VARCHAR2", "VARCHAR2(30)" , "코드 그룹", "", 2 , "N", "CODE_GROUP [varchar2(30)]"    , "코드 그룹" });
            tableNode2Data.Rows.Add(new object[] { nodeID2, 0, "CODE_NM"       , 0, 3, "C", "", "SYS_CODE", "VARCHAR2", "VARCHAR2(100)", "코드명"   , "", 3 , "N", "CODE_NM [varchar2(100)]"      , "코드명"    });
            tableNode2Data.Rows.Add(new object[] { nodeID2, 0, "SORT_ORDER"    , 0, 4, "C", "", "SYS_CODE", "NUMBER"  , "NUMBER"       , "정렬 순서", "", 6 , "N", "SORT_ORDER [number]"          , "정렬 순서" });
            tableNode2Data.Rows.Add(new object[] { nodeID2, 0, "USE_YN"        , 0, 5, "C", "", "SYS_CODE", "VARCHAR2", "VARCHAR2(1)"  , "사용 여부", "", 7 , "N", "USE_YN [varchar2(1)]"         , "사용 여부" });
            tableNode2Data.Rows.Add(new object[] { nodeID2, 0, "NOTE"          , 0, 6, "C", "", "SYS_CODE", "VARCHAR2", "VARCHAR2(500)", "비고"     , "", 8 , "N", "NOTE [varchar2(500)]"         , "비고"      });
            tableNode2Data.Rows.Add(new object[] { nodeID2, 0, "INSERT_USER_ID", 0, 7, "C", "", "SYS_CODE", "VARCHAR2", "VARCHAR2(30)" , "등록자 ID", "", 9 , "N", "INSERT_USER_ID [varchar2(30)]", "등록자 ID" });
            tableNode2Data.Rows.Add(new object[] { nodeID2, 0, "INSERT_DATE"   , 0, 8, "C", "", "SYS_CODE", "DATE"    , "DATE"         , "등록 일시", "", 10, "N", "INSERT_DATE [date]"           , "등록 일시" });

            // SYS_CODE - Node
            TableNode tableNode2 = new TableNode(dataModeler);
            tableNode2.Location = new Point(550, 150);
            tableNode2.ID = nodeID2;
            tableNode2.SEQ = 0;
            tableNode2.Bind(tableNode2Data);
            if (!dataModeler.ContainsNode(tableNode2))
            {
                dataModeler.Controls.Add(tableNode2);
                tableNode2.BringToFront();
            }
            else
            {
                duplicatedNodeList.Add($"[{nodeID2}]");
            }

            // Modeler
            dataModeler.EndControlUpdate();

            if (duplicatedNodeList.Count > 0)
            {
                ModalMessageBox.Show(this, $"{ string.Join(", ", duplicatedNodeList) }는 이미 추가되었습니다.", "AddTableNode");
            }
        }

        private void btAddRelation_Click(object sender, EventArgs e)
        {
            // Validate - 테스트 Relation Data 바인드를 위한 임시 코드
            List<string> valudateIDList = new List<string>();
            foreach (GraphicControl control in dataModeler.Controls)
            {
                TableNode node = control as TableNode;
                if (node != null)
                {
                    if ((node.ID == "SYS_CODE" || node.ID == "SYS_USER") && node.SEQ == 0)
                    {
                        if (!valudateIDList.Contains(node.ID))
                        {
                            valudateIDList.Add(node.ID);
                        }
                    }
                }
            }
            if (valudateIDList.Count != 2)
            {
                ModalMessageBox.Show("SYS_CODE(0) 테이블과 SYS_USER(0) 테이블이 존재해야 Relation을 추가할 수 있습니다.", "AddRelation");
                return;
            }

            DataTable relationDt = GetEmptyRelationDataTable();
            relationDt.Rows.Add(new object[] { "I", "=", "'CPOSI'", "", "SYS_CODE", 0, "CODE_GROUP", 0, 2, "SYS_USER", 0, ""             , 0, 0 });
            relationDt.Rows.Add(new object[] { "I", "" , ""       , "", "SYS_CODE", 0, "CODE_ID"   , 0, 1, "SYS_USER", 0, "USER_POSITION", 0, 5 });

            foreach (DataRow row in relationDt.Rows)
            {
                var model = new RelationModel() 
                {
                      RELATION_TYPE      = row["RELATION_TYPE"].ToString()
                    , RELATION_OPERATOR  = row["RELATION_OPERATOR"].ToString()
                    , RELATION_VALUE     = row["RELATION_VALUE"].ToString()
                    , RELATION_NOTE      = row["RELATION_NOTE"].ToString()
                    , NODE_ID1           = row["NODE_ID1"].ToString()
                    , NODE_SEQ1          = Convert.ToInt32(row["NODE_SEQ1"])
                    , NODE_DETAIL_ID1    = row["NODE_DETAIL_ID1"].ToString()
                    , NODE_DETAIL_SEQ1   = Convert.ToInt32(row["NODE_DETAIL_SEQ1"])
                    , NODE_DETAIL_ORDER1 = Convert.ToInt32(row["NODE_DETAIL_ORDER1"])
                    , NODE_ID2           = row["NODE_ID2"].ToString()
                    , NODE_SEQ2          = Convert.ToInt32(row["NODE_SEQ2"])
                    , NODE_DETAIL_ID2    = row["NODE_DETAIL_ID2"].ToString()
                    , NODE_DETAIL_SEQ2   = Convert.ToInt32(row["NODE_DETAIL_SEQ2"])
                    , NODE_DETAIL_ORDER2 = Convert.ToInt32(row["NODE_DETAIL_ORDER2"])
                };

                RelationControl relationControl = new RelationControl(dataModeler, model);
                dataModeler.Relations.Add(new RelationControl(dataModeler, model));
            }

            dataModeler.Refresh();
        }

        private void btToggleEnabled_Click(object sender, EventArgs e)
        {
            dataModeler.Enabled = !dataModeler.Enabled;
        }

        private void btScalePlus_Click(object sender, EventArgs e)
        {
            dataModeler.ScaleValue += 0.1f;
        }

        private void btScaleMinus_Click(object sender, EventArgs e)
        {
            dataModeler.ScaleValue -= 0.1f;
        }

        private void btGetMainData_Click(object sender, EventArgs e)
        {
            Point innerLocation = dataModeler.InnerLocation;
            float scaleValue = dataModeler.ScaleValue;

            ModalMessageBox.Show(this, $"innerLocation : ({innerLocation.X}, {innerLocation.Y}), ScaleValue : {scaleValue}", "GetMainData", MessageBoxButtons.OK);
        }

        private void btGetNodeData_Click(object sender, EventArgs e)
        {
            var dataTables = dataModeler.GetNodeDataTables();
            DataTable node = dataTables.Node;
            DataTable nodeDetail = dataTables.NodeDetail;

            // Remove not column node details
            for (int i = nodeDetail.Rows.Count - 1; 0 <= i; i--)
            {
                DataRow row = nodeDetail.Rows[i];
                var nodeRows = node.Select($"NODE_ID = '{row["NODE_ID"]}' AND NODE_SEQ = {row["NODE_SEQ"]}");
                if (nodeRows.Length == 0)
                {
                    nodeDetail.Rows.Remove(row);
                }
                else
                {
                    if (nodeRows[0]["NODE_TYPE"].ToString() != "COL")
                    {
                        nodeDetail.Rows.Remove(row);
                    }
                }
            }

            // Remove NODE_DETAIL_VIEW_COLUMN1, Add MENU_IDX to nodeDetail
            nodeDetail.Columns.Remove("NODE_DETAIL_VIEW_COLUMN1");
            nodeDetail.Columns.Remove("NODE_DETAIL_VIEW_COLUMN2");

            ModalMessageBox.Show(this, $"Node Count : {node.Rows.Count}, NodeDetails Count : {nodeDetail.Rows.Count}", "GetRelationData", MessageBoxButtons.OK);
        }

        private void btGetRelationData_Click(object sender, EventArgs e)
        {
            DataTable rsRelation = dataModeler.GetRelationDataTable();
            rsRelation.Columns.Remove("NODE_DETAIL_TABLE_ALIAS1");
            rsRelation.Columns.Remove("NODE_DETAIL_TABLE_ALIAS2");

            ModalMessageBox.Show(this, $"Relations Count : {rsRelation.Rows.Count}", "GetRelationData", MessageBoxButtons.OK);
        }

        #region Function
        private DataTable GetEmptyNodeDataTable()
        {
            DataTable rsDt = new DataTable();
            rsDt.Columns.Add("NODE_ID");
            rsDt.Columns.Add("NODE_SEQ", typeof(int));
            rsDt.Columns.Add("NODE_DETAIL_ID");
            rsDt.Columns.Add("NODE_DETAIL_SEQ", typeof(int));
            rsDt.Columns.Add("NODE_DETAIL_ORDER", typeof(int));
            rsDt.Columns.Add("NODE_DETAIL_TYPE");
            rsDt.Columns.Add("NODE_DETAIL_NOTE");
            rsDt.Columns.Add("NODE_ID_REF");
            rsDt.Columns.Add("NODE_DETAIL_DATA_TYPE");
            rsDt.Columns.Add("NODE_DETAIL_DATA_TYPE_FULL");
            rsDt.Columns.Add("NODE_DETAIL_COMMENT");
            rsDt.Columns.Add("NODE_DETAIL_TABLE_ALIAS");
            rsDt.Columns.Add("ORDINAL_POSITION");
            rsDt.Columns.Add("IS_PRIMARY_KEY");
            rsDt.Columns.Add("NODE_DETAIL_VIEW_COLUMN1");
            rsDt.Columns.Add("NODE_DETAIL_VIEW_COLUMN2");
            return rsDt;
        }

        private DataTable GetEmptyRelationDataTable()
        {
            DataTable rsDt = new DataTable();
            rsDt.Columns.Add("RELATION_TYPE");
            rsDt.Columns.Add("RELATION_OPERATOR");
            rsDt.Columns.Add("RELATION_VALUE");
            rsDt.Columns.Add("RELATION_NOTE");
            rsDt.Columns.Add("NODE_ID1");
            rsDt.Columns.Add("NODE_SEQ1", typeof(int));
            rsDt.Columns.Add("NODE_DETAIL_ID1");
            rsDt.Columns.Add("NODE_DETAIL_SEQ1", typeof(int));
            rsDt.Columns.Add("NODE_DETAIL_ORDER1", typeof(int));
            rsDt.Columns.Add("NODE_ID2");
            rsDt.Columns.Add("NODE_SEQ2", typeof(int));
            rsDt.Columns.Add("NODE_DETAIL_ID2");
            rsDt.Columns.Add("NODE_DETAIL_SEQ2", typeof(int));
            rsDt.Columns.Add("NODE_DETAIL_ORDER2", typeof(int));
            return rsDt;
        }
        #endregion

    }
}
