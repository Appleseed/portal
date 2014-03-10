using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// Paging class, Appleseed special edition
    /// </summary>
    [
        DefaultProperty("PageNumber"),
            ToolboxData("<{0}:Paging TextKey='' runat=server></{0}:Paging>"),
            Designer("Appleseed.Framework.UI.Design.PagingDesigner")
        ]
    public class Paging : WebControl, IPaging
    {
        /// <summary>
        /// Page number
        /// </summary>
        protected TextBox tbPageNumber;

        /// <summary>
        /// Total page count
        /// </summary>
        protected Label lblPageCount;

        /// <summary>
        /// Label containg text 'of'
        /// </summary>
        protected Label lblof;

        /// <summary>
        /// Button 'First'
        /// </summary>
        protected Button btnFirst;

        /// <summary>
        /// Button 'Previous'
        /// </summary>
        protected Button btnPrev;

        /// <summary>
        /// Button 'Next'
        /// </summary>
        protected Button btnNext;

        /// <summary>
        /// Button 'Last'
        /// </summary>
        protected Button btnLast;

        /// <summary>
        /// Move event raised when a move is performed
        /// </summary>
        public event EventHandler OnMove;

        // variables we use to manage state

        private int m_recordsPerPage = 10;

        /// <summary>
        /// Number of records per page
        /// </summary>
        /// <value>The records per page.</value>
        public int RecordsPerPage
        {
            get { return m_recordsPerPage; }
            set { m_recordsPerPage = value; }
        }

        /// <summary>
        /// Hide when on single page hides controls when
        /// there is only one page
        /// </summary>
        /// <value><c>true</c> if [hide on single page]; otherwise, <c>false</c>.</value>
        public bool HideOnSinglePage
        {
            get { return Convert.ToBoolean(ViewState["HideOnSinglePage"]); }
            set { ViewState["HideOnSinglePage"] = value.ToString(); }
        }

        /// <summary>
        /// Current page number
        /// </summary>
        /// <value>The page number.</value>
        public int PageNumber
        {
            get { return Convert.ToInt32(tbPageNumber.Text); }
            set { tbPageNumber.Text = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether validation is performed when the control buttons are clicked.
        /// </summary>
        /// <value><c>true</c> if [causes validation]; otherwise, <c>false</c>.</value>
        [
            Description(
                "Gets or sets a value indicating whether validation is performed when the control buttons are clicked.")
            ]
        public bool CausesValidation
        {
            get
            {
                object causesValidation = ViewState["CausesValidation"];
                if (causesValidation != null)
                    return (bool) causesValidation;
                else
                    return true;
            }
            set { ViewState["CausesValidation"] = value; }
        }

        /// <summary>
        /// Total Record Count
        /// </summary>
        /// <value>The record count.</value>
        public int RecordCount
        {
            get { return Convert.ToInt32(ViewState["RecordCount"]); }
            set { ViewState["RecordCount"] = value.ToString(); }
        }

        /// <summary>
        /// Total pages count
        /// </summary>
        /// <value>The page count.</value>
        public int PageCount
        {
            get
            {
                // Calculate page count
                int _PageCount = RecordCount/RecordsPerPage;

                // adjust for spillover
                if (RecordCount%RecordsPerPage > 0)
                    _PageCount++;

                if (_PageCount < 1)
                    _PageCount = 1;

                return _PageCount;
            }
        }

        /// <summary>
        /// Used by OnMove event
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnMoveHandler(EventArgs e)
        {
            if (OnMove != null)
            {
                OnMove(this, e);
            }
        }

        /// <summary>
        /// Enable/disable the nav controls based on the current context, update labels
        /// </summary>
        public void RefreshButtons()
        {
            // enable/disable the nav controls based on the current context
            // we should only show the first button if we're NOT on the first page already
            btnFirst.Enabled = (PageNumber != 1);
            btnPrev.Enabled = (PageNumber > 1);
            btnNext.Enabled = (PageNumber < PageCount);
            btnLast.Enabled = (PageNumber != PageCount);

            //Update labels
            lblPageCount.Text = PageCount.ToString();

            if (PageCount <= 1 && HideOnSinglePage)
            {
                btnFirst.Visible = false;
                btnPrev.Visible = false;
                btnNext.Visible = false;
                btnLast.Visible = false;
                lblof.Visible = false;
                lblPageCount.Visible = false;
                tbPageNumber.Visible = false;
            }
            else
            {
                btnFirst.Visible = true;
                btnPrev.Visible = true;
                btnNext.Visible = true;
                btnLast.Visible = true;
                lblof.Visible = true;
                lblPageCount.Visible = true;
                tbPageNumber.Visible = true;
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            RefreshButtons();
        }

        /// <summary>
        /// Main class manging pages
        /// </summary>
        public Paging()
        {
            //Construct controls
            btnFirst = new Button();
            btnFirst.Width = new Unit("25px");
            btnFirst.Font.Bold = true;
            btnFirst.Text = " |< ";
            btnFirst.CommandArgument = "first";
            btnFirst.EnableViewState = false;
            Controls.Add(btnFirst);

            btnPrev = new Button();
            btnPrev.Width = new Unit("25px");
            btnPrev.Font.Bold = true;
            btnPrev.Text = " < ";
            btnPrev.CommandArgument = "prev";
            btnPrev.EnableViewState = false;
            Controls.Add(btnPrev);

            Controls.Add(new LiteralControl("&#160;"));

            tbPageNumber = new TextBox();
            tbPageNumber.Width = new Unit("30px");
            tbPageNumber.EnableViewState = true;
            tbPageNumber.AutoPostBack = true;
            Controls.Add(tbPageNumber);

            Controls.Add(new LiteralControl("&#160;"));

            btnNext = new Button();
            btnNext.Width = new Unit("25px");
            btnNext.Font.Bold = true;
            btnNext.Text = " > ";
            btnNext.CommandArgument = "next";
            btnNext.EnableViewState = false;
            Controls.Add(btnNext);

            btnLast = new Button();
            btnLast.Width = new Unit("25px");
            btnLast.Font.Bold = true;
            btnLast.Text = " >| ";
            btnLast.CommandArgument = "last";
            btnLast.EnableViewState = false;
            Controls.Add(btnLast);

            lblof = new Label();
            lblof.EnableViewState = false;
            lblof.Text = "&#160;/&#160;";
            Controls.Add(lblof);

            lblPageCount = new Label();
            lblPageCount.EnableViewState = false;
            Controls.Add(lblPageCount);

            //Set defaults
            if (ViewState["PageNumber"] == null)
                PageNumber = 1;

            if (ViewState["RecordCount"] == null)
                RecordCount = 1;

            if (ViewState["HideOnSinglePage"] == null)
                HideOnSinglePage = true;

            //Add handlers
            Load += new EventHandler(Page_Load);
            tbPageNumber.TextChanged += new EventHandler(NavigationTbClick);
            btnFirst.Click += new EventHandler(NavigationButtonClick);
            btnPrev.Click += new EventHandler(NavigationButtonClick);
            btnNext.Click += new EventHandler(NavigationButtonClick);
            btnLast.Click += new EventHandler(NavigationButtonClick);
        }

        /// <summary>
        /// Navigations the button click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void NavigationButtonClick(Object sender, EventArgs e)
        {
            // get the command
            string arg = ((Button) sender).CommandArgument;
            // do the command
            switch (arg)
            {
                case ("next"):
                    if (PageNumber < PageCount)
                        PageNumber++;
                    break;
                case ("prev"):
                    if (PageNumber > 1)
                        PageNumber--;
                    break;
                case ("last"):
                    PageNumber = PageCount;
                    break;
                case ("first"):
                    PageNumber = 1;
                    break;
            }

            RefreshButtons();

            //Raise the event OnMove
            OnMoveHandler(new EventArgs());
        }

        /// <summary>
        /// Navigations the tb click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void NavigationTbClick(Object sender, EventArgs e)
        {
            int _PageNumber = Convert.ToInt32(tbPageNumber.Text);

            if (_PageNumber > PageCount)
            {
                PageNumber = PageCount;
            }
            else if (_PageNumber < 1)
            {
                PageNumber = 1;
            }
            else
            {
                PageNumber = _PageNumber;
            }

            RefreshButtons();

            //Raise the event OnMove
            OnMoveHandler(new EventArgs());
        }
    }
}