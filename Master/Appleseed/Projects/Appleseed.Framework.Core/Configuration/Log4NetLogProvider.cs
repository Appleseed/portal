// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Log4NetLogProvider.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Log4Net provider implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// Created by Manu
// Import log4net classes.

namespace Appleseed.Framework.Logging
{
    using System;
    using System.Collections.Specialized;
    using System.IO;

    using log4net;
    using log4net.Appender;
    using log4net.Config;
    using log4net.Layout;
    using log4net.Repository.Hierarchy;

    /// <summary>
    /// Log4Net provider implementation
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class Log4NetLogProvider : LogProvider
    {
        // Define a static log4net logger variable so that it references the
        // Logger instance named "LogHelper".
        #region Constants and Fields

        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger("Appleseed");

        /// <summary>
        /// The ma.
        /// </summary>
        private static MemoryAppender ma;

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the specified name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="configValue">
        /// The config value.
        /// </param>
        /// <remarks>
        /// </remarks>
        public override void Initialize(string name, NameValueCollection configValue)
        {
            // Initialize the logging when the application loads
            // DOMConfigurator.Configure(); // Jes1111 - deprecated in log4net 1.2.9
            XmlConfigurator.Configure();

            GlobalContext.Properties["CodeVersion"] = new LogCodeVersionProperty();
            GlobalContext.Properties["User"] = new LogUserNameProperty();
            GlobalContext.Properties["RewrittenUrl"] = new LogRewrittenUrlProperty();
            GlobalContext.Properties["UserAgent"] = new LogUserAgentProperty();
            GlobalContext.Properties["UserIP"] = new LogUserIpProperty();
            GlobalContext.Properties["UserLanguages"] = new LogUserLanguagesProperty();

            var h = (Hierarchy)LogManager.GetRepository();
            ma = (MemoryAppender)((Logger)h.GetLogger("Appleseed")).GetAppender("SmartError");
        }

        /// <summary>
        /// Logs the specified level.
        /// </summary>
        /// <param name="level">
        /// The level.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <remarks>
        /// </remarks>
        public override void Log(LogLevel level, object message)
        {
            // It is VERY Important that the log are 
            // in the very same order that appear on log4net
            if (log.IsDebugEnabled)
            {
                if (level == LogLevel.Debug)
                {
                    log.Debug(message);

                    // no need to go on
                    return;
                }
            }
            else
            {
                // No need to test others: if the lower level is false other will be false
                return;
            }

            if (log.IsInfoEnabled)
            {
                if (level == LogLevel.Info)
                {
                    log.Info(message);

                    // no need to go on
                    return;
                }
            }
            else
            {
                // No need to test others: if the lower level is false other will be false
                return;
            }

            if (log.IsWarnEnabled)
            {
                if (level == LogLevel.Warn)
                {
                    log.Warn(message);

                    // no need to go on
                    return;
                }
            }
            else
            {
                // No need to test others: if the lower level is false other will be false
                return;
            }

            if (log.IsErrorEnabled)
            {
                if (level == LogLevel.Error)
                {
                    log.Error(message);

                    // no need to go on
                    return;
                }
            }
            else
            {
                // No need to test others: if the lower level is false other will be false
                return;
            }

            if (log.IsFatalEnabled && level == LogLevel.Fatal)
            {
                log.Fatal(message);
            }
        }

        /// <summary>
        /// Logs the specified level.
        /// </summary>
        /// <param name="level">
        /// The level.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="t">
        /// The t.
        /// </param>
        /// <remarks>
        /// </remarks>
        public override void Log(LogLevel level, object message, Exception t)
        {
            // It is VERY Important that the log are 
            // in the very same order that appear on log4net
            if (log.IsDebugEnabled)
            {
                if (level == LogLevel.Debug)
                {
                    log.Debug(message, t);

                    // no need to go on
                    return;
                }
            }
            else
            {
                // No need to test others: if the lower level is false other will be false
                return;
            }

            if (log.IsInfoEnabled)
            {
                if (level == LogLevel.Info)
                {
                    log.Info(message, t);

                    // no need to go on
                    return;
                }
            }
            else
            {
                // No need to test others: if the lower level is false other will be false
                return;
            }

            if (log.IsWarnEnabled)
            {
                if (level == LogLevel.Warn)
                {
                    log.Warn(message, t);

                    // no need to go on
                    return;
                }
            }
            else
            {
                // No need to test others: if the lower level is false other will be false
                return;
            }

            if (log.IsErrorEnabled)
            {
                if (level == LogLevel.Error)
                {
                    log.Error(message, t);

                    // no need to go on
                    return;
                }
            }
            else
            {
                // No need to test others: if the lower level is false other will be false
                return;
            }

            if (log.IsFatalEnabled)
            {
                if (level == LogLevel.Fatal)
                {
                    log.Fatal(message, t);
                }
            }
        }

        /// <summary>
        /// Logs the specified level.
        /// </summary>
        /// <param name="level">
        /// The level.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="sw">
        /// The sw.
        /// </param>
        /// <remarks>
        /// </remarks>
        public override void Log(LogLevel level, object message, StringWriter sw)
        {
            // It is VERY Important that the log are 
            // in the very same order that appear on log4net
            if (log.IsDebugEnabled)
            {
                if (level == LogLevel.Debug)
                {
                    log.Debug(message);
                    this.FillTextWriter(sw);

                    // no need to go on
                    return;
                }
            }
            else
            {
                // No need to test others: if the lower level is false other will be false
                return;
            }

            if (log.IsInfoEnabled)
            {
                if (level == LogLevel.Info)
                {
                    log.Info(message);
                    this.FillTextWriter(sw);

                    // no need to go on
                    return;
                }
            }
            else
            {
                // No need to test others: if the lower level is false other will be false
                return;
            }

            if (log.IsWarnEnabled)
            {
                if (level == LogLevel.Warn)
                {
                    log.Warn(message);
                    this.FillTextWriter(sw);

                    // no need to go on
                    return;
                }
            }
            else
            {
                // No need to test others: if the lower level is false other will be false
                return;
            }

            if (log.IsErrorEnabled)
            {
                if (level == LogLevel.Error)
                {
                    log.Error(message);
                    this.FillTextWriter(sw);

                    // no need to go on
                    return;
                }
            }
            else
            {
                // No need to test others: if the lower level is false other will be false
                return;
            }

            if (log.IsFatalEnabled)
            {
                if (level == LogLevel.Fatal)
                {
                    log.Fatal(message);
                    this.FillTextWriter(sw);
                }
            }
        }

        /// <summary>
        /// Logs the specified level.
        /// </summary>
        /// <param name="level">
        /// The level.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="t">
        /// The t.
        /// </param>
        /// <param name="sw">
        /// The sw.
        /// </param>
        /// <remarks>
        /// </remarks>
        public override void Log(LogLevel level, object message, Exception t, StringWriter sw)
        {
            // It is VERY Important that the log are 
            // in the very same order that appear on log4net
            if (log.IsDebugEnabled)
            {
                if (level == LogLevel.Debug)
                {
                    log.Debug(message, t);
                    this.FillTextWriter(sw);

                    // no need to go on
                    return;
                }
            }
            else
            {
                // No need to test others: if the lower level is false other will be false
                return;
            }

            if (log.IsInfoEnabled)
            {
                if (level == LogLevel.Info)
                {
                    log.Info(message, t);
                    this.FillTextWriter(sw);

                    // no need to go on
                    return;
                }
            }
            else
            {
                // No need to test others: if the lower level is false other will be false
                return;
            }

            if (log.IsWarnEnabled)
            {
                if (level == LogLevel.Warn)
                {
                    log.Warn(message, t);
                    this.FillTextWriter(sw);

                    // no need to go on
                    return;
                }
            }
            else
            {
                // No need to test others: if the lower level is false other will be false
                return;
            }

            if (log.IsErrorEnabled)
            {
                if (level == LogLevel.Error)
                {
                    log.Error(message, t);
                    this.FillTextWriter(sw);

                    // no need to go on
                    return;
                }
            }
            else
            {
                // No need to test others: if the lower level is false other will be false
                return;
            }

            if (log.IsFatalEnabled)
            {
                if (level == LogLevel.Fatal)
                {
                    log.Fatal(message, t);
                    this.FillTextWriter(sw);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fills the text writer.
        /// </summary>
        /// <param name="sw">
        /// The sw.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void FillTextWriter(TextWriter sw)
        {
            if (ma != null)
            {
                var events = ma.GetEvents();
                var iLayout = (PatternLayout)ma.Layout;
                iLayout.Format(sw, events[events.GetUpperBound(0)]);
                ma.Clear();
            }
        }

        #endregion
    }
}