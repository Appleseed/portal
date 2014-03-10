// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataListAdapter.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The data list adapter.
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
    /// The data list adapter.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class DataListAdapter : WebControlAdapter
    {
        #region Constants and Fields

        /// <summary>
        /// The extender.
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

        /// <summary>
        ///   Gets the repeat columns.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private int RepeatColumns
        {
            get
            {
                var dataList = this.Control as DataList;
                var nRet = 1;
                if (dataList != null)
                {
                    if (dataList.RepeatColumns == 0)
                    {
                        if (dataList.RepeatDirection == RepeatDirection.Horizontal)
                        {
                            nRet = dataList.Items.Count;
                        }
                    }
                    else
                    {
                        nRet = dataList.RepeatColumns;
                    }
                }

                return nRet;
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
                this.Extender.RenderBeginTag(writer, "AspNet-DataList");
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
                var dataList = this.Control as DataList;
                if (dataList != null)
                {
                    writer.Indent++;
                    writer.WriteLine();
                    writer.WriteBeginTag("table");
                    writer.WriteAttribute("cellpadding", "0");
                    writer.WriteAttribute("cellspacing", "0");
                    writer.WriteAttribute("summary", this.Control.ToolTip);
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Indent++;

                    if (dataList.HeaderTemplate != null)
                    {
                        var container = new PlaceHolder();
                        dataList.HeaderTemplate.InstantiateIn(container);
                        container.DataBind();

                        if ((container.Controls.Count == 1) &&
                            typeof(LiteralControl).IsInstanceOfType(container.Controls[0]))
                        {
                            writer.WriteLine();
                            writer.WriteBeginTag("caption");
                            writer.Write(HtmlTextWriter.TagRightChar);

                            var literalControl = container.Controls[0] as LiteralControl;
                            if (literalControl != null)
                            {
                                writer.Write(literalControl.Text.Trim());
                            }

                            writer.WriteEndTag("caption");
                        }
                        else
                        {
                            writer.WriteLine();
                            writer.WriteBeginTag("thead");
                            writer.Write(HtmlTextWriter.TagRightChar);
                            writer.Indent++;

                            writer.WriteLine();
                            writer.WriteBeginTag("tr");
                            writer.Write(HtmlTextWriter.TagRightChar);
                            writer.Indent++;

                            writer.WriteLine();
                            writer.WriteBeginTag("th");
                            writer.WriteAttribute("colspan", this.RepeatColumns.ToString());
                            writer.Write(HtmlTextWriter.TagRightChar);
                            writer.Indent++;

                            writer.WriteLine();
                            container.RenderControl(writer);

                            writer.Indent--;
                            writer.WriteLine();
                            writer.WriteEndTag("th");

                            writer.Indent--;
                            writer.WriteLine();
                            writer.WriteEndTag("tr");

                            writer.Indent--;
                            writer.WriteLine();
                            writer.WriteEndTag("thead");
                        }
                    }

                    if (dataList.FooterTemplate != null)
                    {
                        writer.WriteLine();
                        writer.WriteBeginTag("tfoot");
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Indent++;

                        writer.WriteLine();
                        writer.WriteBeginTag("tr");
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Indent++;

                        writer.WriteLine();
                        writer.WriteBeginTag("td");
                        writer.WriteAttribute("colspan", this.RepeatColumns.ToString());
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Indent++;

                        var container = new PlaceHolder();
                        dataList.FooterTemplate.InstantiateIn(container);
                        container.DataBind();
                        container.RenderControl(writer);

                        writer.Indent--;
                        writer.WriteLine();
                        writer.WriteEndTag("td");

                        writer.Indent--;
                        writer.WriteLine();
                        writer.WriteEndTag("tr");

                        writer.Indent--;
                        writer.WriteLine();
                        writer.WriteEndTag("tfoot");
                    }

                    if (dataList.ItemTemplate != null)
                    {
                        writer.WriteLine();
                        writer.WriteBeginTag("tbody");
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Indent++;

                        var nItemsInColumn = (int)Math.Ceiling(dataList.Items.Count / ((Double)this.RepeatColumns));
                        for (var iItem = 0; iItem < dataList.Items.Count; iItem++)
                        {
                            var nRow = iItem / this.RepeatColumns;
                            var nCol = iItem % this.RepeatColumns;
                            var nDesiredIndex = iItem;
                            if (dataList.RepeatDirection == RepeatDirection.Vertical)
                            {
                                nDesiredIndex = (nCol * nItemsInColumn) + nRow;
                            }

                            if ((iItem % this.RepeatColumns) == 0)
                            {
                                writer.WriteLine();
                                writer.WriteBeginTag("tr");
                                writer.Write(HtmlTextWriter.TagRightChar);
                                writer.Indent++;
                            }

                            writer.WriteLine();
                            writer.WriteBeginTag("td");
                            writer.Write(HtmlTextWriter.TagRightChar);
                            writer.Indent++;

                            foreach (Control itemCtrl in dataList.Items[iItem].Controls)
                            {
                                itemCtrl.RenderControl(writer);
                            }

                            writer.Indent--;
                            writer.WriteLine();
                            writer.WriteEndTag("td");

                            if (((iItem + 1) % this.RepeatColumns) == 0)
                            {
                                writer.Indent--;
                                writer.WriteLine();
                                writer.WriteEndTag("tr");
                            }
                        }

                        if ((dataList.Items.Count % this.RepeatColumns) != 0)
                        {
                            writer.Indent--;
                            writer.WriteLine();
                            writer.WriteEndTag("tr");
                        }

                        writer.Indent--;
                        writer.WriteLine();
                        writer.WriteEndTag("tbody");
                    }

                    writer.Indent--;
                    writer.WriteLine();
                    writer.WriteEndTag("table");

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
        /// Registers the scripts.
        /// </summary>
        /// /
        /// PRIVATE
        /// <remarks>
        /// </remarks>
        private static void RegisterScripts()
        {
        }

        #endregion
    }
}