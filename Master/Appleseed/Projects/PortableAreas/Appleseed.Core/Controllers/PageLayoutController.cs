using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Appleseed.Framework;
using Appleseed.Framework.Security;
using Appleseed.Framework.Site.Data;
using System.Text;
using System.Collections;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Configuration.Items;

namespace Appleseed.Core.Controllers
{

    public class PageLayoutController : Controller
    {
        //
        // GET: /Module/

        public JsonResult AddModule(string title, string moduleType, string paneLocation, string viewPermission, string pageId, string ModuleId)
        {

            // All new modules go to the end of the content pane
            var m = new ModuleItem();
            m.Title = title;
            m.ModuleDefID = Int32.Parse(moduleType);
            m.Order = 999;

            // save to database
            var mod = new ModulesDB();
            var modId = Int32.Parse(ModuleId);

            m.ID = mod.AddModule(
                Int32.Parse(pageId),
                m.Order,
                paneLocation,
                m.Title,
                m.ModuleDefID,
                0,
                PortalSecurity.GetEditPermissions(modId),
                viewPermission,
                PortalSecurity.GetAddPermissions(modId),
                PortalSecurity.GetDeletePermissions(modId),
                PortalSecurity.GetPropertiesPermissions(modId),
                PortalSecurity.GetMoveModulePermissions(modId),
                PortalSecurity.GetDeleteModulePermissions(modId),
                false,
                PortalSecurity.GetPublishPermissions(modId),
                false,
                false,
                false);

            // End Change Geert.Audenaert@Syntegra.Com

            //// reload the portalSettings from the database

            //this.Context.Items["PortalSettings"] = new PortalSettings(this.PageID, this.PortalSettings.PortalAlias);
            //this.PortalSettings = (PortalSettings)this.Context.Items["PortalSettings"];

            // reorder the modules in the content pane
            //var modules = GetModules("ContentPane", Int32.Parse(pageId), Int32.Parse(portalId));
            //this.OrderModules(modules);

            //// resave the order
            //foreach (ModuleItem item in modules) {
            //    mod.UpdateModuleOrder(item.ID, item.Order, "ContentPane");
            //}

            //// Redirect to the same page to pick up changes
            ////this.Response.Redirect(this.AppendModuleID(this.Request.RawUrl, m.ID));
            var list = GetModules(paneLocation, Int32.Parse(pageId));

            StringBuilder ls = new StringBuilder();
            foreach (ModuleItem md in list)
            {
                ls.AppendFormat("<option value=\"{0}\">{1}</option>", md.ID, md.Title);
            }

            return Json(new { value = ls.ToString() });
        }

        private ArrayList GetModules(string pane, int pageId)
        {
            var paneModules = new ArrayList();
            var modules = new Appleseed.Framework.Site.Data.ModulesDB();

            foreach (ModuleSettings _module in modules.getModulesSettingsInPage(pageId, pane))
            {
                if (_module.PaneName.ToLower() == pane.ToLower() &&
                    pageId == _module.PageID)
                {
                    var m = new ModuleItem();
                    m.Title = _module.ModuleTitle;
                    m.ID = _module.ModuleID;
                    m.ModuleDefID = _module.ModuleDefID;
                    m.Order = _module.ModuleOrder;
                    paneModules.Add(m);
                }
            }

            InsertionSort(paneModules, ModuleItem.Compare);


            return paneModules;
        }

        private void InsertionSort(ArrayList list, Comparison<ModuleItem> comparison)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (comparison == null)
                throw new ArgumentNullException("comparison");

            int count = list.Count;
            for (int j = 1; j < count; j++)
            {
                ModuleItem key = (ModuleItem)list[j];

                int i = j - 1;
                for (; i >= 0 && comparison((ModuleItem)list[i], key) > 0; i--)
                {
                    list[i + 1] = list[i];
                }
                list[i + 1] = key;
            }
        }



        private ArrayList OrderModules(ArrayList list)
        {
            var i = 1;

            // sort the arraylist
            InsertionSort(list, ModuleItem.Compare);

            // renumber the order
            foreach (ModuleItem m in list)
            {
                // number the items 1, 3, 5, etc. to provide an empty order
                // number when moving items up and down in the list.
                m.Order = i;
                i += 2;
            }

            return list;
        }


        public JsonResult UpDown_Click(string cmd, string pane, string pageId, string selectedIndex, string length)
        {
            var modules = this.GetModules(pane, Int32.Parse(pageId));
            int delta;
            var selection = -1;
            var index = Int32.Parse(selectedIndex);

            // Determine the delta to apply in the order number for the module
            // within the list.  +3 moves down one item; -3 moves up one item
            if (cmd == "down")
            {
                delta = 3;
                if (index < Int32.Parse(length) - 1)
                {
                    selection = index + 1;
                }
            }
            else
            {
                delta = -3;
                if (index > 0)
                {
                    selection = index - 1;
                }
            }

            ModuleItem m;
            m = (ModuleItem)modules[index];

            if (PortalSecurity.IsInRoles(PortalSecurity.GetMoveModulePermissions(m.ID)))
            {
                m.Order += delta;

                // reorder the modules in the content pane
                var list = this.OrderModules(modules);

                // resave the order
                var admin = new ModulesDB();
                foreach (ModuleItem item in modules)
                {
                    admin.UpdateModuleOrder(item.ID, item.Order, pane);
                }

                StringBuilder ls = new StringBuilder();
                foreach (ModuleItem md in list)
                {
                    ls.AppendFormat("<option value=\"{0}\">{1}</option>", md.ID, md.Title);
                }
                return Json(new { value = ls.ToString() });

            }
            else
            {
                return Json(new { value = "error" });
            }

        }

