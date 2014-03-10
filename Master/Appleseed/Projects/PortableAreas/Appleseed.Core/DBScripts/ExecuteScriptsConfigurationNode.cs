using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

/// Agileworks execute scripts
namespace Appleseed.Core.ExecuteScripts
{
    public class ExecuteScriptsConfigurationNode : ConfigurationElement
    {

        [ConfigurationProperty("areaName", IsRequired = true)]
        //[StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 60)]
        public String AreaName
        {
            get
            {
                return (String)this["areaName"];
            }
            set
            {
                this["areaName"] = value;
            }
        }

        [ConfigurationProperty("scriptsPath", IsRequired = true)]
        //[StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 60)]
        public String ScriptsPath
        {
            get
            {
                return (String)this["scriptsPath"];
            }
            set
            {
                this["scriptsPath"] = value;
            }
        }

        [ConfigurationProperty("connectionString", IsRequired = true)]
        public String ConnectionString
        {
            get
            {
                return (String)this["connectionString"];
            }
            set
            {
                this["connectionString"] = value;
            }
        }
    }
}
