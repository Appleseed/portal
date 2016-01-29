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

            foreach (DataRow pageRow in dtPages.Rows)
            {
                int pageId = Convert.ToInt32(pageRow["PageID"]);
                string url = HttpUrlBuilder.BuildUrl(pageId);
                if (url.ToLower() == pagepath.ToLower())
                    return pageId.ToString();
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