        public JsonResult RightLeft_Click(string sourcePane, string targetPane, string pageId, string selectedIndex)
        {

            // get source arraylist
            var sourceList = this.GetModules(sourcePane, Int32.Parse(pageId));

            var index = Int32.Parse(selectedIndex);
            // get a reference to the module to move
            // and assign a high order number to send it to the end of the target list
            var m = (ModuleItem)sourceList[index];

            if (PortalSecurity.IsInRoles(PortalSecurity.GetMoveModulePermissions(m.ID)))
            {
                // add it to the database
                var admin = new ModulesDB();
                admin.UpdateModuleOrder(m.ID, 99, targetPane);

                // delete it from the source list
                sourceList.RemoveAt(index);

                // reorder the modules in the source pane
                sourceList = this.GetModules(sourcePane, Int32.Parse(pageId));
                var list = this.OrderModules(sourceList);

                // resave the order
                foreach (ModuleItem item in sourceList)
                {
                    admin.UpdateModuleOrder(item.ID, item.Order, sourcePane);
                }

                // reorder the modules in the target pane
                var targetList = this.GetModules(targetPane, Int32.Parse(pageId));
                var list2 = this.OrderModules(targetList);

                // resave the order
                foreach (ModuleItem item in targetList)
                {
                    admin.UpdateModuleOrder(item.ID, item.Order, targetPane);
                }

                StringBuilder ls = new StringBuilder();
                foreach (ModuleItem md in list)
                {
                    ls.AppendFormat("<option value=\"{0}\">{1}</option>", md.ID, md.Title);
                }

                StringBuilder sb = new StringBuilder();
                foreach (ModuleItem md in list2)
                {
                    sb.AppendFormat("<option value=\"{0}\">{1}</option>", md.ID, md.Title);
                }

                return Json(new { error = false, source = ls.ToString(), target = sb.ToString() });

            }
            else
            {
                return Json(new { error = true });
            }

        }

        public JsonResult DeleteBtn_Click(string pane, string pageId, string selectedIndex)
        {
            var modules = this.GetModules(pane, Int32.Parse(pageId));
            var index = Int32.Parse(selectedIndex);

            var m = (ModuleItem)modules[index];
            if (m.ID > -1)
            {
                // jviladiu@portalServices.net (20/08/2004) Add role control for delete module
                if (PortalSecurity.IsInRoles(PortalSecurity.GetDeleteModulePermissions(m.ID)))
                {
                    // must delete from database too
                    var moddb = new ModulesDB();

                    // TODO add userEmail and useRecycler
                    moddb.DeleteModule(m.ID);

                    // reorder the modules in the pane
                    modules = this.GetModules(pane, Int32.Parse(pageId));
                    var list = this.OrderModules(modules);

                    // resave the order
                    foreach (ModuleItem item in modules)
                    {
                        moddb.UpdateModuleOrder(item.ID, item.Order, pane);
                    }

                    StringBuilder ls = new StringBuilder();
                    foreach (ModuleItem md in list)
                    {
                        ls.AppendFormat("<option value=\"{0}\">{1}</option>", md.ID, md.Title);
                    }
                    return Json(new { error = false, value = ls.ToString() });

                }
                else
                {
                    return Json(new { error = true });
                }
            }
            return Json(new { error = true });


        }

        public JsonResult EditBtn_Click(string pane, string modid, string pageId, string modal)
        {
            var mid = Int32.Parse(modid);

            // Add role control to edit module settings by Mario Endara <mario@softworks.com.uy> (2004/11/09)
            if (PortalSecurity.IsInRoles(PortalSecurity.GetPropertiesPermissions(mid)))
            {
                var url = HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Admin/ModuleSettings.aspx", Int32.Parse(pageId), mid);
                // Redirect to module settings page
                if (Boolean.Parse(modal))
                {
                    url += "&ModalChangeMaster=true&camefromEditPage=true";
                }
                return Json(new { error = false, value = url });
            }
            else
            {
                return Json(new { error = true });
            }

        }

        public JsonResult LoadModule(string pane, string pageId)
        {
            try
            {
                var modules = this.GetModules(pane, Int32.Parse(pageId));
                StringBuilder sb = new StringBuilder();
                foreach (ModuleItem md in modules)
                {
                    sb.AppendFormat("<option value=\"{0}\">{1}</option>", md.ID, md.Title);
                }
                return Json(new { error = false, value = sb.ToString() });
            }
            catch 
            {
                return Json(new { error = true });
            }

        }

