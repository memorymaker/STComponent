using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core
{
    public static partial class Extensions
    {
        /// <summary>
        /// 이 인스턴스의 값을 해당하는 int 값으로 변환합니다.
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int ToInt(this object _this)
        {
            int rs;
            bool canParse = int.TryParse(_this.ToString(), out rs);
            if (!canParse)
            {
                throw new Exception("Can not convert to int.");
            }
            return rs;
        }

        /// <summary>
        /// 이 인스턴스의 값이 int로 변환될 수 있는지 Boolean로 반환합니다.
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static bool IsInt(this object _this)
        {
            return int.TryParse(_this.ToString(), out _);
        }

        /// <summary>
        /// 이 인스턴스의 값을 해당하는 long 값으로 변환합니다.
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static long ToLong(this object _this)
        {
            long rs;
            bool canParse = long.TryParse(_this.ToString(), out rs);
            if (!canParse)
            {
                throw new Exception("Can not convert to long.");
            }
            return rs;
        }

        /// <summary>
        /// 이 인스턴스의 값이 long으로 변환될 수 있는지 Boolean로 반환합니다.
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static bool IsLong(this object _this)
        {
            return long.TryParse(_this.ToString(), out _);
        }

        /// <summary>
        /// 이 인스턴스의 값을 해당하는 float 값으로 변환합니다.
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static float ToFloat(this object _this)
        {
            float rs;
            bool canParse = float.TryParse(_this.ToString(), out rs);
            if (!canParse)
            {
                throw new Exception("Can not convert to long.");
            }
            return rs;
        }

        /// <summary>
        /// 이 인스턴스의 값이 float으로 변환될 수 있는지 Boolean로 반환합니다.
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static bool IsFloat(this object _this)
        {
            return float.TryParse(_this.ToString(), out _);
        }

        /// <summary>
        /// 이 인스턴스의 값을 해당하는 double 값으로 변환합니다.
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static double ToDouble(this object _this)
        {
            double rs;
            bool canParse = double.TryParse(_this.ToString(), out rs);
            if (!canParse)
            {
                throw new Exception("Can not convert to long.");
            }
            return rs;
        }

        /// <summary>
        /// 이 인스턴스의 값이 double로 변환될 수 있는지 Boolean로 반환합니다.
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static bool IsDouble(this object _this)
        {
            return double.TryParse(_this.ToString(), out _);
        }

        /// <summary>
        /// 이 인스턴스의 값을 해당하는 DateTime 값으로 변환합니다.
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static DateTime ToDateTime(this object _this)
        {
            DateTime rs;
            bool canParse = DateTime.TryParse(_this.ToString(), out rs);
            if (!canParse)
            {
                throw new Exception("Can not convert to long.");
            }
            return rs;
        }

        /// <summary>
        /// 이 인스턴스의 값이 DateTime으로 변환될 수 있는지 Boolean로 반환합니다.
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static bool IsDateTime(this object _this)
        {
            return DateTime.TryParse(_this.ToString(), out _);
        }
    }
}