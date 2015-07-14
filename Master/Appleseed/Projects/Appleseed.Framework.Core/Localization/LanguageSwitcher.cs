// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LanguageSwitcher.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Summary description for LanguageSwitcher.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI.WebControls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Globalization;
    using System.Threading;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Appleseed.Framework.Settings;

    /// <summary>
    /// Summary description for LanguageSwitcher.
    /// </summary>
    [ToolboxData("<{0}:LanguageSwitcher runat='server'></{0}:LanguageSwitcher>")]
    [Designer("Esperantus.Design.WebControls.LanguageSwitcherDesigner")]
    [DefaultProperty("LanguageListString")]
    // [PermissionSet(SecurityAction.LinkDemand, XML="<PermissionSet class=\"System.Security.PermissionSet\"\r\n version=\"1\">\r\n <IPermission class=\"System.Web.AspNetHostingPermission, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\r\n version=\"1\"\r\n Level=\"Minimal\"/>\r\n</PermissionSet>\r\n"), PermissionSet(SecurityAction.InheritanceDemand, XML="<PermissionSet class=\"System.Security.PermissionSet\"\r\n version=\"1\">\r\n <IPermission class=\"System.Web.AspNetHostingPermission, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\r\n version=\"1\"\r\n Level=\"Minimal\"/>\r\n</PermissionSet>\r\n")]
    public class LanguageSwitcher : WebControl, IPostBackEventHandler
    {
        #region Constants and Fields

        /// <summary>
        /// </summary>
        private const string SwitcherCookieName = "languageSwitcher"; // TODO: do not hardcode cookie name

        /// <summary>
        /// </summary>
        private const string SwitcherCookiePrefix = "Esperantus_Language_";

        /// <summary>
        /// The language drop down.
        /// </summary>
        private DropDownList langDropDown;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageSwitcher"/> class. 
        /// The language switcher.
        /// </summary>
        public LanguageSwitcher()
        {
            // Trace.WriteLine("LanguageSwitcher() constructor *****************************************");
            if (this.Context != null && this.Context.Request != null)
            {
                this.ChangeLanguageUrl = this.Context.Request.Path;
            }
        }

        #endregion

        #region Events

        /// <summary>
        ///   The ChangeLanguage event is defined using the event keyword.
        ///   The type of ChangeLanguage is EventHandler.
        /// </summary>
        public event LanguageSwitcherEventHandler ChangeLanguage;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets how language switcher change language.
        /// </summary>
        /// <remarks>
        ///   In LanguageSwitcherAction.LinkRedirect mode 
        ///   NO EVENT is raised because no post back occurs.
        /// </remarks>
        [DefaultValue(LanguageSwitcherAction.PostBack)]
        public LanguageSwitcherAction ChangeLanguageAction
        {
            get
            {
                if (this.ViewState["ChangeLanguageAction"] != null)
                {
                    return (LanguageSwitcherAction)this.ViewState["ChangeLanguageAction"];
                }

                return LanguageSwitcherAction.PostBack;
            }

            set
            {
                this.ViewState["ChangeLanguageAction"] = value;
            }
        }

        /// <summary>
        ///   Gets or sets Url where redirecting language changes.
        ///   An empty value reload current page.
        /// </summary>
        [DefaultValue("")]
        public string ChangeLanguageUrl
        {
            get
            {
                var changeLanguageUrl = (string)this.ViewState["ChangeLanguageUrl"];
                return changeLanguageUrl ?? string.Empty;
            }

            set
            {
                this.ViewState["ChangeLanguageUrl"] = value;
            }
        }

        /// <summary>
        ///   Normally the Language part is shown (UI).
        ///   Choose DisplayCultureList to show the Culture part.
        ///   Choose DisplayNone for hide Flags.
        /// </summary>
        [DefaultValue(LanguageSwitcherDisplay.DisplayCultureList)]
        public LanguageSwitcherDisplay Flags
        {
            get
            {
                return this.ViewState["Flags"] != null
                           ? (LanguageSwitcherDisplay)this.ViewState["Flags"]
                           : LanguageSwitcherDisplay.DisplayCultureList;
            }

            set
            {
                this.ViewState["Flags"] = value;
                this.ChildControlsCreated = false;

                // EnsureChildControls();
            }
        }

        /// <summary>
        ///   Image path
        /// </summary>
        [DefaultValue("images/flags/")]
        public string ImagePath
        {
            get
            {
                var imagePath = (string)this.ViewState["ImagePath"];
                return imagePath ?? "images/flags/";
            }

            set
            {
                this.ViewState["ImagePath"] = value;
            }
        }

        /// <summary>
        ///   Normally the Language part is shown (UI).
        ///   Choose DisplayCultureList to show the Culture part.
        ///   Choose DisplayNone for hide labels.
        /// </summary>
        [DefaultValue(LanguageSwitcherDisplay.DisplayUICultureList)]
        public LanguageSwitcherDisplay Labels
        {
            get
            {
                return this.ViewState["Labels"] != null
                           ? (LanguageSwitcherDisplay)this.ViewState["Labels"]
                           : LanguageSwitcherDisplay.DisplayUICultureList;
            }

            set
            {
                this.ViewState["Labels"] = value;
                this.ChildControlsCreated = false;

                // EnsureChildControls();
            }
        }

        /// <summary>
        /// The language list string.
        /// </summary>
        [DefaultValue("en=en-US;it=it-IT")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string LanguageListString
        {
            get
            {
                return this.ViewState["LanguageList"] != null
                           ? (string)this.ViewState["LanguageList"]
                           : Config.DefaultLanguageList;

                // return strLanguageList;
            }

            set
            {
                // strLanguageList = value;
                this.ViewState["LanguageList"] = value;
                this.ChildControlsCreated = false;

                // EnsureChildControls();
            }
        }

        /// <summary>
        ///   Normally the Language part is shown as Native name (the name in the proper language).
        ///   Set ShowNative to false for showing names in English.
        /// </summary>
        [DefaultValue(LanguageSwitcherName.NativeName)]
        public LanguageSwitcherName ShowNameAs
        {
            get
            {
                return this.ViewState["ShowNameAs"] != null
                           ? (LanguageSwitcherName)this.ViewState["ShowNameAs"]
                           : LanguageSwitcherName.NativeName;
            }

            set
            {
                this.ViewState["ShowNameAs"] = value;
                this.ChildControlsCreated = false;

                // EnsureChildControls();
            }
        }

        /// <summary>
        ///   LanguageSwitcher Type
        /// </summary>
        [DefaultValue(LanguageSwitcherType.DropDownList)]
        public LanguageSwitcherType Type
        {
            get
            {
                return this.ViewState["Type"] != null
                           ? (LanguageSwitcherType)this.ViewState["Type"]
                           : LanguageSwitcherType.DropDownList;
            }

            set
            {
                this.ViewState["Type"] = value;
                this.ChildControlsCreated = false;

                // EnsureChildControls();
            }
        }

        /// <summary>
        /// Gets LanguageList.
        /// </summary>
        private LanguageCultureCollection LanguageList
        {
            get
            {
                return (LanguageCultureCollection)this.LanguageListString;
            }
            
            // set
            // {
            //     Trace.WriteLine("LanguageList");
            //     LanguageListString = (string) value;
            // }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the current language.
        /// </summary>
        /// <returns></returns>
        public static LanguageCultureItem GetCurrentLanguage()
        {
            return new LanguageCultureItem(Thread.CurrentThread.CurrentUICulture, Thread.CurrentThread.CurrentCulture);
        }

        /// <summary>
        /// Examines/combines all the variables involved and sets
        ///   CurrentUICulture and CurrentCulture
        /// </summary>
        /// <param name="langList">
        /// Languages list. Something like it=it-IT;en=en-US
        /// </param>
        public static void ProcessCultures(string langList)
        {
            ProcessCultures(langList, null);
        }

        /// <summary>
        /// Examines/combines all the variables involved and sets
        ///   CurrentUICulture and CurrentCulture
        /// </summary>
        /// <param name="langList">
        /// Languages list. Something like it=it-IT;en=en-US
        /// </param>
        /// <param name="cookieAlias">
        /// Alias used to make this cookie unique. Use null is you do not want cookies.
        /// </param>
        public static void ProcessCultures(string langList, string cookieAlias)
        {
            ProcessCultures(langList, cookieAlias, null);
        }

        /// <summary>
        /// Sets the current language.
        /// </summary>
        /// <param name="langItem">The lang item.</param>
        public static void SetCurrentLanguage(LanguageCultureItem langItem)
        {
            SetCurrentLanguage(langItem);
        }

        /// <summary>
        /// The set current language.
        /// </summary>
        /// <param name="langItem">The lang item.</param>
        /// <param name="cookieAlias">Cookie name used for persist Language</param>
        public static void SetCurrentLanguage(LanguageCultureItem langItem, string cookieAlias)
        {
            SetCurrentLanguage(langItem, cookieAlias, null);
        }

        #endregion

        #region Implemented Interfaces

        #region IPostBackEventHandler

        /// <summary>
        /// Implement the RaisePostBackEvent method
        /// from the IPostBackEventHandler interface.
        /// </summary>
        /// <param name="eventArgument">A <see cref="T:System.String"/> that represents an optional event argument to be passed to the event handler.</param>
        public void RaisePostBackEvent(string eventArgument)
        {
            // Trace.WriteLine("RaisingPostBackEvent: eventArgument = '" + eventArgument + "'");
            if (this.ChangeLanguageAction == LanguageSwitcherAction.LinkRedirect)
            {
                this.Context.Response.Redirect(this.GetLangUrl(eventArgument));
            }
            else
            {
                var myItem = this.LanguageList.GetBestMatching(new CultureInfo(eventArgument));
                this.OnChangeLanguage(new LanguageSwitcherEventArgs(myItem));
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Examines/combines all the variables involved and sets
        ///   CurrentUICulture and CurrentCulture
        /// </summary>
        /// <param name="langList">
        /// Languages list. Something like it=it-IT;en=en-US
        /// </param>
        /// <param name="cookieAlias">
        /// Alias used to make this cookie unique. Use null is you do not want cookies.
        /// </param>
        /// <param name="switcher">
        /// A referenced to a Switcher control for accessing view state
        /// </param>
        internal static void ProcessCultures(string langList, string cookieAlias, LanguageSwitcher switcher)
        {
            var myLanguagesCultureList = (LanguageCultureCollection)langList;

            // Verify that at least on language is provided
            if (myLanguagesCultureList.Count <= 0)
            {
                throw new ArgumentException("Please provide at least one language in the list.", "langList");
            }

            // Language Item
            // Query string
            var langItem = InternalGetQuerystring(myLanguagesCultureList);

            // Trace.WriteLine("Evaluated InternalGetQuerystring: '" + (langItem == null ? "null" : langItem) + "'");
            if (langItem != null)
            {
                goto setLanguage;
            }

            // View state
            langItem = InternalGetViewState(switcher);

            // Trace.WriteLine("Evaluated InternalGetViewState: '" + (langItem == null ? "null" : langItem) + "'");
            if (langItem != null)
            {
                goto setLanguage;
            }

            // Cookie
            langItem = InternalGetCookie(myLanguagesCultureList, cookieAlias);

            // Trace.WriteLine("Evaluated InternalGetCookie: '" + (langItem == null ? "null" : langItem) + "'");
            if (langItem != null)
            {
                goto setLanguage;
            }

            // UserLanguageList
            langItem = InternalGetUserLanguages(myLanguagesCultureList);

            // Trace.WriteLine("Evaluated InternalGetUserLanguages: '" + (langItem == null ? "null" : langItem) + "'");
            if (langItem != null)
            {
                goto setLanguage;
            }

            // Default
            langItem = InternalGetDefault(myLanguagesCultureList);

            // Trace.WriteLine("Evaluated InternalGetDefault: '" + (langItem == null ? "null" : langItem) + "'");
            setLanguage:

            // Updates current cultures
            SetCurrentLanguage(langItem, cookieAlias);
        }

        /// <summary>
        /// The set current language.
        /// </summary>
        /// <param name="langItem">The lang item.</param>
        /// <param name="cookieAlias">Cookie name used for persist Language</param>
        /// <param name="switcher">The switcher.</param>
        internal static void SetCurrentLanguage(
            LanguageCultureItem langItem, string cookieAlias, LanguageSwitcher switcher)
        {
            Thread.CurrentThread.CurrentUICulture = langItem.UICulture;
            Thread.CurrentThread.CurrentCulture = langItem.Culture;

            // Persists choice
            InternalSetViewState(langItem, switcher);
            InternalSetCookie(langItem, cookieAlias);
        }

        /// <summary>
        /// Override CreateChildControls to create the control tree.
        /// </summary>
        protected override void CreateChildControls()
        {
            // Trace.WriteLine("Creating controls: Type: " + Type.ToString() + " / ChangeLanguageAction: " + ChangeLanguageAction.ToString() + " / Labels: " + Labels.ToString() + " / Flags: " + Flags.ToString() + " / LanguageListString: " + LanguageListString);
            ProcessCultures(this.LanguageListString, SwitcherCookieName, this);

            this.Controls.Clear();
            var myTable = new Table { CellPadding = 3, CellSpacing = 0 };
            var myRows = myTable.Rows;

            switch (this.Type)
            {
                    // Drop down list
                case LanguageSwitcherType.DropDownList:

                    var myTableRowDd = new TableRow();

                    if (this.Flags != LanguageSwitcherDisplay.DisplayNone)
                    {
                        var myImage = new Image();
                        myImage.ImageUrl = this.GetFlagImg(GetCurrentLanguage());

                        var myTableCellFlag = new TableCell();
                        myTableCellFlag.Controls.Add(myImage);

                        myTableRowDd.Controls.Add(myTableCellFlag);
                    }

                    var myTableCellDropDown = new TableCell();
                    this.langDropDown = new DropDownList();
                    this.langDropDown.CssClass = "rb_LangSw_dd"; // TODO make changeable

                    var myCurrentLanguage = GetCurrentLanguage();

                    // bind the drop down list
                    this.langDropDown.Items.Clear();
                    foreach (LanguageCultureItem i in this.LanguageList)
                    {
                        this.langDropDown.Items.Add(new ListItem(this.GetName(i), i.UICulture.Name));
                        if (i.UICulture.ToString() == myCurrentLanguage.UICulture.ToString())
                        {
                            // Select current language
                            this.langDropDown.Items[this.langDropDown.Items.Count - 1].Selected = true;
                        }
                    }

                    this.langDropDown.Attributes.Add(
                        "OnChange", this.GetLangAction().Replace("''", "this[this.selectedIndex].value"));

                    myTableCellDropDown.Controls.Add(this.langDropDown);
                    myTableRowDd.Controls.Add(myTableCellDropDown);
                    myRows.Add(myTableRowDd);
                    break;

                    // Links
                case LanguageSwitcherType.VerticalLinksList:

                    foreach (LanguageCultureItem l in this.LanguageList)
                    {
                        var tableRowLinks = new TableRow();

                        if (this.Flags != LanguageSwitcherDisplay.DisplayNone)
                        {
                            tableRowLinks.Controls.Add(this.GetFlagCell(l));
                        }

                        if (this.Labels != LanguageSwitcherDisplay.DisplayNone)
                        {
                            tableRowLinks.Controls.Add(this.GetLabelCell(l));
                        }

                        myRows.Add(tableRowLinks);
                    }

                    break;

                    // Horizontal links
                case LanguageSwitcherType.HorizontalLinksList:

                    var tableRowLinksHorizontal = new TableRow();

                    foreach (LanguageCultureItem l in this.LanguageList)
                    {
                        if (this.Flags != LanguageSwitcherDisplay.DisplayNone)
                        {
                            tableRowLinksHorizontal.Controls.Add(this.GetFlagCell(l));
                        }

                        if (this.Labels != LanguageSwitcherDisplay.DisplayNone)
                        {
                            tableRowLinksHorizontal.Controls.Add(this.GetLabelCell(l));
                        }
                    }

                    myRows.Add(tableRowLinksHorizontal);
                    break;
            }

            this.Controls.Add(myTable);
        }

        /// <summary>
        /// Examines/combines all the variables involved and sets
        /// CurrentUICulture and CurrentCulture.
        /// Can be overridden.
        /// </summary>
        /// <param name="e">The <see cref="Appleseed.Framework.Web.UI.WebControls.LanguageSwitcherEventArgs"/> instance containing the event data.</param>
        protected virtual void OnChangeLanguage(LanguageSwitcherEventArgs e)
        {
            // Updates current cultures
            SetCurrentLanguage(e.CultureItem, SwitcherCookieName, this);

            if (this.ChangeLanguage != null)
            {
                this.ChangeLanguage(this, e); // Invokes the delegates
            }
        }

        /// <summary>
        /// Renders the contents.
        /// </summary>
        /// <param name="output">The output.</param>
        protected override void RenderContents(HtmlTextWriter output)
        {
            // Trace.WriteLine("RenderingContents");
            this.EnsureChildControls();
            foreach (Control ctrl in this.Controls)
            {
                ctrl.RenderControl(output);
            }
        }

        /// <summary>
        /// Get current Language from Cookie
        /// </summary>
        /// <param name="myLanguagesCultureList">My languages culture list.</param>
        /// <param name="cookieAlias">The cookie alias.</param>
        /// <returns></returns>
        private static LanguageCultureItem InternalGetCookie(
            LanguageCultureCollection myLanguagesCultureList, string cookieAlias)
        {
            if (HttpContext.Current != null && cookieAlias != null &&
                HttpContext.Current.Request.Cookies[SwitcherCookiePrefix + cookieAlias] != null &&
                HttpContext.Current.Request.Cookies[SwitcherCookiePrefix + cookieAlias].Value.Length > 0)
            {
                try
                {
                    return
                        myLanguagesCultureList.GetBestMatching(
                            new CultureInfo(
                                HttpContext.Current.Request.Cookies[SwitcherCookiePrefix + cookieAlias].Value));
                }
                catch (ArgumentException)
                {
                    // Maybe an invalid CultureInfo
                }
            }

            return null;
        }

        /// <summary>
        /// Get default
        /// </summary>
        /// <param name="myLanguagesCultureList">My languages culture list.</param>
        /// <returns></returns>
        private static LanguageCultureItem InternalGetDefault(LanguageCultureCollection myLanguagesCultureList)
        {
            return myLanguagesCultureList[0];
        }

        /// <summary>
        /// Get current Language from Query string
        /// </summary>
        /// <param name="myLanguagesCultureList">My languages culture list.</param>
        /// <returns></returns>
        private static LanguageCultureItem InternalGetQuerystring(LanguageCultureCollection myLanguagesCultureList)
        {
            if (HttpContext.Current != null && HttpContext.Current.Request.Params["Lang"] != null &&
                HttpContext.Current.Request.Params["Lang"].Length > 0)
            {
                try
                {
                    return
                        myLanguagesCultureList.GetBestMatching(
                            new CultureInfo(HttpContext.Current.Request.Params["Lang"]));
                }
                catch (ArgumentException)
                {
                    // Maybe an invalid CultureInfo
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Get current Language from User language list
        /// </summary>
        /// <param name="myLanguagesCultureList">My languages culture list.</param>
        /// <returns></returns>
        private static LanguageCultureItem InternalGetUserLanguages(LanguageCultureCollection myLanguagesCultureList)
        {
            // Get userLangs
            if (HttpContext.Current != null && HttpContext.Current.Request.UserLanguages != null &&
                HttpContext.Current.Request.UserLanguages.Length > 0)
            {
                var arrUserLangs = new ArrayList(HttpContext.Current.Request.UserLanguages);
                if (arrUserLangs.Count > 0)
                {
                    for (var i = 0; i <= arrUserLangs.Count - 1; i++)
                    {
                        string currentLanguage;
                        if (arrUserLangs[i].ToString().IndexOf(';') >= 0)
                        {
                            currentLanguage = arrUserLangs[i].ToString().Substring(
                                0, arrUserLangs[i].ToString().IndexOf(';'));
                        }
                        else
                        {
                            currentLanguage = arrUserLangs[i].ToString();
                        }

                        try
                        {
                            // We try the full one... if this fails we catch it
                            arrUserLangs[i] = new CultureInfo(currentLanguage);
                        }
                        catch (ArgumentException)
                        {
                            try
                            {
                                // Some browsers can send an invalid language
                                // we try to get first two letters.. this is usually valid
                                arrUserLangs[i] = new CultureInfo(currentLanguage.Substring(2));
                            }
                            catch (ArgumentException)
                            {
                            }

                            return null;
                        }
                    }
                }

                var userLangs = (CultureInfo[])arrUserLangs.ToArray(typeof(CultureInfo));

                // Try to match browser "accept languages" list
                return myLanguagesCultureList.GetBestMatching(userLangs);
            }

            return null;
        }

        /// <summary>
        /// Get current Language from ViewState
        /// </summary>
        /// <param name="switcher">
        /// The switcher.
        /// </param>
        /// <returns>
        /// </returns>
        private static LanguageCultureItem InternalGetViewState(LanguageSwitcher switcher)
        {
            return switcher != null && switcher.ViewState["RB_Language_CurrentUICulture"] != null &&
                   switcher.ViewState["RB_Language_CurrentCulture"] != null
                       ? new LanguageCultureItem(
                             (CultureInfo)switcher.ViewState["RB_Language_CurrentUICulture"], 
                             (CultureInfo)switcher.ViewState["RB_Language_CurrentCulture"])
                       : null;
        }

        /// <summary>
        /// Internals the set cookie.
        /// </summary>
        /// <param name="myLanguageCultureItem">My language culture item.</param>
        /// <param name="cookieAlias">The cookie alias.</param>
        private static void InternalSetCookie(LanguageCultureItem myLanguageCultureItem, string cookieAlias)
        {
            // Set language cookie  --- hack.. do not set cookie if cookieAlias is languageSwitcher
            if (HttpContext.Current == null || cookieAlias == null || cookieAlias == SwitcherCookieName)
            {
                return;
            }

            // Trace.WriteLine("Persisting in cookie: '" + SWITCHER_COOKIE_PREFIX + cookieAlias + "'");
            var langCookie = HttpContext.Current.Response.Cookies[SwitcherCookiePrefix + cookieAlias];
            if (langCookie == null)
            {
                return;
            }

            langCookie.Value = myLanguageCultureItem.UICulture.Name;
            langCookie.Path = "/";

            // Keep the cookie?
            // if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            langCookie.Expires = DateTime.Now.AddYears(50);
        }

        /// <summary>
        /// Internals the state of the set view.
        /// </summary>
        /// <param name="myLanguageCultureItem">My language culture item.</param>
        /// <param name="switcher">The switcher.</param>
        private static void InternalSetViewState(LanguageCultureItem myLanguageCultureItem, LanguageSwitcher switcher)
        {
            if (switcher == null)
            {
                return;
            }

            // Trace.WriteLine("Persisting in view state");
            switcher.ViewState["RB_Language_CurrentUICulture"] = myLanguageCultureItem.UICulture;
            switcher.ViewState["RB_Language_CurrentCulture"] = myLanguageCultureItem.Culture;
        }

        /// <summary>
        /// Gets the flag cell.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <returns></returns>
        private TableCell GetFlagCell(LanguageCultureItem l)
        {
            var tableCellFlag = new TableCell
                {
                    CssClass =
                        l.UICulture.ToString() == GetCurrentLanguage().UICulture.ToString()
                            ? "rb_LangSw_sel"
                            : "rb_LangSw_tbl"
                };

            var image = new HyperLink
                {
                    NavigateUrl = this.GetLangUrl(l.UICulture.Name),
                    ImageUrl = this.GetFlagImg(l),
                    Text = this.GetName(l)
                };
            tableCellFlag.Controls.Add(image);
            return tableCellFlag;
        }

        /// <summary>
        /// Get flage image of language culture item
        /// </summary>
        /// <param name="languageItem"></param>
        /// <returns></returns>
        public string GetFlagImgLCI(LanguageCultureItem languageItem)
        {

            return this.GetFlagImg(languageItem);
        }

        /// <summary>
        /// Gets the flag img.
        /// </summary>
        /// <param name="languageItem">The language item.</param>
        /// <returns></returns>
        private string GetFlagImg(LanguageCultureItem languageItem)
        {
            CultureInfo culture;

            switch (this.Flags)
            {
                default:
                // case LanguageSwitcherDisplay.DisplayNone:
                    return string.Empty;

                case LanguageSwitcherDisplay.DisplayUICultureList:
                    culture = languageItem.UICulture;
                    break;

                case LanguageSwitcherDisplay.DisplayCultureList:
                    culture = languageItem.Culture;
                    break;
            }

            // Flag must be specific
            if (culture.IsNeutralCulture)
            {
                culture = CultureInfo.CreateSpecificCulture(culture.Name);
            }

            string flagImgUrl = culture.Name.Length > 0 ? string.Format("{0}flags_{1}.gif", this.ImagePath, culture.Name) : string.Format("{0}flags_unknown.gif", this.ImagePath);

            return flagImgUrl;
        }

        /// <summary>
        /// Gets the label cell.
        /// </summary>
        /// <param name="l">The l.</param>
        /// <returns></returns>
        private TableCell GetLabelCell(LanguageCultureItem l)
        {
            var tableCellLabel = new TableCell
                {
                    CssClass =
                        l.UICulture.ToString() == GetCurrentLanguage().UICulture.ToString()
                            ? "rb_LangSw_sel"
                            : "rb_LangSw_tbl"
                };

            var label = new HyperLink { NavigateUrl = this.GetLangUrl(l.UICulture.Name), Text = this.GetName(l) };
            tableCellLabel.Controls.Add(label);

            return tableCellLabel;
        }

        /// <summary>
        /// Used by dropdownlist
        /// </summary>
        /// <returns>
        /// The get lang action.
        /// </returns>
        private string GetLangAction()
        {
            return this.Page.ClientScript.GetPostBackEventReference(this, null);
        }

        /// <summary>
        /// Used by flags and label
        /// </summary>
        /// <param name="language">
        /// The language.
        /// </param>
        /// <returns>
        /// The get lang url.
        /// </returns>
        private string GetLangUrl(string language)
        {


            string lang = HttpUrlBuilder.BuildUrl("~/site/1/Home");
                

            return this.ChangeLanguageAction == LanguageSwitcherAction.LinkRedirect
                       ? string.Format("{0}?lang={1}", lang, language)
                       : string.Format("javascript:{0}", this.Page.ClientScript.GetPostBackEventReference(this, language));
        }

        /// <summary>
        /// get language culture item's name
        /// </summary>
        /// <param name="languageItem"></param>
        /// <returns></returns>
        public string getNameLCI(LanguageCultureItem languageItem)
        {
            return this.GetName(languageItem);
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <param name="languageItem">The language item.</param>
        /// <returns></returns>
        private string GetName(LanguageCultureItem languageItem)
        {
            CultureInfo culture;

            switch (this.Labels)
            {
                default:
                // case LanguageSwitcherDisplay.DisplayNone:
                    return string.Empty;

                case LanguageSwitcherDisplay.DisplayUICultureList:
                    culture = languageItem.UICulture;
                    break;

                case LanguageSwitcherDisplay.DisplayCultureList:
                    culture = languageItem.Culture;
                    break;
            }

            switch (this.ShowNameAs)
            {
                default:
                // case LanguageSwitcherName.NativeName:
                    return languageItem.Culture.TextInfo.ToTitleCase(culture.NativeName);

                case LanguageSwitcherName.DisplayName:
                    return languageItem.Culture.TextInfo.ToTitleCase(culture.DisplayName);

                case LanguageSwitcherName.EnglishName:
                    return languageItem.Culture.TextInfo.ToTitleCase(culture.EnglishName);
            }
        }

        #endregion
    }
}