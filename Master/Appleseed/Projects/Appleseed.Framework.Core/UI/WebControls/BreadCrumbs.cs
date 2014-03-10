using System.Collections;
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
    /// </summary>
    public class BreadCrumbs : Table
    {
        private string _textclass = "bc_Text";
        private string _linkclass = "bc_Link";
        private string _separator = " > ";

        /// <summary>
        /// TextCSSClass
        /// </summary>
        /// <value>The text CSS class.</value>
        public string TextCSSClass
        {
            get { return _textclass; }
            set { _textclass = value; }
        }

        /// <summary>
        /// LinkCSSClass
        /// </summary>
        /// <value>The link CSS class.</value>
        public string LinkCSSClass
        {
            get { return _linkclass; }
            set { _linkclass = value; }
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


        // Override CreateChildControls to create the control tree.
        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            // jes1111
            if (!((Page) Page).IsCssFileRegistered("Mod_Breadcrumbs"))
                ((Page) Page).RegisterCssFile("Mod_Breadcrumbs");


            // Create an arraylist to fill with 
            // the PageItems representing the Tree 
            ArrayList crumbs;

            if (HttpContext.Current != null)
            {
                // Obtain PortalSettings from Current Context 
                PortalSettings PortalSettings = (PortalSettings) HttpContext.Current.Items["PortalSettings"];

                //Changes by Indah Fuldner 25.04.2003
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
                    //crumbs.Sort();
                    //Fixing bug: http://support.Appleseedportal.net/jira/browse/RBP-704
                    crumbs.Reverse();
                    //End Changes by Indah Fuldner
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
                TableRow r = new TableRow();
                TableCell c = new TableCell();
                c.CssClass = "breadcrumbs";

                int ct = 0;

                // Build the Breadcrumbs and add them to the table 
                foreach (PageItem item in crumbs)
                {
                    if (ct > 0)
                    {
                        Label divider = new Label();
                        divider.Text = Separator;
                        divider.CssClass = TextCSSClass;
                        divider.EnableViewState = false;
                        c.Controls.Add(divider);
                    }
                    if (ct != (crumbs.Count - 1))
                    {
                        HyperLink link = new HyperLink();
                        link.Text = item.Name;
                        link.NavigateUrl = HttpUrlBuilder.BuildUrl(item.ID);
                        link.EnableViewState = false;
                        link.CssClass = LinkCSSClass;
                        c.Controls.Add(link);
                    }
                    else
                    {
                        Label lastlink = new Label();
                        lastlink.Text = item.Name;
                        lastlink.CssClass = LinkCSSClass;
                        lastlink.EnableViewState = false;
                        c.Controls.Add(lastlink);
                    }
                    ct++;
                }

                r.Cells.Add(c);
                Rows.Add(r);
            }
            else
            {
                Visible = false;
            }
        }

        //Add Indah Fuldner 25.04.2003 (With assumtion that the rootlevel tab has ParentPageID = 0)
        /// <summary>
        /// Gets the bread crumbs.
        /// </summary>
        /// <param name="tab">The tab.</param>
        /// <param name="tabList">The tab list.</param>
        /// <returns></returns>
        private ArrayList GetBreadCrumbs(PageSettings tab, ArrayList tabList)
        {
            int parentPageID = tab.PageID;
            //string test=tab.PageName;      
            int test = tab.PageID;

            ArrayList _breadCrumbsText = new ArrayList();

            PageItem myPageItem = new PageItem();
            myPageItem.ID = tab.PageID;
            myPageItem.Name = tab.PageName;
            myPageItem.Order = tab.PageOrder;

            _breadCrumbsText.Add(myPageItem);

            //Search for the root tab in current array
            PageStripDetails rootTab;

            for (int i = 0; i < tabList.Count; i++)
            {
                rootTab = (PageStripDetails) tabList[i];

                if (rootTab.PageID == parentPageID)
                {
                    parentPageID = rootTab.ParentPageID;
                    if (test != rootTab.PageID) //(test!=rootTab.PageName)
                    {
                        PageItem myItem = new PageItem();
                        myItem.ID = rootTab.PageID;
                        myItem.Name = rootTab.PageName;
                        myItem.Order = rootTab.PageOrder;
                        _breadCrumbsText.Add(myItem);
                    }
                    if (parentPageID != 0)
                        i = -1;
                    else
                        return _breadCrumbsText;
                }
            }
            return _breadCrumbsText;
        }

        //End Indah Fuldner
    }
}