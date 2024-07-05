namespace Sample
{
    partial class Form2
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
            this.dataModeler = new ST.DataModeler.DataModeler();
            this.btGetMainData = new ST.Controls.Modal.ModalButton();
            this.btGetRelationData = new ST.Controls.Modal.ModalButton();
            this.btClear = new ST.Controls.Modal.ModalButton();
            this.btGetNodeData = new ST.Controls.Modal.ModalButton();
            this.btAddTableNode = new ST.Controls.Modal.ModalButton();
            this.btScaleMinus = new ST.Controls.Modal.ModalButton();
            this.btAddRelation = new ST.Controls.Modal.ModalButton();
            this.btScalePlus = new ST.Controls.Modal.ModalButton();
            this.btToggleEnabled = new ST.Controls.Modal.ModalButton();
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
            this.splitContainer1.Panel1.Controls.Add(this.dataModeler);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.btGetMainData);
            this.splitContainer1.Panel2.Controls.Add(this.btGetRelationData);
            this.splitContainer1.Panel2.Controls.Add(this.btClear);
            this.splitContainer1.Panel2.Controls.Add(this.btGetNodeData);
            this.splitContainer1.Panel2.Controls.Add(this.btAddTableNode);
            this.splitContainer1.Panel2.Controls.Add(this.btScaleMinus);
            this.splitContainer1.Panel2.Controls.Add(this.btAddRelation);
            this.splitContainer1.Panel2.Controls.Add(this.btScalePlus);
            this.splitContainer1.Panel2.Controls.Add(this.btToggleEnabled);
            this.splitContainer1.Size = new System.Drawing.Size(1035, 575);
            this.splitContainer1.SplitterDistance = 495;
            this.splitContainer1.TabIndex = 0;
            // 
            // dataModeler
            // 
            this.dataModeler.AllowDrop = true;
            this.dataModeler.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.dataModeler.DisableBackColor = System.Drawing.Color.Gray;
            this.dataModeler.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataModeler.ID = null;
            this.dataModeler.Location = new System.Drawing.Point(0, 0);
            this.dataModeler.Name = "dataModeler";
            this.dataModeler.ReadOnly = false;
            this.dataModeler.ScaleValue = 1F;
            this.dataModeler.ShowPerformanceTestLabel = true;
            this.dataModeler.Size = new System.Drawing.Size(1035, 495);
            this.dataModeler.StatusPanelBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.dataModeler.StatusPanelBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.dataModeler.TabIndex = 0;
            this.dataModeler.Text = "dataModeler1";
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
            this.btGetMainData.TabIndex = 3;
            this.btGetMainData.Text = "Get MainData";
            this.btGetMainData.UseVisualStyleBackColor = false;
            this.btGetMainData.Click += new System.EventHandler(this.btGetMainData_Click);
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
            this.btGetRelationData.TabIndex = 3;
            this.btGetRelationData.Text = "Get RelationData";
            this.btGetRelationData.UseVisualStyleBackColor = false;
            this.btGetRelationData.Click += new System.EventHandler(this.btGetRelationData_Click);
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
            this.btClear.TabIndex = 1;
            this.btClear.Text = "Clear";
            this.btClear.UseVisualStyleBackColor = false;
            this.btClear.Click += new System.EventHandler(this.btClear_Click);
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
            this.btGetNodeData.TabIndex = 2;
            this.btGetNodeData.Text = "Get NodeData";
            this.btGetNodeData.UseVisualStyleBackColor = false;
            this.btGetNodeData.Click += new System.EventHandler(this.btGetNodeData_Click);
            // 
            // btAddTableNode
            // 
            this.btAddTableNode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btAddTableNode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btAddTableNode.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btAddTableNode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btAddTableNode.Location = new System.Drawing.Point(122, 8);
            this.btAddTableNode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btAddTableNode.Name = "btAddTableNode";
            this.btAddTableNode.Size = new System.Drawing.Size(108, 26);
            this.btAddTableNode.TabIndex = 1;
            this.btAddTableNode.Text = "Add TableNode";
            this.btAddTableNode.UseVisualStyleBackColor = false;
            this.btAddTableNode.Click += new System.EventHandler(this.btAddTableNode_Click);
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
            this.btScaleMinus.TabIndex = 1;
            this.btScaleMinus.Text = "Scale - 10%";
            this.btScaleMinus.UseVisualStyleBackColor = false;
            this.btScaleMinus.Click += new System.EventHandler(this.btScaleMinus_Click);
            // 
            // btAddRelation
            // 
            this.btAddRelation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btAddRelation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btAddRelation.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btAddRelation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btAddRelation.Location = new System.Drawing.Point(236, 8);
            this.btAddRelation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btAddRelation.Name = "btAddRelation";
            this.btAddRelation.Size = new System.Drawing.Size(108, 26);
            this.btAddRelation.TabIndex = 1;
            this.btAddRelation.Text = "Add Relation";
            this.btAddRelation.UseVisualStyleBackColor = false;
            this.btAddRelation.Click += new System.EventHandler(this.btAddRelation_Click);
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
            this.btScalePlus.TabIndex = 1;
            this.btScalePlus.Text = "Scale + 10%";
            this.btScalePlus.UseVisualStyleBackColor = false;
            this.btScalePlus.Click += new System.EventHandler(this.btScalePlus_Click);
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
            this.btToggleEnabled.TabIndex = 1;
            this.btToggleEnabled.Text = "Toggle Enabled";
            this.btToggleEnabled.UseVisualStyleBackColor = false;
            this.btToggleEnabled.Click += new System.EventHandler(this.btToggleEnabled_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1035, 575);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form2_DataModeler";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ST.DataModeler.DataModeler dataModeler;
        private ST.Controls.Modal.ModalButton btClear;
        private ST.Controls.Modal.ModalButton btAddTableNode;
        private ST.Controls.Modal.ModalButton btScalePlus;
        private ST.Controls.Modal.ModalButton btToggleEnabled;
        private ST.Controls.Modal.ModalButton btAddRelation;
        private ST.Controls.Modal.ModalButton btScaleMinus;
        private ST.Controls.Modal.ModalButton btGetNodeData;
        private ST.Controls.Modal.ModalButton btGetMainData;
        private ST.Controls.Modal.ModalButton btGetRelationData;
    }
}