using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml.Schema;
using System.Reflection;

namespace ST.Core
{
    public static partial class Extensions
    {
        /// <summary>
        /// 이 인스턴스를 Dictionary 형태로 반환합니다.
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary(this DataRow dataRow)
        {
            Dictionary<string, object> rs = new Dictionary<string, object>();
            foreach (DataColumn column in dataRow.Table.Columns)
            {
                rs.Add(column.ColumnName, dataRow[column]);
            }
            return rs;
        }

        /// <summary>
        /// 이 인스턴스를 TypeParam T 형태로 반환합니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public static T ToObject<T>(this DataRow dataRow)
        where T : new()
        {
            T item = new T();

            foreach (DataColumn column in dataRow.Table.Columns)
            {
                PropertyInfo property = GetProperty(typeof(T), column.ColumnName);

                if (property != null && dataRow[column] != DBNull.Value && dataRow[column].ToString() != "NULL")
                {
                    property.SetValue(item, ChangeType(dataRow[column], property.PropertyType), null);
                }
            }

            return item;
        }

        /// <summary>
        /// 이 인스턴스의 keys(컬럼명)의 값을 각각 values의 값으로 입력합니다.
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="keys"></param>
        /// <param name="values"></param>
        public static void SetValues(this DataRow dataRow, string[] keys, object[] values)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                dataRow[keys[i]] = values[i];
            }
        }

        /// <summary>
        /// 이 인스턴스의 값을 기준으로 생성한 SELECT 쿼리문을 반환합니다. ("SELECT * FROM [TableName] WHERE [키 컬럼 정보 | ColumnName1 = Value1 AND ...]")
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public static string GetSelectString(this DataRow dataRow)
        {
            string rs = string.Format("SELECT * FROM [{0}] WHERE {1}"
                , dataRow.Table.TableName
                , dataRow.GetQueryLineOfWhereByKey()
            );
            return rs;
        }

        /// <summary>
        /// 이 인스턴스의 값을 기준으로 생성한 SELECT 쿼리문을 반환합니다. ("SELECT * FROM [TableName] WHERE [keyColumns 컬럼 정보 | ColumnName1 = Value1 AND ...]")
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="keyColumns"></param>
        /// <returns></returns>
        public static string GetSelectString(this DataRow dataRow, string[] keyColumns)
        {
            string rs = string.Format("SELECT * FROM [{0}] WHERE {1}"
                , dataRow.Table.TableName
                , dataRow.GetQueryLineOfWhere(keyColumns)
            );
            return rs;
        }

        /// <summary>
        /// 이 인스턴스의 값을 기준으로 생성한 SELECT COUNT(*) 쿼리문을 반환합니다. ("SELECT COUNT(*) FROM [TableName] WHERE [키 컬럼 정보 | ColumnName1 = Value1 AND ...]")
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public static string GetSelectCountString(this DataRow dataRow)
        {
            string rs = string.Format("SELECT COUNT(*) CNT FROM [{0}] WHERE {1}"
                , dataRow.Table.TableName
                , dataRow.GetQueryLineOfWhereByKey()
            );
            return rs;
        }

        /// <summary>
        /// 이 인스턴스의 값을 기준으로 생성한 SELECT COUNT(*) 쿼리문을 반환합니다. ("SELECT COUNT(*) FROM [TableName] WHERE [keyColumns 컬럼 정보 | ColumnName1 = Value1 AND ...]")
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="keyColumns"></param>
        /// <returns></returns>
        public static string GetSelectCountString(this DataRow dataRow, string[] keyColumns)
        {
            string rs = string.Format("SELECT COUNT(*) CNT FROM [{0}] WHERE {1}"
                , dataRow.Table.TableName
                , dataRow.GetQueryLineOfWhere(keyColumns)
            );
            return rs;
        }

        /// <summary>
        /// 이 인스턴스의 값을 기준으로 생성한 INSERT 쿼리문을 반환합니다. ("INSERT INTO [TableName] ([ColumnName1], [ColumnName2], ...) VALUES (Value1, Value2, ...)")
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public static string GetInsertString(this DataRow dataRow)
        {
            string rs = string.Format("INSERT INTO [{0}] ({1}) VALUES ({2})"
                , dataRow.Table.TableName
                , dataRow.Table.GetQueryStringOfColumns()
                , dataRow.GetQueryLineOfValues()
            );
            return rs;
        }

        /// <summary>
        /// 이 인스턴스의 값을 기준으로 생성한 UPDATE 쿼리문을 반환합니다. ("UPDATE [TableName] SET [ColumnName1] = Value1, ... WHERE [키 컬럼 정보 | ColumnName1 = Value1 AND ...]")
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public static string GetUpdateString(this DataRow dataRow)
        {
            string rs = string.Format("UPDATE [{0}] SET {1} WHERE {2}"
                , dataRow.Table.TableName
                , dataRow.GetQueryLineOfUpdate()
                , dataRow.GetQueryLineOfWhereByKey()
            );
            return rs;
        }

        /// <summary>
        /// 이 인스턴스의 값을 기준으로 생성한 UPDATE 쿼리문을 반환합니다. ("UPDATE [TableName] SET [ColumnName1] = Value1, ... WHERE [keyColumns 컬럼 정보 | ColumnName1 = Value1 AND ...]")
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="keyColumns"></param>
        /// <returns></returns>
        public static string GetUpdateString(this DataRow dataRow, string[] keyColumns)
        {
            string rs = string.Format("UPDATE [{0}] SET {1} WHERE {2}"
                , dataRow.Table.TableName
                , dataRow.GetQueryLineOfUpdate()
                , dataRow.GetQueryLineOfWhere(keyColumns)
            );
            return rs;
        }

        /// <summary>
        /// 이 인스턴스의 값을 기준으로 생성한 UPDATE 쿼리문을 반환합니다. ("DELETE FROM [TableName] WHERE [키 컬럼 정보 | ColumnName1 = Value1 AND ...]")
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public static string GetDeleteString(this DataRow dataRow)
        {
            string rs = string.Format("DELETE FROM [{0}] WHERE {1}"
                , dataRow.Table.TableName
                , dataRow.GetQueryLineOfWhereByKey()
            );
            return rs;
        }

        /// <summary>
        /// 이 인스턴스의 값을 기준으로 생성한 UPDATE 쿼리문을 반환합니다. ("DELETE FROM [TableName] WHERE [keyColumns 컬럼 정보 | ColumnName1 = Value1 AND ...]")
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="keyColumns"></param>
        /// <returns></returns>
        public static string GetDeleteString(this DataRow dataRow, string[] keyColumns)
        {
            string rs = string.Format("DELETE FROM [{0}] WHERE {1}"
                , dataRow.Table.TableName
                , dataRow.GetQueryLineOfWhere(keyColumns)
            );
            return rs;
        }

        /// <summary>
        /// 이 인스턴스의 값을 기준으로 생성한 Values 영역 쿼리문을 반환합니다. ("Value1, Value2, ...")
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private static string GetQueryLineOfValues(this DataRow dataRow)
        {
            string[] rs = new string[dataRow.Table.Columns.Count];
            for (int i = 0; i < dataRow.Table.Columns.Count; i++)
            {
                rs[i] = dataRow.GetQueryValueString(i);
            }
            return string.Join(", ", rs);
        }

        /// <summary>
        /// 이 인스턴스의 값을 기준으로 생성한 Update 영역 쿼리문을 반환합니다. ("[ColumnName1] = Value1, [ColumnName2] = Value2, ...")
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private static string GetQueryLineOfUpdate(this DataRow dataRow)
        {
            List<string> rs = new List<string>();
            for (int i = 0; i < dataRow.Table.Columns.Count; i++)
            {
                rs.Add("[" + dataRow.Table.Columns[i].ColumnName + "] = " + dataRow.GetQueryValueString(i));
            }
            return string.Join(", ", rs);
        }

        /// <summary>
        /// 이 인스턴스의 값을 기준으로 생성한 Where 영역 쿼리문을 반환합니다. ("WHERE [keyColumns 컬럼 정보 | ColumnName1 = Value1 AND ...]")
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="keyColumns"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string GetQueryLineOfWhere(this DataRow dataRow, string[] keyColumns, string type = "AND")
        {
            List<string> rs = new List<string>();
            for (int i = 0; i < keyColumns.Length; i++)
            {
                rs.Add("[" + keyColumns[i] + "] = " + dataRow.GetQueryValueString(i));
            }
            return string.Join(" " + type + " ", rs);
        }

        /// <summary>
        /// 이 인스턴스의 값을 기준으로 생성한 Where 영역 쿼리문을 반환합니다. ("WHERE [키 컬럼 정보 | ColumnName1 = Value1 AND ...]")
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string GetQueryLineOfWhereByKey(this DataRow dataRow, string type = "AND")
        {
            List<string> rs = new List<string>();
            for (int i = 0; i < dataRow.Table.PrimaryKey.Length; i++)
            {
                rs.Add("[" + dataRow.Table.PrimaryKey[i] + "] = " + dataRow.GetQueryValueString(dataRow.Table.PrimaryKey[i].ColumnName));
            }
            return string.Join(" " + type + " ", rs);
        }

        /// <summary>
        /// dataRow의 columnIndex 컬럼의 값을 쿼리문에 사용될 Value 문자열 형태로 변환합니다. ("Value" -> "'Value'", 1 -> 1, DateTime -> "yyyy-MM-dd HH:mm:ss", null -> NULL)
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private static string GetQueryValueString(this DataRow dataRow, int columnIndex)
        {
            return GetQueryValueStringProc(dataRow, columnIndex);
        }

        /// <summary>
        /// dataRow의 columnName 컬럼의 값을 쿼리문에 사용될 Value 문자열 형태로 변환합니다. ("Value" -> "'Value'", 1 -> 1, DateTime -> "yyyy-MM-dd HH:mm:ss", null -> NULL)
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private static string GetQueryValueString(this DataRow dataRow, string columnName)
        {
            int columnIndex = dataRow.Table.Columns.IndexOf(columnName);
            return GetQueryValueStringProc(dataRow, columnIndex);
        }

        /// <summary>
        /// dataRow의 columnIndex 컬럼의 값을 쿼리문에 사용될 Value 문자열 형태로 변환합니다. ("Value" -> "'Value'", 1 -> 1, DateTime -> "yyyy-MM-dd HH:mm:ss", null -> NULL)
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private static string GetQueryValueStringProc(this DataRow dataRow, int columnIndex)
        {
            string value;
            if (dataRow[columnIndex] == DBNull.Value || dataRow[columnIndex] == null)
            {
                value = "NULL";
            }
            else
            {
                if (dataRow.Table.Columns[columnIndex].DataType == typeof(string))
                {
                    value = "'" + dataRow[columnIndex].ToString().Replace("'", "''") + "'";
                }
                else if (dataRow.Table.Columns[columnIndex].DataType == typeof(DateTime))
                {
                    value = ((DateTime)dataRow[columnIndex]).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    value = dataRow[columnIndex].ToString();
                }
            }
            return value;
        }
    }
}
