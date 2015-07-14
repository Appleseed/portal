namespace Appleseed.Framework.Users.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Security;

    using Appleseed.Framework.Providers.AppleseedMembershipProvider;
    using Appleseed.Framework.Providers.AppleseedRoleProvider;

    /// <summary>
    /// The UsersDB class encapsulates all data logic necessary to add/login/query
    ///    users within the Portal Users database.
    ///    <remarks>
    /// Important Note: The UsersDB class is only used when forms-based cookie
    ///        authentication is enabled within the portal.  When windows based
    ///        authentication is used instead, then either the Windows SAM or Active Directory
    ///        is used to store and validate all username/password credentials.
    ///    </remarks>
    /// </summary>
    [History("gschnyder", "2008/05/29", "Create role by portal alias")]
    [History("jminond", "2005/03/10", "Tab to page conversion")]
    [History("gman3001", "2004/09/29",
        "Added the UpdateLastVisit method to update the user's last visit date indicator.")]
    public class UsersDB
    {
        #region Properties

        /*
        /// <summary>
        /// Gets CurrentPortalSettings.
        /// </summary>
        private PortalSettings CurrentPortalSettings
        {
            get
            {
                return (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            }
        }
*/

        /// <summary>
        /// Gets MembershipProvider.
        /// </summary>
        /// <exception cref="AppleseedMembershipProviderException">
        /// </exception>
        private static AppleseedMembershipProvider MembershipProvider
        {
            get
            {
                if (!(Membership.Provider is AppleseedMembershipProvider))
                {
                    throw new AppleseedMembershipProviderException(
                        "The membership provider must be a AppleseedMembershipProvider implementation");
                }

                return Membership.Provider as AppleseedMembershipProvider;
            }
        }

        /// <summary>
        /// Gets RoleProvider.
        /// </summary>
        /// <exception cref="AppleseedRoleProviderException">
        /// </exception>
        private static AppleseedRoleProvider RoleProvider
        {
            get
            {
                if (!(Roles.Provider is AppleseedRoleProvider))
                {
                    throw new AppleseedRoleProviderException(
                        "The role provider must be a AppleseedRoleProvider implementation");
                }

                return Roles.Provider as AppleseedRoleProvider;
            }
        }

        #endregion

        // private CryptoHelper cryphelp = null;
        #region Public Methods

        /// <summary>
        /// AddRole() Method <a name="AddRole"></a>
        /// The AddRole method creates a new security role for the specified portal,
        /// and returns the new RoleID value.
        /// Other relevant sources:
        /// + <a href="AddRole.htm" style="color:green">AddRole Stored Procedure</a>
        /// </summary>
        /// <param name="portalAlias">Portal alias.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <returns>The Guid of the role.</returns>
        public Guid AddRole(string portalAlias, string roleName)
        {
            return RoleProvider.CreateRole(
                /*CurrentPortalSettings.PortalAlias*/
                portalAlias,
                roleName);
        }

        /// <summary>
        /// Add a User
        /// </summary>
        /// <param name="name">
        /// The user name.
        /// </param>
        /// <param name="company">
        /// The company.
        /// </param>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <param name="city">
        /// The user's city.
        /// </param>
        /// <param name="zip">
        /// The zip code.
        /// </param>
        /// <param name="countryId">
        /// The country ID.
        /// </param>
        /// <param name="stateId">
        /// The state ID.
        /// </param>
        /// <param name="phone">
        /// The phone number.
        /// </param>
        /// <param name="fax">
        /// The fax number.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="email">
        /// The email address.
        /// </param>
        /// <param name="sendNewsletter">
        /// if set to <c>true</c> [send newsletter].
        /// </param>
        /// <param name="portalAlias">
        /// The portal Alias.
        /// </param>
        /// <returns>
        /// The newly created ID
        /// </returns>
        public Guid AddUser(
            string name,
            string company,
            string address,
            string city,
            string zip,
            string countryId,
            int stateId,
            string phone,
            string fax,
            string password,
            string email,
            bool sendNewsletter,
            string portalAlias)
        {
            MembershipCreateStatus status;
            var user = MembershipProvider.CreateUser(
                /*CurrentPortalSettings.PortalAlias*/
                portalAlias, name, password, email, "vacia", "llena", true, out status);

            if (user == null)
            {
                throw new ApplicationException(MembershipProvider.GetErrorMessage(status));
            }

            return (Guid)(user.ProviderUserKey ?? Guid.Empty);
        }

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="fullName">
        /// The full name.
        /// </param>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="portalAlias">
        /// The portal Alias.
        /// </param>
        /// (6)
        /// <returns>
        /// The Guid of the user id.
        /// </returns>
        public Guid AddUser(string fullName, string email, string password, string portalAlias)
        {
            var newUserId = this.AddUser(
                email,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                0,
                string.Empty,
                string.Empty,
                password,
                email,
                false,
                portalAlias);
            var user = MembershipProvider.GetUser(newUserId, false) as AppleseedUser;
            if (user != null)
            {
                user.Name = fullName;
                MembershipProvider.UpdateUser(portalAlias, user);
            }

            return newUserId;
        }

        /// <summary>
        /// AddUserRole() Method <a name="AddUserRole"></a>
        ///     The AddUserRole method adds the user to the specified security role.
        /// </summary>
        /// <param name="roleId">
        /// The role ID.
        /// </param>
        /// <param name="userId">
        /// The user ID.
        /// </param>
        /// <param name="portalAlias">
        /// The portal Alias.
        /// </param>
        public void AddUserRole(Guid roleId, Guid userId, string portalAlias)
        {
            RoleProvider.AddUsersToRoles(
                /*CurrentPortalSettings.PortalAlias*/ portalAlias, new[] { userId }, new[] { roleId });
        }

        /// <summary>
        /// DeleteRole() Method <a name="DeleteRole"></a>
        ///     The DeleteRole deletes the specified role from the portal database.
        ///     Other relevant sources:
        ///     + <a href="DeleteRole.htm" style="color:green">DeleteRole Stored Procedure</a>
        /// </summary>
        /// <param name="roleId">
        /// The role id.
        /// </param>
        /// <param name="portalAlias">
        /// The portal Alias.
        /// </param>
        public void DeleteRole(Guid roleId, string portalAlias)
        {
            RoleProvider.DeleteRole(
                /*CurrentPortalSettings.PortalAlias*/
                portalAlias,
                roleId,
                false);
        }

        /// <summary>
        /// The DeleteUser method deleted a  user record from the "Users" database table.
        /// </summary>
        /// <param name="userId">
        /// The user ID.
        /// </param>
        public void DeleteUser(Guid userId)
        {
            var user = Membership.GetUser(userId);
            if (user != null)
            {
                Membership.DeleteUser(user.UserName);
            }
        }

        /// <summary>
        /// DeleteUserRole() Method <a name="DeleteUserRole"></a>
        ///     The DeleteUserRole method deletes the user from the specified role.
        ///     Other relevant sources:
        ///     + <a href="DeleteUserRole.htm" style="color:green">DeleteUserRole Stored Procedure</a>
        /// </summary>
        /// <param name="roleId">
        /// The role ID.
        /// </param>
        /// <param name="userId">
        /// The user ID.
        /// </param>
        /// <param name="portalAlias">
        /// The portal Alias.
        /// </param>
        public void DeleteUserRole(Guid roleId, Guid userId, string portalAlias)
        {
            RoleProvider.RemoveUsersFromRoles(
                portalAlias /*CurrentPortalSettings.PortalAlias */, new[] { userId }, new[] { roleId });
        }

        /// <summary>
        /// Gets the portal roles.
        /// </summary>
        /// <param name="portalAlias">The portal alias.</param>
        /// <returns>A list of roles.</returns>
        public IList<AppleseedRole> GetPortalRoles(string portalAlias)
        {
            return RoleProvider.GetAllRoles(portalAlias);
        }

        /// <summary>
        /// The GetRoleMembers method returns a list of all members in the specified security role.
        /// </summary>
        /// <param name="roleId">
        /// The role id.
        /// </param>
        /// <param name="portalAlias">
        /// The portal Alias.
        /// </param>
        /// <returns>
        /// a 
        /// <code>
        /// string[]
        /// </code>
        /// of user names
        /// </returns>
        public string[] GetRoleMembers(Guid roleId, string portalAlias)
        {
            return RoleProvider.GetUsersInRole(portalAlias /*CurrentPortalSettings.PortalAlias*/, roleId);
        }

        /// <summary>
        /// The GetRoleNonMembers method returns a list of roles that doesn´t have any members.
        /// </summary>
        /// <param name="roleId">The role id.</param>
        /// <param name="portalAlias">The portal alias</param>
        /// <returns>A list of roles.</returns>
        public IList<AppleseedRole> GetRoleNonMembers(Guid roleId, string portalAlias)
        {
            var allRoles = RoleProvider.GetAllRoles(portalAlias);

            return allRoles.Where(s => RoleProvider.GetUsersInRole(portalAlias, s.Id).Length == 0).ToList();
        }

        /// <summary>
        /// The GetRoles method returns a list of roles for the user.
        /// </summary>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <param name="portalAlias">
        /// The portal alias.
        /// </param>
        /// <returns>
        /// A 
        /// <code>
        /// IList&lt;AppleseedRole&gt;
        /// </code>
        /// containing the user's roles
        /// </returns>
        public IList<AppleseedRole> GetRoles(string email, string portalAlias)
        {
            var userName = MembershipProvider.GetUserNameByEmail(portalAlias, email);
            var user = (AppleseedUser)MembershipProvider.GetUser(portalAlias, userName, false);
            return RoleProvider.GetRolesForUser(portalAlias, user.ProviderUserKey);
        }

        /// <summary>
        /// Return the role list the user's in
        /// </summary>
        /// <param name="userId">
        /// The User Id
        /// </param>
        /// <param name="portalAlias">
        /// The portal alias
        /// </param>
        /// <returns>
        /// A list of roles.
        /// </returns>
        public IList<AppleseedRole> GetRolesByUser(Guid userId, string portalAlias)
        {
            var user = (AppleseedUser)MembershipProvider.GetUser(userId, false);
            return user != null ? RoleProvider.GetRolesForUser(portalAlias, user.ProviderUserKey) : new List<AppleseedRole>();
        }

        /// <summary>
        /// Retrieves a 
        /// <code>
        /// MembershipUser
        /// </code>
        /// .
        /// </summary>
        /// <param name="userName">
        /// the user's email
        /// </param>
        /// <param name="portalAlias">
        /// The portal Alias.
        /// </param>
        /// <returns>
        /// A single user.
        /// </returns>
        public AppleseedUser GetSingleUser(string userName, string portalAlias)
        {
            var user =
                MembershipProvider.GetUser(portalAlias /*CurrentPortalSettings.PortalAlias*/, userName, true) as
                AppleseedUser;
            return user;
        }

        /// <summary>
        /// The GetUser method returns the collection of users.
        /// </summary>
        /// <param name="portalAlias">
        /// The portal Alias.
        /// </param>
        /// <returns>
        /// A collection of users.
        /// </returns>
        public MembershipUserCollection GetUsers(string portalAlias)
        {
            int totalRecords;
            return MembershipProvider.GetAllUsers(
                /*CurrentPortalSettings.PortalAlias*/ portalAlias, 0, int.MaxValue, out totalRecords);
        }

        /// <summary>
        /// The GetUsersCount method returns the users count.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="portalAlias">
        /// The portal Alias.
        /// </param>
        /// <returns>
        /// The get users count.
        /// </returns>
        public int GetUsersCount(int portalId, string portalAlias)
        {
            int totalRecords;
            MembershipProvider.GetAllUsers(
                /*CurrentPortalSettings.PortalAlias*/ portalAlias, 0, 1, out totalRecords);
            return totalRecords;
        }

        /// <summary>
        /// The GetUsersNoRole method retuns a list of all members that doesn´t have any roles.
        /// </summary>
        /// <param name="portalId">
        /// The portal id
        /// </param>
        /// <returns>
        /// A string array.
        /// </returns>
        public string[] GetUsersNoRole(int portalId)
        {
            var userCollection = Membership.GetAllUsers();
            return
                userCollection.Cast<MembershipUser>()
                .Where(user => Roles.GetRolesForUser(user.UserName).Length == 0)
                .Select(user => user.UserName).ToArray();
        }

        /// <summary>
        /// UsersDB.Login() Method.
        ///     The Login method validates a email/password hash pair against credentials
        ///     stored in the users database.  If the email/password hash pair is valid,
        ///     the method returns user's name.
        /// </summary>
        /// <param name="userName">
        /// The userName.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="portalAlias">
        /// The portal Alias.
        /// </param>
        /// <returns>
        /// A membership user.
        /// </returns>
        /// <remarks>
        /// UserLogin Stored Procedure
        /// </remarks>
        public MembershipUser Login(string userName, string password, string portalAlias)
        {
            

            var user = (AppleseedUser)MembershipProvider.GetUser(userName, true);
            if (user != null)
            {
                var valid = MembershipProvider.ValidateUser(user.UserName, password);

                return valid ? user : null;
            }

            return null;
        }

        /// <summary>
        /// Renames a role
        /// </summary>
        /// <param name="roleId">
        /// The role id
        /// </param>
        /// <param name="newRoleName">
        /// The new role name
        /// </param>
        /// <param name="portalAlias">
        /// The portal alias
        /// </param>
        public void UpdateRole(Guid roleId, string newRoleName, string portalAlias)
        {
            RoleProvider.RenameRole(portalAlias, roleId, newRoleName);
        }

        /// <summary>
        /// UpdateUser
        ///     This overload allow to change identity of the user
        /// </summary>
        /// <param name="oldUserId">
        /// The old user ID.
        /// </param>
        /// <param name="userId">
        /// The user ID.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="company">
        /// The company.
        /// </param>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <param name="city">
        /// The city.
        /// </param>
        /// <param name="zip">
        /// The zip.
        /// </param>
        /// <param name="countryId">
        /// The country ID.
        /// </param>
        /// <param name="stateId">
        /// The state ID.
        /// </param>
        /// <param name="phone">
        /// The phone.
        /// </param>
        /// <param name="fax">
        /// The fax.
        /// </param>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <param name="sendNewsletter">
        /// if set to <c>true</c> [send newsletter].
        /// </param>
        public void UpdateUser(
            Guid oldUserId,
            Guid userId,
            string name,
            string company,
            string address,
            string city,
            string zip,
            string countryId,
            int stateId,
            string phone,
            string fax,
            string email,
            bool sendNewsletter)
        {
            if (oldUserId != userId)
            {
                throw new ApplicationException("UpdateUser: oldUserID != userID");
            }

            var user = MembershipProvider.GetUser(userId, false) as AppleseedUser;
            if (user != null)
            {
                user.Email = email;
                user.Name = name;
                user.Company = company;
                user.Address = address;
                user.Zip = zip;
                user.City = city;
                user.CountryID = countryId;
                user.StateID = stateId;
                user.Fax = fax;
                user.Phone = phone;
                user.SendNewsletter = sendNewsletter;

                MembershipProvider.UpdateUser(user);
            }
        }

        /// <summary>
        /// Update User
        ///     Autogenerated by CodeWizard 04/04/2003 17.55.40
        /// </summary>
        /// <param name="userId">
        /// The user ID.
        /// </param>
        /// <param name="name">
        /// The user name.
        /// </param>
        /// <param name="company">
        /// The company.
        /// </param>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <param name="city">
        /// The user's city.
        /// </param>
        /// <param name="zip">
        /// The zip code.
        /// </param>
        /// <param name="countryId">
        /// The country ID.
        /// </param>
        /// <param name="stateId">
        /// The state ID.
        /// </param>
        /// <param name="phone">
        /// The phone.
        /// </param>
        /// <param name="fax">
        /// The fax number.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <param name="sendNewsletter">
        /// if set to <c>true</c> [send newsletter].
        /// </param>
        /// <param name="portalAlias">
        /// The portal Alias.
        /// </param>
        public void UpdateUser(
            Guid userId,
            string name,
            string company,
            string address,
            string city,
            string zip,
            string countryId,
            int stateId,
            string phone,
            string fax,
            string password,
            string email,
            bool sendNewsletter,
            string portalAlias)
        {
            var user = MembershipProvider.GetUser(userId, false) as AppleseedUser;
            if (user == null)
            {
                return;
            }

            user.Email = email;
            user.Name = name;
            user.Company = company;
            user.Address = address;
            user.Zip = zip;
            user.City = city;
            user.CountryID = countryId;
            user.StateID = stateId;
            user.Fax = fax;
            user.Phone = phone;
            user.SendNewsletter = sendNewsletter;

            MembershipProvider.ChangePassword(
                /*CurrentPortalSettings.PortalAlias*/ portalAlias, user.UserName, user.GetPassword(), password);
            MembershipProvider.UpdateUser(user);
        }

        /// <summary>
        /// UpdateUser
        ///     Autogenerated by CodeWizard 04/04/2003 17.55.40
        /// </summary>
        /// <param name="userId">
        /// The user ID.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="company">
        /// The company.
        /// </param>
        /// <param name="address">
        /// The address.
        /// </param>
        /// <param name="city">
        /// The city.
        /// </param>
        /// <param name="zip">
        /// The zip.
        /// </param>
        /// <param name="countryId">
        /// The country ID.
        /// </param>
        /// <param name="stateId">
        /// The state ID.
        /// </param>
        /// <param name="phone">
        /// The phone.
        /// </param>
        /// <param name="fax">
        /// The fax.
        /// </param>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <param name="sendNewsletter">
        /// if set to <c>true</c> [send newsletter].
        /// </param>
        public void UpdateUser(
            Guid userId,
            string name,
            string company,
            string address,
            string city,
            string zip,
            string countryId,
            int stateId,
            string phone,
            string fax,
            string email,
            bool sendNewsletter)
        {
            var user = MembershipProvider.GetUser(userId, false) as AppleseedUser;
            if (user == null)
            {
                return;
            }
            
            user.Email = email;
            user.Name = name;
            user.Company = company;
            user.Address = address;
            user.Zip = zip;
            user.City = city;
            user.CountryID = countryId;
            user.StateID = stateId;
            user.Fax = fax;
            user.Phone = phone;
            user.SendNewsletter = sendNewsletter;

            MembershipProvider.UpdateUser(user);
        }

        /// <summary>
        /// The UpdateUserCheckEmail sets the user email as trusted and verified
        /// </summary>
        /// <param name="userId">
        /// The user ID.
        /// </param>
        /// <param name="checkedEmail">
        /// The checked email.
        /// </param>
        public void UpdateUserCheckEmail(int userId, bool checkedEmail)
        {
            var user = Membership.GetUser(userId);
            if (user == null)
            {
                return;
            }
            
            user.IsApproved = checkedEmail;

            Membership.UpdateUser(user);
        }

        /// <summary>
        /// Change the user password with a new one
        /// </summary>
        /// <param name="userId">
        /// The user ID.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        public void UpdateUserSetPassword(int userId, string password)
        {
            var user = Membership.GetUser(userId);
            if (user == null)
            {
                return;
            }

            user.ChangePassword(user.GetPassword(), password);

            Membership.UpdateUser(user);
        }

        #endregion
    }
}