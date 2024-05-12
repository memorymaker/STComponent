using ST.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ST.CodeGenerator.TemplateProcessor;

namespace Sample
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            userEditor.Text = string.Empty;
        }

        private void btSetSampleText_Click(object sender, EventArgs e)
        {
            userEditor.Text = "if/ {test} == a\r\n\r\nint a = 0;\r\n\r\nif (a == 1)\r\n{\r\n	var t = 0;\r\n}\r\n\r\n// Single LIne Comment\r\n\r\n/*\r\nMulti Lines Comment\r\n*/\r\n\r\nendif/";
        }

        private void btSetStyle_Click(object sender, EventArgs e)
        {
            userEditor.Styles.Clear();

            userEditor.Styles.Add("TemplateKeywordIf", new UserEditorStyleInfo() {
                  Regex = "^(if\\/|elseif\\/|else\\/|endif\\/)"
                , CaseSensitive = true
                , MultiLine = true
                , LineColor = Color.FromArgb(248, 226, 227)
                , FontColor = Color.FromArgb(150, 100, 100)
            });
            
            userEditor.Styles.Add("CSCommand1", new UserEditorStyleInfo() {
                  Regex = "(^|(?<=[^\\w]))(if|else|for|foreach|continue|break|do|while|goto|try|catch|finally|switch|case|default|return|throw)((?=[^\\w])|$)"
                , CaseSensitive = true
                , FontColor = Color.FromArgb(143, 8, 196)
            });

            userEditor.Styles.Add("CSCommand2", new UserEditorStyleInfo() {
                  Regex = "(^|(?<=[^\\w]))(abstract|as|base|bool|byte|char|checked|class|const|decimal|delegate|double|enum|event|explicit|extern|false|fixed|float|implicit|in|int|interface|internal|is|lock|long|namespace|new|null|object|operator|out|override|params|private|protected|public|readonly|ref|sbyte|sealed|short|sizeof|stackalloc|static|string|struct|this|true|typeof|uint|ulong|unchecked|unsafe|ushort|using|virtual|void|volatile|add|and|alias|ascending|args|async|await|by|descending|dynamic|equals|file|from|get|global|group|init|into|join|let|managed|nameof|nint|not|notnull|nuint|on|or|orderby|partial|record|remove|required|scoped|select|set|unmanaged|value|var|when|where|with|yield)((?=[^\\w])|$)"
                , CaseSensitive = true
                , FontColor = Color.FromArgb(0, 0, 255)
            });

            // Comment : UserEditor System Key
            userEditor.Styles.Add("Comment", new UserEditorStyleInfo()
            {
                  MultiLineStartingText = "/*"
                , MultiLineEndText = "*/"
                , SingleLineText = "//"
                , FontColor = Color.FromArgb(0, 128, 0)
            });

            userEditor.Refresh();
        }

        private void btClearStyle_Click(object sender, EventArgs e)
        {
            userEditor.Styles.Clear();
            userEditor.Refresh();
        }

        private void btToggleEnabled_Click(object sender, EventArgs e)
        {
            userEditor.Enabled = !userEditor.Enabled;
        }

        private void btToggleInfo_Click(object sender, EventArgs e)
        {
            userEditor.ShowSelectoinInfo = !userEditor.ShowSelectoinInfo;
        }
    }
}