using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.CodeGenerator
{
    public partial class CodeGenerator
    {
        public static class TEMPLATE
        {
            /// <summary>
            /// 템플릿 시퀀스 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string TEMPLATE_SEQ { get; set; } = "TEMPLATE_SEQ";

            /// <summary>
            /// 템플릿 타이틀 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string TEMPLATE_TITLE { get; set; } = "TEMPLATE_TITLE";

            /// <summary>
            /// 템플릿 내용 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string TEMPLATE_CONTENT { get; set; } = "TEMPLATE_CONTENT";

            /// <summary>
            /// 템플릿 결과 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string TEMPLATE_RESULT { get; set; } = "TEMPLATE_RESULT";

            /// <summary>
            /// 템플릿 스타일 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string TEMPLATE_STYLE { get; set; } = "TEMPLATE_STYLE";

            /// <summary>
            /// 템플릿 순서 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string TEMPLATE_SORT { get; set; } = "TEMPLATE_SORT";

            /// <summary>
            /// 템플릿 노트 필드명을 설정하거나 반환합니다.(현재 사용 안 함)
            /// </summary>
            public static string TEMPLATE_NOTE { get; set; } = "TEMPLATE_NOTE";

            /// <summary>
            /// 템플릿 선택 여부 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string TEMPLATE_SELECTED { get; set; } = "TEMPLATE_SELECTED";
        }

        public static class NODE
        {
            /// <summary>
            /// 컬럼 노드에서 테이이블 노드의 항목을 참조할 때 참조된 테이블 노드의 아이디 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_ID_REF { get; set; } = "NODE_ID_REF";

            /// <summary>
            /// 컬럼 노드에서 테이이블 노드의 항목을 참조할 때 참조된 테이블 노드의 시퀀스 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_SEQ_REF { get; set; } = "NODE_SEQ_REF";

            /// <summary>
            /// 노드 디테일의 아이디 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_DETAIL_ID { get; set; } = "NODE_DETAIL_ID";

            /// <summary>
            /// 노드 디테일의 참조된 테이블 약어 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_DETAIL_TABLE_ALIAS { get; set; } = "NODE_DETAIL_TABLE_ALIAS";
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
            /// 릴레이션의 시작 노드 디테일 테이블 약어 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_DETAIL_TABLE_ALIAS1 { get; set; } = "NODE_DETAIL_TABLE_ALIAS1";

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

            /// <summary>
            /// 릴레이션의 목적 노드 디테일 테이블 약어 필드명을 설정하거나 반환합니다.
            /// </summary>
            public static string NODE_DETAIL_TABLE_ALIAS2 { get; set; } = "NODE_DETAIL_TABLE_ALIAS2";
        }
    }
}
