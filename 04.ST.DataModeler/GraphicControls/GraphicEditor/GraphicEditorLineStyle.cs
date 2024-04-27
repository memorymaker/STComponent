using ST.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.DataModeler
{
    public partial class GraphicEditor
    {
        public Dictionary<string, GraphicEditorLineStyleInfo> LineStyles = new Dictionary<string, GraphicEditorLineStyleInfo>();
    }

    public class GraphicEditorLineStyleInfo
    {
        #region Conditions
        public List<int> Lines { get; set; }
        #endregion

        #region Line
        /// <summary>
        /// 라인의 배경색을 가져오거나 설정합니다.
        /// </summary>
        public Color LineBackColor { get; set; } = Color.Empty;
        #endregion

        /// <summary>
        /// 라인 고정 여부를 가져오거나 설정합니다.
        /// </summary>
        public bool FixedLine { get; set; } = true;

        /// <summary>
        /// 라인 텍스트의 읽기 전용 여부를 가져오거나 설정합니다.
        /// </summary>
        public bool ReadOnlyLine { get; set; } = true;
    }
}
