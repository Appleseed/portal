// Tiptopweb: 27 Jan 2003
// modified from MenuNavigation to replace the Category module:
// the navigation will not be effective and instead we navigate to the same page
// and transmit the PageID as a CatID to the Product list module.
// jviladiu@portalServices.net 21/07/2004: Clean code & added localization for "Shop home"
// bill@billforney.com 2010/12/06: Cleaned up and converted some things to LINQ syntax.

namespace Appleseed.Framework.Web.UI.WebControls
{
    using System;
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
    public class ShopNavigation : Menu, INavigation
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
        /// Initializes a new instance of the <see cref="ShopNavigation"/> class.
        /// </summary>
        public ShopNavigation()
        {
            base.EnableViewState = false;
            this.Load += this.LoadControl;
        }

        #endregion

        // MH: end
        #region Properties

        /// <summary>
        ///     Gets or sets a value indicating whether control should bind when loads
        /// </summary>
        /// <value><c>true</c> if [auto bind]; otherwise, <c>false</c>.</value>
        [Category("Data")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool AutoBind { get; set; }

        /// <summary>
        ///     Gets or sets how this control should bind to db data
        /// </summary>
        /// <value>The bind option.</value>
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
        ///     Gets or sets the parentPageID when using BindOptionDefinedParent
        /// MH: added 23/05/2003 by mario@hartmann.net
        /// </summary>
        /// <value>The parent page ID.</value>
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

        #endregion

        // MH: end
        #region Public Methods

        /// <summary>
        /// Do databind.
        ///     Thanks to abain for cleaning up the code
        /// </summary>
        public override void DataBind()
        {
            var currentTabOnly = this.Bind == BindOption.BindOptionCurrentChilds;

            // Obtain PortalSettings from Current Context 
            var PortalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];

            // Build list of tabs to be shown to user 
            var authorizedTabs = PortalSettings.DesktopPages.Cast<PageStripDetails>().Where(tab => PortalSecurity.IsInRoles(tab.AuthorizedRoles)).ToList();

            // Menu 

            // add the shop home!
            this.AddShopHomeNode();

            if (!currentTabOnly)
            {
                foreach (var mytab in authorizedTabs)
                {
                    this.AddMenuTreeNode(mytab);
                }
            }
            else
            {
                if (authorizedTabs.Count >= 0)
                {
                    var mytab = PortalSettings.GetRootPage(PortalSettings.ActivePage, authorizedTabs);

                    if (mytab.Pages.Count > 0)
                    {
                        foreach (var mysubTab in mytab.Pages)
                        {
                            this.AddMenuTreeNode(mysubTab);
                        }
                    }
                }
            }

            base.DataBind();
        }

        #endregion

        #region Methods

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
        /// Add a Menu Tree Node if user in in the list of Authorized roles.
        ///     Thanks to abain for fixing authorization bug.
        /// </summary>
        /// <param name="mytab">
        /// Tab to add to the MenuTreeNodes collection
        /// </param>
        private void AddMenuTreeNode(PageStripDetails mytab)
        {
            if (PortalSecurity.IsInRoles(mytab.AuthorizedRoles))
            {
                // get index and id from this page and transmit them
                // Obtain PortalSettings from Current Context 
                var PortalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
                var tabIdShop = PortalSettings.ActivePage.PageID;

                var mn = new MenuTreeNode(mytab.PageName)
                    {
                        // change the link to stay on the same page and call a category product
                        Link =
                            HttpUrlBuilder.BuildUrl(
                                "~/" + HttpUrlBuilder.DefaultPage, tabIdShop, "ItemID=" + mytab.PageID),
                        Width = this.Width
                    };

                mn = this.RecourseMenu(tabIdShop, mytab.Pages, mn);
                this.Childs.Add(mn);
            }
        }

        /// <summary>
        /// Adds the shop home node.
        /// </summary>
        private void AddShopHomeNode()
        {
            var PortalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            var tabIdShop = PortalSettings.ActivePage.PageID;

            var mn = new MenuTreeNode(General.GetString("PRODUCT_HOME", "Shop Home"))
                {
                    // change the link to stay on the same page and call a category product
                    Link = HttpUrlBuilder.BuildUrl(tabIdShop),
                    Width = this.Width 
                };

            this.Childs.Add(mn);
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

        /// <summary>
        /// Recourses the menu.
        /// modified to transmit the PageID and TabIndex for the shop page
        /// </summary>
        /// <param name="tabIdShop">
        /// The tab ID shop.
        /// </param>
        /// <param name="t">
        /// The pages box.
        /// </param>
        /// <param name="mn">
        /// The menu tree node.
        /// </param>
        /// <returns>
        /// A menu tree node.
        /// </returns>
        private MenuTreeNode RecourseMenu(int tabIdShop, Collection<PageStripDetails> t, MenuTreeNode mn)
        {
            if (t.Count > 0)
            {
                foreach (var mnc in from mysubTab in t
                                    where PortalSecurity.IsInRoles(mysubTab.AuthorizedRoles)
                                    let mnc = new MenuTreeNode(mysubTab.PageName)
                                        {
                                            Link = HttpUrlBuilder.BuildUrl("~/" + HttpUrlBuilder.DefaultPage, tabIdShop, "ItemID=" + mysubTab.PageID), Width = mn.Width
                                        }
                                    select this.RecourseMenu(tabIdShop, mysubTab.Pages, mnc))
                {
                    mn.Childs.Add(mnc);
                }
            }

            return mn;
        }

        #endregion
    }
}