// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageDTO.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The image dto.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.DataTypes
{
    /// <summary>
    /// The image dto.
    /// </summary>
    public class ImageDTO
    {
        #region Properties

        /// <summary>
        /// Gets or sets CompletePath.
        /// </summary>
        public string CompletePath { get; set; }

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets FileExtension.
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// Gets or sets GalleryDescription.
        /// </summary>
        public string GalleryDescription { get; set; }

        /// <summary>
        /// Gets or sets GalleryId.
        /// </summary>
        public long GalleryId { get; set; }

        /// <summary>
        /// Gets or sets GalleryName.
        /// </summary>
        public string GalleryName { get; set; }

        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public long Id { get; set; }

        #endregion
    }
}