using System;
using Appleseed.Framework;
using Appleseed.Framework.Web.UI.WebControls;

namespace Appleseed.Content.Web.Modules
{
    /// <summary>
    /// The SignInLink module shows "signin" and "register" links, as
    /// an alternative to the signin form. Written by Jes1111.
    /// </summary>
    public partial class SigninLink : PortalModuleControl
    {
        /// <summary>
        /// Handles the Click event of the SignInBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SignInBtn_Click(Object sender, EventArgs e)
        {
            Response.Redirect(HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Admin/LogOn.aspx"));
        }

        /// <summary>
        /// Handles the Click event of the RegisterBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect(HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Register/Register.aspx"));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SigninLink"/> class.
        /// </summary>
        public SigninLink()
        {
        }

        /// <summary>
        /// Overrides ModuleSetting to render this module type un-cacheable
        /// </summary>
        /// <value></value>
        public override bool Cacheable
        {
            get { return false; }
        }

        #region General Implementation

        /// <summary>
        /// GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{E2AE1D7E-E2FE-466f-A2F4-EB9465BC8966}"); }
        }

        #endregion

        #region Web Form Designer generated code

        /// <summary>
        /// On init
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.SignInBtn.Click += new EventHandler(this.SignInBtn_Click);
            this.RegisterBtn.Click += new EventHandler(this.RegisterBtn_Click);
            this.Load += new EventHandler(this.SigninLink_Load);
            base.OnInit(e);
        }

        #endregion

        /// <summary>
        /// Handles the Load event of the SigninLink control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SigninLink_Load(object sender, EventArgs e)
        {
            if (!bool.Parse(this.PortalSettings.CustomSettings["SITESETTINGS_ALLOW_NEW_REGISTRATION"].ToString()))
                RegisterBtn.Visible = false;
        }
    }
}