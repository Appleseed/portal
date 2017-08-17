using Appleseed.PortalTemplate.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Appleseed.PortalTemplate
{
    public interface IModuleExportable
    {
        string GetContentData(int moduleId);

        bool SetContentData(int moduleId, HtmlTextDTO content);
    }
}
