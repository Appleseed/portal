using Appleseed.Framework.Providers.AppleseedRoleProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appleseed.Framework.Configuration.Items
{
    /// <summary>
    /// Proxy item class for setting values
    /// </summary>
    public class ProxyItem
    {
        /// <summary>
        /// service id
        /// </summary>
        public int ServiceId { get; set; }

        /// <summary>
        /// service title
        /// </summary>
        public string ServiceTitle { get; set; }

        /// <summary>
        /// service url
        /// </summary>
        public string ServiceUrl { get; set; }

        /// <summary>
        /// forward headers or not
        /// </summary>
        public bool ForwardHeaders { get; set; }

        /// <summary>
        /// enable content access or not
        /// </summary>
        public bool EnabledContentAccess { get; set; }

        /// <summary>
        /// content access roles
        /// </summary>
        public string ContentAccessRoles { get; set; }

        /// <summary>
        /// All access roles into appleseed
        /// </summary>
        public IList<AppleseedRole> AllAccessRoles { get; set; }

        /// <summary>
        /// Appleseed proxy url
        /// </summary>
        public string ASProxyUrl { get; set; }
    }
}
