namespace Appleseed.DesktopModules.CoreModules.Evolutility_Wizard
{
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
            if (!String.IsNullOrEmpty(Request["md"]) && Request["md"].ToString() == "fld")
            {
                docTitle.InnerHtml = "Design document: fields";
                evoDocument.XMLfile = "/aspnet_client/evolutility/xml/evoDoc_Field.xml";
            }
        }
    }
}