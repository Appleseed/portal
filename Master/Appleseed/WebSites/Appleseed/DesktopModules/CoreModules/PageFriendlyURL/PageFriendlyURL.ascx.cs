// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageFriendlyURL.cs">
//   Copyright © -- 2014. All Rights Reserved.
// </copyright>
// <summary>
//   Page Friendly URL
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Appleseed.DesktopModules.CoreModules.PageFriendlyURL
{
    using Appleseed.Framework;
    using Appleseed.Framework.Site.Data;
    using Appleseed.Framework.Web;
    using Appleseed.Framework.Web.UI.WebControls;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Page Friendly URL 
    /// </summary>
    [History("Ashish.patel@haptix.biz", "2014/12/16", "Add/Update Page Friendly URL")]
    [History("Ashish.patel@haptix.biz", "2014/12/24", "Added code for clearing elements on save")]
    public partial class PageFriendlyURL : PortalModuleControl
    {
        /// <summary>
        /// GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{C1EA4115-E7F2-4CBC-B1E7-DDA46791493C}"); }
        }

        List<PageItem> pages;
        /// <summary>
        /// Set pages to dropdown list
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            PagesDB pgdb = new PagesDB();
            // Get all pages
            pages = pgdb.GetPagesFlat(this.PortalSettings.PortalID);

            if (!IsPostBack)
            {
                drpPageList.DataTextField = "Name";
                drpPageList.DataValueField = "ID";

                //Set it dropdown list
                drpPageList.DataSource = pages;
                drpPageList.DataBind();

                divDyanamicPage.Visible = false;

                drpPageList.Items.Add(new ListItem() { Text = "Dynamic Page", Value = "-1" });

                //div for messages will be false when page is loaded
                divErrorMessage.Visible = false;
                divSuccessMessage.Visible = false;

                // Set the page extension from web.config file to display
                lblFriendlyExtension.Text = System.Configuration.ConfigurationManager.AppSettings["FriendlyUrlExtension"].ToString();

                LoadGrid();
            }
        }

        /// <summary>
        /// Load grid
        /// </summary>
        private void LoadGrid()
        {
            PagesDB pgdb = new PagesDB();

            // Load pages into grid 
            this.gdPages.DataSource = pgdb.GetFriendlyURlPages();
            this.gdPages.DataBind();

            //Load Dynamic Pages into Grid
            this.gdDynamicPages.DataSource = pgdb.GetFriendlyURlFromDynamicPages();
            this.gdDynamicPages.DataBind();
        }

        /// <summary>
        /// Update friendly URL 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// Ashish.patel@haptix.biz - 2014/12/24 - Added code for clearing page url and url elements on save"
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int pageId = Convert.ToInt32(drpPageList.SelectedValue);
            this.AddUpdateFriendlyUrl(pageId, txtFriendlyURL.Text, 0);
            LoadGrid();
        }

        /// <summary>
        /// Add Update friendly url
        /// </summary>
        /// <param name="pageId">page id</param>
        /// <param name="friendlyurl">friendly url</param>
        private void AddUpdateFriendlyUrl(int pageId, string friendlyurl, int dynamicPageId)
        {
            PagesDB pages = new PagesDB();
            string result = string.Empty;
            if (drpPageList.SelectedValue == "-1")
            {
                result = pages.CreateFriendlyURL(txtDyanmicPage.Text, (txtFriendlyURL.Text.StartsWith("/") ? txtFriendlyURL.Text : "/" + txtFriendlyURL.Text) + lblFriendlyExtension.Text, dynamicPageId);
            }
            else
            {
                //when friendlyURL saved, Set result as (0/1) 
                result = pages.UpdateFriendlyURL(pageId, friendlyurl);
            }

            //If result get 0 then error message will display
            divErrorMessage.Visible = (result == "0");

            //If result get 1 then success message will display
            divSuccessMessage.Visible = (result != "0");

            //remove from cache
            SqlUrlBuilderProvider.ClearCachePageUrl(pageId);
            UrlBuilderHelper.ClearUrlElements(pageId);
        }

        /// <summary>
        /// Get the friendlyURL by selecting the page
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void drpPageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            divErrorMessage.Visible = false;
            divSuccessMessage.Visible = false;

            divDyanamicPage.Visible = (drpPageList.SelectedValue == "-1");

            if (drpPageList.SelectedValue != "-1")
            {
                PagesDB pages = new PagesDB();
                // Get and Set friendlyURL from db to Textbox when change the dropdown value
                txtFriendlyURL.Text = pages.GetFriendlyURl(Convert.ToInt32(drpPageList.SelectedValue));
            }
        }

        /// <summary>
        /// Row Editing
        /// </summary>
        /// <param name="sender"> The source of event.</param>
        /// <param name="e"> The <see cref="System.Web.UI.WebControls.GridViewEditEventArgs"/> instance containing the event data.</param>
        protected void GdPages_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gdPages.EditIndex = e.NewEditIndex;
            LoadGrid();
        }

        /// <summary>
        /// Row Deleteting It will clear friendly url
        /// </summary>
        /// <param name="sender"> The source of event.</param>
        /// <param name="e"> The <see cref="System.Web.UI.WebControls.GridViewDeleteEventArgs"/> instance containing the event data.</param>
        protected void gdPages_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = (GridViewRow)gdPages.Rows[e.RowIndex];
            System.Web.UI.WebControls.Label lblPageID = (System.Web.UI.WebControls.Label)row.FindControl("lblPageID");
            int pageID = Convert.ToInt32(lblPageID.Text);

            new PagesDB().DeleteFriendlyUrl(pageID);

            //remove from cache
            SqlUrlBuilderProvider.ClearCachePageUrl(pageID);
            UrlBuilderHelper.ClearUrlElements(pageID);

            this.LoadGrid();
        }

        /// <summary>
        /// Cancel edit
        /// </summary>
        /// <param name="sender">The source of event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewCancelEditEventArgs"/> instance containing the event data.</param>
        protected void gdPages_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.gdPages.EditIndex = -1;
            divErrorMessage.Visible = false;
            divSuccessMessage.Visible = false;
            this.LoadGrid();
        }

        /// <summary>
        /// Update friendly url data
        /// </summary>
        /// <param name="sender">The source of event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewUpdateEventArgs"/> instance containing the event data.</param>
        protected void gdPages_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = (GridViewRow)gdPages.Rows[e.RowIndex];
            TextBox txtFriendlyUrl = (TextBox)row.FindControl("txtFriendlyUrl");
            System.Web.UI.WebControls.Label lblPageID = (System.Web.UI.WebControls.Label)row.FindControl("lblPageID");
            int pageID = Convert.ToInt32(lblPageID.Text);
            AddUpdateFriendlyUrl(pageID, txtFriendlyUrl.Text.ToLower(), 0);
            this.gdPages.EditIndex = -1;
            this.LoadGrid();
        }
        private string FullPageName = "";
        private void AppendFullPageName(int PageID)
        {
            if (PageID > 0)
            {
                var page = pages.FirstOrDefault(pg => pg.ID == PageID);
                if (page != null)
                {
                    FullPageName = string.IsNullOrEmpty(FullPageName) ? page.Name.TrimStart('-') : page.Name.TrimStart('-') + "/" + FullPageName;
                    if (page.ParentPageId > 0)
                        AppendFullPageName(page.ParentPageId);
                }
            }
        }

        /// <summary>
        /// Row bound 
        /// </summary>
        /// <param name="sender">The source of event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gdPages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Data.DataRowView drv = (System.Data.DataRowView)e.Row.DataItem;

                FullPageName = string.Empty;
                AppendFullPageName(Convert.ToInt32(((System.Web.UI.WebControls.Label)e.Row.FindControl("lblPageID")).Text));
                ((System.Web.UI.WebControls.Label)e.Row.FindControl("lblPageFullName")).Text = FullPageName;

                if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
                {
                    System.Web.UI.WebControls.Label lblPageFriendlyUrl = ((System.Web.UI.WebControls.Label)e.Row.FindControl("lblPageFriendlyUrl"));
                    lblPageFriendlyUrl.Text = (drv["FriendlyUrl"].ToString().StartsWith("/") ? drv["FriendlyUrl"].ToString() : "/" + drv["FriendlyUrl"].ToString()) + lblFriendlyExtension.Text;
                }

            }
        }

        /// <summary>
        /// Row bound 
        /// </summary>
        /// <param name="sender">The source of event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gdDynamicPages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Data.DataRowView drv = (System.Data.DataRowView)e.Row.DataItem;

                if (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate)
                {
                    System.Web.UI.WebControls.Label lblPageFriendlyUrl = ((System.Web.UI.WebControls.Label)e.Row.FindControl("lblPageFriendlyUrl"));
                    lblPageFriendlyUrl.Text = drv["FriendlyUrl"].ToString(); // (drv["FriendlyUrl"].ToString().StartsWith("/") ? drv["FriendlyUrl"].ToString() : "/" + drv["FriendlyUrl"].ToString()) + lblFriendlyExtension.Text;
                }

            }
        }

        /// <summary>
        /// Row Deleteting It will clear friendly url
        /// </summary>
        /// <param name="sender"> The source of event.</param>
        /// <param name="e"> The <see cref="System.Web.UI.WebControls.GridViewDeleteEventArgs"/> instance containing the event data.</param>
        protected void gdDynamicPages_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = (GridViewRow)gdDynamicPages.Rows[e.RowIndex];
            System.Web.UI.WebControls.Label lblPageID = (System.Web.UI.WebControls.Label)row.FindControl("lblPageID");
            int pageID = Convert.ToInt32(lblPageID.Text);

            new PagesDB().DeleteDynamicFriendlyUrl(pageID);

            //remove from cache
            SqlUrlBuilderProvider.ClearCachePageUrl(pageID);
            UrlBuilderHelper.ClearUrlElements(pageID);

            this.LoadGrid();
        }

        /// <summary>
        /// Row Editing
        /// </summary>
        /// <param name="sender"> The source of event.</param>
        /// <param name="e"> The <see cref="System.Web.UI.WebControls.GridViewEditEventArgs"/> instance containing the event data.</param>
        protected void GdDynamicPages_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gdDynamicPages.EditIndex = e.NewEditIndex;
            LoadGrid();
        }

        /// <summary>
        /// Cancel edit
        /// </summary>
        /// <param name="sender">The source of event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewCancelEditEventArgs"/> instance containing the event data.</param>
        protected void gdDynamicPages_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.gdDynamicPages.EditIndex = -1;
            divErrorMessage.Visible = false;
            divSuccessMessage.Visible = false;
            this.LoadGrid();
        }

        /// <summary>
        /// Update friendly url data
        /// </summary>
        /// <param name="sender">The source of event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewUpdateEventArgs"/> instance containing the event data.</param>
        protected void gdDynamicPages_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = (GridViewRow)gdDynamicPages.Rows[e.RowIndex];
            TextBox txtFriendlyUrl = (TextBox)row.FindControl("txtFriendlyUrl");
            TextBox txtRedirectToUrl = (TextBox)row.FindControl("txtRedirectToUrl");
            System.Web.UI.WebControls.Label lblPageID = (System.Web.UI.WebControls.Label)row.FindControl("lblPageID");
            int pageID = Convert.ToInt32(lblPageID.Text);
            AddUpdateDaynamicPagesFriendlyUrl(pageID, txtRedirectToUrl.Text, txtFriendlyUrl.Text);
            this.gdDynamicPages.EditIndex = -1;
            this.LoadGrid();
        }

        /// <summary>
        /// Add Update friendly url
        /// </summary>
        /// <param name="daynamicPageId">page id</param>
        /// <param name="friendlyurl">friendly url</param>
        private void AddUpdateDaynamicPagesFriendlyUrl(int daynamicPageId, string redirectToUrl, string friendlyurl)
        {
            PagesDB pages = new PagesDB();

            //when friendlyURL saved, Set result as (0/1) 
            //change this method for rb_Pages_DynamicRedirects Table.
            string result = pages.UpdateFriendlyURL(redirectToUrl, friendlyurl, daynamicPageId);

            //If result get 0 then error message will display
            divErrorMessage.Visible = (result == "0");

            //If result get 1 then success message will display
            divSuccessMessage.Visible = (result != "0");

            //remove from cache
            SqlUrlBuilderProvider.ClearCachePageUrl(daynamicPageId);
            UrlBuilderHelper.ClearUrlElements(daynamicPageId);
        }
    }
}