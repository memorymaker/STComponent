using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.DataModeler
{
    public class NodeModel
    {
        public string NODE_ID { get; set; }  // Key
        public int    NODE_SEQ { get; set; } // Key
        public string NODE_TYPE { get; set; }
        public int    NODE_LEFT { get; set; }
        public int    NODE_TOP { get; set; }
        public int    NODE_WIDTH { get; set; }
        public int    NODE_HEIGHT { get; set; }
        public int    NODE_Z_INDEX { get; set; }
        public string NODE_OPTION { get; set; }
        public string NODE_NOTE { get; set; }
        public List<NodeDetailModel> NODE_DETAIL { get; set; }
    }

    public class NodeDetailModel
    {
        public string NODE_ID { get; set; }           // Key
        public int    NODE_SEQ { get; set; }          // Key
        public string NODE_DETAIL_ID { get; set; }    // Key
        public int    NODE_DETAIL_SEQ { get; set; }   // Key
        public int    NODE_DETAIL_ORDER { get; set; }
        public string NODE_DETAIL_TYPE { get; set; }
        public string NODE_DETAIL_DATA_TYPE { get; set; }
        public string NODE_DETAIL_DATA_TYPE_FULL { get; set; }
        public string NODE_DETAIL_COMMENT { get; set; }
		public string NODE_DETAIL_TABLE_ALIAS { get; set; }
		public string NODE_DETAIL_NOTE { get; set; }
        public string NODE_DETAIL_VIEW_COLUMN1 { get; set; }
        public string NODE_DETAIL_VIEW_COLUMN2 { get; set; }
        public string NODE_ID_REF { get; set; }
        public int    NODE_SEQ_REF { get; set; }
    }
}
