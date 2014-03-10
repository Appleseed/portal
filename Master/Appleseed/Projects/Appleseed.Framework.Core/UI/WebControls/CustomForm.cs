// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomForm.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   This is a server form that is able to post to a different page than than the one it is on.
//   Simply set the Action property to whatever page you want.
//   This control is available from the great MetaBuilders site http://www.metabuilders.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI.WebControls
{
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    /// <summary>
    /// This is a server form that is able to post to a different page than than the one it is on.
    ///  Simply set the Action property to whatever page you want.
    ///  This control is available from the great MetaBuilders site http://www.metabuilders.com
    /// </summary>
    /// <example>
    /// The following is an example page which posts a server form to Google's search engine.
    ///  <code>
    /// <![CDATA[
    /// <%@ Register TagPrefix="rbc" Namespace="Appleseed.Framework.Web.UI.WebControls" Assembly="Appleseed" %>
    /// <html><body>
    /// 		<rbc:CustomForm runat="server" Method="Get" Action="http://www.google.com/search">
    /// 			<asp:TextBox id="q" runat="server"></asp:TextBox>
    /// 			<asp:Button runat="server" Text="Google Search"/>
    /// 		</rbc:CustomForm>
    /// </body></html>
    /// ]]>
    ///  </code>
    /// </example>
    public class CustomForm : HtmlForm
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the target url of the form post.
        /// </summary>
        /// <value>The action.</value>
        /// <remarks>
        ///   Leave blank to post back to the same page.
        /// </remarks>
        [Description("Gets or sets the target url of the form post.")]
        [Category("Behavior")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new virtual string Action
        {
            get
            {
                if (this.ViewState["Action"] != null)
                {
                    return (string)this.ViewState["Action"];
                }

                return this.GetBaseActionAttribute();
            }

            set
            {
                this.ViewState["Action"] = value;
            }
        }

        /// <summary>
        ///   Uses reflection to access the ClientOnSubmitEvent property of the Page class
        /// </summary>
        /// <value>The page client on submit event.</value>
        private string Page_ClientOnSubmitEvent
        {
            get
            {
                return (string)GetHiddenProperty(this.Page, typeof(Page), "ClientOnSubmitEvent");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Overridden to render custom Action attribute
        /// </summary>
        /// <param name="writer">
        /// The <see cref="T:System.Web.UI.HtmlTextWriter"></see> that receives the rendered content.
        /// </param>
        /// <exception cref="T:System.InvalidOperationException">
        /// The control ID set in the <see cref="P:System.Web.UI.HtmlControls.HtmlForm.DefaultButton"></see> property is not of the type <see cref="T:System.Web.UI.WebControls.IButtonControl"></see>.
        /// </exception>
        protected override void RenderAttributes(HtmlTextWriter writer)
        {
            // From HtmlForm, with changes to Action
            writer.WriteAttribute("name", this.Name);
            this.Attributes.Remove("name");

            writer.WriteAttribute("method", this.Method);
            this.Attributes.Remove("method");

            writer.WriteAttribute("action", this.ResolveUrl(this.Action), true);
            this.Attributes.Remove("action");

            var submitEvent = this.Page_ClientOnSubmitEvent;
            if (!string.IsNullOrEmpty(submitEvent))
            {
                if (this.Attributes["onsubmit"] != null)
                {
                    submitEvent = submitEvent + this.Attributes["onsubmit"];
                    this.Attributes.Remove("onsubmit");
                }

                writer.WriteAttribute("language", "javascript");
                writer.WriteAttribute("onsubmit", submitEvent);
            }

            writer.WriteAttribute("id", this.ClientID);

            // From HtmlContainerControl
            this.ViewState.Remove("innerhtml");

            // From HtmlControl
            this.Attributes.Render(writer);
        }

        /// <summary>
        /// Uses reflection to get the result of the private implementation of GetActionAttribute of the base class.
        /// </summary>
        /// <returns>
        /// Returns the normal action attribute of a server form.
        /// </returns>
        private string GetBaseActionAttribute()
        {
            var formType = typeof(HtmlForm);
            var actionMethod = formType.GetMethod("GetActionAttribute", BindingFlags.Instance | BindingFlags.NonPublic);
            var result = actionMethod.Invoke(this, null);
            return (string)result;
        }

        /// <summary>
        /// Uses reflection to access any property on an object, even tho the property is marked protected, internal, or private.
        /// </summary>
        /// <param name="target">
        /// The object being accessed
        /// </param>
        /// <param name="targetType">
        /// The Type to examine. Usually the Type of target arg, but sometimes a superclass of it.
        /// </param>
        /// <param name="propertyName">
        /// The name of the property to access.
        /// </param>
        /// <returns>
        /// The value of the property, or null if the property is not found.
        /// </returns>
        private static object GetHiddenProperty(object target, Type targetType, string propertyName)
        {
            var property = targetType.GetProperty(
                propertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic);
            return property != null ? property.GetValue(target, null) : null;
        }

        #endregion
    }
}