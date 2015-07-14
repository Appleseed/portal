using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Appleseed.Framework.Configuration.Items
{
    /// <summary>
    /// Current module defination
    /// </summary>
    public class CurrentModuleDefination
    {
        /// <summary>
        /// Friendly name
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Desktop Src
        /// </summary>
        public string DesktopSrc { get; set; }

        /// <summary>
        /// Mobile Src
        /// </summary>
        public string MobileSrc { get; set; }

        /// <summary>
        ///is  Admin
        /// </summary>
        public bool Admin { get; set; }

        /// <summary>
        /// Module Definition id
        /// </summary>
        public int ModuleDefId { get; set; }

        /// <summary>
        /// General module def id
        /// </summary>
        public Guid GeneralModDefID { get; set; }

        /// <summary>
        /// ModuleID
        /// </summary>
        public int ModuleID { get; set; }
    }
}
