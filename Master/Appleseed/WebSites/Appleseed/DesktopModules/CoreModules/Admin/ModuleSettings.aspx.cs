// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleSettings.aspx.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Use this page to modify title and permission on portal modules<br />
//   The ModuleSettings.aspx page is used to enable administrators to view/edit/update
//   a portal module's settings (title, output cache properties, edit access)
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Appleseed.Framework;
    using Appleseed.Framework.Exceptions;
    using Appleseed.Framework.Security;
    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.Site.Data;
    using Appleseed.Framework.Users.Data;
    using Appleseed.Framework.Web.UI;
    using Appleseed.Framework.Web.UI.WebControls;

    using HyperLink = Appleseed.Framework.Web.UI.WebControls.HyperLink;
    using LinkButton = Appleseed.Framework.Web.UI.WebControls.LinkButton;

    /// <summary>
    /// Use this page to modify title and permission on portal modules<br/>
    ///   The ModuleSettings.aspx page is used to enable administrators to view/edit/update
    ///   a portal module's settings (title, output cache properties, edit access)
    /// </summary>
    public partial class ModuleSettingsPage : EditItemPage
    {
        #region Constants and Fields

        /// <summary>
        /// The module settings button.
        /// </summary>
        protected HyperLink moduleSettingsButton;

        /// <summary>
        /// The portal tabs.
        /// </summary>
        protected List<PageItem> portalTabs;

        /// <summary>
        /// The save and close button.
        /// </summary>
        protected LinkButton saveAndCloseButton;

        /// <summary>
        /// The allowed modules.
        /// </summary>
        private List<string> allowedModules;

        #endregion

        #region Properties

        /// <summary>
        /// Only can use this page from tab with original module
        /// jviladiu@portalServices.net (2004/07/22)
        /// </summary>
        /// <value>
        /// The allowed modules.
        /// </value>
        protected override List<string> AllowedModules
        {
            get
            {
                if (this.allowedModules == null)
                {
                    var mdb = new ModulesDB();
                    var al = new List<string> { mdb.GetModuleGuid(this.ModuleID).ToString() };
                    this.allowedModules = al;
                }

                return this.allowedModules;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles OnInit event
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.PlaceHolderButtons.EnableViewState = false;
            this.PlaceholderButtons2.EnableViewState = false;

            // Controls must be created here
            this.UpdateButton = new LinkButton { CssClass = "CommandButton" };
            PlaceHolderButtons.Controls.Add(this.UpdateButton);

            // jminond added to top of property page so no need to scroll for save
            var update2 = new LinkButton { CssClass = "CommandButton", TextKey = "Apply", Text = "Apply" };
            update2.Click += this.UpdateButtonClick;
            PlaceholderButtons2.Controls.Add(update2);

            PlaceHolderButtons.Controls.Add(new LiteralControl("&nbsp;"));
            PlaceholderButtons2.Controls.Add(new LiteralControl("&nbsp;"));

            this.saveAndCloseButton = new LinkButton
                {
                    TextKey = "OK", Text = "Save and close", CssClass = "CommandButton" 
                };
            PlaceHolderButtons.Controls.Add(saveAndCloseButton);
            this.saveAndCloseButton.Click += this.SaveAndCloseButtonClick;

            // jminond added to top of property page so no need to scroll for save
            var saveAndCloseButton2 = new LinkButton
                {
                    TextKey = "OK", Text = "Save and close", CssClass = "CommandButton" 
                };
            PlaceholderButtons2.Controls.Add(saveAndCloseButton2);
            saveAndCloseButton2.Click += this.SaveAndCloseButtonClick;

            PlaceHolderButtons.Controls.Add(new LiteralControl("&nbsp;"));
            PlaceholderButtons2.Controls.Add(new LiteralControl("&nbsp;"));

            string NavigateUrlPropertyPage = Appleseed.Framework.HttpUrlBuilder.BuildUrl(
                            "~/DesktopModules/CoreModules/Admin/PropertyPage.aspx", this.PageID, this.ModuleID);

            if (Request.QueryString.GetValues("ModalChangeMaster") != null) {
                NavigateUrlPropertyPage += "&ModalChangeMaster=true";
                if (Request.QueryString.GetValues("camefromEditPage") != null)
                    NavigateUrlPropertyPage += "&camefromEditPage=true";
            }

            this.moduleSettingsButton = new HyperLink
                {
                    TextKey = "MODULESETTINGS_SETTINGS",
                    Text = "Settings",
                    CssClass = "CommandButton",
                    NavigateUrl = NavigateUrlPropertyPage
                        
                };
            if (Request.QueryString.GetValues("ModalChangeMaster") != null)
                moduleSettingsButton.Attributes.Add("onclick", "ChangeModalTitle('Module Settings');");
            PlaceHolderButtons.Controls.Add(moduleSettingsButton);

            // jminond added to top of property page so no need to scroll for save
            var moduleSettingsButton2 = new HyperLink
                {
                    TextKey = "MODULESETTINGS_SETTINGS",
                    Text = "Settings",
                    CssClass = "CommandButton",
                    NavigateUrl = NavigateUrlPropertyPage
                };
            if (Request.QueryString.GetValues("ModalChangeMaster") != null)
                moduleSettingsButton2.Attributes.Add("onclick", "ChangeModalTitle('Module Settings');");
            PlaceholderButtons2.Controls.Add(moduleSettingsButton2);

            PlaceHolderButtons.Controls.Add(new LiteralControl("&nbsp;"));
            PlaceholderButtons2.Controls.Add(new LiteralControl("&nbsp;"));

            this.CancelButton = new LinkButton { CssClass = "CommandButton" };
            if (Request.QueryString.GetValues("ModalChangeMaster")!=null)
                this.CancelButton.ID = "SecurityCancelButton";
            PlaceHolderButtons.Controls.Add(this.CancelButton);

            // jminond added to top of property page so no need to scroll for save
            var cancel2 = new LinkButton { CssClass = "CommandButton", TextKey = "Cancel", Text = "Cancel" };
            if (Request.QueryString.GetValues("ModalChangeMaster") != null)
                cancel2.ID = "SecurityCancelButton2";
            cancel2.Click += this.CancelButtonClick;
            PlaceholderButtons2.Controls.Add(cancel2);

            // if (((Page) this.Page).IsCssFileRegistered("tabsControl") == false)
            // {
            //     string themePath = Path.WebPathCombine(this.CurrentTheme.WebPath, "/tabControl.css");
            //     ((Page) this.Page).RegisterCssFile("tabsControl", themePath);
            // }
           

            this.enableWorkflowSupport.CheckedChanged += this.EnableWorkflowSupportCheckedChanged;
            base.OnInit(e);
        }

        /// <summary>
        /// The ApplyChanges_Click server event handler on this page is used
        ///   to save the module settings into the portal configuration system.
        /// </summary>
        /// <param name="e">
        /// </param>
        protected override void OnUpdate(EventArgs e)
        {
            base.OnUpdate(e);
            var useNTLM = HttpContext.Current.User is WindowsPrincipal;

            // add by Jonathan Fong 22/07/2004 to support LDAP
            // jes1111 - useNTLM |= ConfigurationSettings.AppSettings["LDAPLogin"] != null ? true : false;
            useNTLM |= Config.LDAPLogin.Length != 0 ? true : false;

            var m = this.GetModule();
            if (m == null)
            {
                return;
            }

            // Construct Authorized User Roles string

            // Edit Roles
            var editRoles = this.authEditRoles.Items.Cast<ListItem>().Where(editItem => editItem.Selected).Aggregate(string.Empty, (current, editItem) => current + editItem.Text + ";");

            // View Roles
            var viewRoles = this.authViewRoles.Items.Cast<ListItem>().Where(viewItem => viewItem.Selected).Aggregate(string.Empty, (current, viewItem) => current + viewItem.Text + ";");

            // Add Roles
            var addRoles = this.authAddRoles.Items.Cast<ListItem>().Where(addItem => addItem.Selected).Aggregate(string.Empty, (current, addItem) => current + addItem.Text + ";");

            // Delete Roles
            var deleteRoles = this.authDeleteRoles.Items.Cast<ListItem>().Where(deleteItem => deleteItem.Selected).Aggregate(string.Empty, (current, deleteItem) => current + deleteItem.Text + ";");

            // Move Module Roles
            var moveModuleRoles = this.authMoveModuleRoles.Items.Cast<ListItem>().Where(li => li.Selected).Aggregate(string.Empty, (current, li) => current + (li.Text + ";"));

            // Delete Module Roles
            var deleteModuleRoles = this.authDeleteModuleRoles.Items.Cast<ListItem>().Where(li => li.Selected).Aggregate(string.Empty, (current, li) => current + (li.Text + ";"));

            // Properties Roles
            var propertiesRoles = this.authPropertiesRoles.Items.Cast<ListItem>().Where(propertiesItem => propertiesItem.Selected).Aggregate(string.Empty, (current, propertiesItem) => current + propertiesItem.Text + ";");

            // Change by Geert.Audenaert@Syntegra.Com
            // Date: 6/2/2003
            // Publishing Roles
            var publishingRoles = this.authPublishingRoles.Items.Cast<ListItem>().Where(propertiesItem => propertiesItem.Selected).Aggregate(string.Empty, (current, propertiesItem) => current + propertiesItem.Text + ";");

            // End Change Geert.Audenaert@Syntegra.Com
            // Change by Geert.Audenaert@Syntegra.Com
            // Date: 27/2/2003
            var approvalRoles = this.authApproveRoles.Items.Cast<ListItem>().Where(propertiesItem => propertiesItem.Selected).Aggregate(string.Empty, (current, propertiesItem) => current + propertiesItem.Text + ";");

            // End Change Geert.Audenaert@Syntegra.Com

            // update module
            var modules = new ModulesDB();
            modules.UpdateModule(
                Int32.Parse(this.tabDropDownList.SelectedItem.Value), 
                this.ModuleID, 
                m.ModuleOrder, 
                m.PaneName, 
                this.moduleTitle.Text, 
                Int32.Parse(this.cacheTime.Text), 
                editRoles, 
                viewRoles, 
                addRoles, 
                deleteRoles, 
                propertiesRoles, 
                moveModuleRoles, 
                deleteModuleRoles, 
                this.ShowMobile.Checked, 
                publishingRoles, 
                this.enableWorkflowSupport.Checked, 
                approvalRoles, 
                this.showEveryWhere.Checked, 
                this.allowCollapsable.Checked);

            if (Request.QueryString.GetValues("ModalChangeMaster") != null) {
                if(Request.QueryString.GetValues("camefromEditPage") != null)
                    this.RedirectBackToReferringPage();
                else
                    Response.Write("<script type=\"text/javascript\">window.parent.location = window.parent.location.href;</script>");
            }
                
        }

        /// <summary>
        /// Gives the friendly name.
        /// </summary>
        /// <param name="guid">
        /// The GUID.
        /// </param>
        /// <returns>
        /// The friendly name
        /// </returns>
        private static string GiveMeFriendlyName(Guid guid)
        {
            var auxDr = new ModulesDB().GetSingleModuleDefinition(guid);
            return auxDr.FriendlyName;
        }

        /// <summary>
        /// The BindData helper method is used to populate a asp:datalist
        ///   server control with the current "edit access" permissions
        ///   set within the portal configuration system
        /// </summary>
        private void BindData()
        {
            var useNTLM = HttpContext.Current.User is WindowsPrincipal;

            // add by Jonathan Fong 22/07/2004 to support LDAP
            // jes1111 - useNTLM |= ConfigurationSettings.AppSettings["LDAPLogin"] != null ? true : false;
            useNTLM |= Config.LDAPLogin.Length != 0 ? true : false;

            this.authAddRoles.Visible =
                this.authApproveRoles.Visible =
                this.authDeleteRoles.Visible =
                this.authEditRoles.Visible =
                this.authPropertiesRoles.Visible =
                this.authPublishingRoles.Visible =
                this.authMoveModuleRoles.Visible =
                this.authDeleteModuleRoles.Visible = this.authViewRoles.Visible = !useNTLM;
            var m = this.GetModule();
            if (m != null)
            {
                this.moduleType.Text = GiveMeFriendlyName(m.GuidID);

                // Update Textbox Settings
                this.moduleTitle.Text = m.ModuleTitle;
                this.cacheTime.Text = m.CacheTime.ToString();

                this.portalTabs = new PagesDB().GetPagesFlat(this.PortalSettings.PortalID);
                this.tabDropDownList.DataBind();
                this.tabDropDownList.ClearSelection();
                if (this.tabDropDownList.Items.FindByValue(m.PageID.ToString()) != null)
                {
                    this.tabDropDownList.Items.FindByValue(m.PageID.ToString()).Selected = true;
                }

                // Change by John.Mandia@whitelightsolutions.com
                // Date: 19/5/2003
                this.showEveryWhere.Checked = m.ShowEveryWhere;

                // is the window mgmt support enabled
                // jes1111 - allowCollapsable.Enabled = GlobalResources.SupportWindowMgmt;
                this.allowCollapsable.Enabled = Config.WindowMgmtControls;
                this.allowCollapsable.Checked = m.SupportCollapsable;

                this.ShowMobile.Checked = m.ShowMobile;

                // Change by Geert.Audenaert@Syntegra.Com
                // Date: 6/2/2003
                PortalModuleControl pm = null;
                var controlPath = Path.WebPathCombine(Path.ApplicationRoot, m.DesktopSrc);

                try
                {
                    if (!controlPath.Contains("Area"))
                    {
                        pm = (PortalModuleControl)this.LoadControl(controlPath);
                        if (pm.InnerSupportsWorkflow)
                        {
                            this.enableWorkflowSupport.Checked = m.SupportWorkflow;
                            this.authApproveRoles.Enabled = m.SupportWorkflow;
                            this.authPublishingRoles.Enabled = m.SupportWorkflow;
                            this.PopulateRoles(ref this.authPublishingRoles, m.AuthorizedPublishingRoles);
                            this.PopulateRoles(ref this.authApproveRoles, m.AuthorizedApproveRoles);
                        }
                        else
                        {
                            this.enableWorkflowSupport.Enabled = false;
                            this.authApproveRoles.Enabled = false;
                            this.authPublishingRoles.Enabled = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // ErrorHandler.HandleException("There was a problem loading: '" + controlPath + "'", ex);
                    // throw;
                    throw new AppleseedException(
                        LogLevel.Error, "There was a problem loading: '" + controlPath + "'", ex);
                }

                // End Change Geert.Audenaert@Syntegra.Com

                // Populate checkbox list with all security roles for this portal
                // and "check" the ones already configured for this module
                this.PopulateRoles(ref this.authEditRoles, m.AuthorizedEditRoles);
                this.PopulateRoles(ref this.authViewRoles, m.AuthorizedViewRoles);
                this.PopulateRoles(ref this.authAddRoles, m.AuthorizedAddRoles);
                this.PopulateRoles(ref this.authDeleteRoles, m.AuthorizedDeleteRoles);
                this.PopulateRoles(ref this.authMoveModuleRoles, m.AuthorizedMoveModuleRoles);
                this.PopulateRoles(ref this.authDeleteModuleRoles, m.AuthorizedDeleteModuleRoles);
                this.PopulateRoles(ref this.authPropertiesRoles, m.AuthorizedPropertiesRoles);

                // Jes1111
                if (pm != null)
                {
                    if (!pm.Cacheable)
                    {
                        this.cacheTime.Text = "-1";
                        this.cacheTime.Enabled = false;
                    }
                }
            }
            else
            {
                // Denied access if Module not in Tab. jviladiu@portalServices.net (2004/07/23)
                PortalSecurity.AccessDenied();
            }
        }

        /// <summary>
        /// Handles the Click event of the CancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CancelButtonClick(object sender, EventArgs e)
        {
            this.OnCancel(e);
        }

        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <returns>
        /// The module settings.
        /// </returns>
        private ModuleSettings GetModule()
        {
            // Obtain selected module data
            return this.PortalSettings.ActivePage.Modules.Cast<ModuleSettings>().FirstOrDefault(mod => mod.ModuleID == this.ModuleID);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            if (!UserProfile.HasPortalAdministrationAccess() && !UserProfile.HasModuleAddEditAccess())
            {
                PortalSecurity.AccessDenied();
                return;
            }

            base.OnLoad(e);
        
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }

        /// <summary>
        /// Handles the Click event of the UpdateButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void UpdateButtonClick(object sender, EventArgs e)
        {
            this.OnUpdate(e);
        }

        /// <summary>
        /// Handles the CheckedChanged event of the enableWorkflowSupport control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void EnableWorkflowSupportCheckedChanged(object sender, EventArgs e)
        {
            this.authApproveRoles.Enabled = this.enableWorkflowSupport.Checked;
            this.authPublishingRoles.Enabled = this.enableWorkflowSupport.Checked;
        }

        /// <summary>
        /// Populates the roles.
        /// </summary>
        /// <param name="listRoles">The list roles.</param>
        /// <param name="moduleRoles">The module roles.</param>
        private void PopulateRoles(ref CheckBoxList listRoles, string moduleRoles)
        {
            // Get roles from db
            var users = new UsersDB();
            var roles = users.GetPortalRoles(this.PortalSettings.PortalAlias);

            // Clear existing items in checkbox list
            listRoles.Items.Clear();

            // All Users
            var allItem = new ListItem("All Users");
            listRoles.Items.Add(allItem);

            // Authenticated user role added 15 nov 2002 - by manudea
            var authItem = new ListItem("Authenticated Users");
            listRoles.Items.Add(authItem);

            // Unauthenticated user role added 30/01/2003 - by manudea
            var unauthItem = new ListItem("Unauthenticated Users");
            listRoles.Items.Add(unauthItem);

            listRoles.DataSource = roles.Where(rn => rn.Name.ToLower() != "admins");
            listRoles.DataTextField = "Name";
            listRoles.DataValueField = "Id";
            listRoles.DataBind();

            // Splits up the role string and use array 30/01/2003 - by manudea
            while (moduleRoles.EndsWith(";"))
            {
                moduleRoles = moduleRoles.Substring(0, moduleRoles.Length - 1);
            }

            var arrModuleRoles = moduleRoles.Split(';');
            var roleCount = arrModuleRoles.GetUpperBound(0);

            // Cycle every role and select it if needed
            foreach (ListItem ls in listRoles.Items)
            {
                for (var i = 0; i <= roleCount; i++)
                {
                    if (arrModuleRoles[i].ToLower() == ls.Text.ToLower())
                    {
                        ls.Selected = true;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the saveAndCloseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SaveAndCloseButtonClick(object sender, EventArgs e)
        {
            this.OnUpdate(e);

            // Navigate back to admin page
            if (Request.QueryString.GetValues("ModalChangeMaster") == null && this.Page.IsValid)
            {
                this.RedirectBackToReferringPage();
            }
        }

        #endregion
    }
}