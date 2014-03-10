// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppleseedRole.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The appleseed role.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Providers.AppleseedRoleProvider
{
    using System;

    /// <summary>
    /// The appleseed role.
    /// </summary>
    public class AppleseedRole : IComparable, IEquatable<AppleseedRole>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleseedRole"/> class.
        /// </summary>
        /// <param name="roleId">
        /// The role id.
        /// </param>
        /// <param name="roleName">
        /// The role name.
        /// </param>
        public AppleseedRole(Guid roleId, string roleName)
        {
            this.Id = roleId;
            this.Name = roleName;
            if (roleName.Equals("Admins"))
                this.Description = string.Empty;
            else
                this.Description = roleName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleseedRole"/> class.
        /// </summary>
        /// <param name="roleId">
        /// The role id.
        /// </param>
        /// <param name="roleName">
        /// The role name.
        /// </param>
        /// <param name="roleDescription">
        /// The role description.
        /// </param>
        public AppleseedRole(Guid roleId, string roleName, string roleDescription)
        {
            this.Id = roleId;
            this.Name = roleName;
            this.Description = roleDescription;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///   Gets or sets Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///   Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

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
        public static bool operator ==(AppleseedRole left, AppleseedRole right)
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
        public static bool operator !=(AppleseedRole left, AppleseedRole right)
        {
            return !Equals(left, right);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="System.Object"/> to compare with this instance.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            // Check for null and compare run-time types.
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            var role = (AppleseedRole)obj;
            return (this.Id == role.Id) && (this.Name == role.Name);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var result = this.Description != null ? this.Description.GetHashCode() : 0;
                result = (result * 397) ^ this.Id.GetHashCode();
                result = (result * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                return result;
            }
        }

        #endregion

        #region Implemented Interfaces

        #region IComparable

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">
        /// An object to compare with this instance.
        /// </param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="obj"/> is not the same type as this instance. 
        /// </exception>
        public int CompareTo(object obj)
        {
            if (obj is AppleseedRole)
            {
                var role = (AppleseedRole)obj;
                return this.Name.CompareTo(role.Name);
            }

            throw new ArgumentException("object is not a AppleseedRole");
        }

        #endregion

        #region IEquatable<AppleseedRole>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">
        /// An object to compare with this object.
        /// </param>
        /// <returns>
        /// true if the current object is equal to the other parameter; otherwise, false.
        /// </returns>
        public bool Equals(AppleseedRole other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.Description, this.Description) && other.Id.Equals(this.Id) &&
                   Equals(other.Name, this.Name);
        }

        #endregion

        #endregion
    }
}