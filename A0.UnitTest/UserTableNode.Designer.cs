namespace Common
{
    partial class UserTableNode
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.lst = new ST.Controls.UserListView();
            this.SuspendLayout();
            // 
            // lst
            // 
            this.lst.AllowDrop = true;
            this.lst.BackColor = System.Drawing.Color.White;
            this.lst.ColumnHeight = 26;
            this.lst.ColumnHorizontalDistance = 0;
            this.lst.ItemHeight = 20;
            this.lst.ItemPadding = new System.Windows.Forms.Padding(4);
            this.lst.ItemVerticalDistance = 1;
            this.lst.ItemVerticalDistanceColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.lst.Location = new System.Drawing.Point(0, 46);
            this.lst.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lst.MinimapColor = System.Drawing.Color.Blue;
            this.lst.Name = "lst";
            this.lst.OriginalHeight = 0;
            this.lst.OriginalLeft = 0;
            this.lst.OriginalTop = 0;
            this.lst.OriginalWidth = 0;
            this.lst.ScaleValue = 1F;
            this.lst.ScrollLeft = 0;
            this.lst.ScrollTop = 0;
            this.lst.SelectedItemIndex = 0;
            this.lst.Size = new System.Drawing.Size(136, 123);
            this.lst.TabIndex = 1;
            // 
            // UserTableNode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lst);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Name = "UserTableNode";
            this.Size = new System.Drawing.Size(136, 169);
            this.Controls.SetChildIndex(this.TitleEditor, 0);
            this.Controls.SetChildIndex(this.lst, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ST.Controls.UserListView lst;
    }
}
