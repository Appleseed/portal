// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Services.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The services.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Update
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Xml;

    using Appleseed.Framework.Data;
    using Appleseed.Framework.Helpers;
    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Settings.Cache;

    /// <summary>
    /// The services.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class Services : IDisposable
    {
        #region Constants and Fields

        /// <summary>
        ///   The scripts list.
        /// </summary>
        private UpdateEntry[] scriptsList;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets db version.
        ///   It does not rely on cached value and always gets the actual value.
        /// </summary>
        /// <value>The database version.</value>
        /// <remarks>
        /// </remarks>
        private static int DatabaseVersion
        {
            get
            {
                // Clear version cache so we are sure we update correctly
                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application[Database.dbKey] = null;
                HttpContext.Current.Application.UnLock();
                return Database.DatabaseVersion;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The run db update.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <returns>
        /// The run db update.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public bool RunDBUpdate(string connectionString)
        {
            CurrentCache.Insert(Portal.UniqueID + "_ConnectionString", connectionString);

            var databaseVersion = DatabaseVersion;

            var xmlDocument = new XmlDocument();
            var tempScriptsList = new List<UpdateEntry>();

            try
            {
                if (databaseVersion < Portal.CodeVersion)
                {
                    ErrorHandler.Publish(
                        LogLevel.Debug, string.Format("db:{0} Code:{1}", databaseVersion, Portal.CodeVersion));

                    // load the history file
                    var docPath =
                        HttpContext.Current.Server.MapPath("~/Setup/Scripts/History.xml");
                    
                    xmlDocument.Load(docPath);

                    // get a list of <Release> nodes
                    if (xmlDocument.DocumentElement != null)
                    {
                        var releases = xmlDocument.DocumentElement.SelectNodes("Release");
                        if (releases != null)
                        {

                            // iterate over the <Release> nodes
                            // (we can do this because XmlNodeList implements IEnumerable)
                            foreach (XmlNode release in releases)
                            {
                                var updateEntry = new UpdateEntry();

                                // get the header information
                                // we check for null to avoid exception if any of these nodes are not present
                                if (release.SelectSingleNode("ID") != null)
                                {
                                    updateEntry.VersionNumber = Int32.Parse(release.SelectSingleNode("ID/text()").Value);
                                }

                                if (release.SelectSingleNode("Version") != null)
                                {
                                    updateEntry.Version = release.SelectSingleNode("Version/text()").Value;
                                }

                                if (release.SelectSingleNode("Script") != null)
                                {
                                    updateEntry.ScriptNames.Add(release.SelectSingleNode("Script/text()").Value);
                                }

                                if (release.SelectSingleNode("Date") != null)
                                {
                                    updateEntry.Date = DateTime.Parse(release.SelectSingleNode("Date/text()").Value);
                                }

                                // We should apply this patch
                                if (databaseVersion < updateEntry.VersionNumber)
                                {
                                    // Appleseed.Framework.Helpers.LogHelper.Logger.Log(Appleseed.Framework.Site.Configuration.LogLevel.Debug, "Detected version to apply: " + myUpdate.Version);
                                    updateEntry.Apply = true;

                                    // get a list of <Installer> nodes
                                    var installers = release.SelectNodes("Modules/Installer/text()");

                                    // iterate over the <Installer> Nodes (in original document order)
                                    // (we can do this because XmlNodeList implements IEnumerable)
                                    foreach (XmlNode installer in installers)
                                    {
                                        // and build an ArrayList of the scripts... 
                                        updateEntry.Modules.Add(installer.Value);

                                        // Appleseed.Framework.Helpers.LogHelper.Logger.Log(Appleseed.Framework.Site.Configuration.LogLevel.Debug, "Detected module to install: " + installer.Value);
                                    }

                                    // get a <Script> node, if any
                                    var sqlScripts = release.SelectNodes("Scripts/Script/text()");

                                    // iterate over the <Installer> Nodes (in original document order)
                                    // (we can do this because XmlNodeList implements IEnumerable)
                                    foreach (XmlNode sqlScript in sqlScripts)
                                    {
                                        // and build an ArrayList of the scripts... 
                                        updateEntry.ScriptNames.Add(sqlScript.Value);

                                        // Appleseed.Framework.Helpers.LogHelper.Logger.Log(Appleseed.Framework.Site.Configuration.LogLevel.Debug, "Detected script to run: " + sqlScript.Value);
                                    }

                                    tempScriptsList.Add(updateEntry);
                                }
                            }
                        }
                    }

                    // If we have some version to apply...
                    if (tempScriptsList.Count <= 0)
                    {
                        // No update is needed
                    }
                    else
                    {
                        this.scriptsList = tempScriptsList.ToArray();

                        // by Manu. Versions are sorted by version number
                        Array.Sort(this.scriptsList);

                        // Create a flat version for binding
                        var currentVersion = 0;
                        var databindList = new List<string>();
                        foreach (var updateEntry in this.scriptsList.Where(updateEntry => updateEntry.Apply))
                        {
                            if (currentVersion != updateEntry.VersionNumber)
                            {
                                databindList.Add(string.Format("Version: {0}", updateEntry.VersionNumber));
                                currentVersion = updateEntry.VersionNumber;
                            }

                            databindList.AddRange(
                                updateEntry.ScriptNames.Where(scriptName => scriptName.Length > 0).Select(
                                    scriptName => string.Format("-- Script: {0}", scriptName)));
                            databindList.AddRange(
                                updateEntry.Modules.Where(moduleInstaller => moduleInstaller.Length > 0).Select(
                                    moduleInstaller => string.Format("-- Module: {0}", moduleInstaller)));
                        }

                        var errors = new List<object>();

                        // var messages = new List<object>();
                        foreach (var updateEntry in this.scriptsList)
                        {
                            if (updateEntry.Apply && DatabaseVersion < updateEntry.VersionNumber &&
                                DatabaseVersion < Portal.CodeVersion)
                            {
                                // Version check (a script may update more than one version at once)
                                foreach (var scriptName in updateEntry.ScriptNames)
                                {
                                    if (scriptName.Length <= 0)
                                    {
                                        continue;
                                    }

                                    // It may be a module update only
                                    var currentScriptName =
                                        HttpContext.Current.Server.MapPath(
                                            System.IO.Path.Combine("~/Setup/Scripts/", scriptName));
                                    ErrorHandler.Publish(
                                        LogLevel.Info,
                                        string.Format(
                                            "CODE: {0} - DB: {1} - CURR: {2} - Applying: {3}",
                                            Portal.CodeVersion,
                                            DatabaseVersion,
                                            updateEntry.VersionNumber,
                                            currentScriptName));
                                    var myerrors = DBHelper.ExecuteScript(currentScriptName, true);
                                    errors.AddRange(myerrors); // Display errors if any

                                    if (myerrors.Count <= 0)
                                    {
                                        continue;
                                    }

                                    errors.Insert(0, string.Format("<p>{0}</p>", scriptName));
                                    ErrorHandler.Publish(
                                        LogLevel.Error,
                                        string.Format(
                                            "Version {0} completed with errors.  - {1}", updateEntry.Version, scriptName));
                                    break;
                                }

                                // Installing modules
                                foreach (var currentModuleInstaller in from string moduleInstaller in updateEntry.Modules
                                                                       select HttpContext.Current.Server.MapPath(System.IO.Path.Combine("~/", moduleInstaller)))
                                {
                                    try
                                    {
                                        ModuleInstall.InstallGroup(currentModuleInstaller, true);
                                    }
                                    catch (Exception ex)
                                    {
                                        ErrorHandler.Publish(
                                            LogLevel.Fatal,
                                            string.Format(
                                                "Exception in UpdateDatabaseCommand installing module: {0}",
                                                currentModuleInstaller),
                                            ex);
                                        if (ex.InnerException != null)
                                        {
                                            // Display more meaningful error message if InnerException is defined
                                            ErrorHandler.Publish(
                                                LogLevel.Warn,
                                                string.Format(
                                                    "Exception in UpdateDatabaseCommand installing module: {0}",
                                                    currentModuleInstaller),
                                                ex.InnerException);
                                            errors.Add(
                                                string.Format(
                                                    "Exception in UpdateDatabaseCommand installing module: {0}<br />{1}<br />{2}",
                                                    currentModuleInstaller,
                                                    ex.InnerException.Message,
                                                    ex.InnerException.StackTrace));
                                        }
                                        else
                                        {
                                            ErrorHandler.Publish(
                                                LogLevel.Warn,
                                                string.Format(
                                                    "Exception in UpdateDatabaseCommand installing module: {0}",
                                                    currentModuleInstaller),
                                                ex);
                                            errors.Add(ex.Message);
                                        }
                                    }
                                }

                                if (Equals(errors.Count, 0))
                                {
                                    // Update db with version
                                    var versionUpdater =
                                        string.Format(
                                            "INSERT INTO [rb_Versions] ([Release],[Version],[ReleaseDate]) VALUES('{0}','{1}', CONVERT(datetime, '{2}/{3}/{4}', 101))",
                                            updateEntry.VersionNumber,
                                            updateEntry.Version,
                                            updateEntry.Date.Month,
                                            updateEntry.Date.Day,
                                            updateEntry.Date.Year);
                                    DBHelper.ExeSQL(versionUpdater);
                                    ErrorHandler.Publish(
                                        LogLevel.Info,
                                        string.Format("Version number: {0} applied successfully.", updateEntry.Version));

                                    // Mark this update as done
                                    ErrorHandler.Publish(
                                        LogLevel.Info,
                                        string.Format("Successfully applied version: {0}", updateEntry.Version));
                                }
                            }
                            else
                            {
                                ErrorHandler.Publish(
                                    LogLevel.Info,
                                    string.Format(
                                        "CODE: {0} - DB: {1} - CURR: {2} - Skipping: {3}",
                                        Portal.CodeVersion,
                                        DatabaseVersion,
                                        updateEntry.VersionNumber,
                                        updateEntry.Version));
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorHandler.Publish(LogLevel.Error, "An error occurred during the installation process.", exception);
                throw ;
            }
            return true;
        }

        #endregion

        #region Implemented Interfaces

        #region IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected void Dispose(bool disposing)
        {
            // TODO Implement the pattern properly.
        }

        #endregion
    }
}