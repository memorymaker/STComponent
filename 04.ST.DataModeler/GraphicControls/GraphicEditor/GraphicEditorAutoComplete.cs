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

namespace ST.DataModeler
{
    public partial class GraphicEditor
    {
        public event UserEditorShowAutoCompleteEventHandler AutoCompleteShown;
        private GraphicListView AutoCompleteList;
        private List<string> AutoCompleteData = null;
        private int AutoCompleteStartIndex = -1;

        // Option
        private int AutoCompleteListMaxHeight = 190;
        private int AutoCompleteListMinHeight = 22;
        private int AutoCompleteListMinWidth = 100;

        private bool IsAutoCompleteListShown => AutoCompleteList.Visible;

        private void LoadAutoComplete()
        {
            AutoCompleteList = new GraphicListView(Target);
            AutoCompleteList.Visible = false;
            AutoCompleteList.Font = Font;
            AutoCompleteList.TabStop = false;
            AutoCompleteList.ColumnHeight = 0;

            AutoCompleteList.GotFocus += AutoCompleteList_GotFocus;
            AutoCompleteList.MouseDown += AutoCompleteList_MouseDown;
            AutoCompleteList.DoubleClick += AutoCompleteList_DoubleClick;

            Controls.Add(AutoCompleteList);
            LostFocus += UserEditor_LostFocus1;
        }

        public void OnShowAutoComplete()
        {
            if (!IsAutoCompleteListShown)
            {
                ShowAutoComplete();
            }
        }

        #region Event
        private void AutoCompleteList_GotFocus(object sender, EventArgs e)
        {
            // ActiveControl = null;
        }

        private void AutoCompleteList_MouseDown(object sender, MouseEventArgs e)
        {
            // ActiveControl = null;
        }

        private void AutoCompleteList_DoubleClick(object sender, EventArgs e)
        {
            PickText();
        }

        private void UserEditor_LostFocus1(object sender, EventArgs e)
        {
            if (!Focused)
            {
                AutoCompleteList.Visible = false;
            }
        }
        #endregion

        #region Function Inner
        private void ShowAutoComplete()
        {
            var e = new UserEditorShowAutoCompleteEventArg();
            AutoCompleteShown?.Invoke(this, e);
            if (e.ShowAutoCompleteList && e.Data != null && e.Data.Count > 0)
            {
                // Set Data N Field
                AutoCompleteData = e.Data;
                AutoCompleteList.Columns.Clear();
                AutoCompleteList.AddColumn(new GraphicListViewColumn("Field", "Field"));
                AutoCompleteList.Bind(e.Data.ToDataTable("Field"));
                AutoCompleteList.FocusedIndex = 0;

                // Set Auto Bounds N Column Width
                AutoCompleteList.Bounds = ShowAutoComplete_GetBounds();
                AutoCompleteList.Columns[0].Width = AutoCompleteList.Width - AutoCompleteList.VScrollBarWidth - 1;
                
                // Show
                if (!AutoCompleteList.Visible)
                {
                    AutoCompleteList.BringToFront();
                    AutoCompleteList.Visible = true;
                    AutoCompleteStartIndex = Selection.Start;
                }
                Focus();
            }
        }

        private Rectangle ShowAutoComplete_GetBounds()
        {
            Point location = Point.Empty;
            Size size = new Size(
                  Math.Max(AutoCompleteListMinWidth, ShowAutoComplete_GetBounds_GetWidth())
                , Math.Min(AutoCompleteListMaxHeight, AutoCompleteList.FullHeight).ToInt()
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
            location.X = Draw.CursorRectangle.X;
            int rightAreaWidth = Width - Draw.CursorRectangle.X;
            if (size.Width <= rightAreaWidth)
            {
                location.X = Draw.CursorRectangle.X;
            }
            else
            {
                if (size.Width <= Width)
                {
                    location.X = Width - size.Width;
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
            if (AutoCompleteList.FocusedItem != null)
            {
                int searchTextLength = Selection.Start - AutoCompleteStartIndex;
                if (searchTextLength > 0)
                {
                    RemoveText(AutoCompleteStartIndex, searchTextLength);
                    Selection.Length = 0;
                }
                InsertText(AutoCompleteStartIndex, AutoCompleteList.FocusedItem.Text);
                Selection.Start = AutoCompleteStartIndex + AutoCompleteList.FocusedItem.Text.Length;

                Refresh();
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
                        // Todo : GraphicEditor 수정 필요 ?
                        AutoCompleteList.OnKeyDown(new KeyEventArgs(e.KeyCode));
                        // AutoCompleteList.OnProcessCmdKey(e.KeyCode);
                        rs = true;
                        break;
                    default:
                        int keyCode = (int)e.KeyCode;
                        if (!((0x30 <= keyCode && keyCode <= 0x5A)
                           || (0x60 <= keyCode && keyCode <= 0x6F)
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
                    // A-Z a-z Baskspace
                    if ((65 <= keyChar && keyChar <= 90)
                     || (97 <= keyChar && keyChar <= 122)
                     || keyChar == 8)
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
                                AutoCompleteList.FocusedIndex = 0;
                            }
                            else
                            {
                                AutoCompleteList.FocusedIndex = -1;
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
        public bool ShowAutoCompleteList = true;
        public List<string> Data;
    }
}