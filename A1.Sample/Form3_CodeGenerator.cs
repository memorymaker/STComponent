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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            LoadThis();
        }

        private void LoadThis()
        {
            codeGenerator.MainSplitSplitterDistance = 120;

            codeGenerator.ExecuteButton_Click += CodeGenerator_ExecuteButton_Click;

            codeGenerator.NodeFieldName_Table = "NODE_ID_REF";
            codeGenerator.NodeFieldName_TableSeq = "NODE_SEQ_REF";
            codeGenerator.NodeFieldName_TableAlias = "NODE_DETAIL_TABLE_ALIAS";
            codeGenerator.NodeFieldName_Column = "NODE_DETAIL_ID";

            codeGenerator.RelationFieldName_OriginTable = "NODE_ID2";
            codeGenerator.RelationFieldName_OriginTableSeq = "NODE_SEQ2";
            codeGenerator.RelationFieldName_OriginTableAlias = "NODE_DETAIL_TABLE_ALIAS2";
            codeGenerator.RelationFieldName_OriginColumn = "NODE_DETAIL_ID2";
            codeGenerator.RelationFieldName_DestinationTable = "NODE_ID1";
            codeGenerator.RelationFieldName_DestinationTableSeq = "NODE_SEQ1";
            codeGenerator.RelationFieldName_DestinationTableAlias = "NODE_DETAIL_TABLE_ALIAS1";
            codeGenerator.RelationFieldName_DestinationColumn = "NODE_DETAIL_ID1";
            codeGenerator.RelationFieldName_DestinationColumnOrder = "NODE_DETAIL_ORDER1";
            codeGenerator.RelationFieldName_JoinType = "RELATION_TYPE";
            codeGenerator.RelationFieldName_JoinOperator = "RELATION_OPERATOR";
            codeGenerator.RelationFieldName_JoinValue = "RELATION_VALUE";


            string nodeID1 = "SYS_USER";
            DataTable node1Data = GetEmptyNodeDataTable();
            node1Data.Rows.Add(new object[] { nodeID1, 0, "USER_ID"       , 0, 1 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "사용자 ID"      , "", 1 , "Y", "USER_ID [varchar2(30)]"       , "사용자 ID"       });
            node1Data.Rows.Add(new object[] { nodeID1, 0, "USER_NAME"     , 0, 2 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(50)" , "사용자명"       , "", 2 , "N", "USER_NAME [varchar2(50)]"     , "사용자명"        });
            node1Data.Rows.Add(new object[] { nodeID1, 0, "USER_GROUP"    , 0, 3 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "사용자 그룹"    , "", 3 , "N", "USER_GROUP [varchar2(30)]"    , "사용자 그룹"     });
            node1Data.Rows.Add(new object[] { nodeID1, 0, "USER_PASSWD"   , 0, 4 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(200)", "사용자 비밀번호", "", 4 , "N", "USER_PASSWD [varchar2(200)]"  , "사용자 비밀번호" });
            node1Data.Rows.Add(new object[] { nodeID1, 0, "USER_POSITION" , 0, 5 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "사용자 직위"    , "", 5 , "N", "USER_POSITION [varchar2(30)]" , "사용자 직위"     });
            node1Data.Rows.Add(new object[] { nodeID1, 0, "USER_RESPONS"  , 0, 6 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "사용자 직책"    , "", 6 , "N", "USER_RESPONS [varchar2(30)]"  , "사용자 직책"     });
            node1Data.Rows.Add(new object[] { nodeID1, 0, "EMP_NO"        , 0, 7 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(10)" , "사원 번호"      , "", 7 , "N", "EMP_NO [varchar2(10)]"        , "사원 번호"       });
            node1Data.Rows.Add(new object[] { nodeID1, 0, "EMAIL_ADDRESS" , 0, 8 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(100)", "이메일 주소"    , "", 8 , "N", "EMAIL_ADDRESS [varchar2(100)]", "이메일 주소"     });
            node1Data.Rows.Add(new object[] { nodeID1, 0, "MOBILE_PHONE"  , 0, 9 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(50)" , "휴대폰 번호"    , "", 9 , "N", "MOBILE_PHONE [varchar2(50)]"  , "휴대폰 번호"     });
            node1Data.Rows.Add(new object[] { nodeID1, 0, "USE_YN"        , 0, 10, "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(1)"  , "사용 여부"      , "", 10, "N", "USE_YN [varchar2(1)]"         , "사용 여부"       });
            node1Data.Rows.Add(new object[] { nodeID1, 0, "NOTE"          , 0, 11, "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(500)", "비고"           , "", 11, "N", "NOTE [varchar2(500)]"         , "비고"            });
            node1Data.Rows.Add(new object[] { nodeID1, 0, "INSERT_USER_ID", 0, 12, "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "등록자 ID"      , "", 12, "N", "INSERT_USER_ID [varchar2(30)]", "등록자 ID"       });
            node1Data.Rows.Add(new object[] { nodeID1, 0, "INSERT_DATE"   , 0, 13, "C", "", "SYS_USER", 0, "DATE"    , "DATE"         , "등록 일시"      , "", 13, "N", "INSERT_DATE [date]"           , "등록 일시"       });
            node1Data.Rows.Add(new object[] { nodeID1, 0, "UPDATE_USER_ID", 0, 14, "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "수정자 ID"      , "", 14, "N", "UPDATE_USER_ID [varchar2(30)]", "수정자 ID"       });
            node1Data.Rows.Add(new object[] { nodeID1, 0, "UPDATE_DATE"   , 0, 15, "C", "", "SYS_USER", 0, "DATE"    , "DATE"         , "수정 일시"      , "", 15, "N", "UPDATE_DATE [date]"           , "수정 일시"       });
            codeGenerator.NodeData.Add(nodeID1, node1Data);

            string nodeID2 = "SYS_CODE";
            DataTable node2Data = GetEmptyNodeDataTable();
            node2Data.Rows.Add(new object[] { nodeID2, 0, "CODE_ID"       , 0, 1, "C", "", "SYS_CODE", 0, "VARCHAR2", "VARCHAR2(30)" , "코드 ID"  , "", 1 , "Y", "CODE_ID [varchar2(30)]"       , "코드 ID"   });
            node2Data.Rows.Add(new object[] { nodeID2, 0, "CODE_GROUP"    , 0, 2, "C", "", "SYS_CODE", 0, "VARCHAR2", "VARCHAR2(30)" , "코드 그룹", "", 2 , "N", "CODE_GROUP [varchar2(30)]"    , "코드 그룹" });
            node2Data.Rows.Add(new object[] { nodeID2, 0, "CODE_NM"       , 0, 3, "C", "", "SYS_CODE", 0, "VARCHAR2", "VARCHAR2(100)", "코드명"   , "", 3 , "N", "CODE_NM [varchar2(100)]"      , "코드명"    });
            node2Data.Rows.Add(new object[] { nodeID2, 0, "SORT_ORDER"    , 0, 4, "C", "", "SYS_CODE", 0, "NUMBER"  , "NUMBER"       , "정렬 순서", "", 6 , "N", "SORT_ORDER [number]"          , "정렬 순서" });
            node2Data.Rows.Add(new object[] { nodeID2, 0, "USE_YN"        , 0, 5, "C", "", "SYS_CODE", 0, "VARCHAR2", "VARCHAR2(1)"  , "사용 여부", "", 7 , "N", "USE_YN [varchar2(1)]"         , "사용 여부" });
            node2Data.Rows.Add(new object[] { nodeID2, 0, "NOTE"          , 0, 6, "C", "", "SYS_CODE", 0, "VARCHAR2", "VARCHAR2(500)", "비고"     , "", 8 , "N", "NOTE [varchar2(500)]"         , "비고"      });
            node2Data.Rows.Add(new object[] { nodeID2, 0, "INSERT_USER_ID", 0, 7, "C", "", "SYS_CODE", 0, "VARCHAR2", "VARCHAR2(30)" , "등록자 ID", "", 9 , "N", "INSERT_USER_ID [varchar2(30)]", "등록자 ID" });
            node2Data.Rows.Add(new object[] { nodeID2, 0, "INSERT_DATE"   , 0, 8, "C", "", "SYS_CODE", 0, "DATE"    , "DATE"         , "등록 일시", "", 10, "N", "INSERT_DATE [date]"           , "등록 일시" });
            codeGenerator.NodeData.Add(nodeID2, node2Data);


            string nodeID3 = "selectList";
            DataTable node3Data = GetEmptyNodeDataTable();
            node3Data.Rows.Add(new object[] { nodeID3, 0, "USER_ID"       , 0, 1 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "사용자 ID"      , "SU", 1 , "Y", "USER_ID [varchar2(30)]"       , "사용자 ID"   });
            node3Data.Rows.Add(new object[] { nodeID3, 0, "USER_NAME"     , 0, 2 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(50)" , "사용자명"       , "SU", 2 , "N", "USER_NAME [varchar2(50)]"     , "사용자명"    });
            node3Data.Rows.Add(new object[] { nodeID3, 0, "USER_GROUP"    , 0, 3 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "사용자 그룹"    , "SU", 3 , "N", "USER_GROUP [varchar2(30)]"    , "사용자 그룹" });
            node3Data.Rows.Add(new object[] { nodeID3, 0, "CODE_NM"       , 0, 3 , "C", "", "SYS_CODE", 0, "VARCHAR2", "VARCHAR2(100)", "코드명"         , "SC", 4 , "N", "CODE_NM [varchar2(100)]"      , "코드명"      });
            node3Data.Rows.Add(new object[] { nodeID3, 0, "USER_POSITION" , 0, 5 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "사용자 직위"    , "SU", 5 , "N", "USER_POSITION [varchar2(30)]" , "사용자 직위" });
            node3Data.Rows.Add(new object[] { nodeID3, 0, "USER_RESPONS"  , 0, 6 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "사용자 직책"    , "SU", 6 , "N", "USER_RESPONS [varchar2(30)]"  , "사용자 직책" });
            node3Data.Rows.Add(new object[] { nodeID3, 0, "EMP_NO"        , 0, 7 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(10)" , "사원 번호"      , "SU", 7 , "N", "EMP_NO [varchar2(10)]"        , "사원 번호"   });
            node3Data.Rows.Add(new object[] { nodeID3, 0, "EMAIL_ADDRESS" , 0, 8 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(100)", "이메일 주소"    , "SU", 8 , "N", "EMAIL_ADDRESS [varchar2(100)]", "이메일 주소" });
            node3Data.Rows.Add(new object[] { nodeID3, 0, "MOBILE_PHONE"  , 0, 9 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(50)" , "휴대폰 번호"    , "SU", 9 , "N", "MOBILE_PHONE [varchar2(50)]"  , "휴대폰 번호" });
            node3Data.Rows.Add(new object[] { nodeID3, 0, "USE_YN"        , 0, 10, "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(1)"  , "사용 여부"      , "SU", 10, "N", "USE_YN [varchar2(1)]"         , "사용 여부"   });
            node3Data.Rows.Add(new object[] { nodeID3, 0, "NOTE"          , 0, 11, "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(500)", "비고"           , "SU", 11, "N", "NOTE [varchar2(500)]"         , "비고"        });
            codeGenerator.NodeData.Add(nodeID3, node3Data);

            string nodeID4 = "selectParams";
            DataTable node4Data = GetEmptyNodeDataTable();
            node4Data.Rows.Add(new object[] { nodeID4, 0, "USER_ID"       , 0, 1 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "사용자 ID"      , "SU", 1 , "Y", "USER_ID [varchar2(30)]"       , "사용자 ID"   });
            node4Data.Rows.Add(new object[] { nodeID4, 0, "USER_NAME"     , 0, 2 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(50)" , "사용자명"       , "SU", 2 , "N", "USER_NAME [varchar2(50)]"     , "사용자명"    });
            node4Data.Rows.Add(new object[] { nodeID4, 0, "USER_GROUP"    , 0, 3 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "사용자 그룹"    , "SU", 3 , "N", "USER_GROUP [varchar2(30)]"    , "사용자 그룹" });
            node4Data.Rows.Add(new object[] { nodeID4, 0, "USE_YN"        , 0, 4 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(1)"  , "사용 여부"      , "SU", 10, "N", "USE_YN [varchar2(1)]"         , "사용 여부"   });
            codeGenerator.NodeData.Add(nodeID4, node4Data);


            DataTable relationData = GetEmptyRelationDataTable();
            relationData.Rows.Add(new object[] { "I", "=", "'CPOSI'", "", "SYS_CODE", 0, "CODE_GROUP", 0, 2, "SC", "SYS_USER", 0, ""             , 0, 0, "SU" });
            relationData.Rows.Add(new object[] { "I", "" , ""       , "", "SYS_CODE", 0, "CODE_ID"   , 0, 1, "SC", "SYS_USER", 0, "USER_POSITION", 0, 5, "SU" });
            codeGenerator.RelationData = relationData;
        }

        private void CodeGenerator_ExecuteButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ExecuteButton Click");
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            codeGenerator.Clear();
        }

        private void btLoadData_Click(object sender, EventArgs e)
        {
            DataTable dt = GetTemplateDataTable();
            // TEMPLATE_SEQ : -1 - Common Variable
            dt.Rows.Add(new object[] { -1, "", "name : 홍길동\r\ndesc : 사용자 조회\r\ndate : 2024-04-01\r\nspName: SYS_CODE_SELECT", "", "", 0, "", 0 });
            // NewTab1
            dt.Rows.Add(new object[] { 0, "NewTab1", "/************************************************************************\r\n 설  명: {desc} 조회\r\n 작성자: {name}\r\n 작성일: {date}\r\n 수정일:\r\n/***********************************************************************/\r\nCREATE OR ALTER PROCEDURE {spName} (\r\no/ id:selectParams\r\ns/      @{NODE_DETAIL_ID} {NODE_DETAIL_DATA_TYPE_FULL} -- {NODE_DETAIL_COMMENT}\r\ns/\r\nb/    , @{NODE_DETAIL_ID} {NODE_DETAIL_DATA_TYPE_FULL} -- {NODE_DETAIL_COMMENT}\r\nb/\r\n)\r\nAS\r\nBEGIN\r\n    \r\n    SELECT\r\no/ id:selectList\r\ns/          {NODE_DETAIL_TABLE_ALIAS}.{NODE_DETAIL_ID} \r\ns/\r\nb/        , {NODE_DETAIL_TABLE_ALIAS}.{NODE_DETAIL_ID}\r\nb/\r\n    {from id:selectList}\r\no/ id:selectParams\r\ns/    WHERE (@{NODE_DETAIL_ID} IS NULL OR {NODE_DETAIL_TABLE_ALIAS}.{NODE_DETAIL_ID} = @{NODE_DETAIL_ID})\r\ns/\r\nb/        AND (@{NODE_DETAIL_ID} IS NULL OR {NODE_DETAIL_TABLE_ALIAS}.{NODE_DETAIL_ID} = @{NODE_DETAIL_ID})\r\nb/\r\n\r\nEND;", "", "SQL", 0, "", 0 });
            // NewTab2
            dt.Rows.Add(new object[] { 1, "NewTab2", "{from id:selectList}", "", "SQL", 1, "", 1 });
            codeGenerator.SetDataTable(dt);
        }

        private void btGetData_Click(object sender, EventArgs e)
        {
            DataTable dt = codeGenerator.GetDataTable();
            ModalMessageBox.Show($"Rows Count : {dt.Rows.Count}", "Get Data");
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
            rsDt.Columns.Add("NODE_SEQ_REF", typeof(int));
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
            rsDt.Columns.Add("NODE_DETAIL_TABLE_ALIAS1");
            rsDt.Columns.Add("NODE_ID2");
            rsDt.Columns.Add("NODE_SEQ2", typeof(int));
            rsDt.Columns.Add("NODE_DETAIL_ID2");
            rsDt.Columns.Add("NODE_DETAIL_SEQ2", typeof(int));
            rsDt.Columns.Add("NODE_DETAIL_ORDER2", typeof(int));
            rsDt.Columns.Add("NODE_DETAIL_TABLE_ALIAS2");
            return rsDt;
        }

        private DataTable GetTemplateDataTable()
        {
            DataTable rsDt = new DataTable();
            rsDt.Columns.Add("TEMPLATE_SEQ", typeof(int));
            rsDt.Columns.Add("TEMPLATE_TITLE");
            rsDt.Columns.Add("TEMPLATE_CONTENT");
            rsDt.Columns.Add("TEMPLATE_RESULT");
            rsDt.Columns.Add("TEMPLATE_STYLE");
            rsDt.Columns.Add("TEMPLATE_SORT", typeof(int));
            rsDt.Columns.Add("TEMPLATE_NOTE");
            rsDt.Columns.Add("TEMPLATE_SELECTED", typeof(int));
            return rsDt;
        }
        #endregion

        private void btToggleEnabled_Click(object sender, EventArgs e)
        {
            codeGenerator.Enabled = !codeGenerator.Enabled;
        }

        private void btSetSplitter_Click(object sender, EventArgs e)
        {
            if (codeGenerator.MainSplitSplitterDistance != 120)
            {
                codeGenerator.MainSplitSplitterDistance = 120;
            }
            else
            {
                codeGenerator.MainSplitSplitterDistance = 240;
            }
        }

        private void btAddTab_Click(object sender, EventArgs e)
        {
            codeGenerator.AddNewTab();
        }
    }
}