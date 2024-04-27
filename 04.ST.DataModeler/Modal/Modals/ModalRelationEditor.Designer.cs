
namespace ST.DataModeler
{
    partial class ModalRelationEditor
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
            this.cboRelationType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtOrigin = new System.Windows.Forms.TextBox();
            this.txtDestination = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btApply
            // 
            this.btApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btApply.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btApply.ForeColor = System.Drawing.Color.White;
            this.btApply.Location = new System.Drawing.Point(86, 128);
            this.btApply.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btApply.Name = "btApply";
            this.btApply.Size = new System.Drawing.Size(87, 26);
            this.btApply.TabIndex = 0;
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
            this.btCancel.Location = new System.Drawing.Point(179, 128);
            this.btCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(87, 26);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = false;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // cboRelationType
            // 
            this.cboRelationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRelationType.FormattingEnabled = true;
            this.cboRelationType.Location = new System.Drawing.Point(98, 90);
            this.cboRelationType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboRelationType.Name = "cboRelationType";
            this.cboRelationType.Size = new System.Drawing.Size(239, 23);
            this.cboRelationType.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Origin";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "Relation Type";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Destination";
            // 
            // txtOrigin
            // 
            this.txtOrigin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.txtOrigin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOrigin.ForeColor = System.Drawing.Color.DimGray;
            this.txtOrigin.Location = new System.Drawing.Point(99, 42);
            this.txtOrigin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtOrigin.Name = "txtOrigin";
            this.txtOrigin.ReadOnly = true;
            this.txtOrigin.Size = new System.Drawing.Size(239, 16);
            this.txtOrigin.TabIndex = 9;
            // 
            // txtDestination
            // 
            this.txtDestination.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.txtDestination.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDestination.ForeColor = System.Drawing.Color.DimGray;
            this.txtDestination.Location = new System.Drawing.Point(99, 66);
            this.txtDestination.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDestination.Name = "txtDestination";
            this.txtDestination.ReadOnly = true;
            this.txtDestination.Size = new System.Drawing.Size(239, 16);
            this.txtDestination.TabIndex = 9;
            // 
            // ModalRelationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 166);
            this.Controls.Add(this.txtDestination);
            this.Controls.Add(this.txtOrigin);
            this.Controls.Add(this.btApply);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboRelationType);
            this.Controls.Add(this.label2);
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "ModalRelationEditor";
            this.Text = "Realtion Editor(Column To Column)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.Modal.ModalButton btApply;
        private Controls.Modal.ModalButton btCancel;
        private System.Windows.Forms.ComboBox cboRelationType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtOrigin;
        private System.Windows.Forms.TextBox txtDestination;
    }
}