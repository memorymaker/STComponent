using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.SqlClient;

namespace ST.DAC
{
    public class MssqlDac : DacBase
    {
        private SqlConnection Connection;
        private SqlCommand Command;
        private SqlDataAdapter DataAdapter;
        private SqlTransaction Transaction;

        public MssqlDac(string connectString)
        {
            try
            {
                Connection = new SqlConnection();
                Connection.ConnectionString = connectString;

                Command = new SqlCommand();
                Command.CommandType = CommandType.StoredProcedure;
                Command.Connection = Connection;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Override
        public override void BeginTran()
        {
            Transaction = Connection.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public override void CommitTran()
        {
            Transaction.Commit();
        }

        public override void RollbackTran()
        {
            Transaction.Rollback();
        }

        public override DataSet ExecuteQuery(string query)
        {
            DataSet resultDS = new DataSet();
            DataAdapter = new SqlDataAdapter(query, Connection);
            DataAdapter.Fill(resultDS);
            DataAdapter.Dispose();

            return resultDS;
        }

        public override int ExecuteNonQuery(string query)
        {
            Command.CommandText = query;
            int resultCount = Command.ExecuteNonQuery();

            return resultCount;
        }

        public override object ExecuteScalar(string query)
        {
            Command.CommandText = query;
            object resultFirstRowColumn = Command.ExecuteScalar();

            return resultFirstRowColumn;
        }

        public override DataSet ExecuteProcedure(string procedureName, Dictionary<string, object> parameters)
        {
            DataSet rs = new DataSet();
            SqlTransaction ObjTrans = null;

            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                }

                ObjTrans = Connection.BeginTransaction();
                Command.Transaction = ObjTrans;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = procedureName;

                foreach (KeyValuePair<string, object> pair in parameters)
                {
                    ProcedureParameter paramInfo = pair.Value as ProcedureParameter;
                    if (paramInfo == null)
                    {
                        Command.Parameters.Add(pair.Value);
                    }
                    else
                    {
                        if (paramInfo.DataType != null && paramInfo.DataType is SqlDbType)
                        {
                            Command.Parameters.Add(pair.Key, (SqlDbType)paramInfo.DataType);
                        }
                        else
                        {
                            Command.Parameters.Add(pair.Key);
                        }

                        if (paramInfo.Direction != null)
                        {
                            Command.Parameters[pair.Key].Direction = (ParameterDirection)paramInfo.Direction;
                        }

                        Command.Parameters[pair.Key].Value = paramInfo.Value;
                    }
                }

                DataAdapter = new SqlDataAdapter(Command);
                DataAdapter.Fill(rs);

                ObjTrans.Commit();
            }
            catch (SqlException ex)
            {
                rs = null;
                if (ObjTrans != null) ObjTrans.Rollback();
                throw (ex);
            }

            return rs;
        }

        public override int ExecuteNonProcedure(string procedureName, Dictionary<string, object> parameters)
        {
            int rs;
            SqlTransaction ObjTrans = null;

            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                }

                ObjTrans = Connection.BeginTransaction();
                Command.Transaction = ObjTrans;
                Command.CommandType = CommandType.StoredProcedure;
                Command.CommandText = procedureName;

                foreach(KeyValuePair<string, object> pair in parameters)
                {
                    ProcedureParameter paramInfo = pair.Value as ProcedureParameter;
                    if (paramInfo == null)
                    {
                        Command.Parameters.Add(pair.Value);
                    }
                    else
                    {
                        if (paramInfo.DataType != null && paramInfo.DataType is SqlDbType)
                        {
                            Command.Parameters.Add(pair.Key, (SqlDbType)paramInfo.DataType);
                        }
                        else
                        {
                            Command.Parameters.Add(pair.Key);
                        }

                        if (paramInfo.Direction != null)
                        {
                            Command.Parameters[pair.Key].Direction = (ParameterDirection)paramInfo.Direction;
                        }

                        Command.Parameters[pair.Key].Value = paramInfo.Value;
                    }
                }

                rs = Command.ExecuteNonQuery();
                ObjTrans.Commit();
            }
            catch (SqlException ex)
            {
                rs = -1;
                if (ObjTrans != null) ObjTrans.Rollback();
                throw (ex);
            }

