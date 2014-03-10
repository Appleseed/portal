using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Appleseed.Framework.Site.Data;
using Appleseed.Framework;
using Appleseed.Framework.Site.Configuration;
using System.Web.UI;
using System.Data.SqlClient;
using Appleseed.Framework.Settings.Cache;
using Appleseed.Framework.Security;
using System.Web.UI.WebControls;
using Appleseed.PortalTemplate;
using PageManagerTree.Models;
using PageManagerTree.Massive;
using rb_ModuleSettings = PageManagerTree.Massive.rb_ModuleSettings;
using rb_Modules = PageManagerTree.Massive.rb_Modules;
using rb_Pages = PageManagerTree.Massive.rb_Pages;
using Appleseed.Framework.Core.Model;

namespace PageManagerTree.Controllers
{
    public class PageManagerTreeController : Controller
    {
        //
        // GET: /PageManagerTree/

        public ActionResult Index()
        {
           
            return View();
        }

        protected List<PageItem> PortalPages { get; set; }

        public PortalSettings PortalSettings
        {
            get
            {
                // Obtain PortalSettings from page else Current Context else null
                return (PortalSettings)HttpContext.Items["PortalSettings"];
            }
        }


        /// <summary>
        /// The OrderPages helper method is used to reset
        ///   the display order for tabs within the portal
        /// </summary>
        /// <remarks>
        /// </remarks>
        private void OrderPages()
        {
            var i = 1;
            this.PortalPages = new PagesDB().GetPagesFlat(this.PortalSettings.PortalID);
            this.PortalPages.Sort();

            foreach (var t in this.PortalPages)
            {
                // number the items 1, 3, 5, etc. to provide an empty order
                // number when moving items up and down in the list.
                t.Order = i;
                i += 2;

                // rewrite tab to database
                var tabs = new PagesDB();

                // 12/16/2002 Start - Cory Isakson 
                tabs.UpdatePageOrder(t.ID, t.Order);

                // 12/16/2002 End - Cory Isakson 
            }
            CurrentCache.RemoveAll("_PageNavigationSettings_");
        }


        public ActionResult Module()
        {
            return View();
        }


        public JsTreeModel[] getChildrenTree(PageItem page)
        {
            List<PageStripDetails> childPages = new PagesDB().GetPagesinPage(this.PortalSettings.PortalID, page.ID);
            int count = 0;
            List<JsTreeModel> lstTree = new List<JsTreeModel>();

            foreach (PageStripDetails childPage in childPages)
            {
                PageItem aux = new PageItem ();
                aux.ID = childPage.PageID;
                aux.Name = childPage.PageName;
                
                //JsTreeModel[] childs = getChildrenTree(aux);
                JsTreeModel node = new JsTreeModel
                {
                    data = aux.Name,
                    attr = new JsTreeAttribute { id = "pjson_" + aux.ID.ToString()},
                    //children = childs,
                    state = "closed"
                };

                lstTree.Add(node);
                count++;
            }
            JsTreeModel[] tree = lstTree.ToArray<JsTreeModel>();

            return tree;
        }

        public JsonResult GetTreeData()
        {
            List<PageItem> pages = new PagesDB().GetPagesFlat(this.PortalSettings.PortalID);
            List<JsTreeModel> lstTree = new List<JsTreeModel>();
            foreach (PageItem page in pages)
            {
                if (page.NestLevel == 0)
                {
                    //JsTreeModel[] child = getChildrenTree(page);
                    JsTreeModel node = new JsTreeModel {
                                                    data = page.Name,
                                                    attr = new JsTreeAttribute { id = "pjson_" + page.ID.ToString()},
                                                    //children = child, 
                                                    state = "closed"};
                    lstTree.Add(node);
                }
            }
            int root = 0;
            JsTreeModel rootNode = new JsTreeModel
            {
                data = "Root",
                attr = new JsTreeAttribute { id = "pjson_" + root.ToString(), rel = "root"},
                children = lstTree.ToArray<JsTreeModel>(),
            };

            return Json(rootNode);
        }


