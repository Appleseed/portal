// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleItem.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   This class encapsulates the basic attributes of a Module, and is used
//   by the administration pages when manipulating modules.<br />
//   ModuleItem implements the IComparable interface so that an ArrayList
//   of ModuleItems may be sorted by ModuleOrder, using the
//   ArrayList's Sort() method.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework
{
    using System;

    /// <summary>
    /// This class encapsulates the basic attributes of a Module, and is used
    ///   by the administration pages when manipulating modules.<br/>
    ///   ModuleItem implements the IComparable interface so that an ArrayList
    ///   of ModuleItems may be sorted by ModuleOrder, using the
    ///   ArrayList's Sort() method.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class ModuleItem : IComparable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
        /// <remarks></remarks>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the module def ID.
        /// </summary>
        /// <value>The module def ID.</value>
        /// <remarks></remarks>
        public int ModuleDefID { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        /// <remarks></remarks>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the name of the pane.
        /// </summary>
        /// <value>The name of the pane.</value>
        /// <remarks></remarks>
        public string PaneName { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        /// <remarks></remarks>
        public string Title { get; set; }

        #endregion

        #region Implemented Interfaces

        #region IComparable

        /// <summary>
        /// Public comparer
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

            var compareOrder = ((ModuleItem)value).Order;

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

        static public int Compare(ModuleItem x, ModuleItem y)
        {
            int result = 1;
            if (x != null && x is ModuleItem &&
                y != null && y is ModuleItem) {
                ModuleItem itemX = (ModuleItem)x;
                ModuleItem itemY = (ModuleItem)y;
                result = itemX.CompareTo(itemY);
            }
            return result;
        }

        #endregion

        #endregion
    }
}