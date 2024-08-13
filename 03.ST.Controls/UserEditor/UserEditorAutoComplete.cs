using Newtonsoft.Json.Linq;
using ST.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ST.Controls
{
    public partial class UserEditor
    {
        /// <summary>
        /// 자동 완성 리스트가 표시되기 직전에 발생합니다.
        /// </summary>
        public event UserEditorShowAutoCompleteEventHandler AutoCompleteShown;
        private UserListView AutoCompleteList = new UserListView();
        private List<string> AutoCompleteData = null;
        private int AutoCompleteStartIndex = -1;

        // Option
        private int AutoCompleteListMaxHeight = 190;
        private int AutoCompleteListMinHeight = 22;
        private int AutoCompleteListMinWidth = 100;

        private bool IsAutoCompleteListShown => AutoCompleteList.Visible;

        private void LoadAutoComplete()
        {
            AutoCompleteList.Visible = false;
            AutoCompleteList.Font = Font;
            AutoCompleteList.TabStop = false;
            AutoCompleteList.ColumnHeight = 0;

            AutoCompleteList.GotFocus += AutoCompleteList_GotFocus;
            AutoCompleteList.MouseDown += AutoCompleteList_MouseDown;
            AutoCompleteList.DoubleClick += AutoCompleteList_DoubleClick;

            AutoCompleteList.VisibleChanged += AutoCompleteList_VisibleChanged;

            Controls.Add(AutoCompleteList);
            LostFocus += UserEditor_LostFocus1;
        }

        private void AutoCompleteList_VisibleChanged(object sender, EventArgs e)
        {
            if (!AutoCompleteList.Visible)
            {
            }
        }

        /// <summary>
        /// 자동 완성 이벤트를 호출합니다.
        /// </summary>
        /// <param name="word"></param>
        public void OnShowAutoComplete(string word = "")
        {
            if (!IsAutoCompleteListShown)
            {
                ShowAutoComplete(word);
            }
        }

        #region Event
        private void AutoCompleteList_GotFocus(object sender, EventArgs e)
        {
            ActiveControl = null;
        }

        private void AutoCompleteList_MouseDown(object sender, MouseEventArgs e)
        {
            ActiveControl = null;
        }

        private void AutoCompleteList_DoubleClick(object sender, EventArgs e)
        {
            PickText();
        }

        private void UserEditor_LostFocus1(object sender, EventArgs e)
        {
            if (!ContainsFocus)
            {
                AutoCompleteList.Visible = false;
            }
        }
        #endregion

        #region Function Inner
        private void ShowAutoComplete(string word = "")
        {
            var e = new UserEditorShowAutoCompleteEventArg();
            AutoCompleteShown?.Invoke(this, e);

            if (e.ShowAutoCompleteList && e.Data != null && e.Data.Count > 0)
            {
                if (e.Data.Count > 0)
                {
                    // Set Data N Field
                    AutoCompleteData = e.Data;
                    AutoCompleteList.AddColumn(new UserListViewColumn("Field", "Field"));
                    AutoCompleteList.Bind(e.Data.ToDataTable("Field"));
                    AutoCompleteList.SelectedItemIndexList.Clear();
                    AutoCompleteList.SelectedItemIndex = 0;

                    // Set Auto Bounds N Column Width
                    AutoCompleteList.Bounds = ShowAutoComplete_GetBounds(word);
                    AutoCompleteList.Columns[0].Width = AutoCompleteList.Width - AutoCompleteList.VScrollBarWidth - 1;
                
                    // Show
                    if (!AutoCompleteList.Visible)
                    {
                        AutoCompleteList.ScrollTop = 0;
                        AutoCompleteList.BringToFront();
                        AutoCompleteList.Visible = true;
                        AutoCompleteStartIndex = Selection.Start - word.Length;
                        if (word != string.Empty)
                        {
                            AutoComplete_WndProc_WM_CHAR(0);
                        }
                    }
                    Focus();
                }
            }
        }

        private Rectangle ShowAutoComplete_GetBounds(string word = "")
        {
            Point location = Point.Empty;
            Size size = new Size(
                  Math.Max(AutoCompleteListMinWidth, ShowAutoComplete_GetBounds_GetWidth())
                , Math.Min(AutoCompleteListMaxHeight, AutoCompleteList.FullHeight)
            );

            // Set location.Y, Revise size.Height
            int bottomAreaHeight = Height - Draw.CursorRectangle.Bottom;
            int topAreaHeight = Draw.CursorRectangle.Top;
            if (size.Height <= bottomAreaHeight)
            {
                // Bottom
                location.Y = Draw.CursorRectangle.Bottom;
            }
            else
            {
                if (size.Height <= topAreaHeight)
                {
                    // Top
                    location.Y = Draw.CursorRectangle.Y - size.Height;
                }
                else
                {
                    // Revise Size
                    if (bottomAreaHeight >= topAreaHeight)
                    {
                        // Bottom
                        size.Height = Math.Max(AutoCompleteListMinHeight, bottomAreaHeight - bottomAreaHeight % (AutoCompleteListMinHeight - 1) + 1);
                        location.Y = Draw.CursorRectangle.Bottom;
                    }
                    else
                    {
                        // Top
                        size.Height = Math.Max(AutoCompleteListMinHeight, topAreaHeight - topAreaHeight % (AutoCompleteListMinHeight - 1) + 1);
                        location.Y = Draw.CursorRectangle.Y - size.Height;
                    }
                }
            }

            // Set location.X ,Revise size.Width
            int wordWidth = 0;
            if (word != "")
            {
                using (var g = Graphics.FromImage(new Bitmap(1, 1)))
                {
                    SizeF wordSize = g.MeasureString(word, AutoCompleteList.Font);
                    wordWidth = Convert.ToInt32(Math.Ceiling(wordSize.Width)) - 5;
                }
            }
            location.X = Draw.CursorRectangle.X - wordWidth;
            int rightAreaWidth = Width - Draw.CursorRectangle.X;
            if (size.Width <= rightAreaWidth)
            {
                location.X = Draw.CursorRectangle.X - wordWidth;
            }
            else
            {
                if (size.Width <= Width)
                {
                    location.X = Width - size.Width - wordWidth;
                }
                else
                {
                    // Revise Size
                    size.Width = Width;
                    location.X = 0;
                }
            }

            return new Rectangle(location, size);
        }

        private int ShowAutoComplete_GetBounds_GetWidth()
        {
            int maxWidth = 0;
            int revisionWidth = DefaultVScrollWidth + 8;

            using (var g = Graphics.FromImage(new Bitmap(1, 1)))
            {
                for(int i = 0; i < AutoCompleteList.Items.Count; i++)
                {
                    SizeF size = g.MeasureString(AutoCompleteList.Items[i].Text, AutoCompleteList.Font);
                    int width = Convert.ToInt32(Math.Ceiling(size.Width));
                    if (maxWidth < width)
                    {
                        maxWidth = width;
                    }
                }
            }

            if (maxWidth != 0)
            {
                maxWidth += revisionWidth;
            }

            return maxWidth;
        }

        private void PickText()
        {
            if (AutoCompleteList.SelectedItem != null)
            {
                if (Selection.Length > 0)
                {
                    KeyDelete(false);
                }

                int searchTextLength = Selection.Start - AutoCompleteStartIndex;
                if (searchTextLength > 0)
                {
                    RemoveText(AutoCompleteStartIndex, searchTextLength);
                    Selection.Length = 0;
                }
                InsertText(AutoCompleteStartIndex, AutoCompleteList.SelectedItem.Text);
                Selection.Start = AutoCompleteStartIndex + AutoCompleteList.SelectedItem.Text.Length;

                Draw.Draw();
            }
            AutoCompleteList.Visible = false;
            AutoCompleteStartIndex = -1;
        }
        #endregion

        #region Function Outer
        private bool AutoComplete_KeyDown(KeyEventArgs e)
        {
            bool rs = false;
            if (IsAutoCompleteListShown)
            {
                switch(e.KeyCode)
                {
                    case Keys.Up:
                    case Keys.Down:
                    case Keys.PageUp:
                    case Keys.PageDown:
                        AutoCompleteList.OnProcessCmdKey(e.KeyCode);
                        rs = true;
                        break;
                    default:
                        int keyCode = (int)e.KeyCode;
                        if (!((0x30 <= keyCode && keyCode <= 0x5A)
                           || (0x60 <= keyCode && keyCode <= 0x6F)
                           || e.KeyCode == Keys.Oem1     // /
                           || e.KeyCode == Keys.Oem2     // :
                           || e.KeyCode == Keys.Oem4     // {
                           || e.KeyCode == Keys.Oem6     // }
                           || e.KeyCode == Keys.OemMinus // -
                           || e.KeyCode == Keys.Enter
                           || e.KeyCode == Keys.Escape
                           || e.KeyCode == Keys.Tab
                           || e.KeyCode == Keys.Back
                           || e.KeyCode == Keys.Delete
                           || e.KeyCode == Keys.Control
                           || e.KeyCode == Keys.ControlKey
                           || e.KeyCode == Keys.LControlKey
                           || e.KeyCode == Keys.RControlKey
                           || e.KeyCode == Keys.ShiftKey
                           || e.KeyCode == Keys.LShiftKey
                           || e.KeyCode == Keys.RShiftKey
                           || e.KeyCode == Keys.Space
                           || e.KeyCode == Keys.CapsLock
                           ))
                        {
                            AutoCompleteList.Visible = false;
                        }
                        break;
                }
            }
            return rs;
        }

        private bool AutoComplete_WndProc_WM_CHAR(int keyChar)
        {
            bool rs = false;
            if (IsAutoCompleteListShown)
            {
                switch (keyChar)
                {
                    case 13: // Enter
                        PickText();
                        rs = true;
                        break;
                    case 27: // Esc
                        AutoCompleteList.Visible = false;
                        AutoCompleteStartIndex = -1;
                        rs = true;
                        break;
                    case 32: // Space
                        PickText();
                        break;
                }

                if (!rs)
                {
                    // A-Z a-z Baskspace / : { } - _ Empty(for word)
                    if ((65 <= keyChar && keyChar <= 90)
                     || (97 <= keyChar && keyChar <= 122)
                     || keyChar == 8
                     || keyChar == 47 || keyChar == 58
                     || keyChar == 123 || keyChar == 125
                     || keyChar == 45|| keyChar == 95
                     || keyChar == 0)
                    {
                        string searchText = null;
                        // Baskspace
                        if (keyChar == 8)
                        {
                            if (AutoCompleteStartIndex <= Selection.Start - 1)
                            {
                                searchText = Data.GetText(AutoCompleteStartIndex, Selection.Start - AutoCompleteStartIndex - 1);
                            }
                        }
                        // Empty(for word)
                        else if (keyChar == 0)
                        {
                            searchText = Data.GetText(AutoCompleteStartIndex, Selection.Start - AutoCompleteStartIndex);
                        }
                        // A-Z a-z
                        else
                        {
                            searchText = Data.GetText(AutoCompleteStartIndex, Selection.Start - AutoCompleteStartIndex)
                            + (char)keyChar;
                        }

                        if (searchText == null)
                        {
                            AutoCompleteList.Visible = false;
                        }
                        else
                        {
                            List<string> list = AutoCompleteData.Search(searchText);
                            if (list != null)
                            {
                                AutoCompleteList.Bind(list.ToDataTable("Field"));
                                Rectangle rec = ShowAutoComplete_GetBounds();
                                AutoCompleteList.Width = rec.Width;
                                AutoCompleteList.Height = rec.Height;
                                AutoCompleteList.Columns[0].Width = AutoCompleteList.Width - AutoCompleteList.VScrollBarWidth - 1;
                                AutoCompleteList.SelectedItemIndex = 0;
                            }
                            else
                            {
                                AutoCompleteList.SelectedItemIndex = -1;
                            }
                        }
                    }
                    else
                    {
                        AutoCompleteList.Visible = false;
                    }
                }
            }
            return rs;
        }

        private bool AutoComplete_ProcessCmdKey(Keys keyData)
        {
            bool rs = false;
            if (IsAutoCompleteListShown)
            {
                if(keyData == Keys.Tab)
                {
                    PickText();
                    rs = true;
                }
            }
            return rs;
        }
        #endregion
    }

    public delegate void UserEditorShowAutoCompleteEventHandler(object sender, UserEditorShowAutoCompleteEventArg e);

    public class UserEditorShowAutoCompleteEventArg : EventArgs
    {
        /// <summary>
        /// 자동 완성을 표시할지 여부를 가져오거나 반환합니다.
        /// </summary>
        public bool ShowAutoCompleteList = true;

        /// <summary>
        /// 자동 완성에 사용될 데이터를 가져오거나 반환합니다.
        /// </summary>
        public List<string> Data;
    }
}