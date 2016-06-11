// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Evolutility_ModuleRenderer.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   Selected module will be loaded on page.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.DesktopModules.CoreModules.Evolutility_ModuleRenderer
{
    using System;
    using System.Collections;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Appleseed.Framework;
    using Appleseed.Framework.Content.Data;
    using Appleseed.Framework.Data;
    using Appleseed.Framework.DataTypes;
    using Appleseed.Framework.Helpers;
    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.Web.UI.WebControls;
    using History = Appleseed.Framework.History;

    /// <summary>
    /// Selected module render on page
    /// </summary>
    public partial class Evolutility_ModuleRenderer : PortalModuleControl
    {

        /// <summary>
        /// Set Module specific settings on initialization 
        /// </summary>
        public Evolutility_ModuleRenderer()
        {
            try
            {
                var group = SettingItemGroup.MODULE_SPECIAL_SETTINGS;

                var modules = new SettingItem<string, DropDownList>(new EvolutilityModuleRenderer())
                {
                    Order = (int)group + 1,
                    Group = group,
                    EnglishName = "Modules",
                    Description = "Select the Module"
                };

                this.BaseSettings.Add("Modules", modules);


                var dataSqlConnections = new SettingItem<string, TextBox>()
                {
                    Order = (int)group + 1,
                    Group = group,
                    EnglishName = "Data Connection",
                    Description = "Add data Connectionstring"
                };

                this.BaseSettings.Add("DataConnection", dataSqlConnections);


                var discoSqlConnections = new SettingItem<string, TextBox>()
                {
                    Order = (int)group + 1,
                    Group = group,
                    EnglishName = "Evol.Disco Connection",
                    Description = "Add Disco Connection string"
                };

                this.BaseSettings.Add("Evol.Disco.Connection", discoSqlConnections);

            }
            catch { }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.Settings.ContainsKey("Modules"))
                {
                    this.evoModuleRenderer.XMLfile = this.Settings["Modules"].Value.ToString();
                }

                if (this.Settings.ContainsKey("DataConnection") && !string.IsNullOrEmpty(this.Settings["DataConnection"].Value.ToString()))
                {
                    this.evoModuleRenderer.SqlConnection = this.Settings["DataConnection"].Value.ToString();
                }

                if (this.Settings.ContainsKey("Evol.Disco.Connection") && !string.IsNullOrEmpty(this.Settings["Evol.Disco.Connection"].Value.ToString()))
                {
                    this.evoModuleRenderer.SqlConnectionDico = this.Settings["Evol.Disco.Connection"].Value.ToString();
                }
            }
            catch { }
        }

        /// <summary>
        /// GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{3E9629AE-DBEA-4AF7-B929-076359B929F0}"); }
        }
    }
}