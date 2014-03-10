using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcContrib.PortableAreas;

namespace Appleseed.Core.ApplicationBus
{
    public class BusMessage: IEventMessage
    {
        public string Message { get; set; }
    }
}