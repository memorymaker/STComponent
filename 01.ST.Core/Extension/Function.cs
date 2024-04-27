using ST.Core.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core
{
    public static partial class Extensions
    {
        /// <summary>
        /// 타입의 attributeName로 PropertyInfo를 반환합니다.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        private static PropertyInfo GetProperty(Type type, string attributeName)
        {
            PropertyInfo property = type.GetProperty(attributeName);

            if (property != null)
            {
                return property;
            }
            else
            {
                return type.GetProperties()
                     .Where(p => p.IsDefined(typeof(DisplayAttribute), false) && p.GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().Single().Name == attributeName)
                     .FirstOrDefault();
            }
        }

        /// <summary>
        /// value의 값을 type으로 변환 후 반환합니다.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ChangeType(object value, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                return Convert.ChangeType(value, Nullable.GetUnderlyingType(type));
            }

            return Convert.ChangeType(value, type);
        }

        /// <summary>
        /// refData 리스트의 tableNameIndex 인덱스 값(테이블명)과 tableSeqIndex(테이블 인덱스)으로 Alias 생성 후 tableAliasIndex에 저장합니다.
        /// </summary>
        /// <param name="refData"></param>
        /// <param name="tableNameIndex"></param>
        /// <param name="tableSeqIndex"></param>
        /// <param name="tableAliasIndex"></param>
        public static void SetAlias(ref List<object[]> refData, int tableNameIndex, int tableSeqIndex, int tableAliasIndex)
        {
            // 0: NODE_ID_REF, 1:NODE_SEQ_REF, 2:Alias, 3:Alias Sort
            for (int k = 0; k < refData.Count; k++)
            {
                string tableName = refData[k][tableNameIndex].ToString();
                string rsAlias = tableName.ToAlias(refData, tableNameIndex, tableAliasIndex, k);

                // Numbering
                int rearNumber = 0;
                for (int i = 0; i < k; i++)
                {
                    if (refData[i][tableNameIndex].ToString() == refData[k][tableNameIndex].ToString() && refData[i][tableSeqIndex].ToInt() != refData[k][tableSeqIndex].ToInt())
                    {
                        rearNumber++;
                        refData[i][2] = rsAlias + rearNumber.ToString();
                    }
                }

                refData[k][tableAliasIndex] = rearNumber == 0 ? rsAlias : rsAlias + (rearNumber + 1).ToString();
            }
        }
    }
}
