// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortalModuleControl.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The PortalModuleControl class defines a custom
//   base class inherited by all
//   desktop portal modules within the Portal.<br />
//   The PortalModuleControl class defines portal
//   specific properties that are used by the portal framework
//   to correctly display portal modules.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI.WebControls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    using Appleseed.Framework.Configuration.Settings;
    using Appleseed.Framework.DataTypes;
    using Appleseed.Framework.Design;
    using Appleseed.Framework.Helpers;
    using Appleseed.Framework.Security;
    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Setup;
    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.Site.Data;

    using Page = Appleseed.Framework.Web.UI.Page;
    using Path = Appleseed.Framework.Settings.Path;

    /// <summary>
    /// The PortalModuleControl class defines a custom 
    ///   base class inherited by all
    ///   desktop portal modules within the Portal.<br/>
    ///   The PortalModuleControl class defines portal 
    ///   specific properties that are used by the portal framework
    ///   to correctly display portal modules.
    /// </summary>
    /// <remarks>
    /// This is the "all new" RC4 PortalModuleControl, which no longer has a separate DesktopModuleTitle.
    ///   Includes history from old DesktopModuleTitle class 2004/07/24 and older
    /// </remarks>
    [History("john.mandia@whitelightsolutions.com", "2004/09/17", "Fixed path for help system")]
    [History("Jes1111", "2003/03/05", "Added ShowTitle setting - switches Title visibility on/off")]
    [History("Jes1111", "2003/04/24", "Added PortalAlias to cache key")]
    [History("Jes1111", "2003/04/24", "Added Cacheable property")]
    [History("bja@reedtek.com", "2003/04/26", "Added support for win. mgmt min/max/close")]
    [History("david.verberckmoes@syntegra.com", "2003/06/02",
        "Showing LastModified date & user in a better way with themes")]
    [History("Jes1111", "2004/08/30", "All new version! No more DesktopModuleTitle.")]
    [History("Mark, John and Jose", "2004/09/08", "Corrections in constructor for detect DesignMode")]
    [History("Nicholas Smeaton", "2004/07/24", "Added support for arrow buttons to move modules")]
    [History("jviladiu@portalServices.net", "2004/07/13", "Corrections in workflow buttons")]
    [History("gman3001", "2004/04/08",
        "Added support for custom buttons in the title bar, and set all undefined title bar buttons to 'rb_mod_title_btn' CSS class."
        )]
    [History("Pekka Ylenius", "2004/11/28", "When '?' in URL then '&' is needed not '?'")]
    [History("Hongwei Shen", "2005/09/8", "Fix the publishing problem and RevertToProduction button problem")]
    [History("Hongwei Shen", "2005/09/12", "Fix topic setting order problem(add module specific settings group base)")]
    public class PortalModuleControl : ViewUserControl, ISearchable, IInstaller, ISettingHolder
    {
        #region Constants and Fields

        /// <summary>
        ///   Standard content Delete button
        /// </summary>
        protected ImageButton DeleteBtn;

        /// <summary>
        ///   Standard content Edit button
        /// </summary>
        protected ImageButton EditBtn;

        /// <summary>
        ///   Standard content Update button
        /// </summary>
        protected LinkButton UpdateButton;

        /// <summary>
        ///   used to hold the consolidated list of buttons for the module
        /// </summary>
        private readonly List<Control> buttonList = new List<Control>(3);

        /// <summary>
        ///   The footer.
        /// </summary>
        private readonly PlaceHolder footer = new PlaceHolder();

        /// <summary>
        ///   The header.
        /// </summary>
        private readonly PlaceHolder header = new PlaceHolder();

        /// <summary>
        ///   The header place holder.
        /// </summary>
        private readonly PlaceHolder headerPlaceHolder = new PlaceHolder();

        /// <summary>
        ///   The add button.
        /// </summary>
        private ModuleButton addButton;

        /// <summary>
        ///   The add target.
        /// </summary>
        private string addTarget;

        /// <summary>
        ///   The add text.
        /// </summary>
        private string addText = "ADD";

        /// <summary>
        ///   The add URL.
        /// </summary>
        private string addUrl;

        /// <summary>
        ///   The approve button.
        /// </summary>
        private ModuleButton approveButton;

        /// <summary>
        ///   The back button.
        /// </summary>
        private ModuleButton backButton;

        /// <summary>
        ///   The before content.
        /// </summary>
        private bool beforeContent = true;

        /// <summary>
        ///   Whether to build body.
        /// </summary>
        private bool buildBody = true;

        /// <summary>
        ///   The build buttons.
        /// </summary>
        private bool buildButtons = true;

        /// <summary>
        ///   The build title.
        /// </summary>
        private bool buildTitle = true;

        /// <summary>
        ///   The up button.
        /// </summary>
        /// <remarks>
        ///   Tiptopweb
        ///   Nicholas Smeaton: custom buttons from module developer enhancement added in Appleseed version 1.4.0.1767a - 03/07/2004
        /// </remarks>
        private ModuleButton buttonUp;

        /// <summary>
        ///   The buttons render as.
        /// </summary>
        private ModuleButton.RenderOptions buttonsRenderAs = ModuleButton.RenderOptions.ImageOnly;

        /// <summary>
        ///   The can add.
        /// </summary>
        private int canAdd;

        /// <summary>
        ///   The can close.
        /// </summary>
        private int canClose;

        /// <summary>
        ///   The can delete.
        /// </summary>
        private int canDelete;

        /// <summary>
        ///   The can edit.
        /// </summary>
        private int canEdit;

        /// <summary>
        ///   The can min.
        /// </summary>
        private int canMin;

        /// <summary>
        ///   The can print.
        /// </summary>
        private bool canPrint;

        /// <summary>
        ///   The can properties.
        /// </summary>
        private int canProperties;

        /// <summary>
        ///   The can view.
        /// </summary>
        private int canView;

        /// <summary>
        ///   The content.
        /// </summary>
        private object content;

        /// <summary>
        ///   The delete module button.
        /// </summary>
        private ModuleButton deleteModuleButton;

        /// <summary>
        ///   The down button.
        /// </summary>
        private ModuleButton downButton;

        /// <summary>
        ///   The edit button.
        /// </summary>
        private ModuleButton editButton;

        /// <summary>
        ///   The edit target.
        /// </summary>
        private string editTarget;

        /// <summary>
        ///   The edit text.
        /// </summary>
        private string editText = "EDIT";

        /// <summary>
        ///   The edit URL.
        /// </summary>
        private string editUrl;

        /// <summary>
        ///   The email button.
        /// </summary>
        private ModuleButton emailButton;

        /// <summary>
        ///   The help button.
        /// </summary>
        private ModuleButton helpButton;

        /// <summary>
        ///   The left button.
        /// </summary>
        private ModuleButton leftButton;

        /// <summary>
        ///   The module configuration.
        /// </summary>
        private ModuleSettings moduleConfiguration;

        /// <summary>
        ///   The original module id.
        /// </summary>
        private int originalModuleId = -1;

        /// <summary>
        ///   The page id.
        /// </summary>
        private int pageId;

        /// <summary>
        ///   The print button.
        /// </summary>
        private ModuleButton printButton;

        /// <summary>
        ///   The properties button.
        /// </summary>
        private ModuleButton propertiesButton;

        /// <summary>
        ///   The properties target.
        /// </summary>
        private string propertiesTarget;

        /// <summary>
        ///   The properties text.
        /// </summary>
        private string propertiesText = "PROPERTIES";

        /// <summary>
        ///   The properties URL.
        /// </summary>
        private string propertiesUrl = "~/DesktopModules/CoreModules/Admin/PropertyPage.aspx";

        /// <summary>
        ///   The publish button.
        /// </summary>
        private ModuleButton publishButton;

        /// <summary>
        ///   The ready to approve button.
        /// </summary>
        private ModuleButton readyToApproveButton;

        /// <summary>
        ///   The reject button.
        /// </summary>
        private ModuleButton rejectButton;

        /// <summary>
        ///   The revert button.
        /// </summary>
        private ModuleButton revertButton;

        /// <summary>
        ///   The right button.
        /// </summary>
        private ModuleButton rightButton;

        /// <summary>
        ///   The security button.
        /// </summary>
        private ModuleButton securityButton;

        /// <summary>
        ///   The security target.
        /// </summary>
        private string securityTarget;

        /// <summary>
        ///   The security text.
        /// </summary>
        private string securityText = "SECURITY";

        /// <summary>
        ///   The security URL.
        /// </summary>
        private string securityUrl = "~/DesktopModules/CoreModules/Admin/ModuleSettings.aspx";

        /// <summary>
        ///   The settings.
        /// </summary>
        private Dictionary<string, ISettingItem> settings;

        /// <summary>
        ///   The supports arrows.
        /// </summary>
        private bool supportsArrows = true;

        /// <summary>
        ///   The supports collapsible.
        /// </summary>
        private bool supportsCollapsible;

        /*
                /// <summary>
                /// The supports help.
                /// </summary>
                private bool supportsHelp;
        */

        /// <summary>
        ///   The title text.
        /// </summary>
        private string titleText = string.Empty;

        /// <summary>
        ///   The view control manager.
        /// </summary>
        private ViewControlManager vcm;

        /// <summary>
        ///   The version.
        /// </summary>
        private WorkFlowVersion version = WorkFlowVersion.Production;

        /// <summary>
        ///   The version button.
        /// </summary>
        private ModuleButton versionButton;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PortalModuleControl" /> class.
        /// </summary>
        public PortalModuleControl()
        {
            this.BaseSettings = new Dictionary<string, ISettingItem>();
            this.ButtonListAdmin = new List<Control>(3);
            this.ButtonListCustom = new List<Control>(3);
            this.ButtonListUser = new List<Control>(3);
            this.ApproveText = "SWI_APPROVE";
            this.ProductionVersionText = "SWI_SWAPTOPRODUCTION";
            this.PublishText = "SWI_PUBLISH";
            this.ReadyToApproveText = "SWI_READYTOAPPROVE";
            this.RejectText = "SWI_REJECT";
            this.RevertText = "SWI_REVERT";
            this.StagingVersionText = "SWI_SWAPTOSTAGING";
            this.SupportsPrint = true;
            this.Cacheable = true;

            // MVC
            var wrapper = new HttpContextWrapper(this.Context);

            var viewContext = new ViewContext { HttpContext = wrapper, ViewData = new ViewDataDictionary() };

            this.ViewContext = viewContext;

            // THEME MANAGEMENT
            var group = SettingItemGroup.THEME_LAYOUT_SETTINGS;
            var groupOrderBase = (int)SettingItemGroup.THEME_LAYOUT_SETTINGS;

            var applyTheme = new SettingItem<bool, CheckBox>
                {
                    Order = groupOrderBase + 10,
                    Group = group,
                    Value = true,
                    EnglishName = "Apply Theme",
                    Description = "Check this box to apply theme to this module"
                };
            this.BaseSettings.Add("MODULESETTINGS_APPLY_THEME", applyTheme);

            var themeOptions = new List<SettingOption>
                {
                    new SettingOption((int)ThemeList.Default, General.GetString("MODULESETTINGS_THEME_DEFAULT")),
                    new SettingOption((int)ThemeList.Alt, General.GetString("MODULESETTINGS_THEME_ALT"))
                };
            var theme = new SettingItem<string, ListControl>(new CustomListDataType(themeOptions, "Name", "Val"))
                {
                    Order = groupOrderBase + 20,
                    Group = group,
                    Value = ((int)ThemeList.Default).ToString(),
                    EnglishName = "Theme",
                    Description = "Choose theme for this module"
                };
            this.BaseSettings.Add("MODULESETTINGS_THEME", theme);

            if (HttpContext.Current != null)
            {
                // null in DesignMode
                // Added: Jes1111 - 2004-08-03
                var PortalSettings1 = (PortalSettings)HttpContext.Current.Items["PortalSettings"];

                // end addition: Jes1111
                if (PortalSettings1 != null)
                {
                    // fix by The Bitland Prince
                    this.PortalID = PortalSettings1.PortalID;

                    // added: Jes1111 2004-08-02 - custom module theme
                    if (PortalSettings1.CustomSettings.ContainsKey("SITESETTINGS_ALLOW_MODULE_CUSTOM_THEMES") &&
                        PortalSettings1.CustomSettings["SITESETTINGS_ALLOW_MODULE_CUSTOM_THEMES"].ToString().Length != 0 &&
                        bool.Parse(PortalSettings1.CustomSettings["SITESETTINGS_ALLOW_MODULE_CUSTOM_THEMES"].ToString()))
                    {
                        var tempList = new List<object>(new ThemeManager(PortalSettings1.PortalPath).GetThemes());
                        var themeList =
                            tempList.Cast<ThemeItem>().Where(item => item.Name.ToLower().StartsWith("module")).ToList();

                        var customThemeNo = new ThemeItem { Name = string.Empty };
                        themeList.Insert(0, customThemeNo);
                        var moduleTheme =
                            new SettingItem<string, ListControl>(new CustomListDataType(themeList, "Name", "Name"))
                                {
                                    Order = groupOrderBase + 25,
                                    Group = group,
                                    EnglishName = "Custom Theme",
                                    Description = "Set a custom theme for this module only"
                                };
                        this.BaseSettings.Add("MODULESETTINGS_MODULE_THEME", moduleTheme);
                    }
                }
            }

            // switches title display on/off
            var showTitle = new SettingItem<bool, CheckBox>
                {
                    Order = groupOrderBase + 30,
                    Group = group,
                    Value = true,
                    EnglishName = "Show Title",
                    Description = "Switches title display on/off"
                };
            this.BaseSettings.Add("MODULESETTINGS_SHOW_TITLE", showTitle);

            // switches last modified summary on/off
            var showModifiedBy = new SettingItem<bool, CheckBox>
                {
                    Order = groupOrderBase + 40,
                    Group = group,
                    Value = false,
                    EnglishName = "Show Modified by",
                    Description = "Switches 'Show Modified by' display on/off"
                };
            this.BaseSettings.Add("MODULESETTINGS_SHOW_MODIFIED_BY", showModifiedBy);

            // gman3001: added 10/26/2004
            // - implement width, height, and content scrolling options for all modules 
            // - implement auto-stretch option
            // Windows height
            var controlHeight = new SettingItem<int, TextBox>
                {
                    Value = 0,
                    MinValue = 0,
                    MaxValue = 3000,
                    Required = true,
                    Order = groupOrderBase + 50,
                    Group = group,
                    EnglishName = "Content Height",
                    Description = "Minimum height(in pixels) of the content area of this module. (0 for none)"
                };
            this.BaseSettings.Add("MODULESETTINGS_CONTENT_HEIGHT", controlHeight);

            // Windows width
            var controlWidth = new SettingItem<int, TextBox>
                {
                    Value = 0,
                    MinValue = 0,
                    MaxValue = 3000,
                    Required = true,
                    Order = groupOrderBase + 60,
                    Group = group,
                    EnglishName = "Content Width",
                    Description = "Minimum width(in pixels) of the content area of this module. (0 for none)"
                };
            this.BaseSettings.Add("MODULESETTINGS_CONTENT_WIDTH", controlWidth);

            // Content scrolling option
            var scrollingSetting = new SettingItem<bool, CheckBox>
                {
                    Value = false,
                    Order = groupOrderBase + 70,
                    Group = group,
                    EnglishName = "Content Scrolling",
                    Description = "Set to enable/disable scrolling of Content based on height and width settings."
                };
            this.BaseSettings.Add("MODULESETTINGS_CONTENT_SCROLLING", scrollingSetting);

            // Module Stretching option
            var stretchSetting = new SettingItem<bool, CheckBox>
                {
                    Value = true,
                    Order = groupOrderBase + 80,
                    Group = group,
                    EnglishName = "Module Auto Stretch",
                    Description =
                        "Set to enable/disable automatic stretching of the module's width to fill the empty area to the right of the module."
                };
            this.BaseSettings.Add("MODULESETTINGS_WIDTH_STRETCHING", stretchSetting);

            // gman3001: END

            // BUTTONS
            group = SettingItemGroup.BUTTON_DISPLAY_SETTINGS;
            groupOrderBase = (int)SettingItemGroup.BUTTON_DISPLAY_SETTINGS;

            // Show print button in view mode?
            var printButton1 = new SettingItem<bool, CheckBox>
                {
                    Value = false,
                    Order = groupOrderBase + 20,
                    Group = group,
                    EnglishName = "Show Print Button",
                    Description = "Show print button in view mode?"
                };
            this.BaseSettings.Add("MODULESETTINGS_SHOW_PRINT_BUTTION", printButton1);

            // added: Jes1111 2004-08-29 - choice! Default is 'true' for backward compatibility
            // Show Title for print?
            var showTitlePrint = new SettingItem<bool, CheckBox>
                {
                    Value = true,
                    Order = groupOrderBase + 25,
                    Group = group,
                    EnglishName = "Show Title for Print",
                    Description = "Show Title for this module in print popup?"
                };
            this.BaseSettings.Add("MODULESETTINGS_SHOW_TITLE_PRINT", showTitlePrint);

            // added: Jes1111 2004-08-02 - choices for Button display on module
            var buttonDisplayOptions = new List<SettingOption>
                {
                    new SettingOption(
                        (int)ModuleButton.RenderOptions.ImageOnly,
                        General.GetString("MODULESETTINGS_BUTTON_DISPLAY_IMAGE")),
                    new SettingOption(
                        (int)ModuleButton.RenderOptions.TextOnly,
                        General.GetString("MODULESETTINGS_BUTTON_DISPLAY_TEXT")),
                    new SettingOption(
                        (int)ModuleButton.RenderOptions.ImageAndTextCSS,
                        General.GetString("MODULESETTINGS_BUTTON_DISPLAY_BOTH")),
                    new SettingOption(
                        (int)ModuleButton.RenderOptions.ImageOnlyCSS,
                        General.GetString("MODULESETTINGS_BUTTON_DISPLAY_IMAGECSS"))
                };
            var buttonDisplay =
                new SettingItem<string, ListControl>(new CustomListDataType(buttonDisplayOptions, "Name", "Val"))
                    {
                        Order = groupOrderBase + 30,
                        Group = group,
                        Value = ((int)ModuleButton.RenderOptions.ImageOnly).ToString(),
                        EnglishName = "Display Buttons as:",
                        Description =
                            "Choose how you want module buttons to be displayed. Note that settings other than 'Image only' may require Zen or special treatment in the Theme."
                    };
            this.BaseSettings.Add("MODULESETTINGS_BUTTON_DISPLAY", buttonDisplay);

            // Jes1111 - not implemented yet
            // // Show email button in view mode?
            // SettingItem EmailButton = new SettingItem<bool, CheckBox>();
            // EmailButton.Value = "False";
            // EmailButton.Order = _groupOrderBase + 30;
            // EmailButton.Group = _Group;
            // this.BaseSettings.Add("ShowEmailButton",EmailButton);

            // Show arrows buttons to move modules (admin only, property authorize)
            var arrowButtons = new SettingItem<bool, CheckBox>
                {
                    Value = true,
                    Order = groupOrderBase + 40,
                    Group = group,
                    EnglishName = "Show Arrow Admin Buttons",
                    Description = "Show Arrow Admin buttons?"
                };
            this.BaseSettings.Add("MODULESETTINGS_SHOW_ARROW_BUTTONS", arrowButtons);

            // Show help button if exists
            var helpButton1 = new SettingItem<bool, CheckBox>
                {
                    Value = true,
                    Order = groupOrderBase + 50,
                    Group = group,
                    EnglishName = "Show Help Button",
                    Description = "Show help button in title if exists documentation for this module"
                };
            this.BaseSettings.Add("MODULESETTINGS_SHOW_HELP_BUTTON", helpButton1);

            // LANGUAGE/CULTURE MANAGEMENT
            groupOrderBase = (int)SettingItemGroup.CULTURE_SETTINGS;
            group = SettingItemGroup.CULTURE_SETTINGS;

            var cultureList = Localization.LanguageSwitcher.GetLanguageList(true);

            var culture =
                new SettingItem<string, ListControl>(new MultiSelectListDataType(cultureList, "DisplayName", "Name"))
                    {
                        Value = string.Empty,
                        Order = groupOrderBase + 10,
                        Group = group,
                        EnglishName = "Culture",
                        Description =
                            "Please choose the culture. Invariant cultures shows always the module, if you choose one or more cultures only when culture is selected this module will shown."
                    };
            this.BaseSettings.Add("MODULESETTINGS_CULTURE", culture);

            // Localized module title
            var counter = groupOrderBase + 11;

            // Ignore invariant
            foreach (var c in
                cultureList.Where(c => c != CultureInfo.InvariantCulture && !this.BaseSettings.ContainsKey(c.Name)))
            {
                var localizedTitle = new SettingItem<string, TextBox>
                    {
                        Order = counter,
                        Group = group,
                        EnglishName = string.Format("Title ({0})", c.Name),
                        Description = string.Format("Set title for {0} culture.", c.EnglishName)
                    };
                this.BaseSettings.Add(string.Format("MODULESETTINGS_TITLE_{0}", c.Name), localizedTitle);
                counter++;
            }

            // SEARCH
            if (this.Searchable)
            {
                groupOrderBase = (int)SettingItemGroup.MODULE_SPECIAL_SETTINGS;
                group = SettingItemGroup.MODULE_SPECIAL_SETTINGS;

                var topicName = new SettingItem<string, TextBox>
                    {
                        Required = false,
                        Value = string.Empty,
                        // modified by Hongwei Shen(hongwei.shen@gmail.com) 11/9/2005
                        // group base and order is not specified
                        Group = group,
                        Order = groupOrderBase,
                        EnglishName = "Topic",
                        Description = "Select a topic for this module. You may filter items by topic in Portal Search."
                    };

                // end of modification
                this.BaseSettings.Add("TopicName", topicName);
            }

            // Default configuration
            this.pageId = 0;

            this.moduleConfiguration = new ModuleSettings();

            var share = new SettingItem<bool, CheckBox>
                {
                    Value = false,
                    Order = groupOrderBase + 51,
                    Group = SettingItemGroup.MODULE_SPECIAL_SETTINGS,
                    EnglishName = "ShareModule",
                    Description = "Share Module"
                };
            this.BaseSettings.Add("SHARE_MODULE", share);
        }

        #endregion

        #region Events

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

        #region Enums

        /// <summary>
        /// Localizes Theme types: 'Default' and 'Alt'
        /// </summary>
        public enum ThemeList
        {
            /// <summary>
            ///   The default.
            /// </summary>
            Default = 0,

            /// <summary>
            ///   The alternate.
            /// </summary>
            Alt = 1
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets Module Add button
        /// </summary>
        public ModuleButton AddButton
        {
            get
            {
                if (this.addButton == null && HttpContext.Current != null)
                {
                    // check authority
                    if (this.CanAdd)
                    {
                        // create the button
                        this.addButton = new ModuleButton
                            {
                                Group = ModuleButton.ButtonGroup.Admin,
                                TranslationKey = this.AddText,
                                EnglishName = "Add"
                            };
                        if (this.AddUrl.IndexOf("?") >= 0)
                        {
                            // Do not change if  the query string is present
                            // if ( this.ModuleID != OriginalModuleID )
                            this.AddButton.HRef = this.AddUrl;
                        }
                        else
                        {
                            this.AddButton.HRef = HttpUrlBuilder.BuildUrl(
                                this.AddUrl, this.PageID, string.Format("mID={0}", this.ModuleID));
                        }
                        if (this.AddButton.HRef.Contains("UsersManage.aspx") || AddButton.HRef.Contains("ComponentModuleEdit.aspx"))
                            this.addButton.Attributes.Add("onclick", "openInModal('" + AddButton.HRef + "','" + General.GetString("EDIT_COMPONENTMODULE", "Edit Component") + "');return false;");

                        this.addButton.Target = this.AddTarget;
                        this.addButton.Image = this.CurrentTheme.GetImage("Buttons_Add", "Add.gif");
                        this.addButton.RenderAs = this.ButtonsRenderAs;
                    }
                }

                return this.addButton;
            }
        }

        /// <summary>
        ///   Gets the path of the add control if available.
        ///   Override on derivates classes.
        /// </summary>
        public virtual string AddModuleControl
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///   Gets or sets Target frame/page for Add Link
        /// </summary>
        public string AddTarget
        {
            get
            {
                if (this.ModuleTitle != null && this.ModuleTitle.AddTarget.Length != 0)
                {
                    this.addTarget = this.ModuleTitle.AddTarget;
                }

                return this.addTarget;
            }

            set
            {
                this.addTarget = value;
            }
        }

        /// <summary>
        ///   Gets or sets Text for Add Link
        /// </summary>
        public string AddText
        {
            get
            {
                if (this.ModuleTitle != null && this.ModuleTitle.AddText.Length != 0)
                {
                    this.addText = this.ModuleTitle.AddText;
                }

                return this.addText;
            }

            set
            {
                this.addText = value;
            }
        }

        /// <summary>
        ///   Gets or sets URL for Add Link
        /// </summary>
        /// <value>The add URL.</value>
        public string AddUrl
        {
            get
            {
                if (this.ModuleTitle != null && this.ModuleTitle.AddUrl.Length != 0)
                {
                    this.addUrl = this.ModuleTitle.AddUrl;
                }

                return this.addUrl;
            }

            set
            {
                this.addUrl = value;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether [admin module].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [admin module]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool AdminModule
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether before apply theme DesktopModule designer checks this
        ///   property to be true.<br />
        ///   On inherited modules you can override this
        ///   property to not apply theme on specific controls.
        /// </summary>
        /// <value><c>true</c> if [apply theme]; otherwise, <c>false</c>.</value>
        public virtual bool ApplyTheme
        {
            get
            {
                return HttpContext.Current == null ||
                       ((ISettingItem<bool>)this.Settings["MODULESETTINGS_APPLY_THEME"]).Value;
            }

            set
            {
                if (HttpContext.Current != null)
                {
                    // if it is not design time
                    this.Settings["MODULESETTINGS_APPLY_THEME"].Value = value;
                }
            }
        }

        /// <summary>
        ///   Gets Module Approve button
        /// </summary>
        public ModuleButton ApproveButton
        {
            get
            {
                if (this.approveButton == null && HttpContext.Current != null)
                {
                    // check authority
                    if (this.CanApproveReject)
                    {
                        // create the button
                        this.approveButton = new ModuleButton
                            {
                                Group = ModuleButton.ButtonGroup.Admin,
                                TranslationKey = this.ApproveText,
                                EnglishName = "Approve",
                                HRef =
                                    HttpUrlBuilder.BuildUrl(
                                        "~/DesktopModules/Workflow/ApproveModuleContent.aspx",
                                        this.PageID,
                                        string.Format("mID={0}", this.ModuleID)),
                                Image = this.CurrentTheme.GetImage("Buttons_Approve", "Approve.gif"),
                                RenderAs = this.ButtonsRenderAs
                            };
                    }
                }

                return this.approveButton;
            }
        }

        /// <summary>
        ///   Gets or sets text for approve link
        /// </summary>
        public string ApproveText { get; set; }

        /// <summary>
        ///   Gets a value indicating whether [are properties editable]. (Edit properties permission)
        /// </summary>
        /// <value>
        ///   <c>true</c> if [are properties editable]; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ArePropertiesEditable
        {
            get
            {
                if (this.moduleConfiguration == null || this.moduleConfiguration.AuthorizedPropertiesRoles == null)
                {
                    return false;
                }

                // Perform tri-state switch check to avoid having to perform a security
                // role lookup on every property access (instead caching the result)
                if (this.canProperties == 0)
                {
                    this.canProperties = PortalSecurity.IsInRoles(this.moduleConfiguration.AuthorizedPropertiesRoles)
                                             ? 1
                                             : 2;
                }

                return this.canProperties == 1;
            }
        }

        /// <summary>
        ///   Gets Module button that will return to previous tab
        /// </summary>
        public ModuleButton BackButton
        {
            get
            {
                if (this.backButton == null && HttpContext.Current != null)
                {
                    // check authority
                    if (this.CanBack)
                    {
                        var urlReferrer = this.Request.UrlReferrer == null ? null : this.Request.UrlReferrer.ToString();

                        // create the button
                        this.backButton = new ModuleButton
                            {
                                Group = ModuleButton.ButtonGroup.User,
                                TranslationKey = "BTN_BACK",
                                EnglishName = "Back to previous page",
                                HRef = urlReferrer,
                                Image = this.CurrentTheme.GetImage("Buttons_Back", "Back.gif"),
                                RenderAs = this.ButtonsRenderAs
                            };
                    }
                }

                return this.backButton;
            }
        }

        /// <summary>
        ///   Gets or sets Module base settings defined by control creator
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<string, ISettingItem> BaseSettings { get; protected set; }

        /// <summary>
        ///   Gets or sets the button list admin.
        /// </summary>
        /// <value>
        ///   The button list admin.
        /// </value>
        public List<Control> ButtonListAdmin { get; set; }

        /// <summary>
        ///   Gets or sets the button list custom.
        /// </summary>
        /// <value>
        ///   The button list custom.
        /// </value>
        public List<Control> ButtonListCustom { get; set; }

        /// <summary>
        ///   Gets or sets the button list user.
        /// </summary>
        /// <value>
        ///   The button list user.
        /// </value>
        public List<Control> ButtonListUser { get; set; }

        /// <summary>
        ///   Gets or sets how ModuleButtons are rendered: as TextOnly, TextAndImage or ImageOnly. ImageOnly is the 'classic' Appleseed style.
        /// </summary>
        public ModuleButton.RenderOptions ButtonsRenderAs
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    // if it is not design time
                    if (this.Settings.ContainsKey("MODULESETTINGS_BUTTON_DISPLAY") && this.Settings["MODULESETTINGS_BUTTON_DISPLAY"] != null &&
                        this.Settings["MODULESETTINGS_BUTTON_DISPLAY"].ToString().Length != 0)
                    {
                        this.buttonsRenderAs =
                            (ModuleButton.RenderOptions)
                            int.Parse(this.Settings["MODULESETTINGS_BUTTON_DISPLAY"].ToString());
                    }
                }

                return this.buttonsRenderAs;
            }

            set
            {
                this.buttonsRenderAs = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether this <see cref = "PortalModuleControl" /> is cacheable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if cacheable; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        ///   Jes1111
        /// </remarks>
        public virtual bool Cacheable { get; set; }

        /// <summary>
        ///   Gets a value indicating whether this instance can add.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can add; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanAdd
        {
            get
            {
                return this.ModuleConfiguration != null &&
                       this.PortalSettings.ActivePage.PageID == this.ModuleConfiguration.PageID &&
                       (((this.SupportsWorkflow && this.Version == WorkFlowVersion.Staging) || !this.SupportsWorkflow) &&
                        (PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedAddRoles) &&
                         (!string.IsNullOrEmpty(this.AddUrl)) &&
                         (this.WorkflowStatus == WorkflowState.Original || this.WorkflowStatus == WorkflowState.Working)));
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance can approve reject. (Permission for Approve/Reject Buttons)
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can approve reject; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanApproveReject
        {
            get
            {
                return this.ModuleConfiguration != null &&
                       (this.SupportsWorkflow &&
                        PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedApproveRoles) &&
                        (this.ApproveText != null) && (this.RejectText != null) &&
                        this.Version == WorkFlowVersion.Staging &&
                        this.WorkflowStatus == WorkflowState.ApprovalRequested);
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance can arrows. (Permission for Arrow Buttons (Up/Down/Left/Right))
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can arrows; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanArrows
        {
            get
            {
                return this.ModuleConfiguration != null && this.ModuleID != 0 &&
                       (this.SupportsArrows && this.PortalSettings.ActivePage.PageID == this.ModuleConfiguration.PageID &&
                       (PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedMoveModuleRoles) || UserProfile.CurrentUser.HasPermission(AccessPermissions.MODULE_EDITING))); 
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance can back.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can back; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanBack
        {
            get
            {
                return this.SupportsBack && this.ShowBack && this.Request.UrlReferrer != null;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance can close.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can close; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanClose
        {
            get
            {
                // Perform tri-state switch check to avoid having to perform a security
                // role lookup on every property access (instead caching the result)
                if (this.canClose == 0)
                {
                    this.canClose = PortalSecurity.IsInRoles(this.moduleConfiguration.AuthorizedDeleteRoles) ? 1 : 2;
                }

                return this.canClose == 1;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance can delete module.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can delete module; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanDeleteModule
        {
            get
            {
                return (PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedDeleteModuleRoles) || UserProfile.CurrentUser.HasPermission(AccessPermissions.MODULE_DELETION)) && 
                       this.PortalSettings.ActivePage.PageID == this.ModuleConfiguration.PageID;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance can edit.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can edit; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanEdit
        {
            get
            {
                return this.ModuleConfiguration != null &&
                       this.PortalSettings.ActivePage.PageID == this.ModuleConfiguration.PageID &&
                       (((this.SupportsWorkflow && this.Version == WorkFlowVersion.Staging) || !this.SupportsWorkflow) &&
                        ((PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedEditRoles) || UserProfile.CurrentUser.HasPermission(AccessPermissions.MODULE_HTML_CONTENT_EDITING)) && 
                         (!string.IsNullOrEmpty(this.EditUrl)) &&
                         (this.WorkflowStatus == WorkflowState.Original || this.WorkflowStatus == WorkflowState.Working)));
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance can email.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can email; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanEmail
        {
            get
            {
                return this.SupportsEmail && this.Settings["ShowEmailButton"] != null &&
                       bool.Parse(this.Settings["ShowEmailButton"].ToString());
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance can help.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can help; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanHelp
        {
            get
            {
                return this.SupportsHelp &&
                       (PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedEditRoles) ||
                        PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedAddRoles) ||
                        PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedDeleteRoles) ||
                        PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedPropertiesRoles) ||
                        PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedPublishingRoles));
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance can minimized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can minimized; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanMinimized
        {
            get
            {
                // Perform tri-state switch check to avoid having to perform a security
                // role lookup on every property access (instead caching the result)
                if (this.canMin == 0)
                {
                    this.canMin = PortalSecurity.IsInRoles(this.moduleConfiguration.AuthorizedViewRoles) ? 1 : 2;
                }

                return this.canMin == 1;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance can print.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can print; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanPrint
        {
            get
            {
                return this.SupportsPrint && this.Settings.ContainsKey("MODULESETTINGS_SHOW_PRINT_BUTTION") &&
                       bool.Parse(this.Settings["MODULESETTINGS_SHOW_PRINT_BUTTION"].ToString());
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance can properties. (Permission for Properties Button)
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can properties; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanProperties
        {
            get
            {
                return this.ModuleConfiguration != null &&
                        ((PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedPropertiesRoles) || UserProfile.CurrentUser.HasPermission(AccessPermissions.MODULE_EDITING)) && 
                        this.PortalSettings.ActivePage.PageID == this.ModuleConfiguration.PageID &&
                        !string.IsNullOrEmpty(this.PropertiesUrl));
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance can publish. (Permission for Publish Button)
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can publish; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanPublish
        {
            get
            {
                return this.ModuleConfiguration != null &&
                       (this.SupportsWorkflow &&
                        PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedPublishingRoles) &&
                        (this.PublishText != null) && this.Version == WorkFlowVersion.Staging &&
                        this.WorkflowStatus == WorkflowState.Approved);
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance can request approval. (Permission for ReadyToApprove and Revert Buttons)
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can request approval; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanRequestApproval
        {
            get
            {
                return this.ModuleConfiguration != null &&
                       (this.SupportsWorkflow &&
                        (PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedAddRoles) ||
                         PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedDeleteRoles) ||
                         PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedEditRoles)) &&
                        (this.ReadyToApproveText != null) && this.Version == WorkFlowVersion.Staging &&
                        this.WorkflowStatus == WorkflowState.Working);
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance can security. (Permission for Security Button)
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can security; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanSecurity
        {
            get
            {
                return this.ModuleConfiguration != null &&
                       ((PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedPropertiesRoles) || UserProfile.CurrentUser.HasPermission(AccessPermissions.MODULE_EDITING)) &&
                        this.PortalSettings.ActivePage.PageID == this.ModuleConfiguration.PageID &&
                        !string.IsNullOrEmpty(this.SecurityUrl));
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance can version (permission for version button).
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can version; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanVersion
        {
            get
            {
                return this.ModuleConfiguration != null &&
                       (this.SupportsWorkflow &&
                        (PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedAddRoles) ||
                         PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedDeleteRoles) ||
                         PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedEditRoles) ||
                         PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedApproveRoles) ||
                         PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedPublishingRoles)) &&
                        (this.ProductionVersionText != null) && (this.StagingVersionText != null));
            }
        }

        /// <summary>
        ///   Gets ClassName (Used for Get/Save: not implemented)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string ClassName
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///   Gets or sets module content as an object, if called.
        /// </summary>
        public object Content
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return "Module Content PlaceHolder";
                }

                if (this.content != null)
                {
                    return this.content;
                }

                try
                {
                    this.content = this.GetContent();
                    return this.content;
                }
                catch
                {
                    return "error"; // TODO: change this
                }
            }

            set
            {
                this.content = value;
            }
        }

        /// <summary>
        ///   Gets or sets the module culture. If specified module should be showed
        ///   only if current culture matches this setting.
        ///   Colon separated list
        /// </summary>
        public virtual string Cultures
        {
            get
            {
                return HttpContext.Current != null && this.Settings.ContainsKey("MODULESETTINGS_CULTURE")
                           ? this.Settings["MODULESETTINGS_CULTURE"].ToString()
                           : Thread.CurrentThread.CurrentUICulture.Name;
            }

            set
            {
                if (HttpContext.Current != null)
                {
                    this.Settings["MODULESETTINGS_CULTURE"].Value = value;
                }
            }
        }

        /// <summary>
        ///   Gets or sets the current theme.
        /// </summary>
        /// <value>
        ///   The current theme.
        /// </value>
        public Theme CurrentTheme { get; set; }

        /// <summary>
        ///   Gets "Delete this Module" button
        /// </summary>
        public ModuleButton DeleteModuleButton
        {
            get
            {
                if (this.deleteModuleButton == null && HttpContext.Current != null)
                {
                    // check authority
                    if (this.CanDeleteModule)
                    {
                        // create the button
                        this.deleteModuleButton = new ModuleButton
                            {
                                Group = ModuleButton.ButtonGroup.Admin,
                                TranslationKey = "DELETEMODULE",
                                EnglishName = "Delete this module",
                                Image = this.CurrentTheme.GetImage("Buttons_DeleteModule", "Delete.gif"),
                                RenderAs = this.ButtonsRenderAs
                            };

                        // TODO: This JavaScript Function Is used for different controls and should be in one place
                        // (it's also overweight considering that Javascript has a standard confirm() function - Jes1111)
                        if (this.Page.Request.Browser.EcmaScriptVersion.Major >= 1 &&
                            !this.Page.ClientScript.IsClientScriptBlockRegistered(this.Page.GetType(), "confirmDelete"))
                        {
                            string[] s = { "CONFIRM_DELETE" };
                            this.Page.ClientScript.RegisterClientScriptBlock(
                                this.Page.GetType(),
                                "confirmDelete",
                                PortalSettings.GetStringResource("CONFIRM_DELETE_SCRIPT", s));
                        }

                        if (this.deleteModuleButton.Attributes["onclick"] != null)
                        {
                            this.deleteModuleButton.Attributes["onclick"] = string.Format(
                                "return confirmDelete();{0}", this.deleteModuleButton.Attributes["onclick"]);
                        }
                        else
                        {
                            this.deleteModuleButton.Attributes.Add("onclick", "return confirmDelete();");
                        }

                        this.deleteModuleButton.ServerClick += this.DeleteModuleButtonClick;
                    }
                }

                return this.deleteModuleButton;
            }
        }

        /// <summary>
        ///   Gets Module Down button
        /// </summary>
        public ModuleButton DownButton
        {
            get
            {
                if (this.downButton == null && HttpContext.Current != null)
                {
                    // check authority
                    if (this.CanArrows)
                    {
                        var sourceList = this.GetModules(this.ModuleConfiguration.PaneName.ToLower());
                        var m = sourceList[sourceList.Count - 1];
                        if (this.ModuleConfiguration.ModuleOrder != m.Order)
                        {
                            // create the button
                            this.downButton = new ModuleButton
                                {
                                    Group = ModuleButton.ButtonGroup.Admin,
                                    TranslationKey = "MOVE_DOWN",
                                    EnglishName = "Move down",
                                    Image = this.CurrentTheme.GetImage("Buttons_Down", "Down.gif")
                                };
                            this.downButton.Attributes.Add("direction", "down");
                            this.downButton.Attributes.Add("pane", this.ModuleConfiguration.PaneName.ToLower());
                            this.downButton.RenderAs = this.ButtonsRenderAs;
                            this.downButton.ServerClick += this.UpDownClick;
                        }
                    }
                }

                return this.downButton;
            }
        }

        /// <summary>
        ///   Gets module edit button
        /// </summary>
        public ModuleButton EditButton
        {
            get
            {
                if (this.editButton == null && HttpContext.Current != null)
                {
                    // check authority
                    if (this.CanEdit)
                    {
                        // create the button
                        this.editButton = new ModuleButton
                            {
                                Group = ModuleButton.ButtonGroup.Admin,
                                TranslationKey = this.EditText,
                                EnglishName = "Edit"
                            };
                        if (this.EditUrl.IndexOf("?") >= 0)
                        {
                            // Do not change if  the query string is present
                            // if ( this.ModuleID != OriginalModuleID )
                            this.editButton.HRef = this.EditUrl;
                        }
                        else
                        {
                            this.editButton.HRef = HttpUrlBuilder.BuildUrl(
                                this.EditUrl, this.PageID, string.Format("mID={0}", this.ModuleID));
                        }

                        if (this.editButton.HRef.Contains("/HtmlEdit.aspx"))
                            this.editButton.Attributes.Add("onclick", "openInModal('" + editButton.HRef + "','" + General.GetString("HTML_EDITOR", "Html Editor") + "');return false;");
                        else if (this.editButton.HRef.ToLower().Contains("/evoladvmodsettings.aspx"))
                            this.editButton.Attributes.Add("onclick", "openInModal('" + editButton.HRef + "','" + General.GetString("EVOL_ADVD_MODEL_SETTING_TITLE", "Evolutility Advanced Model Settings") + "');return false;");

                        this.editButton.Target = this.EditTarget;
                        this.editButton.Image = this.CurrentTheme.GetImage("Buttons_Edit", "Edit.gif");
                        this.editButton.RenderAs = this.ButtonsRenderAs;
                    }
                }

                return this.editButton;
            }
        }

        /// <summary>
        ///   Gets the path of the edit control if available.
        ///   Override on derivates classes.
        /// </summary>
        public virtual string EditModuleControl
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///   Gets or sets Target frame/page for Edit Link
        /// </summary>
        public string EditTarget
        {
            get
            {
                if (this.ModuleTitle != null && this.ModuleTitle.EditTarget.Length != 0)
                {
                    this.editUrl = this.ModuleTitle.EditTarget;
                }

                return this.editTarget;
            }

            set
            {
                this.editTarget = value;
            }
        }

        /// <summary>
        ///   Gets or sets Text for Edit Link
        /// </summary>
        public string EditText
        {
            get
            {
                if (this.ModuleTitle != null && this.ModuleTitle.EditText.Length != 0)
                {
                    this.editText = this.ModuleTitle.EditText;
                }

                return this.editText;
            }

            set
            {
                this.editText = value;
            }
        }

        /// <summary>
        ///   Gets or sets Url for Edit Link
        /// </summary>
        public string EditUrl
        {
            get
            {
                if (this.ModuleTitle != null && this.ModuleTitle.EditUrl.Length != 0)
                {
                    this.editUrl = this.ModuleTitle.EditUrl;
                }

                return this.editUrl;
            }

            set
            {
                this.editUrl = value;
            }
        }

        /// <summary>
        ///   Gets module button that will launch a pop-up window to allow the module contents to be emailed
        /// </summary>
        /// <remarks>
        ///   Not implemented yet.
        /// </remarks>
        public ModuleButton EmailButton
        {
            get
            {
                if (this.emailButton == null && HttpContext.Current != null)
                {
                    // check authority
                    if (this.CanEmail)
                    {
                        // not implemented
                        // javaScript = "EmailWindow=window.open('" 
                        // + HttpUrlBuilder.BuildUrl("email.aspx","src=" + portalModule.ModuleCacheKey + "content") 
                        // + "','EmailWindow','toolbar=yes,location=no,directories=no,status=no,menubar=yes,scrollbars=yes,resizable=yes,width=640,height=480,left=15,top=15'); return false;";
                        // EmailButton.Text = Esperantus.General.GetString("BTN_Email","Email this",null) + "...";
                        // EmailButton.NavigateUrl = string.Empty;
                        // EmailButton.CssClass = "rb_mod_btn";
                        // EmailButton.Attributes.Add("onclick", javaScript);
                        // EmailButton.ImageUrl = CurrentTheme.GetImage("Buttons_Email").ImageUrl;
                        // ButtonList.Add(EmailButton);
                    }

                    this.emailButton = null;
                }

                return this.emailButton;
            }
        }

        /// <summary>
        ///   Gets GUID of module (mandatory)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Guid GuidID
        {
            get
            {
                // 1.1.8.1324 - 24/01/2003
                throw new NotImplementedException("You must implement a unique GUID for your module");
            }
        }

        /// <summary>
        ///   Gets Module button that will launch the module help in a pop-up window
        /// </summary>
        public ModuleButton HelpButton
        {
            get
            {
                if (this.helpButton == null && HttpContext.Current != null)
                {
                    // check authority
                    if (this.CanHelp)
                    {
                        // build the HRef
                        var aux = this.ModuleConfiguration.DesktopSrc.Replace(".", "_");
                        var fileNameStart = aux.LastIndexOf("/");
                        var fileName = aux.Substring(fileNameStart + 1);
                        var sb = new StringBuilder();
                        sb.Append(Path.ApplicationRoot);
                        sb.Append(@"/rb_documentation/Viewer.aspx?loc=Appleseed/");
                        sb.Append(aux);
                        sb.Append("&src=");
                        sb.Append(fileName);

                        // create the button
                        this.helpButton = new ModuleButton
                            {
                                Group = ModuleButton.ButtonGroup.User,
                                TranslationKey = "BTN_HELP",
                                EnglishName = "Help",
                                HRef = sb.ToString(),
                                PopUp = true,
                                Target = "AppleseedHelp",
                                PopUpOptions =
                                    "toolbar=1,location=0,directories=0,status=0,menubar=1,scrollbars=1,resizable=1,width=600,height=400,screenX=15,screenY=15,top=15,left=15",
                                Image = this.CurrentTheme.GetImage("Buttons_Help", "Help.gif"),
                                RenderAs = this.ButtonsRenderAs
                            };
                    }
                }

                return this.helpButton;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the specified module supports workflow.
        ///   It returns the module property regardless of current module setting.
        /// </summary>
        /// <remarks>
        ///   changed Jes1111 (from 'internal')
        /// </remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool InnerSupportsWorkflow { get; set; }

        /// <summary>
        ///   Gets a value indicating whether this instance is addable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is addable; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsAddable
        {
            get
            {
                if (this.moduleConfiguration == null || this.moduleConfiguration.AuthorizedAddRoles == null)
                {
                    return false;
                }

                // Perform tri-state switch check to avoid having to perform a security
                // role lookup on every property access (instead caching the result)
                if (this.canAdd == 0)
                {
                    // Change by Geert.Audenaert@Syntegra.Com
                    // Date: 7/2/2003
                    if (this.SupportsWorkflow && this.Version == WorkFlowVersion.Production)
                    {
                        this.canAdd = 2;
                    }
                    else
                    {
                        // End Change Geert.Audenaert@Syntegra.Com
                        this.canAdd = PortalSecurity.IsInRoles(this.moduleConfiguration.AuthorizedAddRoles) ? 1 : 2;

                        // Change by Geert.Audenaert@Syntegra.Com
                    }

                    // Date: 7/2/2003
                    // End Change Geert.Audenaert@Syntegra.Com
                }

                return this.canAdd == 1;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance is delete-able.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is delete-able; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDeleteable
        {
            get
            {
                if (this.moduleConfiguration == null || this.moduleConfiguration.AuthorizedDeleteRoles == null)
                {
                    return false;
                }

                // Perform tri-state switch check to avoid having to perform a security
                // role lookup on every property access (instead caching the result)
                if (this.canDelete == 0)
                {
                    // Change by Geert.Audenaert@Syntegra.Com
                    // Date: 7/2/2003
                    if (this.SupportsWorkflow && this.Version == WorkFlowVersion.Production)
                    {
                        this.canDelete = 2;
                    }
                    else
                    {
                        // End Change Geert.Audenaert@Syntegra.Com
                        this.canDelete = PortalSecurity.IsInRoles(this.moduleConfiguration.AuthorizedDeleteRoles)
                                             ? 1
                                             : 2;

                        // Change by Geert.Audenaert@Syntegra.Com
                        // Date: 7/2/2003
                    }

                    // End Change Geert.Audenaert@Syntegra.Com
                }

                return this.canDelete == 1;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance is editable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is editable; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsEditable
        {
            get
            {
                if (this.moduleConfiguration == null || this.moduleConfiguration.AuthorizedEditRoles == null)
                {
                    return false;
                }

                // Perform tri-state switch check to avoid having to perform a security
                // role lookup on every property access (instead caching the result)
                if (this.canEdit == 0)
                {
                    // Change by Geert.Audenaert@Syntegra.Com
                    // Date: 7/2/2003
                    if (this.SupportsWorkflow && this.Version == WorkFlowVersion.Production)
                    {
                        this.canEdit = 2;
                    }
                    else
                    {
                        // End Change Geert.Audenaert@Syntegra.Com
                        // if (PortalSettings.AlwaysShowEditButton == true || PortalSecurity.IsInRoles(moduleConfiguration.AuthorizedEditRoles))
                        this.canEdit = PortalSecurity.IsInRoles(this.moduleConfiguration.AuthorizedEditRoles) ? 1 : 2;

                        // Change by Geert.Audenaert@Syntegra.Com
                        // Date: 7/2/2003
                    }

                    // End Change Geert.Audenaert@Syntegra.Com
                }

                return this.canEdit == 1;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance is viewable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is viewable; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsViewable
        {
            get
            {
                if (this.moduleConfiguration == null || this.moduleConfiguration.AuthorizedViewRoles == null)
                {
                    return false;
                }

                // Perform tri-state switch check to avoid having to perform a security
                // role lookup on every property access (instead caching the result)
                if (this.canView == 0)
                {
                    this.canView = PortalSecurity.IsInRoles(this.moduleConfiguration.AuthorizedViewRoles) ? 1 : 2;
                }

                return this.canView == 1;
            }
        }

        /// <summary>
        ///   Gets Module Left button
        /// </summary>
        public ModuleButton LeftButton
        {
            get
            {
                if (this.leftButton == null && HttpContext.Current != null)
                {
                    // check authority
                    if (this.CanArrows && this.ModuleConfiguration.PaneName.ToLower() != "leftpane")
                    {
                        var leftButtonTargetPane = "contentpane";
                        switch (this.ModuleConfiguration.PaneName.ToLower())
                        {
                            case "contentpane":
                                leftButtonTargetPane = "leftpane";
                                break;
                            case "rightpane":
                                leftButtonTargetPane = "contentpane";
                                break;
                        }

                        // create the button
                        this.leftButton = new ModuleButton
                            {
                                Group = ModuleButton.ButtonGroup.Admin,
                                TranslationKey = "MOVE_LEFT",
                                EnglishName = "Move left",
                                Image = this.CurrentTheme.GetImage("Buttons_Left", "Left.gif")
                            };
                        this.leftButton.Attributes.Add("sourcepane", this.ModuleConfiguration.PaneName.ToLower());
                        this.leftButton.Attributes.Add("targetpane", leftButtonTargetPane);
                        this.leftButton.RenderAs = this.ButtonsRenderAs;
                        this.leftButton.ServerClick += this.RightLeftClick;
                    }
                }

                return this.leftButton;
            }
        }

        /// <summary>
        ///   Gets unique key for module caching
        /// </summary>
        public string ModuleCacheKey
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    // Change 8/April/2003 Jes1111
                    // changes to Language behaviour require addition of culture names to cache key
                    // Jes1111 2003/04/24 - Added PortalAlias to cachekey
                    var PortalSettings1 = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
                    var sb = new StringBuilder();
                    sb.Append("rb_");
                    sb.Append(PortalSettings1.PortalAlias);
                    sb.Append("_mid");
                    sb.Append(this.ModuleID.ToString());
                    sb.Append("[");
                    sb.Append(PortalSettings1.PortalContentLanguage);
                    sb.Append("+");
                    sb.Append(PortalSettings1.PortalUILanguage);
                    sb.Append("+");
                    sb.Append(PortalSettings1.PortalDataFormattingCulture);
                    sb.Append("]");

                    return sb.ToString();
                }

                return null;
            }
        }

        /// <summary>
        ///   Gets or sets Configuration
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ModuleSettings ModuleConfiguration
        {
            get
            {
                return HttpContext.Current != null && this.moduleConfiguration != null ? this.moduleConfiguration : null;
            }

            set
            {
                this.moduleConfiguration = value;
            }
        }

        /// <summary>
        ///   Gets or sets the current ID of the module. Is unique for all portals.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ModuleID
        {
            get
            {
                try
                {
                    return this.moduleConfiguration.ModuleID;
                }
                catch
                {
                    return -1;
                }
            }

            set
            {
                // made changeable by Manu, please be careful on changing it
                this.moduleConfiguration.ModuleID = value;
                this.settings = null; // force cached settings to be reloaded
            }
        }

        /// <summary>
        ///   Gets or sets Inner Title control. Now only used for backward compatibility
        /// </summary>
        public virtual DesktopModuleTitle ModuleTitle { get; set; }

        /// <summary>
        ///   Gets or sets The ID of the original module (will be different to ModuleID when using shortcut module)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int OriginalModuleID
        {
            get
            {
                try
                {
                    return this.originalModuleId == -1 ? this.ModuleID : this.originalModuleId;
                }
                catch
                {
                    return -1;
                }
            }

            set
            {
                this.originalModuleId = value;
            }
        }

        /// <summary>
        ///   Gets the current page
        /// </summary>
        public new Page Page
        {
            get
            {
                return base.Page is Page ? (Page)base.Page : null;
            }
        }

        /// <summary>
        ///   Gets current linked module ID if applicable
        /// </summary>
        public int PageID
        {
            get
            {
                if (this.pageId == 0)
                {
                    this.Trace.Warn(string.Format("Request.Params['PageID'] = {0}", this.Request.Params["PageID"]));

                    // Determine PageID if specified
                    if (HttpContext.Current != null && this.Request.Params["PageID"] != null)
                    {
                        var pageIdString = this.Request.Params["PageID"];
                        this.pageId = Int32.Parse(pageIdString.Split(',')[0]);
                    }
                    else if (HttpContext.Current != null && this.Request.Params["TabID"] != null)
                    {
                        var pageIdString = this.Request.Params["TabID"];
                        this.pageId = Int32.Parse(pageIdString.Split(',')[0]);
                    }
                }

                return this.pageId;
            }
        }

        /// <summary>
        ///   Gets current tab settings
        /// </summary>
        public Dictionary<string, ISettingItem> PageSettings
        {
            get
            {
                return this.Page != null ? this.Page.PageSettings : null;
            }
        }

        /// <summary>
        ///   Gets or sets ID of portal in which module is instantiated
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PortalID { get; set; }

        /// <summary>
        ///   Gets current portal settings
        /// </summary>
        public PortalSettings PortalSettings
        {
            get
            {
                // Obtain PortalSettings from page else Current Context else null
                return this.Page != null
                           ? this.Page.PortalSettings
                           : (HttpContext.Current != null
                                  ? (PortalSettings)HttpContext.Current.Items["PortalSettings"]
                                  : null);
            }
        }

        /// <summary>
        ///   Gets Module button that will launch the module in a pop-up window suitable for printing
        /// </summary>
        public ModuleButton PrintButton
        {
            get
            {
                if (this.printButton == null && HttpContext.Current != null)
                {
                    // check authority
                    if (this.CanPrint)
                    {
                        // build the HRef
                        var url = new StringBuilder();
                        url.Append(Path.ApplicationRoot);
                        url.Append("/app_support/print.aspx?");
                        url.Append(this.Request.QueryString.ToString());
                        if (!(this.Request.QueryString.ToString().ToLower().IndexOf("mid=") > 0))
                        {
                            url.Append("&mID=");
                            url.Append(this.ModuleID.ToString());
                        }

                        url.Append("&ModId=");
                        url.Append(this.OriginalModuleID.ToString());

                        // create the button
                        this.printButton = new ModuleButton
                            {
                                Group = ModuleButton.ButtonGroup.User,
                                Image = this.CurrentTheme.GetImage("Buttons_Print", "Print.gif"),
                                TranslationKey = "BTN_PRINT",
                                EnglishName = "Print this",
                                HRef = url.ToString(),
                                PopUp = true,
                                Target = "AppleseedPrint",
                                PopUpOptions =
                                    "toolbar=1,menubar=1,resizable=1,scrollbars=1,width=790,height=400,left=15,top=15",
                                RenderAs = this.ButtonsRenderAs
                            };
                    }
                }

                return this.printButton;
            }
        }

        /// <summary>
        ///   Gets or sets text for version Link for swapping to production version
        /// </summary>
        public string ProductionVersionText { get; set; }

        /// <summary>
        ///   Gets Module Properties button
        /// </summary>
        public ModuleButton PropertiesButton
        {
            get
            {
                if (this.propertiesButton == null && HttpContext.Current != null)
                {
                    // check authority
                    if (this.CanProperties)
                    {
                        // create the button
                        this.propertiesButton = new ModuleButton
                            {
                                Group = ModuleButton.ButtonGroup.Admin,
                                EnglishName = "Properties",
                                TranslationKey = "PROPERTIES",
                                Image = this.CurrentTheme.GetImage("Buttons_Properties", "Properties.gif")
                            };
                        if (this.PropertiesUrl.IndexOf("?") >= 0)
                        {
                            // Do not change if  the querystring is present (shortcut patch)
                            // if ( this.ModuleID != OriginalModuleID ) // shortcut
                            this.propertiesButton.HRef = this.PropertiesUrl;
                        }
                        else
                        {
                            this.propertiesButton.HRef = HttpUrlBuilder.BuildUrl(
                                this.PropertiesUrl, this.PageID, string.Format("mID={0}", this.ModuleID));
                        }
                        this.propertiesButton.Attributes.Add("onclick", "openInModal('" + this.propertiesButton.HRef + "','" + General.GetString("MODULESETTINGS_SETTINGS", "Module Settings") + "');return false;");
                        this.propertiesButton.Target = this.PropertiesTarget;
                        this.propertiesButton.RenderAs = this.ButtonsRenderAs;
                    }
                }

                return this.propertiesButton;
            }
        }

        /// <summary>
        ///   Gets or sets Target frame/page for Properties Link
        /// </summary>
        public string PropertiesTarget
        {
            get
            {
                if (this.ModuleTitle != null && this.ModuleTitle.PropertiesTarget.Length != 0)
                {
                    this.propertiesTarget = this.ModuleTitle.PropertiesTarget;
                }

                return this.propertiesTarget;
            }

            set
            {
                this.propertiesTarget = value;
            }
        }

        /// <summary>
        ///   Gets or sets Text for Properties Link
        /// </summary>
        public string PropertiesText
        {
            get
            {
                if (this.ModuleTitle != null && this.ModuleTitle.PropertiesText.Length != 0)
                {
                    this.propertiesText = this.ModuleTitle.PropertiesText;
                }

                return this.propertiesText;
            }

            set
            {
                this.propertiesText = value;
            }
        }

        /// <summary>
        ///   Gets or sets Url for Properties Link
        /// </summary>
        public string PropertiesUrl
        {
            get
            {
                if (this.ModuleTitle != null && this.ModuleTitle.PropertiesUrl.Length != 0)
                {
                    this.propertiesUrl = this.ModuleTitle.PropertiesUrl;
                }

                return this.propertiesUrl;
            }

            set
            {
                this.propertiesUrl = value;
            }
        }

        /// <summary>
        ///   Gets Module Version button
        /// </summary>
        public ModuleButton PublishButton
        {
            get
            {
                if (this.publishButton == null && HttpContext.Current != null)
                {
                    // check authority
                    if (this.CanPublish)
                    {
                        // create the button
                        this.publishButton = new ModuleButton
                            {
                                Group = ModuleButton.ButtonGroup.Admin,
                                TranslationKey = this.PublishText,
                                EnglishName = "Publish"
                            };

                        // modified by Hongwei Shen
                        // publishButton.HRef = GetPublishUrl();
                        this.publishButton.ServerClick += this.PublishButtonServerClick;

                        // end of modification
                        this.publishButton.Image = this.CurrentTheme.GetImage("Buttons_Publish", "Publish.gif");
                        this.publishButton.RenderAs = this.ButtonsRenderAs;
                    }
                }

                return this.publishButton;
            }
        }

        /// <summary>
        ///   Gets or sets text for publish link
        /// </summary>
        public string PublishText { get; set; }

        /// <summary>
        ///   Gets Module ReadyToApprove button
        /// </summary>
        public ModuleButton ReadyToApproveButton
        {
            get
            {
                if (this.readyToApproveButton == null && HttpContext.Current != null)
                {
                    // check authority
                    if (this.CanRequestApproval)
                    {
                        // create the button
                        this.readyToApproveButton = new ModuleButton
                            {
                                Group = ModuleButton.ButtonGroup.Admin,
                                TranslationKey = this.ReadyToApproveText,
                                EnglishName = "Request approval",
                                HRef =
                                    HttpUrlBuilder.BuildUrl(
                                        "~/DesktopModules/Workflow/RequestModuleContentApproval.aspx",
                                        this.PageID,
                                        "mID=" + this.ModuleID),
                                Image = this.CurrentTheme.GetImage("Buttons_ReadyToApprove", "ReadyToApprove.gif"),
                                RenderAs = this.ButtonsRenderAs
                            };
                    }
                }

                return this.readyToApproveButton;
            }
        }

        /// <summary>
        ///   Gets or sets text for request approval link
        /// </summary>
        public string ReadyToApproveText { get; set; }

        /// <summary>
        ///   Gets Module Reject button
        /// </summary>
        public ModuleButton RejectButton
        {
            get
            {
                if (this.rejectButton == null && HttpContext.Current != null)
                {
                    // check authority
                    if (this.CanApproveReject)
                    {
                        // create the button
                        this.rejectButton = new ModuleButton
                            {
                                Group = ModuleButton.ButtonGroup.Admin,
                                TranslationKey = this.RejectText,
                                EnglishName = "Reject",
                                HRef =
                                    HttpUrlBuilder.BuildUrl(
                                        "~/DesktopModules/Workflow/RejectModuleContent.aspx",
                                        this.PageID,
                                        string.Format("mID={0}", this.ModuleID)),
                                Image = this.CurrentTheme.GetImage("Buttons_Reject", "Reject.gif"),
                                RenderAs = this.ButtonsRenderAs
                            };
                    }
                }

                return this.rejectButton;
            }
        }

        /// <summary>
        ///   Gets or sets Text for reject link
        /// </summary>
        public string RejectText { get; set; }

        /// <summary>
        ///   Gets Module Revert button
        /// </summary>
        public ModuleButton RevertButton
        {
            get
            {
                if (this.revertButton == null && HttpContext.Current != null)
                {
                    // check authority
                    if (this.CanRequestApproval)
                    {
                        // create the button
                        this.revertButton = new ModuleButton
                            {
                                Group = ModuleButton.ButtonGroup.Admin,
                                TranslationKey = this.RevertText,
                                EnglishName = "Revert",
                                Image = this.CurrentTheme.GetImage("Buttons_Revert", "Revert.gif")
                            };
                        this.revertButton.ServerClick += this.RevertToProductionContent;
                        this.revertButton.RenderAs = this.ButtonsRenderAs;
                    }
                }

                return this.revertButton;
            }
        }

        /// <summary>
        ///   Gets or sets the revert text.
        /// </summary>
        public string RevertText { get; set; }

        /// <summary>
        ///   Gets Module Right button
        /// </summary>
        public ModuleButton RightButton
        {
            get
            {
                if (this.rightButton == null && HttpContext.Current != null)
                {
                    // check authority
                    if (this.CanArrows && this.ModuleConfiguration.PaneName.ToLower() != "rightpane")
                    {
                        var rightButtonTargetPane = "contentpane";
                        switch (this.ModuleConfiguration.PaneName.ToLower())
                        {
                            case "contentpane":
                                rightButtonTargetPane = "rightpane";
                                break;
                            case "leftpane":
                                rightButtonTargetPane = "contentpane";
                                break;
                        }

                        // create the button
                        this.rightButton = new ModuleButton
                            {
                                Group = ModuleButton.ButtonGroup.Admin,
                                TranslationKey = "MOVE_RIGHT",
                                EnglishName = "Move right",
                                Image = this.CurrentTheme.GetImage("Buttons_Right", "Right.gif")
                            };
                        this.rightButton.Attributes.Add("sourcepane", this.ModuleConfiguration.PaneName.ToLower());
                        this.rightButton.Attributes.Add("targetpane", rightButtonTargetPane);
                        this.rightButton.RenderAs = this.ButtonsRenderAs;
                        this.rightButton.ServerClick += this.RightLeftClick;
                    }
                }

                return this.rightButton;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this <see cref = "PortalModuleControl" /> is searchable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if searchable; otherwise, <c>false</c>.
        /// </value>
        public virtual bool Searchable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        ///   Gets Module Security button
        /// </summary>
        public ModuleButton SecurityButton
        {
            get
            {
                if (this.securityButton == null && HttpContext.Current != null)
                {
                    // check authority
                    if (this.CanSecurity)
                    {
                        // create the button
                        this.securityButton = new ModuleButton
                            {
                                Group = ModuleButton.ButtonGroup.Admin,
                                EnglishName = "Security",
                                TranslationKey = "SECURITY",
                                Image = this.CurrentTheme.GetImage("Buttons_Security", "Security.gif")
                            };
                        if (this.SecurityUrl.IndexOf("?") >= 0)
                        {
                            // Do not change if  the querystring is present (shortcut patch)
                            this.securityButton.HRef = this.SecurityUrl;
                        }
                        else
                        {
                            this.securityButton.HRef = HttpUrlBuilder.BuildUrl(
                                this.SecurityUrl, this.PageID, string.Format("mID={0}", this.ModuleID));


                        }
                        this.securityButton.Attributes.Add("onclick", "openInModal('" + this.securityButton.HRef + "','" + General.GetString("MODULESETTINGS_BASE_SETTINGS", "Security and Workflow") + "');return false;");
                        this.securityButton.Target = this.SecurityTarget;
                        this.securityButton.RenderAs = this.ButtonsRenderAs;

                    }
                }

                return this.securityButton;
            }
        }

        /// <summary>
        ///   Gets or sets Target frame/page for Security Link
        /// </summary>
        public string SecurityTarget
        {
            get
            {
                if (this.ModuleTitle != null && this.ModuleTitle.SecurityTarget.Length != 0)
                {
                    this.securityTarget = this.ModuleTitle.SecurityTarget;
                }

                return this.securityTarget;
            }

            set
            {
                this.securityTarget = value;
            }
        }

        /// <summary>
        ///   Gets or sets Text for Security Link
        /// </summary>
        public string SecurityText
        {
            get
            {
                if (this.ModuleTitle != null && this.ModuleTitle.SecurityText.Length != 0)
                {
                    this.securityText = this.ModuleTitle.SecurityText;
                }

                return this.securityText;
            }

            set
            {
                this.securityText = value;
            }
        }

        /// <summary>
        ///   Gets or sets Url for Security Link
        /// </summary>
        public string SecurityUrl
        {
            get
            {
                if (this.ModuleTitle != null && this.ModuleTitle.SecurityUrl.Length != 0)
                {
                    this.securityUrl = this.ModuleTitle.SecurityUrl;
                }

                return this.securityUrl;
            }

            set
            {
                this.securityUrl = value;
            }
        }

        /// <summary>
        ///   Gets Module custom settings
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<string, ISettingItem> Settings
        {
            get
            {
                return this.settings ??
                       (this.settings = ModuleSettings.GetModuleSettings(this.ModuleID, this.BaseSettings));
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether ShareModule.
        /// </summary>
        public bool ShareModule
        {
            get
            {
                return HttpContext.Current != null && bool.Parse(this.Settings["SHARE_MODULE"].ToString());
            }

            set
            {
                if (HttpContext.Current != null)
                {
                    // if it is not design time
                    this.Settings["SHARE_MODULE"].Value = value;
                }
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether [show back].
        ///   Can be set from module code to indicate whether module should display Back button
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show back]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowBack { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether [show title].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show title]; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        ///   Note: won't turn off the display of Buttons like it used to! You can now have buttons displayed with no title text showing
        /// </remarks>
        public virtual bool ShowTitle
        {
            get
            {
                return HttpContext.Current != null && this.Settings.ContainsKey("MODULESETTINGS_SHOW_TITLE") &&
                       this.Settings["MODULESETTINGS_SHOW_TITLE"].ToBoolean(CultureInfo.InvariantCulture);
            }

            set
            {
                if (HttpContext.Current != null)
                {
                    // if it is not design time
                    this.Settings["MODULESETTINGS_SHOW_TITLE"].Value = value;
                }
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether to display of the module title text (not the buttons) in the print pop-up.
        /// </summary>
        public virtual bool ShowTitlePrint
        {
            get
            {
                return HttpContext.Current != null &&
                       bool.Parse(this.Settings["MODULESETTINGS_SHOW_TITLE_PRINT"].ToString());
            }

            set
            {
                if (HttpContext.Current != null)
                {
                    // if it is not design time
                    this.Settings["MODULESETTINGS_SHOW_TITLE_PRINT"].Value = value;
                }
            }
        }

        /// <summary>
        ///   Gets or sets Text for version Link for swapping to staging version
        /// </summary>
        public string StagingVersionText { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the specified module supports can be
        ///   collapsible (minimized/maximized/closed)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SupportCollapsable
        {
            get
            {
                return this.moduleConfiguration == null
                           ? this.supportsCollapsible
                           : Config.WindowMgmtControls && this.moduleConfiguration.SupportCollapsable;

                // jes1111 - return GlobalResources.SupportWindowMgmt && moduleConfiguration.SupportCollapsable;
            }

            set
            {
                this.supportsCollapsible = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the specified module supports arrows to move modules
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SupportsArrows
        {
            get
            {
                var returnValue = this.supportsArrows;

                if (this.PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_SHOW_MODULE_ARROWS") && this.PortalSettings.CustomSettings["SITESETTINGS_SHOW_MODULE_ARROWS"] != null)
                {
                    returnValue = returnValue &&
                                  bool.Parse(
                                      this.PortalSettings.CustomSettings["SITESETTINGS_SHOW_MODULE_ARROWS"].ToString());
                }

                if (this.PortalSettings.CustomSettings.ContainsKey("MODULESETTINGS_SHOW_ARROW_BUTTONS") && this.Settings["MODULESETTINGS_SHOW_ARROW_BUTTONS"] != null)
                {
                    returnValue = returnValue &&
                                  bool.Parse(this.Settings["MODULESETTINGS_SHOW_ARROW_BUTTONS"].ToString());
                }

                return returnValue;
            }

            set
            {
                this.supportsArrows = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the module supports a Back button
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SupportsBack { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the module supports email
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SupportsEmail { get; set; }

        /// <summary>
        ///   Gets a value indicating whether the specified module supports help
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SupportsHelp
        {
            get
            {
                if ((this.Settings.ContainsKey("MODULESETTINGS_SHOW_HELP_BUTTON") &&
                        this.Settings["MODULESETTINGS_SHOW_HELP_BUTTON"] != null &&
                     !bool.Parse(this.Settings["MODULESETTINGS_SHOW_HELP_BUTTON"].ToString())) ||
                    (this.ModuleConfiguration.DesktopSrc.Length == 0))
                {
                    return false;
                }

                var aux = string.Format(
                    "{0}/rb_documentation/Appleseed/{1}",
                    Path.ApplicationRoot,
                    this.ModuleConfiguration.DesktopSrc.Replace(".", "_"));
                return Directory.Exists(HttpContext.Current.Server.MapPath(aux));
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether [supports print].
        ///   Return true if the module supports print in pop-up window.
        ///   Override on derived class.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [supports print]; otherwise, <c>false</c>.
        /// </value>
        public bool SupportsPrint { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the specified module workflow is enabled.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SupportsWorkflow
        {
            get
            {
                return this.moduleConfiguration == null
                           ? this.InnerSupportsWorkflow
                           : this.InnerSupportsWorkflow && this.moduleConfiguration.SupportWorkflow;
            }

            set
            {
                this.InnerSupportsWorkflow = value;
            }
        }

        /// <summary>
        ///   Gets or sets the module title as it will be displayed on the page. Handles cultures automatically.
        /// </summary>
        public virtual string TitleText
        {
            get
            {
                if (HttpContext.Current != null && this.titleText == string.Empty)
                {
                    var key = string.Format("MODULESETTINGS_TITLE_{0}", this.PortalSettings.PortalContentLanguage.Name);
                    var setting = this.Settings.ContainsKey(key) ? this.Settings[key] : null;

                    // if it is not design time (and not overridden - Jes1111)
                    if (this.PortalSettings.PortalContentLanguage != CultureInfo.InvariantCulture && setting != null &&
                        setting.ToString().Length > 0)
                    {
                        this.titleText = setting.ToString();
                    }
                    else
                    {
                        this.titleText = this.ModuleConfiguration != null
                                             ? this.ModuleConfiguration.ModuleTitle
                                             : "TitleText Placeholder";
                    }
                }
                string title;
                if (PortalSecurity.HasEditPermissions(this.ModuleID))
                {
                    string callurl = string.Format("http://{0}{1}", Request.Url.Host, Page.ResolveUrl("~/Appleseed.Core/Home/SaveTitle"));

                    title = string.Format(
                       "<span id=\"mTitle_{0}\" class=\"editTitle\" onclick=EditTitleInLine(\'{2}\')>{1} </span>", this.ModuleID, this.titleText, callurl);
                }
                else
                    title = string.Format(
                        "<span id=\"mTitle_{0}\">{1}</span>", this.ModuleID, this.titleText);
                return title;
            }

            set
            {
                this.titleText = value;
            }
        }

        /// <summary>
        ///   Gets Module Up button
        /// </summary>
        public ModuleButton UpButton
        {
            get
            {
                if (this.buttonUp == null && HttpContext.Current != null)
                {
                    // check authority
                    if (this.CanArrows && this.ModuleConfiguration.ModuleOrder != 1)
                    {
                        // create the button
                        this.buttonUp = new ModuleButton
                            {
                                Group = ModuleButton.ButtonGroup.Admin,
                                TranslationKey = "MOVE_UP",
                                EnglishName = "Move up",
                                Image = this.CurrentTheme.GetImage("Buttons_Up", "Up.gif")
                            };
                        this.buttonUp.Attributes.Add("direction", "up");
                        this.buttonUp.Attributes.Add("pane", this.ModuleConfiguration.PaneName.ToLower());
                        this.buttonUp.RenderAs = this.ButtonsRenderAs;
                        this.buttonUp.ServerClick += this.UpDownClick;
                    }
                }

                return this.buttonUp;
            }
        }

        /// <summary>
        ///   Gets or sets which content will be shown to the user
        ///   production content or staging content
        /// </summary>
        /// <remarks>
        ///   Change by Geert.Audenaert@Syntegra.Com
        ///   Date: 6/2/2003
        /// </remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WorkFlowVersion Version
        {
            get
            {
                return this.SupportsWorkflow ? this.version : WorkFlowVersion.Staging;
            }

            set
            {
                if (value == WorkFlowVersion.Staging)
                {
                    if (!
                        (PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedAddRoles) ||
                         PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedDeleteRoles) ||
                         PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedEditRoles) ||
                         PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedPublishingRoles) ||
                         PortalSecurity.IsInRoles(this.ModuleConfiguration.AuthorizedApproveRoles)))
                    {
                        PortalSecurity.AccessDeniedEdit();
                    }
                }

                this.version = value;
            }
        }

        /// <summary>
        ///   Gets Module Version button
        /// </summary>
        public ModuleButton VersionButton
        {
            get
            {
                if (this.versionButton == null && HttpContext.Current != null)
                {
                    // check authority
                    if (this.CanVersion)
                    {
                        // create the button
                        this.versionButton = new ModuleButton { Group = ModuleButton.ButtonGroup.Admin };
                        if (this.Version == WorkFlowVersion.Staging)
                        {
                            this.versionButton.TranslationKey = this.ProductionVersionText;
                            this.versionButton.EnglishName = "To production version";
                            this.versionButton.Image = this.CurrentTheme.GetImage(
                                "Buttons_VersionToProduction", "VersionToProduction.gif");
                        }
                        else
                        {
                            this.versionButton.TranslationKey = this.StagingVersionText;
                            this.versionButton.EnglishName = "To staging version";
                            this.versionButton.Image = this.CurrentTheme.GetImage(
                                "Buttons_VersionToStaging", "VersionToStaging.gif");
                        }

                        this.versionButton.HRef = this.GetOtherVersionUrl();
                        this.versionButton.Target = this.EditTarget;
                        this.versionButton.RenderAs = this.ButtonsRenderAs;
                    }
                }

                return this.versionButton;
            }
        }

        /// <summary>
        ///   Gets the staging content state
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WorkflowState WorkflowStatus
        {
            get
            {
                return this.SupportsWorkflow ? this.moduleConfiguration.WorkflowStatus : WorkflowState.Original;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether module has inner control of type title
        /// </summary>
        /// <remarks>
        ///   Left here for backward compatibility until it proves redundant
        /// </remarks>
        protected bool HasTitle
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        ///   Gets the settings.
        /// </summary>
        /// <remarks>
        /// </remarks>
        IDictionary<string, ISettingItem> ISettingHolder.Settings
        {
            get
            {
                return this.Settings;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Used by Content to fetch module content, by raising Init and Load events on the module.
        /// </summary>
        /// <returns>
        /// The content.
        /// </returns>
        public virtual object GetContent()
        {
            this.OnInit(new EventArgs());
            this.OnLoad(new EventArgs());
            return this.Content;
        }

        /// <summary>
        /// Returns the "Last Modified" string, or an empty string if option is not active.
        /// </summary>
        /// <returns>
        /// The get last modified.
        /// </returns>
        public string GetLastModified()
        {
            // CHANGE by david.verberckmoes@syntegra.com on june, 2 2003
            if (((ISettingItem<bool>)this.PortalSettings.CustomSettings["SITESETTINGS_SHOW_MODIFIED_BY"]).Value &&
                ((ISettingItem<bool>)this.Settings["MODULESETTINGS_SHOW_MODIFIED_BY"]).Value)
            {
                // Get stuff from database
                var email = string.Empty;
                var timeStamp = DateTime.MinValue;
                WorkFlowDB.GetLastModified(this.ModuleID, this.Version, ref email, ref timeStamp);

                // Do some checking
                if (email == string.Empty)
                {
                    return string.Empty;
                }

                // Check if email address is valid
                var eal = new EmailAddressList();
                try
                {
                    eal.Add(email);
                    email = string.Format("<a href=\"mailto:{0}\">{1}</a>", email, email);
                }
                catch
                {
                }

                // Construct the rest of the html
                return string.Format(
                    "<span class=\"LastModified\">{0}&#160;{1}&#160;{2}&#160;{3} {4}</span>",
                    General.GetString("LMB_LAST_MODIFIED_BY"),
                    email,
                    General.GetString("LMB_ON"),
                    timeStamp.ToLongDateString(),
                    timeStamp.ToShortTimeString());
            }

            return string.Empty;

            // END CHANGE by david.verberckmoes@syntegra.com on june, 2 2003
        }

        /// <summary>
        /// The get users that can add.
        /// </summary>
        /// <returns>
        /// A list of usernames.
        /// </returns>
        public List<string> GetUsersThatCanAdd()
        {
            var roles = this.ModuleConfiguration.AuthorizedAddRoles;
            return GetUsersInRoles(roles);
        }

        /// <summary>
        /// The get users that can edit.
        /// </summary>
        /// <returns>
        /// A list of usernames.
        /// </returns>
        public List<string> GetUsersThatCanEdit()
        {
            var roles = this.ModuleConfiguration.AuthorizedEditRoles;
            return GetUsersInRoles(roles);
        }

        /// <summary>
        /// Gets the users that can view.
        /// </summary>
        /// <returns>
        /// A list of usernames.
        /// </returns>
        public List<string> GetUsersThatCanView()
        {
            var roles = this.ModuleConfiguration.AuthorizedViewRoles;
            return GetUsersInRoles(roles);
        }

        /// <summary>
        /// Override on derivates classes
        ///   Method to initialize custom settings values (such as lists) 
        ///   only when accessing the edition mode (and not in every class constructor)
        /// </summary>
        public virtual void InitializeCustomSettings()
        {
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <param name="dataSet">
        /// The data set to load.
        /// </param>
        /// <returns>
        /// The loaded data set.
        /// </returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual DataSet LoadData(DataSet dataSet)
        {
            return dataSet;
        }

        /// <summary>
        /// Saves the data.
        /// </summary>
        /// <param name="dataSet">
        /// The data set to save..
        /// </param>
        /// <returns>
        /// The saved data set.
        /// </returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual DataSet SaveData(DataSet dataSet)
        {
            return dataSet;
        }

        #endregion

        #region Implemented Interfaces

        #region IInstaller

        /// <summary>
        /// Commits using the specified state container.
        /// </summary>
        /// <param name="stateContainer">
        /// The state container.
        /// </param>
        public virtual void Commit(IDictionary stateContainer)
        {
        }

        /// <summary>
        /// Installs using the specified state container.
        /// </summary>
        /// <param name="stateContainer">
        /// The state container.
        /// </param>
        public virtual void Install(IDictionary stateContainer)
        {
        }

        /// <summary>
        /// Roll back using the specified state container.
        /// </summary>
        /// <param name="stateContainer">
        /// The state container.
        /// </param>
        public virtual void Rollback(IDictionary stateContainer)
        {
        }

        /// <summary>
        /// Uninstalls using the specified state container.
        /// </summary>
        /// <param name="stateContainer">
        /// The state container.
        /// </param>
        public virtual void Uninstall(IDictionary stateContainer)
        {
        }

        #endregion

        #region ISearchable

        /// <summary>
        /// Searchable module implementation
        /// </summary>
        /// <param name="portalId">
        /// The portal ID
        /// </param>
        /// <param name="userId">
        /// ID of the user is searching
        /// </param>
        /// <param name="searchString">
        /// The text to search
        /// </param>
        /// <param name="searchField">
        /// The fields where perfoming the search
        /// </param>
        /// <returns>
        /// The SELECT sql to perform a search on the current module
        /// </returns>
        public virtual string SearchSqlSelect(int portalId, int userId, string searchString, string searchField)
        {
            return string.Empty;
        }

        #endregion

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
            ModuleSettings.UpdateModuleSetting(this.ModuleID, settingItem.EnglishName, settingItem.Value.ToString());
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Builds the "with theme" versions of the module, with optional Title, Buttons and Body.
        /// </summary>
        protected virtual void Build()
        {
            if (!this.buildTitle && !this.buildButtons)
            {
                this.header.Controls.Add(this.CurrentTheme.GetLiteralControl("ControlNoTitleStart"));
            }
            else
            {
                this.header.Controls.Add(this.CurrentTheme.GetLiteralControl("ControlTitleStart"));

                this.header.Controls.Add(this.CurrentTheme.GetLiteralControl("TitleStart"));

                if (this.buildTitle)
                {
                    this.header.Controls.Add(new LiteralControl(this.TitleText));
                }

                this.header.Controls.Add(this.CurrentTheme.GetLiteralControl("TitleMiddle"));

                if (this.buildButtons)
                {
                    foreach (var button in this.buttonList)
                    {
                        this.header.Controls.Add(this.CurrentTheme.GetLiteralControl("TitleBeforeButton"));
                        this.header.Controls.Add(button);
                        this.header.Controls.Add(this.CurrentTheme.GetLiteralControl("TitleAfterButton"));
                    }
                }

                this.header.Controls.Add(this.CurrentTheme.GetLiteralControl("TitleEnd"));
            }

            if (this.buildBody)
            {
                this.footer.Controls.Add(new LiteralControl(this.GetLastModified()));
            }
            else
            {
                for (var i = 1; i < this.Controls.Count; i++)
                {
                    this.Controls[i].Visible = false;
                }
            }

            // Added by gman3001: 2004/10/26 to support auto width sizing and scrollable module content
            // this must be the first footer control (besides custom ones such as GetLastModified)
            if (this.buildBody)
            {
                this.footer.Controls.Add(this.BuildModuleContentContainer(false));
            }

            // changed Jes1111: https://sourceforge.net/tracker/index.php?func=detail&aid=1034935&group_id=66837&atid=515929
            if (!this.buildTitle && !this.buildButtons)
            {
                this.footer.Controls.Add(this.CurrentTheme.GetLiteralControl("ControlNoTitleEnd"));
            }
            else
            {
                this.header.Controls.Add(this.CurrentTheme.GetLiteralControl("ControlTitleBeforeControl"));

                // Changed Rob Siera: Incorrect positioning of ControlTitleAfterControl
                // this._footer.Controls.AddAt(0, CurrentTheme.GetLiteralControl("ControlTitleAfterControl"));
                this.footer.Controls.Add(this.CurrentTheme.GetLiteralControl("ControlTitleAfterControl"));
                this.footer.Controls.Add(this.CurrentTheme.GetLiteralControl("ControlTitleEnd"));
            }

            // Added by gman3001: 2004/10/26 to support auto width sizing and scrollable module content
            // this must be the last header control
            if (this.buildBody)
            {
                this.header.Controls.Add(this.BuildModuleContentContainer(true));
            }

            if (!this.canPrint && this.header.Controls.Count > 0 && this.footer.Controls.Count > 0)
            {
                // Process the first header control as the module's outer most begin tag element
                this.ProcessModuleStrecthing(this.header.Controls[0], true);

                // Process the last footer control as the module's outer most end tag element
                this.ProcessModuleStrecthing(this.footer.Controls[this.footer.Controls.Count - 1], false);
            }

            // End add by gman3001
        }

        /// <summary>
        /// Builds the three public button lists
        /// </summary>
        protected virtual void BuildButtonLists()
        {
            // user buttons
            if (this.BackButton != null)
            {
                this.ButtonListUser.Add(this.BackButton);
            }

            if (this.PrintButton != null)
            {
                this.ButtonListUser.Add(this.PrintButton);
            }

            if (this.HelpButton != null)
            {
                this.ButtonListUser.Add(this.HelpButton);
            }

            // admin buttons
            if (this.AddButton != null)
            {
                this.ButtonListAdmin.Add(this.AddButton);
            }

            if (this.EditButton != null)
            {
                this.ButtonListAdmin.Add(this.EditButton);
            }

            if (this.PropertiesButton != null)
            {
                this.ButtonListAdmin.Add(this.PropertiesButton);
            }

            if (this.SecurityButton != null)
            {
                this.ButtonListAdmin.Add(this.SecurityButton);
            }

            if (this.DeleteModuleButton != null)
            {
                this.ButtonListAdmin.Add(this.DeleteModuleButton);
            }

            if (this.VersionButton != null)
            {
                this.ButtonListAdmin.Add(this.VersionButton);
            }

            if (this.PublishButton != null)
            {
                this.ButtonListAdmin.Add(this.PublishButton);
            }

            if (this.ApproveButton != null)
            {
                this.ButtonListAdmin.Add(this.ApproveButton);
            }

            if (this.RejectButton != null)
            {
                this.ButtonListAdmin.Add(this.RejectButton);
            }

            if (this.ReadyToApproveButton != null)
            {
                this.ButtonListAdmin.Add(this.ReadyToApproveButton);
            }

            if (this.RevertButton != null)
            {
                this.ButtonListAdmin.Add(this.RevertButton);
            }

            if (this.UpButton != null)
            {
                this.ButtonListAdmin.Add(this.UpButton);
            }

            if (this.DownButton != null)
            {
                this.ButtonListAdmin.Add(this.DownButton);
            }

            if (this.LeftButton != null)
            {
                this.ButtonListAdmin.Add(this.LeftButton);
            }

            if (this.RightButton != null)
            {
                this.ButtonListAdmin.Add(this.RightButton);
            }

            // custom buttons
            // recover any CustomButtons set the 'old way'
            if (this.ModuleTitle != null)
            {
                foreach (Control c in this.ModuleTitle.CustomButtons)
                {
                    this.ButtonListCustom.Add(c);
                }
            }

            // if ( MinMaxButton != null )
            // ButtonListCustom.Add( MinMaxButton );
            // if ( CloseButton != null )
            // ButtonListCustom.Add( CloseButton );

            // set image url for standard buttons edit & delete
            if (this.DeleteBtn != null)
            {
                this.DeleteBtn.ImageUrl = this.CurrentTheme.GetImage("Buttons_Delete", "Delete.gif").ImageUrl;
            }

            if (this.EditBtn != null)
            {
                this.EditBtn.ImageUrl = this.CurrentTheme.GetImage("Buttons_Edit", "Edit.gif").ImageUrl;
            }
        }

        /// <summary>
        /// Makes the decisions about what needs to be built and calls the appropriate method
        /// </summary>
        protected virtual void BuildControlHierarchy()
        {
            if (this.NamingContainer.ToString().EndsWith("ASP.print_aspx"))
            {
                this.canPrint = true;
                this.buildButtons = false;
                if (!this.ShowTitlePrint)
                {
                    this.buildTitle = false;
                }

                // else if ( SupportCollapsable && UserDesktop.isMinimized( ModuleID ) ) {
                // buildBody = false;
                // }
            }
            else if (!this.ShowTitle)
            {
                this.buildTitle = false;
            }

            // added Jes1111: https://sourceforge.net/tracker/index.php?func=detail&aid=1034935&group_id=66837&atid=515929
            if (this.buttonList.Count == 0)
            {
                this.buildButtons = false;
            }

            // changed Jes1111 - 2004-09-29 - to correct shortcut behaviour
            if (this.ModuleID != this.OriginalModuleID)
            {
                this.BuildShortcut();
            }
            else if (!this.ApplyTheme || this.canPrint)
            {
                this.BuildNoTheme();
            }
            else if (this.CurrentTheme.Type.Equals("zen"))
            {
                this.ZenBuild();
            }
            else if (this.CurrentTheme.Type.Equals("htm"))
            {
                this.HtmBuild();
            }
            else
            {
                this.Build();
            }

            // wrap the module in a &lt;div&gt; with the ModuleID and Module type name - needed for Zen support and useful for CSS styling and debugging ;-)
            // Added generic class name ModuleWrap useful for more generic CSS styling - Rob Siera 2004-12-30
            var css =
                new LiteralControl(
                    string.Format("<div id=\"mID{0}\" class=\"{1} ModuleWrap\">", this.ModuleID, this.GetType().Name));
            this.header.Controls.AddAt(0, css);
            this.footer.Controls.Add(new LiteralControl("</div>"));
        }

        /// <summary>
        /// Method builds "no theme" version of module. Now obeys ShowTitle and GetLastModified.
        /// </summary>
        protected virtual void BuildNoTheme()
        {
            var t = new Table();
            t.Attributes.Add("width", "100%");
            t.CssClass = "TitleNoTheme";
            var tr = new TableRow();
            t.Controls.Add(tr);
            TableCell tc;

            if (this.buildTitle)
            {
                tc = new TableCell();
                tc.Attributes.Add("width", "100%");
                tc.CssClass = "TitleNoTheme";
                tc.Controls.Add(new LiteralControl(this.TitleText));
                tr.Controls.Add(tc);
            }

            if (this.buildButtons)
            {
                foreach (var button in this.buttonList)
                {
                    tc = new TableCell();
                    tc.Controls.Add(button); // Add Button
                    tr.Controls.Add(tc);
                }
            }

            if (this.buildTitle || this.buildButtons)
            {
                this.header.Controls.Add(t);
            }

            if (this.buildBody)
            {
                this.footer.Controls.Add(new LiteralControl(this.GetLastModified()));
            }
            else
            {
                foreach (var control in this.Controls.Cast<Control>())
                {
                    control.Visible = false;
                }
            }
        }

        /// <summary>
        /// Builds the shortcut.
        /// </summary>
        protected virtual void BuildShortcut()
        {
            // do nothing - just passes the target contents through. The theme will be applied
            // to the containing shortcut module.
        }

        /// <summary>
        /// Builds the "with theme" versions of the module using html, with optional Title, Buttons and Body.
        /// </summary>
        protected virtual void HtmBuild()
        {
            var template = this.CurrentTheme.GetThemePart("ModuleLayout");

            var title = this.ShowTitle ? this.TitleText : "&nbsp;";
            template = template.Replace("{Title}", title);
            template = template.Replace("{TitleStyle}", this.ShowTitle ? "display:inline" : "display:none");
            template = template.Replace("{NoTitleStyle}", this.ShowTitle ? "display:none" : "display:inline");
            template = template.Replace("{BodyBgColor}", this.CurrentTheme.GetThemePart("DefaultBodyBgColor"));
            template = template.Replace("{TitleBgColor}", this.CurrentTheme.GetThemePart("DefaultTitleBgColor"));

            var indexOfControlPanel = template.IndexOf("{ControlPanel}");
            var indexOfBody = template.IndexOf("{Body}");

            // var indexOfModifiedBy = template.IndexOf("{ModifiedBy}");
            if (indexOfControlPanel < indexOfBody)
            {
                if (indexOfControlPanel != -1)
                {
                    // Both Ctrl & Body : ....Ctrl....Body.....
                    this.header.Controls.Add(new LiteralControl(template.Substring(0, indexOfControlPanel)));
                    this.HtmRenderButtons(this.header);
                    this.header.Controls.Add(
                        new LiteralControl(
                            template.Substring(indexOfControlPanel + 14, indexOfBody - (indexOfControlPanel + 14))));

                    // base.Render(output);
                    this.footer.Controls.Add(new LiteralControl(template.Substring(indexOfBody + 6)));
                }
                else
                {
                    if (indexOfBody == -1)
                    {
                        // No Ctrl No Body...
                        // base.Render(output);
                    }
                    else
                    {
                        // Only Body: ...Body...
                        this.header.Controls.Add(new LiteralControl(template.Substring(0, indexOfBody)));

                        // base.Render(output);
                        this.footer.Controls.Add(new LiteralControl(template.Substring(indexOfBody + 6)));
                    }
                }
            }
            else
            {
                if (indexOfBody != -1)
                {
                    // Both Ctrl & Body : ....Body....Ctrl.....
                    this.header.Controls.Add(new LiteralControl(template.Substring(0, indexOfBody)));

                    // base.Render(output);
                    this.footer.Controls.Add(
                        new LiteralControl(template.Substring(indexOfBody + 6, indexOfControlPanel - (indexOfBody + 6))));
                    this.HtmRenderButtons(this.footer);
                    this.footer.Controls.Add(new LiteralControl(template.Substring(indexOfControlPanel + 14)));
                }
                else
                {
                    if (indexOfControlPanel == -1)
                    {
                        // No Ctrl No Body...
                        // base.Render(output);
                    }
                    else
                    {
                        // Only Ctrl: ...Ctrl...
                        this.header.Controls.Add(new LiteralControl(template.Substring(0, indexOfControlPanel)));
                        this.HtmRenderButtons(this.header);
                        this.footer.Controls.Add(new LiteralControl(template.Substring(indexOfControlPanel + 14)));
                    }
                }
            }
        }

        /// <summary>
        /// Merges the three public button lists into one.
        /// </summary>
        protected virtual void MergeButtonLists()
        {
            if (this.CurrentTheme.Type != "zen")
            {
                string divider;
                try
                {
                    divider = this.CurrentTheme.GetHTMLPart("ButtonGroupsDivider");
                }
                catch
                {
                    divider = string.Concat(
                        "<img src='",
                        this.CurrentTheme.GetImage("Spacer", "Spacer.gif").ImageUrl,
                        "' class='rb_mod_title_sep'/>");
                }

                // merge the button lists
                if (this.ButtonListUser.Count > 0 && (this.ButtonListCustom.Count > 0 || this.ButtonListAdmin.Count > 0))
                {
                    this.ButtonListUser.Add(new LiteralControl(divider));
                }

                if (this.ButtonListCustom.Count > 0 && this.ButtonListAdmin.Count > 0)
                {
                    this.ButtonListCustom.Add(new LiteralControl(divider));
                }
            }

            foreach (var btn in this.ButtonListUser)
            {
                this.buttonList.Add(btn);
            }

            foreach (var btn in this.ButtonListAdmin)
            {
                this.buttonList.Add(btn);
            }

            foreach (var btn in this.ButtonListCustom)
            {
                this.buttonList.Add(btn);
            }
        }

        /// <summary>
        /// Called when [delete].
        /// </summary>
        protected virtual void OnDelete()
        {
            WorkFlowDB.SetLastModified(this.ModuleID, MailHelper.GetCurrentUserEmailAddress());
        }

        /// <summary>
        /// Called when [edit].
        /// </summary>
        protected virtual void OnEdit()
        {
            WorkFlowDB.SetLastModified(this.ModuleID, MailHelper.GetCurrentUserEmailAddress());
        }

        /// <summary>
        /// Handles FlushCache event at Module level<br/>
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
            if (HttpContext.Current != null)
            {
                this.Context.Cache.Remove(this.ModuleCacheKey);
                Debug.WriteLine(string.Format("************* Remove {0}", this.ModuleCacheKey));
            }

            // any other code goes here
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"/> object that contains the event data.
        /// </param>
        protected override void OnInit(EventArgs e)
        {
            this.Controls.AddAt(0, this.headerPlaceHolder);

            if (this.DeleteBtn != null)
            {
                // Assign current permissions to Delete button
                if (this.IsDeleteable == false)
                {
                    this.DeleteBtn.Visible = false;
                }
                else
                {
                    this.DeleteBtn.Visible = true;

                    if (!this.Page.ClientScript.IsClientScriptBlockRegistered("confirmDelete"))
                    {
                        string[] s = { "CONFIRM_DELETE" };
                        this.Page.ClientScript.RegisterClientScriptBlock(
                            this.GetType(),
                            "confirmDelete",
                            PortalSettings.GetStringResource("CONFIRM_DELETE_SCRIPT", s));
                    }

                    if (this.DeleteBtn.Attributes["onclick"] != null)
                    {
                        this.DeleteBtn.Attributes["onclick"] = string.Format(
                            "return confirmDelete();{0}", this.DeleteBtn.Attributes["onclick"]);
                    }
                    else
                    {
                        this.DeleteBtn.Attributes.Add("onclick", "return confirmDelete();");
                    }

                    this.DeleteBtn.Click += this.DeleteButtonClick;
                    this.DeleteBtn.AlternateText = General.GetString("DELETE");
                    this.DeleteBtn.EnableViewState = false;
                }
            }

            if (this.EditBtn != null)
            {
                // Assign current permissions to Edit button
                if (this.IsEditable == false)
                {
                    this.EditBtn.Visible = false;
                }
                else
                {
                    this.EditBtn.Visible = true;
                    this.EditBtn.Click += this.EditButtonClick;
                    this.EditBtn.AlternateText = General.GetString("Edit");
                    this.EditBtn.EnableViewState = false;
                }
            }

            if (this.UpdateButton != null)
            {
                this.UpdateButton.Click += this.UpdateButtonClick;
                this.UpdateButton.Text = General.GetString("UPDATE");

                // updateButton.CssClass = "CommandButton"; // Jes1111 - set in .ascx
                this.UpdateButton.EnableViewState = false;
            }

            base.OnInit(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> object that contains the event data.
        /// </param>
        /// <remarks>
        /// Change by Geert.Audenaert@Syntegra.Com
        ///   Date: 7/2/2003
        /// </remarks>
        protected override void OnLoad(EventArgs e)
        {
            // First Check if the version is specified
            string versionLocal = null;
            try
            {
                versionLocal =
                    this.Page.Request.QueryString[string.Format("wversion{0}", this.ModuleConfiguration.ModuleID)];
            }
            catch (NullReferenceException)
            {
                // string message = ex.Message;
            }

            if (versionLocal != null)
            {
                var requestedVersion = versionLocal == "Staging" ? WorkFlowVersion.Staging : WorkFlowVersion.Production;
                if (requestedVersion != this.Version)
                {
                    this.Version = requestedVersion;
                    this.OnVersionSwap();
                }
            }

            /* modified by Hongwei Shen (hongwei.shen@gmail.com) 8/9/2005
             * The publishing business is moved to the PublishButton click server event 
             * handler

            #region check if publish required

            // Now check if this module needs to published
            string publish = null;
            try
            {
                publish = Page.Request.QueryString["wpublish" + ModuleConfiguration.ModuleID.ToString()];
            }
            catch (NullReferenceException)
            {
                //string message = ex.Message;
            }

            if (publish == "doit")
            {
                // Check if the user has publish permissions on this
                if (! PortalSecurity.IsInRoles(ModuleConfiguration.AuthorizedPublishingRoles))
                    PortalSecurity.AccessDeniedEdit();

                Publish();
            }

            #endregion
             * end of modification
             */
            if (this.ModuleConfiguration != null)
            {
                this.ModuleConfiguration.Cacheable = this.Cacheable;
            }

            this.SetupTheme();

            // Check for window management begin

            // bja@reedtek.com - does this configuration support window mgmt controls?
            // jes1111 - if (GlobalResources.SupportWindowMgmt && SupportCollapsable)
            if (Config.WindowMgmtControls && this.SupportCollapsable)
            {
                this.vcm = new ViewControlManager(this.PageID, this.ModuleID, HttpContext.Current.Request.RawUrl);
            }

            // Check for window management end
            this.BuildButtonLists();

            this.MergeButtonLists();

            // Then call inherited member
            base.OnLoad(e);

            this.BuildControlHierarchy();
            this.headerPlaceHolder.Controls.Add(this.header);
            this.Controls.Add(this.footer);
        }

        /// <summary>
        /// Handles OnUpdate event at Page level<br/>
        ///   Performs OnUpdate actions that are common to all Pages<br/>
        ///   Can be overridden
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

            // any other code goes here
            WorkFlowDB.SetLastModified(this.ModuleID, MailHelper.GetCurrentUserEmailAddress());
        }

        /// <summary>
        /// Called when [version swap].
        /// </summary>
        /// <remarks>
        /// Change by Geert.Audenaert@Syntegra.Com
        ///   Date: 7/2/2003
        /// </remarks>
        protected virtual void OnVersionSwap()
        {
        }

        /// <summary>
        /// Publish staging to production
        /// </summary>
        protected virtual void Publish()
        {
            // Publish module
            WorkFlowDB.Publish(this.ModuleConfiguration.ModuleID);

            // Show the prod version
            this.Version = WorkFlowVersion.Production;
        }

        /// <summary>
        /// Reverts the content to production.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected void RevertToProductionContent(object sender, EventArgs e)
        {
            // Revert
            WorkFlowDB.Revert(this.ModuleID);

            // Refresh current screen
            var querystring = "?";

            // Modified by Hongwei Shen(hongwei.shen@gmail.com) 8/9/2005
            // the key-value pairs are not separated by '&'.
            /* original code
            foreach (string key in Page.Request.QueryString.Keys)
                querystring += key + "=" + Context.Server.UrlEncode(Page.Request.QueryString[key]);
            */

            // start of modification
            var i = 0;
            var totalKeys = this.Page.Request.QueryString.Keys.Count;
            foreach (string key in this.Page.Request.QueryString.Keys)
            {
                querystring += key + "=" + this.Context.Server.UrlEncode(this.Page.Request.QueryString[key]);
                if (i < totalKeys - 1)
                {
                    querystring += '&';
                }

                i++;
            }

            // the call to stored-procedure rb_revert will reset
            // the WorkflowStatus to 0 (original) and we also need
            // to synchronize the module configuration to remove the
            // ReadyToApprove and Revert buttons.
            this.moduleConfiguration.WorkflowStatus = 0;

            // end of modification
            this.Context.Server.Transfer(this.Page.Request.Path + querystring);
        }

        /// <summary>
        /// Sets the CurrentTheme - allowing custom Theme per module
        /// </summary>
        protected virtual void SetupTheme()
        {
            // changed: Jes1111 - 2004-08-05 - supports custom theme per module
            // (better to do this in OnLoad than in RenderChildren, which is too late)
            var themeName = this.Settings.ContainsKey("MODULESETTINGS_THEME") && Int32.Parse(this.Settings["MODULESETTINGS_THEME"].ToString()) == (int)ThemeList.Alt
                                ? "Alt"
                                : "Default";

            // end: Jes1111

            // added: Jes1111 - 2004-08-05 - supports custom theme per module
            if (this.PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_ALLOW_MODULE_CUSTOM_THEMES") &&
                this.PortalSettings.CustomSettings["SITESETTINGS_ALLOW_MODULE_CUSTOM_THEMES"].ToString().Length != 0 &&
                bool.Parse(this.PortalSettings.CustomSettings["SITESETTINGS_ALLOW_MODULE_CUSTOM_THEMES"].ToString()) &&
                this.Settings.ContainsKey("MODULESETTINGS_MODULE_THEME") &&
                this.Settings["MODULESETTINGS_MODULE_THEME"].ToString().Trim().Length > 0)
            {
                // substitute custom theme for this module
                var tm = new ThemeManager(this.PortalSettings.PortalPath);
                tm.Load(this.Settings["MODULESETTINGS_MODULE_THEME"].ToString());
                this.CurrentTheme = tm.CurrentTheme;

                // get CSS file, add ModuleID to each line and add resulting string to CssImportList
                try
                {
                    var cssHelper = new CssHelper();
                    var selectorPrefix = string.Concat("#mID", this.ModuleID);
                    var cssFileName = this.Page.Server.MapPath(this.CurrentTheme.CssFile);
                    this.Page.RegisterCssImport(
                        this.ModuleID.ToString(), cssHelper.ParseCss(cssFileName, selectorPrefix));
                }
                catch (Exception ex)
                {
                    var error =
                        string.Format(
                            "Failed to load custom theme '{0}' for ModuleID {1}. Continuing with default tab theme. Message was: {2}",
                            this.CurrentTheme.CssFile,
                            this.ModuleID,
                            ex.Message);
                    ErrorHandler.Publish(LogLevel.Error, error);
                    this.CurrentTheme = this.PortalSettings.GetCurrentTheme(themeName);
                }
            }
            else
            {
                // original behaviour unchanged
                this.CurrentTheme = this.PortalSettings.GetCurrentTheme(themeName);
            }

            // end change: Jes1111
        }

        /// <summary>
        /// The Zen version of Build(). Parses XML Zen Module Layout.
        /// </summary>
        protected virtual void ZenBuild()
        {
            XmlTextReader xtr = null;
            var nt = new NameTable();
            var nsm = new XmlNamespaceManager(nt);
            nsm.AddNamespace(string.Empty, "http://www.w3.org/1999/xhtml");
            nsm.AddNamespace("if", "urn:MarinaTeq.Appleseed.Zen.Condition");
            nsm.AddNamespace("loop", "urn:Marinateq.Appleseed.Zen.Looping");
            nsm.AddNamespace("content", "urn:www.Appleseedportal.net");
            var context = new XmlParserContext(nt, nsm, string.Empty, XmlSpace.None);
            StringBuilder fragText;

            try
            {
                xtr = new XmlTextReader(this.CurrentTheme.GetThemePart("ModuleLayout"), XmlNodeType.Document, context);

                while (xtr.Read())
                {
                    var frag = new LiteralControl();
                    switch (xtr.Prefix)
                    {
                        case "if":
                            {
                                if (xtr.NodeType == XmlNodeType.Element && !this.ZenEvaluate(xtr.LocalName))
                                {
                                    xtr.Skip();
                                }

                                break;
                            }

                        case "loop":
                            {
                                if (xtr.NodeType == XmlNodeType.Element)
                                {
                                    switch (xtr.LocalName)
                                    {
                                        case "Buttons":
                                            {
                                                // Menu btnMenu = new Menu();
                                                // btnMenu.Orientation = Orientation.Vertical;
                                                // btnMenu.StaticDisplayLevels = 1;
                                                // btnMenu.DisappearAfter = 500;
                                                // btnMenu.DynamicHorizontalOffset = 10;

                                                /*
                                                //btnMenu.StaticMenuStyle.CssClass = "CommandButton";
                                                btnMenu.StaticMenuItemStyle.CssClass = "CommandButton";
                                                
                                                btnMenu.DynamicMenuItemStyle.CssClass = "CommandButton";
                                                //btnMenu.DynamicHoverStyle.CssClass = "CommandButton";

                                                btnMenu.DynamicSelectedStyle.CssClass = "CommandButton";
                                                */

                                                // MenuItem rootNode = new MenuItem("Menu");
                                                // rootNode.ImageUrl = CurrentTheme.GetImage("Navlink", "icon/NavLink.gif").ImageUrl;
                                                // rootNode.ToolTip = "Module Control and Options Menu";
                                                // rootNode.Selected = true;
                                                var loopFrag = xtr.ReadInnerXml();
                                                foreach (var c in this.buttonList)
                                                {
                                                    // ModuleButton mb = (ModuleButton)c;
                                                    /*
                                                    MenuItem MenuItem = new MenuItem(mb.EnglishName);
                                                    if(mb.Image.ImageUrl.Length > 0)
                                                        MenuItem.ImageUrl = mb.Image.ImageUrl;
                                                    else
                                                        MenuItem.ImageUrl = CurrentTheme.GetImage("Navlink", "icon/NavLink.gif").ImageUrl;

                                                    MenuItem.NavigateUrl = mb.HRef;
                                                    
                                                    MenuItem.ToolTip = mb.Title;
                                                    
                                                    MenuItem.Target = mb.Target;

                                                    rootNode.ChildItems.Add(MenuItem);
                                                     * */
                                                    var xtr2 = new XmlTextReader(
                                                        loopFrag, XmlNodeType.Document, context);
                                                    while (xtr2.Read())
                                                    {
                                                        frag = new LiteralControl();
                                                        switch (xtr2.Prefix)
                                                        {
                                                            case "content":
                                                                {
                                                                    switch (xtr2.LocalName)
                                                                    {
                                                                        case "Button":

                                                                            // if ( this.CurrentTheme.Name.ToLower().Equals("zen-zero") && c is ModuleButton )
                                                                            // ((ModuleButton)c).RenderAs = ModuleButton.RenderOptions.TextOnly;
                                                                            // if ( _beforeContent )
                                                                            this.header.Controls.Add(c);

                                                                            // else
                                                                            // this._footer.Controls.Add(c);
                                                                            break;
                                                                        default:
                                                                            break;
                                                                    }

                                                                    break;
                                                                }

                                                            // case "":
                                                            default:
                                                                {
                                                                    switch (xtr2.NodeType)
                                                                    {
                                                                        case XmlNodeType.Element:
                                                                            fragText = new StringBuilder("<");
                                                                            fragText.Append(xtr2.LocalName);
                                                                            while (xtr2.MoveToNextAttribute())
                                                                            {
                                                                                if (xtr2.LocalName != "xmlns")
                                                                                {
                                                                                    fragText.Append(" ");
                                                                                    fragText.Append(xtr.LocalName);
                                                                                    fragText.Append("=\"");
                                                                                    fragText.Append(xtr.Value);
                                                                                    fragText.Append("\"");
                                                                                }
                                                                            }

                                                                            fragText.Append(">");
                                                                            frag.Text = fragText.ToString();
                                                                            if (this.beforeContent)
                                                                            {
                                                                                this.header.Controls.Add(frag);
                                                                            }
                                                                            else
                                                                            {
                                                                                this.footer.Controls.Add(frag);
                                                                            }

                                                                            break;
                                                                        case XmlNodeType.EndElement:
                                                                            frag.Text = string.Format(
                                                                                "</{0}>", xtr2.LocalName);
                                                                            if (this.beforeContent)
                                                                            {
                                                                                this.header.Controls.Add(frag);
                                                                            }
                                                                            else
                                                                            {
                                                                                this.footer.Controls.Add(frag);
                                                                            }

                                                                            break;
                                                                    }

                                                                    break;
                                                                }
                                                        }

                                                        // end switch
                                                    }

                                                    // end while
                                                }

                                                // end foreach

                                                // btnMenu.Items.Add(rootNode);

                                                // this._header.Controls.Add(btnMenu);
                                                break;
                                            }

                                        default:
                                            break;
                                    }

                                    // end inner switch
                                }

                                break;
                            }

                        case "content":
                            {
                                switch (xtr.LocalName)
                                {
                                    case "ModuleContent":
                                        this.beforeContent = false;
                                        break;
                                    case "TitleText":
                                        frag.Text = this.TitleText;
                                        if (this.beforeContent)
                                        {
                                            this.header.Controls.Add(frag);
                                        }
                                        else
                                        {
                                            this.footer.Controls.Add(frag);
                                        }

                                        break;
                                    case "ModifiedBy":
                                        frag.Text = this.GetLastModified();
                                        if (this.beforeContent)
                                        {
                                            this.header.Controls.Add(frag);
                                        }
                                        else
                                        {
                                            this.footer.Controls.Add(frag);
                                        }

                                        break;
                                    default:
                                        break;
                                }

                                break;
                            }

                        // case "":
                        default:
                            {
                                switch (xtr.NodeType)
                                {
                                    case XmlNodeType.Element:
                                        fragText = new StringBuilder("<");
                                        fragText.Append(xtr.LocalName);
                                        while (xtr.MoveToNextAttribute())
                                        {
                                            fragText.Append(" ");
                                            fragText.Append(xtr.LocalName);
                                            fragText.Append("=\"");
                                            fragText.Append(xtr.Value);
                                            fragText.Append("\"");
                                        }

                                        fragText.Append(">");
                                        frag.Text = fragText.ToString();
                                        if (this.beforeContent)
                                        {
                                            this.header.Controls.Add(frag);
                                        }
                                        else
                                        {
                                            this.footer.Controls.Add(frag);
                                        }

                                        break;
                                    case XmlNodeType.EndElement:
                                        frag.Text = string.Format("</{0}>", xtr.LocalName);
                                        if (this.beforeContent)
                                        {
                                            this.header.Controls.Add(frag);
                                        }
                                        else
                                        {
                                            this.footer.Controls.Add(frag);
                                        }

                                        break;
                                }

                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(
                    LogLevel.Fatal, string.Format("Fatal error in ZenBuildControlHierarchy(): {0}", ex.Message));
                throw new Exception(string.Format("Fatal error in ZenBuildControlHierarchy(): {0}", ex.Message));
            }
            finally
            {
                if (xtr != null)
                {
                    xtr.Close();
                }
            }
        }

        /// <summary>
        /// Appends the module id.
        /// </summary>
        /// <param name="url">
        /// The URL to modify.
        /// </param>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <returns>
        /// The modified URL.
        /// </returns>
        private static string AppendModuleId(string url, int moduleId)
        {
            // tiptopweb, sometimes the home page does not have parameters 
            // so we test for both & and ?
            var selectedModIdPos = url.IndexOf("&selectedmodid");
            var selectedModIdPos2 = url.IndexOf("?selectedmodid");
            if (selectedModIdPos >= 0)
            {
                var selectedModIdEndPos = url.IndexOf("&", selectedModIdPos + 1);
                return selectedModIdEndPos >= 0
                           ? string.Format(
                               "{0}&selectedmodid={1}{2}",
                               url.Substring(0, selectedModIdPos),
                               moduleId,
                               url.Substring(selectedModIdEndPos))
                           : string.Format("{0}&selectedmodid={1}", url.Substring(0, selectedModIdPos), moduleId);
            }

            if (selectedModIdPos2 >= 0)
            {
                var selectedModIdEndPos2 = url.IndexOf("?", selectedModIdPos2 + 1);
                return selectedModIdEndPos2 >= 0
                           ? string.Format(
                               "{0}?selectedmodid={1}{2}",
                               url.Substring(0, selectedModIdPos2),
                               moduleId,
                               url.Substring(selectedModIdEndPos2))
                           : string.Format("{0}?selectedmodid={1}", url.Substring(0, selectedModIdPos2), moduleId);
            }

            return url.IndexOf("?") >= 0
                       ? string.Format("{0}&selectedmodid={1}", url, moduleId)
                       : string.Format("{0}?selectedmodid={1}", url, moduleId);
        }

        /// <summary>
        /// Gets the users in roles.
        /// </summary>
        /// <param name="roles">
        /// The roles.
        /// </param>
        /// <returns>
        /// The list of usernames.
        /// </returns>
        private static List<string> GetUsersInRoles(string roles)
        {
            var result = new List<string>();

            var context = HttpContext.Current;

            if (roles != null)
            {
                foreach (var splitRole in
                    roles.Split(new[] { ';' }).Where(splitRole => !String.IsNullOrEmpty(splitRole)))
                {
                    if (splitRole.Length != 0 && splitRole == "All Users")
                    {
                        var collection = Membership.GetAllUsers();
                        foreach (var user in
                            collection.Cast<MembershipUser>().Where(user => !result.Contains(user.Email)))
                        {
                            result.Add(user.Email);
                        }
                    }
                    else if (splitRole == "Authenticated Users" && context.Request.IsAuthenticated)
                    {
                        var memUser = Membership.GetUser();
                        if (memUser != null && !result.Contains(memUser.Email))
                        {
                            result.Add(memUser.Email);
                        }
                    }
                    else if ((splitRole == "Unauthenticated Users") && (!context.Request.IsAuthenticated))
                    {
                        // TODO: no me queda claro que devolver en este caso
                        var collection = Membership.GetAllUsers();
                        foreach (var user in
                            collection.Cast<MembershipUser>().Where(user => !result.Contains(user.Email)))
                        {
                            result.Add(user.Email);
                        }
                    }
                    else
                    {
                        var users = Roles.GetUsersInRole(splitRole);
                        foreach (var user in users.Where(user => !result.Contains(user)))
                        {
                            result.Add(user);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Orders the modules.
        /// </summary>
        /// <param name="list">
        /// The list of modules.
        /// </param>
        private static void OrderModules(List<ModuleItem> list)
        {
            var i = 1;

            // sort the array list
            list.Sort();

            // renumber the order
            foreach (var m in list)
            {
                // number the items 1, 3, 5, etc. to provide an empty order
                // number when moving items up and down in the list.
                m.Order = i;
                i += 2;
            }
        }

        /// <summary>
        /// Returns a module content sizing container tag with properties
        /// </summary>
        /// <param name="isbeginTag">
        /// The is Begin Tag.
        /// </param>
        /// <paramref name="isbeginTag">Specifies whether to output the container's begin(true) or end(false) tag.</paramref>
        /// <returns>
        /// The literal control containing this tag
        /// </returns>
        private LiteralControl BuildModuleContentContainer(bool isbeginTag)
        {
            var modContainer = new LiteralControl();
            var width = (this.Settings["MODULESETTINGS_CONTENT_WIDTH"] != null)
                            ? Int32.Parse(this.Settings["MODULESETTINGS_CONTENT_WIDTH"].ToString())
                            : 0;
            var height = (this.Settings["MODULESETTINGS_CONTENT_HEIGHT"] != null)
                             ? Int32.Parse(this.Settings["MODULESETTINGS_CONTENT_HEIGHT"].ToString())
                             : 0;
            var scrolling = (this.Settings["MODULESETTINGS_CONTENT_SCROLLING"] != null)
                                ? bool.Parse(this.Settings["MODULESETTINGS_CONTENT_SCROLLING"].ToString())
                                : false;
            if (isbeginTag)
            {
                var startContentSizing = string.Format(
                    "<div class='modulePadding moduleScrollBars' id='modcont_{0}' ", this.ClientID);
                startContentSizing += " style='POSITION: static; ";
                if (!this.canPrint && width > 0)
                {
                    startContentSizing += string.Format("width: {0}px; ", width);
                }

                if (!this.canPrint && height > 0)
                {
                    startContentSizing += string.Format("height: {0}px; ", height);
                }

                if (!this.canPrint && scrolling)
                {
                    startContentSizing += "overflow:auto;";
                }

                startContentSizing += "'>";
                modContainer.Text = startContentSizing;
            }
            else
            {
                if (this.Page.Request.Browser.EcmaScriptVersion.Major >= 1 && !this.canPrint &&
                    (width > 0 || height > 0 || (width > 0 && scrolling) || (height > 0 && scrolling)))
                {
                    // Register a client side script that will properly resize the content area of the module
                    // to compensate for different height and width settings, as well as, the browser's tendency
                    // to stretch the middle module width even when a specific width setting is specified.
                    if (!this.Page.ClientScript.IsClientScriptBlockRegistered("autoSizeModules"))
                    {
                        var src = Path.ApplicationRootPath("aspnet_client/Appleseed_scripts/autoResizeModule.js");
                        this.Page.ClientScript.RegisterClientScriptBlock(
                            this.GetType(),
                            "autoSizeModules",
                            string.Format("<script language=javascript src='{0}'></script>", src));
                        this.Page.ClientScript.RegisterStartupScript(
                            this.GetType(),
                            "initAutoSizeModules",
                            "<script defer language=javascript>if (document._portalmodules) document._portalmodules.GetModules(_portalModules); document._portalmodules.ProcessAll();</script>");
                    }

                    this.Page.ClientScript.RegisterArrayDeclaration(
                        "_portalModules", string.Format("'modcont_{0}'", this.ClientID));
                }

                modContainer.Text = "</div>\r";
            }

            return modContainer;
        }

        /// <summary>
        /// Handles the Click event of the DeleteBtn control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Web.UI.ImageClickEventArgs"/> instance containing the event data.
        /// </param>
        private void DeleteButtonClick(object sender, ImageClickEventArgs e)
        {
            this.OnDelete();
        }

        /// <summary>
        /// Handles the Click event of the DeleteModuleButton control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void DeleteModuleButtonClick(object sender, EventArgs e)
        {
            var admin = new ModulesDB();

            // admin.DeleteModule(this.ModuleID);
            // TODO - add userEmail and useRecycler
            admin.DeleteModule(this.ModuleID);

            // Redirect to the same page to pick up changes
            this.Page.Response.Redirect(this.Page.Request.RawUrl);
        }

        /// <summary>
        /// Handles the Click event of the EditBtn control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Web.UI.ImageClickEventArgs"/> instance containing the event data.
        /// </param>
        private void EditButtonClick(object sender, ImageClickEventArgs e)
        {
            this.OnEdit();
        }

        /// <summary>
        /// The GetModules helper method is used to get the modules
        ///   for a single pane within the tab
        /// </summary>
        /// <param name="pane">
        /// The module pane.
        /// </param>
        /// <returns>
        /// A list of modules.
        /// </returns>
        private List<ModuleItem> GetModules(string pane)
        {
            // get the portal setting at the Tab level and not from this class as it is not refreshed
            return
                this.Page.PortalSettings.ActivePage.Modules.Cast<ModuleSettings>().Where(
                    module =>
                    this.PortalSettings.ActivePage.PageID == module.PageID &&
                    module.PaneName.ToLower() == pane.ToLower()).Select(
                        module =>
                        new ModuleItem
                            {
                                Title = module.ModuleTitle,
                                ID = module.ModuleID,
                                ModuleDefID = module.ModuleDefID,
                                Order = module.ModuleOrder,
                                PaneName = module.PaneName // tiptopweb
                            }).ToList();
        }

        /// <summary>
        /// This function constructs the NavigateUrl for the SwapVersions hyperlink
        /// </summary>
        /// <returns>
        /// string URL.
        /// </returns>
        private string GetOtherVersionUrl()
        {
            var url = this.Page.Request.Path;
            var qs = new ArrayList();

            // Added null check by manu
            foreach (var var in
                this.Page.Request.QueryString.Keys.Cast<string>().Where(
                    var => var != null && !(var.StartsWith("wversion") || var.StartsWith("wpublish"))))
            {
                qs.Add(string.Format("{0}={1}", var, this.Page.Server.UrlEncode(this.Page.Request.QueryString[var])));
            }

            var workflow = this.Version == WorkFlowVersion.Production
                               ? WorkFlowVersion.Staging.ToString()
                               : WorkFlowVersion.Production.ToString();
            qs.Add(string.Format("wversion{0}={1}", this.ModuleConfiguration.ModuleID, workflow));
            var querystring = string.Join("&", (string[])qs.ToArray(typeof(string)));
            if (querystring.Length != 0)
            {
                url += string.Format("?{0}", querystring);
            }

            return url;
        }

        /// <summary>
        /// This function constructs the NavigateUrl for the Publish hyperlink
        /// </summary>
        /// <returns>
        /// The publish URL.
        /// </returns>
        private string GetPublishUrl()
        {
            var url = this.Page.Request.Path;

            // modified by Hongwei Shen (hongwei.shen@gmail.com) 8/9/2005
            // qs.Add("wpublish" + this.ModuleConfiguration.ModuleID.ToString() + "=doit"); 
            // end of modification
            var s =
                this.Page.Request.QueryString.Keys.Cast<string>().Where(
                    var => var != null && !(var.StartsWith("wversion") || var.StartsWith("wpublish"))).Select(
                        var =>
                        string.Format("{0}={1}", var, this.Page.Server.UrlEncode(this.Page.Request.QueryString[var]))).
                    ToArray();
            var querystring = string.Join("&", s);
            if (querystring.Length != 0)
            {
                url += string.Format("?{0}", querystring);
            }

            return url;
        }

        /// <summary>
        /// HTMs the render buttons.
        /// </summary>
        /// <param name="placeHolder">
        /// The place holder.
        /// </param>
        private void HtmRenderButtons(Control placeHolder)
        {
            //if (this.ShareModule)
            //{
            //    var publisherkeysetting = this.PortalSettings.CustomSettings["SITESETTINGS_ADDTHIS_USERNAME"];
            //    if (publisherkeysetting != null)
            //    {
            //        if (Convert.ToString(publisherkeysetting).Trim().Length > 0)
            //        {
            //            var culture = Thread.CurrentThread.CurrentUICulture.Name;
            //            var sb = new StringBuilder();
            //            sb.Append(
            //                "<script type=\"text/javascript\">var addthis_config = {data_track_clickback:true, ");
            //            sb.AppendFormat("ui_language:\"{0}\"", culture);
            //            if (this.PortalSettings != null &&
            //                this.PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_GOOGLEANALYTICS") &&
            //                !this.PortalSettings.CustomSettings["SITESETTINGS_GOOGLEANALYTICS"].ToString().Equals(string.Empty))
            //            {

            //                sb.AppendFormat(", data_ga_property:\"{0}\", ", this.PortalSettings.CustomSettings["SITESETTINGS_GOOGLEANALYTICS"].ToString());
            //                sb.AppendFormat("data_ga_social:true");
            //            }
            //            sb.Append("};</script>");
            //            sb.Append("<div class=\"addthis_toolbox addthis_default_style\">");
            //            sb.AppendFormat(
            //                " <a href=\"http://www.addthis.com/bookmark.php?v=250&amp;username={0}\"  class=\"addthis_button_compact\">{1}</a>",
            //                publisherkeysetting,
            //                General.GetString("SHARE", "Share"));
            //            sb.Append(" <span class=\"addthis_separator\">|</span>");
            //            sb.Append(" <a class=\"addthis_button_facebook\"></a>");
            //            sb.Append(" <a class=\"addthis_button_twitter\"></a>");
            //            sb.Append(" <a class=\"addthis_button_myspace\"></a>");
            //            sb.Append("</div>");

            //            placeHolder.Controls.Add(new LiteralControl(sb.ToString()));
            //        }
            //    }
            //}

            if (!this.buildButtons)
            {
                return;
            }

            foreach (var button in this.buttonList)
            {
                placeHolder.Controls.Add(this.CurrentTheme.GetLiteralControl("TitleBeforeButton"));
                placeHolder.Controls.Add(button);
                placeHolder.Controls.Add(this.CurrentTheme.GetLiteralControl("TitleAfterButton"));
            }
        }

        /// <summary>
        /// Updates the moduleControl literal control with proper width settings to render the 'module width stretching' option
        /// </summary>
        /// <param name="moduleControl">
        /// The module Control.
        /// </param>
        /// <param name="isbeginTag">
        /// The is Begin Tag.
        /// </param>
        /// <paramref name="moduleControl">The literal control element to parse and modify.</paramref>
        /// <paramref name="isbeginTag">Specifies whether the moduleElement parameter is for the element's begin(true) or end(false) tag.</paramref>
        private void ProcessModuleStrecthing(Control moduleControl, bool isbeginTag)
        {
            if (moduleControl == null || !(moduleControl is LiteralControl))
            {
                return;
            }

            var moduleElement = (LiteralControl)moduleControl;
            var stretched = this.Settings["MODULESETTINGS_WIDTH_STRETCHING"] != null &&
                            bool.Parse(this.Settings["MODULESETTINGS_WIDTH_STRETCHING"].ToString());
            var tmp = (moduleElement.Text != null) ? moduleElement.Text.Trim() : string.Empty;

            // Need to remove the current width setting of the main table in the module Start(Title/NoTitle)Content section of the theme,
            // if this is to be a stretched module then insert a width attribute into it,
            // if not, then surround this table with another table that has an empty cell after the cell that contains the module's HTML,
            // in order to use up any space in the window that the module has not been defined for.
            // if, no width specified for module then the module will be at least 50% width of area remaining, or expand to hold its contents.
            if (isbeginTag)
            {
                var r = new Regex("<table[^>]*>");
                var mc = r.Matches(tmp.ToLower());

                // Only concerned with first match
                if (mc.Count > 0)
                {
                    var match = mc[0].Value;
                    var index = mc[0].Index;

                    // jminond - variable not in use
                    // int TLength = mc[0].Value.Length;
                    // find a width attribute in this match(if exists remove it)
                    var r1 = new Regex("width=((['\"][^'\"]*['\"])|([0-9]+))");
                    mc = r1.Matches(match);
                    if (mc.Count > 0)
                    {
                        var indexW = mc[0].Index;
                        var lengthW = mc[0].Value.Length;
                        tmp = tmp.Substring(0, indexW + index) + tmp.Substring(indexW + index + lengthW);
                        match = match.Substring(0, indexW) + match.Substring(indexW + lengthW);
                    }

                    // find a style attribute in this match(if exists)
                    var r2 = new Regex("style=['\"][^'\"]*['\"]");
                    mc = r2.Matches(match);
                    if (mc.Count > 0)
                    {
                        var indexS = mc[0].Index;

                        // jminond- variable not in use
                        // int SLength = mc[0].Value.Length;

                        // Next find a width style property(if exists) and modify it
                        var r3 = new Regex("width:[^;'\"]+[;'\"]");
                        mc = r3.Matches(mc[0].Value);
                        if (mc.Count > 0)
                        {
                            var indexSw = mc[0].Index;
                            var lengthSw = mc[0].Value.Length - 1;
                            if (stretched)
                            {
                                tmp = string.Format(
                                    "{0}width:100%{1}",
                                    tmp.Substring(0, indexS + indexSw + index),
                                    tmp.Substring(indexS + indexSw + index + lengthSw));
                            }
                            else
                            {
                                tmp = tmp.Substring(0, indexS + indexSw + index) +
                                      tmp.Substring(indexS + indexSw + index + lengthSw);
                            }
                        }
                        else if (stretched)
                        {
                            // Else, Add width style property to the existing style attribute
                            tmp = string.Format(
                                "{0}width:100%;{1}",
                                tmp.Substring(0, indexS + index + 7),
                                tmp.Substring(indexS + index + 7));
                        }
                    }
                    else if (stretched)
                    {
                        // Else, Add width style property to a new style attribute
                        tmp = string.Format(
                            "{0}style='width:100%' {1}", tmp.Substring(0, index + 7), tmp.Substring(index + 7));
                    }
                }

                if (!stretched)
                {
                    tmp = "<table cellpadding=0 cellspacing=0 border=0><tr>\n<td>" + tmp;
                }
            }
            else if (!stretched)
            {
                tmp += "</td><td></td>\n</tr></table>";
            }

            moduleElement.Text = tmp;
        }

        /// <summary>
        /// Handles the ServerClick event of the publishButton control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void PublishButtonServerClick(object sender, EventArgs e)
        {
            this.Publish();

            // redirect to the same page to pick up changes
            this.Page.Response.Redirect(this.GetPublishUrl());
        }

        /// <summary>
        /// The RightLeftClick server event handler on this page is
        ///   used to move a portal module between layout panes on
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void RightLeftClick(object sender, EventArgs e)
        {
            var sourcePane = ((ModuleButton)sender).Attributes["sourcepane"];
            var targetPane = ((ModuleButton)sender).Attributes["targetpane"];

            // add it to the database
            // tiptopweb : OriginalModuleID to have it work with shortcut module
            var admin = new ModulesDB();
            admin.UpdateModuleOrder(this.OriginalModuleID, 99, targetPane);

            // reload the PortalSettings from the database
            HttpContext.Current.Items["PortalSettings"] = PortalSettings.GetPortalSettings(
                PageID, PortalSettings.PortalAlias);
            this.Page.PortalSettings = (PortalSettings)this.Context.Items["PortalSettings"];

            // get source array list
            var sourceList = this.GetModules(sourcePane);

            // reorder the modules in the source pane
            OrderModules(sourceList);

            // resave the order
            foreach (var item in sourceList)
            {
                admin.UpdateModuleOrder(item.ID, item.Order, sourcePane);
            }

            // reorder the modules in the target pane
            var targetList = this.GetModules(targetPane);
            OrderModules(targetList);

            // resave the order
            foreach (var item in targetList)
            {
                admin.UpdateModuleOrder(item.ID, item.Order, targetPane);
            }

            // Redirect to the same page to pick up changes
            this.Page.Response.Redirect(AppendModuleId(this.Page.Request.RawUrl, this.ModuleID));
        }

        /// <summary>
        /// The UpDownClick server event handler on this page is
        ///   used to move a portal module up or down on a tab's layout pane
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void UpDownClick(object sender, EventArgs e)
        {
            // string cmd = ((ModuleButton)sender).CommandName;
            // string pane = ((ModuleButton)sender).CommandArgument;
            var cmd = ((ModuleButton)sender).Attributes["direction"];
            var pane = ((ModuleButton)sender).Attributes["pane"];

            var modules = this.GetModules(pane);

            // Determine the delta to apply in the order number for the module
            // within the list.  +3 moves down one item; -3 moves up one item
            var delta = cmd == "down" ? 3 : -3;

            foreach (var item in modules.Where(item => item.ID == this.ModuleID))
            {
                item.Order += delta;
            }

            // reorder the modules in the content pane
            OrderModules(modules);

            // resave the order
            var admin = new ModulesDB();
            foreach (var item in modules)
            {
                admin.UpdateModuleOrder(item.ID, item.Order, pane);
            }

            // Redirect to the same page to pick up changes
            this.Page.Response.Redirect(AppendModuleId(this.Page.Request.RawUrl, this.ModuleID));
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
        private void UpdateButtonClick(object sender, EventArgs e)
        {
            this.OnUpdate(e);
        }

        // Nicholas Smeaton (24/07/2004) - Arrow button functions END

        /*
        // Added  - BJA [wjanderson@reedtek.com] [START]
        /// <summary>
        /// Set the close button attributes to prompt user before removing.
        /// </summary>
        /// <param name="delBtn">
        /// The del Btn.
        /// </param>
        private void SetDeleteAttributes(ref LinkButton delBtn)
        {
            // make sure javascript is valid and we have not already
            // added the function
            if (this.Page.Request.Browser.EcmaScriptVersion.Major >= 1 &&
                !this.Page.ClientScript.IsClientScriptBlockRegistered("confirmDelete"))
            {
                string[] s = { "CONFIRM_DELETE" };
                this.Page.ClientScript.RegisterClientScriptBlock(
                    this.GetType(), "confirmDelete", PortalSettings.GetStringResource("CONFIRM_DELETE_SCRIPT", s));
            }

            if (delBtn.Attributes["onclick"] != null)
            {
                delBtn.Attributes["onclick"] = "return confirmDelete();" + delBtn.Attributes["onclick"];
            }
            else
            {
                delBtn.Attributes.Add("onclick", "return confirmDelete();");
            }
        }
*/

        // end of setDeleteAttributes
        // Added - BJA [wjanderson@reedtek.com] [END]

        /// <summary>
        /// Supports ZenBuild(), evaluates 'if' commands
        /// </summary>
        /// <param name="condition">
        /// The condition.
        /// </param>
        /// <returns>
        /// The zen evaluate.
        /// </returns>
        private bool ZenEvaluate(string condition)
        {
            var returnVal = false;

            switch (condition)
            {
                case "Title":
                    if (this.buildTitle)
                    {
                        returnVal = true;
                    }

                    break;
                case "Buttons":

                    // if ( _buildButtons && ButtonList.Count > 0 )
                    if (this.buildButtons)
                    {
                        returnVal = true;
                    }

                    break;
                case "Body":
                case "Footer":
                    if (this.buildBody)
                    {
                        returnVal = true;
                    }
                    else
                    {
                        foreach (var control in this.Controls.Cast<Control>())
                        {
                            control.Visible = false;
                        }
                    }

                    break;
                case "ShowModifiedBy":
                    if (
                        ((ISettingItem<bool>)this.PortalSettings.CustomSettings["SITESETTINGS_SHOW_MODIFIED_BY"]).Value &&
                        ((ISettingItem<bool>)this.Settings["MODULESETTINGS_SHOW_MODIFIED_BY"]).Value)
                    {
                        returnVal = true;
                    }

                    break;
                default:
                    break;
            }

            return returnVal;
        }

        #endregion
    }
}