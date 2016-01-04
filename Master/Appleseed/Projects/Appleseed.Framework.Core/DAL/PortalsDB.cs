// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortalsDB.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Class that encapsulates all data logic necessary to add/query/delete
//   Portals within the Portal database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Site.Data
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Web;

    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.Users.Data;

    using Path = Appleseed.Framework.Settings.Path;
    using System.Web.Profile;

    /// <summary>
    /// Class that encapsulates all data logic necessary to add/query/delete
    ///   Portals within the Portal database.
    /// </summary>
    public class PortalsDB
    {
        #region Constants and Fields

        /// <summary>
        ///   The str admins.
        /// </summary>
        private const string StrAdmins = "Admins;";

        /// <summary>
        ///   The str all users.
        /// </summary>
        private const string StrAllUsers = "All Users";

        /// <summary>
        ///   The str at always show edit button.
        /// </summary>
        private const string StrAtAlwaysShowEditButton = "@AlwaysShowEditButton";

        /// <summary>
        ///   The str at portal id.
        /// </summary>
        private const string StrAtPortalId = "@PortalID";

        /// <summary>
        ///   The str at portal name.
        /// </summary>
        private const string StrAtPortalName = "@PortalName";

        /// <summary>
        ///   The str at portal path.
        /// </summary>
        private const string StrAtPortalPath = "@PortalPath";

        /// <summary>
        ///   The str content pane.
        /// </summary>
        private const string StrContentPane = "ContentPane";

        /// <summary>
        ///   The str guid language switcher.
        /// </summary>
        private const string StrGuidLanguageSwitcher = "{25E3290E-3B9A-4302-9384-9CA01243C00F}";

        /// <summary>
        ///   The str guid login.
        /// </summary>
        private const string StrGuidLogin = "{A0F1F62B-FDC7-4DE5-BBAD-A5DAF31D960A}";

        /// <summary>
        ///   The str guid manage users.
        /// </summary>
        private const string StrGuidManageUsers = "{B6A48596-9047-4564-8555-61E3B31D7272}";

        /// <summary>
        ///   The str guid modules.
        /// </summary>
        private const string StrGuidModules = "{5E0DB0C7-FD54-4F55-ACF5-6ECF0EFA59C0}";

        /// <summary>
        ///   The str guid pages.
        /// </summary>
        private const string StrGuidPages = "{1C575D94-70FC-4A83-80C3-2087F726CBB3}";

        /// <summary>
        ///   The str guid security roles.
        /// </summary>
        private const string StrGuidSecurityRoles = "{A406A674-76EB-4BC1-BB35-50CD2C251F9C}";

        /// <summary>
        ///   The str guid site settings.
        /// </summary>
        private const string StrGuidSiteSettings = "{EBBB01B1-FBB5-4E79-8FC4-59BCA1D0554E}";

        /// <summary>
        ///   The str guidhtml document.
        /// </summary>
        private const string StrGuidhtmlDocument = "{0B113F51-FEA3-499A-98E7-7B83C192FDBB}";

        /// <summary>
        ///   The str left pane.
        /// </summary>
        private const string StrLeftPane = "LeftPane";

        // jes1111 - const string strPortalsDirectory = "PortalsDirectory";

        /// <summary>
        /// The strings admin.
        /// </summary>
        private const string StringsAdmin = "admin";

        /// <summary>
        /// The strings rb get portals.
        /// </summary>
        private const string StringsRbGetPortals = "rb_GetPortals";

        /// <summary>
        ///   The strings right pane.
        /// </summary>
        private const string StringsRightPane = "RightPane";

        #endregion

        /*
        /// <summary>
        /// Gets CurrentPortalSettings.
        /// </summary>
        private PortalSettings CurrentPortalSettings
        {
            get
            {
                return (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            }
        }
*/
        #region Public Methods

        /// <summary>
        /// The AddPortal method add a new portal.<br/>
        ///   AddPortal Stored Procedure
        /// </summary>
        /// <param name="portalAlias">
        /// The portal alias.
        /// </param>
        /// <param name="portalName">
        /// Name of the portal.
        /// </param>
        /// <param name="portalPath">
        /// The portal path.
        /// </param>
        /// <returns>
        /// The add portal.
        /// </returns>
        public int AddPortal(string portalAlias, string portalName, string portalPath)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            {
                using (var command = new SqlCommand("rb_AddPortal", connection))
                {
                    // Mark the Command as a SPROC
                    command.CommandType = CommandType.StoredProcedure;

                    // Add Parameters to SPROC
                    var parameterPortalAlias = new SqlParameter("@PortalAlias", SqlDbType.NVarChar, 128)
                        {
                           Value = portalAlias 
                        };
                    command.Parameters.Add(parameterPortalAlias);
                    var parameterPortalName = new SqlParameter(StrAtPortalName, SqlDbType.NVarChar, 128)
                        {
                           Value = portalName 
                        };
                    command.Parameters.Add(parameterPortalName);

                    // jes1111
                    // string pd = ConfigurationSettings.AppSettings[strPortalsDirectory];
                    // if(pd!=null)
                    // {
                    // if (portalPath.IndexOf (pd) > -1)
                    // portalPath = portalPath.Substring(portalPath.IndexOf (pd) + pd.Length);
                    // }
                    var pd = Config.PortalsDirectory;
                    if (portalPath.IndexOf(pd) > -1)
                    {
                        portalPath = portalPath.Substring(portalPath.IndexOf(pd) + pd.Length);
                    }

                    var parameterPortalPath = new SqlParameter(StrAtPortalPath, SqlDbType.NVarChar, 128)
                        {
                           Value = portalPath 
                        };
                    command.Parameters.Add(parameterPortalPath);
                    var parameterAlwaysShow = new SqlParameter(StrAtAlwaysShowEditButton, SqlDbType.Bit, 1)
                        {
                           Value = false 
                        };
                    command.Parameters.Add(parameterAlwaysShow);
                    var parameterPortalId = new SqlParameter(StrAtPortalId, SqlDbType.Int, 4)
                        {
                           Direction = ParameterDirection.Output 
                        };
                    command.Parameters.Add(parameterPortalId);
                    connection.Open();

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    finally
                    {
                        connection.Close();
                    }

                    return (int)parameterPortalId.Value;
                }
            }
        }

        /// <summary>
        /// The CreatePortal method create a new basic portal based on solutions table.
        /// </summary>
        /// <param name="solutionId">
        /// The solution ID.
        /// </param>
        /// <param name="portalAlias">
        /// The portal alias.
        /// </param>
        /// <param name="portalName">
        /// Name of the portal.
        /// </param>
        /// <param name="portalPath">
        /// The portal path.
        /// </param>
        /// <returns>
        /// The create portal.
        /// </returns>
        [History("john.mandia@whitelightsolutions.com", "2003/05/26", 
            "Added extra info so that sign in is added to home tab of new portal and language switcher is added to module list")]
        [History("bja@reedtek.com", "2003/05/16", "Added extra parameter for collapsible window")]
        public int CreatePortal(int solutionId, string portalAlias, string portalName, string portalPath)
        {
            var tabs = new PagesDB();
            var modules = new ModulesDB();

            // Create a new portal
            var portalId = this.AddPortal(portalAlias, portalName, portalPath);

            // get module definitions
            foreach (var solutionModuleDefinition in modules.GetSolutionModuleDefinitions(solutionId))
            {
                modules.UpdateModuleDefinitions(solutionModuleDefinition.GeneralModuleDefinitionId, portalId, true);
            }
            
            if (!Config.UseSingleUserBase)
            {
                const string AdminEmail = "admin@Appleseedportal.net";

                // Create the stradmin User for the new portal
                var user = new UsersDB();

                // Create the "Admins" role for the new portal
                var roleId = user.AddRole(portalAlias, "Admins");
                var userId = user.AddUser(StringsAdmin, AdminEmail, StringsAdmin, portalAlias);

                // Create the "Admins" profile for the new portal
                var profile = ProfileBase.Create(AdminEmail);
                profile.SetPropertyValue("Email", AdminEmail);
                profile.SetPropertyValue("Name", "admin");
                try {
                    profile.Save();

                } catch  {
                    
                }

                // Create a new row in a many to many table (userroles)
                // giving the "admins" role to the stradmin user
                user.AddUserRole(roleId, userId, portalAlias);
            }

            // Create a new Page "home"
            var homePageId = tabs.AddPage(portalId, "Home", 1);

            // Create a new Page "admin"
            var localizedString = General.GetString("ADMIN_TAB_NAME");
            var adminPageId = tabs.AddPage(portalId, localizedString, StrAdmins, 9999);

            // Add Modules for portal use
            // Html Document
            modules.UpdateModuleDefinitions(new Guid(StrGuidhtmlDocument), portalId, true);

            // Add Modules for portal administration
            // Site Settings (Admin)
            localizedString = General.GetString("MODULE_SITE_SETTINGS");
            modules.UpdateModuleDefinitions(new Guid(StrGuidSiteSettings), portalId, true);
            modules.AddModule(
                adminPageId, 
                1, 
                StrContentPane, 
                localizedString, 
                modules.GetModuleDefinitionByGuid(portalId, new Guid(StrGuidSiteSettings)), 
                0, 
                StrAdmins, 
                StrAllUsers, 
                StrAdmins, 
                StrAdmins, 
                StrAdmins, 
                StrAdmins, 
                StrAdmins, 
                false, 
                string.Empty, 
                false, 
                false, 
                false);

            // Pages (Admin)
            localizedString = General.GetString("MODULE_TABS");
            modules.UpdateModuleDefinitions(new Guid(StrGuidPages), portalId, true);
            modules.AddModule(
                adminPageId, 
                2, 
                StrContentPane, 
                localizedString, 
                modules.GetModuleDefinitionByGuid(portalId, new Guid(StrGuidPages)), 
                0, 
                StrAdmins, 
                StrAllUsers, 
                StrAdmins, 
                StrAdmins, 
                StrAdmins, 
                StrAdmins, 
                StrAdmins, 
                false, 
                string.Empty, 
                false, 
                false, 
                false);

            // Roles (Admin)
            localizedString = General.GetString("MODULE_SECURITY_ROLES");
            modules.UpdateModuleDefinitions(new Guid(StrGuidSecurityRoles), portalId, true);
            modules.AddModule(
                adminPageId, 
                3, 
                StrContentPane, 
                localizedString, 
                modules.GetModuleDefinitionByGuid(portalId, new Guid(StrGuidSecurityRoles)), 
                0, 
                StrAdmins, 
                StrAllUsers, 
                StrAdmins, 
                StrAdmins, 
                StrAdmins, 
                StrAdmins, 
                StrAdmins, 
                false, 
                string.Empty, 
                false, 
                false, 
                false);

            // Manage Users (Admin)
            localizedString = General.GetString("MODULE_MANAGE_USERS");
            modules.UpdateModuleDefinitions(new Guid(StrGuidManageUsers), portalId, true);
            modules.AddModule(
                adminPageId, 
                4, 
                StrContentPane, 
                localizedString, 
                modules.GetModuleDefinitionByGuid(portalId, new Guid(StrGuidManageUsers)), 
                0, 
                StrAdmins, 
                StrAllUsers, 
                StrAdmins, 
                StrAdmins, 
                StrAdmins, 
                StrAdmins, 
                StrAdmins, 
                false, 
                string.Empty, 
                false, 
                false, 
                false);

            // Module Definitions (Admin)
            localizedString = General.GetString("MODULE_MODULES");
            modules.UpdateModuleDefinitions(new Guid(StrGuidModules), portalId, true);
            modules.AddModule(
                adminPageId, 
                1, 
                StringsRightPane, 
                localizedString, 
                modules.GetModuleDefinitionByGuid(portalId, new Guid(StrGuidModules)), 
                0, 
                StrAdmins, 
                StrAllUsers, 
                StrAdmins, 
                StrAdmins, 
                StrAdmins, 
                StrAdmins, 
                StrAdmins, 
                false, 
                string.Empty, 
                false, 
                false, 
                false);

            // End Change Geert.Audenaert@Syntegra.Com
            // Change by john.mandia@whitelightsolutions.com
            // Add Signin Module and put it on the hometab
            // Signin
            localizedString = General.GetString("MODULE_LOGIN", "Login");
            modules.UpdateModuleDefinitions(new Guid(StrGuidLogin), portalId, true);
            modules.AddModule(
                homePageId, 
                -1, 
                StrLeftPane, 
                localizedString, 
                modules.GetModuleDefinitionByGuid(portalId, new Guid(StrGuidLogin)), 
                0, 
                StrAdmins, 
                "Unauthenticated Users;Admins;", 
                StrAdmins, 
                StrAdmins, 
                StrAdmins, 
                StrAdmins, 
                StrAdmins, 
                false, 
                string.Empty, 
                false, 
                false, 
                false);

            // Add language switcher to available modules
            // Language Switcher
            modules.UpdateModuleDefinitions(new Guid(StrGuidLanguageSwitcher), portalId, true);

            // End of change by john.mandia@whitelightsolutions.com
            // Create paths
            this.CreatePortalPath(portalPath);
            return portalId;
        }

        /// <summary>
        /// Creates the portal path.
        /// </summary>
        /// <param name="portalPath">
        /// The portal path.
        /// </param>
        public void CreatePortalPath(string portalPath)
        {
            portalPath = portalPath.Replace("/", string.Empty);
            portalPath = portalPath.Replace("\\", string.Empty);
            portalPath = portalPath.Replace(".", string.Empty);

            if (!portalPath.StartsWith("_"))
            {
                portalPath = "_" + portalPath;
            }

            // jes1111
            // string pd = ConfigurationSettings.AppSettings[strPortalsDirectory];
            // if(pd!=null)
            // {
            // if (portalPath.IndexOf (pd) > -1)
            // portalPath = portalPath.Substring(portalPath.IndexOf (pd) + pd.Length);
            // }
            var pd = Config.PortalsDirectory;
            if (portalPath.IndexOf(pd) > -1)
            {
                portalPath = portalPath.Substring(portalPath.IndexOf(pd) + pd.Length);
            }

            // jes1111 - string portalPhisicalDir = HttpContext.Current.Server.MapPath(Path.ApplicationRoot + "/" + ConfigurationSettings.AppSettings[strPortalsDirectory] + "/" + portalPath);
            var portalPhisicalDir =
                HttpContext.Current.Server.MapPath(
                    Path.WebPathCombine(Path.ApplicationRoot, Config.PortalsDirectory, portalPath));
            if (!Directory.Exists(portalPhisicalDir))
            {
                Directory.CreateDirectory(portalPhisicalDir);
            }

            // Subdirs
            string[] subdirs = { "images", "polls", "documents", "xml" };

            for (var i = 0; i <= subdirs.GetUpperBound(0); i++)
            {
                if (!Directory.Exists(portalPhisicalDir + "\\" + subdirs[i]))
                {
                    Directory.CreateDirectory(portalPhisicalDir + "\\" + subdirs[i]);
                }
            }
        }

        /// <summary>
        /// Removes portal from database. All tabs, modules and data wil be removed.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        public void DeletePortal(int portalId)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_DeletePortal", connection))
            {
                // Mark the Command as a SPROC
                command.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterPortalId = new SqlParameter(StrAtPortalId, SqlDbType.Int, 4) { Value = portalId };
                command.Parameters.Add(parameterPortalId);

                // Execute the command
                connection.Open();

                try
                {
                    command.ExecuteNonQuery();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// The GetPortals method returns a SqlDataReader containing all of the
        ///   Portals registered in this database.<br/>
        ///   GetPortals Stored Procedure
        /// </summary>
        /// <returns>
        /// a sql data reader
        /// </returns>
        public SqlDataReader GetPortals()
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;

            // Mark the Command as a SPROC
            var command = new SqlCommand("rb_GetPortals", connection) { CommandType = CommandType.StoredProcedure };

            // Execute the command
            connection.Open();
            var result = command.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the datareader 
            return result;
        }

        /// <summary>
        /// The GetPortals method returns an ArrayList containing all of the
        ///   Portals registered in this database.<br/>
        ///   GetPortals Stored Procedure
        /// </summary>
        /// <returns>
        /// a list of portals
        /// </returns>
        public ArrayList GetPortalsArrayList()
        {
            var portals = new ArrayList();

            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            {
                using (var command = new SqlCommand(StringsRbGetPortals, connection))
                {
                    // Mark the Command as a SPROC
                    command.CommandType = CommandType.StoredProcedure;

                    // Execute the command
                    connection.Open();

                    using (var dr = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        try
                        {
                            while (dr.Read())
                            {
                                var p = new PortalItem
                                    {
                                        Name = dr["PortalName"].ToString(), 
                                        Path = dr["PortalPath"].ToString(), 
                                        ID = Convert.ToInt32(dr["PortalID"].ToString())
                                    };
                                portals.Add(p);
                            }
                        }
                        finally
                        {
                            dr.Close(); // by Manu, fixed bug 807858
                            connection.Close(); //Added by Ashish - Connection Pool Issues
                        }
                    }

                    // Return the portals
                    return portals;
                }
            }
        }

        /// <summary>
        /// The GetTemplates method returns a SqlDataReader containing all of the
        ///   Templates Availables.
        /// </summary>
        /// <returns>
        /// A sql data reader.
        /// </returns>
        public SqlDataReader GetTemplates()
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;

            // Mark the Command as a SPROC
            var command = new SqlCommand(StringsRbGetPortals, connection) { CommandType = CommandType.StoredProcedure };

            // Execute the command
            connection.Open();
            var result = command.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the datareader 
            return result;
        }

        /// <summary>
        /// The UpdatePortalInfo method updates the name and access settings for the portal.<br/>
        ///   Uses UpdatePortalInfo Stored Procedure.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="portalName">
        /// Name of the portal.
        /// </param>
        /// <param name="portalPath">
        /// The portal path.
        /// </param>
        /// <param name="alwaysShow">
        /// if set to <c>true</c> [always show].
        /// </param>
        public void UpdatePortalInfo(int portalId, string portalName, string portalPath, bool alwaysShow)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            {
                using (var command = new SqlCommand("rb_UpdatePortalInfo", connection))
                {
                    // Mark the Command as a SPROC
                    command.CommandType = CommandType.StoredProcedure;

                    // Add Parameters to SPROC
                    var parameterPortalId = new SqlParameter(StrAtPortalId, SqlDbType.Int, 4) { Value = portalId };
                    command.Parameters.Add(parameterPortalId);
                    var parameterPortalName = new SqlParameter(StrAtPortalName, SqlDbType.NVarChar, 128)
                        {
                           Value = portalName 
                        };
                    command.Parameters.Add(parameterPortalName);

                    // jes1111
                    // string pd = ConfigurationSettings.AppSettings[strPortalsDirectory];
                    // if(pd!=null)
                    // {
                    // if (portalPath.IndexOf (pd) > -1)
                    // portalPath = portalPath.Substring(portalPath.IndexOf (pd) + pd.Length);
                    // }
                    var pd = Config.PortalsDirectory;
                    if (portalPath.IndexOf(pd) > -1)
                    {
                        portalPath = portalPath.Substring(portalPath.IndexOf(pd) + pd.Length);
                    }

                    var parameterPortalPath = new SqlParameter(StrAtPortalPath, SqlDbType.NVarChar, 128)
                        {
                           Value = portalPath 
                        };
                    command.Parameters.Add(parameterPortalPath);
                    var parameterAlwaysShow = new SqlParameter(StrAtAlwaysShowEditButton, SqlDbType.Bit, 1)
                        {
                           Value = alwaysShow 
                        };
                    command.Parameters.Add(parameterAlwaysShow);
                    connection.Open();

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// The UpdatePortalSetting Method updates a single module setting
        ///   in the PortalSettings database table.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="key">
        /// The key to update.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        [Obsolete("UpdatePortalSetting was moved in PortalSettings.UpdatePortalSetting", false)]
        public void UpdatePortalSetting(int portalId, string key, string value)
        {
            PortalSettings.UpdatePortalSetting(portalId, key, value);
        }

        #endregion
    }
}