using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Appleseed.PortalTemplate.DTOs;
using System.IO;

namespace Appleseed.PortalTemplate
{
    public interface IPortalTemplateServices
    {
        bool SerializePortal(int portalID, string portalAlias, string portalFullPath);

        bool DeserializePortal(string file, string portalName, string portalAlias, string portalPath, string filePath, out int portalId);

        HtmlTextDTO GetHtmlTextDTO(int moduleId);

        bool SaveHtmlText(int moduleId, HtmlTextDTO html);

        List<string> GetTemplates(string portalAlias, string portalFullPath);

        void DeleteTemplate(string templateName, string portalFullPath);

        byte[] GetTemplate(string templateName, string portalFullPath);

        FileInfo GetTemplateInfo(string templateName, string p);

        int CopyPage(int id, string name);
    }
}
