using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ExceptionServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using ST.Core;

namespace ST.DataModeler
{
    public partial class GraphicEditor
    {
        private class DataClass
        {
            #region Values
            private GraphicEditor Parent;
            public StringBuilder SB = new StringBuilder();
            public bool WordWrap { get; set; } = false;

            // Line Data
            // Nomal Line Data
            private Dictionary<int, int> FirstCharIndexFromLine = new Dictionary<int, int>() { { 0, 0 } };
            // Etc
            private Dictionary<int, int> LineTextByteLengths = new Dictionary<int, int>() { { 0, 0 } };
            #endregion

            #region Event
            public event EventHandler<DataEventArgs> OnSBChanged;
            #endregion

            #region Properties
            public int LineLength
            {
                get
                {
                    return FirstCharIndexFromLine.Count;
                }
            }

            public int TextLength
            {
                get
                {
                    return this.SB.Length;
                }
            }
            #endregion

            #region Load
            public DataClass(GraphicEditor _UserEditor)
            {
                Parent = _UserEditor;
            }
            #endregion

            #region Methods
            public void BindSB(string value)
            {
                this.SB = new StringBuilder(value);
                this.SBChanged(SBChangedType.Bind, 0, value.Length);
            }

            public void BindSB(StringBuilder sb)
            {
                this.SB = sb;
                this.SBChanged(SBChangedType.Bind, 0, sb.Length);
            }

            public void InsertSB(ref int index, char chr)
            {
                SB.Insert(index, chr);
                index += 1;
                this.SBChanged(SBChangedType.Append, index, 1, 1, true, chr.ToString());
            }

            public void InsertSB(ref int index, string text)
            {
                int textLength = text.Length;
                SB.Insert(index, text);
                int insertLineCount = this.GetFullLineCount(index, textLength);
                index += textLength;
                this.SBChanged(SBChangedType.Append, index, text.Length, insertLineCount, true, text);
            }

            public void ModifierSB(ref int index, char chr, int revisionIndex = 0, bool moveNext = true)
            {
                string removedText = SB[index + revisionIndex].ToString();
                SB[index + revisionIndex] = chr;
                if (moveNext)
                {
                    index += 1;
                }
                this.SBChanged(SBChangedType.Modifier, index + revisionIndex, 1, 1, true, chr.ToString(), removedText);
            }

            public void RemoveSB(ref int index, int length, int revisionIndex = 0)
            {
                int removeLineCount = this.GetFullLineCount(index + revisionIndex, length);
                string removedText = this.SB.ToString(index + revisionIndex, length);

                this.SB.Remove(index + revisionIndex, length);
                index = index + revisionIndex;
                this.SBChanged(SBChangedType.Remove, index, length, removeLineCount, true, null, removedText);
            }

            public void RemoveSB(ref int index, ref int length, int revisionIndex = 0)
            {
                int paramLength = length;
                int removeLineCount = this.GetFullLineCount(index + revisionIndex, length);
                string removedText = this.SB.ToString(index + revisionIndex, length);

                this.SB.Remove(index + revisionIndex, length);
                index = index + revisionIndex;
                length = 0;
                this.SBChanged(SBChangedType.Remove, index, paramLength, removeLineCount, true, null, removedText);
            }

            public void InsertSB(int index, char chr, bool raseEvent = false)
            {
                SB.Insert(index, chr);
                this.SBChanged(SBChangedType.Append, index + 1, 1, 1, raseEvent, chr.ToString());
            }

            public void InsertSB(int index, string text, bool raseEvent = false)
            {
                SB.Insert(index, text);
                int insertLineCount = this.GetFullLineCount(index, text.Length);
                this.SBChanged(SBChangedType.Append, index + text.Length, text.Length, insertLineCount, raseEvent, text);
            }

            public void ModifierSB(int index, char chr, int revisionIndex = 0, bool raseEvent = false)
            {
                string removedText = SB[index + revisionIndex].ToString();
                SB[index + revisionIndex] = chr;
                this.SBChanged(SBChangedType.Modifier, index, 1, 1, raseEvent, chr.ToString(), removedText);
            }

            public void RemoveSB(int index, ref int length, int revisionIndex = 0, bool raseEvent = false)
            {
                int paramLength = length;
                int removeLineCount = this.GetFullLineCount(index + revisionIndex, length);
                string removedText = this.SB.ToString(index + revisionIndex, length);

                this.SB.Remove(index + revisionIndex, length);
                length = 0;
                this.SBChanged(SBChangedType.Remove, index + revisionIndex, paramLength, removeLineCount, raseEvent, null, removedText);
            }

