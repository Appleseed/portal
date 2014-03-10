// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateEntry.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The update entry.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Update
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The update entry.
    /// </summary>
    [Serializable]
    public class UpdateEntry : IComparable<UpdateEntry>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateEntry"/> class. 
        /// </summary>
        /// <remarks>
        /// </remarks>
        public UpdateEntry()
        {
            this.Modules = new List<string>();
            this.Version = string.Empty;
            this.ScriptNames = new List<string>();
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets a value indicating whether this <see cref = "UpdateEntry" /> is apply.
        /// </summary>
        /// <value><c>true</c> if apply; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// </remarks>
        public bool Apply { get; set; }

        /// <summary>
        ///   Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        /// <remarks>
        /// </remarks>
        public DateTime Date { get; set; }

        /// <summary>
        ///   Gets or sets the modules.
        /// </summary>
        /// <value>The modules.</value>
        /// <remarks>
        /// </remarks>
        public List<string> Modules { get; set; }

        /// <summary>
        ///   Gets or sets the script names.
        /// </summary>
        /// <value>The script names.</value>
        /// <remarks>
        /// </remarks>
        public List<string> ScriptNames { get; set; }

        /// <summary>
        ///   Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        /// <remarks>
        /// </remarks>
        public string Version { get; set; }

        /// <summary>
        ///   Gets or sets the version number.
        /// </summary>
        /// <value>The version number.</value>
        /// <remarks>
        /// </remarks>
        public int VersionNumber { get; set; }

        #endregion

        #region Implemented Interfaces

        #region IComparable<UpdateEntry>

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">
        /// An object to compare with this object.
        /// </param>
        public int CompareTo(UpdateEntry other)
        {
            // if .. then Version numbers are equal else ..
            return this.VersionNumber.CompareTo(other.VersionNumber) == 0
                       ? this.Version.CompareTo(other.Version)
                       : this.VersionNumber.CompareTo(other.VersionNumber);
        }

        #endregion

        #endregion
    }
}