        public JsonResult remove(int id)
        {
            try
            {
                var tabs = new PagesDB();
                tabs.DeletePage(id);

                return Json(new {error = false});
            }
            catch (SqlException)
            {
                string errorMessage = General.GetString("TAB_DELETE_FAILED", "Failed to delete Page", this);
                return Json(new { error = true, errorMess = errorMessage });
            }
        }

        public JsonResult RemoveModule(int id)
        {
            string errorMessage = General.GetString("MODULE_DELETE_FAILED", "You don't have permission to delete this module", this);
            try
            {
                if (PortalSecurity.IsInRoles(PortalSecurity.GetDeleteModulePermissions(id)))
                {
                    // must delete from database too
                    var moddb = new ModulesDB();
                    moddb.DeleteModule(id);
                    return Json(new {error = false});
                }
                else
                {
                    return Json(new { error = true, errorMess = errorMessage });
                }
            }
            catch (SqlException)
            {
                return Json(new { error = true, errorMess = errorMessage });
            }
        }

        public JsonResult Clone(int id, int parentId)
        {
            try
            {
                var generalModuleDef = Guid.Parse("F9F9C3A4-6E16-43B4-B540-984DDB5F1CD2");
                object[] queryargs = { generalModuleDef, PortalSettings.PortalID };

                int moduleDefinition;

                try
                {
                    moduleDefinition =
                        new rb_ModuleDefinitions().All(where: "GeneralModDefID = @0 and PortalID = @1", args: queryargs).Single().ModuleDefID;
                }
                catch(Exception e)
                {
                    // Shortcut module doesn't exist in current Portal
                    
                    var modules = new ModulesDB();
                    
                    modules.UpdateModuleDefinitions(
                            generalModuleDef,
                            PortalSettings.PortalID,
                            true);

                    moduleDefinition =
                        new rb_ModuleSettings().All(where: "GeneralModDefID = @0 and PortalID = @1", args: queryargs).Single().ModuleDefID;

                }

                var db = new PagesDB();

                PortalPages = db.GetPagesFlat(PortalSettings.PortalID);
                var t = new PageItem
                            {
                                Name = General.GetString("TAB_NAME", "New Page Name"),
                                ID = -1,
                                Order = 990000
                            };

                PortalPages.Add(t);

                var tabs = new PagesDB();
                t.ID = tabs.AddPage(PortalSettings.PortalID, t.Name, t.Order);

                db.UpdatePageParent(t.ID, parentId, PortalSettings.PortalID);

                OrderPages();
                //JsonResult treeData = GetTreeData();

                // Coping Modules

                

                var pagesModules = new rb_Modules().All(where: "TabID = @0", args: id);

                foreach (var module in pagesModules)
                {
                    var m = new ModuleItem();
                    m.Title = module.ModuleTitle;
                    m.ModuleDefID = moduleDefinition;
                    m.Order = module.ModuleOrder;

                    // save to database
                    var mod = new ModulesDB();

                    m.ID = mod.AddModule(
                        t.ID,
                        m.Order,
                        module.PaneName,
                        module.ModuleTitle,
                        m.ModuleDefID,
                        0,
                        module.AuthorizedEditRoles,
                        module.AuthorizedViewRoles,
                        module.AuthorizedAddRoles,
                        module.AuthorizedDeleteRoles,
                        module.AuthorizedPropertiesRoles,
                        module.AuthorizedMoveModuleRoles,
                        module.AuthorizedDeleteModuleRoles,
                        false,
                        PortalSecurity.GetDeleteModulePermissions(module.ModuleID),
                        false,
                        false,
                        false);

                    var settings = new rb_ModuleSettings();
                    settings.Insert(new { ModuleID = m.ID, SettingName = "LinkedModule", SettingValue = module.ModuleID });

                }

               

                return Json(new {pageId = t.ID});
            }
            catch(Exception e)
            {
                ErrorHandler.Publish(LogLevel.Error, e);
                Response.StatusCode = 500;
                return Json("");
            }
        }


