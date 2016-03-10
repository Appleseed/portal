using Appleseed.Framework.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Appleseed.DesktopModules.CoreModules.Admin
{
    public partial class AdminLeftMenu : PortalModuleControl
    {

        /// <summary>
        /// GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{504F5B7F-400D-46D4-887A-B03479441E04}"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}