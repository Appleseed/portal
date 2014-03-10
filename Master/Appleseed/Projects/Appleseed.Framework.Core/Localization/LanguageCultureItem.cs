// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LanguageCultureItem.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Single item in list. Language culture pair.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI.WebControls
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Single item in list. Language culture pair.
    /// </summary>
    /// <remarks>
    /// Esperantus - The Web translator
    ///   Copyright (C) 2003 Emmanuele De Andreis
    ///   This library is free software; you can redistribute it and/or
    ///   modify it under the terms of the GNU Lesser General Public
    ///   License as published by the Free Software Foundation; either
    ///   version 2 of the License, or (at your option) any later version.
    ///   This library is distributed in the hope that it will be useful,
    ///   but WITHOUT ANY WARRANTY; without even the implied warranty of
    ///   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    ///   Lesser General Public License for more details.
    ///   You should have received a copy of the GNU Lesser General Public
    ///   License along with this library; if not, write to the Free Software
    ///   Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
    ///   Emmanuele De Andreis (manu-dea@hotmail dot it)
    /// </remarks>
    public class LanguageCultureItem
    {
        #region Constants and Fields

        /// <summary>
        /// The m culture.
        /// </summary>
        private CultureInfo mCulture;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageCultureItem"/> class.
        /// </summary>
        /// <param name="uiCulture">
        /// The UI culture.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        public LanguageCultureItem(CultureInfo uiCulture, CultureInfo culture)
        {
            this.UICulture = uiCulture ?? CultureInfo.InvariantCulture;
            this.Culture = culture ?? CultureInfo.InvariantCulture;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "LanguageCultureItem" /> class.
        /// </summary>
        public LanguageCultureItem()
        {
            this.UICulture = CultureInfo.InvariantCulture;
            this.Culture = CultureInfo.CreateSpecificCulture(CultureInfo.InvariantCulture.Name);
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        public CultureInfo Culture
        {
            get
            {
                return this.mCulture;
            }

            set
            {
                if (value.IsNeutralCulture)
                {
                    throw new ArgumentException("Culture value cannot be neutral", "value");
                }

                this.mCulture = value;
            }
        }

        /// <summary>
        ///   Gets or sets the UI culture.
        /// </summary>
        /// <value>The UI culture.</value>
        public CultureInfo UICulture { get; set; }

        #endregion

        #region Operators

        /// <summary>
        ///   Performs an implicit conversion from <see cref = "Appleseed.Framework.Web.UI.WebControls.LanguageCultureItem" /> to <see cref = "System.String" />.
        /// </summary>
        /// <param name = "item">The item.</param>
        /// <returns>
        ///   The result of the conversion.
        /// </returns>
        public static implicit operator string(LanguageCultureItem item)
        {
            return item.ToString();
        }

        #endregion

        // 		public static bool operator==(LanguageCultureItem a, LanguageCultureItem b) 
        // 		{
        // 			return LanguageCultureItem.Equals(a, b);
        // 		}

        // 		public static bool operator!=(LanguageCultureItem a, LanguageCultureItem b) 
        // 		{
        // 			return !LanguageCultureItem.Equals(a, b);
        // 		}
        #region Public Methods

        /// <summary>
        /// Equals the specified a.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <returns>
        /// The equals.
        /// </returns>
        public static bool Equals(LanguageCultureItem a, LanguageCultureItem b)
        {
            return (a != null) && (b != null) && (a.ToString() == b.ToString() || a.Equals(b));
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.
        /// </param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(LanguageCultureItem) && Equals(this, (LanguageCultureItem)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return (this.UICulture.LCID * 5000) + this.Culture.LCID;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}/{1}", this.UICulture.Name, this.Culture.Name);
        }

        #endregion
    }
}