namespace ST.Controls.Modal.MessageBox
{
    partial class ModalMessageBoxForm
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
            this.PanelBottom = new System.Windows.Forms.Panel();
            this.LabelText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PanelBottom
            // 
            this.PanelBottom.BackColor = System.Drawing.SystemColors.Control;
            this.PanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelBottom.Location = new System.Drawing.Point(0, 95);
            this.PanelBottom.Name = "PanelBottom";
            this.PanelBottom.Size = new System.Drawing.Size(256, 42);
            this.PanelBottom.TabIndex = 0;
            // 
            // LabelText
            // 
            this.LabelText.AutoSize = true;
            this.LabelText.Location = new System.Drawing.Point(25, 51);
            this.LabelText.Name = "LabelText";
            this.LabelText.Size = new System.Drawing.Size(29, 15);
            this.LabelText.TabIndex = 0;
            this.LabelText.Text = "Text";
            // 
            // ModalMessageBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 137);
            this.Controls.Add(this.LabelText);
            this.Controls.Add(this.PanelBottom);
            this.Name = "ModalMessageBoxForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel PanelBottom;
        private System.Windows.Forms.Label LabelText;
    }
}
