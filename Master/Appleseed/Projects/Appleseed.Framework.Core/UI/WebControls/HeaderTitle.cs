// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderTitle.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The header title.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI.WebControls
{
    using System.Web;
    using System.Web.UI.WebControls;

    using Appleseed.Framework.Site.Configuration;

    /// <summary>
    /// The header title.
    /// </summary>
    public class HeaderTitle : Label
    {
        #region Public Methods

        /// <summary>
        /// Binds a data source to the invoked server control and all its child controls.
        /// </summary>
        public override void DataBind()
        {
            if (HttpContext.Current != null)
            {
                // Obtain PortalSettings from Current Context
                var PortalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];

                // Dynamically Populate the Portal Site Name
                this.Text = PortalSettings.PortalName;
            }

            base.DataBind();
        }

        #endregion
    }
}