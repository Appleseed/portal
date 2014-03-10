// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleSettings.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The module settings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Site.Configuration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web.UI;

    using Appleseed.Framework.Configuration.Settings;
    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Settings.Cache;
    using Appleseed.Framework.Web.UI.WebControls;

    /// <summary>
    /// The module settings.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class ModuleSettings : IModuleSettings, ISettingHolder
    {
        #region Constants and Fields

        /// <summary>
        ///   The string desktop source.
        /// </summary>
        private const string StringsDesktopSrc = "DesktopSrc";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleSettings"/> class.
        /// </summary>
        public ModuleSettings()
        {
            this.PaneName = "no pane";
            this.ModuleTitle = string.Empty;
            this.MobileSrc = string.Empty;
            this.DesktopSrc = string.Empty;
            this.CacheDependency = new ArrayList();
            this.AuthorizedViewRoles = "All Users;";
            this.AuthorizedPropertiesRoles = "Admin;";
            this.AuthorizedMoveModuleRoles = "Admin;";
            this.AuthorizedEditRoles = "Admin;";
            this.AuthorizedDeleteRoles = "Admin;";
            this.AuthorizedDeleteModuleRoles = "Admin;";
            this.AuthorizedAddRoles = "Admin;";
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets a value indicating whether this <see cref = "IModuleSettings" /> is admin.
        /// </summary>
        /// <value><c>true</c> if admin; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// </remarks>
        public bool Admin { get; set; }

        /// <summary>
        ///   Gets or sets the authorized add roles.
        /// </summary>
        /// <value>The authorized add roles.</value>
        /// <remarks>
        /// </remarks>
        public string AuthorizedAddRoles { get; set; }

        /// <summary>
        ///   Gets or sets the authorized approve roles.
        /// </summary>
        /// <value>The authorized approve roles.</value>
        /// <remarks>
        /// </remarks>
        public string AuthorizedApproveRoles { get; set; }

        /// <summary>
        ///   Gets or sets the authorized delete module roles.
        /// </summary>
        /// <value>The authorized delete module roles.</value>
        /// <remarks>
        /// </remarks>
        public string AuthorizedDeleteModuleRoles { get; set; }

        /// <summary>
        ///   Gets or sets the authorized delete roles.
        /// </summary>
        /// <value>The authorized delete roles.</value>
        /// <remarks>
        /// </remarks>
        public string AuthorizedDeleteRoles { get; set; }

        /// <summary>
        ///   Gets or sets the authorized edit roles.
        /// </summary>
        /// <value>The authorized edit roles.</value>
        /// <remarks>
        /// </remarks>
        public string AuthorizedEditRoles { get; set; }

        /// <summary>
        ///   Gets or sets the authorized move module roles.
        /// </summary>
        /// <value>The authorized move module roles.</value>
        /// <remarks>
        /// </remarks>
        public string AuthorizedMoveModuleRoles { get; set; }

        /// <summary>
        ///   Gets or sets the authorized properties roles.
        /// </summary>
        /// <value>The authorized properties roles.</value>
        /// <remarks>
        /// </remarks>
        public string AuthorizedPropertiesRoles { get; set; }

        /// <summary>
        ///   Gets or sets the authorized publishing roles.
        /// </summary>
        /// <value>The authorized publishing roles.</value>
        /// <remarks>
        /// </remarks>
        public string AuthorizedPublishingRoles { get; set; }

        /// <summary>
        ///   Gets or sets the authorized view roles.
        /// </summary>
        /// <value>The authorized view roles.</value>
        /// <remarks>
        /// </remarks>
        public string AuthorizedViewRoles { get; set; }

        /// <summary>
        ///   Gets or sets the cache dependency.
        /// </summary>
        /// <value>The cache dependency.</value>
        /// <remarks>
        /// </remarks>
        public ArrayList CacheDependency { get; set; }

        /// <summary>
        ///   Gets or sets the cache time.
        /// </summary>
        /// <value>The cache time.</value>
        /// <remarks>
        /// </remarks>
        public int CacheTime { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this <see cref = "IModuleSettings" /> is cacheable.
        /// </summary>
        /// <value><c>true</c> if cacheable; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// </remarks>
        public bool Cacheable { get; set; }

        /// <summary>
        ///   Gets or sets the desktop SRC.
        /// </summary>
        /// <value>The desktop SRC.</value>
        /// <remarks>
        /// </remarks>
        public string DesktopSrc { get; set; }

        /// <summary>
        ///   Gets or sets the GUID ID.
        /// </summary>
        /// <value>The GUID ID.</value>
        /// <remarks>
        /// </remarks>
        public Guid GuidID { get; set; }

        /// <summary>
        ///   Gets or sets the mobile SRC.
        /// </summary>
        /// <value>The mobile SRC.</value>
        /// <remarks>
        /// </remarks>
        public string MobileSrc { get; set; }

        /// <summary>
        ///   Gets or sets the module def ID.
        /// </summary>
        /// <value>The module def ID.</value>
        /// <remarks>
        /// </remarks>
        public int ModuleDefID { get; set; }

        /// <summary>
        ///   Gets or sets the module ID.
        /// </summary>
        /// <value>The module ID.</value>
        /// <remarks>
        /// </remarks>
        public int ModuleID { get; set; }

        /// <summary>
        ///   Gets or sets the module order.
        /// </summary>
        /// <value>The module order.</value>
        /// <remarks>
        /// </remarks>
        public int ModuleOrder { get; set; }

        /// <summary>
        ///   Gets or sets the module title.
        /// </summary>
        /// <value>The module title.</value>
        /// <remarks>
        /// </remarks>
        public string ModuleTitle { get; set; }

        /// <summary>
        ///   Gets or sets the page ID.
        /// </summary>
        /// <value>The page ID.</value>
        /// <remarks>
        /// </remarks>
        public int PageID { get; set; }

        /// <summary>
        ///   Gets or sets the name of the pane.
        /// </summary>
        /// <value>The name of the pane.</value>
        /// <remarks>
        /// </remarks>
        public string PaneName { get; set; }

        /// <summary>
        ///   Gets the settings.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public IDictionary<string, ISettingItem> Settings
        {
            get
            {
                return GetModuleSettings(this.ModuleID);
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether [show every where].
        /// </summary>
        /// <value><c>true</c> if [show every where]; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// </remarks>
        public bool ShowEveryWhere { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether [show mobile].
        /// </summary>
        /// <value><c>true</c> if [show mobile]; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// </remarks>
        public bool ShowMobile { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether [support collapsible].
        /// </summary>
        /// <value><c>true</c> if [support collapsible]; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// </remarks>
        public bool SupportCollapsable { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether [support workflow].
        /// </summary>
        /// <value><c>true</c> if [support workflow]; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// </remarks>
        public bool SupportWorkflow { get; set; }

        /// <summary>
        ///   Gets or sets the workflow status.
        /// </summary>
        /// <value>The workflow status.</value>
        /// <remarks>
        /// </remarks>
        public WorkflowState WorkflowStatus { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the module definition by ID.
        /// </summary>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static SqlDataReader GetModuleDefinitionByID(int moduleId)
        {
            var sqlConnectionString = Config.SqlConnectionString;
            var command = new SqlCommand("rb_GetModuleDefinitionByID", sqlConnectionString)
                {
                   CommandType = CommandType.StoredProcedure 
                };
            var parameter = new SqlParameter("@ModuleID", SqlDbType.Int) { Value = moduleId };
            command.Parameters.Add(parameter);
            sqlConnectionString.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Gets the module desktop SRC.
        /// </summary>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <returns>
        /// The get module desktop src.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static string GetModuleDesktopSrc(int moduleId)
        {
            var str = string.Empty;
            using (var reader = GetModuleDefinitionByID(moduleId))
            {
                if (reader.Read())
                {
                    str = Path.WebPathCombine(new[] { Path.ApplicationRoot, reader["DesktopSrc"].ToString() });
                }
            }

            return str;
        }

        /// <summary>
        /// Gets the module settings.
        /// </summary>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static Dictionary<string, ISettingItem> GetModuleSettings(int moduleId)
        {
            return GetModuleSettings(moduleId, new Page());
        }

        /// <summary>
        /// Gets module settings.
        /// </summary>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <param name="baseSettings">
        /// The base settings.
        /// </param>
        /// <returns>
        /// A hash table.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static Dictionary<string, ISettingItem> GetModuleSettings(
            int moduleId, Dictionary<string, ISettingItem> baseSettings)
        {
            if (!CurrentCache.Exists(Key.ModuleSettings(moduleId)))
            {
                var hashtable = new Hashtable();
                using (var connection = Config.SqlConnectionString)
                using (var command = new SqlCommand("rb_GetModuleSettings", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    var parameter = new SqlParameter("@ModuleID", SqlDbType.Int, 4) { Value = moduleId };
                    command.Parameters.Add(parameter);
                    connection.Open();
                    using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            hashtable[reader["SettingName"].ToString()] = reader["SettingValue"].ToString();
                        }
                    }
                }

                foreach (var key in
                    baseSettings.Keys.Where(key => hashtable[key] != null).Where(
                        key => hashtable[key].ToString().Length != 0))
                {
                    baseSettings[key].Value = hashtable[key];
                }

                CurrentCache.Insert(Key.ModuleSettings(moduleId), baseSettings);
                return baseSettings;
            }

            baseSettings = (Dictionary<string, ISettingItem>)CurrentCache.Get(Key.ModuleSettings(moduleId));
            return baseSettings;
        }

        /// <summary>
        /// Gets the module settings.
        /// </summary>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static Dictionary<string, ISettingItem> GetModuleSettings(int moduleId, Page page)
        {
            var virtualPath = Path.ApplicationRoot + "/";
            var desktopSrc = string.Empty;
            using (var reader = GetModuleDefinitionByID(moduleId))
            {
                if (reader.Read())
                {
                    desktopSrc = reader[StringsDesktopSrc].ToString();
                }
            }

            virtualPath += desktopSrc;

            Dictionary<string, ISettingItem> moduleSettings;
            try
            {
                PortalModuleControl control;
                if (!virtualPath.ToLower().Contains("/areas/"))
                {
                    control = (PortalModuleControl)page.LoadControl(virtualPath);
                }
                else
                {
                    if (Path.ApplicationRoot != "")
                        virtualPath = virtualPath.Replace(Path.ApplicationRoot, "");
                    var strArray = virtualPath.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                    var areaName = (strArray[1].ToLower() == "views") ? string.Empty : strArray[1];
                    var controllerName = strArray[strArray.Length - 2];
                    var actionName = strArray[strArray.Length - 1];
                    var ns = strArray[2];

                    // string query = string.Format("?area={0}&controller={1}&action={2}", areaName, controllerName, actionName);
                    control = (PortalModuleControl)page.LoadControl(Path.ApplicationRoot + "/DesktopModules/CoreModules/MVC/MVCModule.ascx");
                    ((MVCModuleControl)control).ControllerName = controllerName;
                    ((MVCModuleControl)control).ActionName = actionName;
                    ((MVCModuleControl)control).AreaName = areaName;
                    ((MVCModuleControl)control).Initialize();
                }

                moduleSettings = GetModuleSettings(moduleId, control.BaseSettings);
                control.InitializeCustomSettings();
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format("There was a problem loading: '{0}'", virtualPath), exception);
            }

            return moduleSettings;
        }

        /// <summary>
        /// Updates the module setting.
        /// </summary>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void UpdateModuleSetting(int moduleId, string key, string value)
        {
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_UpdateModuleSetting", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                var parameter = new SqlParameter("@ModuleID", SqlDbType.Int, 4) { Value = moduleId };
                command.Parameters.Add(parameter);
                var parameter2 = new SqlParameter("@SettingName", SqlDbType.NVarChar, 50) { Value = key };
                command.Parameters.Add(parameter2);
                var parameter3 = new SqlParameter("@SettingValue", SqlDbType.NVarChar, 0x5dc) { Value = value };
                command.Parameters.Add(parameter3);
                connection.Open();
                command.ExecuteNonQuery();
            }

            CurrentCache.Remove(Key.ModuleSettings(moduleId));
        }

        #endregion

        #region Implemented Interfaces

        #region ISettingHolder

        /// <summary>
        /// Inserts or updates the setting.
        /// </summary>
        /// <param name="settingItem">
        /// The setting item.
        /// </param>
        /// <remarks>
        /// </remarks>
        public void Upsert(ISettingItem settingItem)
        {
            UpdateModuleSetting(this.ModuleID, settingItem.EnglishName, settingItem.Value.ToString());
        }

        #endregion

        #endregion
    }
}