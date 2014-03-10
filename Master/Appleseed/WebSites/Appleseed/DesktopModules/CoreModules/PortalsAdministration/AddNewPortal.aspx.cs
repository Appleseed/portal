using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Appleseed.Framework;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Settings.Cache;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Site.Data;
using Appleseed.Framework.Users.Data;
using Appleseed.Framework.Web.UI;
using Appleseed.Framework.Web.UI.WebControls;
using History = Appleseed.Framework.History;
using Appleseed.Framework.Providers.AppleseedSiteMapProvider;
using Appleseed.PortalTemplate;
using System.Collections.Generic;
using System.IO;

namespace Appleseed.AdminAll
{
    using System.Web.UI.WebControls;

    /// <summary>
    /// New portal wizard
    /// </summary>
    [History("jminond", "march 2005", "Changes for moving Tab to Page")]
    [History("Mario Endara", "2004/10/14", "Now can create a Portal based on other Portal (Roles, Tabs & Modules)")]
    public partial class AddNewPortal : AddItemPage
    {
        private struct moduleTemplate
        {
            public int id;
            public Guid GuidID;
        }

        private struct tabTemplate
        {
            public int oldID;
            public int newID;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            string editPortal = Request.Params["selectedTemplate"];

            if (editPortal != null)
            {
                if (Page.IsPostBack == false)
                {
                    List<String> lstPortal = new List<string>();
                    lstPortal.Add(editPortal);
                    ddlXMLTemplates.DataSource = lstPortal;
                    ddlXMLTemplates.DataBind();
                }
            }
            else
            {
                // Verify that the current user has access to access this page
                // Removed by Mario Endara <mario@softworks.com.uy> (2004/11/04)
                //            if (PortalSecurity.IsInRoles("Admins") == false) 
                //                PortalSecurity.AccessDeniedEdit();
                // If this is the first visit to the page, populate the site data
                if (Page.IsPostBack == false)
                {
                    var templateServices = PortalTemplateFactory.GetPortalTemplateServices(new PortalTemplateRepository());


                    ddlXMLTemplates.DataSource = templateServices.GetTemplates(PortalSettings.PortalAlias, PortalSettings.PortalFullPath);
                    ddlXMLTemplates.DataBind();
                    if (ddlXMLTemplates.Items.Count != 0)
                    {
                        ddlXMLTemplates.SelectedIndex = 0;
                    }
                    else
                    {
                        chkUseXMLTemplate.Enabled = false;
                    }

                }
            }
            var chkbox = Request.Params["chkUseXMLTemplate"];

            if (chkbox != null)
            {
                chkUseXMLTemplate.Checked = bool.Parse(Request.Params["chkUseXMLTemplate"]);
            }
            if (chkUseXMLTemplate.Checked == false)
            {
                // Don't use a template portal, so show the EditTable
                // Remove the cache that can be setted by the new Portal, to get a "clean" PortalBaseSetting
                CurrentCache.Remove(Key.PortalBaseSettings());
                EditTable.DataSource = new SortedList(PortalSettings.GetPortalBaseSettings(null));
                EditTable.DataBind();
                EditTable.Visible = true;
                ddlXMLTemplates.Enabled = false;
            }
            else
            {
                EditTable.Visible = false;
                ddlXMLTemplates.Enabled = true;
            }
        }
        
    
        /// <summary>
        /// Set the module guids with free access to this page
        /// </summary>
        /// <value>The allowed modules.</value>
        protected override List<string> AllowedModules
        {
            get
            {
                List<string> al = new List<string>();
                al.Add("366C247D-4CFB-451D-A7AE-649C83B05841");
                return al;
            }
        }

