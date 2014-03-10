// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProviderConfiguration.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The provider configuration.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Provider
{
    using System.Collections;
    using System.Configuration;
    using System.Linq;
    using System.Xml;
    using System.Collections.Specialized;
    using Appleseed.Framework.Configuration.Provider;

    /// <summary>
    /// The provider configuration.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class ProviderConfiguration
    {
        #region Constants and Fields

        /// <summary>
        ///   The providers.
        /// </summary>
        private readonly Hashtable providers = new Hashtable();

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the default provider
        /// </summary>
        /// <value>The default provider.</value>
        /// <remarks>
        /// </remarks>
        public string DefaultProvider { get; private set; }

        /// <summary>
        ///   Gets the loaded providers
        /// </summary>
        /// <value>The providers.</value>
        /// <remarks>
        /// </remarks>
        public Hashtable Providers
        {
            get
            {
                return this.providers;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the configuration object for the specified provider
        /// </summary>
        /// <param name="provider">
        /// Name of the provider object to retrieve
        /// </param>
        /// <returns>
        /// The Provider Configuration.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static ProviderConfiguration GetProviderConfiguration(string provider)
        {
            return (ProviderConfiguration)ConfigurationManager.GetSection(string.Format("providers/{0}", provider));
        }

        /// <summary>
        /// Loads provider information from the configuration node
        /// </summary>
        /// <param name="node">
        /// Node representing configuration information
        /// </param>
        /// <remarks>
        /// </remarks>
        public void LoadValuesFromConfigurationXml(XmlNode node)
        {
            var attributeCollection = node.Attributes;

            // Get the default provider
            if (attributeCollection != null)
            {
                this.DefaultProvider = attributeCollection["defaultProvider"].Value;
            }

            // Read child nodes
            foreach (var child in node.ChildNodes.Cast<XmlNode>().Where(child => child.Name == "providers"))
            {
                this.GetProviders(child);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Configures Provider(s) based on the configuration node
        /// </summary>
        /// <param name="node">
        /// The configuration node.
        /// </param>
        /// <remarks>
        /// </remarks>
        private void GetProviders(XmlNode node)
        {
            foreach (XmlNode provider in node.ChildNodes)
            {
                if (provider.Attributes == null)
                {
                    continue;
                }

                var attrName = provider.Attributes["name"];
                switch (provider.Name)
                {
                    case "add":
                        if (attrName.Value.Equals("SqlUrlBuilder")) {
                            NameValueCollection col = new NameValueCollection();
                            for (int i = 0; i < provider.Attributes.Count; i++) {
                                col.Add(provider.Attributes.Item(i).Name, provider.Attributes.Item(i).Value);
                            }
                            
                            this.providers.Add(attrName.Value, new AppleseedProviderSettings(attrName.Value, provider.Attributes["type"].Value,col));
                        } else
                            this.providers.Add(attrName.Value, new ProviderSettings(attrName.Value, provider.Attributes["type"].Value));
                        break;

                    case "remove":
                        this.providers.Remove(attrName.Value);
                        break;

                    case "clear":
                        this.providers.Clear();
                        break;
                }
            }
        }

        #endregion
    }
}