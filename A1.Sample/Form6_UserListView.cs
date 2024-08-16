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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
            LoadThis();
        }

        private void LoadThis()
        {
            // 리스트의 항목의 드래그를 막습니다.
            userListView.AllowDrag = false;
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            // UserList의 모든 항목을 삭제합니다.
            userListView.Clear();

            // UserList의 모든 컬럼을 삭제합니다.
            userListView.Columns.Clear();
        }

        private void btAddColumn_Click(object sender, EventArgs e)
        {
            // UserList의 모든 컬럼을 삭제합니다.
            userListView.Columns.Clear();

            // 컬럼을 추가합니다.
            userListView.AddColumn(new UserListViewColumn("ID"   , "CODE_ID"   ));
            userListView.AddColumn(new UserListViewColumn("Name" , "CODE_NAME" ));
            userListView.AddColumn(new UserListViewColumn("Order", "CODE_ORDER"));
            userListView.AddColumn(new UserListViewColumn("Note" , "CODE_NOTE" ));
            
            // 컬럼 크기의 자동 조정을 좌측 우선으로 설정합니다.
            userListView.AutoSizeType = UserListAutoSizeType.LeftFirst;
        }

        private void btBindData_Click(object sender, EventArgs e)
        {
            // 샘플 DataTable을 생성합니다.
            DataTable dt = new DataTable();
            dt.AddColumns("{S}CODE_ID {S}CODE_NAME {I}CODE_ORDER {S}CODE_NOTE");

            // 데이터를 추가합니다.
            dt.Rows.Add(new object[] { "POSI0001", "사원"  , 1, "Note 1" });
            dt.Rows.Add(new object[] { "POSI0002", "대리"  , 2, "Note 2" });
            dt.Rows.Add(new object[] { "POSI0003", "과장"  , 3, "Note 3" });
            dt.Rows.Add(new object[] { "POSI0004", "차장"  , 4, "Note 4" });
            dt.Rows.Add(new object[] { "POSI0005", "부장"  , 5, "Note 5" });
            dt.Rows.Add(new object[] { "RESP0001", "파트장", 1, "Note 6" });
            dt.Rows.Add(new object[] { "RESP0002", "팀장"  , 2, "Note 7" });
            dt.Rows.Add(new object[] { "RESP0003", "본부장", 3, "Note 8" });

            // 데이터를 바인딩합니다.
            userListView.Bind(dt);
        }

        private void btSetStyle_Click(object sender, EventArgs e)
        {
            try
            {
                // Column의 스타일을 설정합니다.
                userListView.Columns[0].Font = new Font("맑은 고딕", 9f, FontStyle.Bold | FontStyle.Italic);
                userListView.Columns[1].BackColor = Color.FromArgb(255, 200, 200);
                userListView.Columns[2].ForeColor = Color.FromArgb(0, 0, 240);

                // Items(Row)의 스타일을 설정합니다.
                // Row 전체에 적용됩니다.
                userListView.Items[1].BackColor = Color.FromArgb(230, 230, 255);
                userListView.Items[2].ForeColor = Color.FromArgb(255, 0, 0);
                userListView.Items[3].Font = new Font("맑은 고딕", 9f, FontStyle.Bold | FontStyle.Italic);

                // Items(SubItem)의 스타일을 설정합니다.
                // Column 별로 적용됩니다.
                userListView.Items[5].SubItems[0].BackColor = Color.FromArgb(230, 255, 230);
                userListView.Items[5].SubItems[1].ForeColor = Color.FromArgb(0, 0, 255);
                userListView.Items[5].SubItems["CODE_NOTE"].Font = new Font("맑은 고딕", 9f, FontStyle.Bold | FontStyle.Italic);

                // UserListView를 다시 그립니다.
                userListView.Refresh();
            }
            catch(Exception ex)
            {
                ModalMessageBox.Show(ex.Message, "Set Style");
            }
        }

        private void btScalePlus_Click(object sender, EventArgs e)
        {
            // UserListView의 배율을 +10% 증가 시킵니다.
            userListView.ScaleValue += 0.1f;
        }

        private void btScaleMinus_Click(object sender, EventArgs e)
        {
            // UserListView의 배율을 -10% 감소 시킵니다.
            userListView.ScaleValue -= 0.1f;
        }
    }
}
