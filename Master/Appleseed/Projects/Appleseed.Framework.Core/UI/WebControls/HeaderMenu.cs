using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Appleseed.Framework.Security;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Site.Data;
using System.Globalization;
using System.Threading;
using System.Linq;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// HeaderMenu
    /// </summary>
    [History("jviladiu@portalServices.net", "2004/09/29", "Added link showHelp for show help window")]
    [
        History("ozan@Appleseed.web.tr", "2004/07/02",
            "Added  showTabMan and showTabAdd properties for managing tab and adding tab only one click... ")]
    [
        History("John.Mandia@whitelightsolutions.com", "2003/11/04",
            "Added extra property DataBindOnInit. So you can decide if you wish it to bind automatically or when you call DataBind()"
            )]
    [
        History("John.Mandia@whitelightsolutions.com", "2003/10/25",
            "Added ability to have more control over the menu by adding more settings.")]

    [History("ashish.patel@haptix.biz", "2014/11/19", "Updated code for Logoff confirmation message")]
    public class HeaderMenu : DataList, INamingContainer
    {
        private object innerDataSource = null;

        private bool _showLogon = false;
        private bool _dialogLogon = false;
        private string _dialogLogonControlPath = "~/DesktopModules/CoreModules/SignIn/SignIn.ascx";
        private bool _showSecureLogon = false; // Thierry (Tiptopweb), 5 May 2003: add link to Secure directory
        private bool _showHome = true;
        private bool _showTabMan = true; // Ozan, 2 June 2004: add link for tab management 
        private bool _showRegister = false;
        private bool _showDragNDrop = false;

        // 26 October 2003 john.mandia@whitelightsolutions.com - Start
        private bool _showEditProfile = true;
        private bool _showWelcome = true;
        private bool _showLogOff = true;
        private bool _dataBindOnInit = true;
        private bool _showLanguage = false;
        private bool _showLangString = false;
        private bool _showFlags = false;

        // 26 October 2003 John Mandia - Finish

        private bool _showHelp = false; // José Viladiu, 29 Sep 2004: Add link for show help window

        /// <summary>
        /// If true shows a link to a Help Window
        /// </summary>
        /// <value><c>true</c> if [show help]; otherwise, <c>false</c>.</value>
        [Category("Data"),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(false)
            ]
        public bool ShowHelp
        {
            get { return _showHelp; }
            set { _showHelp = value; }
        }

        /// <summary>
        /// If true and user is not authenticated shows
        /// a logon link in place of logoff
        /// </summary>
        /// <value><c>true</c> if [show logon]; otherwise, <c>false</c>.</value>
        [Category("Data"),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(false)
            ]
        public bool ShowLogon
        {
            get { return _showLogon; }
            set { _showLogon = value; }
        }

        /// <summary>
        /// If true and ShowLogon is also true, when the user clicks the logon link
        /// a dialog will be displayed for the user to logon.
        /// </summary>
        /// <value><c>true</c> if [dialog logon]; otherwise, <c>false</c>.</value>
        [Category("Data"),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(false)
            ]
        public bool DialogLogon
        {
            get { return _dialogLogon; }
            set { _dialogLogon = value; }
        }

        /// <summary>
        /// The path of the ascx control that will be displayed inside the dialog for the user to sign in.
        /// The default is "~/DesktopModules/CoreModules/SignIn/SignIn.ascx".
        /// </summary>
        [Category("Data"),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue("~/DesktopModules/CoreModules/SignIn/SignIn.ascx")
            ]
        public string DialogLogonControlPath
        {
            get { return _dialogLogonControlPath; }
            set { _dialogLogonControlPath = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        [Category("Data"),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(false)
            ]
        public bool ShowRegister
        {
            get { return _showRegister; }
            set { _showRegister = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        [Category("Data"),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(false)
            ]
        public bool ShowLanguages
        {
            get { return _showLanguage; }
            set { _showLanguage = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Category("Data"),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(false)
            ]
        public bool ShowFlags
        {
            get { return _showFlags; }
            set { _showFlags = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Category("Data"),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(false)
            ]
        public bool ShowLangString
        {
            get { return _showLangString; }
            set { _showLangString = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        [Category("Data"),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(false)
            ]
        public bool ShowDragNDrop
        {
            get { return _showDragNDrop; }
            set { _showDragNDrop = value; }
        }


        /// <summary>
        /// If true and user is not authenticated shows
        /// a SECURE logon link in place of logoff
        /// </summary>
        /// <value><c>true</c> if [show secure logon]; otherwise, <c>false</c>.</value>
        [Category("Data"),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(false)
            ]
        public bool ShowSecureLogon
        {
            get { return _showSecureLogon; }
            set { _showSecureLogon = value; }
        }

        /// <summary>
        /// Whether show home link
        /// </summary>
        /// <value><c>true</c> if [show home]; otherwise, <c>false</c>.</value>
        [DefaultValue(true)]
        public bool ShowHome
        {
            get { return _showHome; }
            set { _showHome = value; }
        }

        // 2 June 2004 Ozan
        /// <summary>
        /// Whether show Manage Tab link
        /// </summary>
        /// <value><c>true</c> if [show tab man]; otherwise, <c>false</c>.</value>
        [Category("Data"),
            PersistenceMode(PersistenceMode.Attribute),
            DefaultValue(false)
            ]
        public bool ShowTabMan
        {
            get { return _showTabMan; }
            set { _showTabMan = value; }
        }

        // 26 October 2003 john.mandia@whitelightsolutions.com - Start
        /// <summary>
        /// Whether Edit Profile link
        /// </summary>
        /// <value><c>true</c> if [show edit profile]; otherwise, <c>false</c>.</value>
        [DefaultValue(true)]
        public bool ShowEditProfile
        {
            get { return _showEditProfile; }
            set { _showEditProfile = value; }
        }

        /// <summary>
        /// Whether Welcome Shows
        /// </summary>
        /// <value><c>true</c> if [show welcome]; otherwise, <c>false</c>.</value>
        [DefaultValue(true)]
        public bool ShowWelcome
        {
            get { return _showWelcome; }
            set { _showWelcome = value; }
        }

        /// <summary>
        /// Whether Logoff Link Shows
        /// </summary>
        /// <value><c>true</c> if [show log off]; otherwise, <c>false</c>.</value>
        [DefaultValue(true)]
        public bool ShowLogOff
        {
            get { return _showLogOff; }
            set { _showLogOff = value; }
        }

        /// <summary>
        /// Whether Logoff Link Shows
        /// </summary>
        /// <value><c>true</c> if [data bind on init]; otherwise, <c>false</c>.</value>
        [DefaultValue(true)]
        public bool DataBindOnInit
        {
            get { return _dataBindOnInit; }
            set { _dataBindOnInit = value; }
        }

        // 26 October 2003 John Mandia - Finish

        /// <summary>
        /// HeaderMenu
        /// </summary>
        public HeaderMenu()
        {
            ViewStateMode = System.Web.UI.ViewStateMode.Disabled;
            RepeatDirection = RepeatDirection.Horizontal;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event for the <see cref="T:System.Web.UI.WebControls.DataList"></see> control.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (DataBindOnInit)
            {
                DataBind();
            }
        }

        // Jes1111
        /// <summary>
        /// Builds a help link for the header menu and registers it with the page
        /// </summary>
        /// <returns></returns>
        private string GetHelpLink()
        {


            // Jes1111 - 27/Nov/2004 - simplified help popup scheme (echoes changes in ModuleButton.cs)
            string helpTarget = "AppleseedHelp";
            string popupOptions =
                "toolbar=1,location=0,directories=0,status=0,menubar=1,scrollbars=1,resizable=1,width=600,height=400,screenX=15,screenY=15,top=15,left=15";
            string helpText = General.GetString("HEADER_HELP", "Help");

            StringBuilder sb = new StringBuilder();
            sb.Append("<a href=\"");
            sb.Append(Path.ApplicationRoot);
            sb.Append("/rb_documentation/Viewer.aspx\"	target=\"");
            sb.Append(helpTarget);
            sb.Append("\" ");
            if (Page is Page)
            {
                sb.Append("onclick=\"link_popup(this,'");
                sb.Append(popupOptions);
                sb.Append("');return false;\"");
            }
            sb.Append(" class=\"");
            sb.Append("link-is-popup");
            if (CssClass.Length != 0)
            {
                sb.Append(" ");
                sb.Append(CssClass);
            }
            sb.Append("\">");
            sb.Append(helpText);
            sb.Append("</a>");

            if (Page is Page)
            {
                if (!((Page)Page).ClientScript.IsClientScriptIncludeRegistered("rb-popup"))
                    ((Page)Page).ClientScript.RegisterClientScriptInclude(((Page)Page).GetType(), "rb-popup",
                                                                           Path.ApplicationRoot +
                                                                           "/aspnet_client/popupHelper/popup.js");
            }

            return sb.ToString();
        }


        /// <summary>
        /// Binds the control and all its child controls to the specified data source.
        /// </summary>
        public override void DataBind()
        {
            if (HttpContext.Current != null)
            {
                //Init data
                ArrayList list = new ArrayList();

                // Obtain PortalSettings from Current Context
                PortalSettings PortalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];

                string homeLink = "<a";
                string menuLink;

                // added Class support by Mario Endara <mario@softworks.com.uy> 2004/10/04
                if (CssClass.Length != 0)
                    homeLink = homeLink + " class=\"" + CssClass + "\"";

                homeLink = homeLink + " href='" + HttpUrlBuilder.BuildUrl() + "'>" +
                           General.GetString("Appleseed", "HOME") + "</a>";

                // If user logged in, customize welcome message
                if (HttpContext.Current.Request.IsAuthenticated == true)
                {
                    if (ShowWelcome)
                    {
                        list.Add(PortalSettings.CurrentUser.Identity.Name + "(" + PortalSettings.CurrentUser.Identity.Email + ")");
                    }

                    var dashboardPage = PortalSettings.DesktopPages.FirstOrDefault(p => p.PageName == "Dashabord");
                    if (dashboardPage != null)
                    {
                        list.Add(string.Format("<a href='{0}'>{1}</a>", Appleseed.Framework.HttpUrlBuilder.BuildUrl(dashboardPage.PageID), General.GetString("Dashabord", "Dashabord")));
                    }

                    if (ShowHome)
                    {
                        list.Add(homeLink);
                    }

                    if (ShowHelp)
                    {
                        list.Add(GetHelpLink());
                    }

                    // Added by Mario Endara <mario@softworks.com.uy> (2004/11/06)
                    // Find Tab module to see if the user has add/edit rights
                    ModulesDB modules = new ModulesDB();
                    Guid TabGuid = new Guid("{1C575D94-70FC-4A83-80C3-2087F726CBB3}");
                    // Added by Xu Yiming <ymhsu@ms2.hinet.net> (2004/12/6)
                    // Modify for support Multi or zero Pages Modules in a single portal.
                    bool HasEditPermissionsOnTabs = false;
                    int TabModuleID = 0;

                    //					SqlDataReader result = modules.FindModulesByGuid(PortalSettings.PortalID, TabGuid);
                    //					while(result.Read()) 
                    //					{
                    //						TabModuleID=(int)result["ModuleId"];

                    foreach (ModuleItem m in modules.FindModuleItemsByGuid(PortalSettings.PortalID, TabGuid))
                    {
                        HasEditPermissionsOnTabs = PortalSecurity.HasEditPermissions(m.ID);
                        if (HasEditPermissionsOnTabs)
                        {
                            TabModuleID = m.ID;
                            break;
                        }
                    }

                    if (!HasEditPermissionsOnTabs || !ShowTabMan)
                    {
                        if (UserProfile.HasEditThisPageAccess())
                        {
                            HasEditPermissionsOnTabs = true;
                            this.ShowTabMan = true;
                        }
                    }

                    // If user logged in and has Edit permission in the Tab module, reach tab management just one click
                    if (
                        ((ShowTabMan) && (HasEditPermissionsOnTabs) && UserPagePermissionDB.HasCurrentPageEditPermission()
                        && PortalSettings.ActivePage.ParentPageID != 100 && PortalSettings.ActivePage.PageID != 100)
                        || ((PortalSettings.ActivePage.ParentPageID == 100 || PortalSettings.ActivePage.PageID == 100) && PortalSecurity.IsInRole("Admins"))
                        )
                    {
                        // added by Mario Endara 2004/08/06 so PageLayout can return to this page
                        // added Class support by Mario Endara <mario@softworks.com.uy> 2004/10/04
                        menuLink = "<a";
                        if (CssClass.Length != 0)
                            menuLink = menuLink + " class=\"" + CssClass + "\"";

                        // added mID by Mario Endara <mario@softworks.com.uy> to support security check (2004/11/09)
                        var url = HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Pages/PageLayout.aspx?PageID=") +
                                   PortalSettings.ActivePage.PageID + "&amp;mID=" + TabModuleID.ToString() +
                                   "&amp;Alias=" + PortalSettings.PortalAlias + "&amp;lang=" + PortalSettings.PortalUILanguage +
                                   "&amp;returntabid=" + PortalSettings.ActivePage.PageID;
                        menuLink = menuLink + " href='" + url + "' onclick=\"openInModal('" + url + "','" + General.GetString("HEADER_MANAGE_TAB", "Edit This Page", null) + "');return false;\");>" +
                                   General.GetString("HEADER_MANAGE_TAB", "Edit This Page", null) + "</a>";
                        list.Add(menuLink);
                    }


                    if (
                       ((ShowTabMan) && (HasEditPermissionsOnTabs) && UserPagePermissionDB.HasCurrentPageEditPermission()
                       && PortalSettings.ActivePage.ParentPageID != 100 && PortalSettings.ActivePage.PageID != 100)
                       || ((PortalSettings.ActivePage.ParentPageID == 100 || PortalSettings.ActivePage.PageID == 100) && PortalSecurity.IsInRole("Admins"))
                       )
                    {

                        menuLink = "<a";
                        if (CssClass.Length != 0)
                            menuLink = menuLink + " class=\"" + CssClass + "\"";

                        menuLink = menuLink + " href='javascript:DnD();' id='hypDND'>" + General.GetString("DRAGNDROP", "DragNDrop", null) + "</a>";
                        list.Add(menuLink);
                    }

                    if (ShowEditProfile)
                    {
                        // 19/08/2004 Jonathan Fong
                        // www.gt.com.au
                        if (Context.User.Identity.AuthenticationType == "LDAP")
                        {
                            // added Class support by Mario Endara <mario@softworks.com.uy> 2004/10/04
                            menuLink = "<a";
                            if (CssClass.Length != 0)
                                menuLink = menuLink + " class=\"" + CssClass + "\"";

                            menuLink = menuLink + " href='" +
                                       HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Register/Register.aspx") +
                                       "'>" + "Profile" + "</a>";
                            list.Add(menuLink);
                        }
                        // If user is form add edit user link
                        else if (!(HttpContext.Current.User is WindowsPrincipal))
                        {
                            // added Class support by Mario Endara <mario@softworks.com.uy> 2004/10/04
                            menuLink = "<a";
                            if (CssClass.Length != 0)
                                menuLink = menuLink + " class=\"" + CssClass + "\"";

                            menuLink = menuLink + " href='" +
                                       HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Register/Register.aspx") +
                                       "'>" +
                                       General.GetString("HEADER_EDIT_PROFILE", "Edit Profile", this) + "</a>";
                            list.Add(menuLink);
                        }
                    }

                    // if authentication mode is Cookie, provide a logoff link
                    if (Context.User.Identity.AuthenticationType == "Forms" ||
                        Context.User.Identity.AuthenticationType == "LDAP")
                    {
                        if (ShowLogOff)
                        {
                            // Corrections when ShowSecureLogon is true. jviladiu@portalServices.net (05/07/2004)
                            string href = Context.Request.Url.AbsolutePath;
                            if (ShowSecureLogon && Context.Request.IsSecureConnection)
                            {
                                string auxref = Context.Request.Url.AbsoluteUri;
                                auxref = auxref.Substring(0, auxref.IndexOf(Context.Request.Url.PathAndQuery));
                                href = auxref + href;
                                href = href.Replace("https", "http");
                            }
                            // added Class support by Mario Endara <mario@softworks.com.uy> 2004/10/04
                            menuLink = "<a";
                            if (CssClass.Length != 0)
                                menuLink = menuLink + " class=\"" + CssClass + "\"";

                            // added code for redirection on same page after logged out
                            menuLink = menuLink + " href='javascript:void();' onclick=\"if(confirm('" + General.GetString("LOGOFF_CNF_MSG", "Log Off Confirmation: \\nAre you sure you want to log off?", null) + "')){window.location = '/DesktopModules/CoreModules/Admin/Logoff.aspx?redirecturl=" + href + "';  }else{return false;} \">" + General.GetString("HEADER_LOGOFF", "Logoff", null) + "</a>";



                            list.Add(menuLink);
                        }
                    }
                }
                else
                {
                    if (ShowHome)
                    {
                        list.Add(homeLink);
                    }

                    if (ShowHelp)
                    {
                        list.Add(GetHelpLink());
                    }

                    // if not authenticated and ShowLogon is true, provide a logon link 

                    if (ShowLogon)
                    {
                        // added Class support by Mario Endara <mario@softworks.com.uy> 2004/10/04
                        menuLink = "<a";
                        if (CssClass.Length != 0)
                            menuLink = menuLink + " class=\"" + CssClass + "\"";

                        menuLink += string.Concat(" id=\"", this.ClientID, "_logon_link", "\"");
                        menuLink = menuLink + " href='" + HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Admin/Logon.aspx") +
                                   "'>" + General.GetString("LOGON", "Logon", null) + "</a>";
                        list.Add(menuLink);
                    }

                    var allowNewRegistration = false;
                    if (PortalSettings.CustomSettings["SITESETTINGS_ALLOW_NEW_REGISTRATION"] != null)
                        if (bool.Parse(PortalSettings.CustomSettings["SITESETTINGS_ALLOW_NEW_REGISTRATION"].ToString()))
                            allowNewRegistration = true;

                    if (ShowRegister && allowNewRegistration)
                    {

                        menuLink = "<a";
                        if (CssClass.Length != 0)
                            menuLink = menuLink + " class=\"" + CssClass + "\"";

                        menuLink = menuLink + " href='" + HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Register/Register.aspx") +
                                   "'>" + General.GetString("REGISTER", "Register", null) + "</a>";
                        list.Add(menuLink);
                    }



                    // Thierry (Tiptopweb) 5 May 2003 : Secure Logon to Secure Directory
                    if (ShowSecureLogon)
                    {
                        // Added localized support. jviladiu@portalServices.net (05/07/2004)
                        // added Class support by Mario Endara <mario@softworks.com.uy> 2004/10/04
                        menuLink = "<a";
                        if (CssClass.Length != 0)
                            menuLink = menuLink + " class=\"" + CssClass + "\"";

                        menuLink = menuLink + " href='" + PortalSettings.PortalSecurePath + "/Logon.aspx'>" +
                                   General.GetString("LOGON", "Logon", null) + "</a>";
                        list.Add(menuLink);
                    }
                }

                LanguageSwitcher ls = new LanguageSwitcher();
                Appleseed.Framework.Web.UI.WebControls.LanguageCultureCollection lcc = Appleseed.Framework.Localization.LanguageSwitcher.GetLanguageCultureList();
                if ((ShowLanguages) && (lcc.Count > 1))
                {
                    var mb = new StringBuilder();

                    mb.Append("<a");
                    if (CssClass.Length != 0)
                        mb.AppendFormat(" class=\"{0}\"", CssClass);

                    mb.AppendFormat("id = \"popUpLang\" >");

                    if ((ShowLangString) || (ShowLanguages))
                    {
                        string aux = General.GetString("LANGUAGE", "Language", null);
                        mb.AppendFormat("{0}", aux);
                    }
                    if (ShowFlags)
                    {
                        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                        string dir = HttpUrlBuilder.BuildUrl("~/aspnet_client/flags/flags_" + cultureInfo.ToString() + ".gif");
                        mb.AppendFormat("<img src=\"{0}\" alt=\"\" style=\"left:13px;position:relative\"/>", dir);
                    }
                    mb.Append("</a>");
                    list.Add(mb);
                }

                if (ShowTabMan && PortalSettings.IsAllowInviteMembers())
                {
                    menuLink = "<a";
                    if (CssClass.Length != 0)
                        menuLink = menuLink + " class=\"" + CssClass + "\"";

                    // added mID by Mario Endara <mario@softworks.com.uy> to support security check (2004/11/09)
                    var url = "/ASMemberInvite/MemberInvite/RenderView?pid=" +
                               PortalSettings.ActivePage.PageID;
                    menuLink = menuLink + " href='" + url + "' onclick=\"openInModal('" + url + "','" + General.GetString("HEADER_INVITE_MEMBERS", "Invite Members", null) + "');return false;\");>" +
                               General.GetString("HEADER_INVITE_MEMBERS", "Invite Members", null) + "</a>";
                    list.Add(menuLink);
                }
                innerDataSource = list;
            }
            base.DataBind();
            if (ShowLogon && DialogLogon)
            {
                //this new list control won't appear in the list, since it has no DataItem. However we need it for "holding" the Signin Control.
                var newItem = new DataListItem(this.Controls.Count, ListItemType.Item);
                this.Controls.Add(newItem);

                var logonDialogPlaceHolder = new PlaceHolder();
                newItem.Controls.Add(logonDialogPlaceHolder);

                if (_logonControl == null) //we ask this in case someone call the Databind more than once.
                {
                    _logonControl = Page.LoadControl(DialogLogonControlPath);
                    _logonControl.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                }
                logonDialogPlaceHolder.Controls.Add(_logonControl);
            }

        }

        //we added this private attribute for store a reference to the signin control. 
        //Its assigned during the binding (and the con
        private Control _logonControl = null;

        /// <summary>
        /// DataSource
        /// </summary>
        /// <value></value>
        /// <returns>An <see cref="T:System.Collections.IEnumerable"></see> or <see cref="T:System.ComponentModel.IListSource"></see> that contains a collection of values used to supply data to this control. The default value is null.</returns>
        /// <exception cref="T:System.Web.HttpException">The data source cannot be resolved because a value is specified for both the <see cref="P:System.Web.UI.WebControls.BaseDataList.DataSource"></see> property and the <see cref="P:System.Web.UI.WebControls.BaseDataList.DataSourceID"></see> property. </exception>
        public override object DataSource
        {
            get { return innerDataSource; }
            set { innerDataSource = value; }
        }


        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that contains the output stream to render on the client.</param>
        /// <remarks></remarks>
        protected override void Render(HtmlTextWriter writer)
        {
            if (ShowLogon && DialogLogon && _logonControl != null)
            {
                PortalSettings PortalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
                string iframewidth = "280px";
                string dialogwidth = "320";
                string iframeheight = "385px";
                string dialogheightdiv = "390";
                string dialogheight = "420";
                if (PortalSettings.CustomSettings["SITESETTINGS_LOGIN_TYPE"].ToString().Contains("signinloginview.ascx"))
                {
                    iframeheight = "250px";
                    dialogheightdiv = "260";
                    dialogheight = "300";
                }
                else if (PortalSettings.CustomSettings["SITESETTINGS_LOGIN_TYPE"].ToString().Contains("/signin.ascx"))
                {
                    iframeheight = "280px";
                    dialogheightdiv = "290";
                    dialogheight = "330";
                }
                else if (PortalSettings.CustomSettings["SITESETTINGS_LOGIN_TYPE"].ToString().Contains("both.ascx"))
                {
                    if ((PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_TWITTER_APP_ID") &&
                        PortalSettings.CustomSettings["SITESETTINGS_TWITTER_APP_ID"].ToString().Equals(string.Empty) ||
                        PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_TWITTER_APP_SECRET") &&
                        PortalSettings.CustomSettings["SITESETTINGS_TWITTER_APP_SECRET"].ToString().Equals(string.Empty)) &&
                        (PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_FACEBOOK_APP_ID") &&
                        PortalSettings.CustomSettings["SITESETTINGS_FACEBOOK_APP_ID"].ToString().Equals(string.Empty) ||
                        PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_FACEBOOK_APP_SECRET") &&
                        PortalSettings.CustomSettings["SITESETTINGS_FACEBOOK_APP_SECRET"].ToString().Equals(string.Empty)) &&
                        PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_GOOGLE_LOGIN") &&
                        PortalSettings.CustomSettings["SITESETTINGS_GOOGLE_LOGIN"].ToString().Length != 0 &&
                        !bool.Parse(PortalSettings.CustomSettings["SITESETTINGS_GOOGLE_LOGIN"].ToString())
                        )
                    {
                        iframewidth = "250px";
                        dialogwidth = "280";
                        iframeheight = "280px";
                        dialogheightdiv = "290";
                        dialogheight = "330";
                    }
                    else
                    {
                        iframewidth = "550px";
                        dialogwidth = "580";
                        iframeheight = "345px";
                        dialogheightdiv = "370";
                        dialogheight = "400";
                    }
                }
                else if (PortalSettings.CustomSettings["SITESETTINGS_LOGIN_TYPE"].ToString().EndsWith("signinsocialnetwork.ascx"))
                {
                    dialogwidth = "370";
                    iframewidth = "320px";
                    iframeheight = "200px";
                    dialogheightdiv = "240";
                    dialogheight = "260";
                }
                else if (PortalSettings.CustomSettings["SITESETTINGS_LOGIN_TYPE"].ToString().EndsWith("cool.ascx"))
                {
                    iframewidth = "320px";
                    dialogwidth = "350";
                    iframeheight = "250px";
                    dialogheightdiv = "260";
                    dialogheight = "550";
                }
                else if (PortalSettings.CustomSettings["SITESETTINGS_LOGIN_TYPE"].ToString().EndsWith("signinwithsocialnetwork.ascx"))
                {
                    iframeheight = "440px";
                    dialogheightdiv = "445";
                    dialogheight = "500";
                    dialogwidth = "370";
                    iframewidth = "320px";
                }
                else if (PortalSettings.CustomSettings["SITESETTINGS_LOGIN_TYPE"].ToString().EndsWith("signinlink.ascx"))
                {
                    iframeheight = "150px";
                    dialogheightdiv = "165";
                    dialogheight = "200";
                }

                string empty = HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/SignIn/empty.htm");
                string div = this.ClientID + "_logon_dialog";
                var url = HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/SignIn/SignInPage.aspx?iframe=true");
                writer.Write(string.Concat("<div id=\"", this.ClientID, "_logon_dialog\" style=\"display:none\" >"));
                writer.Write(string.Concat("<div id=\"AppleseedLogin\" style=\"height: " + dialogheightdiv + "px !important\" >"));
                writer.Write("<iframe id=\"iframeAppleseedLogin\" src=\"" + empty + "\" onload=\"check()\" width=\"" + iframewidth + "\" height=\"" + iframeheight + "\"></iframe>");
                writer.Write("</div>");






                //_logonControl.RenderControl(writer);
                writer.Write("</div>");
                // writer.Write(string.Concat("<div id=\"", this.ClientID, "_logon_dialog\" style=\"display:none\" >"));
                writer.Write(string.Concat("<div id=\"AppleseedLang\" style=\"display:none\" class=\"appleseedlangclass\" >"));
                writer.Write("</div>");

                writer.Write("<script type=\"text/javascript\">");


                //string ajax = "$.ajax({" +
                //              "url: " + "\"" + url + "\"," +
                //              "cache: false," +
                //              "success: function(data){" +
                //              "$('<div>').html(data).dialog();" +
                //              "}" +
                //              "});";

                writer.Write(string.Concat(@"
                        $(document).ready(function () {
                            $('#iframeAppleseedLogin').attr('src','", empty, @"');
                            $('#AppleseedLogin').dialog({
                                autoOpen: false,
                                modal: true,
                                width: ", dialogwidth, @",
                                height: ", dialogheight, @",
                                resizable: false,
                                title: 'Sign in'
                            });

                            $('#", this.ClientID, @"_logon_link').click(function () {
                                $('#iframeAppleseedLogin').attr('src','", url, @"');
                                
                               

                                $('#AppleseedLogin').dialog('open');
                                

                                
                                return false;    
                            });
                            //this is a hack, we should find another way to know if the dialog should autoopen.
                           
                        });"
                       ));



                writer.Write("\nfunction check(){\n" +
                "$(document).ready(function () {\n" +
                    "$('#iframeAppleseedLogin').ready(function() {\n" +
                        "if(!(document.getElementById('iframeAppleseedLogin').src.match(\"/empty.htm\"))){\n" +
                        "try{\n" +
                        "if(!(document.getElementById(\"iframeAppleseedLogin\").contentWindow.location.href ==" +
                                                              "document.getElementById(\"iframeAppleseedLogin\").src)){\n" +
                                      "window.parent.location = document.getElementById(\"iframeAppleseedLogin\").contentWindow.location.href;\n" +
                                "$('#AppleseedLogin').dialog('close');\n" +
                                "}\n" +
                        "}\n" +
                        "catch (e) {}\n" +
                    "}\n" +
                        "})\n" +
                 "})\n" +
                            "};\n"
                       );

                writer.Write(getStringPopUpLanguages());
                writer.Write("</script>");



            }
            base.Render(writer);
        }

        private string getStringPopUpLanguages()
        {
            string txt = General.GetString("LANGUAGE", "Language", null);
            string dir = HttpUrlBuilder.BuildUrl("~/appleseed.Core/home/lstLanguages");
            string url = "\"" + dir + "\"";
            string post = "\"Post\"";
            return string.Concat(@"
                $(document).ready(function () {
                    $('#AppleseedLang').dialog({
                        autoOpen: false,
                        modal: true,
                        width: 310,
                        height: 300,
                        resizable: false
                    });
                    $('#ui-dialog-title-AppleseedLang').append('", txt, @"');
                    $('#popUpLang').click(function () {
                        $('#AppleseedLang').dialog('open');
                        $.ajax({
                            type:", post, @",
                            url:", url, @",
                            success: function (data){
                                $('#AppleseedLang').html(data)
                            }
                        });
                        return false;    
                    });
                });"
             );
        }
    }
}
