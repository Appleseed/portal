// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortalSettings.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   PortalSettings Class encapsulates all of the settings
//   for the Portal, as well as the configuration settings required
//   to execute the current tab view within the portal.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Configuration;

namespace Appleseed.Framework.Site.Configuration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Security;
    using System.Web.UI.WebControls;
    using System.Xml;

    using Appleseed.Framework.Configuration.Settings;
    using Appleseed.Framework.DataTypes;
    using Appleseed.Framework.Design;
    using Appleseed.Framework.Exceptions;
    using Appleseed.Framework.Scheduler;
    using Appleseed.Framework.Security;
    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Settings.Cache;
    using Appleseed.Framework.Site.Data;
    using Appleseed.Framework.Web.UI.WebControls;

    using Path = Appleseed.Framework.Settings.Path;

    /// <summary>
    /// PortalSettings Class encapsulates all of the settings
    ///   for the Portal, as well as the configuration settings required
    ///   to execute the current tab view within the portal.
    /// </summary>
    /// <remarks>
    /// </remarks>
    [History("Ashish.patel@haptix.biz", "2014/12/24", "Added Check/method for Enable Friendly url")]
    [History("jminond", "2005/03/10", "Tab to page conversion")]
    [History("gman3001", "2004/09/29",
        "Added the GetCurrentUserProfile method to obtain a hashtable of the current user's profile details.")]
    [History("jviladiu@portalServices.net", "2004/08/19", "Add support for move & delete module roles")]
    [History("jviladiu@portalServices.net", "2004/07/30", "Added new ActiveModule property")]
    [History("Jes1111", "2003/03/09", "Added new ShowTabs property")]
    [History("Jes1111", "2003/04/02", "Added new DesktopTabsXml property (an XPathDocument)")]
    [History("Thierry", "2003/04/12", "Added PortalSecurePath property")]
    [History("Jes1111", "2003/04/17", "Added new language-related properties and methods")]
    [History("Jes1111", "2003/04/23", "Corrected string comparison case problem in language settings")]
    [History("cisakson@yahoo.com", "2003/04/28", "Added a custom setting for Windows users to assign a portal Admin")]
    public class PortalSettings : ISettingHolder
    {
        #region Constants and Fields

        /// <summary>
        ///   The strings at page id.
        /// </summary>
        private const string StringsAtPageId = "@PageID";

        /// <summary>
        ///   The strings at portal id.
        /// </summary>
        private const string StringsAtPortalId = "@PortalID";

        /// <summary>
        ///   The strings custom layout.
        /// </summary>
        private const string StringsCustomLayout = "CustomLayout";

        /// <summary>
        ///   The strings custom theme.
        /// </summary>
        private const string StringsCustomTheme = "CustomTheme";

        /// <summary>
        ///   The strings name.
        /// </summary>
        private const string StringsName = "Name";

        /// <summary>
        ///   The portal path prefix.
        /// </summary>
        private readonly string portalPathPrefix = HttpContext.Current.Request.ApplicationPath == "/"
                                                       ? string.Empty
                                                       : HttpContext.Current.Request.ApplicationPath;

        /// <summary>
        ///   The appleseed cultures.
        /// </summary>
        private static CultureInfo[] appleseedCultures;

        /// <summary>
        ///   The current layout.
        /// </summary>
        private string currentLayout;

        // private XPathDocument _desktopPagesXml;

        /// <summary>
        ///   The portal pages xml.
        /// </summary>
        private XmlDocument portalPagesXml;

        /// <summary>
        ///   The portal path.
        /// </summary>
        private string portalPath = string.Empty;

        /// <summary>
        ///   The portal secure path.
        /// </summary>
        private string portalSecurePath;

        #endregion

        #region Constructors and Destructors

        private PortalSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortalSettings"/> class.
        ///   The PortalSettings Constructor encapsulates all of the logic
        ///   necessary to obtain configuration settings necessary to render
        ///   a Portal Page view for a given request.<br/>
        ///   These Portal Settings are stored within a SQL database, and are
        ///   fetched below by calling the "GetPortalSettings" stored procedure.<br/>
        ///   This stored procedure returns values as SPROC output parameters,
        ///   and using three result sets.
        /// </summary>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        /// <param name="portalAlias">
        /// The portal alias.
        /// </param>
        /// <remarks>
        /// </remarks>
        private PortalSettings(int pageId, string portalAlias)
        {
            this.ActivePage = new PageSettings();
            this.DesktopPages = new List<PageStripDetails>();
            this.ShowPages = true;
            this.MobilePages = new ArrayList();

            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_GetPortalSettings", connection))
            {
                // Mark the Command as a SPROC
                command.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterPortalAlias = new SqlParameter("@PortalAlias", SqlDbType.NVarChar, 128)
                {
                    Value = portalAlias // Specify the Portal Alias Dynamically 
                };
                command.Parameters.Add(parameterPortalAlias);
                var parameterPageId = new SqlParameter(StringsAtPageId, SqlDbType.Int, 4) { Value = pageId };
                command.Parameters.Add(parameterPageId);
                var parameterPortalLanguage = new SqlParameter("@PortalLanguage", SqlDbType.NVarChar, 12)
                {
                    Value = this.PortalContentLanguage.Name
                };
                command.Parameters.Add(parameterPortalLanguage);

                // Add out parameters to Sproc
                var parameterPortalId = new SqlParameter(StringsAtPortalId, SqlDbType.Int, 4)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(parameterPortalId);
                var parameterPortalName = new SqlParameter("@PortalName", SqlDbType.NVarChar, 128)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(parameterPortalName);
                var parameterPortalPath = new SqlParameter("@PortalPath", SqlDbType.NVarChar, 128)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(parameterPortalPath);
                var parameterEditButton = new SqlParameter("@AlwaysShowEditButton", SqlDbType.Bit, 1)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(parameterEditButton);
                var parameterPageName = new SqlParameter("@PageName", SqlDbType.NVarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(parameterPageName);
                var parameterPageOrder = new SqlParameter("@PageOrder", SqlDbType.Int, 4)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(parameterPageOrder);
                var parameterParentPageId = new SqlParameter("@ParentPageID", SqlDbType.Int, 4)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(parameterParentPageId);
                var parameterMobilePageName = new SqlParameter("@MobilePageName", SqlDbType.NVarChar, 200)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(parameterMobilePageName);
                var parameterAuthRoles = new SqlParameter("@AuthRoles", SqlDbType.NVarChar, 512)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(parameterAuthRoles);
                var parameterShowMobile = new SqlParameter("@ShowMobile", SqlDbType.Bit, 1)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(parameterShowMobile);
                var parameterFriendURL = new SqlParameter("@FriendURL", SqlDbType.NVarChar, 1024)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(parameterFriendURL);
                SqlDataReader result;

                try
                {
                    // Open the database connection and execute the command
                    // try // jes1111
                    // {
                    connection.Open();
                    result = command.ExecuteReader(CommandBehavior.CloseConnection);
                    this.CurrentLayout = "Default";

                    // Read the first resultset -- Desktop Page Information
                    while (result.Read())
                    {
                        var tabDetails = new PageStripDetails
                        {
                            PageID = (int)result["PageID"],
                            ParentPageID = Int32.Parse("0" + result["ParentPageID"]),
                            PageName = (string)result["PageName"],
                            FriendlyURL = (string)result["FriendlyURL"],
                            PageOrder = (int)result["PageOrder"],
                            PageLayout = this.CurrentLayout,
                            AuthorizedRoles = (string)result["AuthorizedRoles"]
                        };
                        this.PortalAlias = portalAlias;

                        // Update the AuthorizedRoles Variable
                        this.DesktopPages.Add(tabDetails);
                    }

                    if (this.DesktopPages.Count == 0)
                    {
                        return; // Abort load

                        // throw new Exception("The portal you requested has no Pages. PortalAlias: '" + portalAlias + "'", new HttpException(404, "Portal not found"));
                    }

                    // Read the second result --  Mobile Page Information
                    result.NextResult();

                    while (result.Read())
                    {
                        var tabDetails = new PageStripDetails
                        {
                            PageID = (int)result["PageID"],
                            PageName = (string)result["MobilePageName"],
                            FriendlyURL = (string)result["FriendlyURL"],
                            PageLayout = this.CurrentLayout,
                            AuthorizedRoles = (string)result["AuthorizedRoles"]
                        };
                        this.MobilePages.Add(tabDetails);
                    }

                    // Read the third result --  Module Page Information
                    result.NextResult();

                    while (result.Read())
                    {
                        var m = new ModuleSettings
                        {
                            ModuleID = (int)result["ModuleID"],
                            ModuleDefID = (int)result["ModuleDefID"],
                            GuidID = (Guid)result["GeneralModDefID"],
                            PageID = (int)result["TabID"],
                            PaneName = (string)result["PaneName"],
                            ModuleTitle = (string)result["ModuleTitle"]
                        };
                        var value = result["AuthorizedEditRoles"];
                        m.AuthorizedEditRoles = !Convert.IsDBNull(value) ? (string)value : string.Empty;
                        value = result["AuthorizedViewRoles"];
                        m.AuthorizedViewRoles = !Convert.IsDBNull(value) ? (string)value : string.Empty;
                        value = result["AuthorizedAddRoles"];
                        m.AuthorizedAddRoles = !Convert.IsDBNull(value) ? (string)value : string.Empty;
                        value = result["AuthorizedDeleteRoles"];
                        m.AuthorizedDeleteRoles = !Convert.IsDBNull(value) ? (string)value : string.Empty;
                        value = result["AuthorizedPropertiesRoles"];
                        m.AuthorizedPropertiesRoles = !Convert.IsDBNull(value) ? (string)value : string.Empty;

                        // jviladiu@portalServices.net (19/08/2004) Add support for move & delete module roles
                        value = result["AuthorizedMoveModuleRoles"];
                        m.AuthorizedMoveModuleRoles = !Convert.IsDBNull(value) ? (string)value : string.Empty;
                        value = result["AuthorizedDeleteModuleRoles"];
                        m.AuthorizedDeleteModuleRoles = !Convert.IsDBNull(value) ? (string)value : string.Empty;

                        // Change by Geert.Audenaert@Syntegra.Com
                        // Date: 6/2/2003
                        value = result["AuthorizedPublishingRoles"];
                        m.AuthorizedPublishingRoles = !Convert.IsDBNull(value) ? (string)value : string.Empty;
                        value = result["SupportWorkflow"];
                        m.SupportWorkflow = !Convert.IsDBNull(value) ? (bool)value : false;

                        // Date: 27/2/2003
                        value = result["AuthorizedApproveRoles"];
                        m.AuthorizedApproveRoles = !Convert.IsDBNull(value) ? (string)value : string.Empty;
                        value = result["WorkflowState"];
                        m.WorkflowStatus = !Convert.IsDBNull(value)
                                               ? (WorkflowState)(0 + (byte)value)
                                               : WorkflowState.Original;

                        // End Change Geert.Audenaert@Syntegra.Com
                        // Start Change bja@reedtek.com
                        try
                        {
                            value = result["SupportCollapsable"];
                        }
                        catch
                        {
                            value = DBNull.Value;
                        }

                        m.SupportCollapsable = DBNull.Value != value ? (bool)value : false;

                        // End Change  bja@reedtek.com
                        // Start Change john.mandia@whitelightsolutions.com
                        try
                        {
                            value = result["ShowEveryWhere"];
                        }
                        catch
                        {
                            value = DBNull.Value;
                        }

                        m.ShowEveryWhere = DBNull.Value != value ? (bool)value : false;

                        // End Change  john.mandia@whitelightsolutions.com
                        m.CacheTime = int.Parse(result["CacheTime"].ToString());
                        m.ModuleOrder = int.Parse(result["ModuleOrder"].ToString());
                        value = result["ShowMobile"];
                        m.ShowMobile = !Convert.IsDBNull(value) ? (bool)value : false;
                        m.DesktopSrc = result["DesktopSrc"].ToString();
                        m.MobileSrc = result["MobileSrc"].ToString();
                        m.Admin = bool.Parse(result["Admin"].ToString());
                        this.ActivePage.Modules.Add(m);
                    }

                    // Now read Portal out params 
                    result.NextResult();
                    this.PortalID = (int)parameterPortalId.Value;
                    this.PortalName = (string)parameterPortalName.Value;

                    // jes1111 - this.PortalTitle = ConfigurationSettings.AppSettings["PortalTitlePrefix"] + this.PortalName;
                    this.PortalTitle = String.Concat(Config.PortalTitlePrefix, this.PortalName);

                    // jes1111 - this.PortalPath = Settings.Path.WebPathCombine(ConfigurationSettings.AppSettings["PortalsDirectory"], (string) parameterPortalPath.Value);
                    this.PortalPath = Path.WebPathCombine(Config.PortalsDirectory, (string)parameterPortalPath.Value);

                    // jes1111 - this.PortalSecurePath = ConfigurationSettings.AppSettings["PortalSecureDirectory"]; // added Thierry (tiptopweb) 12 Apr 2003
                    this.PortalSecurePath = Config.PortalSecureDirectory;
                    this.ActivePage.PageID = pageId;
                    this.ActivePage.PageLayout = this.CurrentLayout;
                    this.ActivePage.ParentPageID = Int32.Parse("0" + parameterParentPageId.Value);
                    this.ActivePage.PageOrder = (int)parameterPageOrder.Value;
                    this.ActivePage.MobilePageName = (string)parameterMobilePageName.Value;
                    this.ActivePage.AuthorizedRoles = (string)parameterAuthRoles.Value;
                    this.ActivePage.PageName = (string)parameterPageName.Value;
                    this.ActivePage.FriendlyURL = (string)parameterFriendURL.Value;
                    this.ActivePage.ShowMobile = (bool)parameterShowMobile.Value;
                    this.ActivePage.PortalPath = this.PortalPath; // thierry@tiptopweb.com.au for page custom layout
                    result.Close(); // by Manu, fixed bug 807858

                    // }
                    // catch (Exception ex)
                    // {
                    // HttpContext.Current.Response.Write("Failed rb_GetPortalSettings for " + pageID.ToString() + ", " + portalAlias + ":<br/>"+ex.Message);
                    // HttpContext.Current.Response.End();
                    // //Response.Redirect("~/app_support/ErrorNoPortal.aspx",true);
                    // }
                }
                catch (SqlException sqex)
                {
                    var requestUri = HttpContext.Current.Request.Url;

                    throw new DatabaseUnreachableException("This may be a new db", sqex);

                    // return;
                }
                finally
                {
                }
            }

            // Provide a valid tab id if it is missing

            // 11-10-2011
            // Changed to go to the first page that the user logged has permission to see

            //if (this.ActivePage.PageID == 0)
            //{
            //    this.ActivePage.PageID = ((PageStripDetails)this.DesktopPages[0]).PageID;
            //}

            // Go to get custom settings
            if (!Directory.Exists(Path.ApplicationPhysicalPath + this.PortalFullPath))
            {
                var portals = new PortalsDB();
                portals.CreatePortalPath(this.PortalAlias);
            }

            this.CustomSettings = GetPortalCustomSettings(this.PortalID, GetPortalBaseSettings(this.PortalPath));

            // Initialize Theme
            var themeManager = new ThemeManager(this.PortalPath);

            // Default
            themeManager.Load(this.CustomSettings["SITESETTINGS_THEME"].ToString());
            this.CurrentThemeDefault = themeManager.CurrentTheme;

            // Alternate
            if (this.CustomSettings["SITESETTINGS_ALT_THEME"].ToString() == this.CurrentThemeDefault.Name)
            {
                this.CurrentThemeAlt = this.CurrentThemeDefault;
            }
            else
            {
                themeManager.Load(this.CustomSettings["SITESETTINGS_ALT_THEME"].ToString());
                this.CurrentThemeAlt = themeManager.CurrentTheme;
            }

            // themeManager.Save(this.CustomSettings["SITESETTINGS_THEME"].ToString());
            // Set layout
            this.CurrentLayout = this.CustomSettings["SITESETTINGS_PAGE_LAYOUT"].ToString();

            // Jes1111
            // Generate DesktopPagesXml
            // jes1111 - if (bool.Parse(ConfigurationSettings.AppSettings["PortalSettingDesktopPagesXml"]))
            // if (Config.PortalSettingDesktopPagesXml)
            // this.DesktopPagesXml = GetDesktopPagesXml();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortalSettings"/> class.
        ///   The PortalSettings Constructor encapsulates all of the logic
        ///   necessary to obtain configuration settings necessary to get
        ///   custom setting for a different portal than current (EditPortal.aspx.cs)<br/>
        ///   These Portal Settings are stored within a SQL database, and are
        ///   fetched below by calling the "GetPortalSettings" stored procedure.<br/>
        ///   This overload it is used
        /// </summary>
        /// <param name="portalId">
        /// The portal id.
        /// </param>
        /// <remarks>
        /// </remarks>
        private PortalSettings(int portalId)
        {
            this.ActivePage = new PageSettings();
            this.DesktopPages = new List<PageStripDetails>();
            this.ShowPages = true;
            this.MobilePages = new ArrayList();

            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_GetPortalSettingsPortalID", connection))
            {
                // Mark the Command as a SPROC
                command.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterPortalId = new SqlParameter(StringsAtPortalId, SqlDbType.Int) { Value = portalId };
                command.Parameters.Add(parameterPortalId);

                // Open the database connection and execute the command
                connection.Open();
                var result = command.ExecuteReader(CommandBehavior.CloseConnection); // by Manu CloseConnection

                try
                {
                    if (result.Read())
                    {
                        this.PortalID = Int32.Parse(result["PortalID"].ToString());
                        this.PortalName = result["PortalName"].ToString();
                        this.PortalAlias = result["PortalAlias"].ToString();

                        // jes1111 - this.PortalTitle = ConfigurationSettings.AppSettings["PortalTitlePrefix"] + result["PortalName"].ToString();
                        this.PortalTitle = String.Concat(Config.PortalTitlePrefix, result["PortalName"].ToString());
                        this.PortalPath = result["PortalPath"].ToString();
                        this.ActivePage.PageID = 0;

                        // added Thierry (tiptopweb) used for dropdown for layout and theme
                        this.ActivePage.PortalPath = this.PortalPath;
                        this.ActiveModule = 0;
                    }
                    else
                    {
                        throw new Exception(
                            "The portal you requested cannot be found. PortalID: " + portalId,
                            new HttpException(404, "Portal not found"));
                    }
                }
                finally
                {
                    result.Close(); // by Manu, fixed bug 807858
                }
            }

            // Go to get custom settings
            this.CustomSettings = GetPortalCustomSettings(portalId, GetPortalBaseSettings(this.PortalPath));
            this.CurrentLayout = this.CustomSettings["SITESETTINGS_PAGE_LAYOUT"].ToString();

            // Initialize Theme
            var themeManager = new ThemeManager(this.PortalPath);

            // Default
            themeManager.Load(this.CustomSettings["SITESETTINGS_THEME"].ToString());
            this.CurrentThemeDefault = themeManager.CurrentTheme;

            // Alternate
            themeManager.Load(this.CustomSettings["SITESETTINGS_ALT_THEME"].ToString());
            this.CurrentThemeAlt = themeManager.CurrentTheme;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortalSettings"/> class.
        ///   The PortalSettings Constructor encapsulates all of the logic
        ///   necessary to obtain configuration settings necessary to get
        ///   custom setting for a different portal than current (EditPortal.aspx.cs)<br/>
        ///   These Portal Settings are stored within a SQL database, and are
        ///   fetched below by calling the "GetPortalSettings" stored procedure.<br/>
        ///   This overload it is used
        /// </summary>
        /// <param name="portalId">
        /// The portal id.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static PortalSettings GetPortalSettings(int portalId)
        {

            return new PortalSettings(portalId);

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortalSettings"/> class.
        ///   The PortalSettings Constructor encapsulates all of the logic
        ///   necessary to obtain configuration settings necessary to render
        ///   a Portal Page view for a given request.<br/>
        ///   These Portal Settings are stored within a SQL database, and are
        ///   fetched below by calling the "GetPortalSettings" stored procedure.<br/>
        ///   This stored procedure returns values as SPROC output parameters,
        ///   and using three result sets.
        /// </summary>
        /// <param name="pageId">
        /// The page id.
        /// </param>
        /// <param name="portalAlias">
        /// The portal alias.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static PortalSettings GetPortalSettings(int pageId, string portalAlias)
        {
            ProcessCurrentLanguage(portalAlias);
            var key = GetPortalSettingsCacheKey(pageId, portalAlias, Thread.CurrentThread.CurrentUICulture.Name);
            var cache = HttpRuntime.Cache;
            if (cache.Get(key) != null)
            {
                return (PortalSettings)cache.Get(key);
            }
            var portalSettings = new PortalSettings(pageId, portalAlias);
            AddToCache(key, portalSettings);

            return portalSettings;
        }

        #endregion

        // public bool AlwaysShowEditButton
        // {
        // get { return alwaysShowEditButton; }
        // set { alwaysShowEditButton = value; }
        // }
        #region Properties

        /// <summary>
        /// Gets the name of the AD user.
        /// </summary>
        /// <remarks></remarks>
        [Obsolete("Please use Appleseed.Framework.Settings.Config.ADUserName")]
        public static string ADUserName
        {
            get
            {
                return Config.ADUserName;
            }
        }

        /// <summary>
        /// Gets the AD user password.
        /// </summary>
        /// <remarks></remarks>
        [Obsolete("Please use Appleseed.Framework.Settings.Config.ADUserPassword")]
        public static string ADUserPassword
        {
            get
            {
                return Config.ADUserPassword;
            }
        }

        /// <summary>
        ///   Gets ApplicationPath, Application dependent.
        ///   Used by newsletter. Needed if you want to reference a page
        ///   from an external resource (an email for example)
        ///   Since it is common for all portals is declared as static.
        /// </summary>
        /// <value>The application full path.</value>
        /// <remarks>
        /// </remarks>
        [Obsolete("Please use Appleseed.Framework.Settings.Path.ApplicationFullPath")]
        public static string ApplicationFullPath
        {
            get
            {
                return Path.ApplicationFullPath;
            }
        }

        /// <summary>
        ///   Gets the ApplicationPath, Application dependent relative Application Path.
        ///   Base dir for all portal code
        ///   Since it is common for all portals is declared as static
        /// </summary>
        /// <value>The application path.</value>
        /// <remarks>
        /// </remarks>
        [Obsolete("Please use Appleseed.Framework.Settings.Path.ApplicationRoot")]
        public static string ApplicationPath
        {
            get
            {
                return Path.ApplicationRoot;
            }
        }

        /// <summary>
        ///   Gets the application physical path on the file system.
        /// </summary>
        /// <remarks>
        /// </remarks>
        [Obsolete("Please use Appleseed.Framework.Settings.Path.ApplicationPhysicalPath")]
        public static string ApplicationPhisicalPath
        {
            get
            {
                return Path.ApplicationPhysicalPath;
            }
        }

        /// <summary>
        /// Gets the code version.
        /// </summary>
        /// <remarks></remarks>
        [Obsolete("Please use Appleseed.Framework.Settings.Portal.CodeVersion")]
        public static int CodeVersion
        {
            get
            {
                return Portal.CodeVersion;
            }
        }

        /// <summary>
        ///   Gets or sets the current user.
        /// </summary>
        /// <value>The current user.</value>
        /// <remarks>
        /// </remarks>
        public static AppleseedPrincipal CurrentUser
        {
            get
            {
                AppleseedPrincipal r;

                if (HttpContext.Current.User is AppleseedPrincipal)
                {
                    r = (AppleseedPrincipal)HttpContext.Current.User;
                }
                else
                {
                    r = new AppleseedPrincipal(HttpContext.Current.User.Identity, Roles.GetRolesForUser());
                    HttpContext.Current.User = r;
                }

                return r;
            }

            set
            {
                HttpContext.Current.User = value;
            }
        }

        /// <summary>
        /// Gets the database version.
        /// </summary>
        /// <remarks></remarks>
        [Obsolete("Please use Appleseed.Framework.Settings.Database.DatabaseVersion", false)]
        public static int DatabaseVersion
        {
            get
            {
                return Database.DatabaseVersion;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [enable AD user].
        /// </summary>
        /// <remarks></remarks>
        [Obsolete("Please use Appleseed.Framework.Settings.Config.EnableADUser")]
        public static bool EnableADUser
        {
            get
            {
                return Config.EnableADUser;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [encrypt password].
        /// </summary>
        /// <remarks></remarks>
        [Obsolete("Please use Appleseed.Framework.Settings.Config.EncryptPassword")]
        public static bool EncryptPassword
        {
            get
            {
                return Config.EncryptPassword;
            }
        }

        /// <summary>
        ///   Gets static string fetches the portal's alias either via querystring, cookie or domain and returns it
        /// </summary>
        /// <value>The get portal unique ID.</value>
        /// <remarks>
        /// </remarks>
        [Obsolete("Use Appleseed.Framework.Settings.Portal.UniqueID")]
        public static string GetPortalUniqueID
        {
            get
            {
                return Portal.UniqueID;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether monitoring is enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is monitoring enabled; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// </remarks>
        [Obsolete("Please use Appleseed.Framework.Settings.Config.EnableMonitoring")]
        public static bool IsMonitoringEnabled
        {
            get
            {
                return Config.EnableMonitoring;
            }
        }

        /// <summary>
        ///   Gets the product version.
        /// </summary>
        /// <value>The product version.</value>
        /// <remarks>
        /// </remarks>
        public static string ProductVersion
        {
            get
            {
                if (HttpContext.Current.Application["ProductVersion"] == null)
                {
                    var f = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
                    HttpContext.Current.Application.Lock();
                    HttpContext.Current.Application["ProductVersion"] = f.ProductVersion;
                    HttpContext.Current.Application.UnLock();
                }

                return (string)HttpContext.Current.Application["ProductVersion"];
            }
        }

        /// <summary>
        ///   Gets or sets the scheduler.
        /// </summary>
        /// <value>The scheduler.</value>
        /// <remarks>
        /// </remarks>
        public static IScheduler Scheduler { get; set; }

        /// <summary>
        ///   Gets SmtpServer
        /// </summary>
        /// <value>The SMTP server.</value>
        /// <remarks>
        /// </remarks>
        [Obsolete("Please use Appleseed.Framework.Settings.Config.SmtpServer")]
        public static string SmtpServer
        {
            get
            {
                return Portal.SmtpServer;
                //return Appleseed.Framework.Settings.Config.SmtpServer;
            }
        }

        /// <summary>
        ///   Gets the SQL connection string.
        /// </summary>
        /// <remarks>
        /// </remarks>
        [Obsolete("Please use Appleseed.Framework.Settings.Config.SqlConnectionString")]
        public static SqlConnection SqlConnectionString
        {
            get
            {
                return Config.SqlConnectionString;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether all users will be loaded from portal 0 instance
        /// </summary>
        /// <value><c>true</c> if [use single user base]; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// </remarks>
        [Obsolete("Please use Appleseed.Framework.Settings.Config.UseSingleUserBase")]
        public static bool UseSingleUserBase
        {
            get
            {
                return Config.UseSingleUserBase;
            }
        }

        /// <summary>
        ///   Gets or sets the active module.
        /// </summary>
        /// <value>The active module.</value>
        /// <remarks>
        /// </remarks>
        public int ActiveModule
        {
            get
            {
                if (HttpContext.Current.Request.Params["mID"] != null)
                {
                    SetActiveModuleCookie(int.Parse(HttpContext.Current.Request.Params["mID"]));
                    return int.Parse(HttpContext.Current.Request.Params["mID"]);
                }

                var activeModule = HttpContext.Current.Request.Cookies["ActiveModule"];
                return activeModule != null ? int.Parse(activeModule.Value) : 0;
            }

            set
            {
                SetActiveModuleCookie(value);
            }
        }

        /// <summary>
        ///   Gets or sets the active page.
        /// </summary>
        /// <value>The active page.</value>
        /// <remarks>
        /// </remarks>
        public PageSettings ActivePage { get; set; }

        /// <summary>
        ///   Gets or sets current layout
        /// </summary>
        /// <value>The current layout.</value>
        /// <remarks>
        /// </remarks>
        public string CurrentLayout
        {
            get
            {
                // Patch for possible .NET framework bug
                // if returned an empty string caused an endless loop
                return string.IsNullOrEmpty(this.currentLayout) ? "Default" : this.currentLayout;
            }

            set
            {
                this.currentLayout = value;
            }
        }

        /// <summary>
        ///   Gets or sets the current theme alt.
        /// </summary>
        /// <value>The current theme alt.</value>
        /// <remarks>
        /// </remarks>
        public Theme CurrentThemeAlt { get; set; }

        /// <summary>
        ///   Gets or sets the current theme default.
        /// </summary>
        /// <value>The current theme default.</value>
        /// <remarks>
        /// </remarks>
        public Theme CurrentThemeDefault { get; set; }

        /// <summary>
        ///   Gets or sets the custom settings.
        /// </summary>
        /// <value>The custom settings.</value>
        /// <remarks>
        /// </remarks>
        public Dictionary<string, ISettingItem> CustomSettings { get; set; }

        /// <summary>
        ///   Gets or sets the desktop pages.
        /// </summary>
        /// <value>The desktop pages.</value>
        /// <remarks>
        /// </remarks>
        public List<PageStripDetails> DesktopPages { get; set; }

        /// <summary>
        ///   Gets the get terms of service.
        /// </summary>
        /// <value>The get terms of service.</value>
        /// <remarks>
        /// </remarks>
        public string GetTermsOfService
        {
            get
            {
                var termsOfService = string.Empty;

                // Verify if we have to show conditions
                if (this.CustomSettings["SITESETTINGS_TERMS_OF_SERVICE"] != null &&
                    this.CustomSettings["SITESETTINGS_TERMS_OF_SERVICE"].ToString().Length != 0)
                {
                    // // Attempt to load the required text
                    // Appleseed.Framework.DataTypes.PortalUrlDataType pt = new Appleseed.Framework.DataTypes.PortalUrlDataType();
                    // pt.Value = CustomSettings["SITESETTINGS_TERMS_OF_SERVICE"].ToString();
                    // string terms = HttpContext.Current.Server.MapPath(pt.FullPath);
                    // //Try to get localized version
                    // string localized_terms;
                    // localized_terms = terms.Replace(".", "_" + Esperantus.Localize.GetCurrentUINeutralCultureName() + ".");
                    // if (System.IO.File.Exists(localized_terms))
                    // terms = localized_terms;
                    // Fix by Joerg Szepan - jszepan 
                    // http://sourceforge.net/tracker/index.php?func=detail&aid=852071&group_id=66837&atid=515929
                    // Wrong Terms-File if Dot in Mappath
                    // Attempt to load the required text
                    var terms = this.CustomSettings["SITESETTINGS_TERMS_OF_SERVICE"].ToString();

                    // Try to get localized version
                    var localizedTerms = string.Empty;

                    // TODO: FIX THIS
                    // localized_terms = terms.Replace(".", "_" + Localize.GetCurrentUINeutralCultureName() + ".");
                    var pt = new PortalUrlDataType { Value = localizedTerms };

                    if (File.Exists(HttpContext.Current.Server.MapPath(pt.FullPath)))
                    {
                        terms = localizedTerms;
                    }

                    pt.Value = terms;
                    terms = HttpContext.Current.Server.MapPath(pt.FullPath);

                    // Load conditions
                    if (File.Exists(terms))
                    {
                        // Try to open file
                        using (var s = new StreamReader(terms, Encoding.Default))
                        {
                            // Get the text of the conditions
                            termsOfService = s.ReadToEnd();

                        }
                    }
                    else
                    {
                        // If load fails use default
                        termsOfService = string.Format("'{0}' not found!", terms);
                    }
                }

                // end Fix by Joerg Szepan - jszepan 
                return termsOfService;
            }
        }

        /// <summary>
        ///   Gets or sets the mobile pages.
        /// </summary>
        /// <value>The mobile pages.</value>
        /// <remarks>
        /// </remarks>
        public ArrayList MobilePages { get; set; }

        /// <summary>
        ///   Gets or sets the portal alias.
        /// </summary>
        /// <value>The portal alias.</value>
        /// <remarks>
        /// </remarks>
        public string PortalAlias { get; set; }

        /// <summary>
        ///   Gets or sets the portal content language.
        /// </summary>
        /// <value>The portal content language.</value>
        /// <remarks>
        /// </remarks>
        public CultureInfo PortalContentLanguage
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture;
            }

            set
            {
                Thread.CurrentThread.CurrentUICulture = value;
            }
        }

        /// <summary>
        ///   Gets or sets the portal data formatting culture.
        /// </summary>
        /// <value>The portal data formatting culture.</value>
        /// <remarks>
        /// </remarks>
        public CultureInfo PortalDataFormattingCulture
        {
            get
            {
                return Thread.CurrentThread.CurrentCulture;
            }

            set
            {
                Thread.CurrentThread.CurrentCulture = value;
            }
        }

        /// <summary>
        ///   Gets or sets the PortalPath.
        ///   Base dir for all portal data, relative to root web dir.
        /// </summary>
        /// <value>The portal full path.</value>
        /// <remarks>
        /// </remarks>
        public string PortalFullPath
        {
            get
            {
                var x = Path.WebPathCombine(this.portalPathPrefix, this.portalPath);

                // (_portalPathPrefix + _portalPath).Replace("//", "/");
                return x == "/" ? string.Empty : x;
            }

            set
            {
                this.portalPath = value.StartsWith(this.portalPathPrefix)
                                      ? value.Substring(this.portalPathPrefix.Length)
                                      : value;
            }
        }

        /// <summary>
        ///   Gets or sets the portal ID.
        /// </summary>
        /// <value>The portal ID.</value>
        /// <remarks>
        /// </remarks>
        public int PortalID { get; set; }

        /// <summary>
        ///   Gets PortalLayoutPath is the full path in which all Layout files are
        /// </summary>
        /// <value>The portal layout path.</value>
        /// <remarks>
        /// </remarks>
        public string PortalLayoutPath
        {
            get
            {
                var thisLayoutPath = this.CurrentLayout;
                var customLayout = string.Empty;

                // Thierry (Tiptopweb), 4 July 2003, switch to custom Layout
                if (this.ActivePage.CustomSettings[StringsCustomLayout] != null &&
                    this.ActivePage.CustomSettings[StringsCustomLayout].ToString().Length > 0)
                {
                    customLayout = this.ActivePage.CustomSettings[StringsCustomLayout].ToString();
                }

                if (customLayout.Length != 0)
                {
                    // we have a custom Layout
                    thisLayoutPath = customLayout;
                }

                // Try to get layout from query string
                if (HttpContext.Current != null && HttpContext.Current.Request.Params["Layout"] != null)
                {
                    thisLayoutPath = HttpContext.Current.Request.Params["Layout"];
                }

                // yiming, 18 Aug 2003, get layout from portalWebPath, if no, then WebPath
                var layoutManager = new LayoutManager(this.PortalPath);

                return Directory.Exists(string.Format("{0}/{1}/", layoutManager.PortalLayoutPath, thisLayoutPath))
                           ? string.Format("{0}/{1}/", layoutManager.PortalWebPath, thisLayoutPath)
                           : string.Format("{0}/{1}/", LayoutManager.WebPath, thisLayoutPath);
            }
        }

        /// <summary>
        ///   Gets or sets the name of the portal.
        /// </summary>
        /// <value>The name of the portal.</value>
        /// <remarks>
        /// </remarks>
        public string PortalName { get; set; }

        /// <summary>
        ///   Gets the portal pages XML.
        /// </summary>
        /// <value>The portal pages XML.</value>
        /// <remarks>
        /// </remarks>
        public XmlDocument PortalPagesXml
        {
            get
            {
                var sw = new StringWriter();
                {
                    var writer = new XmlTextWriter(sw) { Formatting = Formatting.None };
                    writer.WriteStartDocument(true);
                    writer.WriteStartElement("MenuData"); // start MenuData element
                    writer.WriteStartElement("MenuGroup"); // start top MenuGroup element

                    foreach (var page in
                        this.DesktopPages.Cast<PageStripDetails>().Where(page => page.ParentPageID == 0))
                    {
                        writer.WriteStartElement("MenuItem"); // start MenuItem element
                        writer.WriteAttributeString("ParentPageId", page.ParentPageID.ToString());

                        writer.WriteAttributeString(
                            "UrlPageName",
                            HttpUrlBuilder.UrlPageName(page.PageID) == HttpUrlBuilder.DefaultPage
                                ? page.PageName
                                : HttpUrlBuilder.UrlPageName(page.PageID).Replace(".aspx", string.Empty));

                        writer.WriteAttributeString("PageName", page.PageName);

                        // writer.WriteAttributeString("Label",myPage.PageName);
                        writer.WriteAttributeString("PageOrder", page.PageOrder.ToString());
                        writer.WriteAttributeString("PageIndex", page.PageIndex.ToString());
                        writer.WriteAttributeString("PageLayout", page.PageLayout);
                        writer.WriteAttributeString("AuthRoles", page.AuthorizedRoles);
                        writer.WriteAttributeString("ID", page.PageID.ToString());

                        // writer.WriteAttributeString("URL",HttpUrlBuilder.BuildUrl(string.Concat("~/",myPage.PageName,".aspx"),myPage.PageID,0,null,string.Empty,this.PortalAlias,"hello/goodbye"));
                        this.RecursePortalPagesXml(page, writer);
                        writer.WriteEndElement(); // end MenuItem element
                    }

                    writer.WriteEndElement(); // end top MenuGroup element
                    writer.WriteEndElement(); // end MenuData element
                    writer.Flush();
                    this.portalPagesXml = new XmlDocument();
                    this.portalPagesXml.LoadXml(sw.ToString());
                    writer.Close();
                }

                return this.portalPagesXml;
            }
        }

        /// <summary>
        ///   Gets or sets PortalPath.
        ///   Base dir for all portal data, relative to application
        /// </summary>
        /// <value>The portal path.</value>
        /// <remarks>
        /// </remarks>
        public string PortalPath
        {
            get
            {
                return this.portalPath;
            }

            set
            {
                this.portalPath = value;

                // // by manu
                // // be sure it starts with "/"
                // if (_portalPath.Length > 0 && !_portalPath.StartsWith("/"))
                // _portalPath = Appleseed.Framework.Settings.Path.WebPathCombine("/", _portalPath);
            }
        }

        /// <summary>
        ///   Gets or sets PortalSecurePath.
        ///   Base dir for SSL
        /// </summary>
        /// <value>The portal secure path.</value>
        /// <remarks>
        /// </remarks>
        public string PortalSecurePath
        {
            get
            {
                if (this.portalSecurePath == null)
                {
                    this.PortalSecurePath = Config.PortalSecureDirectory;
                }

                return this.portalSecurePath;
            }

            set
            {
                this.portalSecurePath = value;
            }
        }

        /// <summary>
        ///   Gets or sets the portal title.
        /// </summary>
        /// <value>The portal title.</value>
        /// <remarks>
        /// </remarks>
        public string PortalTitle { get; set; }

        /// <summary>
        ///   Gets or sets the portal UI language.
        /// </summary>
        /// <value>The portal UI language.</value>
        /// <remarks>
        /// </remarks>
        public CultureInfo PortalUILanguage
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture;
            }

            set
            {
                Thread.CurrentThread.CurrentUICulture = value;
            }
        }

        /// <summary>
        ///   Gets the settings.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public IDictionary<string, ISettingItem> Settings
        {
            get
            {
                return GetPortalCustomSettings(this.PortalID, GetPortalBaseSettings(this.PortalPath));
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether [show pages].
        /// </summary>
        /// <value><c>true</c> if [show pages]; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// </remarks>
        public bool ShowPages { get; set; }

        /// <summary>
        ///   Gets the Appleseed cultures.
        /// </summary>
        /// <value>The Appleseed cultures.</value>
        /// <remarks>
        /// </remarks>
        private static CultureInfo[] AppleseedCultures
        {
            get
            {
                var locker = new object();
                lock (locker)
                {
                    if (appleseedCultures == null || appleseedCultures.Length == 0)
                    {
                        var cultures = Config.DefaultLanguageList.Split(new[] { ';' });

                        var appleseedCulturesArray = cultures.Select(culture => new CultureInfo(culture)).ToList();

                        appleseedCultures = new CultureInfo[appleseedCulturesArray.Count];
                        appleseedCulturesArray.CopyTo(appleseedCultures);
                    }

                    return appleseedCultures;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get portal settings by pageid and portal alias
        /// </summary>
        /// <param name="pageId">Page ID</param>
        /// <param name="portalAlias">Portal Allias</param>
        /// <returns></returns>
        /// Ashish.patel@haptix.biz - 2014/12/16 - Get PORTAL SETTINGS
        public static PortalSettings GetPortalSettingsbyPageID(int pageId, string portalAlias)
        {
            // Set portal settings
            var portalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];

            //If portal settings null then get it and fill here
            if (portalSettings == null)
            {
                //Get portal settings from the method
                portalSettings = GetPortalSettingFromDB(pageId, portalAlias, portalSettings);

                // Add key as portalsettings into the dictionalry 
                HttpContext.Current.Items.Add("PortalSettings", portalSettings);
                HttpContext.Current.Items.Add("PortalID", portalSettings.PortalID);
            }
            else
            {
                // Check for portalsettings active page.
                //IF null or not match the fill
                if (portalSettings.ActivePage == null || portalSettings.ActivePage.PageID != pageId)
                {
                    // Get portal settings from the method
                    portalSettings = GetPortalSettingFromDB(pageId, portalAlias, portalSettings);
                    HttpContext.Current.Items["PortalSettings"] = portalSettings;
                    HttpContext.Current.Items["PortalID"] = portalSettings.PortalID;
                }
            }

            return portalSettings;
        }

        /// <summary>
        /// Get page friendly url is enabled or not and also set the portal setting
        /// </summary>
        /// <param name="pageId"> Page id</param>
        /// <param name="portalAlias"> Portal Alias</param>
        /// <returns> return enablePageFriendlyurl bool</returns>
        /// Ashish.patel@haptix.biz - 2014/12/24 - Check for Enable Friendly url 
        public static bool HasEnablePageFriendlyUrl(int pageId, string portalAlias)
        {
            // Set portal settings
            var portalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            //If portal settings null then get it and fill here
            if (portalSettings == null)
            {
                //Get portal settings from the method
                portalSettings = GetPortalSettingFromDB(pageId, portalAlias, portalSettings);

                // Add key as portalsettings into the dictionary 
                HttpContext.Current.Items.Add("PortalSettings", portalSettings);
                HttpContext.Current.Items.Add("PortalID", portalSettings.PortalID);
            }
            return portalSettings.EnablePageFriendlyUrl;
        }

        private static PortalSettings GetPortalSettingFromDB(int pageId, string portalAlias, PortalSettings portalSettings)
        {
            try
            {
                portalSettings = PortalSettings.GetPortalSettings(pageId, portalAlias);
            }
            catch (DatabaseUnreachableException dexc)
            {
                // If no database, must update
                ErrorHandler.Publish(LogLevel.Error, dexc);
                using (var s = new Appleseed.Framework.Update.Services())
                {
                    s.RunDBUpdate(Config.ConnectionString);
                }

                portalSettings = PortalSettings.GetPortalSettings(pageId, portalAlias);
            }

            if (portalSettings == null || (portalSettings != null && portalSettings.PortalAlias == null))
            {
                portalSettings = PortalSettings.GetPortalSettings(pageId, Config.DefaultPortal);
            }

            return portalSettings;
        }

        /// <summary>
        /// Flushes the base settings cache.
        /// </summary>
        /// <param name="portalPath">
        /// The portal path.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void FlushBaseSettingsCache(string portalPath)
        {
            CurrentCache.Remove(Key.PortalBaseSettings());
            CurrentCache.Remove(Key.LanguageList());
            RemovePortalSettingsCache();
        }

        /// <summary>
        /// Get the ParentPageID of a certain Page 06/11/2004 Rob Siera
        /// </summary>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        /// <param name="tabList">
        /// The tab list.
        /// </param>
        /// <returns>
        /// The get parent page id.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static int GetParentPageID(int pageId, ArrayList tabList)
        {
            foreach (var tmpPage in tabList.Cast<PageStripDetails>().Where(tmpPage => tmpPage.PageID == pageId))
            {
                return tmpPage.ParentPageID;
            }

            throw new ArgumentOutOfRangeException("pageId", "Root not found");
        }

        /// <summary>
        /// Gets the portal base settings.
        /// </summary>
        /// <param name="portalPath">
        /// The portal path.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static Dictionary<string, ISettingItem> GetPortalBaseSettings(string portalPath)
        {
            Dictionary<string, ISettingItem> baseSettings;

            if (!CurrentCache.Exists(Key.PortalBaseSettings()))
            {
                // fix: Jes1111 - 27-02-2005 - for proper operation of caching
                var layoutManager = new LayoutManager(portalPath);
                var layoutList = layoutManager.GetLayouts();
                var themeManager = new ThemeManager(portalPath);
                var themeList = themeManager.GetThemes();

                // Define base settings
                baseSettings = new Dictionary<string, ISettingItem>();

                var group = SettingItemGroup.THEME_LAYOUT_SETTINGS;
                var groupOrderBase = (int)SettingItemGroup.THEME_LAYOUT_SETTINGS;

                // StringDataType
                var image =
                    new SettingItem<string, TextBox>(
                        new UploadedFileDataType(Path.WebPathCombine(Path.ApplicationRoot, portalPath)))
                    {
                        Order = groupOrderBase + 5,
                        Group = group,
                        EnglishName = "Logo",
                        Description =
                                "Enter the name of logo file here. The logo will be searched in your portal dir. For the default portal is (~/_Appleseed)."
                    };

                baseSettings.Add("SITESETTINGS_LOGO", image);

                // ArrayList layoutList = new LayoutManager(PortalPath).GetLayouts();
                var tabLayoutSetting =
                    new SettingItem<string, ListControl>(new CustomListDataType(layoutList, StringsName, StringsName))
                    {
                        Value = "Default",
                        Order = groupOrderBase + 10,
                        Group = group,
                        EnglishName = "Page layout",
                        Description = "Specify the site level page layout here."
                    };
                baseSettings.Add("SITESETTINGS_PAGE_LAYOUT", tabLayoutSetting);

                // ArrayList themeList = new ThemeManager(PortalPath).GetThemes();
                var theme =
                    new SettingItem<string, ListControl>(new CustomListDataType(themeList, StringsName, StringsName))
                    {
                        Required = true,
                        Order = groupOrderBase + 15,
                        Group = group,
                        EnglishName = "Theme",
                        Description = "Specify the site level theme here."
                    };
                baseSettings.Add("SITESETTINGS_THEME", theme);

                // SettingItem ThemeAlt = new SettingItem(new CustomListDataType(new ThemeManager(PortalPath).GetThemes(), strName, strName));
                var themeAlt =
                    new SettingItem<string, ListControl>(new CustomListDataType(themeList, StringsName, StringsName))
                    {
                        Required = true,
                        Order = groupOrderBase + 20,
                        Group = group,
                        EnglishName = "Alternate theme",
                        Description = "Specify the site level alternate theme here."
                    };
                baseSettings.Add("SITESETTINGS_ALT_THEME", themeAlt);

                // Jes1111 - 2004-08-06 - Zen support
                var allowModuleCustomThemes = new SettingItem<bool, CheckBox>
                {
                    Order = groupOrderBase + 25,
                    Group = group,
                    Value = true,
                    EnglishName = "Allow Module Custom Themes?",
                    Description = "Select to allow Custom Theme to be set on Modules."
                };
                baseSettings.Add("SITESETTINGS_ALLOW_MODULE_CUSTOM_THEMES", allowModuleCustomThemes);

                //Ashish.patel@haptix.biz - 2014/12/10 - Add Textbox for enter the path for user specific jQuery
                var txtjQuery = new SettingItem<string, TextBox>()
                {
                    Order = groupOrderBase + 30,
                    Group = group,
                    EnglishName = "jQuery File Path",
                    Description =
                        "jQuery File Path"
                };
                baseSettings.Add("SITESETTINGS_JQUERY", txtjQuery);

                //Ashish.patel@haptix.biz - 2014/12/10 - Add Textbox for enter the path for user specific jQuery
                var txtjQueryUI =
                 new SettingItem<string, TextBox>()
                 {
                     Order = groupOrderBase + 35,
                     Group = group,
                     EnglishName = "jQuery UI File Path",
                     Description =
                         "jQuery UI File Path"
                 };
                baseSettings.Add("SITESETTINGS_JQUERY_UI", txtjQueryUI);

                groupOrderBase = (int)SettingItemGroup.SECURITY_USER_SETTINGS;
                group = SettingItemGroup.SECURITY_USER_SETTINGS;

                // checkbox for enable private site
                //Ashish.patel@haptix.biz - 2016/07/07 - To may site private
                var enablePrivateSite = new SettingItem<bool, CheckBox>
                {
                    Order = groupOrderBase + 4,
                    Group = group,
                    EnglishName = "Private Site",
                    Description = "This will make entire site as private which require to login",
                    Value = false
                };
                baseSettings.Add("ENABLE_PRIVATE_SITE", enablePrivateSite);

                // Show input for Portal Administrators when using Windows Authentication and Multi-portal
                // cisakson@yahoo.com 28.April.2003
                // This setting is removed in Global.asa for non-Windows authentication sites.
                var portalAdmins = new SettingItem<string, TextBox>
                {
                    Order = groupOrderBase + 5,
                    Group = group,
                    Value = Config.ADAdministratorGroup,
                    Required = false,
                    Description =
                            "Show input for Portal Administrators when using Windows Authentication and Multi-portal"
                };

                // jes1111 - PortalAdmins.Value = ConfigurationSettings.AppSettings["ADAdministratorGroup"];
                baseSettings.Add("WindowsAdmins", portalAdmins);

                // Allow new registrations?
                var allowNewRegistrations = new SettingItem<bool, CheckBox>
                {
                    Order = groupOrderBase + 10,
                    Group = group,
                    Value = true,
                    EnglishName = "Allow New Registrations?",
                    Description =
                            "Check this to allow users register themselves. Leave blank for register through User Manager only."
                };
                baseSettings.Add("SITESETTINGS_ALLOW_NEW_REGISTRATION", allowNewRegistrations);

                // MH: added dynamic load of register types depending on the  content in the DesktopModules/Register/ folder
                // Register
                var regPages = new Hashtable();

                foreach (var registerPage in
                    Directory.GetFiles(
                        HttpContext.Current.Server.MapPath(
                            Path.ApplicationRoot + "/DesktopModules/CoreModules/Register/"),
                        "register*.ascx",
                        SearchOption.AllDirectories))
                {
                    var registerPageDisplayName = registerPage.Substring(
                        registerPage.LastIndexOf("\\") + 1,
                        registerPage.LastIndexOf(".") - registerPage.LastIndexOf("\\") - 1);

                    // string registerPageName = registerPage.Substring(registerPage.LastIndexOf("\\") + 1);
                    var registerPageName = registerPage.Replace(Path.ApplicationPhysicalPath, "~/").Replace("\\", "/");
                    regPages.Add(registerPageDisplayName, registerPageName.ToLower());
                }

                // Register Layout Setting
                var regType = new SettingItem<string, ListControl>(new CustomListDataType(regPages, "Key", "Value"))
                {
                    Required = true,
                    Value = "RegisterFull.ascx",
                    EnglishName = "Register Type",
                    Description = "Choose here how Register Page should look like.",
                    Order = groupOrderBase + 15,
                    Group = group
                };
                baseSettings.Add("SITESETTINGS_REGISTER_TYPE", regType);

                // MH:end
                // Register Layout Setting module id reference by manu
                var regModuleId = new SettingItem<int, TextBox>
                {
                    Value = 0,
                    Required = true,
                    Order = groupOrderBase + 16,
                    Group = group,
                    EnglishName = "Register Module ID",
                    Description =
                            "Some custom registration may require additional settings, type here the ID of the module from where we should load settings (0= not used). Usually this module is added in an hidden area."
                };
                baseSettings.Add("SITESETTINGS_REGISTER_MODULEID", regModuleId);

                // Send mail on new registration to
                var onRegisterSendTo = new SettingItem<string, TextBox>
                {
                    Value = string.Empty,
                    Required = false,
                    Order = groupOrderBase + 17,
                    Group = group,
                    EnglishName = "Send Mail To",
                    Description = "On new registration a mail will be send to the email address you provide here."
                };
                baseSettings.Add("SITESETTINGS_ON_REGISTER_SEND_TO", onRegisterSendTo);

                // Send mail on new registration to User from
                var onRegisterSendFrom = new SettingItem<string, TextBox>
                {
                    Value = string.Empty,
                    Required = false,
                    Order = groupOrderBase + 18,
                    Group = group,
                    EnglishName = "Send Mail From",
                    Description =
                            "On new registration a mail will be send to the new user from the email address you provide here."
                };
                baseSettings.Add("SITESETTINGS_ON_REGISTER_SEND_FROM", onRegisterSendFrom);

                // Terms of service
                var termsOfService = new SettingItem<string, TextBox>(new PortalUrlDataType())
                {
                    Order = groupOrderBase + 20,
                    Group = group,
                    EnglishName = "Terms file name",
                    Description =
                            "Type here a file name used for showing terms and condition in each register page. Provide localized version adding _<culturename>. E.g. Terms.txt, will search for Terms.txt and for Terms_en-US.txt"
                };
                baseSettings.Add("SITESETTINGS_TERMS_OF_SERVICE", termsOfService);

                var loginPages = new Hashtable();

                var baseDir = HttpContext.Current.Server.MapPath(Path.ApplicationRoot + "/DesktopModules/");

                foreach (var loginPage in Directory.GetFiles(baseDir, "signin*.ascx", SearchOption.AllDirectories))
                {
                    var loginPageDisplayName =
                        loginPage.Substring(loginPage.LastIndexOf("DesktopModules") + 1).Replace(".ascx", string.Empty);
                    var loginPageName = loginPage.Replace(Path.ApplicationPhysicalPath, "~/").Replace("\\", "/");
                    loginPages.Add(loginPageDisplayName, loginPageName.ToLower());
                }

                var logonType = new SettingItem<string, ListControl>(new CustomListDataType(loginPages, "Key", "Value"))
                {
                    Required = false,
                    Value = "Signin.ascx",
                    EnglishName = "Login Type",
                    Description = "Choose here how login Page should look like.",
                    Order = groupOrderBase + 21,
                    Group = group
                };
                baseSettings.Add("SITESETTINGS_LOGIN_TYPE", logonType);

                // ReCaptcha public and private key
                var recaptchaPrivateKey = new SettingItem<string, TextBox>()
                {
                    Required = false,
                    Value = "6LeQmsASAAAAADS-WeMyg9mKo5l3ERKcB4LSQieI",
                    EnglishName = "ReCaptcha private key",
                    Description = "Insert here google's ReCaptcha private key for your portal's captchas.",
                    Order = groupOrderBase + 22,
                    Group = group
                };
                baseSettings.Add("SITESETTINGS_RECAPTCHA_PRIVATE_KEY", recaptchaPrivateKey);

                var recaptchaPublicKey = new SettingItem<string, TextBox>()
                {
                    Required = false,
                    Value = "6LeQmsASAAAAAIx9ZoRJXA44sajtJjPl2L_MFrTS",
                    EnglishName = "ReCaptcha public key",
                    Description = "Insert here google's ReCaptcha public key for your portal's captchas.",
                    Order = groupOrderBase + 23,
                    Group = group
                };
                baseSettings.Add("SITESETTINGS_RECAPTCHA_PUBLIC_KEY", recaptchaPublicKey);

                // Facebook keys
                var facebookAppId = new SettingItem<string, TextBox>()
                {
                    Required = false,
                    Value = "",
                    EnglishName = "Facebook Application ID",
                    Description = "Insert here facebook's Application ID for your portal.",
                    Order = groupOrderBase + 24,
                    Group = group
                };
                baseSettings.Add("SITESETTINGS_FACEBOOK_APP_ID", facebookAppId);

                var facebookAppSecret = new SettingItem<string, TextBox>()
                {
                    Required = false,
                    Value = "",
                    EnglishName = "Facebook Application Secret",
                    Description = "Insert here facebook's Application Secret for your portal.",
                    Order = groupOrderBase + 25,
                    Group = group
                };
                baseSettings.Add("SITESETTINGS_FACEBOOK_APP_SECRET", facebookAppSecret);

                // Twitter keys
                var twitterAppId = new SettingItem<string, TextBox>()
                {
                    Required = false,
                    Value = "",
                    EnglishName = "Twitter Application ID",
                    Description = "Insert here twitter's Application ID for your portal.",
                    Order = groupOrderBase + 26,
                    Group = group
                };
                baseSettings.Add("SITESETTINGS_TWITTER_APP_ID", twitterAppId);

                var twitterAppSecret = new SettingItem<string, TextBox>()
                {
                    Required = false,
                    Value = "",
                    EnglishName = "Twitter Application Secret",
                    Description = "Insert here twitter's Application Secret for your portal.",
                    Order = groupOrderBase + 27,
                    Group = group
                };
                baseSettings.Add("SITESETTINGS_TWITTER_APP_SECRET", twitterAppSecret);

                var googleLogin = new SettingItem<bool, CheckBox>()
                {
                    Required = false,
                    Value = false,

                    EnglishName = "Google Login",
                    Description = "Check if you want to see the google login",
                    Order = groupOrderBase + 28,
                    Group = group
                };
                baseSettings.Add("SITESETTINGS_GOOGLE_LOGIN", googleLogin);

                // LinkedIn keys
                var linkedInAppId = new SettingItem<string, TextBox>()
                {
                    Required = false,
                    Value = "",
                    EnglishName = "LinkedIn Application ID",
                    Description = "Insert here linkedIn's Application ID for your portal.",
                    Order = groupOrderBase + 29,
                    Group = group
                };
                baseSettings.Add("SITESETTINGS_LINKEDIN_APP_ID", linkedInAppId);

                var linkedInAppSecret = new SettingItem<string, TextBox>()
                {
                    Required = false,
                    Value = "",
                    EnglishName = "LinkedIn Application Secret",
                    Description = "Insert here linkedIn's Application Secret for your portal.",
                    Order = groupOrderBase + 30,
                    Group = group
                };
                baseSettings.Add("SITESETTINGS_LINKEDIN_APP_SECRET", linkedInAppSecret);


                groupOrderBase = (int)SettingItemGroup.META_SETTINGS;
                group = SettingItemGroup.META_SETTINGS;

                // added: Jes1111 - page DOCTYPE setting
                var docType = new SettingItem<string, TextBox>
                {
                    Order = groupOrderBase + 5,
                    Group = group,
                    EnglishName = "DOCTYPE string",
                    Description =
                            "Allows you to enter a DOCTYPE string which will be inserted as the first line of the HTML output page (i.e. above the <html> element). Use this to force Quirks or Standards mode, particularly in IE. See <a href=\"http://gutfeldt.ch/matthias/articles/doctypeswitch/table.html\" target=\"_blank\">here</a> for details. NOTE: Appleseed.Zen requires a setting that guarantees Standards mode on all browsers.",
                    Value = string.Empty
                };

                baseSettings.Add("SITESETTINGS_DOCTYPE", docType);

                // by John Mandia <john.mandia@whitelightsolutions.com>
                var tabTitle = new SettingItem<string, TextBox>
                {
                    Order = groupOrderBase + 10,
                    Group = group,
                    EnglishName = "Page title",
                    Description = "Allows you to enter a default tab / page title (Shows at the top of your browser)."
                };
                baseSettings.Add("SITESETTINGS_PAGE_TITLE", tabTitle);

                /*
                 * John Mandia: Removed This Setting. Now You can define specific Url Keywords via Tab Settings only. This is to speed up url building.
                 * 
                SettingItem TabUrlKeyword = new SettingItem<string, TextBox>;
                TabUrlKeyword.Order = _groupOrderBase + 15;
                TabUrlKeyword.Group = _Group;
                TabUrlKeyword.Value = "Portal";
                TabUrlKeyword.EnglishName = "Keyword to Identify all pages";
                TabUrlKeyword.Description = "This setting is not fully implemented yet. It was to help with search engine optimisation by allowing you to specify a default keyword that would appear in your url."; 
                BaseSettings.Add("SITESETTINGS_PAGE_URL_KEYWORD", TabUrlKeyword);
                */
                var tabMetaKeyWords = new SettingItem<string, TextBox>
                {
                    Order = groupOrderBase + 15,
                    Group = group,
                    EnglishName = "Page keywords",
                    Description =
                            "This setting is to help with search engine optimisation. Enter 1-15 Default Keywords that represent what your site is about."
                };

                // john.mandia@whitelightsolutions.com: No Default Value In Case People Don't want Meta Keywords; http://sourceforge.net/tracker/index.php?func=detail&aid=915614&group_id=66837&atid=515929
                baseSettings.Add("SITESETTINGS_PAGE_META_KEYWORDS", tabMetaKeyWords);
                var tabMetaDescription = new SettingItem<string, TextBox>
                {
                    Order = groupOrderBase + 20,
                    Group = group,
                    EnglishName = "Page description",
                    Description =
                            "This setting is to help with search engine optimisation. Enter a default description (Not too long though. 1 paragraph is enough) that describes your portal."
                };

                // john.mandia@whitelightsolutions.com: No Default Value In Case People Don't want a defautl descripton
                baseSettings.Add("SITESETTINGS_PAGE_META_DESCRIPTION", tabMetaDescription);
                var tabMetaEncoding = new SettingItem<string, TextBox>
                {
                    Order = groupOrderBase + 25,
                    Group = group,
                    EnglishName = "Page encoding",
                    Description =
                            "Every time your browser returns a page it looks to see what format it is retrieving. This allows you to specify the default content type.",
                    Value = "<META http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1252\" />"
                };
                baseSettings.Add("SITESETTINGS_PAGE_META_ENCODING", tabMetaEncoding);

                // chckbox for enable friendly URL
                //Ashish.patel@haptix.biz - 2014/12/11 - Allow user to access the frindly URL
                var enableFriendlyURL = new SettingItem<bool, CheckBox>
                {
                    Order = groupOrderBase + 27,
                    Group = group,
                    EnglishName = "Enable Friendly URL?",
                    Description =
                        "Allow user to enable friendly URL functionality.",
                    Value = false
                };
                baseSettings.Add("ENABLE_PAGE_FRIENDLY_URL", enableFriendlyURL);

                var tabMetaOther = new SettingItem<string, TextBox>
                {
                    Order = groupOrderBase + 30,
                    Group = group,
                    EnglishName = "Default Additional Meta Tag Entries",
                    Description =
                            "This setting allows you to enter new tags into the Tab / Page's HEAD Tag. As an example we have added a portal tag to identify the version, but you could have a meta refresh tag or something else like a css reference instead.",
                    Value = string.Empty
                };
                baseSettings.Add("SITESETTINGS_PAGE_META_OTHERS", tabMetaOther);
                var tabKeyPhrase = new SettingItem<string, TextBox>
                {
                    Order = groupOrderBase + 35,
                    Group = group,
                    EnglishName = "Default Page Keyphrase",
                    Description =
                            "This setting can be used by a module or by a control. It allows you to define a common message for the entire portal e.g. Welcome to x portal! This can be used for search engine optimisation. It allows you to define a keyword rich phrase to be used throughout your portal.",
                    Value = "Enter your default keyword rich Tab / Page phrase here. "
                };
                baseSettings.Add("SITESETTINGS_PAGE_KEY_PHRASE", tabKeyPhrase);

                // added: Jes1111 - <body> element attributes setting
                var bodyAttributes = new SettingItem<string, TextBox>
                {
                    Order = groupOrderBase + 45,
                    Group = group,
                    EnglishName = "&lt;body&gt; attributes",
                    Description =
                            "Allows you to enter a string which will be inserted within the <body> element, e.g. leftmargin=\"0\" bottommargin=\"0\", etc. NOTE: not advisable to use this to inject onload() function calls as there is a programmatic function for that. NOTE also that is your CSS is well sorted you should not need anything here.",
                    Required = false
                };
                baseSettings.Add("SITESETTINGS_BODYATTS", bodyAttributes);

                // end by John Mandia <john.mandia@whitelightsolutions.com>
                var glAnalytics = new SettingItem<string, TextBox>
                {
                    Order = groupOrderBase + 50,
                    Group = group,
                    EnglishName = "Google-Analytics Code",
                    Description = "Allows you get the tracker, with this can view the statistics of your site.",
                    Value = string.Empty
                };
                baseSettings.Add("SITESETTINGS_GOOGLEANALYTICS", glAnalytics);

                var glAnalyticsCustomVars = new SettingItem<bool, CheckBox>
                {
                    Order = groupOrderBase + 52,
                    Group = group,
                    EnglishName = "Use Google-Analytics Custom Vars?",
                    Description =
                            "Allows you to use custom vars to track members, authenticated users and their domain names.",
                    Value = false
                };
                baseSettings.Add("SITESETTINGS_GOOGLEANALYTICS_CUSTOMVARS", glAnalyticsCustomVars);

                var alternativeUrl = new SettingItem<string, TextBox>
                {
                    Order = groupOrderBase + 55,
                    Group = group,
                    EnglishName = "Alternative site url",
                    Description = "Indicate the site url for an alternative way.",
                    Value = string.Empty
                };
                baseSettings.Add("SITESETTINGS_ALTERNATIVE_URL", alternativeUrl);

                var SnapEngage = new SettingItem<string, TextBox>
                {
                    Order = groupOrderBase + 57,
                    Group = group,
                    EnglishName = "SnapEngage code",
                    Description = "Allows you create a chat. Need an acount on SnapEngage.",
                    Value = string.Empty
                };
                baseSettings.Add("SITESETTINGS_SNAPENGAGE", SnapEngage);


                //var addThisUsername = new SettingItem<string, TextBox>
                //    {
                //        Order = groupOrderBase + 56,
                //        Group = group,
                //        EnglishName = "AddThis Username",
                //        Description = "Username for AddThis sharing and tracking.",
                //        Value = "appleseedapp"
                //    };
                //baseSettings.Add("SITESETTINGS_ADDTHIS_USERNAME", addThisUsername);

                groupOrderBase = (int)SettingItemGroup.CULTURE_SETTINGS;
                group = SettingItemGroup.CULTURE_SETTINGS;

                var langList =
                    new SettingItem<string, ListControl>(
                        new MultiSelectListDataType(AppleseedCultures, "DisplayName", "Name"))
                    {
                        Group = group,
                        Order = groupOrderBase + 10,
                        EnglishName = "Language list",
                        Value = Config.DefaultLanguage,
                        Required = false,
                        Description =
                                "This is a list of the languages that the site will support. You can select multiples languages by pressing shift in your keyboard"
                    };

                // jes1111 - LangList.Value = ConfigurationSettings.AppSettings["DefaultLanguage"]; 
                baseSettings.Add("SITESETTINGS_LANGLIST", langList);

                var langDefault =
                    new SettingItem<string, DropDownList>(
                        new ListDataType<string, DropDownList>(AppleseedCultures, "DisplayName", "Name"))
                    {
                        Group = group,
                        Order = groupOrderBase + 20,
                        EnglishName = "Default Language",
                        Value = Config.DefaultLanguage,
                        Required = false,
                        Description = "This is the default language for the site."
                    };

                // jes1111 - LangList.Value = ConfigurationSettings.AppSettings["DefaultLanguage"]; 
                baseSettings.Add("SITESETTINGS_DEFAULTLANG", langDefault);

                #region Miscellaneous Settings

                groupOrderBase = (int)SettingItemGroup.MISC_SETTINGS;
                group = SettingItemGroup.MISC_SETTINGS;

                // Show modified by summary on/off
                var showModifiedBy = new SettingItem<bool, CheckBox>
                {
                    Order = groupOrderBase + 10,
                    Group = group,
                    Value = false,
                    EnglishName = "Show modified by",
                    Description = "Check to show by whom the module is last modified."
                };
                baseSettings.Add("SITESETTINGS_SHOW_MODIFIED_BY", showModifiedBy);

                // Default Editor Configuration used for new modules and workflow modules. jviladiu@portalServices.net 13/07/2004
                var defaultEditor = new SettingItem<string, DropDownList>(new HtmlEditorDataType())
                {
                    Order = groupOrderBase + 20,
                    Group = group,
                    Value = "FCKeditor",
                    EnglishName = "Default Editor",
                    Description = "This Editor is used by workflow and is the default for new modules."
                };
                baseSettings.Add("SITESETTINGS_DEFAULT_EDITOR", defaultEditor);

                // Default Editor Width. jviladiu@portalServices.net 13/07/2004
                var defaultWidth = new SettingItem<int, TextBox>
                {
                    Order = groupOrderBase + 25,
                    Group = group,
                    Value = 700,
                    EnglishName = "Editor Width",
                    Description = "Default Editor Width"
                };
                baseSettings.Add("SITESETTINGS_EDITOR_WIDTH", defaultWidth);

                // Default Editor Height. jviladiu@portalServices.net 13/07/2004
                var defaultHeight = new SettingItem<int, TextBox>
                {
                    Order = groupOrderBase + 30,
                    Group = group,
                    Value = 400,
                    EnglishName = "Editor Height",
                    Description = "Default Editor Height"
                };
                baseSettings.Add("SITESETTINGS_EDITOR_HEIGHT", defaultHeight);

                // Show Upload (Active up editor only). jviladiu@portalServices.net 13/07/2004
                var showUpload = new SettingItem<bool, CheckBox>
                {
                    Value = true,
                    Order = groupOrderBase + 35,
                    Group = group,
                    EnglishName = "Upload?",
                    Description = "Only used if Editor is ActiveUp HtmlTextBox"
                };
                baseSettings.Add("SITESETTINGS_SHOWUPLOAD", showUpload);

                // Default Image Folder. jviladiu@portalServices.net 29/07/2004
                var defaultImageFolder =
                    new SettingItem<string, Panel>(
                        new FolderDataType(
                            HttpContext.Current.Server.MapPath(
                                string.Format("{0}/{1}/images", Path.ApplicationRoot, portalPath)),
                            "default"))
                    {
                        Order = groupOrderBase + 40,
                        Group = group,
                        Value = "default",
                        EnglishName = "Default Image Folder",
                        Description = "Set the default image folder used by Current Editor"
                    };
                baseSettings.Add("SITESETTINGS_DEFAULT_IMAGE_FOLDER", defaultImageFolder);
                groupOrderBase = (int)SettingItemGroup.MISC_SETTINGS;
                group = SettingItemGroup.MISC_SETTINGS;

                // Show module arrows to an administrator
                var showModuleArrows = new SettingItem<bool, CheckBox>
                {
                    Order = groupOrderBase + 50,
                    Group = group,
                    Value = false,
                    EnglishName = "Show module arrows",
                    Description = "Check to show the arrows in the module title to move modules."
                };
                baseSettings.Add("SITESETTINGS_SHOW_MODULE_ARROWS", showModuleArrows);

                // BOWEN 11 June 2005
                // Use Recycler Module for deleted modules
                var useRecycler = new SettingItem<bool, CheckBox>
                {
                    Order = groupOrderBase + 55,
                    Group = group,
                    Value = true,
                    EnglishName = "Use Recycle Bin for Deleted Modules",
                    Description =
                            "Check to make deleted modules go to the recycler instead of permanently deleting them."
                };
                baseSettings.Add("SITESETTINGS_USE_RECYCLER", useRecycler);

                var detailErrorMessage = new SettingItem<bool, CheckBox>
                {
                    Order = groupOrderBase + 56,
                    Group = group,
                    Value = false,
                    EnglishName = "Show Detail Error Message",
                    Description =
                        "Check to show Full detail Error Message when showing error."
                };
                baseSettings.Add("DETAIL_ERROR_MESSAGE", detailErrorMessage);

                // BOWEN 11 June 2005
                #endregion

                // Fix: Jes1111 - 27-02-2005 - incorrect setting for cache dependency
                // CacheDependency settingDependancies = new CacheDependency(null, new string[]{Appleseed.Framework.Settings.Cache.Key.ThemeList(ThemeManager.Path)});
                // set up a cache dependency object which monitors the four folders we are interested in
                var settingDependencies =
                    new CacheDependency(
                        new[]
                            {
                                LayoutManager.Path, layoutManager.PortalLayoutPath, ThemeManager.Path,
                                themeManager.PortalThemePath
                            });

                using (settingDependencies)
                {
                    CurrentCache.Insert(Key.PortalBaseSettings(), baseSettings, settingDependencies);
                }
            }
            else
            {
                baseSettings = (Dictionary<string, ISettingItem>)CurrentCache.Get(Key.PortalBaseSettings());
            }

            return baseSettings;
        }

        /// <summary>
        /// The PortalSettings.GetPortalSettings Method returns a hashtable of
        ///   custom Portal specific settings from the database. This method is
        ///   used by Portals to access misc settings.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="baseSettings">
        /// The base settings.
        /// </param>
        /// <returns>
        /// The hash table.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static Dictionary<string, ISettingItem> GetPortalCustomSettings(
            int portalId, Dictionary<string, ISettingItem> baseSettings)
        {
            if (!CurrentCache.Exists(Key.PortalSettings()))
            {
                // Get Settings for this Portal from the database
                var settings = new Hashtable();

                // Create Instance of Connection and Command Object
                using (var connection = Config.SqlConnectionString)
                using (var command = new SqlCommand("rb_GetPortalCustomSettings", connection))
                {
                    // Mark the Command as a SPROC
                    command.CommandType = CommandType.StoredProcedure;

                    // Add Parameters to SPROC
                    var parameterportalId = new SqlParameter(StringsAtPortalId, SqlDbType.Int, 4) { Value = portalId };
                    command.Parameters.Add(parameterportalId);

                    // Execute the command
                    connection.Open();
                    var dr = command.ExecuteReader(CommandBehavior.CloseConnection);

                    try
                    {
                        while (dr.Read())
                        {
                            settings[dr["SettingName"].ToString()] = dr["SettingValue"].ToString();
                        }
                    }
                    finally
                    {
                        dr.Close(); // by Manu, fixed bug 807858
                    }
                }

                foreach (var key in
                    baseSettings.Keys.Where(key => settings[key] != null).Where(
                        key => settings[key].ToString().Length != 0))
                {
                    baseSettings[key].Value = settings[key];
                }

                // Fix: Jes1111 - 27-02-2005 - change to make PortalSettings cache item dependent on PortalBaseSettings
                // Appleseed.Framework.Settings.Cache.CurrentCache.Insert(Appleseed.Framework.Settings.Cache.Key.PortalSettings(), _baseSettings);
                var settingDependencies = new CacheDependency(null, new[] { Key.PortalBaseSettings() });

                using (settingDependencies)
                {
                    CurrentCache.Insert(Key.PortalSettings(), baseSettings, settingDependencies);
                }
            }
            else
            {
                baseSettings = (Dictionary<string, ISettingItem>)CurrentCache.Get(Key.PortalSettings());
            }

            return baseSettings;
        }

        /// <summary>
        /// Get the proxy parameters as configured in web.config by Phillo 22/01/2003
        /// </summary>
        /// <returns>
        /// The web proxy.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static WebProxy GetProxy()
        {
            // jes1111 - if(ConfigurationSettings.AppSettings["ProxyServer"].Length > 0) 
            var webProxy = new WebProxy { Address = new Uri(Config.ProxyServer) };
            var credentials = new NetworkCredential
            {
                Domain = Config.ProxyDomain,
                UserName = Config.ProxyUserID,
                Password = Config.ProxyPassword
            };
            webProxy.Credentials = credentials;
            return webProxy;

            // } 

            // else 
            // { 
            // return(null); 
            // } 
        }

        /// <summary>
        /// The get tab root should get the first level tab:
        ///   <pre>
        ///     + Root
        ///     + Page1
        ///     + SubPage1		-&gt; returns Page1
        ///     + Page2
        ///     + SubPage2		-&gt; returns Page2
        ///     + SubPage2.1 -&gt; returns Page2
        ///   </pre>
        /// </summary>
        /// <param name="parentPageId">
        /// The parent page ID.
        /// </param>
        /// <param name="tabList">
        /// The tab list.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static PageStripDetails GetRootPage(int parentPageId, List<PageStripDetails> tabList)
        {
            // Changes Indah Fuldner 25.04.2003 (With assumtion that the rootlevel tab has ParentPageID = 0)
            // Search for the root tab in current array
            PageStripDetails rootPage;

            for (var i = 0; i < tabList.Count; i++)
            {
                rootPage = tabList[i];

                // return rootPage;
                if (rootPage.PageID != parentPageId)
                {
                    continue;
                }

                parentPageId = rootPage.ParentPageID;

                //// string parentName=rootPage.PageName;
                if (parentPageId != 0)
                {
                    i = -1;
                }
                else
                {
                    return rootPage;
                }
            }

            // End Indah Fuldner
            throw new ArgumentOutOfRangeException("parentPageId", "Root not found");
        }

        /// <summary>
        /// The GetRootPage should get the first level tab:
        ///   <pre>
        ///     + Root
        ///     + Page1
        ///     + SubPage1   -&gt; returns Page1
        ///     + Page2
        ///     + SubPage2   -&gt; returns Page2
        ///     + SubPage2.1 -&gt; returns Page2
        ///   </pre>
        /// </summary>
        /// <param name="tab">
        /// The tab.
        /// </param>
        /// <param name="tabList">
        /// The tab list.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static PageStripDetails GetRootPage(PageSettings tab, List<PageStripDetails> tabList)
        {
            return GetRootPage(tab.PageID, tabList);
        }

        /// <summary>
        /// Get resource
        /// </summary>
        /// <param name="resourceId">
        /// The resource ID.
        /// </param>
        /// <returns>
        /// The get string resource.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static string GetStringResource(string resourceId)
        {
            // TODO: Maybe this is doing something else?
            return General.GetString(resourceId);
        }

        /// <summary>
        /// Get resource
        /// </summary>
        /// <param name="resourceId">
        /// The resource ID.
        /// </param>
        /// <param name="localize">
        /// The localize.
        /// </param>
        /// <returns>
        /// The get string resource.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static string GetStringResource(string resourceId, string[] localize)
        {
            var res = General.GetString(resourceId);

            for (var i = 0; i <= localize.GetUpperBound(0); i++)
            {
                var thisparam = string.Format("%{0}%", i);
                res = res.Replace(thisparam, General.GetString(localize[i]));
            }

            return res;
        }

        /// <summary>
        /// The UpdatePortalSetting Method updates a single module setting
        ///   in the PortalSettings database table.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="key">
        /// The setting key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void UpdatePortalSetting(int portalId, string key, string value)
        {
            // Create Instance of Connection and Command Object
            using (var connection = Config.SqlConnectionString)
            using (var command = new SqlCommand("rb_UpdatePortalSetting", connection))
            {
                // Mark the Command as a SPROC
                command.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                var parameterportalId = new SqlParameter(StringsAtPortalId, SqlDbType.Int, 4) { Value = portalId };
                command.Parameters.Add(parameterportalId);
                var parameterKey = new SqlParameter("@SettingName", SqlDbType.NVarChar, 50) { Value = key };
                command.Parameters.Add(parameterKey);
                var parameterValue = new SqlParameter("@SettingValue", SqlDbType.NVarChar, 1500) { Value = value };
                command.Parameters.Add(parameterValue);

                // Execute the command
                connection.Open();

                try
                {
                    command.ExecuteNonQuery();
                }
                finally
                {
                }
            }

            CurrentCache.Remove(Key.PortalSettings());
        }

        /// <summary>
        /// Theme definition and images
        /// </summary>
        /// <returns>
        /// The theme.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public Theme GetCurrentTheme()
        {
            // look for an custom theme
            if (this.ActivePage.CustomSettings[StringsCustomTheme] != null &&
                this.ActivePage.CustomSettings[StringsCustomTheme].ToString().Length > 0)
            {
                var customTheme = this.ActivePage.CustomSettings[StringsCustomTheme].ToString().Trim();
                var themeManager = new ThemeManager(this.PortalPath);
                themeManager.Load(customTheme);
                return themeManager.CurrentTheme;
            }

            // no custom theme
            return this.CurrentThemeDefault;
        }

        /// <summary>
        /// Gets the current theme.
        /// </summary>
        /// <param name="requiredTheme">
        /// The required theme.
        /// </param>
        /// <returns>
        /// The theme.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public Theme GetCurrentTheme(string requiredTheme)
        {
            switch (requiredTheme)
            {
                case "Alt":

                    // look for an alternate custom theme
                    if (this.ActivePage.CustomSettings["CustomThemeAlt"] != null &&
                        this.ActivePage.CustomSettings["CustomThemeAlt"].ToString().Length > 0)
                    {
                        var customTheme = this.ActivePage.CustomSettings["CustomThemeAlt"].ToString().Trim();
                        var themeManager = new ThemeManager(this.PortalPath);
                        themeManager.Load(customTheme);
                        return themeManager.CurrentTheme;
                    }

                    // no custom theme
                    return this.CurrentThemeAlt;
                default:

                    // look for an custom theme
                    if (this.ActivePage.CustomSettings[StringsCustomTheme] != null &&
                        this.ActivePage.CustomSettings[StringsCustomTheme].ToString().Length > 0)
                    {
                        var customTheme = this.ActivePage.CustomSettings[StringsCustomTheme].ToString().Trim();
                        var themeManager = new ThemeManager(this.PortalPath);
                        themeManager.Load(customTheme);
                        return themeManager.CurrentTheme;
                    }

                    // no custom theme
                    return this.CurrentThemeDefault;
            }
        }

        /// <summary>
        /// Get languages list from Portaldb
        /// </summary>
        /// <returns>
        /// The get language list.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public string GetLanguageList()
        {
            return GetLanguageList(this.PortalAlias);
        }

        // Get the value of Enable portal friendly URL value 
        //value if either true/false
        /// <summary>
        /// Eanble page friendly URL
        /// </summary>
        public bool EnablePageFriendlyUrl
        {
            get
            {
                try
                {
                    // return the true / false value
                    return Convert.ToBoolean(this.CustomSettings["ENABLE_PAGE_FRIENDLY_URL"].Value);
                }
                catch { }

                // If nothing then it's return false
                return false;
            }
        }
        public bool EnabledPrivateSite
        {
            get
            {
                try
                {
                    // return the true / false value
                    return Convert.ToBoolean(this.CustomSettings["ENABLE_PRIVATE_SITE"].Value);
                }
                catch { }

                // If nothing then it's return false
                return false;
            }
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
            UpdatePortalSetting(this.PortalID, settingItem.EnglishName, Convert.ToString(settingItem.Value));
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Get languages list from Portaldb
        /// </summary>
        /// <param name="portalAlias">
        /// The portal alias.
        /// </param>
        /// <returns>
        /// The get language list.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private static string GetLanguageList(string portalAlias)
        {
            var langlist = string.Empty;
            var defaultlang = string.Empty;

            if (!CurrentCache.Exists(Key.LanguageList()))
            {
                // Create Instance of Connection and Command Object
                using (var connection = Config.SqlConnectionString)
                {
                    // Open the database connection and execute the command
                    connection.Open();

                    using (var command = new SqlCommand("rb_GetPortalSetting", connection))
                    {
                        // Mark the Command as a SPROC
                        command.CommandType = CommandType.StoredProcedure;

                        // Add Parameters to SPROC
                        // Specify the Portal Alias Dynamically 
                        var parameterPortalAlias = new SqlParameter("@PortalAlias", SqlDbType.NVarChar, 50)
                        {
                            Value = portalAlias
                        };
                        command.Parameters.Add(parameterPortalAlias);

                        // Specify the SettingName 
                        var parameterSettingName = new SqlParameter("@SettingName", SqlDbType.NVarChar, 50)
                        {
                            Value = "SITESETTINGS_LANGLIST"
                        };
                        command.Parameters.Add(parameterSettingName);

                        try
                        {
                            // Better null check here by Manu
                            var tmp = command.ExecuteScalar();

                            if (tmp != null)
                            {
                                langlist = tmp.ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            // Appleseed.Framework.Helpers.LogHelper.Logger.Log(Appleseed.Framework.Configuration.LogLevel.Warn, "Get languages from db", ex);
                            ErrorHandler.Publish(LogLevel.Warn, "Failed to get languages from database.", ex);

                            // Jes1111
                        }
                        finally
                        {
                        }
                    }

                    using (var command = new SqlCommand("rb_GetPortalSetting", connection))
                    {
                        // Mark the Command as a SPROC
                        command.CommandType = CommandType.StoredProcedure;

                        // Add Parameters to SPROC
                        var parameterPortalAlias = new SqlParameter("@PortalAlias", SqlDbType.NVarChar, 50)
                        {
                            Value = portalAlias
                        };
                        command.Parameters.Add(parameterPortalAlias);
                        var parameterSettingName = new SqlParameter("@SettingName", SqlDbType.NVarChar, 50)
                        {
                            Value = "SITESETTINGS_DEFAULTLANG"
                        };
                        command.Parameters.Add(parameterSettingName);

                        try
                        {
                            // Better null check here by Manu
                            var tmp = command.ExecuteScalar();

                            if (tmp != null)
                            {
                                defaultlang = tmp.ToString().Trim();
                            }
                        }
                        catch (Exception ex)
                        {
                            // Appleseed.Framework.Helpers.LogHelper.Logger.Log(Appleseed.Framework.Configuration.LogLevel.Warn, "Get languages from db", ex);
                            ErrorHandler.Publish(LogLevel.Warn, "Failed to get default language from database.", ex);

                            // Jes1111
                        }
                        finally
                        {
                        }
                    }
                }

                if (langlist.Length == 0 && defaultlang.Length == 0)
                {
                    // jes1111 - langlist = ConfigurationSettings.AppSettings["DefaultLanguage"]; //default
                    langlist = Config.DefaultLanguage; // default
                }
                else
                {
                    var sb = new StringBuilder();
                    sb.Append(defaultlang).Append(";"); // Add default lang as first item in lang list

                    foreach (var lang in langlist.Split(";".ToCharArray()))
                    {
                        var trimLang = lang.Trim();
                        if (trimLang.Length != 0 && trimLang != defaultlang)
                        {
                            // add non empty and non default languages in list
                            sb.Append(trimLang).Append(";");
                        }
                    }

                    langlist = sb.ToString();
                }

                CurrentCache.Insert(Key.LanguageList(), langlist);
            }
            else
            {
                langlist = (string)CurrentCache.Get(Key.LanguageList());
            }

            return langlist;
        }

        /// <summary>
        /// Sets the active module cookie.
        /// </summary>
        /// <param name="mId">
        /// The module ID.
        /// </param>
        /// <remarks>
        /// </remarks>
        private static void SetActiveModuleCookie(int mId)
        {
            var cookie = new HttpCookie("ActiveModule", mId.ToString());
            var time = DateTime.Now;
            var span = new TimeSpan(0, 2, 0, 0, 0);
            cookie.Expires = time.Add(span);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// Recurses the portal pages XML.
        /// </summary>
        /// <param name="page">
        /// My page details.
        /// </param>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void RecursePortalPagesXml(PageStripDetails page, XmlWriter writer)
        {
            var children = page.Pages;
            var groupElementWritten = false;

            foreach (var mysubPage in children.Where(mysubPage => mysubPage.ParentPageID == page.PageID))
            {
                // if ( mySubPage.ParentPageID == page.PageID && PortalSecurity.IsInRoles(page.AuthorizedRoles) )
                if (!groupElementWritten)
                {
                    writer.WriteStartElement("MenuGroup"); // start MenuGroup element
                    groupElementWritten = true;
                }

                writer.WriteStartElement("MenuItem"); // start MenuItem element
                writer.WriteAttributeString("ParentPageId", mysubPage.ParentPageID.ToString());

                // writer.WriteAttributeString("Label",mySubPage.PageName);
                writer.WriteAttributeString(
                    "UrlPageName",
                    HttpUrlBuilder.UrlPageName(mysubPage.PageID) == HttpUrlBuilder.DefaultPage
                        ? mysubPage.PageName
                        : HttpUrlBuilder.UrlPageName(mysubPage.PageID).Replace(".aspx", string.Empty));

                writer.WriteAttributeString("PageName", mysubPage.PageName);

                writer.WriteAttributeString("PageOrder", mysubPage.PageOrder.ToString());
                writer.WriteAttributeString("PageIndex", mysubPage.PageIndex.ToString());
                writer.WriteAttributeString("PageLayout", mysubPage.PageLayout);
                writer.WriteAttributeString("AuthRoles", mysubPage.AuthorizedRoles);
                writer.WriteAttributeString("ID", mysubPage.PageID.ToString());

                // writer.WriteAttributeString("URL",HttpUrlBuilder.BuildUrl(string.Concat("~/",mySubPage.PageName,".aspx"), mySubPage.PageID,0,null,string.Empty,this.PortalAlias,"hello/goodbye"));
                this.RecursePortalPagesXml(mysubPage, writer);
                writer.WriteEndElement(); // end MenuItem element
            }

            if (groupElementWritten)
            {
                writer.WriteEndElement(); // end MenuGroup element
            }
        }

        private static string GetPortalSettingsCacheKeyPrefix()
        {
            return "PortalSettingsCacheKey";
        }

        private static string GetPortalSettingsCacheKey(int pageId, string portalAlias, string currentLanguage)
        {
            return string.Format("{0}_{1}", GetPortalSettingsCacheKey(pageId, portalAlias), currentLanguage);
        }

        private static string GetPortalSettingsCacheKey(int pageId, string portalAlias)
        {
            return String.Format("{0}_{1}_{2}", GetPortalSettingsCacheKeyPrefix(), portalAlias.ToLower(), pageId);
        }

        private static void ProcessCurrentLanguage(string portalAlias)
        {
            // Changes culture/language according to settings
            try
            {
                // Moved here for support db call
                LanguageSwitcher.ProcessCultures(GetLanguageList(portalAlias), portalAlias);
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Warn, "Failed to load languages, loading defaults.", ex); // Jes1111
                LanguageSwitcher.ProcessCultures(Localization.LanguageSwitcher.LANGUAGE_DEFAULT, portalAlias);
            }
        }

        private static string GetPortalSettingsCacheKey(int portalId)
        {
            return String.Format("{0}PortalId_{1}", GetPortalSettingsCacheKeyPrefix(), portalId);
        }

        private static void AddToCache(string key, PortalSettings portalSettings)
        {
            var cache = HttpRuntime.Cache;
            int time;
            try
            {
                time = int.Parse(ConfigurationManager.AppSettings["PortalSettingsCacheTime"]);
            }
            catch (Exception)
            {
                time = 0;
            }
            if (time > 0 && cache.Get(key) == null)
            {
                cache.Add(key, portalSettings, null, DateTime.Now.AddMinutes(time), TimeSpan.Zero, CacheItemPriority.Normal, null);
            }
        }

        /// <summary>
        /// Remove portal settings cache
        /// </summary>
        /// <param name="pageId">page id</param>
        /// <param name="portalAlias">portal alias</param>
        public static void RemovePortalSettingsCache(int pageId, string portalAlias)
        {
            RemoveCahedItems(GetPortalSettingsCacheKey(pageId, portalAlias));
        }

        /// <summary>
        /// Remove portal settings cache
        /// </summary>
        private static void RemovePortalSettingsCache()
        {
            RemoveCahedItems(GetPortalSettingsCacheKeyPrefix());
        }

        /// <summary>
        /// Remove cahedItems
        /// </summary>
        /// <param name="keyPart">keypart</param>
        private static void RemoveCahedItems(string keyPart)
        {
            int time;
            try
            {
                time = int.Parse(ConfigurationManager.AppSettings["PortalSettingsCacheTime"]);
            }
            catch (Exception)
            {
                time = 0;
            }
            if (time <= 0) return;
            var cache = HttpRuntime.Cache;
            var en = cache.GetEnumerator();
            while (en.MoveNext())
            {
                if (en.Key.ToString().StartsWith(keyPart))
                {
                    cache.Remove(en.Key.ToString());
                }
            }
        }

        /// <summary>
        /// Update Portal Setting Parent Page Cache
        /// </summary>
        /// <param name="currentPageId">Current Page Id</param>
        /// <param name="previousPageId">Previous page Id</param>
        public static void UpdatePortalSettingParentPageCache(int currentPageId, int previousPageId)
        {
            var portalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            var currentPage = portalSettings.DesktopPages.First(pg => pg.PageID == previousPageId);
            currentPage.ParentPageID = currentPageId;
            HttpContext.Current.Items["PortalSettings"] = portalSettings;
        }

        #endregion
    }
}