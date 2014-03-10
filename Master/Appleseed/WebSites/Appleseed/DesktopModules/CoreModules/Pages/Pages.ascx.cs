// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Pages.ascx.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The pages.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.DesktopModules.CoreModules.Pages
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Appleseed.Framework;
    using Appleseed.Framework.Data;
    using Appleseed.Framework.Providers.AppleseedSiteMapProvider;
    using Appleseed.Framework.Settings.Cache;
    using Appleseed.Framework.Site.Data;
    using Appleseed.Framework.Web.UI.WebControls;

    using ImageButton = Appleseed.Framework.Web.UI.WebControls.ImageButton;

    /// <summary>
    /// The pages.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class Pages : PortalModuleControl
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Pages"/> class. 
        ///   This is where you add module settings. These settings
        ///   are used to control the behavior of the module
        /// </summary>
        /// <remarks>
        /// </remarks>
        public Pages()
        {
            // EHN: Add new version control for tabs module. 
            // Mike Stone - 19/12/2004
            var pageVersion = new SettingItem<bool, CheckBox>
                {
                    Value = true,
                    EnglishName = "Use Old Version?",
                    Description =
                        "If Checked the module acts has it always did. If not it uses the new short form which allows security to be set so the new tab will not be seen by all users.",
                    Order = 10
                };
            this.BaseSettings.Add("TAB_VERSION", pageVersion);
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Admin Module
        /// </summary>
        /// <value><c>true</c> if [admin module]; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// </remarks>
        public override bool AdminModule
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets GUID of module (mandatory)
        /// </summary>
        /// <remarks></remarks>
        public override Guid GuidID
        {
            get
            {
                return new Guid("{1C575D94-70FC-4A83-80C3-2087F726CBB3}");
            }
        }

        /// <summary>
        ///   Gets or sets the portal pages.
        /// </summary>
        /// <value>The portal pages.</value>
        /// <remarks>
        /// </remarks>
        protected List<PageItem> PortalPages { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Installs the specified state saver.
        /// </summary>
        /// <param name="stateSaver">The state saver.</param>
        /// <remarks></remarks>
        public override void Install(IDictionary stateSaver)
        {
            var currentScriptName = this.Server.MapPath(this.TemplateSourceDirectory + "/Install.sql");
            var errors = DBHelper.ExecuteScript(currentScriptName, true);
            if (errors.Count > 0)
            {
                // Call rollback
                throw new Exception("Error occurred:" + errors[0]);
            }
        }

        /// <summary>
        /// Uninstalls the specified state saver.
        /// </summary>
        /// <param name="stateSaver">The state saver.</param>
        /// <remarks></remarks>
        public override void Uninstall(IDictionary stateSaver)
        {
            // Cannot be uninstalled
            throw new Exception("This is an essential module that can be uninstalled");
        }

        #endregion

        #region Methods

        /// <summary>
        /// The AddPage_Click server event handler is used
        ///   to add a new tab for this portal
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected void AddPageClick(object sender, EventArgs e)
        {
            if (this.Settings["TAB_VERSION"] == null)
            {
                return;
            }

            if (this.Settings["TAB_VERSION"].ToString().ToLowerInvariant() == "true")
            {
                // Use Old Version
                // New tabs go to the end of the list
                var t = new PageItem
                    {
                        // Just in case it comes to be empty
                        Name = General.GetString("TAB_NAME", "New Page Name"),
                        ID = -1,
                        Order = 990000
                    };
                this.PortalPages.Add(t);

                // write tab to database
                var tabs = new PagesDB();
                t.ID = tabs.AddPage(this.PortalSettings.PortalID, t.Name, t.Order);

                // Reset the order numbers for the tabs within the list  
                this.OrderPages();

                // Clear SiteMaps Cache
                AppleseedSiteMapProvider.ClearAllAppleseedSiteMapCaches();

                // Redirect to edit page
                // 3_aug_2004 Cory Isakson added returntabid so that PageLayout could return to the tab it was called from.
                // added mID by Mario Endara <mario@softworks.com.uy> to support security check (2004/11/09)
                this.Response.Redirect(
                    HttpUrlBuilder.BuildUrl(
                        "~/DesktopModules/CoreModules/Pages/PageLayout.aspx",
                        t.ID,
                        "mID=" + this.ModuleID + "&returntabid=" + this.Page.PageID));
            }
            else
            {
                // Redirect to New Form - Mike Stone 19/12/2004
                this.Response.Redirect(
                    HttpUrlBuilder.BuildUrl(
                        "~/DesktopModules/CoreModules/Pages/AddPage.aspx",
                        "mID=" + this.ModuleID + "&returntabid=" + this.Page.PageID));
            }
        }

        /// <summary>
        /// The DeleteBtn_Click server event handler is used to delete
        ///   the selected tab from the portal
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Web.UI.ImageClickEventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        //protected void DeleteBtnClick(object sender, ImageClickEventArgs e)
        //{
        //    if (this.tabList.SelectedIndex > -1)
        //    {
        //        try
        //        {
        //            // must delete from database too
        //            var t = this.PortalPages[this.tabList.SelectedIndex];
        //            var tabs = new PagesDB();
        //            tabs.DeletePage(t.ID);

        //            this.PortalPages.RemoveAt(this.tabList.SelectedIndex);

        //            if (this.tabList.SelectedIndex > 0)
        //            {
        //                t = this.PortalPages[this.tabList.SelectedIndex - 1];
        //            }

        //            this.OrderPages();
        //            this.Response.Redirect(
        //                HttpUrlBuilder.BuildUrl("~/" + HttpUrlBuilder.DefaultPage, this.PageID, "SelectedPageID=" + t.ID));
        //        }
        //        catch (SqlException)
        //        {
        //            this.Controls.Add(
        //                new LiteralControl(
        //                    "<br><span class='Error'>" +
        //                    General.GetString("TAB_DELETE_FAILED", "Failed to delete Page", this) + "<br>"));
        //        }
        //    }
        //}

        /// <summary>
        /// Handles the Click event of the EditBtn control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Web.UI.ImageClickEventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        //protected void EditBtnClick(object sender, ImageClickEventArgs e)
        //{
        //    // Redirect to edit page of currently selected tab
        //    if (this.tabList.SelectedIndex > -1)
        //    {
        //        // Redirect to module settings page
        //        var t = this.PortalPages[this.tabList.SelectedIndex];

        //        // added mID by Mario Endara <mario@softworks.com.uy> to support security check (2004/11/09)
        //        this.Response.Redirect(
        //            string.Concat(
        //                "~/DesktopModules/CoreModules/Pages/PageLayout.aspx?PageID=",
        //                t.ID,
        //                "&mID=",
        //                this.ModuleID.ToString(),
        //                "&Alias=",
        //                this.PortalSettings.PortalAlias,
        //                "&returntabid=",
        //                this.Page.PageID));
        //    }
        //}

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> object that contains the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        //    // Set the ImageUrl for controls from current Theme
        //    this.upBtn.ImageUrl = this.CurrentTheme.GetImage("Buttons_Up", "Up.gif").ImageUrl;
        //    this.downBtn.ImageUrl = this.CurrentTheme.GetImage("Buttons_Down", "Down.gif").ImageUrl;
        //    this.DeleteBtn.ImageUrl = this.CurrentTheme.GetImage("Buttons_Delete", "Delete.gif").ImageUrl;
        //    this.EditBtn.ImageUrl = this.CurrentTheme.GetImage("Buttons_Edit", "Edit.gif").ImageUrl;

        //    // If this is the first visit to the page, bind the tab data to the page listbox
        //    this.PortalPages = new PagesDB().GetPagesFlat(this.PortalSettings.PortalID);
        //    if (!this.Page.IsPostBack)
        //    {
        //        this.tabList.DataSource = this.PortalPages;
        //        this.tabList.DataBind();

        //        // 2/27/2003 Start - Ender Malkoc
        //        // After up or down button when the page is refreshed, 
        //        // select the previously selected tab from the list.
        //        if (this.Request.Params["selectedtabID"] != null)
        //        {
        //            int tabIndex;
        //            if (Int32.TryParse(this.Request.Params["selectedtabID"], out tabIndex))
        //            {
        //                this.SelectPage(tabIndex);
        //            }
        //        }

        //        // 2/27/2003 End - Ender Malkoc
        //    }
            TreeRoute.Value = this.getTreeRoute();
            //TreeRoute.Text = this.getTreeRoute();

        }

        /// <summary>
        /// The UpDown_Click server event handler on this page is
        ///   used to move a portal module up or down on a tab's layout pane
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="T:System.Web.UI.ImageClickEventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        //protected void UpDownClick(object sender, ImageClickEventArgs e)
        //{
        //    var cmd = ((ImageButton)sender).CommandName;

        //    if (this.tabList.SelectedIndex > -1)
        //    {
        //        int delta;

        //        // Determine the delta to apply in the order number for the module
        //        // within the list.  +3 moves down one item; -3 moves up one item
        //        if (cmd == "down")
        //        {
        //            delta = 3;
        //        }
        //        else
        //        {
        //            delta = -3;
        //        }

        //        var t = this.PortalPages[this.tabList.SelectedIndex];
        //        t.Order += delta;
        //        this.OrderPages();
        //        this.Response.Redirect(
        //            HttpUrlBuilder.BuildUrl("~/" + HttpUrlBuilder.DefaultPage, this.PageID, "selectedtabID=" + t.ID));
        //    }
        //}

        /// <summary>
        /// The OrderPages helper method is used to reset
        ///   the display order for tabs within the portal
        /// </summary>
        /// <remarks>
        /// </remarks>
        private void OrderPages()
        {
            var i = 1;

            this.PortalPages.Sort();

            foreach (var t in this.PortalPages)
            {
                // number the items 1, 3, 5, etc. to provide an empty order
                // number when moving items up and down in the list.
                t.Order = i;
                i += 2;

                // rewrite tab to database
                var tabs = new PagesDB();

                // 12/16/2002 Start - Cory Isakson 
                tabs.UpdatePageOrder(t.ID, t.Order);

                // 12/16/2002 End - Cory Isakson 
            }

            // gbs: Invalidate cache, fix for bug RBM-220
            CurrentCache.RemoveAll("_PageNavigationSettings_");
        }

        /// <summary>
        /// Given the tabID of a tab, this function selects the right tab in the provided list control
        /// </summary>
        /// <param name="tabID">
        /// tabID of the tab that needs to be selected
        /// </param>
        /// <remarks>
        /// </remarks>
        //private void SelectPage(int tabID)
        //{
        //    for (var i = 0; i < this.tabList.Items.Count; i++)
        //    {
        //        if (this.PortalPages[i].ID == tabID)
        //        {
        //            if (this.tabList.SelectedItem != null)
        //            {
        //                this.tabList.SelectedItem.Selected = false;
        //            }

        //            this.tabList.Items[i].Selected = true;
        //            return;
        //        }
        //    }

        //    return;
        //}

        #endregion

        public string getTreeRoute() {

            return HttpUrlBuilder.BuildUrl("~/PageManagerTree/PageManagerTree/Module");

        }
    }
}