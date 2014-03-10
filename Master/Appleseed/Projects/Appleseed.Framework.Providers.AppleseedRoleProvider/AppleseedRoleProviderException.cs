// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppleseedRoleProviderException.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The appleseed role provider exception.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Providers.AppleseedRoleProvider
{
    using System;
    using System.Configuration.Provider;
    using System.Runtime.Serialization;

    /// <summary>
    /// The appleseed role provider exception.
    /// </summary>
    [Serializable]
    public class AppleseedRoleProviderException : ProviderException
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleseedRoleProviderException"/> class.
        /// </summary>
        public AppleseedRoleProviderException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleseedRoleProviderException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public AppleseedRoleProviderException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleseedRoleProviderException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="inner">
        /// The inner.
        /// </param>
        public AppleseedRoleProviderException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleseedRoleProviderException"/> class.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        protected AppleseedRoleProviderException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}