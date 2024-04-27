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

namespace UnitTest
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
			StartPosition = FormStartPosition.CenterScreen;
			Load += Form3_Load;
		}

		private void Form3_Load(object sender, EventArgs e)
		{
			UserPanel panel1 = new UserPanel();
			panel1.Size = new Size(200, 100);
			panel1.Location = new Point(10, 10);
			panel1.Title = "panel1";
			wrapPanel.Controls.Add(panel1);

			Panel t1 = new Panel();
            t1.BackColor = Color.Gray;
			t1.Dock = DockStyle.Fill;
            panel1.Controls.Add(t1);

            UserPanel panel2 = new UserPanel();
			panel2.Size = new Size(200, 100);
			panel2.Location = new Point(210, 10);
			panel2.Title = "panel2";
			wrapPanel.Controls.Add(panel2);

			UserPanel panel3 = new UserPanel();
			panel3.Size = new Size(200, 100);
			panel3.Location = new Point(10, 110);
			panel3.Title = "panel3";
			wrapPanel.Controls.Add(panel3);

			UserPanel panel4 = new UserPanel();
			panel4.Size = new Size(200, 100);
			panel4.Location = new Point(210, 110);
			panel4.Title = "panel4";
			wrapPanel.Controls.Add(panel4);
		}
	}
}
