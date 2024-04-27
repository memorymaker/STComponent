using ST.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ST.DataModeler
{
    public partial class GraphicEditor
    {
        private string CommentStylesName = "Comment";

        public Dictionary<string, GraphicEditorStyleInfo> Styles = new Dictionary<string, GraphicEditorStyleInfo>();

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
                GraphicEditorLineStyleInfo node = pair.Value;
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
                    GraphicEditorStyleInfo info = pair.Value;
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

    public class GraphicEditorStyleInfo
    {
        #region Name
        public string Name { get; set; }
        #endregion

        #region Conditions
        public string Regex { get; set; }

        public bool CaseSensitive { get; set; } = false;

        public bool MultiLine { get; set; } = false;
        #endregion

        #region System(Name: Comment)
        public string MultiLineStartingText { get; set; }

        public string MultiLineEndText { get; set; }

        public string SingleLineText { get; set; }
        #endregion

        #region Etc
        public string Text { get; set; }
        #endregion

        #region Line
        public Color LineColor { get; set; } = Color.Empty;
        #endregion

        #region Text
        public Color FontColor { get; set; } = Color.Empty;

        public Color BackColor { get; set; } = Color.Empty;

        public bool Bold { get; set; } = false;

        public bool Italic { get; set; } = false;

        public bool Underline { get; set; } = false;
        #endregion
    }
}
