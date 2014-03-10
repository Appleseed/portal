using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

/// Agileworks execute scripts
namespace Appleseed.Core.ExecuteScripts
{
    public class ExecuteScriptsConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("executeScriptsCollection")]
        public ExecuteScriptsConfigurationCollection ExecuteScriptsCollection
        {
            get
            {
                return (ExecuteScriptsConfigurationCollection)this["executeScriptsCollection"];
            }
            set
            {
                this["executeScriptsCollection"] = value;
            }
        }

    }
}
