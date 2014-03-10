// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageItem.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   PageItem Class
//   This class encapsulates the basic attributes of a Page, and is used
//   by the administration pages when manipulating tabs.<br />
//   PageItem implements
//   the IComparable interface so that an ArrayList of PageItems may be sorted
//   by PageOrder, using the ArrayList's Sort() method.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework
{
    using System;

    /// <summary>
    /// PageItem Class
    ///   This class encapsulates the basic attributes of a Page, and is used
    ///   by the administration pages when manipulating tabs.<br/>
    ///   PageItem implements
    ///   the IComparable interface so that an ArrayList of PageItems may be sorted
    ///   by PageOrder, using the ArrayList's Sort() method.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class PageItem : IComparable
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
        /// <remarks>
        /// </remarks>
        public int ID { get; set; }

        /// <summary>
        ///   Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        /// <remarks>
        /// </remarks>
        public string Name { get; set; }

        /// <summary>
        ///   Gets or sets the nest level.
        /// </summary>
        /// <value>The nest level.</value>
        /// <remarks>
        /// </remarks>
        public int NestLevel { get; set; }

        /// <summary>
        ///   Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        /// <remarks>
        /// </remarks>
        public int Order { get; set; }        

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
        /// The compare to.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public int CompareTo(object value)
        {
            if (value == null)
            {
                return 1;
            }

            var compareOrder = ((PageItem)value).Order;

            if (this.Order == compareOrder)
            {
                return 0;
            }

            if (this.Order < compareOrder)
            {
                return -1;
            }

            if (this.Order > compareOrder)
            {
                return 1;
            }

            return 0;
        }

        #endregion

        #endregion
    }
}