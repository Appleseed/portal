using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Appleseed.PortalTemplate.DTOs
{
    [Serializable]
    public class ModuleSettingsDTO
    {
        public int ModuleID
        {
            get;
            set;
        }

        public string SettingName
        {
            get;
            set;
        }

        public string SettingValue
        {
            get;
            set;
        }

    }
}
