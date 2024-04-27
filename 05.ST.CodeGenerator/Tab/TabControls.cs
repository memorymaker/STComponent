using ST.Controls;
using ST.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ST.CodeGenerator.TemplateProcessor;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace ST.CodeGenerator
{
    public partial class Tab
    {
        private TabMainSpllit MainSplit;
        public UserEditor TemplateEditor;
        private List<string> TemplateBaseKeywords;
        private List<string> TemplateBaseLoopKeywords;
        private List<string> TemplateLoopKeywords;
        private List<string> TemplateNoneLoopKeywords;
        private DrawingPanel ErrorListWrapPanel;
        private UserListView ErrorList;
        public UserEditor ResultEditor;

        private Dictionary<string, UserEditorStyleInfo> EditorCommandStyles { get; set; }

        private Color MainSplitterColor = Color.FromArgb(93, 107, 153);
        private int ResultMenuHeight = 30;
        private Color ResultMenuBackColor = Color.FromArgb(204, 213, 240);

        // Ref
        private UserEditor PrevFocusedEditor;

        private void LoadControls()
        {
            this.BeginControlUpdate();
            SetControlsData();
            SetControls();
            SizeChanged += Tab_SizeChanged;
            this.EndControlUpdate();
        }

        private void SetControlsData()
        {
            TemplateBaseKeywords = new List<string>
            {
                "o/ id:",
                "s/",
                "b/",
                "e/",
                "c/",
                "d/",
                "if/",
                "elseif/",
                "endif/"
            };

            TemplateBaseLoopKeywords = new List<string>()
            {
                "s/",
                "b/",
                "e/"
            };

            TemplateLoopKeywords = new List<string>()
            {
                "{row}",
                "{crow}"
            };

            TemplateNoneLoopKeywords = new List<string>()
            {
                "{from id:}",
                "{id:, 0, 0}",
                "{__tabName}",
                "{__tabNameUpper}",
                "{__tabNameLower}"
            };

            EditorCommandStyles = new Dictionary<string, UserEditorStyleInfo>()
            {
                { "TemplateKeywordDeclare", new UserEditorStyleInfo() {
                      Regex = "^(d\\/)"
                    , CaseSensitive = true
                    , MultiLine = true
                    , LineColor = Color.FromArgb(227, 234, 247)
                    , FontColor = Color.FromArgb(100, 100, 150)
                }}
                , { "TemplateKeywordIf", new UserEditorStyleInfo() {
                      Regex = "^(if\\/|elseif\\/|else\\/|endif\\/)"
                    , CaseSensitive = true
                    , MultiLine = true
                    , LineColor = Color.FromArgb(248, 226, 227)
                    , FontColor = Color.FromArgb(150, 100, 100)
                }}
                , { "TemplateKeywordLoop", new UserEditorStyleInfo() {
                      Regex = "^(o\\/|s\\/|b\\/|e\\/|c\\/)"
                    , CaseSensitive = true
                    , MultiLine = true
                    , LineColor = Color.FromArgb(235, 235, 235)
                    , FontColor = Color.FromArgb(125, 125, 125)
                }}
            };
        }

        private void SetControls()
        {
            MainSplit = new TabMainSpllit();
            MainSplit.Orientation = Orientation.Horizontal;
            MainSplit.Dock = DockStyle.Fill;
            MainSplit.SplitterColor = MainSplitterColor;
            MainSplit.MouseUp += MainSplit_MouseUp;
            MainSplit.Panel1MinSize = 95;
            //MainSplit.Panel2Minimum = 50;
            MainSplit.Panel2.BackColor = ResultMenuBackColor;

            TemplateEditor = new UserEditor();
            TemplateEditor.Dock = DockStyle.Fill;
            TemplateEditor.GotFocus += Editor_GotFocus;
            TemplateEditor.AutoCompleteShown += TemplateEditor_AutoCompleteShown;
            TemplateEditor.KeyUp += TemplateEditor_KeyUp;
            foreach (var keyword in EditorCommandStyles)
            {
                TemplateEditor.Styles.Add(keyword.Key, keyword.Value);
            }
            MainSplit.Panel1.Controls.Add(TemplateEditor);

            ErrorListWrapPanel = new DrawingPanel();
            ErrorListWrapPanel.Visible = false;
            ErrorListWrapPanel.Dock = DockStyle.Bottom;
            ErrorListWrapPanel.Height = 64;
            ErrorListWrapPanel.BorderTopWidth = 1;
            ErrorListWrapPanel.BorderTopColor = Color.FromArgb(230, 230, 230);
            MainSplit.Panel1.Controls.Add(ErrorListWrapPanel);

            ErrorList = new UserListView();
            ErrorList.Dock = DockStyle.Fill;
            ErrorList.Height = 63;
            ErrorList.ColumnHeight = 20;
            ErrorList.BackColor = Color.FromArgb(249, 249, 249);
            ErrorList.AddColumn(new UserListViewColumn("Error Description", "DESCRIPTON"));
            ErrorList.AddColumn(new UserListViewColumn("Line", "LINE"));
            ErrorList.AddColumn(new UserListViewColumn("Column", "COLUMN"));
            ErrorList.Columns[1].ItemAlign = UserListAlignType.Center;
            ErrorList.Columns[2].ItemAlign = UserListAlignType.Center;
            ErrorListWrapPanel.Controls.Add(ErrorList);
            
            MainSplit.Panel2.Padding = new Padding(0, ResultMenuHeight, 0, 0);
            ResultEditor = new UserEditor();
            ResultEditor.Dock = DockStyle.Fill;
            ResultEditor.GotFocus += Editor_GotFocus;

            MainSplit.Panel2.Controls.Add(ResultEditor);

            Controls.Add(MainSplit);
        }

        private void Tab_SizeChanged(object sender, EventArgs e)
        {
            ErrorList.Columns[0].Width = Width - 160 - ErrorList.VScrollBarWidth;
            ErrorList.Columns[1].Width = 80;
            ErrorList.Columns[2].Width = 80;
        }

        private void MainSplit_MouseUp(object sender, MouseEventArgs e)
        {
            if (PrevFocusedEditor != null)
            {
                PrevFocusedEditor.Focus();
            }
        }

        private void Editor_GotFocus(object sender, EventArgs e)
        {
            PrevFocusedEditor = (UserEditor)sender;
        }

        private void TemplateEditor_AutoCompleteShown(object sender, UserEditorShowAutoCompleteEventArg e)
        {
            var _this = (UserEditor)sender;
            List<string> declareData = new List<string>();
            TemplateEditor_AutoCompleteShown_SetDeclareData(ref declareData);

            // Append TemplateNoneLoopKeywordList
            declareData.AddRange(TemplateNoneLoopKeywords);

            // Starting point of the line: BaseKeywords + NoneLoopKeywords
            if (_this.SelectionStart == 0
            || (_this.SelectionStart >= 2 && _this.Text.Substring(_this.SelectionStart - 2, 2) == "\r\n"))
            {
                List<string> list = new List<string>();
                list.AddRange(TemplateBaseKeywords);
                if (declareData != null)
                {
                    list.AddRange(declareData);
                }

                e.Data = list;
            }
            else
            {
                CodeGenerator codeGenerator = GetParentCodeGenerator();
                
                // Loop Area - Column Name
                int firstCharIndexInLine = _this.GetFirstCharIndexFromIndex(_this.SelectionStart);
                string commandAreaString = firstCharIndexInLine + 2 <= _this.Text.Length
                    ? _this.Text.Substring(firstCharIndexInLine, 2) : string.Empty;
                if (TemplateBaseLoopKeywords.Contains(commandAreaString))
                {
                    List<string> list = new List<string>();
                    List<string> columnNameData = TemplateEditor_AutoCompleteShown_GetFieldData();
                    if (columnNameData != null)
                    {
                        list.AddRange(columnNameData);
                    }

                    if (declareData != null)
                    {
                        list.AddRange(declareData);
                    }

                    list.AddRange(TemplateLoopKeywords);
                    
                    e.Data = list;
                }
                // ID List {from id:nodeID} {id:nodeID, 0, columnName}
                else if (TemplateEditor_AutoCompleteShown_IsIDPosition(_this, firstCharIndexInLine))
                {
                    List<string> list = TemplateEditor_AutoCompleteShown_GetOptionIDData();
                    e.Data = list;
                }
                // Column List {id:nodeID, 0, columnName}
                else if (TemplateEditor_AutoCompleteShown_IsColumnPosition(_this, codeGenerator.NodeData, firstCharIndexInLine))
                {
                    List<string> list = new List<string>();
                    DataColumnCollection columns = codeGenerator.NodeData[codeGenerator.NodeData.Keys.Min()].Columns;
                    foreach(DataColumn column in columns)
                    {
                        list.Add(column.ColumnName);
                    }

                    e.Data = list;
                    // TemplateLoopKeywordList
                }
                // Condition Line
                else if (commandAreaString == COMMAND.CONDITION)
                {
                    e.Data = TemplateEditor_AutoCompleteShown_GetFieldData();
                    if (declareData != null)
                    {
                        e.Data.AddRange(declareData);
                    }
                }
                // Declare
                else
                {
                    if (declareData != null)
                    {
                        e.Data = declareData;
                    }
                }
            }
        }

        private void TemplateEditor_KeyUp(object sender, KeyEventArgs e)
        {
            var moveKeys = new Keys[]
            {
                  Keys.Up, Keys.Right, Keys.Down, Keys.Left, Keys.PageUp, Keys.PageDown, Keys.Home, Keys.End
                , Keys.Escape, Keys.Space, Keys.Return, Keys.ShiftKey, Keys.ControlKey, Keys.Alt, Keys.Tab, Keys.Menu
                , Keys.Delete, Keys.Back
            };

            if (!moveKeys.Contains(e.KeyCode) && !(e.Control && (e.KeyCode == Keys.Z || e.KeyCode == Keys.Y)))
            {
                CodeGenerator codeGenerator = GetParentCodeGenerator();

                // ID List
                if (TemplateEditor_AutoCompleteShown_IsIDPosition(TemplateEditor))
                {
                    TemplateEditor.OnShowAutoComplete();
                }
                // Column List
                else if (TemplateEditor_AutoCompleteShown_IsColumnPosition(TemplateEditor, codeGenerator.NodeData))
                {
                    TemplateEditor.OnShowAutoComplete();
                }
            }
        }

        private void TemplateEditor_AutoCompleteShown_SetDeclareData(ref List<string> declareData)
        {
            try
            {
                var parent = GetParentCodeGenerator();
                var commonValueDic = parent.GetCommonVariables();
                foreach (KeyValuePair<string, string> pair in commonValueDic)
                {
                    declareData.Add($"{{{pair.Key}}}");
                }

                var valueDic = Processor.GetDictionaryValues(TemplateEditor.Text);
                foreach (KeyValuePair<string, string> pair in valueDic)
                {
                    declareData.Add($"{{{pair.Key}}}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TemplateEditor_AutoCompleteShown_GetDeclareData - {ex.Message}");

                //MessageBox.Show(ex.Message, "Generate Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //TemplateProcessorException templateEx = ex as TemplateProcessorException;
                //if (templateEx != null)
                //{
                //    int index = TemplateEditor.GetFirstCharIndexFromLine(templateEx.LineNumber - 1)
                //              + templateEx.CharNumber - 1;
                //    TemplateEditor.SelectionStart = index;
                //}
            }
        }

        private List<string> TemplateEditor_AutoCompleteShown_GetFieldData()
        {
            List<string> rs = null;
            var parent = GetParentCodeGenerator();
            if (parent.NodeData.Count > 0)
            {
                rs = new List<string>();
                foreach (KeyValuePair<string, DataTable> pair in parent.NodeData)
                {
                    foreach(DataColumn column in pair.Value.Columns)
                    {
                        rs.Add($"{{{column.ColumnName}}}");
                    }
                    break;
                }
            }
            return rs;
        }

        private bool TemplateEditor_AutoCompleteShown_IsIDPosition(UserEditor editor, int firstCharIndexInLine = -1)
        {
            bool rs;
            if (firstCharIndexInLine < 0)
            {
                firstCharIndexInLine = editor.GetFirstCharIndexFromIndex(editor.SelectionStart);
            }

            string text = editor.Text.Substring(firstCharIndexInLine, editor.SelectionStart - firstCharIndexInLine);
            if (text.Length >= 5 && text.Substring(0, 2) == "o/")
            {
                text = text.Remove(0, 2).Trim();
                List<string> idString = new List<string> { "id", ":" };
                rs = MatchesWithTokens(text, idString);
            }
            else if (new Regex("{ *from *id *: *").Match(text).Success)
            {
                List<string> idString = new List<string> { "{", "from" ,"id", ":" };
                rs = MatchesWithTokens(text, idString);
            }
            else if (new Regex("{ *id *: *([A-Za-z0-9_]*) *").Match(text).Success)
            {
                List<string> idString = new List<string> { "{", "id", ":" };
                rs = MatchesWithTokens(text, idString);
            }
            else
            {
                rs = false;
            }

            return rs;
        }

        private bool TemplateEditor_AutoCompleteShown_IsColumnPosition(UserEditor editor, Dictionary<string, DataTable> columnTableDic, int firstCharIndexInLine = -1)
        {
            bool rs = false;

            if (columnTableDic != null && columnTableDic.Count > 0)
            {
                if (firstCharIndexInLine < 0)
                {
                    firstCharIndexInLine = editor.GetFirstCharIndexFromIndex(editor.SelectionStart);
                }

                string text = editor.Text.Substring(firstCharIndexInLine, editor.SelectionStart - firstCharIndexInLine);

                Regex regx = new Regex("{ *id *: *([A-Za-z0-9]*) *, *([0-9]*) *,( *)");
                Match match = regx.Match(text);
                do
                {
                    int rsStringLength = 0;
                    if (match.Success)
                    {
                        if (match.Groups[3].Index + match.Groups[3].Length == text.Length)
                        {
                            rs = true;
                            break;
                        }
                    }
                    match = regx.Match(text, match.Index + (rsStringLength == 0 ? match.Length : rsStringLength));
                }
                while (match.Success);
            }

            return rs;
        }

        private bool MatchesWithTokens(string text, List<string> prevTokens)
        {
            bool rs = true;
            for (int i = text.Length - 1; 0 <= i; i--)
            {
                if (text[i] != ' ')
                {
                    string idNode = prevTokens[prevTokens.Count - 1];
                    int sp = i - (idNode.Length - 1);
                    if (sp >= 0)
                    {
                        if (text.Substring(sp, idNode.Length) == idNode)
                        {
                            i = i - prevTokens[prevTokens.Count - 1].Length + 1;
                            prevTokens.RemoveAt(prevTokens.Count - 1);

                            if (prevTokens.Count == 0)
                            {
                                rs = true;
                                break;
                            }
                        }
                        else
                        {
                            rs = false;
                            break;
                        }
                    }
                    else
                    {
                        rs = false;
                        break;
                    }
                }
            }
            return rs;
        }

        private List<string> TemplateEditor_AutoCompleteShown_GetOptionIDData()
        {
            List<string> rs = null;
            var parent = GetParentCodeGenerator();
            if (parent.NodeData != null && parent.NodeData.Count > 0)
            {
                rs = new List<string>();
                foreach (KeyValuePair<string, DataTable> pair in parent.NodeData)
                {
                    rs.Add($"{pair.Key}");
                }
            }
            return rs;
        }
    }
}