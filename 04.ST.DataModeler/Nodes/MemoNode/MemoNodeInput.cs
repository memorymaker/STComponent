using ST.Controls;
using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public partial class MemoNode : NodeBase
    {
        public override string NodeNote
        {
            get
            {
                return ContentEditor.Text;
            }
            set
            {
                ContentEditor.Text = value;
                SetContentEditorAttribute();
                Refresh();
            }
        }

        private Padding ContentEditorPadding = new Padding(2);

        private void LoadInput()
        {
            AllowDrop = true;

            MouseDown += MemoNode_MouseDown;
            ContentEditor.LostFocus += ContentEditor_LostFocus;

            DragOver += MemoNode_DragOver;
            DragLeave += MemoNode_DragLeave;
            DragDrop += MemoNode_DragDrop;

            BtDeleteMouseDown += MemoNode_BtDeleteMouseDown;
            BtDelete.SetDrawColor(SimpleGraphicControl.StateType.Default, Color.FromArgb(255, 242, 200));
        }

        private void MemoNode_MouseDown(object sender, MouseEventArgs e)
        {
            int revision = 2;
            Rectangle contentRec = new Rectangle(Padding.Left + revision, Padding.Top + TitleHeight + revision, Width - Padding.Horizontal - revision * 2, Height - Padding.Vertical - TitleHeight - revision * 2);
            if (contentRec.Contains(e.Location))
            {
                GraphicMouseAction.Enable = false;
            }
        }

        private void ContentEditor_LostFocus(object sender, EventArgs e)
        {
            GraphicMouseAction.Enable = true;
        }

        private void MemoNode_DragOver(object sender, DragEventArgs e)
        {
            Point location = PointToClient(new Point(e.X, e.Y));
            Rectangle contentRec = new Rectangle(Padding.Left, Padding.Top + TitleHeight, Width - Padding.Horizontal, Height - Padding.Vertical - TitleHeight);

            if (contentRec.Contains(location))
            {
                Dictionary<string, object> dic = e.Data.GetData(typeof(Dictionary<string, object>)) as Dictionary<string, object>;
                if (dic != null)
                {
                    if (dic["Items"].GetType() == typeof(List<GraphicListViewItem>))
                    {
                        location.Offset(-Padding.Horizontal, -(TitleHeight + Padding.Top));
                        ContentEditor.SetSelection(location);
                        ContentEditor.OnDraw();

                        e.Effect = DragDropEffects.Copy;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void MemoNode_DragLeave(object sender, EventArgs e)
        {
            Target.Focus();
        }

        private void MemoNode_DragDrop(object sender, DragEventArgs e)
        {
            Dictionary<string, object> dic = e.Data.GetData(typeof(Dictionary<string, object>)) as Dictionary<string, object>;
            if (dic != null)
            {
                List<GraphicListViewItem> items = dic["Items"] as List<GraphicListViewItem>;
                if (items != null)
                {
                    List<string> itemStringList = new List<string>();
                    for(int i = 0; i < items.Count; i++)
                    {
                        itemStringList.Add(string.Format("{0}.{1}"
                            , items[i].Row[DataModeler.NODE.NODE_ID].ToString()
                            , items[i].Row[DataModeler.NODE.NODE_DETAIL_ID].ToString()
                        ));
                    }

                    ContentEditor.InsertText(ContentEditor.SelectionStart, String.Join("\r\n", itemStringList));
                    ContentEditor.OnDraw();
                }
            }
        }

        private void MemoNode_BtDeleteMouseDown(object sender, UserEventArgs e)
        {
            if (ModalMessageBox.Show("Are you sure you want to delete this memo node?", "Memo Node", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                Parent.Controls.Remove(this);
                Target.Refresh();
                Target.MimimapRefresh();
            }
        }

        public void ShowContentEditor()
        {
            SetContentEditorAttribute();
            ContentEditor.Visible = true;
            ContentEditor.Focus();
            SetContentEditorSelection();
        }

        private  void SetContentEditorSelection()
        {
            Point location = ContentEditor.PointToClient(Cursor.Position);
            ContentEditor.SetSelection(location);
        }

        public void SetContentEditorAttribute()
        {
            Font scaleFont = new Font(Font.FontFamily, Font.Size * ScaleValue);
            if (ContentEditor.Font.Size != scaleFont.Size)
            {
                ContentEditor.Font = scaleFont;
                ContentEditor.VScroll.Width = (ContentEditorVScrollWidth * ScaleValue).ToInt();
            }
        }
    }
}