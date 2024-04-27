using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core
{
    /// <summary>
    /// StartingValue과 EndValue를 가지는 Range 클래스입니다.
    /// </summary>
    public class Range
    {
        /// <summary>
        /// 범위의 시작 값입니다.
        /// </summary>
        public int StartingValue;

        /// <summary>
        /// 범위의 끝 값입니다.
        /// </summary>
        public int EndValue;

        /// <summary>
        /// 시작과 끝의 차를 반환합니다.
        /// </summary>
        public int Interval => EndValue - StartingValue;

        /// <summary>
        /// 시작과 끝 값으로 새 인스턴스를 생성합니다.
        /// </summary>
        /// <param name="startingPoint"></param>
        /// <param name="endPoint"></param>
        public Range(int startingPoint, int endPoint)
        {
            StartingValue = startingPoint;
            EndValue = endPoint;
        }

        /// <summary>
        /// 시작과 끝 값을 입력된 offset 만큼 이동합니다.
        /// </summary>
        /// <param name="offset"></param>
        public void Offset(int offset)
        {
            StartingValue += offset;
            EndValue += offset;
        }

        /// <summary>
        /// 시작 값은 offsetStartingPoint, 끝 값은 offsetEndPoint 만큼 이동합니다.
        /// </summary>
        /// <param name="offsetStartingPoint"></param>
        /// <param name="offsetEndPoint"></param>
        public void Offset(int offsetStartingPoint, int offsetEndPoint)
        {
            StartingValue += offsetStartingPoint;
            EndValue += offsetEndPoint;
        }

        /// <summary>
        /// 입력된 value 값이 시작 값과 끝 값 사이거나 일치하면 True를 반환하고 그렇지 않으면 False를 반환합니다.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(int value)
        {
            return StartingValue <= value && value <= EndValue;
        }
    }
}
