using System.Security.Principal;
using System.Web.Security;
using System;
using Appleseed.Framework.Users.Data;
using Appleseed.Framework.Site.Configuration;
using System.Web;
using Appleseed.Framework.Providers.AppleseedMembershipProvider;

namespace Appleseed.Framework.Security {
    /// <summary>
    /// AppleseedPrincipal
    /// </summary>
    public class AppleseedPrincipal : GenericPrincipal {

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AppleseedPrincipal"/> class.
        /// </summary>
        /// <param name="identity">A basic implementation of <see cref="T:System.Security.Principal.IIdentity"></see> that represents any user.</param>
        /// <param name="roles">An array of role names to which the user represented by the identity parameter belongs.</param>
        /// <returns>
        /// A void value...
        /// </returns>
        public AppleseedPrincipal( IIdentity identity, string[] roles )
            : base( identity, roles ) {
        }

        
        /// <summary>
        /// Get the Appleseed User
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.Security.Principal.GenericIdentity"></see> of the user represented by the <see cref="T:System.Security.Principal.GenericPrincipal"></see>.</returns>
        public new AppleseedUser Identity {
            get {
                if ( base.Identity is MembershipUser ) {
                    ErrorHandler.Publish( LogLevel.Info, "AppleseedPrincipal::Identity  -> base.Identity is MembershipUser" );
                    return base.Identity as AppleseedUser;
                }

                UsersDB users = new UsersDB();
                PortalSettings PortalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
                return users.GetSingleUser( base.Identity.Name, PortalSettings.PortalAlias  );
            }
        }

    }
}