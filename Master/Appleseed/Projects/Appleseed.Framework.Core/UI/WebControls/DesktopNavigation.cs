// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesktopNavigation.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Represents a flat navigation bar.
//   One dimension.
//   Can render horizontally or vertically.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI.WebControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Appleseed.Framework.Security;
    using Appleseed.Framework.Site.Configuration;

    /// <summary>
    /// Represents a flat navigation bar. 
    ///   One dimension. 
    ///   Can render horizontally or vertically.
    /// </summary>
    public class DesktopNavigation : DataList, INavigation
    {
        #region Constants and Fields

        /// <summary>
        /// The _bind.
        /// </summary>
        private BindOption _bind = BindOption.BindOptionTop;

        // MH: added 29/04/2003 by mario@hartmann.net
        /// <summary>
        /// The _defined parent tab.
        /// </summary>
        private int _definedParentTab = -1;

        /// <summary>
        /// The inner data source.
        /// </summary>
        private object innerDataSource;

        /// <summary>
        /// The rd.
        /// </summary>
        private RepeatDirection rd = RepeatDirection.Horizontal;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopNavigation"/> class. 
        ///   Default constructor
        /// </summary>
        public DesktopNavigation()
        {
            this.EnableViewState = false;
            this.RepeatDirection = RepeatDirection.Horizontal;
            this.Load += this.LoadControl;
        }

        #endregion

        // MH: end
        #region Properties

        /// <summary>
        ///   Indicates if control should bind when loads
        /// </summary>
        /// <value><c>true</c> if [auto bind]; otherwise, <c>false</c>.</value>
        [Category("Data")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool AutoBind { get; set; }

        /// <summary>
        ///   Describes how this control should bind to db data
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
        ///   Gets or sets the source containing a list of values used to populate the items within the control.
        /// </summary>
        /// <value>The data source.</value>
        /// <returns>An <see cref = "T:System.Collections.IEnumerable" /> or <see cref = "T:System.ComponentModel.IListSource" /> that contains a collection of values used to supply data to this control. The default value is null.</returns>
        /// <exception cref = "T:System.Web.HttpException">The data source cannot be resolved because a value is specified for both the <see cref = "P:System.Web.UI.WebControls.BaseDataList.DataSource" /> property and the <see cref = "P:System.Web.UI.WebControls.BaseDataList.DataSourceID" /> property. </exception>
        /// <exception cref = "T:System.ArgumentException">The data source is of an invalid type. The data source must be null or implement either the <see cref = "T:System.Collections.IEnumerable" /> or the <see cref = "T:System.ComponentModel.IListSource" /> interface.</exception>
        /// <remarks>
        /// </remarks>
        public override object DataSource
        {
            get
            {
                return this.innerDataSource ?? (this.innerDataSource = this.GetInnerDataSource());
            }

            set
            {
                this.innerDataSource = value;
            }
        }

        // MH: added 23/05/2003 by mario@hartmann.net
        /// <summary>
        ///   defines the parentPageID when using BindOptionDefinedParent
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
        ///   Gets or sets whether the <see cref = "T:System.Web.UI.WebControls.DataList"></see> control displays vertically or horizontally.
        /// </summary>
        /// <value></value>
        /// <returns>One of the <see cref = "T:System.Web.UI.WebControls.RepeatDirection"></see> values. The default is Vertical.</returns>
        /// <exception cref = "T:System.ArgumentException">The specified value is not one of the <see cref = "T:System.Web.UI.WebControls.RepeatDirection"></see> values. </exception>
        [DefaultValue(RepeatDirection.Horizontal)]
        public override RepeatDirection RepeatDirection
        {
            get
            {
                return this.rd;
            }

            set
            {
                this.rd = value;
            }
        }

        /// <summary>
        ///   Indicates if control show the tabname in the url
        /// </summary>
        /// <value><c>true</c> if [use tab name in URL]; otherwise, <c>false</c>.</value>
        [Category("Data")]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool UseTabNameInUrl { get; set; }

        #endregion

        #region Public Methods

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
        public string giveMeUrl(string tab, int id)
        {
            if (!this.UseTabNameInUrl)
            {
                return HttpUrlBuilder.BuildUrl(id);
            }

            var auxtab = tab.Aggregate(
                string.Empty, (current, c) => current + (char.IsLetterOrDigit(c) ? c.ToString() : "_"));
            return HttpUrlBuilder.BuildUrl(string.Format("~/{0}.aspx", auxtab), id);
        }

        #endregion

        // MH: end
        #region Methods

        /// <summary>
        /// Populates ArrayList of tabs based on binding option selected.
        /// </summary>
        /// <returns>
        /// The get inner data source.
        /// </returns>
        protected object GetInnerDataSource()
        {
            var authorizedTabs = new List<PageStripDetails>();

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

                            // 						int tmpPageID = 0;
                            // 						if(PortalSettings.ActivePage.ParentPageID == 0)
                            // 						{
                            // 							tmpPageID = PortalSettings.ActivePage.PageID;
                            // 						}
                            // 						else
                            // 						{
                            // 							tmpPageID = PortalSettings.ActivePage.ParentPageID;
                            // 						}
                            // 						ArrayList parentTabs = GetTabs(tmpPageID, PortalSettings.DesktopPages);
                            // 						try
                            // 						{
                            // 							if (parentTabs.Count > 0)
                            // 							{
                            // 								PageStripDetails currentParentTab = (PageStripDetails) parentTabs[this.SelectedIndex];
                            // 								this.SelectedIndex = -1;
                            // 								authorizedTabs = GetTabs(PortalSettings.ActivePage.PageID, currentParentTab.Pages);
                            // 							}
                            // 						}
                            // 						catch
                            // 						{}
                            // 						break;
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
        /// Gets the selected tab.
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
        private int GetSelectedTab(int parentPageID, int activePageID, IEnumerable<PageStripDetails> allTabs)
        {
            foreach (var tmpTab in allTabs)
            {
                if (tmpTab.PageID != activePageID)
                {
                    continue;
                }

                var selectedPageID = activePageID;
                if (tmpTab.ParentPageID == parentPageID)
                {
                    return selectedPageID;
                }

                selectedPageID = this.GetSelectedTab(parentPageID, tmpTab.ParentPageID, allTabs);
                return selectedPageID;
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
        /// <param name="tabs">
        /// The tabs.
        /// </param>
        /// <returns>
        /// </returns>
        private List<PageStripDetails> GetTabs(int parentID, int tabID, IEnumerable<PageStripDetails> tabs)
        {
            var authorizedTabs = new List<PageStripDetails>();

            // MH:get the selected tab for this 
            var selectedPageID = this.GetSelectedTab(parentID, tabID, tabs);

            // Populate Tab List with authorized tabs
            // Get selected row only
            foreach (var tab in from PageStripDetails tab in tabs
                                where tab.ParentPageID == parentID
                                where PortalSecurity.IsInRoles(tab.AuthorizedRoles)
                                select tab)
            {
                authorizedTabs.Add(tab);
                var index = authorizedTabs.IndexOf(tab);

                // MH:if (tab.PageID == tabID)
                // MH:added to support the selected menutab in each level
                if (tab.PageID == selectedPageID)
                {
                    this.SelectedIndex = index;
                }
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
        /// The <see cref="T:System.EventArgs"/> instance containing the event data.
        /// </param>
        private void LoadControl(object sender, EventArgs e)
        {
            if (this.AutoBind)
            {
                this.DataBind();
            }
        }

        #endregion
    }
}