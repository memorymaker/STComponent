using ST.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ST.DataModeler
{
    public partial class MemoNode : NodeBase
    {
        private Color ContentBackColor = Color.FromArgb(255, 249, 213);
        private Color ContentForeColor = Color.FromArgb(0, 0, 0);

        private void LoadDraw()
        {
            BackColor = Color.FromArgb(255, 201, 14);

            ContentEditor.BackColor = ContentBackColor;
            ContentEditor.ForeColor = ContentForeColor;

            Paint += MemoNode_Paint;
        }

        private void MemoNode_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            // Content BackGround
            Rectangle backgroundRec = new Rectangle(
                Padding.Left
                , Padding.Top + TitleHeight
                , Width - Padding.Horizontal
                , Height - Padding.Vertical - TitleHeight
            );
            g.FillRectangle(new SolidBrush(ContentBackColor), backgroundRec);

            Rectangle contentRec = new Rectangle(
                backgroundRec.Left + ContentEditorPadding.Left
                , backgroundRec.Top + ContentEditorPadding.Top
                , backgroundRec.Width - ContentEditorPadding.Horizontal
                , backgroundRec.Height - ContentEditorPadding.Vertical
            );

            //var t = ContentEditor.ContentImage;
            //g.DrawImage(ContentEditor.ContentImage, contentRec);
        }
    }
}
