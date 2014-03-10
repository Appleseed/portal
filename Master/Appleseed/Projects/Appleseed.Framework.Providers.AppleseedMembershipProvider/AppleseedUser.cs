using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;
using System.Security.Principal;
using System.Web.Profile;

namespace Appleseed.Framework.Providers.AppleseedMembershipProvider {

    /// <summary>
    /// Appleseed specific membership user class.
    /// </summary>
    public class AppleseedUser : MembershipUser, IIdentity {

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleseedUser"/> class.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="name">The name.</param>
        /// <param name="providerUserKey">The provider user key.</param>
        /// <param name="email">The email.</param>
        /// <param name="passwordQuestion">The password question.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="isApproved">if set to <c>true</c> [is approved].</param>
        /// <param name="isLockedOut">if set to <c>true</c> [is locked out].</param>
        /// <param name="creationDate">The creation date.</param>
        /// <param name="lastLoginDate">The last login date.</param>
        /// <param name="lastActivityDate">The last activity date.</param>
        /// <param name="lastPasswordChangeDate">The last password change date.</param>
        /// <param name="lastLockoutDate">The last lockout date.</param>
        public AppleseedUser( string providerName, string name, Guid providerUserKey, string email, string passwordQuestion, string comment, bool isApproved, bool isLockedOut, DateTime creationDate, DateTime lastLoginDate, DateTime lastActivityDate, DateTime lastPasswordChangeDate, DateTime lastLockoutDate )
            : base( providerName, name, providerUserKey, email, passwordQuestion, comment, isApproved, isLockedOut, creationDate, lastLoginDate, lastActivityDate, lastPasswordChangeDate, lastLockoutDate ) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleseedUser"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public AppleseedUser( string name )
            : base( Membership.Provider.Name, name, Guid.Empty, string.Empty, string.Empty, string.Empty, true, false, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue ) {
            this.Name = name;
        }

        #region IIdentity Members

        /// <summary>
        /// Gets the type of authentication used.
        /// </summary>
        /// <value></value>
        /// <returns>The type of authentication used to identify the user.</returns>
        public string AuthenticationType {
            get {
                throw new Exception( "Appleseed." );
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the user has been authenticated.
        /// </summary>
        /// <value></value>
        /// <returns>true if the user was authenticated; otherwise, false.</returns>
        public bool IsAuthenticated {
            get {
                return IsOnline;
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets application-specific information for the membership user. We hide this property, as it isn't used in Appleseed
        /// </summary>
        /// <value></value>
        /// <returns>Application-specific information for the membership user.</returns>
        protected new string Comment {
            get {
                return base.Comment;
            }
            set {
                base.Comment = value;
            }
        }

        /// <summary>
        /// Gets the profile.
        /// </summary>
        /// <value>The profile.</value>
        public ProfileBase Profile {
            get {
                return ProfileBase.Create( this.UserName );
            }
        }

        #region Appleseed properties

        private string name;
        /// <summary>
        /// Gets the name of the current user.
        /// </summary>
        /// <value></value>
        /// <returns>The name of the user on whose behalf the code is running.</returns>
        public string Name {
            get {
                return name;
            }
            set {
                name = value;
            }
        }


        /// <summary>
        /// Gets the Password of the current user.
        /// </summary>
        /// <value></value>
        /// <returns>The name of the user on whose behalf the code is running.</returns>
        public string  Password
        {
            get
            {
                return this.GetPassword();
            }
        }

        private string phone;
        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>The phone.</value>
        public string Phone {
            get {
                return phone;
            }
            set {
                phone = value;
            }
        }

        /// <summary>
        /// Gets the user identifier from the membership data source for the user.
        /// </summary>
        /// <value></value>
        /// <returns>The user identifier from the membership data source for the user.</returns>
        public new Guid ProviderUserKey {
            get {
                return ( Guid )base.ProviderUserKey;
            }
        }

        private string company;

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public string Company {
            get {
                return company;
            }
            set {
                company = value;
            }
        }

        private string address;

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public string Address {
            get {
                return address;
            }
            set {
                address = value;
            }
        }

        private string zip;

        /// <summary>
        /// Gets or sets the zip.
        /// </summary>
        /// <value>The zip.</value>
        public string Zip {
            get {
                return zip;
            }
            set {
                zip = value;
            }
        }

        private string city;

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public string City {
            get {
                return city;
            }
            set {
                city = value;
            }
        }

        private string countryID;

        /// <summary>
        /// Gets or sets the country ID.
        /// </summary>
        /// <value>The country ID.</value>
        public string CountryID {
            get {
                return countryID;
            }
            set {
                countryID = value;
            }
        }

        private int stateID;

        /// <summary>
        /// Gets or sets the state ID.
        /// </summary>
        /// <value>The state ID.</value>
        public int StateID {
            get {
                return stateID;
            }
            set {
                stateID = value;
            }
        }

        private string fax;

        /// <summary>
        /// Gets or sets the fax.
        /// </summary>
        /// <value>The fax.</value>
        public string Fax {
            get {
                return fax;
            }
            set {
                fax = value;
            }
        }

        private bool sendNewsletter;

        /// <summary>
        /// Gets or sets a value indicating whether the user receives newletters.
        /// </summary>
        /// <value><c>true</c> if the user receives newletters; otherwise, <c>false</c>.</value>
        public bool SendNewsletter {
            get {
                return sendNewsletter;
            }
            set {
                sendNewsletter = value;
            }
        }

        #endregion

    }
}