        public JsonResult create(int id)
        {
            PagesDB db = new PagesDB();
            
            this.PortalPages = db.GetPagesFlat(this.PortalSettings.PortalID);
            var t = new PageItem
            {
                Name = General.GetString("TAB_NAME", "New Page Name"),
                ID = -1,
                Order = 990000
            };

            this.PortalPages.Add(t);

            var tabs = new PagesDB();
            t.ID = tabs.AddPage(this.PortalSettings.PortalID, t.Name, t.Order);

            db.UpdatePageParent(t.ID, id, this.PortalSettings.PortalID);

            this.OrderPages();
            //JsonResult treeData = GetTreeData();
            return Json("");
        }


        public JsonResult edit(int id)
        {
            ModulesDB modules = new ModulesDB();
            Guid TabGuid = new Guid("{1C575D94-70FC-4A83-80C3-2087F726CBB3}");
            int TabModuleID = 0;
            foreach (ModuleItem m in modules.FindModuleItemsByGuid(PortalSettings.PortalID, TabGuid)) {
                bool HasEditPermissionsOnTabs = PortalSecurity.HasEditPermissions(m.ID);
                if (HasEditPermissionsOnTabs) {
                    TabModuleID = m.ID;
                    break;
                }
            }

            string dir = HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Pages/PageLayout.aspx?PageID="+ id.ToString() +
                    "&mID=" + TabModuleID + "&Alias=" + this.PortalSettings.PortalAlias + "&returntabid=" +
                    this.PortalSettings.ActiveModule);
            return Json(new { url = dir});
        }

        private List<PageItem> getPagesInLevel(int pageInlevel)
        {
            List<PageItem> pages = new List<PageItem>();

            int level = 0;
            foreach (PageItem page in this.PortalPages)
            {
                if (page.ID == pageInlevel)
                    level = page.NestLevel;
            }

            foreach (PageItem p in this.PortalPages)
            {
                if (p.NestLevel == level)
                    pages.Add(p);
            }
            return pages;
        }

        private int getPageOrder(int idToSearch)
        {
            List<PageItem> pages = new PagesDB().GetPagesFlat(this.PortalSettings.PortalID);

            while (pages.Count > 0)
            {
                PageItem page = pages.First<PageItem>();
                pages.Remove(page);
                if (page.ID == idToSearch)
                {
                    return page.Order;
                }
            }
            return -1;
        }

        public void moveNode(int pageID, int newParent, int idOldNode)
        {
            PagesDB db = new PagesDB();
            this.PortalPages = db.GetPagesFlat(this.PortalSettings.PortalID);

            db.UpdatePageParent(pageID, newParent, this.PortalSettings.PortalID);
            int order;
            if (idOldNode == -1)
            {
                order = 9999;
            }
            else
            {
                order = this.getPageOrder(idOldNode) - 1;
            }

            db.UpdatePageOrder(pageID, order);
            this.OrderPages();
        }

        public void MoveModuleNode(int pageId, int moduleId, string paneName)
        {
            rb_Modules db = new rb_Modules();
            var module = db.Single(moduleId);
            module.TabID = pageId;
            module.PaneName = paneName.TrimEnd();
            db.Update(module, moduleId);
        }

        public JsonResult Rename(int id, string name) {

            try {
                var db = new rb_Pages();
                var page = db.Single(id);
                page.PageName = name;
                db.Update(page, page.PageID);


                //string sql = @"UPDATE rb_Pages SET [] = @0 WHERE PageID = @1";

                //object[] queryargs = { name, id };

                //var t = new .Query(sql, queryargs);
            }
            catch (Exception ) {
                return Json(new { error = true });
            }

            return Json(new { error = false }); 
        }

        public JsonResult RenameModule(int id, string name)
        {
            try
            {
                var db = new rb_Modules();
                var module = db.Single(id);
                module.ModuleTitle = name;
                db.Update(module, module.ModuleID);
            }
            catch (Exception)
            {
                return Json(new { error = true });
            }

            return Json(new { error = false });
        }

