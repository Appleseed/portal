// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppleseedSqlRoleProvider.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The appleseed sql role provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Providers.AppleseedRoleProvider
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Configuration.Provider;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Security;

    using Appleseed.Framework.Providers.AppleseedMembershipProvider;

    /// <summary>
    /// The appleseed sql role provider.
    /// </summary>
    public class AppleseedSqlRoleProvider : AppleseedRoleProvider
    {
        #region Constants and Fields

        /// <summary>
        /// The connection string.
        /// </summary>
        protected string connectionString;

        /// <summary>
        /// The p application name.
        /// </summary>
        protected string pApplicationName;

        /// <summary>
        /// The event log.
        /// </summary>
        private const string EventLog = "Application";

        /// <summary>
        /// The event source.
        /// </summary>
        private const string EventSource = "AppleseedSqlRoleProvider";

        // If false, exceptions are thrown to the caller. If true,
        // exceptions are written to the event log.

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets ApplicationName.
        /// </summary>
        public override string ApplicationName
        {
            get
            {
                return HttpContext.Current != null
                           ? (string)HttpContext.Current.Items["Role.ApplicationName"]
                           : this.pApplicationName;
            }

            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items["Role.ApplicationName"] = value;
                }

                this.pApplicationName = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether WriteExceptionsToEventLog.
        /// </summary>
        public bool WriteExceptionsToEventLog { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The add users to roles.
        /// </summary>
        /// <param name="usernames">
        /// The usernames.
        /// </param>
        /// <param name="roleNames">
        /// The role names.
        /// </param>
        /// <exception cref="AppleseedMembershipProviderException">
        /// </exception>
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            var userIds = new Guid[usernames.Length];
            var roleIds = new Guid[roleNames.Length];

            AppleseedUser user = null;
            for (var i = 0; i < usernames.Length; i++)
            {
                user = (AppleseedUser)Membership.GetUser(usernames[i]);

                if (user == null)
                {
                    throw new AppleseedMembershipProviderException("User " + usernames[i] + " doesn't exist");
                }

                userIds[i] = user.ProviderUserKey;
            }

            AppleseedRole role = null;
            for (var i = 0; i < roleNames.Length; i++)
            {
                role = this.GetRoleByName(this.ApplicationName, roleNames[i]);
                roleIds[i] = role.Id;
            }

            this.AddUsersToRoles(this.ApplicationName, userIds, roleIds);
        }

        /// <summary>
        /// The add users to roles.
        /// </summary>
        /// <param name="portalAlias">
        /// The portal alias.
        /// </param>
        /// <param name="userIds">
        /// The user ids.
        /// </param>
        /// <param name="roleIds">
        /// The role ids.
        /// </param>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedMembershipProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        public override void AddUsersToRoles(string portalAlias, Guid[] userIds, Guid[] roleIds)
        {
            var userIdsStr = string.Empty;
            var roleIdsStr = string.Empty;

            userIdsStr = userIds.Aggregate(userIdsStr, (current, userId) => current + (userId + ","));

            userIdsStr = userIdsStr.Substring(0, userIdsStr.Length - 1);

            roleIdsStr = roleIds.Aggregate(roleIdsStr, (current, roleId) => current + (roleId + ","));

            roleIdsStr = roleIdsStr.Substring(0, roleIdsStr.Length - 1);

            var cmd = new SqlCommand
                {
                    CommandText = "aspnet_UsersInRoles_AddUsersToRoles",
                    CommandType = CommandType.StoredProcedure,
                    Connection = new SqlConnection(this.connectionString)
                };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@UserIds", SqlDbType.VarChar, 4000).Value = userIdsStr;
            cmd.Parameters.Add("@RoleIds", SqlDbType.VarChar, 4000).Value = roleIdsStr;

            var returnCodeParam = cmd.Parameters.Add("@ReturnCode", SqlDbType.Int);
            returnCodeParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();

                var returnCode = (int)returnCodeParam.Value;

                switch (returnCode)
                {
                    case 0:
                        return;
                    case 2:
                        throw new AppleseedRoleProviderException("Application " + portalAlias + " doesn't exist");
                    case 3:
                        throw new AppleseedRoleProviderException("One of the roles doesn't exist");
                    case 4:
                        throw new AppleseedMembershipProviderException("One of the users doesn't exist");
                    default:
                        throw new AppleseedRoleProviderException(
                            "aspnet_UsersInRoles_AddUsersToRoles returned error code " + returnCode);
                }
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    this.WriteToEventLog(e, "CreateRole");
                }

                throw new AppleseedRoleProviderException(
                    "Error executing aspnet_UsersInRoles_AddUsersToRoles stored proc", e);
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// The create role.
        /// </summary>
        /// <param name="roleName">
        /// The role name.
        /// </param>
        public override void CreateRole(string roleName)
        {
            this.CreateRole(this.ApplicationName, roleName);
        }

        /// <summary>
        /// The create role.
        /// </summary>
        /// <param name="portalAlias">
        /// The portal alias.
        /// </param>
        /// <param name="roleName">
        /// The role name.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        public override Guid CreateRole(string portalAlias, string roleName)
        {
            if (roleName.IndexOf(',') != -1)
            {
                throw new AppleseedRoleProviderException("Role name can't contain commas");
            }

            var cmd = new SqlCommand
                {
                    CommandText = "aspnet_Roles_CreateRole",
                    CommandType = CommandType.StoredProcedure,
                    Connection = new SqlConnection(this.connectionString)
                };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 256).Value = roleName;

            var newRoleIdParam = cmd.Parameters.Add("@NewRoleId", SqlDbType.UniqueIdentifier);
            newRoleIdParam.Direction = ParameterDirection.Output;

            var returnCodeParam = cmd.Parameters.Add("@ReturnCode", SqlDbType.Int);
            returnCodeParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                cmd.Connection.Open();

                cmd.ExecuteNonQuery();

                var returnCode = (int)returnCodeParam.Value;

                if (returnCode != 0)
                {
                    throw new AppleseedRoleProviderException("Error creating role " + roleName);
                }

                return (Guid)newRoleIdParam.Value;
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    this.WriteToEventLog(e, "CreateRole");
                }

                throw new AppleseedRoleProviderException("Error executing aspnet_Roles_CreateRole stored proc", e);
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// The delete role.
        /// </summary>
        /// <param name="roleName">
        /// The role name.
        /// </param>
        /// <param name="throwOnPopulatedRole">
        /// The throw on populated role.
        /// </param>
        /// <returns>
        /// The delete role.
        /// </returns>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            var role = this.GetRoleByName(this.ApplicationName, roleName);

            return this.DeleteRole(this.ApplicationName, role.Id, throwOnPopulatedRole);
        }

        /// <summary>
        /// The delete role.
        /// </summary>
        /// <param name="portalAlias">
        /// The portal alias.
        /// </param>
        /// <param name="roleId">
        /// The role id.
        /// </param>
        /// <param name="throwOnPopulatedRole">
        /// The throw on populated role.
        /// </param>
        /// <returns>
        /// The delete role.
        /// </returns>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        public override bool DeleteRole(string portalAlias, Guid roleId, bool throwOnPopulatedRole)
        {
            var cmd = new SqlCommand
                {
                    CommandText = "aspnet_Roles_DeleteRole",
                    CommandType = CommandType.StoredProcedure,
                    Connection = new SqlConnection(this.connectionString)
                };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@RoleId", SqlDbType.UniqueIdentifier).Value = roleId;
            cmd.Parameters.Add("@DeleteOnlyIfRoleIsEmpty", SqlDbType.Bit).Value = throwOnPopulatedRole ? 1 : 0;

            var returnCodeParam = cmd.Parameters.Add("@ReturnCode", SqlDbType.Int);
            returnCodeParam.Direction = ParameterDirection.ReturnValue;

           // SqlDataReader reader = null;
            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();

                var returnCode = (int)returnCodeParam.Value;

                switch (returnCode)
                {
                    case 0:
                        return true;
                    case 1:
                        throw new AppleseedRoleProviderException("Application " + portalAlias + " doesn't exist");
                    case 2:
                        throw new AppleseedRoleProviderException("Role has members and throwOnPopulatedRole is true");
                    default:
                        throw new AppleseedRoleProviderException("Error deleting role");
                }
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    this.WriteToEventLog(e, "DeleteRole");
                }

                throw new AppleseedRoleProviderException("Error executing aspnet_Roles_DeleteRole stored proc", e);
            }
            catch (Exception e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    this.WriteToEventLog(e, "DeleteRole");
                }

                throw new AppleseedRoleProviderException("Error deleting role " + roleId, e);
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// The find users in role.
        /// </summary>
        /// <param name="roleName">
        /// The role name.
        /// </param>
        /// <param name="usernameToMatch">
        /// The username to match.
        /// </param>
        /// <returns>
        /// </returns>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return this.FindUsersInRole(this.ApplicationName, roleName, usernameToMatch);
        }

        /// <summary>
        /// The find users in role.
        /// </summary>
        /// <param name="portalAlias">
        /// The portal alias.
        /// </param>
        /// <param name="roleName">
        /// The role name.
        /// </param>
        /// <param name="usernameToMatch">
        /// The username to match.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public override string[] FindUsersInRole(string portalAlias, string roleName, string usernameToMatch)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// The get all roles.
        /// </summary>
        /// <returns>
        /// </returns>
        public override string[] GetAllRoles()
        {
            var roles = this.GetAllRoles(this.ApplicationName);

            var result = new string[roles.Count];

            for (var i = 0; i < roles.Count; i++)
            {
                result[i] = roles[i].Name;
            }

            return result;
        }

        /// <summary>
        /// The get all roles.
        /// </summary>
        /// <param name="portalAlias">
        /// The portal alias.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        public override IList<AppleseedRole> GetAllRoles(string portalAlias)
        {
            IList<AppleseedRole> result = new List<AppleseedRole>();
            result.Insert(0, new AppleseedRole(AllUsersGuid, AllUsersRoleName, AllUsersRoleName));
            result.Insert(
                1, new AppleseedRole(AuthenticatedUsersGuid, AuthenticatedUsersRoleName, AuthenticatedUsersRoleName));
            result.Insert(
                2, 
                new AppleseedRole(UnauthenticatedUsersGuid, UnauthenticatedUsersRoleName, UnauthenticatedUsersRoleName));

            var cmd = new SqlCommand
                {
                    CommandText = "aspnet_Roles_GetAllRoles",
                    CommandType = CommandType.StoredProcedure,
                    Connection = new SqlConnection(this.connectionString)
                };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;

            SqlDataReader reader = null;
            try
            {
                cmd.Connection.Open();

                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var role = this.GetRoleFromReader(reader);

                        result.Add(role);
                    }

                    reader.Close();
                }

                return result;
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    this.WriteToEventLog(e, "GetAllRoles");
                }

                throw new AppleseedRoleProviderException("Error executing aspnet_Roles_GetAllRoles stored proc", e);
            }
            catch (Exception e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    this.WriteToEventLog(e, "GetAllRoles");
                }

                throw new AppleseedRoleProviderException("Error getting all roles", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// The get role by id.
        /// </summary>
        /// <param name="roleId">
        /// The role id.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        public override AppleseedRole GetRoleById(Guid roleId)
        {
            var cmd = new SqlCommand
                {
                    CommandText = "SELECT RoleId, RoleName, Description FROM aspnet_Roles WHERE RoleId=@RoleId" 
                };
            cmd.Parameters.Add("@RoleId", SqlDbType.UniqueIdentifier).Value = roleId;

            cmd.Connection = new SqlConnection(this.connectionString);

            SqlDataReader reader = null;
            try
            {
                cmd.Connection.Open();

                using (reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return this.GetRoleFromReader(reader);
                    }
                    else
                    {
                        throw new AppleseedRoleProviderException("Role doesn't exist");
                    }
                }
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    this.WriteToEventLog(e, "GetRoleById");
                }

                throw new AppleseedRoleProviderException("Error executing method GetRoleById", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// The get role by name.
        /// </summary>
        /// <param name="portalAlias">
        /// The portal alias.
        /// </param>
        /// <param name="roleName">
        /// The role name.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        public override AppleseedRole GetRoleByName(string portalAlias, string roleName)
        {
            var cmd = new SqlCommand
                {
                    CommandText = "aspnet_Roles_GetRoleByName",
                    CommandType = CommandType.StoredProcedure,
                    Connection = new SqlConnection(this.connectionString)
                };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 256).Value = roleName;

            var returnCodeParam = cmd.Parameters.Add("@ReturnCode", SqlDbType.Int);
            returnCodeParam.Direction = ParameterDirection.ReturnValue;

            SqlDataReader reader = null;
            try
            {
                cmd.Connection.Open();

                using (reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return this.GetRoleFromReader(reader);
                    }
                    else
                    {
                        throw new AppleseedRoleProviderException("Role doesn't exist");
                    }
                }
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    this.WriteToEventLog(e, "CreateRole");
                }

                throw new AppleseedRoleProviderException(
                    "Error executing aspnet_UsersInRoles_IsUserInRole stored proc", e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// The get roles for user.
        /// </summary>
        /// <param name="username">
        /// The username.
        /// </param>
        /// <returns>
        /// </returns>
        public override string[] GetRolesForUser(string username)
        {
            var user = (AppleseedUser)Membership.GetUser(username);
            if (user != null)
            {
                var roles = this.GetRolesForUser(this.ApplicationName, user.ProviderUserKey);

                var result = new string[roles.Count];

                for (var i = 0; i < roles.Count; i++)
                {
                    result[i] = roles[i].Name;
                }

                return result;
            }

            return new string[0];
        }

        /// <summary>
        /// The get roles for user.
        /// </summary>
        /// <param name="portalAlias">
        /// The portal alias.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        public override IList<AppleseedRole> GetRolesForUser(string portalAlias, Guid userId)
        {
            IList<AppleseedRole> result = new List<AppleseedRole>();

            var cmd = new SqlCommand
                {
                    CommandText = "aspnet_UsersInRoles_GetRolesForUser",
                    CommandType = CommandType.StoredProcedure,
                    Connection = new SqlConnection(this.connectionString)
                };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = userId;

            var returnCode = cmd.Parameters.Add("@ReturnCode", SqlDbType.Int);
            returnCode.Direction = ParameterDirection.ReturnValue;

            SqlDataReader reader = null;
            try
            {
                cmd.Connection.Open();

                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var role = this.GetRoleFromReader(reader);

                        result.Add(role);
                    }

                    reader.Close();
                    if (((int)returnCode.Value) == 1)
                    {
                        throw new AppleseedRoleProviderException("User doesn't exist");
                    }
                }

                return result;
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    this.WriteToEventLog(e, "GetAllRoles");
                }

                throw new AppleseedRoleProviderException("Error executing aspnet_Roles_GetAllRoles stored proc", e);
            }
            catch (AppleseedRoleProviderException)
            {
                throw;
            }
            catch (Exception e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    this.WriteToEventLog(e, "GetAllRoles");
                }

                throw new AppleseedRoleProviderException("Error getting roles for user " + userId, e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// The get users in role.
        /// </summary>
        /// <param name="roleName">
        /// The role name.
        /// </param>
        /// <returns>
        /// </returns>
        public override string[] GetUsersInRole(string roleName)
        {
            var role = this.GetRoleByName(this.ApplicationName, roleName);
            return this.GetUsersInRole(this.ApplicationName, role.Id);
        }

        /// <summary>
        /// The get users in role.
        /// </summary>
        /// <param name="portalAlias">
        /// The portal alias.
        /// </param>
        /// <param name="roleId">
        /// The role id.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        public override string[] GetUsersInRole(string portalAlias, Guid roleId)
        {
            var result = new ArrayList();

            var cmd = new SqlCommand
                {
                    CommandText = "aspnet_UsersInRoles_GetUsersInRoles",
                    CommandType = CommandType.StoredProcedure,
                    Connection = new SqlConnection(this.connectionString)
                };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@RoleId", SqlDbType.UniqueIdentifier).Value = roleId;

            var returnCode = cmd.Parameters.Add("@ReturnCode", SqlDbType.Int);
            returnCode.Direction = ParameterDirection.ReturnValue;

            SqlDataReader reader = null;
            try
            {
                cmd.Connection.Open();

                using (reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetString(0));
                    }

                    reader.Close();
                    if (((int)returnCode.Value) == 1)
                    {
                        throw new AppleseedRoleProviderException("Role doesn't exist");
                    }
                }

                return (string[])result.ToArray(typeof(string));
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    this.WriteToEventLog(e, "GetAllRoles");
                }

                throw new AppleseedRoleProviderException(
                    "Error executing aspnet_UsersInRoles_GetUsersInRoles stored proc", e);
            }
            catch (Exception e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    this.WriteToEventLog(e, "GetAllRoles");
                }

                throw new AppleseedRoleProviderException("Error getting users for role " + roleId, e);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="config">
        /// The config.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        public override void Initialize(string name, NameValueCollection config)
        {
            // Initialize values from web.config.
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (name.Length == 0)
            {
                name = "AppleseedSqlRoleProvider";
            }

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Appleseed Sql Role provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);

            this.pApplicationName = this.GetConfigValue(
                config["applicationName"], HostingEnvironment.ApplicationVirtualPath);
            this.WriteExceptionsToEventLog =
                Convert.ToBoolean(this.GetConfigValue(config["writeExceptionsToEventLog"], "true"));

            // Initialize SqlConnection.
            var connectionStringSettings = ConfigurationManager.ConnectionStrings[config["connectionStringName"]];

            if (connectionStringSettings == null ||
                connectionStringSettings.ConnectionString.Trim().Equals(string.Empty))
            {
                throw new AppleseedRoleProviderException("Connection string cannot be blank.");
            }

            this.connectionString = connectionStringSettings.ConnectionString;
        }

        /// <summary>
        /// Gets a value indicating whether the specified user is in the specified role for the configured applicationName.
        /// </summary>
        /// <param name="username">The user name to search for.</param>
        /// <param name="roleName">The role to search in.</param>
        /// <returns>
        /// true if the specified user is in the specified role for the configured applicationName; otherwise, false.
        /// </returns>
        public override bool IsUserInRole(string username, string roleName)
        {
            var user = (AppleseedUser)Membership.GetUser(username);

            if (user == null)
            {
                throw new AppleseedRoleProviderException("User doesn't exist");
            }

            var role = this.GetRoleByName(this.ApplicationName, roleName);

            return this.IsUserInRole(this.ApplicationName, user.ProviderUserKey, role.Id);
        }

        /// <summary>
        /// Takes, as input, a user id and a role id and determines whether the specified user is associated with the specified role.
        /// </summary>
        /// <param name="portalAlias">Appleseed's portal alias</param>
        /// <param name="userId">User id</param>
        /// <param name="roleId">Role id</param>
        /// <returns>
        ///   <code>
        /// true
        /// </code>
        /// if the specified user is associated with the specified role,
        /// <code>
        /// false
        /// </code>
        /// otherwise
        /// </returns>
        /// <exception cref="ProviderException">
        /// If the user or role does not exist, IsUserInRole throws a ProviderException.
        /// </exception>
        public override bool IsUserInRole(string portalAlias, Guid userId, Guid roleId)
        {
            if (roleId.Equals(AllUsersGuid) || roleId.Equals(AuthenticatedUsersGuid))
            {
                return true;
            }

            if (roleId.Equals(UnauthenticatedUsersGuid))
            {
                return false;
            }

            var cmd = new SqlCommand
                {
                    CommandText = "aspnet_UsersInRoles_IsUserInRole",
                    CommandType = CommandType.StoredProcedure,
                    Connection = new SqlConnection(this.connectionString)
                };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@RoleId", SqlDbType.UniqueIdentifier).Value = roleId;
            cmd.Parameters.Add("@userId", SqlDbType.UniqueIdentifier).Value = userId;

            var returnCodeParam = cmd.Parameters.Add("@ReturnCode", SqlDbType.Int);
            returnCodeParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                cmd.Connection.Open();

                cmd.ExecuteNonQuery();

                var returnCode = (int)returnCodeParam.Value;

                switch (returnCode)
                {
                    case 0:
                        return false;
                    case 1:
                        return true;
                    case 2:
                        throw new AppleseedRoleProviderException("User with Id = " + userId + " does not exist");
                    case 3:
                        throw new AppleseedRoleProviderException("Role with Id = " + roleId + " does not exist");
                    default:
                        throw new AppleseedRoleProviderException("Error executing IsUserInRole");
                }
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    this.WriteToEventLog(e, "IsUserInRole");
                }

                throw new AppleseedRoleProviderException(
                    "Error executing aspnet_UsersInRoles_IsUserInRole stored proc", e);
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// The remove users from roles.
        /// </summary>
        /// <param name="usernames">
        /// The usernames.
        /// </param>
        /// <param name="roleNames">
        /// The role names.
        /// </param>
        /// <exception cref="AppleseedMembershipProviderException">
        /// </exception>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            var userIds = new Guid[usernames.Length];
            var roleIds = new Guid[roleNames.Length];

            AppleseedUser user = null;
            for (var i = 0; i < usernames.Length; i++)
            {
                user = (AppleseedUser)Membership.GetUser(usernames[i]);

                if (user == null)
                {
                    throw new AppleseedMembershipProviderException(string.Format("User {0} doesn't exist", usernames[i]));
                }

                userIds[i] = user.ProviderUserKey;
            }

            for (var i = 0; i < roleNames.Length; i++)
            {
                var role = this.GetRoleByName(this.ApplicationName, roleNames[i]);
                roleIds[i] = role.Id;
            }

            this.RemoveUsersFromRoles(this.ApplicationName, userIds, roleIds);
        }

        /// <summary>
        /// The remove users from roles.
        /// </summary>
        /// <param name="portalAlias">
        /// The portal alias.
        /// </param>
        /// <param name="userIds">
        /// The user ids.
        /// </param>
        /// <param name="roleIds">
        /// The role ids.
        /// </param>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedMembershipProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        public override void RemoveUsersFromRoles(string portalAlias, Guid[] userIds, Guid[] roleIds)
        {
            var userIdsStr = string.Empty;
            var roleIdsStr = string.Empty;

            userIdsStr = userIds.Aggregate(userIdsStr, (current, userId) => current + (userId + ","));

            userIdsStr = userIdsStr.Substring(0, userIdsStr.Length - 1);

            roleIdsStr = roleIds.Aggregate(roleIdsStr, (current, roleId) => current + (roleId + ","));

            roleIdsStr = roleIdsStr.Substring(0, roleIdsStr.Length - 1);

            var cmd = new SqlCommand
                {
                    CommandText = "aspnet_UsersInRoles_RemoveUsersFromRoles",
                    CommandType = CommandType.StoredProcedure,
                    Connection = new SqlConnection(this.connectionString)
                };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@UserIds", SqlDbType.VarChar, 4000).Value = userIdsStr;
            cmd.Parameters.Add("@RoleIds", SqlDbType.VarChar, 4000).Value = roleIdsStr;

            var returnCodeParam = cmd.Parameters.Add("@ReturnCode", SqlDbType.Int);
            returnCodeParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();

                var returnCode = (int)returnCodeParam.Value;

                switch (returnCode)
                {
                    case 0:
                        return;
                    case 1:
                        throw new AppleseedRoleProviderException(
                            "One of the users is not in one of the specified roles");
                    case 2:
                        throw new AppleseedRoleProviderException("Application " + portalAlias + " doesn't exist");
                    case 3:
                        throw new AppleseedRoleProviderException("One of the roles doesn't exist");
                    case 4:
                        throw new AppleseedMembershipProviderException("One of the users doesn't exist");
                    default:
                        throw new AppleseedRoleProviderException(
                            "aspnet_UsersInRoles_RemoveUsersToRoles returned error code " + returnCode);
                }
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    this.WriteToEventLog(e, "CreateRole");
                }

                throw new AppleseedRoleProviderException(
                    "Error executing aspnet_UsersInRoles_RemoveUsersToRoles stored proc", e);
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// The rename role.
        /// </summary>
        /// <param name="portalAlias">
        /// The portal alias.
        /// </param>
        /// <param name="roleId">
        /// The role id.
        /// </param>
        /// <param name="newRoleName">
        /// The new role name.
        /// </param>
        /// <returns>
        /// The rename role.
        /// </returns>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        public override bool RenameRole(string portalAlias, Guid roleId, string newRoleName)
        {
            var cmd = new SqlCommand
                {
                    CommandText = "aspnet_rbRoles_RenameRole",
                    CommandType = CommandType.StoredProcedure,
                    Connection = new SqlConnection(this.connectionString)
                };

            cmd.Parameters.Add("@RoleId", SqlDbType.UniqueIdentifier).Value = roleId;
            cmd.Parameters.Add("@NewRoleName", SqlDbType.VarChar, 256).Value = newRoleName;

            var returnCodeParam = cmd.Parameters.Add("@ReturnCode", SqlDbType.Int);
            returnCodeParam.Direction = ParameterDirection.ReturnValue;

          //  SqlDataReader reader = null;
            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();

                var returnCode = (int)returnCodeParam.Value;

                switch (returnCode)
                {
                    case 0:
                        return true;
                    case 1:
                        throw new AppleseedRoleProviderException("Role doesn't exist");
                    default:
                        throw new AppleseedRoleProviderException("Error renaming role");
                }
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    this.WriteToEventLog(e, "DeleteRole");
                }

                throw new AppleseedRoleProviderException("Error executing aspnet_rbRoles_RenameRole stored proc", e);
            }
            catch (Exception e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    this.WriteToEventLog(e, "RenameRole");
                }

                throw new AppleseedRoleProviderException("Error renaming role " + roleId, e);
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// The role exists.
        /// </summary>
        /// <param name="roleName">
        /// The role name.
        /// </param>
        /// <returns>
        /// The role exists.
        /// </returns>
        public override bool RoleExists(string roleName)
        {
            try
            {
                var allRoles = this.GetAllRoles(this.ApplicationName);
                return allRoles.Any(role => role.Name.Equals(roleName));
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// The role exists.
        /// </summary>
        /// <param name="portalAlias">
        /// The portal alias.
        /// </param>
        /// <param name="roleId">
        /// The role id.
        /// </param>
        /// <returns>
        /// The role exists.
        /// </returns>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        public override bool RoleExists(string portalAlias, Guid roleId)
        {
            var cmd = new SqlCommand
                {
                    CommandText = "aspnet_Roles_RoleExists",
                    CommandType = CommandType.StoredProcedure,
                    Connection = new SqlConnection(this.connectionString)
                };

            cmd.Parameters.Add("@ApplicationName", SqlDbType.NVarChar, 256).Value = portalAlias;
            cmd.Parameters.Add("@RoleId", SqlDbType.UniqueIdentifier).Value = roleId;

            var returnCodeParam = cmd.Parameters.Add("@ReturnCode", SqlDbType.Int);
            returnCodeParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                cmd.Connection.Open();

                cmd.ExecuteNonQuery();

                var returnCode = (int)returnCodeParam.Value;
                return returnCode == 1;
            }
            catch (SqlException e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    this.WriteToEventLog(e, "RoleExists");
                }

                throw new AppleseedRoleProviderException("Error executing aspnet_Roles_RoleExists stored proc", e);
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// A helper function to retrieve config values from the configuration file.
        /// </summary>
        /// <param name="configValue">
        /// </param>
        /// <param name="defaultValue">
        /// </param>
        /// <returns>
        /// The get config value.
        /// </returns>
        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
            {
                return defaultValue;
            }

            return configValue;
        }

        /// <summary>
        /// The get role from reader.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <returns>
        /// </returns>
        private AppleseedRole GetRoleFromReader(SqlDataReader reader)
        {
            var roleId = reader.GetGuid(0);
            var roleName = reader.GetString(1);

            var roleDescription = string.Empty;
            if (!reader.IsDBNull(2))
            {
                roleDescription = reader.GetString(2);
            }

            return new AppleseedRole(roleId, roleName, roleDescription);
        }

        /// <summary>
        /// A helper function that writes exception detail to the event log. Exceptions are written to the event log as a security
        /// measure to avoid private database details from being returned to the browser. If a method does not return a status
        /// or Boolean indicating the action succeeded or failed, a generic exception is also thrown by the caller.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="action">The action.</param>
        private void WriteToEventLog(Exception e, string action)
        {
            var log = new EventLog { Source = EventSource, Log = EventLog };

            var message = "An exception occurred communicating with the data source.\n\n";
            message += string.Format("Action: {0}\n\n", action);
            message += string.Format("Exception: {0}", e);
            ErrorHandler.Publish(LogLevel.Error, message, e);

            // log.WriteEntry( message );
        }

        #endregion
    }
}