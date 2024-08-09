namespace Sample
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.codeGenerator = new ST.CodeGenerator.CodeGenerator();
            this.btGetMainData = new ST.Controls.Modal.ModalButton();
            this.btGetRelationData = new ST.Controls.Modal.ModalButton();
            this.btGetNodeData = new ST.Controls.Modal.ModalButton();
            this.btAddTab = new ST.Controls.Modal.ModalButton();
            this.btSetSplitter = new ST.Controls.Modal.ModalButton();
            this.btToggleEnabled = new ST.Controls.Modal.ModalButton();
            this.btGetData = new ST.Controls.Modal.ModalButton();
            this.btClear = new ST.Controls.Modal.ModalButton();
            this.btLoadData = new ST.Controls.Modal.ModalButton();
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
            this.splitContainer1.Panel1.Controls.Add(this.codeGenerator);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.btGetMainData);
            this.splitContainer1.Panel2.Controls.Add(this.btGetRelationData);
            this.splitContainer1.Panel2.Controls.Add(this.btGetNodeData);
            this.splitContainer1.Panel2.Controls.Add(this.btAddTab);
            this.splitContainer1.Panel2.Controls.Add(this.btSetSplitter);
            this.splitContainer1.Panel2.Controls.Add(this.btToggleEnabled);
            this.splitContainer1.Panel2.Controls.Add(this.btGetData);
            this.splitContainer1.Panel2.Controls.Add(this.btClear);
            this.splitContainer1.Panel2.Controls.Add(this.btLoadData);
            this.splitContainer1.Size = new System.Drawing.Size(1077, 875);
            this.splitContainer1.SplitterDistance = 795;
            this.splitContainer1.TabIndex = 1;
            // 
            // codeGenerator
            // 
            this.codeGenerator.BackColor = System.Drawing.Color.White;
            this.codeGenerator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeGenerator.Location = new System.Drawing.Point(0, 0);
            this.codeGenerator.MainSplitterDistance = 74;
            this.codeGenerator.Name = "codeGenerator";
            this.codeGenerator.NodeData = ((System.Collections.Generic.Dictionary<string, System.Data.DataTable>)(resources.GetObject("codeGenerator.NodeData")));
            this.codeGenerator.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.codeGenerator.ReadOnly = false;
            this.codeGenerator.RelationData = null;
            this.codeGenerator.Size = new System.Drawing.Size(1077, 795);
            this.codeGenerator.TabIndex = 0;
            // 
            // btGetMainData
            // 
            this.btGetMainData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btGetMainData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btGetMainData.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btGetMainData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btGetMainData.Location = new System.Drawing.Point(692, 8);
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
            this.btGetRelationData.Location = new System.Drawing.Point(920, 8);
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
            this.btGetNodeData.Location = new System.Drawing.Point(806, 8);
            this.btGetNodeData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btGetNodeData.Name = "btGetNodeData";
            this.btGetNodeData.Size = new System.Drawing.Size(108, 26);
            this.btGetNodeData.TabIndex = 10;
            this.btGetNodeData.Text = "...";
            this.btGetNodeData.UseVisualStyleBackColor = false;
            // 
            // btAddTab
            // 
            this.btAddTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btAddTab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btAddTab.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btAddTab.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btAddTab.Location = new System.Drawing.Point(578, 8);
            this.btAddTab.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btAddTab.Name = "btAddTab";
            this.btAddTab.Size = new System.Drawing.Size(108, 26);
            this.btAddTab.TabIndex = 4;
            this.btAddTab.Text = "Add Tab";
            this.btAddTab.UseVisualStyleBackColor = false;
            this.btAddTab.Click += new System.EventHandler(this.btAddTab_Click);
            // 
            // btSetSplitter
            // 
            this.btSetSplitter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btSetSplitter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSetSplitter.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btSetSplitter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btSetSplitter.Location = new System.Drawing.Point(464, 8);
            this.btSetSplitter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btSetSplitter.Name = "btSetSplitter";
            this.btSetSplitter.Size = new System.Drawing.Size(108, 26);
            this.btSetSplitter.TabIndex = 5;
            this.btSetSplitter.Text = "Set Splitter";
            this.btSetSplitter.UseVisualStyleBackColor = false;
            this.btSetSplitter.Click += new System.EventHandler(this.btSetSplitter_Click);
            // 
            // btToggleEnabled
            // 
            this.btToggleEnabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btToggleEnabled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btToggleEnabled.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btToggleEnabled.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btToggleEnabled.Location = new System.Drawing.Point(350, 8);
            this.btToggleEnabled.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btToggleEnabled.Name = "btToggleEnabled";
            this.btToggleEnabled.Size = new System.Drawing.Size(108, 26);
            this.btToggleEnabled.TabIndex = 6;
            this.btToggleEnabled.Text = "Toggle Enabled";
            this.btToggleEnabled.UseVisualStyleBackColor = false;
            this.btToggleEnabled.Click += new System.EventHandler(this.btToggleEnabled_Click);
            // 
            // btGetData
            // 
            this.btGetData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btGetData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btGetData.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btGetData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btGetData.Location = new System.Drawing.Point(236, 8);
            this.btGetData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btGetData.Name = "btGetData";
            this.btGetData.Size = new System.Drawing.Size(108, 26);
            this.btGetData.TabIndex = 7;
            this.btGetData.Text = "Get Data";
            this.btGetData.UseVisualStyleBackColor = false;
            this.btGetData.Click += new System.EventHandler(this.btGetData_Click);
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
            // btLoadData
            // 
            this.btLoadData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btLoadData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btLoadData.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btLoadData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btLoadData.Location = new System.Drawing.Point(122, 8);
            this.btLoadData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btLoadData.Name = "btLoadData";
            this.btLoadData.Size = new System.Drawing.Size(108, 26);
            this.btLoadData.TabIndex = 9;
            this.btLoadData.Text = "Load Data";
            this.btLoadData.UseVisualStyleBackColor = false;
            this.btLoadData.Click += new System.EventHandler(this.btLoadData_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1077, 875);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form3_CodeGenerator";
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
        private ST.Controls.Modal.ModalButton btAddTab;
        private ST.Controls.Modal.ModalButton btSetSplitter;
        private ST.Controls.Modal.ModalButton btToggleEnabled;
        private ST.Controls.Modal.ModalButton btGetData;
        private ST.Controls.Modal.ModalButton btClear;
        private ST.Controls.Modal.ModalButton btLoadData;
        private ST.CodeGenerator.CodeGenerator codeGenerator;
    }
}