namespace Appleseed.DesktopModules.CoreModules.EvolutilityAdvanced.ModuleRenderer
{
    using Appleseed.Framework;
    using Appleseed.Framework.Web.UI.WebControls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Use to render model on page
    /// </summary>
    [History("Ashish Patel","2014/09/06","Evolutility Advanced Module renderer control")]
    public partial class EvolutilityAdvanced_ModuleRenderer : PortalModuleControl
    {

        /// <summary>
        /// Set settings
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.EditUrl = "~/DesktopModules/CoreModules/EvolutilityAdvanced.ModuleRenderer/EvolAdvModSettings.aspx";
            base.OnInit(e);
        }

        /// <summary>
        /// Set the page to Iframe with PageID and ModuleID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // load page into iframe with pageID and ModuleID
                frmEvolModuleRenderer.Attributes["src"] = "/DesktopModules/CoreModules/EvolutilityAdvanced.ModuleRenderer/EvolutilityAdvancedModuleRendererPage.aspx?pageid=" + Request.QueryString["pageId"].ToString() + "&moduleid=" + this.ModuleID.ToString();
            }
            catch { }
        }

        /// <summary>
        /// GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            //EvolutilityAdvanced.ModuleRenderer GUID
            get { return new Guid("{7222060b-3fdb-466c-8ca8-6ac2c8328140}"); }
        }
    }
}