// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GridViewAdapter.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The grid view adapter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSSFriendly
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.Adapters;

    /// <summary>
    /// The grid view adapter.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class GridViewAdapter : WebControlAdapter
    {
        #region Constants and Fields

        /// <summary>
        ///   The extender.
        /// </summary>
        private WebControlAdapterExtender extender;

        #endregion

        #region Properties

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
        /// Overrides the <see cref="M:System.Web.UI.Control.OnInit(System.EventArgs)"/> method for the associated control.
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"/> that contains the event data.
        /// </param>
        /// /
        /// PROTECTED
        /// <remarks>
        /// </remarks>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (this.Extender.AdapterEnabled)
            {
                RegisterScripts();
            }
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
                this.Extender.RenderBeginTag(writer, "AspNet-GridView");
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
                var gridView = this.Control as GridView;
                if (gridView != null)
                {
                    writer.Indent++;
                    this.WritePagerSection(writer, PagerPosition.Top);

                    writer.WriteLine();
                    writer.WriteBeginTag("table");
                    writer.WriteAttribute("cellpadding", "0");
                    writer.WriteAttribute("cellspacing", "0");
                    writer.WriteAttribute("summary", this.Control.ToolTip);

                    if (!String.IsNullOrEmpty(gridView.CssClass))
                    {
                        writer.WriteAttribute("class", gridView.CssClass);
                    }

                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Indent++;

                    var rows = new ArrayList();

                    ///////////////////// HEAD /////////////////////////////
                    rows.Clear();
                    if (gridView.ShowHeader && (gridView.HeaderRow != null))
                    {
                        rows.Add(gridView.HeaderRow);
                    }

                    var gvrc = new GridViewRowCollection(rows);
                    this.WriteRows(writer, gridView, gvrc, "thead");

                    ///////////////////// FOOT /////////////////////////////
                    rows.Clear();
                    if (gridView.ShowFooter && (gridView.FooterRow != null))
                    {
                        rows.Add(gridView.FooterRow);
                    }

                    gvrc = new GridViewRowCollection(rows);
                    this.WriteRows(writer, gridView, gvrc, "tfoot");

                    ///////////////////// BODY /////////////////////////////
                    this.WriteRows(writer, gridView, gridView.Rows, "tbody");

                    ////////////////////////////////////////////////////////
                    writer.Indent--;
                    writer.WriteLine();
                    writer.WriteEndTag("table");

                    this.WritePagerSection(writer, PagerPosition.Bottom);

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

        /// <summary>
        /// Gets the row class.
        /// </summary>
        /// <param name="gridView">
        /// The grid view.
        /// </param>
        /// <param name="row">
        /// The row.
        /// </param>
        /// <returns>
        /// The get row class.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private static string GetRowClass(GridView gridView, GridViewRow row)
        {
            var className = string.Empty;

            if ((row.RowState & DataControlRowState.Alternate) == DataControlRowState.Alternate)
            {
                className += " AspNet-GridView-Alternate ";
                className += gridView.AlternatingRowStyle.CssClass;
            }
            else if (row.Equals(gridView.HeaderRow) && (!String.IsNullOrEmpty(gridView.HeaderStyle.CssClass)))
            {
                className += " " + gridView.HeaderStyle.CssClass;
            }
            else if (row.Equals(gridView.FooterRow) && (!String.IsNullOrEmpty(gridView.FooterStyle.CssClass)))
            {
                className += " " + gridView.FooterStyle.CssClass;
            }
            else if (!String.IsNullOrEmpty(gridView.RowStyle.CssClass))
            {
                className += " " + gridView.RowStyle.CssClass;
            }

            if ((row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                className += " AspNet-GridView-Edit ";
                className += gridView.EditRowStyle.CssClass;
            }

            if ((row.RowState & DataControlRowState.Insert) == DataControlRowState.Insert)
            {
                className += " AspNet-GridView-Insert ";
            }

            if ((row.RowState & DataControlRowState.Selected) == DataControlRowState.Selected)
            {
                className += " AspNet-GridView-Selected ";
                className += gridView.SelectedRowStyle.CssClass;
            }

            return className.Trim();
        }

        /// <summary>
        /// Registers the scripts.
        /// </summary>
        /// /
        /// PRIVATE
        /// <remarks>
        /// </remarks>
        private static void RegisterScripts()
        {
        }

        /// <summary>
        /// Writes the pager section.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="pos">
        /// The pos.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WritePagerSection(HtmlTextWriter writer, PagerPosition pos)
        {
            var gridView = this.Control as GridView;
            if ((gridView != null) && gridView.AllowPaging &&
                ((gridView.PagerSettings.Position == pos) ||
                 (gridView.PagerSettings.Position == PagerPosition.TopAndBottom)))
            {
                Table innerTable = null;
                if ((pos == PagerPosition.Top) && (gridView.TopPagerRow != null) &&
                    (gridView.TopPagerRow.Cells.Count == 1) && (gridView.TopPagerRow.Cells[0].Controls.Count == 1) &&
                    typeof(Table).IsAssignableFrom(gridView.TopPagerRow.Cells[0].Controls[0].GetType()))
                {
                    innerTable = gridView.TopPagerRow.Cells[0].Controls[0] as Table;
                }
                else if ((pos == PagerPosition.Bottom) && (gridView.BottomPagerRow != null) &&
                         (gridView.BottomPagerRow.Cells.Count == 1) &&
                         (gridView.BottomPagerRow.Cells[0].Controls.Count == 1) &&
                         typeof(Table).IsAssignableFrom(gridView.BottomPagerRow.Cells[0].Controls[0].GetType()))
                {
                    innerTable = gridView.BottomPagerRow.Cells[0].Controls[0] as Table;
                }

                if ((innerTable != null) && (innerTable.Rows.Count == 1))
                {
                    var className = "AspNet-GridView-Pagination AspNet-GridView-";
                    className += (pos == PagerPosition.Top) ? "Top " : "Bottom ";
                    className += gridView.PagerStyle.CssClass;

                    className = className.Trim();

                    writer.WriteLine();
                    writer.WriteBeginTag("div");
                    writer.WriteAttribute("class", className);
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Indent++;

                    var row = innerTable.Rows[0];
                    foreach (TableCell cell in row.Cells)
                    {
                        foreach (Control ctrl in cell.Controls)
                        {
                            writer.WriteLine();
                            ctrl.RenderControl(writer);
                        }
                    }

                    writer.Indent--;
                    writer.WriteLine();
                    writer.WriteEndTag("div");
                }
            }
        }

        /// <summary>
        /// Writes the rows.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="gridView">
        /// The grid view.
        /// </param>
        /// <param name="rows">
        /// The rows.
        /// </param>
        /// <param name="tableSection">
        /// The table section.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void WriteRows(
            HtmlTextWriter writer, GridView gridView, GridViewRowCollection rows, string tableSection)
        {
            if (rows.Count > 0)
            {
                writer.WriteLine();
                writer.WriteBeginTag(tableSection);
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Indent++;

                foreach (GridViewRow row in rows)
                {
                    writer.WriteLine();
                    writer.WriteBeginTag("tr");

                    var className = GetRowClass(gridView, row);
                    if (!String.IsNullOrEmpty(className))
                    {
                        writer.WriteAttribute("class", className);
                    }

                    // Uncomment the following block of code if you want to add arbitrary attributes.
                    /*
                    foreach (string key in row.Attributes.Keys)
                    {
                        writer.WriteAttribute(key, row.Attributes[key]);
                    }
                    */
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Indent++;

                    foreach (TableCell cell in row.Cells)
                    {
                        var fieldCell = cell as DataControlFieldCell;
                        if ((fieldCell != null) && (fieldCell.ContainingField != null))
                        {
                            var field = fieldCell.ContainingField;
                            if (!field.Visible)
                            {
                                cell.Visible = false;
                            }

                            if (!String.IsNullOrEmpty(field.ItemStyle.CssClass))
                            {
                                if (!String.IsNullOrEmpty(cell.CssClass))
                                {
                                    cell.CssClass += " ";
                                }

                                cell.CssClass += field.ItemStyle.CssClass;
                            }
                        }

                        writer.WriteLine();
                        cell.RenderControl(writer);
                    }

                    writer.Indent--;
                    writer.WriteLine();
                    writer.WriteEndTag("tr");
                }

                writer.Indent--;
                writer.WriteLine();
                writer.WriteEndTag(tableSection);
            }
        }

        #endregion
    }
}