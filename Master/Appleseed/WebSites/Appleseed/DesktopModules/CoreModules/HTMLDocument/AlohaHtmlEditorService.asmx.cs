using Appleseed.Framework.Content.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Appleseed.DesktopModules.CoreModules.HTMLDocument
{
    /// <summary>
    /// Summary description for AlohaHtmlEditorService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class AlohaHtmlEditorService : System.Web.Services.WebService
    {
        /// <summary>
        /// Method is used to Save/Update data to Database
        /// </summary>
        /// <param name="moduleid">Contain the ModuleID</param>
        /// <param name="data">Contain the Encoded data</param>
        [WebMethod]
        public void UpdateHtmlData(string moduleid, string data)
        {
            HtmlTextDB text = new HtmlTextDB();
            HtmlTextDB saveText = new HtmlTextDB();

            // Update the text within the HtmlText table
            int version = 0;
            bool published = true;
            string mobileSummary = string.Empty;
            string mobileDetails = string.Empty;
            SqlDataReader drList = text.GetHtmlTextRecord(Convert.ToInt32(moduleid));
            if (drList.HasRows)
            {
                while (drList.Read())
                {
                    if (Convert.ToBoolean(drList["Published"]))
                    {
                        version = Convert.ToInt32(drList["VersionNo"]);
                        published = Convert.ToBoolean(drList["Published"]);
                        mobileSummary = drList["MobileSummary"].ToString();
                        mobileDetails = drList["MobileDetails"].ToString();
                    }
                }
            }
            //Added by Ashish - Connection pool Issue
            if (drList != null)
            {
                drList.Close();
            }
            text.UpdateHtmlText(
                 Convert.ToInt32(moduleid),
               data,
               mobileSummary,
                mobileDetails,
               version,
                published,
                DateTime.Now,
                Appleseed.Framework.Site.Configuration.PortalSettings.CurrentUser.Identity.UserName,
                DateTime.Now,
                Appleseed.Framework.Site.Configuration.PortalSettings.CurrentUser.Identity.UserName
                );


        }
    }
}
