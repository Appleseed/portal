// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesktopPortalBanner.ascx.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Default user control placed on top of each administrative page
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Content.Web.Modules
{
    using System;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Appleseed.Framework;
    using Appleseed.Framework.Site.Configuration;

    using Path = Appleseed.Framework.Settings.Path;

    /// <summary>
    /// Default user control placed on top of each administrative page
    /// </summary>
    public partial class DesktopPortalBanner : UserControl
    {
        #region Constants and Fields

        /// <summary>
        ///   Placeholder for current control
        /// </summary>
        protected PlaceHolder LayoutPlaceHolder;

        /// <summary>
        /// The show tabs.
        /// </summary>
        /// <author>
        ///   jes1111 
        /// </author>
        private bool showTabs = true;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets a value indicating whether [show tabs].
        /// </summary>
        /// <value><c>true</c> if [show tabs]; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// </remarks>
        public bool ShowTabs
        {
            get
            {
                return this.showTabs;
            }

            set
            {
                this.showTabs = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"/> object that contains the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            const string LayoutBasePage = "DesktopPortalBanner.ascx";

            // Obtain PortalSettings from Current Context
            var portalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];

            if (portalSettings == null)
            {
                return;
            }

            // jes1111 
            portalSettings.ShowPages = this.ShowTabs;

            // [START] file path -- bja@reedtek.com
            // Validate that the layout file is present. I have found
            // that sometimes they go away in different releases. So let's check
            // string filepath = portalSettings.PortalLayoutPath + LayoutBasePage;
            var filepath = Path.WebPathCombine(portalSettings.PortalLayoutPath, LayoutBasePage);

            // does it exist
            if (File.Exists(this.Server.MapPath(filepath)))
            {
                this.LayoutPlaceHolder.Controls.Add(this.Page.LoadControl(filepath));
            }
            else
            {
                // create an exception
                var ex = new Exception(string.Format("Portal cannot find layout ('{0}')", filepath));

                // go log/handle it
                // ErrorHandler.HandleException(ex);
                ErrorHandler.Publish(LogLevel.Error, ex);
            }

            // [END] file path -- bja@reedtek.com
        }

        #endregion
    }
}