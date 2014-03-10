using System;

namespace Appleseed.Framework.Exceptions
{
    /// <summary>
    /// Custom exception raised when database appears to be unreachable.
    /// Configured to redirect to static (HTML) page.
    /// </summary>
    [Serializable]
    public sealed class DatabaseUnreachableException : Exception
    {
        /// <summary>
        /// Constructor with message.
        /// </summary>
        /// <param name="message">Text message to be included in log.</param>
        public DatabaseUnreachableException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor with message and innerException.
        /// </summary>
        /// <param name="message">Text message to be included in log.</param>
        /// <param name="inner">Inner exception.</param>
        public DatabaseUnreachableException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}