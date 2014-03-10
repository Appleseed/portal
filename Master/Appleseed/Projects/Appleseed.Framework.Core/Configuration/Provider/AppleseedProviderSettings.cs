using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;

namespace Appleseed.Framework.Configuration.Provider
{
    public class AppleseedProviderSettings : ConfigurationElement
    {

        private string name;
        private string type;
        private NameValueCollection parameters;

        // Summary:
        //     Initializes a new instance of the System.Configuration.ProviderSettings class.
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
        public NameValueCollection Parameters
        {
            get
            {
                return this.parameters;
            }
        }



        

    }
}
