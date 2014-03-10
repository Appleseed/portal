// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThemePart.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   A single named HTML fragment
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Design
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// A single named HTML fragment
    /// </summary>
    [Serializable]
    public class ThemePart
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ThemePart" /> class.
        /// </summary>
        public ThemePart()
        {
            this.Name = string.Empty;
            this.Html = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemePart"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the part.
        /// </param>
        /// <param name="html">
        /// The HTML of the part.
        /// </param>
        public ThemePart(string name, string html)
        {
            this.Name = name;
            this.Html = html;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the HTML.
        /// </summary>
        /// <value>The HTML of the part.</value>
        public string Html { get; set; }

        /// <summary>
        ///   Gets or sets the name.
        /// </summary>
        /// <value>The name of the part.</value>
        [XmlAttribute(DataType = "string")]
        public string Name { get; set; }

        #endregion
    }
}