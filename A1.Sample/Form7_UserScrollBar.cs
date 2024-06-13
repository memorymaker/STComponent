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
            userScrollBar1.ValueChanged += UserScrollBar1_ValueChanged;
        }

        private void UserScrollBar1_ValueChanged(object sender, ST.Controls.UserScrollBarEventArgs e)
        {
            textBox1.Text = $"{userScrollBar1.Value} / {userScrollBar1.Maximum}";
        }

        private void btSetValue50_Click(object sender, EventArgs e)
        {
            userScrollBar1.Value = 50;
        }
    }
}