        public JsonResult ViewPage(int pageId)
        {
            var url = HttpUrlBuilder.BuildUrl(pageId);
            return Json(url);
        }

        public JsonResult CopyPage(int pageId, string name)
        {
            IPortalTemplateServices services = PortalTemplateFactory.GetPortalTemplateServices(new PortalTemplateRepository());
            int newpageid = services.CopyPage(pageId, name + "- Copy");
            return Json(new { pageId = newpageid });
        }

        public JsTreeModel[] getModuleToPane(string paneName, int pageId)
        {
            var pageModulesByPlaceHolder = ModelServices.GetModulesToPage(pageId);
            var result = new List<JsTreeModel>();
            var modulelist = pageModulesByPlaceHolder.Where(m => m.PaneName.ToLower() == paneName.ToLower());
            int i = 0;
            foreach (var module in modulelist)
            {
                JsTreeModel node = new JsTreeModel
                {
                    data = module.ModuleTitle,
                    attr = new JsTreeAttribute { id = "pjson_module_" + module.ModuleID, rel = "file" },
                };
                i++;
                result.Add(node);
            }

            return result.ToArray<JsTreeModel>();
        }

        public JsonResult GetTreeModule(string result, int pageId)
        {
            List<PageItem> pages = new PagesDB().GetPagesFlat(this.PortalSettings.PortalID);
            var page = pages.First(p => p.ID == pageId);

            JsTreeModel[] child = getChildrenTree(page);
            List<JsTreeModel> child2 = new List<JsTreeModel>();
            var panelist = result.Split('+').ToList();
            var panetopage = ModelServices.GetPageModules(pageId);
            var lowerpane = panelist.ConvertAll(d => d.ToLower());
            var i = 0;
            foreach (var pane in panelist)
            {
                JsTreeModel[] childm = getModuleToPane(pane, pageId);
                JsTreeModel nodem = new JsTreeModel
                    {
                        data = pane,
                        attr = new JsTreeAttribute {id = "pjson_pane_" + i, rel= "folder"},
                        children = childm
                    };
                child2.Add(nodem);
                i++;
            }
            // add other pane.
            foreach (var pane in panetopage)
            {
                if (!lowerpane.Contains(pane.Key))
                {
                    JsTreeModel[] childm = getModuleToPane(pane.Key, pageId);
                    JsTreeModel nodem = new JsTreeModel
                    {
                        data = pane.Key + "  [Not present in current layout]",
                        attr = new JsTreeAttribute { id = "pjson_pane_" + i, rel = "folder2" },
                        children = childm
                    };
                    child2.Add(nodem);
                    i++;
                }
                    panelist.Add(pane.Key);
            }
            var childlist = child.ToList();
            foreach (var childmod in child2)
            {
                childlist.Add(childmod);
            }
            child = childlist.ToArray<JsTreeModel>();
            return Json(child);
        }

        public JsonResult AddModule(string id)
        {
            var pageId = Convert.ToInt32(id.Replace("pjson_", ""));

            string URL = ConvertRelativeUrlToAbsoluteUrl(HttpUrlBuilder.BuildUrl("~/?pageid=" + pageId + "&panelist=y"));
            System.Net.WebRequest webRequest = System.Net.WebRequest.Create(URL);
            webRequest.Method = "GET";
            StreamReader sr = new StreamReader(webRequest.GetResponse().GetResponseStream());
            string result = sr.ReadToEnd();
            
            return GetTreeModule(result, pageId);
        }

        public string ConvertRelativeUrlToAbsoluteUrl(string relativeUrl)
        {
            if (Request.IsSecureConnection)
                return string.Format("https://{0}{1}", Request.Url.Host, relativeUrl);
            else
                return string.Format("http://{0}{1}", Request.Url.Host, relativeUrl);

        }

        public string GetUrlToEdit(string pageId, string moduleId)
        {
            var url = HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Admin/ModuleSettings.aspx", Int32.Parse(pageId));
            url += "&mID=" + moduleId;
            return url;
        }

    }
}
