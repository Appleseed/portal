// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MenuAdapter.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The menu adapter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSSFriendly
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Configuration;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// The menu adapter.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class MenuAdapter : System.Web.UI.WebControls.Adapters.MenuAdapter
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

        #endregion

        #region Methods

        /// <summary>
        /// Registers the associated <see cref="T:System.Web.UI.WebControls.Menu"/> control as one that requires control state.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> data associated with this event.
        /// </param>
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
        /// Adds tag attributes and writes the markup for the opening tag of the control to the output stream emitted to the browser or device.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="T:System.Web.UI.HtmlTextWriter"/> instance containing methods to build and render the device-specific output.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (this.Extender.AdapterEnabled)
            {
                this.Extender.RenderBeginTag(writer, "AspNet-Menu-" + this.Control.Orientation);
            }
            else
            {
                base.RenderBeginTag(writer);
            }
        }

        /// <summary>
        /// Writes the associated menu items in the associated <see cref="T:System.Web.UI.WebControls.Menu"/> control to the output stream as a series of hyperlinks.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="T:System.Web.UI.HtmlTextWriter"/> containing methods to build and render the device-specific output.
        /// </param>
        /// <exception cref="T:System.InvalidOperationException">
        /// The depth of the current item is more than allowed.-or-The current item is disabled.
        /// </exception>
        /// <remarks>
        /// </remarks>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (this.Extender.AdapterEnabled)
            {
                writer.Indent++;
                this.BuildItems(this.Control.Items, true, writer);
                writer.Indent--;
                writer.WriteLine();
            }
            else
            {
                base.RenderContents(writer);
            }
        }

        /// <summary>
        /// Creates final markup and writes the markup for the closing tag of the control to the output stream emitted to the browser or device.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="T:System.Web.UI.HtmlTextWriter"/> instance containing methods to build and render the device-specific output.
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
        /// Builds the item.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void BuildItem(MenuItem item, HtmlTextWriter writer)
        {
            var menu = this.Control;
            if ((menu != null) && (item != null) && (writer != null))
            {
                writer.WriteLine();
                writer.WriteBeginTag("li");

                var theClass = (item.ChildItems.Count > 0) ? "AspNet-Menu-WithChildren" : "AspNet-Menu-Leaf";
                var selectedStatusClass = this.GetSelectStatusClass(item);
                if (!String.IsNullOrEmpty(selectedStatusClass))
                {
                    theClass += " " + selectedStatusClass;
                }

                writer.WriteAttribute("class", theClass);

                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Indent++;
                writer.WriteLine();

                if (((item.Depth < menu.StaticDisplayLevels) && (menu.StaticItemTemplate != null)) ||
                    ((item.Depth >= menu.StaticDisplayLevels) && (menu.DynamicItemTemplate != null)))
                {
                    writer.WriteBeginTag("div");
                    writer.WriteAttribute("class", this.GetItemClass(menu, item));
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Indent++;
                    writer.WriteLine();

                    var container = new MenuItemTemplateContainer(menu.Items.IndexOf(item), item);
                    if ((item.Depth < menu.StaticDisplayLevels) && (menu.StaticItemTemplate != null))
                    {
                        menu.StaticItemTemplate.InstantiateIn(container);
                    }
                    else
                    {
                        menu.DynamicItemTemplate.InstantiateIn(container);
                    }

                    container.DataBind();
                    container.RenderControl(writer);

                    writer.Indent--;
                    writer.WriteLine();
                    writer.WriteEndTag("div");
                }
                else
                {
                    if (this.IsLink(item))
                    {
                        writer.WriteBeginTag("a");
                        if (!String.IsNullOrEmpty(item.NavigateUrl))
                        {
                            writer.WriteAttribute(
                                "href", this.Page.Server.HtmlEncode(menu.ResolveClientUrl(item.NavigateUrl)));
                        }
                        else
                        {
                            writer.WriteAttribute(
                                "href", 
                                this.Page.ClientScript.GetPostBackClientHyperlink(
                                    menu, "b" + item.ValuePath.Replace(menu.PathSeparator.ToString(), "\\"), true));
                        }

                        writer.WriteAttribute("class", this.GetItemClass(menu, item));
                        WebControlAdapterExtender.WriteTargetAttribute(writer, item.Target);

                        if (!String.IsNullOrEmpty(item.ToolTip))
                        {
                            writer.WriteAttribute("title", item.ToolTip);
                        }
                        else if (!String.IsNullOrEmpty(menu.ToolTip))
                        {
                            writer.WriteAttribute("title", menu.ToolTip);
                        }

                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Indent++;
                        writer.WriteLine();
                    }
                    else
                    {
                        writer.WriteBeginTag("span");
                        writer.WriteAttribute("class", this.GetItemClass(menu, item));
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Indent++;
                        writer.WriteLine();
                    }

                    if (!String.IsNullOrEmpty(item.ImageUrl))
                    {
                        writer.WriteBeginTag("img");
                        writer.WriteAttribute("src", menu.ResolveClientUrl(item.ImageUrl));
                        writer.WriteAttribute(
                            "alt",
                            !String.IsNullOrEmpty(item.ToolTip)
                                ? item.ToolTip
                                : (!String.IsNullOrEmpty(menu.ToolTip) ? menu.ToolTip : item.Text));
                        writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                    }

                    writer.Write(item.Text);

                    if (this.IsLink(item))
                    {
                        writer.Indent--;
                        writer.WriteEndTag("a");
                    }
                    else
                    {
                        writer.Indent--;
                        writer.WriteEndTag("span");
                    }
                }

                if (item.ChildItems.Count > 0)
                {
                    this.BuildItems(item.ChildItems, false, writer);
                }

                writer.Indent--;
                writer.WriteLine();
                writer.WriteEndTag("li");
            }
        }

        /// <summary>
        /// Builds the items.
        /// </summary>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <param name="isRoot">
        /// if set to <c>true</c> [is root].
        /// </param>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void BuildItems(MenuItemCollection items, bool isRoot, HtmlTextWriter writer)
        {
            if (items.Count <= 0)
            {
                return;
            }

            writer.WriteLine();

            writer.WriteBeginTag("ul");
            if (isRoot)
            {
                writer.WriteAttribute("class", "AspNet-Menu");
            }

            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Indent++;

            foreach (MenuItem item in items)
            {
                this.BuildItem(item, writer);
            }

            writer.Indent--;
            writer.WriteLine();
            writer.WriteEndTag("ul");
        }

        /// <summary>
        /// Gets the item class.
        /// </summary>
        /// <param name="menu">
        /// The menu.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The get item class.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private string GetItemClass(Menu menu, MenuItem item)
        {
            var value = "AspNet-Menu-NonLink";
            if (item != null)
            {
                if (((item.Depth < menu.StaticDisplayLevels) && (menu.StaticItemTemplate != null)) ||
                    ((item.Depth >= menu.StaticDisplayLevels) && (menu.DynamicItemTemplate != null)))
                {
                    value = "AspNet-Menu-Template";
                }
                else if (this.IsLink(item))
                {
                    value = "AspNet-Menu-Link";
                }

                var selectedStatusClass = this.GetSelectStatusClass(item);
                if (!String.IsNullOrEmpty(selectedStatusClass))
                {
                    value += " " + selectedStatusClass;
                }
            }

            return value;
        }

        /// <summary>
        /// Gets the select status class.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The get select status class.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private string GetSelectStatusClass(MenuItem item)
        {
            var value = string.Empty;
            if (item.Selected)
            {
                value += " AspNet-Menu-Selected";
            }
            else if (IsChildItemSelected(item))
            {
                value += " AspNet-Menu-ChildSelected";
            }
            else if (this.IsParentItemSelected(item))
            {
                value += " AspNet-Menu-ParentSelected";
            }

            return value;
        }

        /// <summary>
        /// Determines whether [is child item selected] [the specified item].
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// <c>true</c> if [is child item selected] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private bool IsChildItemSelected(MenuItem item)
        {
            var bRet = false;

            if (item != null)
            {
                bRet = IsChildItemSelected(item.ChildItems);
            }

            return bRet;
        }

        /// <summary>
        /// Determines whether [is child item selected] [the specified items].
        /// </summary>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <returns>
        /// <c>true</c> if [is child item selected] [the specified items]; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private bool IsChildItemSelected(MenuItemCollection items)
        {
            var bRet = false;

            if (items != null)
            {
                bRet = items.Cast<MenuItem>().Any(item => item.Selected || this.IsChildItemSelected(item.ChildItems));
            }

            return bRet;
        }

        /// <summary>
        /// Determines whether the specified item is link.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified item is link; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private bool IsLink(MenuItem item)
        {
            return (item != null) && item.Enabled && ((!String.IsNullOrEmpty(item.NavigateUrl)) || item.Selectable);
        }

        /// <summary>
        /// Determines whether [is parent item selected] [the specified item].
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// <c>true</c> if [is parent item selected] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        private bool IsParentItemSelected(MenuItem item)
        {
            var bRet = false;

            if ((item != null) && (item.Parent != null))
            {
                bRet = item.Parent.Selected || this.IsParentItemSelected(item.Parent);
            }

            return bRet;
        }

        /// <summary>
        /// Registers the scripts.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private void RegisterScripts()
        {
            this.Extender.RegisterScripts();
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered(this.GetType().ToString()))
            {
                var folderPath = WebConfigurationManager.AppSettings.Get("CSSFriendly-JavaScript-Path");
                if (String.IsNullOrEmpty(folderPath))
                {
                    folderPath = "~/JavaScript";
                }

                var filePath = folderPath.EndsWith("/") ? folderPath + "MenuAdapter.js" : folderPath + "/MenuAdapter.js";
                this.Page.ClientScript.RegisterClientScriptInclude(
                    this.GetType(), this.GetType().ToString(), this.Page.ResolveUrl(filePath));
            }
        }

        #endregion
    }
}