            return rs;
        }

        public override void SetTimeOut(int timeOut)
        {
            Command.CommandTimeout = timeOut;
        }

        public override void Dispose()
        {
            if (Transaction != null) { Transaction.Dispose(); }

            Command.Dispose();
            if (Connection.State != ConnectionState.Closed) { Connection.Close(); }
            Connection.Dispose();
        }
        #endregion

        #region Method
        public int ExecuteNoneProcedure(DataSet ds)
        {
            return ExecuteNonProcedure(ds, CommandType.StoredProcedure);
        }

        public int ExecuteNonProcedure(DataSet ds, CommandType cmdType)
        {
            int intReturn = 0;
            int intReturnTemp = 0;
            SqlTransaction ObjTrans = null;

            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                }

                ObjTrans = Connection.BeginTransaction();
                Command.Transaction = ObjTrans;
                Command.CommandType = cmdType;

                foreach (DataTable dt in ds.Tables)
                {
                    Command.CommandText = dt.TableName;

                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ColumnName == "P_CNT")
                        {
                            Command.Parameters.Add(dc.ColumnName, SqlDbType.Int);
                        }
                        else
                        {
                            Command.Parameters.Add(dc.ColumnName, SqlDbType.NVarChar);
                        }
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (dr[dc].ToString() == "OUTPUT")
                            {
                                Command.Parameters[dc.ColumnName].Direction = ParameterDirection.Output;
                            }
                            else if (dr[dc].ToString() == "INOUT")
                            {
                                Command.Parameters[dc.ColumnName].Direction = ParameterDirection.InputOutput;
                            }
                            else
                            {
                                Command.Parameters[dc.ColumnName].Value = dr[dc];
                            }
                        }

                        #region
#if DEBUG
                        System.Diagnostics.Debug.WriteLine("ProcedureName : [" + Command.CommandText + "]");

                        for (int i = 0; i < Command.Parameters.Count; i++)
                        {
                            if (Command.Parameters[i].Direction == ParameterDirection.Input)
                            {
                                System.Diagnostics.Debug.WriteLine("ParamName : [" + Command.Parameters[i].ParameterName + "]"
                                    + ", ParamValue : [" + Command.Parameters[i].Value.ToString() + "]"
                                    + ", ParamType : [" + Command.Parameters[i].DbType.ToString() + "]"
                                    );
                            }
                        }
