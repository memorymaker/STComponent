using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.DataModeler
{
    public partial class DataModeler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetEmptyNodeDataTable()
        {
            DataTable rsDt = new DataTable();
            rsDt.Columns.Add(NODE.NODE_ID);
            rsDt.Columns.Add(NODE.NODE_SEQ, typeof(int));
            rsDt.Columns.Add(NODE.NODE_DETAIL_ID);
            rsDt.Columns.Add(NODE.NODE_DETAIL_SEQ, typeof(int));
            rsDt.Columns.Add(NODE.NODE_DETAIL_ORDER, typeof(int));
            rsDt.Columns.Add(NODE.NODE_DETAIL_TYPE);
            rsDt.Columns.Add(NODE.NODE_DETAIL_NOTE);
            rsDt.Columns.Add(NODE.NODE_ID_REF);
            rsDt.Columns.Add(NODE.NODE_DETAIL_DATA_TYPE);
            rsDt.Columns.Add(NODE.NODE_DETAIL_DATA_TYPE_FULL);
            rsDt.Columns.Add(NODE.NODE_DETAIL_COMMENT);
            rsDt.Columns.Add(NODE.NODE_DETAIL_TABLE_ALIAS);
            rsDt.Columns.Add(NODE.NODE_DETAIL_ORDINAL_POSITION);
            rsDt.Columns.Add(NODE.NODE_DETAIL_IS_PRIMARY_KEY);
            rsDt.Columns.Add(NODE.NODE_DETAIL_VIEW_COLUMN1);
            rsDt.Columns.Add(NODE.NODE_DETAIL_VIEW_COLUMN2);
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
            rsDt.Columns.Add(RELATION.NODE_ID2);
            rsDt.Columns.Add(RELATION.NODE_SEQ2, typeof(int));
            rsDt.Columns.Add(RELATION.NODE_DETAIL_ID2);
            rsDt.Columns.Add(RELATION.NODE_DETAIL_SEQ2, typeof(int));
            rsDt.Columns.Add(RELATION.NODE_DETAIL_ORDER2, typeof(int));
            return rsDt;
        }

    }
}