            public void RemoveSB(int index, int length, int revisionIndex = 0, bool raseEvent = false)
            {
                int removeLineCount = this.GetFullLineCount(index + revisionIndex, length);
                string removedText = this.SB.ToString(index + revisionIndex, length);

                this.SB.Remove(index + revisionIndex, length);
                this.SBChanged(SBChangedType.Remove, index + revisionIndex, length, removeLineCount, raseEvent, null, removedText);
            }

            private void SBChanged(SBChangedType changedType, int index, int length, int lineCount = -1, bool raseEvent = true, string insertedText = null, string removedText = null)
            {
                string returnText = "\r\n";
                int returnTextLength = returnText.Length;

                if (changedType == SBChangedType.Bind)
                {
                    this.SetLineData();
                }
                else if (changedType == SBChangedType.Append)
                {
                    int targetLine = this.GetLineFromCharIndex(index - length);
                    if (length == 1 || lineCount <= 1)
                    {
                        // FirstCharIndexFromLine
                        int iMax = this.FirstCharIndexFromLine.Count;
                        for (int i = targetLine + 1; i < iMax; i++)
                        {
                            this.FirstCharIndexFromLine[i] += length;
                        }

                        // LineTextLength
                        if (!WordWrap)
                        {
                            this.LineTextByteLengths[targetLine] += Encoding.Default.GetByteCount(SB.ToString(index - length, length).ToString());
                        }
                        // WordWrap == true | WordBreak FirstCharIndexFromLine
                        else
                        {
                            ArrangeLineDataForWordBreak(targetLine);
                        }
                    }
                    else
                    {
                        int oldLineCount = FirstCharIndexFromLine.Count;
                        
                        for (int i = oldLineCount + lineCount - 2; targetLine + lineCount - 1 <= i; i--)
                        {
                            // FirstCharIndexFromLine
                            this.FirstCharIndexFromLine[i] = this.FirstCharIndexFromLine[i - lineCount + 1] + length;

                            // LineTextLength
                            if (!WordWrap)
                            {
                                this.LineTextByteLengths[i] = this.LineTextByteLengths[i - lineCount + 1];
                            }
                        }

                        for (int i = targetLine; i < targetLine + lineCount - 1; i++)
                        {
                            // FirstCharIndexFromLine
                            int nextLineIndex = SB.IndexOf(returnText, this.FirstCharIndexFromLine[i]) + returnTextLength;
                            this.FirstCharIndexFromLine[i + 1] = nextLineIndex;

                            // LineTextLength
                            if (!WordWrap)
                            {
                                if (i + 1 == oldLineCount + lineCount)
                                {
                                    this.LineTextByteLengths[i] = Encoding.Default.GetByteCount(SB.ToString(this.FirstCharIndexFromLine[i], SB.Length - this.FirstCharIndexFromLine[i]).ToString());
                                }
                                else
                                {
                                    this.LineTextByteLengths[i] = Encoding.Default.GetByteCount(SB.ToString(this.FirstCharIndexFromLine[i], this.FirstCharIndexFromLine[i + 1] - this.FirstCharIndexFromLine[i] - returnTextLength).ToString());
                                }
                            }
                        }

                        // LineTextLength : Last
                        if (!WordWrap)
                        {
                            if (targetLine + lineCount - 1 == oldLineCount + lineCount - 2)
                            {
                                this.LineTextByteLengths[targetLine + lineCount - 1] = Encoding.Default.GetByteCount(SB.ToString(this.FirstCharIndexFromLine[targetLine + lineCount - 1], SB.Length - this.FirstCharIndexFromLine[targetLine + lineCount - 1]).ToString());
                            }
                            else
                            {
                                this.LineTextByteLengths[targetLine + lineCount - 1] = Encoding.Default.GetByteCount(SB.ToString(this.FirstCharIndexFromLine[targetLine + lineCount - 1], this.FirstCharIndexFromLine[targetLine + lineCount] - this.FirstCharIndexFromLine[targetLine + lineCount - 1] - returnTextLength).ToString());
                            }
                        }
                        // WordWrap == true | WordBreak FirstCharIndexFromLine
                        else 
                        {
                            ArrangeLineDataForWordBreak(targetLine, lineCount);
                        }
                    }
                }
                else if (changedType == SBChangedType.Remove)
                {
                    int targetLine = this.GetLineFromCharIndex(index);
                    if (length == 1)
                    {
                        // FirstCharIndexFromLine
                        int iMax = this.FirstCharIndexFromLine.Count;
                        for (int i = targetLine + 1; i < iMax; i++)
                        {
                            this.FirstCharIndexFromLine[i] -= length;
                        }

                        if (targetLine < iMax - 1 && this.FirstCharIndexFromLine[targetLine] == this.FirstCharIndexFromLine[targetLine + 1])
                        {
                            for (int i = targetLine + 1; i < iMax; i++)
                            {
                                this.FirstCharIndexFromLine[i - 1] = this.FirstCharIndexFromLine[i];
                            }
                            this.FirstCharIndexFromLine.Remove(iMax - 1);
                        }

                        // LineTextLength
                        if (!WordWrap)
                        {
                            this.LineTextByteLengths[targetLine] = Encoding.Default.GetByteCount(this.GetLineText(targetLine));
                        }
                    }
                    else
                    {
                        // FirstCharIndexFromLine
                        int oldLineCount = FirstCharIndexFromLine.Count;
                        for (int i = targetLine + 1; i < oldLineCount; i++)
                        {
                            if (i <= oldLineCount - lineCount)
                            {
                                this.FirstCharIndexFromLine[i] = this.FirstCharIndexFromLine[i + lineCount - 1] - length;
                            }
                            else
                            {
                                this.FirstCharIndexFromLine.Remove(i);
                            }
                        }

                        // LineTextLength
                        if (!WordWrap)
                        {
                            for (int i = targetLine; i < oldLineCount; i++)
                            {
                                if (i <= oldLineCount - lineCount)
                                {
                                    if (i == oldLineCount - lineCount)
                                    {
                                        this.LineTextByteLengths[i] = Encoding.Default.GetByteCount(SB.ToString(this.FirstCharIndexFromLine[i], SB.Length - this.FirstCharIndexFromLine[i]));
                                    }
                                    else if (i == 0)
                                    {
                                        this.LineTextByteLengths[i] = Encoding.Default.GetByteCount(SB.ToString(0, this.FirstCharIndexFromLine[i + 1] - returnTextLength));
                                    }
                                    else
                                    {
                                        this.LineTextByteLengths[i] = Encoding.Default.GetByteCount(SB.ToString(this.FirstCharIndexFromLine[i], this.FirstCharIndexFromLine[i + 1] - this.FirstCharIndexFromLine[i] - returnTextLength));
                                    }
                                }
                                else
                                {
                                    this.LineTextByteLengths.Remove(i);
                                }
                            }
                        }
                    }

                    // WordBreak FirstCharIndexFromLine
                    if (WordWrap)
                    {
                        ArrangeLineDataForWordBreak(targetLine);
                    }
                }

                if (raseEvent)
                {
                    this.OnSBChanged?.Invoke(this, new DataEventArgs(changedType, index, length, lineCount, !raseEvent, insertedText, removedText));
                }
            }

