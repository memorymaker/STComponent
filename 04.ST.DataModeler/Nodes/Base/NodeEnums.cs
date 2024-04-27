using ST.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.DataModeler
{
    public enum NodeType
    {
		[StringValue("TAB")]
		TableNode = 1,
		[StringValue("COL")]
		ColumnNode = 2,
		[StringValue("MEM")]
		MemoNode = 3,
		[StringValue("")]
		None = 4
    }
}
