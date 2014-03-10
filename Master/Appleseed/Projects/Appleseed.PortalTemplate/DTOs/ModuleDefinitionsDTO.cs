using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Appleseed.PortalTemplate.DTOs
{
    [Serializable]
    public class ModuleDefinitionsDTO
    {
        public int ModuleDefID
        {
            get;
            set;
        }

        public int PortalID
        {
            get;
            set;
        }

        public Guid GeneralModDefID
        {
            get;
            set;
        }
    }
}
