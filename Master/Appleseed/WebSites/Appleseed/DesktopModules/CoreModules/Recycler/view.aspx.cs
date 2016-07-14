using System;
using System.Data;
using System.Linq;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Site.Data;
using Appleseed.Framework.Web.UI;
using Appleseed.Framework.Web.UI.WebControls;
using System.Web.UI.WebControls;

namespace Appleseed
{
    /// <summary>
    /// Module print page
    /// </summary>
    public partial class recyclerViewPage : ViewItemPage
    {
        private int _moduleID;
        private int _pageID;
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
                int.TryParse(Request.Params["mID"], out this._moduleID);
                if (_moduleID > 0)
                {
                    module = RecyclerDB.GetModuleSettingsForIndividualModule(_moduleID);
                    if (RecyclerDB.ModuleIsInRecycler(_moduleID))
                    {
                        if (!Page.IsPostBack)
                        {
                            LoadPageDropDown();
                        }

                        // create an instance of the module
                        PortalModuleControl myPortalModule =
                            (PortalModuleControl)LoadControl(Path.ApplicationRoot + "/" + this.module.DesktopSrc);
                        myPortalModule.PortalID = this.PortalSettings.PortalID;
                        myPortalModule.ModuleConfiguration = module;
                        selecteditem.InnerHtml = "Selected Module : " + module.ModuleTitle;
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
                int.TryParse(Request.Params["pID"], out _pageID);
                if (_pageID > 0)
                {
                    if (!Page.IsPostBack)
                    {
                        LoadPageDropDown();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void LoadPageDropDown()
        {
            //load tab names for the dropdown list, then bind them
            // TODO check if this works
            //portalTabs = new PagesDB().GetPagesFlat(portalSettings.PortalID);
            var pgDb = new PagesDB();
            portalTabs = pgDb.GetPagesFlatTable(this.PortalSettings.PortalID);
            ddTabs.DataBind();

            //on initial load, disable the restore button until they make a selection
            restoreButton.Enabled = false;
            if (this._moduleID > 0)
            {
                ddTabs.Items.Insert(0, "--Choose a Page to Restore this Module--");
            }

            if (this._pageID > 0)
            {
                restoreButton.Enabled = true;
                var recyle = pgDb.GetPagesFlat(-1).First(p => p.ID == this._pageID);// get recycle pages
                selecteditem.InnerHtml = "Selected Page : " + recyle.Name.Replace("(Orphan)", string.Empty);
                foreach (DataRow row in portalTabs.Rows)
                {
                    if (recyle.ParentPageId.ToString() == row["PageID"].ToString())
                    {
                        ddTabs.SelectedValue = row["PageID"].ToString();
                        break;
                    }
                }
                ddTabs.Items.Insert(0, new ListItem() { Text = "--Root--", Value = "-1" });
            }
        }

        /// <summary>
        /// Handles OnDelete
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected override void OnDelete(EventArgs e)
        {
            base.OnDelete(e);
            if (this._moduleID > 0)
            {
                ModulesDB modules = new ModulesDB();
                // TODO add userEmail and useRecycler
                modules.DeleteModule(_moduleID);

                _moduleID = 0;
            }

            if (this._pageID > 0)
            {
                var tabs = new PagesDB();
                tabs.DeletePage(this._pageID);
            }

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
            if (ddTabs.SelectedIndex == 0 && this._pageID == 0)
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
            if (this._moduleID > 0)
            {
                RecyclerDB.MoveModuleToNewTab(int.Parse(this.ddTabs.SelectedValue), this._moduleID);
            }

            if (this._pageID > 0)
            {
                RecyclerDB.RestorePage(this._pageID, int.Parse(this.ddTabs.SelectedValue));
            }
            this.RedirectBackToReferringPage();
        }
    }
}