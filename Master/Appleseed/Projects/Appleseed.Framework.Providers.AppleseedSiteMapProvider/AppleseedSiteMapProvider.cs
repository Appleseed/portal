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

        /// <summary>
        /// The clear cache.
        /// </summary>
        public abstract void ClearCache();

        #endregion
    }
}