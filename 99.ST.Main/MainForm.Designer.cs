namespace ST.Main
{
    partial class MainForm
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
            this.PanelWarp = new System.Windows.Forms.Panel();
            this.PanelManu = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // PanelWarp
            // 
            this.PanelWarp.Location = new System.Drawing.Point(0, 39);
            this.PanelWarp.Name = "PanelWarp";
            this.PanelWarp.Size = new System.Drawing.Size(584, 322);
            this.PanelWarp.TabIndex = 0;
            // 
            // PanelManu
            // 
            this.PanelManu.Location = new System.Drawing.Point(0, 0);
            this.PanelManu.Name = "PanelManu";
            this.PanelManu.Size = new System.Drawing.Size(40, 40);
            this.PanelManu.TabIndex = 1;
            this.PanelManu.Visible = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.PanelManu);
            this.Controls.Add(this.PanelWarp);
            this.Name = "Main";
            this.Text = "STManager";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelWarp;
        private System.Windows.Forms.Panel PanelManu;
    }
}

