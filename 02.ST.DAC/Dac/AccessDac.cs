using System;
using System.Data.OleDb;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace ST.DAC
{
    public class AccessDac : DacBase
    {
        private OleDbConnection Connection;
        private OleDbCommand Command;
        private OleDbDataAdapter DataAdapter;
        private OleDbTransaction Transaction;

        public AccessDac(string connectString)
        {
            try
            {
                Connection = new OleDbConnection();
                Connection.ConnectionString = connectString;
                Connection.Open();

                Command = new OleDbCommand();
                Command.CommandType = CommandType.Text;
                Command.Connection = Connection;
            }
            catch (Exception e)
            {
                throw e;
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
            DataAdapter = new OleDbDataAdapter(query, Connection);
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

        public override void Dispose()
        {
            if (Transaction != null) { Transaction.Dispose(); }

            Command.Dispose();
            if (Connection.State != ConnectionState.Closed) { Connection.Close(); }
            Connection.Dispose();
        }

        public override void SetTimeOut(int timeOut)
        {
            Command.CommandTimeout = timeOut;
        }
        #endregion

        #region Override - NotImplementedException
        public override object ExecuteScalar(string query)
        {
            throw new NotImplementedException();
        }

        public override DataSet ExecuteProcedure(string procedureName, Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public override int ExecuteNonProcedure(string procedureName, Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Method
        public DataTable ExecuteQueryResultDataTable(string sql)
        {
            DataTable resultDT = new DataTable();
            Command.CommandText = sql;
            OleDbDataReader reader = null;
            reader = Command.ExecuteReader();
            resultDT.Load(reader);
            reader.Close();

            return resultDT;
        }

        public void PutDataTableSql(string sql, DataTable dataTable)
        {
            try
            {
                Command.CommandText = sql;
                OleDbDataReader reader = null;
                reader = Command.ExecuteReader();
                dataTable.Clear();
                dataTable.Load(reader);
                reader.Close();
            }
            catch (Exception e)
            {
                throw (e);
            }
        }
        #endregion
    }
}