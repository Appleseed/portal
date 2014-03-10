//------------------------------------------------------------------------------
// <copyright file="SqlProfileProvider.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Specialized;
using System.Web.Profile;
using System.Configuration;
using System.Data;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using System.Web;
namespace Microsoft.Samples {
   

    /* TODO:
     * 
     * Test page size/index stuff
     * 6. Utility/sproc generation
     * 
     * REVIEW:
     * - Strings that are too long will throw an exception saying data will be truncated...
     * - Xml, Udt, Variant SqlDbDatatypes won't work (no corresponding column types for objects, for xml etc use strings)
                <add name="Timestamp" type="System.Byte[]" provider="R2" allowAnonymous="true" customProviderData="Timestamp;Timestamp"/>
     * 
     * 
     * DONE:
     * 1. Handle non string cases (double, date time, blob)
     * 2. Batch select up into one query, list<columns>
     * DeleteProfiles[]
     * 4. Implement other methods (GetProfiles, GetNumberInactive, DeleteInactive!)
     * Require LastActivityDate
     */

    public class SqlTableProfileProvider : ProfileProvider {
        private string _appName;
        private Guid _appId;
        private bool _appIdSet;
        private string _sqlConnectionString;
        private int _commandTimeout;
        private string _table;

        public override void Initialize(string name, NameValueCollection config) {
            if (config == null)
                throw new ArgumentNullException("config");
            if (String.IsNullOrEmpty(name))
                name = "SqlTableProfileProvider";
            if (string.IsNullOrEmpty(config["description"])) {
                config.Remove("description");
                config.Add("description", "SqlTableProfileProvider");
            }
            base.Initialize(name, config);

            string temp = config["connectionStringName"];
            if (String.IsNullOrEmpty(temp))
                throw new ProviderException("connectionStringName not specified");
            _sqlConnectionString = SqlStoredProcedureProfileProvider.GetConnectionString(temp);
            if (String.IsNullOrEmpty(_sqlConnectionString)) {
                throw new ProviderException("connectionStringName not specified");
            }

            _appName = config["applicationName"];
            if (string.IsNullOrEmpty(_appName))
                _appName = SqlStoredProcedureProfileProvider.GetDefaultAppName();

            if (_appName.Length > 256) {
                throw new ProviderException("Application name too long");
            }

            _table = config["table"];
            if (string.IsNullOrEmpty(_table)) {
                throw new ProviderException("No table specified");
            }
            EnsureValidTableOrColumnName(_table);

            string timeout = config["commandTimeout"];
            if (string.IsNullOrEmpty(timeout) || !Int32.TryParse(timeout, out _commandTimeout)) {
                _commandTimeout = 30;
            }

            config.Remove("commandTimeout");
            config.Remove("connectionStringName");
            config.Remove("applicationName");
            config.Remove("table");
            if (config.Count > 0) {
                string attribUnrecognized = config.GetKey(0);
                if (!String.IsNullOrEmpty(attribUnrecognized))
                    throw new ProviderException("Unrecognized config attribute:" + attribUnrecognized);
            }
        }

        private bool AppIdSet
        {
            get {
                if (HttpContext.Current != null)
                {
                    return (bool)HttpContext.Current.Items["Profile.ApplicationIdSet"];
                }
                
                return _appIdSet;
                
            }
            set {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items["Profile.ApplicationIdSet"] = value;

                }

                _appIdSet = value;
            }
        }

        //public override string ApplicationName {
        //    get { return _appName; }
        //    set {
        //        if (value == null) 
        //            throw new ArgumentNullException("ApplicationName");
        //        if (value.Length > 256) {
        //            throw new ProviderException("Application name too long");
        //        }
        //        _appName = value;
        //        AppIdSet = false;
        //    }
        //}

