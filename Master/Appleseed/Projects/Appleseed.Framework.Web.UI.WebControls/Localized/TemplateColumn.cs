// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemplateColumn.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Localized TemplateColumn
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI.WebControls
{
    using System;
    using System.Web.UI;

    /// <summary>
    /// Localized TemplateColumn
    /// </summary>
    [ToolboxData("<{0}:TemplateColumn TextKey='' runat=server></{0}:TemplateColumn>")]
    public class TemplateColumn : System.Web.UI.WebControls.TemplateColumn
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
                var o = this.ViewState["HeaderText"];
                return o != null ? General.GetString(this.TextKey, o.ToString()) : General.GetString(this.TextKey);
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