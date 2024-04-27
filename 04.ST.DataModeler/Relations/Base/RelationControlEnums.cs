using ST.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.DataModeler
{
    public enum RelationControlType
    {
        [StringValue("I")]
        InnerJoin = 1, 
        [StringValue("L")]
        LeftJoin = 2,
        [StringValue("R")]
        RightJoin = 3, 
        [StringValue("")]
        None = 0
    }

    public enum RelationControlStatus
    {
        None = 0, MouseOver, Selected
    }

    public enum RelationHorizontalDirectionType
    {
        None = 0, LeftToLeft, LeftToRight, RightToLeft, RightToRight
    }

    public enum RelationVerticalDirectionType
    {
        None = 0, TopToDown, DownToTop, Equals
    }
}
