using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.CodeGenerator
{
    public partial class CodeGenerator
    {
        public static DataTable GetTemplateDataTable()
        {
            DataTable rsDt = new DataTable();
            rsDt.Columns.Add(TEMPLATE.TEMPLATE_SEQ, typeof(int));
            rsDt.Columns.Add(TEMPLATE.TEMPLATE_TITLE);
            rsDt.Columns.Add(TEMPLATE.TEMPLATE_CONTENT);
            rsDt.Columns.Add(TEMPLATE.TEMPLATE_RESULT);
            rsDt.Columns.Add(TEMPLATE.TEMPLATE_STYLE);
            rsDt.Columns.Add(TEMPLATE.TEMPLATE_SORT, typeof(int));
            rsDt.Columns.Add(TEMPLATE.TEMPLATE_NOTE);
            rsDt.Columns.Add(TEMPLATE.TEMPLATE_SELECTED, typeof(int));
            return rsDt;
        }

        public static DataTable GetEmptyRelationDataTable()
        {
            DataTable rsDt = new DataTable();
            rsDt.Columns.Add(RELATION.RELATION_TYPE);
            rsDt.Columns.Add(RELATION.RELATION_OPERATOR);
            rsDt.Columns.Add(RELATION.RELATION_VALUE);
            rsDt.Columns.Add(RELATION.RELATION_NOTE);
            rsDt.Columns.Add(RELATION.NODE_ID1);
            rsDt.Columns.Add(RELATION.NODE_SEQ1, typeof(int));
            rsDt.Columns.Add(RELATION.NODE_DETAIL_ID1);
            rsDt.Columns.Add(RELATION.NODE_DETAIL_SEQ1, typeof(int));
            rsDt.Columns.Add(RELATION.NODE_DETAIL_ORDER1, typeof(int));
            rsDt.Columns.Add(RELATION.NODE_DETAIL_TABLE_ALIAS1);
            rsDt.Columns.Add(RELATION.NODE_ID2);
            rsDt.Columns.Add(RELATION.NODE_SEQ2, typeof(int));
            rsDt.Columns.Add(RELATION.NODE_DETAIL_ID2);
            rsDt.Columns.Add(RELATION.NODE_DETAIL_SEQ2, typeof(int));
            rsDt.Columns.Add(RELATION.NODE_DETAIL_ORDER2, typeof(int));
            rsDt.Columns.Add(RELATION.NODE_DETAIL_TABLE_ALIAS2);
            return rsDt;
        }
    }
}
