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
using System.Xml.Linq;
using static ST.DataModeler.DataModeler;

namespace Sample
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            DataModeler.NODE.NODE_ID = "NODE_ID";
            DataModeler.NODE.NODE_SEQ = "NODE_SEQ";
            DataModeler.NODE.NODE_DETAIL_ID = "NODE_DETAIL_ID";
            DataModeler.NODE.NODE_DETAIL_SEQ = "NODE_DETAIL_SEQ";
            DataModeler.NODE.NODE_DETAIL_ORDER = "NODE_DETAIL_ORDER";
            DataModeler.NODE.NODE_DETAIL_TYPE = "NODE_DETAIL_TYPE";
            DataModeler.NODE.NODE_DETAIL_NOTE = "NODE_DETAIL_NOTE";
            DataModeler.NODE.NODE_ID_REF = "NODE_ID_REF";
            DataModeler.NODE.NODE_SEQ_REF = "NODE_SEQ_REF";
            DataModeler.NODE.NODE_DETAIL_DATA_TYPE = "NODE_DETAIL_DATA_TYPE";
            DataModeler.NODE.NODE_DETAIL_DATA_TYPE_FULL = "NODE_DETAIL_DATA_TYPE_FULL";
            DataModeler.NODE.NODE_DETAIL_COMMENT = "NODE_DETAIL_COMMENT";
            DataModeler.NODE.NODE_DETAIL_TABLE_ALIAS = "NODE_DETAIL_TABLE_ALIAS";
            DataModeler.NODE.NODE_DETAIL_ORDINAL_POSITION = "NODE_DETAIL_ORDINAL_POSITION";
            DataModeler.NODE.NODE_DETAIL_IS_PRIMARY_KEY = "NODE_DETAIL_IS_PRIMARY_KEY";
            DataModeler.NODE.NODE_DETAIL_VIEW_COLUMN1 = "NODE_DETAIL_VIEW_COLUMN1";
            DataModeler.NODE.NODE_DETAIL_VIEW_COLUMN2 = "NODE_DETAIL_VIEW_COLUMN2";

            DataModeler.RELATION.RELATION_TYPE = "RELATION_TYPE";
            DataModeler.RELATION.RELATION_OPERATOR = "RELATION_OPERATOR";
            DataModeler.RELATION.RELATION_VALUE = "RELATION_VALUE";
            DataModeler.RELATION.RELATION_NOTE = "RELATION_NOTE";
            DataModeler.RELATION.NODE_ID1 = "NODE_ID1";
            DataModeler.RELATION.NODE_SEQ1 = "NODE_SEQ1";
            DataModeler.RELATION.NODE_DETAIL_ID1 = "NODE_DETAIL_ID1";
            DataModeler.RELATION.NODE_DETAIL_SEQ1 = "NODE_DETAIL_SEQ1";
            DataModeler.RELATION.NODE_DETAIL_ORDER1 = "NODE_DETAIL_ORDER1";
            DataModeler.RELATION.NODE_ID2 = "NODE_ID2";
            DataModeler.RELATION.NODE_SEQ2 = "NODE_SEQ2";
            DataModeler.RELATION.NODE_DETAIL_ID2 = "NODE_DETAIL_ID2";
            DataModeler.RELATION.NODE_DETAIL_SEQ2 = "NODE_DETAIL_SEQ2";
            DataModeler.RELATION.NODE_DETAIL_ORDER2 = "NODE_DETAIL_ORDER2";
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            dataModeler.Clear();
        }

        private void btAddTableNode_Click(object sender, EventArgs e)
        {
            // 중복된 Node를 저장하기 위한 List입니다.
            List<string> duplicatedNodeList = new List<string>();

            // DataModeler의 갱신을 멈추기 위해 사용됩니다.
            // ST.Core에 있는 Control의 확장 메서드입니다.
            dataModeler.BeginControlUpdate();

            // ------------ 첫 번째 TableNode (ID: SYS_USER) 생성 및 추가 ------------
            // 생성될 TableNode 중 첫 번째 Node의 아이디를 정의합니다.
            string nodeID1 = "SYS_USER";

            // SYS_USER DataTable 생성
            // DataModeler.GetEmptyNodeDataTable는 빈 Node DataTable을 반환하는 정적 메서드입니다.
            // 위 메서드와 Rows.Add로 Node의 DataTable와 데이터를 생성합니다.
            DataTable tableNode1Data = DataModeler.GetEmptyNodeDataTable();
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "USER_ID"       , 0, 1 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "사용자 ID"      , "", 1 , "Y", "USER_ID [varchar2(30)]"       , "사용자 ID"       });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "USER_NAME"     , 0, 2 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(50)" , "사용자명"       , "", 2 , "N", "USER_NAME [varchar2(50)]"     , "사용자명"        });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "USER_GROUP"    , 0, 3 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "사용자 그룹"    , "", 3 , "N", "USER_GROUP [varchar2(30)]"    , "사용자 그룹"     });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "USER_PASSWD"   , 0, 4 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(200)", "사용자 비밀번호", "", 4 , "N", "USER_PASSWD [varchar2(200)]"  , "사용자 비밀번호" });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "USER_POSITION" , 0, 5 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "사용자 직위"    , "", 5 , "N", "USER_POSITION [varchar2(30)]" , "사용자 직위"     });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "USER_RESPONS"  , 0, 6 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "사용자 직책"    , "", 6 , "N", "USER_RESPONS [varchar2(30)]"  , "사용자 직책"     });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "EMP_NO"        , 0, 7 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(10)" , "사원 번호"      , "", 7 , "N", "EMP_NO [varchar2(10)]"        , "사원 번호"       });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "EMAIL_ADDRESS" , 0, 8 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(100)", "이메일 주소"    , "", 8 , "N", "EMAIL_ADDRESS [varchar2(100)]", "이메일 주소"     });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "MOBILE_PHONE"  , 0, 9 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(50)" , "휴대폰 번호"    , "", 9 , "N", "MOBILE_PHONE [varchar2(50)]"  , "휴대폰 번호"     });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "USE_YN"        , 0, 10, "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(1)"  , "사용 여부"      , "", 10, "N", "USE_YN [varchar2(1)]"         , "사용 여부"       });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "NOTE"          , 0, 11, "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(500)", "비고"           , "", 11, "N", "NOTE [varchar2(500)]"         , "비고"            });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "INSERT_USER_ID", 0, 12, "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "등록자 ID"      , "", 12, "N", "INSERT_USER_ID [varchar2(30)]", "등록자 ID"       });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "INSERT_DATE"   , 0, 13, "C", "", "SYS_USER", 0, "DATE"    , "DATE"         , "등록 일시"      , "", 13, "N", "INSERT_DATE [date]"           , "등록 일시"       });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "UPDATE_USER_ID", 0, 14, "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "수정자 ID"      , "", 14, "N", "UPDATE_USER_ID [varchar2(30)]", "수정자 ID"       });
            tableNode1Data.Rows.Add(new object[] { nodeID1, 0, "UPDATE_DATE"   , 0, 15, "C", "", "SYS_USER", 0, "DATE"    , "DATE"         , "수정 일시"      , "", 15, "N", "UPDATE_DATE [date]"           , "수정 일시"       });

            // SYS_USER - TableNode 생성
            // TableNode 객체를 생성합니다. 생성자는 부모가 될 DataModeler를 파라미터로 전달 받습니다.
            TableNode tableNode1 = new TableNode(dataModeler);
            // TableNode의 ID를 설정합니다.
            tableNode1.ID = nodeID1;
            // TableNode의 Location을 설정합니다.
            tableNode1.Location = new Point(200, 100);
            // TableNode의 SEQ를 설정합니다.
            // 동일 TableNode를 추가하고 싶을 때는 SEQ를 증가시켜 사용할 수 있습니다.
            tableNode1.SEQ = 0;
            // Node의 ScaleValue(확대/축소 값. 1: 100%, 0.9: 90%, 1.1: 110%)를
            // DataModeler의 ScaleValue와 동일하게 설정합니다.
            tableNode1.ScaleValue = dataModeler.ScaleValue;

            // 위에서 정의된 SYS_USER DataTable를 바인드합니다.
            tableNode1.Bind(tableNode1Data);

            // 중복 추가를 방지하기 위해 DataModeler 인스턴스의 Controls에 요소가 있는지 여부를 확인합니다.
            if (!dataModeler.ContainsNode(tableNode1))
            {
                // DataModeler TableNode를 추가합니다.
                // DataModeler에 Controls는 일반적으로 사용되는 ControlCollection 클래스가 아니라
                // GraphicControlCollection 클래스로 되어있습니다.
                // TableNode가 Control 클래스에서 상속된 게 아니라 ST.DataModeler.GraphicControl 클래스로 부터
                // 상속되어 만들어졌기 때문입니다.
                dataModeler.Controls.Add(tableNode1);
            }
            else
            {
                // 오류 메시지를 띄우기 위해 중복된 TableNode의 아이디를 저장합니다.
                duplicatedNodeList.Add($"[{nodeID1}]");
            }


            // ------------ 두 번째 TableNode (ID: SYS_CODE) 생성 및 추가 ------------
            // 상세 코드는 첫 번째 TableNode 생성과 동일합니다.
            string nodeID2 = "SYS_CODE";

            // SYS_CODE - Data
            DataTable tableNode2Data = DataModeler.GetEmptyNodeDataTable();
            tableNode2Data.Rows.Add(new object[] { nodeID2, 0, "CODE_ID"       , 0, 1, "C", "", "SYS_CODE", 0, "VARCHAR2", "VARCHAR2(30)" , "코드 ID"  , "", 1 , "Y", "CODE_ID [varchar2(30)]"       , "코드 ID"   });
            tableNode2Data.Rows.Add(new object[] { nodeID2, 0, "CODE_GROUP"    , 0, 2, "C", "", "SYS_CODE", 0, "VARCHAR2", "VARCHAR2(30)" , "코드 그룹", "", 2 , "N", "CODE_GROUP [varchar2(30)]"    , "코드 그룹" });
            tableNode2Data.Rows.Add(new object[] { nodeID2, 0, "CODE_NM"       , 0, 3, "C", "", "SYS_CODE", 0, "VARCHAR2", "VARCHAR2(100)", "코드명"   , "", 3 , "N", "CODE_NM [varchar2(100)]"      , "코드명"    });
            tableNode2Data.Rows.Add(new object[] { nodeID2, 0, "SORT_ORDER"    , 0, 4, "C", "", "SYS_CODE", 0, "NUMBER"  , "NUMBER"       , "정렬 순서", "", 6 , "N", "SORT_ORDER [number]"          , "정렬 순서" });
            tableNode2Data.Rows.Add(new object[] { nodeID2, 0, "USE_YN"        , 0, 5, "C", "", "SYS_CODE", 0, "VARCHAR2", "VARCHAR2(1)"  , "사용 여부", "", 7 , "N", "USE_YN [varchar2(1)]"         , "사용 여부" });
            tableNode2Data.Rows.Add(new object[] { nodeID2, 0, "NOTE"          , 0, 6, "C", "", "SYS_CODE", 0, "VARCHAR2", "VARCHAR2(500)", "비고"     , "", 8 , "N", "NOTE [varchar2(500)]"         , "비고"      });
            tableNode2Data.Rows.Add(new object[] { nodeID2, 0, "INSERT_USER_ID", 0, 7, "C", "", "SYS_CODE", 0, "VARCHAR2", "VARCHAR2(30)" , "등록자 ID", "", 9 , "N", "INSERT_USER_ID [varchar2(30)]", "등록자 ID" });
            tableNode2Data.Rows.Add(new object[] { nodeID2, 0, "INSERT_DATE"   , 0, 8, "C", "", "SYS_CODE", 0, "DATE"    , "DATE"         , "등록 일시", "", 10, "N", "INSERT_DATE [date]"           , "등록 일시" });

            // SYS_CODE - Node
            TableNode tableNode2 = new TableNode(dataModeler);
            tableNode2.Location = new Point(550, 150);
            tableNode2.ID = nodeID2;
            tableNode2.SEQ = 0;
            tableNode2.ScaleValue = dataModeler.ScaleValue;

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
            // DataModeler의 갱신을 재시작하기 위해 사용됩니다.
            // ST.Core에 있는 Control의 확장 메서드 입니다.
            dataModeler.EndControlUpdate();

            // 이미 추가된 TableNode는 메시지로 사용자에게 알립니다.
            if (duplicatedNodeList.Count > 0)
            {
                ModalMessageBox.Show(this, $"{ string.Join(", ", duplicatedNodeList) }는 이미 추가되었습니다.", "AddTableNode");
            }
        }

        private void btAddRelation_Click(object sender, EventArgs e)
        {
            // Validation - 테스트 Relation을 추가하기 위해 TableNode가 존재하는지 검사하는 임시 코드
            List<string> valudationIDList = new List<string>();
            // DataModeler에 TableNode인 SYS_CODE와 SYS_USER가 존재하는지 확인하기 위해
            // dataModeler.Controls 을 foreach 문으로 검사합니다.
            foreach (var control in dataModeler.Controls)
            {
                TableNode node = control as TableNode;
                if (node != null)
                {
                    if ((node.ID == "SYS_CODE" || node.ID == "SYS_USER") && node.SEQ == 0)
                    {
                        if (!valudationIDList.Contains(node.ID))
                        {
                            valudationIDList.Add(node.ID);
                        }
                    }
                }
            }
            // TableNode인 SYS_CODE와 SYS_USER가 존재하지 않으면 Relation을 추가하지 않습니다.
            if (valudationIDList.Count != 2)
            {
                ModalMessageBox.Show("SYS_CODE(0) 테이블과 SYS_USER(0) 테이블이 존재해야 Relation을 추가할 수 있습니다.", "AddRelation");
                return;
            }

            // Relation DataTable 생성
            // DataModeler.GetEmptyRelationDataTable는 빈 Relation DataTable을 반환하는 정적 메서드입니다.
            // 위 메서드와 Rows.Add로 Relation의 DataTable와 데이터를 생성합니다.
            DataTable relationDt = DataModeler.GetEmptyRelationDataTable();
            relationDt.Rows.Add(new object[] { "I", "=", "'CPOSI'", "", "SYS_CODE", 0, "CODE_GROUP", 0, 2, "SYS_USER", 0, ""             , 0, 0 });
            relationDt.Rows.Add(new object[] { "I", "" , ""       , "", "SYS_CODE", 0, "CODE_ID"   , 0, 1, "SYS_USER", 0, "USER_POSITION", 0, 5 });

            // 생성한 Relation DataTable을 순차적으로 돌며 dataModeler.Relations에 추가합니다.
            foreach (DataRow row in relationDt.Rows)
            {
                // RelationControl 객체를 생성합니다.
                // 생성자는 부모가 될 DataModeler와 DataRow 또는 RelationModel을 파라미터로 전달 받습니다.
                RelationControl relationControl = new RelationControl(dataModeler, row);

                // DataModeler에 Relation을 추가합니다.
                // DataModeler의 Relations는 RelationControlCollection 클래스로 되어있습니다.
                dataModeler.Relations.Add(relationControl);
            }

            // 화면 갱신을 위해 DataModeler의 Refresh 메서드를 호출합니다.
            dataModeler.Refresh();
        }


        private void btAddColumnNode_Click(object sender, EventArgs e)
        {
            // 생성될 ColumnNode의 아이디를 정의합니다.
            string nodeID = "C1";

            // C1 DataTable 생성
            // DataModeler.GetEmptyNodeDataTable는 빈 Node DataTable을 반환하는 정적 메서드입니다.
            // 위 메서드와 Rows.Add로 Node의 DataTable와 데이터를 생성합니다.
            DataTable columnNodeData = DataModeler.GetEmptyNodeDataTable();
            columnNodeData.Rows.Add(new object[] { nodeID, 0, "USER_ID"       , 0, 1 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "사용자 ID"      , "", 1 , "Y", "USER_ID [varchar2(30)]"       , "사용자 ID"       });
            columnNodeData.Rows.Add(new object[] { nodeID, 0, "USER_NAME"     , 0, 2 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(50)" , "사용자명"       , "", 2 , "N", "USER_NAME [varchar2(50)]"     , "사용자명"        });
            columnNodeData.Rows.Add(new object[] { nodeID, 0, "USER_GROUP"    , 0, 3 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "사용자 그룹"    , "", 3 , "N", "USER_GROUP [varchar2(30)]"    , "사용자 그룹"     });
            columnNodeData.Rows.Add(new object[] { nodeID, 0, "USER_PASSWD"   , 0, 4 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(200)", "사용자 비밀번호", "", 4 , "N", "USER_PASSWD [varchar2(200)]"  , "사용자 비밀번호" });
            columnNodeData.Rows.Add(new object[] { nodeID, 0, "USER_POSITION" , 0, 5 , "C", "", "SYS_USER", 0, "VARCHAR2", "VARCHAR2(30)" , "사용자 직위"    , "", 5 , "N", "USER_POSITION [varchar2(30)]" , "사용자 직위"     });
            columnNodeData.Rows.Add(new object[] { nodeID, 0, "CODE_NM"       , 0, 6 , "C", "", "SYS_CODE", 0, "VARCHAR2", "VARCHAR2(100)", "코드명"         , "", 3 , "N", "CODE_NM [varchar2(100)]"      , "코드명"          });

            // C1 - ColumnNode 생성
            // ColumnNode 객체를 생성합니다. 생성자는 부모가 될 DataModeler를 파라미터로 전달 받습니다.
            ColumnNode columnNode = new ColumnNode(dataModeler);
            // ColumnNode의 ID를 설정합니다.
            columnNode.ID = nodeID;
            // ColumnNode의 Location을 설정합니다.
            columnNode.Location = new Point(653, 46);
            // 일반적으로 ColumnNode는 SEQ를 0으로 고정하고 사용합니다.
            columnNode.SEQ = 0;
            // Node의 ScaleValue(확대/축소 값. 1: 100%, 0.9: 90%, 1.1: 110%)를
            // DataModeler의 ScaleValue와 동일하게 설정합니다.
            columnNode.ScaleValue = dataModeler.ScaleValue;

            // 위에서 정의된 C1 DataTable를 바인드합니다.
            columnNode.Bind(columnNodeData);
            // 중복 추가를 방지하기 위해 DataModeler 인스턴스의 Controls에 요소가 있는지 여부를 확인합니다.
            if (!dataModeler.ContainsNode(columnNode))
            {
                // DataModeler ColumnNode를 추가합니다.
                dataModeler.Controls.Add(columnNode);
            }

            // 화면 갱신을 위해 DataModeler의 Refresh 메서드를 호출합니다.
            dataModeler.Refresh();
        }

        private void btAddMemoNode_Click(object sender, EventArgs e)
        {
            // 생성될 MemoNode의 아이디를 정의합니다.
            string nodeID = "M1";

            // M1 - MemoNode 생성
            // MemoNode 객체를 생성합니다. 생성자는 부모가 될 DataModeler를 파라미터로 전달 받습니다.
            MemoNode memoNode = new MemoNode(dataModeler);
            // MemoNode의 ID를 설정합니다.
            memoNode.ID = nodeID;
            // MemoNode의 Location을 설정합니다.
            memoNode.Location = new Point(507, 267);
            // 일반적으로 MemoNode는 SEQ를 0으로 고정하고 사용합니다.
            memoNode.SEQ = 0;
            // MemoNode의 크기를 설정합니다.
            memoNode.Size = new Size(200, 200);
            // MemoNode의 내용을 설정합니다.
            memoNode.NodeNote = "memo...";
            // MemoNode의 ScaleValue(확대/축소 값. 1: 100%, 0.9: 90%, 1.1: 110%)를
            // DataModeler의 ScaleValue와 동일하게 설정합니다.
            memoNode.ScaleValue = dataModeler.ScaleValue;

            // 중복 추가를 방지하기 위해 DataModeler 인스턴스의 Controls에 요소가 있는지 여부를 확인합니다.
            if (!dataModeler.ContainsNode(memoNode))
            {
                // DataModeler MemoNode를 추가합니다.
                dataModeler.Controls.Add(memoNode);
            }

            // 화면 갱신을 위해 DataModeler의 Refresh 메서드를 호출합니다.
            dataModeler.Refresh();
        }

        private void btToggleEnabled_Click(object sender, EventArgs e)
        {
            // 사용 가능 여부를 설정합니다.
            dataModeler.Enabled = !dataModeler.Enabled;
        }

        private void btScalePlus_Click(object sender, EventArgs e)
        {
            // 자식 컨트롤(Node, Relation)의 배율을 +10% 증가 시킵니다.
            dataModeler.ScaleValue += 0.1f;
        }

        private void btScaleMinus_Click(object sender, EventArgs e)
        {
            // 자식 컨트롤(Node, Relation)의 배율을 -10% 감소 시킵니다.
            dataModeler.ScaleValue -= 0.1f;
        }

        private void btGetMainData_Click(object sender, EventArgs e)
        {
            // DataModeler의 내부 좌표를 가져옵니다.
            Point innerLocation = dataModeler.InnerLocation;
            // DataModeler의 배율 정보를 가져옵니다.
            float scaleValue = dataModeler.ScaleValue;

            // DataModeler의 내부 좌표와 배율 정보를 메시지박스로 띄움니다.
            ModalMessageBox.Show(this, $"innerLocation : ({innerLocation.X}, {innerLocation.Y}), ScaleValue : {scaleValue}", "GetMainData", MessageBoxButtons.OK);
        }

        private void btGetNodeData_Click(object sender, EventArgs e)
        {
            // DataModeler 내부 Nodes 데이터를 저장합니다.(DataModelerNodeDataTables 클래스 형태)
            var dataTables = dataModeler.GetNodeDataTables();

            // DataModelerNodeDataTables 클래스는 Node, NodeDetail 필드에 각각 노드 메인 정보와 노드 디테일 정보를 가지고 있습니다.
            DataTable node = dataTables.Node;
            DataTable nodeDetail = dataTables.NodeDetail;

            // ColumnNode를 제외한 다른 Node들의 Detail 정보를 제거합니다.
            // DataModeler가 사용될 때 일반적으로 TableNode는 DB에 저장된 값을 사용하기에
            // 사용자가 편집할 수 있는 ColumnNode의 Detail 값 만 의미 있다고 가정한 코드입니다.
            for (int i = nodeDetail.Rows.Count - 1; 0 <= i; i--)
            {
                DataRow row = nodeDetail.Rows[i];
                // 현재 Detail(Row)에서 부모 Node(Row)를 검색합니다.
                var nodeRows = node.Select($"NODE_ID = '{row["NODE_ID"]}' AND NODE_SEQ = {row["NODE_SEQ"]}");
                if (nodeRows.Length == 0)
                {
                    // 부모 Node 정보가 존재하지 않으면 Detail(Row)을 삭제합니다.
                    nodeDetail.Rows.Remove(row);
                }
                else
                {
                    // 부모 Node가 존재하지만 ColumnNode가 아니면 Detail(Row)을 삭제합니다.
                    if (nodeRows[0]["NODE_TYPE"].ToString() != "COL")
                    {
                        nodeDetail.Rows.Remove(row);
                    }
                }
            }

            // NODE_DETAIL_VIEW_COLUMN1, NODE_DETAIL_VIEW_COLUMN2 컬럼은 단순 뷰로 사용되기에 삭제합니다.
            nodeDetail.Columns.Remove("NODE_DETAIL_VIEW_COLUMN1");
            nodeDetail.Columns.Remove("NODE_DETAIL_VIEW_COLUMN2");

            ModalMessageBox.Show(this, $"Node Count : {node.Rows.Count}, NodeDetails Count : {nodeDetail.Rows.Count}", "GetNodeData", MessageBoxButtons.OK);
        }

        private void btGetRelationData_Click(object sender, EventArgs e)
        {
            // DataModeler 내부 Relations 데이터를 DataTable에 저장합니다.
            DataTable rsRelation = dataModeler.GetRelationDataTable();

            // NODE_DETAIL_TABLE_ALIAS1, NODE_DETAIL_TABLE_ALIAS2 컬럼은 DataModeler 내부적으로 사용되기에 삭제합니다.
            rsRelation.Columns.Remove("NODE_DETAIL_TABLE_ALIAS1");
            rsRelation.Columns.Remove("NODE_DETAIL_TABLE_ALIAS2");

            ModalMessageBox.Show(this, $"Relations Count : {rsRelation.Rows.Count}", "GetRelationData", MessageBoxButtons.OK);
        }
    }
}
