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
            dataModeler1.Clear();
        }

        private void btAddTableNode_Click(object sender, EventArgs e)
        {
            // Modeler
            dataModeler1.BeginControlUpdate();


            // ------ SYS_CODE
            string nodeID2 = "SYS_USER";

            // SYS_USER - Data
            DataTable tableNode1Data = GetEmptyNodeDataTable();
            tableNode1Data.Rows.Add(new object[] { nodeID2, 0, "USER_ID"       , 0, 1 , "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(30)" , "사용자 ID"      , "", 1 , "Y", "USER_ID [varchar2(30)]"       , "사용자 ID"       });
            tableNode1Data.Rows.Add(new object[] { nodeID2, 0, "USER_NAME"     , 0, 2 , "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(50)" , "사용자명"       , "", 2 , "N", "USER_NAME [varchar2(50)]"     , "사용자명"        });
            tableNode1Data.Rows.Add(new object[] { nodeID2, 0, "USER_GROUP"    , 0, 3 , "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(30)" , "사용자 그룹"    , "", 3 , "N", "USER_GROUP [varchar2(30)]"    , "사용자 그룹"     });
            tableNode1Data.Rows.Add(new object[] { nodeID2, 0, "USER_PASSWD"   , 0, 4 , "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(200)", "사용자 비밀번호", "", 4 , "N", "USER_PASSWD [varchar2(200)]"  , "사용자 비밀번호" });
            tableNode1Data.Rows.Add(new object[] { nodeID2, 0, "USER_POSITION" , 0, 5 , "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(30)" , "사용자 직위"    , "", 5 , "N", "USER_POSITION [varchar2(30)]" , "사용자 직위"     });
            tableNode1Data.Rows.Add(new object[] { nodeID2, 0, "USER_RESPONS"  , 0, 6 , "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(30)" , "사용자 직책"    , "", 6 , "N", "USER_RESPONS [varchar2(30)]"  , "사용자 직책"     });
            tableNode1Data.Rows.Add(new object[] { nodeID2, 0, "EMP_NO"        , 0, 7 , "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(10)" , "사원 번호"      , "", 7 , "N", "EMP_NO [varchar2(10)]"        , "사원 번호"       });
            tableNode1Data.Rows.Add(new object[] { nodeID2, 0, "EMAIL_ADDRESS" , 0, 8 , "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(100)", "이메일 주소"    , "", 8 , "N", "EMAIL_ADDRESS [varchar2(100)]", "이메일 주소"     });
            tableNode1Data.Rows.Add(new object[] { nodeID2, 0, "MOBILE_PHONE"  , 0, 9 , "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(50)" , "휴대폰 번호"    , "", 9 , "N", "MOBILE_PHONE [varchar2(50)]"  , "휴대폰 번호"     });
            tableNode1Data.Rows.Add(new object[] { nodeID2, 0, "USE_YN"        , 0, 10, "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(1)"  , "사용 여부"      , "", 10, "N", "USE_YN [varchar2(1)]"         , "사용 여부"       });
            tableNode1Data.Rows.Add(new object[] { nodeID2, 0, "NOTE"          , 0, 11, "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(500)", "비고"           , "", 11, "N", "NOTE [varchar2(500)]"         , "비고"            });
            tableNode1Data.Rows.Add(new object[] { nodeID2, 0, "INSERT_USER_ID", 0, 12, "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(30)" , "등록자 ID"      , "", 12, "N", "INSERT_USER_ID [varchar2(30)]", "등록자 ID"       });
            tableNode1Data.Rows.Add(new object[] { nodeID2, 0, "INSERT_DATE"   , 0, 13, "C", "", "SYS_USER", "DATE"    , "DATE"         , "등록 일시"      , "", 13, "N", "INSERT_DATE [date]"           , "등록 일시"       });
            tableNode1Data.Rows.Add(new object[] { nodeID2, 0, "UPDATE_USER_ID", 0, 14, "C", "", "SYS_USER", "VARCHAR2", "VARCHAR2(30)" , "수정자 ID"      , "", 14, "N", "UPDATE_USER_ID [varchar2(30)]", "수정자 ID"       });
            tableNode1Data.Rows.Add(new object[] { nodeID2, 0, "UPDATE_DATE"   , 0, 15, "C", "", "SYS_USER", "DATE"    , "DATE"         , "수정 일시"      , "", 15, "N", "UPDATE_DATE [date]"           , "수정 일시"       });

            // SYS_USER - Node
            TableNode tableNode1 = new TableNode(dataModeler1);
            tableNode1.Location = new Point(200, 100);
            tableNode1.ID = nodeID2;
            tableNode1.SEQ = 0;
            tableNode1.Bind(tableNode1Data);
            dataModeler1.Controls.Add(tableNode1);


            // ------ SYS_CODE
            string nodeID1 = "SYS_CODE";

            // SYS_CODE - Data
            DataTable tableNode2Data = GetEmptyNodeDataTable();
            tableNode2Data.Rows.Add(new object[] { nodeID1, 0, "CODE_ID"       , 0, 1, "C", "", "SYS_CODE", "VARCHAR2", "VARCHAR2(30)" , "코드 ID"  , "", 1 , "Y", "CODE_ID [varchar2(30)]"       , "코드 ID"   });
            tableNode2Data.Rows.Add(new object[] { nodeID1, 0, "CODE_GROUP"    , 0, 2, "C", "", "SYS_CODE", "VARCHAR2", "VARCHAR2(30)" , "코드 그룹", "", 2 , "N", "CODE_GROUP [varchar2(30)]"    , "코드 그룹" });
            tableNode2Data.Rows.Add(new object[] { nodeID1, 0, "CODE_NM"       , 0, 3, "C", "", "SYS_CODE", "VARCHAR2", "VARCHAR2(100)", "코드명"   , "", 3 , "N", "CODE_NM [varchar2(100)]"      , "코드명"    });
            tableNode2Data.Rows.Add(new object[] { nodeID1, 0, "SORT_ORDER"    , 0, 4, "C", "", "SYS_CODE", "NUMBER"  , "NUMBER"       , "정렬 순서", "", 6 , "N", "SORT_ORDER [number]"          , "정렬 순서" });
            tableNode2Data.Rows.Add(new object[] { nodeID1, 0, "USE_YN"        , 0, 5, "C", "", "SYS_CODE", "VARCHAR2", "VARCHAR2(1)"  , "사용 여부", "", 7 , "N", "USE_YN [varchar2(1)]"         , "사용 여부" });
            tableNode2Data.Rows.Add(new object[] { nodeID1, 0, "NOTE"          , 0, 6, "C", "", "SYS_CODE", "VARCHAR2", "VARCHAR2(500)", "비고"     , "", 8 , "N", "NOTE [varchar2(500)]"         , "비고"      });
            tableNode2Data.Rows.Add(new object[] { nodeID1, 0, "INSERT_USER_ID", 0, 7, "C", "", "SYS_CODE", "VARCHAR2", "VARCHAR2(30)" , "등록자 ID", "", 9 , "N", "INSERT_USER_ID [varchar2(30)]", "등록자 ID" });
            tableNode2Data.Rows.Add(new object[] { nodeID1, 0, "INSERT_DATE"   , 0, 8, "C", "", "SYS_CODE", "DATE"    , "DATE"         , "등록 일시", "", 10, "N", "INSERT_DATE [date]"           , "등록 일시" });

            // SYS_CODE - Node
            TableNode tableNode2 = new TableNode(dataModeler1);
            tableNode2.Location = new Point(550, 150);
            tableNode2.ID = nodeID1;
            tableNode2.SEQ = 0;
            tableNode2.Bind(tableNode2Data);
            dataModeler1.Controls.Add(tableNode2);
            tableNode2.BringToFront();


            // Modeler
            dataModeler1.EndControlUpdate();
        }

        private void btAddRelation_Click(object sender, EventArgs e)
        {
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
                dataModeler1.Relations.Add(new RelationControl(dataModeler1, model));
            }

            dataModeler1.Refresh();
        }

        private void btToggleEnabled_Click(object sender, EventArgs e)
        {
            dataModeler1.Enabled = !dataModeler1.Enabled;
        }

        private void btScalePlus_Click(object sender, EventArgs e)
        {
            dataModeler1.ScaleValue += 0.1f;
        }

        private void btScaleMinus_Click(object sender, EventArgs e)
        {
            dataModeler1.ScaleValue -= 0.1f;
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
