// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkButton.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Appleseed.Framework.Web.UI.WebControls Inherits and extends
//   <see cref="System.Web.UI.WebControls.LinkButton" />
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
    ///     <see cref="System.Web.UI.WebControls.LinkButton"/>
    ///     We add properties for default text, and TextKey which results in a search for resources.
    /// </summary>
    [History("Jonathan F. Minond", "2/2/2006", "Created to extend asp.net 2.0 control Localize")]
    [DefaultProperty("TextKey")]
    [ToolboxData("<{0}:LinkButton TextKey='' runat=server></{0}:LinkButton>")]
    public class LinkButton : System.Web.UI.WebControls.LinkButton
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkButton"/> class.
        /// </summary>
        public LinkButton()
        {
            this.Text = string.Empty;
            this.TextKey = string.Empty;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets a defeault text value
        /// </summary>
        /// <value></value>
        /// <returns>The text caption displayed on the <see cref = "T:System.Web.UI.WebControls.LinkButton"></see> control. The default value is an empty string ("").</returns>
        [ToolboxItem("text")]
        public new string Text { get; set; }

        /// <summary>
        ///     Gets or sets the resource key
        /// </summary>
        /// <value>The text key.</value>
        [ToolboxItem("textkey")]
        public string TextKey { get; set; }

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
            if (this.TextKey.Length != 0)
            {
                base.Text = General.GetString(this.TextKey, this.Text);
            }
            else if (this.Text.Length > 0)
            {
                base.Text = this.Text;
            }

            base.OnPreRender(e);
        }

        #endregion
    }
}