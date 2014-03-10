// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOHelper.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
// This code is released under Duemetri Public License (DPL) Version 1.2.
// Coder: Mario Hartmann [mario@hartmann.net // http://mario.hartmann.net/]
// Original version: C#
// Original product name: Appleseed
// Official site: http://www.Appleseedportal.net
// Last updated Date: 28/MAY/2004
// Derivate works, translation in other languages and binary distribution
// of this code must retain this copyright notice and include the complete 
// license text that comes with product.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Helpers
{
    using System.IO;

    /// <summary>
    /// This code is released under Duemetri Public License (DPL) Version 1.2.
    /// Coder: Mario Hartmann [mario@hartmann.net // http://mario.hartmann.net/]
    /// Original version: C#
    /// Original product name: Appleseed
    /// Official site: http://www.Appleseedportal.net
    /// Last updated Date: 28/MAY/2004
    /// Derivate works, translation in other languages and binary distribution
    /// of this code must retain this copyright notice and include the complete 
    /// license text that comes with product.
    /// </summary>
    public class IOHelper
    {
        #region Public Methods

        /// <summary>
        /// Creates all directories and subdirectories as specified and return false in case of an error.
        /// </summary>
        /// <param name="physicalPath">The physical path.</param>
        /// <returns>
        /// The create directory.
        /// </returns>
        public static bool CreateDirectory(string physicalPath)
        {
            var returnValue = true;

            if (!Directory.Exists(physicalPath))
            {
                try
                {
                    Directory.CreateDirectory(physicalPath);
                }
                catch
                {
                    returnValue = false;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Returns the names of files in the specified directory that match the specified search patterns.
        /// </summary>
        /// <param name="path">
        /// the directory to search.
        /// </param>
        /// <param name="searchPatterns">
        /// the search strings to match against the names of files in the path deliminated by a ';'. For example:"*.gif;*.xl?;my*.txt"
        /// </param>
        /// <returns>
        /// A string array of the files.
        /// </returns>
        public static string[] GetFiles(string path, string searchPatterns)
        {
            // declare the return array
            var returnArray = new string[0];

            if (Directory.Exists(path))
            {
                // loop through the given search patterns
                foreach (var ext in searchPatterns.Split(';'))
                {
                    var tmpArray = Directory.GetFiles(path, ext);
                    if (tmpArray.Length <= 0)
                    {
                        continue;
                    }

                    var newArray = new string[returnArray.Length + tmpArray.Length];
                    returnArray.CopyTo(newArray, 0);
                    tmpArray.CopyTo(newArray, returnArray.Length);
                    returnArray = newArray;
                }
            }

            return returnArray;
        }

        #endregion
    }
}