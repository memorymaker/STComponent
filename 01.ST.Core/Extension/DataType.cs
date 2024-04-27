using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core
{
    public static partial class Extensions
    {
        /// <summary>
        /// 이 인스턴스의 값을 반올림 후 int 값으로 변환합니다.
        /// </summary>
        /// <param name="floatValue"></param>
        /// <returns></returns>
        public static int ToInt(this float floatValue)
        {
            return Convert.ToInt32(Math.Round(floatValue));
        }
    }
}
