using System;
using Appleseed.Framework.Security;
using Appleseed.Framework.Web.UI;
using Appleseed.Framework.UI.WebControls;
using Appleseed.Framework;

namespace Appleseed.Admin
{
    /// <summary>
    /// The Logoff page is responsible for signing out a user 
    /// from the cookie authentication, and then redirecting 
    /// the user back to the portal home page.
    /// This page is executed when the user	clicks 
    /// the Logoff button at the top of the page.
    /// </summary>
    public partial class Logoff : Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
			var controlStr = "~/DesktopModules/CoreModules/SignIn/Signin.ascx";
            if (this.PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_LOGIN_TYPE"))
            {
                controlStr = Convert.ToString(this.PortalSettings.CustomSettings["SITESETTINGS_LOGIN_TYPE"]);
            }

            try
            {
                var control= this.LoadControl(controlStr);
                if(control is SignInControl){
                    ((SignInControl)this.LoadControl(controlStr)).Logoff();
                }
            }
            catch (Exception exc)
            {
                ErrorHandler.Publish(LogLevel.Error, exc);
                var control = this.LoadControl("~/DesktopModules/CoreModules/SignIn/Signin.ascx");
                if (control is SignInControl)
                {
                    ((SignInControl)this.LoadControl("~/DesktopModules/CoreModules/SignIn/Signin.ascx")).Logoff();
                }
            }
		
            // Signout
            PortalSecurity.SignOut();
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises the Init event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            base.OnInit(e);
        }

        #endregion
    }
}