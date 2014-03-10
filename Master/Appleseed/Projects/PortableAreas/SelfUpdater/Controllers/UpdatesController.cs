
using NuGet;
using System;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Collections.Generic;
using SelfUpdater.Models;
using Appleseed.Framework;
using System.IO;
using System.Xml;
using System.Text;
using System.Dynamic;
using Appleseed.Framework.Security;

namespace SelfUpdater.Controllers
{
    [Authorize]
    public class UpdatesController : BaseController
    {
        public ActionResult Module()
        {
            return View();
        }

        public ActionResult UpdateModule() {

            try {

                WebProjectManager[] projectManagers = GetProjectManagers();
                var installed = new List<InstallationState>();
                foreach (var projectManager in projectManagers) {
                    var installedPackages = this.GetInstalledPackages(projectManager);
                    var packagesList = installedPackages.GroupBy(x => x.Id);
                    var installedPackagesList = packagesList.Select(pack => pack.Single(y => y.Version == pack.Max(x => x.Version))).ToList();

                    foreach (var installedPackage in installedPackagesList)
                    {
                        IPackage update = projectManager.GetUpdatedPackage(installedPackage);
                        var package = new InstallationState();
                        package.Installed = installedPackage;
                        package.Update = update;
                        package.Source = projectManager.SourceRepository.Source;

                        if (installed.Any(d => d.Installed.Id == package.Installed.Id)) {
                            var addedPackage = installed.Where(d => d.Installed.Id == package.Installed.Id).First();
                            if (package.Update != null) {
                                if (addedPackage.Update == null || addedPackage.Update.Version < package.Update.Version) {
                                    installed.Remove(addedPackage);
                                    installed.Add(package);
                                }
                            }
                        }
                        else {
                            installed.Add(package);
                        }
                    }
                }

                var model = new UpdatesModel();
                model.Updates = installed;


                return View(model);
            }
            catch (Exception e) {
                ErrorHandler.Publish(LogLevel.Error, "Nuget Get packages from feed", e);
                return View("ExceptionError");
            }
        
        }

        //public ActionResult Upgrade(string packageId, string source)
        //{
        //    try {

        //        var projectManager = GetProjectManagers().Where(p => p.SourceRepository.Source == source).First();

        //        projectManager.addLog("Starting update...");

        //        IPackage installedPackage = GetInstalledPackage(projectManager, packageId);

        //        IPackage update = projectManager.GetUpdate(installedPackage);

        //        projectManager.UpdatePackage(update);

        //        projectManager.addLog("Waiting to Reload Site");

        //        var logger = (string)System.Web.HttpContext.Current.Application["NugetLogger"];

        //        return Json(new {
        //            msg = "Updated " + packageId + " to " + update.Version.ToString() + "!",
        //            updated = true, NugetLog = logger
        //        }, JsonRequestBehavior.AllowGet);
        //    } catch (Exception exc) {
        //        ErrorHandler.Publish(LogLevel.Error, exc);

        //        return Json(new {
        //            msg = "Error updating " + packageId,
        //            updated = false
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //}


        //public ActionResult DelayedUpgrade(string packageId, string source, string version)
        //{
        //    SelfUpdaterEntities context = new SelfUpdaterEntities();

        //    var entity = context.SelfUpdatingPackages.Where(d => d.PackageId == packageId).FirstOrDefault();
        //    if (entity == default(SelfUpdatingPackages)) {

        //        entity = new SelfUpdatingPackages() {
        //            PackageId = packageId,
        //            Source = source,
        //            PackageVersion = version                    
        //        };

        //        context.SelfUpdatingPackages.AddObject(entity);
        //        context.SaveChanges();
        //    }

        //    return Json(new {
        //        msg = "Package " + packageId + " scheduled to update!",
        //        res = true
        //    }, JsonRequestBehavior.AllowGet);

        //}

        //public ActionResult RemoveDelayedUpgrade(string packageId)
        //{
        //    SelfUpdaterEntities context = new SelfUpdaterEntities();

        //    var entity = context.SelfUpdatingPackages.Where(d => d.PackageId == packageId).FirstOrDefault();
        //    if (entity != default(SelfUpdatingPackages)) {
        //        context.SelfUpdatingPackages.DeleteObject(entity);
        //        context.SaveChanges();


        //        return Json(new {
        //            msg = "Package " + packageId + " unscheduled!",
        //            res = true
        //        }, JsonRequestBehavior.AllowGet);
        //    } else {
        //        return Json(new {
        //            msg = "Package " + packageId + " unscheduled!",
        //            res = false
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        //public void RestartSite()
        //{
        //    if (PortalSecurity.IsInRole("Admins")) {
        //        /*Forcing site restart*/
        //        var doc = new XmlDocument();
        //        doc.PreserveWhitespace = true;
        //        var configFile = Server.MapPath("~/web.config");
        //        doc.Load(configFile);

        //        var writer = new XmlTextWriter(configFile, Encoding.UTF8) { Formatting = Formatting.Indented };
        //        doc.Save(writer);
        //        writer.Flush();
        //        writer.Close();
        //        /*....................*/
        //    }
        //}

        //public ActionResult Status()
        //{
        //    var logger = (string)System.Web.HttpContext.Current.Application["NugetLogger"];
        //    if (logger == null || logger.Equals(string.Empty)) {
        //        return Json(true);
        //    }
        //    else {
        //        return Json(false);
        //    }
        //}

        private IPackage GetInstalledPackage(WebProjectManager projectManager, string packageId)
        {
            IPackage package = projectManager.GetInstalledPackages(false).Where(d => d.Id == packageId).FirstOrDefault();

            if (package == null) {
                throw new InvalidOperationException(string.Format("The package for package ID '{0}' is not installed in this website. Copy the package into the App_Data/packages folder.", packageId));
            }
            return package;
        }

        //public JsonResult NugetStatus() {

        //    //var projectManager = GetProjectManagers().Where(p => p.SourceRepository.Source == source).First();

        //    //var logger = (string)System.Web.HttpContext.Current.Application["NugetLogger"];
            

        //    //var logger = projectManager.getLogs();
        //    return Json(logger, JsonRequestBehavior.AllowGet);

        //}
    }
}