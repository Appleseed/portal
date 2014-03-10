// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormViewAdapter.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The form view adapter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSSFriendly
{
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// The form view adapter.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class FormViewAdapter : CompositeDataBoundControlAdapter
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FormViewAdapter"/> class. 
        ///   Initializes a new instance of the <see cref="T:System.Web.UI.WebControls.Adapters.DataBoundControlAdapter"/> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public FormViewAdapter()
        {
            this._classMain = "AspNet-FormView";
            this._classHeader = "AspNet-FormView-Header";
            this._classData = "AspNet-FormView-Data";
            this._classFooter = "AspNet-FormView-Footer";
            this._classPagination = "AspNet-FormView-Pagination";
            this._classOtherPage = "AspNet-FormView-OtherPage";
            this._classActivePage = "AspNet-FormView-ActivePage";
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets a value indicating whether [allow paging].
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected override bool AllowPaging
        {
            get
            {
                return this.ControlAsFormView.AllowPaging;
            }
        }

        /// <summary>
        ///   Gets the data item count.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected override int DataItemCount
        {
            get
            {
                return this.ControlAsFormView.DataItemCount;
            }
        }

        /// <summary>
        ///   Gets the index of the data item.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected override int DataItemIndex
        {
            get
            {
                return this.ControlAsFormView.DataItemIndex;
            }
        }

        /// <summary>
        ///   Gets the footer row.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected override TableRow FooterRow
        {
            get
            {
                return this.ControlAsFormView.FooterRow;
            }
        }

        /// <summary>
        ///   Gets the footer template.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected override ITemplate FooterTemplate
        {
            get
            {
                return this.ControlAsFormView.FooterTemplate;
            }
        }

        /// <summary>
        ///   Gets the footer text.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected override string FooterText
        {
            get
            {
                return this.ControlAsFormView.FooterText;
            }
        }

        /// <summary>
        ///   Gets the header row.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected override TableRow HeaderRow
        {
            get
            {
                return this.ControlAsFormView.HeaderRow;
            }
        }

        /// <summary>
        ///   Gets the header template.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected override ITemplate HeaderTemplate
        {
            get
            {
                return this.ControlAsFormView.HeaderTemplate;
            }
        }

        /// <summary>
        ///   Gets the header text.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected override string HeaderText
        {
            get
            {
                return this.ControlAsFormView.HeaderText;
            }
        }

        /// <summary>
        ///   Gets the pager settings.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected override PagerSettings PagerSettings
        {
            get
            {
                return this.ControlAsFormView.PagerSettings;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds the item.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void BuildItem(HtmlTextWriter writer)
        {
            if ((this.ControlAsFormView.Row == null) || (this.ControlAsFormView.Row.Cells.Count <= 0) ||
                (this.ControlAsFormView.Row.Cells[0].Controls.Count <= 0))
            {
                return;
            }

            writer.WriteLine();
            writer.WriteBeginTag("div");
            writer.WriteAttribute("class", this._classData);
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Indent++;
            writer.WriteLine();

            foreach (Control itemCtrl in this.ControlAsFormView.Row.Cells[0].Controls)
            {
                itemCtrl.RenderControl(writer);
            }

            writer.Indent--;
            writer.WriteLine();
            writer.WriteEndTag("div");
        }

        #endregion
    }
}