        /// <summary>
        /// OnUpdate
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        protected override void OnUpdate(EventArgs e)
        {
            base.OnUpdate(e);

            if (Page.IsValid) {
                //Get Solutions
                PortalsDB portals = new PortalsDB();

                try {
                    PathField.Text = PathField.Text.Replace("/", string.Empty);
                    PathField.Text = PathField.Text.Replace("\\", string.Empty);
                    PathField.Text = PathField.Text.Replace(".", string.Empty);

                    if (!chkUseXMLTemplate.Checked) {
                        // Create portal the "old" way
                        int NewPortalID =
                            portals.CreatePortal(this.PortalSettings.PortalID, AliasField.Text, TitleField.Text, PathField.Text);

                        // Update custom settings in the database
                        EditTable.ObjectID = NewPortalID;
                        EditTable.UpdateControls();
                    } else {

                        bool createdOk = true;
                        int newPortalID = CreatePortal(out createdOk);
                        if (!createdOk) {
                            string aux = General.GetString("NEW_PORTAL_ERROR", "There was an error on creating the portal", this);

                            ErrorMessage.Visible = true;
                            ErrorMessage.Text = aux + "<br>";
                            return;
                        }
                    }


                    // Redirect back to calling page
                    RedirectBackToReferringPage();
                } catch (Exception ex) {
                    string aux =
                        General.GetString("NEW_PORTAL_ERROR", "There was an error on creating the portal", this);
                    ErrorHandler.Publish(LogLevel.Error, aux, ex);

                    ErrorMessage.Visible = true;
                    ErrorMessage.Text = aux + "<br>";
                }
            }
        }

        private int CreatePortal(out bool createdOk)
        {
            string fileName = ddlXMLTemplates.Text;
            string portalAlias = AliasField.Text;
            string portalName = TitleField.Text;
            string portalPath = "/" + PathField.Text;
            IPortalTemplateRepository repository = new PortalTemplateRepository();
            IPortalTemplateServices services = PortalTemplateFactory.GetPortalTemplateServices(repository);
            int newPortalID = 1;

            createdOk = services.DeserializePortal(fileName, portalName, portalAlias, portalPath, PortalSettings.PortalFullPath, out newPortalID);
            if (createdOk && !Config.UseSingleUserBase) {
                string AdminEmail = "admin@Appleseedportal.net";

                // Create the stradmin User for the new portal
                UsersDB User = new UsersDB();
                // Create the "Admins" role for the new portal
                Guid roleID = User.AddRole(portalAlias, "Admins");
                Guid userID = User.AddUser("admin", AdminEmail, "admin", portalAlias);
                // Create a new row in a many to many table (userroles)
                // giving the "admins" role to the stradmin user
                User.AddUserRole(roleID, userID, portalAlias);
                PortalsDB portals = new PortalsDB();

                portals.CreatePortalPath(portalPath);
            }
            return newPortalID;
        }

