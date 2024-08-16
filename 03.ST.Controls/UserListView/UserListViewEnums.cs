using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Controls
{
    public enum UserListAlignType
    {
        None = 0, Left, Right, Center
    }

    public enum UserListAutoSizeType
    {
        /// <summary>
        /// 컬럼 크기의 자동 조정을 사용하지 않습니다.
        /// </summary>
        None = 0,
        /// <summary>
        /// 컬럼 크기의 자동 조정을 좌측 우선으로 설정합니다.
        /// </summary>
        LeftFirst,
        /// <summary>
        /// 컬럼 크기의 자동 조정을 공평하도록 설정합니다.
        /// </summary>
        Fairly
    }
}
