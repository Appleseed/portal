//
namespace Appleseed.Framework.Web.UI.WebControls
{
    // mario@hartmann.net: 24/07/2003
    // modified from MenuNavigation
    // the navigation will not be effective and instead we navigate to the same page
    // and transmit the PageID as a ItemID.
    // thierry@tiptopweb.com.au: 17/09/2003
    // replace Default.aspx by DesktopDefault.aspx as we are loosing the parameters
    // when transfering from Default.aspx to DesktopDefault.aspx and not using the UrlBuilder
    // bill@billforney.com 2010/12/06 cleaned up code
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Web;

    using Appleseed.Framework.Security;
    using Appleseed.Framework.Site.Configuration;

    using DUEMETRI.UI.WebControls.HWMenu;

    /// <summary>
    /// ItemNavigation inherits from MenuNavigation
    ///     and adds the functionality of the ShopNavigation with ALL types of binding.
    ///     all subcategories are added as an ItemID property.
    /// </summary>
    public class ItemNavigation : MenuNavigation
    {
        #region Public Methods

        /// <summary>
        /// Do databind.
        /// </summary>
        public override void DataBind()
        {
            // add the root!
            this.AddRootNode();

            base.DataBind();
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
        protected override void AddMenuTreeNode(int tabIndex, PageStripDetails mytab)
        {
            if (!PortalSecurity.IsInRoles(mytab.AuthorizedRoles))
            {
                return;
            }

            // get index and id from this page and transmit them
            // Obtain PortalSettings from Current Context 
            var PortalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            var tabIdItemsRoot = PortalSettings.ActivePage.PageID;

            var mn = new MenuTreeNode(mytab.PageName)
                {
                    // change the link to stay on the same page and call a category product
                    Link = HttpUrlBuilder.BuildUrl("~/" + HttpUrlBuilder.DefaultPage, tabIdItemsRoot, "ItemID=" + mytab.PageID),
                    Width = this.Width
                };

            // fixed by manu
            mn = this.RecourseMenu(tabIdItemsRoot, mytab.Pages, mn);
            this.Childs.Add(mn);
        }

        /// <summary>
        /// modified to transmit the PageID and TabIndex for the item page
        /// </summary>
        /// <param name="tabIdItemsRoot">
        /// The tab ID items root.
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
        protected override MenuTreeNode RecourseMenu(int tabIdItemsRoot, Collection<PageStripDetails> t, MenuTreeNode mn)
        {
            if (t.Count > 0)
            {
                foreach (var mnc in from mysubTab in t
                                    where PortalSecurity.IsInRoles(mysubTab.AuthorizedRoles)
                                    let mnc = new MenuTreeNode(mysubTab.PageName)
                                        {
                                            // change PageID into ItemID for the product module on the same page
                                            Link = HttpUrlBuilder.BuildUrl("~/" + HttpUrlBuilder.DefaultPage, tabIdItemsRoot, string.Format("ItemID={0}", mysubTab.PageID)),
                                            Width = mn.Width
                                        }
                                    select this.RecourseMenu(tabIdItemsRoot, mysubTab.Pages, mnc))
                {
                    mn.Childs.Add(mnc);
                }
            }

            return mn;
        }

        /// <summary>
        /// Add the current tab as top menu item.
        /// </summary>
        private void AddRootNode()
        {
            var PortalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            var tabItemsRoot = PortalSettings.ActivePage;

            using (var mn = new MenuTreeNode(tabItemsRoot.PageName))
            {
                // change the link to stay on the same page and call a category product
                mn.Link = HttpUrlBuilder.BuildUrl("~/" + HttpUrlBuilder.DefaultPage, tabItemsRoot.PageID);
                mn.Width = this.Width;
                this.Childs.Add(mn);
            }
        }

        #endregion
    }
}