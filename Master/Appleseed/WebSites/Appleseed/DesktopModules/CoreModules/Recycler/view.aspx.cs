using System;
using System.Data;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Site.Data;
using Appleseed.Framework.Web.UI;
using Appleseed.Framework.Web.UI.WebControls;

namespace Appleseed
{
    /// <summary>
    /// Module print page
    /// </summary>
    public partial class recyclerViewPage : ViewItemPage
    {
        private int _moduleID;

        // TODO check if this works
        //protected ArrayList portalTabs;
        protected DataTable portalTabs;
        protected ModuleSettings module;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            this.DeleteButton.Visible = true;
            this.UpdateButton.Visible = false;

            try
            {
                _moduleID = int.Parse(Request.Params["mID"]);

                module = RecyclerDB.GetModuleSettingsForIndividualModule(_moduleID);
                if (RecyclerDB.ModuleIsInRecycler(_moduleID))
                {
                    if (!Page.IsPostBack)
                    {
                        //load tab names for the dropdown list, then bind them
                        // TODO check if this works
                        //portalTabs = new PagesDB().GetPagesFlat(portalSettings.PortalID);
                        portalTabs = new PagesDB().GetPagesFlatTable(this.PortalSettings.PortalID);

                        ddTabs.DataBind();

                        //on initial load, disable the restore button until they make a selection
                        restoreButton.Enabled = false;
                        ddTabs.Items.Insert(0, "--Choose a Page to Restore this Module--");
                    }

                    // create an instance of the module
                    PortalModuleControl myPortalModule =
                        (PortalModuleControl) LoadControl(Path.ApplicationRoot + "/" + this.module.DesktopSrc);
                    myPortalModule.PortalID = this.PortalSettings.PortalID;
                    myPortalModule.ModuleConfiguration = module;

                    // add the module to the placeholder
                    PrintPlaceHolder.Controls.Add(myPortalModule);
                }
                else
                    //they're trying to view a module that isn't in the recycler - maybe a manual manipulation of the url...?
                {
                    pnlMain.Visible = false;
                    pnlError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Handles OnDelete
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected override void OnDelete(EventArgs e)
        {
            base.OnDelete(e);

            ModulesDB modules = new ModulesDB();
            // TODO add userEmail and useRecycler
            modules.DeleteModule(_moduleID);

            _moduleID = 0;

            RedirectBackToReferringPage();
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises the Init event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.ddTabs.SelectedIndexChanged += new EventHandler(this.ddTabs_SelectedIndexChanged);
            this.restoreButton.Click += new EventHandler(this.restoreButton_Click);
            this.Load += new EventHandler(this.Page_Load);
            base.OnInit(e);
        }

        #endregion

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddTabs control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void ddTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddTabs.SelectedIndex == 0)
                this.restoreButton.Enabled = false;
            else
                this.restoreButton.Enabled = true;
        }

        /// <summary>
        /// Handles the Click event of the restoreButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected void restoreButton_Click(object sender, EventArgs e)
        {
            RecyclerDB.MoveModuleToNewTab(int.Parse(this.ddTabs.SelectedValue), this._moduleID);
            this.RedirectBackToReferringPage();
        }
    }
}