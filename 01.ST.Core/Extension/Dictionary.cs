using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml.Schema;

namespace ST.Core
{
    public static partial class Extensions
    {
        /// <summary>
        /// 이 인스턴스의 데이터를 dataTable의 DataRow(NewRow) 형태로 변환 후 반환합니다.
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static DataRow ToDataRow(this Dictionary<string, string> dictionary, DataTable dataTable)
        {
            DataRow dr = dataTable.NewRow();
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                dr[pair.Key] = pair.Value;
            }
            return dr;
        }

        
    }
}
