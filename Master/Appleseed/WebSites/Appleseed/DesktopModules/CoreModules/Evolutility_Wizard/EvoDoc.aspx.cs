namespace Appleseed.DesktopModules.CoreModules.Evolutility_Wizard
{
    using Framework.Site.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class EvoDoc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var Settings = ModuleSettings.GetModuleSettings(Convert.ToInt32(Request.QueryString["mid"]));
            if (Settings.ContainsKey("DataConnection") && Settings["DataConnection"].Value != null && !string.IsNullOrEmpty(Settings["DataConnection"].Value.ToString()))
            {
                this.evoDocument.SqlConnection = Settings["DataConnection"].Value.ToString();
            }

            if (Settings.ContainsKey("Evol.Disco.Connection") && Settings["Evol.Disco.Connection"].Value != null && !string.IsNullOrEmpty(Settings["Evol.Disco.Connection"].Value.ToString()))
            {
                this.evoDocument.SqlConnectionDico = Settings["Evol.Disco.Connection"].Value.ToString();
            }


            if (!String.IsNullOrEmpty(Request["md"]) && Request["md"].ToString() == "fld")
            {
                docTitle.InnerHtml = "Design document: fields";
                evoDocument.XMLfile = "/aspnet_client/evolutility/xml/evoDoc_Field.xml";
            }
        }
    }
}