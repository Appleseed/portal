using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.OracleClient;
using System.IO;

namespace Appleseed.Framwork.Data
{
    public class DatabaseHelper:IDisposable
    {
        private string strConnectionString;
        private DbConnection objConnection;
        private DbCommand objCommand;
        private DbProviderFactory objFactory = null;
        private bool boolHandleErrors;
        private string strLastError;
        private bool boolLogError;
        private string strLogFile;

        public DatabaseHelper(string connectionstring,Providers provider)
        {
            strConnectionString = connectionstring;
            switch (provider)
            {
                case Providers.SqlServer:
                    objFactory = SqlClientFactory.Instance;
                    break;
                case Providers.OleDb:
                    objFactory = OleDbFactory.Instance;
                    break;
                case Providers.Oracle:
                    objFactory = OracleClientFactory.Instance;
                    break;
                case Providers.ODBC:
                    objFactory = OdbcFactory.Instance;
                    break;
                case Providers.ConfigDefined:
                    string providername=ConfigurationManager.ConnectionStrings["connectionstring"].ProviderName;
                    switch (providername)
                    {
                        case "System.Data.SqlClient":
                            objFactory = SqlClientFactory.Instance;
                            break;
                        case "System.Data.OleDb":
                            objFactory = OleDbFactory.Instance;
                            break;
                        case "System.Data.OracleClient":
                            objFactory = OracleClientFactory.Instance;
                            break;
                        case "System.Data.Odbc":
                            objFactory = OdbcFactory.Instance;
                            break;
                    }
                    break;

            }
            objConnection = objFactory.CreateConnection();
            objCommand = objFactory.CreateCommand();

            objConnection.ConnectionString = strConnectionString;
            objCommand.Connection = objConnection;
        }

        public DatabaseHelper(Providers provider):this(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString,provider)
        {
        }

        public DatabaseHelper(string connectionstring): this(connectionstring, Providers.SqlServer)
        {
        }

        public DatabaseHelper():this(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString,Providers.ConfigDefined)
        {
        }

        public bool HandleErrors
        {
            get
            {
                return boolHandleErrors;
            }
            set
            {
                boolHandleErrors = value;
            }
        }

        public string LastError
        {
            get
            {
                return strLastError;
            }
        }

        public bool LogErrors
        {
            get
            {
                return boolLogError;
            }
            set
            {
                boolLogError=value;
            }
        }

        public string LogFile
        {
            get
            {
                return strLogFile;
            }
            set
            {
                strLogFile = value;
            }
        }

        public int AddParameter(string name,object value)
        {
            DbParameter p = objFactory.CreateParameter();
            p.ParameterName = name;
            p.Value=value;
            return objCommand.Parameters.Add(p);
        }

        public int AddParameter(DbParameter parameter)
        {
            return objCommand.Parameters.Add(parameter);
        }

        public DbCommand Command
        {
            get
            {
                return objCommand;
            }
        }

        public void BeginTransaction()
        {
            if (objConnection.State == System.Data.ConnectionState.Closed)
            {
                objConnection.Open();
            }
            objCommand.Transaction = objConnection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            objCommand.Transaction.Commit();
            objConnection.Close();
        }

        public void RollbackTransaction()
        {
            objCommand.Transaction.Rollback();
            objConnection.Close();
        }

        #region ExecuteNonQuery
        public int ExecuteNonQuery(string spName)
        {
            return ExecuteNonQuery(spName, CommandType.StoredProcedure, ConnectionState.CloseOnExit);
        }

        public int ExecuteNonQuery(string spName, params object[] parameterValues)
        {
            return ExecuteNonQuery(spName, ConnectionState.CloseOnExit, parameterValues);
        }
        
