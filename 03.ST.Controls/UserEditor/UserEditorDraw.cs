﻿using ST.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace ST.Controls
{
    public partial class UserEditor
    {
        public class DarwClass
        {
            #region Option
            public Color SelectionColorFocused = Color.FromArgb(255, 128, 128);
            public Color SelectionColorUnfocused = Color.FromArgb(255, 198, 198);
            public int PaddingTop = 0;
            public int PaddingLeft = 1;
            public float DisableBrightnessColorPoint = -0.1f;
            #endregion

            #region SelectionInfo
            // Options
            private string SelectionInfoString = "Line: {0}  Col: {1}  Pos: {2}";
            private int SelectionInfoPaddingLeft = 4;
            private int SelectionInfoPaddingRight = 4;
            private Font SelectionInfoFont = new Font("맑은 고딕", 8f);
            // Ref
            public int SelectionInfoWidth
            {
                get
                {
                    return _SelectionInfoWidth;
                }
                set
                {
                    if (_SelectionInfoWidth != value)
                    {
                        _SelectionInfoWidth = value;
                    }

                    if (Parent.HScroll.Visible && Parent.HScroll.Width != Parent.Width - _SelectionInfoWidth)
                    {
                        Parent.HScroll.Width = Parent.Width - _SelectionInfoWidth;
                        Parent.HScroll.Draw();
                    }
                }
            }
            private int _SelectionInfoWidth = 0;
            #endregion

            #region Reference
            private UserEditor Parent;
            public Size FontPixelSize;
            public bool DrawCursor = false;
            #endregion

            #region Etc
            public Rectangle CursorRectangle = Rectangle.Empty;
            #endregion

            #region DrawContentStringLine Variable
            private int DrawContentStringLine_CurrentPageFirstLineIndex = -1;
            private bool DrawContentStringLine_HasCommentAbovePage = false;
            private bool DrawContentStringLine_HasCommentAboveLine = false;
            #endregion

            public DarwClass(UserEditor _UserEditor)
            {
                // Temporary Size
                FontPixelSize = new Size(10, 10);
                this.Parent = _UserEditor;
            }

            public void Draw()
            {
                if (Parent.Visible && Parent.Width > 0 && Parent.Height > 0)
                {
                    try
                    {
                        DrawContent(Parent.CreateGraphics(), DrawCursor);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }

            private void DrawContent(Graphics g, bool drawCursor = false)
            {
                // Get hdc & Clear
                BufferedGraphicsContext context = new BufferedGraphicsContext();
                context.MaximumBuffer = new Size(Parent.Width, Parent.Height);
                BufferedGraphics bufferGraphics = context.Allocate(g, new Rectangle(0, 0, Parent.Width, Parent.Height));
                bufferGraphics.Graphics.Clear(Parent.BackColor.GetColor(!Parent.Enabled, DisableBrightnessColorPoint));
                IntPtr hdc = bufferGraphics.Graphics.GetHdc();

                // Declare IntPtr // 아래와 병합?
                IntPtr fontPtr = this.Parent.Font.ToHfont();
                IntPtr penPtr = GDI32.GetStockObject((int)GDI32.StockObjects.DC_PEN);
                IntPtr brushPtr = GDI32.GetStockObject((int)GDI32.StockObjects.DC_BRUSH);

                // Select IntPtr
                GDI32.SelectObject(hdc, fontPtr);
                GDI32.SelectObject(hdc, penPtr);
                GDI32.SelectObject(hdc, brushPtr);

                // Get FontPixelSize & Get fullText
                GDI32.GetTextExtentPoint32(hdc, " ", 1, out this.FontPixelSize);
                string fullText = Parent.Data.SB.ToString();

                // Common
                int left = Parent.WordWrap ? 0 : (this.PaddingLeft + Parent.HScrollValue);
                int top = (this.PaddingTop + Parent.VScrollValue);
                int drawStartLine = Parent.VScroll.Value;
                int drawLastLine = ((Parent.VScroll.Value + Parent.PageLineSize + Parent.Data.LineLength - Math.Abs(Parent.VScroll.Value + Parent.PageLineSize - Parent.Data.LineLength)) / 2) + 1;

                // Draw Selection #1
                int selectionColorFocused = ColorTranslator.ToWin32(this.SelectionColorFocused.GetColor(!Parent.Enabled, DisableBrightnessColorPoint));
                int selectionColorUnfocused = ColorTranslator.ToWin32(this.SelectionColorUnfocused.GetColor(!Parent.Enabled, DisableBrightnessColorPoint));

                // Draw Selection #2
                bool drawSelection = Parent.Selection.Length > 0;
                int selectionSp = -1, selectionEp = -1, selectionStartLine = -1, selectionLastLine = -1;
                if (drawSelection)
                {
                    selectionSp = Parent.Selection.Start;
                    selectionEp = Parent.Selection.Start + Parent.Selection.Length;
                    selectionStartLine = Parent.Data.GetLineFromCharIndex(selectionSp);
                    selectionLastLine = Parent.Data.GetLineFromCharIndex(selectionEp);
                }

                // Draw String Line #1
                GDI32.SetBkMode(hdc, (int)GDI32.BkModes.MODE_TRANSPARENT);
                GDI32.SetBkColor(hdc, ColorTranslator.ToWin32(Parent.BackColor.GetColor(!Parent.Enabled, DisableBrightnessColorPoint)));
                string[] drawLines = Parent.Data.GetLines(drawStartLine, drawLastLine);
                // Draw String Line #2
                int maxVisabledLineIndex = drawLastLine - drawStartLine;
                maxVisabledLineIndex = drawLines.Length - 1 < maxVisabledLineIndex ? drawLines.Length - 1 : maxVisabledLineIndex;
                // Draw String Line #3 - Get Comment Info
                bool usingSingleLineComment;
                bool usingMultiLineComment;
                // Draw Line Background #1 - Get maxTextWidth
                var drawLineBackgoundRightX = Parent.HScrollValue == 0
                    ? left + Parent.Width
                    : Parent.Data.GetMaxLineTextByteLength() * Parent.Draw.FontPixelSize.Width + 1;
                UserEditorStyleInfo commentStyleInfo = DrawContent_GetCommentStyleInfo(out usingSingleLineComment, out usingMultiLineComment);
                for (int i = 0; i <= maxVisabledLineIndex; i++)
                {
                    // Common
                    string drawString = drawLines[i];
                    int currentLineIndex = i + drawStartLine;

                    // Draw Line Background #2(End) - Use UserEditor.LineStyles, UserEditor.Styles
                    Color _lineBackgroundColor = Parent.GetLineBackColor(i + drawStartLine, drawString);
                    if (_lineBackgroundColor != Color.Empty)
                    {
                        int lineBackgroundColor = ColorTranslator.ToWin32(_lineBackgroundColor.GetColor(!Parent.Enabled, DisableBrightnessColorPoint));
                        GDI32.SetDCBrushColor(hdc, lineBackgroundColor);
                        GDI32.SetDCPenColor(hdc, lineBackgroundColor);
                        GDI32.Rectangle(hdc, left, top + currentLineIndex * this.FontPixelSize.Height, drawLineBackgoundRightX, top + (currentLineIndex + 1) * this.FontPixelSize.Height);
                    }

                    // Draw Selection #3
                    if (drawSelection)
                    {
                        if (selectionStartLine <= currentLineIndex && currentLineIndex <= selectionLastLine)
                        {
                            GDI32.SetDCBrushColor(hdc, Parent.Focused ? selectionColorFocused : selectionColorUnfocused);
                            GDI32.SetDCPenColor(hdc, Parent.Focused ? selectionColorFocused : selectionColorUnfocused);

                            Size linePixelSize;
                            GDI32.GetTextExtentPoint32(hdc, drawString, drawString.Length, out linePixelSize);
                            // Default : Full text in the line
                            int x1 = 0;
                            int x2 = currentLineIndex != selectionLastLine && Parent.Data.IsWordBreakLine(currentLineIndex)
                                ? linePixelSize.Width
                                : linePixelSize.Width + this.FontPixelSize.Width;

                            // Start Line
                            if (currentLineIndex == selectionStartLine)
                            {
                                string targetText = drawString.Substring(0, selectionSp - Parent.Data.GetFirstCharIndexFromLine(selectionStartLine));
                                Size targetTextPixelSize;
                                GDI32.GetTextExtentPoint32(hdc, targetText, targetText.Length, out targetTextPixelSize);
                                x1 = targetTextPixelSize.Width;
                            }

                            // Last Line
                            if (currentLineIndex == selectionLastLine)
                            {
                                string targetText = selectionEp < Parent.Data.TextLength && Parent.Data.GetText(selectionEp, 1) == "\n"
                                    ? drawString.Substring(0, selectionEp - Parent.Data.GetFirstCharIndexFromLine(selectionLastLine) - 1)
                                    : drawString.Substring(0, selectionEp - Parent.Data.GetFirstCharIndexFromLine(selectionLastLine));
                                Size targetTextPixelSize;
                                GDI32.GetTextExtentPoint32(hdc, targetText, targetText.Length, out targetTextPixelSize);
                                x2 = targetTextPixelSize.Width;
                            }

                            GDI32.Rectangle(hdc, left + x1, top + currentLineIndex * this.FontPixelSize.Height, left + x2, top + (currentLineIndex + 1) * this.FontPixelSize.Height);
                        }
                    }

                    // Draw String Line #3
                    int drawTop = this.PaddingTop + i * this.FontPixelSize.Height;
                    DrawContentStringLine(hdc, i, left, drawTop, currentLineIndex, drawString, drawString.Length, ref fullText, commentStyleInfo, usingMultiLineComment, usingSingleLineComment);
                }

                this.DrawContentCursor(hdc, left, top, drawCursor);
                if (Parent.ShowSelectoinInfo)
                {
                    this.DrawContentSelectionInfo(hdc);
                }
                else
                {
                    this.DrawContentEdge(hdc);
                }

                // Clean up IntPtr
                GDI32.DeleteObject(fontPtr);
                GDI32.DeleteObject(penPtr);
                GDI32.DeleteObject(brushPtr);

                bufferGraphics.Graphics.ReleaseHdc(hdc);
                bufferGraphics.Render(g);
            }

            private UserEditorStyleInfo DrawContent_GetCommentStyleInfo(out bool usingSingleLineComment, out bool usingMultiLineComment)
            {
                usingSingleLineComment = false;
                usingMultiLineComment = false;

                UserEditorStyleInfo rs = null;
                if (Parent.Styles.ContainsKey(Parent.CommentStylesName))
                {
                    rs = Parent.Styles[Parent.CommentStylesName];
                    usingSingleLineComment = !string.IsNullOrWhiteSpace(rs.SingleLineText);
                    usingMultiLineComment = !string.IsNullOrWhiteSpace(rs.MultiLineStartingText) && !string.IsNullOrWhiteSpace(rs.MultiLineEndText);
                }
                return rs;
            }

            private void DrawContentStringLine(IntPtr hdc, int i, int left, int top, int currentLineIndex, string text, int length, ref string fullText, UserEditorStyleInfo commentStyleInfo, bool usingMultiLineComment, bool usingSingleLineComment)
            {
                Size frontTextPixelSize;
                int lineStartingIndex = Parent.Data.GetFirstCharIndexFromLine(currentLineIndex);
                int lineEndIndex = lineStartingIndex + length - 1;

                // Comment Proc #1
                if (usingMultiLineComment)
                {
                    if (i == 0 && DrawContentStringLine_CurrentPageFirstLineIndex != currentLineIndex)
                    {
                        DrawContentStringLine_HasCommentAbovePage = DrawContentStringLine_CheckCommentAbovePage(currentLineIndex, ref fullText, commentStyleInfo, usingSingleLineComment);
                        DrawContentStringLine_CurrentPageFirstLineIndex = currentLineIndex;
                    }
                    
                    DrawContentStringLine_HasCommentAboveLine = i == 0 ? DrawContentStringLine_HasCommentAbovePage : DrawContentStringLine_HasCommentAboveLine;
                }

                // Comment Proc #2
                List<Range> commentRanges = null;
                if (usingMultiLineComment || usingSingleLineComment)
                {
                    commentRanges = DrawContentStringLine_GetCommentRanges(ref text, ref DrawContentStringLine_HasCommentAboveLine, commentStyleInfo, usingMultiLineComment, usingSingleLineComment);
                }

                // Comment Proc #3 - All line text is comments
                if (commentRanges != null && commentRanges.Count == 1
                && commentRanges[0].StartingValue == 0
                && commentRanges[0].EndValue == lineEndIndex - lineStartingIndex)
                {
                    GDI32.SetTextColor(hdc, ColorTranslator.ToWin32(commentStyleInfo.FontColor.GetColor(!Parent.Enabled, DisableBrightnessColorPoint)));
                    GDI32.TextOut(hdc, left, top, text, length);
                }
                // Not comments or only partially comments
                else
                {
                    // Default
                    GDI32.SetTextColor(hdc, ColorTranslator.ToWin32(this.Parent.ForeColor.GetColor(!Parent.Enabled, DisableBrightnessColorPoint)));
                    GDI32.TextOut(hdc, left, top, text, length);

                    // Style
                    foreach (var pair in Parent.Styles)
                    {
                        if (pair.Key != Parent.CommentStylesName)
                        {
                            UserEditorStyleInfo info = pair.Value;

                            if (info.FontColor != Color.Empty)
                            {
                                // Set text color
                                GDI32.SetTextColor(hdc, ColorTranslator.ToWin32(info.FontColor.GetColor(!Parent.Enabled, DisableBrightnessColorPoint)));

                                // Set regex
                                RegexOptions regexOptionsCaseSensitive = info.CaseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase;
                                RegexOptions regexOptionsCaseMultiline = info.MultiLine ? RegexOptions.Multiline : RegexOptions.None;
                                Regex regex = new Regex(info.Regex, regexOptionsCaseSensitive | regexOptionsCaseMultiline);

                                MatchCollection matches = regex.Matches(text);
                                foreach (Match match in matches)
                                {
                                    // Get width of text in front
                                    GDI32.GetTextExtentPoint32(hdc, text, match.Index, out frontTextPixelSize);

                                    // Draw text
                                    GDI32.TextOut(hdc, left + frontTextPixelSize.Width, top, match.Value, match.Value.Length);
                                }
                            }
                        }
                    }

                    // Index Style
                    foreach (var pair in Parent.RangeStyles)
                    {
                        UserEditorRangeStyleInfo info = pair.Value;
                        foreach (Range range in info.Ranges)
                        {
                            int targetSp = -1, targetEp = -1;
                            if (lineStartingIndex <= range.StartingValue && range.StartingValue <= lineEndIndex)
                            {
                                targetSp = range.StartingValue - lineStartingIndex;
                                targetEp = Math.Min(range.EndValue, lineEndIndex) - lineStartingIndex;
                            }
                            else if (lineStartingIndex <= range.EndValue && range.EndValue <= lineEndIndex)
                            {
                                targetSp = Math.Max(range.StartingValue, lineStartingIndex) - lineStartingIndex;
                                targetEp = range.EndValue - lineStartingIndex;
                            }
                            else if (range.StartingValue <= lineStartingIndex && lineEndIndex <= range.EndValue)
                            {
                                targetSp = 0;
                                targetEp = lineEndIndex - lineStartingIndex;
                            }

                            if (targetSp >= 0 && targetEp >= 0)
                            {
                                string targetText = text.Substring(targetSp, targetEp - targetSp + 1);
                                Size targetTextSize;
                                GDI32.GetTextExtentPoint32(hdc, targetText, targetText.Length, out targetTextSize);

                                // Get width of text in front
                                GDI32.GetTextExtentPoint32(hdc, text, targetSp, out frontTextPixelSize);

                                // Draw BackColor
                                if (info.BackColor != Color.Empty)
                                {
                                    int lineBackgroundColor = ColorTranslator.ToWin32(info.BackColor.GetColor(!Parent.Enabled, DisableBrightnessColorPoint));
                                    GDI32.SetDCBrushColor(hdc, lineBackgroundColor);
                                    GDI32.SetDCPenColor(hdc, lineBackgroundColor);
                                    GDI32.Rectangle(hdc, left + frontTextPixelSize.Width, top, left + frontTextPixelSize.Width + targetTextSize.Width, top + this.FontPixelSize.Height);
                                }

                                // Draw Border
                                if (info.BorderColor != Color.Empty)
                                {
                                    int lineBorderColor = ColorTranslator.ToWin32(info.BorderColor.GetColor(!Parent.Enabled, DisableBrightnessColorPoint));
                                    GDI32.SetDCPenColor(hdc, lineBorderColor);
                                    GDI32.DrawRactangle(hdc, left + frontTextPixelSize.Width, top, targetTextSize.Width, this.FontPixelSize.Height - 1);
                                }

                                // Draw Text
                                if (info.FontColor != Color.Empty)
                                {
                                    GDI32.SetTextColor(hdc, ColorTranslator.ToWin32(info.FontColor.GetColor(!Parent.Enabled, DisableBrightnessColorPoint)));
                                }

                                GDI32.TextOut(hdc, left + frontTextPixelSize.Width, top, targetText, targetEp - targetSp + 1);
                            }
                        }
                    }

                    // Comment Proc #4
                    if (commentRanges != null && commentRanges.Count > 0)
                    {
                        GDI32.SetTextColor(hdc, ColorTranslator.ToWin32(commentStyleInfo.FontColor.GetColor(!Parent.Enabled, DisableBrightnessColorPoint)));

                        foreach (Range range in commentRanges)
                        {
                            // Get width of text in front
                            GDI32.GetTextExtentPoint32(hdc, text, range.StartingValue, out frontTextPixelSize);

                            // Draw text
                            string value = text.Substring(range.StartingValue, range.Interval + 1);
                            GDI32.TextOut(hdc, left + frontTextPixelSize.Width, top, value, range.Interval + 1);
                        }
                    }
                }
            }

            private bool DrawContentStringLine_CheckCommentAbovePage(int currentLineIndex, ref string fullText, UserEditorStyleInfo commentStyleInfo, bool usingSingleLineComment)
            {
                int lineSp = Parent.Data.GetFirstCharIndexFromLine(currentLineIndex);
                int commentSp = fullText.LastIndexOf(commentStyleInfo.MultiLineStartingText, lineSp);
                if (commentSp == -1)
                {
                    return false;
                }
                else
                {
                    // Check single line comment
                    if (usingSingleLineComment)
                    {
                        int commentSpLineIndex = Parent.Data.GetLineFromCharIndex(commentSp);
                        string commentSpLineString = Parent.Data.GetLineText(commentSpLineIndex);

                        int singleLineCommentSpInLine = commentSpLineString.IndexOf(commentStyleInfo.SingleLineText);
                        int multiLineCommentSpInLine = commentSpLineString.IndexOf(commentStyleInfo.MultiLineStartingText);
                        if (singleLineCommentSpInLine > 0 && singleLineCommentSpInLine < multiLineCommentSpInLine)
                        {
                            return false;
                        }
                    }

                    int commentEp = fullText.IndexOf(commentStyleInfo.MultiLineEndText, commentSp);
                    if (commentEp > 0 && commentEp < lineSp)
                    {
                        return false;
                    }
                    else
                    {
                        if (commentEp == -1)
                        {
                            // None - all comment
                        }
                        else
                        {
                            // None - out commentEp
                        }
                    }
                }

                return true;
            }

            private List<Range> DrawContentStringLine_GetCommentRanges(ref string text, ref bool hasCommentAboveLine, UserEditorStyleInfo commentStyleInfo, bool usingMultiLineComment, bool usingSingleLineComment)
            {
                List<Range> rs = new List<Range>();

                bool multiLineCommentStart = false;
                int multiLineCommentStartIndex = 0;
                bool singleLineCommentStart = false;
                int singleLineCommentStartIndex = 0;

                if (hasCommentAboveLine)
                {
                    multiLineCommentStart = true;
                }

                int textLength = text.Length;
                for (int i = 0; i < textLength; i++)
                {
                    // Find multi line comment starting point
                    if (usingMultiLineComment && !multiLineCommentStart && !singleLineCommentStart
                    && text[i] == commentStyleInfo.MultiLineStartingText[0]
                    && DrawContentStringLine_GetCommentRanges_IsMatchedText(ref text, commentStyleInfo.MultiLineStartingText, i))
                    {
                        multiLineCommentStart = true;
                        multiLineCommentStartIndex = i;
                        i -= commentStyleInfo.MultiLineStartingText.Length - 1;
                    }
                    // Find multi line comment end point
                    else if (usingMultiLineComment && multiLineCommentStart
                    && text[i] == commentStyleInfo.MultiLineEndText[0]
                    && DrawContentStringLine_GetCommentRanges_IsMatchedText(ref text, commentStyleInfo.MultiLineEndText, i))
                    {
                        multiLineCommentStart = false;
                        rs.Add(new Range(multiLineCommentStartIndex, i + commentStyleInfo.MultiLineEndText.Length - 1));
                    }
                    // Find single line comment starting ponig
                    else if (usingSingleLineComment && !multiLineCommentStart && !singleLineCommentStart
                    && text[i] == commentStyleInfo.SingleLineText[0]
                    && DrawContentStringLine_GetCommentRanges_IsMatchedText(ref text, commentStyleInfo.SingleLineText, i))
                    {
                        singleLineCommentStart = true;
                        singleLineCommentStartIndex = i;
                    }
                }

                if (multiLineCommentStart)
                {
                    hasCommentAboveLine = true;
                    rs.Add(new Range(multiLineCommentStartIndex, textLength - 1));
                }
                else if (singleLineCommentStart)
                {
                    rs.Add(new Range(singleLineCommentStartIndex, textLength - 1));
                    hasCommentAboveLine = false;
                }
                else
                {
                    hasCommentAboveLine = false;
                }

                return rs;
            }

            private bool DrawContentStringLine_GetCommentRanges_IsMatchedText(ref string text, string matchText, int i)
            {
                bool rsMatched = true;
                
                if (text.Length >= i + matchText.Length)
                {
                    for (int k = 1; k < matchText.Length; k++)
                    {
                        if (text[i + k] != matchText[k])
                        {
                            rsMatched = false;
                            break;
                        }
                    }
                }
                else
                {
                    rsMatched = false;
                }

                return rsMatched;
            }

            private void DrawContentCursor(IntPtr hdc, int left, int top, bool drawCursor)
            {
                if (Parent.Focused || Parent.Selection.ShowNotFocused)
                {
                    if ((Parent.Selection.ShowingType == SelectionShowingTypeOption.VisableChaging && drawCursor) || Parent.Selection.ShowingType == SelectionShowingTypeOption.ColorChanging)
                    {
                        int lineIndex = Parent.Data.GetLineFromCharIndex(Parent.Selection.Start);
                        int textIndexInLine = Parent.Selection.Start - Parent.Data.GetFirstCharIndexFromLine(lineIndex);
                        Size targetTextPixelSize = new Size(0, this.FontPixelSize.Height);
                        if (Parent.Data.LineLength > 0)
                        {
                            string targetText = Parent.Data.GetLineText(lineIndex).Substring(0, textIndexInLine);
                            GDI32.GetTextExtentPoint32(hdc, targetText, targetText.Length, out targetTextPixelSize);
                        }

                        if (drawCursor)
                        {
                            GDI32.SetDCPenColor(hdc, Parent.Selection.Color1);
                        }
                        else
                        {
                            GDI32.SetDCPenColor(hdc, Parent.Selection.Color2);
                        }

                        CursorRectangle.X = left + targetTextPixelSize.Width + Parent.Selection.LeftRivision;
                        CursorRectangle.Y = top + lineIndex * this.FontPixelSize.Height + Parent.Selection.TopRivision;
                        CursorRectangle.Width = 1;
                        CursorRectangle.Height = this.FontPixelSize.Height + Parent.Selection.HeightRevision;

                        GDI32.Rectangle(hdc
                            , CursorRectangle.X
                            , CursorRectangle.Y
                            , CursorRectangle.X + CursorRectangle.Width
                            , CursorRectangle.Y + CursorRectangle.Height
                        );
                    }
                }
            }

            private void DrawContentEdge(IntPtr hdc)
            {
                if (Parent.ScrollBars == ScrollBars.Both)
                {
                    int backColor = ColorTranslator.ToWin32(SystemColors.Control.GetColor(!Parent.Enabled, DisableBrightnessColorPoint));

                    GDI32.SetDCBrushColor(hdc, backColor);
                    GDI32.SetDCPenColor(hdc, backColor);

                    GDI32.Rectangle(hdc
                        , Parent.Width - Parent.DefaultVScrollWidth
                        , Parent.Height - Parent.DefaultHScrollHeight
                        , Parent.Width
                        , Parent.Height
                    );
                }
            }

            private void DrawContentSelectionInfo(IntPtr hdc)
            {
                if (Parent.ScrollBars == ScrollBars.Both)
                {
                    int backColor = ColorTranslator.ToWin32(SystemColors.Control.GetColor(!Parent.Enabled, DisableBrightnessColorPoint));
                    
                    GDI32.SetDCBrushColor(hdc, backColor);
                    GDI32.SetDCPenColor(hdc, backColor);
                    GDI32.SetBkColor(hdc, backColor);
                    GDI32.SetTextColor(hdc, ColorTranslator.ToWin32(Color.FromArgb(0, 0, 0).GetColor(!Parent.Enabled, DisableBrightnessColorPoint)));
                    GDI32.SetBkMode(hdc, (int)GDI32.BkModes.MODE_TRANSPARENT);

                    int line = Parent.Data.GetLineFromCharIndex(Parent.SelectionStart) + 1;
                    int column = Parent.SelectionStart - Parent.Data.GetFirstCharIndexFromLine(line - 1) + 1;
                    string selectionInfoString = string.Format(SelectionInfoString, line, column, Parent.SelectionStart);

                    Size infoSize;
                    GDI32.GetTextExtentPoint32(hdc, selectionInfoString, selectionInfoString.Length, out infoSize);

                    GDI32.Rectangle(hdc
                        , Parent.Width - infoSize.Width - SelectionInfoPaddingLeft - SelectionInfoPaddingRight
                        , Parent.Height - Parent.DefaultHScrollHeight
                        , Parent.Width + infoSize.Width + SelectionInfoPaddingLeft + SelectionInfoPaddingRight
                        , Parent.Height
                    );

                    GDI32.TextOut(hdc
                        , Parent.Width - infoSize.Width - SelectionInfoPaddingRight
                        , Parent.Height - Parent.DefaultHScrollHeight / 2 - infoSize.Height / 2
                        , selectionInfoString, selectionInfoString.Length
                    );

                    SelectionInfoWidth = infoSize.Width + SelectionInfoPaddingLeft + SelectionInfoPaddingRight;
                }
            }
        }
    }
}
