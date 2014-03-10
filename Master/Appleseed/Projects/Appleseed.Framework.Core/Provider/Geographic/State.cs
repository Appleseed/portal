// --------------------------------------------------------------------------------------------------------------------
// <copyright file="State.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The state.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Providers.Geographic
{
    using System;
    using System.Threading;

    /// <summary>
    /// The state.
    /// </summary>
    public class State : IEquatable<State>
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "State" /> class.
        /// </summary>
        public State()
        {
            this.StateID = 0;
            this.CountryID = string.Empty;
            this.NeutralName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="State"/> class.
        /// </summary>
        /// <param name="stateId">
        /// The state ID.
        /// </param>
        /// <param name="countryId">
        /// The country ID.
        /// </param>
        /// <param name="neutralName">
        /// Neutral Name.
        /// </param>
        public State(int stateId, string countryId, string neutralName)
        {
            this.StateID = stateId;
            this.CountryID = countryId;
            this.NeutralName = neutralName;
        }

        #endregion

        #region Properties

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
                return GeographicProvider.Current.GetStateDisplayName(this.StateID, Thread.CurrentThread.CurrentCulture);
            }
        }

        /// <summary>
        ///   Gets or sets the neutral name.
        /// </summary>
        /// <value>The name of the neutral.</value>
        public string NeutralName { get; set; }

        /// <summary>
        ///   Gets or sets the state ID.
        /// </summary>
        /// <value>The state ID.</value>
        public int StateID { get; set; }

        #endregion

        #region Operators

        /// <summary>
        ///   Implements the operator ==.
        /// </summary>
        /// <param name = "left">The left.</param>
        /// <param name = "right">The right.</param>
        /// <returns>
        ///   The result of the operator.
        /// </returns>
        public static bool operator ==(State left, State right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///   Implements the operator !=.
        /// </summary>
        /// <param name = "left">The left.</param>
        /// <param name = "right">The right.</param>
        /// <returns>
        ///   The result of the operator.
        /// </returns>
        public static bool operator !=(State left, State right)
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

            var otherState = (State)obj;
            return (this.CountryID == otherState.CountryID) && (this.NeutralName == otherState.NeutralName) &&
                   (this.StateID == otherState.StateID);
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
                var result = this.CountryID != null ? this.CountryID.GetHashCode() : 0;
                result = (result * 397) ^ (this.NeutralName != null ? this.NeutralName.GetHashCode() : 0);
                result = (result * 397) ^ this.StateID;
                return result;
            }
        }

        #endregion

        #region Implemented Interfaces

        #region IEquatable<State>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">
        /// An object to compare with this object.
        /// </param>
        public bool Equals(State other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.CountryID, this.CountryID) && Equals(other.NeutralName, this.NeutralName) &&
                   other.StateID == this.StateID;
        }

        #endregion

        #endregion
    }
}