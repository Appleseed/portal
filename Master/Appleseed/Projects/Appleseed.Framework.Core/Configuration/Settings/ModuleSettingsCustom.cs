// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleSettingsCustom.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   ModuleSettingsCustom extends the ModuleSettings class to allow authenticated users
//   to 'customize' a module to their own preference.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Site.Configuration
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Appleseed.Framework.Exceptions;
    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Web.UI.WebControls;

    /// <summary>
    /// ModuleSettingsCustom extends the ModuleSettings class to allow authenticated users
    ///   to 'customize' a module to their own preference.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class ModuleSettingsCustom : ModuleSettings
    {
        #region Constants and Fields

        /// <summary>
        ///   The string desktop source.
        /// </summary>
        private const string StringsDesktopSrc = "DesktopSrc";

        #endregion

        #region Public Methods

        /// <summary>
        /// The GetModuleSettings Method returns a hashtable of
        ///   custom module specific settings from the database. This method is
        ///   used by some user control modules to access misc settings.
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="userId">
        /// The user ID.
        /// </param>
        /// <param name="page">
        /// The page for settings.
        /// </param>
        /// <returns>
        /// The hash table.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static Hashtable GetModuleUserSettings(int moduleId, Guid userId, Page page)
        {
            var controlPath = Path.ApplicationRoot + "/";

            using (var dr = GetModuleDefinitionByID(moduleId))
            {
                if (dr.Read())
                {
                    controlPath += dr[StringsDesktopSrc].ToString();
                }
                // Added by Ashish - Connection Pool Issues
                if (dr != null)
                {
                    dr.Close();
                }
            }

            PortalModuleControlCustom portalModule;
            Hashtable setting;
            try
            {
                portalModule = (PortalModuleControlCustom)page.LoadControl(controlPath);
                setting = GetModuleUserSettings(
                    moduleId, PortalSettings.CurrentUser.Identity.ProviderUserKey, portalModule.CustomizedUserSettings);
            }
            catch (Exception ex)
            {
                // Appleseed.Framework.Configuration.ErrorHandler.HandleException("There was a problem loading: '" + ControlPath + "'", ex);
                // throw;
                throw new AppleseedException(
                    LogLevel.Fatal, string.Format("There was a problem loading: '{0}'", controlPath), ex);
            }

            return setting;
        }

        /// <summary>
        /// Retrieves the custom user settings for the current user for this module
        ///   from the database.
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="userId">
        /// The user ID.
        /// </param>
        /// <param name="customSettings">
        /// The custom settings.
        /// </param>
        /// <returns>
        /// The hash table.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static Hashtable GetModuleUserSettings(int moduleId, Guid userId, Hashtable customSettings)
        {
            // Get Settings for this module from the database
            var settings = new Hashtable();

            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_GetModuleUserSettings", connection))
            {
                // Mark the Command as a SPROC
                command.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4) { Value = moduleId };
                command.Parameters.Add(parameterModuleId);

                var parameterUserId = new SqlParameter("@UserID", SqlDbType.Int, 4) { Value = userId };
                command.Parameters.Add(parameterUserId);

                // Execute the command
                connection.Open();
                using (var dr = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        settings[dr["SettingName"].ToString()] = dr["SettingValue"].ToString();
                    }
                    dr.Close(); // Added by Ashish - Connection Pool Issues
                }
                connection.Close(); // Added by Ashish - Connection Pool Issues
            }

            foreach (string key in customSettings.Keys)
            {
                if (settings[key] == null)
                {
                    continue;
                }

                var s = customSettings[key];

                if (settings[key].ToString().Length == 0)
                {
                    continue;
                }

                var conv = TypeDescriptor.GetConverter(typeof(SettingItem<string, TextBox>));
                if (conv == null)
                {
                    continue;
                }

                var setting = (SettingItem<string, TextBox>)conv.ConvertFrom(s);
                if (setting != null)
                {
                    setting.Value = settings[key].ToString();
                }
            }

            return customSettings;
        }

        /// <summary>
        /// The UpdateCustomModuleSetting Method updates a single module setting
        ///   for the current user in the rb_ModuleUserSettings database table.
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="userId">
        /// The user ID.
        /// </param>
        /// <param name="key">
        /// The setting key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void UpdateCustomModuleSetting(int moduleId, Guid userId, string key, string value)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_UpdateModuleUserSetting", connection))
            {
                // Mark the Command as a SPROC
                command.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4) { Value = moduleId };
                command.Parameters.Add(parameterModuleId);

                var parameterUserId = new SqlParameter("@UserID", SqlDbType.Int, 4) { Value = userId };
                command.Parameters.Add(parameterUserId);

                var parameterKey = new SqlParameter("@SettingName", SqlDbType.NVarChar, 50) { Value = key };
                command.Parameters.Add(parameterKey);

                var parameterValue = new SqlParameter("@SettingValue", SqlDbType.NVarChar, 1500) { Value = value };
                command.Parameters.Add(parameterValue);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        #endregion
    }
}