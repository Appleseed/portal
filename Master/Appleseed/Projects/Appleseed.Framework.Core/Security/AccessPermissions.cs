using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Appleseed.Framework.Security
{
    /// <summary>
    /// Access Permission Enum
    /// </summary>
    public enum AccessPermissions
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Portal administration
        /// </summary>
        PORTAL_ADMINISTRATION = 1,

        /// <summary>
        /// portal theme and layout administration
        /// </summary>
        PORTAL_THEME_AND_LAYOUT_ADMINISTRATION = 2,

        /// <summary>
        /// Page list
        /// </summary>
        PAGE_LIST = 3,

        /// <summary>
        /// Page creation
        /// </summary>
        PAGE_CREATION = 4,

        /// <summary>
        /// Page Editing
        /// </summary>
        PAGE_EDITING = 5,

        /// <summary>
        /// Page Deletion
        /// </summary>
        PAGE_DELETION = 6,

        /// <summary>
        /// Module Creation
        /// </summary>
        MODULE_CREATION = 7,

        /// <summary>
        /// Module delettion
        /// </summary>
        MODULE_DELETION = 8,

        /// <summary>
        /// Module edition
        /// </summary>
        MODULE_EDITING = 9,

        /// <summary>
        /// Module Html content edition
        /// </summary>
        MODULE_HTML_CONTENT_EDITING = 10
    }
}
