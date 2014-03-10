using System;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Appleseed.Framework.Exceptions
{
	/// <summary>
	/// Custom Exception class for Appleseed.
	/// </summary>
	[Serializable]
	public class AppleseedException : Exception
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public AppleseedException()
		{
		}

		/// <summary>
		/// Constructor with message.
		/// </summary>
		/// <param name="message">Text message to be included in log.</param>
		public AppleseedException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor with message and innerException.
		/// </summary>
		/// <param name="message">Text message to be included in log.</param>
		/// <param name="inner">Inner exception</param>
		public AppleseedException(string message, Exception inner)
			: base(message, inner)
		{
		}

		/// <summary>
		/// Constructor with ExceptionLevel, message and innerException.
		/// </summary>
		/// <param name="level">ExceptionLevel enum</param>
		/// <param name="message">Text message to be included in log.</param>
		/// <param name="inner">Inner exception</param>
		public AppleseedException(Appleseed.Framework.LogLevel level, string message, Exception inner)
			: base(message, inner)
		{
			_level = level;
		}

		/// <summary>
		/// Constructor with ExceptionLevel, HttpStatusCode, message and innerException.
		/// </summary>
		/// <param name="level">ExceptionLevel enumerator</param>
		/// <param name="statusCode">HttpStatusCode enum</param>
		/// <param name="message">Text message to be included in log.</param>
		/// <param name="inner">Inner exception</param>
		public AppleseedException(Appleseed.Framework.LogLevel level, HttpStatusCode statusCode, string message, Exception inner)
			: base(message, inner)
		{
			_level = level;
			_statusCode = statusCode;
		}


		private LogLevel _level = LogLevel.Fatal;
		/// <summary>
		/// ExceptionLevel enumerator.
		/// </summary>
		/// <value>The level.</value>
		public LogLevel Level
		{
			get { return _level; }
			set { _level = value; }
		}

		private HttpStatusCode _statusCode = HttpStatusCode.InternalServerError;
		/// <summary>
		/// Gets or sets the status code.
		/// </summary>
		/// <value>The status code.</value>
		public HttpStatusCode StatusCode
		{
			get { return _statusCode; }
			set { _statusCode = value; }
		}

		/// <summary>
		/// Helper for de-serialization.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
		protected AppleseedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			_level = (Appleseed.Framework.LogLevel)info.GetValue("_level", typeof(Appleseed.Framework.LogLevel));
			_statusCode = (HttpStatusCode)info.GetValue("_statusCode", typeof(HttpStatusCode));
		}

		/// <summary>
		/// Helper for serialization.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.ArgumentNullException">The info parameter is a null reference (Nothing in Visual Basic). </exception>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/></PermissionSet>
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_level", (int)_level, typeof(Appleseed.Framework.LogLevel));
			info.AddValue("_statusCode", (int)_statusCode, typeof(HttpStatusCode));
			base.GetObjectData(info, context);
		}
	}
}