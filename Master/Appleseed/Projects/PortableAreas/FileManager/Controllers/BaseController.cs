using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using Appleseed.Framework;
using Appleseed.Framework.Site.Configuration;
using FileManager.Models;

namespace FileManager.Controllers
{
    public class BaseController : Controller
    {
        private AreaContext _context;
        private Dictionary<string, ISettingItem> _moduleSettings;
        private int _moduleId = -1;


        public virtual Dictionary<string, ISettingItem> Module_SettingsDefinitions() {
            return new Dictionary<string, ISettingItem>();
        }


        protected AreaContext Context {
            get {
                if (_context == null) {
                    var portalId = this.PortalSettings == null ? 1 : this.PortalSettings.PortalID;
                    var userId = Guid.Empty;
                    try {
                        var user = Membership.GetUser();
                        if (user != null) {
                            userId = (Guid)user.ProviderUserKey;
                        }
                    }
                    catch {
                        //por ahora no hago nada en el catch
                    }
                    _context = new AreaContext(portalId, userId);
                }
                return _context;
            }
        }


        private Dictionary<string, ISettingItem> ModuleSettings {
            get {
                if (_moduleId < 0) {
                    return Module_SettingsDefinitions();
                }
                if (_moduleSettings == null) {
                    try {
                        _moduleSettings = Appleseed.Framework.Site.Configuration.ModuleSettings.GetModuleSettings(_moduleId, Module_SettingsDefinitions());
                    }
                    catch {
                        return Module_SettingsDefinitions();
                    }
                }
                return _moduleSettings;
            }
        }


        protected PortalSettings PortalSettings {
            get {
                return (PortalSettings)HttpContext.Items["PortalSettings"];
            }
        }

        protected string GetSettingValue(string settingName)
        {
            if (ControllerContext.RouteData.Values[settingName] == null)
            {
                return ModuleSettings[settingName].ToString();
            }
            var settingValue = (ISettingItem) ControllerContext.RouteData.Values[settingName];
            return settingValue.Value == null ? null : settingValue.ToString();
        }


        protected void AddSettingToViewData(string settingName, object defaultValue) {
            if (ControllerContext.RouteData.Values[settingName] != null) {
                var settingValue = (ISettingItem)ControllerContext.RouteData.Values[settingName];
                ViewData[settingName] = settingValue.Value == null ? null : settingValue.ToString();
            }
            else if (ModuleSettings[settingName] != null) {
                var settingValue = ModuleSettings[settingName];
                if (settingValue == null) {
                    ViewData[settingName] = defaultValue.ToString();
                }
                else {
                    ViewData[settingName] = settingValue.ToString();
                }
            }
            else {
                ViewData[settingName] = defaultValue.ToString();
            }
        }


        protected void SetModuleId(int mid) {
            _moduleId = mid;
        }

        protected void SetModuleId() {
            if (ControllerContext.RouteData.Values["moduleId"] != null) {
                _moduleId = (int) ControllerContext.RouteData.Values["moduleId"];
            }
        }

        protected void AddModuleIdToViewData() {
            if (ControllerContext.RouteData.Values["moduleId"] != null) {
                ViewData["ModuleID"] = ControllerContext.RouteData.Values["moduleId"];
            }
            else {
                ViewData["ModuleID"] = _moduleId;
            }
        }

        protected int ModuleId
        {
            get { return _moduleId; }
        }
    


    }
}
