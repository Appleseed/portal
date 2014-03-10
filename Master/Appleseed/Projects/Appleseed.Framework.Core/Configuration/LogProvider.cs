// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogProvider.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Summary description for LogProvider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Logging
{
    using System;
    using System.Configuration;
    using System.Configuration.Provider;
    using System.IO;
    using System.Web;
    using System.Xml;

    using Appleseed.Framework.Provider;

    /// <summary>
    /// Summary description for LogProvider.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public abstract class LogProvider : ProviderBase
    {
        #region Constants and Fields

        /// <summary>
        ///   Camel case. Must match web.config section name
        /// </summary>
        private const string ProviderType = "log";

        #endregion

        #region Public Methods

        /// <summary>
        /// Instances this instance.
        /// </summary>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static LogProvider Instance()
        {
            // Use the cache because the reflection used later is expensive
            var cache = HttpRuntime.Cache;

            // Get the names of providers
            var config = ProviderConfiguration.GetProviderConfiguration(ProviderType);

            // If config not found (missing web.config)
            if (config == null)
            {
                // Try to provide a default anyway
                var defaultNode = new XmlDocument();
                defaultNode.LoadXml(
                    "<log defaultProvider=\"Log4NetLog\"><providers><clear /><add name=\"Log4NetLog\" type=\"Appleseed.Framework.Logging.Log4NetLogProvider, Appleseed.Provider.Implementation\" /></providers></log>");

                // Get the names of providers
                config = new ProviderConfiguration();
                config.LoadValuesFromConfigurationXml(defaultNode.DocumentElement);
            }

            // Read specific configuration information for this provider
            var providerSettings = (ProviderSettings)config.Providers[config.DefaultProvider];

            // In the cache?
            var cacheKey = "Appleseed::Configuration::Log::" + config.DefaultProvider;
            if (cache[cacheKey] == null)
            {
                // The assembly should be in \bin or GAC, so we simply need
                // to get an instance of the type
                try
                {
                    cache.Insert(cacheKey, ProviderHelper.InstantiateProvider(providerSettings, typeof(LogProvider)));
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to load provider", e);
                }
            }

            return (LogProvider)cache[cacheKey];
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
        public abstract void Log(LogLevel level, object message);

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
        public abstract void Log(LogLevel level, object message, Exception t);

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
        public abstract void Log(LogLevel level, object message, StringWriter sw);

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
        public abstract void Log(LogLevel level, object message, Exception t, StringWriter sw);

        #endregion
    }
}