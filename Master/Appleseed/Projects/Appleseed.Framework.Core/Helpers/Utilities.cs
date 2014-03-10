// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Utilities.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   General utility methods
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Helpers
{
    using System;
    using System.Linq;
    using System.Net;

    /// <summary>
    /// General utility methods
    /// </summary>
    public class Utilities
    {
        #region Public Methods

        /// <summary>
        /// Intersects the string arrays.
        /// </summary>
        /// <param name="arrayA">
        /// The array A.
        /// </param>
        /// <param name="arrayB">
        /// The array B.
        /// </param>
        /// <returns>
        /// An <c>string[]</c> with the elements obtained from intersecting the arrays
        /// </returns>
        public static string[] IntersectArrays(string[] arrayA, string[] arrayB)
        {
            return arrayA.Where(elem1 => arrayB.Contains(elem1)).ToArray();
        }

        /// <summary>
        /// Tests a string to ensure that it is equivalent to a valid HTTP Status code
        /// </summary>
        /// <param name="str">
        /// the string to be tested
        /// </param>
        /// <returns>
        /// a Boolean value
        /// </returns>
        public static bool IsHttpStatusCode(string str)
        {
            return str != null && Enum.IsDefined(typeof(HttpStatusCode), str);
        }

        /// <summary>
        /// Determines whether the specified STR is integer.
        /// </summary>
        /// <param name="str">
        /// The string to test.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified STR is integer; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInteger(string str)
        {
            int aux;

            return int.TryParse(str, out aux);
        }

        #endregion
    }
}