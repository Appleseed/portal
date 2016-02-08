// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DBHelper.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Summary description for DBHelper
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    using Appleseed.Framework.Exceptions;
    using Appleseed.Framework.Settings;
    using System.Reflection;

    /// <summary>
    /// </summary>
    public class DBHelper
    {
        #region Public Methods

        /// <summary>
        /// Exes the SQL.
        /// </summary>
        /// <param name="sql">
        /// The SQL.
        /// </param>
        /// <returns>
        /// A int value...
        /// </returns>
        public static int ExeSQL(string sql)
        {
            int returnValue;

            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand(sql, connection))
            {
                try
                {
                    sqlCommand.Connection.Open();
                    returnValue = sqlCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    ErrorHandler.Publish(
                        LogLevel.Error, string.Format("Error in DBHelper - ExeSQL - SQL: '{0}'", sql), e);
                    throw new DatabaseUnreachableException("Error in DBHelper - ExeSQL", e);

                    // throw new Exception("Error in DBHelper:ExeSQL()-> " + e.ToString());
                }
                finally
                {
                    sqlCommand.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Execute script using transaction
        /// </summary>
        /// <param name="scriptPath">
        /// The script path.
        /// </param>
        /// <param name="useTransaction">
        /// if set to <c>true</c> [use transaction].
        /// </param>
        /// <returns>
        /// </returns>
        public static List<string> ExecuteScript(string scriptPath, bool useTransaction)
        {
            return ExecuteScript(scriptPath, Config.SqlConnectionString, useTransaction);
        }

        /// <summary>
        /// Execute script (no transaction)
        /// </summary>
        /// <param name="scriptPath">
        /// The script path.
        /// </param>
        /// <returns>
        /// </returns>
        public static List<string> ExecuteScript(string scriptPath)
        {
            return ExecuteScript(scriptPath, Config.SqlConnectionString);
        }

        /// <summary>
        /// Execute script using transaction
        /// </summary>
        /// <param name="scriptPath">
        /// The script path.
        /// </param>
        /// <param name="connection">
        /// My connection.
        /// </param>
        /// <param name="useTransaction">
        /// if set to <c>true</c> [use transaction].
        /// </param>
        /// <returns>
        /// </returns>
        public static List<string> ExecuteScript(string scriptPath, SqlConnection connection, bool useTransaction)
        {
            if (!useTransaction)
            {
                return ExecuteScript(scriptPath, connection); // FIX: Must pass connection as well
            }

            var strScript = GetScript(scriptPath);
            ErrorHandler.Publish(LogLevel.Info, string.Format("Executing Script '{0}'", scriptPath));
            var errors = new List<string>();

            // Subdivide script based on GO keyword
            var sqlCommands = Regex.Split(strScript, "\\sGO\\s", RegexOptions.IgnoreCase);

            // Open connection
            connection.Open();

            // Wraps execution on a transaction 
            // so we know that the script runs or fails
            const string TransactionName = "Appleseed";
            var trans = connection.BeginTransaction(IsolationLevel.RepeatableRead, TransactionName);
            ErrorHandler.Publish(LogLevel.Debug, "Start Script Transaction ");

            try
            {
                // Cycles command and execute them
                for (var s = 0; s <= sqlCommands.GetUpperBound(0); s++)
                {
                    var sqlText = sqlCommands[s].Trim();

                    try
                    {
                        if (sqlText.Length > 0)
                        {
                            // Appleseed.Framework.Helpers.LogHelper.Logger.Log(Appleseed.Framework.Configuration.LogLevel.Debug, "Executing: " + mySqlText.Replace("\n", " "));
                            // Must assign both transaction object and connection
                            // to Command object for a pending local transaction
                            using (var sqldbCommand = new SqlCommand())
                            {
                                sqldbCommand.Connection = connection;
                                sqldbCommand.CommandType = CommandType.Text;
                                sqldbCommand.Transaction = trans;
                                sqldbCommand.CommandText = sqlText;
                                sqldbCommand.CommandTimeout = 150;
                                sqldbCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        errors.Add(string.Format("<p>{0}<br />{1}</p>", ex.Message, sqlText));
                        ErrorHandler.Publish(LogLevel.Warn, "ExecuteScript Failed: " + sqlText, ex);
                        throw new DatabaseUnreachableException("ExecuteScript Failed: " + sqlText, ex);
                    }
                }

                // Successfully applied this script
                trans.Commit();
                ErrorHandler.Publish(LogLevel.Debug, "Commit Script Transaction.");
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                var count = 0;

                while (ex.InnerException != null && count < 100)
                {
                    ex = ex.InnerException;
                    errors.Add(ex.Message);
                    count++;
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return errors;
        }

        /// <summary>
        /// Execute script (no transaction)
        /// </summary>
        /// <param name="scriptPath">
        /// The script path.
        /// </param>
        /// <param name="connection">
        /// My connection.
        /// </param>
        /// <returns>
        /// A list of results.
        /// </returns>
        public static List<string> ExecuteScript(string scriptPath, SqlConnection connection)
        {
            var strScript = GetScript(scriptPath);
            ErrorHandler.Publish(LogLevel.Info, string.Format("Executing Script '{0}'", scriptPath));
            var errors = new List<string>();

            // Subdivide script based on GO keyword
            var sqlCommands = Regex.Split(strScript, "\\sGO\\s", RegexOptions.IgnoreCase);

            try
            {
                // Cycles command and execute them
                for (var s = 0; s <= sqlCommands.GetUpperBound(0); s++)
                {
                    var commandText = sqlCommands[s].Trim();

                    try
                    {
                        if (commandText.Length > 0)
                        {
                            // Open connection
                            connection.Open();

                            ErrorHandler.Publish(LogLevel.Debug, "Executing: " + commandText.Replace("\n", " "));
                            using (var sqldbCommand = new SqlCommand())
                            {
                                sqldbCommand.Connection = connection;
                                sqldbCommand.CommandType = CommandType.Text;
                                sqldbCommand.CommandText = commandText;
                                sqldbCommand.CommandTimeout = 150;
                                sqldbCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errors.Add(string.Format("<p>{0}<br />{1}</p>", ex.Message, commandText));
                        ErrorHandler.Publish(LogLevel.Warn, "ExecuteScript Failed: " + commandText, ex);

                        // Re-throw exception
                        throw new AppleseedException(
                            LogLevel.Fatal, 
                            HttpStatusCode.ServiceUnavailable, 
                            "Script failed, please correct the error and retry: " + commandText, 
                            ex);

                        // throw new Exception("Script failed, please correct the error and retry", ex);
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                var count = 0;

                while (ex.InnerException != null && count < 100)
                {
                    ex = ex.InnerException;
                    errors.Add(ex.Message);
                    count++;
                }
            }

            return errors;
        }

        /// <summary>
        /// Executes the SQL scalar.
        /// </summary>
        /// <typeparam name="T">
        /// The type to return.
        /// </typeparam>
        /// <param name="sql">
        /// The SQL string.
        /// </param>
        /// <returns>
        /// A object value...
        /// </returns>
        public static T ExecuteSqlScalar<T>(string sql)
        {
            T returnValue;

            using (var connection = Config.SqlConnectionString)
            using (var sqlCommand = new SqlCommand(sql, connection))
            {
                try
                {
                    sqlCommand.Connection.Open();
                    returnValue = (T)sqlCommand.ExecuteScalar();
                }
                catch (Exception e)
                {
                    throw new DatabaseUnreachableException("Error in DBHelper - ExecuteSQLScalar", e);
                }
                finally
                {
                    sqlCommand.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the data reader.
        /// </summary>
        /// <param name="selectCmd">
        /// The select CMD.
        /// </param>
        /// <returns>
        /// A System.Data.SqlClient.SqlDataReader value...
        /// </returns>
        public static SqlDataReader GetDataReader(string selectCmd)
        {
            var connection = Config.SqlConnectionString;

            using (var sqlCommand = new SqlCommand(selectCmd, connection))
            {
                try
                {
                    sqlCommand.Connection.Open();
                }
                catch (Exception e)
                {
                    throw new DatabaseUnreachableException("Error in DBHelper - GetDataReader", e);

                    // throw new Exception("Error in DBHelper::GetDataReader()-> " + e.ToString());
                }

                return sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        /// <summary>
        /// Gets the data reader.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public static SqlDataReader GetDataReader(SqlCommand command)
        {
            try
            {
                command.Connection = Config.SqlConnectionString;
                command.Connection.Open();
            }
            catch (SqlException e)
            {
                throw new DatabaseUnreachableException("Error in DBHelper - GetDataReader", e);
            }

            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Gets the data set.
        /// </summary>
        /// <param name="selectCmd">
        /// The select CMD.
        /// </param>
        /// <returns>
        /// A System.Data.DataSet value...
        /// </returns>
        public static DataSet GetDataSet(string selectCmd)
        {
            DataSet ds;

            using (var connection = Config.SqlConnectionString)
            {
                using (var sqlDataAdapter = new SqlDataAdapter(selectCmd, connection))
                {
                    try
                    {
                        ds = new DataSet();
                        sqlDataAdapter.Fill(ds, "Table0");
                    }
                    catch (Exception e)
                    {
                        throw new DatabaseUnreachableException("Error in DBHelper - GetDataSet", e);

                        // throw new Exception("Error in ItemBase:GetDataSet()-> " + e.ToString());
                    }
                    finally
                    {
                        sqlDataAdapter.Dispose();
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }

            return ds;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the script from a file
        /// </summary>
        /// <param name="scriptPath">
        /// The script path.
        /// </param>
        /// <returns>
        /// The get script.
        /// </returns>
        private static string GetScript(string scriptPath)
        {
            string strScript;

            // Load script file 
            // using (System.IO.StreamReader objStreamReader = System.IO.File.OpenText(scriptPath)) 
            // http://support.Appleseedportal.net/jira/browse/RBP-693
            // to make it possible to have German umlauts or other special characters in the install_scripts
            using (var objStreamReader = new StreamReader(scriptPath, Encoding.Default))
            {
                strScript = objStreamReader.ReadToEnd();
                objStreamReader.Close();
            }

            return strScript + Environment.NewLine; // Append carriage for execute last command 
        }

        #endregion

        #region convert sql query result to entity class list

        /// <summary>
        /// Data rader to object list
        /// </summary>
        /// <typeparam name="TType">Type</typeparam>
        /// <param name="sqlQuery">sql query</param>
        /// <param name="fieldsToSkip">fields to skip</param>
        /// <param name="piList">plist</param>
        /// <returns></returns>
        public static List<TType> DataReaderToObjectList<TType>(string sqlQuery, string fieldsToSkip = null, Dictionary<string, PropertyInfo> piList = null)
           where TType : new()
        {
            var selectCmd = new SqlCommand(sqlQuery, Config.SqlConnectionString);
            selectCmd.CommandType = CommandType.Text;
            selectCmd.Connection.Open();
            IDataReader reader = selectCmd.ExecuteReader();

            if (reader == null)
                return null;

            var items = new List<TType>();

            // Create lookup list of property info objects            
            if (piList == null)
            {
                piList = new Dictionary<string, PropertyInfo>();
                var props = typeof(TType).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (var prop in props)
                    piList.Add(prop.Name.ToLower(), prop);
            }

            while (reader.Read())
            {
                var inst = new TType();
                DataReaderToObject(reader, inst, fieldsToSkip, piList);
                items.Add(inst);
            }
            // Added by Ashish - Connection Pool Issue
            if(reader != null)
            {
                reader.Close();
                selectCmd.Connection.Close();
            }
            
            return items;
        }


        private static void DataReaderToObject(IDataReader reader, object instance, string fieldsToSkip = null, Dictionary<string, PropertyInfo> piList = null)
        {
            if (reader.IsClosed)
                throw new InvalidOperationException("Data reader cannot be used because it's already closed");

            if (string.IsNullOrEmpty(fieldsToSkip))
                fieldsToSkip = string.Empty;
            else
                fieldsToSkip = "," + fieldsToSkip + ",";

            fieldsToSkip = fieldsToSkip.ToLower();

            // create a dictionary of properties to look up
            // we can pass this in so we can cache the list once 
            // for a list operation 
            if (piList == null)
            {
                piList = new Dictionary<string, PropertyInfo>();
                var props = instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (var prop in props)
                    piList.Add(prop.Name.ToLower(), prop);
            }

            for (int index = 0; index < reader.FieldCount; index++)
            {
                string name = reader.GetName(index).ToLower();
                if (piList.ContainsKey(name))
                {
                    var prop = piList[name];

                    if (fieldsToSkip.Contains("," + name + ","))
                        continue;

                    if ((prop != null) && prop.CanWrite)
                    {
                        var val = reader.GetValue(index);
                        prop.SetValue(instance, (val == DBNull.Value) ? null : val, null);
                    }
                }
            }

            return;
        }
        #endregion
    }
}