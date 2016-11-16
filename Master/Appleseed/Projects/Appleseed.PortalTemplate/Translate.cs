using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Appleseed.PortalTemplate.DTOs;
using System.Collections;
using System.Data.Linq;
using Appleseed.Framework.Web.UI.WebControls;
//using System.Web.UI;
using Appleseed.Framework.Settings;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Web.UI;
using Appleseed.Framework;
using Appleseed.Framework.DataTypes;

namespace Appleseed.PortalTemplate
{
    public class Translate
    {

        private int moduleIndex;
        #region rb entities into DTOs

        public PortalsDTO TranslateRb_PortalsIntoPortalDTO(rb_Portals portal)
        {
            PortalsDTO _portal = new PortalsDTO();
            _portal.AlwaysShowEditButton = portal.AlwaysShowEditButton;
            _portal.PortalAlias = portal.PortalAlias;
            _portal.PortalID = portal.PortalID;
            _portal.PortalName = portal.PortalName;
            _portal.PortalPath = portal.PortalPath;

            List<PagesDTO> pages = new List<PagesDTO>();
            foreach (rb_Pages p in portal.rb_Pages) {
                pages.Add(TranslateRb_PagesIntoPagesDTO(p));
            }
            _portal.Pages = pages;

            List<PortalSettingsDTO> settings = new List<PortalSettingsDTO>();
            foreach (rb_PortalSettings p in portal.rb_PortalSettings) {
                settings.Add(TranslateRb_PortalSettingsIntoPortalSettingsDTO(p));
            }
            _portal.PortalSettings = settings;

            return _portal;
        }

        public PortalSettingsDTO TranslateRb_PortalSettingsIntoPortalSettingsDTO(rb_PortalSettings portalSettings)
        {
            PortalSettingsDTO _portalSettings = new PortalSettingsDTO();
            _portalSettings.PortalID = portalSettings.PortalID;
            _portalSettings.SettingName = portalSettings.SettingName;
            _portalSettings.SettingValue = portalSettings.SettingValue;

            return _portalSettings;
        }

        public PagesDTO TranslateRb_PagesIntoPagesDTO(rb_Pages pages)
        {
            PagesDTO _pages = new PagesDTO();
            _pages.AuthorizedRoles = pages.AuthorizedRoles;
            _pages.MobilePageName = pages.MobilePageName;
            _pages.PageDescription = pages.PageDescription;
            _pages.PageID = pages.PageID;
            _pages.PageLayout = pages.PageLayout;
            _pages.PageName = pages.PageName;
            _pages.PageOrder = pages.PageOrder;
            _pages.PortalID = pages.PortalID;
            _pages.ShowMobile = pages.ShowMobile;

            List<ModulesDTO> modules = new List<ModulesDTO>();
            foreach (rb_Modules m in pages.rb_Modules) {
                modules.Add(TranslateRb_ModulesIntoModulesDTO(m));
            }
            _pages.Modules = modules;

            List<TabSettingsDTO> settings = new List<TabSettingsDTO>();
            foreach (rb_TabSetting s in pages.rb_TabSettings) {
                settings.Add(TranslateRb_TabSettingsIntoTabSettingsDTO(s));
            }
            _pages.TabSettings = settings;

            _pages.ParentPage = pages.rb_Page1 == null ? null : TranslateRb_PagesIntoPagesDTO(pages.rb_Page1);

            return _pages;
        }

        public TabSettingsDTO TranslateRb_TabSettingsIntoTabSettingsDTO(rb_TabSetting settings)
        {
            TabSettingsDTO _settings = new TabSettingsDTO();
            _settings.SettingName = settings.SettingName;
            _settings.SettingValue = settings.SettingValue;
            _settings.TabID = settings.TabID;
            return _settings;
        }

