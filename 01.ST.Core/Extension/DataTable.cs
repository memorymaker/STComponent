using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml.Schema;
using System.Collections;

namespace ST.Core
{
    public static partial class Extensions
    {
        /// <summary>
        /// DataTable에 컬럼을 추가합니다. ex) dataTable.AddColummns("{S}Field1 {F}Field2"); // FIeld1: string, Field2: float({f}, {F} 둘 다 사용 가능. 대소문자 구분하지 않음)
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="columns"></param>
        public static void AddColumns(this DataTable dataTable, string columns)
        {
            string[] arrColumns = columns.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            dataTable.AddColumns(arrColumns);
        }

		/// <summary>
		/// DataTable에 컬럼을 추가 후 object[] 형태의 rowData를 입력합니다. ex) dataTable.AddColummns("{S}Field1 {F}Field2", rowData); // FIeld1: string, Field2: float({f}, {F} 둘 다 사용 가능. 대소문자 구분하지 않음)
		/// </summary>
		/// <param name="dataTable"></param>
		/// <param name="columns"></param>
		/// <param name="rowData"></param>
		public static void AddColumns(this DataTable dataTable, string columns, object[] rowData)
		{
            dataTable.AddColumns(columns);
            dataTable.Rows.Add(rowData);
		}

		/// <summary>
		/// DataTable에 컬럼을 추가합니다. ex) dataTable.AddColummns(new string[]{"{S}Field1", "{F}Field2"}); // FIeld1: string, Field2: float({f}, {F} 둘 다 사용 가능. 대소문자 구분하지 않음)
		/// </summary>
		/// <param name="dataTable"></param>
		/// <param name="columns"></param>
		public static void AddColumns(this DataTable dataTable, string[] columns)
        {
            foreach (string columnRaw in columns)
            {
                string columnName = columnRaw;
                string columnDataType = "O";

                var dataTypeEndPoint = columnRaw.IndexOf("}");
                if (dataTypeEndPoint >= 0)
                {
                    if (dataTypeEndPoint == 2)
                    {
                        columnDataType = columnRaw.Substring(1, 1);
                        columnName = columnRaw.Substring(3, columnRaw.Length - 3);
                    }
                    else
                    {
                        throw new Exception(string.Format("Error DataTable.AddColummns - columns 파라미터의 \"{데이터타입}필드명\" 형식이 잘못 입력되었습니다.  - {0}", columns));
                    }
                }

                switch (columnDataType.ToUpper())
                {
                    case "O": dataTable.Columns.Add(columnName, typeof(object)); break;
                    case "S": dataTable.Columns.Add(columnName, typeof(string)); break;
                    case "I": dataTable.Columns.Add(columnName, typeof(int)); break;
                    case "L": dataTable.Columns.Add(columnName, typeof(long)); break;
                    case "F": dataTable.Columns.Add(columnName, typeof(float)); break;
                    case "E": dataTable.Columns.Add(columnName, typeof(decimal)); break;
                    case "D": dataTable.Columns.Add(columnName, typeof(double)); break;
                    case "B": dataTable.Columns.Add(columnName, typeof(bool)); break;
                    case "T": dataTable.Columns.Add(columnName, typeof(DateTime)); break;
                    default:
                        throw new Exception(string.Format("Error DataTable.AddColummns - columns 파라미터의 데이터 타입이 잘못 입력되었습니다. - {0} - {1}", columns, columnDataType));
                }
            }
        }

		/// <summary>
		/// DataTable에 컬럼을 추가 후 object[] 형태의 rowData를 입력합니다. ex) dataTable.AddColummns(new string[]{"{S}Field1", "{F}Field2"}, rowData); // FIeld1: string, Field2: float({f}, {F} 둘 다 사용 가능. 대소문자 구분하지 않음)
		/// </summary>
		/// <param name="dataTable"></param>
		/// <param name="columns"></param>
		/// <param name="rowData"></param>
		public static void AddColumns(this DataTable dataTable, string[] columns, object[] rowData)
        {
			dataTable.AddColumns(columns);
			dataTable.Rows.Add(rowData);
		}

		/// <summary>
		/// 다른 DataTable에 속해있는 DataRow를 필드명에 맞춰 추가합니다. 대상 DataTable에 필드가 존재하지 않으면 DBNull.Value로 입됩니다.
		/// </summary>
		/// <param name="dataTable"></param>
		/// <param name="dataRow"></param>
		public static void AddRow(this DataTable dataTable, DataRow dataRow)
        {
            object[] values = new object[dataTable.Columns.Count];
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                if (dataRow.Table.Columns.Contains(dataTable.Columns[i].ColumnName))
                {
                    values[i] = dataRow[dataTable.Columns[i].ColumnName];
                }
                else
                {
                    values[i] = DBNull.Value;
                }
            }
            dataTable.Rows.Add(values);
        }

        /// <summary>
        /// DataTable의 컬럼들을 "[Column1], [Column2], ..." 형태로 반환합니다.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static string GetQueryStringOfColumns(this DataTable dataTable)
        {
            string[] rs = new string[dataTable.Columns.Count];
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                rs[i] = "[" + dataTable.Columns[i].ColumnName + "]";
            }
            return string.Join(", ", rs);
        }

        /// <summary>
        ///  DataTable의 targetColumnIndex 필드의 데이터를 searchText로 검색(대/소문자 무시) 후 결과 DataTable을 반환합니다.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="searchText"></param>
        /// <param name="targetColumnIndex"></param>
        /// <returns></returns>
        public static DataTable Search(this DataTable dataTable, string searchText, int targetColumnIndex = 0)
        {
            DataTable rsDataTable = null;
            if (searchText.Length == 0)
            {
                rsDataTable = dataTable;
            }
            else
            {
                var rows = dataTable.Select($"{dataTable.Columns[targetColumnIndex].ColumnName} LIKE '%{searchText}%'");
                if (rows.Length > 0)
                {
                    rsDataTable = dataTable.Clone();

                    // Get indexOrder int[0]: index, int[1]: sort
                    List<int[]> indexOrder = new List<int[]>();
                    for (int i = 0; i < rows.Length; i++)
                    {
                        int searchTextIndex = rows[i][targetColumnIndex].ToString().IndexOf(searchText);
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
                                    string nodeString = rows[i][targetColumnIndex].ToString();
                                    string targetString = rows[indexOrder[k][0]][targetColumnIndex].ToString();
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

                    // Set rsDataTable
                    foreach (int[] node in indexOrder)
                    {
                        rsDataTable.Rows.Add(rows[node[0]].ItemArray);
                    }
                }
            }

            return rsDataTable;
        }
    }
}