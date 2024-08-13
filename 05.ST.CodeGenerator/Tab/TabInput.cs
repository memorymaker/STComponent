using ST.Controls;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ST.Core;

namespace ST.CodeGenerator
{
    public partial class Tab
    {
        // Options
        private Color NoneLoopValueFontColor = Color.FromArgb(140, 140, 140);
        private Color NoneLoopValueBackColor = Color.Empty;
        private Color NoneLoopValueBorderColor = Color.Empty;

        private Color LoopValueFontColor = Color.FromArgb(170, 110, 110);
        private Color LoopValueBackColor = Color.Empty;
        private Color LoopValueBorderColor = Color.Empty;

        private Color LoopTextFontColor = Color.FromArgb(160, 160, 160);
        private Color LoopTextBackColor = Color.Empty;
        private Color LoopTextBorderColor = Color.Empty;

        private Color LoopLineBackColor = Color.FromArgb(235, 235, 235);

        // Ref
        private List<int> IndexMapper = new List<int>();
        private bool TemplateEditor_TextChanged_GeneratorBlock = false;

        private void LoadInput()
        {
            TitleChanged += Tab_TitleChanged;

            MainSplit.Panel2.MouseMove += Panel2_MouseMove;

            TemplateEditor.KeyDown += TemplateEditor_KeyDown;
            TemplateEditor.DelayedDataChanged += TemplateEditor_DelayedDataChanged;
            TemplateEditor.KeyDownHook += Editor_KeyDownHook;

            ErrorList.ItemDoubleClick += ErrorList_ItemDoubleClick;

            ResultEditor.KeyDown += ResultEditor_KeyDown;
            ResultEditor.KeyDownHook += Editor_KeyDownHook;
            ResultEditor.ReadOnly = true;
        }

        private void Tab_TitleChanged(object sender, EventArgs e)
        {
            Generate();
        }

        private void ErrorList_ItemDoubleClick(object sender, UserListView.GraphicListViewEventArgs e)
        {
            int lineIndex = Convert.ToInt32(e.Item.Row["LINE"]) - 1;
            int charIndexOfLine = Convert.ToInt32(e.Item.Row["COLUMN"]) - 1;
            if (lineIndex >= 0)
            {
                int selectionStart = TemplateEditor.GetFirstCharIndexFromLine(lineIndex) + charIndexOfLine;
                TemplateEditor.SelectionStart = selectionStart;
                TemplateEditor.SelectionLength = 0;
                TemplateEditor.Focus();
            }
        }

        private void Panel2_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            DrawPanel2();
        }