        public ModulesDTO TranslateRb_ModulesIntoModulesDTO(rb_Modules module)
        {
            ModulesDTO _module = new ModulesDTO();
            _module.AuthorizedAddRoles = module.AuthorizedAddRoles;
            _module.AuthorizedApproveRoles = module.AuthorizedApproveRoles;
            _module.AuthorizedDeleteModuleRoles = module.AuthorizedDeleteModuleRoles;
            _module.AuthorizedDeleteRoles = module.AuthorizedDeleteRoles;
            _module.AuthorizedEditRoles = module.AuthorizedEditRoles;
            _module.AuthorizedMoveModuleRoles = module.AuthorizedMoveModuleRoles;
            _module.AuthorizedPropertiesRoles = module.AuthorizedPropertiesRoles;
            _module.AuthorizedPublishingRoles = module.AuthorizedPublishingRoles;
            _module.AuthorizedViewRoles = module.AuthorizedViewRoles;
            _module.CacheTime = module.CacheTime;
            _module.LastEditor = module.LastEditor;
            _module.LastModified = module.LastModified;
            _module.ModuleDefID = module.ModuleDefID;
            _module.ModuleID = module.ModuleID;
            _module.ModuleOrder = module.ModuleOrder;
            _module.ModuleTitle = module.ModuleTitle;
            _module.NewVersion = module.NewVersion;
            _module.PaneName = module.PaneName;
            _module.ShowEveryWhere = module.ShowEveryWhere;
            _module.ShowMobile = module.ShowMobile;
            _module.StagingLastEditor = module.StagingLastEditor;
            _module.StagingLastModified = module.StagingLastModified;
            _module.SupportCollapsable = module.SupportCollapsable;
            _module.SupportWorkflow = module.SupportWorkflow;
            _module.TabID = module.TabID;
            _module.WorkflowState = module.WorkflowState;

            List<ModuleSettingsDTO> settings = new List<ModuleSettingsDTO>();
            foreach (rb_ModuleSettings m in module.rb_ModuleSettings) {
                settings.Add(TranslateRb_ModuleSettingsIntoModuleSettingsDTO(m));
            }
            _module.ModuleSettings = settings;
            _module.ModuleDefinitions = TranslateRb_ModuleDefinitionsIntoModuleDefinitionsDTO(module.rb_ModuleDefinition);

            Page p = new Page();
            string portalModuleName = string.Concat(Appleseed.Framework.Settings.Path.ApplicationRoot, "/", this.DesktopSources[_module.ModuleDefinitions.GeneralModDefID]);
            if (!portalModuleName.Contains("/Areas/") && !portalModuleName.StartsWith("Areas/")) {
                PortalModuleControl portalModule = (PortalModuleControl)p.LoadControl(portalModuleName);
                if (portalModule is IModuleExportable) {
                    _module.Content = ((IModuleExportable)portalModule).GetContentData(module.ModuleID);
                }
            }

            return _module;
        }

        private ModuleDefinitionsDTO TranslateRb_ModuleDefinitionsIntoModuleDefinitionsDTO(rb_ModuleDefinition definitions)
        {
            ModuleDefinitionsDTO _definitions = new ModuleDefinitionsDTO();
            _definitions.GeneralModDefID = definitions.GeneralModDefID;
            _definitions.ModuleDefID = definitions.ModuleDefID;
            _definitions.PortalID = definitions.PortalID;
            return _definitions;
        }

        public HtmlTextDTO TranslateRb_HtmlTextIntoHtmlTextDTO(rb_HtmlText html)
        {
            HtmlTextDTO _html = new HtmlTextDTO();
            _html.DesktopHtml = html.DesktopHtml;
            _html.MobileDetails = html.MobileDetails;
            _html.MobileSummary = html.MobileSummary;
            _html.ModuleID = html.ModuleID;

            return _html;
        }

        public ModuleSettingsDTO TranslateRb_ModuleSettingsIntoModuleSettingsDTO(rb_ModuleSettings settings)
        {
            ModuleSettingsDTO _settings = new ModuleSettingsDTO();
            _settings.ModuleID = settings.ModuleID;
            _settings.SettingName = settings.SettingName;
            _settings.SettingValue = settings.SettingValue;
            return _settings;
        }

        #endregion

        #region DTOs into rb entities
        public rb_Portals TranslatePortalDTOIntoRb_Portals(PortalsDTO portal)
        {
            rb_Portals _portal = new rb_Portals();
            _portal.AlwaysShowEditButton = portal.AlwaysShowEditButton;
            _portal.PortalAlias = portal.PortalAlias;
            _portal.PortalName = portal.PortalName;
            _portal.PortalPath = portal.PortalPath;
            _portal.PortalID = portal.PortalID;
            _portal.rb_Pages = new EntitySet<rb_Pages>();

            moduleIndex = 0;
            this.ContentModules = new Dictionary<int, string>();
            this.ModulesNotInserted = new Dictionary<int, string>();
            this.PageList = new Dictionary<int, int>();
            this.ModuleDefinitionsDeserialized = new Dictionary<Guid, rb_ModuleDefinition>();
            int index = 0;
            foreach (PagesDTO p in portal.Pages) {
                _portal.rb_Pages.Add(TranslatePagesDTOIntoRb_Pages(p));
                this.PageList.Add(p.PageID, index);
                index++;
            }
            foreach (rb_Pages pages in _portal.rb_Pages) {
                if (pages.ParentPageID != null) {
                    pages.rb_Page1 = _portal.rb_Pages.First(p => p.PageID == pages.ParentPageID.Value);
                }
            }

            _portal.rb_PortalSettings = new EntitySet<rb_PortalSettings>();
            foreach (PortalSettingsDTO p in portal.PortalSettings) {
                _portal.rb_PortalSettings.Add(TranslatePortalSettingsDTOIntoRb_PortalSettings(p));
            }

            return _portal;
        }

