using ST.Controls;
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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            LoadThis();
        }

        private void LoadThis()
        {
            userPanel1.UsingPanelMerge = true;
            userPanel2.UsingPanelMerge = true;
            userPanel3.UsingPanelMerge = false;
        }

        private void btAddPanel_Click(object sender, EventArgs e)
        {
            UserPanel panel = new UserPanel();
            panel.Bounds = new Rectangle(10, 10, 200, 100);
            panel.UsingPanelMerge = true;

            splitContainer1.Panel1.Controls.Add(panel);
            panel.BringToFrontCustom();
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel1.Controls.Clear();
        }

        private void btToggleEnabled_Click(object sender, EventArgs e)
        {
            userPanel1.Enabled = !userPanel1.Enabled;
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
    }
}
