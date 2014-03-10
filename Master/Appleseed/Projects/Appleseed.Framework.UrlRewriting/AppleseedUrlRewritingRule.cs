
namespace Appleseed.Framework.UrlRewriting
{
    using System.Configuration;
    using System.Globalization;
    using System.Web;

    using UrlRewritingNet.Configuration;
    using UrlRewritingNet.Web;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The appleseed url rewriting rule.
    /// </summary>
    public class AppleseedUrlRewritingRule : RewriteRule
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

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the specified rewrite settings.
        /// </summary>
        /// <param name="rewriteSettings">The rewrite settings.</param>
        public override void Initialize(RewriteSettings rewriteSettings)
        {
            base.Initialize(rewriteSettings);

            if (!string.IsNullOrEmpty(rewriteSettings.Attributes["handlerflag"]))
            {
                this.handlerFlag = rewriteSettings.Attributes["handlerflag"].ToLower(CultureInfo.InvariantCulture);
            }

            if (!string.IsNullOrEmpty(rewriteSettings.Attributes["handlersplitter"]))
            {
                this.defaultSplitter = rewriteSettings.Attributes["handlersplitter"];
            }
            else
            {
                if (ConfigurationManager.AppSettings["HandlerDefaultSplitter"] != null)
                {
                    this.defaultSplitter = ConfigurationManager.AppSettings["HandlerDefaultSplitter"];
                }
            }

            if (!string.IsNullOrEmpty(rewriteSettings.Attributes["pageidnosplitter"]))
            {
                bool.Parse(rewriteSettings.Attributes["pageidnosplitter"]);
            }

            if (!string.IsNullOrEmpty(rewriteSettings.Attributes["friendlyPageName"]))
            {
                this.friendlyPageName = rewriteSettings.Attributes["friendlyPageName"];
            }
        }

        /// <summary>
        /// Determines whether the specified request URL is rewrite.
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <returns>
        /// <c>true</c> if the specified request URL is rewrite; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsRewrite(string requestUrl)
        {
            if (requestUrl.Contains(string.Format("/{0}/", this.handlerFlag))) {
                return true;
            }
            var path = HttpContext.Current.Request.ApplicationPath;
            if (!path.EndsWith("/"))
            {
                path = string.Concat(path,"/");
            }
            if (requestUrl.Equals(string.Format("{0}{1}", path, this.handlerFlag)))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Rewrites the URL.
        /// </summary>
        /// <param name="url">The URL to rewrite.</param>
        /// <returns>The rewritten URL.</returns>
        public override string RewriteUrl(string url)
        {
            var handler = string.Format("/{0}", this.handlerFlag);
            var rewrittenUrl = url.Substring(0, url.IndexOf(handler));

            var parts = url.Substring(url.IndexOf(handler) + handler.Length).Split(new char[] {'/'}, System.StringSplitOptions.RemoveEmptyEntries);

            rewrittenUrl += string.Format("/{0}", this.friendlyPageName);

            var pageId = "0"; //this is made in order to allow urls formed only with the handler (/site/ been the default). Those urls will be redirected to the portal home.
            Regex regex = new Regex("^\\d+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            // Need to search for the pageId in the url
            int indexNumber = -1;
            for (int i = 0; i < parts.Length && indexNumber == -1; i++) {
                if (regex.IsMatch(parts[i])) {
                    indexNumber = i;
                }
            }
            if (url.Contains("alias" + this.defaultSplitter))
            {
                pageId = 0.ToString();
            }
            else if (indexNumber != -1) {
                pageId = parts[indexNumber];
            }
            
            var queryString = string.Format("?pageId={0}", pageId);

            if (parts.Length > 2)
            {
                for (var i = 0; i < indexNumber; i++)
                {
                    var queryStringParam = parts[i];

                    if (queryStringParam.IndexOf(this.defaultSplitter) < 0)
                    {
                        continue;
                    }

                    queryString += string.Format(
                        "&{0}={1}",
                        queryStringParam.Substring(0, queryStringParam.IndexOf(this.defaultSplitter)),
                        queryStringParam.Substring(queryStringParam.IndexOf(this.defaultSplitter) + this.defaultSplitter.Length));
                }
            }
            if (HttpContext.Current.Request.Form["signed_request"] != null) 
            {
                queryString += string.Format(
                    "&signed_request={0}",
                    HttpContext.Current.Request.Params["signed_request"] ) ;
            }

            HttpContext.Current.RewritePath(rewrittenUrl, string.Empty, queryString);

            return rewrittenUrl + queryString;
        }

        #endregion
    }
}