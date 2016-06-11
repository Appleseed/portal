namespace Appleseed.DesktopModules.CoreModules.Evolutility_ModuleList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Evolutility;

    /// <summary>
    /// Display details of selected control
    /// </summary>
    public partial class EvolutilityFormTest : System.Web.UI.Page
    {
        //Pass ModuleID and display the list of records.
        protected void Page_Load(object sender, EventArgs e)
        {
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