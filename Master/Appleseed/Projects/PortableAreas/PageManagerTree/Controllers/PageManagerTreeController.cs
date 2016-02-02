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
    [History("Ashish.patel@haptix.biz", "2014/12/24", "Added clearing cache and clearing url elements on rename")]
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
            bool isCurrentUserAdmin = UserProfile.isCurrentUserAdmin;
            Permissions perm = new Permissions
            {
                HasPageCreatePermission = UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_CREATION) || isCurrentUserAdmin,
                HasPageDeletePermission = UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_DELETION) || isCurrentUserAdmin,
                HasPageList = UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_LIST) || isCurrentUserAdmin,
                HasPageUpdatePermission = UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_EDITING) || isCurrentUserAdmin
            };

            return View(perm);
        }
        public JsonResult GetRootPage()
        {
            JsTreeModel rootNode = new JsTreeModel();
            // rootNode.attr = new JsTreeAttribute();
            rootNode.text = "Root";
            rootNode.id = "0";// new DirectoryInfo(Request.MapPath("/")).Name;
            return Json(rootNode, JsonRequestBehavior.AllowGet);
        }
        public JsTreeModel[] getChildrenTree(PageItem page)
        {
            List<PageStripDetails> childPages = new PagesDB().GetPagesinPage(this.PortalSettings.PortalID, page.ID);
            int count = 0;
            List<JsTreeModel> lstTree = new List<JsTreeModel>();
            foreach (PageStripDetails childPage in childPages)
            {
                PageItem aux = new PageItem();
                aux.ID = childPage.PageID;
                aux.Name = childPage.PageName;
                //JsTreeModel[] childs = getChildrenTree(aux);
                JsTreeModel node = new JsTreeModel
                {
                    data = aux.Name,
                    attr = new JsTreeAttribute { id = "pjson_" + aux.ID.ToString() },
                    //children = childs,
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
                    JsTreeModel node = new JsTreeModel
                    {
                        text = page.Name,
                        id = page.ID.ToString(),
                        data = page.Name,
                        attr = new JsTreeAttribute { id = "pjson_" + page.ID.ToString() },
                        //children = child,
                    };
                    List<PageStripDetails> childPages = new PagesDB().GetPagesinPage(this.PortalSettings.PortalID, page.ID);
                    if (childPages.Count > 0)
                    {
                        //node.children
                    }
                    lstTree.Add(node);
                }
            }
            int root = 0;
            JsTreeModel rootNode = new JsTreeModel
            {
                text = "Root",
                id = root.ToString(),
                data = "Root",
                attr = new JsTreeAttribute { id = "pjson_" + root.ToString(), rel = "root" },
                // children = lstTree.ToArray<JsTreeModel>()
            };
            return Json(rootNode);
        }
        /// <summary>
        /// Gets sub pages of given page
        /// </summary>
        /// <param name="pageid">page id</param>
        /// <returns> return json</returns>
        public JsonResult GetSubPages(string pageid, bool isPane, string parentid)
        {
            List<JsTreeModel> pages = new List<JsTreeModel>();
            if (!isPane)
            {
                List<PageStripDetails> childPages = new PagesDB().GetPagesinPage(this.PortalSettings.PortalID, Convert.ToInt32(pageid));
                int i = 0;
                pages = (from page in childPages
                         orderby page.PageOrder
                         select new JsTreeModel()
                         {
                             id = page.PageID.ToString(),
                             text = page.PageName,
                             position = (i++).ToString(),//page.PageOrder.ToString(),
                             lastpos = (childPages.Count - 1).ToString(),
                             nodeType = "page",
                             type = "page"
                             //icon = "http://jstree.com/tree.png"
                         }).ToList();
                //pages.Where(pg => pg.lastpos == i.ToString());
            }
            // Get panes and modules
            //if (isPane)
            //{
            //    GetPanes(parentid, pages, isPane, pageid);
            //}
            //else
            //{
            //    if (pageid != "0")
            //        GetPanes(pageid, pages, isPane, string.Empty);
            //}
            return Json(pages, JsonRequestBehavior.AllowGet);
        }
        private void GetPanes(string pageid, List<JsTreeModel> pages, bool loadModules, string panename)
        {
            string URL = ConvertRelativeUrlToAbsoluteUrl(HttpUrlBuilder.BuildUrl("~/?pageid=" + Convert.ToInt32(pageid) + "&panelist=y"));
            System.Net.WebRequest webRequest = System.Net.WebRequest.Create(URL);
            webRequest.Method = "GET";
            StreamReader sr = new StreamReader(webRequest.GetResponse().GetResponseStream());
            string result = sr.ReadToEnd();
            List<JsTreeModel> child2 = new List<JsTreeModel>();
            var panelist = result.Split('+').ToList();
            //var panetopage = ModelServices.GetPageModules(Convert.ToInt32(pageid));
            var lowerpane = panelist.ConvertAll(d => d.ToLower());
            var i = 0;
            if (!loadModules)
            {
                foreach (var pane in panelist)
                {
                    //JsTreeModel[] childm = getModuleToPane(pane, Convert.ToInt32(pageid));
                    JsTreeModel nodem = new JsTreeModel
                    {
                        text = pane,
                        id = pane,
                        isPane = true,
                        nodeType = "pane",
                        // icon = @"/aspnet_client/jQuery/jsTree/icon.png", //+ "http://jstree.com/tree.png",
                        type = "pane",
                        //data = pane,
                        attr = new JsTreeAttribute { id = "pjson_pane_" + i, rel = "folder" },
                        //children2 = childm
                    };
                    child2.Add(nodem);
                    i++;
                }
                pages.AddRange(child2);
            }
            else
            {
                // add other pane.
                JsTreeModel[] childm = getModuleToPane(panename, Convert.ToInt32(pageid));
                pages.AddRange(childm);
                //JsTreeModel nodem = new JsTreeModel
                //{
                //    text = pane.Key,
                //    id = pane.Key + i.ToString(),
                //    isPane = true,
                //    //data = pane.Key + "  [Not present in current layout]",
                //    attr = new JsTreeAttribute { id = "pjson_pane_" + i, rel = "folder2" },
                //    //children2 = childm
                //};
                //child2.Add(nodem);
                //i++;
            }
        }
        [HttpPost]
        public JsonResult remove(int id)
        {
            try
            {
                if ((UserProfile.isCurrentUserAdmin) || UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_DELETION))
                {
                    var tabs = new PagesDB();
                    tabs.DeletePage(id);
                    return Json(new { error = false });
                }
                else
                {
                    string errorMessage = General.GetString("ACCESS_DENIED", "You don't have permissin to delete page", this);
                    return Json(new { error = true, errorMess = errorMessage });
                }
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
                if (PortalSecurity.IsInRoles(PortalSecurity.GetDeleteModulePermissions(id)) && ((UserProfile.isCurrentUserAdmin) || UserProfile.CurrentUser.HasPermission(AccessPermissions.MODULE_DELETION)))
                {
                    // must delete from database too
                    var moddb = new ModulesDB();
                    moddb.DeleteModule(id);
                    return Json(new { error = false });
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
                if ((UserProfile.isCurrentUserAdmin) || UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_CREATION))
                {
                    var generalModuleDef = Guid.Parse("F9F9C3A4-6E16-43B4-B540-984DDB5F1CD2");
                    object[] queryargs = { generalModuleDef, PortalSettings.PortalID };
                    int moduleDefinition;
                    try
                    {
                        moduleDefinition =
                            new rb_ModuleDefinitions().All(where: "GeneralModDefID = @0 and PortalID = @1", args: queryargs).Single().ModuleDefID;
                    }
                    catch 
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
                    return Json(new { pageId = t.ID });
                }
                else
                {
                    string errorMessage = General.GetString("ACCESS_DENIED", "You don't have permissin to clone this page", this);
                    return Json(new { error = true, errorMess = errorMessage });
                }
            }
            catch (Exception e)
            {
                ErrorHandler.Publish(LogLevel.Error, e);
                Response.StatusCode = 500;
                return Json("");
            }
        }

        public JsonResult create(string pageid, string pagename)
        {
            try
            {
                if ((UserProfile.isCurrentUserAdmin) || UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_CREATION))
                {
                    pageid = pageid.Trim('"');
                    PagesDB db = new PagesDB();
                    this.PortalPages = db.GetPagesFlat(this.PortalSettings.PortalID);
                    var t = new PageItem
                    {
                        Name = General.GetString("TAB_NAME", "New Page Name"),
                        ID = -1,
                        Order = 990000
                    };
                    if (!string.IsNullOrEmpty(pagename))
                    {
                        t.Name = pagename;
                    }
                    this.PortalPages.Add(t);
                    var tabs = new PagesDB();
                    t.ID = tabs.AddPage(this.PortalSettings.PortalID, t.Name, t.Order);
                    db.UpdatePageParent(t.ID, Convert.ToInt32(pageid), this.PortalSettings.PortalID);
                    this.OrderPages();
                    return Json("");
                }
                else
                {
                    string errorMessage = General.GetString("ACCESS_DENIED", "You don't have permissin to add new page", this);
                    return Json(new { error = true, errorMess = errorMessage });
                }
            }
            catch (Exception)
            {
                string errorMessage = General.GetString("ADD_NEW_PAGE_FAILED", "Failed to add new Page", this);
                return Json(new { error = true, errorMess = errorMessage });
            }
        }

        public JsonResult edit(int id)
        {
            if ((UserProfile.isCurrentUserAdmin) || UserProfile.CurrentUser.HasPermission(AccessPermissions.MODULE_EDITING))
            {
                ModulesDB modules = new ModulesDB();
                Guid TabGuid = new Guid("{1C575D94-70FC-4A83-80C3-2087F726CBB3}");
                int TabModuleID = 0;
                foreach (ModuleItem m in modules.FindModuleItemsByGuid(PortalSettings.PortalID, TabGuid))
                {
                    bool HasEditPermissionsOnTabs = PortalSecurity.HasEditPermissions(m.ID);
                    if (HasEditPermissionsOnTabs)
                    {
                        TabModuleID = m.ID;
                        break;
                    }
                }
                string dir = HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Pages/PageLayout.aspx?PageID=" + id.ToString() +
                        "&mID=" + TabModuleID + "&Alias=" + this.PortalSettings.PortalAlias + "&returntabid=" +
                        this.PortalSettings.ActiveModule);
                return Json(new { url = dir });
            }
            else
            {
                string errorMessage = General.GetString("ACCESS_DENIED", "You don't have permissin to edit this module", this);
                return Json(new { error = true, errorMess = errorMessage });
            }
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

        public JsonResult moveNode(int pageID, int newParent, int idOldNode, int selectedposition)
        {
            //Cache clearing
            Appleseed.Framework.Web.SqlUrlBuilderProvider.ClearCachePageUrl(pageID);
            Appleseed.Framework.Web.UrlBuilderHelper.ClearUrlElements(pageID);
            CurrentCache.RemoveAll("_PageNavigationSettings_");
            PortalSettings.RemovePortalSettingsCache(pageID, PortalSettings.PortalAlias);
            Appleseed.Framework.Providers.AppleseedSiteMapProvider.AppleseedSiteMapProvider.ClearAllAppleseedSiteMapCaches();
            PortalSettings.UpdatePortalSettingParentPageCache(newParent, pageID);

            if (UserProfile.isCurrentUserAdmin || UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_EDITING))
            {
                PagesDB db = new PagesDB();
                this.PortalPages = db.GetPagesFlat(this.PortalSettings.PortalID);
                db.UpdatePageParent(Convert.ToInt32(pageID), Convert.ToInt32(newParent), this.PortalSettings.PortalID);
                int order;
                if (Convert.ToInt32(idOldNode) == -1)
                {
                    order = 9999;
                }
                else
                {
                    List<PageStripDetails> childPages = new PagesDB().GetPagesinPage(this.PortalSettings.PortalID, newParent).Where(pg => pg.PageID != pageID).ToList();
                    if (childPages.Count == 0)
                    {
                        order = 0;
                    }
                    else
                    {
                        if (selectedposition < childPages.Count)
                            order = childPages[selectedposition].PageOrder - 1;
                        else
                            order = childPages[childPages.Count - 1].PageOrder + 1;
                    }
                }
                db.UpdatePageOrder(Convert.ToInt32(pageID), order);
                this.OrderPages();
                return Json("");
            }
            else
            {
                this.OrderPages();
                string errorMessage = General.GetString("ACCESS_DENIED", "You don't have permissin to move page", this);
                return Json(new { error = true, errorMess = errorMessage });
            }
        }
        public JsonResult MoveModuleNode(int pageId, int moduleId, string paneName)
        {
            if (UserProfile.isCurrentUserAdmin || UserProfile.CurrentUser.HasPermission(AccessPermissions.MODULE_EDITING))
            {
                rb_Modules db = new rb_Modules();
                var module = db.Single(moduleId);
                module.TabID = pageId;
                module.PaneName = paneName.TrimEnd();
                db.Update(module, moduleId);
                return Json("");
            }
            else
            {
                string errorMessage = General.GetString("ACCESS_DENIED", "You don't have permissin to move module", this);
                return Json(new { error = true, errorMess = errorMessage });
            }
        }
        /// Ashish.patel@haptix.biz - 2014/12/24 - Added clearing cache and clearing url elements
        public JsonResult Rename(int id, string name)
        {
            try
            {
                if ((UserProfile.isCurrentUserAdmin) || UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_EDITING))
                {
                    var db = new rb_Pages();
                    var page = db.Single(id);
                    page.PageName = name;
                    db.Update(page, page.PageID);
                    // Added for clearing page url and elements from cache
                    Appleseed.Framework.Web.SqlUrlBuilderProvider.ClearCachePageUrl(id);
                    Appleseed.Framework.Web.UrlBuilderHelper.ClearUrlElements(id);
                    //string sql = @"UPDATE rb_Pages SET [] = @0 WHERE PageID = @1";
                    //object[] queryargs = { name, id };
                    //var t = new .Query(sql, queryargs);
                }
                else
                {
                    string errorMessage = General.GetString("ACCESS_DENIED", "You don't have permissin to rename page", this);
                    return Json(new { error = true, errorMess = errorMessage });
                }
            }
            catch (Exception)
            {
                return Json(new { error = true });
            }
            return Json(new { error = false });
        }
        public JsonResult RenameModule(int id, string name)
        {
            try
            {
                if ((UserProfile.isCurrentUserAdmin) || UserProfile.CurrentUser.HasPermission(AccessPermissions.MODULE_EDITING))
                {
                    var db = new rb_Modules();
                    var module = db.Single(id);
                    module.ModuleTitle = name;
                    db.Update(module, module.ModuleID);
                }
                else
                {
                    string errorMessage = General.GetString("ACCESS_DENIED", "You don't have permissin to rename module", this);
                    return Json(new { error = true, errorMess = errorMessage });
                }
            }
            catch (Exception)
            {
                return Json(new { error = true });
            }
            return Json(new { error = false });
        }
        public JsonResult ViewPage(string page_Id)
        {
            int pageId = Convert.ToInt32(page_Id.Trim('"'));
            var url = HttpUrlBuilder.BuildUrl(pageId);
            return Json(url);
        }
        public JsonResult CutCopyPage(int pageId, string name, int parentId, bool isCopy)
        {
            if ((UserProfile.isCurrentUserAdmin) || UserProfile.CurrentUser.HasPermission(AccessPermissions.PAGE_CREATION))
            {
                PagesDB db = new PagesDB();
                if (isCopy)
                {
                    IPortalTemplateServices services = PortalTemplateFactory.GetPortalTemplateServices(new PortalTemplateRepository());
                    int newpageid = services.CopyPage(pageId, name + "- Copy");
                    db.UpdatePageParent(newpageid, parentId, this.PortalSettings.PortalID);
                    return Json(new { pageId = newpageid });
                }
                else
                {
                    db.UpdatePageParent(pageId, parentId, this.PortalSettings.PortalID);
                    return Json(new { pageId = pageId });
                }
            }
            else
            {
                string errorMessage = General.GetString("ACCESS_DENIED", "You don't have permissin to cut page", this);
                if (isCopy)
                {
                    errorMessage = General.GetString("ACCESS_DENIED", "You don't have permissin to copy page", this);
                }
                return Json(new { error = true, errorMess = errorMessage });
            }
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
                    text = module.ModuleTitle,
                    id = module.ModuleID.ToString(),
                    data = module.ModuleTitle,
                    nodeType = "module",
                    type = "module"
                    // attr = new JsTreeAttribute { id = "pjson_module_" + module.ModuleID, rel = "file" },
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
                    text = pane,
                    id = i.ToString(),
                    data = pane,
                    attr = new JsTreeAttribute { id = "pjson_pane_" + i, rel = "folder" },
                    //children2 = childm
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
                        text = pane.Key,
                        id = i.ToString(),
                        data = pane.Key + "  [Not present in current layout]",
                        attr = new JsTreeAttribute { id = "pjson_pane_" + i, rel = "folder2" },
                        //children2 = childm
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