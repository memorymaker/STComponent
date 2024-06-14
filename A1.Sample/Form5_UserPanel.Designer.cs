namespace Sample
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.userPanel3 = new ST.Controls.UserPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.userPanel1 = new ST.Controls.UserPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.userPanel2 = new ST.Controls.UserPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.btScaleMinus = new ST.Controls.Modal.ModalButton();
            this.btScalePlus = new ST.Controls.Modal.ModalButton();
            this.btToggleEnabled = new ST.Controls.Modal.ModalButton();
            this.btAddRelation = new ST.Controls.Modal.ModalButton();
            this.btClear = new ST.Controls.Modal.ModalButton();
            this.btAddPanel = new ST.Controls.Modal.ModalButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.userPanel3.SuspendLayout();
            this.userPanel1.SuspendLayout();
            this.userPanel2.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.userPanel3);
            this.splitContainer1.Panel1.Controls.Add(this.userPanel1);
            this.splitContainer1.Panel1.Controls.Add(this.userPanel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.btScaleMinus);
            this.splitContainer1.Panel2.Controls.Add(this.btScalePlus);
            this.splitContainer1.Panel2.Controls.Add(this.btToggleEnabled);
            this.splitContainer1.Panel2.Controls.Add(this.btAddRelation);
            this.splitContainer1.Panel2.Controls.Add(this.btClear);
            this.splitContainer1.Panel2.Controls.Add(this.btAddPanel);
            this.splitContainer1.Size = new System.Drawing.Size(1077, 575);
            this.splitContainer1.SplitterDistance = 495;
            this.splitContainer1.TabIndex = 3;
            // 
            // userPanel3
            // 
            this.userPanel3.AwaysOnTop = false;
            this.userPanel3.BackColor = System.Drawing.Color.White;
            this.userPanel3.Controls.Add(this.label1);
            this.userPanel3.Controls.Add(this.button3);
            this.userPanel3.Location = new System.Drawing.Point(704, 12);
            this.userPanel3.Name = "userPanel3";
            this.userPanel3.Padding = new System.Windows.Forms.Padding(2, 23, 2, 2);
            this.userPanel3.Size = new System.Drawing.Size(340, 200);
            this.userPanel3.TabIndex = 12;
            this.userPanel3.TabStop = true;
            this.userPanel3.Title = "Title3";
            this.userPanel3.TitleStartIndex = 0;
            this.userPanel3.UsingAwaysOnTopMenuButton = true;
            this.userPanel3.UsingMaximize = true;
            this.userPanel3.UsingPanelMerge = false;
            this.userPanel3.UsingTitleRename = false;
            this.userPanel3.UsingTitleSlider = false;
            this.userPanel3.UsingViewContextMenuButton = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "UsingPanelMerge = false";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(5, 157);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 13;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // userPanel1
            // 
            this.userPanel1.AwaysOnTop = false;
            this.userPanel1.BackColor = System.Drawing.Color.White;
            this.userPanel1.Controls.Add(this.label3);
            this.userPanel1.Controls.Add(this.button1);
            this.userPanel1.Location = new System.Drawing.Point(12, 12);
            this.userPanel1.Name = "userPanel1";
            this.userPanel1.Padding = new System.Windows.Forms.Padding(2, 23, 2, 2);
            this.userPanel1.Size = new System.Drawing.Size(340, 200);
            this.userPanel1.TabIndex = 11;
            this.userPanel1.TabStop = true;
            this.userPanel1.Title = "Title1";
            this.userPanel1.TitleStartIndex = 0;
            this.userPanel1.UsingAwaysOnTopMenuButton = true;
            this.userPanel1.UsingMaximize = true;
            this.userPanel1.UsingPanelMerge = false;
            this.userPanel1.UsingTitleRename = false;
            this.userPanel1.UsingTitleSlider = false;
            this.userPanel1.UsingViewContextMenuButton = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(86, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 12);
            this.label3.TabIndex = 16;
            this.label3.Text = "UsingPanelMerge = true";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(5, 71);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // userPanel2
            // 
            this.userPanel2.AwaysOnTop = false;
            this.userPanel2.BackColor = System.Drawing.Color.White;
            this.userPanel2.Controls.Add(this.label2);
            this.userPanel2.Controls.Add(this.button2);
            this.userPanel2.Location = new System.Drawing.Point(358, 12);
            this.userPanel2.Name = "userPanel2";
            this.userPanel2.Padding = new System.Windows.Forms.Padding(2, 23, 2, 2);
            this.userPanel2.Size = new System.Drawing.Size(340, 200);
            this.userPanel2.TabIndex = 1;
            this.userPanel2.TabStop = true;
            this.userPanel2.Title = "Title2";
            this.userPanel2.TitleStartIndex = 0;
            this.userPanel2.UsingAwaysOnTopMenuButton = true;
            this.userPanel2.UsingMaximize = true;
            this.userPanel2.UsingPanelMerge = false;
            this.userPanel2.UsingTitleRename = false;
            this.userPanel2.UsingTitleSlider = false;
            this.userPanel2.UsingViewContextMenuButton = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "UsingPanelMerge = true";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(5, 116);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btScaleMinus
            // 
            this.btScaleMinus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btScaleMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btScaleMinus.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btScaleMinus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btScaleMinus.Location = new System.Drawing.Point(612, 8);
            this.btScaleMinus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btScaleMinus.Name = "btScaleMinus";
            this.btScaleMinus.Size = new System.Drawing.Size(108, 26);
            this.btScaleMinus.TabIndex = 4;
            this.btScaleMinus.Text = "...";
            this.btScaleMinus.UseVisualStyleBackColor = false;
            // 
            // btScalePlus
            // 
            this.btScalePlus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btScalePlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btScalePlus.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btScalePlus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btScalePlus.Location = new System.Drawing.Point(498, 8);
            this.btScalePlus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btScalePlus.Name = "btScalePlus";
            this.btScalePlus.Size = new System.Drawing.Size(108, 26);
            this.btScalePlus.TabIndex = 5;
            this.btScalePlus.Text = "...";
            this.btScalePlus.UseVisualStyleBackColor = false;
            // 
            // btToggleEnabled
            // 
            this.btToggleEnabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btToggleEnabled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btToggleEnabled.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btToggleEnabled.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btToggleEnabled.Location = new System.Drawing.Point(236, 8);
            this.btToggleEnabled.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btToggleEnabled.Name = "btToggleEnabled";
            this.btToggleEnabled.Size = new System.Drawing.Size(142, 26);
            this.btToggleEnabled.TabIndex = 6;
            this.btToggleEnabled.Text = "Toggle Enabled(Title1)";
            this.btToggleEnabled.UseVisualStyleBackColor = false;
            this.btToggleEnabled.Click += new System.EventHandler(this.btToggleEnabled_Click);
            // 
            // btAddRelation
            // 
            this.btAddRelation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btAddRelation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btAddRelation.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btAddRelation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btAddRelation.Location = new System.Drawing.Point(384, 8);
            this.btAddRelation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btAddRelation.Name = "btAddRelation";
            this.btAddRelation.Size = new System.Drawing.Size(108, 26);
            this.btAddRelation.TabIndex = 7;
            this.btAddRelation.Text = "...";
            this.btAddRelation.UseVisualStyleBackColor = false;
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
            // btAddPanel
            // 
            this.btAddPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(122)))), ((int)(((byte)(182)))));
            this.btAddPanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btAddPanel.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btAddPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btAddPanel.Location = new System.Drawing.Point(122, 8);
            this.btAddPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btAddPanel.Name = "btAddPanel";
            this.btAddPanel.Size = new System.Drawing.Size(108, 26);
            this.btAddPanel.TabIndex = 9;
            this.btAddPanel.Text = "Add Panel";
            this.btAddPanel.UseVisualStyleBackColor = false;
            this.btAddPanel.Click += new System.EventHandler(this.btAddPanel_Click);
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1077, 575);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form5";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form5";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.userPanel3.ResumeLayout(false);
            this.userPanel3.PerformLayout();
            this.userPanel1.ResumeLayout(false);
            this.userPanel1.PerformLayout();
            this.userPanel2.ResumeLayout(false);
            this.userPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ST.Controls.Modal.ModalButton btScaleMinus;
        private ST.Controls.Modal.ModalButton btScalePlus;
        private ST.Controls.Modal.ModalButton btToggleEnabled;
        private ST.Controls.Modal.ModalButton btAddRelation;
        private ST.Controls.Modal.ModalButton btClear;
        private ST.Controls.Modal.ModalButton btAddPanel;
        private ST.Controls.UserPanel userPanel2;
        private ST.Controls.UserPanel userPanel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private ST.Controls.UserPanel userPanel3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}