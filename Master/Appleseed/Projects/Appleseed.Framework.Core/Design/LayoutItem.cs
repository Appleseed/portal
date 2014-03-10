// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LayoutItem.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   LayoutItem encapsulates the items of Layout list.
//   Uses IComparable interface to allow sorting by name.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Design
{
    using System;

    /// <summary>
    /// LayoutItem encapsulates the items of Layout list.
    ///   Uses IComparable interface to allow sorting by name.
    /// </summary>
    /// <remarks>
    /// by Cory Isakson
    /// </remarks>
    public class LayoutItem : IComparable, ICloneable
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the name of the layout.
        /// </summary>
        /// <value>The name of the layout.</value>
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
        /// An integer.
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
            return new LayoutItem { Name = this.Name };
        }
    }
}