#endif
                        #endregion

                        intReturnTemp += Command.ExecuteNonQuery();

                        bool flag = true;
                        for (int i = 0; i < Command.Parameters.Count; i++)
                        {
                            if (Command.Parameters[i].Direction == ParameterDirection.Output)
                            {
                                if (Command.Parameters[i].ParameterName == "P_CNT")
                                {
                                    if (Command.Parameters[i].Value != DBNull.Value
                                        && Command.Parameters[i].Value != null
                                        && Command.Parameters[i].Value.ToString() != "null"
                                        && Command.Parameters[i].Value.ToString() != "")
                                    {
                                        dr[Command.Parameters[i].ParameterName] = Command.Parameters[i].Value;
                                        intReturn += int.Parse(Command.Parameters[i].Value.ToString());
                                        flag = false;
                                    }
                                }
                                else
                                {
                                    dr[Command.Parameters[i].ParameterName] = Command.Parameters[i].Value;
                                }
                            }
                            else if (Command.Parameters[i].Direction == ParameterDirection.InputOutput)
                            {
                                dr[Command.Parameters[i].ParameterName] = Command.Parameters[i].Value;
                            }
                        }

                        if (flag)
                        {
                            intReturn = intReturnTemp;
                        }
                    }
                    Command.Parameters.Clear();
                }
                ObjTrans.Commit();
            }
            catch (SqlException ex)
            {
                intReturn = -1;
                if (ObjTrans != null) ObjTrans.Rollback();
                throw (ex);
            }
            finally
            {
                Command.Dispose();
                if (Connection.State != ConnectionState.Closed)
                {
                    Connection.Close();
                }
                Connection.Dispose();
            }
            return intReturn;
        }

        public int ExecuteNonProcedure(string spName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable(spName);
            ds.Tables.Add(dt);
            return ExecuteNoneProcedure(ds);
        }

        public int ExecuteNonProcedureForFileUpload(DataSet ds)
        {
            int intReturn = 0;
            int intReturnTemp = 0;
            SqlTransaction ObjTrans = null;
            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                }

                ObjTrans = Connection.BeginTransaction();
                Command.Transaction = ObjTrans;

                foreach (DataTable dt in ds.Tables)
                {
                    Command.CommandText = dt.TableName;

                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ColumnName == "P_CNT")
                        {
                            Command.Parameters.Add(dc.ColumnName, SqlDbType.Int);
                        }
                        else
                        {
                            SqlParameter p = new SqlParameter();
                            p.ParameterName = dc.ColumnName;

                            if (dc.DataType == System.Type.GetType("System.Byte[]"))
                                p.SqlDbType = SqlDbType.Binary;

                            Command.Parameters.Add(p);
                        }
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (dr[dc].ToString() == "OUTPUT")
                            {
                                Command.Parameters[dc.ColumnName].Direction = ParameterDirection.Output;
                            }
                            else
                            {
                                Command.Parameters[dc.ColumnName].Value = dr[dc];
                            }
                        }

                        intReturnTemp += Command.ExecuteNonQuery();

                        bool flag = true;
                        for (int i = 0; i < Command.Parameters.Count; i++)
                        {
                            if (Command.Parameters[i].Direction == ParameterDirection.Output)
                            {
                                intReturn += int.Parse(Command.Parameters[i].Value.ToString());
                                flag = false;
                            }
                        }
                        if (flag)
                        {
                            intReturn = intReturnTemp;
                        }
                    }
                    Command.Parameters.Clear();
                }
                ObjTrans.Commit();
            }

            catch (Exception ex)
            {
                ObjTrans.Rollback();
                throw (ex);
            }
            finally
            {
                Command.Dispose();
                if (Connection.State != ConnectionState.Closed)
                {
                    Connection.Close();
                }
                Connection.Dispose();
            }
            return intReturn;
        }

        public DataSet GetDataSet(DataSet ds)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                foreach (DataTable dt in ds.Tables)
                {
                    Command.CommandText = dt.TableName;

                    foreach (DataColumn dc in dt.Columns)
                    {
                        Command.Parameters.AddWithValue(dc.ColumnName, null);
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            Command.Parameters[dc.ColumnName].Value = dr[dc];
                        }
                    }
                    DataAdapter = new SqlDataAdapter(Command);
                    DataAdapter.Fill(dsReturn);

                    Command.Parameters.Clear();
                    DataAdapter.Dispose();
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
            return dsReturn;
        }
        
        public DataSet ExecuteProcedure(string spName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable(spName);
            ds.Tables.Add(dt);
            return this.GetDataSet(ds);
        }

        public DataSet ExecuteQuery(string query, string tableName)
        {
            DataSet dsReturn = new DataSet();
            DataAdapter = new SqlDataAdapter(query, Connection);
            DataAdapter.Fill(dsReturn, tableName);
            DataAdapter.Dispose();

            return dsReturn;
        }

        public object ExecuteProcedureScalar(string spName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable(spName);
            ds.Tables.Add(dt);

            DataSet getds = GetDataSet(ds);
            object resultObj = (getds.Tables[0].Rows.Count > 0) ? (object)getds.Tables[0].Rows[0][0] : null;

            return resultObj;
        }
        #endregion

    }
}
