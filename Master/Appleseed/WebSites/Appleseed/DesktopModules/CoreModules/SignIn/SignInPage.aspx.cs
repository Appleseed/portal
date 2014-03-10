using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Page = Appleseed.Framework.Web.UI.Page;
using System.Web.UI.WebControls;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Web.UI.WebControls;
using Appleseed.Framework;
using System.Web.UI;


namespace Appleseed.DesktopModules.CoreModules.SignIn
{
    public partial class SignInPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Control myControl = GetCurrentProfileControl();
            signin.Controls.Add(myControl);

            if (this.Request.IsAuthenticated) {
                
                Response.Write("<script type=\"text/javascript\">window.parent.location = window.parent.location.href;</script>");
            }


        }

        public static Control GetCurrentProfileControl()
        {
            //default
            var controlStr = "~/DesktopModules/CoreModules/SignIn/Signin.ascx";

            PortalSettings portalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];

            //Select the actual login page
            if (portalSettings.CustomSettings.ContainsKey("SITESETTINGS_LOGIN_TYPE")) {
                controlStr = Convert.ToString(portalSettings.CustomSettings["SITESETTINGS_LOGIN_TYPE"]);
            }

            System.Web.UI.Page x = new System.Web.UI.Page();

            Control myControl = null;//x.LoadControl(SignInPage);

            try {
                myControl = x.LoadControl(controlStr);
            } catch (Exception exc) {
                ErrorHandler.Publish(LogLevel.Error, "The SignIn page from settings doesn't exists" ,exc);
                myControl = x.LoadControl("~/DesktopModules/CoreModules/SignIn/Signin.ascx");
            }
            // End Modification by gman3001

            PortalModuleControl p = ((PortalModuleControl)myControl);
            
            return ((Control)p);


            
        }
    }
}