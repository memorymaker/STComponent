using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.DataModeler
{
    public static class NODE
    {
        /// <summary>
        /// 노드의 아이디의 필드명을 설정하거나 반환합니다.
        /// </summary>
        public static string NODE_ID { get; set; } = "NODE_ID";

        /// <summary>
        /// 노드의 아이디가 동일할 때 시퀀스로 사용될 필드명을 설정하거나 반환합니다.
        /// </summary>
        public static string NODE_SEQ { get; set; } = "NODE_SEQ";
        
        /// <summary>
        /// 노드 디테일의 아이디를 설정하거나 반환합니다.
        /// </summary>
        public static string NODE_DETAIL_ID { get; set; } = "NODE_DETAIL_ID";
        
        public static string NODE_DETAIL_SEQ { get; set; } = "NODE_DETAIL_SEQ";
        
        public static string NODE_DETAIL_ORDER { get; set; } = "NODE_DETAIL_ORDER";
        
        public static string NODE_DETAIL_TYPE { get; set; } = "NODE_DETAIL_TYPE";
        
        public static string NODE_DETAIL_NOTE { get; set; } = "NODE_DETAIL_NOTE";
        
        public static string NODE_ID_REF { get; set; } = "NODE_ID_REF";
        
        public static string NODE_DETAIL_DATA_TYPE { get; set; } = "NODE_DETAIL_DATA_TYPE";
        
        public static string NODE_DETAIL_DATA_TYPE_FULL { get; set; } = "NODE_DETAIL_DATA_TYPE_FULL";
        
        public static string NODE_DETAIL_COMMENT { get; set; } = "NODE_DETAIL_COMMENT";
        
        public static string NODE_DETAIL_TABLE_ALIAS { get; set; } = "NODE_DETAIL_TABLE_ALIAS";
        
        public static string NODE_DETAIL_ORDINAL_POSITION { get; set; } = "ORDINAL_POSITION";
        
        public static string NODE_DETAIL_IS_PRIMARY_KEY { get; set; } = "IS_PRIMARY_KEY";
        
        public static string NODE_DETAIL_VIEW_COLUMN1 { get; set; } = "NODE_DETAIL_VIEW_COLUMN1";
        
        public static string NODE_DETAIL_VIEW_COLUMN2 { get; set; } = "NODE_DETAIL_VIEW_COLUMN2";
    }

    public static class RELATION
    {
        public static string RELATION_TYPE { get; set; } = "RELATION_TYPE";
        
        public static string RELATION_OPERATOR { get; set; } = "RELATION_OPERATOR";
        
        public static string RELATION_VALUE { get; set; } = "RELATION_VALUE";
        
        public static string RELATION_NOTE { get; set; } = "RELATION_NOTE";
        
        public static string NODE_ID1 { get; set; } = "NODE_ID1";
        
        public static string NODE_SEQ1 { get; set; } = "NODE_SEQ1";
        
        public static string NODE_DETAIL_ID1 { get; set; } = "NODE_DETAIL_ID1";
        
        public static string NODE_DETAIL_SEQ1 { get; set; } = "NODE_DETAIL_SEQ1";
        
        public static string NODE_DETAIL_ORDER1 { get; set; } = "NODE_DETAIL_ORDER1";
        
        public static string NODE_ID2 { get; set; } = "NODE_ID2";
        
        public static string NODE_SEQ2 { get; set; } = "NODE_SEQ2";
        
        public static string NODE_DETAIL_ID2 { get; set; } = "NODE_DETAIL_ID2";
        
        public static string NODE_DETAIL_SEQ2 { get; set; } = "NODE_DETAIL_SEQ2";
        
        public static string NODE_DETAIL_ORDER2 { get; set; } = "NODE_DETAIL_ORDER2";
    }
}
