using System;

namespace Appleseed.Framework.Exceptions
{
	/// <summary>
	/// Custom exception raised when database version is behind code version. Causes redirect to Database Update page.
	/// </summary>
	[Serializable]
	public sealed class DatabaseVersionException : Exception
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public DatabaseVersionException()
		{
		}

		/// <summary>
		/// Constructor with message.
		/// </summary>
		/// <param name="message">Text message to be included in log.</param>
		public DatabaseVersionException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor with message and innerException.
		/// </summary>
		/// <param name="message">Text message to be included in log.</param>
		/// <param name="inner">Inner exception.</param>
		public DatabaseVersionException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}