namespace Sample
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.userEditor = new ST.Controls.UserEditor();
            this.btGetMainData = new ST.Controls.Modal.ModalButton();
            this.btGetRelationData = new ST.Controls.Modal.ModalButton();
            this.btGetNodeData = new ST.Controls.Modal.ModalButton();
            this.btClearStyle = new ST.Controls.Modal.ModalButton();
            this.btToggleInfo = new ST.Controls.Modal.ModalButton();
            this.btToggleEnabled = new ST.Controls.Modal.ModalButton();
            this.btSetSampleText = new ST.Controls.Modal.ModalButton();
            this.btClear = new ST.Controls.Modal.ModalButton();
            this.btSetStyle = new ST.Controls.Modal.ModalButton();
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
            this.splitContainer1.Panel1.Controls.Add(this.userEditor);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.btGetMainData);
            this.splitContainer1.Panel2.Controls.Add(this.btGetRelationData);
            this.splitContainer1.Panel2.Controls.Add(this.btGetNodeData);
            this.splitContainer1.Panel2.Controls.Add(this.btClearStyle);
            this.splitContainer1.Panel2.Controls.Add(this.btToggleInfo);
            this.splitContainer1.Panel2.Controls.Add(this.btToggleEnabled);
            this.splitContainer1.Panel2.Controls.Add(this.btSetSampleText);
            this.splitContainer1.Panel2.Controls.Add(this.btClear);
            this.splitContainer1.Panel2.Controls.Add(this.btSetStyle);
            this.splitContainer1.Size = new System.Drawing.Size(1060, 575);
            this.splitContainer1.SplitterDistance = 495;
            this.splitContainer1.TabIndex = 2;
            // 
            // userEditor
            // 
            this.userEditor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.userEditor.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.userEditor.DelayedDataChangedPeriod = 100;
            this.userEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userEditor.Font = new System.Drawing.Font("돋움체", 10F);
            this.userEditor.Location = new System.Drawing.Point(0, 0);
            this.userEditor.Name = "userEditor";
            this.userEditor.ReadOnly = false;
            this.userEditor.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.userEditor.SelectionLength = 0;
            this.userEditor.SelectionStart = 0;
            this.userEditor.ShowSelectoinInfo = true;
            this.userEditor.Size = new System.Drawing.Size(1060, 495);
            this.userEditor.TabIndex = 0;
            this.userEditor.TabSpaceCount = 4;
            this.userEditor.TabStop = false;
            this.userEditor.WordWrap = false;
            // 
            // btGetMainData
            // 
            this.btGetMainData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btGetMainData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btGetMainData.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btGetMainData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btGetMainData.Location = new System.Drawing.Point(716, 8);
            this.btGetMainData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btGetMainData.Name = "btGetMainData";
            this.btGetMainData.Size = new System.Drawing.Size(108, 26);
            this.btGetMainData.TabIndex = 11;
            this.btGetMainData.Text = "...";
            this.btGetMainData.UseVisualStyleBackColor = false;
            // 
            // btGetRelationData
            // 
            this.btGetRelationData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btGetRelationData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btGetRelationData.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btGetRelationData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btGetRelationData.Location = new System.Drawing.Point(944, 8);
            this.btGetRelationData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btGetRelationData.Name = "btGetRelationData";
            this.btGetRelationData.Size = new System.Drawing.Size(108, 26);
            this.btGetRelationData.TabIndex = 12;
            this.btGetRelationData.Text = "...";
            this.btGetRelationData.UseVisualStyleBackColor = false;
            // 
            // btGetNodeData
            // 
            this.btGetNodeData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btGetNodeData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btGetNodeData.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btGetNodeData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btGetNodeData.Location = new System.Drawing.Point(830, 8);
            this.btGetNodeData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btGetNodeData.Name = "btGetNodeData";
            this.btGetNodeData.Size = new System.Drawing.Size(108, 26);
            this.btGetNodeData.TabIndex = 10;
            this.btGetNodeData.Text = "...";
            this.btGetNodeData.UseVisualStyleBackColor = false;
            // 
            // btClearStyle
            // 
            this.btClearStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btClearStyle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btClearStyle.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btClearStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btClearStyle.Location = new System.Drawing.Point(350, 8);
            this.btClearStyle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btClearStyle.Name = "btClearStyle";
            this.btClearStyle.Size = new System.Drawing.Size(108, 26);
            this.btClearStyle.TabIndex = 4;
            this.btClearStyle.Text = "Clear Style";
            this.btClearStyle.UseVisualStyleBackColor = false;
            this.btClearStyle.Click += new System.EventHandler(this.btClearStyle_Click);
            // 
            // btToggleInfo
            // 
            this.btToggleInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btToggleInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btToggleInfo.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btToggleInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btToggleInfo.Location = new System.Drawing.Point(577, 8);
            this.btToggleInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btToggleInfo.Name = "btToggleInfo";
            this.btToggleInfo.Size = new System.Drawing.Size(132, 26);
            this.btToggleInfo.TabIndex = 5;
            this.btToggleInfo.Text = "Toggle Selection Info";
            this.btToggleInfo.UseVisualStyleBackColor = false;
            this.btToggleInfo.Click += new System.EventHandler(this.btToggleInfo_Click);
            // 
            // btToggleEnabled
            // 
            this.btToggleEnabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btToggleEnabled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btToggleEnabled.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btToggleEnabled.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btToggleEnabled.Location = new System.Drawing.Point(463, 8);
            this.btToggleEnabled.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btToggleEnabled.Name = "btToggleEnabled";
            this.btToggleEnabled.Size = new System.Drawing.Size(108, 26);
            this.btToggleEnabled.TabIndex = 6;
            this.btToggleEnabled.Text = "Toggle Enabled";
            this.btToggleEnabled.UseVisualStyleBackColor = false;
            this.btToggleEnabled.Click += new System.EventHandler(this.btToggleEnabled_Click);
            // 
            // btSetSampleText
            // 
            this.btSetSampleText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btSetSampleText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSetSampleText.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btSetSampleText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btSetSampleText.Location = new System.Drawing.Point(122, 8);
            this.btSetSampleText.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btSetSampleText.Name = "btSetSampleText";
            this.btSetSampleText.Size = new System.Drawing.Size(108, 26);
            this.btSetSampleText.TabIndex = 7;
            this.btSetSampleText.Text = "Set Sample Text";
            this.btSetSampleText.UseVisualStyleBackColor = false;
            this.btSetSampleText.Click += new System.EventHandler(this.btSetSampleText_Click);
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
            // btSetStyle
            // 
            this.btSetStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btSetStyle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSetStyle.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btSetStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btSetStyle.Location = new System.Drawing.Point(236, 8);
            this.btSetStyle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btSetStyle.Name = "btSetStyle";
            this.btSetStyle.Size = new System.Drawing.Size(108, 26);
            this.btSetStyle.TabIndex = 9;
            this.btSetStyle.Text = "Set Style";
            this.btSetStyle.UseVisualStyleBackColor = false;
            this.btSetStyle.Click += new System.EventHandler(this.btSetStyle_Click);
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 575);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form4";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form4_UserEditor";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ST.Controls.Modal.ModalButton btGetMainData;
        private ST.Controls.Modal.ModalButton btGetRelationData;
        private ST.Controls.Modal.ModalButton btGetNodeData;
        private ST.Controls.Modal.ModalButton btClearStyle;
        private ST.Controls.Modal.ModalButton btToggleInfo;
        private ST.Controls.Modal.ModalButton btToggleEnabled;
        private ST.Controls.Modal.ModalButton btSetSampleText;
        private ST.Controls.Modal.ModalButton btClear;
        private ST.Controls.Modal.ModalButton btSetStyle;
        private ST.Controls.UserEditor userEditor;
    }
}