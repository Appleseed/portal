// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ButtonColumn.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Localized ButtonColumn
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI.WebControls
{
    using System;
    using System.Web.UI;

    /// <summary>
    /// Localized ButtonColumn
    /// </summary>
    [ToolboxData("<{0}:ButtonColumn TextKey='' runat=server></{0}:ButtonColumn>")]
    public class ButtonColumn : System.Web.UI.WebControls.ButtonColumn
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the caption that is displayed in the buttons of the <see cref = "T:System.Web.UI.WebControls.ButtonColumn"></see> object.
        /// </summary>
        /// <value></value>
        /// <returns>The caption displayed in the buttons of the <see cref = "T:System.Web.UI.WebControls.ButtonColumn"></see>. The default is an empty string ("").</returns>
        public override string Text
        {
            get
            {
                var text = this.ViewState["Text"].ToString();
                return General.GetString(this.TextKey, text);
            }

            set
            {
                this.ViewState["Text"] = value;
                this.OnColumnChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the text key.
        /// </summary>
        /// <value>The text key.</value>
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
    }
}