        public rb_PortalSettings TranslatePortalSettingsDTOIntoRb_PortalSettings(PortalSettingsDTO portalSettings)
        {
            rb_PortalSettings _portalSettings = new rb_PortalSettings();
            _portalSettings.PortalID = portalSettings.PortalID;
            _portalSettings.SettingName = portalSettings.SettingName;
            _portalSettings.SettingValue = portalSettings.SettingValue;

            return _portalSettings;
        }

        public rb_Pages TranslatePagesDTOIntoRb_Pages(PagesDTO pages)
        {
            rb_Pages _pages = new rb_Pages();
            _pages.AuthorizedRoles = pages.AuthorizedRoles;
            _pages.MobilePageName = pages.MobilePageName;
            _pages.PageDescription = pages.PageDescription;
            _pages.PageID = pages.PageID;
            _pages.PageLayout = pages.PageLayout;
            _pages.PageName = pages.PageName;
            _pages.PageOrder = pages.PageOrder;
            _pages.PortalID = pages.PortalID;
            _pages.ShowMobile = pages.ShowMobile;
            if (pages.ParentPage == null) {
                _pages.ParentPageID = null;
            } else {
                _pages.ParentPageID = pages.ParentPage.PageID;
            }


            _pages.rb_Modules = new EntitySet<rb_Modules>();
            foreach (ModulesDTO m in pages.Modules) {
                rb_Modules _modules = TranslateModulesDTOIntoRb_Modules(m);
                if (_modules != null) {
                    _pages.rb_Modules.Add(_modules);
                }
            }

            _pages.rb_TabSettings = new EntitySet<rb_TabSetting>();
            foreach (TabSettingsDTO s in pages.TabSettings) {
                _pages.rb_TabSettings.Add(TranslateTabSettingsDTOIntoRb_TabSettings(s));
            }


            return _pages;
        }

        public rb_TabSetting TranslateTabSettingsDTOIntoRb_TabSettings(TabSettingsDTO settings)
        {
            rb_TabSetting _settings = new rb_TabSetting();
            _settings.SettingName = settings.SettingName;
            _settings.SettingValue = settings.SettingValue;
            _settings.TabID = settings.TabID;
            return _settings;
        }

