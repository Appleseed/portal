// Created by John Mandia (john.mandia@whitelightsolutions.com)
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Exceptions;
using Appleseed.Framework.Site.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Appleseed.Framework.Web
{
    /// <summary>
    /// Appleseed standard implementation.
    /// This code has been developed and extended by John Mandia (www.whitelightsolutions.com), 
    /// Manu (www.duemetri.com), Jes (www.marinateq.com) and Cory.
    /// </summary>
    [History("Ashish.patel@haptix.biz", "2014/12/24", "Changes into build url for friendly url")]
    public class SqlUrlBuilderProvider : UrlBuilderProvider
    {
        private string _defaultSplitter = "__";
        private string _handlerFlag = string.Empty;
        private bool _aliasInUrl = false;
        private bool _langInUrl = false;
        private string _ignoreTargetPage = "tablayout.aspx";
        private double _cacheMinutes = 5;
        private bool _pageidNoSplitter = false;
        private string _friendlyPageName = "default.aspx";
        private StringDictionary queryList = new StringDictionary();
        private bool Hieranchy = false;

        //Ashish.patel@haptix.biz - 2014/12/16 - Set default extension
        private string _friendlyUrlExtension = ".aspx";


        /// <summary> 
        /// Takes a Tab ID and builds the url for get the desidered page (non default)
        /// containing the application path, portal alias, tab ID, and language. 
        /// </summary> 
        /// <param name="targetPage">Linked page</param> 
        /// <param name="pageID">ID of the page</param> 
        /// <param name="modID">ID of the module</param> 
        /// <param name="culture">Client culture</param> 
        /// <param name="customAttributes">Any custom attribute that can be needed. Use the following format...single attribute: paramname--paramvalue . Multiple attributes: paramname--paramvalue/paramname2--paramvalue2/paramname3--paramvalue3 </param> 
        /// <param name="currentAlias">Current Alias</param> 
        /// <param name="urlKeywords">Add some keywords to uniquely identify this tab. Usual source is UrlKeyword from TabSettings.</param> 
        public override string BuildUrl(string targetPage, int pageID, int modID, CultureInfo culture,
                                        string customAttributes, string currentAlias, string urlKeywords)
        {


            // Get Url Elements this helper method (Will either retrieve from cache or database)
            UrlElements urlElements = UrlBuilderHelper.GetUrlElements(pageID, _cacheMinutes);

            //2_aug_2004 Cory Isakson
            //Begin Navigation Enhancements
            if (!(targetPage.ToLower().EndsWith(_ignoreTargetPage.ToLower())))
            // Do not modify URLs when working with TabLayout Administration Page
            {
                // if it is a placeholder it is not supposed to have any url
                if (urlElements.IsPlaceHolder) return string.Empty;

                // if it is a tab link it means it is a link to an external resource
                //if (urlElements.TabLink.Length != 0) return urlElements.TabLink;
                if (urlElements.TabLink.Length != 0)
                {
                    if (urlElements.TabLink.ToLower().Contains("http://") || urlElements.TabLink.ToLower().Contains("https://"))
                        return urlElements.TabLink;
                    else
                        return "http://" + urlElements.TabLink;
                }
            }
            //End Navigation Enhancements

            if (targetPage.StartsWith("~/"))
            {
                targetPage = targetPage.Substring(2);
            }

            //Ashish.patel@haptix.biz - 2014/12/24 - Added for getting friendly url from cache
            PageUrl pageurl = GetCachePageUrl(pageID);
            string cacheUrl = string.Empty;
            if (pageurl != null && (targetPage.Equals(HttpUrlBuilder.DefaultPage) || targetPage.Equals("DesktopDefault.aspx")))
            {
                if (PortalSettings.HasEnablePageFriendlyUrl(pageID, currentAlias))
                {
                    if (!string.IsNullOrEmpty(pageurl.PageFriendlyUrl))
                        return pageurl.PageFriendlyUrl;
                }
                else
                {
                    if (!string.IsNullOrEmpty(pageurl.PageNormalUrl))
                        return pageurl.PageNormalUrl;
                }
            }


            StringBuilder sb = new StringBuilder();

            // Obtain ApplicationPath
            if (targetPage.StartsWith("~/"))
            {
                sb.Append(UrlBuilderHelper.ApplicationPath);
                targetPage = targetPage.Substring(2);
            }
            sb.Append("/");

            //if (!targetPage.EndsWith(".aspx")) //Images
            //{
            //    sb.Append(targetPage);
            //    return sb.ToString();
            //}

            HttpContext.Current.Trace.Warn("Target Page = " + targetPage);

            // Separate path
            // If page contains path, or it is not an aspx 
            // or handlerFlag is not set: do not use handler
            if (!targetPage.Equals(HttpUrlBuilder.DefaultPage) && !targetPage.Equals("DesktopDefault.aspx"))
            {
                sb.Append(targetPage);
                // if !!targetPage.EndsWith(".aspx") it's an image. Return
                if (!targetPage.EndsWith(".aspx"))
                {
                    return sb.ToString();
                }
                else
                {
                    sb.Append("?");
                    // Add pageID to URL
                    sb.Append("pageId=");
                    sb.Append(pageID.ToString());

                    // Add Alias to URL
                    if (_aliasInUrl)
                    {
                        sb.Append("&alias="); // changed for compatibility with handler
                        sb.Append(currentAlias);
                    }

                    // Add ModID to URL
                    if (modID > 0)
                    {
                        sb.Append("&mid=");
                        sb.Append(modID.ToString());
                    }

                    // Add Language to URL
                    if (_langInUrl && culture != null)
                    {
                        sb.Append("&lang="); // changed for compatibility with handler
                        sb.Append(culture.Name); // manu fix: culture.Name
                    }

                    // Add custom attributes
                    if (customAttributes != null && customAttributes != string.Empty)
                    {
                        sb.Append("&");
                        customAttributes = customAttributes.ToString().Replace("/", "&");
                        customAttributes = customAttributes.ToString().Replace(_defaultSplitter, "=");
                        sb.Append(customAttributes);
                    }
                    return sb.ToString().Replace("&&", "&");
                }
            }
            else // use handler
            {
                PortalSettings settings = PortalSettings.GetPortalSettingsbyPageID(pageID, currentAlias);
                var hasFriendUrl = false;
                if (settings.EnablePageFriendlyUrl)
                {
                    PagesDB pages = new PagesDB();
                    //Ashish.patel@haptix.biz - 2014/12/16 - Get friendlyUrl from database
                    var friendlyURL = pages.GetFriendlyURl(pageID);
                    //Ashish.patel@haptix.biz - 2014/12/16 - Check friendlyUrl not null
                    if (friendlyURL != "")
                    {
                        hasFriendUrl = true;
                        sb.Append(friendlyURL).ToString();
                    }
                }

                string queryLeft = "";
                string queryRigth = "";

                // Add custom attributes
                #region custom attributes
                if (customAttributes != null && customAttributes != string.Empty)
                {

                    var parts = customAttributes.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < parts.Length; i++)
                    {
                        try
                        {
                            var key = parts[i].Split('=')[0];
                            if (!(key.Equals("pageId") || key.Equals("pageID")))
                            {
                                if (queryList.ContainsKey(key))
                                {
                                    var q = parts[i].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                                    queryRigth += HttpUtility.UrlEncode(q[0], System.Text.Encoding.GetEncoding(28591)) + "=" + HttpUtility.UrlEncode(q[1], System.Text.Encoding.GetEncoding(28591)) + "&";
                                }
                                else
                                {
                                    var q = parts[i].Split('=');
                                    queryLeft += HttpUtility.UrlEncode(q[0], System.Text.Encoding.GetEncoding(28591)) + "=" + HttpUtility.UrlEncode(q[1], System.Text.Encoding.GetEncoding(28591)) + "&";
                                }
                            }
                        }
                        catch (Exception) { }

                    }

                    if (!string.IsNullOrEmpty(queryLeft))
                    {
                        // If its null, all the attributes are at the end, else, should add the ones from the queryLeft
                        queryLeft = queryLeft.Remove(queryLeft.Length - 1);
                        queryLeft = queryLeft.Replace("+", "%20");
                        queryLeft = queryLeft.ToString().Replace("&", "/");
                        queryLeft = queryLeft.ToString().Replace("=", _defaultSplitter);
                    }
                }
                #endregion

                if (!hasFriendUrl)
                {
                    #region Existing Url Builing code
                    // Add smarturl tag and if friendlyURL false
                    if (!string.IsNullOrEmpty(_handlerFlag) && !settings.EnablePageFriendlyUrl)
                    {
                        sb.Append(_handlerFlag);
                        sb.Append("/");
                    }

                    // Add custom Keywords to the Url
                    if (urlKeywords != null && urlKeywords != string.Empty)
                    {
                        sb.Append(urlKeywords);
                        sb.Append("/");
                    }
                    else
                    {
                        urlKeywords = urlElements.UrlKeywords;

                        // Add custom Keywords to the Url
                        if (urlKeywords != null && urlKeywords.Length != 0)
                        {
                            sb.Append(urlKeywords);
                            sb.Append("/");
                        }
                    }

                    // Add Alias to URL
                    if (_aliasInUrl)
                    {
                        sb.Append("alias");
                        sb.Append(_defaultSplitter + currentAlias);
                        sb.Append("/");
                    }

                    // Add Language to URL
                    if (_langInUrl && culture != null)
                    {
                        sb.Append("lang");
                        sb.Append(_defaultSplitter + culture.Name);
                        sb.Append("/");
                    }
                    // Add ModID to URL
                    if (modID > 0)
                    {
                        sb.Append("mid");
                        sb.Append(_defaultSplitter + modID.ToString());
                        sb.Append("/");
                    }

                    if (!string.IsNullOrEmpty(queryLeft))
                    {
                        sb.Append(queryLeft);
                        sb.Append("/");
                    }

                    //get setting from portalsettings 
                    //HttpContext.Item - add/update key all the codes are managed by this method
                    //PortalSettings settings = PortalSettings.GetPortalSettingsbyPageID(Portal.PageID, Portal.UniqueID);

                    //Ashish.patel@haptix.biz - 2014/12/16 - Appends if EnableFrindlyURl is false
                    if (!settings.EnablePageFriendlyUrl)
                    {
                        sb.Append(pageID);
                        sb.Append("/");
                    }

                    if (!string.IsNullOrEmpty(urlElements.PageName))// TODO : Need to fix page names rewrites
                        sb.Append(urlElements.PageName);
                    else
                        if (!string.IsNullOrEmpty(urlElements.PageTitle))
                        {
                            string PageName = urlElements.PageTitle;
                            // Write page Hieranchy
                            if (Hieranchy)
                            {

                                int parentId = 0;

                                bool found = false;
                                //Get the parent pageId of the actual pageId
                                for (int i = 0; i < settings.DesktopPages.Count && !found; i++)
                                {
                                    if (settings.DesktopPages[i].PageID == pageID)
                                    {
                                        parentId = settings.DesktopPages[i].ParentPageID;
                                        found = true;
                                    }
                                }
                                if (found)
                                {
                                    bool exit = false;
                                    // while the parentId it's diferent of 0 or the parentId isn't in settings
                                    while (parentId != 0 && !exit)
                                    {
                                        found = false;
                                        // find the parent in the setting
                                        for (int i = 0; i < settings.DesktopPages.Count && !found; i++)
                                        {
                                            if (settings.DesktopPages[i].PageID == parentId)
                                            {
                                                PageName = UrlBuilderHelper.CleanNoAlphanumerics(settings.DesktopPages[i].PageName) + "/" + PageName;
                                                parentId = settings.DesktopPages[i].ParentPageID;
                                                found = true;
                                            }
                                        }
                                        // If the parent isn't in settings the loop should stop
                                        if (!found)
                                            exit = true;
                                    }
                                }
                            }
                            sb.Append(PageName);
                        }
                        else
                            sb.Append(_friendlyPageName);

                    #endregion
                }

                // Ashish.patel@haptix.biz - 2014/12/16 - 
                // check friendly URL is enabled and requested file is exist physically or not. 
                // If not exists and friendly url is enabled, the extension will be appended.
                if (settings.EnablePageFriendlyUrl
                    && !System.IO.File.Exists(HttpContext.Current.Server.MapPath(sb.ToString())))
                {
                    sb.Append(this._friendlyUrlExtension);
                }

                // Add the query at the end
                if (!string.IsNullOrEmpty(queryRigth))
                {
                    queryRigth = queryRigth.Remove(queryRigth.Length - 1);
                    sb.Append("?" + queryRigth);
                }

                //Ashish.patel@haptix.biz - 2014/12/24 - Modified for setting friendly url into cache
                var url = sb.ToString().Replace("//", "/");
                if (settings.EnablePageFriendlyUrl)
                    SetCachePageUrl(pageID, string.Empty, url);
                else
                    SetCachePageUrl(pageID, url, string.Empty);

                return url;
            }
        }
        /// <summary>
        /// The initialize method lets you retrieve provider specific settings from web.config
        /// </summary>
        /// <param name="name"></param>
        /// <param name="configValue"></param>
        public override void Initialize(string name, NameValueCollection configValue)
        {

            base.Initialize(name, configValue);

            // For legacy support first check provider settings then web.config/Appleseed.config legacy settings
            if (configValue["handlersplitter"] != null)
            {
                _defaultSplitter = configValue["handlersplitter"].ToString();
            }
            else
            {
                if (ConfigurationManager.AppSettings["HandlerDefaultSplitter"] != null)
                    _defaultSplitter = ConfigurationManager.AppSettings["HandlerDefaultSplitter"];
            }

            // For legacy support first check provider settings then web.config/Appleseed.config legacy settings
            if (configValue["handlerflag"] != null)
            {
                _handlerFlag = configValue["handlerflag"].ToString();
            }
            else
            {
                _handlerFlag = string.Empty;
            }

            if (configValue["FriendlyUrlExtension"] != null)
            {
                _friendlyUrlExtension = configValue["FriendlyUrlExtension"].ToString();
            }
            else
            {
                _friendlyUrlExtension = ".aspx";
            }

            // For legacy support first check provider settings then web.config/Appleseed.config legacy settings
            if (configValue["aliasinurl"] != null)
            {
                _aliasInUrl = bool.Parse(configValue["aliasinurl"].ToString());
            }
            else
            {
                if (ConfigurationManager.AppSettings["UseAlias"] != null)
                    _aliasInUrl = bool.Parse(ConfigurationManager.AppSettings["UseAlias"]);
            }

            // For legacy support first check provider settings then web.config/Appleseed.config legacy settings
            if (configValue["langinurl"] != null)
            {
                _langInUrl = bool.Parse(configValue["langinurl"].ToString());
            }
            else
            {
                if (ConfigurationManager.AppSettings["LangInURL"] != null)
                    _langInUrl = bool.Parse(ConfigurationManager.AppSettings["LangInURL"]);
            }

            if (configValue["ignoretargetpage"] != null)
            {
                _ignoreTargetPage = configValue["ignoretargetpage"].ToString();
            }

            if (configValue["cacheminutes"] != null)
            {
                _cacheMinutes = Convert.ToDouble(configValue["cacheminutes"].ToString());
            }

            if (configValue["pageidnosplitter"] != null)
            {
                _pageidNoSplitter = bool.Parse(configValue["pageidnosplitter"].ToString());
            }
            else
            {
                if (ConfigurationManager.AppSettings["PageIdNoSplitter"] != null)
                    _pageidNoSplitter = bool.Parse(ConfigurationManager.AppSettings["PageIdNoSplitter"]);
            }

            // For legacy support first check provider settings then web.config/Appleseed.config legacy settings
            if (configValue["friendlypagename"] != null)
            {
                // TODO: Friendly url's need to be fixed
                _friendlyPageName = configValue["friendlypagename"].ToString();
            }
            else
            {
                if (ConfigurationManager.AppSettings["FriendlyPageName"] != null)
                    _friendlyPageName = ConfigurationManager.AppSettings["FriendlyPageName"];
            }
            if (configValue["Querylist"] != null)
            {
                queryList = new StringDictionary();
                string list = configValue["Querylist"].ToString();
                var parts = list.Split(';');
                for (int i = 0; i < parts.Length; i++)
                {
                    queryList.Add(parts[i], parts[i]);
                }
            }
            if (configValue["ShowHieranchy"] != null)
            {
                string ShowHieranchy = configValue["ShowHieranchy"].ToString();
                try
                {
                    Hieranchy = bool.Parse(ShowHieranchy);

                }
                catch (Exception)
                {
                    Hieranchy = false;
                }
            }


        }

        /// <summary> 
        /// Determines if a tab is simply a placeholder in the navigation
        /// </summary> 
        public override bool IsPlaceholder(int pageID)
        {
            return
                bool.Parse(
                    UrlBuilderHelper.PageSpecificProperty(pageID, UrlBuilderHelper.IsPlaceHolderID, _cacheMinutes).
                        ToString());
        }

        /// <summary> 
        /// Returns the URL for a tab that is a link only.
        /// </summary> 
        public override string TabLink(int pageID)
        {
            return UrlBuilderHelper.PageSpecificProperty(pageID, UrlBuilderHelper.TabLinkID, _cacheMinutes);
        }

        /// <summary> 
        /// Returns any keywords which are meant to be placed in the url
        /// </summary> 
        public override string UrlKeyword(int pageID)
        {
            return
                UrlBuilderHelper.PageSpecificProperty(pageID, UrlBuilderHelper.UrlKeywordsID, _cacheMinutes).ToString();
        }

        /// <summary> 
        /// Returns the page name that has been specified. 
        /// </summary> 
        public override string UrlPageName(int pageID)
        {
            string _urlPageName =
                UrlBuilderHelper.PageSpecificProperty(pageID, UrlBuilderHelper.PageNameID, _cacheMinutes).ToString();
            // TODO: URL Firendly names need to be fixed
            if (_urlPageName.Length == 0)
                _urlPageName = _friendlyPageName;

            return _urlPageName;
        }

        /// <summary>
        /// Gets the default page from web.config/Appleseed.config
        /// </summary>
        public override string DefaultPage
        {
            get
            {
                // TODO: Jes1111 - check this with John
                //string strTemp = ConfigurationSettings.AppSettings["HandlerTargetUrl"];

                // TODO : JONATHAN - PROBLEM WITH DEFAULT PAGE LIKE THIS
                string strTemp = _friendlyPageName;
                // TODO : JONATHAN - PROBLEM WITH DEFAULT PAGE LIKE THIS
                if (strTemp.Length == 0 || strTemp == null)
                    strTemp = "Default.aspx";

                return strTemp;
            }
        }

        /// <summary>
        /// Returns the default paramater splitter from provider settings (or web.config/Appleseed.config if not specified in provider) 
        /// </summary>
        public override string DefaultSplitter
        {
            get { return _defaultSplitter; }
        }

        /// <summary> 
        /// Clears the cached url element settings
        /// </summary> 
        public override void Clear(int pageID)
        {
            UrlBuilderHelper.ClearUrlElements(pageID);
        }

        /// <summary>
        ///  Get the page url object from cache
        /// </summary>
        /// <param name="pageID"> page id</param>
        /// <returns> returns object of pageurl</returns>
        /// Ashish.patel@haptix.biz - 2014/12/24 - Added for getting page url from cache
        public PageUrl GetCachePageUrl(int pageID)
        {
            List<PageUrl> lstUrls = new List<PageUrl>();
            if (HttpContext.Current.Cache["PageUrlList"] != null)
            {
                lstUrls = (List<PageUrl>)HttpContext.Current.Cache["PageUrlList"];
            }
            return lstUrls.FirstOrDefault(url => url.PageID == pageID);
        }

        /// <summary>
        /// Set the url into cache
        /// </summary>
        /// <param name="pageID"> page id</param>
        /// <param name="pageUrl"> page url</param>
        /// <param name="pageFriendlyUrl"> page friendly url</param>
        /// Ashish.patel@haptix.biz - 2014/12/24 - Added for setting list of page url into cache
        public void SetCachePageUrl(int pageID, string pageUrl, string pageFriendlyUrl)
        {
            List<PageUrl> lstUrls = new List<PageUrl>();
            if (HttpContext.Current.Cache["PageUrlList"] != null)
                lstUrls = (List<PageUrl>)HttpContext.Current.Cache["PageUrlList"];
            else
                HttpContext.Current.Cache.Insert("PageUrlList", lstUrls);
            PageUrl pgurl = lstUrls.FirstOrDefault(url => url.PageID == pageID);
            if (pgurl == null)
            {
                pgurl = new PageUrl();
                pgurl.PageID = pageID;
                lstUrls.Add(pgurl);
            }

            if (!string.IsNullOrEmpty(pageUrl) && !System.IO.File.Exists(HttpContext.Current.Server.MapPath(pageUrl.Split('?').GetValue(0).ToString())))
            {
                pgurl.PageNormalUrl = pageUrl;
            }
            if (!string.IsNullOrEmpty(pageFriendlyUrl) && !System.IO.File.Exists(HttpContext.Current.Server.MapPath(pageUrl.Split('?').GetValue(0).ToString())))
            {
                pgurl.PageFriendlyUrl = pageFriendlyUrl;
            }

            HttpContext.Current.Cache["PageUrlList"] = lstUrls;
        }

        /// <summary>
        /// It will clear page url from cache
        /// </summary>
        /// <param name="pageID">page id</param>
        /// Ashish.patel@haptix.biz - 2014/12/24 - Added for clearing list of page url from cache
        public static void ClearCachePageUrl(int pageID)
        {
            List<PageUrl> lstUrls = new List<PageUrl>();
            if (HttpContext.Current.Cache["PageUrlList"] != null)
                lstUrls = (List<PageUrl>)HttpContext.Current.Cache["PageUrlList"];
            else
                HttpContext.Current.Cache.Insert("PageUrlList", lstUrls);

            HttpContext.Current.Cache["PageUrlList"] = lstUrls.Where(url => url.PageID != pageID).ToList();
        }
    }
}