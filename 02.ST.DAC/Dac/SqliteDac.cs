using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ST.Core;

namespace ST.DAC
{
    public class SqliteDac : DacBase
    {
        private SQLiteConnection Connection;
        private SQLiteCommand Command;
        private SQLiteTransaction Transaction;

        public string ProcedureDirectory { get; set; } = @"Procedures\";

        public SqliteDac(string connectionString)
        {
            Connection = new SQLiteConnection(connectionString);
            Connection.Open();
            Command = new SQLiteCommand(Connection);
        }

        private string GetFulFilePath(string filePath)
        {
            filePath = filePath.Trim();
            string fullFilePath;

            if (filePath.Length == 0)
            {
                throw new Exception("The filePath is empty.");
            }
            else
            {
                
                // Absolute path
                if (filePath.Length >= 3 && filePath.Substring(1, 2) == @":\")
                {
                    fullFilePath = filePath;
                }
                // Relative path
                else
                {
                    string currentPath = AppDomain.CurrentDomain.BaseDirectory;
                    if (currentPath[currentPath.Length - 1] == '\\' && filePath[0] == '\\' && filePath.Length > 1)
                    {
                        fullFilePath = currentPath + filePath.Substring(1, filePath.Length - 1);
                    }
                    else
                    {
                        fullFilePath = currentPath + filePath;
                    }
                }
            }

            return fullFilePath;
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
            DataTable resultDT = new DataTable();

            Command.CommandText = query;
            SQLiteDataReader reader = Command.ExecuteReader();
            resultDT.Load(reader);

            DataSet resultDS = new DataSet();
            resultDS.Tables.Add(resultDT);
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
            DataTable resultDT = new DataTable();
            DataSet resultDS = new DataSet();
            try
            {
                // Get procedureParameters, procedureQuery
                string _sqlFileText = File.ReadAllText(ProcedureDirectory + procedureName + ".sql");
                List<ParameterInfo> procedureDefaultParameters;
                string procedureQuery;
                DivideDefaultParametersNQuery(_sqlFileText, out procedureDefaultParameters, out procedureQuery);

                // Throw error
                ValidateParameters(procedureDefaultParameters, procedureQuery);

                // User Parameters
                foreach (KeyValuePair<string, object> pair in parameters)
                {
                    string key = pair.Key.Trim();
                    if (key.Length > 0)
                    {
                        Command.Parameters.AddWithValue((key[0] == '@' ? "" : "@") + pair.Key, pair.Value);
                    }
                    else
                    {
                        throw new Exception(string.Format(""));
                    }
                }

                // Default Parameters
                for (int i = 0; i < procedureDefaultParameters.Count; i++)
                {
                    if (!Command.Parameters.Contains(procedureDefaultParameters[i].FieldName))
                    {
                        if (procedureDefaultParameters[i].HasDefault)
                        {
                            Command.Parameters.AddWithValue(procedureDefaultParameters[i].FieldName, procedureDefaultParameters[i].DefaultValue);
                        }
                    }
                }

                // Execute
                Command.CommandText = procedureQuery;
                SQLiteDataReader reader = Command.ExecuteReader();

                // Set resultDS
                resultDT.Load(reader);
                resultDS.Tables.Add(resultDT);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return resultDS;
        }

        public override int ExecuteNonProcedure(string procedureName, Dictionary<string, object> parameters)
        {
            int resultCount;
            try
            {
                // Get procedureParameters, procedureQuery
                string _sqlFileText = File.ReadAllText(ProcedureDirectory + procedureName + ".sql");
                List<ParameterInfo> procedureDefaultParameters;
                string procedureQuery;
                DivideDefaultParametersNQuery(_sqlFileText, out procedureDefaultParameters, out procedureQuery);

                // Throw error
                ValidateParameters(procedureDefaultParameters, procedureQuery);

                // User Parameters
                foreach (KeyValuePair<string, object> pair in parameters)
                {
                    string key = pair.Key.Trim();
                    if (key.Length > 0)
                    {
                        Command.Parameters.AddWithValue((key[0] == '@' ? "" : "@") + pair.Key, pair.Value);
                    }
                    else
                    {
                        throw new Exception(string.Format(""));
                    }
                }

                // Default Parameters
                for (int i = 0; i < procedureDefaultParameters.Count; i++)
                {
                    if (!Command.Parameters.Contains(procedureDefaultParameters[i].FieldName))
                    {
                        if (procedureDefaultParameters[i].HasDefault)
                        {
                            Command.Parameters.AddWithValue(procedureDefaultParameters[i].FieldName, procedureDefaultParameters[i].DefaultValue);
                        }
                    }
                }

                // Execute
                Command.CommandText = procedureQuery;
                resultCount = Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return resultCount;
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

        #region Function
        private void DivideDefaultParametersNQuery(string sqlFileText, out List<ParameterInfo> procedureDefaultParameters, out string procedureQuery)
        {
            // Get parametersSp, parametersEp
            int parametersSp = sqlFileText.IndexOf("/*");
            int parametersEp = 0;
            if (parametersSp >= 0)
            {
                parametersEp = sqlFileText.IndexOf("*/", parametersSp);
            }

            // Get procedureDefaultParametersText / Set procedureQuery
            string procedureDefaultParametersText;
            if (parametersSp >= 0 && parametersEp >= 0)
            {
                procedureDefaultParametersText = sqlFileText.Substring(parametersSp + 2, parametersEp - 2);
                procedureQuery = sqlFileText.Substring(parametersEp + 2);
            }
            else
            {
                procedureDefaultParametersText = string.Empty;
                procedureQuery = sqlFileText;
            }

            // Set procedureDefaultParameters
            procedureDefaultParameters = new List<ParameterInfo>();
            string[] procedureParametersLines = procedureDefaultParametersText.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for(int i = 0; i < procedureParametersLines.Length; i++)
            {
                string[] procedureParametersLineItems = procedureParametersLines[i].Split('=');

                string fieldName = procedureParametersLineItems[0].Trim();
                object defaultValue = null;
                if (fieldName.Length == 0)
                {
                    throw new Exception(string.Format("Field name is empty. Line text : {0}", procedureParametersLines[i]));
                }
                else
                {
                    if (procedureParametersLineItems.Length > 1)
                    {
                        string defaultValueText = procedureParametersLineItems[1].Trim();
                        if (defaultValueText.Length == 0)
                        {
                            throw new Exception(string.Format("Default value is empty. Line text : {0}", procedureParametersLines[i]));
                        }
                        else
                        {
                            if (defaultValueText[0] == '\'')
                            {
                                if (defaultValueText.Length < 2 || defaultValueText[defaultValueText.Length - 1] != '\'')
                                {
                                    throw new Exception(string.Format("Default value is invalid. Line text : {0}", procedureParametersLines[i]));
                                }
                                else
                                {
                                    defaultValue = defaultValueText.Substring(1, defaultValueText.Length - 2);
                                }
                            }
                            else
                            {
                                if (defaultValueText == "null")
                                {
                                    defaultValue = DBNull.Value;
                                }
                                else if (defaultValueText.ToLower() == "true")
                                {
                                    defaultValue = true;
                                }
                                else if (defaultValueText.ToLower() == "false")
                                {
                                    defaultValue = false;
                                }
                                else
                                {
                                    long longValue;
                                    double doubleValue;
                                    if (long.TryParse(defaultValueText, out longValue))
                                    {
                                        defaultValue = longValue;
                                    }
                                    else if (double.TryParse(defaultValueText, out doubleValue))
                                    {
                                        defaultValue = doubleValue;
                                    }
                                    else
                                    {
                                        throw new Exception(string.Format("Default value is invalid. Line text : {0}", procedureParametersLines[i]));
                                    }
                                }
                            }
                        }
                    }
                }

                procedureDefaultParameters.Add(new ParameterInfo(fieldName, defaultValue != null, defaultValue));
            }
        }

        private void ValidateParameters(List<ParameterInfo> procedureParameters, string procedureQuery)
        {
            for(int i = 0; i < procedureParameters.Count; i++)
            {
                if (procedureQuery.IndexOf(procedureParameters[i].FieldName) < 0)
                {
                    throw new Exception(string.Format("Can not found the field name in query. Field Name : {0}", procedureParameters[i].FieldName));
                }
            }
        }
        #endregion

        #region Struct - ParameterInfo
        private struct ParameterInfo
        {
            public string FieldName;
            public bool HasDefault;
            public object DefaultValue;

            public ParameterInfo(string _Name, bool _HasDefault, object _DefaultValue)
            {
                FieldName = _Name;
                HasDefault = _HasDefault;
                DefaultValue = _DefaultValue;
            }
        }
        #endregion
    }
}
