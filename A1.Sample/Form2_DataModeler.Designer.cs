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
            this.dataModeler1 = new ST.DataModeler.DataModeler();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btScalePlus = new ST.Controls.Modal.ModalButton();
            this.btToggleEnabled = new ST.Controls.Modal.ModalButton();
            this.btAddRelation = new ST.Controls.Modal.ModalButton();
            this.btClear = new ST.Controls.Modal.ModalButton();
            this.btAddTableNode = new ST.Controls.Modal.ModalButton();
            this.btScaleMinus = new ST.Controls.Modal.ModalButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.dataModeler1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(1077, 575);
            this.splitContainer1.SplitterDistance = 495;
            this.splitContainer1.TabIndex = 0;
            // 
            // dataModeler1
            // 
            this.dataModeler1.AllowDrop = true;
            this.dataModeler1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.dataModeler1.DisableBackColor = System.Drawing.Color.Gray;
            this.dataModeler1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataModeler1.ID = null;
            this.dataModeler1.Location = new System.Drawing.Point(0, 0);
            this.dataModeler1.Name = "dataModeler1";
            this.dataModeler1.ReadOnly = false;
            this.dataModeler1.ScaleValue = 1F;
            this.dataModeler1.ShowPerformanceTestLabel = true;
            this.dataModeler1.Size = new System.Drawing.Size(1077, 495);
            this.dataModeler1.StatusPanelBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.dataModeler1.StatusPanelBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.dataModeler1.TabIndex = 0;
            this.dataModeler1.Text = "dataModeler1";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.btScaleMinus);
            this.panel1.Controls.Add(this.btScalePlus);
            this.panel1.Controls.Add(this.btToggleEnabled);
            this.panel1.Controls.Add(this.btAddRelation);
            this.panel1.Controls.Add(this.btClear);
            this.panel1.Controls.Add(this.btAddTableNode);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1077, 76);
            this.panel1.TabIndex = 0;
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
            this.btScalePlus.Text = "Scale + 0.1F";
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
            // btClear
            // 
            this.btClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btClear.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btClear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btClear.Location = new System.Drawing.Point(122, 8);
            this.btClear.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(108, 26);
            this.btClear.TabIndex = 1;
            this.btClear.Text = "Clear";
            this.btClear.UseVisualStyleBackColor = false;
            this.btClear.Click += new System.EventHandler(this.btClear_Click);
            // 
            // btAddTableNode
            // 
            this.btAddTableNode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btAddTableNode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btAddTableNode.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btAddTableNode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btAddTableNode.Location = new System.Drawing.Point(8, 8);
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
            this.btScaleMinus.Text = "Scale - 0.1F";
            this.btScaleMinus.UseVisualStyleBackColor = false;
            this.btScaleMinus.Click += new System.EventHandler(this.btScaleMinus_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1077, 575);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ST.DataModeler.DataModeler dataModeler1;
        private System.Windows.Forms.Panel panel1;
        private ST.Controls.Modal.ModalButton btClear;
        private ST.Controls.Modal.ModalButton btAddTableNode;
        private ST.Controls.Modal.ModalButton btScalePlus;
        private ST.Controls.Modal.ModalButton btToggleEnabled;
        private ST.Controls.Modal.ModalButton btAddRelation;
        private ST.Controls.Modal.ModalButton btScaleMinus;
    }
}