        public int ExecuteNonQuery(string spName, ConnectionState connectionstate, params object[] parameterValues)
        {
            objCommand.CommandText = spName;
            objCommand.CommandType = CommandType.StoredProcedure;

            DbParameter[] parameters = ParameterCache.GetSpParameterSet(objConnection, spName);
            AssignParameterValues(parameters, parameterValues);
            objCommand.Parameters.AddRange(parameters);

            int i = -1;
            try
            {
                if (objConnection.State == System.Data.ConnectionState.Closed)
                {
                    objConnection.Open();
                }
                i = objCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            finally
            {
                objCommand.Parameters.Clear();
                if (connectionstate == ConnectionState.CloseOnExit)
                {
                    objConnection.Close();
                }
            }

            return i;
        }

        public int ExecuteNonQuery(string query, CommandType commandtype)
        {
            return ExecuteNonQuery(query, commandtype, ConnectionState.CloseOnExit);
        }
        
        public int ExecuteNonQuery(string query, CommandType commandtype, ConnectionState connectionstate)
        {
            objCommand.CommandText = query;
            objCommand.CommandType = commandtype;

            int i = -1;
            try
            {
                if (objConnection.State == System.Data.ConnectionState.Closed)
                {
                    objConnection.Open();
                }
                i = objCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            finally
            {
                objCommand.Parameters.Clear();
                if (connectionstate == ConnectionState.CloseOnExit)
                {
                    objConnection.Close();
                }
            }

            return i;
        }
        #endregion

        #region ExecuteScalar
        public object ExecuteScalar(string spName)
        {
            return ExecuteScalar(spName, CommandType.StoredProcedure, ConnectionState.CloseOnExit);
        }
        
        public object ExecuteScalar(string spName, params object[] parameterValues)
        {
            return ExecuteScalar(spName, ConnectionState.CloseOnExit, parameterValues);
        }
        
        public object ExecuteScalar(string spName, ConnectionState connectionstate, params object[] parameterValues)
        {
            objCommand.CommandText = spName;
            objCommand.CommandType = CommandType.StoredProcedure;

            DbParameter[] parameters = ParameterCache.GetSpParameterSet(objConnection, spName);
            AssignParameterValues(parameters, parameterValues);
            objCommand.Parameters.AddRange(parameters);

            object o = null;
            try
            {
                if (objConnection.State == System.Data.ConnectionState.Closed)
                {
                    objConnection.Open();
                }
                o = objCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            finally
            {
                objCommand.Parameters.Clear();
                if (connectionstate == ConnectionState.CloseOnExit)
                {
                    objConnection.Close();
                }
            }

            return o;
        }

        public object ExecuteScalar(string query, CommandType commandtype)
        {
            return ExecuteScalar(query, commandtype, ConnectionState.CloseOnExit);
        }

        public object ExecuteScalar(string query, CommandType commandtype, ConnectionState connectionstate)
        {
            objCommand.CommandText = query;
            objCommand.CommandType = commandtype;
            object o = null;
            try
            {
                if (objConnection.State == System.Data.ConnectionState.Closed)
                {
                    objConnection.Open();
                }
                o = objCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            finally
            {
                objCommand.Parameters.Clear();
                if (connectionstate == ConnectionState.CloseOnExit)
                {
                    objConnection.Close();
                }
            }

            return o;
        }
        #endregion

        #region ExecuteReader
        public DbDataReader ExecuteReader(string spName)
        {
            return ExecuteReader(spName, CommandType.StoredProcedure, ConnectionState.CloseOnExit);
        }
     
        public DbDataReader ExecuteReader(string spName, params object[] parameterValues)
        {
            return ExecuteReader(spName, ConnectionState.CloseOnExit, parameterValues);
        }
        
        public DbDataReader ExecuteReader(string spName, ConnectionState connectionstate, params object[] parameterValues)
        {
            objCommand.CommandText = spName;
            objCommand.CommandType = CommandType.StoredProcedure;

            DbParameter[] parameters = ParameterCache.GetSpParameterSet(objConnection, spName);
            AssignParameterValues(parameters, parameterValues);
            objCommand.Parameters.AddRange(parameters);

            DbDataReader reader = null;
            try
            {
                if (objConnection.State == System.Data.ConnectionState.Closed)
                {
                    objConnection.Open();
                }
                if (connectionstate == ConnectionState.CloseOnExit)
                {                    
                    reader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                }
                else
                {
                    reader = objCommand.ExecuteReader();
                }

            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            finally
            {
                objCommand.Parameters.Clear();
            }

            return reader;
        }
        
        public DbDataReader ExecuteReader(string query, CommandType commandtype)
        {
            return ExecuteReader(query, commandtype, ConnectionState.CloseOnExit);
        }

        public DbDataReader ExecuteReader(string query, CommandType commandtype, ConnectionState connectionstate)
        {
            objCommand.CommandText = query;
            objCommand.CommandType = commandtype;
            DbDataReader reader = null;
            try
            {
                if (objConnection.State == System.Data.ConnectionState.Closed)
                {
                    objConnection.Open();
                }
                if (connectionstate == ConnectionState.CloseOnExit)
                {
                    reader = objCommand.ExecuteReader(CommandBehavior.CloseConnection);
                }
                else
                {
                    reader = objCommand.ExecuteReader();
                }

            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            finally
            {
                objCommand.Parameters.Clear();
            }

            return reader;
        }
        #endregion

        #region ExecuteDataSet
        public DataSet ExecuteDataSet(string spName)
        {
            return ExecuteDataSet(spName, CommandType.StoredProcedure, ConnectionState.CloseOnExit);
        }
        
        public DataSet ExecuteDataSet(string spName, params object[] parameterValues)
        {
            return ExecuteDataSet(spName, ConnectionState.CloseOnExit, parameterValues);
        }
        
        public DataSet ExecuteDataSet(string spName, ConnectionState connectionstate, params object[] parameterValues)
        {
            DbDataAdapter adapter = objFactory.CreateDataAdapter();
            
            DbParameter[] parameters = ParameterCache.GetSpParameterSet(objConnection, spName);
            AssignParameterValues(parameters, parameterValues);
            objCommand.Parameters.AddRange(parameters);

            adapter.SelectCommand = objCommand;
            DataSet ds = new DataSet();
            try
            {
                adapter.Fill(ds);
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            finally
            {
                objCommand.Parameters.Clear();
                if (connectionstate == ConnectionState.CloseOnExit)
                {
                    if (objConnection.State == System.Data.ConnectionState.Open)
                    {
                        objConnection.Close();
                    }
                }
            }
            return ds;
        }
        
        public DataSet ExecuteDataSet(string query, CommandType commandtype)
        {
            return ExecuteDataSet(query, commandtype, ConnectionState.CloseOnExit);
        }

        public DataSet ExecuteDataSet(string query, CommandType commandtype, ConnectionState connectionstate)
        {
            DbDataAdapter adapter = objFactory.CreateDataAdapter();
            objCommand.CommandText = query;
            objCommand.CommandType = commandtype;
            adapter.SelectCommand = objCommand;
            DataSet ds = new DataSet();
            try
            {
                adapter.Fill(ds);
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            finally
            {
                objCommand.Parameters.Clear();
                if (connectionstate == ConnectionState.CloseOnExit)
                {
                    if (objConnection.State == System.Data.ConnectionState.Open)
                    {
                        objConnection.Close();
                    }
                }
            }
            return ds;
        }
        #endregion
        
        private void HandleExceptions(Exception ex)
        {
            if (LogErrors)
            {
                WriteToLog(ex.Message);
            }
            if (HandleErrors)
            {
                strLastError = ex.Message;
            }
            else
            {
                throw ex;
            }
        }

        private void WriteToLog(string msg)
        {
            StreamWriter writer= File.AppendText(LogFile);
            writer.WriteLine(DateTime.Now.ToString() + " - " + msg);
            writer.Close();
        }
        
        public void Dispose()
        {
            objConnection.Close();
            objConnection.Dispose();
            objCommand.Dispose();
        }
        
        /// <summary>
        /// This method assigns an array of values to an array of SqlParameters
        /// </summary>
        /// <param name="commandParameters">Array of SqlParameters to be assigned values</param>
        /// <param name="parameterValues">Array of objects holding the values to be assigned</param>
        private static void AssignParameterValues(DbParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                // Do nothing if we get no data
                return;
            }

            // We must have the same number of values as we pave parameters to put them in
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            // Iterate through the SqlParameters, assigning the values from the corresponding position in the 
            // value array
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                // If the current array value derives from IDbDataParameter, then assign its Value property
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                    if (paramInstance.Value == null)
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if (parameterValues[i] == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum Providers
    {
        /// <summary>
        /// 
        /// </summary>
        SqlServer,
        /// <summary>
        /// 
        /// </summary>
        OleDb,
        /// <summary>
        /// 
        /// </summary>
        Oracle,
        /// <summary>
        /// 
        /// </summary>
        ODBC,
        /// <summary>
        /// 
        /// </summary>
        ConfigDefined
    }
    /// <summary>
    /// 
    /// </summary>
    public enum ConnectionState
    {
        /// <summary>
        /// 
        /// </summary>
        KeepOpen,
        /// <summary>
        /// 
        /// </summary>
        CloseOnExit
    }
}
