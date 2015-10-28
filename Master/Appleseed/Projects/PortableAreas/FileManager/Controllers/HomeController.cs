using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Appleseed.Framework.Security;
using FileManager.Models;
using Appleseed.Framework.Configuration.Items;
namespace FileManager.Controllers
{
    public class HomeController : BaseController
    {
        JsTreeItem jstree = new JsTreeItem();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Module()
        {
            SetModuleId();
            if (PortalSecurity.HasViewPermissions(ModuleId))
            {
                var model = new FileManagerModel
                                {
                                    PortalName = PortalSettings.PortalFullPath,
                                    ModuleId = ModuleId,
                                    ViewPermission = PortalSecurity.HasViewPermissions(ModuleId),
                                    EditPermission = PortalSecurity.HasEditPermissions(ModuleId)
                                };
                return View(model);
            }
            PortalSecurity.AccessDenied();
            return new EmptyResult();
        }

        /// <summary>
        /// A method to populate a TreeView with directories, subdirectories, etc
        /// </summary>
        /// <param name="dir">The path of the directory</param>
        /// <param name="node">The "master" node, to populate</param>
        public void PopulateTree(string dir, FilesTree node, bool roottree)
        {
            var directory = new DirectoryInfo(Request.MapPath(dir));

            if (node.children == null && directory.GetDirectories().Length > 0)
            {
                node.children = new List<FilesTree>();
            }
            // get the information of the directory

            // loop through each subdirectory

            // Get list of folders to be render in left side folder tree
            var allowfolderlist = System.Configuration.ConfigurationManager.AppSettings["FileManager.AllowFolders.Tree"].ToString();
            var allowfolders = allowfolderlist.Split('|');

            foreach (var t in from d in directory.GetDirectories() let dirName = string.Format("{0}/{1}", dir, d.Name) select new FilesTree { attr = new FilesTreeAttribute { id = dirName }, data = d.Name, state = "closed" } into t where node.children != null select t)
            {
                if (roottree)
                {
                    foreach (var folder in allowfolders)
                    {
                        if (t.data == folder)
                        {
                            node.children.Add(t); // add the node to the "master" node                    
                        }
                    }


                }
                else
                {
                    node.children.Add(t); // add the node to the "master" node                    
                }

            }

            // lastly, loop through each file in the directory, and add these as nodes
            //foreach (var f in directory.GetFiles()) {
            //    // create a new node
            //    var t = new FilesTree {attr = new FilesTreeAttribute {id = f.FullName}, data = f.Name};
            //    // add it to the "master"
            //    node.children.Add(t);
            //}
        }


