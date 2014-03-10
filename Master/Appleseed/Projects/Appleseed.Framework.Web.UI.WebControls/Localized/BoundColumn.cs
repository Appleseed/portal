// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BoundColumn.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Localized BoundColumn
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI.WebControls
{
    using System;
    using System.Web.UI;

    /// <summary>
    /// Localized BoundColumn
    /// </summary>
    [ToolboxData("<{0}:ButtonColumn TextKey='' runat=server></{0}:ButtonColumn>")]
    public class BoundColumn : System.Web.UI.WebControls.BoundColumn
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the text displayed in the header section of the column.
        /// </summary>
        /// <value></value>
        /// <returns>The text displayed in the header section of the column. The default value is <see cref = "F:System.String.Empty"></see>.</returns>
        public override string HeaderText
        {
            get
            {
                var text = this.ViewState["HeaderText"].ToString();
                return General.GetString(this.TextKey, text);
            }

            set
            {
                this.ViewState["HeaderText"] = value;
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