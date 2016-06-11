namespace Appleseed.DesktopModules.CoreModules.Evolutility_ModuleList
{
    using Appleseed.Framework.Web.UI.WebControls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Evolutility.SideBar;
    using Evolutility.ExportWizard;
    using Evolutility.DataServer;
    using LinkButton = Appleseed.Framework.Web.UI.WebControls.LinkButton;

    /// <summary>
    /// Mudules listing page
    /// </summary>
    public partial class Evolutility_ModuleList : PortalModuleControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{8230D43A-7C14-4ED8-8429-6F0A60730C9D}"); }
        }
    }
}