            public string GetText(int index, int length)
            {
                return SB.ToString(index, length);
            }

            public int GetFirstCharIndexFromLine(int lineNumber)
            {
                return FirstCharIndexFromLine.ContainsKey(lineNumber) ? FirstCharIndexFromLine[lineNumber] : -1;
            }

            public int GetLastCharIndexFromLine(int lineNumber)
            {
                int rs = -1;
                if (FirstCharIndexFromLine.ContainsKey(lineNumber))
                {
                    if (FirstCharIndexFromLine.ContainsKey(lineNumber + 1))
                    {
                        int sp = FirstCharIndexFromLine[lineNumber];
                        int nextLineIndex = FirstCharIndexFromLine[lineNumber + 1];
                        if (nextLineIndex - sp >= 2 && SB[nextLineIndex - 2] == '\r' && SB[nextLineIndex - 1] == '\n')
                        {
                            rs = FirstCharIndexFromLine[lineNumber + 1] - 2;
                        }
                        else
                        {
                            rs = FirstCharIndexFromLine[lineNumber + 1];
                        }
                    }
                    else
                    {
                        rs = SB.Length;
                    }
                }
                return rs;
            }

            public int GetFirstCharIndexFromIndex(int index)
            {
                int lineNumber = GetLineFromCharIndex(index);
                int rs = GetFirstCharIndexFromLine(lineNumber);
                return rs;
            }

