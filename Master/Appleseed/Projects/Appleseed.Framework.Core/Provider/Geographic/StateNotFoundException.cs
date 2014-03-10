// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StateNotFoundException.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The state not found exception.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Providers.Geographic
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The state not found exception.
    /// </summary>
    [Serializable]
    public class StateNotFoundException : ApplicationException
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StateNotFoundException"/> class.
        /// </summary>
        public StateNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateNotFoundException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public StateNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateNotFoundException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="inner">
        /// The inner.
        /// </param>
        public StateNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateNotFoundException"/> class.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        protected StateNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}