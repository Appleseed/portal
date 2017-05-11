// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The global.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Threading;

namespace Appleseed
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Profile;
    using System.Web.Routing;
    using System.Web.Security;

    using Appleseed.Context;
    using Appleseed.Framework;
    using Appleseed.Framework.Exceptions;
    using Appleseed.Framework.Helpers;
    using Appleseed.Framework.Scheduler;
    using Appleseed.Framework.Security;
    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Site.Configuration;

    using MvcContrib.UI.InputBuilder;

    using Path = System.IO.Path;
    using Reader = Appleseed.Context.Reader;
    using MvcContrib;
    using Appleseed.Core.ApplicationBus;
    using Appleseed.Framework.Update;
    using System.Configuration;
    using MvcContrib.Routing;
    using MvcContrib.PortableAreas;
    using System.Collections.Generic;
    using SelfUpdater.Controllers;
    using NuGet;
    using System.Linq;
    using Appleseed.Core;
    using Appleseed.Code;
    using SelfUpdater.Models;
    using StructureMap;

    /// <summary>
    /// The global.
    /// </summary>
    public class Global : HttpApplication
    {
        #region Public Methods

        /// <summary>
        /// Registers the routes.
        /// </summary>
        /// <param name="routes">
        /// The routes.
        /// </param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("Images/{*path}");
            routes.IgnoreRoute("Design/{*path}");
            routes.IgnoreRoute("Scripts/{*path}");
            routes.IgnoreRoute("Portals/{*path}");
            routes.IgnoreRoute("Content/{*path}");
            routes.IgnoreRoute("aspnet_client/{*path}");

            //Set the page extenstion from Web.Config
            routes.IgnoreRoute("UploadDialog.aspx");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}" + ConfigurationManager.AppSettings["FriendlyUrlExtension"].ToString() + "/{*pathInfo}");

            routes.IgnoreRoute(string.Empty);

            routes.MapRoute(
                 "Default",                                              // Route name
                 "{controller}/{action}/{id}",                           // URL with parameters
                 new { controller = "Home", action = "Index", id = "1" },  // Parameter defaults
                 new string[] { "FileManager.Controllers" }
             );
        }

        /// <summary>
        /// Runs on application end.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        public void Application_OnEnd(object sender, EventArgs e)
        {
            ErrorHandler.Publish(LogLevel.Info, "Application Ended");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the BeginRequest event of the AppleseedApplication control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected void AppleseedApplication_BeginRequest(object sender, EventArgs e)
        {
            string rawUrlLower = Request.RawUrl.ToLower();
            if (rawUrlLower != "/" && !rawUrlLower.Contains("/installer") && !rawUrlLower.Contains("/webresource.axd") && !File.Exists(Server.MapPath(rawUrlLower.Split('?')[0])))
            {
                Appleseed.Framework.Site.Data.PagesDB pagedb = new Framework.Site.Data.PagesDB();
                string redirectToUrl = pagedb.GetDynamicPageUrl(rawUrlLower);
                if (!string.IsNullOrEmpty(redirectToUrl))
                {
                    Response.Redirect(redirectToUrl, true);
                    return;
                }
            }
            if (rawUrlLower.Contains("tkn=") && !string.IsNullOrEmpty(Request.QueryString["tkn"]) && Request.Url.LocalPath == "/")
            {
                Appleseed.Framework.Site.Data.PagesDB pagedb1 = new Framework.Site.Data.PagesDB();
                var pageid = pagedb1.GetPageIdByToken(Request.QueryString["tkn"]);
                string redirectToUrl1 = Appleseed.Framework.HttpUrlBuilder.BuildUrl(pageid);
                if (!string.IsNullOrEmpty(redirectToUrl1))
                {
                    Response.Redirect(redirectToUrl1 + "?tkn=" + Request.QueryString["tkn"], true);
                    return;
                }
            }
            
            string Addwww = System.Configuration.ConfigurationManager.AppSettings.Get("AddWwwToRequest");
            if (Addwww != null && Addwww.Equals("true"))
            {
                if (!Request.IsSecureConnection)
                {
                    if (!Request.Url.AbsoluteUri.ToLower().Contains("www"))
                    {
                        var newUrl = Request.Url.AbsoluteUri.Replace("http://", "http://www.");
                        Response.Redirect(newUrl, true);
                    }
                }
            }

            /*Send a signal to allow custom js registration (not enabled yet)*/
            Bus.Send(new JSRegisterDescriptor() { Scripts = new List<string>() });

            var contextReader = new Reader(new WebContextReader());
            var context = contextReader.Current;

            var currentUrl = context.Request.Path.ToLower();

            if (Debugger.IsAttached && currentUrl.Contains("trace.axd"))
            {
                return;
            }

            context.Trace.Warn("Application_BeginRequest :: " + currentUrl);
            if (Portal.PageID > 0)
            {
                var physicalPath = context.Server.MapPath(currentUrl.Substring(currentUrl.LastIndexOf("/") + 1));

                if (!File.Exists(physicalPath))
                {
                    // Rewrites the path
                    context.RewritePath("~/default.aspx?" + context.Request.ServerVariables["QUERY_STRING"]);
                }
            }
            else
            {
                var pname = currentUrl.Substring(currentUrl.LastIndexOf("/") + 1);

                // if the request was not caused by an MS Ajax Client script invoking a WS.
                if (!currentUrl.ToLower().EndsWith(".asmx/js"))
                {
                    if (!String.IsNullOrEmpty(pname) && pname.Length > 5)
                    {
                        pname = pname.Substring(0, pname.Length - 5);
                    }

                    if (Regex.IsMatch(pname, @"^\d+$"))
                    {
                        context.RewritePath(
                            string.Format(
                                "~/default.aspx?pageid={0}&{1}", pname, context.Request.ServerVariables["QUERY_STRING"]));
                    }
                }
            }

            // 1st Check: is it a dangerously malformed request?
            #region
            // Important patch http://support.microsoft.com/?kbid=887459
            if (context.Request.Path.IndexOf('\\') >= 0 ||
                Path.GetFullPath(context.Request.PhysicalPath) != context.Request.PhysicalPath)
            {
                throw new AppleseedRedirect(LogLevel.Warn, HttpStatusCode.NotFound, "Malformed request", null);
            }

            #endregion

            // 2nd Check: is the AllPortals Lock switched on?
            // let the user through if client IP address is in LockExceptions list, otherwise throw...
            #region
            if (Config.LockAllPortals)
            {
                var rawUrl = context.Request.RawUrl.ToLower(CultureInfo.InvariantCulture);
                var lockRedirect = Config.LockRedirect;
                if (!rawUrl.EndsWith(lockRedirect))
                {
                    // construct IPList
                    var lockKeyHolders = Config.LockKeyHolders.Split(new[] { ';' });
                    var ipList = new IPList();
                    foreach (var lockKeyHolder in lockKeyHolders)
                    {
                        if (lockKeyHolder.IndexOf("-") > -1)
                        {
                            ipList.AddRange(
                                lockKeyHolder.Substring(0, lockKeyHolder.IndexOf("-")),
                                lockKeyHolder.Substring(lockKeyHolder.IndexOf("-") + 1));
                        }
                        else
                        {
                            ipList.Add(lockKeyHolder);
                        }
                    }

                    // check if requestor's IP address is in allowed list
                    if (!ipList.CheckNumber(context.Request.UserHostAddress))
                    {
                        throw new PortalsLockedException();
                    }
                }
            }
            #endregion

            // 3rd Check: is database/code version correct?
            var requestUri = context.Request.Url;
            var requestPath = requestUri.AbsolutePath.ToLower(CultureInfo.InvariantCulture);
            var returnToRequest = CheckAndUpdateDB(context, requestPath);


            if (returnToRequest)
            {
                return;
            }

            // Get portalsettings and add both key "PortalSettings","PortalID" into the Context.Item if not exisit 
            // All neccessory checks and oprations are managed by this method
            //Ashish.patel@haptix.biz - 2014/12/16 - Get portalsettings by pageid and portal id
            PortalSettings portalSettings = PortalSettings.GetPortalSettingsbyPageID(Portal.PageID, Portal.UniqueID);

            Membership.Provider.ApplicationName = portalSettings.PortalAlias;
            ProfileManager.Provider.ApplicationName = portalSettings.PortalAlias;
            Roles.ApplicationName = portalSettings.PortalAlias;

            var smartErrorRedirect = Config.SmartErrorRedirect;
            if (smartErrorRedirect.StartsWith("~/"))
            {
                smartErrorRedirect = smartErrorRedirect.TrimStart(new[] { '~' });
            }

            if (requestPath.EndsWith(smartErrorRedirect.ToLower(CultureInfo.InvariantCulture)))
            {
                return; // this is SmartError page... so continue             
            }

            // WLF: This was backwards before so it would always set refreshSite true because the cookie was changed before it was checked.
            // WLF: REVIEW: This whole section needs a code review.
            // Try to get alias from cookie to determine if alias has been changed
            var refreshSite = false;
            var portalAliasCookie = context.Request.Cookies["PortalAlias"];
            if (portalAliasCookie != null && portalAliasCookie.Value.ToLower() != Portal.UniqueID)
            {
                refreshSite = true; // Portal has changed since last page request
            }

            if (portalSettings != null)
            {
                portalAliasCookie = new HttpCookie("PortalAlias") { Path = "/", Value = portalSettings.PortalAlias };
                if (context.Response.Cookies["PortalAlias"] == null)
                {
                    context.Response.Cookies.Add(portalAliasCookie);
                }
                else
                {
                    context.Response.Cookies.Set(portalAliasCookie);
                }
            }

            // if switching portals then clean parameters [TipTopWeb]
            // Must be the last instruction in this method 
            var refreshedCookie = context.Request.Cookies["refreshed"];

            // 5/7/2006 Ed Daniel
            // Added hack for Http 302 by extending condition below to check for more than 3 cookies
            if (refreshSite && context.Request.Cookies.Keys.Count > 3)
            {
                // Sign out and force the browser to refresh only once to avoid any dead-lock
                if (refreshedCookie == null || refreshedCookie.Value == "false")
                {
                    var rawUrl = context.Request.RawUrl;
                    var newRefreshedCookie = new HttpCookie("refreshed", "true")
                    {
                        Path = "/",
                        Expires = DateTime.Now.AddMinutes(1)
                    };
                    if (refreshedCookie == null)
                    {
                        context.Response.Cookies.Add(newRefreshedCookie);
                    }
                    else
                    {
                        context.Response.Cookies.Set(newRefreshedCookie);
                    }

                    var msg =
                        string.Format(
                            "User logged out on global.asax line 423. Values -> refreshsite: {0}, context.Request.Cookies.Keys.count: {1}, rawurl: {2}",
                            refreshSite,
                            context.Request.Cookies.Keys.Count,
                            rawUrl);

                    ErrorHandler.Publish(
                        LogLevel.Warn,
                        msg);

                    // sign-out, if refreshed parameter on the command line we will not call it again
                    PortalSecurity.SignOut(rawUrl, false);
                }
            }

            // invalidate cookie, so the page can be refreshed when needed
            refreshedCookie = context.Request.Cookies["refreshed"];
            if (refreshedCookie != null && context.Request.Cookies.Keys.Count > 3)
            {
                var newRefreshedCookie = new HttpCookie("refreshed", "false")
                {
                    Path = "/",
                    Expires = DateTime.Now.AddMinutes(1)
                };
                context.Response.Cookies.Set(newRefreshedCookie);
            }

            // This is done in order to allow the sitemap to reference a page that is outside this website.
            var targetPage = this.Request.Params["sitemapTargetPage"];
            if (!string.IsNullOrEmpty(targetPage))
            {
                int mvcPageId;
                if (int.TryParse(targetPage, out mvcPageId))
                {
                    var url = HttpUrlBuilder.BuildUrl(mvcPageId);
                    this.Response.Redirect(url);
                }
            }
        }

        private bool CheckAndUpdateDB(HttpContext context, string requestPath)
        {
            var requestUri = context.Request.Url;

            var installRedirect = Config.InstallerRedirect;
            if (installRedirect.StartsWith("~/"))
            {
                installRedirect = installRedirect.TrimStart(new[] { '~', '/' });
            }

            installRedirect = installRedirect.ToLower(CultureInfo.InvariantCulture);
            if (requestPath.EndsWith(installRedirect) || requestPath.Contains(installRedirect.Split(new[] { '/' })[0]))
            {
                return true; // this is Install page... so skip creation of PortalSettings
            }

            UpdateDB();

            return false;
        }

        private void UpdateDB()
        {

            var versionDelta = Database.DatabaseVersion.CompareTo(Portal.CodeVersion);

            // if DB and code versions do not match
            if (versionDelta != 0)
            {
                // ...and this is not DB Update page
                var errorMessage = string.Format(
                    "Database version: {0} Code version: {1}", Database.DatabaseVersion, Portal.CodeVersion);

                if (versionDelta < 0)
                {
                    // DB Version is behind Code Version
                    ErrorHandler.Publish(LogLevel.Warn, errorMessage);
                    using (var s = new Services())
                    {
                        s.RunDBUpdate(Config.ConnectionString);
                    }
                }
                else
                {
                    // DB version is ahead of Code Version
                    ErrorHandler.Publish(LogLevel.Warn, errorMessage);
                }
            }
        }

        /// <summary>
        /// Handles the BeginRequest event of the Application control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (!Request.Path.ToLower().Contains("images.ashx") &&
                  !Request.Url.AbsoluteUri.ToLower().Contains("/images/") &&
                  !Request.Url.AbsoluteUri.ToLower().Contains("/i/"))
            {
                this.AppleseedApplication_BeginRequest(sender, e);
            }
        }

        /// <summary>
        /// Handles the Error event of the Application control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        protected void Application_Error(object sender, EventArgs e)
        {
            ErrorHandler.ProcessUnhandledException();
        }

        /// <summary>
        /// Runs when the application starts.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        /// <remarks>
        /// </remarks>
        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.MapHubs();
            var context = HttpContext.Current;

            // moved from PortalSettings
            var f = FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof(Portal)).Location);
            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application["CodeVersion"] = 1918; //f.FilePrivatePart;
            HttpContext.Current.Application["NugetSelfUpdatesToInstall"] = false;
            HttpContext.Current.Application.UnLock();

            ErrorHandler.Publish(
                LogLevel.Info, string.Format("Application Started: code version {0}", Portal.CodeVersion));

            if (Config.CheckForFilePermission)
            {
                try
                {
                    var newDir = Path.Combine(Framework.Settings.Path.ApplicationPhysicalPath, "_createnewdir");

                    if (!Directory.Exists(newDir))
                    {
                        Directory.CreateDirectory(newDir);
                    }

                    if (Directory.Exists(newDir))
                    {
                        Directory.Delete(newDir);
                    }
                }
                catch (Exception ex)
                {
                    throw new AppleseedException(
                        LogLevel.Fatal,
                        HttpStatusCode.ServiceUnavailable,
                        "ASPNET Account does not have rights to the file system",
                        ex); // Jes1111
                }
            }

            // Start scheduler
            if (Config.SchedulerEnable)
            {
                PortalSettings.Scheduler =
                    CachedScheduler.GetScheduler(
                        context.Server.MapPath(Framework.Settings.Path.ApplicationRoot),
                        Config.SqlConnectionString,
                        Config.SchedulerPeriod,
                        Config.SchedulerCacheSize);
                PortalSettings.Scheduler.Start();
            }

            // Start proxy
            if (Config.UseProxyServerForServerWebRequests)
            {
                WebRequest.DefaultWebProxy = PortalSettings.GetProxy();
            }

            try
            {
                UpdateDB();

                //Register first core portable area (just in case...)
                Appleseed.Core.PortableAreaUtils.RegisterArea<Appleseed.Core.AppleseedCoreRegistration>(RouteTable.Routes, PortableAreaUtils.RegistrationState.Initializing);

                //Then, register all portable areas
                AreaRegistration.RegisterAllAreas(PortableAreaUtils.RegistrationState.Bootstrapping);
                RegisterRoutes(RouteTable.Routes);

                if (ConfigurationManager.AppSettings["RouteTesting"] != null &&
                    bool.Parse(ConfigurationManager.AppSettings["RouteTesting"]))
                {
                    //RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);

                }

                ErrorHandler.Publish(LogLevel.Info, "Registing Routes");


                InputBuilder.BootStrap();
                ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());

                ViewEngines.Engines.Clear();
                ViewEngines.Engines.Add(new AppleseedWebFormViewEngine());
                ViewEngines.Engines.Add(new AppleseedRazorViewEngine());

            }
            catch (Exception exc)
            {

                ErrorHandler.Publish(LogLevel.Error, exc);
            }

            try
            {
                var dbContext = new SelfUpdaterEntities();
                if (dbContext.SelfUpdatingPackages.Any())
                {
                    var selfUpdatesThread = new SelfUpdateThread();
                    var workerThread = new Thread(selfUpdatesThread.CheckForSelfUpdates);
                    workerThread.Start();
                    HttpContext.Current.Application.Lock();
                    HttpContext.Current.Application["NugetSelfUpdatesToInstall"] = true; //f.FilePrivatePart;
                    HttpContext.Current.Application.UnLock();

                    // Delete Session logger
                    //HttpContext.Current.Session["NugetLogger"] = String.Empty;
                    HttpContext.Current.Application["NugetLogger"] = String.Empty;

                }

            }
            catch (Exception exc)
            {

                ErrorHandler.Publish(LogLevel.Error, exc);
            }

        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            System.Data.SqlClient.SqlConnection.ClearAllPools();
        }

        //private bool CheckForSelfUpdates()
        //{
        //    bool updateNeeded = false;

        //    try {

        //        SelfUpdaterEntities context = new SelfUpdaterEntities();

        //        var packagesToUpdate = context.SelfUpdatingPackages.AsQueryable();

        //        //updateNeeded = (packagesToUpdate.Count() > 0);

        //        if (packagesToUpdate.Count() > 0) {
        //            updateNeeded = true;

        //            /*This forces a site restart for each update scheduled */
        //            /*Must be improved trying to updated all at once */

        //            var packageToUpdate = packagesToUpdate.First();
        //            var projectManager = this.GetProjectManagers().Where(d => d.SourceRepository.Source.ToLower().Trim() == packageToUpdate.Source.ToLower().Trim()).First();
        //            var packageName = packageToUpdate.PackageId;
        //            IPackage installedPackage = projectManager.GetInstalledPackages().Where(d => d.Id == packageName).First();
        //            IPackage update = projectManager.GetUpdate(installedPackage);

        //            if (update != null) {
        //                ErrorHandler.Publish(LogLevel.Info, String.Format("SelfUpdater: Updating {0} from {1} to {2}", packageName, installedPackage.Version, update.Version));
        //                try {
        //                    projectManager.UpdatePackage(update);
        //                } catch (Exception exc) {
        //                    ErrorHandler.Publish(LogLevel.Info, String.Format("SelfUpdater: Error updating {0} from {1} to {2}", packageName, installedPackage.Version, update.Version), exc);
        //                    context.SelfUpdatingPackages.DeleteObject(packageToUpdate);
        //                }
        //            } else {
        //                ErrorHandler.Publish(LogLevel.Info, "SelfUpdater: " + packageName + " update applied !");
        //                context.SelfUpdatingPackages.DeleteObject(packageToUpdate);
        //            }

        //            context.SaveChanges();

        //        } else {
        //            ErrorHandler.Publish(LogLevel.Info, "SelfUpdater: Nothing to update");
        //        }

        //        return updateNeeded;
        //    } catch (Exception exc) {

        //        ErrorHandler.Publish(LogLevel.Error, exc);
        //        return updateNeeded;
        //    }
        //}

        //private WebProjectManager[] GetProjectManagers()
        //{
        //    string remoteSources = ConfigurationManager.AppSettings["PackageSource"] ?? @"D:\";
        //    List<WebProjectManager> managers = new List<WebProjectManager>();
        //    foreach (var remoteSource in remoteSources.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)) {
        //        managers.Add(new WebProjectManager(remoteSource, HttpContext.Current.Server.MapPath("~/")));
        //    }

        //    return managers.ToArray();
        //}

        #endregion
    }
}