        private void TemplateEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                Generate();
            }
        }

        private void TemplateEditor_DataChanged(object sender, UserEditor.DataEventArgs e)
        {
            Generate();
        }

        private void TemplateEditor_DelayedDataChanged(object sender, EventArgs e)
        {
            Generate();
        }

        private void Editor_KeyDownHook(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.PageDown:
                        MoveNextTab();
                        e.SuppressKeyPress = true;
                        break;
                    case Keys.PageUp:
                        MovePrevTab();
                        e.SuppressKeyPress = true;
                        break;
                }
            }
        }

        private void ResultEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                Execute();
            }
        }

        private void ResultEditor_DataChanged(object sender, UserEditor.DataEventArgs e)
        {
            if (e.Type != UserEditor.SBChangedType.Bind)
            {
                bool isContainsByLineStyles = e.Type == UserEditor.SBChangedType.Remove
                    ? ResultEditor.ContainsByLineStyles(e.Index)
                    : ResultEditor.ContainsByLineStyles(Math.Max(e.Index - e.Length, 0));

                int insertedTextRNIndex = (e.InsertedText ?? string.Empty).IndexOf("\r\n");
                int removedTextRNIndex = (e.RemovedText ?? string.Empty).IndexOf("\r\n");
                bool isInsertEnterOrRemovedEnterAtLastIndexOfLoopBlock =
                       (e.Type == UserEditor.SBChangedType.Append && insertedTextRNIndex >= 0)
                    || (e.Type == UserEditor.SBChangedType.Remove && removedTextRNIndex >= 0);

                if (isContainsByLineStyles && !isInsertEnterOrRemovedEnterAtLastIndexOfLoopBlock)
                {
                    // Pending...
                }
                else
                {
                    TemplateEditor_TextChanged_GeneratorBlock = true;
                    UserEditor.DataEventArgs revisedE = new UserEditor.DataEventArgs(e.Type, e.Index, e.Length, e.LineCount, e.IsUnredoProc, e.InsertedText, e.RemovedText);
                    switch (revisedE.Type)
                    {
                        case UserEditor.SBChangedType.Append:
                            if (isInsertEnterOrRemovedEnterAtLastIndexOfLoopBlock)
                            {
                                revisedE.InsertedText = revisedE.InsertedText.Substring(insertedTextRNIndex, revisedE.Length - insertedTextRNIndex);
                            }

                            int insertIndex = revisedE.Index - revisedE.Length;
                            if (insertIndex >= IndexMapper.Count)
                            {
                                if (insertIndex == 0)
                                {
                                    TemplateEditor.InsertText(0, revisedE.InsertedText);
                                }
                                else
                                {
                                    TemplateEditor.InsertText(TemplateEditor.Text.Length, revisedE.InsertedText);
                                }
                            }
                            else
                            {
                                if (IndexMapper[insertIndex] == -1)
                                {
                                    int i = 1;
                                    while (true)
                                    {
                                        if (insertIndex - i >= 0 && IndexMapper[insertIndex - i] >= 0)
                                        {
                                            TemplateEditor.InsertText(IndexMapper[insertIndex - i] - i, revisedE.InsertedText);
                                            break;
                                        }
                                        else if (insertIndex + i < IndexMapper.Count && IndexMapper[insertIndex + i] >= 0)
                                        {
                                            TemplateEditor.InsertText(IndexMapper[insertIndex - i] + i, revisedE.InsertedText);
                                            break;
                                        }
                                        else if (!(insertIndex - i >= 0) || !(insertIndex + i < IndexMapper.Count))
                                        {
                                            if (insertIndex - i >= IndexMapper.Count - insertIndex + i)
                                            {
                                                TemplateEditor.InsertText(0, revisedE.InsertedText);
                                            }
                                            else
                                            {
                                                TemplateEditor.InsertText(TemplateEditor.Text.Length - 1, revisedE.InsertedText);
                                            }
                                        }

                                        i++;
                                    }
                                }
                                else
                                {
                                    TemplateEditor.InsertText(IndexMapper[insertIndex], revisedE.InsertedText);
                                }
                            }
                            break;
                        case UserEditor.SBChangedType.Remove:
                            if (isInsertEnterOrRemovedEnterAtLastIndexOfLoopBlock)
                            {
                                revisedE.Length -= removedTextRNIndex;
                            }

                            TemplateEditor.RemoveText(IndexMapper[revisedE.Index], revisedE.Length);
                            TemplateEditor.SelectionLength = 0;
                            break;
                        case UserEditor.SBChangedType.Modifier:
                            TemplateEditor.ModifierSB(IndexMapper[revisedE.Index - 1], ResultEditor.Text.Substring(revisedE.Index - 1, 1)[0]);
                            break;
                    }

                    TemplateEditor.Refresh();
                    RefreshIndexMapper();
                    TemplateEditor_TextChanged_GeneratorBlock = false;
                }
            }
        }

        #region Function
        public void Generate()
        {
            var parent = GetParentCodeGenerator();

            TemplateProcessor.Result result = new TemplateProcessor.Result();

            Dictionary<string, string> customValueDic = parent.GetCommonVariables();
            customValueDic.Add("__tabName", Title);
            customValueDic.Add("__tabNameUpper", Title.ToUpper());
            customValueDic.Add("__tabNameLower", Title.ToLower());

            string resultText = Processor.CreateDocument(TemplateEditor.Text, parent.NodeData, parent.RelationData, ref result, customValueDic);
            IndexMapper = result.IndexMapper;

            // ResultEditor.RangeStyles - Clear
            ResultEditor.RangeStyles.Clear();

            // ResultEditor.RangeStyles - Add NoneLoopValueStyleInfo
            UserEditorRangeStyleInfo noneLoopValueStyleInfo = new UserEditorRangeStyleInfo();
            noneLoopValueStyleInfo.FontColor = NoneLoopValueFontColor;
            noneLoopValueStyleInfo.BackColor = NoneLoopValueBackColor;
            noneLoopValueStyleInfo.BorderColor = NoneLoopValueBorderColor;
            noneLoopValueStyleInfo.Ranges = result.NoneLoopValueRangeList;
            noneLoopValueStyleInfo.ReadOnly = true;
            ResultEditor.RangeStyles.Add("noneLoopValueStyleInfo", noneLoopValueStyleInfo);

            // ResultEditor.RangeStyles - Add LoopValueStyleInfo
            UserEditorRangeStyleInfo loopValueStyleInfo = new UserEditorRangeStyleInfo();
            loopValueStyleInfo.FontColor = LoopValueFontColor;
            loopValueStyleInfo.BackColor = LoopValueBackColor;
            loopValueStyleInfo.BorderColor = LoopValueBorderColor;
            loopValueStyleInfo.Ranges = result.LoopValueRangeList;
            loopValueStyleInfo.ReadOnly = true;
            ResultEditor.RangeStyles.Add("loopValueStyleInfo", loopValueStyleInfo);

            // ResultEditor.RangeStyles - Add LoopValueStyleInfo
            UserEditorRangeStyleInfo loopTextStyleInfo = new UserEditorRangeStyleInfo();
            loopTextStyleInfo.FontColor = LoopTextFontColor;
            loopTextStyleInfo.BackColor = LoopTextBackColor;
            loopTextStyleInfo.BorderColor = LoopTextBorderColor;
            loopTextStyleInfo.Ranges = result.LoopTextRangeList;
            loopTextStyleInfo.ReadOnly = true;
            ResultEditor.RangeStyles.Add("loopTextStyleInfo", loopTextStyleInfo);

            // ResultEditor.LineStyles - Clear
            ResultEditor.LineStyles.Clear();

            // ResultEditor.LineStyles - Add loopLineStyleInfo
            foreach (KeyValuePair<string, List<int>> pair in result.LoopLineListDictionary)
            {
                UserEditorLineStyleInfo loopLineStyleInfo = new UserEditorLineStyleInfo();
                loopLineStyleInfo.LineBackColor = LoopLineBackColor;
                loopLineStyleInfo.FixedLine = true;
                loopLineStyleInfo.ReadOnlyLine = true;
                loopLineStyleInfo.Lines = pair.Value;
                ResultEditor.LineStyles.Add(pair.Key, loopLineStyleInfo);
            }

            // For Refresh
            Console.WriteLine(resultText);
            ResultEditor.Text = resultText;

            // Set errorData
            if (result.ExceptionList.Count > 0)
            {
                DataTable errorData = new DataTable();
                errorData.AddColumns("{s}DESCRIPTON {s}LINE {s}COLUMN");
                foreach(TemplateProcessorException exceptionNode in result.ExceptionList)
                {
                    errorData.Rows.Add(new object[] { exceptionNode.Message, $"{exceptionNode.LineNumber}", $"{exceptionNode.CharNumber}" });
                }

                // Bind ErrorList
                ErrorList.SuspendLayout();
                ErrorList.Bind(errorData);
                foreach (UserListViewItem item in ErrorList.Items)
                {
                    item.ForeColor = Color.Red;
                    item.BackColor = Color.FromArgb(249, 249, 249);
                }
                ErrorList.ResumeLayout();
                ErrorList.Draw();

                if (!ErrorListWrapPanel.Visible)
                {
                    MainSplit.Panel1.BeginControlUpdate();
                    ErrorListWrapPanel.Visible = true;
                    TemplateEditor.SetScrollToCursor();
                    MainSplit.Panel1.EndControlUpdate();
                }
            }
            else
            {
                if (ErrorListWrapPanel.Visible)
                {
                    MainSplit.Panel1.BeginControlUpdate();
                    ErrorListWrapPanel.Visible = false;
                    ErrorList.Clear();
                    MainSplit.Panel1.EndControlUpdate();
                }
            }
        }

        // Not Used
        private void RefreshIndexMapper()
        {
            var parent = GetParentCodeGenerator();
            TemplateProcessor.Result result = new TemplateProcessor.Result();
            //string resultText = Processor.CreateDocument(TemplateEditor.Text, parent.NodeData, ref result);
            IndexMapper = result.IndexMapper;
        }

        private void Execute()
        {
            // Exec ResultEditor.Text
        }

        private CodeGenerator GetParentCodeGenerator()
        {
            CodeGenerator rs = null;

            Control parent = Parent;
            do
            {
                if (parent == null)
                {
                    break;
                }
                rs = parent as CodeGenerator;
                parent = parent.Parent;
            }
            while (rs == null);

            return rs;
        }

        private void MoveNextTab()
        {
            Tab parent = Parent as Tab;
            Tab target = parent == null ? this : parent;

            var selectedTitleInfo = (from titleInfo in target.TitleList where titleInfo.Selected == true select titleInfo).ToList()[0];
            if (selectedTitleInfo.Sort < target.TitleList.Count - 1)
            {
                target.SelectTitle(selectedTitleInfo.Sort + 1);
                target.SetPrevFocusedEditor();
            }
        }

        private void MovePrevTab()
        {
            Tab parent = Parent as Tab;
            Tab target = parent == null ? this : parent;

            var selectedTitleInfo = (from titleInfo in target.TitleList where titleInfo.Selected == true select titleInfo).ToList()[0];
            if (0 < selectedTitleInfo.Sort)
            {
                target.SelectTitle(selectedTitleInfo.Sort - 1);
                target.SetPrevFocusedEditor();
            }
        }

        public void SetPrevFocusedEditor()
        {
            //if (PrevFocusedEditor != null)
            //{
            //    PrevFocusedEditor.Focus();
            //}
        }
        #endregion
    }
}
