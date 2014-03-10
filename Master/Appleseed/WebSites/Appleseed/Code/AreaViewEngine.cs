using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Caching;
using System.Web.Mvc;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Site.Configuration;
using System.Web;
using Appleseed.Framework;

namespace Appleseed.Code {
    public class AppleseedWebFormViewEngine : WebFormViewEngine {
        public AppleseedWebFormViewEngine() {
            var viewLocationFormatsList = new List<string>
            {               
                "~/{0}.aspx",
                "~/{0}.ascx",
                "~/Views/{1}/{0}.aspx",
                "~/Views/{1}/{0}.ascx",
                "~/Views/Shared/{0}.aspx",
                "~/Views/Shared/{0}.ascx",
            };

            var masterLocationFormatsList = new List<string>
            {
                "~/{0}.master",
                "~/Shared/{0}.master",
                "~/Views/{1}/{0}.master",
                "~/Views/Shared/{0}.master",
            };


            viewLocationFormatsList.Insert(0, @"~/Portals/_$$/MVCTemplates/{0}.ascx");

            ViewLocationFormats = viewLocationFormatsList.ToArray();
            PartialViewLocationFormats = ViewLocationFormats;

            MasterLocationFormats = masterLocationFormatsList.ToArray();
            AreaMasterLocationFormats = MasterLocationFormats;
        }

        private static string PortalAlias {
            get {
                return (string)HttpContext.Current.Items["PortalAlias"];
            }
        }

        private static string CacheKey(string action, ControllerContext controllerContext, string viewName, string masterName) {
            var controllerName = controllerContext.RouteData.GetRequiredString("controller");
            var area = controllerContext.RouteData.DataTokens["area"] ?? string.Empty;
            return string.Format("WebForm_PortalAlias{0}_{1}_{2}_{3}_{4}_{5}", PortalAlias, action, area, controllerName, viewName, masterName);

        }

        private static void AddToCache(string key, ViewEngineResult result) {
            var cache = HttpRuntime.Cache;
            int time;
            try {
                time = int.Parse(ConfigurationManager.AppSettings["ViewEngineCacheTime"]);
            }
            catch (Exception) {
                time = 10;
            }
            if (cache.Get(key) == null && result.View != null) {
                cache.Add(key, result, null, DateTime.Now.AddMinutes(time), TimeSpan.Zero, CacheItemPriority.Normal, null);
            }
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache) {

            var cache = HttpRuntime.Cache;
            var key = CacheKey("FindPartialView", controllerContext, partialViewName, string.Empty);
            if (cache.Get(key) != null)
                return (ViewEngineResult)cache.Get(key);

            ViewLocationFormats[0] = ViewLocationFormats[0].Replace("$$", PortalAlias);
            PartialViewLocationFormats[0] = PartialViewLocationFormats[0].Replace("$$", PortalAlias);

            /*custom partialview exists ?*/
            var formattedView = FormatViewName(controllerContext, partialViewName);
            string str2 = formattedView.path;
            ViewEngineResult result;
            if (formattedView.custom) {

                result = new ViewEngineResult(new WebFormView(controllerContext, str2), this);
                AddToCache(key, result);
                return result;
            }

            result = base.FindPartialView(controllerContext, str2, useCache);
            if ((result != null) && (result.View != null)) {
                AddToCache(key, result);
                return result;
            }
            /**/

            /*or custom shared partialview exists ?*/
            formattedView = FormatSharedViewName(controllerContext, partialViewName);
            string str3 = formattedView.path;
            if (formattedView.custom) {
                result = new ViewEngineResult(new WebFormView(controllerContext, str3), this);
                AddToCache(key, result);
                return result;
            }

            result = base.FindPartialView(controllerContext, str3, useCache);
            if ((result != null) && (result.View != null)) {
                AddToCache(key, result);
                return result;
            }

            /*else return original partialview*/
            result = base.FindPartialView(controllerContext, partialViewName, useCache);
            AddToCache(key, result);
            return result;

        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache) {

            var cache = HttpRuntime.Cache;
            var key = CacheKey("FindView", controllerContext, viewName, masterName);
            if (cache.Get(key) != null)
                return (ViewEngineResult)cache.Get(key);

            ViewLocationFormats[0] = ViewLocationFormats[0].Replace("$$", PortalAlias);
            PartialViewLocationFormats[0] = PartialViewLocationFormats[0].Replace("$$", PortalAlias);

            /*custom partialview exists ?*/
            var formattedView = FormatViewName(controllerContext, viewName);
            string str2 = formattedView.path;
            ViewEngineResult result;
            if (formattedView.custom) {
                result = new ViewEngineResult(new WebFormView(controllerContext, str2), this);
                AddToCache(key, result);
                return result;
            }

            result = base.FindPartialView(controllerContext, str2, useCache);
            if ((result != null) && (result.View != null)) {
                AddToCache(key, result);
                return result;
            }
            /**/

            /*or custom shared partialview exists ?*/
            formattedView = FormatSharedViewName(controllerContext, viewName);
            string str3 = formattedView.path;
            if (formattedView.custom) {
                result = new ViewEngineResult(new WebFormView(controllerContext, str3), this);
                AddToCache(key, result);
                return result;
            }

            result = base.FindPartialView(controllerContext, str3, useCache);
            if ((result != null) && (result.View != null)) {
                AddToCache(key, result);
                return result;
            }

            /*else return original partialview*/
            result = base.FindView(controllerContext, viewName, masterName, useCache);
            AddToCache(key, result);
            return result;
        }

