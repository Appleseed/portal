// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecyclerDB.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Summary description for recycler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Site.Data
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Site.Configuration;

    /// <summary>
    /// Recycler Database Layer.
    /// </summary>
    public class RecyclerDB
    {
        #region Constants and Fields

        /// <summary>
        /// The strings no module.
        /// </summary>
        private const string StringsNoModule = "NO_MODULE";

        #endregion

        #region Public Methods

        /// <summary>
        /// MOST OF THIS METHOD'S CODE IS COPIED DIRECTLY FROM THE PortalSettings() CLASS
        ///   THE RECYCLER NEEDS TO BE ABLE TO RETRIEVE A MODULE'S ModuleSettings INDEPENDENT
        ///   OF THE TAB THE MODULE IS LOCATED ON (AND INDEPENDENT OF THE CURRENT 'ActiveTab'
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <returns>
        /// The module settings.
        /// </returns>
        public static ModuleSettings GetModuleSettingsForIndividualModule(int moduleId)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_GetModuleSettingsForIndividualModule", connection))
            {
                // Mark the Command as a SPROC
                command.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4) { Value = moduleId };
                command.Parameters.Add(parameterModuleId);

                // Open the database connection and execute the command
                connection.Open();
                var result = command.ExecuteReader(CommandBehavior.CloseConnection);

                var m = new ModuleSettings();

                // Read the result set -- There is only one row!
                while (result.Read())
                {
                    m.ModuleID = (int)result["ModuleID"];
                    m.ModuleDefID = (int)result["ModuleDefID"];
                    m.PageID = (int)result["TabID"];
                    m.PaneName = (string)result["PaneName"];
                    m.ModuleTitle = (string)result["ModuleTitle"];

                    var myvalue = result["AuthorizedEditRoles"];
                    m.AuthorizedEditRoles = !Convert.IsDBNull(myvalue) ? (string)myvalue : string.Empty;

                    myvalue = result["AuthorizedViewRoles"];
                    m.AuthorizedViewRoles = !Convert.IsDBNull(myvalue) ? (string)myvalue : string.Empty;

                    myvalue = result["AuthorizedAddRoles"];
                    m.AuthorizedAddRoles = !Convert.IsDBNull(myvalue) ? (string)myvalue : string.Empty;

                    myvalue = result["AuthorizedDeleteRoles"];
                    m.AuthorizedDeleteRoles = !Convert.IsDBNull(myvalue) ? (string)myvalue : string.Empty;

                    myvalue = result["AuthorizedPropertiesRoles"];
                    m.AuthorizedPropertiesRoles = !Convert.IsDBNull(myvalue) ? (string)myvalue : string.Empty;

                    // jviladiu@portalServices.net (19/08/2004) Add support for move & delete module roles
                    myvalue = result["AuthorizedMoveModuleRoles"];
                    m.AuthorizedMoveModuleRoles = !Convert.IsDBNull(myvalue) ? (string)myvalue : string.Empty;

                    myvalue = result["AuthorizedDeleteModuleRoles"];
                    m.AuthorizedDeleteModuleRoles = !Convert.IsDBNull(myvalue) ? (string)myvalue : string.Empty;

                    // Change by Geert.Audenaert@Syntegra.Com
                    // Date: 6/2/2003
                    myvalue = result["AuthorizedPublishingRoles"];
                    m.AuthorizedPublishingRoles = !Convert.IsDBNull(myvalue) ? (string)myvalue : string.Empty;

                    myvalue = result["SupportWorkflow"];
                    m.SupportWorkflow = !Convert.IsDBNull(myvalue) ? (bool)myvalue : false;

                    // Date: 27/2/2003
                    myvalue = result["AuthorizedApproveRoles"];
                    m.AuthorizedApproveRoles = !Convert.IsDBNull(myvalue) ? (string)myvalue : string.Empty;

                    myvalue = result["WorkflowState"];
                    m.WorkflowStatus = !Convert.IsDBNull(myvalue)
                                           ? (WorkflowState)(0 + (byte)myvalue)
                                           : WorkflowState.Original;

                    // End Change Geert.Audenaert@Syntegra.Com
                    // Start Change bja@reedtek.com
                    try
                    {
                        myvalue = result["SupportCollapsable"];
                    }
                    catch
                    {
                        myvalue = DBNull.Value;
                    }

                    m.SupportCollapsable = DBNull.Value != myvalue ? (bool)myvalue : false;

                    // End Change  bja@reedtek.com
                    // Start Change john.mandia@whitelightsolutions.com
                    try
                    {
                        myvalue = result["ShowEveryWhere"];
                    }
                    catch
                    {
                        myvalue = DBNull.Value;
                    }

                    m.ShowEveryWhere = DBNull.Value != myvalue ? (bool)myvalue : false;

                    // End Change  john.mandia@whitelightsolutions.com
                    m.CacheTime = int.Parse(result["CacheTime"].ToString());
                    m.ModuleOrder = int.Parse(result["ModuleOrder"].ToString());

                    myvalue = result["ShowMobile"];
                    m.ShowMobile = !Convert.IsDBNull(myvalue) ? (bool)myvalue : false;

                    m.DesktopSrc = result["DesktopSrc"].ToString();
                    m.MobileSrc = result["MobileSrc"].ToString();
                    m.Admin = bool.Parse(result["Admin"].ToString());
                }

                return m;
            }
        }

        /// <summary>
        /// The GetModulesInRecycler method returns a SqlDataReader containing all of the
        ///   Modules for a specific portal module that have been 'deleted' to the recycler.
        ///   <a href="GetModulesInRecycler.htm" style="color:green">GetModulesInRecycler Stored Procedure</a>
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="sortField">
        /// The sort field.
        /// </param>
        /// <returns>
        /// A data table.
        /// </returns>
        public static DataTable GetModulesInRecycler(int portalId, string sortField)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlDataAdapter("rb_GetModulesInRecycler", connection))
            {
                // Mark the Command as a SPROC
                command.SelectCommand.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterModuleId = new SqlParameter("@PortalID", SqlDbType.Int, 4) { Value = portalId };
                command.SelectCommand.Parameters.Add(parameterModuleId);

                var parameterSortField = new SqlParameter("@SortField", SqlDbType.VarChar, 50) { Value = sortField };
                command.SelectCommand.Parameters.Add(parameterSortField);

                // Create and Fill the DataSet
                using (var dataTable = new DataTable())
                {
                    try
                    {
                        command.Fill(dataTable);
                    }
                    finally
                    {
                        // by Manu fix close bug #2
                        connection.Close();
                    }

                    // Translate
                    foreach (var dr in
                        dataTable.Rows.Cast<DataRow>().Where(dr => dr[1].ToString() == StringsNoModule))
                    {
                        dr[1] = General.GetString(StringsNoModule);
                        break;
                    }

                    // Return the data reader
                    return dataTable;
                }
            }
        }

        /// <summary>
        /// Modules the is in recycler.
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <returns>
        /// The module is in recycler.
        /// </returns>
        public static bool ModuleIsInRecycler(int moduleId)
        {
            var ms = GetModuleSettingsForIndividualModule(moduleId);
            return ms.PageID == 0;
        }

        /// <summary>
        /// MoveModuleToNewTab assigns the given module to the given tab
        /// </summary>
        /// <param name="pageId">
        /// The tab ID.
        /// </param>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        public static void MoveModuleToNewTab(int pageId, int moduleId)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_MoveModuleToNewTab", connection))
            {
                // Mark the Command as a SPROC
                command.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4) { Value = moduleId };
                command.Parameters.Add(parameterModuleId);

                var parameterTabId = new SqlParameter("@TabID", SqlDbType.Int, 4) { Value = pageId };
                command.Parameters.Add(parameterTabId);

                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ErrorHandler.Publish(
                        LogLevel.Warn, "An Error Occurred in MoveModuleToNewTab. Parameter : " + moduleId, ex);
                }
            }
        }

        #endregion
    }
}