using System;
using System.Web;
using System.Web.UI;
using Appleseed.Framework;
using Appleseed.Framework.Security;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Web.UI.WebControls;
using History=Appleseed.Framework.History;
using Page=Appleseed.Framework.Web.UI.Page;

namespace Appleseed.Admin
{
    using System.Web.UI.WebControls;

    /// <summary>
    /// Summary description for Register.
    /// </summary>    
    public partial class Register : Page
    {
        protected IEditUserProfile EditControl;

        /// <summary>
        /// Gets a value indicating whether [edit mode].
        /// </summary>
        /// <value><c>true</c> if [edit mode]; otherwise, <c>false</c>.</value>
        public bool EditMode
        {
            get { return (userName.Length != 0); }
        }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        private string userName
        {
            get
            {
                string uid = string.Empty;

                if (Request.Path.Contains("/Users/") && Request.Params["userName"] == null) {
                    return uid;
                }

                if (Request.Params["userName"] != null) {
                    uid = Request.Params["userName"];
                }

                if (uid.Length == 0 && PortalSettings.CurrentUser != null && PortalSettings.CurrentUser.Identity != null)
                    uid = PortalSettings.CurrentUser.Identity.UserName;

                if (uid.Length == 0 && HttpContext.Current.Items["userName"] != null) {
                    uid = HttpContext.Current.Items["userName"].ToString();
                }
                return uid;

            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            if (!EditMode &&
                !bool.Parse(this.PortalSettings.CustomSettings["SITESETTINGS_ALLOW_NEW_REGISTRATION"].ToString()))
                PortalSecurity.AccessDeniedEdit();

            Control myControl = GetCurrentProfileControl();

            EditControl = ((IEditUserProfile)myControl);
            EditControl.RedirectPage = HttpUrlBuilder.BuildUrl(PageID);

            register.Controls.Add(myControl);
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises the Init event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            base.OnInit(e);
        }

        #endregion

        /// <summary>
        /// Gets the current profile control.
        /// </summary>
        /// <returns></returns>
        public static Control GetCurrentProfileControl()
        {
            //default
            string RegisterPage = "Register.ascx";

            // 19/08/2004 Jonathan Fong 
            // www.gt.com.au
            AppleseedPrincipal user = HttpContext.Current.User as AppleseedPrincipal;

            PortalSettings portalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];

            //Select the actual register page
            if (portalSettings.CustomSettings["SITESETTINGS_REGISTER_TYPE"] != null &&
                portalSettings.CustomSettings["SITESETTINGS_REGISTER_TYPE"].ToString() != "Register.ascx") {
                RegisterPage = portalSettings.CustomSettings["SITESETTINGS_REGISTER_TYPE"].ToString();
            }
            System.Web.UI.Page x = new System.Web.UI.Page();

            // Modified by gman3001 10/06/2004, to support proper loading of a register module specified by 'Register Module ID' setting in the Portal Settings admin page
            int moduleID = int.Parse(portalSettings.CustomSettings["SITESETTINGS_REGISTER_MODULEID"].ToString());
            string moduleDesktopSrc = string.Empty;
            if (moduleID > 0)
                moduleDesktopSrc = Framework.Site.Configuration.ModuleSettings.GetModuleDesktopSrc(moduleID);
            if (moduleDesktopSrc.Length == 0)
                moduleDesktopSrc = RegisterPage;

            Control myControl = x.LoadControl(moduleDesktopSrc);
            // End Modification by gman3001

            PortalModuleControl p = ((PortalModuleControl)myControl);
            //p.ModuleID = int.Parse(portalSettings.CustomSettings["SITESETTINGS_REGISTER_MODULEID"].ToString());
            p.ModuleID = moduleID;
            if (p.ModuleID == 0)
            {
                ((SettingItem<bool, CheckBox>)p.Settings["MODULESETTINGS_SHOW_TITLE"]).Value = false;
            }
            return ((Control)p);
        }
    }
}