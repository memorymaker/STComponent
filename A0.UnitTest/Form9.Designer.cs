namespace UnitTest
{
    partial class Form9
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
            this.userSplitContainerInner1 = new ST.Controls.UserSplitContainer();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.userSplitContainer1 = new ST.Controls.UserSplitContainer();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.userSplitContainerInner1)).BeginInit();
            this.userSplitContainerInner1.Panel1.SuspendLayout();
            this.userSplitContainerInner1.Panel2.SuspendLayout();
            this.userSplitContainerInner1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userSplitContainer1)).BeginInit();
            this.userSplitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // userSplitContainerInner1
            // 
            this.userSplitContainerInner1.BackColor = System.Drawing.Color.LightGray;
            this.userSplitContainerInner1.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.userSplitContainerInner1.Location = new System.Drawing.Point(33, 41);
            this.userSplitContainerInner1.Name = "userSplitContainerInner1";
            // 
            // userSplitContainerInner1.Panel1
            // 
            this.userSplitContainerInner1.Panel1.Controls.Add(this.button1);
            // 
            // userSplitContainerInner1.Panel2
            // 
            this.userSplitContainerInner1.Panel2.Controls.Add(this.button2);
            this.userSplitContainerInner1.Size = new System.Drawing.Size(351, 290);
            this.userSplitContainerInner1.SplitterColor = System.Drawing.Color.DodgerBlue;
            this.userSplitContainerInner1.SplitterDistance = 115;
            this.userSplitContainerInner1.SplitterMouseOverColor = System.Drawing.Color.Red;
            this.userSplitContainerInner1.SplitterWidthRevision = 4;
            this.userSplitContainerInner1.TabIndex = 1;
            this.userSplitContainerInner1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(21, 37);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(52, 160);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(25, 60);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(99, 114);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // userSplitContainer1
            // 
            this.userSplitContainer1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.userSplitContainer1.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.userSplitContainer1.Location = new System.Drawing.Point(465, 41);
            this.userSplitContainer1.Name = "userSplitContainer1";
            this.userSplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.userSplitContainer1.Size = new System.Drawing.Size(304, 290);
            this.userSplitContainer1.SplitterColor = System.Drawing.Color.Blue;
            this.userSplitContainer1.SplitterDistance = 84;
            this.userSplitContainer1.SplitterMouseOverColor = System.Drawing.Color.Red;
            this.userSplitContainer1.SplitterWidthRevision = 8;
            this.userSplitContainer1.TabIndex = 2;
            this.userSplitContainer1.TabStop = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 1;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form9
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.userSplitContainerInner1);
            this.Controls.Add(this.userSplitContainer1);
            this.Controls.Add(this.button3);
            this.Name = "Form9";
            this.Text = "Form9";
            this.userSplitContainerInner1.Panel1.ResumeLayout(false);
            this.userSplitContainerInner1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.userSplitContainerInner1)).EndInit();
            this.userSplitContainerInner1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.userSplitContainer1)).EndInit();
            this.userSplitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ST.Controls.UserSplitContainer userSplitContainerInner1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private ST.Controls.UserSplitContainer userSplitContainer1;
    }
}