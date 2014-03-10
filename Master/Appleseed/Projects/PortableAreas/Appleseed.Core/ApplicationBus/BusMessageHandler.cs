using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcContrib.PortableAreas;

namespace Appleseed.Core.ApplicationBus
{
    public class BusMessageHandler : MessageHandler<BusMessage>
    {

        public override void Handle(BusMessage message)
        {
            //Nothing to do right now, maybe logging events or something
        }
    }
}