// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogHelper.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Log Helper for use with Provider implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Helpers
{
    using System;

    using Appleseed.Framework.Logging;

    /// <summary>
    /// Log Helper for use with Provider implementation
    /// </summary>
    [History("jminond", "2006/2/23", "Made Log Helper Obsolete and removed from all core code")]
    public sealed class LogHelper
    {
        #region Constants and Fields

        /// <summary>
        /// The provider.
        /// </summary>
        private static readonly LogProvider Provider = LogProvider.Instance();

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the LogProvider Logger
        /// </summary>
        /// <value>The logger.</value>
        public static LogProvider Logger
        {
            get
            {
                return Provider;
            }
        }

        #endregion

        /// <summary>
        /// Log Class - Old style call
        /// </summary>
        public sealed class Log
        {
            #region Public Methods

            /// <summary>
            /// Debug Level
            /// </summary>
            /// <param name="message">The message.</param>
            [Obsolete("You should be using ErrorHandler.Publish()")]
            public static void Debug(object message)
            {
                ErrorHandler.Publish(LogLevel.Debug, message.ToString());
            }

            /// <summary>
            /// Debug Level
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="ex">The exception.</param>
            [Obsolete("You should be using ErrorHandler.Publish()")]
            public static void Debug(object message, Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Debug, message.ToString(), ex);
            }

            /// <summary>
            /// Error Level
            /// </summary>
            /// <param name="message">The message.</param>
            [Obsolete("You should be using ErrorHandler.Publish()")]
            public static void Error(object message)
            {
                ErrorHandler.Publish(LogLevel.Error, message.ToString());
            }

            /// <summary>
            /// Error Level
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="ex">The exception.</param>
            [Obsolete("You should be using ErrorHandler.Publish()")]
            public static void Error(object message, Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Error, message.ToString(), ex);
            }

            /// <summary>
            /// Fatal Error
            /// </summary>
            /// <param name="message">The message.</param>
            [Obsolete("You should be using ErrorHandler.Publish()")]
            public static void Fatal(object message)
            {
                ErrorHandler.Publish(LogLevel.Fatal, message.ToString());
            }

            /// <summary>
            /// FAtal Error
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="ex">The exception.</param>
            [Obsolete("You should be using ErrorHandler.Publish()")]
            public static void Fatal(object message, Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Fatal, message.ToString(), ex);
            }

            /// <summary>
            /// Info  Level
            /// </summary>
            /// <param name="message">The message.</param>
            [Obsolete("You should be using ErrorHandler.Publish()")]
            public static void Info(object message)
            {
                ErrorHandler.Publish(LogLevel.Info, message.ToString());
            }

            /// <summary>
            /// Info Level
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="ex">The exception.</param>
            [Obsolete("You should be using ErrorHandler.Publish()")]
            public static void Info(object message, Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Info, message.ToString(), ex);
            }

            /// <summary>
            /// Warn Level
            /// </summary>
            /// <param name="message">The message.</param>
            [Obsolete("You should be using ErrorHandler.Publish()")]
            public static void Warn(object message)
            {
                ErrorHandler.Publish(LogLevel.Warn, message.ToString());
            }

            /// <summary>
            /// Warn Level
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="ex">The exception.</param>
            [Obsolete("You should be using ErrorHandler.Publish()")]
            public static void Warn(object message, Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Warn, message.ToString(), ex);
            }

            #endregion
        }
    }
}