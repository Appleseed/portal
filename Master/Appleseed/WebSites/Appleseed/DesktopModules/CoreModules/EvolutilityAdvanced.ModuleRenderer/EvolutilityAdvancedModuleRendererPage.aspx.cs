namespace Appleseed.DesktopModules.CoreModules.EvolutilityAdvanced.ModuleRenderer
{
    using Appleseed.Framework;
    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.Site.Data;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// This page is used to render the Evolutility Advanced module
    /// </summary>
    [History("Ashish Patel", "2014/09/06", "Evolutility Advanced Module renderer page")]
    public partial class EvolutilityAdvancedModuleRendererPage : System.Web.UI.Page
    {
        /// <summary>
        /// Module renderer control render on page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            int mid = Convert.ToInt32( Request.QueryString["moduleid"]);
            this.Settings = ModuleSettings.GetModuleSettings(mid);
            getModelSettings();
        }

        public Dictionary<string, ISettingItem> Settings { get; set; }

        public string ModelID { get; set; }
        public string ModelLabel { get; set; }
        public string ModelEntity { get; set; }
        public string ModelEntities { get; set; }
        public string ModelLeadField { get; set; }
        public string ModelElements { get; set; }

       /// <summary>
       /// Get model settings from Database
       /// </summary>
        public void getModelSettings()
        {
            EvolutilityModuleDB evolSettings = new EvolutilityModuleDB();
            SqlDataReader reader = evolSettings.EvolutilityAdvGetModelSettings(Convert.ToInt32(Request.QueryString["moduleid"]));
            if (reader.Read())
            {
                this.ModelID = reader["ModelID"].ToString();
                this.ModelLabel = reader["ModelLabel"].ToString();
                this.ModelEntity = reader["ModelEntity"].ToString();
                this.ModelEntities = reader["ModelEntities"].ToString();
                this.ModelLeadField = reader["ModelLeadField"].ToString();
                this.ModelElements = reader["ModelElements"].ToString();
            }
            reader.Close();

        }
    }
}