
namespace ST.DataModeler
{
    partial class ModalImageSave
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
            this.txtSize = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMargin = new System.Windows.Forms.TextBox();
            this.picMain = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).BeginInit();
            this.SuspendLayout();
            // 
            // btApply
            // 
            this.btApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btApply.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btApply.ForeColor = System.Drawing.Color.White;
            this.btApply.Location = new System.Drawing.Point(311, 554);
            this.btApply.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btApply.Name = "btApply";
            this.btApply.Size = new System.Drawing.Size(87, 26);
            this.btApply.TabIndex = 3;
            this.btApply.Text = "Save";
            this.btApply.UseVisualStyleBackColor = false;
            this.btApply.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btCancel
            // 
            this.btCancel.BackColor = System.Drawing.Color.DimGray;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCancel.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btCancel.ForeColor = System.Drawing.Color.White;
            this.btCancel.Location = new System.Drawing.Point(404, 554);
            this.btCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(87, 26);
            this.btCancel.TabIndex = 4;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = false;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 516);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Size";
            // 
            // txtSize
            // 
            this.txtSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.txtSize.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtSize.Location = new System.Drawing.Point(45, 516);
            this.txtSize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSize.Name = "txtSize";
            this.txtSize.ReadOnly = true;
            this.txtSize.Size = new System.Drawing.Size(97, 16);
            this.txtSize.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(157, 516);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "Margin(Max: 100)";
            // 
            // txtMargin
            // 
            this.txtMargin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.txtMargin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMargin.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtMargin.Location = new System.Drawing.Point(267, 516);
            this.txtMargin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMargin.Name = "txtMargin";
            this.txtMargin.Size = new System.Drawing.Size(97, 16);
            this.txtMargin.TabIndex = 6;
            // 
            // picMain
            // 
            this.picMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picMain.Location = new System.Drawing.Point(0, 26);
            this.picMain.Name = "picMain";
            this.picMain.Size = new System.Drawing.Size(800, 483);
            this.picMain.TabIndex = 14;
            this.picMain.TabStop = false;
            // 
            // ModalImageSave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.picMain);
            this.Controls.Add(this.txtMargin);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSize);
            this.Controls.Add(this.btApply);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.label2);
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "ModalImageSave";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Image Save";
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.Modal.ModalButton btApply;
        private Controls.Modal.ModalButton btCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMargin;
        private System.Windows.Forms.PictureBox picMain;
    }
}