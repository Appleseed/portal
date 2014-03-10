// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagingNumbers.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The paging control is used to display paging options for controls that
//   page through data. This control is necessary since some data rendering
//   controls, such as the DataList, do not support paging and it must be
//   a custom implementation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI.WebControls
{
    using System;
    using System.ComponentModel;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// The paging control is used to display paging options for controls that
    ///     page through data. This control is necessary since some data rendering 
    ///     controls, such as the DataList, do not support paging and it must be 
    ///     a custom implementation.
    /// </summary>
    public class PagingNumbers : WebControl, INamingContainer, IPaging
    {
        // TODO: Upgrade to comosite control... :-)
        #region Constants and Fields

        /// <summary>
        ///     The default record count.
        /// </summary>
        private const int DefaultRecordCount = 0;

        /// <summary>
        ///     The current page.
        /// </summary>
        private System.Web.UI.WebControls.Label currentPage;

        /// <summary>
        ///     The next button.
        /// </summary>
        private LinkButton next;

        /// <summary>
        ///     The next button.
        /// </summary>
        private PlaceHolder nextButton;

        /// <summary>
        ///     The numerical link buttons.
        /// </summary>
        private System.Web.UI.WebControls.LinkButton[] numericalLinkButtons;

        /// <summary>
        ///     The numerical paging.
        /// </summary>
        private PlaceHolder numericalPaging;

        /// <summary>
        ///     The page number.
        /// </summary>
        private int pageNumber;

        /// <summary>
        ///     The page size.
        /// </summary>
        private int pageSize = -1;

        /// <summary>
        ///     The previous button.
        /// </summary>
        private LinkButton prev;

        /// <summary>
        ///     The previous button.
        /// </summary>
        private PlaceHolder previousButton;

        /// <summary>
        ///     The total pages.
        /// </summary>
        private int totalPages;

        #endregion

        #region Events

        /// <summary>
        ///     Event raised when a an index has been selected by the end user
        /// </summary>
        public event EventHandler OnMove;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the current page number.
        /// </summary>
        /// <value>The page number.</value>
        [Description("Specifies the current page in the index.")]
        public int PageNumber
        {
            get
            {
                // Internal is 0 based, external is 1 based
                return this.PageIndex + 1;
            }

            set
            {
                this.PageIndex = value - 1;
            }
        }

        /// <summary>
        ///     Gets or sets the Record Count
        /// </summary>
        /// <value>The record count.</value>
        public int RecordCount
        {
            get
            {
                // the RecordCount is stuffed in the ViewState so that
                // it is persisted across postbacks.
                if (this.ViewState["totalRecords"] == null)
                {
                    return DefaultRecordCount; // if it's not found in the ViewState, return the default value
                }

                return Convert.ToInt32(this.ViewState["totalRecords"].ToString());
            }

            set
            {
                this.TotalPages = CalculateTotalPages(value, this.RecordsPerPage);

                // set the viewstate
                this.ViewState["totalRecords"] = value;
            }
        }

        /// <summary>
        ///     Gets or sets the page size used in paging.
        /// </summary>
        /// <value>The records per page.</value>
        [Category("Required")]
        [Description("Specifies the page size used in paging.")]
        public int RecordsPerPage
        {
            get
            {
                if (this.pageSize == -1)
                {
                    return 10; // default
                }

                return this.pageSize;
            }

            set
            {
                this.pageSize = value;
            }
        }

        /// <summary>
        ///     Gets or sets the Forum's posts you want to view.
        /// </summary>
        /// <value>The total pages.</value>
        public int TotalPages
        {
            get
            {
                return this.totalPages + 1;
            }

            set
            {
                this.totalPages = value - 1;
            }
        }

        /// <summary>
        ///     Gets or sets the current page in the index.
        /// </summary>
        /// <value>The index of the page.</value>
        [Description("Specifies the current page in the index.")]
        private int PageIndex
        {
            get
            {
                return this.ViewState["PageIndex"] != null
                           ? Convert.ToInt32(this.ViewState["PageIndex"])
                           : this.pageNumber;
            }

            set
            {
                this.ViewState["PageIndex"] = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Static that caculates the total pages available.
        /// </summary>
        /// <param name="totalRecords">
        /// The total records.
        /// </param>
        /// <param name="pageSize">
        /// Size of the page.
        /// </param>
        /// <returns>
        /// Total number of pages available
        /// </returns>
        public static int CalculateTotalPages(int totalRecords, int pageSize)
        {
            // First calculate the division
            var totalPagesAvailable = totalRecords / pageSize;

            // Now do a mod for any remainder
            if ((totalRecords % pageSize) > 0)
            {
                totalPagesAvailable++;
            }

            return totalPagesAvailable;
        }

        #endregion

        #region Methods

        /// <summary>
        /// This event handler adds the children controls and is resonsible
        ///     for determining the template type used for the control.
        /// </summary>
        protected override void CreateChildControls()
        {
            // If the total number of records is less than the
            // number of records we display in a page, we'll display page 1 of 1.
            if ((this.RecordCount <= this.RecordsPerPage) && (this.RecordCount != 0))
            {
                this.Controls.Add(this.NavigationDisplay(true));
                this.DisplayCurrentPage();
                return;
            }

            // Quick check to ensure the PageIndex is not greater than the Page Size
            if ((this.PageIndex > this.RecordsPerPage) || (this.PageIndex < 0))
            {
                this.PageIndex = 0;
            }

            // How many link buttons do we need?
            this.numericalLinkButtons = new System.Web.UI.WebControls.LinkButton[this.TotalPages];

            // Add the control to display navigation
            this.Controls.Add(this.NavigationDisplay(false));
        }

        /// <summary>
        /// Override OnPreRender and databind
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"></see> object that contains the event data.
        /// </param>
        protected override void OnPreRender(EventArgs e)
        {
            // If the total number of records is less than the
            // number of records we display in a page, we'll simply
            // return.
            if (this.RecordCount <= this.RecordsPerPage)
            {
                return;
            }

            // Control what gets displayed
            this.DisplayPager();
        }

        /// <summary>
        /// Display the page n of n+1 text
        /// </summary>
        /// <returns>
        /// the control
        /// </returns>
        private Control CreateCurrentPage()
        {
            this.currentPage = new System.Web.UI.WebControls.Label { CssClass = "normalTextSmallBold" };

            return this.currentPage;
        }

        /// <summary>
        /// Creates numerical navigation link buttons
        /// </summary>
        /// <returns>
        /// A Control woth the navigation links
        /// </returns>
        private Control CreateNumericalNavigation()
        {
            this.numericalPaging = new PlaceHolder();

            var linkButtonsToCreate = this.TotalPages > this.numericalLinkButtons.Length
                                          ? this.numericalLinkButtons.Length
                                          : this.TotalPages;

            // Create all the link buttons
            for (var i = 0; i < linkButtonsToCreate; i++)
            {
                this.numericalLinkButtons[i] = new System.Web.UI.WebControls.LinkButton
                    {
                       CssClass = "normalTextSmallBold", Text = (i + 1).ToString("n0"), CommandArgument = i.ToString() 
                    };
                this.numericalLinkButtons[i].Click += this.PageIndexClick;
                this.numericalPaging.Controls.Add(this.numericalLinkButtons[i]);
            }

            return this.numericalPaging;
        }

        /// <summary>
        /// Creates previous/next navigation link buttons
        /// </summary>
        /// <returns>
        /// The Control
        /// </returns>
        private Control CreatePrevNextNavigation()
        {
            var prevNext = new PlaceHolder();

            // Create the previous button
            this.previousButton = new PlaceHolder();
            var whitespace = new System.Web.UI.WebControls.Label { CssClass = "normalTextSmallBold", Text = "&nbsp;" };
            this.prev = new LinkButton
                {
                   CssClass = "normalTextSmallBold", TextKey = "PREVIOUS", Text = "Prev", ID = "Prev" 
                };
            this.prev.Click += this.PrevNextClick;
            this.previousButton.Controls.Add(whitespace);
            this.previousButton.Controls.Add(this.prev);
            prevNext.Controls.Add(this.previousButton);

            // Create the next button
            this.nextButton = new PlaceHolder();
            whitespace = new System.Web.UI.WebControls.Label { CssClass = "normalTextSmallBold", Text = "&nbsp;" };
            this.next = new LinkButton
                {
                   CssClass = "normalTextSmallBold", TextKey = "NEXT", Text = "Next", ID = "Next" 
                };
            this.next.Click += this.PrevNextClick;
            this.nextButton.Controls.Add(whitespace);
            this.nextButton.Controls.Add(this.next);
            prevNext.Controls.Add(this.nextButton);

            return prevNext;
        }

        /// <summary>
        /// Displays the current page that the user is viewing
        /// </summary>
        private void DisplayCurrentPage()
        {
            this.currentPage.Text = string.Format(
                "{0} {1} {2} {3}", 
                General.GetString("PAGE", "Page", null), 
                (this.PageIndex + 1).ToString("n0"), 
                General.GetString("OF", "of", null), 
                this.TotalPages.ToString("n0"));
        }

        /// <summary>
        /// Controls how the numerical link buttons get rendered
        /// </summary>
        private void DisplayNumericalPaging()
        {
            var itemsToDisplay = 30;
            const int LowerBoundPosition = 1;
            var upperBoundPosition = this.TotalPages - 1;
            System.Web.UI.WebControls.Label label;

            // Clear out the controls
            this.numericalPaging.Controls.Clear();

            // If we have less than 6 items we don't need the fancier paging display
            if ((upperBoundPosition + 1) < (itemsToDisplay + 3))
            {
                for (var i = 0; i < (upperBoundPosition + 1); i++)
                {
                    // Don't display a link button for the existing page
                    if (i == this.PageIndex)
                    {
                        label = new System.Web.UI.WebControls.Label
                            {
                                CssClass = "normalTextSmallBold", 
                                Text = string.Format("[{0}]", (this.PageIndex + 1).ToString("n0"))
                            };
                        this.numericalPaging.Controls.Add(label);
                    }
                    else
                    {
                        this.numericalPaging.Controls.Add(this.numericalLinkButtons[i]);
                    }

                    if (i + 1 == this.numericalLinkButtons.Length)
                    {
                        continue;
                    }

                    label = new System.Web.UI.WebControls.Label { CssClass = "normalTextSmallBold", Text = ", " };

                    this.numericalPaging.Controls.Add(label);
                }

                return;
            }

            // Always display the first 3 if available
            if (this.numericalLinkButtons.Length < itemsToDisplay)
            {
                itemsToDisplay = this.numericalLinkButtons.Length;
            }

            for (var i = 0; i < itemsToDisplay; i++)
            {
                this.numericalPaging.Controls.Add(this.numericalLinkButtons[i]);

                if (i + (itemsToDisplay / 2) == itemsToDisplay)
                {
                    continue;
                }

                label = new System.Web.UI.WebControls.Label { CssClass = "normalTextSmallBold", Text = ", " };

                this.numericalPaging.Controls.Add(label);
            }

            // Handle the lower end first
            if ((this.PageIndex - LowerBoundPosition) <= (upperBoundPosition - this.PageIndex))
            {
                for (var i = itemsToDisplay; i < this.PageIndex + 2; i++)
                {
                    label = new System.Web.UI.WebControls.Label { CssClass = "normalTextSmallBold", Text = ", " };

                    this.numericalPaging.Controls.Add(label);
                    this.numericalPaging.Controls.Add(this.numericalLinkButtons[i]);
                }
            }

            // Insert the ellipses or a trailing comma if necessary
            label = new System.Web.UI.WebControls.Label { CssClass = "normalTextSmallBold" };
            if (upperBoundPosition == 3)
            {
                label.Text = ", ";
            }
            else if (upperBoundPosition >= 4)
            {
                label = new System.Web.UI.WebControls.Label { CssClass = "normalTextSmallBold", Text = " ... " };
            }

            this.numericalPaging.Controls.Add(label);

            // Handle the upper end
            if ((this.PageIndex - LowerBoundPosition) > (upperBoundPosition - this.PageIndex))
            {
                for (var i = this.PageIndex - 1; i < upperBoundPosition; i++)
                {
                    label = new System.Web.UI.WebControls.Label { CssClass = "normalTextSmallBold", Text = ", " };

                    if (i > this.PageIndex - 1)
                    {
                        this.numericalPaging.Controls.Add(label);
                    }

                    this.numericalPaging.Controls.Add(this.numericalLinkButtons[i]);
                }
            }

            // Always display the last 2 if available
            if ((this.numericalLinkButtons.Length <= 3) || (this.TotalPages <= 5))
            {
                return;
            }

            itemsToDisplay = 2;

            for (var i = itemsToDisplay; i > 0; i--)
            {
                this.numericalPaging.Controls.Add(this.numericalLinkButtons[(upperBoundPosition + 1) - i]);

                if (i + 1 == itemsToDisplay)
                {
                    continue;
                }

                var tmp = new System.Web.UI.WebControls.Label { CssClass = "normalTextSmallBold", Text = ", " };
                this.numericalPaging.Controls.Add(tmp);
            }
        }

        /// <summary>
        /// Used to display the pager. Is public so that the parent control can
        ///     reset the pager when a post back occurs that the pager did not raise.
        /// </summary>
        private void DisplayPager()
        {
            this.DisplayCurrentPage();
            this.DisplayNumericalPaging();
            this.DisplayPrevNext();
        }

        /// <summary>
        /// Controls how the previous next link buttons get rendered
        /// </summary>
        private void DisplayPrevNext()
        {
            this.prev.CommandArgument = (this.PageIndex - 1).ToString();
            this.next.CommandArgument = (this.PageIndex + 1).ToString();

            // Control what gets displayed
            if ((this.PageIndex > 0) && ((this.PageIndex + 1) < this.TotalPages))
            {
                this.nextButton.Visible = true;
                this.previousButton.Visible = true;
            }
            else if (this.PageIndex == 0)
            {
                this.nextButton.Visible = true;
                this.previousButton.Visible = false;
            }
            else if ((this.PageIndex + 1) == this.TotalPages)
            {
                this.nextButton.Visible = false;
                this.previousButton.Visible = true;
            }
        }

        /// <summary>
        /// Control that contains all the navigation display details.
        /// </summary>
        /// <param name="singlePage">
        /// if set to <c>true</c> [single page].
        /// </param>
        /// <returns>
        /// The navigation control
        /// </returns>
        private Control NavigationDisplay(bool singlePage)
        {
            System.Web.UI.WebControls.Label navigation;

            // Create a new table
            var table = new Table { CellPadding = 0, CellSpacing = 0, Width = Unit.Percentage(100) };

            // We only have a single row
            var tr = new TableRow();

            // Two columns. One for the current page and one for navigation
            var td = new TableCell();

            // Display the current page
            td.Controls.Add(this.CreateCurrentPage());
            tr.Controls.Add(td);

            // Do we have multiple pages to display?
            if (!singlePage)
            {
                // Create page navigation
                td = new TableCell { HorizontalAlign = HorizontalAlign.Right };
                navigation = new System.Web.UI.WebControls.Label();
                var navigationText = new Label
                    {
                       CssClass = "normalTextSmallBold", TextKey = "GOTO_PAGE", Text = "Goto to page: " 
                    };

                navigation.Controls.Add(navigationText);

                // Numerical Paging
                navigation.Controls.Add(this.CreateNumericalNavigation());

                // Prev Next Paging
                navigation.Controls.Add(this.CreatePrevNextNavigation());

                td.Controls.Add(navigation);
                tr.Controls.Add(td);
            }

            table.Controls.Add(tr);

            return table;
        }

        /// <summary>
        /// Event raised when a new index is selected from the paging control
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void PageIndexClick(object sender, EventArgs e)
        {
            var requestedPage = ((System.Web.UI.WebControls.LinkButton)sender).CommandArgument;

            if (requestedPage.Length <= 0)
            {
                return;
            }

            this.PageIndex = Convert.ToInt32(requestedPage);

            if (null != this.OnMove)
            {
                this.OnMove(sender, e);
            }
        }

        /// <summary>
        /// Event raised when a new index is selected from the paging control
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void PrevNextClick(object sender, EventArgs e)
        {
            var requestedPage = ((System.Web.UI.WebControls.LinkButton)sender).CommandArgument;

            if (requestedPage.Length <= 0)
            {
                return;
            }

            this.PageIndex = Convert.ToInt32(requestedPage);

            if (null != this.OnMove)
            {
                this.OnMove(sender, e);
            }
        }

        #endregion
    }
}