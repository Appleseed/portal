// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmailAddressList.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   This implementation of the ArrayList class, allows only valid
//   email addresses to be added to the collection
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.DataTypes
{
    using System;
    using System.Collections;
    using System.Text.RegularExpressions;

    /// <summary>
    /// This implementation of the ArrayList class, allows only valid 
    ///   email addresses to be added to the collection
    /// </summary>
    public class EmailAddressList : ArrayList
    {
        #region Constants and Fields

        /// <summary>
        /// The regular expression.
        /// </summary>
        private static Regex regex;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the email address.
        /// </summary>
        /// <value>The email address.</value>
        private static Regex EmailAddressRegex
        {
            get
            {
                return regex ??
                       (regex =
                        new Regex(
                            "^([a-zA-Z0-9_\\.\\-])+\\@(([a-zA-Z0-9\\-])+\\.)+([a-zA-Z0-9]{2,4})+$", 
                            RegexOptions.Compiled & RegexOptions.IgnoreCase));
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds an object to the end of the <see cref="T:System.Collections.ArrayList"></see>.
        /// </summary>
        /// <param name="value">
        /// The <see cref="T:System.Object"></see> to be added to the end of the <see cref="T:System.Collections.ArrayList"></see>. The value can be null.
        /// </param>
        /// <returns>
        /// The <see cref="T:System.Collections.ArrayList"></see> index at which the value has been added.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// The <see cref="T:System.Collections.ArrayList"></see> is read-only.-or- The <see cref="T:System.Collections.ArrayList"></see> has a fixed size. 
        /// </exception>
        public override int Add(object value)
        {
            // Check if the value isn't null
            if (value == null)
            {
                throw new ArgumentNullException("value", "You can not add null email-addresses to the collection.");
            }

            // Check if the value is a string
            if (! (value is string))
            {
                throw new ArgumentOutOfRangeException("value", "Only string values are allowed.");
            }

            // Check if the value can be matched to the regular expression
            if (! EmailAddressRegex.IsMatch((string)value))
            {
                throw new ArgumentException("Only valid email-addresses are allowed.", "value");
            }

            // This is a valid email address
            return base.Add(value);
        }

        #endregion
    }
}