// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UrlRewritingFriendlyUrl.cs">
//   Copyright © -- 2014. All Rights Reserved.
// </copyright>
// <summary>
//   Page Friendly URL
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Appleseed.Framework.UrlRewriting
{
    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.Site.Data;
    using System;
    using System.Data;
    using System.Web;
    using Appleseed.Framework.Settings;

    /// <summary>
    /// UrlRewriting Friendly URL
    /// </summary>
    public class UrlRewritingFriendlyUrl
    {
        /// <summary>
        /// Get pageId from pagename
        /// </summary>
        /// <returns>PageID</returns>
        public static string GetPageIDFromPageName(string pagepath)
        {
            PagesDB DB = new PagesDB();
            var settings = PortalSettings.GetPortalSettingsbyPageID(Portal.PageID, Config.DefaultPortal);

            int portalID = 0;
            if (settings != null)
            {
                portalID = settings.PortalID;
            }

            DataTable dtPages = DB.GetPagesFlatTable(portalID);

            //When friendly URl applied and go to Home page from and sub pages 
            if (pagepath.ToLower().Contains("default.aspx"))
            {
                string page_ID = "1";
                return page_ID;
            }

            // Check requested page url contains the /site when friendly URL is on
            var handlerFlag = System.Configuration.ConfigurationManager.AppSettings["handlerFlag"];
            if (pagepath.Contains("/" + handlerFlag))
            {
                string[] splitpaths = pagepath.Split('/');
                int index = Array.IndexOf(splitpaths, handlerFlag);
                int requesetedPageId = Convert.ToInt32(splitpaths[index + 1]);
                pagepath = HttpUrlBuilder.BuildUrl(requesetedPageId);
            }
            string _friendlyUrlExtension = ".aspx";
            bool _friendlyUrlNoExtension = false;
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["FriendlyUrlExtension"]))
            {
                _friendlyUrlExtension = System.Configuration.ConfigurationManager.AppSettings["FriendlyUrlExtension"];
            }

            //if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["friendlyUrlNoExtension"]) && System.Configuration.ConfigurationManager.AppSettings["friendlyUrlNoExtension"] == "1")
            if (PortalSettings.FriendlyUrlNoExtensionEnabled())
            {
                _friendlyUrlNoExtension = true;
            }

            pagepath = pagepath.ToLower();
            if (!pagepath.Contains(_friendlyUrlExtension) && !_friendlyUrlNoExtension)
            {
                pagepath += _friendlyUrlExtension;
            }
           
            foreach (DataRow pageRow in dtPages.Rows)
            {
                int pageId = Convert.ToInt32(pageRow["PageID"]);
                string url = HttpUrlBuilder.BuildUrl(pageId).ToLower();
                if (url == pagepath)
                    return pageId.ToString();
                else
                {
                    if (!url.Contains(_friendlyUrlExtension))
                    {
                        url += _friendlyUrlExtension;
                    }
                    if (url == pagepath)
                        return pageId.ToString();
                }
            }

            string dynamicPage = DB.GetDynamicPageUrl(pagepath);
            if (!string.IsNullOrEmpty(dynamicPage))
            {
                //-1 for dynamic pages
                return "-1";
            }

            // if page is not found it will throw 404 error
            throw new HttpException(404, "Page not Found", 3);
        }

        /// <summary>
        /// Get pageid by rootnood and URL
        /// </summary>
        /// <param name="rootnode">root node</param>
        /// <param name="pagepath">page url</param>
        /// <returns>page id</returns>
        private static string GetPageID(SiteMapNode rootnode, string pagepath)
        {
            foreach (SiteMapNode child in rootnode.ChildNodes)
            {
                if (child.Url.ToLower() == pagepath.ToLower())
                {
                    return child.Key;
                }
                else if (child.HasChildNodes)
                {
                    return GetPageID(child, pagepath);
                }
            }

            return "1";
        }

        /// <summary>
        /// Get PageFriendlyURL by PageID 
        /// If exist then return otherwise emplty
        /// </summary>
        /// <param name="pageID">PageID</param>
        /// <returns>FriendlyURL</returns>
        private static string GetFriendlyURlbyPageID(int pageID)
        {
            PagesDB pages = new PagesDB();
            //Get FriendlyURL from DB by pageid
            return pages.GetFriendlyURl(pageID);
        }
    }
}
