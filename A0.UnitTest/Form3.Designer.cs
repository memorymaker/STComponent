namespace UnitTest
{
	partial class Form3
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.wrapPanel = new ST.Controls.UserWrapPanelForUserPanel();
			this.SuspendLayout();
			// 
			// wrapPanel
			// 
			this.wrapPanel.Location = new System.Drawing.Point(12, 12);
			this.wrapPanel.Name = "wrapPanel";
			this.wrapPanel.Size = new System.Drawing.Size(480, 289);
			this.wrapPanel.TabIndex = 0;
			// 
			// Form3
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(509, 321);
			this.Controls.Add(this.wrapPanel);
			this.Name = "Form3";
			this.Text = "Form3";
			this.ResumeLayout(false);

		}

		#endregion

		private ST.Controls.UserWrapPanelForUserPanel wrapPanel;
	}
}