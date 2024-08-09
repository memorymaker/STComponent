using ST.CodeGenerator;
using ST.Controls;
using ST.Core;
using ST.DataModeler;
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
            CodeGenerator.TEMPLATE.TEMPLATE_SEQ = "TEMPLATE_SEQ";
            CodeGenerator.TEMPLATE.TEMPLATE_TITLE = "TEMPLATE_TITLE";
            CodeGenerator.TEMPLATE.TEMPLATE_CONTENT = "TEMPLATE_CONTENT";
            CodeGenerator.TEMPLATE.TEMPLATE_RESULT = "TEMPLATE_RESULT";
            CodeGenerator.TEMPLATE.TEMPLATE_STYLE = "TEMPLATE_STYLE";
            CodeGenerator.TEMPLATE.TEMPLATE_SORT = "TEMPLATE_SORT";
            CodeGenerator.TEMPLATE.TEMPLATE_NOTE = "TEMPLATE_NOTE";
            CodeGenerator.TEMPLATE.TEMPLATE_SELECTED = "TEMPLATE_SELECTED";

            CodeGenerator.NODE.NODE_ID_REF = "NODE_ID_REF";
            CodeGenerator.NODE.NODE_SEQ_REF = "NODE_SEQ_REF";
            CodeGenerator.NODE.NODE_DETAIL_ID = "NODE_DETAIL_ID";
            CodeGenerator.NODE.NODE_DETAIL_TABLE_ALIAS = "NODE_DETAIL_TABLE_ALIAS";

            CodeGenerator.RELATION.NODE_ID2 = "NODE_ID2";
            CodeGenerator.RELATION.NODE_SEQ2 = "NODE_SEQ2";
            CodeGenerator.RELATION.NODE_DETAIL_TABLE_ALIAS2 = "NODE_DETAIL_TABLE_ALIAS2";
            CodeGenerator.RELATION.NODE_DETAIL_ID2 = "NODE_DETAIL_ID2";
            CodeGenerator.RELATION.NODE_ID1 = "NODE_ID1";
            CodeGenerator.RELATION.NODE_SEQ1 = "NODE_SEQ1";
            CodeGenerator.RELATION.NODE_DETAIL_TABLE_ALIAS1 = "NODE_DETAIL_TABLE_ALIAS1";
            CodeGenerator.RELATION.NODE_DETAIL_ID1 = "NODE_DETAIL_ID1";
            CodeGenerator.RELATION.NODE_DETAIL_ORDER1 = "NODE_DETAIL_ORDER1";
            CodeGenerator.RELATION.RELATION_TYPE = "RELATION_TYPE";
            CodeGenerator.RELATION.RELATION_OPERATOR = "RELATION_OPERATOR";
            CodeGenerator.RELATION.RELATION_VALUE = "RELATION_VALUE";

            // Common Variables 영역과 Tab 영역 사이의 간격의 가져오거나 설정합니다.
            codeGenerator.MainSplitterDistance = 120;

            // SYS_USER 노드 데이터 생성 후 CodeGenerator.NodeData에 추가합니다.
            string nodeID1 = "SYS_USER";
            DataTable node1Data = DataModeler.GetEmptyNodeDataTable();
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

            // SYS_CODE 노드 데이터 생성 후 CodeGenerator.NodeData에 추가합니다.
            string nodeID2 = "SYS_CODE";
            DataTable node2Data = DataModeler.GetEmptyNodeDataTable();
            node2Data.Rows.Add(new object[] { nodeID2, 0, "CODE_ID"       , 0, 1, "C", "", "SYS_CODE", 0, "VARCHAR2", "VARCHAR2(30)" , "코드 ID"  , "", 1 , "Y", "CODE_ID [varchar2(30)]"       , "코드 ID"   });
            node2Data.Rows.Add(new object[] { nodeID2, 0, "CODE_GROUP"    , 0, 2, "C", "", "SYS_CODE", 0, "VARCHAR2", "VARCHAR2(30)" , "코드 그룹", "", 2 , "N", "CODE_GROUP [varchar2(30)]"    , "코드 그룹" });
            node2Data.Rows.Add(new object[] { nodeID2, 0, "CODE_NM"       , 0, 3, "C", "", "SYS_CODE", 0, "VARCHAR2", "VARCHAR2(100)", "코드명"   , "", 3 , "N", "CODE_NM [varchar2(100)]"      , "코드명"    });
            node2Data.Rows.Add(new object[] { nodeID2, 0, "SORT_ORDER"    , 0, 4, "C", "", "SYS_CODE", 0, "NUMBER"  , "NUMBER"       , "정렬 순서", "", 6 , "N", "SORT_ORDER [number]"          , "정렬 순서" });
            node2Data.Rows.Add(new object[] { nodeID2, 0, "USE_YN"        , 0, 5, "C", "", "SYS_CODE", 0, "VARCHAR2", "VARCHAR2(1)"  , "사용 여부", "", 7 , "N", "USE_YN [varchar2(1)]"         , "사용 여부" });
            node2Data.Rows.Add(new object[] { nodeID2, 0, "NOTE"          , 0, 6, "C", "", "SYS_CODE", 0, "VARCHAR2", "VARCHAR2(500)", "비고"     , "", 8 , "N", "NOTE [varchar2(500)]"         , "비고"      });
            node2Data.Rows.Add(new object[] { nodeID2, 0, "INSERT_USER_ID", 0, 7, "C", "", "SYS_CODE", 0, "VARCHAR2", "VARCHAR2(30)" , "등록자 ID", "", 9 , "N", "INSERT_USER_ID [varchar2(30)]", "등록자 ID" });
            node2Data.Rows.Add(new object[] { nodeID2, 0, "INSERT_DATE"   , 0, 8, "C", "", "SYS_CODE", 0, "DATE"    , "DATE"         , "등록 일시", "", 10, "N", "INSERT_DATE [date]"           , "등록 일시" });
            codeGenerator.NodeData.Add(nodeID2, node2Data);

            // selectList 노드 데이터 생성 후 CodeGenerator.NodeData에 추가합니다.
            string nodeID3 = "selectList";
            DataTable node3Data = DataModeler.GetEmptyNodeDataTable();
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

            // selectParams 노드 데이터 생성 후 CodeGenerator.NodeData에 추가합니다.
            string nodeID4 = "selectParams";
            DataTable node4Data = DataModeler.GetEmptyNodeDataTable();
            node4Data.Rows.Add(new object[] { nodeID4, 0, "USER_ID"       , 0, 1 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "사용자 ID"      , "SU", 1 , "Y", "USER_ID [varchar2(30)]"       , "사용자 ID"   });
            node4Data.Rows.Add(new object[] { nodeID4, 0, "USER_NAME"     , 0, 2 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(50)" , "사용자명"       , "SU", 2 , "N", "USER_NAME [varchar2(50)]"     , "사용자명"    });
            node4Data.Rows.Add(new object[] { nodeID4, 0, "USER_GROUP"    , 0, 3 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "사용자 그룹"    , "SU", 3 , "N", "USER_GROUP [varchar2(30)]"    , "사용자 그룹" });
            node4Data.Rows.Add(new object[] { nodeID4, 0, "USE_YN"        , 0, 4 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(1)"  , "사용 여부"      , "SU", 10, "N", "USE_YN [varchar2(1)]"         , "사용 여부"   });
            codeGenerator.NodeData.Add(nodeID4, node4Data);

            // 릴레이션 데이터 생성 후 CodeGenerator.RelationData에 추가합니다.
            DataTable relationData = CodeGenerator.GetEmptyRelationDataTable();
            relationData.Rows.Add(new object[] { "I", "=", "'CPOSI'", "", "SYS_CODE", 0, "CODE_GROUP", 0, 2, "SC", "SYS_USER", 0, ""             , 0, 0, "SU" });
            relationData.Rows.Add(new object[] { "I", "" , ""       , "", "SYS_CODE", 0, "CODE_ID"   , 0, 1, "SC", "SYS_USER", 0, "USER_POSITION", 0, 5, "SU" });
            codeGenerator.RelationData = relationData;
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            // CodeGenerator의 모든 Tab을 삭제하고 Common Variables 텍스트를 초기화합니다.
            codeGenerator.Clear();
        }

        private void btLoadData_Click(object sender, EventArgs e)
        {
            // CodeGenerator.GetTemplateDataTable는 빈 Template DataTable을 반환하는 정적 메서드입니다.
            // 위 메서드와 Rows.Add로 Template DataTable와 데이터를 생성합니다.
            DataTable dt = CodeGenerator.GetTemplateDataTable();

            // 추가되는 데이터 중 첫 번째 항목은 -1은 TEMPLATE_SEQ 컬럼의 데이터로
            // 해당 값이 -1이면 Common Variable의 데이터를 의미합니다.
            dt.Rows.Add(new object[] { -1, "", "name : 홍길동\r\ndesc : 사용자 조회\r\ndate : 2024-04-01\r\nspName: SYS_CODE_SELECT", "", "", 0, "", 0 });

            // TEMPLATE_SEQ 값은 0 부터 탭의 데이터입니다. 탭의 타이틀 값은 "NewTab1" 입니다.
            dt.Rows.Add(new object[] { 0, "NewTab1", "/************************************************************************\r\n 설  명: {desc} 조회\r\n 작성자: {name}\r\n 작성일: {date}\r\n 수정일:\r\n/***********************************************************************/\r\nCREATE OR ALTER PROCEDURE {spName} (\r\no/ id:selectParams\r\ns/      @{NODE_DETAIL_ID} {NODE_DETAIL_DATA_TYPE_FULL} -- {NODE_DETAIL_COMMENT}\r\ns/\r\nb/    , @{NODE_DETAIL_ID} {NODE_DETAIL_DATA_TYPE_FULL} -- {NODE_DETAIL_COMMENT}\r\nb/\r\n)\r\nAS\r\nBEGIN\r\n    \r\n    SELECT\r\no/ id:selectList\r\ns/          {NODE_DETAIL_TABLE_ALIAS}.{NODE_DETAIL_ID} \r\ns/\r\nb/        , {NODE_DETAIL_TABLE_ALIAS}.{NODE_DETAIL_ID}\r\nb/\r\n    {from id:selectList}\r\no/ id:selectParams\r\ns/    WHERE (@{NODE_DETAIL_ID} IS NULL OR {NODE_DETAIL_TABLE_ALIAS}.{NODE_DETAIL_ID} = @{NODE_DETAIL_ID})\r\ns/\r\nb/        AND (@{NODE_DETAIL_ID} IS NULL OR {NODE_DETAIL_TABLE_ALIAS}.{NODE_DETAIL_ID} = @{NODE_DETAIL_ID})\r\nb/\r\n\r\nEND;", "", "SQL", 0, "", 0 });

            // TEMPLATE_SEQ 값은 0 부터 시작이므로 1은 두 번째 탭의 데이터입니다. 탭의 타이틀 값은 "NewTab2" 입니다.
            dt.Rows.Add(new object[] { 1, "NewTab2", "{from id:selectList}", "", "SQL", 1, "", 1 });

            // 생성된 DataTable을 CodeGenerator에 바인딩합니다.
            codeGenerator.SetDataTable(dt);
        }

        private void btGetData_Click(object sender, EventArgs e)
        {
            // CodeGenerator의 데이터를 DataTable 형태로 반환합니다.
            DataTable dt = codeGenerator.GetDataTable();
            ModalMessageBox.Show($"Rows Count : {dt.Rows.Count}", "Get Data");
        }

        private void btToggleEnabled_Click(object sender, EventArgs e)
        {
            // 사용 가능 여부를 설정합니다.
            codeGenerator.Enabled = !codeGenerator.Enabled;
        }

        private void btSetSplitter_Click(object sender, EventArgs e)
        {
            // Common Variables 영역과 Tab 영역 사이의 간격의 가져오거나 설정합니다.
            if (codeGenerator.MainSplitterDistance != 120)
            {
                codeGenerator.MainSplitterDistance = 120;
            }
            else
            {
                codeGenerator.MainSplitterDistance = 240;
            }
        }

        private void btAddTab_Click(object sender, EventArgs e)
        {
            // 새 탭을 추가합니다.
            codeGenerator.AddNewTab();
        }
    }
}