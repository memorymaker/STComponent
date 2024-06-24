using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.DataModeler
{
    public class RelationModel
    {
		public string RELATION_TYPE { get; set; }

        public string RELATION_OPERATOR { get; set; }

        public string RELATION_VALUE { get; set; }

        public string RELATION_NOTE { get; set; }

        public string NODE_ID1 { get; set; }

        public int    NODE_SEQ1 { get; set; }

		public string NODE_DETAIL_ID1 { get; set; }

        public int    NODE_DETAIL_SEQ1 { get; set; }

		public int    NODE_DETAIL_ORDER1 { get; set; }

        public string NODE_ID2 { get; set; }

        public int    NODE_SEQ2 { get; set; }

		public string NODE_DETAIL_ID2 { get; set; }

        public int    NODE_DETAIL_SEQ2 { get; set; }

		public int    NODE_DETAIL_ORDER2 { get; set; }

        public string GetSortingString()
        {
            char empty = (char)0;
            StringBuilder sb = new StringBuilder();
            sb.Append(NODE_ID1.PadLeft(64, empty));
            sb.Append(NODE_SEQ1.ToString().PadLeft(4, '0'));
            //sb.Append(NODE_DETAIL_ID1.PadLeft(64, empty));
            sb.Append(NODE_DETAIL_ORDER1.ToString().PadLeft(4, '0'));
            sb.Append(NODE_ID2.PadLeft(64, empty));
            sb.Append(NODE_SEQ2.ToString().PadLeft(4, '0'));
            //sb.Append(NODE_DETAIL_ID2.PadLeft(64, empty));
            sb.Append(NODE_DETAIL_ORDER2.ToString().PadLeft(4, '0'));

            return sb.ToString();
        }
    }
}