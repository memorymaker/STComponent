
namespace UnitTest
{
    partial class Form2
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.userScaleControlWarpPanelBase1 = new ST.Controls.UserScaleControlWarpPanel();
            this.listView2 = new ST.Controls.UserListView();
            this.listView = new ST.Controls.UserListView();
            this.userTableNode1 = new Common.UserTableNode();
            this.userTableNode2 = new Common.UserTableNode();
            this.button3 = new System.Windows.Forms.Button();
            this.userScaleControlWarpPanelBase1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 399);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 38);
            this.button1.TabIndex = 2;
            this.button1.Text = "+";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(90, 399);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(72, 38);
            this.button2.TabIndex = 2;
            this.button2.Text = "-";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // userScaleControlWarpPanelBase1
            // 
            this.userScaleControlWarpPanelBase1.BackColor = System.Drawing.Color.White;
            this.userScaleControlWarpPanelBase1.Controls.Add(this.userTableNode2);
            this.userScaleControlWarpPanelBase1.Controls.Add(this.userTableNode1);
            this.userScaleControlWarpPanelBase1.Controls.Add(this.listView);
            this.userScaleControlWarpPanelBase1.Controls.Add(this.listView2);
            this.userScaleControlWarpPanelBase1.Location = new System.Drawing.Point(22, 12);
            this.userScaleControlWarpPanelBase1.Name = "userScaleControlWarpPanelBase1";
            this.userScaleControlWarpPanelBase1.ScaleValue = 1F;
            this.userScaleControlWarpPanelBase1.Size = new System.Drawing.Size(729, 381);
            this.userScaleControlWarpPanelBase1.TabIndex = 3;
            // 
            // listView2
            // 
            this.listView2.AllowDrop = true;
            this.listView2.BackColor = System.Drawing.Color.White;
            this.listView2.ColumnHeight = 26;
            this.listView2.ColumnHorizontalDistance = 1;
            this.listView2.ItemHeight = 20;
            this.listView2.ItemPadding = new System.Windows.Forms.Padding(4);
            this.listView2.ItemVerticalDistance = 1;
            this.listView2.ItemVerticalDistanceColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.listView2.Location = new System.Drawing.Point(531, 25);
            this.listView2.MinimapColor = System.Drawing.Color.Blue;
            this.listView2.Name = "listView2";
            this.listView2.OriginalHeight = 0;
            this.listView2.OriginalLeft = 0;
            this.listView2.OriginalTop = 0;
            this.listView2.OriginalWidth = 0;
            this.listView2.ScaleValue = 1F;
            this.listView2.ScrollLeft = 0;
            this.listView2.ScrollTop = 0;
            this.listView2.SelectedItemIndex = -1;
            this.listView2.Size = new System.Drawing.Size(133, 141);
            this.listView2.TabIndex = 1;
            // 
            // listView
            // 
            this.listView.AllowDrop = true;
            this.listView.BackColor = System.Drawing.Color.White;
            this.listView.ColumnHeight = 26;
            this.listView.ColumnHorizontalDistance = 1;
            this.listView.ItemHeight = 20;
            this.listView.ItemPadding = new System.Windows.Forms.Padding(4);
            this.listView.ItemVerticalDistance = 1;
            this.listView.ItemVerticalDistanceColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.listView.Location = new System.Drawing.Point(287, 25);
            this.listView.MinimapColor = System.Drawing.Color.Blue;
            this.listView.Name = "listView";
            this.listView.OriginalHeight = 0;
            this.listView.OriginalLeft = 0;
            this.listView.OriginalTop = 0;
            this.listView.OriginalWidth = 0;
            this.listView.ScaleValue = 1F;
            this.listView.ScrollLeft = 0;
            this.listView.ScrollTop = 0;
            this.listView.SelectedItemIndex = -1;
            this.listView.Size = new System.Drawing.Size(206, 204);
            this.listView.TabIndex = 0;
            // 
            // userTableNode1
            // 
            this.userTableNode1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(121)))), ((int)(((byte)(47)))));
            this.userTableNode1.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.userTableNode1.ForeColor = System.Drawing.Color.White;
            this.userTableNode1.ID = null;
            this.userTableNode1.Location = new System.Drawing.Point(18, 25);
            this.userTableNode1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userTableNode1.MinimapColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(121)))), ((int)(((byte)(47)))));
            this.userTableNode1.Name = "userTableNode1";
            this.userTableNode1.OriginalHeight = 0;
            this.userTableNode1.OriginalLeft = 0;
            this.userTableNode1.OriginalTop = 0;
            this.userTableNode1.OriginalWidth = 0;
            this.userTableNode1.Padding = new System.Windows.Forms.Padding(3);
            this.userTableNode1.ScaleValue = 1F;
            this.userTableNode1.SEQ = 0;
            this.userTableNode1.Size = new System.Drawing.Size(180, 169);
            this.userTableNode1.TabIndex = 2;
            this.userTableNode1.TABLE_ID = null;
            this.userTableNode1.Title = "TT2";
            this.userTableNode1.TitleBold = true;
            this.userTableNode1.TitleHeight = 25;
            this.userTableNode1.TitleVisible = false;
            // 
            // userTableNode2
            // 
            this.userTableNode2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(121)))), ((int)(((byte)(47)))));
            this.userTableNode2.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.userTableNode2.ForeColor = System.Drawing.Color.White;
            this.userTableNode2.ID = null;
            this.userTableNode2.Location = new System.Drawing.Point(250, 162);
            this.userTableNode2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userTableNode2.MinimapColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(121)))), ((int)(((byte)(47)))));
            this.userTableNode2.Name = "userTableNode2";
            this.userTableNode2.OriginalHeight = 0;
            this.userTableNode2.OriginalLeft = 0;
            this.userTableNode2.OriginalTop = 0;
            this.userTableNode2.OriginalWidth = 0;
            this.userTableNode2.Padding = new System.Windows.Forms.Padding(3);
            this.userTableNode2.ScaleValue = 1F;
            this.userTableNode2.SEQ = 0;
            this.userTableNode2.Size = new System.Drawing.Size(207, 173);
            this.userTableNode2.TabIndex = 3;
            this.userTableNode2.TABLE_ID = null;
            this.userTableNode2.Title = "TT1";
            this.userTableNode2.TitleBold = true;
            this.userTableNode2.TitleHeight = 25;
            this.userTableNode2.TitleVisible = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(168, 399);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(72, 38);
            this.button3.TabIndex = 2;
            this.button3.Text = "Test";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 448);
            this.Controls.Add(this.userScaleControlWarpPanelBase1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.userScaleControlWarpPanelBase1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ST.Controls.UserListView listView2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private ST.Controls.UserScaleControlWarpPanel userScaleControlWarpPanelBase1;
        private ST.Controls.UserListView listView;
        private Common.UserTableNode userTableNode2;
        private Common.UserTableNode userTableNode1;
        private System.Windows.Forms.Button button3;
    }
}