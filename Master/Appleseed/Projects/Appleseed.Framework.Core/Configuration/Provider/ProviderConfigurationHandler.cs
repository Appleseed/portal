// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProviderConfigurationHandler.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The provider configuration handler.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Provider
{
    using System.Configuration;
    using System.Xml;

    /// <summary>
    /// The provider configuration handler.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class ProviderConfigurationHandler : IConfigurationSectionHandler
    {
        #region Implemented Interfaces

        #region IConfigurationSectionHandler

        /// <summary>
        /// Creates the specified parent.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="node">
        /// The configuration node.
        /// </param>
        /// <returns>
        /// The create.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public virtual object Create(object parent, object context, XmlNode node)
        {
            var config = new ProviderConfiguration();
            config.LoadValuesFromConfigurationXml(node);
            return config;
        }

        #endregion

        #endregion
    }
}