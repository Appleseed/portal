namespace Appleseed.Framework.Web.UI.WebControls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Web;
    using System.Web.UI;

    using Appleseed.Framework.Security;
    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Site.Configuration;

    using DUEMETRI.UI.WebControls.HWMenu;

    /// <summary>
    /// Menu navigation inherits from Menu Webcontrol
    ///     and adds the 'glue' to link to tabs tree.
    ///     Bugfix #656794 'Menu rendering adds all tabs' by abain
    /// </summary>
    [History("jperry", "2003/05/01", "Code changed to more closely resemble DesktopNavigation")]
    [History("jperry", "2003/05/02", "Support for new binding options.")]
    [History("MH", "2003/05/23", "Added bind option 'BindOptionDefinedParent' and 'ParentPageID'.")]
    [History("jviladiu@portalServices.net", "2004/08/26",
        "Add AutoShopDetect support and set url's for categories of products")]
    public class MenuNavigation : Menu, INavigation
    {
        #region Constants and Fields

        /// <summary>
        /// The bind option.
        /// </summary>
        private BindOption bind = BindOption.BindOptionTop;

        /// <summary>
        /// The defined parent tab.
        /// MH: added 29/04/2003 by mario@hartmann.net
        /// </summary>
        private int definedParentTab = -1;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref = "MenuNavigation" /> class.
        /// </summary>
        public MenuNavigation()
        {
            this.EnableViewState = false;
            this.Load += this.LoadControl;
        }

        #endregion

        // MH: end
        #region Properties

        /// <summary>
        ///     Indicates if control should bind when loads
        /// </summary>
        /// <value></value>
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
        /// <value></value>
        [Category("Data")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public BindOption Bind
        {
            get
            {
                return this.bind;
            }

            set
            {
                    this.bind = value;
            }
        }

        /// <summary>
        ///     defines the parentPageID when using BindOptionDefinedParent
        /// MH: added 23/05/2003 by mario@hartmann.net
        /// </summary>
        /// <value></value>
        [Category("Data")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public int ParentPageID
        {
            get
            {
                return this.definedParentTab;
            }

            set
            {
                    this.definedParentTab = value;
            }
        }

        /// <summary>
        ///     Indicates if control should show the tabname in the url
        /// </summary>
        /// <value><c>true</c> if [use tab name in URL]; otherwise, <c>false</c>.</value>
        [Category("Data")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool UseTabNameInUrl { get; set; }

        #endregion

        // MH: end
        #region Public Methods

        /// <summary>
        /// Do databind.
        ///     Thanks to abain for cleaning up the code and fixing the bugs
        /// </summary>
        public override void DataBind()
        {
            base.DataBind();

            // Obtain PortalSettings from Current Context 
            ////var PortalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];

            // Build list of tabs to be shown to user 
            var authorizedTabs = (ArrayList)this.GetInnerDataSource();

            foreach (var mytab in authorizedTabs.Cast<PageStripDetails>())
            {
                this.AddMenuTreeNode(0, mytab);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a Menu Tree Node if user in in the list of Authorized roles.
        ///     Thanks to abain for fixing authorization bug.
        /// </summary>
        /// <param name="tabIndex">
        /// Index of the tab
        /// </param>
        /// <param name="mytab">
        /// Tab to add to the MenuTreeNodes collection
        /// </param>
        protected virtual void AddMenuTreeNode(int tabIndex, PageStripDetails mytab)
        {
            // MH:
            if (PortalSecurity.IsInRoles(mytab.AuthorizedRoles))
            {
                var mn = new MenuTreeNode(mytab.PageName)
                    { Link = this.GiveMeUrl(mytab.PageName, mytab.PageID), Height = this.Height, Width = this.Width };

                mn = this.RecourseMenu(tabIndex, mytab.Pages, mn);
                this.Childs.Add(mn);
            }
        }

        /// <summary>
        /// Gets the client script path.
        /// </summary>
        /// <returns>
        /// The get client script path.
        /// </returns>
        protected override string GetClientScriptPath()
        {
            return string.Concat(Path.ApplicationRoot, "/aspnet_client/DUEMETRI_UI_WebControls_HWMenu/1_0_0_0/");
        }

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
                            authorizedTabs = GetTabs(
                                0, PortalSettings.DesktopPages);
                            break;
                        }

                    case BindOption.BindOptionCurrentChilds:
                        {
                            var currentTabRoot =
                                PortalSettings.GetRootPage(PortalSettings.ActivePage, PortalSettings.DesktopPages).
                                    PageID;
                            authorizedTabs = GetTabs(
                                currentTabRoot, PortalSettings.DesktopPages);
                            break;
                        }

                    case BindOption.BindOptionSubtabSibling:
                        {
                            int currentTabRoot = PortalSettings.ActivePage.ParentPageID == 0 ? PortalSettings.ActivePage.PageID : PortalSettings.ActivePage.ParentPageID;

                            authorizedTabs = GetTabs(
                                currentTabRoot, PortalSettings.DesktopPages);
                            break;
                        }

                    case BindOption.BindOptionChildren:
                        {
                            authorizedTabs = GetTabs(
                                PortalSettings.ActivePage.PageID,
                                PortalSettings.DesktopPages);
                            break;
                        }

                    case BindOption.BindOptionSiblings:
                        {
                            authorizedTabs = GetTabs(
                                PortalSettings.ActivePage.ParentPageID,
                                PortalSettings.DesktopPages);
                            break;
                        }

                    // MH: added 19/09/2003 by mario@hartmann.net
                    case BindOption.BindOptionTabSibling:
                        {
                            authorizedTabs = GetTabs(
                                PortalSettings.ActivePage.PageID,
                                PortalSettings.DesktopPages);

                            if (authorizedTabs.Count == 0)
                            {
                                authorizedTabs = GetTabs(
                                    PortalSettings.ActivePage.ParentPageID,
                                    PortalSettings.DesktopPages);
                            }

                            break;
                        }

                    // MH: added 29/04/2003 by mario@hartmann.net
                    case BindOption.BindOptionDefinedParent:
                        if (this.ParentPageID != -1)
                        {
                            authorizedTabs = GetTabs(
                                this.ParentPageID, PortalSettings.DesktopPages);
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
        /// <param name="tabIndex">
        /// Index of the tab.
        /// </param>
        /// <param name="t">
        /// The pages.
        /// </param>
        /// <param name="mn">
        /// The menu tree node.
        /// </param>
        /// <returns>
        /// </returns>
        protected virtual MenuTreeNode RecourseMenu(int tabIndex, Collection<PageStripDetails> t, MenuTreeNode mn)
        {
            // mh:
            if (t.Count > 0)
            {
                foreach (var mnc in from mysubTab in t
                                    where PortalSecurity.IsInRoles(mysubTab.AuthorizedRoles)
                                    let mnc = new MenuTreeNode(mysubTab.PageName)
                                        {
                                            Link = this.GiveMeUrl(mysubTab.PageName, mysubTab.PageID), Width = mn.Width
                                        }
                                    select this.RecourseMenu(tabIndex, mysubTab.Pages, mnc))
                {
                    mn.Childs.Add(mnc);
                }
            }

            return mn;
        }

        /// <summary>
        /// Recourses the menu shop.
        /// </summary>
        /// <param name="tabIndex">Index of the tab.</param>
        /// <param name="t">The t.</param>
        /// <param name="mn">The mn.</param>
        /// <param name="idShop">The id shop.</param>
        /// <returns></returns>
        protected virtual MenuTreeNode RecourseMenuShop(int tabIndex, Collection<PageStripDetails> t, MenuTreeNode mn, int idShop)
        {
            if (t.Count > 0)
            {
                foreach (var mnc in from mysubTab in t
                                    where PortalSecurity.IsInRoles(mysubTab.AuthorizedRoles)
                                    let mnc = new MenuTreeNode(mysubTab.PageName)
                                        {
                                            Link = HttpUrlBuilder.BuildUrl(string.Format("~/{0}", HttpUrlBuilder.DefaultPage), idShop, string.Format("ItemID={0}", mysubTab.PageID)),
                                            Width = mn.Width
                                        }
                                    select this.RecourseMenuShop(tabIndex, mysubTab.Pages, mnc, idShop))
                {
                    mn.Childs.Add(mnc);
                }
            }

            return mn;
        }

/*
        /// <summary>
        /// Seems to be unused - Jes1111
        /// </summary>
        /// <param name="parentPageId">
        /// The parent page ID.
        /// </param>
        /// <param name="activePageId">
        /// The active page ID.
        /// </param>
        /// <param name="allTabs">
        /// All tabs.
        /// </param>
        /// <returns>
        /// The get selected tab.
        /// </returns>
        private int GetSelectedTab(int parentPageId, int activePageId, IList allTabs)
        {
            foreach (var tmpTab in allTabs.Cast<PageStripDetails>().Where(tmpTab => tmpTab.PageID == activePageId))
            {
                var selectedPageId = activePageId;
                if (tmpTab.ParentPageID == parentPageId)
                {
                    return selectedPageId;
                }
                
                selectedPageId = this.GetSelectedTab(parentPageId, tmpTab.ParentPageID, allTabs);
                return selectedPageId;
            }

            return 0;
        }
*/

        /// <summary>
        /// Gets the tabs.
        /// </summary>
        /// <param name="parentId">
        /// The parent ID.
        /// </param>
        /// <param name="tabs">
        /// The list of tabs.
        /// </param>
        /// <returns>
        /// An array list.
        /// </returns>
        private static ArrayList GetTabs(int parentId, IList tabs)
        {
            var authorizedTabs = new ArrayList();

            // int index = -1;

            // MH:get the selected tab for this 
            // int selectedPageID = GetSelectedTab (parentID, tabID,Tabs);

            // Populate Tab List with authorized tabs
            foreach (var tab in from PageStripDetails tab in tabs
                                where tab.ParentPageID == parentId
                                where PortalSecurity.IsInRoles(tab.AuthorizedRoles)
                                select tab)
            {
                // index = authorizedTabs.Add(tab);
                authorizedTabs.Add(tab);
            }

            return authorizedTabs;
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
            if (this.AutoBind)
            {
                this.DataBind();
            }
        }

/*
        /// <summary>
        /// Shops the desktop navigation.
        /// </summary>
        /// <param name="mytab">
        /// My tab.
        /// </param>
        private void ShopDesktopNavigation(PageStripDetails myTab)
        {
            if (PortalSecurity.IsInRoles(myTab.AuthorizedRoles))
            {
                var mn = new MenuTreeNode(myTab.PageName)
                    {
                        Link =
                            HttpUrlBuilder.BuildUrl(
                                "~/" + HttpUrlBuilder.DefaultPage, myTab.ParentPageID, "ItemID=" + myTab.PageID),
                        Height = this.Height,
                        Width = this.Width
                    };

                mn = this.RecourseMenuShop(0, myTab.Pages, mn, myTab.ParentPageID);
                this.Childs.Add(mn);
            }
        }
*/

        /// <summary>
        /// Gives the me URL.
        /// </summary>
        /// <param name="tab">
        /// The tab string.
        /// </param>
        /// <param name="id">
        /// The id int.
        /// </param>
        /// <returns>
        /// The give me url.
        /// </returns>
        private string GiveMeUrl(IEnumerable<char> tab, int id)
        {
            if (!this.UseTabNameInUrl)
            {
                return HttpUrlBuilder.BuildUrl(id);
            }

            var auxtab = tab.Aggregate(string.Empty, (current, c) => current + (char.IsLetterOrDigit(c) ? c.ToString() : "_"));

            return HttpUrlBuilder.BuildUrl(string.Format("~/{0}.aspx", auxtab), id);
        }

        #endregion
    }
}