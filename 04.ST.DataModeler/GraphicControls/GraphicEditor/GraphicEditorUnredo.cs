using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public partial class GraphicEditor
    {
        private class UnredoClass
        {
            #region Reference
            #endregion
            private int KeyDownSelectionStart = -1;
            private string KeyDownSelectionText = string.Empty;
            private int KeyDownTextLength = -1;
            public bool TextChangedEventBolck = false;
            private Stack<UnredoStackInfo> UndoStack = new Stack<UnredoStackInfo>();
            private Stack<UnredoStackInfo> RedoStack = new Stack<UnredoStackInfo>();
            public string ReturnText = "\r\n";

            public UnredoClass()
            {
            }

            public UnredoClass(GraphicEditor UserEditor)
            {
                this.BindUserEditor(UserEditor);
            }

            public void Clear()
            {
                this.UndoStack.Clear();
                this.RedoStack.Clear();
            }

            public void BindUserEditor(GraphicEditor UserEditor)
            {
                this.Clear();
                UserEditor.KeyDown += UserEditor_KeyDown;
                UserEditor.TextChanged += UserEditor_TextChanged;
            }

            public void UpdateUndoStack(string data)
            {
                UnredoStackInfo stack = UndoStack.Pop();
                stack.Data = data;
                UndoStack.Push(stack);
            }

            public void UpdateUndoStack(UnredoNodeType type)
            {
                UnredoStackInfo stack = UndoStack.Pop();
                stack.NodeType = type;
                UndoStack.Push(stack);
            }

            public void UpdateUndoStack(UnredoStackInfo unredoStackInfo)
            {
                UndoStack.Pop();
                UndoStack.Push(unredoStackInfo);
            }

            public void PushUndoStack(UnredoStackInfo unredoStackInfo)
            {
                UndoStack.Push(unredoStackInfo);
            }

            public UnredoStackInfo PopUndoStack()
            {
                return UndoStack.Pop();
            }

            public UnredoStackInfo PeekUndoStack()
            {
                return UndoStack.Peek();
            }

            public void UpdateKeyDownData(int keyDownSelectionStart, string keyDownSelectionText, int keyDownTextLength)
            {
                KeyDownSelectionStart = keyDownSelectionStart;
                KeyDownSelectionText = keyDownSelectionText;
                KeyDownTextLength = keyDownTextLength;
            }

            private void UserEditor_KeyDown(object sender, KeyEventArgs e)
            {
                GraphicEditor UserEditor = (GraphicEditor)sender;
                if (UserEditor.Selection.Start >= 0)
                {
                    if (e.Control && e.KeyCode == Keys.Z)
                    {
                        this.TextChangedEventBolck = true;
                        this.Undo(UserEditor);
                        e.SuppressKeyPress = true;
                    }
                    else if (e.Control && e.KeyCode == Keys.Y)
                    {
                        this.TextChangedEventBolck = true;
                        this.Redo(UserEditor);
                        e.SuppressKeyPress = true;
                    }
                    else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                    {
                        this.TextChangedEventBolck = true;
                        if (e.KeyCode == Keys.Delete || UserEditor.Selection.Length > 0)
                        {
                            this.PushDelete(UserEditor);
                        }
                        else if (e.KeyCode == Keys.Back)
                        {
                            this.PushBack(UserEditor);
                        }
                    }
                    else if (e.Shift && e.KeyCode == Keys.Tab)
                    {
                        this.TextChangedEventBolck = true;
                        this.PushShiftTab(UserEditor);
                    }
                    else if (!e.Shift && e.KeyCode == Keys.Tab
                             && (UserEditor.Selection.Length == UserEditor.Data.GetLineLengthFromCharIndex(UserEditor.Selection.Start)
                                 || UserEditor.Data.GetFullLineCount(UserEditor.Selection.Start, UserEditor.Selection.Length) > 1
                                )
                            )
                    {
                        this.TextChangedEventBolck = true;
                        this.PushTab(UserEditor);
                    }
                    else
                    {
                        this.TextChangedEventBolck = false;
                        this.KeyDownSelectionStart = UserEditor.Selection.Start;
                        this.KeyDownSelectionText = UserEditor.Data.SB.ToString(UserEditor.Selection.Start, UserEditor.Selection.Length);
                        this.KeyDownTextLength = UserEditor.TextLength;
                    }
                }
            }

            private void UserEditor_TextChanged(object sender, EventArgs e)
            {
                GraphicEditor UserEditor = (GraphicEditor)sender;

                if (!this.TextChangedEventBolck && this.KeyDownSelectionStart >= 0)
                {
                    // Cursor -> Cursor
                    if (this.KeyDownSelectionText.Length == 0)
                    {
                        this.PushAppend(UserEditor);
                    }
                    // Selection -> Cursor
                    else
                    {
                        // Push Delete
                        this.UndoStack.Push(new UnredoStackInfo(this.KeyDownSelectionStart, this.KeyDownSelectionText.Length, this.KeyDownSelectionText, ProcessType.Delete, UnredoNodeType.Parent));
                        this.RedoStack.Clear();
                        // Set Value
                        this.KeyDownSelectionStart = UserEditor.Selection.Start;
                        this.KeyDownSelectionText = UserEditor.Data.SB.ToString(UserEditor.Selection.Start, UserEditor.Selection.Length);
                        this.KeyDownTextLength = UserEditor.TextLength;
                    }
                }
            }

            private void PushDelete(GraphicEditor UserEditor)
            {
                int sp = UserEditor.Selection.Start;
                if (sp < UserEditor.TextLength)
                {
                    int len = UserEditor.Selection.Length == 0 ? 1 : UserEditor.Selection.Length;
                    if (UserEditor.Data.SB.ToString(sp, len) == "\r" && UserEditor.Data.SB.ToString(sp, len + 1) == this.ReturnText) // CR/LF
                    {
                        len = 2;
                    }
                    this.UndoStack.Push(new UnredoStackInfo(sp, UserEditor.Selection.Length, UserEditor.Data.SB.ToString(sp, len), ProcessType.Delete));
                    this.RedoStack.Clear();
                }
            }

            private void PushBack(GraphicEditor UserEditor)
            {
                int sp = UserEditor.Selection.Start;
                if (sp > 0)
                {
                    int len = UserEditor.Selection.Length == 0 ? 1 : UserEditor.Selection.Length;
                    if (UserEditor.Data.SB.ToString(sp - len, len) == "\n" && UserEditor.Data.SB.ToString(sp - (len + 1), (len + 1)) == this.ReturnText) // CR/LF
                    {
                        len = 2;
                    }
                    this.UndoStack.Push(new UnredoStackInfo(sp, UserEditor.Selection.Length, UserEditor.Data.SB.ToString(sp - len, len), ProcessType.Back));
                    this.RedoStack.Clear();
                }
            }

            private void PushShiftTab(GraphicEditor UserEditor)
            {
                int lineCount = UserEditor.Data.GetFullLineCount(UserEditor.Selection.Start, UserEditor.Selection.Length);
                lineCount = lineCount == 0 ? 1 : lineCount;
                int startLine = UserEditor.Data.GetLineFromCharIndex(UserEditor.Selection.Start);
                int accumulatedLenght = 0;

                for (int i = 0; i < lineCount; i++)
                {
                    UnredoNodeType nodeType = (i == 0 && i == lineCount - 1) ? UnredoNodeType.None
                                      : (i == 0 ? UnredoNodeType.GroupStart
                                      : (i == lineCount - 1 ? UnredoNodeType.GroupEnd : UnredoNodeType.GroupNode));
                    int sp = UserEditor.Data.GetFirstCharIndexFromLine(startLine + i);
                    int len = UserEditor.Data.GetLeftSpaceCount(sp, UserEditor.TabSpaceCount);
                    if (len > 0)
                    {
                        this.UndoStack.Push(
                            new UnredoStackInfo(sp - accumulatedLenght
                                , len
                                , new string(' ', UserEditor.TabSpaceCount)
                                , ProcessType.ShiftTab
                                , nodeType)
                        );
                        accumulatedLenght += len;
                    }
                }

                if (accumulatedLenght > 0)
                {
                    this.RedoStack.Clear();
                }
            }

            private void PushTab(GraphicEditor UserEditor)
            {
                int sp = UserEditor.Selection.Start;
                int len = UserEditor.TabSpaceCount;
                int lineCount = UserEditor.Data.GetFullLineCount(sp, UserEditor.Selection.Length);
                int startLine = UserEditor.Data.GetLineFromCharIndex(sp);

                for (int i = 0; i < lineCount; i++)
                {
                    UnredoNodeType nodeType = (i == 0 && i == lineCount - 1) ? UnredoNodeType.None
                                      : (i == 0                        ? UnredoNodeType.GroupStart
                                      : (i == lineCount - 1            ? UnredoNodeType.GroupEnd : UnredoNodeType.GroupNode));
                    this.UndoStack.Push(
                        new UnredoStackInfo(UserEditor.Data.GetFirstCharIndexFromLine(startLine + i) + (i * len)
                            , len
                            , new string(' ', UserEditor.TabSpaceCount)
                            , ProcessType.Tab
                            , nodeType)
                    );
                }
                this.RedoStack.Clear();
            }

            private void PushAppend(GraphicEditor UserEditor)
            {
                if (UserEditor.TextLength > this.KeyDownTextLength)
                {
                    int sp = this.KeyDownSelectionStart;
                    int len = UserEditor.TextLength - this.KeyDownTextLength;

                    UnredoNodeType nodeType = UndoStack.Count > 0 && UndoStack.Peek().NodeType == UnredoNodeType.Parent
                        ? UnredoNodeType.Child
                        : UnredoNodeType.None;

                    this.UndoStack.Push(new UnredoStackInfo(this.KeyDownSelectionStart, this.KeyDownSelectionText.Length, UserEditor.Data.SB.ToString(sp, len), ProcessType.Append, nodeType));
                    this.RedoStack.Clear();
                }
            }

            private void Undo(GraphicEditor UserEditor, int GroupCalledCount = 0)
            {
                if (this.UndoStack.Count > 0)
                {
                    UnredoStackInfo stack = this.UndoStack.Pop();

                    switch (stack.ProcessType)
                    {
                        case ProcessType.Append:
                            UserEditor.Data.RemoveSB(stack.SelectionStart, stack.Data.Length);
                            UserEditor.SetSelection(stack.SelectionStart, stack.SelectionLength);
                            break;
                        case ProcessType.Delete:
                            UserEditor.Data.InsertSB(stack.SelectionStart, stack.Data);
                            UserEditor.SetSelection(stack.SelectionStart, stack.SelectionLength);
                            break;
                        case ProcessType.Back:
                            UserEditor.Data.InsertSB(stack.SelectionStart - stack.Data.Length, stack.Data);
                            UserEditor.SetSelection(stack.SelectionStart, stack.SelectionLength);
                            break;
                        case ProcessType.ShiftTab:
                            UserEditor.Data.InsertSB(stack.SelectionStart, stack.Data);
                            if (stack.NodeType == UnredoNodeType.GroupStart || stack.NodeType == UnredoNodeType.None)
                            {
                                int endLine = UserEditor.Data.GetLineFromCharIndex(stack.SelectionStart) + GroupCalledCount;
                                int endLineLastIndex = UserEditor.Data.GetFirstCharIndexFromLine(endLine) + UserEditor.Data.GetLineLength(endLine);
                                UserEditor.SetSelection(stack.SelectionStart, endLineLastIndex - stack.SelectionStart);
                            }
                            break;
                        case ProcessType.Tab:
                            UserEditor.Data.RemoveSB(stack.SelectionStart, stack.Data.Length);
                            if (stack.NodeType == UnredoNodeType.GroupStart || stack.NodeType == UnredoNodeType.None)
                            {
                                int endLine = UserEditor.Data.GetLineFromCharIndex(stack.SelectionStart) + GroupCalledCount;
                                int endLineLastIndex = UserEditor.Data.GetFirstCharIndexFromLine(endLine) + UserEditor.Data.GetLineLength(endLine);
                                UserEditor.SetSelection(stack.SelectionStart, endLineLastIndex - stack.SelectionStart);
                            }
                            break;
                    }

                    this.RedoStack.Push(stack);

                    if (stack.NodeType == UnredoNodeType.Child && UndoStack.Peek().NodeType == UnredoNodeType.Parent)
                    {
                        Undo(UserEditor);
                    }

                    if (stack.NodeType == UnredoNodeType.GroupEnd || stack.NodeType == UnredoNodeType.GroupNode)
                    {
                        Undo(UserEditor, ++GroupCalledCount);
                    }
                }
            }

            private void Redo(GraphicEditor UserEditor, int GroupCalledCount = 0)
            {
                if (this.RedoStack.Count > 0)
                {
                    UnredoStackInfo stack = this.RedoStack.Pop();
                    if (stack.ProcessType == ProcessType.Append)
                    {
                        UserEditor.Data.InsertSB(stack.SelectionStart, stack.Data);
                        if (stack.SelectionLength == 0)
                        {
                            UserEditor.SetSelection(stack.SelectionStart + stack.Data.Length, 0);
                        }
                        else
                        {
                            UserEditor.SetSelection(stack.SelectionStart, stack.Data.Length);
                        }
                        this.UndoStack.Push(stack);
                    }
                    else if (stack.ProcessType == ProcessType.Delete || stack.ProcessType == ProcessType.Back)
                    {
                        if (stack.ProcessType == ProcessType.Delete || stack.SelectionLength > 0)
                        {
                            UserEditor.Data.RemoveSB(stack.SelectionStart, stack.Data.Length);
                            UserEditor.SetSelection(stack.SelectionStart, 0);
                            this.UndoStack.Push(stack);
                        }
                        else if (stack.ProcessType == ProcessType.Back)
                        {
                            UserEditor.Data.RemoveSB(stack.SelectionStart - stack.Data.Length, stack.Data.Length);
                            UserEditor.SetSelection(stack.SelectionStart - stack.Data.Length, 0);
                            this.UndoStack.Push(stack);
                        }
                    }
                    else if (stack.ProcessType == ProcessType.ShiftTab)
                    {
                        UserEditor.Data.RemoveSB(stack.SelectionStart, stack.Data.Length);
                        if (stack.NodeType == UnredoNodeType.GroupEnd || stack.NodeType == UnredoNodeType.None)
                        {
                            int startLine = UserEditor.Data.GetLineFromCharIndex(stack.SelectionStart) - GroupCalledCount;
                            int startLineIndex = UserEditor.Data.GetFirstCharIndexFromLine(startLine);
                            UserEditor.SetSelection(startLineIndex, stack.SelectionStart + UserEditor.Data.GetLineLengthFromCharIndex(stack.SelectionStart) - startLineIndex);
                        }
                        this.UndoStack.Push(stack);
                    }
                    else if (stack.ProcessType == ProcessType.Tab)
                    {
                        UserEditor.Data.InsertSB(stack.SelectionStart, stack.Data);
                        if (stack.NodeType == UnredoNodeType.GroupEnd || stack.NodeType == UnredoNodeType.None)
                        {
                            int startLine = UserEditor.Data.GetLineFromCharIndex(stack.SelectionStart) - GroupCalledCount;
                            int startLineIndex = UserEditor.Data.GetFirstCharIndexFromLine(startLine);
                            UserEditor.SetSelection(startLineIndex, stack.SelectionStart + UserEditor.Data.GetLineLengthFromCharIndex(stack.SelectionStart) - startLineIndex);
                        }
                        this.UndoStack.Push(stack);
                    }

                    if (stack.NodeType == UnredoNodeType.Parent && RedoStack.Peek().NodeType == UnredoNodeType.Child)
                    {
                        Redo(UserEditor);
                    }

                    if (stack.NodeType == UnredoNodeType.GroupStart || stack.NodeType == UnredoNodeType.GroupNode)
                    {
                        Redo(UserEditor, ++GroupCalledCount);
                    }
                }
            }

            public enum ProcessType { Append, Back, Delete, ShiftTab, Tab };

            public struct UnredoStackInfo
            {
                public int SelectionStart;
                public int SelectionLength;
                public string Data;
                public ProcessType ProcessType;
                public UnredoNodeType NodeType;

                public UnredoStackInfo(int _SelectionStart, int _SelectionLength, string _Data, ProcessType _Type, UnredoNodeType _NodeType = UnredoNodeType.None)
                {
                    SelectionStart = _SelectionStart;
                    SelectionLength = _SelectionLength;
                    Data = _Data;
                    ProcessType = _Type;
                    NodeType = _NodeType;
                }

                public static UnredoStackInfo Empty => new UnredoStackInfo();
            }
        }

        public enum UnredoNodeType { None, Parent, Child, GroupStart, GroupNode, GroupEnd };

    }
}
