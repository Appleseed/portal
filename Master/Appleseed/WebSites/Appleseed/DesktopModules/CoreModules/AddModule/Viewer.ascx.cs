// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Viewer.ascx.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   This module has been built by John Mandia (www.whitelightsolutions.com)
//   It allows administrators to give permission to selected roles to add modules to pages
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Content.Web.Modules.AddModule
{
    using System;
    using System.IO;
    using System.Web;
    using System.Web.UI.WebControls;

    using Appleseed.Framework;
    using Appleseed.Framework.Core.Model;
    using Appleseed.Framework.Security;
    using Appleseed.Framework.Site.Data;
    using Appleseed.Framework.Web.UI.WebControls;

    using Localize = Appleseed.Framework.Web.UI.WebControls.Localize;
    using Path = Appleseed.Framework.Settings.Path;

    /// <summary>
    /// This module has been built by John Mandia (www.whitelightsolutions.com)
    ///   It allows administrators to give permission to selected roles to add modules to pages
    /// </summary>
    /// <remarks>
    /// </remarks>
    [History("jminond", "2006/03/25", "Converted to partial class")]
    [History("jminond", "2006/03/19", "Corrected adding module to root page for site")]
    [History("jminond", "2005/03/10", "Changes for moving Tab to Page")]
    public partial class Viewer : PortalModuleControl
    {
        #region Constants and Fields

        /// <summary>
        ///   Localized label for add module
        /// </summary>
        protected Localize addmodule;

        #endregion

        #region Properties

        /// <summary>
        ///   Marks This Module To Be An Admin Module
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
        ///   Gets the GUID for this module.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public override Guid GuidID
        {
            get
            {
                return new Guid("{350CED6F-6739-43f3-8BF1-1D95187CA0BF}");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"/> object that contains the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void OnInit(EventArgs e)
        {
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            this.InitializeComponent();

            // Create a new Title the control
            // 			ModuleTitle = new DesktopModuleTitle();
            // Set here title properties
            // Add title ad the very beginning of
            // the control's controls collection
            // 			Controls.AddAt(0, ModuleTitle);

            // Call base init procedure
            base.OnInit(e);
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
            base.OnLoad(e);

            // If first visit to the page, update all entries
            if (this.Page.IsPostBack)
            {
                return;
            }

            this.BindData();
            this.SetHelpPath();
            this.SetModuleName();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the moduleType control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected void moduleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetHelpPath();
            this.SetModuleName();
        }

        /// <summary>
        /// Gets the folder help path.
        /// </summary>
        /// <param name="desktopSrc">
        /// Desktop SRC.
        /// </param>
        /// <returns>
        /// The name of the help folder in the correct format
        /// </returns>
        /// <remarks>
        /// </remarks>
        private static string GetHelpPath(string desktopSrc)
        {
            var helpPath = desktopSrc.Replace(".", "_");
            return "Appleseed/" + helpPath;
        }

        /*[Ajax.AjaxMethod]*/
        /*
        public System.Collections.Specialized.StringCollection ModuleChangeStrings(string moduleType, string moduleName)
        {
            SetDatata(moduleType);
            moduleTitle.Text = moduleName;

            System.Collections.Specialized.StringCollection s = new System.Collections.Specialized.StringCollection();

            s.Add(moduleTitle.Text);

            if (AddModuleHelp.Visible)
            {
                s.Add(AddModuleHelp.Attributes["onclick"].ToString());
                s.Add(AddModuleHelp.NavigateUrl);
                s.Add(AddModuleHelp.ImageUrl);
                s.Add(AddModuleHelp.ToolTip);
            }

            return s;
        }
         * */

        /// <summary>
        /// The AddModule_Click server event handler
        ///   on this page is used to add a new portal module
        ///   into the tab
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void AddModule_Click(object sender, EventArgs e)
        {
            // TODO: IF PAGE ID = 0 Then we know it's home page, cant we get from db the id?
            // PagesDB _d = new PagesDB();
            var pid = this.PageID;
            if (pid == 0)
            {
                pid = PagesDB.PortalHomePageId(this.PortalID);
            }

            if (pid != 0)
            {
                // All new modules go to the end of the content pane
                var selectedModule = this.moduleType.SelectedItem.Value;
                var start = selectedModule.IndexOf("|");
                var moduleID = Convert.ToInt32(selectedModule.Substring(0, start).Trim());

                // Hide error message in case there was a previous error.
                this.moduleError.Visible = false;

                // This allows the user to pick what type of people can view the module being added.
                // If Authorized Roles is selected from the dropdown then every role that has view permission for the
                // Add Role module will be added to the view permissions of the module being added.
                var viewPermissionRoles = this.viewPermissions.SelectedValue;
                if (viewPermissionRoles == "Authorised Roles")
                {
                    viewPermissionRoles = PortalSecurity.GetViewPermissions(this.ModuleID);
                }

                try
                {
                    var m = new ModuleItem { Title = this.TitleTextBox.Value, ModuleDefID = moduleID, Order = 999 };

                    // save to database
                    var mod = new ModulesDB();
                    m.ID = mod.AddModule(
                        pid, 
                        m.Order, 
                        this.paneLocation.SelectedValue, 
                        m.Title, 
                        m.ModuleDefID, 
                        0, 
                        PortalSecurity.GetEditPermissions(this.ModuleID), 
                        viewPermissionRoles, 
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
                }
                catch (Exception ex)
                {
                    this.moduleError.Visible = true;
                    ErrorHandler.Publish(
                        LogLevel.Error, 
                        "There was an error with the Add Module Module while trying to add a new module.", 
                        ex);
                }
                finally
                {
                    if (this.moduleError.Visible == false)
                    {
                        // Reload page to pick up changes
                        this.Response.Redirect(this.Request.RawUrl, false);
                    }
                }
            }
            else
            {
                // moduleError.TextKey = "ADDMODULE_HOMEPAGEERROR";
                this.moduleError.Text = General.GetString(
                    "ADDMODULE_HOMEPAGEERROR", 
                    "You are currently on the homepage using the default virtual ID (The default ID is set when no specific page is selected. e.g. www.yourdomain.com. Please select your homepage from the Navigation menu e.g. 'Home' so that you can add a module against the page's actual ID.");
                this.moduleError.Visible = true;
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
            // Populate the "Add Module" Data
            var m = new ModulesDB();

            var drCurrentModuleDefinitions = m.GetCurrentModuleDefinitions(this.PortalSettings.PortalID);

            try
            {
                // 				if(this.ArePropertiesEditable)
                // 				{
                // 					while(drCurrentModuleDefinitions.Read())
                // 					{
                // 						moduleType.Items.Add(new ListItem(drCurrentModuleDefinitions["FriendlyName"].ToString(),drCurrentModuleDefinitions["ModuleDefID"].ToString() + "|" + GetHelpPath(drCurrentModuleDefinitions["DesktopSrc"].ToString())));
                // 					}
                // 				}
                // 				else
                // 				{
                while (drCurrentModuleDefinitions.Read())
                {
                    // Added by Mario Endara <mario@softworks.com.uy> 2004/11/04
                    // only users members of the "Amins" role can add Admin modules to a Tab
                    if (PortalSecurity.IsInRoles("Admins") ||
                        !bool.Parse(drCurrentModuleDefinitions["Admin"].ToString()))
                    {
                        this.moduleType.Items.Add(
                            new ListItem(
                                drCurrentModuleDefinitions["FriendlyName"].ToString(), 
                                string.Format(
                                    "{0}|{1}", 
                                    drCurrentModuleDefinitions["ModuleDefID"], 
                                    GetHelpPath(drCurrentModuleDefinitions["DesktopSrc"].ToString()))));

                        var actions = ModelServices.GetMVCActionModules();
                        foreach (var key in actions.Keys)
                        {
                            this.moduleType.Items.Add(new ListItem(key, actions[key]));
                        }
                    }
                }

                // 				}
            }
            finally
            {
                drCurrentModuleDefinitions.Close();
            }
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private void InitializeComponent()
        {
            this.moduleType.SelectedIndexChanged += this.moduleType_SelectedIndexChanged;
            this.AddModuleBtn.Click += this.AddModule_Click;
        }

        /// <summary>
        /// Sets the datata.
        /// </summary>
        /// <param name="modulePath">
        /// The module path.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void SetDatata(string modulePath)
        {
            var folderName = modulePath;
            var start = folderName.IndexOf("|");
            folderName = folderName.Substring(start + 1);
            var fileNameStart = folderName.LastIndexOf("/");
            var fileName = folderName.Substring(fileNameStart + 1);
            var completePath = folderName + "/" + fileName;

            if (
                File.Exists(
                    HttpContext.Current.Server.MapPath(
                        string.Format("{0}/rb_documentation/{1}.xml", Path.ApplicationRoot, completePath))))
            {
                this.AddModuleHelp.Visible = true;
                var javaScript =
                    string.Format(
                        "HelpWindow=window.open('{0}/rb_documentation/Viewer.aspx?loc={1}&src={2}','HelpWindow','toolbar=no,location=no,directories=no,status=no,menubar=yes,scrollbars=yes,resizable=yes,width=640,height=480,left=15,top=15'); return false;", 
                        Path.ApplicationRoot, 
                        folderName, 
                        fileName);
                this.AddModuleHelp.Attributes.Add("onclick", javaScript);
                this.AddModuleHelp.Attributes.Add("style", "cursor: hand;");
                this.AddModuleHelp.NavigateUrl = string.Empty;
                this.AddModuleHelp.ImageUrl = this.CurrentTheme.GetImage("Buttons_Help", "Help.gif").ImageUrl;
                this.AddModuleHelp.ToolTip = this.moduleType.SelectedItem.Text + " Help";
            }
            else
            {
                this.AddModuleHelp.Visible = false;
            }
        }

        /// <summary>
        /// Sets the help path. This method checks to see whether the currently selected module has a
        ///   help file associated with it. If it does then it shows the help icon. If it doesn't then it
        ///   hides it.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private void SetHelpPath()
        {
            this.SetDatata(this.moduleType.SelectedValue);
        }

        /// <summary>
        /// Sets the name of the module.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private void SetModuleName()
        {
            // by Manu, set title like module name
            if (this.moduleType.SelectedItem != null)
            {
                //this.moduleTitle.Text = this.moduleType.SelectedItem.Text;
            }
        }

        #endregion
    }
}