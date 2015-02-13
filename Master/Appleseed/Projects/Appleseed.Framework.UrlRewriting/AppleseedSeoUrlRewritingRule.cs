using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Appleseed.Framework.UrlRewriting
{
    using System.Configuration;
    using System.Globalization;
    using System.Web;

    using UrlRewritingNet.Configuration;
    using UrlRewritingNet.Web;
    using System.Text.RegularExpressions;


    public class AppleseedSeoUrlRewritingRule : RewriteRule
    {
        #region Constants and Fields

        /// <summary>
        /// The default splitter.
        /// </summary>
        private string defaultSplitter = "__";

        /// <summary>
        /// The friendly page name.
        /// </summary>
        private string friendlyPageName = "Default.aspx";

        /// <summary>
        /// The handler flag.
        /// </summary>
        private string handlerFlag = "site";

        /// <summary>
        /// The friendly Url extension.
        /// </summary>
        private string friendlyUrlExtension = ".aspx";

        private Regex regex;

        #endregion

        public override void Initialize(RewriteSettings rewriteSettings)
        {
            base.Initialize(rewriteSettings);

            if (!string.IsNullOrEmpty(rewriteSettings.Attributes["handlerflag"])) {
                this.handlerFlag = rewriteSettings.Attributes["handlerflag"].ToLower(CultureInfo.InvariantCulture);
            }

            if (!string.IsNullOrEmpty(rewriteSettings.Attributes["handlersplitter"])) {
                this.defaultSplitter = rewriteSettings.Attributes["handlersplitter"];
            } else {
                if (ConfigurationManager.AppSettings["HandlerDefaultSplitter"] != null) {
                    this.defaultSplitter = ConfigurationManager.AppSettings["HandlerDefaultSplitter"];
                }
            }

            if (!string.IsNullOrEmpty(rewriteSettings.Attributes["pageidnosplitter"])) {
                bool.Parse(rewriteSettings.Attributes["pageidnosplitter"]);
            }

            if (!string.IsNullOrEmpty(rewriteSettings.Attributes["friendlyPageName"])) {
                this.friendlyPageName = rewriteSettings.Attributes["friendlyPageName"];
            }

            // Ashish.patel@haptix.biz - 2014/12/16 - Set friendlyURl from Web.config
            if (!string.IsNullOrEmpty(rewriteSettings.Attributes["friendlyUrlExtension"]))
            {
                this.friendlyUrlExtension = rewriteSettings.Attributes["friendlyUrlExtension"];
            }

        }

        public override bool IsRewrite(string requestUrl)
        {
            var parts = requestUrl.Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);
            if(parts.Length > 1){
                regex = new Regex("^\\d+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                // If it hasn't the pageID -> return false
                int indexNumber = -1;
                for (int i = 0; i < parts.Length && indexNumber == -1; i++) {
                    if (regex.IsMatch(parts[i])) {
                        indexNumber = i;
                    }
                }
                if (indexNumber != -1) {
                    bool exit = false;

                    // chek if the parts before the number has the split
                    for (int i = 0; i < indexNumber && !exit; i++) {
                        var queryStringParam = parts[i];

                        // Chequeo que el primer elemento no sea el nombre del directorio virtual
                        bool Check = true;
                        if (i == 0) {
                            if (HttpContext.Current.Request.ApplicationPath.Length > 1) {
                                if (!HttpContext.Current.Request.ApplicationPath.Contains(queryStringParam))
                                    return false;
                                else
                                    Check = false;
                            }
                        }
                        // Si no esta, o esta en el primer o ultimo lugar, no esta bien formado el separador
                        if (Check) {
                            if (queryStringParam.IndexOf(this.defaultSplitter) < 1 || queryStringParam.IndexOf(this.defaultSplitter) == queryStringParam.Length - 2) {
                                exit = true;
                            }
                        }
                    }
                    if (exit)
                        return false;


                    // Queda chekear que la ultima parte no tenga extension o si la tiene, que sea .aspx
                    exit = false;
                    for (int i = indexNumber + 1; i < parts.Length && !exit; i++) {
                        if (parts[i].Contains('.') && !parts[i].Contains(".aspx"))
                            exit = true;
                    }

                    if (exit)
                        return false;
                    else
                        return true;
                } else
                    return false;
           }

            //Check the page extenstion 
            if (requestUrl.Contains(this.friendlyUrlExtension))
            {
                return true;
            }

            return false;

        }

        public override string RewriteUrl(string url)
        {

            var parts = url.Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);

            // If it is an virtual app
            var path = HttpContext.Current.Request.ApplicationPath;
            var rewrittenUrl = "/";
            
            if (!path.Equals('/')) {
                rewrittenUrl += path;
                if (!path.EndsWith("/")) {
                    rewrittenUrl += "/";
                }
            }
            rewrittenUrl += string.Format("{0}", this.friendlyPageName);

            var pageId = "0"; //this is made in order to allow urls formed only with the handler (/site/ been the default). Those urls will be redirected to the portal home.
            regex = new Regex("^\\d+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            // Need to search for the pageId in the url
            int indexNumber = -1;
            for (int i = 0; i < parts.Length && indexNumber == -1; i++) {
                if (regex.IsMatch(parts[i])) {
                    indexNumber = i;
                }
            }
            if (indexNumber != -1) {
                pageId = parts[indexNumber];
            }

            // Get PageID from the URL
            pageId = UrlRewritingFriendlyUrl.GetPageIDFromPageName(url);

            //pageId = 1.ToString();
            var queryString = string.Format("?pageId={0}", pageId);

            if (parts.Length > 2) {
                for (var i = 0; i < indexNumber; i++) {
                    var queryStringParam = parts[i];
                    queryStringParam = Regex.Replace(queryStringParam, @" ", "%20");
                    if (queryStringParam.IndexOf(this.defaultSplitter) < 0) {
                        continue;
                    }

                    
                    queryString += string.Format(
                        "&{0}",
                        queryStringParam.Substring(0, queryStringParam.IndexOf(this.defaultSplitter)));
                    queryString += string.Format(
                        "={0}",
                        queryStringParam.Substring(queryStringParam.IndexOf(this.defaultSplitter) + this.defaultSplitter.Length));
                }
            }

            //Agregar los query que haya en el ultimo, y el hash
            //if(parts.Length > 2){
            //    string last = parts[parts.Length - 1];
            //    // Hay algun atributo de query
            //    if(last.IndexOf('?') > 0){
            //        var queryparts = last.Split(new char[] { '&' }, System.StringSplitOptions.RemoveEmptyEntries);
            //        queryparts[0] = queryparts[0].Substring(1, queryparts[0].Length - 1);
            //        queryString += queryparts[0];
            //        // si query parts tiene mas de un &, tiene mas de un atributo
            //        if (queryparts.Length > 1) {
            //            for (int i = 1; i < queryparts.Length - 1; i++) {
            //                queryString += queryparts[i];
            //            }
            //        }
    
            //    }
            //}
            if (HttpContext.Current.Request.Form["signed_request"] != null) {
                queryString += string.Format(
                    "&signed_request={0}",
                    HttpContext.Current.Request.Params["signed_request"]);
            }
                     
            HttpContext.Current.RewritePath(rewrittenUrl, string.Empty, queryString);

            return rewrittenUrl + queryString;
        }


    }
}