        private dynamic FormatViewName(ControllerContext controllerContext, string viewName) {
            var requiredString = controllerContext.RouteData.GetRequiredString("controller");
            var str2 = Convert.ToString(controllerContext.RouteData.Values["area"] ?? string.Empty);

            var basePath = "Areas";

            var tempPath = @"~/Portals/_" + PortalAlias + "/MVCTemplates/";
            var relativeFilePath = Path.WebPathCombine(tempPath, str2, "Views", requiredString, viewName);
            var absoluteFilePath = controllerContext.HttpContext.Server.MapPath(relativeFilePath);

            if (FileExists(controllerContext, Path.WebPathCombine(tempPath, str2, "Views", requiredString, viewName))) {
                basePath = tempPath;
            }
            else {
                if (System.IO.File.Exists(absoluteFilePath + ".ascx")) {
                    return new { path = relativeFilePath + ".ascx", custom = true };
                }
                if (System.IO.File.Exists(absoluteFilePath + ".aspx")) {
                    return new { path = relativeFilePath + ".aspx", custom = true };
                }
            }

            return new { path = Path.WebPathCombine(basePath, str2, "Views", requiredString, viewName), custom = false };
        }

        private dynamic FormatSharedViewName(ControllerContext controllerContext, string viewName) {
            //string requiredString = controllerContext.RouteData.GetRequiredString("controller");
            var str = Convert.ToString(controllerContext.RouteData.Values["area"] ?? string.Empty);

            var basePath = "Areas";

            var tempPath = @"~/Portals/_" + PortalAlias + "/MVCTemplates/";
            var relativeFilePath = Path.WebPathCombine(tempPath, str, "Views", "Shared", viewName);
            var absoluteFilePath = controllerContext.HttpContext.Server.MapPath(relativeFilePath);

            if (FileExists(controllerContext, Path.WebPathCombine(tempPath, str, "Views", "Shared", viewName))) {
                basePath = tempPath;
            }
            else {
                if (System.IO.File.Exists(absoluteFilePath + ".ascx")) {
                    return new { path = relativeFilePath + ".ascx", custom = true };
                }
                if (System.IO.File.Exists(absoluteFilePath + ".aspx")) {
                    return new { path = relativeFilePath + ".aspx", custom = true };
                }
            }


            return new { path = Path.WebPathCombine(basePath, str, "Views", "Shared", viewName), custom = false };
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath) {

            var cache = HttpRuntime.Cache;
            var controllerName = controllerContext.RouteData.GetRequiredString("controller");
            var area = Convert.ToString(controllerContext.RouteData.Values["area"] ?? string.Empty);
            var key = string.Format("_CreateView_{0}_{1}_{2}_{3}_{4}", PortalAlias, controllerName, area, viewPath, masterPath);
            if (cache.Get(key) != null)
                return (IView)cache.Get(key);


            var newMasterPath = masterPath;

            if (viewPath.ToLower().EndsWith(".aspx")) {
                var customMasterPath = string.Format("~/Portals/_{0}/MVCTemplates/Menu.master", PortalAlias);
                if (System.IO.File.Exists(controllerContext.HttpContext.Server.MapPath(customMasterPath))) {
                    newMasterPath = customMasterPath;
                }
                else {
                    try {
                        var layout = ((PortalSettings)HttpContext.Current.Items["PortalSettings"]).CurrentLayout;
                        customMasterPath = string.Format("~/Design/DesktopLayouts/{0}/Menu.master", layout);
                        if (System.IO.File.Exists(controllerContext.HttpContext.Server.MapPath(customMasterPath))) {
                            newMasterPath = customMasterPath;
                        }
                    }
                    catch (Exception e) {
                        ErrorHandler.Publish(LogLevel.Error, e);
                    }

                }
            }

            var view = base.CreateView(controllerContext, viewPath, newMasterPath);

            int time;
            try {
                time = int.Parse(ConfigurationManager.AppSettings["ViewEngineCacheTime"]);
            }
            catch (Exception) {
                time = 10;
            }

            cache.Add(key, view, null, DateTime.Now.AddMinutes(time), TimeSpan.Zero, CacheItemPriority.Normal, null);


            return view;
        }
    }

