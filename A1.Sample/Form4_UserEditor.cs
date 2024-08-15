using ST.Controls;
using ST.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            // 텍스트 박스를 초기화합니다.
            userEditor.Text = string.Empty;
        }

        private void btSetSampleText_Click(object sender, EventArgs e)
        {
            // 텍스트 박스에 값을 입력합니다.
            userEditor.Text = "if/ {test} == a\r\n\r\nint a = 0;\r\n\r\nif (a == 1)\r\n{\r\n	var t = 0;\r\n}\r\n\r\n// Single LIne Comment\r\n\r\n/*\r\nMulti Lines Comment\r\n*/\r\n\r\nendif/";
        }

        private void btSetStyle_Click(object sender, EventArgs e)
        {
            // 텍스트 스타일을 초기화합니다.
            userEditor.Styles.Clear();

            // if 관련 키워드의 스타일을 설정합니다.
            userEditor.Styles.Add("TemplateKeywordIf", new UserEditorStyleInfo() {
                  Regex = "^(if\\/|elseif\\/|else\\/|endif\\/)"
                , CaseSensitive = true
                , MultiLine = true
                , LineColor = Color.FromArgb(248, 226, 227)
                , FontColor = Color.FromArgb(150, 100, 100)
            });

            // C# 관련 키워드의 스타일을 설정합니다.
            userEditor.Styles.Add("CSCommand1", new UserEditorStyleInfo() {
                  Regex = "(^|(?<=[^\\w]))(if|else|for|foreach|continue|break|do|while|goto|try|catch|finally|switch|case|default|return|throw)((?=[^\\w])|$)"
                , CaseSensitive = true
                , FontColor = Color.FromArgb(143, 8, 196)
            });

            // C# 관련 키워드의 스타일을 설정합니다.
            userEditor.Styles.Add("CSCommand2", new UserEditorStyleInfo() {
                  Regex = "(^|(?<=[^\\w]))(abstract|as|base|bool|byte|char|checked|class|const|decimal|delegate|double|enum|event|explicit|extern|false|fixed|float|implicit|in|int|interface|internal|is|lock|long|namespace|new|null|object|operator|out|override|params|private|protected|public|readonly|ref|sbyte|sealed|short|sizeof|stackalloc|static|string|struct|this|true|typeof|uint|ulong|unchecked|unsafe|ushort|using|virtual|void|volatile|add|and|alias|ascending|args|async|await|by|descending|dynamic|equals|file|from|get|global|group|init|into|join|let|managed|nameof|nint|not|notnull|nuint|on|or|orderby|partial|record|remove|required|scoped|select|set|unmanaged|value|var|when|where|with|yield)((?=[^\\w])|$)"
                , CaseSensitive = true
                , FontColor = Color.FromArgb(0, 0, 255)
            });

            // 주석 관련 스타일을 설정합니다.
            // Comment : UserEditor System Key
            userEditor.Styles.Add("Comment", new UserEditorStyleInfo()
            {
                  MultiLineStartingText = "/*"
                , MultiLineEndText = "*/"
                , SingleLineText = "//"
                , FontColor = Color.FromArgb(0, 128, 0)
            });

            // 텍스트 박스를 새로 고칩니다.
            userEditor.Refresh();
        }

        private void btClearStyle_Click(object sender, EventArgs e)
        {
            // 텍스트 스타일을 초기화합니다.
            userEditor.Styles.Clear();

            // 텍스트 박스를 새로 고칩니다.
            userEditor.Refresh();
        }

        private void btToggleEnabled_Click(object sender, EventArgs e)
        {
            // 사용 가능 여부를 설정합니다.
            userEditor.Enabled = !userEditor.Enabled;
        }

        private void btSetAutoComplate_Click(object sender, EventArgs e)
        {
            // 중복 방지를 위해 이벤트를 해제합니다.
            userEditor.KeyUp -= UserEditor_KeyUp;
            userEditor.AutoCompleteShown -= UserEditor_AutoCompleteShown;

            // 이벤트를 바인딩합니다.
            // KeyUp 이벤트에 자동 완성을 사용하기 위해 바인딩합니다.
            userEditor.KeyUp += UserEditor_KeyUp;
            // 자동 완성 리스트가 뜨기 직전에 발생합니다. 자동 완성 목록을 정의하기 위해 바인딩합니다.
            userEditor.AutoCompleteShown += UserEditor_AutoCompleteShown;
        }

        // 자동 완성 목록을 정의합니다.
        private List<string> AutoCompleteList = new List<string>()
        {
            "if"
            , "else"
            , "private"
            , "protected"
            , "internal"
            , "public"
        };

        private void UserEditor_AutoCompleteShown(object sender, UserEditorShowAutoCompleteEventArg e)
        {
            // 자동 완성에 사용될 데이터를 설정합니다.
            e.Data = AutoCompleteList;
        }

        private void UserEditor_KeyUp(object sender, KeyEventArgs e)
        {
            // 이동 키를 정의합니다.
            var moveKeys = new Keys[]
            {
                  Keys.Up, Keys.Right, Keys.Down, Keys.Left, Keys.PageUp, Keys.PageDown, Keys.Home, Keys.End
                , Keys.Escape, Keys.Space, Keys.Return, Keys.ShiftKey, Keys.ControlKey, Keys.Alt, Keys.Tab, Keys.Menu
                , Keys.Delete, Keys.Back
            };

            // 이동 키 및 Ctrl + Z(Undo), Ctrl + Y(Redo)
            if (!moveKeys.Contains(e.KeyCode) && !(e.Control && (e.KeyCode == Keys.Z || e.KeyCode == Keys.Y)))
            {
                // 커서 위치를 마지막으로 하는 단어를 가져옵니다.
                string word = userEditor.GetCurrentWord();
                if (word != string.Empty)
                {
                    // 현재 단어가 자동 완성 목록에 있는지 확인합니다.
                    List<string> searchResultList = AutoCompleteList.Search(word);
                    if (searchResultList?.Count > 0)
                    {
                        // 자동 완성 이벤트를 호출합니다.
                        userEditor.OnShowAutoComplete(word);
                    }
                }
            }
        }

        private void btClearAutoComplate_Click(object sender, EventArgs e)
        {
            // 이벤트를 해제합니다.
            userEditor.KeyUp -= UserEditor_KeyUp;
            userEditor.AutoCompleteShown -= UserEditor_AutoCompleteShown;
        }

        private void btToggleInfo_Click(object sender, EventArgs e)
        {
            // 커서 정보 표시 여부를 가져오거나 설정합니다.
            userEditor.ShowSelectoinInfo = !userEditor.ShowSelectoinInfo;
        }

    }
}