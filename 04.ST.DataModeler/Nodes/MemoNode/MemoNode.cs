using ST.Controls;
using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public partial class MemoNode : NodeBase
    {
        #region Values
        public GraphicEditor ContentEditor;
        
        private int ContentEditorVScrollWidth = 8;
        #endregion

        #region Propertise
        public override NodeType NodeType { get; set; } = NodeType.MemoNode;
        #endregion

        #region Load
        public MemoNode(DataModeler target) : base(target)
        {
            LoadThis();
            LoadDraw();
            LoadInput();
        }

        private void LoadThis()
        {
            SetDefault();
            SetEvents();
        }

        private void SetDefault()
        {
            // Base Propertise
            _AutoSize = false;
            EnableCaptionEdit = true;

            Padding = new Padding(3);

            // GraphicMouseAction
            GraphicMouseAction.UseSizing = true;

            // UserPanelControl
            EnableCaptionEdit = false;

            // ContentEditor
            ContentEditor = new GraphicEditor(Target);
            ContentEditor.Font = Font;

            ContentEditor.Visible = true;
            ContentEditor.ScrollBars = ScrollBars.None;
            ContentEditor.WordWrap = true;
            ContentEditor.Dock = DockStyle.Fill;
            ContentEditor.VScroll.Width = ContentEditorVScrollWidth;
            ContentEditor.VScroll.BackColor = ContentBackColor;
            ContentEditor.VScroll.IncreaseDecreaseButtonVisible = false;

            Controls.Add(ContentEditor);
        }
        #endregion

        #region Event
        private void SetEvents()
        {
            SizeChanging += MemoNode_SizeChanging;
            SizeChanged += MemoNode_SizeChanged;

            ContentEditor.GotFocus += ContentEditor_GotFocus;
        }

        private void MemoNode_SizeChanging(object sender, EventArgs e)
        {
            SetContentEditorAttribute();
        }

        private void MemoNode_SizeChanged(object sender, EventArgs e)
        {
            
        }

        private void ContentEditor_GotFocus(object sender, EventArgs e)
        {
            BringToFront();
        }
        #endregion

        #region Override
        public override NodeModel GetNodeModel()
        {
            var rs = new NodeModel
            {
                NODE_ID = ID,
                NODE_SEQ = SEQ,
                NODE_TYPE = NodeType.GetStringValue(),
                NODE_LEFT = AbsoluteLeft,
                NODE_TOP = AbsoluteTop,
                NODE_WIDTH = OriginalWidth == 0 ? Width : OriginalWidth,
                NODE_HEIGHT = OriginalHeight == 0 ? Height : OriginalHeight,
                NODE_Z_INDEX = ZIndex,
                NODE_OPTION = NodeOption,
                NODE_NOTE = NodeNote
            };
            return rs;
        }
        #endregion

        #region IScaleControl
        public override Color MinimapColor { get; set; } = Color.FromArgb(255, 201, 14);
        #endregion
    }
}
