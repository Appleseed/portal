namespace Appleseed.Framework.Configuration.Provider
{
    using System.Configuration;
    using System.Collections.Specialized;

    /// <summary>
    /// Appleseed provider settings
    /// </summary>
    public class AppleseedProviderSettings : ConfigurationElement
    {

        private string name;
        private string type;
        private NameValueCollection parameters;

        // Summary:
        //     Initializes a new instance of the System.Configuration.ProviderSettings class.
        /// <summary>
        /// Appleseed privder settings
        /// </summary>
        public AppleseedProviderSettings() {
            name = string.Empty;
            type = string.Empty;
            parameters = new NameValueCollection();
        }
        
        //
        // Summary:
        //     Initializes a new instance of the System.Configuration.ProviderSettings class.
        //
        // Parameters:
        //   name:
        //     The name of the provider to configure settings for.
        //
        //   type:
        //     The type of the provider to configure settings for.
        /// <summary>
        /// Appleseed provider settings
        /// </summary>
        /// <param name="Name">name</param>
        /// <param name="Type">type</param>
        public AppleseedProviderSettings(string Name, string Type) {
            name = Name;
            type = Type;
            parameters = new NameValueCollection();
        }

        //
        // Summary:
        //     Initializes a new instance of the System.Configuration.ProviderSettings class.
        //
        // Parameters:
        //   name:
        //     The name of the provider to configure settings for.
        //
        //   type:
        //     The type of the provider to configure settings for.
        //
        //  Parameters:
        //      The colecction from web.Config
        /// <summary>
        /// Appleseed provder settings
        /// </summary>
        /// <param name="Name">Name</param>
        /// <param name="Type">Type</param>
        /// <param name="Parameters">Parameters</param>
        public AppleseedProviderSettings(string Name, string Type, NameValueCollection Parameters) {
            name = Name;
            type = Type;
            parameters = Parameters;
        
        }

        // Summary:
        //     Gets or sets the name of the provider configured by this class.
        //
        // Returns:
        //     The name of the provider.
        /// <summary>
        /// Name
        /// </summary>
        [ConfigurationProperty("Name", IsRequired = true, IsKey = true)]
        public string Name { 
            get {
                return name;
            }
            set {
                Name = value;
            }
        }

        // Summary:
        //     Gets or sets the type of the provider configured by this class.
        //
        // Returns:
        //     The type of the provider.

        /// <summary>
        /// Type
        /// </summary>
        [ConfigurationProperty("Name", IsRequired = true, IsKey = true)]
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                Type = value;
            }
        }


        // Summary:
        //     Gets a collection of user-defined parameters for the provider.
        //
        // Returns:
        //     A System.Collections.Specialized.NameValueCollection of parameters for the
        //     provider.
        /// <summary>
        /// Parameters
        /// </summary>
        public NameValueCollection Parameters
        {
            get
            {
                return this.parameters;
            }
        }



        

    }
}
