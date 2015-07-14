// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelServices.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The model services.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Core.Model
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Linq;

    using Appleseed.Framework.Security;
    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Site.Configuration;
    using Appleseed.Framework.Site.Data;
    using Appleseed.Framework.Web.UI.WebControls;

    using Path = Appleseed.Framework.Settings.Path;
    using Appleseed.Framework.Settings.Cache;
    using Appleseed.Framework.Models;

    /// <summary>
    /// The model services.
    /// </summary>
    public class ModelServices
    {
        // Fields
        #region Constants and Fields

        /// <summary>
        /// The content pane idx.
        /// </summary>
        public const string ContentPaneIdx = "contentpane";

        #endregion

        // Methods
        #region Public Methods

        /// <summary>
        /// Adds the MVC action module.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="actionPath">The action path.</param>
        /// <returns></returns>
        public static Guid AddMVCActionModule(string name, string actionPath)
        {
            var sdb = new ModulesDB();
            var generalModDefId = Guid.NewGuid();
            sdb.AddGeneralModuleDefinitions(
                generalModDefId, name, actionPath, string.Empty, string.Empty, string.Empty, false, false);
            return generalModDefId;
        }

        /// <summary>
        /// Gets the current page modules.
        /// </summary>
        /// <returns>A dictionary.</returns>
        public static Dictionary<string, List<Control>> GetCurrentPageModules()
        {
            
            var settings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            var modules = settings.ActivePage.Modules;
            return ConvertModuleListToDictionary(modules);
        }

        /// <summary>
        /// Convert the module list to dictionary.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns>A dictionary</returns>
        /// <exception cref="Exception"></exception>
        private static Dictionary<string, List<Control>> ConvertModuleListToDictionary(List<IModuleSettings> modules)
        {
            var settings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            var dictionary = new Dictionary<string, List<Control>>();
            dynamic faultyModule = null;
            var modErrKey = HttpContext.Current.Request.Params["modErr"];
            //we receive this param if in the Application_Error it was discovered that a module is broken
            if (!string.IsNullOrEmpty(modErrKey))
            {
                faultyModule = HttpContext.Current.Cache.Get(modErrKey);
                HttpContext.Current.Cache.Remove(modErrKey);
            }

            if (modules.Count > 0)
            {
                var page = new Page();
                foreach (ModuleSettings settings2 in modules)
                {
                    if (!settings2.Cacheable)
                    {
                        settings2.CacheTime = -1;
                    }

                    if (PortalSecurity.IsInRoles(settings2.AuthorizedViewRoles))
                    {
                        List<Control> list;
                        Exception exception;
                        var str = settings2.PaneName.ToLower();
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (!dictionary.ContainsKey(str))
                            {
                                dictionary.Add(str, new List<Control>());
                            }

                            list = dictionary[str];
                        }
                        else
                        {
                            if (!dictionary.ContainsKey("contentpane"))
                            {
                                dictionary.Add("contentpane", new List<Control>());
                            }

                            list = dictionary["contentpane"];
                        }

                        if (!settings2.Admin && (settings2.CacheTime == 0))
                        {
                            var moduleOverrideCache = Config.ModuleOverrideCache;
                            if (moduleOverrideCache > 0)
                            {
                                settings2.CacheTime = moduleOverrideCache;
                            }
                        }

                        if ((((settings2.CacheTime <= 0) || PortalSecurity.HasEditPermissions(settings2.ModuleID)) ||
                             (PortalSecurity.HasPropertiesPermissions(settings2.ModuleID) ||
                              PortalSecurity.HasAddPermissions(settings2.ModuleID))) ||
                            PortalSecurity.HasDeletePermissions(settings2.ModuleID))
                        {
                            try
                            {
                                PortalModuleControl control;
                                var virtualPath = Path.ApplicationRoot + "/" + settings2.DesktopSrc;
                                if (virtualPath.ToLowerInvariant().Trim().EndsWith(".ascx"))
                                {
                                    if (faultyModule != null && faultyModule.ModuleDefID == settings2.ModuleDefID)
                                    {
                                        throw new Exception(faultyModule.Message); //if this was the module that was generating the error, we then show the error.
                                    }
                                    control = (PortalModuleControl)page.LoadControl(virtualPath);
                                }
                                else
                                {
                                    var strArray = virtualPath.Split(
                                        new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                                    int index = 1;
                                    if (!Path.ApplicationRoot.Equals(string.Empty))
                                    {
                                        index++;
                                    }
                                    var areaName = (strArray[index].ToLower() == "views") ? string.Empty : strArray[index];
                                    var controllerName = strArray[strArray.Length - 2];
                                    var actionName = strArray[strArray.Length - 1];

                                    // var ns = strArray[2];
                                    control =
                                        (PortalModuleControl)
                                        page.LoadControl("~/DesktopModules/CoreModules/MVC/MVCModule.ascx");

                                    ((MVCModuleControl)control).ControllerName = controllerName;
                                    ((MVCModuleControl)control).ActionName = actionName;
                                    ((MVCModuleControl)control).AreaName = areaName;
                                    ((MVCModuleControl)control).ModID = settings2.ModuleID;

                                    ((MVCModuleControl)control).Initialize();
                                }

                                control.PortalID = settings.PortalID;
                                control.ModuleConfiguration = settings2;
                                if ((control.Cultures == string.Empty) ||
                                    ((control.Cultures + ";").IndexOf(settings.PortalContentLanguage.Name + ";") >= 0))
                                {
                                    list.Add(control);
                                }
                            }
                            catch (Exception exception1)
                            {
                                exception = exception1;
                                ErrorHandler.Publish(
                                    LogLevel.Error,
                                    string.Format("DesktopPanes: Unable to load control '{0}'!", settings2.DesktopSrc),
                                    exception);
                                if (PortalSecurity.IsInRoles("Admins"))
                                {
                                    list.Add(
                                        new LiteralControl(
                                            string.Format("<br><span class=NormalRed>Unable to load control '{0}'! (Full Error Logged)<br />Error Message: {1}", settings2.DesktopSrc, exception.Message)));
                                }
                                else
                                {
                                    list.Add(
                                        new LiteralControl(
                                            string.Format("<br><span class=NormalRed>Unable to load control '{0}'!", settings2.DesktopSrc)));
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                using (var control2 = new CachedPortalModuleControl())
                                {
                                    control2.PortalID = settings.PortalID;
                                    control2.ModuleConfiguration = settings2;
                                    list.Add(control2);
                                }
                            }
                            catch (Exception exception2)
                            {
                                exception = exception2;
                                ErrorHandler.Publish(
                                    LogLevel.Error,
                                    string.Format("DesktopPanes: Unable to load cached control '{0}'!", settings2.DesktopSrc),
                                    exception);
                                if (PortalSecurity.IsInRoles("Admins"))
                                {
                                    list.Add(
                                        new LiteralControl(
                                            string.Format("<br><span class=NormalRed>Unable to load cached control '{0}'! (Full Error Logged)<br />Error Message: {1}", settings2.DesktopSrc, exception.Message)));
                                }
                                else
                                {
                                    list.Add(
                                        new LiteralControl(
                                            string.Format("<br><span class=NormalRed>Unable to load cached control '{0}'!", settings2.DesktopSrc)));
                                }
                            }
                        }
                    }
                }
            }
            return dictionary;
        }

        /// <summary>
        /// Get the page modules
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns>A dictionary</returns>
        public static Dictionary<string, List<Control>> GetPageModules(int pageId)
        {
            var modules = GetModulesToPage(pageId);
            return ConvertModuleListToDictionary(modules);
        }
            
        /// <summary>
        /// Get Modules to page
        /// </summary>
        /// <param name="pageId">page id</param>
        /// <returns>list of module settings</returns>
        public static List<IModuleSettings> GetModulesToPage(int pageId)
        {
            var result = new List<IModuleSettings>();
            var context = new AppleseedDBContext();
            var modules = context.rb_Modules.Where(d => d.TabID == pageId);
            foreach (var rbModulese in modules)
            {
                var newmodule = ConvertRb_ModuleToModuleSettings(rbModulese, context);
                result.Add(newmodule);
            }
            return result;
        }

        private static IModuleSettings ConvertRb_ModuleToModuleSettings(rb_Modules rbModules, AppleseedDBContext context)
        {
            var guidid = new ModulesDB().GetModuleGuid(rbModules.ModuleID);
            var newmodule = new ModuleSettings
            {
                PageID = rbModules.TabID,
                ModuleID = rbModules.ModuleID,
                PaneName = rbModules.PaneName,
                ModuleTitle = rbModules.ModuleTitle,
                AuthorizedEditRoles = rbModules.AuthorizedEditRoles,
                AuthorizedViewRoles = rbModules.AuthorizedViewRoles,
                AuthorizedAddRoles = rbModules.AuthorizedAddRoles,
                AuthorizedDeleteRoles = rbModules.AuthorizedDeleteModuleRoles,
                AuthorizedPropertiesRoles = rbModules.AuthorizedPropertiesRoles,
                CacheTime = rbModules.CacheTime,
                ModuleOrder = rbModules.ModuleOrder,
                ShowMobile = rbModules.ShowMobile != null && ((rbModules.ShowMobile == null) && (bool)rbModules.ShowMobile),
                DesktopSrc = context.rb_GeneralModuleDefinitions.First(d => d.GeneralModDefID == guidid).DesktopSrc,
                //MobileSrc =  // not supported yet
                SupportCollapsable = rbModules.SupportCollapsable != null && (bool)rbModules.SupportCollapsable,
                ShowEveryWhere = rbModules.ShowEveryWhere != null && (bool)rbModules.ShowEveryWhere,
                GuidID = guidid,
            };
            return newmodule;
        }

        /// <summary>
        /// Gets the MVC action modules.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetMVCActionModules()
        {
            var dictionary = new Dictionary<string, string>();
            var info = new DirectoryInfo(Path.ApplicationPhysicalPath + "/Areas/");
            if (info.Exists) {
                var files = info.GetFiles("*.ascx", SearchOption.AllDirectories);
                foreach (var info2 in files) {
                    var key = string.Format(@"[MVC Action] {0}\{1}", info2.DirectoryName.Substring(info2.DirectoryName.LastIndexOf("MVC") + 4), info2.Name.Split(new[] { '.' })[0]);
                    if(!dictionary.ContainsKey(key))
                        dictionary.Add(key, info2.FullName);
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Registers the portable area module.
        /// </summary>
        /// <param name="areaName">Name of the area.</param>
        /// <param name="controllerName">Name of the controller</param>
        /// <param name="assemblyFullName">Full name of the assembly.</param>
        /// <returns>mid</returns>
        public static Guid RegisterPortableAreaModule(string areaName, string assemblyFullName, string controllerName) {
            Guid mId;
            var sdb = new ModulesDB();
            var friendlyName = String.Format("{0} - {1}", areaName, controllerName);
            
            try {
                mId = sdb.GetGeneralModuleDefinitionByName(friendlyName);
            }
            catch (ArgumentException) {
                // No existe el módulo, entonces lo creo
                mId = AddPortableArea(areaName, assemblyFullName, controllerName, friendlyName, sdb, "Module");
            }

            return mId;
        }

        /// <summary>
        /// Registers the portable area module.
        /// </summary>
        /// <param name="areaName">Name of the area.</param>
        /// <param name="controllerName">Name of the controller</param>
        /// <param name="assemblyFullName">Full name of the assembly.</param>
        /// <param name="actionName">The action name</param>
        /// <returns></returns>
        public static Guid RegisterPortableAreaModule(string areaName, string assemblyFullName, string controllerName, string actionName)
        {
            Guid mId;
            var sdb = new ModulesDB();
            var friendlyName = String.Format("{0} - {1}", areaName, controllerName);
            if (actionName != "Module") friendlyName += String.Format(" - {0}", actionName);
            try {
                mId = sdb.GetGeneralModuleDefinitionByName(friendlyName);
            }
            catch (ArgumentException) {
                // No existe el módulo, entonces lo creo
                mId = AddPortableArea(areaName, assemblyFullName, controllerName, friendlyName, sdb, actionName);
            }

            return mId;
        }



        /// <summary>
        /// The reorder.
        /// </summary>
        /// <param name="modulesByPane">
        /// The modules by pane.
        /// </param>
        public static void Reorder(Dictionary<string, ArrayList> modulesByPane)
        {
            // var settings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            // var pageId = settings.ActivePage.PageID;
            var sdb = new ModulesDB();
            foreach (var str in modulesByPane.Keys) {
                var list = modulesByPane[str];
                var moduleOrder = 0;
                foreach (int num3 in list) {
                    sdb.UpdateModuleOrder(num3, moduleOrder, str);
                    moduleOrder++;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the portable area.
        /// </summary>
        /// <param name="areaName">Name of the area.</param>
        /// <param name="assemblyFullName">Full name of the assembly.</param>
        /// <param name="controllerName">The module.</param>
        /// <param name="friendlyName">Name of the friendly.</param>
        /// <param name="sdb">The SDB.</param>
        /// <param name="actionName">The actionName.</param>
        /// <returns></returns>
        private static Guid AddPortableArea(string areaName, string assemblyFullName, string controllerName, string friendlyName, ModulesDB sdb, string actionName) {
            var mId = Guid.NewGuid();
            var action = string.Format("Areas/{0}/Views/{1}/{2}", areaName, controllerName, actionName);
            sdb.AddGeneralModuleDefinitions(
                mId, friendlyName, action, string.Empty, assemblyFullName, areaName, false, false);

            return mId;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generalModuleDefId"></param>
        /// <param name="portalId"></param>
        /// <returns></returns>
        public static int AddModuleToPortal(Guid generalModuleDefId, int portalId)
        {
            return ModulesDB.AddModuleDefinition(generalModuleDefId, portalId);
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="moduleDefId"></param>
       /// <param name="pageId"></param>
       /// <param name="title"></param>
       /// <param name="alsoIfExists"></param>
       /// <returns></returns>
        public static int AddModuleToPage(int moduleDefId, int pageId, string title, bool alsoIfExists)
        {
            var module = default(rb_Modules);
            using (var context = new AppleseedDBContext()) {
                module = context.rb_Modules.Where(d => d.TabID == pageId && d.ModuleDefID == moduleDefId).FirstOrDefault();
            }

            if (module == default(rb_Modules) || alsoIfExists) {
                var sdb = new ModulesDB();
                return  sdb.AddModule(pageId, 0, "ContentPane", title, moduleDefId, 0, "Admins", "All Users", "Admins", "Admins", "Admins", "Admins", "Admins", false, string.Empty, true, false, false);
            } else {
                return module.ModuleID;
            }
        }


        /// <summary>
        /// Get page by page id
        /// </summary>
        /// <param name="pageId">pageid</param>
        /// <returns>object</returns>
        public static Object GetPage(int pageId)
        {
            var context = new AppleseedDBContext();
            var page = context.rb_Pages.First(p => p.PageID == pageId);
            return page;
        }
    }
}