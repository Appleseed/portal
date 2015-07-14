using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Appleseed.Framework.Web
{
    /// <summary>
    /// Url elements
    /// </summary>
    [History("Ashish.patel@haptix.biz", "2014/12/24", "Changed Accessibility of GetUrlElements for caching")]
    public class UrlElements
    {
        /// <summary>
        /// Is placeholder
        /// </summary>
        internal bool IsPlaceHolder { get; set; }

        /// <summary>
        /// tab link
        /// </summary>
        internal string TabLink { get; set; }

        /// <summary>
        /// Url keywords
        /// </summary>
        internal string UrlKeywords { get; set; }

        /// <summary>
        /// Page name
        /// </summary>
        internal string PageName { get; set; }

        /// <summary>
        /// Page title
        /// </summary>
        internal string PageTitle { get; set; }

        //Ashish.patel@haptix.biz - 2014/12/24 - Changed Accessibility for caching
        /// <summary>
        /// set value to properties
        /// </summary>
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
