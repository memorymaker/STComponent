using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST.DataModeler
{
    public interface IRelationControlParent
    {
        int Left { get; set; }

        int Top { get; set; }

        int Width { get; set; }

        int Height { get; set; }

        int Right { get;}

        int Bottom { get; }

        Point Location { get; set; }

        Size Size { get; set; }

        Rectangle Bounds { get; set; }

        Padding Padding { get; set; }

        RelationControlCollection Relations { get; }

        void OnRelationAdded(RelationControlEventArgs e);

        void OnRelationRemoved(RelationControlEventArgs e);
    }
}
