using System;

namespace Appleseed.Framework.Exceptions
{
    /// <summary>
    /// Custom exception raised when code version is behind database version. Causes redirect to error page.
    /// </summary>
    [Serializable]
    public sealed class CodeVersionException : Exception
    {
        /// <summary>
        /// Constructor with message.
        /// </summary>
        /// <param name="message">Text message to be included in log.</param>
        public CodeVersionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor with message and innerException.
        /// </summary>
        /// <param name="message">Text message to be included in log.</param>
        /// <param name="inner">Inner exception.</param>
        public CodeVersionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}