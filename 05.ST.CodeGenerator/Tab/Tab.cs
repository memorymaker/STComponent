using ST.Controls;
using ST.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.CodeGenerator
{
    public partial class Tab : UserPanel
    {
        public TemplateProcessor Processor = new TemplateProcessor();

        public string EditStyleName => _EditorStyleName;
        public string _EditorStyleName = "None";

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
                    ErrorList.Enabled = value;
                    ErrorListWrapPanel.BorderTopColor = Color.FromArgb(230, 230, 230).GetColor(!value, -0.1f);

                    foreach(Control control in Controls)
                    {
                        Tab tab = control as Tab;
                        if (tab != null)
                        {
                            tab.Enabled = value;
                        }
                    }

                    base.Enabled = value;
                }
                Draw();
            }
        }

        public int SplitterDistance
        {
            get
            {
                return MainSplit.SplitterDistance;
            }
            set
            {
                MainSplit.SplitterDistance = value;
            }
        }

        public Tab()
        {
            BlockDrawing = true;
            LoadThis();
            LoadControls();
            LoadGraphicControls();
            LoadStyle();
            LoadDraw();
            LoadInput();
            BlockDrawing = false;
        }

        public Tab(string title, string guid): base(guid)
        {
            BlockDrawing = true;
            LoadThis(title);
            LoadControls();
            LoadGraphicControls();
            LoadStyle();
            LoadDraw();
            LoadInput();
            BlockDrawing = false;
        }

        private void LoadThis(string title = "")
        {
            Title = title;
            UsingMaximize = false;
            UsingViewContextMenuButton = false;
            UsingAwaysOnTopMenuButton = false;
            UsingTitleSlider = true;
            UsingTitleRename = true;
            _Padding = new Padding(0, TitleHeight, 0, 0);
            BorderLeftDrawing = false;
            BorderRightDrawing = false;
            BorderBottomDrawing = false;

            ParentChanged += Tab_ParentChanged;
            Closing += Tab_Closing;
            Disposed += Tab_Disposed;
        }

        private void Tab_ParentChanged(object sender, EventArgs e)
        {
            CodeGenerator parent = GetParentCodeGenerator();
            if (parent != null)
            {
                Processor.NodeFieldName_Table = parent.NodeFieldName_Table;
                Processor.NodeFieldName_TableSeq = parent.NodeFieldName_TableSeq;
                Processor.NodeFieldName_TableAlias = parent.NodeFieldName_TableAlias;
                Processor.NodeFieldName_Column = parent.NodeFieldName_Column;
                Processor.RelationFieldName_OriginTable = parent.RelationFieldName_OriginTable;
                Processor.RelationFieldName_OriginTableSeq = parent.RelationFieldName_OriginTableSeq;
                Processor.RelationFieldName_OriginTableAlias = parent.RelationFieldName_OriginTableAlias;
                Processor.RelationFieldName_OriginColumn = parent.RelationFieldName_OriginColumn;
                Processor.RelationFieldName_DestinationTable = parent.RelationFieldName_DestinationTable;
                Processor.RelationFieldName_DestinationTableSeq = parent.RelationFieldName_DestinationTableSeq;
                Processor.RelationFieldName_DestinationTableAlias = parent.RelationFieldName_DestinationTableAlias;
                Processor.RelationFieldName_DestinationColumn = parent.RelationFieldName_DestinationColumn;
                Processor.RelationFieldName_DestinationColumnOrder = parent.RelationFieldName_DestinationColumnOrder;
                Processor.RelationFieldName_JoinType = parent.RelationFieldName_JoinType;
                Processor.RelationFieldName_JoinOperator = parent.RelationFieldName_JoinOperator;
                Processor.RelationFieldName_JoinValue = parent.RelationFieldName_JoinValue;
            }
        }

        private void Tab_Closing(object sender, UserPanelClosingEventArgs e)
        {
            if (ModalMessageBox.Show("Are you sure you want to delete the tab?", "Tab", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void Tab_Disposed(object sender, EventArgs e)
        {
            TemplateEditor?.Dispose();
            ResultEditor?.Dispose();
            MainSplit?.Dispose();
        }

        public override void OnShown()
        {
            TemplateEditor.Focus();
            base.OnShown();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 70 || m.Msg == 641) // if (m.Msg == 70 || m.Msg == 641)
            {
                DrawPanel2();
            }
            base.WndProc(ref m);
        }

        public void SetEditorStyle(EditorStyle style)
        {
            _EditorStyleName = style.Name;

            TemplateEditor.Styles.Clear();
            ResultEditor.Styles.Clear();

            foreach (var content in style.Content)
            {
                TemplateEditor.Styles.Add(content.Name, content.UserEditorStyleInfo);
                ResultEditor.Styles.Add(content.Name, content.UserEditorStyleInfo);
            }

            foreach(var keyword in EditorCommandStyles)
            {
                TemplateEditor.Styles.Add(keyword.Key, keyword.Value);
            }

            TemplateEditor.OnDraw();
            ResultEditor.OnDraw();
        }
    }
}