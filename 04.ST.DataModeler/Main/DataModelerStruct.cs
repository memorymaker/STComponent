using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.DataModeler
{
    public struct DataModelerNodeDataTables
    {
        public DataTable Node;
        public DataTable NodeDetail;

        public DataModelerNodeDataTables(DataTable _Node, DataTable _NodeDetails)
        {
            Node = _Node;
            NodeDetail = _NodeDetails;
        }
    }
}
