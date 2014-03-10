using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Appleseed.PortalTemplate.DTOs
{
    [Serializable]
    public class TabSettingsDTO
    {
        public int TabID
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
