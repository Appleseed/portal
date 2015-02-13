using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Appleseed.Framework.Web
{
    [History("Ashish.patel@haptix.biz", "2014/12/24", "Changed Accessibility of GetUrlElements for caching")]
    public class UrlElements
    {
        internal bool IsPlaceHolder { get; set; }
        internal string TabLink { get; set; }
        internal string UrlKeywords { get; set; }
        internal string PageName { get; set; }
        internal string PageTitle { get; set; }

        //Ashish.patel@haptix.biz - 2014/12/24 - Changed Accessibility for caching
        public UrlElements()
        {
            IsPlaceHolder = false;
            TabLink = string.Empty;
            UrlKeywords = string.Empty;
            PageName = string.Empty;
            PageTitle = string.Empty;
        }
    }
}
