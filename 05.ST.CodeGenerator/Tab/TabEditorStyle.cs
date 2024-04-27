using ST.Controls;
using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.CodeGenerator
{
    public partial class Tab
    {
        private List<EditorStyle> EditorStyleList = new List<EditorStyle>();

        private void LoadStyle()
        {
            //HistoryButton = new GraphicControl(MainSplit.Panel2, "HistoryButton");
            //HistoryButton
            //    .SetArea(new Rectangle(3 + 73, 3, 73, 23))
            //    .SetDrawType(GraphicControl.DrawTypeEnum.ImageTextLeftRight)
            //    .SetDrawFont(new Font("맑은 고딕", 8f))
            //    .SetDrawInfo(GraphicControl.StateType.Default, new GraphicControl.DrawInfo()
            //    {
            //          DrawBackColor = ResultMenuBackColor
            //        , DrawImage = CodeGeneratorResource.Icon_File
            //        , DrawText = "History"
            //        , DrawTextColor = Color.FromArgb(60, 60, 60)
            //    })
            //    .SetDrawInfo(GraphicControl.StateType.Over, new GraphicControl.DrawInfo()
            //    {
            //          DrawBackColor = ResultMenuBackColor.GetBrightnessColor(20)
            //        , DrawBorderColor = Color.FromArgb(93, 107, 153)
            //    });
            //HistoryButton.Click += _HistoryButton_Click;
        }

        public void SetEditorStyleList(List<EditorStyle> editorStyleList)
        {
            EditorStyleList = editorStyleList;
        }


    }
}
