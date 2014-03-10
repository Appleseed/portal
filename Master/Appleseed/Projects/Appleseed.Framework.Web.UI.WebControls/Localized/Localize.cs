// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Localize.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Appleseed.Framework.Web.UI.WebControls Inherits and extends
//   <see cref="System.Web.UI.WebControls.Localize" />
//   We add properties for default text, and TextKey which results in a search for resources.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI.WebControls
{
    using System;
    using System.ComponentModel;
    using System.Web.UI;

    /// <summary>
    /// Appleseed.Framework.Web.UI.WebControls Inherits and extends
    ///     <see cref="System.Web.UI.WebControls.Localize"/>
    ///     We add properties for default text, and TextKey which results in a search for resources.
    /// </summary>
    [History("Jonathan F. Minond", "2/2/2006", "Created to extend asp.net 2.0 control Localize")]
    [DefaultProperty("TextKey")]
    [ToolboxData("<{0}:Localize TextKey='' runat=server></{0}:Localize>")]
    public class Localize : System.Web.UI.WebControls.Localize
    {
        #region Properties

        /// <summary>
        ///     Gets or sets Wrap a span with a class around the literal
        /// </summary>
        /// <value>The CSS class.</value>
        public string CssClass
        {
            get
            {
                var txt = this.ViewState["CssClass"] as string;
                return txt ?? String.Empty;
            }

            set
            {
                this.ViewState["CssClass"] = value;
            }
        }

        /// <summary>
        ///     Gets or sets a defeault text value
        /// </summary>
        /// <value></value>
        /// <returns>The caption displayed in the <see cref = "T:System.Web.UI.WebControls.Literal"></see> control.</returns>
        [ToolboxItem("text")]
        public new string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                base.Text = value;
            }
        }

        /// <summary>
        ///     Gets or sets the resource key
        /// </summary>
        /// <value>The text key.</value>
        [ToolboxItem("textkey")]
        public string TextKey
        {
            get
            {
                var txt = this.ViewState["TextKey"] as string;
                return txt ?? String.Empty;
            }

            set
            {
                this.ViewState["TextKey"] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Before rendering, set the keys for the text
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"></see> object that contains the event data.
        /// </param>
        protected override void OnPreRender(EventArgs e)
        {
            var str = string.Empty;

            if (this.TextKey.Length != 0)
            {
                str = General.GetString(this.TextKey, this.Text);
            }

            if (this.CssClass.Length > 0)
            {
                str = string.Format("<span class={0}>{1}</span>", this.CssClass, str);
            }

            if (str.Length > 0)
            {
                base.Text = str;
            }

            base.OnPreRender(e);
        }

        #endregion
    }
}