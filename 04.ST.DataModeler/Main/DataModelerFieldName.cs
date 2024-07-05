using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.DataModeler
{
    public partial class  DataModeler
    {
        public static class NODE
        {
            /// <summary>
            /// 노드의 아이디 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_ID { get; set; } = "NODE_ID";

            /// <summary>
            /// 노드의 아이디가 동일할 때 시퀀스로 사용될 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_SEQ { get; set; } = "NODE_SEQ";

            /// <summary>
            /// 노드 디테일의 아이디 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_DETAIL_ID { get; set; } = "NODE_DETAIL_ID";

            /// <summary>
            /// 노드 디테일의 아이디가 동일할 때 시퀀스로 사용될 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_DETAIL_SEQ { get; set; } = "NODE_DETAIL_SEQ";

            /// <summary>
            /// 노드 디테일의 순서 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_DETAIL_ORDER { get; set; } = "NODE_DETAIL_ORDER";

            /// <summary>
            /// 노드 디테일의 타입 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_DETAIL_TYPE { get; set; } = "NODE_DETAIL_TYPE";

            /// <summary>
            /// 노드 디테일의 노트 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_DETAIL_NOTE { get; set; } = "NODE_DETAIL_NOTE";

            /// <summary>
            /// 컬럼 노드에서 테이이블 노드의 항목을 참조할 때 참조된 테이블 노드의 아이디 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_ID_REF { get; set; } = "NODE_ID_REF";

            /// <summary>
            /// 컬럼 노드에서 테이이블 노드의 항목을 참조할 때 참조된 테이블 노드의 시퀀스 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_SEQ_REF { get; set; } = "NODE_SEQ_REF";

            /// <summary>
            /// 노드 디테일의 데이터 타입 필드명을 설정하거나 반환합니다.(Data Ex: VARCHAR, INT, LONG, ...)
            /// </summary>
            public static string NODE_DETAIL_DATA_TYPE { get; set; } = "NODE_DETAIL_DATA_TYPE";

            /// <summary>
            /// 노드 디테일의 전체 데이터 타입 필드명을 설정하거나 반환합니다.(Data Ex: VARCHAR[20], NCHAR[5], ...)
            /// </summary>
            public static string NODE_DETAIL_DATA_TYPE_FULL { get; set; } = "NODE_DETAIL_DATA_TYPE_FULL";

            /// <summary>
            /// 노드 디테일의 코멘트 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_DETAIL_COMMENT { get; set; } = "NODE_DETAIL_COMMENT";

            /// <summary>
            /// 노드 디테일의 테이블 약어 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_DETAIL_TABLE_ALIAS { get; set; } = "NODE_DETAIL_TABLE_ALIAS";

            /// <summary>
            /// 노드 디테일의 원본 테이블 컬럼 순서 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_DETAIL_ORDINAL_POSITION { get; set; } = "NODE_DETAIL_ORDINAL_POSITION";

            /// <summary>
            /// 노드 디테일의 원본 테이블 PK 구분 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_DETAIL_IS_PRIMARY_KEY { get; set; } = "NODE_DETAIL_IS_PRIMARY_KEY";

            /// <summary>
            /// 노드 디테일의 첫 번째로 보여질 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_DETAIL_VIEW_COLUMN1 { get; set; } = "NODE_DETAIL_VIEW_COLUMN1";

            /// <summary>
            /// 노드 디테일의 두 번째로 보여질 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_DETAIL_VIEW_COLUMN2 { get; set; } = "NODE_DETAIL_VIEW_COLUMN2";
        }

        public static class RELATION
        {
            /// <summary>
            /// 릴레이션의 타입 필드명을 설정하거나 반환합니다.(Data Ex: I(INNER JOIN), L(LEFT JOIN), R(RIGHT JOIN))
            /// </summary>
            public static string RELATION_TYPE { get; set; } = "RELATION_TYPE";

            /// <summary>
            /// 릴레이션의 연산자 필드명을 설정하거나 반환합니다.(Data Ex: =, >=, <=, LIKE, ...)
            /// </summary>
            public static string RELATION_OPERATOR { get; set; } = "RELATION_OPERATOR";
        
            /// <summary>
            /// 릴레이션의 조인 값 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string RELATION_VALUE { get; set; } = "RELATION_VALUE";

            /// <summary>
            /// 릴레이션의 노트 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string RELATION_NOTE { get; set; } = "RELATION_NOTE";

            /// <summary>
            /// 릴레이션의 시작 노드 아이디 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_ID1 { get; set; } = "NODE_ID1";

            /// <summary>
            /// 릴레이션의 시작 노드 시퀀스 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_SEQ1 { get; set; } = "NODE_SEQ1";

            /// <summary>
            /// 릴레이션의 시작 노드 디테일 아이디 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_DETAIL_ID1 { get; set; } = "NODE_DETAIL_ID1";

            /// <summary>
            /// 릴레이션의 시작 노드 디테일 시퀀스 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_DETAIL_SEQ1 { get; set; } = "NODE_DETAIL_SEQ1";

            /// <summary>
            /// 릴레이션의 시작 노드 디테일 순서 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_DETAIL_ORDER1 { get; set; } = "NODE_DETAIL_ORDER1";

            /// <summary>
            /// 릴레이션의 목적 노드 시퀀스 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_ID2 { get; set; } = "NODE_ID2";

            /// <summary>
            /// 릴레이션의 목적 노드 시퀀스 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_SEQ2 { get; set; } = "NODE_SEQ2";

            /// <summary>
            /// 릴레이션의 목적 노드 디테일 아이디 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_DETAIL_ID2 { get; set; } = "NODE_DETAIL_ID2";

            /// <summary>
            /// 릴레이션의 목적 노드 디테일 시퀀스 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_DETAIL_SEQ2 { get; set; } = "NODE_DETAIL_SEQ2";

            /// <summary>
            /// 릴레이션의 목적 노드 디테일 순서 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_DETAIL_ORDER2 { get; set; } = "NODE_DETAIL_ORDER2";
        }
    }
}