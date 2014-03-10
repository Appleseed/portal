// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortalItem.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   This class encapsulates the basic attributes of a Portal, and is used
//   by the administration pages when manipulating Portals.  PortalItem implements
//   the IComparable interface so that an ArrayList of PortalItems may be sorted
//   by PortalOrder, using the ArrayList's Sort() method.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework
{
    using System;

    /// <summary>
    /// This class encapsulates the basic attributes of a Portal, and is used
    ///   by the administration pages when manipulating Portals.  PortalItem implements
    ///   the IComparable interface so that an ArrayList of PortalItems may be sorted
    ///   by PortalOrder, using the ArrayList's Sort() method.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class PortalItem : IComparable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
        /// <remarks></remarks>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        /// <remarks></remarks>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        /// <remarks></remarks>
        public string Path { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return this.Name;
        }

        #endregion

        #region Implemented Interfaces

        #region IComparable

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int CompareTo(object value)
        {
            return this.CompareTo(this.Name);
        }

        #endregion

        #endregion
    }
}