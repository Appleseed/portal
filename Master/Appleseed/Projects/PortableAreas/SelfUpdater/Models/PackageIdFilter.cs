using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;

namespace SelfUpdater.Models
{


    public class PackageIdFilter : IActionFilter
    {
        [CompilerGenerated]
        private string PackageId_k__BackingField;

        public PackageIdFilter(string packageId)
        {
            this.PackageId = packageId;
        }

        private string GetPackageId(RequestContext context)
        {
            string name = context.RouteData.DataTokens["PackageId"] as string;
            if (name == null)
            {
                name = new DirectoryInfo(HostingEnvironment.ApplicationPhysicalPath).Name;
            }
            return name;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (this.PackageId == null)
            {
                this.PackageId = this.GetPackageId(filterContext.RequestContext);
            }
            filterContext.ActionParameters["PackageId"] = this.PackageId;
        }

        public string PackageId
        {
            [CompilerGenerated]
            get
            {
                return this.PackageId_k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.PackageId_k__BackingField = value;
            }
        }
    }
}

