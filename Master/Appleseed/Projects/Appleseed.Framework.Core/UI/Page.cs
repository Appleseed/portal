// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Page.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   TODO: this class needs a better write-up ;-)
//   A template page useful for constructing custom edit pages for module settings.<br />
//   Encapsulates some common code including: module id,
//   PortalSettings and settings, referrer redirection, edit permission,
//   cancel event, etc.
//   This page is a base page.
//   It is named USECURE because no check about security is made.
//   Unsecure page reminds you that you have to do your own security on it.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Resources;
    using System.Text;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    using Appleseed.Framework.Design;
    using Appleseed.Framework.Security;
    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.Site.Data;

    using Path = Appleseed.Framework.Settings.Path;
	using System.Web.Security;
    using Newtonsoft.Json.Linq;
    using System.Data.SqlClient;

    /// <summary>
    /// TODO: this class needs a better write-up ;-)
    ///   A template page useful for constructing custom edit pages for module settings.<br/>
    ///   Encapsulates some common code including: module id,
    ///   PortalSettings and settings, referrer redirection, edit permission,
    ///   cancel event, etc.
    ///   This page is a base page.
    ///   It is named USECURE because no check about security is made.
    ///   Unsecure page reminds you that you have to do your own security on it.
    /// </summary>
    [History("ozan@Appleseed.web.tr", "2005/06/01", "Added new overload for RegisterCSSFile")]
    [History("jminond", "2005/03/10", "Tab to page conversion")]
    [History("Jes1111", "2004/08/18",
        "Extensive changes - new way to build head element, support for multiple CSS style sheets, etc.")]
    [History("jviladiu@portalServices.net", "2004/07/22", "Added Security Access.")]
    [History("John.Mandia@whitelightsolutions.com", "2003/10/11",
        "Added ability for each portal to have it's own custom icon instead of sharing one icon among many.")]
    [History("mario@hartmann.net", "2003/09/08", "Solpart Menu style sheet support added.")]
    [History("Jes1111", "2003/03/04",
        "Smoothed out page event inheritance hierarchy - placed security checks and cache flushing")]
    public class Page : ViewPage
    {
        #region Constants and Fields

        /// <summary>
        ///   Standard cancel button
        /// </summary>
        protected LinkButton CancelButton;

        /// <summary>
        ///   Standard delete button
        /// </summary>
        protected LinkButton DeleteButton;

        /// <summary>
        ///   The master page base page.
        /// </summary>
        protected string MasterpageBasePage = "SiteMaster.master";

        /// <summary>
        ///   Standard update button
        /// </summary>
        protected LinkButton UpdateButton;

        /*
                /// <summary>
                /// The layout base page.
                /// </summary>
                private const string LayoutBasePage = "DesktopDefault.ascx";
        */

        /// <summary>
        ///   Stores any additional Meta entries requested by modules or other code.
        /// </summary>
        private readonly Hashtable additionalMetaElements = new Hashtable(3);

        /// <summary>
        ///   Holds a list of JavaScript function calls which will be output to the body tag as a semicolon-delimited list in the 'onload' attribute
        /// </summary>
        private readonly Hashtable bodyOnLoadList = new Hashtable(3);

        /// <summary>
        ///   The client scripts.
        /// </summary>
        private readonly Hashtable clientScripts = new Hashtable(3);

        /// <summary>
        ///   List of CSS files to be applied to this page
        /// </summary>
        private readonly Hashtable cssFileList = new Hashtable(3);

        /// <summary>
        ///   List of CSS blocks to be applied to this page.
        ///   Strings added to this list will injected into a &lt;style&gt;
        ///   block in the page head.
        /// </summary>
        private readonly Hashtable cssImportList = new Hashtable(3);

        /*
                /// <summary>
                /// The body other attributes.
                /// </summary>
                private string bodyOtherAttributes = string.Empty;
        */

        /// <summary>
        ///   The current theme.
        /// </summary>
        private Theme currentTheme;

        /// <summary>
        ///   The doc type.
        /// </summary>
        private string docType;

        /// <summary>
        ///   The item id.
        /// </summary>
        private int itemId;

        /// <summary>
        ///   The module.
        /// </summary>
        private ModuleSettings module;

        /// <summary>
        ///   The module id.
        /// </summary>
        private int moduleId;

        /// <summary>
        ///   The module settings.
        /// </summary>
        private Dictionary<string, ISettingItem> moduleSettings;

        /// <summary>
        ///   The page settings.
        /// </summary>
        private Dictionary<string, ISettingItem> page;

        /// <summary>
        ///   The page meta description.
        /// </summary>
        private string pageMetaDescription;

        /// <summary>
        ///   The page meta encoding.
        /// </summary>
        private string pageMetaEncoding;

        /// <summary>
        ///   The page meta key words.
        /// </summary>
        private string pageMetaKeyWords;

        /// <summary>
        ///   The page meta other.
        /// </summary>
        private string pageMetaOther;

        /// <summary>
        ///   The set title.
        /// </summary>
        private bool setTitle;

        /// <summary>
        ///   The portal settings.
        /// </summary>
        private PortalSettings settings;

        /// <summary>
        ///   The tab id.
        /// </summary>
        private int tabId;

        /// <summary>
        ///   The user culture.
        /// </summary>
        private string userCulture = "en-us";

        /// <summary>
        ///   The user culture set.
        /// </summary>
        private ResourceSet userCultureSet = null;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Page" /> class. 
        ///   The default constructor initializes all fields to their default values.
        /// </summary>
        public Page()
        {
            this.EnsureChildControls();

            // MVC
            // var wrapper = new HttpContextWrapper(this.Context);

            // var viewContext = new ViewContext { HttpContext = wrapper, ViewData = new ViewDataDictionary() };

            // */************************//
        }

        #endregion

        #region Events

        /// <summary>
        ///   The Add event is defined using the event keyword.
        ///   The type of Add is EventHandler.
        /// </summary>
        public event EventHandler Add;

        /// <summary>
        ///   The Cancel event is defined using the event keyword.
        ///   The type of Cancel is EventHandler.
        /// </summary>
        public event EventHandler Cancel;

        /// <summary>
        ///   The Delete event is defined using the event keyword.
        ///   The type of Delete is EventHandler.
        /// </summary>
        public event EventHandler Delete;

        /// <summary>
        ///   The FlushCache event is defined using the event keyword.
        ///   The type of FlushCache is EventHandler.
        /// </summary>
        public event EventHandler FlushCache;

        /// <summary>
        ///   The Update event is defined using the event keyword.
        ///   The type of Update is EventHandler.
        /// </summary>
        public event EventHandler Update;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the current page theme
        /// </summary>
        /// <value>The current theme.</value>
        public Theme CurrentTheme
        {
            get
            {
                return this.currentTheme ??
                       (this.PortalSettings == null ? null : (this.currentTheme = this.PortalSettings.GetCurrentTheme()));
            }
        }

        /// <summary>
        ///   Gets page DOCTYPE
        /// </summary>
        /// <value>The type of the doc.</value>
        public string DocType
        {
            get
            {
                return this.docType ??
                       (this.PortalSettings == null
                            ? string.Empty
                            : this.docType =
                              this.PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_DOCTYPE") &&
                              this.PortalSettings.CustomSettings["SITESETTINGS_DOCTYPE"].ToString().Trim().Length > 0
                                  ? this.PortalSettings.CustomSettings["SITESETTINGS_DOCTYPE"].ToString()
                                  : string.Empty);
            }
        }

        /// <summary>
        ///   Gets or sets current item id
        /// </summary>
        /// <value>The item ID.</value>
        public int ItemID
        {
            get
            {
                if (this.itemId == 0)
                {
                    // Determine ItemID if specified
                    if (HttpContext.Current != null && this.Request.Params["ItemID"] != null)
                    {
                        this.itemId = Int32.Parse(this.Request.Params["ItemID"]);
                    }
                }

                return this.itemId;
            }

            set
            {
                this.itemId = value;
            }
        }

        /// <summary>
        ///   Gets current module if applicable
        /// </summary>
        /// <value>The module.</value>
        public ModuleSettings Module
        {
            get
            {
                if (this.module == null)
                {
                    if (this.ModuleID <= 0)
                    {
                        // Return null
                        return null;
                    }

                    // Obtain selected module data
                    foreach (var mod in
                        this.PortalSettings.ActivePage.Modules.Cast<ModuleSettings>().Where(
                            mod => mod.ModuleID == this.ModuleID))
                    {
                        this.module = mod;
                        return this.module;
                    }
                }

                return this.module;
            }
        }

        /// <summary>
        ///   Gets current linked module ID if applicable
        /// </summary>
        /// <value>The module ID.</value>
        public int ModuleID
        {
            get
            {
                if (this.moduleId == 0)
                {
                    // Determine ModuleID if specified
                    if (HttpContext.Current != null && this.Request.Params["Mid"] != null)
                    {
                        this.moduleId = Int32.Parse(this.Request.Params["Mid"]);
                    }
                }

                return this.moduleId;
            }
        }

        /// <summary>
        ///   Gets the module settings.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public Dictionary<string, ISettingItem> ModuleSettings
        {
            get
            {
                // Get settings from the database
                // Or provides an empty hash table
                return this.moduleSettings ??
                       (this.moduleSettings =
                        this.ModuleID > 0
                            ? Framework.Site.Configuration.ModuleSettings.GetModuleSettings(this.ModuleID, this)
                            : new Dictionary<string, ISettingItem>());
            }
        }

        /// <summary>
        ///   Gets current linked module ID if applicable
        /// </summary>
        /// <value>The page ID.</value>
        public int PageID
        {
            get
            {
                if (this.tabId == 0)
                {
                    // Determine PageID if specified
                    if (HttpContext.Current != null && this.Request.Params["PageID"] != null)
                    {
                        this.tabId = Int32.Parse(this.Request.Params["PageID"]);
                    }
                    else if (HttpContext.Current != null && this.Request.Params["TabID"] != null)
                    {
                        this.tabId = Int32.Parse(this.Request.Params["TabID"]);
                    }
                }

                return this.tabId;
            }
        }

        /// <summary>
        ///   Gets "description" meta element
        /// </summary>
        /// <value>The page meta description.</value>
        public string PageMetaDescription
        {
            get
            {
                // Try saved view state value
                // tabMetaDescription = (string) ViewState["PageMetaDescription"];
                if (this.pageMetaDescription == null)
                {
                    if (this.PortalSettings == null)
                    {
                        return this.pageMetaDescription = string.Empty;
                    }

                    var tabMetaDescription = this.PortalSettings.ActivePage.CustomSettings["TabMetaDescription"];
                    var metaDescription = this.PortalSettings.CustomSettings["SITESETTINGS_PAGE_META_DESCRIPTION"];
                    return
                        this.pageMetaDescription =
                        HttpContext.Current != null && tabMetaDescription.ToString().Length != 0
                            ? tabMetaDescription.ToString()
                            : (HttpContext.Current != null && metaDescription.ToString().Length != 0
                                   ? metaDescription.ToString()
                                   : string.Empty);

                    // ViewState["PageMetaDescription"] = tabMetaDescription;
                }

                return this.pageMetaDescription;
            }
        }

        /// <summary>
        ///   Gets "encoding" meta element
        /// </summary>
        /// <value>The page meta encoding.</value>
        public string PageMetaEncoding
        {
            get
            {
                // Try saved view state value
                // tabMetaEncoding = (string) ViewState["PageMetaEncoding"];
                if (this.pageMetaEncoding == null)
                {
                    if (this.PortalSettings == null)
                    {
                        return this.pageMetaEncoding = string.Empty;
                    }

                    if (HttpContext.Current != null &&
                        this.PortalSettings.ActivePage.CustomSettings["TabMetaEncoding"].ToString().Length != 0)
                    {
                        this.pageMetaEncoding =
                            this.PortalSettings.ActivePage.CustomSettings["TabMetaEncoding"].ToString();
                    }
                    else if (HttpContext.Current != null &&
                             this.PortalSettings.CustomSettings["SITESETTINGS_PAGE_META_ENCODING"].ToString().Length != 0)
                    {
                        this.pageMetaEncoding =
                            this.PortalSettings.CustomSettings["SITESETTINGS_PAGE_META_ENCODING"].ToString();
                    }
                    else
                    {
                        this.pageMetaEncoding = string.Empty;
                    }

                    // ViewState["PageMetaEncoding"] = tabMetaEncoding;
                }

                return this.pageMetaEncoding;
            }
        }

        /// <summary>
        ///   Gets "keywords" meta element
        /// </summary>
        /// <value>The page meta key words.</value>
        public string PageMetaKeyWords
        {
            get
            {
                return this.pageMetaKeyWords ??
                       (this.pageMetaKeyWords =
                        this.PortalSettings == null
                            ? string.Empty
                            : (HttpContext.Current != null &&
                               this.PortalSettings.ActivePage.CustomSettings["TabMetaKeyWords"].ToString().Length != 0
                                   ? this.PortalSettings.ActivePage.CustomSettings["TabMetaKeyWords"].ToString()
                                   : (HttpContext.Current != null &&
                                      this.PortalSettings.CustomSettings["SITESETTINGS_PAGE_META_KEYWORDS"].ToString().
                                          Length != 0
                                          ? this.PortalSettings.CustomSettings["SITESETTINGS_PAGE_META_KEYWORDS"].
                                                ToString()
                                          : string.Empty)));
            }
        }

        /// <summary>
        ///   Gets the page meta other.
        /// </summary>
        /// <value>The page meta other.</value>
        public string PageMetaOther
        {
            get
            {
                // Try saved viewstate value
                // tabMetaOther = (string) ViewState["PageMetaOther"];
                if (this.pageMetaOther == null)
                {
                    if (this.PortalSettings == null)
                    {
                        return this.pageMetaOther = string.Empty;
                    }

                    if (HttpContext.Current != null &&
                        this.PortalSettings.ActivePage.CustomSettings["TabMetaOther"].ToString().Length != 0)
                    {
                        this.pageMetaOther = this.PortalSettings.ActivePage.CustomSettings["TabMetaOther"].ToString();
                    }
                    else if (HttpContext.Current != null &&
                             this.PortalSettings.CustomSettings["SITESETTINGS_PAGE_META_OTHERS"].ToString().Length != 0)
                    {
                        this.pageMetaOther =
                            this.PortalSettings.CustomSettings["SITESETTINGS_PAGE_META_OTHERS"].ToString();
                    }
                    else
                    {
                        this.pageMetaOther = string.Empty;
                    }

                    // ViewState["PageMetaOther"] = tabMetaOther;
                }

                return this.pageMetaOther;
            }
        }

        /// <summary>
        ///   Gets the page settings.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public Dictionary<string, ISettingItem> PageSettings
        {
            get
            {
                return this.page ?? (this.page = this.PageID > 0
                                                     ? // _Page = Page.GetPageCustomSettings(PageID);
                    // _Page = Page.GetPageCustomSettings(PageID);
                                                 this.PortalSettings.ActivePage.GetPageCustomSettings(this.PageID)
                                                     : // Or provides an empty hash table
                                                 new Dictionary<string, ISettingItem>());
            }
        }

        /// <summary>
        ///   Gets or sets Page Title
        /// </summary>
        /// <value>The page title.</value>
        public string PageTitle
        {
            get
            {
                if (!this.setTitle && (HttpContext.Current != null))
                {
                    // see if we have a value somewhere to put.
                    string tabTitle;

                    if (this.PortalSettings == null)
                    {
                        tabTitle = string.Empty;
                    }
                    else
                    {
                        if (this.PortalSettings.ActivePage.CustomSettings["TabTitle"].ToString().Length != 0)
                        {
                            tabTitle = this.PortalSettings.ActivePage.CustomSettings["TabTitle"].ToString();
                        }
                        else if (this.PortalSettings.CustomSettings["SITESETTINGS_PAGE_TITLE"].ToString().Length != 0)
                        {
                            tabTitle = this.PortalSettings.CustomSettings["SITESETTINGS_PAGE_TITLE"].ToString();
                        }
                        else
                        {
                            tabTitle = this.PortalSettings.PortalTitle;
                        }
                    }

                    if (tabTitle.Length > 0)
                    {
                        this.Title = tabTitle;
                        this.setTitle = true;
                    }
                }

                return this.Title;
            }

            set
            {
                this.Title = value;
                this.setTitle = true;
            }
        }

        /// <summary>
        ///   Gets or sets current portal settings
        /// </summary>
        /// <value>The portal settings.</value>
        public PortalSettings PortalSettings
        {
            get
            {
                if (this.settings == null && HttpContext.Current != null)
                {
                    // TODO: Implement checking for null AFTER getting from context as the context may not contain this when called.
                    this.settings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
                }

                return this.settings;
            }

            set
            {
                this.settings = value;
            }
        }

        /// <summary>
        ///   Gets the user culture.
        /// </summary>
        /// <value>The user culture.</value>
        public string UserCulture
        {
            get
            {
                return this.userCulture;
            }
        }

        /// <summary>
        ///   Gets the user culture set.
        /// </summary>
        /// <value>The user culture set.</value>
        public ResourceSet UserCultureSet
        {
            get
            {
                // TODO: Leverage HttpContext.GetGlobalResourceObject(key, key); ???
                if (this.userCultureSet == null)
                {
                    this.userCulture = Thread.CurrentThread.CurrentCulture.Name;
                }

                return this.userCultureSet;
            }
        }

        /// <summary>
        ///   Gets array of allowed modules.
        /// </summary>
        /// <remarks>
        ///   This array is override for edit and view pages
        ///   with the guids allowed to access.
        ///   jviladiu@portalServices.net (2004/07/22)
        /// </remarks>
        /// <value>The allowed modules.</value>
        protected virtual List<string> AllowedModules
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether IsMasterPageLayout.
        /// </summary>
        protected bool IsMasterPageLayout { get; set; }

        /// <summary>
        ///   Gets or sets Referring URL
        /// </summary>
        /// <value>The URL referrer.</value>
        protected string UrlReferrer
        {
            get
            {
                return this.ViewState["UrlReferrer"] != null
                           ? (string)this.ViewState["UrlReferrer"]
                           : HttpUrlBuilder.BuildUrl();
            }

            set
            {
                this.ViewState["UrlReferrer"] = value;
            }
        }

        #endregion

        // Jes1111
        #region Public Methods

        /// <summary>
        /// Clears registered css files
        /// </summary>
        public void ClearCssFileList()
        {
            this.cssFileList.Clear();
        }

        /// <summary>
        /// Determines whether [is additional meta element registered] [the specified key].
        /// </summary>
        /// <param name="key">
        /// The meta key.
        /// </param>
        /// <returns>
        /// <c>true</c> if [is additional meta element registered] [the specified key]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAdditionalMetaElementRegistered(string key)
        {
            return this.additionalMetaElements.ContainsKey(key.ToLower());
        }

        /// <summary>
        /// Determines whether [is body on load registered] [the specified key].
        /// </summary>
        /// <param name="key">
        /// The body key.
        /// </param>
        /// <returns>
        /// <c>true</c> if [is body on load registered] [the specified key]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsBodyOnLoadRegistered(string key)
        {
            return this.bodyOnLoadList.ContainsKey(key.ToLower());
        }

        /// <summary>
        /// Determines whether [is client script registered] [the specified key].
        /// </summary>
        /// <param name="key">
        /// The script key.
        /// </param>
        /// <returns>
        /// <c>true</c> if [is client script registered] [the specified key]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsClientScriptRegistered(string key)
        {
            return this.clientScripts.ContainsKey(key.ToLower());
        }

        /// <summary>
        /// Determines whether [is CSS file registered] [the specified key].
        /// </summary>
        /// <param name="key">
        /// The css key.
        /// </param>
        /// <returns>
        /// <c>true</c> if [is CSS file registered] [the specified key]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsCssFileRegistered(string key)
        {
            return this.cssFileList.ContainsKey(key.ToLower());
        }

        /// <summary>
        /// Determines whether [is CSS import registered] [the specified key].
        /// </summary>
        /// <param name="key">
        /// The css key.
        /// </param>
        /// <returns>
        /// <c>true</c> if [is CSS import registered] [the specified key]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsCssImportRegistered(string key)
        {
            return this.cssImportList.ContainsKey(key.ToLower());
        }

        /// <summary>
        /// Redirect back to the referring page
        /// </summary>
        public void RedirectBackToReferringPage()
        {
            // Response.Redirect throws a ThreadAbortException to make it work,
            // which is handled by the ASP.NET runtime.
            // By catching an Exception (not a specialized exception, just the
            // base exception class), you end up catching the ThreadAbortException which is
            // always thrown by the Response.Redirect method. Normally, the ASP.NET runtime
            // catches this exception and handles it itself, hence your page never really
            // realized an exception occurred. So by catching this exception, you stop the
            // normal order of events that happen when redirecting.
            try
            {
                this.Response.Redirect(this.UrlReferrer);
            }
            catch (ThreadAbortException)
            {
                // Do nothing it is normal
            }
        }

        /// <summary>
        /// Registers the additional meta element.
        /// </summary>
        /// <param name="key">
        /// The meta key.
        /// </param>
        /// <param name="element">
        /// The element.
        /// </param>
        public void RegisterAdditionalMetaElement(string key, string element)
        {
            this.additionalMetaElements.Add(key.ToLower(), element);
        }

        /// <summary>
        /// Registers the body on load.
        /// </summary>
        /// <param name="key">
        /// The body key.
        /// </param>
        /// <param name="functionCall">
        /// The function call.
        /// </param>
        public void RegisterBodyOnLoad(string key, string functionCall)
        {
            this.bodyOnLoadList.Add(key.ToLower(), functionCall);
        }

        /// <summary>
        /// Registers the client script.
        /// </summary>
        /// <param name="key">
        /// The script key.
        /// </param>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        public void RegisterClientScript(string key, string filePath)
        {
            this.clientScripts.Add(key.ToLower(), filePath);
        }

        /// <summary>
        /// Registers CSS file given path.
        /// </summary>
        /// <param name="key">
        /// The css key.
        /// </param>
        /// <param name="file">
        /// The file path.
        /// </param>
        public void RegisterCssFile(string key, string file)
        {
            this.cssFileList.Add(key.ToLower(), file);
        }

        /// <summary>
        /// Registers CSS file in which current theme folder or Default theme folder
        /// </summary>
        /// <param name="key">
        /// CSS file name
        /// </param>
        public void RegisterCssFile(string key)
        {
            if (this.CurrentTheme == null)
            {
                return;
            }

            var path = string.Format("{0}/{1}.css", this.CurrentTheme.WebPath, key);
            var filePath = string.Format("{0}/{1}.css", this.CurrentTheme.Path, key);
            if (!File.Exists(filePath))
            {
                // jes 11111 - path=ThemeManager.WebPath+"/Default/"+key+".css";
                // filePath=ThemeManager.Path+"/Default/"+key+".css";
                path = string.Format("{0}/Default/{1}.css", ThemeManager.WebPath, key);
                filePath = string.Format("{0}/Default/{1}.css", ThemeManager.Path, key);
                if (!File.Exists(filePath))
                {
                    return;
                }
            }

            this.cssFileList.Add(key.ToLower(), path);
        }

        /// <summary>
        /// Registers the CSS import.
        /// </summary>
        /// <param name="key">
        /// The css key.
        /// </param>
        /// <param name="import">
        /// The import.
        /// </param>
        public void RegisterCssImport(string key, string import)
        {
            this.cssImportList.Add(key.ToLower(), import);
        }

        /// <summary>
        /// Register the correct css module file searching in this order in current theme/mod,
        ///   default theme/mod and in module folder.
        /// </summary>
        /// <param name="folderModuleName">
        /// The name of module directory
        /// </param>
        /// <param name="file">
        /// The Css file
        /// </param>
        public void RegisterCssModule(string folderModuleName, string file)
        {
            if (this.IsCssFileRegistered(file))
            {
                return;
            }

            var cssFile = this.currentTheme.Module_CssFile(file);
            if (cssFile.Equals(string.Empty))
            {
                cssFile = Path.WebPathCombine(Path.ApplicationRoot, "DesktopModules", folderModuleName, file);
                if (!File.Exists(HttpContext.Current.Server.MapPath(cssFile)))
                {
                    cssFile = string.Empty;
                }
            }

            if (!cssFile.Equals(string.Empty))
            {
                this.RegisterCssFile(file, cssFile);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds the HTML &lt;head&gt; element, adding body's onload event listeners
        /// </summary>
        protected virtual void BuildBody()
        {
            var body =
                this.Controls.OfType<HtmlGenericControl>().FirstOrDefault(
                    myControl => myControl.TagName.ToLower() == "body");

            // output onload attribute
            if (this.bodyOnLoadList.Count <= 0)
            {
                return;
            }

            var sb = new StringBuilder();

            foreach (string functionCall in this.bodyOnLoadList.Values)
            {
                sb.Append(functionCall);
            }

            if (body != null)
            {
                body.Attributes["onload"] = sb.ToString();
            }
        }

        /// <summary>
        /// Builds the DOCTYPE statement when requested by the Render() override.
        /// </summary>
        protected virtual void BuildDocType()
        {
            if (string.IsNullOrEmpty(this.DocType) &&
                ((this.CurrentTheme != null && this.CurrentTheme.Type == "zen") ||
                 this.Request.Url.PathAndQuery.IndexOf("Viewer") > 0))
            {
                // this.DocType = Server.HtmlDecode( Config.DefaultDOCTYPE );
            }
        }

        /// <summary>
        /// Builds the HTML &lt;body&gt; element, adding meta tags, stylesheets and client scripts
        /// </summary>
        protected virtual void BuildHead()
        {
            this.Title = this.PageTitle;
            this.Header.Controls.Add(
                new LiteralControl(
                    "<meta name=\"generator\" content=\"Appleseed Portal - see http://www.Appleseedportal.net\"/>\n"));

            if (this.PageMetaKeyWords.Length != 0)
            {
                this.Header.Controls.Add(
                    new LiteralControl(
                        string.Format("<meta name=\"keywords\" content=\"{0}\"/>\n", this.PageMetaKeyWords)));
            }

            if (this.PageMetaDescription.Length != 0)
            {
                this.Header.Controls.Add(
                    new LiteralControl(
                        string.Format("<meta name=\"description\" content=\"{0}\"/>\n", this.PageMetaDescription)));
            }

            if (this.PageMetaEncoding.Length != 0)
            {
                this.Header.Controls.Add(new LiteralControl(this.PageMetaEncoding + "\n"));
            }

            if (this.PageMetaOther.Length != 0)
            {
                this.Header.Controls.Add(new LiteralControl(this.PageMetaOther + "\n"));
            }

            // additional metas (added by code)
            foreach (string metaElement in this.additionalMetaElements.Values)
            {
                this.Header.Controls.Add(new LiteralControl(metaElement + "\n"));
            }

             //ADD THE CSS <LINK> ELEMENT(S)
            if (ConfigurationManager.AppSettings["CSSLoadTop"] == null || !bool.Parse(ConfigurationManager.AppSettings["CSSLoadTop"]))
            {
                foreach (string cssFile in this.cssFileList.Values)
                {
                    this.Header.Controls.Add(
                        new LiteralControl(
                            string.Format("<link rel=\"stylesheet\" href=\"{0}\" type=\"text/css\"/>\n", cssFile)));
                }
            }

            if (this.PortalSettings != null)
            {
                this.Header.Controls.Add(
                    new LiteralControl(
                        string.Format(
                            "<link rel=\"SHORTCUT ICON\" href=\"{0}/portalicon.ico\"/>\n",
                            Path.WebPathCombine(Path.ApplicationRoot, this.PortalSettings.PortalPath))));
            }

            if (this.cssImportList.Count > 0)
            {
                var sb = new StringBuilder();

                sb.AppendLine("<style type=\"text/css\">");
                sb.AppendLine("<!--");
                foreach (string cssBlock in this.cssImportList.Values)
                {
                    sb.AppendLine(cssBlock);
                }

                sb.AppendLine("-->");
                sb.AppendLine("</style>");

                //CSS - custom CSS loaded from the TabMenu - Done by Ashish Patel on 28/05/15
                
                SqlDataReader dr = Appleseed.Framework.Site.Data.TabSettings.CSSDataReader(PortalSettings.ActivePage.PageID, "TabLinkCSS");
                
                while (dr.Read())
                {
                    sb.AppendLine("<style type=\"text/css\">");
                    sb.AppendLine (dr[0].ToString());
                    sb.AppendLine("</style>");
                }

                this.Header.Controls.Add(new LiteralControl(sb + "\n"));
            }

            // ADD CLIENTSCRIPTS 
            foreach (string script in this.clientScripts.Values)
            {
                this.Header.Controls.Add(
                    new LiteralControl(
                        string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>\n", script)));
            }
        }

       /* protected virtual void BuildDefaultCss()
        {
            
            // ADD THE CSS <LINK> ELEMENT(S)
            foreach (string cssFile in this.cssFileList.Values)
            {
                this.Header.Controls.Add(
                    new LiteralControl(
                        string.Format("<link rel=\"stylesheet\" href=\"{0}\" type=\"text/css\"/>\n", cssFile)));
            }

            
        }*/

        /// <summary>
        /// Loads the settings.
        /// </summary>
        protected virtual void LoadSettings()
        {
        }

        /// <summary>
        /// Every guid module in page is set in cookie.
        ///   This method is override in edit &amp; view controls for read the cookie
        ///   and pass or denied access to edit or view module.
        ///   jviladiu@portalServices.net (2004/07/22)
        /// </summary>
        protected virtual void ModuleGuidInCookie()
        {
            var guidsInUse = string.Empty;
            Guid guid;

            var mdb = new ModulesDB();

            if (this.PortalSettings != null && this.PortalSettings.ActivePage.Modules.Count > 0)
            {
                foreach (ModuleSettings ms in this.PortalSettings.ActivePage.Modules)
                {
                    guid = mdb.GetModuleGuid(ms.ModuleID);
                    if (guid != Guid.Empty)
                    {
                        guidsInUse += guid.ToString().ToUpper() + "@";
                    }
                }
            }

            var cookie = new HttpCookie("AppleseedSecurity", guidsInUse);
            var time = DateTime.Now;
            var span = new TimeSpan(0, 2, 0, 0, 0);
            cookie.Expires = time.Add(span);
            this.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// Called when [add].
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected void OnAdd(object source, EventArgs e)
        {
            this.OnAdd(e);
        }

        /// <summary>
        /// Raises the <see cref="Add"/> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnAdd(EventArgs e)
        {
            if (this.Add != null)
            {
                this.Add(this, e); // Invokes the delegates
            }

            // Flush cache
            this.OnFlushCache();

            // Verify that the current user has access to edit this module
            if (PortalSecurity.HasAddPermissions(this.ModuleID) == false)
            {
                PortalSecurity.AccessDeniedEdit();
            }

            // any other code goes here
        }

        /// <summary>
        /// Called when [cancel].
        /// </summary>
        protected virtual void OnCancel()
        {
            this.OnCancel(new EventArgs());
        }

        /// <summary>
        /// Called when [cancel].
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected void OnCancel(object source, EventArgs e)
        {
            this.OnCancel(e);
        }

        /// <summary>
        /// Raises the <see cref="Cancel"/> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnCancel(EventArgs e)
        {
            if (Request.QueryString.GetValues("ModalChangeMaster") == null) {
                if (this.Cancel != null) {
                    this.Cancel(this, e); // Invokes the delegates
                }

                // any other code goes here
                this.RedirectBackToReferringPage();
            } else if (Request.QueryString.GetValues("camefromEditPage") != null)
                this.RedirectBackToReferringPage();
            else
                Response.Write("<script type=\"text/javascript\">window.parent.$('#iframemodal').dialog(\"close\");</script>");
            
        }

        /// <summary>
        /// Called when [delete].
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected void OnDelete(object source, EventArgs e)
        {
            this.OnDelete(e);
        }

        /// <summary>
        /// Handles OnDelete event at Page level<br/>
        ///   Performs OnDelete actions that are common to all Pages<br/>
        ///   Can be overridden
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnDelete(EventArgs e)
        {
            if (this.Delete != null)
            {
                this.Delete(this, e); // Invokes the delegates
            }

            // Flush cache
            this.OnFlushCache();

            // Verify that the current user has access to delete in this module
            if (PortalSecurity.HasDeletePermissions(this.ModuleID) == false)
            {
                PortalSecurity.AccessDeniedEdit();
            }

            // any other code goes here
        }

        /// <summary>
        /// Handles FlushCache event at Page level<br/>
        ///   Performs FlushCache actions that are common to all Pages<br/>
        ///   Can be overridden
        /// </summary>
        protected virtual void OnFlushCache()
        {
            if (this.FlushCache != null)
            {
                this.FlushCache(this, new EventArgs()); // Invokes the delegates
            }

            // remove module output from cache, if it's there
            var sb = new StringBuilder();
            sb.Append("rb_");
            sb.Append(this.PortalSettings.PortalAlias.ToLower());
            sb.Append("_mid");
            sb.Append(this.ModuleID.ToString());
            sb.Append("[");
            sb.Append(this.PortalSettings.PortalContentLanguage);
            sb.Append("+");
            sb.Append(this.PortalSettings.PortalUILanguage);
            sb.Append("+");
            sb.Append(this.PortalSettings.PortalDataFormattingCulture);
            sb.Append("]");

            if (this.Context.Cache[sb.ToString()] != null)
            {
                this.Context.Cache.Remove(sb.ToString());
                Debug.WriteLine("************* Remove " + sb);
            }

            // any other code goes here
        }

        /// <summary>
        /// prerender event
        /// </summary>
        /// <param name="e">an instance of EventArgs</param>
        protected override void OnPreRender(EventArgs e) {

            if(Request.Browser.Browser.ToString().ToLower().Contains("ie"))
                HttpContext.Current.Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\""); 
                //Response.AppendHeader("P3P", "CP=\"CAO PSA OUR\"");
            
            base.OnPreRender(e);
        }

        /// <summary>
        /// Handles the OnInit event at Page level<br/>
        ///   Performs OnInit events that are common to all Pages<br/>
        ///   Can be overridden
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"></see> that contains the event data.
        /// </param>
        protected override void OnInit(EventArgs e)
        {
            bool FacebookSetting = false;
            try{
                if (PageSettings.ContainsKey("FB_LikeGate_Page") &&
                   !string.IsNullOrEmpty(PageSettings["FB_LikeGate_Page"].ToString()) &&
                    int.Parse(PageSettings["FB_LikeGate_Page"].ToString()) != -1
                    )
                    FacebookSetting = true;
            }
            catch(Exception){
                FacebookSetting = false;
            }            
            
            if (FacebookSetting) {
                if (Request.QueryString["signed_request"] != null) {

                    // Decode signed_request value and saved it in session
                    userLikedPage();

                }

                if (Session["FacebookLikeGate"] != null) {

                    bool liked = false;
                    try {
                        liked = (bool)Session["FacebookLikeGate"];
                    }
                    catch (Exception) {
                        liked = false;
                    }
                    if (!liked) {

                        int id = int.Parse(PageSettings["FB_LikeGate_Page"].ToString());
                        string url = HttpUrlBuilder.BuildUrl(id);

                        Response.Redirect(url);
                        //}
                    }
                    
                }
                
            }
            


            this.LoadSettings();

            Control control = null;

            if (this.CancelButton != null || (control = this.GetControl("cancelButton")) != null)
            {
                if (this.CancelButton == null)
                {
                    this.CancelButton = (LinkButton)control;
                }

                this.CancelButton.Click += this.CancelBtnClick;
                this.CancelButton.Text = General.GetString("CANCEL", "Cancel");
                this.CancelButton.CausesValidation = false;
                this.CancelButton.EnableViewState = false;
            }

            if (this.UpdateButton != null || (control = this.GetControl("updateButton")) != null)
            {
                if (this.UpdateButton == null)
                {
                    this.UpdateButton = (LinkButton)control;
                }

                this.UpdateButton.Click += this.UpdateBtnClick;
                this.UpdateButton.Text = General.GetString("APPLY", "Apply", this.UpdateButton);
                this.UpdateButton.EnableViewState = false;
            }

            if (this.DeleteButton != null || (control = this.GetControl("deleteButton")) != null)
            {
                if (this.DeleteButton == null)
                {
                    this.DeleteButton = (LinkButton)control;
                }

                this.DeleteButton.Click += this.DeleteBtnClick;
                this.DeleteButton.Text = General.GetString("DELETE", "Delete", this.DeleteButton);
                this.DeleteButton.EnableViewState = false;

                // Assign current permissions to Delete button
                if (PortalSecurity.HasDeletePermissions(this.ModuleID) == false)
                {
                    this.DeleteButton.Visible = false;
                }
                else
                {
                    if (!this.ClientScript.IsClientScriptBlockRegistered("confirmDelete"))
                    {
                        string[] s = { "CONFIRM_DELETE" };
                        this.ClientScript.RegisterClientScriptBlock(
                            this.GetType(),
                            "confirmDelete",
                            PortalSettings.GetStringResource("CONFIRM_DELETE_SCRIPT", s));
                    }

                    this.DeleteButton.Attributes.Add("OnClick", "return confirmDelete()");
                }
            }

            this.ModuleGuidInCookie();
          
            //// AddThis
            //if (!this.Page.ClientScript.IsStartupScriptRegistered("ADD_THIS"))
            //{
            //    if (this.PortalSettings != null && this.PortalSettings.CustomSettings != null)
            //    {
            //        var addThisUsernameSetting = this.PortalSettings.CustomSettings["SITESETTINGS_ADDTHIS_USERNAME"];
            //        if (addThisUsernameSetting != null)
            //        {
            //            if (Convert.ToString(addThisUsernameSetting).Trim().Length > 0)
            //            {
            //                var addThisUsername = Convert.ToString(addThisUsernameSetting);
            //                this.Page.ClientScript.RegisterClientScriptInclude(
            //                    this.Page.GetType(),
            //                    "ADD_THIS",
            //                    "http://s7.addthis.com/js/250/addthis_widget.js#username=" + addThisUsername);
            //            }
            //        }
            //    }

            //}

            base.OnInit(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> object that contains the event data.
        /// </param>
        protected override void OnLoad(EventArgs e)
        {

            // add CurrentTheme CSS
            if (this.CurrentTheme != null)
            {
                this.RegisterCssFile(this.CurrentTheme.Name, this.CurrentTheme.CssFile);
            }

            this.InsertGlAnalyticsScript();
            this.InsertSnapEngageScript();

            if (this.PortalSettings != null &&
                this.Request.Cookies["Appleseed_" + this.PortalSettings.PortalAlias] != null)
            {
                if (!Config.ForceExpire)
                {
                    // jminond - option to kill cookie after certain time always
                    var minuteAdd = Config.CookieExpire;
                    PortalSecurity.ExtendCookie(this.PortalSettings, minuteAdd);
                }
            }

            // Stores referring URL in viewstate
            if (!this.Page.IsPostBack && this.Request.UrlReferrer != null)
            {
                this.UrlReferrer = this.Request.UrlReferrer.ToString();
            }

            base.OnLoad(e);
        }

        /// <summary>
        /// Raises the <see cref="System.Web.UI.Page.PreInit"/> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected override void OnPreInit(EventArgs e)
        {
            // TODO : Assign masters and themes here... :-)
            //// this.Theme = "Default";
            if (Request.QueryString.GetValues("ModalChangeMaster") == null) {
                if (this.PortalSettings != null) {
                    var masterLayoutPath = string.Concat(this.PortalSettings.PortalLayoutPath, this.MasterpageBasePage);
                    if (HttpContext.Current != null &&
                        (File.Exists(HttpContext.Current.Server.MapPath(masterLayoutPath)) && this.Page.Master != null)) {
                        this.Page.MasterPageFile = masterLayoutPath;
                        this.IsMasterPageLayout = true;
                    }
                }
            } else {
                //TO DO
                //Find the layout master for modals, if it don't exists set the default Modal Master
                this.Page.MasterPageFile = "~/Shared/ModalMaster.Master";
            }
            base.OnPreInit(e);
        }

        /// <summary>
        /// Called when [update].
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnUpdate(object source, EventArgs e)
        {
            this.OnUpdate(e);
        }

        /// <summary>
        /// Raises the <see cref="Update"/> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnUpdate(EventArgs e)
        {
            if (this.Update != null)
            {
                this.Update(this, e); // Invokes the delegates
            }

            // Flush cache
            this.OnFlushCache();

            // Verify that the current user has access to edit this module
            // June 23, 2003: Mark McFarlane made change to check for both Add AND Edit permissions
            // Since UI.Page.EditPage and UI.Page.AddPage both inherit from this UI.Page class
            if (PortalSecurity.HasEditPermissions(this.ModuleID) == false &&
                PortalSecurity.HasAddPermissions(this.ModuleID) == false)
            {
                PortalSecurity.AccessDeniedEdit();
            }

            // any other code goes here
        }

        /// <summary>
        /// Overrides Render() and writes out &lt;html&gt;, &lt;head&gt; and &lt;body&gt; elements along with page contents.
        /// </summary>
        /// <param name="writer">
        /// the HtmlTextWriter connected to the output stream
        /// </param>
        protected override void Render(HtmlTextWriter writer)
        {
            this.BuildDocType();
            //this.BuildDefaultCss();
            this.BuildHead();
            this.BuildBody();
            base.Render(writer);
        }

        /// <summary>
        /// Handles the Click event of the CancelBtn control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void CancelBtnClick(object sender, EventArgs e)
        {
            this.OnCancel(e);
        }

        /// <summary>
        /// Handles the Click event of the DeleteBtn control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void DeleteBtnClick(object sender, EventArgs e)
        {
            this.OnDelete(e);
        }

        /// <summary>
        /// Gets the control.
        /// </summary>
        /// <param name="name">
        /// The name of the control.
        /// </param>
        /// <returns>
        /// A control.
        /// </returns>
        private Control GetControl(string name)
        {
            var control = this.Page.FindControl(name);
            if (control == null)
            {
                try
                {
                    var master = this.Page.Master;
                    while (control == null && master != null)
                    {
                        var container = master.FindControl("Content");
                        if (container != null)
                        {
                            control = container.FindControl(name);
                        }

                        master = master.Master;
                    }
                }
                catch (Exception exc)
                {
                    ErrorHandler.Publish(
                        LogLevel.Warn,
                        string.Format(
                            "Error while trying to get the '{0}' control in Appleseed.Framework.Web.UI.Page.GetControl(controlName).",
                            name),
                        exc);
                }
            }

            return control;
        }

        /// <summary>
        /// Inserts the GoogleAnalytics code if necessary
        /// </summary>
        private void InsertGlAnalyticsScript()
        {
            var useCustVars = false;

            if (this.PortalSettings == null ||
                 !this.PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_GOOGLEANALYTICS") ||
                  this.PortalSettings.CustomSettings["SITESETTINGS_GOOGLEANALYTICS"].ToString().Equals(string.Empty))
            {
                return;
            }

            try
            {
                useCustVars = this.PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_GOOGLEANALYTICS_CUSTOMVARS") &&
                            Convert.ToBoolean(this.PortalSettings.CustomSettings["SITESETTINGS_GOOGLEANALYTICS_CUSTOMVARS"].ToString());
            }
            catch (Exception e)
            {
                ErrorHandler.Publish(LogLevel.Warn, e);
            }


            var script = new StringBuilder();
            
            script.Append("<script type=\"text/javascript\">");
            script.Append("var _gaq = _gaq || [];");
            script.AppendFormat("_gaq.push(['_setAccount', '{0}']);", this.PortalSettings.CustomSettings["SITESETTINGS_GOOGLEANALYTICS"].ToString());
            script.Append("_gaq.push(['_setSiteSpeedSampleRate', 5]);");
            script.Append("_gaq.push(['_trackPageview']);");
            if (Request.IsAuthenticated && useCustVars) {
                var email = Membership.GetUser().Email;
                var index = email.IndexOf('@');

                // Slot 1, visitor-level scope.
                script.Append("_gaq.push(['_setCustomVar', 1, \"User Type\", \"Member\", 1]);");
                
                // Slot 2, session-level scope.
                script.Append("_gaq.push(['_setCustomVar', 2, \"Authenticated\", \"Yes\", 2]);");
                if (index >= 0 && index < email.Length - 1) {
                    // Slot 3, visitor-level scope.
                    script.Append("_gaq.push(['_setCustomVar', 3, \"Domain\", \"" + email.Substring(index + 1).ToLowerInvariant() + "\", 1]);");
                }
            }
            script.Append("(function() {");
            script.Append("var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;");
            script.Append("ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';");
            script.Append("var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);");
            script.Append("})();");
            script.Append("</script>");





            // TODO: Add tracking variables
            // Sets the script in the head
            if(Header != null)
                Header.Controls.Add(new LiteralControl(script.ToString()));
            
        }

        /// <summary>
        /// Handles the Click event of the UpdateBtn control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void UpdateBtnClick(object sender, EventArgs e)
        {
            this.OnUpdate(e);
        }


        private void InsertSnapEngageScript() {

            try {

                if (this.PortalSettings == null ||
                     !this.PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_SNAPENGAGE") ||
                      this.PortalSettings.CustomSettings["SITESETTINGS_SNAPENGAGE"].ToString().Equals(string.Empty)) {
                    return;
                }

                var script = new StringBuilder();

                script.Append("<script type=\"text/javascript\">");
                script.Append("document.write(unescape(\"%3Cscript src='\" + ((document.location.protocol==\"https:\")?\"https://snapabug.appspot.com\":\"http://www.snapengage.com\") + \"/snapabug.js' type='text/javascript'%3E%3C/script%3E\"));</script><script type=\"text/javascript\">");
                script.AppendFormat("SnapABug.setLocale(\"{0}\");", Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToString());
                script.AppendFormat("SnapABug.addButton(\"{0}\",\"1\",\"55%\");", this.PortalSettings.CustomSettings["SITESETTINGS_SNAPENGAGE"].ToString());
                script.Append("</script>");

                //this.ClientScript.RegisterStartupScript(this.GetType(), "SITESETTINGS_SNAPENGAGE", script.ToString());

                LiteralControl c = null;
                var container = Page.Controls[0];
                c = FindClosingBodyLiteral(container);

                if (c == null) {
                    c = FindClosingBodyLiteral(Page);
                }
                if (c != null) {
                    script.Append(c.Text);
                    c.Text = script.ToString();
                }
                else {
                    // if we cant find the closing of body we add this at the end
                    var controlCollection = Page.Controls[0].Controls;
                    var insertAt = controlCollection.Count; //get last control in page
                    //if (insertAt > 2) insertAt = insertAt - 2; // try to skip closing html and body tags
                    if (insertAt == 0) {
                        controlCollection = Page.Controls;
                        insertAt = controlCollection.Count;
                    }
                    controlCollection.AddAt(insertAt, new LiteralControl(script.ToString()));
                }
            }
            catch (Exception e) {
                ErrorHandler.Publish(LogLevel.Error, "Error adding SnapEngage script", e);
            }
        }

        private LiteralControl FindClosingBodyLiteral(Control container) {
            LiteralControl c = null;
            var literals = FindControls<LiteralControl>(container);
            literals.Reverse();

            foreach (LiteralControl ctrl in literals) {

                if (ctrl.Text.Contains("</body>")) {
                    c = ctrl;
                    break;
                }
            }
            return c;
        }

        /// <summary>
        /// find controls 
        /// </summary>
        /// <typeparam name="T">control</typeparam>
        /// <param name="parent">parent</param>
        /// <returns>result</returns>
        public List<T> FindControls<T>(Control parent) where T : Control
        {
            List<T> foundControls = new List<T>();

            FindControls<T>(parent, foundControls);

            return foundControls;
        }

        void FindControls<T>(Control parent, List<T> foundControls) where T : Control
        {
            foreach (Control c in parent.Controls)
            {
                if (c is T)
                    foundControls.Add((T)c);
                else if (c.Controls.Count > 0)
                    FindControls<T>(c, foundControls);                
            }
        }

        #endregion

        #region Facebook_LikeGate

        private void userLikedPage() {


            try {
                
                string payload = Request.QueryString["signed_request"].Split('.')[1];
                var encoding = new UTF8Encoding();
                var decodedJson = payload.Replace("=", string.Empty).Replace('-', '+').Replace('_', '/');
                var base64JsonArray = Convert.FromBase64String(decodedJson.PadRight(decodedJson.Length + (4 - decodedJson.Length % 4) % 4, '='));
                var json = encoding.GetString(base64JsonArray);
                var o = JObject.Parse(json);

                //var lPid = Convert.ToString(o.SelectToken("page.id")).Replace("\"", "");
                var lLiked = Convert.ToString(o.SelectToken("page.liked")).Replace("\"", "");
                //var lUserId = Convert.ToString(o.SelectToken("user.id")).Replace("\"", "");
                bool like = bool.Parse(lLiked.ToString());

                Session.Add("FacebookLikeGate", like);

                
                
            }
            catch (Exception ex) {
                ErrorHandler.Publish(LogLevel.Error, ex);
                
            }
            
        }

        #endregion
    }
}