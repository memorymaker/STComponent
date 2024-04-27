using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Oracle.DataAccess.Client;

namespace ST.DAC
{
    public class OracleDac : DacBase
    {
        private OracleConnection Connection;
        private OracleCommand Command;
        private OracleDataAdapter DataAdapter;
        private OracleTransaction Transaction;

        public OracleDac(string connectString)
        {
            try
            {
                Connection = new OracleConnection();
                Connection.ConnectionString = connectString;

                Command = new OracleCommand();
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
            DataSet dsReturn = new DataSet();
            try
            {
                DataAdapter = new OracleDataAdapter(query, Connection);
                DataAdapter.Fill(dsReturn, "DataTable1");
                DataAdapter.Dispose();
            }
            catch (Exception e)
            {
                throw (e);
            }

            return dsReturn;
        }

        public override int ExecuteNonQuery(string query)
        {
            int retValue;
            OracleCommand cmd = null;
            OracleTransaction ObjTrans = null;

            try
            {
                cmd = new OracleCommand(query, Connection);
                if (Connection.State != ConnectionState.Open)
                {
                    cmd.Connection.Open();
                }

                ObjTrans = Connection.BeginTransaction();
                cmd.Transaction = ObjTrans;

                retValue = cmd.ExecuteNonQuery();
                ObjTrans.Commit();
            }
            catch (OracleException ex)
            {
                ObjTrans.Rollback();
                throw (ex);
            }
            finally
            {
                cmd.Dispose();
                if (Connection.State != ConnectionState.Closed)
                {
                    Connection.Close();
                }
                Connection.Dispose();
            }
            return retValue;
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
            OracleTransaction ObjTrans = null;

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

                DataAdapter = new OracleDataAdapter(Command);
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
            OracleTransaction ObjTrans = null;

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
        public int CommitChangeWithParameter(string query, OracleParameter[] param)
        {
            try
            {
                foreach (OracleParameter pa in param)
                {
                    this.Command.Parameters.Add(pa);
                }

                if (this.Connection.State != System.Data.ConnectionState.Open)
                {
                    Connection.Open();
                }
                this.Command.Connection = this.Connection;
                this.Command.CommandText = query;
                this.Command.CommandType = System.Data.CommandType.Text;

                int result = 0;


                OracleTransaction transaction = Connection.BeginTransaction();
                Command.Transaction = transaction;

                result = this.Command.ExecuteNonQuery();

                transaction.Commit();

                return result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (this.Connection.State != ConnectionState.Closed)
                {
                    this.Connection.Close();
                }
            }
        }

        public int CommitChange(DataSet ds)
        {
            return CommitChange(ds, CommandType.StoredProcedure);
        }

        public int CommitChange(DataSet ds, CommandType cmdType)
        {
            int intReturn = 0;
            int intReturnTemp = 0;
            OracleTransaction ObjTrans = null;

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
                        if (dc.ColumnName.Substring(0, 1) == "R" || dc.ColumnName == "P_RSDT")
                        {
                            Command.Parameters.Add(dc.ColumnName, OracleDbType.Varchar2, 500);
                        }
                        else if (dc.ColumnName.Substring(0, 1) == "V")
                        {
                            Command.Parameters.Add(dc.ColumnName, OracleDbType.Varchar2, 500);
                        }
                        else if (dc.ColumnName.Substring(0, 1) == "N")
                        {
                            Command.Parameters.Add(dc.ColumnName, OracleDbType.Int32);
                        }
                        else if (dc.ColumnName.Substring(0, 1) == "A")
                        {
                            Command.Parameters.Add(dc.ColumnName, OracleDbType.Array);
                        }
                        else if (dc.ColumnName.Substring(0, 1) == "D")
                        {
                            Command.Parameters.Add(dc.ColumnName, OracleDbType.Decimal);
                        }
                        else if (dc.ColumnName == "P_CNT" || dc.ColumnName == "P_RSCD")
                        {
                            Command.Parameters.Add(dc.ColumnName, OracleDbType.Int32);
                        }
                        else
                        {
                            if (dc.Caption.Equals(null))
                            {
                                Command.Parameters.Add(dc.ColumnName, null);
                            }
                            else
                            {
                                Command.Parameters.Add(dc.ColumnName, null);
                            }
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
                        ///////////////////////////////////////////
                        //intReturn += com.ExecuteNonQuery();


                        bool flag = true;
                        for (int i = 0; i < Command.Parameters.Count; i++)
                        {
                            if (Command.Parameters[i].Direction == ParameterDirection.Output)
                            {
                                if (Command.Parameters[i].ParameterName == "P_CNT")
                                {
                                    if (Command.Parameters[i].Value != null
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
                //intReturn = 1;
                ObjTrans.Commit();
            }
            catch (OracleException ex)
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

        public int CommitChangeMix(DataSet ds)
        {
            int intReturn = 0;
            int intReturnTemp = 0;
            OracleTransaction ObjTrans = null;

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
                    PropertyCollection userCollection = dt.ExtendedProperties;

                    if (userCollection["MIXTYPE"].ToString() == "SP")
                    {
                        Command.CommandType = CommandType.StoredProcedure;
                        Command.CommandText = dt.TableName;

                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (dc.ColumnName == "P_CNT")
                            {
                                Command.Parameters.Add(dc.ColumnName, OracleDbType.Int32);
                            }
                            else
                            {
                                if (dc.Caption.Equals(null))
                                {
                                    Command.Parameters.Add(dc.ColumnName, null);
                                }
                                else
                                {
                                    Command.Parameters.Add(dc.ColumnName, null);
                                }
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

                            ///////////////////////////////////////////
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
                    else
                    {
                        Command.CommandType = CommandType.Text;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            Command.CommandText = dt.Rows[i][0].ToString();
                            intReturn += Command.ExecuteNonQuery();
                        }
                    }
                }

                intReturn = 1;


                ObjTrans.Commit();
            }
            catch (OracleException ex)
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

        public int CommitChange(string spName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable(spName);	//  Table명은 프로시져명으로 한다.
            ds.Tables.Add(dt);
            return this.CommitChange(ds);
        }

        public int CommitChangeForFileUpload(DataSet ds)
        {
            int intReturn = 0;
            int intReturnTemp = 0;
            OracleTransaction ObjTrans = null;
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
                            Command.Parameters.Add(dc.ColumnName, OracleDbType.Int32);
                        }
                        else
                        {
                            OracleParameter p = new OracleParameter();
                            p.ParameterName = dc.ColumnName;

                            if (dc.DataType == System.Type.GetType("System.Byte[]"))
                                p.OracleDbType = OracleDbType.Blob;

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

                        ///////////////////////////////////////////
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
                        if (dc.ColumnName == "P_CURSOR")
                        {
                            Command.Parameters.Add(dc.ColumnName, OracleDbType.RefCursor);
                        }
                        else if (dc.ColumnName.Substring(0, 1) == "R" || dc.ColumnName == "P_RSDT")
                        {
                            Command.Parameters.Add(dc.ColumnName, OracleDbType.Varchar2, 50);
                        }
                        else if (dc.ColumnName.Substring(0, 1) == "V")
                        {
                            Command.Parameters.Add(dc.ColumnName, OracleDbType.Varchar2, 50);
                        }
                        else if (dc.ColumnName.Substring(0, 1) == "N")
                        {
                            Command.Parameters.Add(dc.ColumnName, OracleDbType.Int32);
                        }
                        else if (dc.ColumnName.Substring(0, 1) == "S")
                        {
                            Command.Parameters.Add(dc.ColumnName, OracleDbType.Int16);
                        }
                        else if (dc.ColumnName.Substring(0, 1) == "D")
                        {
                            Command.Parameters.Add(dc.ColumnName, OracleDbType.Decimal);
                        }
                        else if (dc.ColumnName == "P_CNT" || dc.ColumnName == "P_RSCD")
                        {
                            Command.Parameters.Add(dc.ColumnName, OracleDbType.Int32);
                        }
                        else
                        {
                            Command.Parameters.Add(dc.ColumnName, null);
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
                    }

                    DataAdapter = new OracleDataAdapter(Command);
                    DataAdapter.Fill(dsReturn, dt.TableName);

                    foreach (DataRow dr in dt.Rows)
                    {
                        for (int i = 0; i < Command.Parameters.Count; i++)
                        {
                            if (Command.Parameters[i].Direction == ParameterDirection.Output
                                || Command.Parameters[i].Direction == ParameterDirection.InputOutput)
                            {
                                if (Command.Parameters[i].ParameterName != "P_CURSOR")
                                {
                                    dr[Command.Parameters[i].ParameterName] = Command.Parameters[i].Value;
                                }

                                //if (com.Parameters[i].ParameterName.Substring(0, 1) == "N")
                                //{
                                //    if (com.Parameters[i].Value != DBNull.Value)
                                //        dr[com.Parameters[i].ParameterName] = int.Parse(com.Parameters[i].Value.ToString());
                                //    else
                                //        dr[com.Parameters[i].ParameterName] = DBNull.Value;
                                //}
                                //else if (com.Parameters[i].ParameterName.Substring(0, 1) == "D")
                                //{
                                //    if (com.Parameters[i].Value != DBNull.Value)
                                //        dr[com.Parameters[i].ParameterName] = decimal.Parse(com.Parameters[i].Value.ToString());
                                //    else
                                //        dr[com.Parameters[i].ParameterName] = DBNull.Value;
                                //}
                                //else if (com.Parameters[i].ParameterName.Substring(0, 1) == "S")
                                //{
                                //    if (com.Parameters[i].Value.ToString() != "null")
                                //        dr[com.Parameters[i].ParameterName] = int.Parse(com.Parameters[i].Value.ToString());
                                //    else
                                //        dr[com.Parameters[i].ParameterName] = DBNull.Value;
                                //}
                                //else
                                //{
                                //    if (com.Parameters[i].Value.ToString() != "null")
                                //        dr[com.Parameters[i].ParameterName] = com.Parameters[i].Value.ToString();
                                //    else
                                //        dr[com.Parameters[i].ParameterName] = DBNull.Value;
                                //}
                            }
                        }
                    }

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

        public DataSet GetDataSetMulti(DataSet ds, string[] arrCursor)
        {
            DataSet dsReturn = new DataSet();
            try
            {

                foreach (DataTable dt in ds.Tables)
                {
                    Command.CommandText = dt.TableName;

                    foreach (DataColumn dc in dt.Columns)
                    {
                        Command.Parameters.Add(dc.ColumnName, null);
                    }
                    for (int i = 0; i < arrCursor.Length; i++)
                    {
                        Command.Parameters[arrCursor[i]].OracleDbType = OracleDbType.RefCursor;
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            for (int i = 0; i < arrCursor.Length; i++)
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
                        }
                    }

                    DataAdapter = new OracleDataAdapter(Command);
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
        
        public DataSet GetDataSetMulti(DataSet ds, string[] arrCursor, System.Collections.Hashtable hashTable)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                foreach (DataTable dt in ds.Tables)
                {
                    Command.CommandText = dt.TableName;

                    foreach (DataColumn dc in dt.Columns)
                    {
                        Command.Parameters.Add(dc.ColumnName, null);
                    }
                    for (int i = 0; i < arrCursor.Length; i++)
                    {
                        Command.Parameters[arrCursor[i]].OracleDbType = OracleDbType.RefCursor;
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            for (int i = 0; i < arrCursor.Length; i++)
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
                        }
                    }


                    DataAdapter = new OracleDataAdapter(Command);
                    DataAdapter.Fill(dsReturn);
                    Command.Parameters.Clear();
                    DataAdapter.Dispose();
                }
            }
            catch (Exception e)
            {
                throw (e);
            }

            //INFIS_Dac.Dac_Function dacFunction = new Dac_Function();


            //foreach (System.Collections.DictionaryEntry de in hashTable)
            //{
            //    if (de.Key is int && de.Value is string[])
            //    {
            //        foreach (string col in (string[])de.Value)
            //        {
            //            foreach (DataRow dr in dsReturn.Tables[(int)de.Key].Rows)
            //            {
            //                dr[col] = dacFunction.EnCrypt(dr[col].ToString());
            //            }
            //        }

            //    }
            //    else continue;
            //}


            return dsReturn;
        }

        public DataSet GetDataSet(string spName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable(spName);
            ds.Tables.Add(dt);
            return this.GetDataSet(ds);
        }

        public DataSet GetDataSetSql(string Sql, string TableName)
        {
            DataSet dsReturn = new DataSet();
            try
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(Sql);
#endif
                DataAdapter = new OracleDataAdapter(Sql, Connection);
                DataAdapter.Fill(dsReturn, TableName);
                DataAdapter.Dispose();
            }
            catch (Exception e)
            {
                throw (e);
            }

            return dsReturn;
        }

        public object GetScalar(DataSet ds)
        {
            object obj = null;
            DataSet getds = this.GetDataSet(ds);
            try
            {
                obj = (getds.Tables[0].Rows.Count > 0) ? (object)getds.Tables[0].Rows[0][0] : null;
            }
            catch (Exception e)
            {
                throw (e);
            }
            return obj;
        }

        public object GetScalar(string spName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable(spName);
            ds.Tables.Add(dt);
            return (this.GetScalar(ds));
        }
        #endregion
    }
}