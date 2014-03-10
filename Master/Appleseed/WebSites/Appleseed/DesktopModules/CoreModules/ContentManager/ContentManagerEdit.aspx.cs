using System;
using Appleseed.Framework.Web.UI;

namespace Appleseed.Content.Web.Modules
{
    /// <summary>
    /// Add/Remove modules, assign modules to portals
    /// </summary>
    public partial class ContentManagerEdit : Page
    {
        /// <summary>
        /// The Page_Load server event handler on this page is used
        /// to populate the role information for the page
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            // Verify that the current user has access to access this page
            // Removed by Mario Endara <mario@softworks.com.uy> (2004/11/13)
            // if (PortalSecurity.IsInRoles("Admins") == false)
            //	PortalSecurity.AccessDeniedEdit();

            // If this is the first visit to the page, bind the definition data
            if (Page.IsPostBack == false)
            {
                InstallerFileName.Text = "DesktopModules/ContentManager/InstallFiles/";
            }
        }

        /*
		/// <summary>
		/// OnUpdate installs or refresh module definiton on db
		/// </summary>
		/// <param name="e"></param>
		protected override void OnUpdate(EventArgs e)
		{
			if (Page.IsValid)
			{
				try
				{
				    Install Module Here.
					// Redirect back to the portal admin page
					RedirectBackToReferringPage();
				}
				catch(Exception ex)
				{
					lblInvalidModule.Visible = true;
					lblErrorDetail.Text = string.Empty;
					while (ex != null)
					{
						lblErrorDetail.Text += ex.Message + "<br />";
						Appleseed.Framework.Helpers.LogHelper.Log.Error("Installing: ", ex);
						ex = ex.InnerException;
					}
				}
			}
		}


		/// <summary>
		/// Delete a Module definition
		/// </summary>
		/// <param name="e"></param>
		protected override void OnDelete(EventArgs e)
		{
			try
			{
				if (!btnUseInstaller.Visible)
					Appleseed.Framework.Helpers.ModuleInstall.UninstallGroup(Server.MapPath(PortalSettings.ApplicationPath + "/" + InstallerFileName.Text));
				else
					Appleseed.Framework.Helpers.ModuleInstall.Uninstall(DesktopSrc.Text, MobileSrc.Text);

				// Redirect back to the portal admin page
				RedirectBackToReferringPage();
			}
			catch(Exception ex)
			{
				Appleseed.Framework.Helpers.LogHelper.Log.Error("Error deleting module", ex);

				lblInvalidModule.Visible = true;
				lblErrorDetail.Text = string.Empty;
				while (ex != null)
				{
					lblErrorDetail.Text += ex.Message + "<br />" ;
					ex = ex.InnerException;
				}
			}
		}
        */

        #region Web Form Designer generated code

        /// <summary>
        /// OnInit
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            base.OnInit(e);
        }

        #endregion
    }
}