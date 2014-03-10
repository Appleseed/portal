using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Appleseed.PortalTemplate.DTOs
{
    [Serializable]
    public class PortalsDTO
    {
        public int PortalID
        {
            get;
            set;
        }

        public string PortalAlias
        {
            get;
            set;
        }

        public string PortalName
        {
            get;
            set;
        }

        public string PortalPath
        {
            get;
            set;
        }

        public bool AlwaysShowEditButton
        {
            get;
            set;
        }

        public List<PortalSettingsDTO> PortalSettings
        {
            get;
            set;
        }

        public List<PagesDTO> Pages
        {
            get;
            set;
        }
    }
}
