// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SliderModuleRenderer.cs">
//   Copyright © -- 2015. All Rights Reserved.
// </copyright>
// <summary>
//   Dynemic Sliders renderer
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Appleseed.DesktopModules.CoreModules.SliderRenderer
{
    using Appleseed.Framework;
    using Appleseed.Framework.Site.Data;
    using Appleseed.Framework.Web.UI.WebControls;
    using System;
    using System.Text;
    using System.Web.UI.HtmlControls;

    /// <summary>
    /// Render Slider Module
    /// </summary>
    [History("Ashish.patel@haptix.biz", "2015/02/10", "Dynamic Slider Renderer")]
    public partial class SliderModuleRenderer : PortalModuleControl
    {

        /// <summary>
        /// GUID of module (mandatory)
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{A8423224-230A-4561-AED0-1DF41850981B}"); }
        }

        /// <summary>
        /// Page Load Generates Html for slider
        /// </summary>
        /// <param name="sender"> The source of event</param>
        /// <param name="e"> The <see cref="System.EventArgs"/> instance containing event data. </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isSliderloaded = false;
            foreach (object itemobj in this.Page.Header.Controls)
            {
                if (itemobj is HtmlGenericControl)
                {
                    HtmlGenericControl item = (HtmlGenericControl)itemobj;
                    if (item.Attributes["SliderCss"] != null && item.Attributes["SliderCss"].ToString() == "yes")
                    {
                        isSliderloaded = true;
                        break;
                    }
                }
            }

            if (!isSliderloaded)
            {
                var IncludeSliderCSS = new HtmlGenericControl("link");
                IncludeSliderCSS.Attributes.Add("type", "text/css");
                IncludeSliderCSS.Attributes.Add("rel", "stylesheet");
                IncludeSliderCSS.Attributes.Add("SliderCss", "yes");
                IncludeSliderCSS.Attributes.Add("href", "/aspnet_client/FlexSlider/css/flexslider.css");
                this.Page.Header.Controls.Add(IncludeSliderCSS);
                this.plcLoadScripts.Visible = true;
            }

            SliderDB sldDB = new SliderDB();
            StringBuilder sbGenereateHtml = new StringBuilder();
            int sliderID = 1;
            if (this.Settings.ContainsKey("Sliders") && this.Settings["Sliders"].Value != null && !string.IsNullOrEmpty(this.Settings["Sliders"].Value.ToString()))
            {
                sliderID = Convert.ToInt32(this.Settings["Sliders"].Value.ToString());
            }

            foreach (var item in sldDB.SliderImages(sliderID, true))
            {
                sbGenereateHtml.AppendLine("<li>");
                sbGenereateHtml.AppendLine("<img src=\"/images/sliders/" + item.SliderID + "/" + item.SliderImageID + item.SliderImageExt + " \"/>");
                if (!string.IsNullOrEmpty(item.SliderCaption))
                {
                    sbGenereateHtml.AppendLine("<p class=\"flex-caption\">" + item.SliderCaption + "</p>");
                }
                sbGenereateHtml.AppendLine("</li>");
            }
            ltrSliderLi.Text = sbGenereateHtml.ToString();
        }

        //public override bool EnableSlider
        //{
        //    get
        //    {
        //        return true;
        //    }
        //}
    }
}