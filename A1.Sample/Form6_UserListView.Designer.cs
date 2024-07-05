namespace Sample
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.userListView1 = new ST.Controls.UserListView();
            this.btScaleMinus = new ST.Controls.Modal.ModalButton();
            this.btScalePlus = new ST.Controls.Modal.ModalButton();
            this.btBindData = new ST.Controls.Modal.ModalButton();
            this.modalButton1 = new ST.Controls.Modal.ModalButton();
            this.btSetStyle = new ST.Controls.Modal.ModalButton();
            this.btClear = new ST.Controls.Modal.ModalButton();
            this.btAddColumn = new ST.Controls.Modal.ModalButton();
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
            this.splitContainer1.Panel1.Controls.Add(this.userListView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.btScaleMinus);
            this.splitContainer1.Panel2.Controls.Add(this.btScalePlus);
            this.splitContainer1.Panel2.Controls.Add(this.btBindData);
            this.splitContainer1.Panel2.Controls.Add(this.modalButton1);
            this.splitContainer1.Panel2.Controls.Add(this.btSetStyle);
            this.splitContainer1.Panel2.Controls.Add(this.btClear);
            this.splitContainer1.Panel2.Controls.Add(this.btAddColumn);
            this.splitContainer1.Size = new System.Drawing.Size(1035, 575);
            this.splitContainer1.SplitterDistance = 495;
            this.splitContainer1.TabIndex = 4;
            // 
            // userListView1
            // 
            this.userListView1.AllowDrop = true;
            this.userListView1.AutoSizeType = ST.Controls.UserListAutoSizeType.None;
            this.userListView1.BackColor = System.Drawing.Color.White;
            this.userListView1.ColumnHeight = 26;
            this.userListView1.ColumnHorizontalDistance = 0;
            this.userListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.userListView1.ItemHeight = 20;
            this.userListView1.ItemPadding = new System.Windows.Forms.Padding(4);
            this.userListView1.ItemVerticalDistance = 1;
            this.userListView1.ItemVerticalDistanceColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.userListView1.Location = new System.Drawing.Point(12, 12);
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
            this.userListView1.Size = new System.Drawing.Size(508, 301);
            this.userListView1.TabIndex = 0;
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
            this.btScaleMinus.Click += new System.EventHandler(this.btScaleMinus_Click);
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
            this.btScalePlus.Click += new System.EventHandler(this.btScalePlus_Click);
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
            this.btBindData.Click += new System.EventHandler(this.btBindData_Click);
            // 
            // modalButton1
            // 
            this.modalButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.modalButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.modalButton1.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.modalButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.modalButton1.Location = new System.Drawing.Point(692, 8);
            this.modalButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.modalButton1.Name = "modalButton1";
            this.modalButton1.Size = new System.Drawing.Size(108, 26);
            this.modalButton1.TabIndex = 7;
            this.modalButton1.Text = "...";
            this.modalButton1.UseVisualStyleBackColor = false;
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
            this.btSetStyle.Click += new System.EventHandler(this.btSetStyle_Click);
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
            this.btClear.Click += new System.EventHandler(this.btClear_Click);
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
            this.btAddColumn.Click += new System.EventHandler(this.btAddColumn_Click);
            // 
            // Form6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1035, 575);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form6";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form6_UserListView";
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
        private ST.Controls.UserListView userListView1;
        private ST.Controls.Modal.ModalButton modalButton1;
    }
}