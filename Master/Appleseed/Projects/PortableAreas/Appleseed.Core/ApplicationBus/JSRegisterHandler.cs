using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcContrib.PortableAreas;
using System.Web.UI;

namespace Appleseed.Core.ApplicationBus
{
    public class JSRegisterHandler : MessageHandler<JSRegisterDescriptor>
    {
        public override void Handle(JSRegisterDescriptor message)
        {
        }
    }
}