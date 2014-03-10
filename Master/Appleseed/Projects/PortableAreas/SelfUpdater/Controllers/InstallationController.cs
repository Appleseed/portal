using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Dynamic;
using Appleseed.Framework;
using SelfUpdater.Code;
using SelfUpdater.Models;

namespace SelfUpdater.Controllers
{
    public class InstallationController : BaseController
    {
        public ActionResult Module()
        {

            var projectManagers = GetProjectManagers();
            var packagesToInstall = new List<InstallPackagesModel>();
            var installed = new List<InstallationState>();
            foreach (var projectManager in projectManagers)
            {
                var availablePackages = ProjectManagerHelper.GetAvailablePackagesLatestList(projectManager);

                var installedPackages = ProjectManagerHelper.GetInstalledPackagesLatestList(projectManager, false);

                foreach (var package in availablePackages)
                {
                    if (installedPackages.All(d => d.Id != package.Id))
                    {
                        var pack = new InstallPackagesModel
                                       {
                                           icon =
                                               string.IsNullOrEmpty(package.IconUrl.ToString())
                                                   ? package.IconUrl.ToString()
                                                   : string.Empty,
                                           name = package.Id,
                                           version = package.Version.ToString(),
                                           author = package.Authors.FirstOrDefault(),
                                           source = projectManager.SourceRepository.Source
                                       };

                        packagesToInstall.Add(pack);
                    }
                }

                foreach (var installedPackage in installedPackages)
                {
                    var update = projectManager.GetUpdatedPackage(availablePackages, installedPackage);
                    var package = new InstallationState();
                    package.Installed = installedPackage;
                    package.Update = update;
                    package.Source = projectManager.SourceRepository.Source;

                    if (installed.Any(d => d.Installed.Id == package.Installed.Id))
                    {
                        var addedPackage = installed.First(d => d.Installed.Id == package.Installed.Id);
                        if (package.Update != null)
                        {
                            if (addedPackage.Update == null || addedPackage.Update.Version < package.Update.Version)
                            {
                                installed.Remove(addedPackage);
                                installed.Add(package);
                            }
                        }
                    }
                    else
                    {
                        installed.Add(package);
                    }
                }
            }

            var model = new NugetPackagesModel {Install = packagesToInstall, Updates = installed};

            return View(model);
        }

        public ActionResult InstallModule()
        {
            try {

                var projectManagers = GetProjectManagers();
                var list = new List<dynamic>();
                var installed = projectManagers.SelectMany(d => d.GetInstalledPackages(false).ToList());

                foreach (var pM in projectManagers)
                {
                    var packages = GetAvailablePackages(pM);
                    foreach (var package in packages) {
                        if (!installed.Any(d => d.Id == package.Id)) {
                            dynamic p = new ExpandoObject();
                            p.icon = package.IconUrl;
                            p.icon = p.icon ?? string.Empty;
                            p.name = package.Id;
                            p.version = package.Version;
                            p.author = package.Authors.FirstOrDefault();
                            p.source = pM.SourceRepository.Source;

                            list.Add(p);
                        }
                    }
                }

                

                return View(list);
            }
            catch (Exception e) {
                ErrorHandler.Publish(LogLevel.Error, "Nuget Get packages from feed", e);
                return View("ExceptionError");
            }
        
        }

        public JsonResult InstallPackages(string packages)
        {
            try
            {
                var packagesToInstall = new JavaScriptSerializer().Deserialize<IEnumerable<PackageModel>>(packages);

                var context = new SelfUpdaterEntities();

                foreach (var self in packagesToInstall.Select(pack => new SelfUpdatingPackages { PackageId = pack.Name, PackageVersion = pack.Version, Source = pack.Source, Install = pack.Install}))
                {
                    context.AddToSelfUpdatingPackages(self);
                }

                context.SaveChanges();

                var config = WebConfigurationManager.OpenWebConfiguration("~/");
                var section = config.GetSection("system.web/httpRuntime");
                ((HttpRuntimeSection)section).WaitChangeNotification = 123456789;
                ((HttpRuntimeSection)section).MaxWaitChangeNotification = 123456789;
                config.Save();
                

                return Json("Ok");
            }
            catch(Exception e)
            {
                ErrorHandler.Publish(LogLevel.Error, e);
                Response.StatusCode = 500;
                return Json(e.Message);
            }


        }

        //public ActionResult InstallPackage(string packageId, string source, string version)
        //{
        //    //System.Web.HttpContext.Current.Session["NugetLogger"] = "Installing packages...";
        //    try
        //    {
        //        var projectManager = GetProjectManagers().Where(p => p.SourceRepository.Source == source).First();

        //        projectManager.addLog("Starting installation...");

        //       projectManager.InstallPackage(packageId, new SemanticVersion(version));

        //        projectManager.addLog("Waiting to Reload Site");

        //        var logger = (string) System.Web.HttpContext.Current.Application["NugetLogger"];

        //        return Json(new
        //                        {
        //                            msg = "Package " + packageId + " scheduled to install!",
        //                            res = true,
        //                            NugetLog = logger
        //                        }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch(Exception e)
        //    {
        //        ErrorHandler.Publish(LogLevel.Error, e);
        //        Response.StatusCode = 500;
        //        return Json(e.Message);
        //    }
        //}    
    
        

    }
}