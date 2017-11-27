using Appleseed.Framework.DAL;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Users.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Appleseed.Framework.Security
{
    /// <summary>
    /// Current User Profile class
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// Gets or sets user data
        /// </summary>
        public MembershipUser UserInfo { get; set; }

        /// <summary>
        /// Gets or Sets current user permissin list
        /// </summary>
        public List<AccessPermissions> Permissions { get; set; }

        /// <summary>
        /// user Profile constructor
        /// </summary>
        /// <param name="username">Current logged in username</param>
        public UserProfile(string username)
        {
            UserInfo = Membership.GetUser(username);
            Permissions = PermissionsDB.GetUserPermissions(username);
        }

        /// <summary>
        /// check for currrent user has permissin
        /// </summary>
        /// <param name="permission">The permission</param>
        /// <returns>True or False</returns>
        public bool HasPermission(AccessPermissions permission)
        {
            if (this.Permissions != null)
                return HasAdminAccess() || this.Permissions.Contains(permission);
            else
                return false;
        }

        /// <summary>
        /// Is current user has admin access
        /// </summary>
        /// <returns>Return True if current user is admin other wise false</returns>
        public bool HasAdminAccess()
        {
            if (UserInfo != null)
                return Roles.IsUserInRole(UserInfo.UserName, "Admins");
            else
                return false;
        }

        /// <summary>
        /// Is current user Admin
        /// </summary>
        public static bool isCurrentUserAdmin
        {
            get
            {
                if (HttpContext.Current.User != null)
                    return Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, "Admins");
                else
                    return false;
            }
        }

        /// <summary>
        /// User profile for current user
        /// </summary>
        public static UserProfile CurrentUser
        {
            get
            {
                if (HttpContext.Current.User != null && PortalSettings.CurrentUser.Identity != null)
                {
                    return new UserProfile(PortalSettings.CurrentUser.Identity.UserName);
                }
                else
                {
                    //for not logged in / anonymous user
                    return new UserProfile(string.Empty);
                }
            }
        }

        /// <summary>
        /// Show edit this page link on top navbar
        /// </summary>
        /// <returns></returns>
        public static bool HasEditThisPageAccess()
        {
            return UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_EDITING)
                            || UserProfile.CurrentUser.HasPermission(AccessPermissions.MODULE_CREATION)
                            || UserProfile.CurrentUser.HasPermission(AccessPermissions.MODULE_EDITING)
                            || UserProfile.CurrentUser.HasPermission(AccessPermissions.MODULE_DELETION)
        || UserProfile.CurrentUser.HasPermission(AccessPermissions.PORTAL_THEME_AND_LAYOUT_ADMINISTRATION);
        }

        /// <summary>
        /// Secure page access
        /// </summary>
        /// <param name="page">Curent page</param>
        /// <returns>True or False</returns>
        public static bool HasPageAccess(SecurePages page)
        {
            switch (page)
            {
                case SecurePages.SiteSettings:
                    return UserProfile.CurrentUser.HasPermission(AccessPermissions.PORTAL_ADMINISTRATION)
                        || UserProfile.CurrentUser.HasPermission(AccessPermissions.PORTAL_THEME_AND_LAYOUT_ADMINISTRATION);

                case SecurePages.PageManager:
                    return UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_LIST) || UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_CREATION) || UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_DELETION) || UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_EDITING);

            }
            return false;
        }

        /// <summary>
        /// Show Administration link on top menu
        /// </summary>
        /// <returns>True or False</returns>
        public static bool HasAdminPageAccess()
        {
            return UserProfile.CurrentUser.HasPermission(AccessPermissions.PORTAL_ADMINISTRATION)
                || UserProfile.CurrentUser.HasPermission(AccessPermissions.PORTAL_THEME_AND_LAYOUT_ADMINISTRATION)
                || UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_LIST)
                || UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_CREATION) 
                || UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_DELETION) 
                || UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_EDITING);
        }

        /// <summary>
        /// Returns True if current user has permission to access module
        /// </summary>
        /// <returns>True or False</returns>
        public static bool HasModuleAccess()
        {
            return UserProfile.CurrentUser.HasPermission(AccessPermissions.PORTAL_ADMINISTRATION)
                || UserProfile.CurrentUser.HasPermission(AccessPermissions.PORTAL_THEME_AND_LAYOUT_ADMINISTRATION)
                || UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_LIST)
                || UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_CREATION)
                || UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_DELETION)
                || UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_EDITING);
        }


        public static bool HasModuleAddEditAccess()
        {
            return UserProfile.CurrentUser.HasPermission(AccessPermissions.PORTAL_ADMINISTRATION)
                || UserProfile.CurrentUser.HasPermission(AccessPermissions.PORTAL_THEME_AND_LAYOUT_ADMINISTRATION)
                || UserProfile.CurrentUser.HasPermission(AccessPermissions.MODULE_CREATION)
                || UserProfile.CurrentUser.HasPermission(AccessPermissions.MODULE_EDITING);
        }

        public static bool HasModuleDeleteAccess()
        {
            return UserProfile.CurrentUser.HasPermission(AccessPermissions.PORTAL_ADMINISTRATION)
                || UserProfile.CurrentUser.HasPermission(AccessPermissions.PORTAL_THEME_AND_LAYOUT_ADMINISTRATION)
                || UserProfile.CurrentUser.HasPermission(AccessPermissions.MODULE_EDITING);
        }

        public static bool HasPortalAdministrationAccess()
        {
            return UserProfile.CurrentUser.HasPermission(AccessPermissions.PORTAL_ADMINISTRATION);
        }
    }
}
