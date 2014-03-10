using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using NuGet;
using SelfUpdater.Controllers;

namespace SelfUpdater.Code
{
    public static class ProjectManagerHelper
    {
        public static WebProjectManager[] GetProjectManagers()
        {
            var remoteSources = ConfigurationManager.AppSettings["PackageSource"] ?? @"D:\";
            var managers = new List<WebProjectManager>();
            foreach (var remoteSource in remoteSources.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                managers.Add(new WebProjectManager(remoteSource, HttpRuntime.AppDomainAppPath));
            }

            return managers.ToArray();
        }

        public static List<IPackage> GetInstalledPackages(WebProjectManager projectManager, bool filterTags)
        {
            var packages = projectManager.GetInstalledPackages(filterTags).ToList();

            return packages;
        }

        public static List<IPackage> GetAvailablePackages(WebProjectManager projectManager)
        {
            var packages = projectManager.GetRemotePackages().ToList();

            return packages;
        }

        public static List<IPackage> GetAvailablePackagesLatestList(WebProjectManager projectManager)
        {
            var availablePackages = GetAvailablePackages(projectManager);
            return availablePackages.GroupBy(x => x.Id).Select(pack => pack.Single(y => y.Version == pack.Max(x => x.Version))).ToList();
        }

        public static List<IPackage> GetInstalledPackagesLatestList(WebProjectManager projectManager, bool filterTags)
        {
            var installedPackages = GetInstalledPackages(projectManager, filterTags);
            var packagesList = installedPackages.GroupBy(x => x.Id);
            return packagesList.Select(pack => pack.Single(y => y.Version == pack.Max(x => x.Version))).ToList();
        }
    }
}