        public override string ApplicationName
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return (string)HttpContext.Current.Items["Profile.ApplicationName"];
                }
                else
                {
                    return _appName;
                }
            }
            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items["Profile.ApplicationName"] = value;
                 
                }

                _appName = value;
                AppIdSet = false;
            }
        }

        private Guid AppId {
            get {
                if (!AppIdSet)
                {
                    SqlConnection conn = null;
                    try {
                        conn = new SqlConnection(_sqlConnectionString);
                        conn.Open();

                        SqlCommand cmd = new SqlCommand("dbo.aspnet_Applications_CreateApplication", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@applicationname", ApplicationName);
                        cmd.Parameters.Add(CreateOutputParam("@ApplicationId", SqlDbType.UniqueIdentifier, 0));

                        cmd.ExecuteNonQuery();
                        _appId = (Guid)cmd.Parameters["@ApplicationId"].Value;
                        AppIdSet = true;
                    }
                    finally {
                        if (conn != null) {
                            conn.Close();
                        }
                    }

                }
                return _appId;
            }
        }

        private int CommandTimeout {
            get { return _commandTimeout; }
        }

        ////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////

        private static string s_legalChars = "_@#$";
        private static void EnsureValidTableOrColumnName(string name) {
            for (int i = 0; i < name.Length; ++i) {
                if (!Char.IsLetterOrDigit(name[i]) && s_legalChars.IndexOf(name[i]) == -1)
                    throw new ProviderException("Table and column names cannot contain: "+name[i]);
            }
        }

        private void GetProfileDataFromTable(SettingsPropertyCollection properties, SettingsPropertyValueCollection svc, string username, SqlConnection conn) {
            List<ProfileColumnData> columnData = new List<ProfileColumnData>(properties.Count);
            StringBuilder commandText = new StringBuilder("SELECT u.UserID");
            SqlCommand cmd = new SqlCommand(String.Empty, conn);

            int columnCount = 0;
            foreach (SettingsProperty prop in properties) {
                SettingsPropertyValue value = new SettingsPropertyValue(prop);
                svc.Add(value);

                string persistenceData = prop.Attributes["CustomProviderData"] as string;
                // If we can't find the table/column info we will ignore this data
                if (String.IsNullOrEmpty(persistenceData)) {
                    // REVIEW: Perhaps we should throw instead?
                    continue;
                }
                string[] chunk = persistenceData.Split(new char[] { ';' });
                if (chunk.Length != 2) {
                    // REVIEW: Perhaps we should throw instead?
                    continue;
                }
                string columnName = chunk[0];
                // REVIEW: Should we ignore case?
                SqlDbType datatype = (SqlDbType)Enum.Parse(typeof(SqlDbType), chunk[1], true);

                columnData.Add(new ProfileColumnData(columnName, value, null /* not needed for get */, datatype));
                commandText.Append(", ");
                commandText.Append("t."+columnName);
                ++columnCount;
            }

            commandText.Append(" FROM "+_table+" t, vw_aspnet_Users u WHERE u.ApplicationId = '").Append(AppId);
            commandText.Append("' AND u.UserName = LOWER(@Username) AND t.UserID = u.UserID");
            cmd.CommandText = commandText.ToString();
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@Username", username);
            SqlDataReader reader = null;

            try {
                reader = cmd.ExecuteReader();
                //If no row exists in the database, then the default Profile values
                //from configuration are used.
                if (reader.Read()) {
                    Guid userId = reader.GetGuid(0);
                    for (int i = 0; i < columnData.Count; ++i) {
                        object val = reader.GetValue(i+1);
                        ProfileColumnData colData = columnData[i];
                        SettingsPropertyValue propValue = colData.PropertyValue;

                        //Only initialize a SettingsPropertyValue for non-null values
                        if (!(val is DBNull || val == null))
                        {
                            propValue.PropertyValue = val;
                            propValue.IsDirty = false;
                            propValue.Deserialized = true;
                        }
                    }

                    // need to close reader before we try to update the user
                    if (reader != null) {
                        reader.Close();
                        reader = null;
                    }

                }
            }
            finally {
                if (reader != null) {
                    reader.Close();
                }
            }
        }

        

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection) {
            SettingsPropertyValueCollection svc = new SettingsPropertyValueCollection();

            if (collection == null || collection.Count < 1 || context == null)
                return svc;

            string username = (string)context["UserName"];
            if (String.IsNullOrEmpty(username))
                return svc;

            SqlConnection conn = null;
            try {
                conn = new SqlConnection(_sqlConnectionString);
                conn.Open();

                GetProfileDataFromTable(collection, svc, username, conn);
            }
            finally {
                if (conn != null) {
                    conn.Close();
                }
            }

            return svc;
        }

        ////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////

        // Container struct for use in aggregating columns for queries
        private struct ProfileColumnData {
            public string ColumnName;
            public SettingsPropertyValue PropertyValue;
            public object Value;
            public SqlDbType DataType;

            public ProfileColumnData(string col, SettingsPropertyValue pv, object val, SqlDbType type) {
                EnsureValidTableOrColumnName(col);
                ColumnName = col;
                PropertyValue = pv;
                Value = val;
                DataType = type;
            }
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection) {
            string username = (string)context["UserName"];
            bool userIsAuthenticated = (bool)context["IsAuthenticated"];

            if (username == null || username.Length < 1 || collection.Count < 1)
                return;

            SqlConnection conn = null;
            SqlDataReader reader = null;
            SqlCommand cmd = null;
            try {
                bool anyItemsToSave = false;

                // First make sure we have at least one item to save
                foreach (SettingsPropertyValue pp in collection) {
                    if (pp.IsDirty) {
                        if (!userIsAuthenticated) {
                            bool allowAnonymous = (bool)pp.Property.Attributes["AllowAnonymous"];
                            if (!allowAnonymous)
                                continue;
                        }
                        anyItemsToSave = true;
                        break;
                    }
                }

                if (!anyItemsToSave)
                    return;

                conn = new SqlConnection(_sqlConnectionString);
                conn.Open();

                List<ProfileColumnData> columnData = new List<ProfileColumnData>(collection.Count);

                foreach (SettingsPropertyValue pp in collection) {
                    if (!userIsAuthenticated) {
                        bool allowAnonymous = (bool)pp.Property.Attributes["AllowAnonymous"];
                        if (!allowAnonymous)
                            continue;
                    }

                    //Normal logic for original SQL provider
                    //if (!pp.IsDirty && pp.UsingDefaultValue) // Not fetched from DB and not written to
                    
                    //Can eliminate unnecessary updates since we are using a table though
                    if (!pp.IsDirty)
                        continue;

                    string persistenceData = pp.Property.Attributes["CustomProviderData"] as string;
                    // If we can't find the table/column info we will ignore this data
                    if (String.IsNullOrEmpty(persistenceData)) {
                        // REVIEW: Perhaps we should throw instead?
                        continue;
                    }
                    string[] chunk = persistenceData.Split(new char[] { ';' });
                    if (chunk.Length != 2) {
                        // REVIEW: Perhaps we should throw instead?
                        continue;
                    }
                    string columnName = chunk[0];
                    // REVIEW: Should we ignore case?
                    SqlDbType datatype = (SqlDbType)Enum.Parse(typeof(SqlDbType), chunk[1], true);

                    object value = null;

                    // REVIEW: Is this handling null case correctly?
                    if (pp.Deserialized && pp.PropertyValue == null) { // is value null?
                        value = DBNull.Value;
                    }
                    else {
                        value = pp.PropertyValue;
                    }

                    // REVIEW: Might be able to ditch datatype
                    columnData.Add(new ProfileColumnData(columnName, pp, value, datatype));
                }

                // Figure out userid, if we don't find a userid, go ahead and create a user in the aspnetUsers table
                Guid userId = Guid.Empty;
                cmd = new SqlCommand("SELECT u.UserId FROM vw_aspnet_Users u WHERE u.ApplicationId = '" + AppId + "' AND u.UserName = LOWER(@Username)", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Username",username);
                try {
                    reader = cmd.ExecuteReader();
                    if (reader.Read()) {
                        userId = reader.GetGuid(0);
                    }
                    else {
                        reader.Close();
                        cmd.Dispose();
                        reader = null;

                        cmd = new SqlCommand("dbo.aspnet_Users_CreateUser", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ApplicationId", AppId);
                        cmd.Parameters.AddWithValue("@UserName", username);
                        cmd.Parameters.AddWithValue("@IsUserAnonymous", !userIsAuthenticated);
                        cmd.Parameters.AddWithValue("@LastActivityDate", DateTime.UtcNow);
                        cmd.Parameters.Add(CreateOutputParam("@UserId", SqlDbType.UniqueIdentifier, 16));

                        cmd.ExecuteNonQuery();
                        userId = (Guid)cmd.Parameters["@userid"].Value;
                    }
                }
                finally {
                    if (reader != null) {
                        reader.Close();
                        reader = null;
                    }
                    cmd.Dispose();
                }

                // Figure out if the row already exists in the table and use appropriate SELECT/UPDATE
                cmd = new SqlCommand(String.Empty, conn);
                StringBuilder sqlCommand = new StringBuilder("IF EXISTS (SELECT 1 FROM ").Append(_table);
                sqlCommand.Append(" WHERE UserId = @UserId) ");
                cmd.Parameters.AddWithValue("@UserId", userId);

                // Build up strings used in the query
                StringBuilder columnStr = new StringBuilder();
                StringBuilder valueStr = new StringBuilder();
                StringBuilder setStr = new StringBuilder();
                int count = 0;
                foreach (ProfileColumnData data in columnData) {
                    columnStr.Append(", ");
                    valueStr.Append(", ");
                    columnStr.Append(data.ColumnName);
                    string valueParam = "@Value" + count;
                    valueStr.Append(valueParam);
                    cmd.Parameters.AddWithValue(valueParam, data.Value);

                    // REVIEW: Can't update Timestamps?
                    if (data.DataType != SqlDbType.Timestamp) {
                        if (count > 0) {
                            setStr.Append(",");
                        }
                        setStr.Append(data.ColumnName);
                        setStr.Append("=");
                        setStr.Append(valueParam);
                    }

                    ++count;
                }
                columnStr.Append(",LastUpdatedDate ");
                valueStr.Append(",@LastUpdatedDate");
                setStr.Append(",LastUpdatedDate=@LastUpdatedDate");
                cmd.Parameters.AddWithValue("@LastUpdatedDate", DateTime.UtcNow);

                sqlCommand.Append("BEGIN UPDATE ").Append(_table).Append(" SET ").Append(setStr.ToString());
                sqlCommand.Append(" WHERE UserId = '").Append(userId).Append("'");

                sqlCommand.Append("END ELSE BEGIN INSERT ").Append(_table).Append(" (UserId").Append(columnStr.ToString());
                sqlCommand.Append(") VALUES ('").Append(userId).Append("'").Append(valueStr.ToString()).Append(") END");

                cmd.CommandText = sqlCommand.ToString();
                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();

                // Need to close reader before we try to update
                if (reader != null) {
                    reader.Close();
                    reader = null;
                }
            }
            finally {
                if (reader != null)
                    reader.Close();
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Close();
            }
        }

        ////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////

        private static SqlParameter CreateInputParam(string paramName, SqlDbType dbType, object objValue) {
            SqlParameter param = new SqlParameter(paramName, dbType);
            if (objValue == null)
                objValue = String.Empty;
            param.Value = objValue;
            return param;
        }

        private static SqlParameter CreateOutputParam(string paramName, SqlDbType dbType, int size) {
            SqlParameter param = new SqlParameter(paramName, dbType);
            param.Direction = ParameterDirection.Output;
            param.Size = size;
            return param;
        }

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        // Mangement APIs from ProfileProvider class

        public override int DeleteProfiles(ProfileInfoCollection profiles) {
            if (profiles == null) {
                throw new ArgumentNullException("profiles");
            }

            if (profiles.Count < 1) {
                throw new ArgumentException("Profiles collection is empty");
            }

            string[] usernames = new string[profiles.Count];

            int iter = 0;
            foreach (ProfileInfo profile in profiles) {
                usernames[iter++] = profile.UserName;
            }

            return DeleteProfiles(usernames);
        }
        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        public override int DeleteProfiles(string[] usernames) {
            if (usernames == null || usernames.Length < 1) {
                return 0;
            }

            int numProfilesDeleted = 0;
            bool beginTranCalled = false;
            try {
                SqlConnection conn = null;
                try {
                    conn = new SqlConnection(_sqlConnectionString);
                    conn.Open();

                    SqlCommand cmd;
                    int numUsersRemaing = usernames.Length;
                    while (numUsersRemaing > 0) {
                        cmd = new SqlCommand(String.Empty, conn);
                        cmd.Parameters.AddWithValue("@UserName0", usernames[usernames.Length - numUsersRemaing]);
                        StringBuilder allUsers = new StringBuilder("@UserName0");
                        numUsersRemaing--;

                        int userIndex = 1;
                        for (int iter = usernames.Length - numUsersRemaing; iter < usernames.Length; iter++) {
                            // REVIEW: Should we check length of command string instead of parameter lengths?
                            if (allUsers.Length + usernames[iter].Length + 3 >= 4000)
                                break;
                            string userNameParam = "@UserName" + userIndex;
                            allUsers.Append(",");
                            allUsers.Append(userNameParam);
                            cmd.Parameters.AddWithValue(userNameParam, usernames[iter]);
                            numUsersRemaing--;
                            ++userIndex;
                        }

                        // We don't need to start a transaction if we can finish this in one sql command
                        if (!beginTranCalled && numUsersRemaing > 0) {
                            SqlCommand beginCmd = new SqlCommand("BEGIN TRANSACTION", conn);
                            beginCmd.ExecuteNonQuery();
                            beginTranCalled = true;
                        }

                       
                        cmd.CommandText = "DELETE FROM " + _table + " WHERE UserId IN ( SELECT u.UserId FROM vw_aspnet_Users u WHERE u.ApplicationId = '" + AppId + "' AND u.UserName IN (" + allUsers.ToString() + "))";
                        cmd.CommandTimeout = CommandTimeout;
                        numProfilesDeleted += cmd.ExecuteNonQuery(); 
                    }

                    if (beginTranCalled) {
                        cmd = new SqlCommand("COMMIT TRANSACTION", conn);
                        cmd.ExecuteNonQuery();
                        beginTranCalled = false;
                    }
                }
                catch {
                    if (beginTranCalled) {
                        SqlCommand cmd = new SqlCommand("ROLLBACK TRANSACTION", conn);
                        cmd.ExecuteNonQuery();
                        beginTranCalled = false;
                    }
                    throw;
                }
                finally {
                    if (conn != null) {
                        conn.Close();
                        conn = null;
                    }
                }
            }
            catch {
                throw;
            }
            return numProfilesDeleted;
        }

        private string GenerateQuery(bool delete, ProfileAuthenticationOption authenticationOption) {
            StringBuilder cmdStr = new StringBuilder(200);
            if (delete) cmdStr.Append("DELETE FROM ");
            else cmdStr.Append("SELECT COUNT(*) FROM ");
            cmdStr.Append(_table);
            cmdStr.Append(" WHERE UserId IN (SELECT u.UserId FROM vw_aspnet_Users u WHERE u.ApplicationId = '").Append(AppId);
            cmdStr.Append("' AND (u.LastActivityDate <= @InactiveSinceDate)");
            switch (authenticationOption) {
                case ProfileAuthenticationOption.Anonymous:
                    cmdStr.Append(" AND u.IsAnonymous = 1");
                    break;
                case ProfileAuthenticationOption.Authenticated:
                    cmdStr.Append(" AND u.IsAnonymous = 0");
                    break;
                case ProfileAuthenticationOption.All:
                    // Want to delete all profiles here, so nothing more needed
                    break;
            }
            cmdStr.Append(")");
            return cmdStr.ToString();
        }

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate) {
            try {
                SqlConnection conn = null;
                SqlCommand cmd = null;
                try {
                    conn = new SqlConnection(_sqlConnectionString);
                    conn.Open();

                    cmd = new SqlCommand(GenerateQuery(true, authenticationOption), conn);
                    cmd.CommandTimeout = CommandTimeout;
                    cmd.Parameters.Add(CreateInputParam("@InactiveSinceDate", SqlDbType.DateTime, userInactiveSinceDate.ToUniversalTime()));

                    return cmd.ExecuteNonQuery();
                }
                finally {
                    if (cmd != null) {
                        cmd.Dispose();
                    }
                    if (conn != null) {
                        conn.Close();
                        conn = null;
                    }
                }
            }
            catch {
                throw;
            }
        }

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate) {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try {
                conn = new SqlConnection(_sqlConnectionString);
                conn.Open();

                cmd = new SqlCommand(GenerateQuery(false, authenticationOption), conn);
                cmd.CommandTimeout = CommandTimeout;
                cmd.Parameters.Add(CreateInputParam("@InactiveSinceDate", SqlDbType.DateTime, userInactiveSinceDate.ToUniversalTime()));

                object o = cmd.ExecuteScalar();
                if (o == null || !(o is int))
                    return 0;
                return (int)o;
            }
            finally {
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null) {
                    conn.Close();
                    conn = null;
                }
            }
        }

        // TODO: Implement size
        private StringBuilder GenerateTempInsertQueryForGetProfiles(ProfileAuthenticationOption authenticationOption) {
            StringBuilder cmdStr = new StringBuilder(200);
            cmdStr.Append("INSERT INTO #PageIndexForProfileUsers (UserId) ");
            cmdStr.Append("SELECT u.UserId FROM vw_aspnet_Users u, ").Append(_table);
            cmdStr.Append(" p WHERE ApplicationId = '").Append(AppId);
            cmdStr.Append("' AND u.UserId = p.UserId");
            switch (authenticationOption) {
                case ProfileAuthenticationOption.Anonymous:
                    cmdStr.Append(" AND u.IsAnonymous = 1");
                    break;
                case ProfileAuthenticationOption.Authenticated:
                    cmdStr.Append(" AND u.IsAnonymous = 0");
                    break;
                case ProfileAuthenticationOption.All:
                    // Want to delete all profiles here, so nothing more needed
                    break;
            }
            return cmdStr;
        }

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords) {
            StringBuilder insertQuery = GenerateTempInsertQueryForGetProfiles(authenticationOption);
            return GetProfilesForQuery(null, pageIndex, pageSize, insertQuery, out totalRecords);
        }

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords) {
            StringBuilder insertQuery = GenerateTempInsertQueryForGetProfiles(authenticationOption);
            insertQuery.Append(" AND u.LastActivityDate <= @InactiveSinceDate");
            SqlParameter[] args = new SqlParameter[1];
            args[0] = CreateInputParam("@InactiveSinceDate", SqlDbType.DateTime, userInactiveSinceDate.ToUniversalTime());
            return GetProfilesForQuery(args, pageIndex, pageSize, insertQuery, out totalRecords);
        }

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords) {
            StringBuilder insertQuery = GenerateTempInsertQueryForGetProfiles(authenticationOption);
            insertQuery.Append(" AND u.UserName LIKE LOWER(@UserName)");
            SqlParameter[] args = new SqlParameter[1];
            args[0] = CreateInputParam("@UserName", SqlDbType.NVarChar, usernameToMatch);
            return GetProfilesForQuery(args, pageIndex, pageSize, insertQuery, out totalRecords);
        }

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords) {
            StringBuilder insertQuery = GenerateTempInsertQueryForGetProfiles(authenticationOption);
            insertQuery.Append(" AND u.UserName LIKE LOWER(@UserName) AND u.LastActivityDate <= @InactiveSinceDate");
            SqlParameter[] args = new SqlParameter[2];
            args[0] = CreateInputParam("@InactiveSinceDate", SqlDbType.DateTime, userInactiveSinceDate.ToUniversalTime());
            args[1] = CreateInputParam("@UserName", SqlDbType.NVarChar, usernameToMatch);
            return GetProfilesForQuery(args, pageIndex, pageSize, insertQuery, out totalRecords);
        }

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        // Private methods

        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////


        public List<Guid> GetUserIdsByCategoryId(int categoryId)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            SqlCommand cmd = null;

            List<Guid> result = new List<Guid>();
            conn = new SqlConnection(_sqlConnectionString);
            conn.Open();

            StringBuilder cmdStr = new StringBuilder(200);
            cmdStr.Append("SELECT u.UserId FROM aspnet_CustomProfile u");
            cmdStr.AppendFormat(" WHERE u.CategoryId = '{0}'", categoryId);
            cmd = new SqlCommand(cmdStr.ToString(), conn);
            cmd.CommandTimeout = CommandTimeout;

            reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
            ProfileInfoCollection profiles = new ProfileInfoCollection();
            while (reader.Read())
            {
                result.Add(reader.GetGuid(0));
            }

            if (reader != null)
            {
                reader.Close();
                reader = null;
            }

            cmd.Dispose();


            return result;
        }

        private ProfileInfoCollection GetProfilesForQuery(SqlParameter[] insertArgs, int pageIndex, int pageSize, StringBuilder insertQuery, out int totalRecords) {
            if (pageIndex < 0)
                throw new ArgumentException("pageIndex");
            if (pageSize < 1)
                throw new ArgumentException("pageSize");

            long lowerBound = (long)pageIndex * pageSize;
            long upperBound = lowerBound + pageSize - 1;
            if (upperBound > Int32.MaxValue) {
                throw new ArgumentException("pageIndex and pageSize");
            }

            SqlConnection conn = null;
            SqlDataReader reader = null;
            SqlCommand cmd = null;
            try {
                conn = new SqlConnection(_sqlConnectionString);
                conn.Open();

                StringBuilder cmdStr = new StringBuilder(200);
                // Create a temp table TO store the select results
                cmd = new SqlCommand("CREATE TABLE #PageIndexForProfileUsers(IndexId int IDENTITY (0, 1) NOT NULL, UserId uniqueidentifier)", conn);
                cmd.CommandTimeout = CommandTimeout;
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                insertQuery.Append(" ORDER BY UserName");
                cmd = new SqlCommand(insertQuery.ToString(), conn);
                cmd.CommandTimeout = CommandTimeout;
                if (insertArgs != null) {
                    foreach (SqlParameter arg in insertArgs)
                        cmd.Parameters.Add(arg);
                }

                cmd.ExecuteNonQuery();
                cmd.Dispose();

                cmdStr = new StringBuilder(200);
                cmdStr.Append("SELECT u.UserName, u.IsAnonymous, u.LastActivityDate, p.LastUpdatedDate FROM (vw_aspnet_Users u LEFT OUTER JOIN ").Append(_table);
                cmdStr.Append(" p ON u.UserId = p.UserId) INNER JOIN  #PageIndexForProfileUsers i ON  u.UserId = i.UserId WHERE i.IndexId >= ");
                cmdStr.Append(lowerBound).Append(" AND i.IndexId <= ").Append(upperBound);
                cmd = new SqlCommand(cmdStr.ToString(), conn);
                cmd.CommandTimeout = CommandTimeout;

                reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
                ProfileInfoCollection profiles = new ProfileInfoCollection();
                while (reader.Read()) {
                    string username;
                    DateTime dtLastActivity, dtLastUpdated = DateTime.UtcNow;
                    bool isAnon;

                    username = reader.GetString(0);
                    isAnon = reader.GetBoolean(1);
                    dtLastActivity = DateTime.SpecifyKind(reader.GetDateTime(2), DateTimeKind.Utc);
                    dtLastUpdated = DateTime.SpecifyKind(reader.GetDateTime(3), DateTimeKind.Utc);
                    profiles.Add(new ProfileInfo(username, isAnon, dtLastActivity, dtLastUpdated, 0));
                }
                totalRecords = profiles.Count;

                if (reader != null) {
                    reader.Close();
                    reader = null;
                }

                cmd.Dispose();

                // Cleanup, REVIEW: should move to finally?
                cmd = new SqlCommand("DROP TABLE #PageIndexForProfileUsers", conn);
                cmd.ExecuteNonQuery();

                return profiles;
            }
            finally {
                if (reader != null)
                    reader.Close();

                if (cmd != null)
                    cmd.Dispose();

                if (conn != null) {
                    conn.Close();
                    conn = null;
                }
            }
        }
    }
}





