using System;
using System.Collections;
using System.Security.Principal;
using System.Web;
using System.Web.UI.WebControls;
using Appleseed.Framework;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Settings.Cache;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Site.Data;
using Appleseed.Framework.Users.Data;
using Appleseed.Framework.Web.UI;
using System.Collections.Generic;
using Appleseed.Framework.Providers.AppleseedRoleProvider;
using Appleseed.Framework.Providers.AppleseedSiteMapProvider;

namespace Appleseed.Admin
{
    public partial class AddPage : EditItemPage
    {

        #region Page_Load

        /// <summary>
        /// The Page_Load server event handler on this page is used
        /// to populate a tab's layout settings on the page
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            // If first visit to the page, update all entries
            if (!Page.IsPostBack)
            {
                msgError.Visible = false;
                BindData();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// The cancelButton_Click is used to return to the
        /// previous page if present
        /// Created by Mike Stone 30/12/2004
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            string returnPage =
                HttpUrlBuilder.BuildUrl("~/DesktopDefault.aspx", int.Parse(Request.QueryString["returntabid"]));
            Response.Redirect(returnPage);
        }


        /// <summary>
        /// The SaveButton_Click is used to commit the tab/page
        /// information from the form to the database.
        /// Created by Mike Stone 29/12/2004
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            //Only Save if Input Data is Valid
            int NewPageID = 0;
            string returnPage;

            if (Page.IsValid == true)
            {
                try
                {
                    NewPageID = SavePageData();

                    // Flush all tab navigation cache keys. Very important for recovery the changes
                    // made in all languages and not get a error if user change the tab parent.
                    // jviladiu@portalServices.net (05/10/2004)
                    // Copied to here 29/12/2004 by Mike Stone
                    CurrentCache.RemoveAll("_PageNavigationSettings_");

                    //Jump to Page option
                    if (cb_JumpToPage.Checked == true)
                    {
                        // Redirect to New Form - Mike Stone 19/12/2004
                        returnPage =
                            HttpUrlBuilder.BuildUrl("~/" + HttpUrlBuilder.DefaultPage, NewPageID,
                                                    "SelectedPageID=" + NewPageID.ToString());
                    }
                    else
                    {
                        // Do NOT Redirect to New Form - Mike Stone 19/12/2004
                        // I guess every .aspx page needs to have a module tied to it. 
                        // or you will get an error about edit access denied. 
                        // Fix: RBP-594 by mike stone added returntabid to url.
                        returnPage =
                            HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Pages/AddPage.aspx",
                                                    "mID=" + Request.QueryString["mID"] + "&returntabid=" +
                                                    Request.QueryString["returntabid"]);
                    }
                    Response.Redirect(returnPage);
                }
                catch
                {
                    lblErrorNotAllowed.Visible = true;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The SavePageData helper method is used to persist the
        /// current tab settings to the database.
        /// </summary>
        /// <returns></returns>
        private int SavePageData()
        {
            // Construct Authorized User Roles string
            string authorizedRoles = "";

            foreach ( ListItem item in authRoles.Items ) {
                if ( item.Selected == true ) {
                    authorizedRoles = authorizedRoles + item.Text + ";";
                }
            }

            // Add Page info in the database
            int NewPageID =
                new PagesDB().AddPage(this.PortalSettings.PortalID, Int32.Parse(parentPage.SelectedItem.Value), tabName.Text,
                                      990000, authorizedRoles, showMobile.Checked, mobilePageName.Text);
            //Clear SiteMaps Cache
            AppleseedSiteMapProvider.ClearAllAppleseedSiteMapCaches();

            // Update custom settings in the database
            EditTable.UpdateControls();
            return NewPageID;
        }

        /// <summary>
        /// The BindData helper method is used to update the tab's
        /// layout panes with the current configuration information
        /// </summary>
        private void BindData() {
            PageSettings tab = this.PortalSettings.ActivePage;

            // Populate Page Names, etc.
            tabName.Text = "New Page";
            mobilePageName.Text = "";
            showMobile.Checked = false;

            // Populate the "ParentPage" Data
            PagesDB t = new PagesDB();
            IList<PageItem> items = t.GetPagesParent( this.PortalSettings.PortalID, PageID );
            parentPage.DataSource = items;
            parentPage.DataBind();

            // Translate
            if ( parentPage.Items.FindByText( " ROOT_LEVEL" ) != null )
                parentPage.Items.FindByText( " ROOT_LEVEL" ).Text =
                    General.GetString( "ROOT_LEVEL", "Root Level", parentPage );

            // Populate checkbox list with all security roles for this portal
            // and "check" the ones already configured for this tab
            UsersDB users = new UsersDB();
            IList<AppleseedRole> roles = users.GetPortalRoles( this.PortalSettings.PortalAlias );

            // Clear existing items in checkboxlist
            authRoles.Items.Clear();

            foreach ( AppleseedRole role in roles ) {
                ListItem item = new ListItem();
                item.Text = role.Name;
                item.Value = role.Id.ToString();

                if ( ( tab.AuthorizedRoles.LastIndexOf( item.Text ) ) > -1 )
                    item.Selected = true;

                authRoles.Items.Add( item );
            }
        }

        #endregion

        #region Web Form Designer generated code

        /// <summary>
        /// Raises the Init event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.saveButton.Click += new EventHandler(this.SaveButton_Click);
            this.CancelButton.Click += new EventHandler(this.cancelButton_Click);
            this.Load += new EventHandler(this.Page_Load);

            //Confirm delete
            if (!(ClientScript.IsClientScriptBlockRegistered("confirmDelete")))
            {
                string[] s = {"CONFIRM_DELETE"};
                ClientScript.RegisterClientScriptBlock(this.GetType(), "confirmDelete",
                                                       PortalSettings.GetStringResource(
                                                           "CONFIRM_DELETE_SCRIPT", s));
            }

            base.OnInit(e);
        }

        #endregion
    }
}
