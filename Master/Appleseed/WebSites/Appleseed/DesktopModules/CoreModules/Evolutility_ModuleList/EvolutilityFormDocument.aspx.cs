namespace Appleseed.DesktopModules.CoreModules.Evolutility_ModuleList
{
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
            //Check request and set path the
            if (!String.IsNullOrEmpty(Request["md"]) && Request["md"].ToString() == "fld")
            {
                docTitle.InnerHtml = "Design document: fields";
                evoFormDocument.XMLfile = "/aspnet_client/evolutility/xml/evoDoc_Field.xml";
            }
        }
    }
}