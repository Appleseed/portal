// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Button.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Appleseed.Framework.Web.UI.WebControls Inherits and extends
//   <see cref="System.Web.UI.WebControls.Button" />
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
    ///     <see cref="System.Web.UI.WebControls.Button"/>
    ///     We add properties for default text, and TextKey which results in a search for resources.
    /// </summary>
    [History("Jonathan F. Minond", "2/2/2006", "Created to extend asp.net 2.0 control Localize")]
    [DefaultProperty("TextKey")]
    [ToolboxData("<{0}:Button TextKey='' runat=server></{0}:Button>")]
    public class Button : System.Web.UI.WebControls.Button
    {
        #region Properties

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
            if (this.TextKey.Length != 0)
            {
                this.Text = General.GetString(this.TextKey, this.Text);
            }

            base.OnPreRender(e);
        }

        #endregion
    }
}