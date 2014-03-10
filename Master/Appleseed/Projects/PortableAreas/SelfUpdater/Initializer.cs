using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Appleseed.Framework;
using NuGet;
using SelfUpdater.Controllers;
using SelfUpdater.Models;

namespace SelfUpdater
{
    public class Initializer
    {
        public static void Initialize()
        {
            // Whatever can we do here?
            //CheckForSelfUpdates();
        }

        private static void CheckForSelfUpdates()
        {

            try {

                SelfUpdaterEntities context = new SelfUpdaterEntities();

                var packagesToUpdate = context.SelfUpdatingPackages.AsQueryable();

                if (packagesToUpdate.Count() > 0) {

                    /*This forces a site restart for each update scheduled */
                    /*Must be improved trying to updated all at once */


                    var packageToUpdate = packagesToUpdate.First();
                    var projectManager = GetProjectManagers().Where(d => d.SourceRepository.Source.ToLower().Trim() == packageToUpdate.Source.ToLower().Trim()).First();
                    var packageName = packageToUpdate.PackageId;
                    IPackage installedPackage = projectManager.GetInstalledPackages(true).Where(d => d.Id == packageName).First();
                    IPackage update = projectManager.GetUpdate(installedPackage);

                    if (update != null) {
                        ErrorHandler.Publish(LogLevel.Info, String.Format("SelfUpdater: Updating {0} from {1} to {2}", packageName, installedPackage.Version, update.Version));
                        try {
                            projectManager.UpdatePackage(update);
                        } catch (Exception exc) {
                            ErrorHandler.Publish(LogLevel.Info, String.Format("SelfUpdater: Error updating {0} from {1} to {2}", packageName, installedPackage.Version, update.Version), exc);
                            context.SelfUpdatingPackages.DeleteObject(packageToUpdate);
                        }
                    } else {
                        ErrorHandler.Publish(LogLevel.Info, "SelfUpdater: " + packageName + " update applied !");
                        context.SelfUpdatingPackages.DeleteObject(packageToUpdate);
                    }

                    context.SaveChanges();

                } else {
                    ErrorHandler.Publish(LogLevel.Info, "SelfUpdater: Nothing to update");
                }
            } catch (Exception exc) {

                ErrorHandler.Publish(LogLevel.Error, exc);
            }
        }

        private static WebProjectManager[] GetProjectManagers()
        {
            string remoteSources = ConfigurationManager.AppSettings["PackageSource"] ?? @"D:\";
            List<WebProjectManager> managers = new List<WebProjectManager>();
            foreach (var remoteSource in remoteSources.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)) {
                managers.Add(new WebProjectManager(remoteSource, HttpContext.Current.Server.MapPath("~/")));
            }

            return managers.ToArray();
        }

    }
}