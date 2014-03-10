// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppleseedRoleProvider.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The Appleseed provider for roles.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Providers.AppleseedRoleProvider
{
    using System;
    using System.Collections.Generic;
    using System.Configuration.Provider;
    using System.Web.Security;

    /// <summary>
    /// The Appleseed provider for roles.
    /// </summary>
    public abstract class AppleseedRoleProvider : RoleProvider
    {
        #region Constants and Fields

        /// <summary>
        /// The all users role name.
        /// </summary>
        public const string AllUsersRoleName = "All Users";

        /// <summary>
        /// The authenticated users role name.
        /// </summary>
        public const string AuthenticatedUsersRoleName = "Authenticated Users";

        /// <summary>
        /// The unauthenticated users role name.
        /// </summary>
        public const string UnauthenticatedUsersRoleName = "Unauthenticated Users";

        /// <summary>
        /// The all users guid.
        /// </summary>
        public static Guid AllUsersGuid = new Guid("{8E9E3841-A27A-49da-BC02-2048F1F1FD54}");

        /// <summary>
        /// The authenticated users guid.
        /// </summary>
        public static Guid AuthenticatedUsersGuid = new Guid("{40D335D3-8C46-4009-B456-53F254959042}");

        /// <summary>
        /// The unauthenticated users guid.
        /// </summary>
        public static Guid UnauthenticatedUsersGuid = new Guid("{6E21FB8C-F345-4071-870B-151E79008B44}");

        #endregion

        #region Public Methods

        /// <summary>
        /// Takes, as input, a list of user names and a list of role ids and adds the specified users to the specified roles.
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="userIds">
        /// A list of user ids
        /// </param>
        /// <param name="roleIds">
        /// A list of role ids
        /// </param>
        /// <exception cref="ProviderException">
        /// AddUsersToRoles throws a ProviderException if any of the user names or role names do not exist.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// If any user name or role name is null (Nothing in Visual Basic), AddUsersToRoles throws an ArgumentNullException.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If any user name or role name is an empty string, AddUsersToRoles throws an ArgumentException.
        /// </exception>
        public abstract void AddUsersToRoles(string portalAlias, Guid[] userIds, Guid[] roleIds);

        /// <summary>
        /// Takes, as input, a role name and creates the specified role.
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="roleName">
        /// A role name
        /// </param>
        /// <returns>
        /// The new role ID
        /// </returns>
        /// <exception cref="ProviderException">
        /// CreateRole throws a ProviderException if the role already exists, the role 
        ///   name contains a comma, or the role name exceeds the maximum length allowed by the data source.
        /// </exception>
        public abstract Guid CreateRole(string portalAlias, string roleName);

        /// <summary>
        /// Takes, as input, a role id and a Boolean value that indicates whether to throw an exception if there are 
        ///   users currently associated with the role, and then deletes the specified role. 
        ///   If throwOnPopulatedRole is false, DeleteRole deletes the role whether it is empty or not. 
        ///   When DeleteRole deletes a role and there are users assigned to that role, it also removes users from the role.
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="roleId">
        /// The role id
        /// </param>
        /// <param name="throwOnPopulatedRole">
        /// indicates whether to throw an exception if there are users currently associated with 
        ///   the role, and then deletes the specified role.
        /// </param>
        /// <exception cref="ProviderException">
        /// If the throwOnPopulatedRole input parameter is true and the specified role has one 
        ///   or more members, DeleteRole throws a ProviderException and does not delete the role.
        /// </exception>
        /// <returns>
        /// The delete role.
        /// </returns>
        public abstract bool DeleteRole(string portalAlias, Guid roleId, bool throwOnPopulatedRole);

        /// <summary>
        /// Takes, as input, a search pattern and a role name and returns a list of users belonging to the specified role 
        ///   whose user names match the pattern. Wildcard syntax is data-source-dependent and may vary from provider to provider. 
        ///   User names are returned in alphabetical order.
        ///   If the search finds no matches, FindUsersInRole returns an empty string array (a string array with no elements).
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="roleName">
        /// </param>
        /// <param name="usernameToMatch">
        /// </param>
        /// <exception cref="ProviderException">
        /// If the role does not exist, FindUsersInRole throws a ProviderException.
        /// </exception>
        /// <returns>
        /// A list of user names
        /// </returns>
        public abstract string[] FindUsersInRole(string portalAlias, string roleName, string usernameToMatch);

        /// <summary>
        /// Returns the names of all existing roles. If no roles exist, GetAllRoles returns an empty string array 
        ///   (a string array with no elements).
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <returns>
        /// A list of 
        /// <code>
        /// AppleseedRole
        /// </code>
        /// objects
        /// </returns>
        public abstract IList<AppleseedRole> GetAllRoles(string portalAlias);

        /// <summary>
        /// Retrieves a 
        /// <code>
        /// AppleseedRole
        /// </code>
        /// given a role id
        /// </summary>
        /// <param name="roleId">
        /// A role id
        /// </param>
        /// <returns>
        /// A 
        /// <code>
        /// AppleseedRole
        /// </code>
        /// </returns>
        /// <exception cref="ProviderException">
        /// GetRole throws a ProviderException if the role doesn't exist
        /// </exception>
        public abstract AppleseedRole GetRoleById(Guid roleId);

        /// <summary>
        /// Retrieves a 
        /// <code>
        /// AppleseedRole
        /// </code>
        /// given a role name
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="roleName">
        /// The role name
        /// </param>
        /// <returns>
        /// A 
        /// <code>
        /// AppleseedRole
        /// </code>
        /// </returns>
        /// <exception cref="ProviderException">
        /// GetRole throws a ProviderException if the role doesn't exist
        /// </exception>
        public abstract AppleseedRole GetRoleByName(string portalAlias, string roleName);

        /// <summary>
        /// Takes, as input, a user name and returns the names of the roles to which the user belongs.
        ///   If the user is not assigned to any roles, GetRolesForUser returns an empty string array 
        ///   (a string array with no elements).
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="userId">
        /// A user id
        /// </param>
        /// <exception cref="ProviderException">
        /// If the user name does not exist, GetRolesForUser throws a ProviderException.
        /// </exception>
        /// <returns>
        /// A list of role names.
        /// </returns>
        public abstract IList<AppleseedRole> GetRolesForUser(string portalAlias, Guid userId);

        /// <summary>
        /// Takes, as input, a role id and returns the ids of all users assigned to that role.
        ///   If no users are associated with the specified role, GetUserInRole returns an empty string
        ///   array (a string array with no elements).
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="roleId">
        /// A role id
        /// </param>
        /// <exception cref="ProviderException">
        /// If the role does not exist, GetUsersInRole throws a ProviderException.
        /// </exception>
        /// <returns>
        /// A list of user names.
        /// </returns>
        public abstract string[] GetUsersInRole(string portalAlias, Guid roleId);

        /// <summary>
        /// Takes, as input, a user id and a role id and determines whether the specified user is associated with the specified role.
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="userId">
        /// User id
        /// </param>
        /// <param name="roleId">
        /// Role id
        /// </param>
        /// <exception cref="ProviderException">
        /// If the user or role does not exist, IsUserInRole throws a ProviderException.
        /// </exception>
        /// <returns>
        /// <code>
        /// true
        /// </code>
        /// if the specified user is associated with the specified role, 
        /// <code>
        /// false
        /// </code>
        /// otherwise
        /// </returns>
        public abstract bool IsUserInRole(string portalAlias, Guid userId, Guid roleId);

        /// <summary>
        /// Takes, as input, a list of user ids and a list of role ids and removes the specified users from the specified roles.
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="userIds">
        /// A collection of user ids
        /// </param>
        /// <param name="roleIds">
        /// A collection of role ids
        /// </param>
        /// <exception cref="ProviderException">
        /// RemoveUsersFromRoles throws a ProviderException if any of the users or roles 
        ///   do not exist, or if any user specified in the call does not belong to the role from which he or she is being removed.
        /// </exception>
        public abstract void RemoveUsersFromRoles(string portalAlias, Guid[] userIds, Guid[] roleIds);

        /// <summary>
        /// Takes as an input a role Id and a role name and updates the role's name identified by roleId.
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="roleId">
        /// The role id
        /// </param>
        /// <param name="newRoleName">
        /// The new role name
        /// </param>
        /// <returns>
        /// <code>
        /// true
        /// </code>
        /// if the update was successful, or 
        /// <code>
        /// false
        /// </code>
        /// if the role was not found
        /// </returns>
        /// <exception cref="AppleseedRoleProviderException">
        /// RenameRole throws an exception if there was an unexpected error 
        ///   renaming the role.
        /// </exception>
        public abstract bool RenameRole(string portalAlias, Guid roleId, string newRoleName);

        /// <summary>
        /// Takes, as input, a role name and determines whether the role exists.
        /// </summary>
        /// <param name="portalAlias">
        /// Appleseed's portal alias
        /// </param>
        /// <param name="roleId">
        /// A role id
        /// </param>
        /// <returns>
        /// Whether the specified role exists or not
        /// </returns>
        public abstract bool RoleExists(string portalAlias, Guid roleId);

        #endregion
    }
}