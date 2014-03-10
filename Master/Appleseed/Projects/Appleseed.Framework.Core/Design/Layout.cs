// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Layout.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The Layout class encapsulates all the settings
//   of the currently selected layout
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Design
{
    using System;
    using System.Web;

    /// <summary>
    /// The Layout class encapsulates all the settings
    ///   of the currently selected layout
    /// </summary>
    /// <remarks>
    /// by Cory Isakson
    /// </remarks>
    public class Layout
    {
        #region Constants and Fields

        /// <summary>
        ///   Current Web Path.
        ///   It is set at runtime and therefore is not serialized
        /// </summary>
        [NonSerialized]
        public string WebPath;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the Layout Name (must be the directory in which is located)
        /// </summary>
        /// <value>The layout name.</value>
        public string Name { get; set; }

        /// <summary>
        ///   Gets the current Physical Path. Read-only.
        /// </summary>
        /// <value>The physical path.</value>
        public string Path
        {
            get
            {
                return HttpContext.Current.Server.MapPath(this.WebPath);
            }
        }

        #endregion
    }
}