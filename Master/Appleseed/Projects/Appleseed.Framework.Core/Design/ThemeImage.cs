// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThemeImage.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   A single named Image
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Design
{
    using System;
    using System.Web.UI.WebControls;
    using System.Xml.Serialization;

    /// <summary>
    /// A single named Image
    /// </summary>
    [Serializable]
    public class ThemeImage
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ThemeImage" /> class.
        /// </summary>
        public ThemeImage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeImage"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the image.
        /// </param>
        /// <param name="imageUrl">
        /// The image URL.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        public ThemeImage(string name, string imageUrl, double width, double height)
        {
            this.Name = name;
            this.ImageUrl = imageUrl;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeImage"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the theme image.
        /// </param>
        /// <param name="img">
        /// The image.
        /// </param>
        public ThemeImage(string name, Image img)
        {
            this.Name = name;
            this.ImageUrl = img.ImageUrl;
            this.Width = img.Width.Value;
            this.Height = img.Height.Value;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        [XmlAttribute]
        public double Height { get; set; }

        /// <summary>
        ///   Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        [XmlAttribute]
        public string ImageUrl { get; set; }

        /// <summary>
        ///   Gets or sets the name.
        /// </summary>
        /// <value>The name of the theme image.</value>
        [XmlAttribute(DataType = "string")]
        public string Name { get; set; }

        /// <summary>
        ///   Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        [XmlAttribute]
        public double Width { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <returns>
        /// A System.Web.UI.WebControls.Image value...
        /// </returns>
        public Image GetImage()
        {
            using (var img = new Image())
            {
                img.ImageUrl = this.ImageUrl;
                img.Width = new Unit(this.Width);
                img.Height = new Unit(this.Height);
                return img;
            }
        }

        #endregion
    }
}