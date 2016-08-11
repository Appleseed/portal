namespace Appleseed.DesktopModules.CoreModules.Evolutility_ModuleList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Evolutility;
    using Framework.Site.Configuration;
    /// <summary>
    /// Display details of selected control
    /// </summary>
    public partial class EvolutilityFormTest : System.Web.UI.Page
    {
        //Pass ModuleID and display the list of records.
        protected void Page_Load(object sender, EventArgs e)
        {
            var Settings = ModuleSettings.GetModuleSettings(Convert.ToInt32(Request.QueryString["mid"]));
            if (Settings.ContainsKey("DataConnection") && Settings["DataConnection"].Value != null && !string.IsNullOrEmpty(Settings["DataConnection"].Value.ToString()))
            {
                this.EvoFormTest.SqlConnection = Settings["DataConnection"].Value.ToString();
            }

            if (Settings.ContainsKey("Evol.Disco.Connection") && Settings["Evol.Disco.Connection"].Value != null && !string.IsNullOrEmpty(Settings["Evol.Disco.Connection"].Value.ToString()))
            {
                this.EvoFormTest.SqlConnectionDico = Settings["Evol.Disco.Connection"].Value.ToString();
            }

            string formid;

            formid = Request["formID"];
            if (!String.IsNullOrEmpty(formid))
                EvoFormTest.XMLfile = formid;
            else
            {
                EvoFormTest.XMLfile = "/aspnet_client/evolutility/xml/evoDico_Form.xml";
                EvoFormTest.ShowTitle = false;
                EvoFormTest.ToolbarPosition = UIServer.EvolToolbarPosition.None;
            }
        }
    }
}