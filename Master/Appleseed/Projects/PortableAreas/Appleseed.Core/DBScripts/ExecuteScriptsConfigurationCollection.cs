using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

/// Agileworks execute scripts
namespace Appleseed.Core.ExecuteScripts
{
    public class ExecuteScriptsConfigurationCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ExecuteScriptsConfigurationNode();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExecuteScriptsConfigurationNode)element).AreaName;
        }
    }
}
