// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Generic extension methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Generic extension methods.
    /// </summary>
    public static class Extensions
    {
        #region Public Methods

        /// <summary>
        /// Clones the specified list.
        /// </summary>
        /// <typeparam name="T">
        /// The type of items in the list.
        /// </typeparam>
        /// <param name="listToClone">
        /// The list to clone.
        /// </param>
        /// <returns>
        /// A clone of the list.
        /// </returns>
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        /// <summary>
        /// Clones the specified list.
        /// </summary>
        /// <typeparam name="T">
        /// The type of items in the list.
        /// </typeparam>
        /// <param name="listToClone">
        /// The list to clone.
        /// </param>
        /// <returns>
        /// A clone of the list.
        /// </returns>
        public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => item.Clone()).Cast<T>().ToList();
        }

        #endregion
    }
}