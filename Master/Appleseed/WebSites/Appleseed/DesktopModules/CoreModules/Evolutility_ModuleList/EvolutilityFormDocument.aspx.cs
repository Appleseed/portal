namespace Appleseed.DesktopModules.CoreModules.Evolutility_ModuleList
{
    using Framework.Site.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Design document for selected control
    /// </summary>
    public partial class EvolutilityFormDocument : System.Web.UI.Page
    {
       //Set evoDoc_Field.xml path to cotrol
        protected void Page_Load(object sender, EventArgs e)
        {
            var Settings = ModuleSettings.GetModuleSettings(Convert.ToInt32(Request.QueryString["mid"]));
            if (Settings.ContainsKey("DataConnection") && Settings["DataConnection"].Value != null && !string.IsNullOrEmpty(Settings["DataConnection"].Value.ToString()))
            {
                this.evoFormDocument.SqlConnection = Settings["DataConnection"].Value.ToString();
            }

            if (Settings.ContainsKey("Evol.Disco.Connection") && Settings["Evol.Disco.Connection"].Value != null && !string.IsNullOrEmpty(Settings["Evol.Disco.Connection"].Value.ToString()))
            {
                this.evoFormDocument.SqlConnectionDico = Settings["Evol.Disco.Connection"].Value.ToString();
            }

            //Check request and set path the
            if (!String.IsNullOrEmpty(Request["md"]) && Request["md"].ToString() == "fld")
            {
                docTitle.InnerHtml = "Design document: fields";
                evoFormDocument.XMLfile = "/aspnet_client/evolutility/xml/evoDoc_Field.xml";
            }
        }
    }
}