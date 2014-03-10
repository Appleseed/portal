using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Appleseed.PortalTemplate.DTOs
{
    [Serializable]
    public class HtmlTextDTO
    {
        public int ModuleID
        {
            get;
            set;
        }

        public string DesktopHtml
        {
            get;
            set;
        }

        public string MobileSummary
        {
            get;
            set;
        }

        public string MobileDetails
        {
            get;
            set;
        }
    }
}