        [HttpPost]
        [FileManagerViewFilter]
        public JsonResult GetTreeData(string mID)
        {
            SetModuleId(Convert.ToInt32(mID));
            //var rootPath = PortalSettings.PortalFullPath;
            var rootPath = "/";
            var rootNode = new FilesTree { attr = new FilesTreeAttribute { id = rootPath }, data = rootPath };
            rootNode.attr.id = rootPath;
            PopulateTree(rootPath, rootNode, true);
            return Json(rootNode, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [FileManagerViewFilter]
        public JsonResult GetChildreenTree(string dir, int mID)
        {
            SetModuleId(mID);
            var rootNode = new FilesTree { attr = new FilesTreeAttribute { id = dir }, data = dir.Substring(1) };
            var rootPath = dir;
            rootNode.attr.id = rootPath;
            PopulateTree(rootPath, rootNode, false);

            return Json(rootNode.children);
        }

        [HttpPost]
        [FileManagerEditFilter]
        public ActionResult MoveData(string path, string destination, int mID, bool isCopy, string folder)
        {
            try
            {
                SetModuleId(mID);
                path = Request.MapPath(path);
                destination = Request.MapPath(destination);
                // get the file attributes for file or directory
                var attPath = System.IO.File.GetAttributes(path);

                var attDestination = System.IO.File.GetAttributes(destination);

                var fi = new FileInfo(path);

                //detect whether its a directory or file
                if ((attPath & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    if ((attDestination & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        if (!isCopy)
                        {
                            MoveDirectory(path, destination);
                        }
                        else
                        {
                            var fullNewName = string.Format(@"{0}\{1}", destination, folder);
                            CopyDirectories(path, fullNewName);
                        }

                    }
                }
                else
                {
                    System.IO.File.Move(path, destination + "\\" + fi.Name);
                }
                return null;
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
                return null;
            }
        }

        [HttpPost]
        public ActionResult CreateFolder(string path, string newname)
        {
            CreateFolderInPath(path, newname);
            return null;
        }

        private static void CreateFolderInPath(string path, string newname)
        {
            Directory.CreateDirectory(path + "\\" + newname);
        }

        [FileManagerEditFilter]
        public JsonResult RenameFolderTree(string path, string newName, int mID)
        {
            try
            {
                SetModuleId(mID);
                var index = path.LastIndexOf('/');

                var directory = path.Substring(0, index + 1);
                newName = directory + newName;
                Directory.Move(Request.MapPath(path), Request.MapPath(newName));

                return Json("ok");
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
                return Json("");
            }
        }

        private static void CopyDirectories(string sourceDirName, string destDirName)
        {
            var dir = new DirectoryInfo(sourceDirName);
            var dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            var files = dir.GetFiles();
            foreach (var file in files)
            {
                var temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            foreach (var subdir in dirs)
            {
                var temppath = Path.Combine(destDirName, subdir.Name);
                CopyDirectories(subdir.FullName, temppath);

            }
        }

        public void MoveDirectory(string source, string target)
        {
            var stack = new Stack<Folders>();
            stack.Push(new Folders(source, target));

            while (stack.Count > 0)
            {
                var folders = stack.Pop();
                //Directory.CreateDirectory(folders.Target);

                // Create Directory
                var sourceFolderName =
                    folders.Source.Substring(folders.Source.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                CreateFolderInPath(folders.Target, sourceFolderName);


                foreach (var file in Directory.GetFiles(folders.Source, "*.*"))
                {
                    var targetFile = Path.Combine(string.Format("{0}\\{1}", folders.Target, sourceFolderName), Path.GetFileName(file));
                    if (System.IO.File.Exists(targetFile)) System.IO.File.Delete(targetFile);
                    System.IO.File.Move(file, targetFile);
                }

                foreach (var folder in Directory.GetDirectories(folders.Source))
                {
                    stack.Push(new Folders(folder, Path.Combine(string.Format("{0}\\{1}", folders.Target, sourceFolderName))));
                }
            }
            Directory.Delete(source, true);
        }

        public void DragDirectory(string sourcepath, string destpath, string source)
        {
            var destinationPath = "/" + Request.Url.Host + destpath;
            var sourcePath = "/" + Request.Url.Host + sourcepath;

            var fullOldName = Request.MapPath(sourcepath);
            var fullNewName = Request.MapPath(destpath);

            Directory.CreateDirectory(fullNewName + "\\" + source);
            var newDestination = string.Format(@"{0}\{1}", Request.MapPath(destpath), source);

            /* start  - copy sub directory  */
            var dir = new DirectoryInfo(fullOldName);
            var dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + fullOldName);
            }

            if (!Directory.Exists(newDestination))
            {
                Directory.CreateDirectory(newDestination);
            }

            var files = dir.GetFiles();
            foreach (var file in files)
            {
                var temppath = Path.Combine(newDestination, file.Name);
                file.CopyTo(temppath, false);
            }

            foreach (var subdir in dirs)
            {
                var temppath = Path.Combine(newDestination, subdir.Name);
                CopyDirectories(subdir.FullName, temppath);

            }
            /* end sub directory */

            Directory.Delete(fullOldName, true);
        }

        private class Folders
        {
            public string Source { get; private set; }
            public string Target { get; private set; }

            public Folders(string source, string target)
            {
                Source = source;
                Target = target;
            }
        }

        [FileManagerViewFilter]
        public ActionResult ViewFilesFromFolder(string folder, int mID)
        {
            try
            {
                SetModuleId(mID);
                var directory = new DirectoryInfo(Request.MapPath(folder));
                var folderContent = new FolderContent();

                foreach (var f in directory.GetFiles())
                {

                    folderContent.Files.Add(new Files
                                                {
                                                    fullName = string.Format("{0}/{1}", folder, f.Name),
                                                    name = f.Name,
                                                    folder = folder
                                                });
                }

                foreach (var f in directory.GetDirectories())
                {
                    folderContent.Folders.Add(new Files
                                                  {
                                                      fullName = string.Format("{0}/{1}", folder, f.Name),
                                                      name = f.Name,
                                                      folder = folder
                                                  });
                }

                return View("FilesView", folderContent);
            }
            catch (Exception)
            {
                return View("FolderDoesntExist");
            }
        }

        public ActionResult UploadFile(HttpPostedFileBase fileData, string folderName)
        {

            if (fileData == null)
            {
                return new EmptyResult();
            }

            if (fileData.ContentLength == 0)
            {
                return Json("Nothing to upload");
            }

            var path = Request.MapPath(folderName);

            if (!Directory.Exists(path))
            {
                return Json("wrong directory");
            }

            var fullName = string.Format(@"{0}\{1}", path, fileData.FileName);
            fileData.SaveAs(fullName);

            return Json("OK");
        }

        [FileManagerEditFilter]
        public JsonResult DeleteFile(string file, string folder, int mID)
        {
            try
            {
                SetModuleId(mID);
                var fullName = string.Format(@"{0}\{1}", Request.MapPath(folder), file);
                System.IO.File.Delete(fullName);
                return Json("ok");
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(e.Message);
            }
        }

        [FileManagerEditFilter]
        public JsonResult CreateNewFolder(string folder, string name, int mID)
        {
            try
            {
                SetModuleId(mID);

                //var folderpath = folder;
                //var index = folderpath.LastIndexOf('/');

                //var directory = folderpath.Substring(0, index + 1);

                CreateFolderInPath(Request.MapPath(folder), name);

                return Json("ok");
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(e.Message);
            }
        }

        [FileManagerEditFilter]
        public JsonResult RenameFile(string file, string folder, string name, int mID)
        {
            try
            {
                SetModuleId(mID);
                if (file.LastIndexOf('.') > 0)
                {
                    var extension = file.Substring(file.LastIndexOf('.'));
                    name += extension;
                }
                var fullOldName = string.Format(@"{0}\{1}", Request.MapPath(folder), file);
                var fullNewName = string.Format(@"{0}\{1}", Request.MapPath(folder), name);
                System.IO.File.Move(fullOldName, fullNewName);

                return Json("ok");
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(e.Message);
            }
        }

        [FileManagerEditFilter]
        public JsonResult RenameFolder(string folder, string parentFolder, string newName, int mID)
        {
            try
            {
                SetModuleId(mID);
                if (folder != newName)
                {
                    var fullOldName = string.Format(@"{0}\{1}", Request.MapPath(parentFolder), folder);
                    var fullNewName = string.Format(@"{0}\{1}", Request.MapPath(parentFolder), newName);
                    Directory.Move(fullOldName, fullNewName);
                }
                return Json("ok");
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(e.Message);
            }
        }

        [FileManagerEditFilter]
        public JsonResult PasteFile(string file, string folder, string newFolder, bool isCopy, bool isFolder, int mID)
        {
            try
            {
                SetModuleId(mID);
                if (isFolder)
                {
                    var fullOldName = string.Format(@"{0}\{1}", Request.MapPath(folder), file);
                    var fullNewName = string.Format(@"{0}\{1}", Request.MapPath(newFolder), file);
                    CopyDirectories(fullOldName, fullNewName);

                    if (!isCopy)
                    {
                        DeleteDirectory(fullOldName);
                    }

                }
                else
                {
                    var fullOldName = string.Format(@"{0}\{1}", Request.MapPath(folder), file);
                    var fullNewName = string.Format(@"{0}\{1}", Request.MapPath(newFolder), file);
                    System.IO.File.Copy(fullOldName, fullNewName);

                    if (!isCopy)
                    {
                        System.IO.File.Delete(fullOldName);
                    }
                }

                //

                return Json("ok");
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(e.Message);
            }
        }

        [FileManagerEditFilter]
        public JsonResult DeleteFolder(string folder, string parentfolder, int mID)
        {
            try
            {
                SetModuleId(mID);
                var fullName = string.Format(@"{0}\{1}", Request.MapPath(parentfolder), folder);
                DeleteDirectory(fullName);
                return Json("ok");
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(e.Message);
            }
        }

        private static void DeleteDirectory(string sourceDirName)
        {
            var dir = new DirectoryInfo(sourceDirName);
            dir.Delete(true);

        }

        #region File Opretions
        public JsonResult CopyFileFullPath(string file, string folder, int mID)
        {
            var filepath = Request.Url.Host + folder + "/" + file;
            return Json(filepath.ToUpper());
        }

        public JsonResult CopyFolderFullPath(string file, string folder, int mID)
        {
            var filepath = Request.Url.Host;
            return Json(filepath.ToUpper());
        }

        public JsonResult GetPageText(string file, string folder, int mID)
        {
            var path = Request.MapPath(folder + "/" + file);
            var filetext = System.IO.File.ReadAllText(path);
            return Json(filetext);
        }

        public JsonResult EditPageText(string file, string folder, string content)
        {
            var path = Request.MapPath(folder + "/" + file);
            System.IO.File.WriteAllText(path, content);

            var filetext = System.IO.File.ReadAllText(path);
            return Json(filetext);
        }
        #endregion

        # region Updated Js Tree
        [HttpPost]
        [FileManagerViewFilter]
        public JsonResult GetRootNodes(string mID)
        {
            JsTreeItem rootNode = new JsTreeItem();
            rootNode.text = "/";
            rootNode.data = "/";
            rootNode.id = "0";
            return Json(rootNode, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// A method to populate a TreeView with directories, subdirectories, etc
        /// </summary>
        /// <param name="dir">The path of the directory</param>
        /// <param name="node">The "master" node, to populate</param>
        public List<JsTreeItem> PopulateJsTree(string dir, JsTreeItem node)
        {
            List<JsTreeItem> nodelist = new List<JsTreeItem>();
            var directory = new DirectoryInfo(Request.MapPath(dir));

            if (directory.GetDirectories().Length > 0)
            {
                nodelist = new List<JsTreeItem>();
            }
            // get the information of the directory

            // loop through each subdirectory

            // Get list of folders to be render in left side folder tree
            var allowfolderlist = System.Configuration.ConfigurationManager.AppSettings["FileManager.AllowFolders.Tree"].ToString();
            var allowfolders = allowfolderlist.Split('|');

            foreach (var t in from d in directory.GetDirectories()
                              let dirName = string.Format("{0}/{1}", dir, d.Name)
                              select new JsTreeItem
                              {
                                  attr = new JsTreeAttribute { id = dirName },
                                  data = d.Name,
                                  text = d.Name,
                                  state = new state { opened = false }
                              }
                                  into t
                                  select t)
            {
                if (dir == "/")
                {
                    foreach (var folder in allowfolders)
                    {
                        if (t.data == folder)
                        {
                            nodelist.Add(t);// add the node to the "master" node    
                        }
                    }
                }
                else
                {
                    nodelist.Add(t);// add the child nodes to the "master" nodes  
                }
            }

            return nodelist;
        }


        [HttpPost]
        [FileManagerViewFilter]
        public JsonResult GetChildrenNodes(string dir, int mID)
        {
            List<JsTreeItem> listnodes = new List<JsTreeItem>();

            SetModuleId(mID);
            var rootNode = new JsTreeItem { attr = new JsTreeAttribute { id = dir }, data = dir.Substring(1), text = dir.Substring(1) };
            var rootPath = dir;
            rootNode.attr.id = rootPath;
            listnodes = (PopulateJsTree(rootPath, rootNode));
            return Json(listnodes, JsonRequestBehavior.AllowGet);
        }

        [FileManagerEditFilter]
        public JsonResult DeleteJsTreeFolder(string folderpath, int mID)
        {
            try
            {
                SetModuleId(mID);
                var fullName = string.Format(@"{0}", Request.MapPath(folderpath));
                DeleteDirectory(fullName);
                return Json("ok");
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(e.Message);
            }
        }

        [FileManagerEditFilter]
        public JsonResult PasteJsTreeFolder(string oldfolder, string newFolder, bool isCopy, int mID)
        {
            try
            {
                SetModuleId(mID);
                var foldername = System.IO.Path.GetFileName(oldfolder);
              
                    var fullOldName = string.Format(@"{0}", Request.MapPath(oldfolder));
                    var fullNewName = string.Format(@"{0}\{1}", Request.MapPath(newFolder),foldername);
                    CopyDirectories(fullOldName, fullNewName);

                    if (!isCopy)
                    {
                        DeleteDirectory(fullOldName);
                    }
                //

                return Json("ok");
            }
            catch (Exception e)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(e.Message);
            }
        }
        #endregion

    }
}
