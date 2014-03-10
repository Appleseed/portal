using System;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Xml;
using Appleseed.Framework.Data;
using Appleseed.Framework.Helpers;
using Appleseed.Framework;

namespace Appleseed.Tests
{
    [Serializable]
    class UpdateEntry : IComparable
    {
        /// <summary>
        /// IComparable.CompareTo implementation.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than obj. Zero This instance is equal to obj. Greater than zero This instance is greater than obj.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">obj is not the same type as this instance. </exception>
        public int CompareTo(object obj)
        {
            if (obj is UpdateEntry)
            {
                UpdateEntry upd = (UpdateEntry)obj;
                if (VersionNumber.CompareTo(upd.VersionNumber) == 0) //Version numbers are equal
                {
                    return Version.CompareTo(upd.Version);
                }
                else
                {
                    return VersionNumber.CompareTo(upd.VersionNumber);
                }
            }
            throw new ArgumentException("object is not a UpdateEntry");
        }

        public int VersionNumber = 0;
        public string Version = string.Empty;
        public ArrayList scriptNames = new ArrayList();
        public DateTime Date;
        public ArrayList Modules = new ArrayList();
        public bool Apply = false;
    }


    public class TestHelper
    {
        #region Tear down DB

        public static void TearDownDB()
        {
            RunDataScript("TearDown.sql");
        }

        #endregion

        #region Recreate DB based off last Update

        static UpdateEntry[] scriptsList;

        public static void RecreateDBSchema()
        {
            InitializeScriptList();
            RunScriptList();

        }

        private static void InitializeScriptList()
        {

            int dbVersion = 0;

            XmlDocument myDoc = new XmlDocument();
            ArrayList tempScriptsList = new ArrayList();

            // load the history file
            string myDocPath = ConfigurationManager.AppSettings["AppleseedSetupScriptsPath"] + "History.xml";
            myDoc.Load(myDocPath);

            // get a list of <Release> nodes
            XmlNodeList releases = myDoc.DocumentElement.SelectNodes("Release");

            foreach (XmlNode release in releases)
            {
                UpdateEntry myUpdate = new UpdateEntry();

                // get the header information
                // we check for null to avoid exception if any of these nodes are not present
                if (release.SelectSingleNode("ID") != null)
                {
                    myUpdate.VersionNumber = Int32.Parse(release.SelectSingleNode("ID/text()").Value);
                }

                if (release.SelectSingleNode("Version") != null)
                {
                    myUpdate.Version = release.SelectSingleNode("Version/text()").Value;
                }

                if (release.SelectSingleNode("Script") != null)
                {
                    myUpdate.scriptNames.Add(release.SelectSingleNode("Script/text()").Value);
                }

                if (release.SelectSingleNode("Date") != null)
                {
                    myUpdate.Date = DateTime.Parse(release.SelectSingleNode("Date/text()").Value);
                }

                //We should apply this patch
                if (dbVersion < myUpdate.VersionNumber)
                {
                    //Appleseed.Framework.Helpers.LogHelper.Logger.Log(Appleseed.Framework.Site.Configuration.LogLevel.Debug, "Detected version to apply: " + myUpdate.Version);

                    myUpdate.Apply = true;

                    // get a list of <Installer> nodes
                    XmlNodeList installers = release.SelectNodes("Modules/Installer/text()");

                    // iterate over the <Installer> Nodes (in original document order)
                    // (we can do this because XmlNodeList implements IEnumerable)
                    foreach (XmlNode installer in installers)
                    {
                        //and build an ArrayList of the scripts... 
                        myUpdate.Modules.Add(installer.Value);
                        //Appleseed.Framework.Helpers.LogHelper.Logger.Log(Appleseed.Framework.Site.Configuration.LogLevel.Debug, "Detected module to install: " + installer.Value);
                    }

                    // get a <Script> node, if any
                    XmlNodeList sqlScripts = release.SelectNodes("Scripts/Script/text()");

                    // iterate over the <Installer> Nodes (in original document order)
                    // (we can do this because XmlNodeList implements IEnumerable)
                    foreach (XmlNode sqlScript in sqlScripts)
                    {
                        //and build an ArrayList of the scripts... 
                        myUpdate.scriptNames.Add(sqlScript.Value);
                        //Appleseed.Framework.Helpers.LogHelper.Logger.Log(Appleseed.Framework.Site.Configuration.LogLevel.Debug, "Detected script to run: " + sqlScript.Value);
                    }

                    tempScriptsList.Add(myUpdate);
                }
            }

            //If we have some version to apply...
            if (tempScriptsList.Count > 0)
            {
                scriptsList = (UpdateEntry[])tempScriptsList.ToArray(typeof(UpdateEntry));

                //by Manu. Versions are sorted by version number
                Array.Sort(scriptsList);

                //Create a flat version for binding
                int currentVersion = 0;
                foreach (UpdateEntry myUpdate in scriptsList)
                {
                    if (myUpdate.Apply)
                    {
                        if (currentVersion != myUpdate.VersionNumber)
                        {
                            LogHelper.Logger.Log(LogLevel.Debug, "Version: " + myUpdate.VersionNumber);
                            currentVersion = myUpdate.VersionNumber;
                        }

                        foreach (string scriptName in myUpdate.scriptNames)
                        {
                            if (scriptName.Length > 0)
                            {
                                LogHelper.Logger.Log(LogLevel.Debug, "-- Script: " + scriptName);
                            }
                        }

                        foreach (string moduleInstaller in myUpdate.Modules)
                        {
                            if (moduleInstaller.Length > 0)
                                LogHelper.Logger.Log(LogLevel.Debug, "-- Module: " + moduleInstaller + " (ignored recreating test DB)");
                        }
                    }
                }
            }
        }