            public int GetLastCharIndexFromIndex(int index)
            {
                int lineNumber = GetLineFromCharIndex(index);
                int rs = GetLastCharIndexFromLine(lineNumber);
                return rs;
            }

            public int GetLineFromCharIndex(int index)
            {
                int count = FirstCharIndexFromLine.Count;
                int low = 0;
                int high = count - 1;
                int mid;

                while (low <= high)
                {
                    mid = (low + high) / 2;
                    if (FirstCharIndexFromLine[mid] <= index && (mid == count - 1 || index < FirstCharIndexFromLine[mid + 1]))
                    {
                        return mid;
                    }
                    else if (index < FirstCharIndexFromLine[mid])
                    {
                        high = mid - 1;
                    }
                    else
                    {
                        low = mid + 1;
                    }
                }
                return -1;
            }

            public int GetLineStartIndexFromIndex(int index)
            {
                int rs = -1;

                int count = FirstCharIndexFromLine.Count;
                int oldValue = 0;
                for(int i = 0; i < count; i++)
                {
                    if (index < FirstCharIndexFromLine[i])
                    {
                        rs = oldValue;
                        break;
                    }
                    oldValue = FirstCharIndexFromLine[i];
                }
                rs = rs < 0 ? oldValue : rs;

                //foreach (KeyValuePair<int, int> pair in FirstCharIndexFromLine)
                //{
                //    if (index < pair.Value)
                //    {
                //        rs = oldValue;
                //        break;
                //    }
                //    oldValue = pair.Value;
                //}
                //rs = rs < 0 ? oldValue : rs;

                return rs;
            }

            public string GetLineText(int lineNumber)
            {
                int sp = FirstCharIndexFromLine[lineNumber];
                int ep = GetLastCharIndexFromLine(lineNumber);
                return this.GetText(sp, ep - sp);
            }

            public string[] GetLines(int startLineIndex, int endLineIndex)
            {
                if (FirstCharIndexFromLine.Count - 1 < endLineIndex)
                {
                    endLineIndex = FirstCharIndexFromLine.Count - 1;
                }
                string[] rsLines = new string[endLineIndex - startLineIndex + 1];
                
                int k = 0;
                for(int i = startLineIndex; i <= endLineIndex; i++)
                {
                    rsLines[k] = GetLineText(i);
                    k++;
                }

                return rsLines;
            }

