using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcContrib.PortableAreas;

namespace Appleseed.Core.ApplicationBus
{
    public class JSRegisterDescriptor : IEventMessage
    {
        public List<string> Scripts { get; set; }

    }
}