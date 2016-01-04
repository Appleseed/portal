// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModulesDB.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Class that encapsulates all data logic necessary to add/query/delete
//   configuration, layout and security settings values within the Portal database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Site.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web.UI.WebControls;

    using Appleseed.Framework.Core.BLL;
    using Appleseed.Framework.Data;
    using Appleseed.Framework.Helpers;
    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Settings.Cache;
    using Appleseed.Framework.Site.Configuration;
    using System.Web;
    using Appleseed.Framework.Models;
    using Appleseed.Framework.DAL;
    using Appleseed.Framework.Configuration.Items;

    /// <summary>
    /// Class that encapsulates all data logic necessary to add/query/delete
    ///   configuration, layout and security settings values within the Portal database.
    /// </summary>
    public class ModulesDB
    {
        #region Constants and Fields

        /// <summary>
        /// The str at add roles.
        /// </summary>
        private const string StringsAddRoles = "@AddRoles";

        /// <summary>
        /// The str at admin.
        /// </summary>
        private const string StringsAdmin = "@Admin";

        /// <summary>
        /// The str at approval roles.
        /// </summary>
        private const string StringsApprovalRoles = "@ApprovalRoles";

        /// <summary>
        /// The str at assembly name.
        /// </summary>
        private const string StringsAssemblyName = "@AssemblyName";

        /// <summary>
        /// The str at cache time.
        /// </summary>
        private const string StringsCacheTime = "@CacheTime";

        /// <summary>
        /// The str at class name.
        /// </summary>
        private const string StringsClassName = "@ClassName";

        /// <summary>
        /// The str at delete module roles.
        /// </summary>
        private const string StringsDeleteModuleRoles = "@DeleteModuleRoles";

        /// <summary>
        /// The str at delete roles.
        /// </summary>
        private const string StringsDeleteRoles = "@DeleteRoles";

        /// <summary>
        /// The str at desktop src.
        /// </summary>
        private const string StringsDesktopSrc = "@DesktopSrc";

        /// <summary>
        /// The str at edit roles.
        /// </summary>
        private const string StringsEditRoles = "@EditRoles";

        /// <summary>
        /// The str at friendly name.
        /// </summary>
        private const string StringsFriendlyName = "@FriendlyName";

        /// <summary>
        /// The str at general mod def id.
        /// </summary>
        private const string StringsGeneralModDefId = "@GeneralModDefID";

        /// <summary>
        /// The str at guid.
        /// </summary>
        private const string StringsGuid = "@Guid";

        /// <summary>
        /// The str at mobile src.
        /// </summary>
        private const string StringsMobileSrc = "@MobileSrc";

        /// <summary>
        /// The str at module def id.
        /// </summary>
        private const string StringsModuleDefId = "@ModuleDefID";

        /// <summary>
        /// The str at module id.
        /// </summary>
        private const string StringsModuleId = "@ModuleID";

        /// <summary>
        /// The str at module order.
        /// </summary>
        private const string StringsModuleOrder = "@ModuleOrder";

        /// <summary>
        /// The str at module title.
        /// </summary>
        private const string StringsModuleTitle = "@ModuleTitle";

        /// <summary>
        /// The str at move module roles.
        /// </summary>
        private const string StringsMoveModuleRoles = "@MoveModuleRoles";

        /// <summary>
        /// The str at page id.
        /// </summary>
        private const string StringsPageId = "@TabID";

        /// <summary>
        /// The str at pane name.
        /// </summary>
        private const string StringsPaneName = "@PaneName";

        /// <summary>
        /// The str at portal id.
        /// </summary>
        private const string StringsPortalId = "@PortalID";

        /// <summary>
        /// The str at properties roles.
        /// </summary>
        private const string StringsPropertiesRoles = "@PropertiesRoles";

        /// <summary>
        /// The str at publishing roles.
        /// </summary>
        private const string StringsPublishingRoles = "@PublishingRoles";

        /// <summary>
        /// The str at searchable.
        /// </summary>
        private const string StringsSearchable = "@Searchable";

        /// <summary>
        /// The str at show every where.
        /// </summary>
        private const string StringsShowEveryWhere = "@ShowEveryWhere";

        /// <summary>
        /// The str at show mobile.
        /// </summary>
        private const string StringsShowMobile = "@ShowMobile";

        /// <summary>
        /// Support Collapsible
        /// </summary>
        private const string StringsSupportCollapsible = "@SupportCollapsable";

        /// <summary>
        /// The str at support workflow.
        /// </summary>
        private const string StringsSupportWorkflow = "@SupportWorkflow";

        /// <summary>
        /// The str at view roles.
        /// </summary>
        private const string StringsViewRoles = "@ViewRoles";

        // const string strGUID = "GUID";

        /// <summary>
        /// The str no module.
        /// </summary>
        private const string StringsNoModule = "NO_MODULE";

        /// <summary>
        /// The strrb_ get modules in page.
        /// </summary>
        private const string StringsGetModulesInPage = "rb_GetModulesInTab";

        /// <summary>
        /// Portal ID
        /// </summary>
        public const string SelectedPortalID = "@PortalID";

        #endregion

        #region Public Methods

        /// <summary>
        /// Add General Module Definitions
        /// </summary>
        /// <param name="generalModDefId">
        /// General Mod Def ID
        /// </param>
        /// <param name="friendlyName">
        /// Name of the friendly.
        /// </param>
        /// <param name="desktopSrc">
        /// The desktop SRC.
        /// </param>
        /// <param name="mobileSrc">
        /// The mobile SRC.
        /// </param>
        /// <param name="assemblyName">
        /// Name of the assembly.
        /// </param>
        /// <param name="className">
        /// Name of the class.
        /// </param>
        /// <param name="admin">
        /// if set to <c>true</c> [admin].
        /// </param>
        /// <param name="searchable">
        /// if set to <c>true</c> [searchable].
        /// </param>
        /// <returns>
        /// The newly created ID
        /// </returns>
        public Guid AddGeneralModuleDefinitions(
            Guid generalModDefId,
            string friendlyName,
            string desktopSrc,
            string mobileSrc,
            string assemblyName,
            string className,
            bool admin,
            bool searchable)
        {
            using (var connection = Config.SqlConnectionString)
            {
                using (var command = new SqlCommand("rb_AddGeneralModuleDefinitions", connection))
                {
                    // Mark the Command as a SPROC
                    command.CommandType = CommandType.StoredProcedure;

                    // Add Parameters to SPROC
                    var parameterGeneralModDefId = new SqlParameter(StringsGeneralModDefId, SqlDbType.UniqueIdentifier)
                    {
                        Value = generalModDefId
                    };
                    command.Parameters.Add(parameterGeneralModDefId);
                    var parameterFriendlyName = new SqlParameter(StringsFriendlyName, SqlDbType.NVarChar, 128)
                    {
                        Value = friendlyName
                    };
                    command.Parameters.Add(parameterFriendlyName);
                    var parameterDesktopSrc = new SqlParameter(StringsDesktopSrc, SqlDbType.NVarChar, 256)
                    {
                        Value = desktopSrc
                    };
                    command.Parameters.Add(parameterDesktopSrc);
                    var parameterMobileSrc = new SqlParameter(StringsMobileSrc, SqlDbType.NVarChar, 256)
                    {
                        Value = mobileSrc
                    };
                    command.Parameters.Add(parameterMobileSrc);
                    var parameterAssemblyName = new SqlParameter(StringsAssemblyName, SqlDbType.VarChar, 50)
                    {
                        Value = assemblyName
                    };
                    command.Parameters.Add(parameterAssemblyName);
                    var parameterClassName = new SqlParameter(StringsClassName, SqlDbType.NVarChar, 128)
                    {
                        Value = className
                    };
                    command.Parameters.Add(parameterClassName);
                    var parameterAdmin = new SqlParameter(StringsAdmin, SqlDbType.Bit) { Value = admin };
                    command.Parameters.Add(parameterAdmin);
                    var parameterSearchable = new SqlParameter(StringsSearchable, SqlDbType.Bit) { Value = searchable };
                    command.Parameters.Add(parameterSearchable);

                    // Open the database connection and execute the command
                    connection.Open();

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.Publish(LogLevel.Warn, "An Error Occurred in AddGeneralModuleDefinitions. ", ex);
                    }
                    //Added by Ashish - Connection Pool Issue
                    finally
                    {
                        connection.Close();
                    }

                    // Return the newly created ID
                    return new Guid(parameterGeneralModDefId.Value.ToString());
                }
            }
        }

        /// <summary>
        /// The AddModule method updates a specified Module within the Modules database table.
        ///   If the module does not yet exist,the stored procedure adds it.<br/>
        ///   AddModule Stored Procedure
        /// </summary>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        /// <param name="moduleOrder">
        /// The module order.
        /// </param>
        /// <param name="paneName">
        /// Name of the pane.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="moduleDefId">
        /// The module def ID.
        /// </param>
        /// <param name="cacheTime">
        /// The cache time.
        /// </param>
        /// <param name="editRoles">
        /// The edit roles.
        /// </param>
        /// <param name="viewRoles">
        /// The view roles.
        /// </param>
        /// <param name="addRoles">
        /// The add roles.
        /// </param>
        /// <param name="deleteRoles">
        /// The delete roles.
        /// </param>
        /// <param name="propertiesRoles">
        /// The properties roles.
        /// </param>
        /// <param name="moveModuleRoles">
        /// The move module roles.
        /// </param>
        /// <param name="deleteModuleRoles">
        /// The delete module roles.
        /// </param>
        /// <param name="showMobile">
        /// if set to <c>true</c> [show mobile].
        /// </param>
        /// <param name="publishingRoles">
        /// The publishing roles.
        /// </param>
        /// <param name="supportWorkflow">
        /// if set to <c>true</c> [support workflow].
        /// </param>
        /// <param name="showEveryWhere">
        /// if set to <c>true</c> [show every where].
        /// </param>
        /// <param name="supportCollapsable">
        /// if set to <c>true</c> [support collapsible].
        /// </param>
        /// <returns>
        /// The add module.
        /// </returns>
        [History("jviladiu@portalServices.net", "2004/08/19", "Added support for move & delete modules roles")]
        [History("john.mandia@whitelightsolutions.com", "2003/05/24", "Added support for showEveryWhere")]
        [History("bja@reedtek.com", "2003/05/16", "Added support for win. mgmt min/max/close -- supportCollapsable")]
        public int AddModule(
            int pageId,
            int moduleOrder,
            string paneName,
            string title,
            int moduleDefId,
            int cacheTime,
            string editRoles,
            string viewRoles,
            string addRoles,
            string deleteRoles,
            string propertiesRoles,
            string moveModuleRoles,
            string deleteModuleRoles,
            bool showMobile,
            string publishingRoles,
            bool supportWorkflow,
            bool showEveryWhere,
            bool supportCollapsable)
        {
            // Changes by Geert.Audenaert@Syntegra.Com Date: 6/2/2003
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_AddModule", connection))
            {
                // Mark the Command as a SPROC
                command.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterModuleId = new SqlParameter(StringsModuleId, SqlDbType.Int, 4)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(parameterModuleId);
                var parameterModuleDefinitionId = new SqlParameter(StringsModuleDefId, SqlDbType.Int, 4)
                {
                    Value = moduleDefId
                };
                command.Parameters.Add(parameterModuleDefinitionId);
                var parameterPageId = new SqlParameter(StringsPageId, SqlDbType.Int, 4) { Value = pageId };
                command.Parameters.Add(parameterPageId);
                var parameterModuleOrder = new SqlParameter(StringsModuleOrder, SqlDbType.Int, 4) { Value = moduleOrder };
                command.Parameters.Add(parameterModuleOrder);
                var parameterTitle = new SqlParameter(StringsModuleTitle, SqlDbType.NVarChar, 256) { Value = title };
                command.Parameters.Add(parameterTitle);
                var parameterPaneName = new SqlParameter(StringsPaneName, SqlDbType.NVarChar, 256) { Value = paneName };
                command.Parameters.Add(parameterPaneName);
                var parameterCacheTime = new SqlParameter(StringsCacheTime, SqlDbType.Int, 4) { Value = cacheTime };
                command.Parameters.Add(parameterCacheTime);
                var parameterEditRoles = new SqlParameter(StringsEditRoles, SqlDbType.NVarChar, 256) { Value = editRoles };
                command.Parameters.Add(parameterEditRoles);
                var parameterViewRoles = new SqlParameter(StringsViewRoles, SqlDbType.NVarChar, 256) { Value = viewRoles };
                command.Parameters.Add(parameterViewRoles);
                var parameterAddRoles = new SqlParameter(StringsAddRoles, SqlDbType.NVarChar, 256) { Value = addRoles };
                command.Parameters.Add(parameterAddRoles);
                var parameterDeleteRoles = new SqlParameter(StringsDeleteRoles, SqlDbType.NVarChar, 256)
                {
                    Value = deleteRoles
                };
                command.Parameters.Add(parameterDeleteRoles);
                var parameterPropertiesRoles = new SqlParameter(StringsPropertiesRoles, SqlDbType.NVarChar, 256)
                {
                    Value = propertiesRoles
                };
                command.Parameters.Add(parameterPropertiesRoles);

                // Added by jviladiu@portalservices.net (19/08/2004)
                var parameterMoveModuleRoles = new SqlParameter(StringsMoveModuleRoles, SqlDbType.NVarChar, 256)
                {
                    Value = moveModuleRoles
                };
                command.Parameters.Add(parameterMoveModuleRoles);

                // Added by jviladiu@portalservices.net (19/08/2004)
                var parameterDeleteModuleRoles = new SqlParameter(StringsDeleteModuleRoles, SqlDbType.NVarChar, 256)
                {
                    Value = deleteModuleRoles
                };
                command.Parameters.Add(parameterDeleteModuleRoles);

                // Change by Geert.Audenaert@Syntegra.Com
                // Date: 6/2/2003
                var parameterPublishingRoles = new SqlParameter(StringsPublishingRoles, SqlDbType.NVarChar, 256)
                {
                    Value = publishingRoles
                };
                command.Parameters.Add(parameterPublishingRoles);
                var parameterSupportWorkflow = new SqlParameter(StringsSupportWorkflow, SqlDbType.Bit, 1)
                {
                    Value = supportWorkflow
                };
                command.Parameters.Add(parameterSupportWorkflow);

                // End Change Geert.Audenaert@Syntegra.Com
                var parameterShowMobile = new SqlParameter(StringsShowMobile, SqlDbType.Bit, 1) { Value = showMobile };
                command.Parameters.Add(parameterShowMobile);

                // Start Change john.mandia@whitelightsolutions.com
                var parameterShowEveryWhere = new SqlParameter(StringsShowEveryWhere, SqlDbType.Bit, 1)
                {
                    Value = showEveryWhere
                };
                command.Parameters.Add(parameterShowEveryWhere);

                // End Change  john.mandia@whitelightsolutions.com
                // Start Change bja@reedtek.com
                var parameterSupportCollapsable = new SqlParameter(StringsSupportCollapsible, SqlDbType.Bit, 1)
                {
                    Value = supportCollapsable
                };
                command.Parameters.Add(parameterSupportCollapsable);

                // End Change  bja@reedtek.com
                connection.Open();

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // ErrorHandler.Publish(Appleseed.Framework.LogLevel.Warn, "An Error Occurred in AddModule. ", ex);
                    ErrorHandler.Publish(LogLevel.Warn, "An Error Occurred in AddModule. ", ex);
                }
                //Added by Ashish - Connection Pool Issue
                finally
                {
                    connection.Close();
                }

                return (int)parameterModuleId.Value;
            }
        }

        /// <summary>
        /// The DeleteModule method deletes a specified Module from the Modules database table.<br/>
        ///   DeleteModule Stored Procedure
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        [History("JB - john@bowenweb.com", "2005/05/12", "Added support for Recycler module")]
        public void DeleteModule(int moduleId)
        {
            var PortalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            var useRecycler =
                bool.Parse(
                    PortalSettings.GetPortalCustomSettings(
                        PortalSettings.PortalID,
                        PortalSettings.GetPortalBaseSettings(PortalSettings.PortalPath))["SITESETTINGS_USE_RECYCLER"].ToString());

            //const bool UseRecycler = false;

#pragma warning disable 162
            // ReSharper disable HeuristicUnreachableCode
            using (var connection = Config.SqlConnectionString)
            using (
                var command = new SqlCommand(useRecycler ? "rb_DeleteModuleToRecycler" : "rb_DeleteModule", connection))
            {
                // Mark the Command as a SPROC
                command.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterModuleId = new SqlParameter(StringsModuleId, SqlDbType.Int, 4) { Value = moduleId };
                command.Parameters.Add(parameterModuleId);

                // ReSharper disable ConditionIsAlwaysTrueOrFalse
                if (useRecycler)
                {
                    // ReSharper restore ConditionIsAlwaysTrueOrFalse
                    // Recycler needs some extra params for entry
                    // Add Recycler-specific Parameters to SPROC
                    var paramDeletedBy = new SqlParameter("@DeletedBy", SqlDbType.NVarChar, 250)
                    {
                        Value = MailHelper.GetCurrentUserEmailAddress()
                    };
                    command.Parameters.Add(paramDeletedBy);

                    var paramDeletedDate = new SqlParameter("@DateDeleted", SqlDbType.DateTime, 8)
                    {
                        Value = DateTime.Now
                    };
                    command.Parameters.Add(paramDeletedDate);
                }

                // ReSharper restore HeuristicUnreachableCode
#pragma warning restore 162

                // BOWEN 11 June 2005 - END
                connection.Open();

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ErrorHandler.Publish(
                        LogLevel.Warn, "An Error Occurred in DeleteModule. Parameter : " + moduleId, ex);
                }
                //Added by Ashish - Connection Pool Issue
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// The DeleteModuleDefinition method deletes the specified
        ///   module type definition from the portal.
        /// </summary>
        /// <param name="defId">
        /// The def ID.
        /// </param>
        /// <remarks>
        /// Other relevant sources: DeleteModuleDefinition Stored Procedure
        /// </remarks>
        public void DeleteModuleDefinition(Guid defId)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            {
                using (var command = new SqlCommand("rb_DeleteModuleDefinition", connection))
                {
                    // Mark the Command as a SPROC
                    command.CommandType = CommandType.StoredProcedure;

                    // Add Parameters to SPROC
                    var parameterModuleDefId = new SqlParameter(StringsModuleDefId, SqlDbType.UniqueIdentifier)
                    {
                        Value = defId
                    };
                    command.Parameters.Add(parameterModuleDefId);

                    // Open the database connection and execute the command
                    connection.Open();

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // ErrorHandler.Publish(Appleseed.Framework.LogLevel.Warn, "An Error Occurred in DeleteModuleDefinition. Parameter : " + defID.ToString(), ex);
                        ErrorHandler.Publish(
                            LogLevel.Warn, "An Error Occurred in DeleteModuleDefinition. Parameter : " + defId, ex);
                    }
                    //Added by Ashish - Connection Pool Issue
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Exists the module products in page.
        /// </summary>
        /// <param name="pageId">
        /// The tab ID.
        /// </param>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <returns>
        /// A bool value...
        /// </returns>
        public bool ExistModuleProductsInPage(int pageId, int portalId)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            {
                using (var command = new SqlCommand(StringsGetModulesInPage, connection))
                {
                    // Mark the Command as a SPROC
                    command.CommandType = CommandType.StoredProcedure;

                    // Add Parameters to SPROC
                    var parameterPageId = new SqlParameter(StringsPageId, SqlDbType.Int, 4) { Value = pageId };
                    command.Parameters.Add(parameterPageId);

                    // Add Parameters to SPROC
                    var parameterPortalId = new SqlParameter(StringsPortalId, SqlDbType.Int, 4) { Value = portalId };
                    command.Parameters.Add(parameterPortalId);

                    connection.Open();
                    var moduleGuid = new Guid("{EC24FABD-FB16-4978-8C81-1ADD39792377}");
                    var retorno = false;

                    using (var result = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (result.Read())
                        {
                            if (moduleGuid.Equals(result.GetGuid(1)))
                            {
                                retorno = true;
                            }
                        }
                        //Added by Ashish - Connection Pool Issue
                        result.Close();
                    }

                    return retorno;
                }
            }
        }

        /// <summary>
        /// Find module id defined by the guid in a tab in the portal
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="guid">
        /// The module GUID.
        /// </param>
        /// <returns>
        /// A list of module items.
        /// </returns>
        public List<ModuleItem> FindModuleItemsByGuid(int portalId, Guid guid)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_FindModulesByGuid", connection))
            {
                // Mark the Command as a SPROC
                command.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterFriendlyName = new SqlParameter(StringsGuid, SqlDbType.UniqueIdentifier) { Value = guid };
                command.Parameters.Add(parameterFriendlyName);
                var parameterPortalId = new SqlParameter(StringsPortalId, SqlDbType.Int, 4) { Value = portalId };
                command.Parameters.Add(parameterPortalId);

                // Open the database connection and execute the command
                connection.Open();
                var modList = new List<ModuleItem>();

                using (var result = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (result.Read())
                    {
                        var m = new ModuleItem { ID = (int)result["ModuleId"] };
                        modList.Add(m);
                    }
                    //Added by Ashish - Connection Pool issue
                    result.Close();
                }

                return modList;
            }
        }

        /// <summary>
        /// Find modules defined by the guid in a tab in the portal
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="guid">
        /// The module GUID.
        /// </param>
        /// <returns>
        /// A SQL data reader.
        /// </returns>
        [Obsolete("Use FindModuleItemsByGuid instead.")]
        public SqlDataReader FindModulesByGuid(int portalId, Guid guid)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_FindModulesByGuid", connection)
            {
                // Mark the Command as a SPROC
                CommandType = CommandType.StoredProcedure
            };

            // Add Parameters to SPROC
            var parameterFriendlyName = new SqlParameter(StringsGuid, SqlDbType.UniqueIdentifier) { Value = guid };
            command.Parameters.Add(parameterFriendlyName);
            var parameterPortalId = new SqlParameter(StringsPortalId, SqlDbType.Int, 4) { Value = portalId };
            command.Parameters.Add(parameterPortalId);

            // Open the database connection and execute the command
            connection.Open();
            var dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the data reader
            return dr;
        }

        /// <summary>
        /// Get all portal modules 
        /// </summary>
        /// <param name="portalId">portal id</param>
        /// <returns>module list</returns>
        public List<CurrentModuleDefination> GetCurrentModuleDefinitions(int portalId)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_GetCurrentModuleDefinitions", connection)
            {
                // Mark the Command as a SPROC
                CommandType = CommandType.StoredProcedure
            };

            // Add Parameters to SPROC
            var parameterPortalId = new SqlParameter(StringsPortalId, SqlDbType.Int, 4) { Value = portalId };
            command.Parameters.Add(parameterPortalId);

            // Open the database connection and execute the command
            connection.Open();
            var dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            List<CurrentModuleDefination> modulelist = new List<CurrentModuleDefination>();

            try
            {
                while (dr.Read())
                {
                    CurrentModuleDefination module = new CurrentModuleDefination();
                    module.FriendlyName = dr["FriendlyName"].ToString();
                    module.DesktopSrc = dr["DesktopSrc"].ToString();
                    module.MobileSrc = dr["MobileSrc"].ToString();
                    module.Admin = Convert.ToBoolean(dr["Admin"].ToString());
                    module.ModuleDefId = Convert.ToInt32(dr["ModuleDefId"].ToString());
                    modulelist.Add(module);
                }
            }
            //Added by Ashish - Connection Pool Issue
            finally { dr.Close(); }
            

            return modulelist;
            // Return the data reader
            // return dr;
        }
        /// <summary>
        /// The GetModuleDefinitions method returns a list of all module type definitions.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <returns>
        /// A list of general module definitions.
        /// </returns>
        /// <remarks>
        /// Other relevant sources: GetModuleDefinitions Stored Procedure
        /// </remarks>
        public List<GeneralModuleDefinition> GetCurrentModuleDefinitionsList(int portalId)
        {
            var result = new List<GeneralModuleDefinition>();

            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (
                var command = new SqlCommand("rb_GetCurrentModuleDefinitions", connection)
                {
                    // Mark the Command as a SPROC
                    CommandType = CommandType.StoredProcedure
                })
            {
                var parameterPortalId = new SqlParameter(StringsPortalId, SqlDbType.Int, 4) { Value = portalId };
                command.Parameters.Add(parameterPortalId);

                // Open the database connection and execute the command
                connection.Open();

                using (var dr = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        var genModDef = new GeneralModuleDefinition
                        {
                            FriendlyName = dr.GetString(0),
                            DesktopSource = dr.GetString(1),
                            MobileSource = dr.GetString(2),
                            Admin = dr.GetBoolean(3),
                            GeneralModDefID = dr.GetGuid(4)
                        };

                        result.Add(genModDef);
                    }
                    //Added by Ashish - Connection Pool Issue
                    if (dr != null)
                        dr.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// The GetGeneralModuleDefinitionByName method returns the id of the Module
        ///   that matches the named Module in general list.
        /// </summary>
        /// <param name="moduleName">
        /// Name of the module.
        /// </param>
        /// <returns>
        /// Module GUID.
        /// </returns>
        public Guid GetGeneralModuleDefinitionByName(string moduleName)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_GetGeneralModuleDefinitionByName", connection))
            {
                // Mark the Command as a SPROC
                command.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterFriendlyName = new SqlParameter("@FriendlyName", SqlDbType.NVarChar, 128)
                {
                    Value = moduleName
                };
                command.Parameters.Add(parameterFriendlyName);
                var parameterModuleId = new SqlParameter(StringsModuleId, SqlDbType.UniqueIdentifier)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(parameterModuleId);

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

                if (parameterModuleId.Value != null && parameterModuleId.Value.ToString().Length != 0)
                {
                    try
                    {
                        return new Guid(parameterModuleId.Value.ToString());
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("'{0}' seems not a valid Module GUID.", parameterModuleId.Value), ex);

                        // Jes1111
                        // Appleseed.Framework.Configuration.ErrorHandler.HandleException("'" + parameterModuleID.Value.ToString() + "' seems not a valid GUID.", ex);
                        // throw;
                    }
                }

                throw new ArgumentException("Null Module GUID!"); // Jes1111

                // Appleseed.Framework.Configuration.ErrorHandler.HandleException("Null GUID!.", new ArgumentException("Null GUID!", strGUID));

                // throw new ArgumentException("Invalid GUID", strGUID);
            }
        }

        /// <summary>
        /// The GetModuleDefinitionByGUID method returns the id of the Module
        ///   that matches the named Module for the specified Portal.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="guid">
        /// The module GUID.
        /// </param>
        /// <returns>
        /// The get module definition by guid.
        /// </returns>
        public int GetModuleDefinitionByGuid(int portalId, Guid guid)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_GetModuleDefinitionByGuid", connection))
            {
                // Mark the Command as a SPROC
                command.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterFriendlyName = new SqlParameter(StringsGuid, SqlDbType.UniqueIdentifier) { Value = guid };
                command.Parameters.Add(parameterFriendlyName);
                var parameterPortalId = new SqlParameter(StringsPortalId, SqlDbType.Int, 4) { Value = portalId };
                command.Parameters.Add(parameterPortalId);
                var parameterModuleId = new SqlParameter(StringsModuleId, SqlDbType.Int, 4)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(parameterModuleId);

                connection.Open();

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ErrorHandler.Publish(
                        LogLevel.Warn, "An Error Occurred in GetModuleDefinitionByGuid. Parameter : " + guid, ex);
                }
                //Added by Ashish - Connection Pool Issue
                finally
                {
                    connection.Close();
                }

                return (int)parameterModuleId.Value;
            }
        }

        /// <summary>
        /// The GetModuleDefinitionByName method returns the id of the Module
        ///   that matches the named Module for the specified Portal.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="moduleName">
        /// Name of the module.
        /// </param>
        /// <returns>
        /// The get module definition by name.
        /// </returns>
        public int GetModuleDefinitionByName(int portalId, string moduleName)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_GetModuleDefinitionByName", connection))
            {
                // Mark the Command as a SPROC
                command.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterFriendlyName = new SqlParameter(StringsFriendlyName, SqlDbType.NVarChar, 128)
                {
                    Value = moduleName
                };
                command.Parameters.Add(parameterFriendlyName);
                var parameterPortalId = new SqlParameter(StringsPortalId, SqlDbType.Int, 4) { Value = portalId };
                command.Parameters.Add(parameterPortalId);
                var parameterModuleId = new SqlParameter(StringsModuleId, SqlDbType.Int, 4)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(parameterModuleId);

                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ErrorHandler.Publish(
                        LogLevel.Warn, "An Error Occurred in GetModuleDefinitionByName. Parameter : " + moduleName, ex);
                }
                //Added by Ashish - Connection Pool Issue
                finally
                {
                    connection.Close();
                }

                return (int)parameterModuleId.Value;
            }
        }

        /// <summary>
        /// Get module definitions
        /// </summary>
        /// <returns></returns>
        public List<CurrentModuleDefination> GetModuleDefinitions()
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_GetModuleDefinitions", connection)
            {
                // Mark the Command as a SPROC
                CommandType = CommandType.StoredProcedure
            };

            // Open the database connection and execute the command
            connection.Open();
            var dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            List<CurrentModuleDefination> modulelist = new List<CurrentModuleDefination>();
            CurrentModuleDefination module = new CurrentModuleDefination();
            try
            {
                while (dr.Read())
                {
                    Guid moddef = new Guid();
                    Guid.TryParse(dr["GeneralModDefID"].ToString(), out moddef);
                    module.GeneralModDefID = moddef;
                    module.FriendlyName = dr["FriendlyName"].ToString();
                    module.DesktopSrc = dr["DesktopSrc"].ToString();
                    module.MobileSrc = dr["MobileSrc"].ToString();
                    modulelist.Add(module);
                }
            }

             //Added by Ashish - Connection Pool Issue
            finally
            {
                dr.Close();
            }

            return modulelist;
            // Return the data reader
            //return dr;
        }
        /// <summary>
        /// Gets the module GUID.
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <returns>
        /// A System.Guid value...
        /// </returns>
        public Guid GetModuleGuid(int moduleId)
        {
            var moduleGuid = Guid.Empty;
            var cacheGuid = string.Format("{0}GUID", Key.ModuleSettings(moduleId));
            if (CurrentCache.Get(cacheGuid) == null)
            {
                using (var connection = Config.SqlConnectionString)
                using (var command = new SqlCommand("rb_GetGuid", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    var parameterModuleId = new SqlParameter(StringsModuleId, SqlDbType.Int, 4) { Value = moduleId };
                    command.Parameters.Add(parameterModuleId);
                    connection.Open();
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            moduleGuid = dr.GetGuid(0);
                        }
                        if (dr != null)
                            dr.Close();
                    }
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }

                CurrentCache.Insert(cacheGuid, moduleGuid);
            }
            else
            {
                moduleGuid = (Guid)CurrentCache.Get(cacheGuid);
            }

            return moduleGuid;
        }

        /// <summary>
        /// The GetModuleInUse method returns a list of modules in use with this portal<br/>
        ///   GetModuleInUse Stored Procedure
        /// </summary>
        /// <param name="defId">
        /// The def ID.
        /// </param>
        /// <returns>
        /// A list of ListItem controls.
        /// </returns>
        public List<ListItem> GetModuleInUse(Guid defId)
        {
            var portalList = new List<ListItem>();

            using (var sqlCommand = new SqlCommand("rb_GetModuleInUse")
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                var parameterdefId = new SqlParameter(StringsModuleId, SqlDbType.UniqueIdentifier) { Value = defId };
                sqlCommand.Parameters.Add(parameterdefId);

                using (var portals = DBHelper.GetDataReader(sqlCommand))
                {
                    while (portals.Read())
                    {
                        if (Convert.ToInt32(portals["PortalID"]) < 0)
                        {
                            continue;
                        }

                        var item = new ListItem
                        {
                            Text = portals["PortalName"].ToString(),
                            Value = portals["PortalID"].ToString(),
                            Selected = portals["checked"].ToString() == "1"
                        };

                        portalList.Add(item);
                    }

                    portals.Close();
                }
            }

            return portalList;
        }

        /// <summary>
        /// Get Modules All Portals
        /// </summary>
        /// <returns>
        /// A data table.
        /// </returns>
        public DataTable GetModulesAllPortals()
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlDataAdapter("rb_GetModulesAllPortals", connection))
            {
                // Mark the Command as a SPROC
                command.SelectCommand.CommandType = CommandType.StoredProcedure;

                // Create and Fill the DataSet
                using (var dataTable = new DataTable())
                {
                    try
                    {
                        command.Fill(dataTable);
                    }
                    finally
                    {
                        connection.Close(); // by Manu fix close bug #2
                    }

                    // Translate
                    foreach (var dr in dataTable.Rows.Cast<DataRow>().Where(dr => dr[1].ToString() == StringsNoModule))
                    {
                        dr[1] = General.GetString(StringsNoModule);
                        break;
                    }

                    // Return the data reader
                    return dataTable;
                }
            }
        }

        ///// <summary>
        ///// The GetModuleByName method returns a list of all module with
        /////   the specified Name (Type) within the Portal.
        /////   It is used to get all instances of a specified module used in a Portal.
        /////   e.g. All Image Gallery
        ///// </summary>
        ///// <param name="moduleName">
        ///// Name of the module.
        ///// </param>
        ///// <param name="portalId">
        ///// The portal ID.
        ///// </param>
        ///// <returns>
        ///// A SQL data reader.
        ///// </returns>
        ///// <remarks>
        ///// Other relevant sources: GetModuleByName Stored Procedure
        ///// </remarks>
        //[Obsolete("Replace me, bad design practice to pass SqlDataReaders to the UI")]
        //public SqlDataReader GetModulesByName(string moduleName, int portalId)
        //{
        //    // Create Instance of Connection and Command Object
        //    var connection = Config.SqlConnectionString;
        //    var command = new SqlCommand("rb_GetModulesByName", connection) {
        //        // Mark the Command as a SPROC
        //        CommandType = CommandType.StoredProcedure
        //    };

        //    // Add Parameters to SPROC
        //    var parameterFriendlyName = new SqlParameter("@moduleName", SqlDbType.NVarChar, 128) { Value = moduleName };
        //    command.Parameters.Add(parameterFriendlyName);
        //    var parameterPortalId = new SqlParameter(StringsPortalId, SqlDbType.Int, 4) { Value = portalId };
        //    command.Parameters.Add(parameterPortalId);

        //    // Open the database connection and execute the command
        //    connection.Open();
        //    var dr = command.ExecuteReader(CommandBehavior.CloseConnection);

        //    // Return the data reader
        //    return dr;
        //}

        /// <summary>
        /// Get modules by Name
        /// </summary>
        /// <param name="moduleName">modulename</param>
        /// <param name="portalId">portal id</param>
        /// <returns></returns>
        public List<ModuleItem> GetModulesByName(string moduleName, int portalId)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_GetModulesByName", connection)
            {
                // Mark the Command as a SPROC
                CommandType = CommandType.StoredProcedure
            };

            // Add Parameters to SPROC
            var parameterFriendlyName = new SqlParameter("@moduleName", SqlDbType.NVarChar, 128) { Value = moduleName };
            command.Parameters.Add(parameterFriendlyName);
            var parameterPortalId = new SqlParameter(StringsPortalId, SqlDbType.Int, 4) { Value = portalId };
            command.Parameters.Add(parameterPortalId);

            // Open the database connection and execute the command
            connection.Open();
            var dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            List<ModuleItem> modulelist = new List<ModuleItem>();
            try
            {
                while (dr.Read())
                {
                    ModuleItem module = new ModuleItem();
                    module.ID = Convert.ToInt32(dr["ModuleID"]);
                    module.Title = dr["ModuleTitle"].ToString();
                    modulelist.Add(module);
                }
            }
            //Added by Ashish - Connection Pool Issue
            finally
            {
                dr.Close();
            }

            // Return the data reader
            return modulelist;
        }


        /// <summary>
        /// Gets the modules in page.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        /// <returns>
        /// A System.Data.SqlClient.SqlDataReader value...
        /// </returns>
        [Obsolete("Replace me, bad design practice to pass SqlDataReaders to the UI")]
        public SqlDataReader GetModulesInPage(int portalId, int pageId)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand(StringsGetModulesInPage, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Mark the Command as a SPROC
            var parameterPortalId = new SqlParameter(StringsPortalId, SqlDbType.Int, 4) { Value = portalId };
            command.Parameters.Add(parameterPortalId);

            // Add Parameters to SPROC
            var parameterPageId = new SqlParameter(StringsPageId, SqlDbType.Int, 4) { Value = pageId };
            command.Parameters.Add(parameterPageId);

            // Open the database connection and execute the command
            connection.Open();
            var dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the data reader
            return dr;
        }

        /// <summary>
        /// Get Modules Single Portal
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <returns>
        /// A data table.
        /// </returns>
        public DataTable GetModulesSinglePortal(int portalId)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlDataAdapter("rb_GetModulesSinglePortal", connection))
            {
                // Mark the Command as a SPROC
                command.SelectCommand.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterPortalId = new SqlParameter(StringsPortalId, SqlDbType.Int) { Value = portalId };
                command.SelectCommand.Parameters.Add(parameterPortalId);

                // Create and Fill the DataSet
                using (var dataTable = new DataTable())
                {
                    try
                    {
                        command.Fill(dataTable);
                    }
                    finally
                    {
                        connection.Close(); // by Manu fix close bug #2
                    }

                    // Translate
                    foreach (var dr in dataTable.Rows.Cast<DataRow>().Where(dr => dr[1].ToString() == StringsNoModule))
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
        /// The GetSingleModuleDefinition method returns a SqlDataReader
        ///   containing details about a specific module definition
        ///   from the ModuleDefinitions table.
        /// </summary>
        /// <param name="generalModDefId">
        /// The general mod def ID.
        /// </param>
        /// <returns>
        /// A single module definition.
        /// </returns>
        /// <remarks>
        /// Other relevant sources: GetSingleModuleDefinition Stored Procedure
        /// </remarks>
        public GeneralModuleDefinition GetSingleModuleDefinition(Guid generalModDefId)
        {
            // Create Instance of Connection and Command Object
            using (var sqlCommand = new SqlCommand("rb_GetSingleModuleDefinition")
            {
                // Mark the Command as a SPROC
                CommandType = CommandType.StoredProcedure
            })
            {
                var parameterGeneralModDefId = new SqlParameter(StringsGeneralModDefId, SqlDbType.UniqueIdentifier)
                {
                    Value = generalModDefId
                };
                sqlCommand.Parameters.Add(parameterGeneralModDefId);

                // Execute the command and get the data reader 
                var dr = DBHelper.GetDataReader(sqlCommand);
                var moduleDefinition = new GeneralModuleDefinition();
                while (dr.Read())
                {
                    moduleDefinition.FriendlyName = dr["FriendlyName"].ToString();
                    moduleDefinition.DesktopSource = dr["DesktopSrc"].ToString();
                    moduleDefinition.MobileSource = dr["MobileSrc"].ToString();
                    moduleDefinition.GeneralModDefID = new Guid(Convert.ToString(dr["GeneralModDefID"]));
                }

                dr.Close();

                return moduleDefinition;
            }
        }

        /*
        /// <summary>
        /// The GetSolutionModuleDefinitions method returns a list of all module type definitions.<br></br>
        ///   GetSolutionModuleDefinitions Stored Procedure
        /// </summary>
        /// <param name="solutionId">
        /// The solution ID.
        /// </param>
        /// <returns>
        /// A SQL Data Reader.
        /// </returns>
        [Obsolete("Replace me, bad design practice to pass SqlDataReaders to the UI")]
        public SqlDataReader GetSolutionModuleDefinitions(int solutionId)
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_GetSolutionModuleDefinitions", connection)
                {
                    // Mark the Command as a SPROC
                    CommandType = CommandType.StoredProcedure
                };

            // Add Parameters to SPROC
            var parameterSolutionId = new SqlParameter("@SolutionID", SqlDbType.Int, 4) { Value = solutionId };
            command.Parameters.Add(parameterSolutionId);

            // Open the database connection and execute the command
            connection.Open();
            var dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the data reader
            return dr;
        }
        */

        /// <summary>
        /// Gets the solution module definitions.
        /// </summary>
        /// <param name="solutionId">The solution id.</param>
        /// <returns>A list of solution module definitions.</returns>
        public List<SolutionModuleDefinition> GetSolutionModuleDefinitions(int solutionId)
        {
            var definitions = new List<SolutionModuleDefinition>();

            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (
                var command = new SqlCommand("rb_GetSolutionModuleDefinitions", connection)
                {
                    // Mark the Command as a SPROC
                    CommandType = CommandType.StoredProcedure
                })
            {
                var parameterSolutionId = new SqlParameter("@SolutionID", SqlDbType.Int, 4) { Value = solutionId };
                command.Parameters.Add(parameterSolutionId);

                // Open the database connection and execute the command
                connection.Open();
                using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    try
                    {
                        // Always call Read before accessing data.
                        while (reader.Read())
                        {
                            var definition = new SolutionModuleDefinition
                            {
                                SolutionModuleDefinitionId = reader.GetInt32(0),
                                GeneralModuleDefinitionId = reader.GetGuid(1),
                                SolutionsId = reader.GetInt32(2)
                            };

                            definitions.Add(definition);
                        }
                    }
                    finally
                    {
                        reader.Close(); // by Manu, fixed bug 807858
                    }
                }
            }

            return definitions;
        }

        /// <summary>
        /// The GetSolutions method returns the Solution list.
        /// </summary>
        /// <returns>
        /// A SQL Data Reader.
        /// </returns>
        /// <remarks>
        /// Other relevant sources: GetUsers Stored Procedure
        /// </remarks>
        [Obsolete("Replace me, bad design practice to pass SqlDataReaders to the UI")]
        public SqlDataReader GetSolutions()
        {
            // Create Instance of Connection and Command Object
            var connection = Config.SqlConnectionString;
            var command = new SqlCommand("rb_GetSolutions", connection)
            {
                // Mark the Command as a SPROC
                CommandType = CommandType.StoredProcedure
            };

            // Open the database connection and execute the command
            connection.Open();
            var dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the data reader
            return dr;
        }

        /// <summary>
        /// Update General Module Definitions
        /// </summary>
        /// <param name="generalModDefId">
        /// General Mod Def ID
        /// </param>
        /// <param name="friendlyName">
        /// Name of the friendly.
        /// </param>
        /// <param name="desktopSrc">
        /// The desktop SRC.
        /// </param>
        /// <param name="mobileSrc">
        /// The mobile SRC.
        /// </param>
        /// <param name="assemblyName">
        /// Name of the assembly.
        /// </param>
        /// <param name="className">
        /// Name of the class.
        /// </param>
        /// <param name="admin">
        /// if set to <c>true</c> [admin].
        /// </param>
        /// <param name="searchable">
        /// if set to <c>true</c> [searchable].
        /// </param>
        public void UpdateGeneralModuleDefinitions(
            Guid generalModDefId,
            string friendlyName,
            string desktopSrc,
            string mobileSrc,
            string assemblyName,
            string className,
            bool admin,
            bool searchable)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_UpdateGeneralModuleDefinitions", connection))
            {
                // Mark the Command as a SPROC
                command.CommandType = CommandType.StoredProcedure;

                // Update Parameters to SPROC
                var parameterGeneralModDefId = new SqlParameter(StringsGeneralModDefId, SqlDbType.UniqueIdentifier)
                {
                    Value = generalModDefId
                };
                command.Parameters.Add(parameterGeneralModDefId);
                var parameterFriendlyName = new SqlParameter(StringsFriendlyName, SqlDbType.NVarChar, 128)
                {
                    Value = friendlyName
                };
                command.Parameters.Add(parameterFriendlyName);
                var parameterDesktopSrc = new SqlParameter(StringsDesktopSrc, SqlDbType.NVarChar, 256)
                {
                    Value = desktopSrc
                };
                command.Parameters.Add(parameterDesktopSrc);
                var parameterMobileSrc = new SqlParameter(StringsMobileSrc, SqlDbType.NVarChar, 256) { Value = mobileSrc };
                command.Parameters.Add(parameterMobileSrc);
                var parameterAssemblyName = new SqlParameter(StringsAssemblyName, SqlDbType.VarChar, 50)
                {
                    Value = assemblyName
                };
                command.Parameters.Add(parameterAssemblyName);
                var parameterClassName = new SqlParameter(StringsClassName, SqlDbType.NVarChar, 128) { Value = className };
                command.Parameters.Add(parameterClassName);
                var parameterAdmin = new SqlParameter(StringsAdmin, SqlDbType.Bit) { Value = admin };
                command.Parameters.Add(parameterAdmin);
                var parameterSearchable = new SqlParameter(StringsSearchable, SqlDbType.Bit) { Value = searchable };
                command.Parameters.Add(parameterSearchable);

                // Execute the command
                connection.Open();

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // ErrorHandler.Publish(Appleseed.Framework.LogLevel.Warn, "An Error Occurred in UpdateGeneralModuleDefinitions", ex));
                    ErrorHandler.Publish(LogLevel.Warn, "An Error Occurred in UpdateGeneralModuleDefinitions", ex);
                }
                //Added by Ashish - Connection Pool Issue
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// The UpdateModule method updates a specified Module within the Modules database table.
        ///   If the module does not yet exist, the stored procedure adds it.<br/>
        ///   UpdateModule Stored Procedure
        /// </summary>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="moduleOrder">
        /// The module order.
        /// </param>
        /// <param name="paneName">
        /// Name of the pane.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="cacheTime">
        /// The cache time.
        /// </param>
        /// <param name="editRoles">
        /// The edit roles.
        /// </param>
        /// <param name="viewRoles">
        /// The view roles.
        /// </param>
        /// <param name="addRoles">
        /// The add roles.
        /// </param>
        /// <param name="deleteRoles">
        /// The delete roles.
        /// </param>
        /// <param name="propertiesRoles">
        /// The properties roles.
        /// </param>
        /// <param name="moveModuleRoles">
        /// The move module roles.
        /// </param>
        /// <param name="deleteModuleRoles">
        /// The delete module roles.
        /// </param>
        /// <param name="showMobile">
        /// if set to <c>true</c> [show mobile].
        /// </param>
        /// <param name="publishingRoles">
        /// The publishing roles.
        /// </param>
        /// <param name="supportWorkflow">
        /// if set to <c>true</c> [support workflow].
        /// </param>
        /// <param name="approvalRoles">
        /// The approval roles.
        /// </param>
        /// <param name="showEveryWhere">
        /// if set to <c>true</c> [show every where].
        /// </param>
        /// <param name="supportCollapsable">
        /// if set to <c>true</c> [support collapsible].
        /// </param>
        /// <returns>
        /// The update module.
        /// </returns>
        public int UpdateModule(
            int pageId,
            int moduleId,
            int moduleOrder,
            string paneName,
            string title,
            int cacheTime,
            string editRoles,
            string viewRoles,
            string addRoles,
            string deleteRoles,
            string propertiesRoles,
            string moveModuleRoles,
            string deleteModuleRoles,
            bool showMobile,
            string publishingRoles,
            bool supportWorkflow,
            string approvalRoles,
            bool showEveryWhere,
            bool supportCollapsable)
        {
            // Changes by Geert.Audenaert@Syntegra.Com Date: 6/2/2003
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            {
                using (var command = new SqlCommand("rb_UpdateModule", connection))
                {
                    // Mark the Command as a SPROC
                    command.CommandType = CommandType.StoredProcedure;

                    // Add Parameters to SPROC
                    var parameterModuleId = new SqlParameter(StringsModuleId, SqlDbType.Int, 4) { Value = moduleId };
                    command.Parameters.Add(parameterModuleId);
                    var parameterPageId = new SqlParameter(StringsPageId, SqlDbType.Int, 4) { Value = pageId };
                    command.Parameters.Add(parameterPageId);
                    var parameterModuleOrder = new SqlParameter(StringsModuleOrder, SqlDbType.Int, 4)
                    {
                        Value = moduleOrder
                    };
                    command.Parameters.Add(parameterModuleOrder);
                    var parameterTitle = new SqlParameter(StringsModuleTitle, SqlDbType.NVarChar, 256) { Value = title };
                    command.Parameters.Add(parameterTitle);
                    var parameterPaneName = new SqlParameter(StringsPaneName, SqlDbType.NVarChar, 256)
                    {
                        Value = paneName
                    };
                    command.Parameters.Add(parameterPaneName);
                    var parameterCacheTime = new SqlParameter(StringsCacheTime, SqlDbType.Int, 4) { Value = cacheTime };
                    command.Parameters.Add(parameterCacheTime);
                    var parameterEditRoles = new SqlParameter(StringsEditRoles, SqlDbType.NVarChar, 256)
                    {
                        Value = editRoles
                    };
                    command.Parameters.Add(parameterEditRoles);
                    var parameterViewRoles = new SqlParameter(StringsViewRoles, SqlDbType.NVarChar, 256)
                    {
                        Value = viewRoles
                    };
                    command.Parameters.Add(parameterViewRoles);
                    var parameterAddRoles = new SqlParameter(StringsAddRoles, SqlDbType.NVarChar, 256)
                    {
                        Value = addRoles
                    };
                    command.Parameters.Add(parameterAddRoles);
                    var parameterDeleteRoles = new SqlParameter(StringsDeleteRoles, SqlDbType.NVarChar, 256)
                    {
                        Value = deleteRoles
                    };
                    command.Parameters.Add(parameterDeleteRoles);
                    var parameterPropertiesRoles = new SqlParameter(StringsPropertiesRoles, SqlDbType.NVarChar, 256)
                    {
                        Value = propertiesRoles
                    };
                    command.Parameters.Add(parameterPropertiesRoles);

                    // Added by jviladiu@portalservices.net (19/08/2004)
                    var parameterMoveModuleRoles = new SqlParameter(StringsMoveModuleRoles, SqlDbType.NVarChar, 256)
                    {
                        Value = moveModuleRoles
                    };
                    command.Parameters.Add(parameterMoveModuleRoles);

                    // Added by jviladiu@portalservices.net (19/08/2004)
                    var parameterDeleteModuleRoles = new SqlParameter(StringsDeleteModuleRoles, SqlDbType.NVarChar, 256)
                    {
                        Value = deleteModuleRoles
                    };
                    command.Parameters.Add(parameterDeleteModuleRoles);

                    // Change by Geert.Audenaert@Syntegra.Com
                    // Date: 6/2/2003
                    var parameterPublishingRoles = new SqlParameter(StringsPublishingRoles, SqlDbType.NVarChar, 256)
                    {
                        Value = publishingRoles
                    };
                    command.Parameters.Add(parameterPublishingRoles);
                    var parameterSupportWorkflow = new SqlParameter(StringsSupportWorkflow, SqlDbType.Bit, 1)
                    {
                        Value = supportWorkflow
                    };
                    command.Parameters.Add(parameterSupportWorkflow);

                    // End Change Geert.Audenaert@Syntegra.Com
                    // Change by Geert.Audenaert@Syntegra.Com
                    // Date: 27/2/2003
                    var parameterApprovalRoles = new SqlParameter(StringsApprovalRoles, SqlDbType.NVarChar, 256)
                    {
                        Value = approvalRoles
                    };
                    command.Parameters.Add(parameterApprovalRoles);

                    // End Change Geert.Audenaert@Syntegra.Com
                    var parameterShowMobile = new SqlParameter(StringsShowMobile, SqlDbType.Bit, 1) { Value = showMobile };
                    command.Parameters.Add(parameterShowMobile);

                    // Addition by john.mandia@whitelightsolutions.com to add show on every page functionality
                    var parameterShowEveryWhere = new SqlParameter(StringsShowEveryWhere, SqlDbType.Bit, 1)
                    {
                        Value = showEveryWhere
                    };
                    command.Parameters.Add(parameterShowEveryWhere);

                    // Change by baj@reedtek.com
                    // Date: 16/5/2003
                    var parameterSupportCollapsable = new SqlParameter(StringsSupportCollapsible, SqlDbType.Bit, 1)
                    {
                        Value = supportCollapsable
                    };
                    command.Parameters.Add(parameterSupportCollapsable);

                    // End Change baj@reedtek.com
                    connection.Open();

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // ErrorHandler.Publish(Appleseed.Framework.LogLevel.Warn, "An Error Occurred in UpdateModule", ex);
                        ErrorHandler.Publish(LogLevel.Warn, "An Error Occurred in UpdateModule", ex);
                    }
                    //Added by Ashish - Connection Pool Issue
                    finally
                    {
                        connection.Close();
                    }

                    return (int)parameterModuleId.Value;
                }
            }
        }

        /// <summary>
        /// The UpdateModuleDefinitions method updates
        ///   all module definitions in every portal
        /// </summary>
        /// <param name="generalModDefId">
        /// The general mod def ID.
        /// </param>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="ischecked">
        /// if set to <c>true</c> [is checked].
        /// </param>
        public void UpdateModuleDefinitions(Guid generalModDefId, int portalId, bool ischecked)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_UpdateModuleDefinitions", connection))
            {
                // Mark the Command as a SPROC
                command.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterGeneralModDefId = new SqlParameter(StringsGeneralModDefId, SqlDbType.UniqueIdentifier)
                {
                    Value = generalModDefId
                };
                command.Parameters.Add(parameterGeneralModDefId);

                // Add Parameters to SPROC
                var parameterPortalId = new SqlParameter(StringsPortalId, SqlDbType.Int, 4) { Value = portalId };
                command.Parameters.Add(parameterPortalId);
                var parameterischecked = new SqlParameter("@ischecked", SqlDbType.Bit) { Value = ischecked };
                command.Parameters.Add(parameterischecked);

                connection.Open();

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // ErrorHandler.Publish(Appleseed.Framework.LogLevel.Warn, "An Error Occurred in UpdateModuleDefinitions", ex);
                    ErrorHandler.Publish(LogLevel.Warn, "An Error Occurred in UpdateModuleDefinitions", ex);
                }
                //Added by Ashish - Connection Pool Issue
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// The UpdateModuleOrder method update Modules Order.<br/>
        ///   UpdateModuleOrder Stored Procedure
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="moduleOrder">
        /// The module order.
        /// </param>
        /// <param name="pane">
        /// The pane name.
        /// </param>
        public void UpdateModuleOrder(int moduleId, int moduleOrder, string pane)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_UpdateModuleOrder", connection))
            {
                // Mark the Command as a SPROC
                command.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterModuleId = new SqlParameter(StringsModuleId, SqlDbType.Int, 4) { Value = moduleId };
                command.Parameters.Add(parameterModuleId);
                var parameterModuleOrder = new SqlParameter(StringsModuleOrder, SqlDbType.Int, 4) { Value = moduleOrder };
                command.Parameters.Add(parameterModuleOrder);
                var parameterPaneName = new SqlParameter(StringsPaneName, SqlDbType.NVarChar, 256) { Value = pane };
                command.Parameters.Add(parameterPaneName);
                connection.Open();

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // ErrorHandler.Publish(Appleseed.Framework.LogLevel.Warn, "An Error Occurred in UpdateModuleOrder", ex);
                    ErrorHandler.Publish(LogLevel.Warn, "An Error Occurred in UpdateModuleOrder", ex);
                }
                //Added by Ashish - Connection Pool Issue
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// The UpdateModuleSetting Method updates a single module setting
        ///   in the ModuleSettings database table.
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="key">
        /// The setting key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        [Obsolete("UpdateModuleSetting was moved to ModuleSettings.UpdateModuleSetting", false)]
        public void UpdateModuleSetting(int moduleId, string key, string value)
        {
            ModuleSettings.UpdateModuleSetting(moduleId, key, value);
        }

        /// <summary>
        /// The UpdateModuleTitle method updates a specified Module within the Modules database table.
        ///   If the module does not yet exist, the stored procedure adds it.<br/>
        ///   UpdateModuleTitle Stored Procedure
        /// </summary>
        /// <param name="moduleId">
        /// The module ID.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>

        public int UpdateModuleTitle(int moduleId, string title)
        {
            using (var connection = Config.SqlConnectionString)
            {
                using (var command = new SqlCommand("rb_UpdateModuleTitle", connection))
                {
                    // Mark the Command as a SPROC
                    command.CommandType = CommandType.StoredProcedure;

                    // Add Parameters to SPROC
                    var parameterModuleId = new SqlParameter(StringsModuleId, SqlDbType.Int, 4) { Value = moduleId };
                    command.Parameters.Add(parameterModuleId);
                    var parameterTitle = new SqlParameter(StringsModuleTitle, SqlDbType.NVarChar, 256) { Value = title };
                    command.Parameters.Add(parameterTitle);

                    // End Change baj@reedtek.com
                    connection.Open();

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // ErrorHandler.Publish(Appleseed.Framework.LogLevel.Warn, "An Error Occurred in UpdateModule", ex);
                        ErrorHandler.Publish(LogLevel.Warn, "An Error Occurred in UpdateModuleTitle", ex);
                        throw new Exception("An Error Occurred in UpdateModuleTitle");
                    }
                    //Added by Ashish - Connection Pool Issue
                    finally
                    {
                        connection.Close();
                    }

                    return (int)parameterModuleId.Value;
                }
            }
        }

        /// <summary>
        /// Get module settings in page
        /// </summary>
        /// <param name="PageId">page id</param>
        /// <param name="PaneName">pane name</param>
        /// <returns>list of module settings</returns>
        public List<IModuleSettings> getModulesSettingsInPage(int PageId, string PaneName)
        {

            var modules = new List<IModuleSettings>();


            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_GetModulesSettingsInPage", connection)
            {
                // Mark the Command as a SPROC
                CommandType = CommandType.StoredProcedure
            })
            {

                // Add Parameters to SPROC
                var parameterPageId = new SqlParameter(StringsPageId, SqlDbType.Int, 4) { Value = PageId };
                command.Parameters.Add(parameterPageId);

                var parameterFriendlyName = new SqlParameter(StringsPaneName, SqlDbType.NVarChar, 50) { Value = PaneName };
                command.Parameters.Add(parameterFriendlyName);

                // Open the database connection and execute the command
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    try
                    {
                        // Always call Read before accessing data.
                        while (reader.Read())
                        {
                            IModuleSettings module = new ModuleSettings();
                            module.ModuleID = reader.GetInt32(0);
                            module.ModuleDefID = reader.GetInt32(1);
                            module.ModuleOrder = reader.GetInt32(2);
                            module.ModuleTitle = reader.GetString(3);
                            module.PageID = PageId;
                            module.PaneName = PaneName;

                            modules.Add(module);
                        }
                    }
                    finally
                    {
                        reader.Close();
                        connection.Close();
                    }
                }
            }

            return modules;
        }

        /// <summary>
        /// Get module by portal id
        /// </summary>
        /// <param name="portalID"></param>
        /// <returns></returns>
        public List<IModuleSettings> GetModuleByPortalID(int portalID)
        {
            var modules = new List<IModuleSettings>();
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_GetHtmlModuleByPortalID", connection)
            {
                // Mark the Command as a SPROC
                CommandType = CommandType.StoredProcedure
            })
            {

                // Add Parameters to SPROC
                var parameterPageId = new SqlParameter(SelectedPortalID, SqlDbType.Int, 4) { Value = portalID };
                command.Parameters.Add(parameterPageId);

                // Open the database connection and execute the command
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    try
                    {
                        // Always call Read before accessing data.
                        while (reader.Read())
                        {
                            IModuleSettings module = new ModuleSettings();
                            module.ModuleID = reader.GetInt32(0);
                            module.ModuleTitle = reader.GetString(1);
                            modules.Add(module);
                        }
                    }
                    finally
                    {
                        reader.Close();
                        connection.Close();
                    }
                }
            }
            return modules;
        }

        #endregion

        internal static int AddModuleDefinition(Guid generalModuleDefId, int portalId)
        {
            var moduleDefId = -1;
            using (AppleseedDBContext context = new AppleseedDBContext())
            {
                var current = context.rb_ModuleDefinitions.Where(d => d.GeneralModDefID == generalModuleDefId && d.PortalID == portalId).FirstOrDefault();

                if (current != default(rb_ModuleDefinitions))
                {
                    moduleDefId = current.ModuleDefID;
                }
                else
                {
                    var entity = new rb_ModuleDefinitions() { PortalID = portalId, GeneralModDefID = generalModuleDefId };
                    context.rb_ModuleDefinitions.AddObject(entity);
                    context.SaveChanges();
                    moduleDefId = entity.ModuleDefID;
                }
            }

            return moduleDefId;
        }
    }
}