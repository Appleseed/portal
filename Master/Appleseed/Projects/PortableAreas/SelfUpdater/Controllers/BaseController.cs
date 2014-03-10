using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NuGet;
using System.Dynamic;
using System.Configuration;
using SelfUpdater.Code;

namespace SelfUpdater.Controllers
{
    public class BaseController : Controller
    {

        protected WebProjectManager[] GetProjectManagers()
        {
            return ProjectManagerHelper.GetProjectManagers();
        }

        protected List<IPackage> GetInstalledPackages(WebProjectManager projectManager)
        {
            var packages = projectManager.GetInstalledPackages(false).ToList();

            return packages;
        }

        protected List<IPackage> GetAvailablePackages(WebProjectManager projectManager)
        {
            var packages = projectManager.GetRemotePackages().ToList();

            return packages;
        }

    }
}
