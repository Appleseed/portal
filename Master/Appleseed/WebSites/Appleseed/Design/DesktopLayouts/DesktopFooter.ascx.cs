// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesktopFooter.ascx.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Default user control placed at the bottom of each administrative page.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Content.Web.Modules
{
    using System;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Appleseed.Framework.Site.Configuration;

    using Path = Appleseed.Framework.Settings.Path;

    /// <summary>
    /// Default user control placed at the bottom of each administrative page.
    /// </summary>
    public partial class DesktopFooter : UserControl
    {
        #region Constants and Fields

        /// <summary>
        ///   Placeholder for current control
        /// </summary>
        protected PlaceHolder LayoutPlaceHolder;

        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        /// <remarks></remarks>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            const string LayoutBasePage = "DesktopFooter.ascx";

            // Obtain PortalSettings from Current Context
            var portalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            if (portalSettings == null)
            {
                return;
            }

            var footerPage = Path.WebPathCombine(portalSettings.PortalLayoutPath, LayoutBasePage);
            if (File.Exists(this.Server.MapPath(footerPage)))
            {
                this.LayoutPlaceHolder.Controls.Add(this.Page.LoadControl(footerPage));
            }

            // try
            // {
            //     //LayoutPlaceHolder.Controls.Add(Page.LoadControl(portalSettings.PortalLayoutPath + LayoutBasePage));
            //     LayoutPlaceHolder.Controls.Add(Page.LoadControl(portalSettings.PortalLayoutPath + LayoutBasePage));
            // }
            // catch
            // {
            //     //No footer available
            // }
        }

        #endregion
    }
}