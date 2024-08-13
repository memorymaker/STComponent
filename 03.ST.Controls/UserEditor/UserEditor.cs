using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Design;
using System.Reflection;
using ST.Core;
using ST.Core.Extension;
using System.Text.RegularExpressions;

namespace ST.Controls
{
    public partial class UserEditor : UserControl
    {
        #region Option
        public int TabSpaceCount { get; set; } = 4;

        public bool ReadOnly { get; set; } = false;

        public int DelayedDataChangedPeriod { get; set; } = 100;
        #endregion

        #region Event
        public new event EventHandler<EventArgs> TextChanged;
        public event EventHandler<DataEventArgs> DataChanged;
        public event EventHandler<EventArgs> DelayedDataChanged;
        public event KeyEventHandler KeyDownHook;
        #endregion

        #region Class
        private DataClass Data;
        private SelectionClass Selection = new SelectionClass();
        private DarwClass Draw;
        private UnredoClass Unredo;
        #endregion

        #region Timer
        private System.Threading.Timer Timer;
        #endregion

        #region Reference
        private bool IsMouseDown
        {
            get
            {
                return _IsMouseDown;
            }
            set
            {
                if (_IsMouseDown != value)
                {
                    _IsMouseDown = value;
                }
            }
        }
        private bool _IsMouseDown = false;
        private bool IsCtrlDown
        {
            get
            {
                return _IsCtrlDown;
            }
            set
            {
                if (_IsCtrlDown != value)
                {
                    _IsCtrlDown = value;
                }
            }
        }
        private bool _IsCtrlDown = false;
        private bool IsAltDown = false;
        private bool IsShiftDown = false;

        private DateTime DelayedDataChangedDateTime = DateTime.MinValue;
        #endregion

        #region Properties
        /// <summary>
        /// 이 컨트롤과 관련된 텍스트를 가져오거나 설정합니다.
        /// </summary>
        [Category("Text"), Description("Text")]
        new public string Text
        {
            get
            {
                return Data.SB.ToString();
            }
            set
            {
                Unredo.Clear();
                StringBuilder sb = GetRevisedTextRNT(value);

                if (sb.Length < Selection.Start)
                {
                    Selection.Start = sb.Length;
                }

                if (sb.Length < Selection.Start + Selection.Length)
                {
                    Selection.Length = sb.Length - Selection.Start;
                }

                Data.BindSB(sb);
                Draw.Draw();
            }
        }

        new public Font Font
        {
            get
            {

                return base.Font;
            }
            set
            {
                base.Font = value;
                Draw.Draw();
            }
        }

        public int SelectionStart
        {
            get
            {
                return Selection.Start;
            }
            set
            {
                Selection.SetSelection(Data, value, Selection.Length);
            }
        }

        public int SelectionLength
        {
            get
            {
                return Selection.Length;
            }
            set
            {
                Selection.SetSelection(Data, Selection.Start, value);
            }
        }

        public int TextLength
        {
            get
            {
                return Data.SB.Length;
            }
        }

        public string SelectedText
        {
            get
            {
                return Selection.Length == 0 ? "" : Data.SB.ToString(Selection.Start, Selection.Length);
            }
        }

        public bool WordWrap
        {
            get
            {
                return Data.WordWrap;
            }
            set
            {
                if (Data.WordWrap != value)
                {
                    HScroll.Visible = !value;

                    Data.WordWrap = value;
                    Data.SetLineData();
                    // todo : WordWrap 변경 시 코드 추가
                    // 스크롤 리셋
                    // 스크롤 value 설정
                    // Draw?
                }
            }
        }

