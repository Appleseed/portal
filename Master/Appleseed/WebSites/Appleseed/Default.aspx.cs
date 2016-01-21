using System;
using System.Web.UI;
using System.Web.Security;
using Appleseed.Framework.Security;
using System.Web;


namespace Appleseed
{
    /// <summary>
    /// The Default.aspx page simply tests 
    /// the browser type and redirects either to
    /// the DesktopDefault or MobileDefault pages, 
    /// depending on the device type.
    /// </summary>
    public partial class Default : Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            
            string user = Request.Params["u"];
            string pass = Request.Params["p"];
            
            string lnkid = Request.QueryString["lnkid"];
            if (!string.IsNullOrEmpty(lnkid))
            {
                int pageId = 0;
                if (int.TryParse(lnkid, out pageId))
                {
                    this.Response.Redirect(Appleseed.Framework.HttpUrlBuilder.BuildUrl(pageId),true);
                }
            }

            if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pass)) {
                bool rem = (Request.Params["rem"] ?? "0").ToString().Equals("1") ? true : false;
                PortalSecurity.SignOn(user, pass, rem, "~/DesktopDefault.aspx");
                //PortalSecurity.SignOn(user, pass, false, "~/DesktopDefault.aspx");
            }
            Server.Transfer("DesktopDefault.aspx", true);
            
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises OnInitEvent
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