using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core
{
    public static partial class Extensions
    {
        /// <summary>
        /// 이 인스턴스의 데이터를 DataTable 형태로 반환합니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        /// <summary>
        /// 이 인스턴스의 데이터를 새 DataTable의 지정된 columnName으로 입력한 후 반환합니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> list, string columnName)
        {
            DataTable rs = new DataTable();
            rs.Columns.Add(columnName);
            foreach (object item in list)
            {
                rs.Rows.Add(new object[] { item });
            }
            return rs;
        }

        /// <summary>
        /// 이 인스턴스의 데이터를 searchText로 검색(대/소문자 무시) 후 결과 List를 반환합니다.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public static List<string> Search(this List<string> list, string searchText)
        {
            List<string> rsData = null;
            if (searchText.Length == 0)
            {
                rsData = list;
            }
            else
            {
                // Get indexOrder int[0]: index, int[1]: sort
                List<int[]> indexOrder = new List<int[]>();
                for (int i = 0; i < list.Count; i++)
                {
                    int searchTextIndex = list[i].IndexOf(searchText, StringComparison.OrdinalIgnoreCase);
                    if (searchTextIndex >= 0)
                    {
                        int[] node = new int[] { i, searchTextIndex };

                        if (indexOrder.Count == 0)
                        {
                            indexOrder.Add(node);
                        }
                        else
                        {
                            for (int k = 0; k < indexOrder.Count; k++)
                            {
                                if (searchTextIndex < indexOrder[k][1])
                                {
                                    indexOrder.Insert(k, node);
                                    break;
                                }
                                else if (searchTextIndex == indexOrder[k][1])
                                {
                                    string nodeString = list[i];
                                    string targetString = list[indexOrder[k][0]];
                                    if (nodeString == targetString)
                                    {
                                        indexOrder.Insert(k, node);
                                        break;
                                    }
                                    else
                                    {
                                        bool isNodeSmall = false;
                                        int maxM = Math.Min(nodeString.Length, targetString.Length);
                                        for (int m = 0; m < maxM; m++)
                                        {
                                            if (nodeString[m] < targetString[m])
                                            {
                                                isNodeSmall = true;
                                            }
                                        }

                                        if (isNodeSmall)
                                        {
                                            indexOrder.Insert(k, node);
                                            break;
                                        }
                                    }
                                }

                                if (k == indexOrder.Count - 1)
                                {
                                    indexOrder.Add(node);
                                    break;
                                }
                            }
                        }
                    }
                }

                // Set rsDataTable
                if (indexOrder.Count > 0)
                {
                    rsData = new List<string>();
                    foreach (int[] node in indexOrder)
                    {
                        rsData.Add(list[node[0]]);
                    }
                }
            }

            return rsData;
        }
    }
}
