using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Appleseed.PortalTemplate
{
    public class PortalTemplateFactory
    {
        
        public static IPortalTemplateServices GetPortalTemplateServices(IPortalTemplateRepository repository)
        {
            return new PortalTemplateServices(repository);
        }

    }
}
