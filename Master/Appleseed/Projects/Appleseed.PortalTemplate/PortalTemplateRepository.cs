using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Appleseed.PortalTemplate.DTOs;
using System.Configuration;

namespace Appleseed.PortalTemplate
{
    public class PortalTemplateRepository: IPortalTemplateRepository
    {

        PortalTemplateDataContext db;

        public PortalTemplateRepository()
        {
            db = new PortalTemplateDataContext(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        }

        public PortalsDTO GetPortal(int portalID)
        {
            Translate _translate = new Translate();
            IList<rb_Portals> list=(from p in db.rb_Portals where p.PortalID == portalID select p).ToList<rb_Portals>();
            _translate.DesktopSources = this.GetDesktopSources();

            return list.Count == 0 ? null : _translate.TranslateRb_PortalsIntoPortalDTO(list[0]);
        }

        public Dictionary<Guid,string> GetDesktopSources()
        {
            return db.rb_GeneralModuleDefinitions.Select(g => new { g.GeneralModDefID, g.DesktopSrc }).AsEnumerable().
                                            ToDictionary(kvp => kvp.GeneralModDefID, kvp => kvp.DesktopSrc);

        }

        public HtmlTextDTO GetHtmlTextDTO(int moduleId)
        {
            Translate _translate = new Translate();
            IList<rb_HtmlText> list = (from h in db.rb_HtmlTexts where h.ModuleID == moduleId select h).ToList<rb_HtmlText>();
            return list.Count == 0 ? null : _translate.TranslateRb_HtmlTextIntoHtmlTextDTO(list[0]); 
        }

    }
}
