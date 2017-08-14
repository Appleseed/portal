// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortalTemplateServices.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   The portal template services.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.PortalTemplate
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.UI;
    using System.Xml.Serialization;
    using Ionic.Zip;

    using Appleseed.Framework;
    using Appleseed.Framework.DataTypes;
    using Appleseed.Framework.Web.UI.WebControls;
    using Appleseed.PortalTemplate.DTOs;
    

    using Path = Appleseed.Framework.Settings.Path;
    using System.Configuration;
    

    /// <summary>
    /// The portal template services.
    /// </summary>
    public class PortalTemplateServices : IPortalTemplateServices
    {
        #region Constants and Fields

        /// <summary>
        /// The ipt repository.
        /// </summary>
        private readonly IPortalTemplateRepository iptRepository;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PortalTemplateServices"/> class.
        /// </summary>
        /// <param name="iptRepository">
        /// The ipt repository.
        /// </param>
        public PortalTemplateServices(IPortalTemplateRepository iptRepository)
        {
            this.iptRepository = iptRepository;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the modules not inserted.
        /// </summary>
        /// <value>
        ///   The modules not inserted.
        /// </value>
        public Dictionary<int, string> ModulesNotInserted { get; set; }

        #endregion

        #region Implemented Interfaces

        #region IPortalTemplateServices

        /// <summary>
        /// Deserializes the portal.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="portalName">
        /// Name of the portal.
        /// </param>
        /// <param name="portalAlias">
        /// The portal alias.
        /// </param>
        /// <param name="portalPath">
        /// The portal path.
        /// </param>
        /// <param name="portalId">
        /// The portal id.
        /// </param>
        /// <returns>
        /// The deserialize portal.
        /// </returns>
        public bool DeserializePortal(
            string file, string portalName, string portalAlias, string portalPath, string filePath, out int portalId)
        {
            var result = true;
            try {
                PortalsDTO portal;
                if (file.EndsWith("AppleSeedTemplates")) {
                    using (var ms = new MemoryStream()) {
                        using (ZipFile zip = ZipFile.Read(GetPhysicalPackageTemplatesPath(filePath) + "\\" + file)) {
                            if (zip.Count == 1) {
                                ms.Position = 0;
                                string name = file.Replace(".AppleSeedTemplates", ".XML");
                                ZipEntry e = zip[name];
                                e.Extract(ms);

                                //FileStream s = new FileStream(GetPhysicalPackageTemplatesPath("") + "\\" + name, FileMode.Create);
                                //int pos = int.Parse(ms.Position.ToString());
                                //s.Write(ms.GetBuffer(), 0, pos);
                                //s.Close();

                                ms.Position = 0;

                                var xs = new XmlSerializer(typeof(PortalsDTO));
                                portal = (PortalsDTO)xs.Deserialize(ms);

                                ms.Close();
                                
                            } else {
                                portal = null;
                            }
                             
                        }
                        // the application can now access the MemoryStream here
                    }

                } else {
                    var fs = new FileStream(GetPhysicalPackageTemplatesPath(filePath) + "\\" + file, FileMode.Open);
                    var xs = new XmlSerializer(typeof(PortalsDTO));
                    portal = (PortalsDTO)xs.Deserialize(fs);
                    fs.Close();
                }

                var db = new PortalTemplateDataContext(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                var desktopSources = this.iptRepository.GetDesktopSources();

                var translate = new Translate { DesktopSources = desktopSources, PTDataContext = db };
                var _portal = translate.TranslatePortalDTOIntoRb_Portals(portal);
                this.ModulesNotInserted = translate.ModulesNotInserted;

                _portal.PortalName = portalName;
                _portal.PortalPath = portalPath;
                _portal.PortalAlias = portalAlias;

                db.rb_Portals.InsertOnSubmit(_portal);

                db.SubmitChanges(ConflictMode.FailOnFirstConflict);
                portalId = _portal.PortalID;
                this.SaveModuleContent(_portal, desktopSources, translate.ContentModules);
                AlterModuleSettings(_portal, translate.PageList, desktopSources);
                AlterModuleDefinitions(_portal.PortalID, translate.ModuleDefinitionsDeserialized);


            } catch (Exception ex) {
                result = false;
                portalId = -1;
                ErrorHandler.Publish(LogLevel.Error, "There was an error on creating the portal", ex);
            }

            return result;
        }

        /// <summary>
        /// Gets the HTML text DTO.
        /// </summary>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <returns>
        /// </returns>
        public HtmlTextDTO GetHtmlTextDTO(int moduleId)
        {
            return this.iptRepository.GetHtmlTextDTO(moduleId);
        }

        /// <summary>
        /// Saves the HTML text.
        /// </summary>
        /// <param name="moduleId">
        /// The module id.
        /// </param>
        /// <param name="html">
        /// The HTML.
        /// </param>
        /// <returns>
        /// The save html text.
        /// </returns>
        public bool SaveHtmlText(int moduleId, HtmlTextDTO html)
        {
            var result = true;
            try {
                var db = new PortalTemplateDataContext(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                var translate = new Translate();
                var htmlText = translate.TranslateHtmlTextDTOIntoRb_HtmlText(html);
                htmlText.ModuleID = moduleId;
                var htmlst = translate.TranslateHtmlTextDTOIntoRb_HtmlText_st(html);
                htmlst.ModuleID = moduleId;
                db.rb_HtmlTexts.InsertOnSubmit(htmlText);
                db.rb_HtmlText_sts.InsertOnSubmit(htmlst);
                db.SubmitChanges(ConflictMode.FailOnFirstConflict);
            } catch (Exception ex) {
                result = false;
                ErrorHandler.Publish(LogLevel.Error, "There was an error saving the content modules", ex);
            }

            return result;
        }

        /// <summary>
        /// Serializes the portal.
        /// </summary>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The serialize portal.
        /// </returns>
        public bool SerializePortal(int portalId, string portalAlias, string portalFullPath)
        {
            var result = true;
            try {
                string path = GetPhysicalPackageTemplatesPath(portalFullPath);
                
               
                var portal = this.iptRepository.GetPortal(portalId);
                var dir = new DirectoryInfo(path);
                if (!dir.Exists) {
                    dir.Create();
                }

                // Create the xmlFile
                string filePath = path + "\\" + portal.PortalAlias + "-" + DateTime.Now.ToString("dd-MM-yyyy") + ".XML";
                //var fs = new FileStream(filePath
                //    , FileMode.Create);
                //var xs = new XmlSerializer(typeof(PortalsDTO));
                //xs.Serialize(fs, portal);
                //fs.Close();

                Stream s = new MemoryStream();
                var xs = new XmlSerializer(typeof(PortalsDTO));
                xs.Serialize(s, portal);

                using (ZipFile zip = new ZipFile()) {
                    //zip.AddFile(filePath);
                    s.Position = 0;
                    string name = portal.PortalAlias + "-" + DateTime.Now.ToString("dd-MM-yyyy") + ".XML";
                    zip.AddEntry(name, s);
                   
                    zip.Save(Regex.Replace(filePath,@".XML",".AppleSeedTemplates"));
                }

                s.Close();


            } catch (Exception ex) {
                result = false;
                ErrorHandler.Publish(LogLevel.Error, "There was an error on serialize the portal", ex);
            }

            return result;
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Alters the module definitions.
        /// </summary>
        /// <param name="portalId">
        /// The portal id.
        /// </param>
        /// <param name="moduleDefinitions">
        /// The module definitions.
        /// </param>
        private static void AlterModuleDefinitions(int portalId, Dictionary<Guid, rb_ModuleDefinition> moduleDefinitions)
        {
            var db = new PortalTemplateDataContext(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            var keys = moduleDefinitions.Keys.ToList();
            try {
                foreach (var key in keys) {
                    rb_ModuleDefinition oldDef;
                    moduleDefinitions.TryGetValue(key, out oldDef);
                    var def = db.rb_ModuleDefinitions
                        .Where(d => d.PortalID == oldDef.PortalID && d.GeneralModDefID == key && d.ModuleDefID == oldDef.ModuleDefID) /*ModuleDefID is already updated so must match*/
                        .OrderByDescending(d => d.ModuleDefID)
                        .First(); /*to avoid possible duplicates, Last() is not supported*/

                    def.PortalID = portalId;
                }

                db.SubmitChanges(ConflictMode.FailOnFirstConflict);
            } catch (Exception e) {
                ErrorHandler.Publish(LogLevel.Error, "There was an error on modifying the module definitions", e);
            }
        }

        /// <summary>
        /// Alters the module settings.
        /// </summary>
        /// <param name="portal">
        /// The portal.
        /// </param>
        /// <param name="pageList">
        /// The page list.
        /// </param>
        /// <param name="desktopSources">
        /// The desktop sources.
        /// </param>
        private static void AlterModuleSettings(
            rb_Portals portal, IDictionary<int, int> pageList, IDictionary<Guid, string> desktopSources)
        {
            var db = new PortalTemplateDataContext(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            var modules = db.rb_Modules.Where(m => m.rb_Pages.PortalID == portal.PortalID).ToList();
            AlterModuleSettingsAux(modules, portal, pageList, desktopSources);

        }

        private static void AlterModuleSettingsPage(rb_Portals portal,
                        rb_Pages pages, IDictionary<int, int> pageList, IDictionary<Guid, string> desktopSources)
        {

            var db = new PortalTemplateDataContext(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            var modules = db.rb_Modules.Where(m => m.rb_Pages.PageID == pages.PageID).ToList();
            AlterModuleSettingsAux(modules, portal, pageList, desktopSources);

        }

        private static void AlterModuleSettingsAux(
                IEnumerable<rb_Modules> modules, rb_Portals portal, IDictionary<int, int> pageList, IDictionary<Guid, string> desktopSources)
        {
            var p = new Page();
            var db = new PortalTemplateDataContext(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            foreach (var module in modules)
            {
                var portalModuleName = string.Concat(
                    Path.ApplicationRoot, "/", desktopSources[module.rb_ModuleDefinition.GeneralModDefID]);
                if (!portalModuleName.Contains("/Areas/") && !portalModuleName.StartsWith("Areas/"))
                {
                    var portalModule = (PortalModuleControl)p.LoadControl(portalModuleName);
                    foreach (var key in
                        portalModule.BaseSettings.Keys.Cast<string>().Where(
                            key =>
                            key.Equals("TARGETURL") ||
                            portalModule.BaseSettings[key] is PageListDataType))
                    {
                        try
                        {
                            var setting = module.rb_ModuleSettings.First(s => s.SettingName.Equals(key));
                            var oldPageId =
                                Regex.Match(setting.SettingValue, "(/\\d+/)|(^\\d+$)", RegexOptions.IgnoreCase).Value.
                                    Replace("/", string.Empty);
                            var newPageId = portal.rb_Pages[pageList[Convert.ToInt16(oldPageId)]].PageID;
                            setting.SettingValue = setting.SettingValue.Replace(oldPageId, newPageId.ToString());
                        }
                        catch (Exception e)
                        {
                            ErrorHandler.Publish(
                                LogLevel.Error,
                                string.Format(
                                    "There was an error on modifying the module settings for moduleID= {0} and setting= {1}",
                                    module.ModuleID,
                                    key),
                                e);
                        }
                    }
                }
            }

            try
            {
                db.SubmitChanges(ConflictMode.FailOnFirstConflict);
            }
            catch (Exception e)
            {
                ErrorHandler.Publish(LogLevel.Error, "There was an error on modifying the module settings", e);
            }
        }

        /// <summary>
        /// Saves the content of the module.
        /// </summary>
        /// <param name="portal">
        /// The portal.
        /// </param>
        /// <param name="desktopSources">
        /// The desktop sources.
        /// </param>
        /// <param name="contentModules">
        /// The content modules.
        /// </param>
        private void SaveModuleContent(
            rb_Portals portal, IDictionary<Guid, string> desktopSources, IDictionary<int, HtmlTextDTO> contentModules)
        {
            var modules = portal.rb_Pages.SelectMany(page => page.rb_Modules);
            SaveModuleContentAux(modules, desktopSources, contentModules);

        }

        private void SaveModuleContentPage(
            rb_Pages pages, IDictionary<Guid, string> desktopSources, IDictionary<int, HtmlTextDTO> contentModules)
        {
            var modules = pages.rb_Modules;
            SaveModuleContentAux(modules, desktopSources, contentModules);

        }

        private void SaveModuleContentAux(IEnumerable<rb_Modules> modules, IDictionary<Guid, string> desktopSources, IDictionary<int, HtmlTextDTO> contentModules)
        {
            var p = new Page();
            var moduleIndex = 0;
            foreach (var module in modules)
            {
                if (contentModules.ContainsKey(moduleIndex))
                {
                    var portalModuleName = string.Concat(
                        Path.ApplicationRoot, "/", desktopSources[module.rb_ModuleDefinition.GeneralModDefID]);
                    if (!portalModuleName.Contains("/Areas/") && !portalModuleName.StartsWith("Areas/"))
                    {
                        var portalModule = (PortalModuleControl)p.LoadControl(portalModuleName);
                        if (portalModule is IModuleExportable)
                        {
                            HtmlTextDTO content;
                            if (!contentModules.TryGetValue(moduleIndex, out content) ||
                                !((IModuleExportable)portalModule).SetContentData(module.ModuleID, content))
                            {
                                this.ModulesNotInserted.Add(module.ModuleID, module.ModuleTitle);
                            }
                        }
                    }
                }

                moduleIndex++;
            }
        }

        #endregion


        public List<string> GetTemplates(string portalAlias, string portalFullPath)
        {
            List<string> result = new List<string>();
            string path = GetPhysicalPackageTemplatesPath(portalFullPath);

            if (Directory.Exists(path)) {
                DirectoryInfo directory = new DirectoryInfo(path);
                FileInfo[] templates = directory.GetFiles("*.xml");
                foreach (FileInfo template in templates) {
                    result.Add(template.Name);
                }
                templates = directory.GetFiles("*.AppleSeedTemplates");
                foreach (FileInfo template in templates) {
                    result.Add(template.Name);
                }
            }
            return result;
        }

        private string GetPhysicalPackageTemplatesPath(string portalFullPath)
        {
            string path = Appleseed.Framework.Settings.Path.ApplicationPhysicalPath;
            path = string.Format(@"{0}{1}\PortalTemplates", path, portalFullPath);
            path = path.Replace("/", @"\");
            return path;
        }


        public void DeleteTemplate(string templateName, string portalFullPath)
        {
            File.Delete(GetPhysicalPackageTemplatesPath(portalFullPath) + "\\" + templateName);
        }

        public byte[] GetTemplate(string templateName, string portalFullPath)
        {
            byte[] fileBytes = File.ReadAllBytes(GetPhysicalPackageTemplatesPath(portalFullPath) + "\\" + templateName);

            return fileBytes;
        }

        public FileInfo GetTemplateInfo(string templateName, string portalFullPath)
        {
            FileInfo fInfo = new FileInfo(GetPhysicalPackageTemplatesPath(portalFullPath) + "\\" + templateName);

            return fInfo;
        }

        public int CopyPage(int id, string name)
        {
            var ok = -1;
            var db = new PortalTemplateDataContext(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            var desktopSources = this.iptRepository.GetDesktopSources();

            var translate = new Translate { DesktopSources = desktopSources, PTDataContext = db };
            IList < rb_Pages > pagelist = (from p in db.rb_Pages where p.PageID == id select p).ToList<rb_Pages>();
            var page = (pagelist.Count == 0) ? null: translate.TranslateRb_PagesIntoPagesDTO(pagelist[0]);
            if (page != null)
            {
                this.ModulesNotInserted = new Dictionary<int, string>();
                translate.ContentModules = new Dictionary<int, HtmlTextDTO>();
                translate.ModuleDefinitionsDeserialized = new Dictionary<Guid, rb_ModuleDefinition>();
                
                page.Modules.RemoveAll(m=> m.ShowEveryWhere == true);
                var newpage = translate.TranslatePagesDTOIntoRb_Pages(page);
                newpage.PageName = name;

                db.rb_Pages.InsertOnSubmit(newpage);
                db.SubmitChanges(ConflictMode.FailOnFirstConflict);

                IList < rb_Portals > portallist = (from p in db.rb_Portals where p.PortalID == newpage.PortalID select p).ToList<rb_Portals>();
                var portal = portallist[0];
                translate.PageList = new Dictionary<int, int> { { newpage.PageID, 0 } };

                this.SaveModuleContentPage(newpage, desktopSources, translate.ContentModules);
                AlterModuleSettingsPage(portal, newpage, translate.PageList, desktopSources);
                AlterModuleDefinitions(portal.PortalID, translate.ModuleDefinitionsDeserialized);
                ok = newpage.PageID;
            }

            return ok;
        }
    }
}