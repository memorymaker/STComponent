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
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
            LoadThis();
        }

        private void LoadThis()
        {
            // 이벤트를 바인딩합니다.
            userScrollBar.ValueChanged += UserScrollBar_ValueChanged;
        }

        private void UserScrollBar_ValueChanged(object sender, ST.Controls.UserScrollBarEventArgs e)
        {
            // 스크롤의 값이 변경될 때 텍스트 박스에 값을 표시합니다.
            textBox1.Text = $"{userScrollBar.Value} / {userScrollBar.Maximum}";
        }

        private void btSetValue50_Click(object sender, EventArgs e)
        {
            // 스크롤 값을 설정합니다.
            userScrollBar.Value = 50;
        }

        private void btSetConfig_Click(object sender, EventArgs e)
        {
            // 스크롤할 수 있는 범위의 하한 값을 가져오거나 설장합니다.
            userScrollBar.Minimum = 10;
            // 스크롤할 수 있는 범위의 상한 값을 가져오거나 설장합니다.
            userScrollBar.Maximum = 200;
            // 스크롤 상자를 많이 이동시킬 때 증가되거나 감소되는 값을 설정합니다.
            userScrollBar.LargeChange = 20;
            // 스크롤 상자를 조금 이동시킬 때 증가되거나 감소되는 값을 설정합니다.
            userScrollBar.SmallChange = 2;
            // 스크롤 값을 설정합니다.
            userScrollBar.Value = 0;
        }

        private void btSetColor_Click(object sender, EventArgs e)
        {
            // 기본 색상을 설정합니다.
            userScrollBar.DecrementButtonColor = Color.FromArgb(255, 0, 0);
            userScrollBar.EncrementButtonColor = Color.FromArgb(0, 0, 255);
            userScrollBar.ScrollButtonColor = Color.FromArgb(0, 192, 0);

            // 컨트롤의 Enabled 값이 false일 때의 색상입니다.
            userScrollBar.DecrementButtonDisabledColor = Color.FromArgb(255, 180, 180);
            userScrollBar.EncrementButtonDisabledColor = Color.FromArgb(180, 180, 255);
            userScrollBar.ScrollButtonDisabledColor = Color.FromArgb(200, 255, 200);

            // 감소 버튼의 마우스 오버 시 색상을 설정합니다.
            userScrollBar.DecrementButtonOverColor = Color.FromArgb(200, 0, 0);
            // 감소 버튼의 마우스 다운 시 색상을 설정합니다.
            userScrollBar.DecrementButtonMouseDownColor = Color.FromArgb(155, 0, 0);

            // 증가 버튼의 마우스 오버 시 색상을 설정합니다.
            userScrollBar.EncrementButtonOverColor = Color.FromArgb(0, 0, 200);
            // 증가 버튼의 마우스 다운 시 색상을 설정합니다.
            userScrollBar.EncrementButtonMouseDownColor = Color.FromArgb(0, 0, 155);

            // 스크롤 버튼의 마우스 오버 시 색상을 설정합니다.
            userScrollBar.ScrollButtonOverColor = Color.FromArgb(0, 152, 0);
            // 스크롤 버튼의 마우스 다운 시 색상을 설정합니다.
            userScrollBar.ScrollButtonMouseDownColor = Color.FromArgb(0, 102, 0);
        }

        private void btToggleEnabled_Click(object sender, EventArgs e)
        {
            // 사용 가능 여부를 설정합니다.
            userScrollBar.Enabled = !userScrollBar.Enabled;
        }
    }
}
