namespace UnitTest
{
    partial class Form6
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
            this.userListView1 = new ST.Controls.UserListView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // userListView1
            // 
            this.userListView1.AllowDrop = true;
            this.userListView1.AutoSizeType = ST.Controls.UserListAutoSizeType.None;
            this.userListView1.BackColor = System.Drawing.Color.White;
            this.userListView1.ColumnHeight = 26;
            this.userListView1.ColumnHorizontalDistance = 0;
            this.userListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.userListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userListView1.ItemHeight = 20;
            this.userListView1.ItemPadding = new System.Windows.Forms.Padding(4);
            this.userListView1.ItemVerticalDistance = 1;
            this.userListView1.ItemVerticalDistanceColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.userListView1.Location = new System.Drawing.Point(0, 0);
            this.userListView1.MinimapColor = System.Drawing.Color.Blue;
            this.userListView1.Name = "userListView1";
            this.userListView1.OriginalHeight = 0;
            this.userListView1.OriginalLeft = 0;
            this.userListView1.OriginalTop = 0;
            this.userListView1.OriginalWidth = 0;
            this.userListView1.ScaleValue = 1F;
            this.userListView1.ScrollLeft = 0;
            this.userListView1.ScrollTop = 0;
            this.userListView1.SelectedItemIndex = -1;
            this.userListView1.Size = new System.Drawing.Size(310, 275);
            this.userListView1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(60, 198);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(158, 198);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 275);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.userListView1);
            this.Name = "Form6";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form6";
            this.ResumeLayout(false);

        }

        #endregion

        private ST.Controls.UserListView userListView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}