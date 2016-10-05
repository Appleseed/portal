namespace Appleseed.DesktopModules.CoreModules.Evolutility_ModuleList
{
    using Appleseed.Framework.Web.UI.WebControls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Evolutility.SideBar;
    using Evolutility.ExportWizard;
    using Evolutility.DataServer;
    using LinkButton = Appleseed.Framework.Web.UI.WebControls.LinkButton;
    using Framework;
    using Framework.DataTypes;

    /// <summary>
    /// Mudules listing page
    /// </summary>
    public partial class Evolutility_ModuleList : PortalModuleControl
    {
        public Evolutility_ModuleList()
        {
            var group = SettingItemGroup.BUTTON_DISPLAY_SETTINGS;
            var groupOrderBase = (int)SettingItemGroup.BUTTON_DISPLAY_SETTINGS;

            var dataSqlConnections = new SettingItem<string, TextBox>(new BaseDataType<string, TextBox>())
            {
                Order = (int)groupOrderBase + 1,
                Group = group,
                EnglishName = "Data Connection",
                Description = "Add data Connectionstring"
            };

            this.BaseSettings.Add("DataConnection", dataSqlConnections);

            var discoSqlConnections = new SettingItem<string, TextBox>(new BaseDataType<string, TextBox>())
            {
                Order = (int)groupOrderBase + 1,
                Group = group,
                EnglishName = "Evol.Disco Connection",
                Description = "Add Disco Connection string"
            };

            this.BaseSettings.Add("Evol.Disco.Connection", discoSqlConnections);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            hdnModuleID.Value = this.ModuleID.ToString();
            WizardBasic.HRef = "/DesktopModules/CoreModules/Evolutility_Wizard/Evolutility_Wizard.aspx?mid=" + this.ModuleID;
            WizardBasic.Attributes.Add("onclick", "openInModal('/DesktopModules/CoreModules/Evolutility_Wizard/Evolutility_Wizard.aspx?mid=" + this.ModuleID + "');");
            DBMapper.HRef = "/DesktopModules/CoreModules/Evolutility_Wizard/Evolutility_Wizard.aspx?mid=" + this.ModuleID + "&WIZ=dbscan";
            DBMapper.Attributes.Add("onclick", "openInModal('/DesktopModules/CoreModules/Evolutility_Wizard/Evolutility_Wizard.aspx?mid=" + this.ModuleID + "&WIZ=dbscan');");
            ImportXML.HRef = "/DesktopModules/CoreModules/Evolutility_Wizard/Evolutility_Wizard.aspx?mid=" + this.ModuleID + "&WIZ=xml2db";
            ImportXML.Attributes.Add("onclick", "openInModal('/DesktopModules/CoreModules/Evolutility_Wizard/Evolutility_Wizard.aspx?mid=" + this.ModuleID + "&WIZ=xml2db');");

            if (this.Settings.ContainsKey("DataConnection") && this.Settings["DataConnection"].Value != null && !string.IsNullOrEmpty(this.Settings["DataConnection"].Value.ToString()))
            {
                this.evoModuleList.SqlConnection = this.Settings["DataConnection"].Value.ToString();
            }

            if (this.Settings.ContainsKey("Evol.Disco.Connection") && this.Settings["Evol.Disco.Connection"].Value != null && !string.IsNullOrEmpty(this.Settings["Evol.Disco.Connection"].Value.ToString()))
            {
                this.evoModuleList.SqlConnectionDico = this.Settings["Evol.Disco.Connection"].Value.ToString();
            }
        }

        /// <summary>
        /// GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{8230D43A-7C14-4ED8-8429-6F0A60730C9D}"); }
        }
    }
}