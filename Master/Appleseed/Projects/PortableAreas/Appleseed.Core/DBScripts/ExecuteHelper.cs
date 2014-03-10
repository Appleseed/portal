using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Globalization;

/// Agileworks execute scripts
namespace Appleseed.Core.ExecuteScripts
{
    public class ExecuteHelper
    {

        private static string TableName = "DBVersion";
        private static string AreaName = "AreaName";

        #region execute

        /// <summary>
        /// Ejecuta todos los scipts encontrados en la carpeta de scripts desde la siguiente version que la encontrada en la base de datos
        /// </summary>
        public static void Execute(string _scriptsPath, string _ConnectionString, string areaName)
        {
            string sPath = _scriptsPath ?? ScriptsPath;
            string cnx = _ConnectionString ?? ConnectionString;
            Execute("-1", sPath, cnx, areaName);
        }

        /// <summary>
        /// Ejecuta todos los scipts encontrados en la carpeta de scripts desde la siguiente version que la encontrada en la base de datos
        /// </summary>
        public static void Execute()
        {
            Execute("-1");
        }

        public static void Execute(string version)
        {
            Execute(version, ScriptsPath, ConnectionString, null);
        }

        /// <summary>
        /// Ejecuta todos los scripts encontrados en la carpeta de scripts desde la siguiente version que la encontrada en la base de datos hasta la 
        /// version pasada como parametro inclusive
        /// </summary>
        /// <param name="version"></param>
        public static void Execute(string version, string _ScriptsPath, string _connectionString, string areaName)
        {
            LogInfo("\n");
            LogInfo("Executing scripts on " + DateTime.Now.ToString());
            LogInfo("\n");

            try
            {
                ValidateVersionParameter(version);
                string minVersion = GetLastDBVersion(_connectionString, areaName);
                try
                {
                    ValidateVersionParameter(minVersion);
                }
                catch (Exception)
                {
                    LogInfo("DBVersion has wrong entry; assuming no database at all.");
                    minVersion = "-1";
                }
                string[] directories = Directory.GetDirectories(_ScriptsPath);
                if (directories != null && directories.Length != 0)
                {
                    Array.Sort(directories);
                    Encoding ec = Encoding.Default;
                    #region desgloso la version de la base y la que me pasan por parametro
                    string valueMinVersion = "-1";
                    long dateMinVersion = GetDateAndValueVersion(minVersion, out valueMinVersion);
                    string valueVersion = "-1";
                    long dateVersion = GetDateAndValueVersion(version, out valueVersion);
                    #endregion

                    if (directories.Length != 0 && (dateMinVersion <= dateVersion || dateVersion == -1))
                    {
                        long newDateVersion = dateMinVersion;
                        string newValueVersion = valueMinVersion.ToString();
                        int executionResult = 0;
                        try
                        {
                            foreach (string directory in directories)
                            {
                                string directoryName = directory.Substring(directory.LastIndexOf('\\') + 1, directory.Length - directory.LastIndexOf('\\') - 1);
                                long name;
                                if (ValidateDateFormat(directoryName))
                                { //valida que el nombre del directorio tenga el formato de fecha esperado
                                    if (long.TryParse(directoryName, out name) &&
                                        (name <= dateVersion || dateVersion == -1) && //si la version del directorio es menor que la version pedida
                                        (name >= dateMinVersion || dateMinVersion == -1))//si la version del directorio es mayor que la version de la base de datos
                                    {
                                        string[] scripts = Directory.GetFiles(directory);
                                        if (scripts != null && scripts.Length != 0)
                                        {
                                            foreach (string script in scripts)
                                            {
                                                if (executionResult != -100)
                                                {
                                                    string s = script.Substring(script.LastIndexOf('\\') + 1, script.Length - script.LastIndexOf('\\') - 1);
                                                    s = s.Substring(0, s.IndexOf('.'));
                                                    ValidateFileName(s);
                                                    s = s.Substring(0, 2);
                                                    int scriptName = Convert.ToInt16(s);
                                                    if ((scriptName > Convert.ToInt16(valueMinVersion) || name != dateMinVersion || valueMinVersion.Equals("-1")) &&
                                                        (scriptName <= Convert.ToInt16(valueVersion) || name != dateVersion || valueVersion.Equals("-1")))
                                                    {
                                                        LogInfo("Executing script: " + directoryName + "_" + s);
                                                        try
                                                        {
                                                            string command;
                                                            Encoding encoding = Encoding.GetEncoding("ISO-8859-1");
                                                            using (FileStream strm = File.OpenRead(script))
                                                            {
                                                                StreamReader file = new StreamReader(strm, encoding);
                                                                command = file.ReadToEnd();
                                                            }
                                                            executionResult = ExeSQL(command, _connectionString);

                                                            newDateVersion = name;
                                                            newValueVersion = s;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            LogInfo(ex.Message);
                                                        }
                                                    }
                                                }
                                                if (executionResult != -100)
                                                {
                                                    LogInfo("Updating db version");
                                                    UpdateDBVersion(newDateVersion, newValueVersion, _connectionString, areaName);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            LogInfo("No scripts to execute");
                                        }
                                    }
                                }
                                else
                                {
                                    LogInfo("Warning: '" + directoryName + "' has wrong format.");
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            LogInfo(e.Message);
                        }
                    }
                }
                else
                {
                    LogInfo("No scripts to execute");
                }
            }
            catch (Exception e)
            {
                LogInfo("General error: " + e.Message);
            }
        }

        #endregion

        #region scripts
        /// <summary>
        /// Obtiene la coleccion de nombres de los directorios de scripts de la carpeta de scripts desde la siguiente version de la base de datos en adelante
        /// </summary>
        /// <returns></returns>
        public static StringCollection GetScriptsDirectoriesNames()
        {
            return GetScriptsDirectoriesNames("-1", ScriptsPath, ConnectionString, null);
        }

        public static StringCollection GetScriptsDirectoriesNames(string version)
        {
            return GetScriptsDirectoriesNames(version, ScriptsPath, ConnectionString, null);
        }


        /// <summary>
        /// Obtiene una coleccion de nombres de los directorios de scripts de la carpeta de scripts desde la siguiente version de la base de datos hasta la 
        /// version pasada como parametro
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static StringCollection GetScriptsDirectoriesNames(string version, string _ScriptsPath, string _connectionString, string areaName)
        {
            StringCollection result = new StringCollection();
            try
            {
                ValidateVersionParameter(version);
                string minVersion = GetLastDBVersion(_connectionString, areaName);
                try
                {
                    ValidateVersionParameter(minVersion);
                }
                catch (Exception)
                {
                    LogInfo("DBVersion has wrong entry; assuming no database at all.");
                    minVersion = "-1";
                }
                string[] directories = Directory.GetDirectories(_ScriptsPath);
                if (directories != null && directories.Length != 0)
                {
                    Array.Sort(directories);

                    string valueMinVersion = "-1";
                    long dateMinVersion = GetDateAndValueVersion(minVersion, out valueMinVersion);
                    string valueVersion = "-1";
                    long dateVersion = GetDateAndValueVersion(version, out valueVersion);

                    if (directories.Length != 0 && (dateMinVersion <= dateVersion || dateVersion == -1))
                    {
                        foreach (string directory in directories)
                        {
                            string directoryName = directory.Substring(directory.LastIndexOf('\\') + 1, directory.Length - directory.LastIndexOf('\\') - 1);
                            long name;
                            if (ValidateDateFormat(directoryName))
                            {//valida que el nombre del directorio tenga el formato de fecha esperado
                                if (long.TryParse(directoryName, out name) &&
                                    (name <= dateVersion || dateVersion == -1) && //si la version del directorio es menor que la version pedida
                                    (name > dateMinVersion || dateMinVersion == -1)) //si la version del directorio es mayor que la version de la base de datos
                                {
                                    result.Add(directoryName);
                                    string[] scripts = Directory.GetFiles(directory);
                                    if (scripts != null && scripts.Length != 0)
                                    {
                                        foreach (string script in scripts)
                                        {
                                            string s = script.Substring(script.LastIndexOf('\\') + 1, script.Length - script.LastIndexOf('\\') - 1);
                                            s = s.Substring(0, s.IndexOf('.'));
                                            ValidateFileName(s);
                                            int scriptName = Convert.ToInt16(s);
                                            if ((scriptName > Convert.ToInt16(valueMinVersion) || valueMinVersion.Equals("-1")) &&
                                                (scriptName <= Convert.ToInt16(valueVersion) || valueVersion.Equals("-1")))
                                            {
                                                result.Add("    ∟" + s);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        LogInfo("No scripts to execute");
                                    }
                                }
                            }
                            else
                            {
                                LogInfo("Warning: '" + directoryName + "' has wrong format.");
                            }
                        }
                    }
                }
                else
                {
                    LogInfo("No scripts to execute");
                }
            }
            catch (Exception e)
            {
                LogInfo("General error: " + e.Message);
            }
            return result;
        }
        #endregion

        #region version

        public static string GetLastDBVersion()
        {
            return GetLastDBVersion(ConnectionString, null);
        }
        /// <summary>
        /// Obtiene el numero de version actual de la base de datos. 
        /// Si la base no tiene version entonces se devuelve -1.
        /// </summary>
        /// <returns></returns>
        public static string GetLastDBVersion(string _connectionString, string areaName)
        {
            string result = "-1";
            string createPAVersions =
                            "IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[" + TableName + "]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) " +
                            "CREATE TABLE [" + TableName + "] ( " +
                            "[Version] [nvarchar](50) NOT NULL,  " +
                            "[" + AreaName + "] [nvarchar](100) NOT NULL  " +
                            ") ON [PRIMARY] ; " +
                            "if not Exists(select * from sys.columns where Name = N'" + AreaName + "' " +
                            "and Object_ID = Object_ID(N'[" + TableName + "]')) " +
                            " ALTER TABLE [" + TableName + "] " +
                            " ADD " + AreaName + " [nvarchar](100) NULL ; ";


            string updatePAVersion = " update [" + TableName + "] set [" + AreaName + "] = ";
            if (string.IsNullOrEmpty(areaName))
            {
                updatePAVersion += "'' ";
            }
            else
            {
                updatePAVersion += "'" + areaName + "' ";
            }

            updatePAVersion += " where [" + AreaName + "] is null ; " +
                                " ALTER TABLE [" + TableName + "] " +
             " Alter column [" + AreaName + "] [nvarchar](100) NOT NULL ; " +
             " if exists (select count(*) from [" + TableName + "] db " +
                " group by db.AreaName " +
                " having COUNT(*) >1) " +
                "RAISERROR (N'Actualmente hay mas de una version para la misma area!. Revisar',1,1) ";

            try
            {
                ExeSQL(createPAVersions, _connectionString);
                ExeSQL(updatePAVersion, _connectionString);
                string sql = "SELECT [Version] FROM [" + TableName + "]";
                if (!string.IsNullOrEmpty(areaName))
                {
                    sql += " where [" + AreaName + "] = '" + areaName + "'";
                }
                Object obj = ExecuteSQLScalar(sql, _connectionString);
                if (obj != null)
                {
                    result = Convert.ToString(obj);
                }
            }
            catch (Exception ex)
            {
                LogInfo("Cannot get db version: " + ex.Message);
            }
            return result;
        }
        #endregion

        #region consultas y ejecuciones sobre la base

        private static object ExecuteSQLScalar(string sql)
        {
            return ExecuteSQLScalar(sql, ConnectionString);
        }

        private static object ExecuteSQLScalar(string sql, string _connectionString)
        {
            object returnValue = null;

            using (SqlConnection myConnection = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, myConnection))
                {
                    try
                    {
                        sqlCommand.Connection.Open();
                        sqlCommand.CommandTimeout = 300;
                        returnValue = sqlCommand.ExecuteScalar();
                    }
                    catch (Exception e)
                    {
                        LogInfo("SQL executing error: " + e.Message);
                    }
                    finally
                    {
                        sqlCommand.Dispose();
                        myConnection.Close();
                        myConnection.Dispose();
                    }
                }
            }
            return returnValue;
        }
        private static void UpdateDBVersion(long dateVersion, string valueVersion)
        {
            UpdateDBVersion(dateVersion, valueVersion, ConnectionString, null);
        }

        private static void UpdateDBVersion(long dateVersion, string valueVersion, string _connectionString, string areaName)
        {
            if (dateVersion != -1 && !valueVersion.Equals("-1"))
            {
                try
                {
                    string sql = "UPDATE [" + TableName + "] SET [Version] = '" + dateVersion.ToString() + "_" + valueVersion + "'";
                    if (!string.IsNullOrEmpty(areaName))
                    {
                        sql += " where [" + AreaName + "] = '" + areaName + "'";
                    }
                    int count = ExeSQL(sql, _connectionString);//actualiza el valor de la version

                    if (count < 1)
                    {
                        sql = "INSERT INTO [" + TableName + "] VALUES('" + dateVersion.ToString() + "_" + valueVersion + "', '";
                        if (!string.IsNullOrEmpty(areaName))
                        {
                            sql += areaName;
                        }
                        sql += "')";
                        ExeSQL(sql, _connectionString);//inserta el registro con la version nueva
                    }
                }
                catch (Exception ex)
                {
                    LogInfo("Couldn´t update database: " + ex.Message);
                }
            }
        }

        private static int ExeSQL(string sqlInput)
        {
            return ExeSQL(sqlInput, ConnectionString);
        }

        private static int ExeSQL(string sqlInput, string _ConnectionString)
        {
            int returnValue = -1;

            SqlConnection conn = new SqlConnection(_ConnectionString);
            conn.Open();

            Regex regex = new Regex("^GO", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex regex2 = new Regex("^USE \\S*]", RegexOptions.IgnoreCase);
            Regex regex3 = new Regex(@" \[.+\]\.\[dbo\]", RegexOptions.IgnoreCase);

            string sql = regex2.Replace(sqlInput, string.Empty);
            sql = regex3.Replace(sql, " [dbo]");
            string[] cmds = regex.Split(sql);

            SqlCommand sp_createcmd = new SqlCommand(string.Empty, conn);
            sp_createcmd.Transaction = conn.BeginTransaction();
            string currentCmd = "";
            try
            {
                foreach (string cmd in cmds)
                {
                    currentCmd = cmd;
                    if (cmd.Length > 0 && !cmd.Contains("Inicio"))
                    {
                        sp_createcmd.CommandText = cmd;
                        sp_createcmd.CommandType = System.Data.CommandType.Text;
                        sp_createcmd.CommandTimeout = 300;
                        returnValue = sp_createcmd.ExecuteNonQuery();
                    }
                }
                sp_createcmd.Transaction.Commit();
                // returnValue = 1;
            }
            catch (Exception e)
            {
                if (sp_createcmd.Transaction != null)
                {
                    sp_createcmd.Transaction.Rollback();
                }
                LogInfo("SQL Command Error: " + currentCmd + " . Details: " + e.Message);
                returnValue = -100;

            }
            finally
            {
                conn.Close();
            }

            return returnValue;
        }

        #endregion

        #region validaciones
        private static bool ValidateDateFormat(string dateName)
        {
            dateName = dateName.Trim();
            if (dateName.Length != 8)
            {
                return false;
            }
            bool result = false;
            try
            {
                int year = Convert.ToInt32(dateName.Substring(0, 4));
                int month = Convert.ToInt32(dateName.Substring(4, 2));
                int day = Convert.ToInt32(dateName.Substring(6, 2));
                DateTime date = new DateTime(year, month, day);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        private static void ValidateFileName(string scriptName)
        {
            if (scriptName.Trim().Length < 2)
            {
                LogInfo("Script name (" + scriptName + ") has wrong format. It should start with a two digit number.");
                throw new Exception("Script name (" + scriptName + ") has wrong format. It should start with a two digit number.");
            }
            else
            {
                string substring = scriptName.Substring(0, 2);
                int scriptValue = -1;
                if (!int.TryParse(substring, out scriptValue))
                {
                    throw new Exception("Script name (" + scriptName + ") has wrong format. It should start with a two digit number.");
                }
            }
        }

        private static void ValidateVersionParameter(string version)
        {
            if (!version.Equals("-1"))
            {
                try
                {
                    string[] array = version.Split('_');
                    if (!ValidateDateFormat(array[0]))
                    {
                        throw new Exception();
                    }
                    else
                    {
                        ValidateFileName(array[1]);
                    }
                }
                catch (Exception)
                {
                    throw new Exception(ErrorMessage);
                }
            }
        }
        #endregion

        #region propiedades
        private static string ConnectionString
        {
            get
            {
                System.Configuration.AppSettingsReader reader = new System.Configuration.AppSettingsReader();
                return reader.GetValue("ConnectionString", typeof(string)).ToString();
            }
        }

        private static string ScriptsPath
        {
            get
            {
                System.Configuration.AppSettingsReader reader = new System.Configuration.AppSettingsReader();
                return reader.GetValue("ScriptsPath", typeof(string)).ToString();
            }
        }

        private static string ErrorMessage
        {
            get
            {
                return "El formato del la version debe ser aaaammdd_vv, donde aaaa representa un año, dd un dia, mm un mes y vv un valor numerico cualquiera de dos cifras";
            }
        }
        #endregion

        #region metodos auxiliares
        private static long GetDateAndValueVersion(string minVersion, out string valueMinVersion)
        {
            long dateMinVersion = -1;
            valueMinVersion = "-1";
            if (!minVersion.Equals("-1"))
            {
                string[] array = minVersion.Split('_');
                dateMinVersion = Convert.ToInt64(array[0]);
                valueMinVersion = array[1];
            }
            return dateMinVersion;
        }
        #endregion

        private static void LogInfo(string info)
        {
            info += "\n";
            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\rb_logs\\DbScriptsLog.txt", info);

        }

        public static string GetFileCurrentVersion(string _ScriptsPath)
        {
            string currentVersion = string.Empty;
            try
            {
                string[] directories = Directory.GetDirectories(_ScriptsPath);
                if (directories == null || directories.Length == 0)
                {
                    LogInfo("Warning: El directorio de scripts esta vacio.");
                    return string.Empty;
                }
                Array.Sort(directories);
                for (int i = directories.Length - 1; i >= 0; i--)
                {
                    string directoryPath = directories.GetValue(i).ToString();
                    string directoryName = directoryPath.Substring(directoryPath.LastIndexOf('\\') + 1, directoryPath.Length - directoryPath.LastIndexOf('\\') - 1);
                    long name;

                    if (long.TryParse(directoryName, out name) &&
                        ValidateDateFormat(directoryName) //valida que el nombre del directorio tenga el formato de fecha esperado
                        )
                    {
                        string[] files = Directory.GetFiles(directoryPath);
                        if (files != null && files.Length != 0)
                        {
                            Array.Sort(files);

                            string fullPath = files.GetValue(files.Length - 1).ToString();
                            currentVersion = fullPath.Substring(fullPath.LastIndexOf("\\") + 1, 2);

                            int result = 0;
                            if (int.TryParse(currentVersion, out result))
                            {
                                fullPath = fullPath.Substring(0, fullPath.LastIndexOf("\\"));
                                string strDate = fullPath.Substring(fullPath.LastIndexOf("\\") + 1);
                                CultureInfo esES = new CultureInfo("es-ES");
                                DateTime date;
                                if (DateTime.TryParse(strDate.Substring(6, 2) + "/" +
                                                      strDate.Substring(4, 2) + "/" +
                                                      strDate.Substring(0, 4), esES, DateTimeStyles.None, out date))
                                {
                                    currentVersion = strDate + "_" + currentVersion;
                                }
                                else
                                {
                                    throw new Exception("Formato de nombre de directorio incorrecto");
                                }
                                return currentVersion;

                            }
                            else
                            {
                                throw new Exception("Formato de nombre de archivo incorrecto");
                            }
                        }
                        else
                        {
                            LogInfo("Warning: La carpeta '" + directoryName + "' no contiene scripts. No será tomada en cuenta como la última versión.");
                        }
                    }
                    else
                    {
                        LogInfo("Warning: La carpeta '" + directoryName + "' no tiene el formato esperado. No será tomada en cuenta como la última versión.");
                    }


                }


            }
            catch (Exception ex)
            {
                LogInfo("ERROR al intentar obtener la versión de archivo. Detalle -> " + ex.Message);
                currentVersion = string.Empty;
            }
            return currentVersion;
        }

        public static string GetFileCurrentVersion()
        {
            return GetFileCurrentVersion(ScriptsPath);
        }
    }
}
