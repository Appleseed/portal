// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Country.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The country.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Providers.Geographic
{
    using System;
    using System.Threading;

    /// <summary>
    /// The country.
    /// </summary>
    public class Country : IEquatable<Country>
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Country" /> class.
        /// </summary>
        public Country()
        {
            this.CountryID = string.Empty;
            this.NeutralName = string.Empty;
            this.AdministrativeDivisionNeutralName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Country"/> class.
        /// </summary>
        /// <param name="countryId">
        /// The country id.
        /// </param>
        /// <param name="neutralName">
        /// The neutral name.
        /// </param>
        /// <param name="administrativeDivision">
        /// The administrative division.
        /// </param>
        public Country(string countryId, string neutralName, string administrativeDivision)
        {
            this.CountryID = countryId;
            this.NeutralName = neutralName;
            this.AdministrativeDivisionNeutralName = administrativeDivision;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the name of the administrative division.
        /// </summary>
        /// <value>The name of the administrative division.</value>
        public string AdministrativeDivisionName
        {
            get
            {
                return GeographicProvider.Current.GetAdministrativeDivisionName(
                    this.AdministrativeDivisionNeutralName, Thread.CurrentThread.CurrentCulture);
            }
        }

        /// <summary>
        ///   Gets or sets the name of the administrative division neutral name.
        /// </summary>
        /// <value>The name of the administrative division neutral.</value>
        public string AdministrativeDivisionNeutralName { get; set; }

        /// <summary>
        ///   Gets or sets the country ID.
        /// </summary>
        /// <value>The country ID.</value>
        public string CountryID { get; set; }

        /// <summary>
        ///   Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return GeographicProvider.Current.GetCountryDisplayName(
                    this.CountryID, Thread.CurrentThread.CurrentCulture);
            }
        }

        /// <summary>
        ///   Gets or sets the neutral name.
        /// </summary>
        /// <value>The name of the neutral.</value>
        public string NeutralName { get; set; }

        #endregion

        #region Operators

        /// <summary>
        /// The ==.
        /// </summary>
        /// <param name="left">
        /// The left.
        /// </param>
        /// <param name="right">
        /// The right.
        /// </param>
        /// <returns>
        /// </returns>
        public static bool operator ==(Country left, Country right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// The !=.
        /// </summary>
        /// <param name="left">
        /// The left.
        /// </param>
        /// <param name="right">
        /// The right.
        /// </param>
        /// <returns>
        /// </returns>
        public static bool operator !=(Country left, Country right)
        {
            return !Equals(left, right);
        }

        #endregion

        #region Public Methods

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
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            var otherCountry = (Country)obj;
            return (this.CountryID == otherCountry.CountryID) && (this.NeutralName == otherCountry.NeutralName) &&
                   (this.AdministrativeDivisionNeutralName == otherCountry.AdministrativeDivisionNeutralName);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                var result = this.AdministrativeDivisionNeutralName != null
                                  ? this.AdministrativeDivisionNeutralName.GetHashCode()
                                  : 0;
                result = (result * 397) ^ (this.CountryID != null ? this.CountryID.GetHashCode() : 0);
                result = (result * 397) ^ (this.NeutralName != null ? this.NeutralName.GetHashCode() : 0);
                return result;
            }
        }

        #endregion

        #region Implemented Interfaces

        #region IEquatable<Country>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">
        /// An object to compare with this object.
        /// </param>
        public bool Equals(Country other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.AdministrativeDivisionNeutralName, this.AdministrativeDivisionNeutralName) &&
                   Equals(other.CountryID, this.CountryID) && Equals(other.NeutralName, this.NeutralName);
        }

        #endregion

        #endregion
    }
}