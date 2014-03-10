using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Appleseed.PortalTemplate.DTOs;
using System.Collections;

namespace Appleseed.PortalTemplate
{
    public interface IPortalTemplateRepository
    {
        /// <summary>
        /// Obtiene el portal junto con todas sus paginas, modulos, settings, etc.
        /// </summary>
        /// <param name="portalID">Identificador del portal que queremos buscar</param>
        /// <returns></returns>
        PortalsDTO GetPortal(int portalID);

        /// <summary>
        /// Obtiene un diccionario con los ids de los generalModuleDefinitions y sus desktop
        /// </summary>
        /// <returns></returns>
        Dictionary<Guid, string> GetDesktopSources();


        /// <summary>
        /// Obtiene el HTMLTextDTO relacionado con el modulo especificado
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        HtmlTextDTO GetHtmlTextDTO(int moduleId);

    }
}
