using ST.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.CodeGenerator
{
    public class EditorStyle
    {
        public string Name { get; set; }

        public int Sort { get; set; }

        public List<EditorStyleContent> Content { get; set; }
    }

    public class EditorStyleContent
    {
        #region Name
        public string Name { get; set; }
        #endregion

        #region Conditions
        public string Regex { get; set; }

        public bool? CaseSensitive { get; set; } = false;

        public bool? MultiLine { get; set; } = false;
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
        public string LineColor { get; set; }
        #endregion

        #region Text
        public string FontColor { get; set; }

        public string BackColor { get; set; }

        public bool? Bold { get; set; } = false;

        public bool? Italic { get; set; } = false;

        public bool? Underline { get; set; } = false;
        #endregion

        #region Create By CodeGenerator.LoadConfig()
        public UserEditorStyleInfo UserEditorStyleInfo { get; set; }
        #endregion
    }
}
