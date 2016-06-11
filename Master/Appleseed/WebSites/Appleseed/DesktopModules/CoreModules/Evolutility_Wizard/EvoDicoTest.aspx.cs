namespace Appleseed.DesktopModules.CoreModules.Evolutility_Wizard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Evolutility;

    public partial class EvoDicoTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string formid;

            formid = Request["formID"];
            if (!String.IsNullOrEmpty(formid))
                EvoTest.XMLfile = formid;
            else
            {
                EvoTest.XMLfile = "/aspnet_client/evolutility/xml/evoDico_Form.xml";
                EvoTest.ShowTitle = false;
                EvoTest.ToolbarPosition = UIServer.EvolToolbarPosition.None;
            }
        }
    }
}