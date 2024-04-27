using ST.Controls;
using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.CodeGenerator
{
    public partial class Tab
    {
        private GraphicControl CopyButton;
        // ExecuteButton, HistoryButton

        //public event EventHandler CopyButton_Click;

        private void LoadGraphicControls()
        {
            CopyButton = new GraphicControl(MainSplit.Panel2, "CopyButton");
            CopyButton
                .SetArea(new Rectangle(3, 3, 128, 23))
                .SetDrawType(GraphicControl.DrawTypeEnum.ImageTextLeftRight)
                .SetDrawFont(new Font("맑은 고딕", 8f))
                .SetDrawInfo(GraphicControl.StateType.Default, new GraphicControl.DrawInfo()
                {
                      DrawBackColor = ResultMenuBackColor
                    , DrawImage = CodeGeneratorResource.Icon_File
                    , DrawText = "Copy To Clipboard"
                    , DrawTextColor = Color.FromArgb(60, 60, 60)
                    , LeftRevision = -1
                })
                .SetDrawInfo(GraphicControl.StateType.Over, new GraphicControl.DrawInfo()
                {
                      DrawBackColor = ResultMenuBackColor.GetColor(0.05f)
                    , DrawBorderColor = Color.FromArgb(93, 107, 153)
                    , LeftRevision = -1
                })
                .SetDrawInfo(GraphicControl.StateType.MouseDown, new GraphicControl.DrawInfo()
                {
                      DrawBackColor = ResultMenuBackColor.GetColor(0.1f)
                    , DrawBorderColor = Color.FromArgb(93, 107, 153)
                    , LeftRevision = -1
                });
            CopyButton.Click += CopyButton_Click;
        }

        private void CopyButton_Click(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(ResultEditor.Text))
            {
                Clipboard.SetText(ResultEditor.Text);
                ModalMessageBox.Show("It's been copied.", "Copy");
            }
            else
            {
                ModalMessageBox.Show("There is nothing to copy.", "Copy");
            }
        }

        private void ExecuteButton_Click(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //ExecuteButton_Click?.Invoke(this, e);
        }

        private void HistoryButton_Click(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //HistoryButton?.Invoke(this, e);
        }
    }
}
