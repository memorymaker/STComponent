namespace UnitTest
{
    partial class Form5
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.codeGenerator1 = new ST.CodeGenerator.CodeGenerator();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(430, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 422);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(823, 31);
            this.panel1.TabIndex = 2;
            // 
            // codeGenerator1
            // 
            this.codeGenerator1.BackColor = System.Drawing.Color.White;
            this.codeGenerator1.NodeData = null;
            this.codeGenerator1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeGenerator1.Location = new System.Drawing.Point(0, 0);
            this.codeGenerator1.Name = "codeGenerator1";
            this.codeGenerator1.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.codeGenerator1.Size = new System.Drawing.Size(823, 422);
            this.codeGenerator1.TabIndex = 0;
            this.codeGenerator1.Text = "codeGenerator1";
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 453);
            this.Controls.Add(this.codeGenerator1);
            this.Controls.Add(this.panel1);
            this.Name = "Form5";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form5";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ST.CodeGenerator.CodeGenerator codeGenerator1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
    }
}