// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProviderHelper.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The provider helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Provider
{
    using System;
    using System.Configuration;
    using System.Configuration.Provider;
    using Appleseed.Framework.Configuration.Provider;

    /// <summary>
    /// The provider helper.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public static class ProviderHelper
    {
        #region Public Methods

        /// <summary>
        /// Instantiates the provider.
        /// </summary>
        /// <param name="providerSettings">
        /// The provider settings.
        /// </param>
        /// <param name="provType">
        /// Type of the prov.
        /// </param>
        /// <returns>
        /// A Provider Base.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static ProviderBase InstantiateProvider(ProviderSettings providerSettings, Type provType)
        {
            if (string.IsNullOrEmpty(providerSettings.Type))
            {
                throw new ConfigurationErrorsException(
                    "Provider could not be instantiated. The Type parameter cannot be null.");
            }

            var providerType = Type.GetType(providerSettings.Type);
            if (providerType == null)
            {
                throw new ConfigurationErrorsException(
                    "Provider could not be instantiated. The Type could not be found.");
            }

            if (!provType.IsAssignableFrom(providerType))
            {
                throw new ConfigurationErrorsException("Provider must implement type \'" + provType + "\'.");
            }

            var providerObj = Activator.CreateInstance(providerType);
            if (providerObj == null)
            {
                throw new ConfigurationErrorsException("Provider could not be instantiated.");
            }

            var providerBase = (ProviderBase)providerObj;

            providerBase.Initialize(providerSettings.Name, providerSettings.Parameters);
            return providerBase;
        }

        /// <summary>
        /// Instantiates the provider.
        /// </summary>
        /// <param name="providerSettings">
        /// The Appleseed provider settings.
        /// </param>
        /// <param name="provType">
        /// Type of the prov.
        /// </param>
        /// <returns>
        /// A Provider Base.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static ProviderBase InstantiateProvider(AppleseedProviderSettings providerSettings, Type provType)
        {
            if (string.IsNullOrEmpty(providerSettings.Type)) {
                throw new ConfigurationErrorsException(
                    "Provider could not be instantiated. The Type parameter cannot be null.");
            }

            var providerType = Type.GetType(providerSettings.Type);
            if (providerType == null) {
                throw new ConfigurationErrorsException(
                    "Provider could not be instantiated. The Type could not be found.");
            }

            if (!provType.IsAssignableFrom(providerType)) {
                throw new ConfigurationErrorsException("Provider must implement type \'" + provType + "\'.");
            }

            var providerObj = Activator.CreateInstance(providerType);
            if (providerObj == null) {
                throw new ConfigurationErrorsException("Provider could not be instantiated.");
            }

            var providerBase = (ProviderBase)providerObj;

            providerBase.Initialize(providerSettings.Name, providerSettings.Parameters);
            return providerBase;
        }

        /// <summary>
        /// Instantiates the providers.
        /// </summary>
        /// <param name="configProviders">
        /// The config providers.
        /// </param>
        /// <param name="providers">
        /// The providers.
        /// </param>
        /// <param name="provType">
        /// Type of the prov.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void InstantiateProviders(
            ProviderCollection configProviders, ref ProviderCollection providers, Type provType)
        {
            foreach (ProviderSettings providerSettings in configProviders)
            {
                providers.Add(InstantiateProvider(providerSettings, provType));
            }
        }

        #endregion
    }
}