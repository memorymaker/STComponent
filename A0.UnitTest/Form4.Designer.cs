namespace UnitTest
{
    partial class Form4
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
            this.button1 = new System.Windows.Forms.Button();
            this.userEditor1 = new ST.Controls.UserEditor();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(846, 500);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // userEditor1
            // 
            this.userEditor1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.userEditor1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.userEditor1.Font = new System.Drawing.Font("돋움체", 10F);
            this.userEditor1.Location = new System.Drawing.Point(12, 12);
            this.userEditor1.Name = "userEditor1";
            this.userEditor1.ReadOnly = false;
            this.userEditor1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.userEditor1.SelectionLength = 0;
            this.userEditor1.SelectionStart = 0;
            this.userEditor1.ShowSelectoinInfo = true;
            this.userEditor1.Size = new System.Drawing.Size(400, 512);
            this.userEditor1.TabIndex = 0;
            this.userEditor1.TabSpaceCount = 4;
            this.userEditor1.TabStop = false;
            this.userEditor1.WordWrap = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(846, 529);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 564);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.userEditor1);
            this.Name = "Form4";
            this.Text = "Form4";
            this.ResumeLayout(false);

        }

        #endregion

        private ST.Controls.UserEditor userEditor1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}