        private static void RunScriptList()
        {

            XmlDocument dataScriptsDoc = new XmlDocument();
            dataScriptsDoc.Load(ConfigurationManager.AppSettings["DataScriptsDefinitionFile"]);

            int DatabaseVersion = 0;
            ArrayList errors = new ArrayList();

            foreach (UpdateEntry myUpdate in scriptsList)
            {
                //Version check (a script may update more than one version at once)
                if (myUpdate.Apply && DatabaseVersion < myUpdate.VersionNumber)
                {
                    foreach (string scriptName in myUpdate.scriptNames)
                    {
                        //It may be a module update only
                        if (scriptName.Length > 0)
                        {
                            string currentScriptName = scriptName;
                            Console.WriteLine("DB: " + DatabaseVersion + " - CURR: " +
                                               myUpdate.VersionNumber + " - Applying: " + currentScriptName);

                            RunAppleseedScript(currentScriptName);
                        }
                    }

                    ////Installing modules
                    //foreach ( string moduleInstaller in myUpdate.Modules ) {
                    //    string currentModuleInstaller =
                    //        Server.MapPath( System.IO.Path.Combine( Path.ApplicationRoot + "/", moduleInstaller ) );

                    //    try {
                    //        ModuleInstall.InstallGroup( currentModuleInstaller, true );
                    //    }
                    //    catch ( Exception ex ) {
                    //        Console.WriteLine( "Exception in UpdateDatabaseCommand installing module: " +
                    //                           currentModuleInstaller, ex );
                    //        if ( ex.InnerException != null ) {
                    //            // Display more meaningful error message if InnerException is defined
                    //            Console.WriteLine( "Exception in UpdateDatabaseCommand installing module: " +
                    //                               currentModuleInstaller, ex.InnerException );
                    //            errors.Add( "Exception in UpdateDatabaseCommand installing module: " +
                    //                        currentModuleInstaller + "<br/>" + ex.InnerException.Message + "<br/>" +
                    //                        ex.InnerException.StackTrace );
                    //        }
                    //        else {
                    //            Console.WriteLine( "Exception in UpdateDatabaseCommand installing module: " +
                    //                               currentModuleInstaller, ex );
                    //            errors.Add( ex.Message );
                    //        }
                    //    }
                    //}

                    if (Equals(errors.Count, 0))
                    {
                        //Update db with version
                        DatabaseVersion = myUpdate.VersionNumber;
                        Console.WriteLine("Version number: " + myUpdate.Version + " applied successfully.");

                        // apply any additional data scripts after applying version
                        XmlNodeList dataScriptsNodeList = dataScriptsDoc.SelectNodes("/SqlDataScripts/SqlDataScript[@runAfterVersion=" + myUpdate.VersionNumber + "]");
                        foreach (XmlNode node in dataScriptsNodeList)
                        {
                            Console.WriteLine("Running data script " + node.Attributes["fileName"].Value + " after version " + myUpdate.VersionNumber);
                            RunDataScript(node.Attributes["fileName"].Value);
                        }

                        //Mark this update as done
                        Console.WriteLine("Sucessfully applied version: " + myUpdate.Version);
                    }
                }
                else
                {
                    Console.WriteLine("DB: " + DatabaseVersion + " - CURR: " +
                                       myUpdate.VersionNumber + " - Skipping: " + myUpdate.Version);
                }
            }
        }

        #endregion

        #region RunScript

        public static void RunAppleseedScript(string strRelativeScriptPath)
        {
            //1 - prepend full root
            /*NOTE: we can't simply get the Executing Path of the calling assembly
				* because that will vary if the assembly is shadow-copied by NUnit or 
				* other testing tools. Therefore, for simplicity in this demo, we just
				* get it as a App key.
				*/

            string strUTRoot = ConfigurationManager.AppSettings["AppleseedSetupScriptsPath"].ToString();
            string strFullScriptPath = strUTRoot + strRelativeScriptPath;
            _RunScript(strFullScriptPath);
        }

        public static void RunDataScript(string strRelativeScriptPath)
        {
            //1 - prepend full root
            /*NOTE: we can't simply get the Executing Path of the calling assembly
				* because that will vary if the assembly is shadow-copied by NUnit or 
				* other testing tools. Therefore, for simplicity in this demo, we just
				* get it as a App key.
				*/

            string strUTRoot = ConfigurationManager.AppSettings["DataScriptsPath"].ToString();
            string strFullScriptPath = strUTRoot + strRelativeScriptPath;
            _RunScript(strFullScriptPath);
        }

        private static void _RunScript(string strFullScriptPath)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            DBHelper.ExecuteScript(strFullScriptPath, conn);
            Console.WriteLine("Ran script " + strFullScriptPath);
        }

        #endregion
    }

    internal struct SqlAdmin
    {
        public SqlAdmin(string strUserId, string strPassword, string strServer, string database)
        {
            _strUserId = strUserId;
            _strPassword = strPassword;
            _strServer = strServer;
            _database = database;
        }

        private string _strUserId;

        public string UserId
        {
            get { return _strUserId; }
        }

        private string _strPassword;

        public string Password
        {
            get { return _strPassword; }
        }

        private string _strServer;

        public string Server
        {
            get { return _strServer; }
        }

        private string _database;

        public string Database
        {
            get { return _database; }
        }
    }
}
