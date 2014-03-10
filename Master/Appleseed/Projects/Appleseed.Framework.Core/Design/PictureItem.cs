// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PictureItem.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   PictureItem
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Design
{
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    using Appleseed.Framework.Site.Configuration;

    /// <summary>
    /// PictureItem
    /// </summary>
    public class PictureItem : UserControl
    {
        #region Constants and Fields

        /// <summary>
        /// The edit link.
        /// </summary>
        protected HyperLink editLink;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the metadata.
        /// </summary>
        /// <value>The metadata.</value>
        /// <remarks>
        /// </remarks>
        public XmlDocument Metadata { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the current image from theme.
        /// </summary>
        /// <param name="name">
        /// The name of the image.
        /// </param>
        /// <param name="bydefault">
        /// By default.
        /// </param>
        /// <returns>
        /// A string value...
        /// </returns>
        public string GetCurrentImageFromTheme(string name, string bydefault)
        {
            // Obtain PortalSettings from Current Context
            if (HttpContext.Current != null && HttpContext.Current.Items["PortalSettings"] != null)
            {
                var pS = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
                return pS.GetCurrentTheme().GetImage(name, bydefault).ImageUrl;
            }

            return bydefault;
        }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <param name="key">
        /// The key of the metadata.
        /// </param>
        /// <returns>
        /// A string value...
        /// </returns>
        public string GetMetadata(string key)
        {
            var targetNode = this.Metadata.SelectSingleNode("/Metadata/@" + key);

            return targetNode == null ? null : targetNode.Value;
        }

        #endregion
    }
}