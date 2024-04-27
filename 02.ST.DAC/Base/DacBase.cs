using ST.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ST.DAC
{
    public abstract class DacBase : IDisposable
    {
        #region Abstract
        public abstract void BeginTran();

        public abstract void CommitTran();

        public abstract void RollbackTran();

        public abstract DataSet ExecuteQuery(string query);

        public abstract int ExecuteNonQuery(string query);

        public abstract object ExecuteScalar(string query);

        public abstract DataSet ExecuteProcedure(string procedureName, Dictionary<string, object> parameters);

        public abstract int ExecuteNonProcedure(string procedureName, Dictionary<string, object> parameters);

        public abstract void SetTimeOut(int timeOut);

        public abstract void Dispose();
        #endregion

        #region Public
        public DataSet SelectDataTable(DataTable dataTable)
        {
            return ExecuteQuery(dataTable.Rows[0].GetSelectString());
        }

        public DataSet SelectDataTable(DataTable dataTable, string[] keyColumns)
        {
            return ExecuteQuery(dataTable.Rows[0].GetSelectString(keyColumns));
        }

        public int InsertDataTable(DataTable dataTable)
        {
            int rs = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                rs += ExecuteNonQuery(row.GetInsertString());
            }
            return rs;
        }

        public int UpdateDataTable(DataTable dataTable)
        {
            int rs = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                rs += ExecuteNonQuery(row.GetUpdateString());
            }
            return rs;
        }

        public int UpdateDataTable(DataTable dataTable, string[] keyColumns)
        {
            int rs = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                rs += ExecuteNonQuery(row.GetUpdateString(keyColumns));
            }
            return rs;
        }

        public int SetDataTable(DataTable dataTable)
        {
            int rs = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                int cnt = Convert.ToInt32(ExecuteScalar(row.GetSelectCountString()));
                if (cnt == 0)
                {
                    rs += ExecuteNonQuery(row.GetInsertString());
                }
                else
                {
                    rs += ExecuteNonQuery(row.GetUpdateString());
                }
            }
            return rs;
        }

        public int SetDataTable(DataTable dataTable, string[] keyColumns)
        {
            int rs = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                int cnt = Convert.ToInt32(ExecuteScalar(row.GetSelectCountString(keyColumns)));
                if (cnt == 0)
                {
                    rs += ExecuteNonQuery(row.GetInsertString());
                }
                else
                {
                    rs += ExecuteNonQuery(row.GetUpdateString(keyColumns));
                }
            }
            return rs;
        }

        public int DeleteDataTable(DataTable dataTable)
        {
            int rs = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                rs += ExecuteNonQuery(row.GetDeleteString());
            }
            return rs;
        }

        public int DeleteDataTable(DataTable dataTable, string[] keyColumns)
        {
            int rs = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                rs += ExecuteNonQuery(row.GetDeleteString(keyColumns));
            }
            return rs;
        }
        #endregion
    }
}
