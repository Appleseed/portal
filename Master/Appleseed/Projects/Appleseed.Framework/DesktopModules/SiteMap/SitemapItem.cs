// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SitemapItem.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   A sitemap item. This just defines the simple data needed for the sitemap items.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Content.Web.Modules.Sitemap
{
    /// <summary>
    /// A sitemap item. This just defines the simple data needed for the sitemap items.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class SitemapItem
    {
        #region Constants and Fields

        /// <summary>
        ///   Item Id
        /// </summary>
        public int ID;

        /// <summary>
        ///   Item Name
        /// </summary>
        public string Name;

        /// <summary>
        ///   Item Nest Level
        /// </summary>
        public int NestLevel;

        /// <summary>
        ///   Item URL
        /// </summary>
        public string Url;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapItem"/> class. 
        /// </summary>
        /// <remarks>
        /// </remarks>
        public SitemapItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapItem"/> class. 
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="url">
        /// The URL.
        /// </param>
        /// <param name="nestlevel">
        /// The nestlevel.
        /// </param>
        /// <remarks>
        /// </remarks>
        public SitemapItem(int id, string name, string url, int nestlevel)
        {
            this.ID = id;
            this.Name = name;
            this.Url = url;
            this.NestLevel = nestlevel;
        }

        #endregion
    }
}