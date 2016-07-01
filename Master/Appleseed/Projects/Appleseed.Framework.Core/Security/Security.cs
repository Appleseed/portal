// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Security.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The PortalSecurity class encapsulates two helper methods that enable
//   developers to easily check the role status of the current browser client.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Security
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Principal;
    using System.Text;
    using System.Web;
    using System.Web.Security;

    using Appleseed.Framework.Providers.AppleseedRoleProvider;
    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.Site.Data;
    using Appleseed.Framework.Users.Data;

    /// <summary>
    /// The PortalSecurity class encapsulates two helper methods that enable
    ///   developers to easily check the role status of the current browser client.
    /// </summary>
    /// <remarks>
    /// </remarks>
    [History("jminond", "2004/09/29", 
        "added killsession method mimic of sign out, as well as modified sign on to use cookieexpire in Appleseed.config"
        )]
    [History("gman3001", "2004/09/29", "Call method for recording the user's last visit date on successful signon")]
    [History("jviladiu@portalServices.net", "2004/09/23", "Get users & roles from true portal if UseSingleUserBase=true"
        )]
    [History("jviladiu@portalServices.net", "2004/08/23", 
        "Deleted repeated code in HasxxxPermissions and GetxxxPermissions")]
    [History("cisakson@yahoo.com", "2003/04/28", 
        "Changed the IsInRole function so it support's a custom setting for Windows portal admins!")]
    [History("Geert.Audenaert@Syntegra.Com", "2003/03/26", 
        "Changed the IsInRole function so it support's users to in case of windowsauthentication!")]
    [History("Thierry (tiptopweb)", "2003/04/12", "Migrate shopping cart in SignOn for E-Commerce")]
    public class PortalSecurity
    {
        #region Constants and Fields

        /// <summary>
        /// The portal settings.
        /// </summary>
        private const string StringsPortalSettings = "PortalSettings";

        #endregion

        #region Public Methods

        /// <summary>
        /// Single point access deny.
        ///   Called when there is an unauthorized access attempt.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public static void AccessDenied()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                throw new HttpException(403, "Access Denied", 2);
            }
            
            HttpContext.Current.Response.Redirect(
                HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Admin/Logon.aspx"));
        }

        /// <summary>
        /// Single point edit access deny.
        ///   Called when there is an unauthorized access attempt to an edit page.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public static void AccessDeniedEdit()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                throw new HttpException(403, "Access Denied Edit", 3);
            }
            
            HttpContext.Current.Response.Redirect(
                HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Admin/Logon.aspx"));
        }

        /// <summary>
        /// ExtendCookie
        /// </summary>
        /// <param name="PortalSettings">
        /// The portal settings.
        /// </param>
        /// <param name="minuteAdd">
        /// The minute add.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void ExtendCookie(PortalSettings PortalSettings, int minuteAdd)
        {
            var time = DateTime.Now;
            var span = new TimeSpan(0, 0, minuteAdd, 0, 0);

            HttpContext.Current.Response.Cookies["Appleseed_" + PortalSettings.PortalAlias].Expires = time.Add(span);

            return;
        }

        /// <summary>
        /// ExtendCookie
        /// </summary>
        /// <param name="PortalSettings">
        /// The portal settings.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void ExtendCookie(PortalSettings PortalSettings)
        {
            var minuteAdd = Config.CookieExpire;
            ExtendCookie(PortalSettings, minuteAdd);
            return;
        }

        // [Flags]
        // public enum SecurityPermission : uint
        // {
        // NoAccess    = 0x0000,
        // View        = 0x0001,
        // Add         = 0x0002,
        // Edit        = 0x0004,
        // Delete      = 0x0008
        // }

        /// <summary>
        /// The GetAddPermissions method enables developers to easily retrieve
        ///   a list of roles that have Add permissions for the
        ///   specified portal module
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <returns>
        /// A list of roles that have Add permissions for the specified module separated by ;
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static string GetAddPermissions(int moduleId)
        {
            return GetPermissions(moduleId, "rb_GetAuthAddRoles", "@AddRoles");
        }

        /// <summary>
        /// The GetApprovePermissions method enables developers to easily retrieve
        ///   a list of roles that have Approve permissions for the
        ///   specified portal module
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <returns>
        /// A string of roles that have approve permissions separated by ;
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static string GetApprovePermissions(int moduleId)
        {
            return GetPermissions(moduleId, "rb_GetAuthApproveRoles", "@ApproveRoles");
        }

        /// <summary>
        /// The GetDeleteModulePermissions method enables developers to easily retrieve
        ///   a list of roles that have access to delete specified portal moduleModule.
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <returns>
        /// A list of roles that have delete module permission for the specified module separated by ;
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static string GetDeleteModulePermissions(int moduleId)
        {
            return GetPermissions(moduleId, "rb_GetAuthDeleteModuleRoles", "@DeleteModuleRoles");
        }

        /// <summary>
        /// The GetDeletePermissions method enables developers to easily retrieve
        ///   a list of roles that have access to Delete the
        ///   specified portal module
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <returns>
        /// A list of roles that have delete permissions for the specified module separated by ;
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static string GetDeletePermissions(int moduleId)
        {
            return GetPermissions(moduleId, "rb_GetAuthDeleteRoles", "@DeleteRoles");
        }

        /// <summary>
        /// The GetEditPermissions method enables developers to easily retrieve
        ///   a list of roles that have Edit Permissions
        ///   of a specified portal module.
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <returns>
        /// A list of roles that have Edit permissions separated by ;
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static string GetEditPermissions(int moduleId)
        {
            return GetPermissions(moduleId, "rb_GetAuthEditRoles", "@EditRoles");
        }

        /// <summary>
        /// The GetMoveModulePermissions method enables developers to easily retrieve
        ///   a list of roles that have access to move specified portal moduleModule.
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <returns>
        /// A list of roles that have move module permission for the specified module separated by ;
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static string GetMoveModulePermissions(int moduleId)
        {
            return GetPermissions(moduleId, "rb_GetAuthMoveModuleRoles", "@MoveModuleRoles");
        }

        /// <summary>
        /// The GetPropertiesPermissions method enables developers to easily retrieve
        ///   a list of roles that have access to Properties for the
        ///   specified portal module
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <returns>
        /// A list of roles that have Properties permission for the specified module separated by ;
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static string GetPropertiesPermissions(int moduleId)
        {
            return GetPermissions(moduleId, "rb_GetAuthPropertiesRoles", "@PropertiesRoles");
        }

        /// <summary>
        /// The GetPublishPermissions method enables developers to easily retrieve
        ///   the list of roles that have Publish Permissions.
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <returns>
        /// A list of roles that has Publish Permissions separated by ;
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static string GetPublishPermissions(int moduleId)
        {
            return GetPermissions(moduleId, "rb_GetAuthPublishingRoles", "@PublishingRoles");
        }

        /// <summary>
        /// Single point get roles
        /// </summary>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static IList<AppleseedRole> GetRoles()
        {
            // Obtain PortalSettings from Current Context
            var PortalSettings = (PortalSettings)HttpContext.Current.Items[StringsPortalSettings];
            var portalId = PortalSettings.PortalID;

            // john.mandia@whitelightsolutions.com: 29th May 2004 When retrieving/editing/adding roles or users etc then portalID should be 0 if it is shared
            // But I commented this out as this check is done in UsersDB.GetRoles Anyway
            // if (Config.UseSingleUserBase) portalID = 0;
            IList<AppleseedRole> roles;

            // TODO: figure out if we could persist role Guid in cookies

            //// Create the roles cookie if it doesn't exist yet for this session.
            // if ((HttpContext.Current.Request.Cookies["portalroles"] == null) || (HttpContext.Current.Request.Cookies["portalroles"].Value == string.Empty) || (HttpContext.Current.Request.Cookies["portalroles"].Expires < DateTime.Now)) 
            // {
            try
            {
                // Get roles from UserRoles table, and add to cookie
                var accountSystem = new UsersDB();
                MembershipUser u = accountSystem.GetSingleUser(
                    HttpContext.Current.User.Identity.Name, PortalSettings.PortalAlias);
                roles = accountSystem.GetRoles(u.Email, PortalSettings.PortalAlias);
            }
            catch (Exception exc)
            {
                ErrorHandler.Publish(LogLevel.Error, exc);

                // no roles
                roles = new List<AppleseedRole>();
            }

            // // Create a string to persist the roles
            // string roleStr = string.Empty;
            // foreach ( AppleseedRole role in roles ) 
            // {
            // roleStr += role.Name;
            // roleStr += ";";
            // }

            // // Create a cookie authentication ticket.
            // FormsAuthenticationTicket ticket = new FormsAuthenticationTicket
            // (
            // 1,                              // version
            // HttpContext.Current.User.Identity.Name,     // user name
            // DateTime.Now,                   // issue time
            // DateTime.Now.AddHours(1),       // expires every hour
            // false,                          // don't persist cookie
            // roleStr                         // roles
            // );

            // // Encrypt the ticket
            // string cookieStr = FormsAuthentication.Encrypt(ticket);

            // // Send the cookie to the client
            // HttpContext.Current.Response.Cookies["portalroles"].Value = cookieStr;
            // HttpContext.Current.Response.Cookies["portalroles"].Path = "/";
            // HttpContext.Current.Response.Cookies["portalroles"].Expires = DateTime.Now.AddMinutes(1);
            // }
            // else 
            // {
            // // Get roles from roles cookie
            // FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(HttpContext.Current.Request.Cookies["portalroles"].Value);

            // //convert the string representation of the role data into a string array
            // ArrayList userRoles = new ArrayList();

            // //by Jes
            // string _ticket = ticket.UserData.TrimEnd(new char[] {';'}); 
            // foreach (string role in _ticket.Split(new char[] {';'} )) 
            // { 
            // userRoles.Add(role + ";"); 
            // }
            // roles = (string[]) userRoles.ToArray(typeof(string));
            // }
            return roles;
        }

        /// <summary>
        /// The GetViewPermissions method enables developers to easily retrieve
        ///   a list of roles that have View permissions for the
        ///   specified portal module
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <returns>
        /// A list of roles that have View permissions for the specified module separated by ;
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static string GetViewPermissions(int moduleId)
        {
            return GetPermissions(moduleId, "rb_GetAuthViewRoles", "@ViewRoles");
        }

        /// <summary>
        /// The HasAddPermissions method enables developers to easily check
        ///   whether the current browser client has access to Add the
        ///   specified portal module
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <returns>
        /// <c>true</c> if [has add permissions] [the specified module ID]; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static bool HasAddPermissions(int moduleId)
        {
            return hasPermissions(moduleId, "rb_GetAuthAddRoles", "@AddRoles");
        }

        /// <summary>
        /// The HasApprovePermissions method enables developers to easily check
        ///   whether the current browser client has access to Approve the
        ///   specified portal module
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <returns>
        /// <c>true</c> if [has approve permissions] [the specified module ID]; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static bool HasApprovePermissions(int moduleId)
        {
            return hasPermissions(moduleId, "rb_GetAuthApproveRoles", "@ApproveRoles");
        }

        /// <summary>
        /// The HasDeletePermissions method enables developers to easily check
        ///   whether the current browser client has access to Delete the
        ///   specified portal module
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <returns>
        /// <c>true</c> if [has delete permissions] [the specified module ID]; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static bool HasDeletePermissions(int moduleId)
        {
            return hasPermissions(moduleId, "rb_GetAuthDeleteRoles", "@DeleteRoles");
        }

        /// <summary>
        /// The HasEditPermissions method enables developers to easily check
        ///   whether the current browser client has access to edit the settings
        ///   of a specified portal module.
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <returns>
        /// <c>true</c> if [has edit permissions] [the specified module ID]; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static bool HasEditPermissions(int moduleId)
        {
            // 			if (RecyclerDB.ModuleIsInRecycler(moduleID))
            // 				return hasPermissions (moduleID, "rb_GetAuthEditRolesRecycler", "@EditRoles");
            // 			else
            return hasPermissions(moduleId, "rb_GetAuthEditRoles", "@EditRoles");
        }

        /// <summary>
        /// The HasPropertiesPermissions method enables developers to easily check
        ///   whether the current browser client has access to Properties the
        ///   specified portal module
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <returns>
        /// <c>true</c> if [has properties permissions] [the specified module ID]; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static bool HasPropertiesPermissions(int moduleId)
        {
            return hasPermissions(moduleId, "rb_GetAuthPropertiesRoles", "@PropertiesRoles");
        }

        /// <summary>
        /// The HasPublishPermissions method enables developers to easily check
        ///   whether the current browser client has access to Approve the
        ///   specified portal module
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <returns>
        /// <c>true</c> if [has publish permissions] [the specified module ID]; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static bool HasPublishPermissions(int moduleId)
        {
            return hasPermissions(moduleId, "rb_GetAuthPublishingRoles", "@PublishingRoles");
        }

        /// <summary>
        /// The HasViewPermissions method enables developers to easily check
        ///   whether the current browser client has access to view the
        ///   specified portal module
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <returns>
        /// <c>true</c> if [has view permissions] [the specified module ID]; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        [History("JB - john@bowenweb.com", "2005/06/11", "Added support for module Recycle Bin")]
        public static bool HasViewPermissions(int moduleId)
        {
            return hasPermissions(moduleId, "rb_GetAuthViewRoles", "@ViewRoles");
        }

        /// <summary>
        /// PortalSecurity.IsInRole() Method
        ///   The IsInRole method enables developers to easily check the role
        ///   status of the current browser client.
        /// </summary>
        /// <param name="role">
        /// The role.
        /// </param>
        /// <returns>
        /// <c>true</c> if [is in role] [the specified role]; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static bool IsInRole(string role)
        {
            // Check if integrated windows authentication is used ?
            var useNTLM = HttpContext.Current.User is WindowsPrincipal;

            // Check if the user is in the Admins role.
            if (useNTLM && role.Trim() == "Admins")
            {
                // Obtain PortalSettings from Current Context
                // WindowsAdmins added 28.4.2003 Cory Isakson
                var PortalSettings = (PortalSettings)HttpContext.Current.Items[StringsPortalSettings];
                var winRoles = new StringBuilder();
                winRoles.Append(PortalSettings.CustomSettings["WindowsAdmins"]);
                winRoles.Append(";");

                // jes1111 - winRoles.Append(ConfigurationSettings.AppSettings["ADAdministratorGroup"]);
                winRoles.Append(Config.ADAdministratorGroup);
                return IsInRoles(winRoles.ToString());
            }

            // Allow giving access to users 
            if (useNTLM && role == HttpContext.Current.User.Identity.Name)
            {
                return true;
            }
            
            return HttpContext.Current.User.IsInRole(role);
        }

        /// <summary>
        /// The IsInRoles method enables developers to easily check the role
        ///   status of the current browser client against an array of roles
        /// </summary>
        /// <param name="roles">
        /// The roles.
        /// </param>
        /// <returns>
        /// <c>true</c> if [is in roles] [the specified roles]; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static bool IsInRoles(string roles)
        {
            var context = HttpContext.Current;

            // if (!context.User.Identity.IsAuthenticated || Membership.GetUser(context.User.Identity.Name) != null) {
            if (roles != null)
            {
                foreach (var role in roles.Split(new[] { ';' }).Select(splitRole => splitRole.Trim()))
                {
                    if (!string.IsNullOrEmpty(role) && ((role == "All Users") || IsInRole(role)))
                    {
                        return true;
                    }

                    // Authenticated user role added
                    // 15 nov 2002 - by manudea
                    if ((role == "Authenticated Users") && context.Request.IsAuthenticated)
                    {
                        return true;
                    }

                    // end authenticated user role added

                    // Unauthenticated user role added
                    // 30/01/2003 - by manudea
                    if ((role == "Unauthenticated Users") && (!context.Request.IsAuthenticated))
                    {
                        return true;
                    }
                }
            }

            return false;

            // }
            // PortalSecurity.SignOut();
            // return false;
        }

        /// <summary>
        /// Kills session after timeout
        ///   jminond - fix kill session after timeout.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public static void KillSession()
        {
            SignOut(HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Admin/Logon.aspx"), true);

            // HttpContext.Current.Response.Redirect(urlToRedirect);
            // PortalSecurity.AccessDenied();
        }

        /// <summary>
        /// Redirect user back to the Portal Home Page.
        ///   Mainly used after a successful login.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public static void PortalHome()
        {
            HttpContext.Current.Response.Redirect(HttpUrlBuilder.BuildUrl("~/"+HttpUrlBuilder.DefaultPage));
        }

        /// <summary>
        /// Single point edit access deny from the Secure server (SSL)
        ///   Called when there is an unauthorized access attempt to an edit page.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public static void SecureAccessDenied()
        {
            throw new HttpException(403, "Secure Access Denied", 3);
        }

        /// <summary>
        /// Single point for logging on an user, not persistent.
        /// </summary>
        /// <param name="user">
        /// Username or email
        /// </param>
        /// <param name="password">
        /// Password
        /// </param>
        /// <returns>
        /// The sign on.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static string SignOn(string user, string password)
        {
            return SignOn(user, password, false);
        }

        /// <summary>
        /// Single point for logging on an user.
        /// </summary>
        /// <param name="user">
        /// Username or email
        /// </param>
        /// <param name="password">
        /// Password
        /// </param>
        /// <param name="persistent">
        /// Use a cookie to make it persistent
        /// </param>
        /// <returns>
        /// The sign on.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static string SignOn(string user, string password, bool persistent)
        {
            return SignOn(user, password, persistent, null);
        }

        /// <summary>
        /// Single point for logging on an user.
        /// </summary>
        /// <param name="user">
        /// Username or email
        /// </param>
        /// <param name="password">
        /// Password
        /// </param>
        /// <param name="persistent">
        /// Use a cookie to make it persistent
        /// </param>
        /// <param name="redirectPage">
        /// The redirect page.
        /// </param>
        /// <returns>
        /// The sign on.
        /// </returns>
        /// <remarks>
        /// </remarks>
        [History("bja@reedtek.com", "2003/05/16", "Support for collapsible")]
        public static string SignOn(string user, string password, bool persistent, string redirectPage)
        {
            // Obtain PortalSettings from Current Context
            var PortalSettings = (PortalSettings)HttpContext.Current.Items[StringsPortalSettings];

            MembershipUser usr;
            var accountSystem = new UsersDB();

            // Attempt to Validate User Credentials using UsersDB
            usr = accountSystem.Login(user, password, PortalSettings.PortalAlias);

            // Thierry (tiptopweb), 12 Apr 2003: Save old ShoppingCartID
            // 			ShoppingCartDB shoppingCart = new ShoppingCartDB();
            // 			string tempCartID = ShoppingCartDB.GetCurrentShoppingCartID();

            if (usr != null)
            {
                // Ender, 31 July 2003: Support for the monitoring module by Paul Yarrow
                if (Config.EnableMonitoring)
                {
                    try
                    {
                        Monitoring.LogEntry(
                            (Guid)usr.ProviderUserKey, PortalSettings.PortalID, -1, "Logon", string.Empty);
                    }
                    catch
                    {
                        ErrorHandler.Publish(LogLevel.Info, "Cannot monitoring login user " + usr.UserName);
                    }
                }

                // Use security system to set the UserID within a client-side Cookie
                FormsAuthentication.SetAuthCookie(usr.ToString(), persistent);

                // Appleseed Security cookie Required if we are sharing a single domain 
                // with portal Alias in the URL

                // Set a cookie to persist authentication for each portal 
                // so user can be reauthenticated 
                // automatically if they chose to Remember Login
                var hck = HttpContext.Current.Response.Cookies["Appleseed_" + PortalSettings.PortalAlias.ToLower()];
                if (hck != null)
                {
                    hck.Value = usr.ToString(); // Fill all data: name + email + id
                    hck.Path = "/";

                    if (persistent)
                    {
                        // Keep the cookie?
                        hck.Expires = DateTime.Now.AddYears(50);
                    }
                    else
                    {
                        // jminond - option to kill cookie after certain time always
                        // jes1111
                        // 					if(ConfigurationSettings.AppSettings["CookieExpire"] != null)
                        // 					{
                        // 						int minuteAdd = int.Parse(ConfigurationSettings.AppSettings["CookieExpire"]);
                        var minuteAdd = Config.CookieExpire;

                        var time = DateTime.Now;
                        var span = new TimeSpan(0, 0, minuteAdd, 0, 0);

                        hck.Expires = time.Add(span);

                        // 					}
                    }
                }

                if (string.IsNullOrEmpty(redirectPage))
                {
                    // Redirect browser back to originating page
                    HttpContext.Current.Response.Redirect(
                        HttpContext.Current.Request.UrlReferrer != null
                            ? HttpContext.Current.Request.UrlReferrer.ToString()
                            : (HttpContext.Current.Request.ApplicationPath != null ? HttpContext.Current.Request.ApplicationPath.ToString() : "/"));

                    return usr.Email;
                }
                
                HttpContext.Current.Response.Redirect(redirectPage);
            }

            return null;
        }

        /// <summary>
        /// Single point logoff
        /// </summary>
        /// <remarks>
        /// </remarks>
        public static void SignOut()
        {
            SignOut(HttpUrlBuilder.BuildUrl("~/" + HttpUrlBuilder.DefaultPage), true);
        }

        /// <summary>
        /// Single point logoff
        /// </summary>
        /// <param name="urlToRedirect">
        /// The URL to redirect.
        /// </param>
        /// <param name="removeLogin">
        /// if set to <c>true</c> [remove login].
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void SignOut(string urlToRedirect, bool removeLogin)
        {
            var st = new StackTrace(new StackFrame(1, true));
            var frames = st.GetFrames();
            if (frames != null)
            {
                var stackString = frames.Aggregate(string.Empty, (current, frame) => current + ("> " + frame.GetMethod().Name));

                ErrorHandler.Publish(LogLevel.Info, "Hago signout: " + stackString);
            }

            // Log User Off from Cookie Authentication System
            FormsAuthentication.SignOut();

            // Invalidate roles token
            var hck = HttpContext.Current.Response.Cookies["portalroles"];
            if (hck != null)
            {
                hck.Value = null;
                hck.Expires = new DateTime(1999, 10, 12);
                hck.Path = "/";
            }

            if (removeLogin)
            {
                // Obtain PortalSettings from Current Context
                var PortalSettings = (PortalSettings)HttpContext.Current.Items[StringsPortalSettings];

                // Invalidate Portal Alias Cookie security
                var xhck = HttpContext.Current.Response.Cookies["Appleseed_" + PortalSettings.PortalAlias.ToLower()];
                if (xhck != null)
                {
                    xhck.Value = null;
                    xhck.Expires = new DateTime(1999, 10, 12);
                    xhck.Path = "/";
                }
            }

            // [START]  bja@reedtek.com remove user window information
            // User Information
            // valid user
            if (HttpContext.Current.User != null)
            {
                // Obtain PortalSettings from Current Context
                // Ender 4 July 2003: Added to support the Monitoring module by Paul Yarrow
                var PortalSettings = (PortalSettings)HttpContext.Current.Items[StringsPortalSettings];

                // User Information
                var users = new UsersDB();
                MembershipUser user = users.GetSingleUser(
                    HttpContext.Current.User.Identity.Name, PortalSettings.PortalAlias);

                if (user != null && user.ProviderUserKey != null)
                {
                    // get user id
                    var uid = (Guid)user.ProviderUserKey;

                    if (!uid.Equals(Guid.Empty))
                    {
                        try
                        {
                            if (Config.EnableMonitoring)
                            {
                                Monitoring.LogEntry(uid, PortalSettings.PortalID, -1, "Logoff", string.Empty);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }

            // [END ]  bja@reedtek.com remove user window information

            // Redirect user back to the Portal Home Page
            if (urlToRedirect.Length > 0)
            {
                HttpContext.Current.Response.Redirect(urlToRedirect);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="procedureName">
        /// Name of the procedure.
        /// </param>
        /// <param name="parameterRole">
        /// The parameter role.
        /// </param>
        /// <returns>
        /// The get permissions.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private static string GetPermissions(int moduleId, string procedureName, string parameterRole)
        {
            // Obtain PortalSettings from Current Context
            var PortalSettings = (PortalSettings)HttpContext.Current.Items[StringsPortalSettings];
            var portalId = PortalSettings.PortalID;

            // jviladiu@portalServices.net: Get users & roles from true portal (2004/09/23)
            if (Config.UseSingleUserBase)
            {
                portalId = 0;
            }

            // Create Instance of Connection and Command Object
            using (var myConnection = Config.SqlConnectionString)
            using (var myCommand = new SqlCommand(procedureName, myConnection))
            {
                // Mark the Command as a SPROC
                myCommand.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
                parameterModuleID.Value = moduleId;
                myCommand.Parameters.Add(parameterModuleID);

                var parameterPortalID = new SqlParameter("@PortalID", SqlDbType.Int, 4);
                parameterPortalID.Value = portalId;
                myCommand.Parameters.Add(parameterPortalID);

                // Add out parameters to Sproc
                var parameterAccessRoles = new SqlParameter("@AccessRoles", SqlDbType.NVarChar, 256);
                parameterAccessRoles.Direction = ParameterDirection.Output;
                myCommand.Parameters.Add(parameterAccessRoles);

                var parameterRoles = new SqlParameter(parameterRole, SqlDbType.NVarChar, 256);
                parameterRoles.Direction = ParameterDirection.Output;
                myCommand.Parameters.Add(parameterRoles);

                // Open the database connection and execute the command
                myConnection.Open();
                try
                {
                    myCommand.ExecuteNonQuery();
                }
                finally
                {
                }

                return parameterRoles.Value.ToString();
            }
        }

        /// <summary>
        /// Determines whether the specified module ID has permissions.
        /// </summary>
        /// <param name="moduleID">
        /// The module ID.
        /// </param>
        /// <param name="procedureName">
        /// Name of the procedure.
        /// </param>
        /// <param name="parameterRol">
        /// The parameter rol.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified module ID has permissions; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private static bool hasPermissions(int moduleID, string procedureName, string parameterRol)
        {
            if (RecyclerDB.ModuleIsInRecycler(moduleID))
            {
                procedureName = procedureName + "Recycler";
            }

            if (moduleID <= 0)
            {
                return false;
            }

            // Obtain PortalSettings from Current Context
            var PortalSettings = (PortalSettings)HttpContext.Current.Items[StringsPortalSettings];
            int portalID;
            if (PortalSettings != null)
                portalID = PortalSettings.PortalID;
            else
            {
                var alias = HttpContext.Current.Items["PortalAlias"];
                var db = new Massive.DynamicModel("ConnectionString", "rb_Portals");
                var elemenents = db.All("PortalAlias = @0", columns: "PortalID", args: alias);
                if (elemenents.Count() != 1)
                    return false;
                portalID = ((dynamic)((System.Dynamic.ExpandoObject)(elemenents.First()))).PortalID;
            }

            // jviladiu@portalServices.net: Get users & roles from true portal (2004/09/23)
            if (Config.UseSingleUserBase)
            {
                portalID = 0;
            }

            // Create Instance of Connection and Command Object
            using (var myConnection = Config.SqlConnectionString)
            {
                using (var myCommand = new SqlCommand(procedureName, myConnection))
                {
                    // Mark the Command as a SPROC
                    myCommand.CommandType = CommandType.StoredProcedure;

                    // Add Parameters to SPROC
                    var parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
                    parameterModuleID.Value = moduleID;
                    myCommand.Parameters.Add(parameterModuleID);

                    var parameterPortalID = new SqlParameter("@PortalID", SqlDbType.Int, 4);
                    parameterPortalID.Value = portalID;
                    myCommand.Parameters.Add(parameterPortalID);

                    // Add out parameters to Sproc
                    var parameterAccessRoles = new SqlParameter("@AccessRoles", SqlDbType.NVarChar, 256);
                    parameterAccessRoles.Direction = ParameterDirection.Output;
                    myCommand.Parameters.Add(parameterAccessRoles);

                    var parameterRoles = new SqlParameter(parameterRol, SqlDbType.NVarChar, 256);
                    parameterRoles.Direction = ParameterDirection.Output;
                    myCommand.Parameters.Add(parameterRoles);

                    // Open the database connection and execute the command
                    myConnection.Open();
                    try
                    {
                        myCommand.ExecuteNonQuery();
                    }
                    finally
                    {
                    }

                    return IsInRoles(parameterAccessRoles.Value.ToString()) &&
                           IsInRoles(parameterRoles.Value.ToString());
                }
            }
        }

        #endregion

        #region MembershipExtension

        /// <summary>
        /// Admin chnage user password
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="Password">password</param>
        /// <returns>true/false</returns>
        public static bool AdminChangeUsersPassword(string username, string Password) {

            var provider = (Appleseed.Framework.Providers.AppleseedMembershipProvider.AppleseedMembershipProvider)Membership.Provider;
            return provider.AdminChangePassword(username, Password);
        
        
        }

        #endregion
    }
}