        /// <summary>
        /// Handles the UpdateControl event of the EditTable control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:Appleseed.Framework.Web.UI.WebControls.SettingsTableEventArgs"/> instance containing the event data.</param>
        private void EditTable_UpdateControl(object sender, SettingsTableEventArgs e)
        {
            var edt = (SettingsTable)sender;
            PortalSettings.UpdatePortalSetting(edt.ObjectID, ((ISettingItem)e.CurrentItem).EditControl.ID, ((ISettingItem)e.CurrentItem).Value.ToString());
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <returns></returns>
        private SqlConnection GetConnection()
        {
            // Watch if there's a Template's database in the config.sys
            // else, use the same database as the portal
            //jes1111- string portalSqlConnectionID = "PortalTemplatesConnectionString";
            string strSqlConnection;

            //jes1111 - if(ConfigurationSettings.AppSettings[portalSqlConnectionID] != null)
            if (Config.PortalTemplatesConnectionString.Length != 0)
                //jes1111 - strSqlConnection = ConfigurationSettings.AppSettings[portalSqlConnectionID];
                strSqlConnection = Config.PortalTemplatesConnectionString;
            else
                //jes1111 - strSqlConnection = ConfigurationSettings.AppSettings["ConnectionString"];
                strSqlConnection = Config.ConnectionString;

            return (new SqlConnection(strSqlConnection));
        }

        /// <summary>
        /// Gets the portals.
        /// </summary>
        /// <returns></returns>
        private DataSet GetPortals()
        {
            // Create Instance of Connection and Command Object
            SqlConnection myConnection = GetConnection();
            string selectSQL = "SELECT PortalID, PortalAlias from rb_Portals WHERE PortalID >= 0";
            SqlDataAdapter myCommand = new SqlDataAdapter(selectSQL, myConnection);

            // Create and Fill the DataSet
            DataSet myDataSet = new DataSet();
            try {
                myCommand.Fill(myDataSet);
            } finally {
                myCommand.Dispose();
                myConnection.Close();
                myConnection.Dispose();
            }
            // Return the dataset
            return myDataSet;
        }

        /// <summary>
        /// Creates the portal.
        /// </summary>
        /// <param name="templateID">The template ID.</param>
        /// <param name="templateAlias">The template alias.</param>
        /// <param name="portalAlias">The portal alias.</param>
        /// <param name="portalName">Name of the portal.</param>
        /// <param name="portalPath">The portal path.</param>
        /// <returns></returns>
        private int CreatePortal(int templateID, string templateAlias, string portalAlias, string portalName,
                                 string portalPath)
        {
            int newPortalID;

            PortalsDB portals = new PortalsDB();
            PagesDB tabs = new PagesDB();
            ModulesDB modules = new ModulesDB();
            UsersDB users = new UsersDB();

            // create an Array to stores modules ID and GUID for finding them later
            ArrayList templateModules = new ArrayList();
            moduleTemplate module;
            // create an Array to stores tabs ID for finding them later
            ArrayList templateTabs = new ArrayList();
            tabTemplate tab;

            // Create a new portal
            newPortalID = portals.AddPortal(portalAlias, portalName, portalPath);

            // Open the connection to the PortalTemplates Database
            SqlConnection myConnection = GetConnection();
            SqlConnection my2ndConnection = GetConnection();
            SqlConnection my3rdConnection = GetConnection();
            myConnection.Open();
            my2ndConnection.Open();
            my3rdConnection.Open();

            // get module definitions and save them in the new portal
            SqlDataReader myReader = GetTemplateModuleDefinitions(templateID, myConnection);

            // Always call Read before accessing data.
            while (myReader.Read()) {
                module.id = (int)myReader["ModuleDefID"];
                module.GuidID = GetGeneralModuleDefinitionByName(myReader["FriendlyName"].ToString(), my2ndConnection);
                try {
                    // save module definitions in the new portal
                    modules.UpdateModuleDefinitions(module.GuidID, newPortalID, true);
                    // Save the modules into a list for finding them later
                    templateModules.Add(module);
                } catch {
                    // tried to add a Module thas doesn´t exists in this implementation of the portal
                }
            }

            myReader.Close();

            // TODO: Is this still valid? Admin user will be created the first time the portal is accessed
            //if (!Config.UseSingleUserBase)
            //{
            //    // TODO: multiple portals still not supported
            //    Guid userID;

            //    // Create the "admin" User for the new portal
            //    string AdminEmail = "admin@Appleseedportal.net";
            //    userID = users.AddUser("admin", AdminEmail, "admin", newPortalID);

            //    // Create a new row in a many to many table (userroles)
            //    // giving the "admins" role to the "admin" user
            //    users.AddUserRole("admin", userID);
            //}

            // Get all the Tabs in the Template Portal, store IDs in a list for finding them later
            // and create the Tabs in the new Portal
            myReader = GetTabsByPortal(templateID, myConnection);

            // Always call Read before accessing data.
            while (myReader.Read()) {
                // Save the tabs into a list for finding them later
                tab.oldID = (int)myReader["PageID"];
                tab.newID =
                    tabs.AddPage(newPortalID, myReader["PageName"].ToString(),
                                 Int32.Parse(myReader["PageOrder"].ToString()));
                templateTabs.Add(tab);
            }
            myReader.Close();

            //Clear SiteMaps Cache
            AppleseedSiteMapProvider.ClearAllAppleseedSiteMapCaches();

            // now I have to get them again to set up the ParentID for each Tab
            myReader = GetTabsByPortal(templateID, myConnection);

            // Always call Read before accessing data.
            while (myReader.Read()) {
                // Find the news TabID and ParentTabID
                IEnumerator myEnumerator = templateTabs.GetEnumerator();
                int newTabID = -1;
                int newParentTabID = -1;

                while (myEnumerator.MoveNext() && (newTabID == -1 || newParentTabID == -1)) {
                    tab = (tabTemplate)myEnumerator.Current;
                    if (tab.oldID == (int)myReader["PageID"])
                        newTabID = tab.newID;
                    if (tab.oldID == Int32.Parse("0" + myReader["ParentPageID"]))
                        newParentTabID = tab.newID;
                }

                if (newParentTabID == -1)
                    newParentTabID = 0;

                // Update the Tab in the new portal
                tabs.UpdatePage(newPortalID, newTabID, newParentTabID, myReader["PageName"].ToString(),
                                Int32.Parse(myReader["PageOrder"].ToString()), myReader["AuthorizedRoles"].ToString(),
                                myReader["MobilePageName"].ToString(), (bool)myReader["ShowMobile"]);

                // Finally use GetPortalSettings to access each Tab and its Modules in the Template Portal
                // and create them in the new Portal
                SqlDataReader result;

                try {
                    result = GetPageModules(Int32.Parse(myReader["PageID"].ToString()), my2ndConnection);

                    object myValue;

                    while (result.Read()) {
                        ModuleSettings m = new ModuleSettings();
                        m.ModuleID = (int)result["ModuleID"];
                        m.ModuleDefID = (int)result["ModuleDefID"];
                        m.PageID = newTabID;
                        m.PaneName = (string)result["PaneName"];
                        m.ModuleTitle = (string)result["ModuleTitle"];

                        myValue = result["AuthorizedEditRoles"];
                        m.AuthorizedEditRoles = !Convert.IsDBNull(myValue) ? (string)myValue : string.Empty;

                        myValue = result["AuthorizedViewRoles"];
                        m.AuthorizedViewRoles = !Convert.IsDBNull(myValue) ? (string)myValue : string.Empty;

                        myValue = result["AuthorizedAddRoles"];
                        m.AuthorizedAddRoles = !Convert.IsDBNull(myValue) ? (string)myValue : string.Empty;

                        myValue = result["AuthorizedDeleteRoles"];
                        m.AuthorizedDeleteRoles = !Convert.IsDBNull(myValue) ? (string)myValue : string.Empty;

                        myValue = result["AuthorizedPropertiesRoles"];
                        m.AuthorizedPropertiesRoles = !Convert.IsDBNull(myValue) ? (string)myValue : string.Empty;

                        myValue = result["AuthorizedMoveModuleRoles"];
                        m.AuthorizedMoveModuleRoles = !Convert.IsDBNull(myValue) ? (string)myValue : string.Empty;

                        myValue = result["AuthorizedDeleteModuleRoles"];
                        m.AuthorizedDeleteModuleRoles = !Convert.IsDBNull(myValue) ? (string)myValue : string.Empty;

                        myValue = result["AuthorizedPublishingRoles"];
                        m.AuthorizedPublishingRoles = !Convert.IsDBNull(myValue) ? (string)myValue : string.Empty;

                        myValue = result["SupportWorkflow"];
                        m.SupportWorkflow = !Convert.IsDBNull(myValue) ? (bool)myValue : false;

                        myValue = result["AuthorizedApproveRoles"];
                        m.AuthorizedApproveRoles = !Convert.IsDBNull(myValue) ? (string)myValue : string.Empty;

                        myValue = result["WorkflowState"];
                        m.WorkflowStatus = !Convert.IsDBNull(myValue)
                                               ? (WorkflowState)(0 + (byte)myValue)
                                               : WorkflowState.Original;

                        try {
                            myValue = result["SupportCollapsable"];
                        } catch {
                            myValue = DBNull.Value;
                        }
                        m.SupportCollapsable = DBNull.Value != myValue ? (bool)myValue : false;

                        try {
                            myValue = result["ShowEveryWhere"];
                        } catch {
                            myValue = DBNull.Value;
                        }
                        m.ShowEveryWhere = DBNull.Value != myValue ? (bool)myValue : false;

                        m.CacheTime = int.Parse(result["CacheTime"].ToString());
                        m.ModuleOrder = int.Parse(result["ModuleOrder"].ToString());

                        myValue = result["ShowMobile"];
                        m.ShowMobile = !Convert.IsDBNull(myValue) ? (bool)myValue : false;

                        // Find the new ModuleDefID assigned to the module in the new portal
                        myEnumerator = templateModules.GetEnumerator();
                        int newModuleDefID = 0;

                        while (myEnumerator.MoveNext() && newModuleDefID == 0) {
                            module = (moduleTemplate)myEnumerator.Current;
                            if (module.id == m.ModuleDefID)
                                newModuleDefID = modules.GetModuleDefinitionByGuid(newPortalID, module.GuidID);
                        }

                        if (newModuleDefID > 0) {
                            // add the module to the new tab
                            int newModuleID = modules.AddModule(newTabID, m.ModuleOrder, m.PaneName, m.ModuleTitle,
                                                                newModuleDefID, m.CacheTime, m.AuthorizedEditRoles,
                                                                m.AuthorizedViewRoles,
                                                                m.AuthorizedAddRoles, m.AuthorizedDeleteRoles,
                                                                m.AuthorizedPropertiesRoles,
                                                                m.AuthorizedMoveModuleRoles,
                                                                m.AuthorizedDeleteModuleRoles,
                                                                m.ShowMobile, m.AuthorizedPublishingRoles,
                                                                m.SupportWorkflow,
                                                                m.ShowEveryWhere, m.SupportCollapsable);
                            // At the end, get all ModuleSettings and save them in the new module
                            SqlDataReader dr = GetModuleSettings(m.ModuleID, my3rdConnection);

                            while (dr.Read()) {
                                Framework.Site.Configuration.ModuleSettings.UpdateModuleSetting(newModuleID, dr["SettingName"].ToString(),
                                                                   dr["SettingValue"].ToString());
                            }
                            dr.Close();
                        }
                    }

                    result.Close();
                } catch {
                    // Error? ignore Tab ...
                }
            }
            myReader.Close();

            // Set the CustomSettings of the New Portal based in the Template Portal
            myReader = GetPortalCustomSettings(templateID, myConnection);

            // Always call Read before accessing data.
            while (myReader.Read()) {
                PortalSettings.UpdatePortalSetting(newPortalID, myReader["SettingName"].ToString(),
                                                   myReader["SettingValue"].ToString());
            }

            myReader.Close();

            // close the conections
            myConnection.Close();
            myConnection.Dispose();
            my2ndConnection.Close();
            my2ndConnection.Dispose();
            my3rdConnection.Close();
            my3rdConnection.Dispose();

            // Create paths
            portals.CreatePortalPath(portalPath);

            return newPortalID;
        }

        /// <summary>
        /// Gets the template module definitions.
        /// </summary>
        /// <param name="templateID">The template ID.</param>
        /// <param name="myConnection">My connection.</param>
        /// <returns></returns>
        private SqlDataReader GetTemplateModuleDefinitions(int templateID, SqlConnection myConnection)
        {
            SqlCommand myCommand = new SqlCommand("rb_GetCurrentModuleDefinitions", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterPortalID = new SqlParameter("@PortalID", SqlDbType.Int, 4);
            parameterPortalID.Value = templateID;
            myCommand.Parameters.Add(parameterPortalID);

            // execute the command
            SqlDataReader dr = myCommand.ExecuteReader();

            // Return the datareader
            return dr;
        }

        /// <summary>
        /// Gets the portal roles.
        /// </summary>
        /// <param name="templateID">The template ID.</param>
        /// <param name="myConnection">My connection.</param>
        /// <returns></returns>
        private SqlDataReader GetPortalRoles(int templateID, SqlConnection myConnection)
        {

            // Create Instance of Command Object
            SqlCommand myCommand = new SqlCommand("rb_GetPortalRoles", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterPortalID = new SqlParameter("@PortalID", SqlDbType.Int, 4);
            parameterPortalID.Value = templateID;
            myCommand.Parameters.Add(parameterPortalID);

            // execute the command
            SqlDataReader dr = myCommand.ExecuteReader();

            // Return the datareader
            return dr;
        }

        /// <summary>
        /// Gets the portal custom settings.
        /// </summary>
        /// <param name="templateID">The template ID.</param>
        /// <param name="myConnection">My connection.</param>
        /// <returns></returns>
        private SqlDataReader GetPortalCustomSettings(int templateID, SqlConnection myConnection)
        {
            // Create Instance of Command Object
            SqlCommand myCommand = new SqlCommand("rb_GetPortalCustomSettings", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterPortalID = new SqlParameter("@PortalID", SqlDbType.Int, 4);
            parameterPortalID.Value = templateID;
            myCommand.Parameters.Add(parameterPortalID);

            // execute the command
            SqlDataReader dr = myCommand.ExecuteReader();

            // Return the datareader
            return dr;
        }

        /// <summary>
        /// Gets the name of the general module definition by.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="myConnection">My connection.</param>
        /// <returns></returns>
        private Guid GetGeneralModuleDefinitionByName(string moduleName, SqlConnection myConnection)
        {
            // Instance of Command Object
            SqlCommand myCommand = new SqlCommand("rb_GetGeneralModuleDefinitionByName", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterFriendlyName = new SqlParameter("@FriendlyName", SqlDbType.NVarChar, 128);
            parameterFriendlyName.Value = moduleName;
            myCommand.Parameters.Add(parameterFriendlyName);

            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.UniqueIdentifier);
            parameterModuleID.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterModuleID);

            // Execute the command
            myCommand.ExecuteNonQuery();

            if (parameterModuleID.Value != null && parameterModuleID.Value.ToString().Length != 0) {
                try {
                    return new Guid(parameterModuleID.Value.ToString());
                } catch (Exception ex) {
                    ErrorHandler.Publish(LogLevel.Error,
                                         "'" + parameterModuleID.Value.ToString() + "' seems not a valid GUID.", ex);
                    throw;
                }
            } else {
                ErrorHandler.Publish(LogLevel.Error, "Null GUID!.", new ArgumentException("Null GUID!", "GUID"));
            }
            throw new ArgumentException("Invalid GUID", "GUID");
        }

        /// <summary>
        /// Gets the page modules.
        /// </summary>
        /// <param name="TabID">The tab ID.</param>
        /// <param name="myConnection">My connection.</param>
        /// <returns></returns>
        private SqlDataReader GetPageModules(int TabID, SqlConnection myConnection)
        {
            string selectSQL = "select ModuleID, ModuleDefID, ModuleOrder, PaneName, ModuleTitle, " +
                               "AuthorizedEditRoles, AuthorizedViewRoles, AuthorizedAddRoles, " +
                               "AuthorizedDeleteRoles, AuthorizedPropertiesRoles, CacheTime, " +
                               "ShowMobile, AuthorizedPublishingRoles, SupportWorkflow, " +
                               "AuthorizedApproveRoles, WorkflowState, SupportCollapsable, " +
                               "ShowEveryWhere, AuthorizedMoveModuleRoles, AuthorizedDeleteModuleRoles " +
                               "from rb_Modules where TabID=" + TabID.ToString();
            SqlCommand myCommand = new SqlCommand(selectSQL, myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.Text;

            // execute the command
            SqlDataReader dr = myCommand.ExecuteReader();

            // Return the datareader
            return dr;
        }

        /// <summary>
        /// Gets the module settings.
        /// </summary>
        /// <param name="moduleID">The module ID.</param>
        /// <param name="myConnection">My connection.</param>
        /// <returns></returns>
        private SqlDataReader GetModuleSettings(int moduleID, SqlConnection myConnection)
        {
            SqlCommand myCommand = new SqlCommand("rb_GetModuleSettings", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = moduleID;
            myCommand.Parameters.Add(parameterModuleID);

            // Execute the command
            SqlDataReader dr = myCommand.ExecuteReader();

            return dr;
        }

        /// <summary>
        /// Gets the tabs by portal.
        /// </summary>
        /// <param name="templateID">The template ID.</param>
        /// <param name="myConnection">My connection.</param>
        /// <returns></returns>
        private SqlDataReader GetTabsByPortal(int templateID, SqlConnection myConnection)
        {
            SqlCommand myCommand = new SqlCommand("rb_GetTabsByPortal", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterPortalID = new SqlParameter("@PortalID", SqlDbType.Int, 4);
            parameterPortalID.Value = templateID;
            myCommand.Parameters.Add(parameterPortalID);

            // Execute the command
            SqlDataReader dr = myCommand.ExecuteReader();

            return dr;
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises the Init event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.EditTable.UpdateControl +=
                new Appleseed.Framework.Web.UI.WebControls.UpdateControlEventHandler(this.EditTable_UpdateControl);
            this.Load += new EventHandler(this.Page_Load);

            //Translations
            RequiredTitle.ErrorMessage = General.GetString("VALID_FIELD");
            RequiredAlias.ErrorMessage = General.GetString("VALID_FIELD");
            RequiredSitepath.ErrorMessage = General.GetString("VALID_FIELD");

            base.OnInit(e);
        }

        #endregion
    }
}
