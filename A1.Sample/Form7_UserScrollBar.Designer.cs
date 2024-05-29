namespace Sample
{
    partial class Form7
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btScaleMinus = new ST.Controls.Modal.ModalButton();
            this.btScalePlus = new ST.Controls.Modal.ModalButton();
            this.btBindData = new ST.Controls.Modal.ModalButton();
            this.btSetStyle = new ST.Controls.Modal.ModalButton();
            this.btClear = new ST.Controls.Modal.ModalButton();
            this.btAddColumn = new ST.Controls.Modal.ModalButton();
            this.userScrollBar1 = new ST.Controls.UserScrollBar();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.userScrollBar1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.btScaleMinus);
            this.splitContainer1.Panel2.Controls.Add(this.btScalePlus);
            this.splitContainer1.Panel2.Controls.Add(this.btBindData);
            this.splitContainer1.Panel2.Controls.Add(this.btSetStyle);
            this.splitContainer1.Panel2.Controls.Add(this.btClear);
            this.splitContainer1.Panel2.Controls.Add(this.btAddColumn);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 370;
            this.splitContainer1.TabIndex = 5;
            // 
            // btScaleMinus
            // 
            this.btScaleMinus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btScaleMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btScaleMinus.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btScaleMinus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btScaleMinus.Location = new System.Drawing.Point(578, 8);
            this.btScaleMinus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btScaleMinus.Name = "btScaleMinus";
            this.btScaleMinus.Size = new System.Drawing.Size(108, 26);
            this.btScaleMinus.TabIndex = 4;
            this.btScaleMinus.Text = "Scale - 10%";
            this.btScaleMinus.UseVisualStyleBackColor = false;
            // 
            // btScalePlus
            // 
            this.btScalePlus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btScalePlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btScalePlus.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btScalePlus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btScalePlus.Location = new System.Drawing.Point(464, 8);
            this.btScalePlus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btScalePlus.Name = "btScalePlus";
            this.btScalePlus.Size = new System.Drawing.Size(108, 26);
            this.btScalePlus.TabIndex = 5;
            this.btScalePlus.Text = "Scale + 10%";
            this.btScalePlus.UseVisualStyleBackColor = false;
            // 
            // btBindData
            // 
            this.btBindData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btBindData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btBindData.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btBindData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btBindData.Location = new System.Drawing.Point(236, 8);
            this.btBindData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btBindData.Name = "btBindData";
            this.btBindData.Size = new System.Drawing.Size(108, 26);
            this.btBindData.TabIndex = 6;
            this.btBindData.Text = "Bind Data";
            this.btBindData.UseVisualStyleBackColor = false;
            // 
            // btSetStyle
            // 
            this.btSetStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btSetStyle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSetStyle.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btSetStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btSetStyle.Location = new System.Drawing.Point(350, 8);
            this.btSetStyle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btSetStyle.Name = "btSetStyle";
            this.btSetStyle.Size = new System.Drawing.Size(108, 26);
            this.btSetStyle.TabIndex = 7;
            this.btSetStyle.Text = "Set Style";
            this.btSetStyle.UseVisualStyleBackColor = false;
            // 
            // btClear
            // 
            this.btClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btClear.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btClear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btClear.Location = new System.Drawing.Point(8, 8);
            this.btClear.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(108, 26);
            this.btClear.TabIndex = 8;
            this.btClear.Text = "Clear";
            this.btClear.UseVisualStyleBackColor = false;
            // 
            // btAddColumn
            // 
            this.btAddColumn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btAddColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btAddColumn.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btAddColumn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btAddColumn.Location = new System.Drawing.Point(122, 8);
            this.btAddColumn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btAddColumn.Name = "btAddColumn";
            this.btAddColumn.Size = new System.Drawing.Size(108, 26);
            this.btAddColumn.TabIndex = 9;
            this.btAddColumn.Text = "Add Column";
            this.btAddColumn.UseVisualStyleBackColor = false;
            // 
            // userScrollBar1
            // 
            this.userScrollBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.userScrollBar1.IncreaseDecreaseButtonVisible = true;
            this.userScrollBar1.LargeChange = 10;
            this.userScrollBar1.Location = new System.Drawing.Point(12, 12);
            this.userScrollBar1.Maximum = 100;
            this.userScrollBar1.Minimum = 0;
            this.userScrollBar1.Name = "userScrollBar1";
            this.userScrollBar1.Size = new System.Drawing.Size(29, 160);
            this.userScrollBar1.SmallChange = 1;
            this.userScrollBar1.TabIndex = 0;
            this.userScrollBar1.Type = ST.Controls.UserScrollBarType.Vertical;
            this.userScrollBar1.Value = 0;
            // 
            // Form7
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form7";
            this.Text = "Form7";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ST.Controls.Modal.ModalButton btScaleMinus;
        private ST.Controls.Modal.ModalButton btScalePlus;
        private ST.Controls.Modal.ModalButton btBindData;
        private ST.Controls.Modal.ModalButton btSetStyle;
        private ST.Controls.Modal.ModalButton btClear;
        private ST.Controls.Modal.ModalButton btAddColumn;
        private ST.Controls.UserScrollBar userScrollBar1;
    }
}