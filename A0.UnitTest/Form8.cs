using ST.Controls;
using ST.Controls.Modal.MessageBox;
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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace UnitTest
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            userSplitContainerInner1.SplitterDistance = 10;
        }

        private void modalButton5_Click(object sender, EventArgs e)
        {
            ModalMessageBox.ShowError("Connection failed. SQL Server에 연결을 설정하는 중에 네트워크 관련 또는 인스턴스 관련 오류가 발생했습니다. 서버를 찾을 수 없거나 액세스할 수 없습니다. 인스턴스 이름이 올바르고 SQL Server가 원격 연결을 허용하도록 구성되어 있는지 확인하십시오. (provider: TCP Provider, error: 0 - 네트워크 위치를 찾을 수 없습니다. 네트워크 문제 해결에 대한 내용은 Windows 도움말을 참고하십시오.)", "c", MessageBoxButtons.OK);
            //ModalMessageBox.ShowError("t 1111111111111 ", "c", MessageBoxButtons.OK);
            //var t = ModalMessageBox.Show(this, "text", "caption", MessageBoxButtons.OK);
        }

        private void modalButton2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("asdf", "aa", MessageBoxButtons.OK, MessageBoxIcon.Error);
            var t = ModalMessageBox.Show("text 11111111111 22222222222222 333333333333333 11111111111 22222222222222 333333333333333 11111111111 22222222222222 333333333333333 11111111111 22222222222222 333333333333333 11111111111 22222222222222 333333333333333 11111111111 22222222222222 333333333333333 11111111111 22222222222222 333333333333333 11111111111 22222222222222 333333333333333 11111111111 22222222222222 333333333333333 11111111111 22222222222222 333333333333333 11111111111 22222222222222 333333333333333 11111111111 22222222222222 333333333333333 11111111111 22222222222222 333333333333333 11111111111 22222222222222 333333333333333 11111111111 22222222222222 333333333333333 11111111111 22222222222222 333333333333333 11111111111 22222222222222 333333333333333", "Caption", MessageBoxButtons.RetryCancel);
        }

        private void modalButton3_Click(object sender, EventArgs e)
        {
            var t = ModalMessageBox.Show("text", "Caption", MessageBoxButtons.AbortRetryIgnore);

        }

        private void modalButton4_Click(object sender, EventArgs e)
        {
            var modal = new ModalMessageBoxForm("aaaa", "caption", MessageBoxButtons.OK);
        }

    }
}
