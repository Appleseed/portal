using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Appleseed.Framework.Web
{
    /// <summary>
    /// Page Url
    /// </summary>
    public class PageUrl
    {
        /// <summary>
        /// Page ID
        /// </summary>
        public int PageID { get; set; }

        /// <summary>
        /// Page Normal Url
        /// </summary>
        public string PageNormalUrl { get; set; }

        /// <summary>
        /// Page friendly Url
        /// </summary>
        public string PageFriendlyUrl { get; set; }
    }
}
