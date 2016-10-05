using Appleseed.Framework.Security;
using Appleseed.Framework.Site.Data;
using Appleseed.Framework.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Appleseed.DesktopModules.CoreModules.AdminModuleInstances
{
    public partial class AdminModuleInstances : PortalModuleControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindModules();
                BindData();
            }
        }

        protected void ddlModules_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            PagesDB pages = new PagesDB();
            this.rptPages.DataSource = pages.GetPagesByModule(Convert.ToInt32(this.ddlModules.SelectedValue));
            this.rptPages.DataBind();
        }

        private void BindModules()
        {
            var asd = new PagesDB();

            // Populate the "Add Module" Data
            var m = new ModulesDB();
            var modules = new SortedList<string, string>();
            var drCurrentModuleDefinitions = m.GetCurrentModuleDefinitions(this.PortalSettings.PortalID);

            var htmlId = "0";
            try
            {
                foreach (var item in drCurrentModuleDefinitions)
                {
                    if ((!modules.ContainsKey(item.FriendlyName)) &&
                       (PortalSecurity.IsInRoles("Admins") || !item.Admin))
                    {
                        modules.Add(

                           item.FriendlyName,
                           item.ModuleDefId.ToString());
                        if (item.FriendlyName.ToString().Equals("HTML Content"))
                            htmlId = item.ModuleDefId.ToString();
                    }
                }
            }
            finally
            {

            }

            this.ddlModules.DataSource = modules;
            this.ddlModules.DataTextField = "key";
            this.ddlModules.DataValueField = "value";
            this.ddlModules.DataBind();
        }
    }
}