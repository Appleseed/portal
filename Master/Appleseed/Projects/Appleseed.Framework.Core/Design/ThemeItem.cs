// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThemeItem.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   ThemeItem encapsulates the items of Theme list.
//   Uses IComparable interface to allow sorting by name.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Design
{
    using System;

    /// <summary>
    /// ThemeItem encapsulates the items of Theme list.
    ///   Uses IComparable interface to allow sorting by name.
    /// </summary>
    public class ThemeItem : IComparable, ICloneable
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the name of the theme.
        /// </summary>
        /// <value>The name of the theme.</value>
        public string Name { get; set; }

        #endregion

        #region Implemented Interfaces

        #region IComparable

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// An integer value...
        /// </returns>
        public int CompareTo(object value)
        {
            return this.CompareTo(this.Name);
        }

        #endregion

        #endregion

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public object Clone()
        {
            return new ThemeItem { Name = this.Name };
        }
    }
}