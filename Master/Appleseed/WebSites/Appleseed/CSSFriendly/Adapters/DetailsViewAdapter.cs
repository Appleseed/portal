// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DetailsViewAdapter.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The details view adapter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSSFriendly
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// The details view adapter.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class DetailsViewAdapter : CompositeDataBoundControlAdapter
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailsViewAdapter"/> class. 
        ///   Initializes a new instance of the <see cref="T:System.Web.UI.WebControls.Adapters.DataBoundControlAdapter"/> class.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public DetailsViewAdapter()
        {
            this._classMain = "AspNet-DetailsView";
            this._classHeader = "AspNet-DetailsView-Header";
            this._classData = "AspNet-DetailsView-Data";
            this._classFooter = "AspNet-DetailsView-Footer";
            this._classPagination = "AspNet-DetailsView-Pagination";
            this._classOtherPage = "AspNet-DetailsView-OtherPage";
            this._classActivePage = "AspNet-DetailsView-ActivePage";
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
                return this.ControlAsDetailsView.AllowPaging;
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
                return this.ControlAsDetailsView.DataItemCount;
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
                return this.ControlAsDetailsView.DataItemIndex;
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
                return this.ControlAsDetailsView.FooterRow;
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
                return this.ControlAsDetailsView.FooterTemplate;
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
                return this.ControlAsDetailsView.FooterText;
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
                return this.ControlAsDetailsView.HeaderRow;
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
                return this.ControlAsDetailsView.HeaderTemplate;
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
                return this.ControlAsDetailsView.HeaderText;
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
                return this.ControlAsDetailsView.PagerSettings;
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
            if (this.IsDetailsView && (this.ControlAsDetailsView.Rows.Count > 0))
            {
                writer.WriteLine();
                writer.WriteBeginTag("div");
                writer.WriteAttribute("class", this._classData);
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Indent++;

                writer.WriteLine();
                writer.WriteBeginTag("ul");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Indent++;

                var useFields = (!this.ControlAsDetailsView.AutoGenerateRows) &&
                                (this.ControlAsDetailsView.Fields.Count == this.ControlAsDetailsView.Rows.Count);
                var countRenderedRows = 0;
                for (var iRow = 0; iRow < this.ControlAsDetailsView.Rows.Count; iRow++)
                {
                    if (useFields && !this.ControlAsDetailsView.Fields[iRow].Visible)
                    {
                        continue;
                    }

                    var row = this.ControlAsDetailsView.Rows[iRow];
                    if ((!this.ControlAsDetailsView.AutoGenerateRows) &&
                        ((row.RowState & DataControlRowState.Insert) == DataControlRowState.Insert) &&
                        (!this.ControlAsDetailsView.Fields[row.RowIndex].InsertVisible))
                    {
                        continue;
                    }

                    writer.WriteLine();
                    writer.WriteBeginTag("li");
                    var theClass = ((countRenderedRows % 2) == 1) ? "AspNet-DetailsView-Alternate" : string.Empty;
                    if (useFields &&
                        (!String.IsNullOrEmpty(this.ControlAsDetailsView.Fields[iRow].ItemStyle.CssClass)))
                    {
                        if (!String.IsNullOrEmpty(theClass))
                        {
                            theClass += " ";
                        }

                        theClass += this.ControlAsDetailsView.Fields[iRow].ItemStyle.CssClass;
                    }

                    if (!String.IsNullOrEmpty(theClass))
                    {
                        writer.WriteAttribute("class", theClass);
                    }

                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Indent++;
                    writer.WriteLine();

                    for (var iCell = 0; iCell < row.Cells.Count; iCell++)
                    {
                        var cell = row.Cells[iCell];
                        writer.WriteBeginTag("span");
                        switch (iCell)
                        {
                            case 0:
                                writer.WriteAttribute("class", "AspNet-DetailsView-Name");
                                break;
                            case 1:
                                writer.WriteAttribute("class", "AspNet-DetailsView-Value");
                                break;
                            default:
                                writer.WriteAttribute("class", "AspNet-DetailsView-Misc");
                                break;
                        }

                        writer.Write(HtmlTextWriter.TagRightChar);
                        if (!String.IsNullOrEmpty(cell.Text))
                        {
                            writer.Write(cell.Text);
                        }

                        foreach (Control cellChildControl in cell.Controls)
                        {
                            cellChildControl.RenderControl(writer);
                        }

                        writer.WriteEndTag("span");
                    }

                    writer.Indent--;
                    writer.WriteLine();
                    writer.WriteEndTag("li");
                    countRenderedRows++;
                }

                writer.Indent--;
                writer.WriteLine();
                writer.WriteEndTag("ul");

                writer.Indent--;
                writer.WriteLine();
                writer.WriteEndTag("div");
            }
        }

        #endregion
    }
}