// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesktopPanes.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The desktop panes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI.WebControls
{
    using Appleseed.Framework.Core.Model;

    /// <summary>
    /// The desktop panes.
    /// </summary>
    public class DesktopPanes : DesktopPanesBase
    {
        #region Methods

        /// <summary>
        /// This method determines the tab index of the currently
        ///   requested portal view, and then dynamically populate the left,
        ///   center and right hand sections of the portal tab.
        /// </summary>
        protected override void InitializeDataSource()
        {
            this.DataSource = ModelServices.GetCurrentPageModules();
        }

        #endregion
    }
}