        /// <summary>
        /// 컨트롤이 사용자 상호 작용에 응답할 수 있는지를 나타내는 값을 가져오거나 설정합니다.
        /// </summary>
        /// <returns>컨트롤이 사용자 상호 작용에 응답할 수 있으면 <see langword="true"/>이고, 그렇지 않으면 <see langword="false"/>입니다. 기본값은 <see langword="true"/>입니다.</returns>
        new public bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                if (base.Enabled != value)
                {
                    base.Enabled = value;
                }
            }
        }
        #endregion

        #region Load
        public UserEditor()
        {
            LoadScreen();
            LoadThis();
            LoadAutoComplete();
            LoadUserEditorStyle();
        }

        private void LoadThis()
        {
            this.SetDefault();
            this.SetEvents();
        }

        private void SetDefault()
        {
            Data = new DataClass(this);
            Data.BindSB(string.Empty);
            Draw = new DarwClass(this);

            BackColor = Color.FromArgb(250, 250, 250);
            base.Font = new Font("돋움체", 10);

            Timer = new System.Threading.Timer(TimerTick, null, Selection.ShowingPeriod, Selection.ShowingPeriod);
            Cursor = Cursors.IBeam;
            TabStop = false;
        }
        #endregion

        #region Event
        private void SetEvents()
        {
            this.GotFocus += UserEditor_GotFocus;
            this.LostFocus += UserEditor_LostFocus;
            this.SizeChanged += UserEditor_SizeChanged;
            this.VisibleChanged += UserEditor_VisibleChanged;
            // this.Paint += UserEditor_Paint;

            this.KeyDown += UserEditor_KeyDown;
            this.KeyUp += UserEditor_KeyUp;
            this.MouseWheel += UserEditor_MouseWheel;
            this.MouseDown += UserEditor_MouseDown;
            this.MouseMove += UserEditor_MouseMove;
            this.MouseUp += UserEditor_MouseUp;
            this.MouseLeave += UserEditor_MouseLeave;

            this.Disposed += UserEditor_Disposed;

            Data.OnSBChanged += Data_OnSBChanged;
            Selection.OnSelectionChanged += Selection_OnSelectionChanged;
            Unredo = new UnredoClass(this);
        }

        private void UserEditor_Disposed(object sender, EventArgs e)
        {
            Timer.Dispose();
        }

        private void UserEditor_Paint(object sender, PaintEventArgs e)
        {
            Draw.Draw();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            Draw.Draw();
        }

        private void UserEditor_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                ScrollBars = ScrollBars;
            }
        }

        private void UserEditor_GotFocus(object sender, EventArgs e)
        {
        }

        private void UserEditor_LostFocus(object sender, EventArgs e)
        {
            IsMouseDown = false;
            IsCtrlDown = false;
            IsAltDown = false;
            IsShiftDown = false;

            if (!ContainsFocus)
            {
                Draw.DrawCursor = false;
                Draw.Draw();
            }
        }

        private void Selection_OnSelectionChanged(object sender, SelectionEventArgs e)
        {
            Draw.Draw();
            this.SetScrollToCursor();
        }

        private void Data_OnSBChanged(object sender, DataEventArgs e)
        {
            if (!e.IsUnredoProc)
            {
                this.SetScroll();
                this.SetScrollToCursor();

                Data_OnSBChanged_SetLineNRangeStyleValue(e);
            }
            else
            {
                Data_OnSBChanged_SetLineNRangeStyleValue(e);
            }

            this.TextChanged?.Invoke(this, null);
            this.DataChanged?.Invoke(this, e);
            DelayedDataChangedDateTime = DateTime.Now;
        }

        private void Data_OnSBChanged_SetLineNRangeStyleValue(DataEventArgs e)
        {
            switch (e.Type)
            {
                case SBChangedType.Append:
                    if (e.LineCount > 1)
                    {
                        int indexRevision = 0;
                        int appendTextInFirstLineLength = Data.SB.ToString().IndexOf("\r\n", e.Index - e.Length);
                        if (IsLastIndexInBlock(appendTextInFirstLineLength))
                        {
                            indexRevision = e.Length - 2;
                        }

                        ReviseLineStylesValue(e.Index - e.Length + indexRevision, e.LineCount - 1);
                    }
                    ReviseRangeStylesValue(e.Index - e.Length, e.Length);
                    break;
                case SBChangedType.Remove:
                    if (e.LineCount > 1)
                    {
                        ReviseLineStylesValue(e.Index, -(e.LineCount - 1));
                    }
                    ReviseRangeStylesValue(e.Index, -e.Length);
                    break;
            }
        }

        private void UserEditor_SizeChanged(object sender, EventArgs e)
        {
            int widthRevision = 0;
            int heightRevision = 0;

            if (WordWrap)
            {
                VScroll.BlockDrawing = true;
                VScroll.Left = this.Width - VScroll.Width - widthRevision;
                VScroll.Top = 0;
                VScroll.Height = this.Height - heightRevision;
                VScroll.BlockDrawing = false;
                VScroll.Draw();

                Data.SetLineData();
            }
            else
            {
                VScroll.BlockDrawing = true;
                VScroll.Left = this.Width - VScroll.Width - widthRevision;
                VScroll.Top = 0;
                VScroll.Height = this.Height - HScroll.Height - heightRevision;
                VScroll.BlockDrawing = false;
                VScroll.Draw();

                HScroll.BlockDrawing = true;
                HScroll.Left = 0;
                HScroll.Top = this.Height - HScroll.Height - heightRevision;
                HScroll.Width = this.Width - VScroll.Width - widthRevision
                    - (ShowSelectoinInfo ? Draw.SelectionInfoWidth : 0);
                HScroll.BlockDrawing = false;
                HScroll.Draw();
            }

            SetScroll();
            Draw.Draw();
        }

        private void VScroll_ValueChanged(object sender, EventArgs e)
        {
            this.VScrollValue = (this.VScroll.Value * Draw.FontPixelSize.Height) * -1;
            if (sender != null)
            {
                Draw.Draw();
            }
        }

        private void HScroll_ValueChanged(object sender, EventArgs e)
        {
            this.HScrollValue = (this.HScroll.Value * Draw.FontPixelSize.Width) * -1;
            if (sender != null)
            {
                Draw.Draw();
            }
        }

        private void UserEditor_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!(IsAutoCompleteListShown && AutoCompleteList.Bounds.Contains(e.Location)))
            {
                if (IsAutoCompleteListShown)
                {
                    AutoCompleteList.Visible = false;
                }
                this.SetVScrollValue(this.VScrollValue + (e.Delta / 20) * Draw.FontPixelSize.Height);
            }
        }

        private void UserEditor_KeyDown(object sender, KeyEventArgs e)
        {
            // ! Do not change Data.SB in this method
            // ! Called by ProcessCmdKey

            this.IsCtrlDown = e.Control;
            this.IsAltDown = e.Alt;
            this.IsShiftDown = e.Shift;

            Selection.IndexReference = e.Shift && Selection.IndexReference < 0 ? Selection.Start : Selection.IndexReference;

            bool executed = AutoComplete_KeyDown(e);
            if (!executed)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                    case Keys.Down:
                    case Keys.Left:
                    case Keys.Right:
                    case Keys.PageUp:
                    case Keys.PageDown:
                    case Keys.Home:
                    case Keys.End:
                        bool executedHook = UserEditor_KeyDownHook(e);
                        if (!executedHook)
                        {
                            int index = Selection.Start;
                            int length = Selection.Length;

                            Selection.Proc(Data, e.KeyCode, e.Control, e.Alt, e.Shift, this.Font, this.PageMoveLineSize, ref index, ref length);

                            switch (e.KeyCode)
                            {
                                case Keys.PageUp:
                                    this.SetVScrollValue(this.VScrollValue + this.PageMovePixelSize);
                                    break;
                                case Keys.PageDown:
                                    this.SetVScrollValue(this.VScrollValue - this.PageMovePixelSize);
                                    break;
                            }

                            Selection.SetSelection(Data, index, length);
                        }

                        e.SuppressKeyPress = true;
                        Draw.DrawCursor = true;
                        break;
                }

                if (!executed)
                {
                    if (e.KeyCode != Keys.Up && e.KeyCode != Keys.Down && e.KeyCode != Keys.PageUp && e.KeyCode != Keys.PageDown)
                    {
                        Selection.ClearLeftReference();
                    }

                    if (IsMouseDown && (e.KeyCode != Keys.ControlKey && e.KeyCode != Keys.Menu && e.KeyCode != Keys.ShiftKey))
                    {
                        Selection.IndexReference = Selection.Start;
                    }
                }
            }
        }

        private bool UserEditor_KeyDownHook(KeyEventArgs e)
        {
            KeyEventArgs hookE = new KeyEventArgs(e.KeyData);
            KeyDownHook?.Invoke(this, hookE);
            return hookE.SuppressKeyPress;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            KeyEventArgs e = new KeyEventArgs(keyData);
            this.OnKeyDown(e);
            if (e.SuppressKeyPress == true)
            {
                return true;
            }
            else if (e.KeyCode == Keys.Tab)
            {
                bool executed = AutoComplete_ProcessCmdKey(e.KeyCode);
                if (!executed)
                {
                    this.KeyTab(e.Shift);
                    Draw.Draw();
                }
                return true;
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        private void UserEditor_KeyUp(object sender, KeyEventArgs e)
        {
            this.IsCtrlDown = e.Control;
            this.IsAltDown = e.Alt;
            this.IsShiftDown = e.Shift;
            if (!this.IsMouseDown)
            {
                Selection.IndexReference = !e.Shift && Selection.Length == 0 ? -1 : Selection.IndexReference;
            }
        }

        private void UserEditor_MouseDown(object sender, MouseEventArgs e)
        {
            this.Focus();

            if (IsAutoCompleteListShown)
            {
                AutoCompleteList.Visible = false;
            }

            int x = e.X - Draw.PaddingLeft - this.HScrollValue;
            int y = e.Y - Draw.PaddingTop - this.VScrollValue;
            int index = Selection.Start;
            int length = Selection.Length;
            bool selectionChanged;
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                selectionChanged = Selection.Proc(Data, SelectionMouseActionType.DoubleDown, x, y, this.IsCtrlDown, this.IsAltDown, IsShiftDown, this.Font, ref index, ref length);
            }
            else
            {
                selectionChanged = Selection.Proc(Data, SelectionMouseActionType.Down, x, y, this.IsCtrlDown, this.IsAltDown, IsShiftDown, this.Font, ref index, ref length);
                this.IsMouseDown = true;
            }
            Selection.Start = index;
            Selection.Length = length;

            if (selectionChanged)
            {
                this.ThisSelectionChanged();
            }

            Draw.DrawCursor = true;
            Draw.Draw();
        }

        public void SetSelection(Point location)
        {
            int x = location.X - Draw.PaddingLeft - this.HScrollValue;
            int y = location.Y - Draw.PaddingTop - this.VScrollValue;
            int index = Selection.Start;
            int length = Selection.Length;
            bool selectionChanged = Selection.Proc(Data, SelectionMouseActionType.Down, x, y, this.IsCtrlDown, this.IsAltDown, IsShiftDown, this.Font, ref index, ref length);
            Selection.Start = index;
            Selection.Length = length;
            if (selectionChanged)
            {
                this.ThisSelectionChanged();
            }
        }

        private void UserEditor_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.IsMouseDown)
            {
                int x = e.X - Draw.PaddingLeft - this.HScrollValue;
                int y = e.Y - Draw.PaddingTop - this.VScrollValue;
                int index = Selection.Start;
                int length = Selection.Length;
                bool selectionChanged = Selection.Proc(Data, SelectionMouseActionType.Move, x, y, this.IsCtrlDown, this.IsAltDown, IsShiftDown, this.Font, ref index, ref length);
                
                Selection.Start = index;
                Selection.Length = length;

                if (selectionChanged)
                {
                    this.ThisSelectionChanged();
                }
                Draw.Draw();
            }
            else
            {
            }
        }

        private void UserEditor_MouseUp(object sender, MouseEventArgs e)
        {
            this.IsMouseDown = false;
        }

        private void UserEditor_MouseLeave(object sender, EventArgs e)
        {
            this.IsMouseDown = false;
        }

        private void ThisSelectionChanged()
        {
            this.SetScrollToCursor();
        }
        #endregion

        #region Callback
        public void TimerTick(object obj)
        {
            try
            {
                Invoke(new Action(delegate ()
                {
                    if (Focused && Visible && Width > 0 && Height > 0 && !IsDisposed)
                    {
                        Draw.Draw();
                        Draw.DrawCursor = !Draw.DrawCursor;

                        if (DelayedDataChangedDateTime != DateTime.MinValue)
                        {
                            if ((DateTime.Now - DelayedDataChangedDateTime).TotalMilliseconds >= DelayedDataChangedPeriod)
                            {
                                DelayedDataChangedDateTime = DateTime.MinValue;
                                DelayedDataChanged?.Invoke(this, new EventArgs());
                            }
                        }
                    }
                }));
            }
            catch (InvalidOperationException)
            {
            }
        }
        #endregion

        #region Function
        public void SetSelection(int selectionStart, int selectionLength)
        {
            Selection.SetSelection(Data, selectionStart, selectionLength);
            this.SetScroll();
            this.SetScrollToCursor();
        }

        public int FindText(string text, int index = 0)
        {
            return Data.GetTextIndex(text, index);
        }

        public void OnDraw()
        {
            this.Draw.Draw();
            VScroll.Draw();
            HScroll.Draw();
        }

        public void ReplaceSelectedText(string text)
        {
            if (Selection.Length > 0)
            {
                // OnKeyDown, Unredo.UpdateUndoStack : For Unredo
                OnKeyDown(new KeyEventArgs(Keys.Delete));
                Data.RemoveSB(ref Selection.Start, ref Selection.Length);
                Unredo.UpdateUndoStack(UnredoNodeType.GroupStart);

                OnKeyDown(new KeyEventArgs(Keys.Control | Keys.V));
                Data.InsertSB(ref Selection.Start, text);
                Unredo.UpdateUndoStack(UnredoNodeType.GroupEnd);
            }
        }

        public void RemoveSelectedText()
        {
            if (Selection.Length > 0)
            {
                // OnKeyDown : For Unredo
                OnKeyDown(new KeyEventArgs(Keys.Delete));
                Data.RemoveSB(ref Selection.Start, ref Selection.Length);
            }
        }

        public void InsertText(int index, string text)
        {
            // OnKeyDown : For Unredo
            Selection.Start = index;
            OnKeyDown(new KeyEventArgs(Keys.Control | Keys.V));
            Data.InsertSB(ref Selection.Start, text);
            IsCtrlDown = false;
        }

        public void RemoveText(int index, int length)
        {
            // OnKeyDown : For Unredo
            Selection.Start = index;
            Selection.Length = length;
            OnKeyDown(new KeyEventArgs(Keys.Delete));
            Data.RemoveSB(ref Selection.Start, length);
            IsCtrlDown = false;
        }

        public void ModifierSB(int index, char character)
        {
            // OnKeyDown : For Unredo
            Selection.Start = index;
            OnKeyDown(new KeyEventArgs(Keys.Control | Keys.V));
            Data.ModifierSB(ref Selection.Start, character);
            IsCtrlDown = false;
        }

        private StringBuilder GetRevisedTextRNT(string text)
        {
            var rsSB = new StringBuilder(text);
            for (int i = rsSB.Length - 1; 0 <= i; i--)
            {
                if (rsSB[i] == '\r' && (i == rsSB.Length - 1 || rsSB[i + 1] != '\n'))
                {
                    rsSB.Insert(i + 1, '\n');
                }
                else if (rsSB[i] == '\n' && (i == 0 || rsSB[i - 1] != '\r'))
                {
                    rsSB.Insert(i, '\r');
                }
                else if (rsSB[i] == '\t')
                {
                    rsSB[i] = ' ';
                    rsSB.Insert(i, new char[3] { ' ', ' ', ' ' });
                }
            }
            return rsSB;
        }

        public int GetFirstCharIndexFromIndex(int index)
        {
            return Data.GetFirstCharIndexFromIndex(index);
        }

        public int GetFirstCharIndexFromLine(int index)
        {
            return Data.GetFirstCharIndexFromLine(index);
        }
        
        public bool ContainsByLineStyles(int charIndex)
        {
            bool rs = false;

            int lineIndex = Data.GetLineFromCharIndex(charIndex);
            foreach(KeyValuePair<string, UserEditorLineStyleInfo> pair in LineStyles)
            {
                if (pair.Value.Lines.Contains(lineIndex))
                {
                    rs = true;
                    break;
                }
            }

            return rs;
        }

        /// <summary>
        /// 현재 커서의 위치를 마지막으로 하는 단어를 반환합니다.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentWord()
        {
            StringBuilder sb = new StringBuilder();

            for(int i = SelectionStart - 1; i >= 0; i--)
            {
                Regex regex = new Regex(@"\w");
                string character = Data.SB[i].ToString();
                if (regex.IsMatch(character))
                {
                    sb.Insert(0, character);
                }
                else
                {
                    break;
                }
            }

            return sb.ToString();
        }
        #endregion

        #region WndProc
        const int WM_IME_SETCONTEXT = 0x0281;
        const int WM_IME_CHAR = 0x0286;
        const int WM_IME_STARTCOMPOSITION = 0x10D;
        const int WM_IME_COMPOSITION = 0x10F;
        const int WM_IME_ENDCOMPOSITION = 0x10E;
        const int WM_IME_NOTIFY = 0x0282;
        const int WM_CHAR = 0x0102;
        const int WM_KEYDOWN = 0x0100;

        bool imeStarting = false;
        bool imeOn = false;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_IME_SETCONTEXT:
                    if (m.WParam.ToInt32() != 0)
                    {
                        bool rc = IMM32.ImmAssociateContextEx(this.Handle, IntPtr.Zero, IMM32.ImmAssociateContextExFlags.IACE_DEFAULT);
                        if (rc)
                        {
                            DefWndProc(ref m);
                        }
                        else
                        {
                            base.WndProc(ref m);
                        }
                    }
                    else
                    {
                        base.WndProc(ref m);
                    }
                    break;

                case WM_IME_STARTCOMPOSITION:
                    imeStarting = true;
                    imeOn = true;
                    break;

                case WM_IME_ENDCOMPOSITION:
                    imeOn = false;
                    break;

                case WM_IME_CHAR: // Completed
                    KeyImeChar(this.Handle, m.WParam.ToInt32(), m.LParam.ToInt32(), imeOn, imeStarting, true);
                    imeStarting = false;
                    Draw.Draw(); // ?
                    break;

                case WM_IME_COMPOSITION: // Composion
                    KeyImeChar(this.Handle, m.WParam.ToInt32(), m.LParam.ToInt32(), imeOn, imeStarting, false);
                    imeStarting = false;
                    base.WndProc(ref m);
                    Draw.Draw(); // ?
                    break;

                case WM_IME_NOTIFY:
                    // 한자 입력
                    break;

                case WM_CHAR:
                    int keyChar = m.WParam.ToInt32();

                    bool executed = AutoComplete_WndProc_WM_CHAR(keyChar);
                    if (!executed)
                    {
                        switch (keyChar)
                        {
                            case 1: this.KeyAllSelect(); break;
                            case 2: break;
                            case 3: this.KeyCopy(); break;
                            case 4: break;
                            case 5: break;
                            case 6: break;
                            case 7: break;
                            case 8: this.KeyBaskspace(); break;
                            case 9: this.KeyTab(); break;
                            case 10: break;
                            case 11: break;
                            case 12: break;
                            case 13: this.KeyEnter(); break;
                            case 14: break;
                            case 15: break;
                            case 16: break;
                            case 17: break;
                            case 18: break;
                            case 19: break;
                            case 20: break;
                            case 21: break;
                            case 22: this.KeyPaste(); break;
                            case 23: break;
                            case 24: this.KeyCut(); break;
                            case 25: break;
                            case 26: break;
                            case 27: this.KeyEsc(); break;
                            default:
                                if (keyChar == 32 && ModifierKeys.HasFlag(Keys.Control))
                                {
                                    this.KeyControlSpace();
                                }
                                else
                                {
                                    this.KeyCharacter(keyChar);
                                }
                                break;
                        }
                    }
                    base.WndProc(ref m);
                    Draw.Draw(); // ?
                    break;

                case WM_KEYDOWN:
                    int oldSelectionStart = Selection.Start;
                    int oldSelectionLength = Selection.Length;
                    int oldVScrollValue = VScroll.Value;
                    int oldHScrollValue = HScroll.Value;
                    this.ThisAfterKeyDown();

                    var keyCode = m.WParam.ToInt32();
                    switch (keyCode)
                    {
                        case 45:
                            if (IsShiftDown)
                            {
                                this.KeyPaste();
                            }
                            break;
                        case 46:
                            if (IsShiftDown)
                            {
                                this.KeyCut();
                            }
                            else
                            {
                                this.KeyDelete();
                            }
                            break;
                        case 89: // Redo
                        case 90: // Undo 
                            if (!IsCtrlDown)
                            {
                                base.WndProc(ref m);
                            }
                            break;
                        default:
                            base.WndProc(ref m);
                            break;
                    }
                    
                    if (oldSelectionStart != Selection.Start || oldSelectionLength != Selection.Length)
                    {
                        this.ThisSelectionChanged();
                        if (oldVScrollValue == VScroll.Value && oldHScrollValue == HScroll.Value)
                        {
                            Draw.Draw();
                        }
                    }
                    else
                    {
                        Draw.Draw();
                    }
                    break;
                    
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        public void KeyImeChar(IntPtr hWnd, int WParam, int LParam, bool imeStarted, bool imeStarting, bool completed)
        {
            char aChar = (char)(WParam);
            if (LParam == 1 && imeStarted || imeStarting)
            {
                if (Selection.Length > 0)
                {
                    KeyDelete();
                }

                if (completed)
                {
                    Unredo.UpdateUndoStack(aChar.ToString());
                }
                Data.InsertSB(ref Selection.Start, aChar);
            }
            else
            {
                if (aChar == 27)
                {
                    this.KeyBaskspace();
                }
                else
                {
                    Data.ModifierSB(Selection.Start, aChar, -1, false);
                    DataChanged?.Invoke(this, new DataEventArgs(SBChangedType.Modifier, SelectionStart, 1));
                    DelayedDataChangedDateTime = DateTime.Now;
                    if (completed)
                    {
                        Unredo.UpdateUndoStack(aChar.ToString());
                    }
                }
            }
            Draw.DrawCursor = true;
        }

        private void KeyControlSpace()
        {
            if (!ReadOnly)
            {
                ShowAutoComplete();
            }
        }

        private void KeyCharacter(int keyChar)
        {
            if (!ReadOnly)
            {
                if (Selection.Length > 0)
                {
                    this.KeyDelete(false);
                }

                if (IsKeyBlockedByRangeStyles(Selection.Start, 1))
                {
                    return;
                }

                if (IsKeyBlockedByLineStyles(Selection.Start))
                {
                    return;
                }

                Data.InsertSB(ref Selection.Start, (char)keyChar);
                Draw.DrawCursor = true;
            }
        }

        private void KeyAllSelect()
        {
            Selection.IndexReference = 0;
            Selection.SetSelection(Data, 0, Data.TextLength);
        }

        private void KeyCopy()
        {
            if (Selection.Length > 0)
            {
                Clipboard.SetText(Data.GetText(Selection.Start, Selection.Length));
            }
        }

        private void KeyBaskspace()
        {
            if (!ReadOnly)
            {
                if (Selection.Length > 0)
                {
                    this.KeyDelete();
                }
                else if (0 < Data.SB.Length && 0 < Selection.Start)
                {
                    if (Selection.Start >= 2 && Data.GetText(Selection.Start - 2, 2) == "\r\n")
                    {
                        if (IsKeyBlockedDeleteLineByLineStyles(Selection.Start))
                        {
                            Unredo.PopUndoStack();
                            return;
                        }

                        Data.RemoveSB(ref Selection.Start, 2, -2);
                    }
                    else
                    {
                        if (IsKeyBlockedByRangeStyles(Selection.Start, 1, 1))
                        {
                            Unredo.PopUndoStack();
                            return;
                        }

                        if (IsKeyBlockedByLineStyles(Selection.Start))
                        {
                            Unredo.PopUndoStack();
                            return;
                        }

                        Data.RemoveSB(ref Selection.Start, 1, -1);
                    }
                }
                Draw.DrawCursor = true;
            }
        }

        private void KeyTab(bool shift = false)
        {
            if (!ReadOnly)
            {
                if (!shift)
                {
                    int selectedLineCount = Data.GetFullLineCount(Selection.Start, Selection.Length);
                    if (Selection.Length > 0 && (selectedLineCount > 1 || Selection.Length == Data.GetLineLengthFromCharIndex(Selection.Start)))
                    {
                        int startIndex = (Selection.Start + Selection.IndexReference - Math.Abs(Selection.Start - Selection.IndexReference)) / 2;
                        int startLine = Data.GetLineFromCharIndex(startIndex);

                        Selection.Start = Data.GetFirstCharIndexFromLine(startLine);
                        for (int i = selectedLineCount - 1; 0 <= i; i--)
                        {
                            Data.InsertSB(Data.GetFirstCharIndexFromLine(startLine + i), new string(' ', TabSpaceCount), true);
                        }
                        Selection.Length = Data.GetFirstCharIndexFromLine(startLine + selectedLineCount - 1) + Data.GetLineLength(startLine + selectedLineCount - 1) - Selection.Start;
                    }
                    else
                    {
                        if (Selection.Length > 0)
                        {
                            this.KeyDelete(false);
                        }
                        Data.InsertSB(ref Selection.Start, new string(' ', TabSpaceCount));
                    }
                }
                else
                {
                    if (Selection.Length == 0)
                    {
                        int toBeDeleteSpaceCount = 0;
                        for (int i = 1; i <= 4; i++)
                        {
                            if (Selection.Start - i >= 0 && Data.SB[Selection.Start - i] == ' ')
                            {
                                toBeDeleteSpaceCount++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (toBeDeleteSpaceCount > 0)
                        {
                            int _toBeDeleteSpaceCount = toBeDeleteSpaceCount;
                            Data.RemoveSB(Selection.Start - toBeDeleteSpaceCount, ref toBeDeleteSpaceCount, 0, true);
                            Selection.Start -= _toBeDeleteSpaceCount;
                        }
                    }
                    else
                    {
                        int selectedLineCount = Data.GetFullLineCount(Selection.Start, Selection.Length);
                        selectedLineCount = selectedLineCount == 0 ? 1 : selectedLineCount;

                        int startIndex = Selection.Length == 0 ? Selection.Start
                            : (Selection.Start + Selection.IndexReference - Math.Abs(Selection.Start - Selection.IndexReference)) / 2;

                        int startLine = Data.GetLineFromCharIndex(startIndex);

                        Selection.Start = Data.GetFirstCharIndexFromLine(startLine);
                        for (int i = selectedLineCount - 1; 0 <= i; i--)
                        {
                            int targetLineStartIndex = Data.GetFirstCharIndexFromLine(startLine + i);
                            int removeSpaceCount = Data.GetLeftSpaceCount(targetLineStartIndex, TabSpaceCount);
                            if (removeSpaceCount > 0)
                            {
                                Selection.Length = removeSpaceCount;
                                Data.RemoveSB(targetLineStartIndex, ref Selection.Length, 0, true);
                            }
                        }
                        Selection.Length = Data.GetFirstCharIndexFromLine(startLine + selectedLineCount - 1) + Data.GetLineLength(startLine + selectedLineCount - 1) - Selection.Start;
                    }

                }
                Draw.DrawCursor = true;
            }
        }

        private void KeyEnter()
        {
            if (!ReadOnly)
            {
                if (Selection.Length > 0)
                {
                    this.KeyDelete(false);
                }

                if (IsKeyBlockedAddLineByLineStyles(Selection.Start))
                {
                    return;
                }

                Data.InsertSB(ref Selection.Start, "\r\n");
                Draw.DrawCursor = true;
            }
        }

        private void KeyPaste()
        {
            if (!ReadOnly)
            {
                if (Selection.Length > 0)
                {
                    this.KeyDelete(false);
                }

                string text = GetRevisedTextRNT(Clipboard.GetText()).ToString();

                if (IsKeyBlockedByRangeStyles(Selection.Start, 1))
                {
                    return;
                }

                if (IsKeyBlockedByLineStyles(Selection.Start))
                {
                    return;
                }

                if (text.IndexOf("\r\n") >= 0)
                {
                    if (IsKeyBlockedAddLineByLineStyles(Selection.Start))
                    {
                        text = text.Replace("\r\n", "    ");
                    }
                }

                Data.InsertSB(ref Selection.Start, text);
                Draw.DrawCursor = true;
            }
        }

        private void KeyCut()
        {
            if (!ReadOnly)
            {
                if (Selection.Length > 0)
                {
                    Clipboard.SetText(Data.GetText(Selection.Start, Selection.Length));
                    this.KeyDelete(false);
                }
            }
        }

        private void KeyEsc()
        {
            if (Selection.Length > 0)
            {
                if (Selection.Start >= Selection.IndexReference)
                {
                    Selection.SetSelection(Data, Selection.Start + Selection.Length);
                }
                else
                {
                    Selection.Length = 0;
                }
            }
        }

        private void KeyDelete(bool onlyDelete = true)
        {
            if (!ReadOnly)
            {
                if (Selection.Length > 0)
                {
                    // line 처리
                    // [90]char 처리
                    bool hasBlock = false;
                    List<Range> deletableRanges = GetDeletableRange(Selection.Start, Selection.Length, ref hasBlock);
                    if (deletableRanges.Count > 0)
                    {
                        if (hasBlock)
                        {
                            if (onlyDelete)
                            {
                                Unredo.PopUndoStack();
                            }

                            int oldSelectionStart = Selection.Start;
                            Selection.Length = 0;
                            for (int i = deletableRanges.Count - 1; i >= 0; i--)
                            {
                                int selectionStart = deletableRanges[i].StartingValue;
                                int selectionLength = deletableRanges[i].EndValue - deletableRanges[i].StartingValue + 1;

                                Unredo.PushUndoStack(new UnredoClass.UnredoStackInfo(
                                      selectionStart
                                    , selectionLength
                                    , Data.SB.ToString(selectionStart, selectionLength)
                                    , UnredoClass.ProcessType.Delete
                                    , UnredoNodeType.None)
                                );

                                Unredo.TextChangedEventBolck = true;
                                Data.RemoveSB(ref selectionStart, ref selectionLength);
                                Unredo.TextChangedEventBolck = false;
                            }

                            Selection.Start = oldSelectionStart;
                            Selection.Length = 0;
                            Unredo.UpdateKeyDownData(Selection.Start, "", TextLength);
                        }
                        else
                        {
                            Data.RemoveSB(ref Selection.Start, ref Selection.Length);
                        }
                    }
                    else
                    {
                        if (hasBlock)
                        {
                            if (onlyDelete)
                            {
                                Unredo.PopUndoStack();
                            }
                            else
                            {
                                Selection.Length = 0;
                                Unredo.UpdateKeyDownData(Selection.Start, "", TextLength);
                            }
                        }
                        else
                        {
                            Data.RemoveSB(ref Selection.Start, ref Selection.Length);
                        }
                    }
                }
                else if (0 < Data.SB.Length && Selection.Start < Data.SB.Length)
                {
                    if (Selection.Start <= Data.SB.Length - 2 && Data.SB[Selection.Start] == '\r' && Data.SB[Selection.Start + 1] == '\n')
                    {
                        if (IsKeyBlockedDeleteLineByLineStyles(Selection.Start))
                        {
                            Unredo.PopUndoStack();
                            return;
                        }

                        Data.RemoveSB(ref Selection.Start, 2);
                    }
                    else
                    {
                        if (IsKeyBlockedByRangeStyles(Selection.Start + 1, 1, 1))
                        {
                            Unredo.PopUndoStack();
                            return;
                        }

                        if (IsKeyBlockedByLineStyles(Selection.Start + 1))
                        {
                            Unredo.PopUndoStack();
                            return;
                        }

                        Data.RemoveSB(ref Selection.Start, 1);
                    }
                }
                Draw.DrawCursor = true;
            }
        }
        
        private void ThisAfterKeyDown()
        {
        }

        private bool IsKeyBlockedByRangeStyles(int index, int startValueRevision = 0, int endValueRevision = 0)
        {
            bool rsBlock = false;

            foreach (KeyValuePair<string, UserEditorRangeStyle> pair in RangeStyles)
            {
                if (pair.Value.ReadOnly)
                {
                    foreach (Range range in pair.Value.Ranges)
                    {
                        if (range.StartingValue + startValueRevision <= index && index <= range.EndValue + endValueRevision)
                        {
                            rsBlock = true;
                            break;
                        }
                    }

                    if (rsBlock)
                    {
                        break;
                    }
                }
            }

            return rsBlock;
        }

        private void ReviseRangeStylesValue(int index, int length)
        {
            foreach (KeyValuePair<string, UserEditorRangeStyle> pair in RangeStyles)
            {
                foreach (Range range in pair.Value.Ranges)
                {
                    if (index <= range.StartingValue)
                    {
                        range.Offset(length);
                    }
                }
            }
        }

        private bool IsKeyBlockedByLineStyles(int index)
        {
            bool rsBlock = false;
            int lineIndex = Data.GetLineFromCharIndex(index);

            foreach (KeyValuePair<string, UserEditorLineStyleInfo> pair in LineStyles)
            {
                if (pair.Value.FixedLine)
                {
                    if (pair.Value.ReadOnlyLine)
                    {
                        for (int i = 0; i < pair.Value.Lines.Count; i++)
                        {
                            if (lineIndex == pair.Value.Lines[i])
                            {
                                rsBlock = true;
                                break;
                            }
                        }
                    }

                    if (rsBlock)
                    {
                        break;
                    }
                }
            }

            return rsBlock;
        }

        private bool IsKeyBlockedAddLineByLineStyles(int index)
        {
            bool rsBlock = false;
            int lineIndex = Data.GetLineFromCharIndex(index);
            int firstCharIndexInLine = Data.GetFirstCharIndexFromIndex(index);

            foreach (KeyValuePair<string, UserEditorLineStyleInfo> pair in LineStyles)
            {
                if (pair.Value.FixedLine)
                {
                    for (int i = 0; i < pair.Value.Lines.Count; i++)
                    {
                        if (lineIndex == pair.Value.Lines[i])
                        {
                            var lineLength = Data.GetLineLength(lineIndex);
                            // Is not the index of the first character in the first line of the block
                            // And is not the index of the last character in the last line of the block
                            if (!(i == 0 && index == firstCharIndexInLine)
                             && !(i == pair.Value.Lines.Count - 1 && index == firstCharIndexInLine + lineLength))
                            {
                                rsBlock = true;
                            }
                            break;
                        }
                    }

                    if (rsBlock)
                    {
                        break;
                    }
                }
            }

            return rsBlock;
        }

        private bool IsKeyBlockedDeleteLineByLineStyles(int index)
        {
            bool rsBlock = false;
            int lineIndex = Data.GetLineFromCharIndex(index);
            int nextLineIndex = lineIndex + 1;
            int prevLineIndex = lineIndex - 1;
            int firstCharIndexInLine = Data.GetFirstCharIndexFromIndex(index);

            foreach (KeyValuePair<string, UserEditorLineStyleInfo> pair in LineStyles)
            {
                if (pair.Value.FixedLine)
                {
                    for (int i = 0; i < pair.Value.Lines.Count; i++)
                    {
                        int nodeLineIndex = -1;
                        if (pair.Value.Lines[i] == lineIndex)
                        {
                            nodeLineIndex = lineIndex;
                        }
                        else if (pair.Value.Lines[i] == nextLineIndex)
                        {
                            nodeLineIndex = nextLineIndex;
                        }
                        else if (pair.Value.Lines[i] == prevLineIndex && firstCharIndexInLine == index)
                        {
                            nodeLineIndex = prevLineIndex;
                        }

                        if (nodeLineIndex >= 0)
                        {
                            var lineLength = Data.GetLineLength(nodeLineIndex);
                            // When the line above the block is blank 
                            if (i == 0 && (nodeLineIndex > 0 && Data.GetLineLength(nodeLineIndex - 1) == 0))
                            {
                                // None
                            }
                            // When the line below the block is blank 
                            else if (i == pair.Value.Lines.Count - 1
                                 && (nodeLineIndex < Data.LineLength - 1 && Data.GetLineLength(nodeLineIndex + 1) == 0))
                            {
                                // None
                            }
                            // First index in block
                            else if (i == 0 && index == firstCharIndexInLine)
                            {
                                rsBlock = true;
                            }
                            // Last index in block
                            else if (i == pair.Value.Lines.Count - 1
                                 && index == firstCharIndexInLine + lineLength)
                            {
                                rsBlock = true;
                            }
                            else
                            {
                                rsBlock = true;
                            }
                            break;
                        }
                    }

                    if (rsBlock)
                    {
                        break;
                    }
                }
            }

            return rsBlock;
        }

        private bool IsFirstIndexInBlock(int index)
        {
            bool isFirstIndexInBlock = false;
            bool isLastIndexInBlock = false;
            IsFirstNLastIndexInBlockProc(index, out isFirstIndexInBlock, out isLastIndexInBlock);
            return isFirstIndexInBlock;
        }

        private bool IsLastIndexInBlock(int index)
        {
            bool isFirstIndexInBlock = false;
            bool isLastIndexInBlock = false;
            IsFirstNLastIndexInBlockProc(index, out isFirstIndexInBlock, out isLastIndexInBlock);
            return isLastIndexInBlock;
        }

        private bool IsFirstNLastIndexInBlockProc(int index, out bool isFirstIndexInBlock, out bool isLastIndexInBlock)
        {
            isFirstIndexInBlock = false;
            isLastIndexInBlock = false;

            bool rsBlock = false;
            int lineIndex = Data.GetLineFromCharIndex(index);

            foreach (KeyValuePair<string, UserEditorLineStyleInfo> pair in LineStyles)
            {
                if (pair.Value.FixedLine)
                {
                    for (int i = 0; i < pair.Value.Lines.Count; i++)
                    {
                        if (lineIndex == pair.Value.Lines[i])
                        {
                            if (i == 0)
                            {
                                rsBlock = true;
                                isFirstIndexInBlock = true;
                            }
                            else if (i == pair.Value.Lines.Count - 1)
                            {
                                rsBlock = true;
                                isLastIndexInBlock = true;
                            }
                            else
                            {
                                rsBlock = true;
                            }
                            break;
                        }
                    }

                    if (rsBlock)
                    {
                        break;
                    }
                }
            }

            return rsBlock;
        }

        private void ReviseLineStylesValue(int charIndex, int lineCount)
        {
            int lineIndex = Data.GetLineFromCharIndex(charIndex);
            int firstCharIndexInLine = Data.GetFirstCharIndexFromLine(lineIndex);
            int lastCharIndexInLine = firstCharIndexInLine + Data.GetLineLength(lineIndex);

            foreach (KeyValuePair<string, UserEditorLineStyleInfo> pair in LineStyles)
            {
                for (int i = 0; i < pair.Value.Lines.Count; i++)
                {
                    // When the index of the first character in the first line of the block
                    if (lastCharIndexInLine == charIndex && firstCharIndexInLine != charIndex)
                    {
                        if (lineIndex < pair.Value.Lines[i])
                        {
                            pair.Value.Lines[i] += lineCount;
                        }
                    }
                    else
                    {
                        if (lineIndex <= pair.Value.Lines[i])
                        {
                            pair.Value.Lines[i] += lineCount;
                        }
                    }
                }
            }
        }

        private List<Range> GetDeletableRange(int selectionStart, int selectionLength, ref bool hasBlock)
        {
            int startingIndex = selectionStart;
            int endIndex = selectionStart + selectionLength - 1;
            bool[] boolsReadonly = new bool[selectionLength];

            // By RangeStyles
            foreach (KeyValuePair<string, UserEditorRangeStyle> pair in RangeStyles)
            {
                if (pair.Value.ReadOnly)
                {
                    foreach (Range range in pair.Value.Ranges)
                    {
                        if (range.StartingValue <= startingIndex && startingIndex <= range.EndValue)
                        {
                            int iMax = Math.Min(endIndex, range.EndValue);
                            for (int i = startingIndex; i <= iMax; i++)
                            {
                                boolsReadonly[i - startingIndex] = true;
                            }
                            hasBlock = true;
                        }
                        else if (range.StartingValue <= endIndex && endIndex <= range.EndValue)
                        {
                            for (int i = range.StartingValue; i <= endIndex; i++)
                            {
                                boolsReadonly[i - startingIndex] = true;
                            }
                            hasBlock = true;
                        }
                        else if (startingIndex <= range.StartingValue && range.EndValue <= endIndex)
                        {
                            for (int i = range.StartingValue; i <= range.EndValue; i++)
                            {
                                boolsReadonly[i - startingIndex] = true;
                            }
                            hasBlock = true;
                        }
                    }
                }
            }

            // By LineStyles
            List<int> readOnlyLines = new List<int>();
            List<int> fixedLines = new List<int>();
            GetRefFixedLinesByLineStyles(ref readOnlyLines, ref fixedLines);
            int startLineIndex = Data.GetLineFromCharIndex(selectionStart);
            int endLineIndex = startLineIndex + Data.SB.ToString(startLineIndex, SelectionLength).GetContainsCount("\r\n");
            int lastLineIndex = Data.LineLength;
            for (int i = startLineIndex; i <= endLineIndex; i++)
            {
                if (readOnlyLines.Contains(i))
                {
                    int lineStartIndex = Data.GetFirstCharIndexFromLine(i);
                    int lineEndIndex = Data.GetFirstCharIndexFromLine(i + 1);
                    lineEndIndex = lineEndIndex == -1 ? Data.SB.Length - 1 : lineEndIndex;
                    for (int k = lineStartIndex; k < lineEndIndex; k++)
                    {
                        int nodeIndex = k - startingIndex;
                        if (0 <= nodeIndex && nodeIndex < selectionLength)
                        {
                            boolsReadonly[nodeIndex] = true;
                            hasBlock = true;
                        }
                    }
                }
                else if (fixedLines.Contains(i))
                {
                    if (i < lastLineIndex)
                    {
                        int enterStartIndex = Data.GetFirstCharIndexFromLine(i + 1) - 2;
                        boolsReadonly[enterStartIndex - startingIndex] = true;
                        boolsReadonly[enterStartIndex - startingIndex + 1] = true;
                        hasBlock = true;
                    }
                }
            }

            // Set rsRanges
            List<Range> rsRanges = new List<Range>();
            if (hasBlock)
            {
                int startValue = -1;
                for(int i = 0; i < boolsReadonly.Length; i++)
                {
                    if (boolsReadonly[i])
                    {
                        if (startValue != -1)
                        {
                            rsRanges.Add(new Range(selectionStart + startValue, selectionStart + i - 1));
                            startValue = -1;
                        }
                    }
                    else
                    {
                        if (startValue == -1)
                        {
                            startValue = i;
                        }
                    }
                }
                if (startValue != -1)
                {
                    rsRanges.Add(new Range(selectionStart + startValue, selectionStart + selectionLength - 1));
                }
            }
            else
            {
                rsRanges.Add(new Range(selectionStart, selectionStart + selectionLength - 1));
            }

            return rsRanges;
        }

        private void GetRefFixedLinesByLineStyles(ref List<int> readOnlyLines, ref List<int> fixedLines)
        {
            foreach(KeyValuePair<string, UserEditorLineStyleInfo> pair in LineStyles)
            {
                if (pair.Value.ReadOnlyLine || pair.Value.FixedLine)
                {
                    foreach(int lineIndex in pair.Value.Lines)
                    {
                        if (pair.Value.ReadOnlyLine && !readOnlyLines.Contains(lineIndex))
                        {
                            readOnlyLines.Add(lineIndex);
                        }

                        if (pair.Value.FixedLine && !fixedLines.Contains(lineIndex))
                        {
                            fixedLines.Add(lineIndex);
                        }
                    }
                }
            }
        }
        #endregion
    }
}