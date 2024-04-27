using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Controls
{
    public partial class UserEditor
    {
        public Dictionary<string, UserEditorLineStyleInfo> LineStyles = new Dictionary<string, UserEditorLineStyleInfo>();
    }

    public class UserEditorLineStyleInfo
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
