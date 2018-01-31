using Appleseed.Framework.Configuration.Items;
using Appleseed.Framework.Security;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Users.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AppleseedProxy.Controllers
{
    public class ProxyController : Controller
    {
        private static readonly HashSet<string> _restrictedHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
            "Accept",
            "Accept-Encoding",
            "Accept-Language",
            "Connection",
            "Content-Length",
            "Content-Type",
            "Date",
            "Expect",
            "Host",
            "If-Modified-Since",
            "Range",
            "Referer",
            "Transfer-Encoding",
            "User-Agent",
            "Proxy-Connection"
        };

        private Appleseed.Framework.DAL.ProxyDB proxyDB = new Appleseed.Framework.DAL.ProxyDB();

        #region Invoke Proxy Call
        
        [AcceptVerbs("GET", "HEAD", "POST", "PUT", "DELETE")]
        [ValidateInput(false)]
        public void Index([Bind(Prefix = "id")] int proxyid)
        {
            var proxy = proxyDB.GetById(proxyid);

            if (proxy == null)
            {
                Response.StatusCode = 404;
                Response.End();
            }

            bool hasAccess = false;
            if (proxy.EnabledContentAccess)
            {
                if (Request.IsAuthenticated)
                {
                    hasAccess = PortalSecurity.IsInRoles(proxy.ContentAccessRoles);
                }

                if (!hasAccess)
                {
                    Response.Redirect("/app_support/SmartError.aspx?403");
                    return;
                }
            }

            if (HttpContext.Request.Url == null)
            {
                Response.Redirect("/app_support/SmartError.aspx?403");
                return;
            }

            var url = HttpContext.Request.Url.PathAndQuery.ToLower().Replace(string.Format("/asproxy/proxy/index/{0}", proxyid), string.Empty);

            RelayContent(CombinePath(proxy.ServiceUrl, System.Web.HttpUtility.UrlDecode(url)), Request, Response, proxy.ForwardHeaders);
        }

        private static string CombinePath(string proxyUrl, string requestedUrl)
        {
            if (proxyUrl.EndsWith("/") && requestedUrl.StartsWith("/"))
            {
                return proxyUrl + requestedUrl.TrimStart(new[] { '/' });
            }
            return proxyUrl + requestedUrl;
        }

        private static void RelayContent(
            string url,
            HttpRequestBase request,
            HttpResponseBase response,
            bool forwardHeaders
        )
        {

            var uri = new Uri(url);
            var serviceRequest = System.Net.WebRequest.Create(uri);
            ((HttpWebRequest)serviceRequest).UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            serviceRequest.Method = request.HttpMethod;
            serviceRequest.ContentType = request.ContentType;

            if (forwardHeaders)
            {
                foreach (var key in request.Headers.AllKeys.Where(key => !_restrictedHeaders.Contains(key)))
                {
                    serviceRequest.Headers.Add(key, request.Headers[key]);
                }
            }

            //pull in input
            if (serviceRequest.Method != "GET")
            {

                request.InputStream.Position = 0;

                var inStream = request.InputStream;
                System.IO.Stream webStream = null;
                try
                {
                    //copy incoming request body to outgoing request
                    if (inStream != null && inStream.Length > 0)
                    {
                        serviceRequest.ContentLength = inStream.Length;
                        webStream = serviceRequest.GetRequestStream();
                        inStream.CopyTo(webStream);
                    }
                }
                finally
                {
                    if (null != webStream)
                    {
                        webStream.Flush();
                        webStream.Close();
                    }
                }
            }

            //get and push output
            try
            {
                using (var resourceResponse = (System.Net.HttpWebResponse)serviceRequest.GetResponse())
                {
                    using (var resourceStream = resourceResponse.GetResponseStream())
                    {
                        resourceStream.CopyTo(response.OutputStream);
                    }
                    response.ContentType = uri.IsFile ? MimeTypeMap.GetMimeType(System.IO.Path.GetExtension(uri.LocalPath)) : resourceResponse.ContentType;
                }
            }
            catch (System.Net.WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var resourceResponse = ex.Response as System.Net.HttpWebResponse;
                    if (resourceResponse != null)
                    {
                        response.StatusCode = (int)resourceResponse.StatusCode;
                        response.ContentType = resourceResponse.ContentType;
                        using (var resourceStream = resourceResponse.GetResponseStream())
                        {
                            resourceStream.CopyTo(response.OutputStream);
                        }
                    }
                    else
                    {
                        response.StatusCode = 500;
                        response.ContentType = ex.Response.ContentType;
                        //logger.Error(ex, "Proxy module protocol error: {0}", ex.Message);
                    }
                }
                else
                {
                    response.StatusCode = 500;
                    response.ContentType = ex.Response.ContentType;
                    //logger.Error(ex, "Proxy module error: {0}", ex.Message);
                }
            }
            finally
            {
                response.Flush();
                response.End();
            }
        }

        #endregion

        [Authorize]
        public ActionResult ProxyList()
        {
            var proxyList = proxyDB.GetAll();
            foreach (var item in proxyList)
            {
                item.ASProxyUrl = HttpContext.Request.Url.ToString().Replace(HttpContext.Request.Url.PathAndQuery, string.Format("/ASProxy/Proxy/Index/{0}/", item.ServiceId));
            }
            return View(proxyList);
        }

        [Authorize]
        public ActionResult CreateProxy([Bind(Prefix = "id")] int proxyid)
        {
            var portalSettings = (PortalSettings)System.Web.HttpContext.Current.Items["PortalSettings"];
            var roles = new UsersDB().GetPortalRoles(portalSettings.PortalAlias);
            if (proxyid > 0)
            {
                var proxy = proxyDB.GetById(proxyid);
                proxy.AllAccessRoles = roles;
                return View(proxy);
            }

            return View(new ProxyItem() { AllAccessRoles = roles });
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateProxy(ProxyItem proxyItem, FormCollection frmFields)
        {
            proxyDB.InsertProxy(proxyItem);
            return Redirect(frmFields["hdnReturnUrl"]);
        }

        [Authorize]
        public void DeleteProxy([Bind(Prefix = "id")] int serviceId)
        {
            if (serviceId > 0)
            {
                proxyDB.DeleteProxy(serviceId);
            }
        }


    }
}
