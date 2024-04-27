
namespace ST.DataModeler
{
    partial class ModalUserColumn
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
            this.btApply = new ST.Controls.Modal.ModalButton();
            this.btCancel = new ST.Controls.Modal.ModalButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtColumnName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTableAlias = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDataTypeFull = new System.Windows.Forms.TextBox();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btApply
            // 
            this.btApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btApply.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btApply.ForeColor = System.Drawing.Color.White;
            this.btApply.Location = new System.Drawing.Point(86, 148);
            this.btApply.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btApply.Name = "btApply";
            this.btApply.Size = new System.Drawing.Size(87, 26);
            this.btApply.TabIndex = 4;
            this.btApply.Text = "Apply";
            this.btApply.UseVisualStyleBackColor = false;
            this.btApply.Click += new System.EventHandler(this.btApply_Click);
            // 
            // btCancel
            // 
            this.btCancel.BackColor = System.Drawing.Color.DimGray;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCancel.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btCancel.ForeColor = System.Drawing.Color.White;
            this.btCancel.Location = new System.Drawing.Point(179, 148);
            this.btCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(87, 26);
            this.btCancel.TabIndex = 5;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = false;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Column Name";
            // 
            // txtColumnName
            // 
            this.txtColumnName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.txtColumnName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtColumnName.Location = new System.Drawing.Point(106, 42);
            this.txtColumnName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtColumnName.Name = "txtColumnName";
            this.txtColumnName.Size = new System.Drawing.Size(232, 16);
            this.txtColumnName.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "Data Type Full";
            // 
            // txtTableAlias
            // 
            this.txtTableAlias.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.txtTableAlias.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTableAlias.Location = new System.Drawing.Point(106, 114);
            this.txtTableAlias.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTableAlias.Name = "txtTableAlias";
            this.txtTableAlias.Size = new System.Drawing.Size(232, 16);
            this.txtTableAlias.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 15);
            this.label1.TabIndex = 11;
            this.label1.Text = "Table Alias";
            // 
            // txtDataTypeFull
            // 
            this.txtDataTypeFull.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.txtDataTypeFull.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDataTypeFull.Location = new System.Drawing.Point(106, 66);
            this.txtDataTypeFull.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDataTypeFull.Name = "txtDataTypeFull";
            this.txtDataTypeFull.Size = new System.Drawing.Size(232, 16);
            this.txtDataTypeFull.TabIndex = 1;
            // 
            // txtComment
            // 
            this.txtComment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.txtComment.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtComment.Location = new System.Drawing.Point(106, 90);
            this.txtComment.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(232, 16);
            this.txtComment.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 15);
            this.label3.TabIndex = 13;
            this.label3.Text = "Comment";
            // 
            // ModalUserColumn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 190);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.txtDataTypeFull);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTableAlias);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtColumnName);
            this.Controls.Add(this.btApply);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.label2);
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "ModalUserColumn";
            this.Text = "User Column";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.Modal.ModalButton btApply;
        private Controls.Modal.ModalButton btCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtColumnName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTableAlias;
        private System.Windows.Forms.TextBox txtDataTypeFull;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtComment;
    }
}