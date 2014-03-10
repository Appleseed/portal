using System.Web;
using System.Web.UI.WebControls;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Site.Configuration;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// HeaderImage
    /// </summary>
    public class HeaderImage : Image
    {
        /// <summary>
        /// DataBind
        /// </summary>
        public override void DataBind()
        {
            if (HttpContext.Current != null)
            {
                // Obtain PortalSettings from Current Context
                PortalSettings PortalSettings = (PortalSettings) HttpContext.Current.Items["PortalSettings"];

                //PortalImage
                if (PortalSettings.CustomSettings["SITESETTINGS_LOGO"] != null &&
                    PortalSettings.CustomSettings["SITESETTINGS_LOGO"].ToString().Length != 0)
                {
                    ImageUrl =
                        Path.WebPathCombine(Path.ApplicationRoot, PortalSettings.PortalPath,
                                            PortalSettings.CustomSettings["SITESETTINGS_LOGO"].ToString());
                    // Added by Mario Endara to Reinforce portal Title for Search Engines <mario@softworks.com.uy>
                    if (PortalSettings.CustomSettings["SITESETTINGS_PAGE_TITLE"] != null &&
                        PortalSettings.CustomSettings["SITESETTINGS_PAGE_TITLE"].ToString().Length != 0)
                        AlternateText = PortalSettings.CustomSettings["SITESETTINGS_PAGE_TITLE"].ToString();
                    Visible = true;
                }
                else
                {
                    Visible = false;
                }
            }
            base.DataBind();
        }
    }
}