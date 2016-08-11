namespace Appleseed.DesktopModules.CoreModules.Evolutility_Wizard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Evolutility;
    using Framework.Site.Configuration;
    public partial class EvoDicoTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var Settings = ModuleSettings.GetModuleSettings(Convert.ToInt32(Request.QueryString["mid"]));
            if (Settings.ContainsKey("DataConnection") && Settings["DataConnection"].Value != null && !string.IsNullOrEmpty(Settings["DataConnection"].Value.ToString()))
            {
                this.EvoTest.SqlConnection = Settings["DataConnection"].Value.ToString();
            }

            if (Settings.ContainsKey("Evol.Disco.Connection") && Settings["Evol.Disco.Connection"].Value != null && !string.IsNullOrEmpty(Settings["Evol.Disco.Connection"].Value.ToString()))
            {
                this.EvoTest.SqlConnectionDico = Settings["Evol.Disco.Connection"].Value.ToString();
            }

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