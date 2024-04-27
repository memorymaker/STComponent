using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ST.Controls
{
    public partial class UserEditor
    {
        private class SelectionClass
        {
            #region Option Cursor
            public int Color1 = ColorTranslator.ToWin32(Color.Red);
            public int Color2 = ColorTranslator.ToWin32(Color.Blue);
            public bool ShowNotFocused = false;
            public SelectionShowingTypeOption ShowingType = SelectionShowingTypeOption.VisableChaging;
            public int ShowingPeriod = 200;
            #endregion

            #region Controller
            public int Start = 0;
            public int Length = 0;
            #endregion

            #region Event
            public event EventHandler<SelectionEventArgs> OnSelectionChanged;
            #endregion

            #region Reference(Draw)
            public int LeftRivision = 0;
            public int TopRivision = 0;
            public int HeightRevision = 0;
            #endregion

            #region Reference(This)
            public int IndexReference = -1;
            public int LeftReference = -1;
            public string ReturnText = "\r\n";
            public string Splitter = "`~!@#$%^&*()-+=[]{}\\|;:'\",.<>/?\t\r\n ";
            #endregion

            #region Load
            public SelectionClass()
            {
            }
            #endregion

            #region Proc Key
            public bool Proc(DataClass data, Keys keyCode, bool ctrl, bool alt, bool shift, Font font, int pageSize, ref int index, ref int length)
            {
                int inputIndex = index, inputlength = length;
                int targetIndex = this.GetTargetIndex(data, keyCode, index, length, font, this.IndexReference);
                int newIndex = this.GetNewIndex(data, keyCode, targetIndex, index, length, ctrl, alt, shift, pageSize, font, this.LeftReference, this.ReturnText, this.Splitter);
                this.SetNewIndexNLength(keyCode, newIndex, shift, this.IndexReference, ref index, ref length);
                return inputIndex != index || inputlength != length;
            }

            private int GetTargetIndex(DataClass data, Keys keyCode, int index, int length, Font font, int indexReference)
            {
                int rs = 0;
                switch (keyCode)
                {
                    case Keys.Up:
                    case Keys.Down:
                    case Keys.PageUp:
                    case Keys.PageDown:
                        rs = (length != 0 && index >= indexReference) ? index + length : index;
                        this.SetLeftReference(data, rs, font, this.ReturnText);
                        break;
                    case Keys.Left:
                    case Keys.Right:
                    case Keys.Home:
                    case Keys.End:
                        rs = (length != 0 && index >= indexReference) ? index + length : index;
                        break;
                }
                return rs;
            }

            private int GetNewIndex(DataClass data, Keys keyCode, int targetIndex, int index, int length, bool ctrl, bool alt, bool shift, int pageSize, Font font, int leftReference, string returnText, string splitter)
            {
                int rs = 0;
                switch (keyCode)
                {
                    case Keys.Up:
                        rs = this.GetIndexFromLineToBeMoved(data, targetIndex, -1, font, returnText, leftReference);
                        break;
                    case Keys.Down:
                        rs = this.GetIndexFromLineToBeMoved(data, targetIndex, 1, font, returnText, leftReference);
                        break;
                    case Keys.Left:
                        if (length > 0 && !shift && !ctrl)
                        {
                            rs = index;
                        }
                        else if (ctrl)
                        {
                            rs = this.GetFirstIndexOfTextBlock(data.SB, targetIndex - 1, SelectionProcType.Key, returnText, splitter);
                        }
                        else
                        {
                            if (this.IsThisText(data.SB, targetIndex - 2, returnText))
                            {
                                rs = targetIndex - 2;
                            }
                            else
                            {
                                rs = targetIndex - 1;
                            }
                        }

                        if (rs < 0)
                        {
                            rs = 0;
                        }
                        break;
                    case Keys.Right:
                        if (length > 0 && !shift && !ctrl)
                        {
                            rs = index + length;
                        }
                        else if (ctrl)
                        {
                            rs = this.GetLastIndexOfTextBlock(data.SB, targetIndex, SelectionProcType.Key, returnText, splitter);
                        }
                        else
                        {
                            if (this.IsThisText(data.SB, targetIndex, returnText))
                            {
                                rs = targetIndex + 2;
                            }
                            else
                            {
                                rs = targetIndex + 1;
                            }
                        }
                        break;
                    case Keys.PageUp:
                        rs = this.GetIndexFromLineToBeMoved(data, targetIndex, pageSize * -1, font, returnText, leftReference);
                        break;
                    case Keys.PageDown:
                        rs = this.GetIndexFromLineToBeMoved(data, targetIndex, pageSize, font, returnText, leftReference);
                        break;
                    case Keys.Home:
                        if (ctrl)
                        {
                            rs = 0;
                        }
                        else
                        {
                            rs = data.GetFirstCharIndexFromIndex(targetIndex);
                            if (rs == targetIndex)
                            {
                                rs = data.SB.LastIndexOf(returnText, targetIndex);
                                if (rs < 0)
                                {
                                    rs = 0;
                                }
                                else
                                {
                                    rs += returnText.Length;
                                }
                            }
                        }
                        break;
                    case Keys.End:
                        if (ctrl)
                        {
                            rs = data.TextLength;
                        }
                        else
                        {
                            int lineNumber = data.GetLineFromCharIndex(targetIndex);
                            rs = data.IsWordBreakLine(lineNumber)
                                ? data.GetLastCharIndexFromLine(lineNumber) - 1
                                : data.GetLastCharIndexFromLine(lineNumber);
                            if (rs == targetIndex)
                            {
                                rs = data.SB.IndexOf(returnText, targetIndex);
                                if (rs < 0)
                                {
                                    rs = data.TextLength;
                                }
                            }
                        }
                        break;
                }
                return rs;
            }

            private void SetNewIndexNLength(Keys keyCode, int newIndex, bool shift, int indexReference, ref int index, ref int length)
            {
                switch (keyCode)
                {
                    case Keys.Up:
                    case Keys.Down:
                    case Keys.Left:
                    case Keys.Right:
                    case Keys.PageUp:
                    case Keys.PageDown:
                    case Keys.Home:
                    case Keys.End:
                        if (shift)
                        {
                            index = (this.IndexReference + newIndex - Math.Abs(this.IndexReference - newIndex)) / 2;
                            length = Math.Abs(newIndex - this.IndexReference);
                        }
                        else
                        {
                            index = newIndex;
                            length = 0;
                        }
                        break;
                }
            }
            #endregion

            #region Proc Mouse
            public bool Proc(DataClass data, SelectionMouseActionType mouseActionType, int x, int y, bool ctrl, bool alt, bool shift, Font font, ref int index, ref int length)
            {
                int inputIndex = index, inputlength = length;
                int targetIndex = index;
                int newIndex = this.GetNewIndex(data, mouseActionType, x, y, ctrl, alt, shift, font, this.IndexReference, this.ReturnText);
                this.SetNewIndexNLength(data.SB, mouseActionType, newIndex, ctrl, alt, shift, font, this.IndexReference, ref index, ref length);
                return inputIndex != index || inputlength != length;
            }

            private int GetNewIndex(DataClass data, SelectionMouseActionType mouseActionType, int x, int y, bool ctrl, bool alt, bool shift, Font font, int indexReference, string returnText)
            {
                int rs = this.GetTextIndex(data, x, y, font, this.ReturnText);
                if (ctrl || mouseActionType == SelectionMouseActionType.DoubleDown)
                {
                    rs = (shift || mouseActionType == SelectionMouseActionType.Move) && IndexReference <= rs
                        ? this.GetLastIndexOfTextBlock(data.SB, rs, SelectionProcType.Mouse)
                        : this.GetFirstIndexOfTextBlock(data.SB, rs, SelectionProcType.Mouse);
                }
                return rs;
            }

            private void SetNewIndexNLength(string text, SelectionMouseActionType mouseActionType, int newIndex, bool ctrl, bool alt, bool shift, Font font, int indexReference, ref int index, ref int length)
            {
                switch (mouseActionType)
                {
                    case SelectionMouseActionType.Down:
                        if (shift)
                        {
                            index = (this.IndexReference + newIndex - Math.Abs(this.IndexReference - newIndex)) / 2;
                            length = Math.Abs(newIndex - this.IndexReference);
                        }
                        else if (!shift && ctrl)
                        {
                            index = newIndex;
                            length = this.GetLastIndexOfTextBlock(text, newIndex, SelectionProcType.Mouse) - index;
                            this.IndexReference = newIndex;
                        }
                        else
                        {
                            index = newIndex;
                            length = 0;
                            this.IndexReference = newIndex;
                        }
                        break;
                    case SelectionMouseActionType.DoubleDown:
                        if (shift)
                        {
                            index = (this.IndexReference + newIndex - Math.Abs(this.IndexReference - newIndex)) / 2;
                            length = Math.Abs(newIndex - this.IndexReference);
                        }
                        else
                        {
                            index = newIndex;
                            length = this.GetLastIndexOfTextBlock(text, newIndex, SelectionProcType.Mouse) - index;
                            this.IndexReference = newIndex;
                        }
                        break;
                    case SelectionMouseActionType.Move:
                        index = (this.IndexReference + newIndex - Math.Abs(this.IndexReference - newIndex)) / 2;
                        length = Math.Abs(newIndex - this.IndexReference);
                        break;
                }

                this.ClearLeftReference();
            }

            private void SetNewIndexNLength(StringBuilder sb, SelectionMouseActionType mouseActionType, int newIndex, bool ctrl, bool alt, bool shift, Font font, int indexReference, ref int index, ref int length)
            {
                switch (mouseActionType)
                {
                    case SelectionMouseActionType.Down:
                        if (shift)
                        {
                            index = (this.IndexReference + newIndex - Math.Abs(this.IndexReference - newIndex)) / 2;
                            length = Math.Abs(newIndex - this.IndexReference);
                        }
                        else if (!shift && ctrl)
                        {
                            index = newIndex;
                            length = this.GetLastIndexOfTextBlock(sb, newIndex, SelectionProcType.Mouse) - index;
                            this.IndexReference = newIndex;
                        }
                        else
                        {
                            index = newIndex;
                            length = 0;
                            this.IndexReference = newIndex;
                        }
                        break;
                    case SelectionMouseActionType.DoubleDown:
                        if (shift)
                        {
                            index = (this.IndexReference + newIndex - Math.Abs(this.IndexReference - newIndex)) / 2;
                            length = Math.Abs(newIndex - this.IndexReference);
                        }
                        else
                        {
                            index = newIndex;
                            length = this.GetLastIndexOfTextBlock(sb, newIndex, SelectionProcType.Mouse) - index;
                            this.IndexReference = newIndex;
                        }
                        break;
                    case SelectionMouseActionType.Move:
                        if (0 <= this.IndexReference)
                        {
                            index = (this.IndexReference + newIndex - Math.Abs(this.IndexReference - newIndex)) / 2;
                            length = Math.Abs(newIndex - this.IndexReference);
                        }
                        else
                        {
                            index = newIndex;
                            length = 0;
                        }
                        break;
                }

                this.ClearLeftReference();
            }

            private int GetTextIndex(DataClass data, int x, int y, Font font, string returnText)
            {
                x = x < 0 ? 0 : x;
                y = y < 0 ? 0 : y;
                int rs = 0;

                if (data.SB.Length > 0)
                {
                    using (var tempImage = GetTemporaryImage())
                    {
                        // hdc Proc #1 Start
                        var g = GetTemporaryGraphics(tempImage);
                        IntPtr hdc = g.GetHdc();

                        IntPtr fontPtr = font.ToHfont();
                        GDI32.SelectObject(hdc, fontPtr);

                        int fontHeight = this.GetFontHeight(hdc);

                        int line = y / fontHeight;
                        int lineStartIndex = data.GetFirstCharIndexFromLine(line);
                        if (lineStartIndex == -1)
                        {
                            line = data.LineLength - 1;
                            lineStartIndex = data.GetFirstCharIndexFromLine(line);
                        }

                        int lineLastIndex = data.GetLastCharIndexFromLine(line);

                        string targetText = data.SB.ToString(lineStartIndex, lineLastIndex - lineStartIndex);
                        rs = lineStartIndex + this.GetTextindex(targetText, x, hdc);

                        GDI32.DeleteObject(fontPtr);

                        // hdc Proc #2 End
                        g.ReleaseHdc(hdc);
                        g.Dispose();
                    }
                }
                return rs;
            }

            private int GetFontHeight(IntPtr hdc)
            {
                Size textPixelSize;
                GDI32.GetTextExtentPoint32(hdc, " ", 1, out textPixelSize);
                return textPixelSize.Height;
            }
            #endregion

            #region Function Process
            private int GetIndexFromLineToBeMoved(DataClass data, int index, int lineIncrement, Font font, string returnText, int leftReference)
            {
                int rs;
                string targetText;
                int targetFirstIndex;
                int targetLastIndex;
                if (lineIncrement < 0)
                {
                    int line = data.GetLineFromCharIndex(index);
                    if (line + lineIncrement >= 0)
                    {
                        targetFirstIndex = data.GetFirstCharIndexFromLine(line + lineIncrement);
                        targetLastIndex = data.IsWordBreakLine(line + lineIncrement)
                            ? data.GetLastCharIndexFromLine(line + lineIncrement) - 1
                            : data.GetLastCharIndexFromLine(line + lineIncrement);
                    }
                    else
                    {
                        targetFirstIndex = 0;
                        targetLastIndex = data.IsWordBreakLine(line + lineIncrement)
                            ? data.GetLastCharIndexFromLine(0) - 1
                            : data.GetLastCharIndexFromLine(0);
                    }
                }
                else
                {
                    int line = data.GetLineFromCharIndex(index);
                    if (line + lineIncrement < data.LineLength)
                    {
                        targetFirstIndex = data.GetFirstCharIndexFromLine(line + lineIncrement);
                        targetLastIndex = data.IsWordBreakLine(line + lineIncrement)
                            ? data.GetLastCharIndexFromLine(line + lineIncrement) - 1
                            : data.GetLastCharIndexFromLine(line + lineIncrement);
                    }
                    else
                    {
                        targetFirstIndex = data.GetFirstCharIndexFromLine(data.LineLength - 1);
                        targetLastIndex = data.SB.Length;
                    }
                }
                targetText = data.SB.ToString(targetFirstIndex, targetLastIndex - targetFirstIndex);

                if (data.GetLineFromCharIndex(index) == 0 && lineIncrement < 0)
                {
                    rs = 0;
                }
                else if (data.GetLineFromCharIndex(index) == data.LineLength - 1 && lineIncrement > 0)
                {
                    rs = data.TextLength;
                }
                else
                {
                    rs = targetFirstIndex + this.GetTextindex(targetText, leftReference, font);
                }
                return rs;
            }

            private int GetTextindex(string text, int x, Font font)
            {
                int rs;
                using (var tempImage = GetTemporaryImage())
                {
                    // hdc Proc #1 Start
                    var g = GetTemporaryGraphics(tempImage);
                    IntPtr hdc = g.GetHdc();

                    IntPtr fontInptr = font.ToHfont();
                    GDI32.SelectObject(hdc, fontInptr);

                    rs = this.GetTextindex(text, x, hdc);

                    GDI32.DeleteObject(fontInptr);

                    // hdc Proc #2 End
                    g.ReleaseHdc(hdc);
                    g.Dispose();
                }
                return rs;
            }

            private int GetTextindex(string text, int x, IntPtr hdc)
            {
                int rs = 0;
                int textLehgth = text.Length;

                Size textPixelSize;
                GDI32.GetTextExtentPoint32(hdc, text, textLehgth, out textPixelSize);

                if (textPixelSize.Width < x)
                {
                    rs = textLehgth;
                }
                else
                {
                    int lineTextIndex = textPixelSize.Width == 0 ? 0 : Convert.ToInt32(textLehgth * (Convert.ToDouble(x) / textPixelSize.Width));
                    while (lineTextIndex < textLehgth - 1)
                    {
                        Size textPixelSizeFirst, textPixelSizeSecond;
                        GDI32.GetTextExtentPoint32(hdc, text.Substring(0, lineTextIndex), lineTextIndex, out textPixelSizeFirst);
                        GDI32.GetTextExtentPoint32(hdc, text.Substring(0, lineTextIndex + 1), lineTextIndex + 1, out textPixelSizeSecond);
                        if (textPixelSizeFirst.Width <= x && x <= textPixelSizeSecond.Width)
                        {
                            lineTextIndex = x - textPixelSizeFirst.Width <= textPixelSizeSecond.Width - x
                                ? lineTextIndex
                                : lineTextIndex + 1;
                            break;
                        }
                        else if (x < textPixelSizeFirst.Width)
                        {
                            lineTextIndex--;
                        }
                        else if (textPixelSizeSecond.Width < x)
                        {
                            lineTextIndex++;
                        }
                    }

                    rs = lineTextIndex;
                }
                return rs;
            }

            public int GetFirstIndexOfTextBlock(string text, int index, SelectionProcType procType = SelectionProcType.Key, string returnText = null, string splitter = null)
            {
                returnText = returnText ?? this.ReturnText;
                splitter = splitter ?? this.Splitter;

                int rs = 0;
                if (0 <= index && 0 < text.Length)
                {
                    if (text.Length == index
                    || (procType == SelectionProcType.Mouse && this.IsThisText(text, index, returnText)))
                    {
                        index--;
                    }

                    int splitterStartIndex = splitter.IndexOf(text.Substring(index, 1));
                    if (splitter.IndexOf(text.Substring(index, 1)) >= 0)
                    {
                        if (this.IsThisText(text, index - 1, returnText))
                        {
                            rs = index - 1;
                        }
                        else
                        {
                            for (int i = index; 0 < i; i--)
                            {
                                if (splitterStartIndex != splitter.IndexOf(text.Substring(i - 1, 1)))
                                {
                                    rs = i;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = index; i >= rs; i--)
                        {
                            if (splitter.IndexOf(text.Substring(i, 1)) >= 0)
                            {
                                rs = i + 1;
                                break;
                            }
                        }
                    }
                }
                return rs;
            }

            public int GetFirstIndexOfTextBlock(StringBuilder sb, int index, SelectionProcType procType = SelectionProcType.Key, string returnText = null, string splitter = null)
            {
                returnText = returnText ?? this.ReturnText;
                splitter = splitter ?? this.Splitter;

                int rs = 0;
                if (0 <= index && 0 < sb.Length)
                {
                    if (sb.Length == index
                    || (procType == SelectionProcType.Mouse && this.IsThisText(sb, index, returnText)))
                    {
                        index--;
                    }

                    int splitterStartIndex = splitter.IndexOf(sb.ToString(index, 1));
                    if (splitterStartIndex >= 0)
                    {
                        if (this.IsThisText(sb, index - 1, returnText))
                        {
                            rs = index - 1;
                        }
                        else
                        {
                            for (int i = index; 0 < i; i--)
                            {
                                if (splitterStartIndex != splitter.IndexOf(sb.ToString(i - 1, 1)))
                                {
                                    rs = i;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = index; i >= rs; i--)
                        {
                            if (splitter.IndexOf(sb.ToString(i, 1)) >= 0)
                            {
                                rs = i + 1;
                                break;
                            }
                        }
                    }
                }
                return rs;
            }

            public int GetLastIndexOfTextBlock(string text, int index, SelectionProcType procType = SelectionProcType.Key, string returnText = null, string splitter = null)
            {
                returnText = returnText ?? this.ReturnText;
                splitter = splitter ?? this.Splitter;

                int rs = text.Length;
                if (0 < text.Length)
                {
                    if (text.Length == index
                    || (procType == SelectionProcType.Mouse && this.IsThisText(text, index, returnText)))
                    {
                        index--;
                    }

                    int splitterStartIndex = splitter.IndexOf(text.Substring(index, 1));
                    if (splitterStartIndex >= 0)
                    {
                        if (this.IsThisText(text, index, returnText))
                        {
                            rs = index + 2;
                        }
                        else
                        {
                            for (int i = index + 1; i < text.Length; i++)
                            {
                                if (splitterStartIndex != splitter.IndexOf(text.Substring(i, 1)))
                                {
                                    rs = i;
                                    break;
                                }
                            }
                        }
                        //rs = this.IsThisText(text, index, returnText)
                        //    ? index + 2
                        //    : index + 1;
                    }
                    else
                    {
                        for (int i = index; i < rs; i++)
                        {
                            if (splitter.IndexOf(text.Substring(i, 1)) >= 0)
                            {
                                rs = i;
                                break;
                            }
                        }
                    }
                }
                return rs;
            }

            public int GetLastIndexOfTextBlock(StringBuilder sb, int index, SelectionProcType procType = SelectionProcType.Key, string returnText = null, string splitter = null)
            {
                returnText = returnText ?? this.ReturnText;
                splitter = splitter ?? this.Splitter;

                int rs = sb.Length;
                if (0 < sb.Length)
                {
                    if (sb.Length == index
                    || (procType == SelectionProcType.Mouse && this.IsThisText(sb, index, returnText)))
                    {
                        index--;
                    }

                    int splitterStartIndex = splitter.IndexOf(sb.ToString(index, 1));
                    if (splitter.IndexOf(sb.ToString(index, 1)) >= 0)
                    {
                        if (this.IsThisText(sb, index, returnText))
                        {
                            rs = index + 2;
                        }
                        else
                        {
                            for(int i = index + 1; i < sb.Length; i++)
                            {
                                if (splitterStartIndex != splitter.IndexOf(sb.ToString(i, 1)))
                                {
                                    rs = i;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = index; i < rs; i++)
                        {
                            if (splitter.IndexOf(sb.ToString(i, 1)) >= 0)
                            {
                                rs = i;
                                break;
                            }
                        }
                    }
                }
                return rs;
            }

            public bool SetLeftReference(DataClass data, int index, Font font, string returnText = null)
            {
                returnText = returnText ?? this.ReturnText;

                bool rs = false;
                if (this.LeftReference < 0)
                {
                    int lineNumber = data.GetLineFromCharIndex(index);
                    int targetFirstIndex = data.GetFirstCharIndexFromLine(lineNumber);

                    targetFirstIndex = targetFirstIndex < 0 ? 0 : targetFirstIndex;
                    string targetText = data.SB.ToString(targetFirstIndex, index - targetFirstIndex);

                    this.LeftReference = this.GetTextWidth(targetText, font);
                    rs = true;
                }
                return rs;
            }

            public void ClearLeftReference()
            {
                this.LeftReference = -1;
            }

            public int GetTextWidth(string text, Font font)
            {
                if (text.Length > 0)
                {
                    Size textPixelSize;
                    using (var tempImage = GetTemporaryImage())
                    {
                        // hdc Proc #1 Start
                        var g = GetTemporaryGraphics(tempImage);
                        IntPtr hdc = g.GetHdc();

                        IntPtr fontInptr = font.ToHfont();
                        GDI32.SelectObject(hdc, fontInptr);

                        GDI32.GetTextExtentPoint32(hdc, text, text.Length, out textPixelSize);
                        
                        GDI32.DeleteObject(fontInptr);

                        // hdc Proc #2 End
                        g.ReleaseHdc(hdc);
                        g.Dispose();
                    }

                    return textPixelSize.Width;
                }
                else
                {
                    return 0;
                }
            }

            public int GetTextWidth(string text, IntPtr hdc)
            {
                if (text.Length > 0)
                {
                    Size textPixelSize;
                    GDI32.GetTextExtentPoint32(hdc, text, text.Length, out textPixelSize);
                    return textPixelSize.Width;
                }
                else
                {
                    return 0;
                }
            }

            public void SetSelection(DataClass data, int selectionStart, int selectionLength = 0, bool riseSelectionChangedEvent = true)
            {
                int oldSelectionStart = this.Start;
                int oldSelectionLength = this.Length;
                this.Start = selectionStart;
                this.Length = selectionLength;

                if (data.SB.Length < this.Start)
                {
                    this.Start = data.SB.Length;
                }
                else if (this.Start < 0)
                {
                    this.Start = 0;
                }

                if (data.SB.Length < this.Start + this.Length)
                {
                    this.Length = data.SB.Length - this.Start;
                }

                if (riseSelectionChangedEvent && (oldSelectionStart != this.Start || oldSelectionLength != this.Length))
                {
                    this.OnSelectionChanged?.Invoke(this, new SelectionEventArgs(this.Start, this.Length));

                }
            }
            #endregion

            #region Function String
            private int LastIndexOf(string text, string searchText, int index, int number = 0)
            {
                int rs = 0;
                for (int i = 0; i <= number; i++)
                {
                    rs = text.LastIndexOf(searchText, index);
                    if (rs < 0)
                    {
                        break;
                    }
                    index = rs - 1;
                }
                return rs;
            }

            private int IndexOf(string text, string searchText, int index, int number = 0)
            {
                int rs = 0;
                for (int i = 0; i <= number; i++)
                {
                    rs = text.IndexOf(searchText, index);
                    if (rs < 0)
                    {
                        break;
                    }
                    index = rs + 1;
                }
                return rs;
            }

            private int IndexOf(StringBuilder sb, string searchText, int index, int number = 0)
            {
                int rs = 0;
                for (int i = 0; i <= number; i++)
                {
                    rs = sb.IndexOf(searchText, index);
                    if (rs < 0)
                    {
                        break;
                    }
                    index = rs + 1;
                }
                return rs;
            }

            private string GetTextBlock(string text, string startText, string lastText, int index)
            {
                string rs = null;
                try
                {
                    int startIndex = this.IndexOf(text, startText, index);
                    int lastIndex = this.LastIndexOf(text, lastText, index);
                    text.Substring(startIndex, lastIndex - startIndex);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return rs;
            }

            private string GetTextBlock(string text, int startIndex, string lastText, int index)
            {
                string rs = null;
                try
                {
                    int lastIndex = this.LastIndexOf(text, lastText, index);
                    text.Substring(startIndex, lastIndex - startIndex);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return rs;
            }

            private string GetTextBlock(string text, string startText, int lastIndex, int index)
            {
                string rs = null;
                try
                {
                    int startIndex = this.IndexOf(text, startText, index);
                    text.Substring(startIndex, lastIndex - startIndex);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return rs;
            }

            private string GetTextBlock(string text, int startIndex, int lastIndex)
            {
                string rs = null;
                try
                {
                    rs = text.Substring(startIndex, lastIndex - startIndex);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return rs;
            }

            private bool IsThisText(string text, int index, string reference)
            {
                return index >= 0
                    && index + reference.Length - 1 < text.Length
                    && text.Substring(index, reference.Length) == reference;
            }

            private bool IsThisText(StringBuilder sb, int index, string reference)
            {
                return index >= 0
                    && index + reference.Length - 1 < sb.Length
                    && sb.ToString(index, reference.Length) == reference;
            }
            #endregion

            #region Temporary Image
            private Bitmap GetTemporaryImage()
            {
                return new Bitmap(1, 1);
            }

            private Graphics GetTemporaryGraphics(Bitmap image)
            {
                return Graphics.FromImage(image);
            }
            #endregion
        }

        public class SelectionEventArgs : EventArgs
        {
            public int SelectionStart;
            public int SelectionLength;
            public SelectionEventArgs(int _SelectionStart, int _SelectionLength)
            {
                this.SelectionStart = _SelectionStart;
                this.SelectionLength = _SelectionLength;
            }
        }

        public enum SelectionShowingTypeOption { VisableChaging, ColorChanging }

        public enum SelectionProcType { Key, Mouse };

        public enum SelectionMouseActionType { Down, Move, DoubleDown };

    }
}
