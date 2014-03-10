// --------------------------------------------------------------------------------------------------------------------
// <copyright file="default.aspx.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The default.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Installer
{
    using System;
    using System.Collections;
    using System.Data.SqlClient;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    using Appleseed.Framework.Update;
    using Appleseed.Framework;
using System.Configuration;

    /// <summary>
    /// The default.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class Default : Page
    {
        /**************************************************************
        Title: Appleseed Web Installer v.1.0
        Author: Rahul Singh (Anant), rahul.singh@anant.us
        Date: 9.8.2006

        Special thanks to the following projects from which I borrowed ideas from.
        1. CommunityServer.org - First ASP.NET Open Source project with a web based installer.
        2. Joomla.org - Really nice open source CMS / Portal written in PHP with a nice installer.
        3. WordPress.org - Great Open Source Blogging Software written in PHP with a nice installer.

        The original file for this installer came from CommunityServer.
        I later used a few functions from it to complete this installer. 


        ***************************************************************/

        /**************************************************************
        To enable the web based installer change the
        line beneath this section to ‘true’.

        After running the installer it is highly recommended that you 
        set this value back to false to disable unauthorized access.
        **************************************************************/
        #region Constants and Fields

        /// <summary>
        ///   The email from text.
        /// </summary>
        public string EmailFromText;

        /// <summary>
        ///   The portal prefix text.
        /// </summary>
        public string PortalPrefixText;

        /// <summary>
        ///   The smtp server text.
        /// </summary>
        public string SmtpServerText;

        /// <summary>
        ///   The installer enabled.
        /// </summary> 
        private const bool InstallerEnabled =  true;

        /// <summary>
        ///   constant string used to allow host to pass the database name to the wizard. If the database can be found in the
        ///   list of database returned, the wizard will skip the database selection page.
        /// </summary>
        private const string QskDatabase = "database"; // query string key

        /*
        /// <summary>
        /// The password for db.
        /// </summary>
        private string PasswordForDB;
*/

        /// <summary>
        ///   arraylist of InstallerMessages. We construct this on every page request to only keep track of the errors
        ///   that have occurred during this web request. We don't store it in viewstate because we only want the errors
        ///   that have happened on each page request
        /// </summary>
        private ArrayList messages;

        #endregion

        /*
        /// <summary>
        ///   flag indicating that the web.config file was successfully updated. This only works if you have write access
        ///   to your virtual directory.
        /// </summary>
        private bool updatedConfigFile;
*/

        /* TODO: protected string AdminPassword : randomly created admin password 
            CreateKey(8);

        */
        #region Enums

        /// <summary>
        /// The wizard panel.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public enum WizardPanel
        {
            /// <summary>
            ///   The pre install.
            /// </summary>
            PreInstall, 

            /// <summary>
            ///   The license.
            /// </summary>
            License, 

            /// <summary>
            ///   The connect to db.
            /// </summary>
            ConnectToDb, 

            /// <summary>
            ///   The select db.
            /// </summary>
            SelectDb, 

            /// <summary>
            ///   The site information.
            /// </summary>
            SiteInformation, 

            /// <summary>
            ///   The install.
            /// </summary>
            Install, 

            /// <summary>
            ///   The done.
            /// </summary>
            Done, 

            /// <summary>
            ///   The errors.
            /// </summary>
            Errors, 
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the current wizard panel.
        /// </summary>
        /// <value>The current wizard panel.</value>
        /// <remarks>
        /// </remarks>
        public WizardPanel CurrentWizardPanel
        {
            get
            {
                if (this.ViewState["WizardPanel"] != null)
                {
                    return (WizardPanel)this.ViewState["WizardPanel"];
                }

                return WizardPanel.PreInstall;
            }

            set
            {
                this.ViewState["WizardPanel"] = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Nexts the panel.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        public void NextPanel(object sender, EventArgs e)
        {
            string errorMessage;

            switch (this.CurrentWizardPanel)
            {
                case WizardPanel.PreInstall:
                    this.SetActivePanel(WizardPanel.License, this.License);
                    break;

                case WizardPanel.License:
                    if (this.chkIAgree.Checked)
                    {
                        this.SetActivePanel(WizardPanel.ConnectToDb, this.ConnectToDb);
                        this.errIAgree.Visible = false;
                    }
                    else
                    {
                        this.errIAgree.Visible = true;
                    }

                    break;

                case WizardPanel.ConnectToDb:
                    if (this.Validate_ConnectToDb(out errorMessage))
                    {
                        if (this.Validate_SelectDb_ListDatabases(out errorMessage))
                        {
                            if (this.Request.QueryString[QskDatabase] != null &&
                                this.Request.QueryString[QskDatabase] != String.Empty)
                            {
                                try
                                {
                                    this.db_name_list.SelectedValue =
                                        HttpUtility.UrlDecode(this.Request.QueryString[QskDatabase]);

                                    this.SetActivePanel(WizardPanel.SiteInformation, this.SiteInformation);
                                }
                                catch
                                {
                                    // an error occurred setting the database, lets let the user select the database
                                    this.SetActivePanel(WizardPanel.SelectDb, this.SelectDb);
                                }
                            }
                            else
                            {
                                this.SetActivePanel(WizardPanel.SelectDb, this.SelectDb);
                            }
                        }
                        else
                        {
                            this.lblErrMsgConnect.Text = errorMessage;
                        }
                    }
                    else
                    {
                        this.lblErrMsgConnect.Text = errorMessage;
                    }

                    break;

                case WizardPanel.SelectDb:
                    if (this.Validate_SelectDb(out errorMessage))
                    {
                        this.SetActivePanel(WizardPanel.SiteInformation, this.SiteInformation);
                    }
                    else
                    {
                        this.lblErrMsg.Text = errorMessage;
                    }

                    break;

                case WizardPanel.SiteInformation:

                    if (this.CheckSiteInfoValid())
                    {
                        this.PortalPrefixText = this.rb_portalprefix.Text;
                        this.SmtpServerText = this.rb_smtpserver.Text;
                        this.EmailFromText = this.rb_emailfrom.Text;

                        this.SetActivePanel(WizardPanel.Install, this.Install);
                    }

                    break;
                case WizardPanel.Install:
                    if (this.InstallConfig())
                    {
                        this.SetActivePanel(WizardPanel.Done, this.Done);
                    }
                    else
                    {
                        this.lstMessages.DataSource = this.messages;
                        this.lstMessages.DataBind();

                        this.SetActivePanel(WizardPanel.Errors, this.Errors);
                    }

                    break;

                case WizardPanel.Done:
                    Thread.Sleep(3000);
                    break;
            }
        }

        /// <summary>
        /// Previouses the panel.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        public void PreviousPanel(object sender, EventArgs e)
        {
            switch (this.CurrentWizardPanel)
            {
                case WizardPanel.PreInstall:
                    break;

                case WizardPanel.License:
                    this.SetActivePanel(WizardPanel.PreInstall, this.PreInstall);
                    break;

                case WizardPanel.ConnectToDb:
                    this.SetActivePanel(WizardPanel.License, this.License);
                    break;

                case WizardPanel.SelectDb:
                    this.SetActivePanel(WizardPanel.ConnectToDb, this.ConnectToDb);
                    break;

                case WizardPanel.SiteInformation:
                    if (this.Page.Request.QueryString[QskDatabase] != null &&
                        this.Page.Request.QueryString[QskDatabase] != String.Empty)
                    {
                        this.SetActivePanel(WizardPanel.ConnectToDb, this.ConnectToDb);
                    }
                    else
                    {
                        this.SetActivePanel(WizardPanel.SelectDb, this.SelectDb);
                    }

                    break;
                case WizardPanel.Install:
                    this.SetActivePanel(WizardPanel.SiteInformation, this.SiteInformation);
                    break;
                case WizardPanel.Done:
                    this.SetActivePanel(WizardPanel.Install, this.Install);
                    break;
            }
        }

        /// <summary>
        /// Reports the exception.
        /// </summary>
        /// <param name="module">
        /// The module.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <remarks>
        /// </remarks>
        public void ReportException(string module, Exception e)
        {
            ReportException(module, e.Message);
        }

        /// <summary>
        /// Reports the exception.
        /// </summary>
        /// <param name="module">
        /// The module.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <remarks>
        /// </remarks>
        public void ReportException(string module, string message)
        {
            this.messages.Add(new InstallerMessage(module, message));
        }

        /// <summary>
        /// Steps the class.
        /// </summary>
        /// <param name="panelName">
        /// Name of the panel.
        /// </param>
        /// <returns>
        /// The step class.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public string StepClass(WizardPanel panelName)
        {
            var returnValue = this.CurrentWizardPanel != panelName ? "stepnotselected" : "stepselected";

            return returnValue;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on load.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // We use the installer enabled flag to prevent someone from accidentally running the web installer, or
            // someone trying to maliciously trying to run the installer 

            if (InstallerEnabled)
            {
                var cs = ConfigurationManager.ConnectionStrings["ConnectionString"];
                if (cs != null)
                {
                    //If connectionstring, redirect to home
                    if (!String.IsNullOrEmpty(cs.ConnectionString) && cs.ConnectionString != "foo")
                        Response.Redirect("~");
                }


                this.messages = new ArrayList();

                if (!this.Page.IsPostBack)
                {
                    this.SetActivePanel(WizardPanel.PreInstall, this.PreInstall);
                    this.CheckEnvironment();
                }
            }
            else
            {
                // TODO: make this error display on a nice panel
                this.Response.Write("<h1>Appleseed Installation Wizard is disabled.</h1>");
                this.Response.Flush();
                this.Response.End();
            }

            Next.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(Next, null) + ";");
        }

        /// <summary>
        /// Updates the web config.
        /// </summary>
        /// <returns>
        /// The update web config.
        /// </returns>
        /// <remarks>
        /// </remarks>
        protected bool UpdateWebConfig()
        {
            var returnValue = false;
            try
            {
                var doc = new XmlDocument();

                doc.PreserveWhitespace = true;

                var configFile = HttpContext.Current.Server.MapPath("~/web.config");

                doc.Load(configFile);
                var dirty = false;

                // for Appleseed 2.0
                var ns = new XmlNamespaceManager(doc.NameTable);
                ns.AddNamespace("x", "http://schemas.microsoft.com/.NetConfiguration/v2.0");

                var connectionStrings = doc.SelectSingleNode("/x:configuration/x:connectionStrings", ns);
                if (connectionStrings != null)
                {
                    foreach (XmlNode connString in connectionStrings)
                    {
                        if (connString.Name != "add")
                        {
                            continue;
                        }

                        var attrName = connString.Attributes["name"];
                        if (attrName == null)
                        {
                            continue;
                        }

                        switch (attrName.Value)
                        {
                            case "ConnectionString":
                                {
                                    var attrCSTRValue = connString.Attributes["connectionString"];
                                    if (attrCSTRValue != null)
                                    {
                                        attrCSTRValue.Value = this.GetDatabaseConnectionString();
                                        dirty = true;
                                    }
                                }

                                break;
                            case "Providers.ConnectionString":
                                {
                                    var attrPCSTRValue = connString.Attributes["connectionString"];
                                    if (attrPCSTRValue != null)
                                    {
                                        attrPCSTRValue.Value = this.GetDatabaseConnectionString();
                                        dirty = true;
                                    }
                                }

                                break;
                            case "AppleseedProviders.ConnectionString":
                                {
                                    var attrRPCSTRValue = connString.Attributes["connectionString"];
                                    if (attrRPCSTRValue != null)
                                    {
                                        attrRPCSTRValue.Value = this.GetDatabaseConnectionString();
                                        dirty = true;
                                    }
                                }

                                break;
                            case "Main.ConnectionString":
                                {
                                    var attrMCSTRValue = connString.Attributes["connectionString"];
                                    if (attrMCSTRValue != null)
                                    {
                                        attrMCSTRValue.Value = this.GetDatabaseConnectionString();
                                        dirty = true;
                                    }
                                }

                                break;
                            case "AppleseedDBContext":
                                {
                                    var attrMCSTRValue = connString.Attributes["connectionString"];
                                    if (attrMCSTRValue != null)
                                    {
                                        attrMCSTRValue.Value = this.GetEntityModelConnectionString();
                                        dirty = true;
                                    }

                                    var attrPVValue = connString.Attributes["providerName"];
                                    if (attrPVValue != null)
                                    {
                                        attrPVValue.Value = "System.Data.EntityClient";
                                        dirty = true;
                                    }
                                }

                                break;
                            case "AppleseedMembershipEntities":
                                {
                                    var attrMCSTRValue = connString.Attributes["connectionString"];
                                    if (attrMCSTRValue != null)
                                    {
                                        attrMCSTRValue.Value = this.GetMembershipModelConnectionString();
                                        dirty = true;
                                    }

                                    var attrPVValue = connString.Attributes["providerName"];
                                    if (attrPVValue != null)
                                    {
                                        attrPVValue.Value = "System.Data.EntityClient";
                                        dirty = true;
                                    }
                                }

                                break;
                            case "SelfUpdaterEntities": {
                                    var attrMCSTRValue = connString.Attributes["connectionString"];
                                    if (attrMCSTRValue != null) {
                                        attrMCSTRValue.Value = this.GetEntityModelConnectionString().Replace("AppleseedModel", "SelfUpdater");
                                        dirty = true;
                                    }

                                    var attrPVValue = connString.Attributes["providerName"];
                                    if (attrPVValue != null) {
                                        attrPVValue.Value = "System.Data.EntityClient";
                                        dirty = true;
                                    }
                                }

                                break;
                        }
                    }
                }

                var appSettings = doc.SelectSingleNode("/x:configuration/x:appSettings", ns);
                if (appSettings != null)
                {
                    foreach (XmlNode setting in appSettings)
                    {
                        if (setting.Name != "add")
                        {
                            continue;
                        }

                        var attrKey = setting.Attributes["key"];
                        if (attrKey == null)
                        {
                            continue;
                        }

                        switch (attrKey.Value)
                        {
                            case "SmtpServer":
                                {
                                    var attrSMTPValue = setting.Attributes["value"];
                                    if (attrSMTPValue != null)
                                    {
                                        attrSMTPValue.Value = this.SmtpServerText;
                                        dirty = true;
                                    }
                                }

                                break;
                            case "EmailFrom":
                                {
                                    var attrEFROMValue = setting.Attributes["value"];
                                    if (attrEFROMValue != null)
                                    {
                                        attrEFROMValue.Value = this.EmailFromText;
                                        dirty = true;
                                    }
                                }

                                break;
                            case "PortalTitlePrefix":
                                {
                                    var attrPREFIXValue = setting.Attributes["value"];
                                    if (attrPREFIXValue != null)
                                    {
                                        attrPREFIXValue.Value = this.PortalPrefixText;
                                        dirty = true;
                                    }
                                }

                                break;
                        }
                    }
                }

                if (dirty)
                {
                    // Save the document to a file and auto-indent the output.
                    var writer = new XmlTextWriter(configFile, Encoding.UTF8) { Formatting = Formatting.Indented };
                    doc.Save(writer);
                    writer.Flush();
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                ReportException("UpdateWebConfig", e);
            }

            return returnValue;
        }

        /// <summary>
        /// Ifs the directory writable.
        /// </summary>
        /// <param name="directoryName">
        /// Name of the directory.
        /// </param>
        /// <returns>
        /// The if directory writable.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private static string IfDirectoryWritable(string directoryName)
        {
            string returnInfo;
            const string FileName = "TempFile.txt";

            var directoryNameInfo = new DirectoryInfo(directoryName);
            if (directoryNameInfo.Exists)
            {
                returnInfo = "<span style='color:green;' >Exists</span>";

                try
                {
                    var sw = File.AppendText(directoryName + "\\" + FileName);
                    sw.Write("-");
                    sw.Close();
                    File.Delete(directoryName + "\\" + FileName);
                    returnInfo += ",<span style='color:green;' >Writable</span>";
                }
                catch (Exception e)
                {
                    returnInfo += string.Format(",<span style='color:red;' >Un-Writable: {0}</span>", e.Message);
                }
            }
            else
            {
                returnInfo = "Directory Doesn't Exist";
            }

            return returnInfo;
        }

        /// <summary>
        /// Ifs the file writable.
        /// </summary>
        /// <param name="fileName">
        /// Name of the file.
        /// </param>
        /// <returns>
        /// The if file writable.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private static string IfFileWritable(string fileName)
        {
            string returnInfo;

            var fileNameInfo = new FileInfo(fileName);

            if (fileNameInfo.Exists)
            {
                returnInfo = "<span style='color:green;' >Exists</span>";

                try
                {
                    var sw = File.AppendText(fileName);
                    sw.Write(" ");
                    sw.Close();
                    returnInfo += ",<span style='color:green;' >Writable</span>";
                }
                catch (Exception e)
                {
                    returnInfo += ",<span style='color:red;' >Un-Writable: " + e.Message + "</span>";
                }
            }
            else
            {
                returnInfo = "File Doesn't Exist";
            }

            return returnInfo;
        }

        /// <summary>
        /// Checks the environment.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private void CheckEnvironment()
        {
            var configFile = HttpContext.Current.Server.MapPath("~/web.config");
            var logsDir = HttpContext.Current.Server.MapPath("~/rb_Logs");
            var portalsDir = HttpContext.Current.Server.MapPath("~/Portals");

            this.lblAspNetVersion.Text = string.Format("<span style=\"color:green;\">{0}</span>", Environment.Version);
            this.lblWebConfigWritable.Text = IfFileWritable(configFile);
            this.lblLogsDirWritable.Text = IfDirectoryWritable(logsDir);
            this.lblPortalsDirWritable.Text = IfDirectoryWritable(portalsDir);
        }

        /// <summary>
        /// Checks the site info valid.
        /// </summary>
        /// <returns>
        /// The check site info valid.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private bool CheckSiteInfoValid()
        {
            if (this.rb_smtpserver.Text.Trim().Length == 0)
            {
                this.req_rb_smtpserver.IsValid = false;
                return false;
            }

            if (this.rb_portalprefix.Text.Trim().Length == 0)
            {
                this.req_rb_portalprefix.IsValid = false;
                return false;
            }

            if (this.rb_emailfrom.Text.Trim().Length == 0)
            {
                this.req_rb_emailfrom.IsValid = false;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns>
        /// The get connection string.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private string GetConnectionString()
        {
            return String.Format(
                "server={0};uid={1};pwd={2};Trusted_Connection={3}", 
                this.db_server.Text, 
                this.db_login.Text, 
                this.db_password.Text, 
                this.db_Connect.SelectedIndex == 0 ? "yes" : "no");
        }

        /// <summary>
        /// Gets the database connection string.
        /// </summary>
        /// <returns>
        /// The get database connection string.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private string GetDatabaseConnectionString()
        {
            return String.Format("{0};database={1}", this.GetConnectionString(), this.db_name_list.SelectedValue);
        }

        /// <summary>
        /// Gets the entity model connection string.
        /// </summary>
        /// <returns>
        /// The get entity model connection string.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private string GetEntityModelConnectionString()
        {
            return
                string.Format(
                    "metadata=res://*/Models.AppleseedModel.csdl|res://*/Models.AppleseedModel.ssdl|res://*/Models.AppleseedModel.msl;provider=System.Data.SqlClient;provider connection string=\"Data Source={0};Initial Catalog={1};User ID={2};pwd={3};Trusted_Connection={4};MultipleActiveResultSets=True\"", 
                    this.db_server.Text, 
                    this.db_name_list.SelectedValue, 
                    this.db_login.Text, 
                    this.db_password.Text, 
                    this.db_Connect.SelectedIndex == 0 ? "yes" : "no");
        }

        /// <summary>
        /// Gets the membership model conection string.
        /// </summary>
        /// <returns>
        /// The get membership model conection string.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private string GetMembershipModelConnectionString()
        {
            return
                string.Format(
                    "metadata=res://*/AppleseedMembershipModel.csdl|res://*/AppleseedMembershipModel.ssdl|res://*/AppleseedMembershipModel.msl;provider=System.Data.SqlClient;provider connection string=\"Data Source={0};Initial Catalog={1};User ID={2};pwd={3};Trusted_Connection={4};MultipleActiveResultSets=True\"", 
                    this.db_server.Text, 
                    this.db_name_list.SelectedValue, 
                    this.db_login.Text, 
                    this.db_password.Text, 
                    this.db_Connect.SelectedIndex == 0 ? "yes" : "no");
        }

        /*
        /// <summary>
        /// Hides all panels.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private void HideAllPanels()
        {
            this.PreInstall.Visible = false;
            this.License.Visible = false;
            this.ConnectToDb.Visible = false;
            this.SiteInformation.Visible = false;
            this.Install.Visible = false;
            this.Done.Visible = false;
            this.Errors.Visible = false;
        }
*/

        /// <summary>
        /// Installs the config.
        /// </summary>
        /// <returns>
        /// The install config.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private bool InstallConfig()
        {
            using (var s = new Services())
            {
                if (s.RunDBUpdate(this.GetDatabaseConnectionString()))
                {
                    this.UpdateWebConfig();
                }
            }

            return true;
        }

        /// <summary>
        /// Sets the active panel.
        /// </summary>
        /// <param name="panel">
        /// The panel.
        /// </param>
        /// <param name="controlToShow">
        /// The control to show.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void SetActivePanel(WizardPanel panel, Control controlToShow)
        {
            var currentPanel = this.FindControl(this.CurrentWizardPanel.ToString()) as Panel;
            if (currentPanel != null)
            {
                currentPanel.Visible = false;
            }

            switch (panel)
            {
                case WizardPanel.PreInstall:
                    this.Previous.Enabled = false;
                    this.License.Visible = false;
                    break;
                case WizardPanel.Done:
                    this.Next.Enabled = false;
                    this.Previous.Enabled = false;
                    break;
                case WizardPanel.Errors:
                    this.Previous.Enabled = false;
                    this.Next.Enabled = false;
                    break;
                default:
                    this.Previous.Enabled = true;
                    this.Next.Enabled = true;
                    break;
            }

            controlToShow.Visible = true;
            this.CurrentWizardPanel = panel;
        }

        /// <summary>
        /// Validate_s the connect to db.
        /// </summary>
        /// <param name="errorMessage">
        /// The error message.
        /// </param>
        /// <returns>
        /// The validate_ connect to db.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private bool Validate_ConnectToDb(out string errorMessage)
        {
            // 	ConnectionString = "server=" + db_server.Text + ";uid="+ db_login.Text +";pwd=" + db_password.Text + ";Trusted_Connection=" + (db_Connect.SelectedIndex == 0 ? "yes" : "no");
            try
            {
                var connection = new SqlConnection(this.GetConnectionString());
                connection.Open();
                connection.Close();
                errorMessage = string.Empty;
                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }
        }

        /// <summary>
        /// Validate_s the select db.
        /// </summary>
        /// <param name="errorMessage">
        /// The error message.
        /// </param>
        /// <returns>
        /// The validate_ select db.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private bool Validate_SelectDb(out string errorMessage)
        {
            try
            {
                using (var connection = new SqlConnection(this.GetDatabaseConnectionString()))
                {
                    connection.Open();
                    connection.Close();
                }

                errorMessage = string.Empty;

                return true;
            }
            catch (SqlException se)
            {
                switch (se.Number)
                {
                    case 4060: // login fails
                        if (this.db_Connect.SelectedIndex == 0)
                        {
                            errorMessage =
                                "The installer is unable to access the specified database using the Windows credentials that the web server is running under. Contact your system administrator to have them add	" +
                                Environment.UserName + " to the list of authorized logins";
                        }
                        else
                        {
                            errorMessage = "You can't login to that database. Please select another one<br />" +
                                           se.Message;
                        }

                        break;
                    default:
                        errorMessage = String.Format("Number:{0}:<br/>Message:{1}", se.Number, se.Message) + "<br/>" +
                                       this.GetConnectionString();
                        break;
                }

                return false;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }
        }

        /// <summary>
        /// Validate_s the select DB_ list databases.
        /// </summary>
        /// <param name="errorMessage">
        /// The error message.
        /// </param>
        /// <returns>
        /// The validate_ select db_ list databases.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private bool Validate_SelectDb_ListDatabases(out string errorMessage)
        {
            try
            {
                var connection = new SqlConnection(this.GetConnectionString());
                SqlDataReader dr;
                var command = new SqlCommand("select name from master..sysdatabases order by name asc", connection);

                connection.Open();

                // Change to the master database
                connection.ChangeDatabase("master");

                dr = command.ExecuteReader();

                this.db_name_list.Items.Clear();

                while (dr.Read())
                {
                    var dbName = dr["name"] as string;
                    if (dbName == null)
                    {
                        continue;
                    }

                    if (dbName == "master" || dbName == "msdb" || dbName == "tempdb" || dbName == "model")
                    {
                        // skip the system databases
                        continue;
                    }

                    this.db_name_list.Items.Add(dbName);
                }

                connection.Close();

                errorMessage = string.Empty;

                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }
        }

        #endregion

        /// <summary>
        /// Class to encapsulate the module (method) along with the error message that occurred within the module(method)
        /// </summary>
        /// <remarks>
        /// </remarks>
        public class InstallerMessage
        {
            #region Constants and Fields

            /// <summary>
            ///   The message.
            /// </summary>
            public string Message;

            /// <summary>
            ///   The module.
            /// </summary>
            public string Module;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="InstallerMessage"/> class.
            /// </summary>
            /// <param name="module">
            /// The module.
            /// </param>
            /// <param name="message">
            /// The message.
            /// </param>
            /// <remarks>
            /// </remarks>
            public InstallerMessage(string module, string message)
            {
                this.Module = module;
                this.Message = message;
            }

            #endregion
        }
    }
}