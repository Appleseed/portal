using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;

namespace Appleseed.Framework.Providers.AppleseedMembershipProvider {

    /// <summary>
    /// Appleseed-specific provider exception
    /// </summary>
    [global::System.Serializable]
    public class AppleseedMembershipProviderException : ProviderException {

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleseedMembershipProviderException"/> class.
        /// </summary>
        public AppleseedMembershipProviderException() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleseedMembershipProviderException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public AppleseedMembershipProviderException( string message )
            : base( message ) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleseedMembershipProviderException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public AppleseedMembershipProviderException( string message, Exception inner )
            : base( message, inner ) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleseedMembershipProviderException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the information to deserialize.</param>
        /// <param name="context">Contextual information about the source or destination.</param>
        protected AppleseedMembershipProviderException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context ) : base( info, context ) {
        }
    }
}
