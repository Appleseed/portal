// --------------------------------------------------------------------------------------------------------------------
// <copyright company="--" file="SolpartNavigation.cs">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   TODO: CAN WE REPLACE THIS WITH ASP.NET Menu naviagion?
//   Therby not breaking any existing themes?
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI.WebControls
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Xml;

    using Appleseed.Framework.Security;
    using Appleseed.Framework.Settings.Cache;
    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.Site.Data;

    using Solpart.WebControls;

    using Path = Appleseed.Framework.Settings.Path;

    /// <summary>
    /// TODO: CAN WE REPLACE THIS WITH ASP.NET Menu naviagion? 
    ///     Therby not breaking any existing themes?
    /// </summary>
    [History("gman3001", "2004/10/06", 
        "Add support for the active root tab to use a custom css style for normal and highlighting purposes")]
    [History("jviladiu@portalServices.net", "2004/08/26", 
        "Add AutoShopDetect support and set url's for categories of products")]
    [History("jviladiu@portalServices.net", "2004/08/26", "Add ShowIconMenu property")]
    public class SolpartNavigation : SolpartMenu, INavigation
    {
        #region Constants and Fields

        /// <summary>
        /// The _bind.
        /// </summary>
        private BindOption _bind = BindOption.BindOptionTop;

        /// <summary>
        /// The _defined parent tab.
        /// </summary>
        private int _definedParentTab = -1;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref = "SolpartNavigation" /> class.
        /// </summary>
        public SolpartNavigation()
        {
            this.EnableViewState = false;
            this.Load += this.LoadControl;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Indicates if control should bind when loads
        /// </summary>
        /// <value><c>true</c> if [auto bind]; otherwise, <c>false</c>.</value>
        [Category("Data")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool AutoBind { get; set; }

        /// <summary>
        ///     Indicates if control should detect products module when loads
        /// </summary>
        /// <value><c>true</c> if [auto shop detect]; otherwise, <c>false</c>.</value>
        [Category("Data")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool AutoShopDetect { get; set; }

        /// <summary>
        ///     Describes how this control should bind to db data
        /// </summary>
        /// <value>The bind.</value>
        [Category("Data")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public BindOption Bind
        {
            get
            {
                return this._bind;
            }

            set
            {
                if (this._bind != value)
                {
                    this._bind = value;
                }
            }
        }

        /// <summary>
        ///     defines the parentPageID when using BindOptionDefinedParent
        /// </summary>
        /// <value>The parent page ID.</value>
        [Category("Data")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public int ParentPageID
        {
            get
            {
                return this._definedParentTab;
            }

            set
            {
                if (this._definedParentTab != value)
                {
                    this._definedParentTab = value;
                }
            }
        }

        /// <summary>
        ///     Indicates if control should render images in menu
        /// </summary>
        /// <value><c>true</c> if [show icon menu]; otherwise, <c>false</c>.</value>
        [Category("Data")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowIconMenu { get; set; }

        /// <summary>
        ///     Indicates if control should show the tabname in the url
        /// </summary>
        /// <value><c>true</c> if [use tab name in URL]; otherwise, <c>false</c>.</value>
        [Category("Data")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool UseTabNameInUrl { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Do databind.
        ///     Thanks to abain for cleaning up the code and fixing the bugs
        /// </summary>
        public override void DataBind()
        {
            base.DataBind();

            // bool currentTabOnly = (Bind == BindOption.BindOptionCurrentChilds); 

            // Obtain PortalSettings from Current Context 
            var PortalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];

            // Build list of tabs to be shown to user 
            var authorizedTabs = this.setAutorizedTabsWithImage();

            for (var i = 0; i < authorizedTabs.Count; i++)
            {
                var myTab = (PageStripDetails)authorizedTabs[i];
                if (this.products(myTab.PageID))
                {
                    this.AddGraphMenuItem(
                        null, 
                        myTab.PageID.ToString(), 
                        myTab.PageName, 
                        myTab.PageImage, 
                        this.giveMeUrl(myTab.PageName, myTab.PageID), 
                        false);
                    if (PortalSettings.ActivePage.PageID == myTab.PageID)
                    {
                        this.ShopMenu(myTab, PortalSettings.ActivePage.PageID);
                    }
                }
                else
                {
                    // gman3001: Statement Added/Modified 2004/10/06
                    // for now only set default css styles for the active root menu item, if it is not a products menu
                    if (this.IsActiveTabIn(PortalSettings.ActivePage.PageID, myTab))
                    {
                        this.AddGraphMenuItem(
                            null, 
                            myTab.PageID.ToString(), 
                            myTab.PageName, 
                            myTab.PageImage, 
                            this.giveMeUrl(myTab.PageName, myTab.PageID), 
                            false, 
                            this.Attributes["MenuCSS-MenuDefaultItem"], 
                            this.Attributes["MenuCSS-MenuDefaultItemHighLight"]);
                    }
                    else
                    {
                        this.AddGraphMenuItem(
                            null, 
                            myTab.PageID.ToString(), 
                            myTab.PageName, 
                            myTab.PageImage, 
                            this.giveMeUrl(myTab.PageName, myTab.PageID), 
                            false);
                    }

                    this.RecourseMenu(myTab, PortalSettings.ActivePage.PageID);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Populates ArrayList of tabs based on binding option selected.
        /// </summary>
        /// <returns>
        /// The get inner data source.
        /// </returns>
        protected object GetInnerDataSource()
        {
            var authorizedTabs = new ArrayList();

            if (HttpContext.Current != null)
            {
                // Obtain PortalSettings from Current Context
                var PortalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];

                switch (this.Bind)
                {
                    case BindOption.BindOptionTop:
                        {
                            authorizedTabs = this.GetTabs(
                                0, PortalSettings.ActivePage.PageID, PortalSettings.DesktopPages);
                            break;
                        }

                    case BindOption.BindOptionCurrentChilds:
                        {
                            var currentTabRoot =
                                PortalSettings.GetRootPage(PortalSettings.ActivePage, PortalSettings.DesktopPages).
                                    PageID;
                            authorizedTabs = this.GetTabs(
                                currentTabRoot, PortalSettings.ActivePage.PageID, PortalSettings.DesktopPages);
                            break;
                        }

                    case BindOption.BindOptionSubtabSibling:
                        {
                            int currentTabRoot;
                            if (PortalSettings.ActivePage.ParentPageID == 0)
                            {
                                currentTabRoot = PortalSettings.ActivePage.PageID;
                            }
                            else
                            {
                                currentTabRoot = PortalSettings.ActivePage.ParentPageID;
                            }

                            authorizedTabs = this.GetTabs(
                                currentTabRoot, PortalSettings.ActivePage.PageID, PortalSettings.DesktopPages);
                            break;
                        }

                    case BindOption.BindOptionChildren:
                        {
                            authorizedTabs = this.GetTabs(
                                PortalSettings.ActivePage.PageID, 
                                PortalSettings.ActivePage.PageID, 
                                PortalSettings.DesktopPages);
                            break;
                        }

                    case BindOption.BindOptionSiblings:
                        {
                            authorizedTabs = this.GetTabs(
                                PortalSettings.ActivePage.ParentPageID, 
                                PortalSettings.ActivePage.PageID, 
                                PortalSettings.DesktopPages);
                            break;
                        }

                        // MH: added 19/09/2003 by mario@hartmann.net
                    case BindOption.BindOptionTabSibling:
                        {
                            authorizedTabs = this.GetTabs(
                                PortalSettings.ActivePage.PageID, 
                                PortalSettings.ActivePage.PageID, 
                                PortalSettings.DesktopPages);

                            if (authorizedTabs.Count == 0)
                            {
                                authorizedTabs = this.GetTabs(
                                    PortalSettings.ActivePage.ParentPageID, 
                                    PortalSettings.ActivePage.PageID, 
                                    PortalSettings.DesktopPages);
                            }

                            break;
                        }

                        // MH: added 29/04/2003 by mario@hartmann.net
                    case BindOption.BindOptionDefinedParent:
                        if (this.ParentPageID != -1)
                        {
                            authorizedTabs = this.GetTabs(
                                this.ParentPageID, PortalSettings.ActivePage.PageID, PortalSettings.DesktopPages);
                        }

                        break;

                        // MH: end
                    default:
                        {
                            break;
                        }
                }
            }

            return authorizedTabs;
        }

        /// <summary>
        /// Recourses the menu.
        /// </summary>
        /// <param name="PageStripDetails">
        /// The page strip details.
        /// </param>
        /// <param name="activePageID">
        /// The active page ID.
        /// </param>
        protected virtual void RecourseMenu(PageStripDetails PageStripDetails, int activePageID)
        {
            var childTabs = PageStripDetails.Pages;
            if (childTabs.Count > 0)
            {
                foreach (var mySubTab in
                    childTabs.Where(mySubTab => PortalSecurity.IsInRoles(mySubTab.AuthorizedRoles)))
                {
                    if (mySubTab.PageImage == null)
                    {
                        mySubTab.PageImage =
                            new PageSettings().GetPageCustomSettings(mySubTab.PageID)["CustomMenuImage"].ToString();
                    }

                    if (this.products(mySubTab.PageID))
                    {
                        this.AddGraphMenuItem(
                            PageStripDetails.PageID.ToString(), 
                            mySubTab.PageID.ToString(), 
                            mySubTab.PageName, 
                            mySubTab.PageImage, 
                            this.giveMeUrl(mySubTab.PageName, mySubTab.PageID), 
                            false);
                        if (activePageID == mySubTab.PageID)
                        {
                            this.ShopMenu(mySubTab, activePageID);
                        }
                    }
                    else
                    {
                        this.AddGraphMenuItem(
                            PageStripDetails.PageID.ToString(), 
                            mySubTab.PageID.ToString(), 
                            mySubTab.PageName, 
                            mySubTab.PageImage, 
                            this.giveMeUrl(mySubTab.PageName, mySubTab.PageID), 
                            false);
                        this.RecourseMenu(mySubTab, activePageID);
                    }
                }

                childTabs = null;
            }
        }

        /// <summary>
        /// Shops the menu.
        /// </summary>
        /// <param name="pageStripDetails">
        /// The page strip details.
        /// </param>
        /// <param name="activePageId">
        /// The active page ID.
        /// </param>
        protected virtual void ShopMenu(PageStripDetails pageStripDetails, int activePageId)
        {
            Collection<PageStripDetails> childTabs = pageStripDetails.Pages;
            if (childTabs.Count > 0)
            {
                foreach (var mySubTab in childTabs.Where(mySubTab => PortalSecurity.IsInRoles(mySubTab.AuthorizedRoles)))
                {
                    this.AddGraphMenuItem(
                        pageStripDetails.PageID.ToString(), 
                        mySubTab.PageID.ToString(), 
                        mySubTab.PageName, 
                        mySubTab.PageImage, 
                        HttpUrlBuilder.BuildUrl(
                            "~/" + HttpUrlBuilder.DefaultPage, activePageId, "ItemID=" + mySubTab.PageID), 
                        false);
                    this.ShopMenu(mySubTab, activePageId);
                }

                childTabs = null;
            }
        }

        /// <summary>
        /// Adds the attributeto item.
        /// </summary>
        /// <param name="curNode">
        /// The cur node.
        /// </param>
        /// <param name="AttributeName">
        /// Name of the attribute.
        /// </param>
        /// <param name="Value">
        /// The value.
        /// </param>
        private void AddAttributetoItem(XmlNode curNode, string AttributeName, string Value)
        {
            if (curNode != null && AttributeName != null && AttributeName.Length > 0 && Value != null &&
                Value.Length > 0)
            {
                var myItemAttribute = curNode.Attributes[AttributeName];

                // if current attribute exists assign new value to it
                if (myItemAttribute != null)
                {
                    myItemAttribute.Value = Value;
                }
                    
                    // otherwise add a new attribute to the node and assign the value to it
                else
                {
                    myItemAttribute = curNode.OwnerDocument.CreateAttribute(AttributeName);
                    myItemAttribute.Value = Value;
                    curNode.Attributes.SetNamedItem(myItemAttribute);
                }
            }
        }

        // gman3001: 2004/10/06 Method modified to call more detailed overloaded version below.
        /// <summary>
        /// Adds the graph menu item.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="tab">
        /// The tab.
        /// </param>
        /// <param name="tabname">
        /// The tabname.
        /// </param>
        /// <param name="iconfile">
        /// The iconfile.
        /// </param>
        /// <param name="url">
        /// The URL.
        /// </param>
        /// <param name="translation">
        /// if set to <c>true</c> [translation].
        /// </param>
        private void AddGraphMenuItem(
            string parent, string tab, string tabname, string iconfile, string url, bool translation)
        {
            this.AddGraphMenuItem(parent, tab, tabname, iconfile, url, translation, string.Empty, string.Empty);
        }

        // gman3001: 2004/10/06 Method overload added to support custom css styles for individuals menu items
        /// <summary>
        /// Adds the graph menu item.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="tab">
        /// The tab.
        /// </param>
        /// <param name="tabname">
        /// The tabname.
        /// </param>
        /// <param name="iconfile">
        /// The iconfile.
        /// </param>
        /// <param name="url">
        /// The URL.
        /// </param>
        /// <param name="translation">
        /// if set to <c>true</c> [translation].
        /// </param>
        /// <param name="customcss">
        /// The customcss.
        /// </param>
        /// <param name="customhighlightcss">
        /// The customhighlightcss.
        /// </param>
        private void AddGraphMenuItem(
            string parent, 
            string tab, 
            string tabname, 
            string iconfile, 
            string url, 
            bool translation, 
            string customcss, 
            string customhighlightcss)
        {
            var PortalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            var pathGraph = HttpContext.Current.Server.MapPath(PortalSettings.PortalLayoutPath + "/menuimages/");
            var tabTranslation = tabname;
            if (translation)
            {
                tabTranslation = General.GetString(tabname);
            }

            XmlNode padre = null;

            // gman3001: Line added 2004/10/06
            XmlNode newNode = null;

            if (parent != null)
            {
                padre = this.FindMenuItem(parent).Node;
            }

            if (this.ShowIconMenu)
            {
                if (File.Exists(pathGraph + iconfile))
                {
                    // gman3001: Line modified 2004/10/06, added assignment to newNode
                    newNode = this.AddMenuItem(
                        padre, tab, tabTranslation, url, iconfile, false, string.Empty, string.Empty);
                }
                else
                {
                    if (padre == null)
                    {
                        // gman3001: Line modified 2004/10/06, added assignment to newNode
                        newNode = this.AddMenuItem(
                            padre, tab, tabTranslation, url, "menu.gif", false, string.Empty, string.Empty);
                    }
                    else
                    {
                        // gman3001: Line modified 2004/10/06, added assignment to newNode
                        newNode = this.AddMenuItem(
                            padre, tab, tabTranslation, url, string.Empty, false, string.Empty, string.Empty);
                    }
                }
            }
            else
            {
                // gman3001: Line modified 2004/10/06, added assignment to newNode
                newNode = this.AddMenuItem(
                    padre, tab, tabTranslation, url, string.Empty, false, string.Empty, string.Empty);
            }

            // gman3001: Added 2004/10/06
            // Added support to add a custom css class and a custom css highlight class to individual menu items		
            if (newNode != null)
            {
                this.AddAttributetoItem(newNode, "css", customcss);
                this.AddAttributetoItem(newNode, "highlightcss", customhighlightcss);
            }
        }

        // gman3001: Method Added 2004/10/06
        // Method to support adding/modifying Custom Attributes to a Menu Item, to be rendered by the client

        /// <summary>
        /// Seems to be unused - Jes1111
        /// </summary>
        /// <param name="parentPageID">
        /// The parent page ID.
        /// </param>
        /// <param name="activePageID">
        /// The active page ID.
        /// </param>
        /// <param name="allTabs">
        /// All tabs.
        /// </param>
        /// <returns>
        /// The get selected tab.
        /// </returns>
        private int GetSelectedTab(int parentPageID, int activePageID, IList allTabs)
        {
            for (var i = 0; i < allTabs.Count; i++)
            {
                var tmpTab = (PageStripDetails)allTabs[i];
                if (tmpTab.PageID == activePageID)
                {
                    var selectedPageID = activePageID;
                    if (tmpTab.ParentPageID != parentPageID)
                    {
                        selectedPageID = this.GetSelectedTab(parentPageID, tmpTab.ParentPageID, allTabs);
                        return selectedPageID;
                    }
                    else
                    {
                        return selectedPageID;
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// Gets the tabs.
        /// </summary>
        /// <param name="parentID">
        /// The parent ID.
        /// </param>
        /// <param name="tabID">
        /// The tab ID.
        /// </param>
        /// <param name="Tabs">
        /// The tabs.
        /// </param>
        /// <returns>
        /// </returns>
        private ArrayList GetTabs(int parentID, int tabID, IList Tabs)
        {
            var authorizedTabs = new ArrayList();

            // int index = -1;

            // MH:get the selected tab for this 
            // int selectedPageID = GetSelectedTab (parentID, tabID, Tabs);

            // Populate Tab List with authorized tabs
            for (var i = 0; i < Tabs.Count; i++)
            {
                var tab = (PageStripDetails)Tabs[i];

                if (tab.ParentPageID == parentID)
                {
                    // Get selected row only
                    if (PortalSecurity.IsInRoles(tab.AuthorizedRoles))
                    {
                        // index = authorizedTabs.Add(tab);
                        authorizedTabs.Add(tab);
                    }
                }
            }

            return authorizedTabs;
        }

        /// <summary>
        /// Determines whether [is active tab in] [the specified active page ID].
        /// </summary>
        /// <param name="activePageId">
        /// The active page ID.
        /// </param>
        /// <param name="pageStripDetails">
        /// The page strip details.
        /// </param>
        /// <returns>
        /// <c>true</c> if [is active tab in] [the specified active page ID]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsActiveTabIn(int activePageId, PageStripDetails pageStripDetails)
        {
            if (pageStripDetails.PageID == activePageId)
            {
                return true;
            }

            var childTabs = pageStripDetails.Pages;
            if (childTabs.Count > 0)
            {
                if (
                    childTabs.Where(mySubTab => PortalSecurity.IsInRoles(mySubTab.AuthorizedRoles)).Any(
                        mySubTab => this.IsActiveTabIn(activePageId, mySubTab)))
                {
                    return true;
                }

                childTabs = null;
            }

            return false;
        }

        /// <summary>
        /// Loads the control.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void LoadControl(object sender, EventArgs e)
        {
            base.SystemScriptPath = string.Concat(
                Path.ApplicationRoot, "/aspnet_client/SolpartWebControls_SolpartMenu/1_4_0_0/");
            var PortalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            var solpart = string.Concat(Path.ApplicationRoot, "/aspnet_client/SolpartWebControls_SolpartMenu/1_4_0_0/");

            if (this.ShowIconMenu)
            {
                base.SystemImagesPath = Path.WebPathCombine(PortalSettings.PortalLayoutPath, "menuimages/");
                var menuDirectory = HttpContext.Current.Server.MapPath(base.SystemImagesPath);

                // Create directory and copy standard images for solpart
                if (!Directory.Exists(menuDirectory))
                {
                    Directory.CreateDirectory(menuDirectory);
                    var solpartPhysicalDir = HttpContext.Current.Server.MapPath(solpart);
                    if (File.Exists(solpartPhysicalDir + "/spacer.gif"))
                    {
                        File.Copy(solpartPhysicalDir + "/spacer.gif", menuDirectory + "/spacer.gif");
                        File.Copy(solpartPhysicalDir + "/spacer.gif", menuDirectory + "/menu.gif");
                    }

                    if (File.Exists(solpartPhysicalDir + "/icon_arrow.gif"))
                    {
                        File.Copy(solpartPhysicalDir + "/icon_arrow.gif", menuDirectory + "/icon_arrow.gif");
                    }
                }
            }
            else
            {
                base.SystemImagesPath = solpart;
            }

            base.MenuCSSPlaceHolderControl = "spMenuStyle";
            base.SeparateCSS = true;

            if (this.AutoBind)
            {
                this.DataBind();
            }
        }

        /// <summary>
        /// Gives the me URL.
        /// </summary>
        /// <param name="tab">
        /// The tab.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The give me url.
        /// </returns>
        private string giveMeUrl(string tab, int id)
        {
            if (!this.UseTabNameInUrl)
            {
                return HttpUrlBuilder.BuildUrl(id);
            }

            var auxtab = string.Empty;
            foreach (var c in tab)
            {
                if (char.IsLetterOrDigit(c))
                {
                    auxtab += c;
                }
                else
                {
                    auxtab += "_";
                }
            }

            return HttpUrlBuilder.BuildUrl("~/" + auxtab + ".aspx", id);
        }

        /// <summary>
        /// Productses the specified tab.
        /// </summary>
        /// <param name="tab">
        /// The tab.
        /// </param>
        /// <returns>
        /// The products.
        /// </returns>
        private bool products(int tab)
        {
            if (!this.AutoShopDetect)
            {
                return false;
            }

            if (!CurrentCache.Exists(Key.TabNavigationSettings(tab, "Shop")))
            {
                var PortalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
                var exists = new ModulesDB().ExistModuleProductsInPage(tab, PortalSettings.PortalID);
                CurrentCache.Insert(Key.TabNavigationSettings(tab, "Shop"), exists);
            }

            return (bool)CurrentCache.Get(Key.TabNavigationSettings(tab, "Shop"));
        }

        /// <summary>
        /// Sets the autorized tabs with image.
        /// </summary>
        /// <returns>
        /// </returns>
        private ArrayList setAutorizedTabsWithImage()
        {
            var authorizedTabs = (ArrayList)this.GetInnerDataSource();
            if (!this.ShowIconMenu)
            {
                return authorizedTabs;
            }

            for (var i = 0; i < authorizedTabs.Count; i++)
            {
                var myTab = (PageStripDetails)authorizedTabs[i];
                if (myTab.PageImage == null)
                {
                    myTab.PageImage =
                        new PageSettings().GetPageCustomSettings(myTab.PageID)["CustomMenuImage"].ToString();
                }
            }

            return authorizedTabs;
        }

        #endregion
    }
}