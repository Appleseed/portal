// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISitemapRenderer.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   This defines an interface for a Sitemap renderer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Content.Web.Modules.Sitemap
{
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    /// <summary>
    /// This defines an interface for a Sitemap renderer.
    /// </summary>
    public interface ISitemapRenderer
    {
        #region Public Methods

        /// <summary>
        /// Renders the specified list.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <returns>
        /// A web control.
        /// </returns>
        /// <remarks>
        /// </remarks>
        WebControl Render(IList<SitemapItem> list);

        #endregion
    }
}