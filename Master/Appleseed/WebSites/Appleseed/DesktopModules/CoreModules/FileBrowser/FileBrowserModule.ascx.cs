using Appleseed.Framework.Security;
using Appleseed.Framework.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Appleseed.DesktopModules.CoreModules.FileBrowser
{
    public partial class FileBrowserModule : PortalModuleControl
    {
        /// <summary>
        /// Override on derivates class.
        /// Return true if the module is an Admin Module.
        /// </summary>
        /// <value></value>
        public override bool AdminModule
        {
            get { return true; }
        }

        /// <summary>
        /// GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{D7B8B22F-366B-4D80-9E49-13C09120A89F}"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}