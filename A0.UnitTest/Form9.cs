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

namespace UnitTest
{
    public partial class Form9 : Form
    {
        public Form9()
        {
            InitializeComponent();
            Load += Form9_Load;
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            var a = 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            userSplitContainerInner1.Width++;
            //userSplitContainerInner1.EndControlUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
