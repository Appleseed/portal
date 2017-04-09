using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Appleseed.Framework.Web.UI.WebControls;
using System.Web.Mvc;
//using Microsoft.Web.Mvc;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Appleseed.Framework;

public partial class DesktopModules_CoreModules_MVC_MVCModule : MVCModuleControl
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public DesktopModules_CoreModules_MVC_MVCModule()
    {

    }


    public override Guid GuidID
    {
        get
        {
            return new Guid("{9073EC6C-9E21-44ba-A33E-22F0E301B867}");
        }
    }


}