        # region jstreeFunctionality
        /// <summary>
        /// Get Root node
        /// </summary>
        /// <returns>Root node</returns>
        public JsonResult GetRootNodes()
        {
            JsTreeItem rootNode = new JsTreeItem();
            rootNode.text = "Page";
            rootNode.id = "0";
            return Json(rootNode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Sub nodes 
        /// </summary>
        /// <param name="pane">pane id</param>
        /// <param name="pageId">page id</param>
        /// <returns>Current page modules</returns>
        public JsonResult GetSubNodes(string pane, string pageId)
        {
            if (pane == "0")
            {
                List<JsTreeItem> rootnodelist = new List<JsTreeItem>();

                JsTreeItem rootNode = new JsTreeItem();

                rootNode.text = "Top Pane";
                rootNode.id = "TopPane";
                rootnodelist.Add(rootNode);

                rootNode = new JsTreeItem();
                rootNode.text = "Left Pane";
                rootNode.id = "LeftPane";
                rootnodelist.Add(rootNode);

                rootNode = new JsTreeItem();
                rootNode.text = "Center Pane";
                rootNode.id = "ContentPane";
                rootnodelist.Add(rootNode);

                rootNode = new JsTreeItem();
                rootNode.text = "Right Pane";
                rootNode.id = "RightPane";
                rootnodelist.Add(rootNode);

                rootNode = new JsTreeItem();
                rootNode.text = "Bottom Pane";
                rootNode.id = "BottomPane";
                rootnodelist.Add(rootNode);

                return Json(rootnodelist, JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    var modules = this.GetModules(pane, Int32.Parse(pageId));
                    List<JsTreeItem> listNode = new List<JsTreeItem>();
                    foreach (ModuleItem md in modules)
                    {
                        JsTreeItem node = new JsTreeItem();
                        node.id = md.ID.ToString();
                        node.text = md.Title;
                        listNode.Add(node);
                    }
                    return Json(listNode, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(new { error = true });
                }
            }
        }


        public JsonResult DeleteModuleFromTree(string pane, string pageId, string moduleid)
        {
            List<JsTreeItem> nodelist = new List<JsTreeItem>();

            int mID = Convert.ToInt32(moduleid);
            if (mID > -1)
            {
                if (PortalSecurity.IsInRoles(PortalSecurity.GetDeleteModulePermissions(mID)))
                {
                    var moddb = new ModulesDB();

                    // Delete module
                    moddb.DeleteModule(mID);

                    // reorder the modules in the pane
                  var  modules = this.GetModules(pane, Int32.Parse(pageId));
                    var list = this.OrderModules(modules);

                    foreach (ModuleItem item in modules)
                    {
                        moddb.UpdateModuleOrder(item.ID, item.Order, pane);
                    }

                    foreach (ModuleItem md in list)
                    {
                        JsTreeItem rootNode = new JsTreeItem();
                        rootNode.id = md.ID.ToString();
                        rootNode.text = md.Title;
                        nodelist.Add(rootNode);
                    }
                    return Json(nodelist, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = true });
                }
            }
            return Json(new { error = true });
        }

        /// <summary>
        /// Drag and drop functionality 
        /// </summary>
        /// <param name="sourcePane">Source Pane</param>
        /// <param name="targetPane">Target Pane</param>
        /// <param name="pageId">Page id</param>
        /// <param name="moduleid">Module id</param>
        /// <returns>return </returns>
        public JsonResult MoveModule(string sourcePane, string targetPane, string pageId, string moduleid)
        {
            
            //If targetPane is "0" then it will return So module will not lost
            if (string.Compare(targetPane, "0") == 0)
                return Json(new { error = true });

            // get source arraylist
            var sourceList = this.GetModules(sourcePane, Int32.Parse(pageId));

            var mID = Convert.ToInt32( moduleid); //(ModuleItem)sourceList[index];

            if (PortalSecurity.IsInRoles(PortalSecurity.GetMoveModulePermissions(mID)))
            {
                // add it to the database
                var admin = new ModulesDB();
                admin.UpdateModuleOrder(mID, 99, targetPane);

                // reorder the modules in the source pane
                sourceList = this.GetModules(sourcePane, Int32.Parse(pageId));
                var list = this.OrderModules(sourceList);

                // resave the order
                foreach (ModuleItem item in sourceList)
                {
                    admin.UpdateModuleOrder(item.ID, item.Order, sourcePane);
                }

                // reorder the modules in the target pane
                var targetList = this.GetModules(targetPane, Int32.Parse(pageId));
                var list2 = this.OrderModules(targetList);

                // resave the order
                foreach (ModuleItem item in targetList)
                {
                    admin.UpdateModuleOrder(item.ID, item.Order, targetPane);
                }

                return Json(new { error = false });
            }
            else
            {
                return Json(new { error = true });
            }
        }

        # endregion
    }
}