    public class AppleseedRazorViewEngine : RazorViewEngine {
        public AppleseedRazorViewEngine() {

            var viewLocationFormatsList = new List<string>
            {               
                "~/{0}.cshtml",
                "~/{0}.vbhtml",
                "~/Views/{1}/{0}.cshtml",
                "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Shared/{0}.vbhtml",
            };

            var masterLocationFormatsList = new List<string>
            {
                "~/{0}.master",
                "~/Shared/{0}.master",
                "~/Views/{1}/{0}.master",
                "~/Views/Shared/{0}.master",
            };


            viewLocationFormatsList.Insert(0, @"~/Portals/_$$/MVCTemplates/{0}.cshtml");
            viewLocationFormatsList.Insert(1, @"~/Portals/_$$/MVCTemplates/{0}.vbhtml");

            ViewLocationFormats = viewLocationFormatsList.ToArray();
            PartialViewLocationFormats = ViewLocationFormats;

            MasterLocationFormats = masterLocationFormatsList.ToArray();
            AreaMasterLocationFormats = MasterLocationFormats;
        }

        private static string PortalAlias {
            get {
                return (string)HttpContext.Current.Items["PortalAlias"];
            }
        }

        private static string CacheKey(string action, ControllerContext controllerContext, string viewName, string masterName) {
            var controllerName = controllerContext.RouteData.GetRequiredString("controller");
            var area = controllerContext.RouteData.DataTokens["area"] ?? string.Empty;
            return string.Format("WebForm_PortalAlias{0}_{1}_{2}_{3}_{4}_{5}", PortalAlias, action, area, controllerName, viewName, masterName);

        }

        private static void AddToCache(string key, ViewEngineResult result) {
            var cache = HttpRuntime.Cache;
            int time;
            try {
                time = int.Parse(ConfigurationManager.AppSettings["ViewEngineCacheTime"]);
            }
            catch (Exception) {
                time = 10;
            }
            if (cache.Get(key) == null && result.View != null) {
                cache.Add(key, result, null, DateTime.Now.AddMinutes(time), TimeSpan.Zero, CacheItemPriority.Normal, null);
            }
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache) {

            var cache = HttpRuntime.Cache;
            var key = CacheKey("FindPartialView", controllerContext, partialViewName, string.Empty);
            if (cache.Get(key) != null)
                return (ViewEngineResult)cache.Get(key);

            ViewLocationFormats[0] = ViewLocationFormats[0].Replace("$$", PortalAlias);
            PartialViewLocationFormats[0] = PartialViewLocationFormats[0].Replace("$$", PortalAlias);
            ViewLocationFormats[1] = ViewLocationFormats[1].Replace("$$", PortalAlias);
            PartialViewLocationFormats[1] = PartialViewLocationFormats[1].Replace("$$", PortalAlias);

            /*custom partialview exists ?*/
            var formattedView = FormatViewName(controllerContext, partialViewName);
            string str2 = formattedView.path;
            ViewEngineResult result;
            if (formattedView.custom) {
                result = new ViewEngineResult(new RazorView(controllerContext, str2, null, false, null), this);
                AddToCache(key, result);
                return result;
            }

            result = base.FindPartialView(controllerContext, str2, useCache);
            if ((result != null) && (result.View != null)) {
                AddToCache(key, result);
                return result;
            }
            /**/

            /*or custom shared partialview exists ?*/
            formattedView = FormatSharedViewName(controllerContext, partialViewName);
            string str3 = formattedView.path;
            if (formattedView.custom) {
                result = new ViewEngineResult(new RazorView(controllerContext, str3, null, false, null), this);
                AddToCache(key, result);
                return result;
            }

            result = base.FindPartialView(controllerContext, str3, useCache);
            if ((result != null) && (result.View != null)) {
                AddToCache(key, result);
                return result;
            }

            /*else return original partialview*/
            result = base.FindPartialView(controllerContext, partialViewName, useCache);
            AddToCache(key, result);
            return result;

        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache) {

            var cache = HttpRuntime.Cache;
            var key = CacheKey("FindView", controllerContext, viewName, masterName);
            if (cache.Get(key) != null)
                return (ViewEngineResult)cache.Get(key);

            ViewLocationFormats[0] = ViewLocationFormats[0].Replace("$$", PortalAlias);
            PartialViewLocationFormats[0] = PartialViewLocationFormats[0].Replace("$$", PortalAlias);
            ViewLocationFormats[1] = ViewLocationFormats[1].Replace("$$", PortalAlias);
            PartialViewLocationFormats[1] = PartialViewLocationFormats[1].Replace("$$", PortalAlias);

            /*custom partialview exists ?*/
            var formattedView = FormatViewName(controllerContext, viewName);
            string str2 = formattedView.path;
            ViewEngineResult result;
            if (formattedView.custom) {
                result = new ViewEngineResult(new RazorView(controllerContext, str2, null, false, null), this);
                AddToCache(key, result);
                return result;
            }

            result = base.FindPartialView(controllerContext, str2, useCache);
            if ((result != null) && (result.View != null)) {
                AddToCache(key, result);
                return result;
            }
            /**/

            /*or custom shared partialview exists ?*/
            formattedView = FormatSharedViewName(controllerContext, viewName);
            string str3 = formattedView.path;
            if (formattedView.custom) {
                result = new ViewEngineResult(new RazorView(controllerContext, str3, null, false, null), this);
                AddToCache(key, result);
                return result;
            }

            result = base.FindPartialView(controllerContext, str3, useCache);
            if ((result != null) && (result.View != null)) {
                AddToCache(key, result);
                return result;
            }

            /*else return original partialview*/
            result = base.FindView(controllerContext, viewName, masterName, useCache);
            AddToCache(key, result);
            return result;
        }