        public rb_Modules TranslateModulesDTOIntoRb_Modules(ModulesDTO module)
        {
            if (this.DesktopSources.ContainsKey(module.ModuleDefinitions.GeneralModDefID)) {

                rb_Modules _module = new rb_Modules();
                _module.AuthorizedAddRoles = module.AuthorizedAddRoles;
                _module.AuthorizedApproveRoles = module.AuthorizedApproveRoles;
                _module.AuthorizedDeleteModuleRoles = module.AuthorizedDeleteModuleRoles;
                _module.AuthorizedDeleteRoles = module.AuthorizedDeleteRoles;
                _module.AuthorizedEditRoles = module.AuthorizedEditRoles;
                _module.AuthorizedMoveModuleRoles = module.AuthorizedMoveModuleRoles;
                _module.AuthorizedPropertiesRoles = module.AuthorizedPropertiesRoles;
                _module.AuthorizedPublishingRoles = module.AuthorizedPublishingRoles;
                _module.AuthorizedViewRoles = module.AuthorizedViewRoles;
                _module.CacheTime = module.CacheTime;
                _module.LastEditor = module.LastEditor;
                _module.LastModified = module.LastModified;
                _module.ModuleDefID = module.ModuleDefID;
                _module.ModuleID = module.ModuleID;
                _module.ModuleOrder = module.ModuleOrder;
                _module.ModuleTitle = module.ModuleTitle;
                _module.NewVersion = module.NewVersion;
                _module.PaneName = module.PaneName;
                _module.ShowEveryWhere = module.ShowEveryWhere;
                _module.ShowMobile = module.ShowMobile;
                _module.StagingLastEditor = module.StagingLastEditor;
                _module.StagingLastModified = module.StagingLastModified;
                _module.SupportCollapsable = module.SupportCollapsable;
                _module.SupportWorkflow = module.SupportWorkflow;
                _module.TabID = module.TabID;
                _module.WorkflowState = module.WorkflowState;

                _module.rb_ModuleSettings = new EntitySet<rb_ModuleSettings>();
                foreach (ModuleSettingsDTO m in module.ModuleSettings) {
                    _module.rb_ModuleSettings.Add(TranslateModuleSettingsDTOIntoRb_ModuleSettings(m));
                }

                if (this.ModuleDefinitionsDeserialized.ContainsKey(module.ModuleDefinitions.GeneralModDefID)) {
                    rb_ModuleDefinition def = this.ModuleDefinitionsDeserialized[module.ModuleDefinitions.GeneralModDefID];
                    _module.rb_ModuleDefinition = def;
                } else {
                    _module.rb_ModuleDefinition = TranslateModuleDefinitionsDTOIntoRb_ModuleDefinitions(module.ModuleDefinitions);
                    this.ModuleDefinitionsDeserialized.Add(module.ModuleDefinitions.GeneralModDefID, _module.rb_ModuleDefinition);
                }

                Page p = new Page();
                string portalModuleName = string.Concat(Appleseed.Framework.Settings.Path.ApplicationRoot, "/", this.DesktopSources[_module.rb_ModuleDefinition.GeneralModDefID]);
                if (!portalModuleName.Contains("/Areas/") && !portalModuleName.StartsWith("Areas/")) {
                    PortalModuleControl portalModule = (PortalModuleControl)p.LoadControl(portalModuleName);

                    if (portalModule is IModuleExportable) {
                        this.ContentModules.Add(moduleIndex, module.Content);
                        //((IModuleExportable)portalModule).SetContentData(modules.ModuleID, modules.Content, this.PTDataContext);
                    }
                }
                moduleIndex++;

                return _module;

            } else {
                moduleIndex++;
                this.ModulesNotInserted.Add(module.ModuleID, module.ModuleTitle);
                return null;
            }
        }

        public rb_HtmlText TranslateHtmlTextDTOIntoRb_HtmlText(HtmlTextDTO html)
        {
            rb_HtmlText _html = new rb_HtmlText();
            _html.DesktopHtml = html.DesktopHtml;
            _html.MobileDetails = html.MobileDetails;
            _html.MobileSummary = html.MobileSummary;
            _html.ModuleID = html.ModuleID;
            _html.CWCSS = html.CWCSS;
            _html.CWHTML = html.CWHTML;
            _html.CWJS = html.CWJS;
            return _html;
        }

        public rb_HtmlText_st TranslateHtmlTextDTOIntoRb_HtmlText_st(HtmlTextDTO html)
        {
            rb_HtmlText_st _html = new rb_HtmlText_st();
            _html.DesktopHtml = html.DesktopHtml;
            _html.MobileDetails = html.MobileDetails;
            _html.MobileSummary = html.MobileSummary;
            _html.ModuleID = html.ModuleID;
            _html.CWCSS = html.CWCSS;
            _html.CWHTML = html.CWHTML;
            _html.CWJS = html.CWJS;
            return _html;
        }


        public rb_ModuleSettings TranslateModuleSettingsDTOIntoRb_ModuleSettings(ModuleSettingsDTO settings)
        {
            rb_ModuleSettings _settings = new rb_ModuleSettings();
            _settings.ModuleID = settings.ModuleID;
            _settings.SettingName = settings.SettingName;
            _settings.SettingValue = settings.SettingValue;
            return _settings;
        }


        public rb_ModuleDefinition TranslateModuleDefinitionsDTOIntoRb_ModuleDefinitions(ModuleDefinitionsDTO definitions)
        {
            rb_ModuleDefinition _definitions = new rb_ModuleDefinition();
            _definitions.GeneralModDefID = definitions.GeneralModDefID;
            _definitions.ModuleDefID = definitions.ModuleDefID;
            _definitions.PortalID = definitions.PortalID;

            return _definitions;
        }

        #endregion

        #region properties
        public Dictionary<Guid, string> DesktopSources
        {
            get;
            set;
        }

        public Dictionary<int, string> ModulesNotInserted
        {
            get;
            set;
        }

        public PortalTemplateDataContext PTDataContext
        {
            get;
            set;
        }

        public Dictionary<int, string> ContentModules
        {
            get;
            set;
        }

        public Dictionary<int, int> PageList
        {
            get;
            set;
        }

        public Dictionary<Guid, rb_ModuleDefinition> ModuleDefinitionsDeserialized
        {
            get;
            set;
        }

        #endregion

    }
}
