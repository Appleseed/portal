// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlStoredProcedureProfileProvider.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The sql stored procedure profile provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.Samples
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Configuration.Provider;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Security;
    using System.Web.Hosting;
    using System.Web.Profile;

    /* TODO:
     * 
     * REVIEW:
     * - Strings that are too long will throw an exception saying data will be truncated...
     * - State where I couldn't log in??   ASPXANONYMOUS set
     * 
     */

    /// <summary>
    /// The sql stored procedure profile provider.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class SqlStoredProcedureProfileProvider : ProfileProvider
    {
        #region Constants and Fields

        /// <summary>
        /// The app name.
        /// </summary>
        private string _appName;

        /// <summary>
        /// The command timeout.
        /// </summary>
        private int _commandTimeout;

        /// <summary>
        /// The read sproc.
        /// </summary>
        private string _readSproc;

        /// <summary>
        /// The set sproc.
        /// </summary>
        private string _setSproc;

        /// <summary>
        /// The sql connection string.
        /// </summary>
        private string _sqlConnectionString;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the name of the currently running application.
        /// </summary>
        /// <value>The name of the application.</value>
        /// <returns>A <see cref = "T:System.String" /> that contains the application's shortened name, which does not contain a full path or extension, for example, SimpleAppSettings.</returns>
        /// <remarks>
        /// </remarks>
        public override string ApplicationName
        {
            get
            {
                return this._appName;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("ApplicationName");
                }

                if (value.Length > 256)
                {
                    throw new ProviderException("Application name too long");
                }

                this._appName = value;
            }
        }

        /// <summary>
        ///   Gets the command timeout.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private int CommandTimeout
        {
            get
            {
                return this._commandTimeout;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// When overridden in a derived class, deletes all user-profile data for profiles in which the last activity date occurred before the specified date.
        /// </summary>
        /// <param name="authenticationOption">
        /// One of the <see cref="T:System.Web.Profile.ProfileAuthenticationOption"/> values, specifying whether anonymous, authenticated, or both types of profiles are deleted.
        /// </param>
        /// <param name="userInactiveSinceDate">
        /// A <see cref="T:System.DateTime"/> that identifies which user profiles are considered inactive. If the <see cref="P:System.Web.Profile.ProfileInfo.LastActivityDate"/>  value of a user profile occurs on or before this date and time, the profile is considered inactive.
        /// </param>
        /// <returns>
        /// The number of profiles deleted from the data source.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override int DeleteInactiveProfiles(
            ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new NotSupportedException("This method is not supported for this provider.");
        }

        ////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////
        // customProviderData = "Varname;SqlDbType;size"

        /////////////////////////////////////////////////////////////////////////////
        // Mangement APIs from ProfileProvider class

        /// <summary>
        /// When overridden in a derived class, deletes profile properties and information for the supplied list of profiles.
        /// </summary>
        /// <param name="profiles">
        /// A <see cref="T:System.Web.Profile.ProfileInfoCollection"/>  of information about profiles that are to be deleted.
        /// </param>
        /// <returns>
        /// The number of profiles deleted from the data source.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            throw new NotSupportedException("This method is not supported for this provider.");
        }

        /// <summary>
        /// When overridden in a derived class, deletes profile properties and information for profiles that match the supplied list of user names.
        /// </summary>
        /// <param name="usernames">
        /// A string array of user names for profiles to be deleted.
        /// </param>
        /// <returns>
        /// The number of profiles deleted from the data source.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override int DeleteProfiles(string[] usernames)
        {
            throw new NotSupportedException("This method is not supported for this provider.");
        }

        /// <summary>
        /// When overridden in a derived class, retrieves profile information for profiles in which the last activity date occurred on or before the specified date and the user name matches the specified user name.
        /// </summary>
        /// <param name="authenticationOption">
        /// One of the <see cref="T:System.Web.Profile.ProfileAuthenticationOption"/> values, specifying whether anonymous, authenticated, or both types of profiles are returned.
        /// </param>
        /// <param name="usernameToMatch">
        /// The user name to search for.
        /// </param>
        /// <param name="userInactiveSinceDate">
        /// A <see cref="T:System.DateTime"/> that identifies which user profiles are considered inactive. If the <see cref="P:System.Web.Profile.ProfileInfo.LastActivityDate"/> value of a user profile occurs on or before this date and time, the profile is considered inactive.
        /// </param>
        /// <param name="pageIndex">
        /// The index of the page of results to return.
        /// </param>
        /// <param name="pageSize">
        /// The size of the page of results to return.
        /// </param>
        /// <param name="totalRecords">
        /// When this method returns, contains the total number of profiles.
        /// </param>
        /// <returns>
        /// A <see cref="T:System.Web.Profile.ProfileInfoCollection"/> containing user profile information for inactive profiles where the user name matches the supplied <paramref name="usernameToMatch"/> parameter.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override ProfileInfoCollection FindInactiveProfilesByUserName(
            ProfileAuthenticationOption authenticationOption, 
            string usernameToMatch, 
            DateTime userInactiveSinceDate, 
            int pageIndex, 
            int pageSize, 
            out int totalRecords)
        {
            throw new NotSupportedException("This method is not supported for this provider.");
        }

        /// <summary>
        /// When overridden in a derived class, retrieves profile information for profiles in which the user name matches the specified user names.
        /// </summary>
        /// <param name="authenticationOption">
        /// One of the <see cref="T:System.Web.Profile.ProfileAuthenticationOption"/> values, specifying whether anonymous, authenticated, or both types of profiles are returned.
        /// </param>
        /// <param name="usernameToMatch">
        /// The user name to search for.
        /// </param>
        /// <param name="pageIndex">
        /// The index of the page of results to return.
        /// </param>
        /// <param name="pageSize">
        /// The size of the page of results to return.
        /// </param>
        /// <param name="totalRecords">
        /// When this method returns, contains the total number of profiles.
        /// </param>
        /// <returns>
        /// A <see cref="T:System.Web.Profile.ProfileInfoCollection"/> containing user-profile information for profiles where the user name matches the supplied <paramref name="usernameToMatch"/> parameter.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override ProfileInfoCollection FindProfilesByUserName(
            ProfileAuthenticationOption authenticationOption, 
            string usernameToMatch, 
            int pageIndex, 
            int pageSize, 
            out int totalRecords)
        {
            throw new NotSupportedException("This method is not supported for this provider.");
        }

        /// <summary>
        /// When overridden in a derived class, retrieves user-profile data from the data source for profiles in which the last activity date occurred on or before the specified date.
        /// </summary>
        /// <param name="authenticationOption">
        /// One of the <see cref="T:System.Web.Profile.ProfileAuthenticationOption"/> values, specifying whether anonymous, authenticated, or both types of profiles are returned.
        /// </param>
        /// <param name="userInactiveSinceDate">
        /// A <see cref="T:System.DateTime"/> that identifies which user profiles are considered inactive. If the <see cref="P:System.Web.Profile.ProfileInfo.LastActivityDate"/>  of a user profile occurs on or before this date and time, the profile is considered inactive.
        /// </param>
        /// <param name="pageIndex">
        /// The index of the page of results to return.
        /// </param>
        /// <param name="pageSize">
        /// The size of the page of results to return.
        /// </param>
        /// <param name="totalRecords">
        /// When this method returns, contains the total number of profiles.
        /// </param>
        /// <returns>
        /// A <see cref="T:System.Web.Profile.ProfileInfoCollection"/> containing user-profile information about the inactive profiles.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override ProfileInfoCollection GetAllInactiveProfiles(
            ProfileAuthenticationOption authenticationOption, 
            DateTime userInactiveSinceDate, 
            int pageIndex, 
            int pageSize, 
            out int totalRecords)
        {
            throw new NotSupportedException("This method is not supported for this provider.");
        }

        /// <summary>
        /// When overridden in a derived class, retrieves user profile data for all profiles in the data source.
        /// </summary>
        /// <param name="authenticationOption">
        /// One of the <see cref="T:System.Web.Profile.ProfileAuthenticationOption"/> values, specifying whether anonymous, authenticated, or both types of profiles are returned.
        /// </param>
        /// <param name="pageIndex">
        /// The index of the page of results to return.
        /// </param>
        /// <param name="pageSize">
        /// The size of the page of results to return.
        /// </param>
        /// <param name="totalRecords">
        /// When this method returns, contains the total number of profiles.
        /// </param>
        /// <returns>
        /// A <see cref="T:System.Web.Profile.ProfileInfoCollection"/> containing user-profile information for all profiles in the data source.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override ProfileInfoCollection GetAllProfiles(
            ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException("This method is not supported for this provider.");
        }

        /// <summary>
        /// When overridden in a derived class, returns the number of profiles in which the last activity date occurred on or before the specified date.
        /// </summary>
        /// <param name="authenticationOption">
        /// One of the <see cref="T:System.Web.Profile.ProfileAuthenticationOption"/> values, specifying whether anonymous, authenticated, or both types of profiles are returned.
        /// </param>
        /// <param name="userInactiveSinceDate">
        /// A <see cref="T:System.DateTime"/> that identifies which user profiles are considered inactive. If the <see cref="P:System.Web.Profile.ProfileInfo.LastActivityDate"/>  of a user profile occurs on or before this date and time, the profile is considered inactive.
        /// </param>
        /// <returns>
        /// The number of profiles in which the last activity date occurred on or before the specified date.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override int GetNumberOfInactiveProfiles(
            ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new NotSupportedException("This method is not supported for this provider.");
        }

        /// <summary>
        /// Returns the collection of settings property values for the specified application instance and settings property group.
        /// </summary>
        /// <param name="context">
        /// A <see cref="T:System.Configuration.SettingsContext"/> describing the current application use.
        /// </param>
        /// <param name="collection">
        /// A <see cref="T:System.Configuration.SettingsPropertyCollection"/> containing the settings property group whose values are to be retrieved.
        /// </param>
        /// <returns>
        /// A <see cref="T:System.Configuration.SettingsPropertyValueCollection"/> containing the values for the specified settings property group.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override SettingsPropertyValueCollection GetPropertyValues(
            SettingsContext context, SettingsPropertyCollection collection)
        {
            var svc = new SettingsPropertyValueCollection();

            if (collection == null || collection.Count < 1 || context == null)
            {
                return svc;
            }

            var username = (string)context["UserName"];
            var userIsAuthenticated = (bool)context["IsAuthenticated"];
            if (String.IsNullOrEmpty(username))
            {
                return svc;
            }

            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(this._sqlConnectionString);
                conn.Open();

                this.GetProfileDataFromSproc(collection, svc, username, conn, userIsAuthenticated);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return svc;
        }

        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="name">
        /// The friendly name of the provider.
        /// </param>
        /// <param name="config">
        /// A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The name of the provider is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// The name of the provider has a length of zero.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// An attempt is made to call <see cref="M:System.Configuration.Provider.ProviderBase.Initialize(System.String,System.Collections.Specialized.NameValueCollection)"/> on a provider after the provider has already been initialized.
        /// </exception>
        /// <remarks>
        /// </remarks>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (String.IsNullOrEmpty(name))
            {
                name = "StoredProcedureDBProfileProvider";
            }

            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "StoredProcedureDBProfileProvider");
            }

            base.Initialize(name, config);

            var temp = config["connectionStringName"];
            if (String.IsNullOrEmpty(temp))
            {
                throw new ProviderException("connectionStringName not specified");
            }

            this._sqlConnectionString = GetConnectionString(temp);
            if (String.IsNullOrEmpty(this._sqlConnectionString))
            {
                throw new ProviderException("connectionStringName not specified");
            }

            this._appName = config["applicationName"];
            if (string.IsNullOrEmpty(this._appName))
            {
                this._appName = GetDefaultAppName();
            }

            if (this._appName.Length > 256)
            {
                throw new ProviderException("Application name too long");
            }

            this._setSproc = config["setProcedure"];
            if (String.IsNullOrEmpty(this._setSproc))
            {
                throw new ProviderException("setProcedure not specified");
            }

            this._readSproc = config["readProcedure"];
            if (String.IsNullOrEmpty(this._readSproc))
            {
                throw new ProviderException("readProcedure not specified");
            }

            var timeout = config["commandTimeout"];
            if (string.IsNullOrEmpty(timeout) || !Int32.TryParse(timeout, out this._commandTimeout))
            {
                this._commandTimeout = 30;
            }

            config.Remove("commandTimeout");
            config.Remove("connectionStringName");
            config.Remove("applicationName");
            config.Remove("readProcedure");
            config.Remove("setProcedure");
            if (config.Count > 0)
            {
                var attribUnrecognized = config.GetKey(0);
                if (!String.IsNullOrEmpty(attribUnrecognized))
                {
                    throw new ProviderException("Unrecognized config attribute:" + attribUnrecognized);
                }
            }
        }

        /// <summary>
        /// Sets the values of the specified group of property settings.
        /// </summary>
        /// <param name="context">
        /// A <see cref="T:System.Configuration.SettingsContext"/> describing the current application usage.
        /// </param>
        /// <param name="collection">
        /// A <see cref="T:System.Configuration.SettingsPropertyValueCollection"/> representing the group of property settings to set.
        /// </param>
        /// <remarks>
        /// </remarks>
        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            var username = (string)context["UserName"];
            var userIsAuthenticated = (bool)context["IsAuthenticated"];

            if (username == null || username.Length < 1 || collection.Count < 1)
            {
                return;
            }

            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                var anyItemsToSave = false;

                // First make sure we have at least one item to save
                foreach (SettingsPropertyValue pp in collection)
                {
                    if (pp.IsDirty)
                    {
                        if (!userIsAuthenticated)
                        {
                            var allowAnonymous = (bool)pp.Property.Attributes["AllowAnonymous"];
                            if (!allowAnonymous)
                            {
                                continue;
                            }
                        }

                        anyItemsToSave = true;
                        break;
                    }
                }

                if (!anyItemsToSave)
                {
                    return;
                }

                conn = new SqlConnection(this._sqlConnectionString);
                conn.Open();

                var columnData = new List<ProfileColumnData>(collection.Count);

                foreach (SettingsPropertyValue pp in collection)
                {
                    if (!userIsAuthenticated)
                    {
                        var allowAnonymous = (bool)pp.Property.Attributes["AllowAnonymous"];
                        if (!allowAnonymous)
                        {
                            continue;
                        }
                    }

                    // Unlike the table provider, the sproc provider works against a fixed stored procedure
                    // signature, and must provide values for each stored procedure parameter
                    // if (!pp.IsDirty && pp.UsingDefaultValue) // Not fetched from DB and not written to
                    // continue;
                    var persistenceData = pp.Property.Attributes["CustomProviderData"] as string;

                    // If we can't find the table/column info we will ignore this data
                    if (String.IsNullOrEmpty(persistenceData))
                    {
                        // REVIEW: Perhaps we should throw instead?
                        continue;
                    }

                    var chunk = persistenceData.Split(new[] { ';' });
                    if (chunk.Length != 3)
                    {
                        // REVIEW: Perhaps we should throw instead?
                        continue;
                    }

                    var varname = chunk[0];

                    // REVIEW: Should we ignore case?
                    var datatype = (SqlDbType)Enum.Parse(typeof(SqlDbType), chunk[1], true);

                    // chunk[2] = size, which we ignore
                    object value = null;

                    if (!pp.IsDirty && pp.UsingDefaultValue)
                    {
                        // Not fetched from DB and not written to
                        value = DBNull.Value;
                    }
                    else if (pp.Deserialized && pp.PropertyValue == null)
                    {
                        // value was explicitly set to null
                        value = DBNull.Value;
                    }
                    else
                    {
                        value = pp.PropertyValue;
                    }

                    // REVIEW: Might be able to ditch datatype
                    columnData.Add(new ProfileColumnData(varname, pp, value, datatype));
                }

                cmd = this.CreateSprocSqlCommand(this._setSproc, conn, username, userIsAuthenticated);
                foreach (var data in columnData)
                {
                    cmd.Parameters.AddWithValue(data.VariableName, data.Value);
                    cmd.Parameters[data.VariableName].SqlDbType = data.DataType;
                }

                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }

                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="specifiedConnectionString">
        /// The specified connection string.
        /// </param>
        /// <returns>
        /// The get connection string.
        /// </returns>
        /// <remarks>
        /// </remarks>
        internal static string GetConnectionString(string specifiedConnectionString)
        {
            if (String.IsNullOrEmpty(specifiedConnectionString))
            {
                return null;
            }

            // Check <connectionStrings> config section for this connection string
            var connObj = ConfigurationManager.ConnectionStrings[specifiedConnectionString];
            if (connObj != null)
            {
                return connObj.ConnectionString;
            }

            return null;
        }

        /// <summary>
        /// Gets the default name of the app.
        /// </summary>
        /// <returns>
        /// The get default app name.
        /// </returns>
        /// <remarks>
        /// </remarks>
        internal static string GetDefaultAppName()
        {
            try
            {
                var appName = HostingEnvironment.ApplicationVirtualPath;
                if (String.IsNullOrEmpty(appName))
                {
                    appName = Process.GetCurrentProcess().MainModule.ModuleName;
                    var indexOfDot = appName.IndexOf('.');
                    if (indexOfDot != -1)
                    {
                        appName = appName.Remove(indexOfDot);
                    }
                }

                if (String.IsNullOrEmpty(appName))
                {
                    return "/";
                }
                else
                {
                    return appName;
                }
            }
            catch (SecurityException)
            {
                return "/";
            }
        }

        /// <summary>
        /// Creates the output param.
        /// </summary>
        /// <param name="paramName">
        /// Name of the param.
        /// </param>
        /// <param name="dbType">
        /// Type of the db.
        /// </param>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        private static SqlParameter CreateOutputParam(string paramName, SqlDbType dbType, int size)
        {
            var param = new SqlParameter(paramName, dbType);
            param.Direction = ParameterDirection.Output;
            param.Size = size;
            return param;
        }

        /// <summary>
        /// Creates the sproc SQL command.
        /// </summary>
        /// <param name="sproc">
        /// The sproc.
        /// </param>
        /// <param name="conn">
        /// The conn.
        /// </param>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="isAnonymous">
        /// if set to <c>true</c> [is anonymous].
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        private SqlCommand CreateSprocSqlCommand(string sproc, SqlConnection conn, string username, bool isAnonymous)
        {
            var cmd = new SqlCommand(sproc, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = this.CommandTimeout;
            cmd.Parameters.AddWithValue("@ApplicationName", this.ApplicationName);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@IsUserAnonymous", isAnonymous);
            return cmd;
        }

        /// <summary>
        /// Gets the profile data from sproc.
        /// </summary>
        /// <param name="properties">
        /// The properties.
        /// </param>
        /// <param name="svc">
        /// The SVC.
        /// </param>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <param name="conn">
        /// The conn.
        /// </param>
        /// <param name="userIsAuthenticated">
        /// if set to <c>true</c> [user is authenticated].
        /// </param>
        /// <remarks>
        /// </remarks>
        private void GetProfileDataFromSproc(
            SettingsPropertyCollection properties, 
            SettingsPropertyValueCollection svc, 
            string username, 
            SqlConnection conn, 
            bool userIsAuthenticated)
        {
            var cmd = this.CreateSprocSqlCommand(this._readSproc, conn, username, userIsAuthenticated);
            try
            {
                cmd.Parameters.RemoveAt("@IsUserAnonymous"); // anonymous flag not needed on get

                var columnData = new List<ProfileColumnData>(properties.Count);
                foreach (SettingsProperty prop in properties)
                {
                    var value = new SettingsPropertyValue(prop);
                    svc.Add(value);

                    var persistenceData = prop.Attributes["CustomProviderData"] as string;

                    // If we can't find the table/column info we will ignore this data
                    if (String.IsNullOrEmpty(persistenceData))
                    {
                        // REVIEW: Perhaps we should throw instead?
                        continue;
                    }

                    var chunk = persistenceData.Split(new[] { ';' });
                    if (chunk.Length != 3)
                    {
                        // REVIEW: Perhaps we should throw instead?
                        continue;
                    }

                    var varname = chunk[0];

                    // REVIEW: Should we ignore case?
                    var datatype = (SqlDbType)Enum.Parse(typeof(SqlDbType), chunk[1], true);

                    var size = 0;
                    if (!Int32.TryParse(chunk[2], out size))
                    {
                        throw new ArgumentException("Unable to parse as integer: " + chunk[2]);
                    }

                    columnData.Add(new ProfileColumnData(varname, value, null /* not needed for get */, datatype));
                    cmd.Parameters.Add(CreateOutputParam(varname, datatype, size));
                }

                cmd.ExecuteNonQuery();
                for (var i = 0; i < columnData.Count; ++i)
                {
                    var colData = columnData[i];
                    var val = cmd.Parameters[colData.VariableName].Value;
                    var propValue = colData.PropertyValue;

                    // Only initialize a SettingsPropertyValue for non-null values
                    if (!(val is DBNull || val == null))
                    {
                        propValue.PropertyValue = val;
                        propValue.IsDirty = false;
                        propValue.Deserialized = true;
                    }
                }
            }
            finally
            {
                cmd.Dispose();
            }
        }

        #endregion

        /// <summary>
        /// The profile column data.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private struct ProfileColumnData
        {
            #region Constants and Fields

            /// <summary>
            /// The data type.
            /// </summary>
            public readonly SqlDbType DataType;

            /// <summary>
            /// The property value.
            /// </summary>
            public readonly SettingsPropertyValue PropertyValue;

            /// <summary>
            /// The value.
            /// </summary>
            public readonly object Value;

            /// <summary>
            /// The variable name.
            /// </summary>
            public readonly string VariableName;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="ProfileColumnData"/> struct.
            /// </summary>
            /// <param name="var">
            /// The var.
            /// </param>
            /// <param name="pv">
            /// The pv.
            /// </param>
            /// <param name="val">
            /// The val.
            /// </param>
            /// <param name="type">
            /// The type.
            /// </param>
            /// <remarks>
            /// </remarks>
            public ProfileColumnData(string var, SettingsPropertyValue pv, object val, SqlDbType type)
            {
                this.VariableName = var;
                this.PropertyValue = pv;
                this.Value = val;
                this.DataType = type;
            }

            #endregion
        }
    }
}