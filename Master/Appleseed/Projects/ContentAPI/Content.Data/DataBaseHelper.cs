/* 
-- =============================================
-- Author:		Jonathan F. Minond
-- Create date: April 2006
-- Description:	Database provider class
-- =============================================
*/

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

namespace Content.API.Data
{
    public class DatabaseHelper : IDisposable
    {
        private string strConnectionString;
        private DbConnection objConnection;
        private DbCommand objCommand;
        private DbProviderFactory objFactory = null;
        private bool boolHandleErrors;
        private string strLastError;
        private bool boolLogError;
        private string strLogFile;

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>The connection.</value>
        public DbConnection Connection
        {
            get
            {
                return objConnection;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DatabaseHelper"/> class.
        /// </summary>
        /// <param name="connectionstring">The connectionstring.</param>
        /// <param name="provider">The provider.</param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DatabaseHelper"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public DatabaseHelper(Providers provider):this(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString,provider)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DatabaseHelper"/> class.
        /// </summary>
        /// <param name="connectionstring">The connectionstring.</param>
        public DatabaseHelper(string connectionstring): this(connectionstring, Providers.SqlServer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DatabaseHelper"/> class.
        /// </summary>
        public DatabaseHelper():this(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString,Providers.ConfigDefined)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether [handle errors].
        /// </summary>
        /// <value><c>true</c> if [handle errors]; otherwise, <c>false</c>.</value>
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

        /// <summary>
        /// Gets the last error.
        /// </summary>
        /// <value>The last error.</value>
        public string LastError
        {
            get
            {
                return strLastError;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [log errors].
        /// </summary>
        /// <value><c>true</c> if [log errors]; otherwise, <c>false</c>.</value>
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

        /// <summary>
        /// Gets or sets the log file.
        /// </summary>
        /// <value>The log file.</value>
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

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public int AddParameter(string name,object value)
        {
            DbParameter p = objFactory.CreateParameter();
            p.ParameterName = name;
            p.Value=value;
            return objCommand.Parameters.Add(p);
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        public int AddParameter(DbParameter parameter)
        {
            return objCommand.Parameters.Add(parameter);
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <value>The command.</value>
        public DbCommand Command
        {
            get
            {
                return objCommand;
            }
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        public void BeginTransaction()
        {
            if (objConnection.State == System.Data.ConnectionState.Closed)
            {
                objConnection.Open();
            }
            objCommand.Transaction = objConnection.BeginTransaction();
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public void CommitTransaction()
        {
            objCommand.Transaction.Commit();
            objConnection.Close();
        }

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            objCommand.Transaction.Rollback();
            objConnection.Close();
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string query)
        {
            return ExecuteNonQuery(query, CommandType.Text, ConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandtype">The commandtype.</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string query,CommandType commandtype)
        {
            return ExecuteNonQuery(query, commandtype, ConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="connectionstate">The connectionstate.</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string query,ConnectionState connectionstate)
        {
            return ExecuteNonQuery(query,CommandType.Text,connectionstate);
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandtype">The commandtype.</param>
        /// <param name="connectionstate">The connectionstate.</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string query,CommandType commandtype, ConnectionState connectionstate)
        {
            objCommand.CommandText = query;
            objCommand.CommandType = commandtype;
            int i=-1;
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

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public object ExecuteScalar(string query)
        {
            return ExecuteScalar(query, CommandType.Text, ConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandtype">The commandtype.</param>
        /// <returns></returns>
        public object ExecuteScalar(string query,CommandType commandtype)
        {
            return ExecuteScalar(query, commandtype, ConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="connectionstate">The connectionstate.</param>
        /// <returns></returns>
        public object ExecuteScalar(string query, ConnectionState connectionstate)
        {
            return ExecuteScalar(query, CommandType.Text, connectionstate);
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandtype">The commandtype.</param>
        /// <param name="connectionstate">The connectionstate.</param>
        /// <returns></returns>
        public object ExecuteScalar(string query,CommandType commandtype, ConnectionState connectionstate)
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

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string query)
        {
            return ExecuteReader(query, CommandType.Text, ConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandtype">The commandtype.</param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string query,CommandType commandtype)
        {
            return ExecuteReader(query, commandtype, ConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="connectionstate">The connectionstate.</param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string query, ConnectionState connectionstate)
        {
            return ExecuteReader(query, CommandType.Text, connectionstate);
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandtype">The commandtype.</param>
        /// <param name="connectionstate">The connectionstate.</param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string query,CommandType commandtype, ConnectionState connectionstate)
        {
            objCommand.CommandText = query;
            objCommand.CommandType = commandtype;
            DbDataReader reader=null;
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

        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string query)
        {
            return ExecuteDataSet(query, CommandType.Text, ConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandtype">The commandtype.</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string query,CommandType commandtype)
        {
            return ExecuteDataSet(query, commandtype, ConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="connectionstate">The connectionstate.</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string query,ConnectionState connectionstate)
        {
            return ExecuteDataSet(query, CommandType.Text, connectionstate);
        }

        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandtype">The commandtype.</param>
        /// <param name="connectionstate">The connectionstate.</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string query,CommandType commandtype, ConnectionState connectionstate)
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

        /// <summary>
        /// Handles the exceptions.
        /// </summary>
        /// <param name="ex">The ex.</param>
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

        /// <summary>
        /// Writes to log.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        private void WriteToLog(string msg)
        {
            StreamWriter writer= File.AppendText(LogFile);
            writer.WriteLine(DateTime.Now.ToString() + " - " + msg);
            writer.Close();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            objConnection.Close();
            objConnection.Dispose();
            objCommand.Dispose();
        }

    }

    /// <summary>
    /// Available DB Providers
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
    /// State of connection
    /// </summary>
    public enum ConnectionState
    {
        KeepOpen,
        CloseOnExit
    }
}
