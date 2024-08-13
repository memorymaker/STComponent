using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ST.Controls
{
    public partial class UserEditor
    {
        private string CommentStylesName = "Comment";

        /// <summary>
        /// 이 컨트롤의 텍스트 스타일의 가져오거나 반환합니다.
        /// </summary>
        public Dictionary<string, UserEditorStyleInfo> Styles = new Dictionary<string, UserEditorStyleInfo>();

        private void LoadUserEditorStyle()
        {
        }

        #region Function innser
        #endregion

        #region Function Outer
        private Color GetLineBackColor(int lineIndex, string lineText)
        {
            Color rs = Color.Empty;

            // LineStyles
            foreach (var pair in LineStyles)
            {
                UserEditorLineStyleInfo node = pair.Value;
                if (node.Lines.Contains(lineIndex))
                {
                    rs = node.LineBackColor;
                    break;
                }
            }

            // Styles
            if (rs.IsEmpty)
            {
                foreach (var pair in Styles)
                {
                    UserEditorStyleInfo info = pair.Value;
                    if (info.LineColor != Color.Empty)
                    {
                        RegexOptions regexOptionsCaseSensitive = info.CaseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase;
                        RegexOptions regexOptionsCaseMultiline = info.MultiLine ? RegexOptions.Multiline : RegexOptions.None;
                        Regex regex = new Regex(info.Regex, regexOptionsCaseSensitive | regexOptionsCaseMultiline);
                        Match match = regex.Match(lineText);
                        if (match.Success)
                        {
                            rs = info.LineColor;
                        }
                    }
                }
            }

            return rs;
        }
        #endregion
    }

    public class UserEditorStyleInfo
    {
        #region Name
        /// <summary>
        /// 텍스트 스타일의 이름을 가져오거나 설정합니다.
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region Conditions
        /// <summary>
        /// 텍스트 스타일의 정규식을 가져오거나 설정합니다.
        /// </summary>
        public string Regex { get; set; }

        /// <summary>
        /// 정규식의 대소문자 구분 여부를 가져오거나 설정합니다.
        /// </summary>
        public bool CaseSensitive { get; set; } = false;

        /// <summary>
        /// 정규식의 멀티라인 적용 여부를 가져오거나 설정합니다.
        /// </summary>
        public bool MultiLine { get; set; } = false;
        #endregion

        #region System(Name: Comment)
        /// <summary>
        /// 현재 스타일의 Name이 "Comment" 일 때 멀티라인의 매핑 시작 문자를 가져오거나 설정합니다.
        /// </summary>
        public string MultiLineStartingText { get; set; }

        /// <summary>
        /// 현재 스타일의 Name이 "Comment" 일 때 멀티라인의 매핑 끝 문자를 가져오거나 설정합니다.
        /// </summary>
        public string MultiLineEndText { get; set; }

        /// <summary>
        /// 현재 스타일의 Name이 "Comment" 일 때 싱글 라인의 매핑 문자를 가져오거나 설정합니다.
        /// </summary>
        public string SingleLineText { get; set; }
        #endregion

        #region Etc
        public string Text { get; set; }
        #endregion

        #region Line
        /// <summary>
        /// 매핑된 스타일의 라인 색상을 가져오거나 설정합니다.
        /// </summary>
        public Color LineColor { get; set; } = Color.Empty;
        #endregion

        #region Text
        /// <summary>
        /// 매핑된 스타일의 폰트 색상을 가져오거나 설정합니다.
        /// </summary>
        public Color FontColor { get; set; } = Color.Empty;

        /// <summary>
        /// 매핑된 스타일의 배경 색상을 가져오거나 설정합니다.
        /// </summary>
        public Color BackColor { get; set; } = Color.Empty;

        /// <summary>
        /// 매핑된 스타일의 폰트 굵기 여부를 가져오거나 설정합니다.
        /// </summary>
        public bool Bold { get; set; } = false;

        /// <summary>
        /// 매핑된 스타일의 이탤릭 여부을 가져오거나 설정합니다.
        /// </summary>
        public bool Italic { get; set; } = false;

        /// <summary>
        /// 매핑된 스타일의 밑줄 여부를 가져오거나 설정합니다.
        /// </summary>
        public bool Underline { get; set; } = false;
        #endregion
    }
}
