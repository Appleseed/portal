// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginStatusAdapter.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The login status adapter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CSSFriendly
{
    using System;
    using System.Diagnostics;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.Adapters;

    /// <summary>
    /// The login status adapter.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class LoginStatusAdapter : WebControlAdapter
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
        /// Overrides the <see cref="M:System.Web.UI.Control.OnInit(System.EventArgs)"/> method for the associated control.
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"/> that contains the event data.
        /// </param>
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
                // The LoginStatus is very simple INPUT or A tag so we don't wrap it with an being/end tag (e.g., no DIV wraps it).
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
                var loginStatus = this.Control as LoginStatus;
                if (loginStatus != null)
                {
                    var className = (!String.IsNullOrEmpty(loginStatus.CssClass))
                                        ? ("AspNet-LoginStatus " + loginStatus.CssClass)
                                        : "AspNet-LoginStatus";

                    if (Membership.GetUser() == null)
                    {
                        if (!String.IsNullOrEmpty(loginStatus.LoginImageUrl))
                        {
                            var ctl = loginStatus.FindControl("ctl03");
                            if (ctl != null)
                            {
                                writer.WriteBeginTag("input");
                                writer.WriteAttribute("id", loginStatus.ClientID);
                                writer.WriteAttribute("type", "image");
                                writer.WriteAttribute("name", ctl.UniqueID);
                                writer.WriteAttribute("title", loginStatus.ToolTip);
                                writer.WriteAttribute("class", className);
                                writer.WriteAttribute("src", loginStatus.ResolveClientUrl(loginStatus.LoginImageUrl));
                                writer.WriteAttribute("alt", loginStatus.LoginText);
                                writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                                this.Page.ClientScript.RegisterForEventValidation(ctl.UniqueID);
                            }
                        }
                        else
                        {
                            var ctl = loginStatus.FindControl("ctl02");
                            if (ctl != null)
                            {
                                writer.WriteBeginTag("a");
                                writer.WriteAttribute("id", loginStatus.ClientID);
                                writer.WriteAttribute("title", loginStatus.ToolTip);
                                writer.WriteAttribute("class", className);
                                writer.WriteAttribute(
                                    "href", 
                                    this.Page.ClientScript.GetPostBackClientHyperlink(
                                        loginStatus.FindControl("ctl02"), string.Empty));
                                writer.Write(HtmlTextWriter.TagRightChar);
                                writer.Write(loginStatus.LoginText);
                                writer.WriteEndTag("a");
                                this.Page.ClientScript.RegisterForEventValidation(ctl.UniqueID);
                            }
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(loginStatus.LogoutImageUrl))
                        {
                            var ctl = loginStatus.FindControl("ctl01");
                            if (ctl != null)
                            {
                                writer.WriteBeginTag("input");
                                writer.WriteAttribute("id", loginStatus.ClientID);
                                writer.WriteAttribute("type", "image");
                                writer.WriteAttribute("name", ctl.UniqueID);
                                writer.WriteAttribute("title", loginStatus.ToolTip);
                                writer.WriteAttribute("class", className);
                                writer.WriteAttribute("src", loginStatus.ResolveClientUrl(loginStatus.LogoutImageUrl));
                                writer.WriteAttribute("alt", loginStatus.LogoutText);
                                writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                                this.Page.ClientScript.RegisterForEventValidation(ctl.UniqueID);
                            }
                        }
                        else
                        {
                            var ctl = loginStatus.FindControl("ctl00");
                            if (ctl != null)
                            {
                                writer.WriteBeginTag("a");
                                writer.WriteAttribute("id", loginStatus.ClientID);
                                writer.WriteAttribute("title", loginStatus.ToolTip);
                                writer.WriteAttribute("class", className);
                                writer.WriteAttribute(
                                    "href", 
                                    this.Page.ClientScript.GetPostBackClientHyperlink(
                                        loginStatus.FindControl("ctl00"), string.Empty));
                                writer.Write(HtmlTextWriter.TagRightChar);
                                writer.Write(loginStatus.LogoutText);
                                writer.WriteEndTag("a");
                                this.Page.ClientScript.RegisterForEventValidation(ctl.UniqueID);
                            }
                        }
                    }
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
                // The LoginStatus is very simple INPUT or A tag so we don't wrap it with an being/end tag (e.g., no DIV wraps it).
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