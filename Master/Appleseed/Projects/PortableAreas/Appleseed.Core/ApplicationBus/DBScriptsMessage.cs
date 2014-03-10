using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcContrib.PortableAreas;

namespace Appleseed.Core.ApplicationBus
{
    public class DBScriptsMessage : IEventMessage
    {
        public string AreaName { get; set; }
        public string LastVersion { get; set; }
        public List<DBScriptDescriptor> Scripts { get; set; }
    }
}