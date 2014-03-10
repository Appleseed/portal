// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleInstall.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   ModuleInstall incapsulates all the logic for install,
//   uninstall modules on portal
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Helpers
{
    using System;
    using System.Data;
    using System.IO;
    using System.Web;
    using System.Web.UI;

    using Appleseed.Framework.Site.Data;
    using Appleseed.Framework.Web.UI.WebControls;

    using Path = Appleseed.Framework.Settings.Path;

    /// <summary>
    /// ModuleInstall incapsulates all the logic for install, 
    ///   uninstall modules on portal
    /// </summary>
    [History("jminond", "2006/02/22", "corrected case where install group is null exception")]
    public class ModuleInstall
    {
        #region Public Methods

        /// <summary>
        /// Installs the specified friendly name.
        /// </summary>
        /// <param name="friendlyName">
        /// Name of the friendly.
        /// </param>
        /// <param name="desktopSource">
        /// The desktop source.
        /// </param>
        /// <param name="mobileSource">
        /// The mobile source.
        /// </param>
        public static void Install(string friendlyName, string desktopSource, string mobileSource)
        {
            Install(friendlyName, desktopSource, mobileSource, true);
        }

        /// <summary>
        /// Installs module
        /// </summary>
        /// <param name="friendlyName">
        /// Name of the friendly.
        /// </param>
        /// <param name="desktopSource">
        /// The desktop source.
        /// </param>
        /// <param name="mobileSource">
        /// The mobile source.
        /// </param>
        /// <param name="install">
        /// if set to <c>true</c> [install].
        /// </param>
        public static void Install(string friendlyName, string desktopSource, string mobileSource, bool install)
        {
            ErrorHandler.Publish(
                LogLevel.Info, string.Format("Installing DesktopModule '{0}' from '{1}'", friendlyName, desktopSource));
            if (!string.IsNullOrEmpty(mobileSource))
            {
                ErrorHandler.Publish(
                    LogLevel.Info, string.Format("Installing MobileModule '{0}' from '{1}'", friendlyName, mobileSource));
            }

            var controlFullPath = Path.ApplicationRoot + "/" + desktopSource;

            // if ascx User Control based Module-> Instantiate the module
            if (controlFullPath.ToLowerInvariant().Trim().EndsWith(".ascx"))
            {
                var page = new Page();

                var control = page.LoadControl(controlFullPath);
                if (!(control is PortalModuleControl))
                {
                    throw new Exception(string.Format("Module '{0}' is not a PortalModule Control", control.GetType().FullName));
                }

                var portalModule = (PortalModuleControl)control;

                // Check mobile module
                if (!string.IsNullOrEmpty(mobileSource) && mobileSource.ToLower().EndsWith(".ascx"))
                {
                    // TODO: Check mobile module
                    // TODO: MobilePortalModuleControl mobileModule = (MobilePortalModuleControl) page.LoadControl(Appleseed.Framework.Settings.Path.ApplicationRoot + "/" + mobileSource);
                    if (!File.Exists(HttpContext.Current.Server.MapPath(Path.ApplicationRoot + "/" + mobileSource)))
                    {
                        throw new FileNotFoundException("Mobile Control not found");
                    }
                }

                // Get Module ID
                var defId = portalModule.GuidID;

                var baseType = portalModule.GetType().BaseType;
                
                // Get Assembly name
                if (baseType != null)
                {
                    var assemblyName = baseType.Assembly.CodeBase;
                    assemblyName = assemblyName.Substring(assemblyName.LastIndexOf('/') + 1); // Get name only

                    // Get Module Class name
                    var className = baseType.FullName;

                    // Now we add the definition to module list 
                    var modules = new ModulesDB();

                    if (install)
                    {
                        // Install as new module

                        // Call Install
                        try
                        {
                            ErrorHandler.Publish(LogLevel.Debug, string.Format("Installing '{0}' as new module.", friendlyName));
                            portalModule.Install(null);
                        }
                        catch (Exception ex)
                        {
                            // Error occurred
                            portalModule.Rollback(null);

                            // Re-throw exception
                            throw new Exception(string.Format("Exception occurred installing '{0}'!", portalModule.GuidID), ex);
                        }

                        try
                        {
                            // Add a new module definition to the database
                            modules.AddGeneralModuleDefinitions(
                                defId, 
                                friendlyName, 
                                desktopSource, 
                                mobileSource, 
                                assemblyName, 
                                className, 
                                portalModule.AdminModule, 
                                portalModule.Searchable);
                        }
                        catch (Exception ex)
                        {
                            // Re-throw exception
                            throw new Exception(string.Format("AddGeneralModuleDefinitions Exception '{0}'!", portalModule.GuidID), ex);
                        }

                        // All is fine: we can call Commit
                        portalModule.Commit(null);
                    }
                    else
                    {
                        // Update the general module definition
                        try
                        {
                            ErrorHandler.Publish(LogLevel.Debug, string.Format("Updating '{0}' as new module.", friendlyName));
                            modules.UpdateGeneralModuleDefinitions(
                                defId, 
                                friendlyName, 
                                desktopSource, 
                                mobileSource, 
                                assemblyName, 
                                className, 
                                portalModule.AdminModule, 
                                portalModule.Searchable);
                        }
                        catch (Exception ex)
                        {
                            // Re-throw exception
                            throw new Exception(
                                string.Format("UpdateGeneralModuleDefinitions Exception '{0}'!", portalModule.GuidID), ex);
                        }
                    }

                    // Update the module definition - install for portal 0
                    modules.UpdateModuleDefinitions(defId, 0, true);
                }
            }
        }

        /// <summary>
        /// Installs the group.
        /// </summary>
        /// <param name="groupFileName">
        /// Name of the group file.
        /// </param>
        /// <param name="install">
        /// if set to <c>true</c> [install].
        /// </param>
        public static void InstallGroup(string groupFileName, bool install)
        {
            var modules = GetInstallGroup(groupFileName);

            // In case Modules are null
            if (modules != null && (modules.Rows.Count > 0))
            {
                foreach (DataRow r in modules.Rows)
                {
                    var friendlyName = r["FriendlyName"].ToString();
                    var desktopSource = r["DesktopSource"].ToString();
                    var mobileSource = r["MobileSource"].ToString();

                    Install(friendlyName, desktopSource, mobileSource, install);
                }
            }
            else
            {
                var ex = new Exception("Tried to install 0 modules in groupFileName:" + groupFileName);
                ErrorHandler.Publish(LogLevel.Warn, ex);
            }
        }

        /// <summary>
        /// Uninstalls the specified desktop source.
        /// </summary>
        /// <param name="desktopSource">
        /// The desktop source.
        /// </param>
        /// <param name="mobileSource">
        /// The mobile source.
        /// </param>
        public static void Uninstall(string desktopSource, string mobileSource)
        {
            var page = new Page();

            // Instantiate the module
            var portalModule = (PortalModuleControl)page.LoadControl(string.Format("{0}/{1}", Path.ApplicationRoot, desktopSource));

            // Call Uninstall
            try
            {
                portalModule.Uninstall(null);
            }
            catch (Exception ex)
            {
                // Re-throw exception
                throw new Exception("Exception during uninstall!", ex);
            }

            // Delete definition
            new ModulesDB().DeleteModuleDefinition(portalModule.GuidID);
        }

        /// <summary>
        /// Uninstalls the group.
        /// </summary>
        /// <param name="groupFileName">
        /// Name of the group file.
        /// </param>
        public static void UninstallGroup(string groupFileName)
        {
            var modules = GetInstallGroup(groupFileName);

            foreach (DataRow r in modules.Rows)
            {
                // string friendlyName = r["FriendlyName"].ToString();
                var desktopSource = r["DesktopSource"].ToString();
                var mobileSource = r["MobileSource"].ToString();

                Uninstall(desktopSource, mobileSource);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the install group.
        /// </summary>
        /// <param name="groupFileName">
        /// Name of the group file.
        /// </param>
        /// <returns>
        /// A data table.
        /// </returns>
        private static DataTable GetInstallGroup(string groupFileName)
        {
            // Load the XML as dataset
            using (var ds = new DataSet())
            {
                var installer = groupFileName;

                try
                {
                    ds.ReadXml(installer);
                }
                catch (Exception ex)
                {
                    ErrorHandler.Publish(LogLevel.Error, string.Format("Exception installing module: {0}", installer), ex);
                    return null;
                }

                return ds.Tables[0];
            }
        }

        #endregion
    }
}