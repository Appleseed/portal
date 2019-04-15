// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppleseedSiteMapProvider.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The appleseed site map provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Providers.AppleseedSiteMapProvider
{
    using System;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// The appleseed site map provider.
    /// </summary>
    public abstract class AppleseedSiteMapProvider : StaticSiteMapProvider
    {
        #region Public Methods

        /// <summary>
        /// The clear all appleseed site map caches.
        /// </summary>
        public static void ClearAllAppleseedSiteMapCaches()
        {
            // Removing Sitemap Cache
            foreach (var siteMap in SiteMap.Providers.OfType<AppleseedSiteMapProvider>())
            {
                siteMap.ClearCache();
            }
        }
        // Implement the CurrentNode property.
        public override SiteMapNode CurrentNode
        {
            get
            {
                var currentUrl = FindCurrentUrl();

                // Find the SiteMapNode that represents the current page.
                var currentNode = FindSiteMapNode(currentUrl);
                return currentNode;
            }
        }

        // Get the URL of the currently displayed page.
        string FindCurrentUrl()
        {
            try
            {
                // The current HttpContext.
                var currentContext = HttpContext.Current;

                if (currentContext != null) return currentContext.Request.Path;

                throw new Exception("HttpContext.Current is Invalid");

            }
            catch (Exception e)
            {
                throw new NotSupportedException("This provider requires a valid context.", e);
            }
        }
        /// <summary>
        /// The clear cache.
        /// </summary>
        public abstract void ClearCache();

        #endregion
    }
}