            // 테스트 필요
            public int _GetLineCount(int startIndex, int length)
            {
                int rs = 1;

                bool startCounting = false;
                int endIndex = startIndex + length;
                foreach (KeyValuePair<int, int> pair in FirstCharIndexFromLine)
                {
                    if (startIndex <= pair.Value)
                    {
                        if (!startCounting)
                        {
                            startCounting = true;
                        }
                        else
                        {
                            // todo : 부등호 확인 필요
                            if (pair.Value <= endIndex)
                            {
                                rs++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }

                return rs;
            }

            public int GetFullLineCount(int startIndex, int length)
            {
                int rs = 0, index = startIndex;
                while (index >= 0 && index < startIndex + length)
                {
                    rs++;
                    index = this.SB.IndexOf("\r\n", index);
                    if (index >= 0)
                    {
                        index++;
                    }
                }
                return rs;
            }

            public int GetLineLength(int lineNumber)
            {
                int rs = -1;
                if (FirstCharIndexFromLine.ContainsKey(lineNumber))
                {
                    int sp = FirstCharIndexFromLine[lineNumber];
                    int ep;
                    if (FirstCharIndexFromLine.ContainsKey(lineNumber + 1))
                    {
                        ep = FirstCharIndexFromLine[lineNumber + 1] - 2;
                    }
                    else
                    {
                        ep = SB.Length;
                    }
                    rs = ep - sp;
                }
                return rs;
            }

            public int GetLineLengthFromCharIndex(int index)
            {
                int rs = -1;
                int line = GetLineFromCharIndex(index);
                if (line >= 0)
                {
                    rs = GetLineLength(line);
                }
                return rs;
            }

            public string GetLineTextFromCharIndex(int index)
            {
                return GetLineText(GetLineFromCharIndex(index));
            }

            public int GetMaxLineTextByteLength()
            {
                int rs = !WordWrap ? LineTextByteLengths.Values.Max() : 0;
                return rs;
            }

            public bool IsWordBreakLine(int lineNumber)
            {
                bool rs = false;
                if (lineNumber >= 0 && lineNumber < LineLength - 1)
                {
                    int lineIndex = FirstCharIndexFromLine[lineNumber];
                    int nextLineIndex = FirstCharIndexFromLine[lineNumber + 1];
                    rs = !(nextLineIndex - lineIndex >= 2 && SB[nextLineIndex - 2] == '\r' && SB[nextLineIndex - 1] == '\n');
                }

                return rs;
            }

            public void SetLineData()
            {
                this.LineTextByteLengths.Clear();
                this.LineTextByteLengths[0] = 0;
                this.FirstCharIndexFromLine.Clear();
                this.FirstCharIndexFromLine[0] = 0;

                int index = 0;
                int iMax = SB.Length - 1;
                int i = 1;
                
                if (iMax > 0)
                {
                    using (var tempImage = GetTemporaryImage())
                    {
                        // hdc Proc #1 Start
                        var g = GetTemporaryGraphics(tempImage);
                        IntPtr hdc = g.GetHdc();

                        IntPtr fontPtr = Parent.Font.ToHfont();
                        GDI32.SelectObject(hdc, fontPtr);
                        int maxWidth = Parent.Width - Parent.Padding.Horizontal - (Parent.VScroll.Visible ? Parent.VScroll.Width : 0);

                        while (index < iMax)
                        {
                            index = this.SB.IndexOf("\r\n", index);
                            if (index < 0)
                            {
                                if (WordWrap)
                                {
                                    string lineString = SB.ToString(this.FirstCharIndexFromLine[i - 1], SB.Length - this.FirstCharIndexFromLine[i - 1]);
                                    SetLineData_SetBreakLineData(ref i, lineString, index, maxWidth, hdc);
                                }
                                break;
                            }
                            else
                            {
                                // FirstCharIndexFromLine
                                this.FirstCharIndexFromLine[i] = index + 2;

                                // FirstCharIndexFromWordBreakLine
                                if (WordWrap)
                                {
                                    string lineString = SB.ToString(this.FirstCharIndexFromLine[i - 1], this.FirstCharIndexFromLine[i] - this.FirstCharIndexFromLine[i - 1] - 2);
                                    SetLineData_SetBreakLineData(ref i, lineString, index, maxWidth, hdc);
                                }
                                else
                                {
                                    // LineTextLength
                                    this.LineTextByteLengths[i - 1] = Encoding.Default.GetByteCount(SB.ToString(this.FirstCharIndexFromLine[i - 1], this.FirstCharIndexFromLine[i] - this.FirstCharIndexFromLine[i - 1] - 2));
                                }

                                // Loop Ref
                                index++;
                                i++;
                            }
                        }

                        if (!WordWrap)
                        {
                            // Last LineTextLength
                            this.LineTextByteLengths[i - 1] = Encoding.Default.GetByteCount(SB.ToString(0, this.SB.Length - this.FirstCharIndexFromLine[i - 1]));
                        }

                        GDI32.DeleteObject(fontPtr);

                        // hdc Proc #2 End
                        g.ReleaseHdc(hdc);
                        g.Dispose();
                    }
                }
            }

            private void SetLineData_SetBreakLineData(ref int i, string lineString, int index, int maxWidth, IntPtr hdc)
            {
                int removedStringLength = 0;
                int mainLineStartPoint = i > 0 ? this.FirstCharIndexFromLine[i - 1] : 0;
                while (true)
                {
                    Size lineSize;
                    GDI32.GetTextExtentPoint32(hdc, lineString, lineString.Length, out lineSize);
                    if (lineSize.Width <= maxWidth)
                    {
                        if(index >= 0)
                        {
                            // Last & End
                            this.FirstCharIndexFromLine[i] = index + 2;
                        }
                        break;
                    }
                    else
                    {
                        // Get lineEndIndex
                        int lineEndIndex;
                        float _per = (float)maxWidth / lineSize.Width;
                        int checkIndex = (int)Math.Round(lineString.Length * _per);
                        bool? increment = null;
                        int m = 0;
                        while (true)
                        {
                            Size lineSizeCurrent;
                            GDI32.GetTextExtentPoint32(hdc, lineString, checkIndex + m, out lineSizeCurrent);
                            if (lineSizeCurrent.Width == maxWidth)
                            {
                                break;
                            }
                            else if (lineSizeCurrent.Width < maxWidth)
                            {
                                if (increment == false)
                                {
                                    break;
                                }
                                m++;
                                increment = true;
                            }
                            else if (maxWidth < lineSizeCurrent.Width)
                            {
                                m--;
                                if (increment == true)
                                {
                                    break;
                                }
                                increment = false;
                            }
                        }
                        lineEndIndex = checkIndex + m - 1; // - 1: m is length

                        // Get spaceStartingIndex, spaceEndIndex
                        int lastSpaceIndex = -1;
                        for (int n = lineEndIndex - 1; 0 <= n; n--)
                        {
                            if (lineString[n] == ' ' || lineString[n] == '\t')
                            {
                                lastSpaceIndex = n;
                                break;
                            }
                        }

                        int resultIndex = lastSpaceIndex < 0
                            ? lineEndIndex
                            : lastSpaceIndex;
                        this.FirstCharIndexFromLine[i] = mainLineStartPoint + removedStringLength + resultIndex + 1;
                        lineString = lineString.Substring(resultIndex + 1, lineString.Length - (resultIndex + 1));

                        removedStringLength += resultIndex + 1;
                        i++;
                    }
                }
            }

            public void ArrangeLineDataForWordBreak(int startLineNumber, int fullLineCount = 1)
            {
                using (var tempImage = GetTemporaryImage())
                {
                    // hdc Proc #1 Start
                    var g = GetTemporaryGraphics(tempImage);
                    IntPtr hdc = g.GetHdc();

                    IntPtr fontPtr = Parent.Font.ToHfont();
                    GDI32.SelectObject(hdc, fontPtr);
                    int maxWidth = Parent.Width - Parent.Padding.Horizontal - (Parent.VScroll.Visible ? Parent.VScroll.Width : 0);

                    int targetLineNumber = startLineNumber;
                    for (int i = 0; i < fullLineCount; i++)
                    {
                        // Get fullLineStartIndex / Get newFirstCharIndexFromLine
                        int _prevReturnIndex = SB.LastIndexOf("\r\n", FirstCharIndexFromLine[targetLineNumber]);
                        int fullStartLineNumber = _prevReturnIndex < 0 ? 0 : GetLineFromCharIndex(_prevReturnIndex + 2);
                        int fullLineStartIndex = FirstCharIndexFromLine[fullStartLineNumber];

                        // Get fullLineEndIndex / Get newFirstCharIndexFromLine
                        int _nextReturnIndex = SB.IndexOf("\r\n", fullLineStartIndex);
                        int fullLineEndIndex = _nextReturnIndex >= 0 ? _nextReturnIndex : SB.Length;

                        // Get newFirstCharIndexFromLine
                        string _fullLineString = SB.ToString(fullLineStartIndex, fullLineEndIndex - fullLineStartIndex);
                        int refLineNumber = fullStartLineNumber;
                        Dictionary<int, int> newFirstCharIndexFromLine = ArrangeLineDataForWordBreak_GetBreakLineData(ref refLineNumber, _fullLineString, fullLineStartIndex, maxWidth, hdc);

                        // Set FirstCharIndexFromLine
                        int oldBreakEndLineNumber = GetLineFromCharIndex(fullLineEndIndex);
                        if (oldBreakEndLineNumber == fullStartLineNumber + newFirstCharIndexFromLine.Count - 1)
                        {
                            // None
                        }
                        else if (oldBreakEndLineNumber < fullStartLineNumber + newFirstCharIndexFromLine.Count - 1)
                        {
                            int addedCount = fullStartLineNumber + newFirstCharIndexFromLine.Count - 1 - oldBreakEndLineNumber;
                            for (int k = LineLength - 1; fullStartLineNumber <= k; k--)
                            {
                                FirstCharIndexFromLine[k + addedCount] = FirstCharIndexFromLine[k];
                            }
                        }
                        else if (oldBreakEndLineNumber > fullStartLineNumber + newFirstCharIndexFromLine.Count - 1)
                        {
                            int removedCount = oldBreakEndLineNumber - (fullStartLineNumber + newFirstCharIndexFromLine.Count - 1);
                            int lineLength = LineLength;
                            for (int k = fullStartLineNumber; k < lineLength; k++)
                            {
                                if (k < lineLength - removedCount)
                                {
                                    FirstCharIndexFromLine[k] = FirstCharIndexFromLine[k + removedCount];
                                }
                                else
                                {
                                    FirstCharIndexFromLine.Remove(k);
                                }
                            }
                        }

                        // Apply newFirstCharIndexFromLine
                        foreach (KeyValuePair<int, int> pair in newFirstCharIndexFromLine)
                        {
                            FirstCharIndexFromLine[pair.Key] = pair.Value;
                        }

                        // Next
                        targetLineNumber = GetLineFromCharIndex(fullLineEndIndex + 2);
                    }

                    // hdc Proc #2 End
                    g.ReleaseHdc(hdc);
                    g.Dispose();
                }
            }

            private Dictionary<int, int> ArrangeLineDataForWordBreak_GetBreakLineData(ref int lineNumber, string lineString, int lineStartIndex, int maxWidth, IntPtr hdc)
            {
                Dictionary<int, int> rsNewFirstCharIndexFromLine = new Dictionary<int, int>() {
                    { lineNumber, lineStartIndex }
                };

                int removedStringLength = 0;
                while (true)
                {
                    Size lineSize;
                    GDI32.GetTextExtentPoint32(hdc, lineString, lineString.Length, out lineSize);
                    if (lineSize.Width <= maxWidth)
                    {
                        break;
                    }
                    else
                    {
                        // Get lineEndIndex
                        int lineEndIndex;
                        float _per = (float)maxWidth / lineSize.Width;
                        int checkIndex = (int)Math.Round(lineString.Length * _per);
                        bool? increment = null;
                        int m = 0;
                        while (true)
                        {
                            Size lineSizeCurrent;
                            GDI32.GetTextExtentPoint32(hdc, lineString, checkIndex + m, out lineSizeCurrent);
                            if (lineSizeCurrent.Width == maxWidth)
                            {
                                break;
                            }
                            else if (lineSizeCurrent.Width < maxWidth)
                            {
                                if (increment == false)
                                {
                                    break;
                                }
                                m++;
                                increment = true;
                            }
                            else if (maxWidth < lineSizeCurrent.Width)
                            {
                                m--;
                                if (increment == true)
                                {
                                    break;
                                }
                                increment = false;
                            }
                        }
                        lineEndIndex = checkIndex + m - 1; // - 1: m is length

                        // Get spaceStartingIndex, spaceEndIndex
                        int lastSpaceIndex = -1;
                        for (int n = lineEndIndex - 1; 0 <= n; n--)
                        {
                            if (lineString[n] == ' ' || lineString[n] == '\t')
                            {
                                lastSpaceIndex = n;
                                break;
                            }
                        }

                        int resultIndex = lastSpaceIndex < 0
                            ? lineEndIndex
                            : lastSpaceIndex;
                        rsNewFirstCharIndexFromLine[lineNumber + 1] = lineStartIndex + removedStringLength + resultIndex + 1; // + 1: Next line start index
                        lineString = lineString.Substring(resultIndex + 1, lineString.Length - (resultIndex + 1));

                        removedStringLength += resultIndex + 1;
                        lineNumber++;
                    }
                }

                return rsNewFirstCharIndexFromLine;
            }
            #endregion

            #region Public Methods
            public int GetLeftSpaceCount(int index, int length)
            {
                int rs = 0;
                int iMax = index + length;
                iMax = SB.Length < iMax ? SB.Length : iMax;

                if (index < 0 || length < 1)
                {
                    return -1;
                }

                for(int i = index; i < iMax; i++)
                {
                    if (SB[i] == ' ')
                    {
                        rs++;
                    }
                    else
                    {
                        break;
                    }
                }
                return rs;
            }

            public int GetTextIndex(string text, int startIndex = 0)
            {
                return SB.IndexOf(text, startIndex);
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

        public class DataEventArgs : EventArgs
        {
            public SBChangedType Type;
            public int Index;
            public int Length;
            public int LineCount;
            public bool IsUnredoProc = false;
            public string InsertedText = null;
            public string RemovedText = null;

            public DataEventArgs(SBChangedType _Type, int index, int length, int lineCount = -1, bool isUnredoProc = false, string insertedText = null, string removedText = null)
            {
                this.Type = _Type;
                this.Index = index;
                this.Length = length;
                this.LineCount = lineCount;
                this.IsUnredoProc = isUnredoProc;
                this.InsertedText = insertedText;
                this.RemovedText = removedText;
            }
        }

        public enum SBChangedType { Append, Remove, Modifier, Bind }

    }
}
