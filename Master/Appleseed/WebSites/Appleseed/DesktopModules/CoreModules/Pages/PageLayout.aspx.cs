// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageLayout.aspx.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Edit page for page layouts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Admin
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Appleseed.Framework;
    using Appleseed.Framework.Providers.AppleseedSiteMapProvider;
    using Appleseed.Framework.Security;
    using Appleseed.Framework.Settings.Cache;
    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.Site.Data;
    using Appleseed.Framework.Users.Data;
    using Appleseed.Framework.Web.UI;
    using Appleseed.Framework.Web.UI.WebControls;

    using ImageButton = System.Web.UI.WebControls.ImageButton;
    using Appleseed.Framework.Web;

    /// <summary>
    /// Edit page for page layouts
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class PageLayout : EditItemPage
    {
        #region Constants and Fields

        /// <summary>
        ///   list of sorted modules in the bottom region of the page
        /// </summary>
        protected ArrayList bottomList;

        /// <summary>
        ///   list of sorted module in the main content pane
        /// </summary>
        protected ArrayList contentList;

        /// <summary>
        ///   list of sorted modules in the left region of a page
        /// </summary>
        protected ArrayList leftList;

        /// <summary>
        ///   list of sorted modules in the right region of the page
        /// </summary>
        protected ArrayList rightList;

        /// <summary>
        ///   list of sorted modules in the top region of the page
        /// </summary>
        protected ArrayList topList;

        protected String urlToLoadModules;

        #endregion

        #region Methods

        /// <summary>
        /// The AddModuleToPane_Click server event handler
        ///   on this page is used to add a new portal module
        ///   into the tab
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        [History("bja@reedtek.com", "2003/05/16", "Added extra parameter for collpasable window")]
        [History("john.mandia@whitelightsolutions.com", "2003/05/24", "Added extra parameter for ShowEverywhere")]
        protected void AddModuleToPane_Click(object sender, EventArgs e)
        {
            // All new modules go to the end of the content pane
            var m = new ModuleItem();
            m.Title = this.moduleTitle.Text;
            m.ModuleDefID = Int32.Parse(this.moduleType.SelectedItem.Value);
            m.Order = 999;

            // save to database
            var mod = new ModulesDB();

            // Change by Geert.Audenaert@Syntegra.Com
            // Date: 6/2/2003
            // Original:             m.ID = _mod.AddModule(tabID, m.Order, "ContentPane", m.Title, m.ModuleDefID, 0, "Admins", "All Users", "Admins", "Admins", "Admins", false);
            // Changed by Mario Endara <mario@softworks.com.uy> (2004/11/09)
            // The new module inherits security from Pages module (current ModuleID) 
            // so who can edit the tab properties/content can edit the module properties/content (except view that remains =)
            m.ID = mod.AddModule(
                this.PageID,
                m.Order,
                this.paneLocation.SelectedItem.Value,
                m.Title,
                m.ModuleDefID,
                0,
                PortalSecurity.GetEditPermissions(this.ModuleID),
                this.viewPermissions.SelectedItem.Value,
                PortalSecurity.GetAddPermissions(this.ModuleID),
                PortalSecurity.GetDeletePermissions(this.ModuleID),
                PortalSecurity.GetPropertiesPermissions(this.ModuleID),
                PortalSecurity.GetMoveModulePermissions(this.ModuleID),
                PortalSecurity.GetDeleteModulePermissions(this.ModuleID),
                false,
                PortalSecurity.GetPublishPermissions(this.ModuleID),
                false,
                false,
                false);

            // End Change Geert.Audenaert@Syntegra.Com

            // reload the portalSettings from the database
            this.Context.Items["PortalSettings"] = PortalSettings.GetPortalSettings(this.PageID, this.PortalSettings.PortalAlias);
            this.PortalSettings = (PortalSettings)this.Context.Items["PortalSettings"];

            // reorder the modules in the content pane
            var modules = this.GetModules("ContentPane");
            this.OrderModules(modules);

            // resave the order
            foreach (ModuleItem item in modules)
            {
                mod.UpdateModuleOrder(item.ID, item.Order, "ContentPane");
            }

            // Redirect to the same page to pick up changes
            this.Response.Redirect(this.AppendModuleID(this.Request.RawUrl, m.ID));
        }

        /// <summary>
        /// The DeleteBtn_Click server event handler on this page is
        ///   used to delete a portal module from the page
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="T:System.Web.UI.ImageClickEventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected void DeleteBtn_Click(object sender, ImageClickEventArgs e)
        {
            var pane = ((ImageButton)sender).CommandArgument;
            var _listbox = (ListBox)this.Page.FindControl(pane);
            if (_listbox == null)
            {
                _listbox = (ListBox)this.Page.Master.FindControl("Content").FindControl(pane);
            }

            var modules = this.GetModules(pane);

            if (_listbox.SelectedIndex != -1)
            {
                var m = (ModuleItem)modules[_listbox.SelectedIndex];
                if (m.ID > -1)
                {
                    // jviladiu@portalServices.net (20/08/2004) Add role control for delete module
                    if (PortalSecurity.IsInRoles(PortalSecurity.GetDeleteModulePermissions(m.ID)))
                    {
                        // must delete from database too
                        var moddb = new ModulesDB();

                        // TODO add userEmail and useRecycler
                        moddb.DeleteModule(m.ID);
                    }
                    else
                    {
                        this.msgError.Visible = true;
                        return;
                    }
                }
            }

            // Redirect to the same page to pick up changes
            this.Response.Redirect(this.Request.RawUrl);
        }

        /// <summary>
        /// The EditBtn_Click server event handler on this page is
        ///   used to edit an individual portal module's settings
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="T:System.Web.UI.ImageClickEventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected void EditBtn_Click(object sender, ImageClickEventArgs e)
        {
            var pane = ((ImageButton)sender).CommandArgument;
            var _listbox = (ListBox)this.Page.FindControl(pane);
            if (_listbox == null)
            {
                _listbox = (ListBox)this.Page.Master.FindControl("Content").FindControl(pane);
            }

            if (_listbox.SelectedIndex != -1)
            {
                var mid = Int32.Parse(_listbox.SelectedItem.Value);

                // Add role control to edit module settings by Mario Endara <mario@softworks.com.uy> (2004/11/09)
                if (PortalSecurity.IsInRoles(PortalSecurity.GetPropertiesPermissions(mid)))
                {
                    var url = HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Admin/ModuleSettings.aspx", this.PageID, mid);
                    // Redirect to module settings page
                    if (Request.QueryString.GetValues("ModalChangeMaster") != null)
                    {
                        url += "&ModalChangeMaster=true&camefromEditPage=true";
                    }
                    this.Response.Redirect(url);
                }
                else
                {
                    this.msgError.Visible = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Handles the UpdateControl event of the EditTable control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="T:Appleseed.Framework.Web.UI.WebControls.SettingsTableEventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected void EditTable_UpdateControl(object sender, SettingsTableEventArgs e)
        {
            Framework.Site.Configuration.PageSettings.UpdatePageSettings(
                this.PageID,
                ((ISettingItem)e.CurrentItem).EditControl.ID,
                ((ISettingItem)e.CurrentItem).Value.ToString());
        }

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
            if (!this.Page.IsPostBack)
            {

                // Invalidate settings cache
                if (CurrentCache.Exists(Key.TabSettings(this.PageID)))
                {
                    CurrentCache.Remove(Key.TabSettings(this.PageID));
                }

                lblFriendlyExtension.Text = System.Configuration.ConfigurationManager.AppSettings["FriendlyUrlExtension"].ToString();
            }

            base.OnLoad(e);

            // Confirm delete
            if (!this.ClientScript.IsClientScriptBlockRegistered("confirmDelete"))
            {
                string[] s = { "CONFIRM_DELETE" };
                this.ClientScript.RegisterClientScriptBlock(
                    this.GetType(), "confirmDelete", PortalSettings.GetStringResource("CONFIRM_DELETE_SCRIPT", s));
            }

            //this.TopDeleteBtn.Attributes.Add("00", "return confirmDelete()");
            //this.LeftDeleteBtn.Attributes.Add("OnClick", "return confirmDelete()");
            //this.RightDeleteBtn.Attributes.Add("OnClick", "return confirmDelete()");
            //this.ContentDeleteBtn.Attributes.Add("OnClick", "return confirmDelete()");
            //this.BottomDeleteBtn.Attributes.Add("OnClick", "return confirmDelete()");
            urlToLoadModules = "'" + HttpUrlBuilder.BuildUrl("~/Appleseed.Core/PageLayout/LoadModule") + "'";
            // If first visit to the page, update all entries
            if (!this.Page.IsPostBack)
            {

                this.msgError.Visible = false;

                this.BindData();

                this.SetSecurityAccess();


                // 2/27/2003 Start - Ender Malkoc
                // After up or down button when the page is refreshed, select the previously selected
                // tab from the list.
                if (this.Request.Params["selectedmodid"] != null)
                {
                    try
                    {
                        var modIndex = Int32.Parse(this.Request.Params["selectedmodid"]);
                        //this.SelectModule(this.topPane, this.GetModules("TopPane"), modIndex);
                        //this.SelectModule(this.leftPane, this.GetModules("LeftPane"), modIndex);
                        //this.SelectModule(this.contentPane, this.GetModules("ContentPane"), modIndex);
                        //this.SelectModule(this.rightPane, this.GetModules("RightPane"), modIndex);
                        //this.SelectModule(this.bottomPane, this.GetModules("BottomPane"), modIndex);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.Publish(
                            LogLevel.Error,
                            "After up or down button when the page is refreshed, select the previously selected tab from the list.",
                            ex);
                    }
                }

                // 2/27/2003 end - Ender Malkoc
            }

            // Binds custom settings to table
            this.EditTable.DataSource = new SortedList(this.PageSettings);
            this.EditTable.DataBind();

            this.ModuleIdField.Value = this.ModuleID.ToString();
            this.PageIdField.Value = this.PageID.ToString();

        }

        /// <summary>
        /// The OnUpdate on this page is used to save
        ///   the current tab settings to the database and
        ///   then redirect back to the main admin page.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void OnUpdate(EventArgs e)
        {
            // Only Update if Input Data is Valid
            if (this.Page.IsValid)
            {
                try
                {
                    this.SavePageData();
                    //remove from cache
                    SqlUrlBuilderProvider.ClearCachePageUrl(this.PageID);
                    UrlBuilderHelper.ClearUrlElements(this.PageID);

                    // Flush all tab navigation cache keys. Very important for recovery the changes
                    // made in all languages and not get a error if user change the tab parent.
                    // jviladiu@portalServices.net (05/10/2004)
                    CurrentCache.RemoveAll("_PageNavigationSettings_");
                    PortalSettings.RemovePortalSettingsCache(PageID, PortalSettings.PortalAlias);

                    // Clear AppleseedSiteMapCache
                    AppleseedSiteMapProvider.ClearAllAppleseedSiteMapCaches();

                    // redirect back to the admin page
                    // int adminIndex = portalSettings.DesktopPages.Count-1;        
                    // 3_aug_2004 Cory Isakson use returntabid from QueryString
                    // Updated 6_Aug_2004 by Cory Isakson to accomodate addtional Page Management
                    var retPage = this.Request.QueryString["returnPageID"];
                    string returnPage;

                    if (Request.QueryString.GetValues("ModalChangeMaster") != null)
                    {
                        if (retPage != null)
                        {
                            // user is returned to the calling tab.
                            returnPage = HttpUrlBuilder.BuildUrl(int.Parse(retPage));
                        }
                        else
                        {
                            // user is returned to updated tab
                            returnPage = HttpUrlBuilder.BuildUrl(this.PageID);
                        }

                        Response.Write("<script type=\"text/javascript\">window.parent.location = '" + returnPage + "';</script>");
                    }
                        
                    else
                    {
                        if (retPage != null)
                        {
                            // user is returned to the calling tab.
                            returnPage = HttpUrlBuilder.BuildUrl(int.Parse(retPage));
                        }
                        else
                        {
                            // user is returned to updated tab
                            returnPage = HttpUrlBuilder.BuildUrl(this.PageID);
                        }

                        this.Response.Redirect(returnPage);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message == "FriendlyUrlIsAlreadyExists")
                    {
                        this.lblUrlAlreadyExist.Visible = true;
                    }
                    else
                    {
                        this.lblErrorNotAllowed.Visible = true;
                    }

                }
            }
        }

        /// <summary>
        /// The PageSettings_Change server event handler on this page is
        ///   invoked any time the tab name or access security settings
        ///   change.  The event handler in turn calls the "SavePageData"
        ///   helper method to ensure that these changes are persisted
        ///   to the portal configuration file.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected void PageSettings_Change(object sender, EventArgs e)
        {
            // Ensure that settings are saved
            try
            {
                this.SavePageData();
            }
            catch (Exception ex)
            {
                if (ex.Message == "FriendlyUrlIsAlreadyExists")
                {
                    this.lblUrlAlreadyExist.Visible = true;
                }
                else
                {
                    this.lblErrorNotAllowed.Visible = true;
                }

            }
        }

        /// <summary>
        /// The RightLeft_Click server event handler on this page is
        ///   used to move a portal module between layout panes on
        ///   the tab page
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="T:System.Web.UI.ImageClickEventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected void RightLeft_Click(object sender, ImageClickEventArgs e)
        {
            var sourcePane = ((ImageButton)sender).Attributes["sourcepane"];
            var targetPane = ((ImageButton)sender).Attributes["targetpane"];
            var sourceBox = (ListBox)this.Page.FindControl(sourcePane);
            if (sourceBox == null)
            {
                sourceBox = (ListBox)this.Page.Master.FindControl("Content").FindControl(sourcePane);
            }

            var targetBox = (ListBox)this.Page.FindControl(targetPane);
            if (targetBox == null)
            {
                targetBox = (ListBox)this.Page.Master.FindControl("Content").FindControl(targetPane);
            }

            if (sourceBox.SelectedIndex != -1)
            {
                // get source arraylist
                var sourceList = this.GetModules(sourcePane);

                // get a reference to the module to move
                // and assign a high order number to send it to the end of the target list
                var m = (ModuleItem)sourceList[sourceBox.SelectedIndex];

                if (PortalSecurity.IsInRoles(PortalSecurity.GetMoveModulePermissions(m.ID)))
                {
                    // add it to the database
                    var admin = new ModulesDB();
                    admin.UpdateModuleOrder(m.ID, 99, targetPane);

                    // delete it from the source list
                    sourceList.RemoveAt(sourceBox.SelectedIndex);

                    // reload the portalSettings from the database
                    HttpContext.Current.Items["PortalSettings"] = PortalSettings.GetPortalSettings(
                        this.PageID, this.PortalSettings.PortalAlias);
                    this.PortalSettings = (PortalSettings)this.Context.Items["PortalSettings"];

                    // reorder the modules in the source pane
                    sourceList = this.GetModules(sourcePane);
                    this.OrderModules(sourceList);

                    // resave the order
                    foreach (ModuleItem item in sourceList)
                    {
                        admin.UpdateModuleOrder(item.ID, item.Order, sourcePane);
                    }

                    // reorder the modules in the target pane
                    var targetList = this.GetModules(targetPane);
                    this.OrderModules(targetList);

                    // resave the order
                    foreach (ModuleItem item in targetList)
                    {
                        admin.UpdateModuleOrder(item.ID, item.Order, targetPane);
                    }

                    // Redirect to the same page to pick up changes
                    this.Response.Redirect(this.AppendModuleID(this.Request.RawUrl, m.ID));
                }
                else
                {
                    this.msgError.Visible = true;
                }
            }
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
        protected void UpDown_Click(object sender, ImageClickEventArgs e)
        {
            var cmd = ((ImageButton)sender).CommandName;
            var pane = ((ImageButton)sender).CommandArgument;
            var _listbox = (ListBox)this.Page.FindControl(pane);
            if (_listbox == null)
            {
                _listbox = (ListBox)this.Page.Master.FindControl("Content").FindControl(pane);
            }

            var modules = this.GetModules(pane);

            if (_listbox.SelectedIndex != -1)
            {
                int delta;
                var selection = -1;

                // Determine the delta to apply in the order number for the module
                // within the list.  +3 moves down one item; -3 moves up one item
                if (cmd == "down")
                {
                    delta = 3;
                    if (_listbox.SelectedIndex < _listbox.Items.Count - 1)
                    {
                        selection = _listbox.SelectedIndex + 1;
                    }
                }
                else
                {
                    delta = -3;
                    if (_listbox.SelectedIndex > 0)
                    {
                        selection = _listbox.SelectedIndex - 1;
                    }
                }

                ModuleItem m;
                m = (ModuleItem)modules[_listbox.SelectedIndex];

                if (PortalSecurity.IsInRoles(PortalSecurity.GetMoveModulePermissions(m.ID)))
                {
                    m.Order += delta;

                    // reorder the modules in the content pane
                    this.OrderModules(modules);

                    // resave the order
                    var admin = new ModulesDB();
                    foreach (ModuleItem item in modules)
                    {
                        admin.UpdateModuleOrder(item.ID, item.Order, pane);
                    }

                    // Redirect to the same page to pick up changes
                    this.Response.Redirect(this.AppendModuleID(this.Request.RawUrl, m.ID));
                }
                else
                {
                    this.msgError.Visible = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Appends the module ID.
        /// </summary>
        /// <param name="url">
        /// The URL.
        /// </param>
        /// <param name="moduleID">
        /// The module ID.
        /// </param>
        /// <returns>
        /// The append module id.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private string AppendModuleID(string url, int moduleID)
        {
            var selectedModIDPos = url.IndexOf("&selectedmodid");
            if (selectedModIDPos >= 0)
            {
                var selectedModIDEndPos = url.IndexOf("&", selectedModIDPos + 1);
                if (selectedModIDEndPos >= 0)
                {
                    return url.Substring(0, selectedModIDPos) + "&selectedmodid=" + moduleID +
                           url.Substring(selectedModIDEndPos);
                }
                else
                {
                    return url.Substring(0, selectedModIDPos) + "&selectedmodid=" + moduleID;
                }
            }
            else
            {
                return url + "&selectedmodid=" + moduleID;
            }
        }

        /// <summary>
        /// The BindData helper method is used to update the tab's
        ///   layout panes with the current configuration information
        /// </summary>
        /// <remarks>
        /// </remarks>
        private void BindData()
        {
            var page = this.PortalSettings.ActivePage;

            // Populate Page Names, etc.
            this.tabName.Text = page.PageName;
            this.friendlyUrl.Text = page.FriendlyURL;
            this.mobilePageName.Text = page.MobilePageName;
            this.showMobile.Checked = page.ShowMobile;

            // Populate the "ParentPage" Data
            var t = new PagesDB();
            var items = t.GetPagesParent(this.PortalSettings.PortalID, this.PageID);
            this.parentPage.DataSource = items;
            this.parentPage.DataBind();

            if (this.parentPage.Items.FindByValue(page.ParentPageID.ToString()) != null)
            {
                // parentPage.Items.FindByValue( tab.ParentPageID.ToString() ).Selected = true;
                this.parentPage.SelectedValue = page.ParentPageID.ToString();
            }

            // Translate
            if (this.parentPage.Items.FindByText(" ROOT_LEVEL") != null)
            {
                this.parentPage.Items.FindByText(" ROOT_LEVEL").Text = General.GetString(
                    "ROOT_LEVEL", "Root Level", this.parentPage);
            }

            // Populate checkbox list with all security roles for this portal
            // and "check" the ones already configured for this tab
            var users = new UsersDB();
            var roles = users.GetPortalRoles(this.PortalSettings.PortalAlias);

            // Clear existing items in checkboxlist
            this.authRoles.Items.Clear();

            foreach (var role in roles)
            {
                var item = new ListItem();
                item.Text = role.Name;
                item.Value = role.Id.ToString();

                if (page.AuthorizedRoles.LastIndexOf(item.Text) > -1)
                {
                    item.Selected = true;
                }

                this.authRoles.Items.Add(item);
            }

            // Populate the "Add Module" Data
            var m = new ModulesDB();
            var modules = new SortedList<string, string>();
            var drCurrentModuleDefinitions = m.GetCurrentModuleDefinitions(this.PortalSettings.PortalID);
            //if (PortalSecurity.IsInRoles("Admins") || !bool.Parse(drCurrentModuleDefinitions["Admin"].ToString()))
            //{
            var htmlId = "0";
            try
            {
                while (drCurrentModuleDefinitions.Read())
                {
                    if ((!modules.ContainsKey(drCurrentModuleDefinitions["FriendlyName"].ToString())) &&
                        (PortalSecurity.IsInRoles("Admins") || !bool.Parse(drCurrentModuleDefinitions["Admin"].ToString())))
                    {
                        modules.Add(
                            // moduleType.Items.Add(
                            // new ListItem(drCurrentModuleDefinitions["FriendlyName"].ToString(),
                            // drCurrentModuleDefinitions["ModuleDefID"].ToString()));
                            drCurrentModuleDefinitions["FriendlyName"].ToString(),
                            drCurrentModuleDefinitions["ModuleDefID"].ToString());
                        if (drCurrentModuleDefinitions["FriendlyName"].ToString().Equals("HTML Content"))
                            htmlId = drCurrentModuleDefinitions["ModuleDefID"].ToString();
                    }
                }
            }
            finally
            {
                drCurrentModuleDefinitions.Close();
            }
            //}

            // Dictionary<string, string> actions = ModelServices.GetMVCActionModules();
            // foreach (string key in actions.Keys) {
            // modules.Add(key, actions[key]);
            // }
            this.moduleType.DataSource = modules;
            this.moduleType.DataBind();
            this.moduleType.SelectedValue = htmlId;


            // Now it's the load is by ajax 1/september/2011
            // Populate Top Pane Module Data
            //this.topList = this.GetModules("TopPane");
            //this.topPane.DataBind();

            //// Populate Left Hand Pane Module Data
            //this.leftList = this.GetModules("LeftPane");
            //this.leftPane.DataBind();

            //// Populate Content Pane Module Data
            //this.contentList = this.GetModules("ContentPane");
            //this.contentPane.DataBind();

            //// Populate Right Hand Module Data
            //this.rightList = this.GetModules("RightPane");
            //this.rightPane.DataBind();

            //// Populate Bottom Module Data
            //this.bottomList = this.GetModules("BottomPane");
            //this.bottomPane.DataBind();
        }

        /// <summary>
        /// The GetModules helper method is used to get the modules
        ///   for a single pane within the tab
        /// </summary>
        /// <param name="pane">
        /// The pane.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        private ArrayList GetModules(string pane)
        {
            var paneModules = new ArrayList();

            foreach (ModuleSettings _module in this.PortalSettings.ActivePage.Modules)
            {
                if (_module.PaneName.ToLower() == pane.ToLower() &&
                    this.PortalSettings.ActivePage.PageID == _module.PageID)
                {
                    var m = new ModuleItem();
                    m.Title = _module.ModuleTitle;
                    m.ID = _module.ModuleID;
                    m.ModuleDefID = _module.ModuleDefID;
                    m.Order = _module.ModuleOrder;
                    paneModules.Add(m);
                }
            }

            return paneModules;
        }

        /// <summary>
        /// The OrderModules helper method is used to reset the display
        ///   order for modules within a pane
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void OrderModules(ArrayList list)
        {
            var i = 1;

            // sort the arraylist
            list.Sort();

            // renumber the order
            foreach (ModuleItem m in list)
            {
                // number the items 1, 3, 5, etc. to provide an empty order
                // number when moving items up and down in the list.
                m.Order = i;
                i += 2;
            }
        }

        /// <summary>
        /// The SavePageData helper method is used to persist the
        ///   current tab settings to the database.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private void SavePageData()
        {
            // Construct Authorized User Roles string
            var authorizedRoles = string.Empty;

            foreach (ListItem item in this.authRoles.Items)
            {
                if (item.Selected)
                {
                    authorizedRoles = authorizedRoles + item.Text + ";";
                }
            }

            var pageDB = new PagesDB();
            if (!string.IsNullOrEmpty(this.friendlyUrl.Text) && pageDB.IsAlreadyExistsFriendlyUrl(this.friendlyUrl.Text, this.PageID))
            {
                throw new Exception("FriendlyUrlIsAlreadyExists");
            }
            else
            {
                // update Page info in the database
                pageDB.UpdatePage(
                    this.PortalSettings.PortalID,
                    this.PageID,
                    Int32.Parse(this.parentPage.SelectedItem.Value),
                    this.tabName.Text,
                    this.PortalSettings.ActivePage.PageOrder,
                    authorizedRoles,
                    this.mobilePageName.Text,
                    this.showMobile.Checked,
                    this.friendlyUrl.Text);

                // Update custom settings in the database
                this.EditTable.UpdateControls();
            }
        }

        /// <summary>
        /// Given the moduleID of a module, this function selects the right tab in the provided list control
        /// </summary>
        /// <param name="listBox">
        /// Listbox that contains the list of modules
        /// </param>
        /// <param name="modules">
        /// ArrayList containing the Module Items
        /// </param>
        /// <param name="moduleID">
        /// moduleID of the module that needs to be selected
        /// </param>
        /// <remarks>
        /// </remarks>
        private void SelectModule(ListBox listBox, ArrayList modules, int moduleID)
        {
            for (var i = 0; i < modules.Count; i++)
            {
                if (((ModuleItem)modules[i]).ID == moduleID)
                {
                    if (listBox.SelectedItem != null)
                    {
                        listBox.SelectedItem.Selected = false;
                    }

                    listBox.Items[i].Selected = true;
                    return;
                }
            }

            return;
        }

        /// <summary>
        /// This method override the security cookie for allow
        ///   to access property pages of selected module in tab.
        ///   jviladiu@portalServices.net (2004/07/23)
        /// </summary>
        /// <remarks>
        /// </remarks>
        private void SetSecurityAccess()
        {
            HttpCookie cookie;
            DateTime time;
            TimeSpan span;
            var guidsInUse = string.Empty;
            Guid guid;

            var mdb = new ModulesDB();

            //foreach (ListItem li in this.topPane.Items)
            //{
            //    guid = mdb.GetModuleGuid(int.Parse(li.Value));
            //    if (guid != Guid.Empty)
            //    {
            //        guidsInUse += guid.ToString().ToUpper() + "@";
            //    }
            //}

            //foreach (ListItem li in this.leftPane.Items)
            //{
            //    guid = mdb.GetModuleGuid(int.Parse(li.Value));
            //    if (guid != Guid.Empty)
            //    {
            //        guidsInUse += guid.ToString().ToUpper() + "@";
            //    }
            //}

            //foreach (ListItem li in this.contentPane.Items)
            //{
            //    guid = mdb.GetModuleGuid(int.Parse(li.Value));
            //    if (guid != Guid.Empty)
            //    {
            //        guidsInUse += guid.ToString().ToUpper() + "@";
            //    }
            //}

            //foreach (ListItem li in this.rightPane.Items)
            //{
            //    guid = mdb.GetModuleGuid(int.Parse(li.Value));
            //    if (guid != Guid.Empty)
            //    {
            //        guidsInUse += guid.ToString().ToUpper() + "@";
            //    }
            //}

            //foreach (ListItem li in this.bottomPane.Items)
            //{
            //    guid = mdb.GetModuleGuid(int.Parse(li.Value));
            //    if (guid != Guid.Empty)
            //    {
            //        guidsInUse += guid.ToString().ToUpper() + "@";
            //    }
            //}

            cookie = new HttpCookie("AppleseedSecurity", guidsInUse);
            time = DateTime.Now;
            span = new TimeSpan(0, 2, 0, 0, 0); // 120 minutes to expire
            cookie.Expires = time.Add(span);
            base.Response.AppendCookie(cookie);
        }

        protected string getUrlToEdit()
        {
            var url = HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Admin/ModuleSettings.aspx", this.PageID);
            if (Request.QueryString.GetValues("ModalChangeMaster") != null)
            {
                url += "&ModalChangeMaster=true&camefromEditPage=true";
            }
            return "'" + url + "'";
        }
        #endregion
    }
}