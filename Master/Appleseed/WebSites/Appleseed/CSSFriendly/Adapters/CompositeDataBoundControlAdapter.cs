// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositeDataBoundControlAdapter.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The composite data bound control adapter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSSFriendly
{
    using System;
    using System.Diagnostics;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.Adapters;

    /// <summary>
    /// The composite data bound control adapter.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public abstract class CompositeDataBoundControlAdapter : DataBoundControlAdapter
    {
        #region Constants and Fields

        /// <summary>
        /// The _class active page.
        /// </summary>
        protected string _classActivePage = string.Empty;

        /// <summary>
        /// The _class data.
        /// </summary>
        protected string _classData = string.Empty;

        /// <summary>
        /// The _class footer.
        /// </summary>
        protected string _classFooter = string.Empty;

        /// <summary>
        /// The _class header.
        /// </summary>
        protected string _classHeader = string.Empty;

        /// <summary>
        /// The _class main.
        /// </summary>
        protected string _classMain = string.Empty;

        /// <summary>
        /// The _class other page.
        /// </summary>
        protected string _classOtherPage = string.Empty;

        /// <summary>
        /// The _class pagination.
        /// </summary>
        protected string _classPagination = string.Empty;

        /// <summary>
        /// The extender.
        /// </summary>
        private WebControlAdapterExtender extender;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets a value indicating whether [allow paging].
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected abstract bool AllowPaging { get; }

        /// <summary>
        ///   Gets the control as details view.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected DetailsView ControlAsDetailsView
        {
            get
            {
                return this.Control as DetailsView;
            }
        }

        /// <summary>
        ///   Gets the control as form view.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected FormView ControlAsFormView
        {
            get
            {
                return this.Control as FormView;
            }
        }

        /// <summary>
        ///   Gets the data item count.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected abstract int DataItemCount { get; }

        /// <summary>
        ///   Gets the index of the data item.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected abstract int DataItemIndex { get; }

        /// <summary>
        ///   Gets the footer row.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected abstract TableRow FooterRow { get; }

        /// <summary>
        ///   Gets the footer template.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected abstract ITemplate FooterTemplate { get; }

        /// <summary>
        ///   Gets the footer text.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected abstract string FooterText { get; }

        /// <summary>
        ///   Gets the header row.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected abstract TableRow HeaderRow { get; }

        /// <summary>
        ///   Gets the header template.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected abstract ITemplate HeaderTemplate { get; }

        /// <summary>
        ///   Gets the header text.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected abstract string HeaderText { get; }

        /// <summary>
        ///   Gets a value indicating whether this instance is details view.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected bool IsDetailsView
        {
            get
            {
                return this.ControlAsDetailsView != null;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance is form view.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected bool IsFormView
        {
            get
            {
                return this.ControlAsFormView != null;
            }
        }

        /// <summary>
        ///   Gets the pager settings.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected abstract PagerSettings PagerSettings { get; }

        /// <summary>
        ///   Gets the view.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected CompositeDataBoundControl View
        {
            get
            {
                return this.Control as CompositeDataBoundControl;
            }
        }

        /// <summary>
        ///   Gets the extender.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private WebControlAdapterExtender Extender
        {
            get
            {
                if (((this.extender == null) && (this.Control != null)) ||
                    ((this.extender != null) && (this.Control != this.extender.AdaptedControl)))
                {
                    this.extender = new WebControlAdapterExtender(this.Control);
                }

                Debug.Assert(this.extender != null, "CSS Friendly adapters internal error", "Null extender instance");
                return this.extender;
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
        protected virtual void BuildItem(HtmlTextWriter writer)
        {
        }

        /// <summary>
        /// Builds the paging.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected virtual void BuildPaging(HtmlTextWriter writer)
        {
            if (this.AllowPaging && (this.DataItemCount > 0))
            {
                writer.WriteLine();
                writer.WriteBeginTag("div");
                writer.WriteAttribute("class", this._classPagination);
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Indent++;

                var iStart = 0;
                var iEnd = this.DataItemCount;
                var nPages = iEnd - iStart + 1;
                var bExceededPageButtonCount = nPages > this.PagerSettings.PageButtonCount;

                if (bExceededPageButtonCount)
                {
                    iStart = (this.DataItemIndex / this.PagerSettings.PageButtonCount) *
                             this.PagerSettings.PageButtonCount;
                    iEnd = Math.Min(iStart + this.PagerSettings.PageButtonCount, this.DataItemCount);
                }

                writer.WriteLine();

                if (bExceededPageButtonCount && (iStart > 0))
                {
                    writer.WriteBeginTag("a");
                    writer.WriteAttribute("class", this._classOtherPage);
                    writer.WriteAttribute(
                        "href", this.Page.ClientScript.GetPostBackClientHyperlink(this.Control, "Page$" + iStart, true));
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Write("...");
                    writer.WriteEndTag("a");
                }

                for (var iDataItem = iStart; iDataItem < iEnd; iDataItem++)
                {
                    var strPage = (iDataItem + 1).ToString();
                    if (this.DataItemIndex == iDataItem)
                    {
                        writer.WriteBeginTag("span");
                        writer.WriteAttribute("class", this._classActivePage);
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Write(strPage);
                        writer.WriteEndTag("span");
                    }
                    else
                    {
                        writer.WriteBeginTag("a");
                        writer.WriteAttribute("class", this._classOtherPage);
                        writer.WriteAttribute(
                            "href", 
                            this.Page.ClientScript.GetPostBackClientHyperlink(this.Control, "Page$" + strPage, true));
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Write(strPage);
                        writer.WriteEndTag("a");
                    }
                }

                if (bExceededPageButtonCount && (iEnd < this.DataItemCount))
                {
                    writer.WriteBeginTag("a");
                    writer.WriteAttribute("class", this._classOtherPage);
                    writer.WriteAttribute(
                        "href", 
                        this.Page.ClientScript.GetPostBackClientHyperlink(this.Control, "Page$" + (iEnd + 1), true));
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Write("...");
                    writer.WriteEndTag("a");
                }

                writer.Indent--;
                writer.WriteLine();
                writer.WriteEndTag("div");
            }
        }

        /// <summary>
        /// Builds the row.
        /// </summary>
        /// <param name="row">
        /// The row.
        /// </param>
        /// <param name="cssClass">
        /// The CSS class.
        /// </param>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected virtual void BuildRow(TableRow row, string cssClass, HtmlTextWriter writer)
        {
            if ((row == null) || !row.Visible)
            {
                return;
            }

            // If there isn't any content, don't render anything.
            var bHasContent = false;
            TableCell cell = null;
            for (var iCell = 0; iCell < row.Cells.Count; iCell++)
            {
                cell = row.Cells[iCell];
                if (String.IsNullOrEmpty(cell.Text) && (cell.Controls.Count <= 0))
                {
                    continue;
                }

                bHasContent = true;
                break;
            }

            if (bHasContent)
            {
                writer.WriteLine();
                writer.WriteBeginTag("div");
                writer.WriteAttribute("class", cssClass);
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Indent++;
                writer.WriteLine();

                for (var iCell = 0; iCell < row.Cells.Count; iCell++)
                {
                    cell = row.Cells[iCell];
                    if (!String.IsNullOrEmpty(cell.Text))
                    {
                        writer.Write(cell.Text);
                    }

                    foreach (Control cellChildControl in cell.Controls)
                    {
                        cellChildControl.RenderControl(writer);
                    }
                }

                writer.Indent--;
                writer.WriteLine();
                writer.WriteEndTag("div");
            }
        }

        /// <summary>
        /// Overrides the <see cref="M:System.Web.UI.Control.OnInit(System.EventArgs)"/> method for the associated control.
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"/> that contains the event data.
        /// </param>
        /// /
        /// METHODS
        /// <remarks>
        /// </remarks>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (this.Extender.AdapterEnabled)
            {
                this.RegisterScripts();
            }
        }

        /// <summary>
        /// Registers the scripts.
        /// </summary>
        /// <remarks>
        /// </remarks>
        protected virtual void RegisterScripts()
        {
        }

        /// <summary>
        /// Creates the beginning tag for the Web control in the markup that is transmitted to the target browser.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="T:System.Web.UI.HtmlTextWriter"/> containing methods to render the target-specific output.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (this.Extender.AdapterEnabled)
            {
                this.Extender.RenderBeginTag(writer, this._classMain);
            }
            else
            {
                base.RenderBeginTag(writer);
            }
        }

        /// <summary>
        /// Generates the target-specific inner markup for the Web control to which the control adapter is attached.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="T:System.Web.UI.HtmlTextWriter"/> containing methods to render the target-specific output.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (this.Extender.AdapterEnabled)
            {
                if (this.View != null)
                {
                    writer.Indent++;

                    this.BuildRow(this.HeaderRow, this._classHeader, writer);
                    this.BuildItem(writer);
                    this.BuildRow(this.FooterRow, this._classFooter, writer);
                    this.BuildPaging(writer);

                    writer.Indent--;
                    writer.WriteLine();
                }
            }
            else
            {
                base.RenderContents(writer);
            }
        }

        /// <summary>
        /// Creates the ending tag for the Web control in the markup that is transmitted to the target browser.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="T:System.Web.UI.HtmlTextWriter"/> containing methods to render the target-specific output.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void RenderEndTag(HtmlTextWriter writer)
        {
            if (this.Extender.AdapterEnabled)
            {
                this.Extender.RenderEndTag(writer);
            }
            else
            {
                base.RenderEndTag(writer);
            }
        }

        #endregion
    }
}