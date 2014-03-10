using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Appleseed.Framework.Security;
using Appleseed.Framework.Site.Configuration;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    ///	This user control will render the breadcrumb navigation for the current tab.
    ///	It does not render anything when the user on a first level tab.
    ///	Ver. 1.0 - 24 dec 2002 - First release by Cory Isakson
    ///	Ver. 1.1 - 31 jan 2003 - Update by jes - see readme
    ///	Ver. 2.0 - 28 feb 2003 - Update by Manu - Transformed in Table Webcontrol
    ///	                         Cleaned up the code, added support for design time
    ///	Ver. 2.1 - 25.04.2003  - Indah Fuldner: Display breadcrumbs if the user has click a tab link  (Without hit the Database again)
    /// Ver. 2.2 - 20.09.2004 -  john.mandia@whitelightsolutions.com ported code to support zen style of layout. 
    /// </summary>
    public class ZenBreadCrumbs : Literal
    {
        private string _cssClass = "breadcrumbs";
        private string _separator = " > ";
        private string _urlStyle = string.Empty;

        /// <summary>
        /// CssClass
        /// </summary>
        /// <value>The CSS class.</value>
        public string CssClass
        {
            get { return _cssClass; }
            set { _cssClass = value; }
        }

        /// <summary>
        /// Separator
        /// </summary>
        /// <value>The separator.</value>
        public string Separator
        {
            get { return _separator; }
            set { _separator = value; }
        }

        /// <summary>
        /// If string is tabname it appends the tabname to the end of the url
        /// </summary>
        /// <value>The URL style.</value>
        public virtual string UrlStyle
        {
            get { return _urlStyle; }
            set { _urlStyle = value; }
        }


        /// <summary>
        /// Override CreateChildControls to create the control tree.
        /// </summary>
        protected override void CreateChildControls()
        {
            // Create an arraylist to fill with 
            // the TabItems representing the Tree 
            ArrayList crumbs;

            if (HttpContext.Current != null)
            {
                // Obtain PortalSettings from Current Context 
                PortalSettings PortalSettings = (PortalSettings) HttpContext.Current.Items["PortalSettings"];

                //Display breadcrumbs if the user has click a tab link  (Without hit the Database again)
                if (PortalSettings.ActivePage.PageID > 0)
                {
                    ArrayList authorizedTabs = new ArrayList();
                    int addedTabs = 0;
                    for (int i = 0; i < PortalSettings.DesktopPages.Count; i++)
                    {
                        PageStripDetails tab = (PageStripDetails) PortalSettings.DesktopPages[i];

                        if (PortalSecurity.IsInRoles(tab.AuthorizedRoles))
                            authorizedTabs.Add(tab);
                        addedTabs++;
                    }

                    crumbs = GetBreadCrumbs(PortalSettings.ActivePage, authorizedTabs);
                    crumbs.Sort();
                }
                else
                    crumbs = new ArrayList();
            }
            else //design time
            {
                crumbs = new ArrayList();
                crumbs.Add("Item1");
                crumbs.Add("Item2");
                crumbs.Add("Item3");
            }

            if (crumbs.Count > 1)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<div class='");
                sb.Append(CssClass);
                sb.Append("'>");

                int ct = 0;

                // Build the Breadcrumbs and add them to the div 
                foreach (PageItem item in crumbs)
                {
                    if (ct > 0)
                    {
                        sb.Append(Separator.ToString());
                    }
                    if (ct != (crumbs.Count - 1))
                    {
                        sb.Append("<a href='");
                        sb.Append(HttpUrlBuilder.BuildUrl(item.ID));
                        sb.Append("'>");
                        sb.Append(item.Name.ToString());
                        sb.Append("</a>");
                    }
                    else
                    {
                        sb.Append(item.Name.ToString());
                    }
                    ct++;
                }
                sb.Append("</div>");
                Text = sb.ToString();
            }
            else
            {
                Visible = false;
            }
        }

        /// <summary>
        /// Gets the bread crumbs.
        /// </summary>
        /// <param name="tab">The tab.</param>
        /// <param name="tabList">The tab list.</param>
        /// <returns></returns>
        private ArrayList GetBreadCrumbs(PageSettings tab, ArrayList tabList)
        {
            int parentTabID = tab.PageID;
            int test = tab.PageID;

            ArrayList _breadCrumbsText = new ArrayList();

            PageItem myTabItem = new PageItem();
            myTabItem.ID = tab.PageID;
            myTabItem.Name = tab.PageName;
            myTabItem.Order = tab.PageOrder;

            _breadCrumbsText.Add(myTabItem);

            //Search for the root tab in current array
            PageStripDetails rootTab;

            for (int i = 0; i < tabList.Count; i++)
            {
                rootTab = (PageStripDetails) tabList[i];

                if (rootTab.PageID == parentTabID)
                {
                    parentTabID = rootTab.ParentPageID;
                    if (test != rootTab.PageID)
                    {
                        PageItem myItem = new PageItem();
                        myItem.ID = rootTab.PageID;
                        myItem.Name = rootTab.PageName;
                        myItem.Order = rootTab.PageOrder;
                        _breadCrumbsText.Add(myItem);
                    }
                    if (parentTabID != 0)
                        i = -1;
                    else
                        return _breadCrumbsText;
                }
            }
            return _breadCrumbsText;
        }
    }
}