        private dynamic FormatViewName(ControllerContext controllerContext, string viewName) {
            var requiredString = controllerContext.RouteData.GetRequiredString("controller");
            var str2 = Convert.ToString(controllerContext.RouteData.Values["area"] ?? string.Empty);

            var basePath = "Areas";

            var tempPath = @"~/Portals/_" + PortalAlias + "/MVCTemplates/";
            var relativeFilePath = Path.WebPathCombine(tempPath, str2, "Views", requiredString, viewName);
            var absoluteFilePath = controllerContext.HttpContext.Server.MapPath(relativeFilePath);

            if (FileExists(controllerContext, Path.WebPathCombine(tempPath, str2, "Views", requiredString, viewName))) {
                basePath = tempPath;
            }
            else {
                if (System.IO.File.Exists(absoluteFilePath + ".cshtml")) {
                    return new { path = relativeFilePath + ".cshtml", custom = true };
                }
                if (System.IO.File.Exists(absoluteFilePath + ".vbhtml")) {
                    return new { path = relativeFilePath + ".vbhtml", custom = true };
                }
            }

            return new { path = Path.WebPathCombine(basePath, str2, "Views", requiredString, viewName), custom = false };
        }

        private dynamic FormatSharedViewName(ControllerContext controllerContext, string viewName) {
            //string requiredString = controllerContext.RouteData.GetRequiredString("controller");
            var str = Convert.ToString(controllerContext.RouteData.Values["area"] ?? string.Empty);

            var basePath = "Areas";

            var tempPath = @"~/Portals/_" + PortalAlias + "/MVCTemplates/";
            var relativeFilePath = Path.WebPathCombine(tempPath, str, "Views", "Shared", viewName);
            var absoluteFilePath = controllerContext.HttpContext.Server.MapPath(relativeFilePath);

            if (FileExists(controllerContext, Path.WebPathCombine(tempPath, str, "Views", "Shared", viewName))) {
                basePath = tempPath;
            }
            else {
                if (System.IO.File.Exists(absoluteFilePath + ".cshtml")) {
                    return new { path = relativeFilePath + ".cshtml", custom = true };
                }
                if (System.IO.File.Exists(absoluteFilePath + ".vbhtml")) {
                    return new { path = relativeFilePath + ".vbhtml", custom = true };
                }
            }


            return new { path = Path.WebPathCombine(basePath, str, "Views", "Shared", viewName), custom = false };
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath) {
            var newMasterPath = masterPath;

            if (viewPath.ToLower().EndsWith(".aspx")) {
                var customMasterPath = "~/Portals/_" + PortalAlias + "/MVCTemplates/Menu.master";
                if (System.IO.File.Exists(controllerContext.HttpContext.Server.MapPath(customMasterPath))) {
                    newMasterPath = customMasterPath;
                }
                else {
                    try {
                        var layout = ((PortalSettings)HttpContext.Current.Items["PortalSettings"]).CurrentLayout;
                        customMasterPath = "~/Design/DesktopLayouts/" + layout + "/Menu.master";
                        if (System.IO.File.Exists(controllerContext.HttpContext.Server.MapPath(customMasterPath))) {
                            newMasterPath = customMasterPath;
                        }
                    }
                    catch (Exception e) {
                        ErrorHandler.Publish(LogLevel.Error, e);
                    }

                }
            }

            var view = base.CreateView(controllerContext, viewPath, newMasterPath);

            return view;
        }
    }
}