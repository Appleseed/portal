using System.Web.Mvc;
using MvcContrib.PortableAreas;
using Appleseed.Core.ApplicationBus;
using Appleseed.Core;
using System.Reflection;
using Appleseed.Framework.Core.Model;

namespace PageManagerTree {
    public class PageManagerTreeRegistration : MvcContrib.PortableAreas.PortableAreaRegistration {
        public override string AreaName {
            get {
                return "PageManagerTree";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus) {
            context.MapRoute("PageManagerTree_ResourceRoute", "PageManagerTree/resource/{resourceName}",
               new { controller = "EmbeddedResource", action = "Index" },
               new string[] { "MvcContrib.PortableAreas" });

            context.MapRoute("PageManagerTree_ResourceImageRoute", "PageManagerTree/images/{resourceName}",
              new { controller = "EmbeddedResource", action = "Index", resourcePath = "images" },
              new string[] { "MvcContrib.PortableAreas" });

            context.MapRoute("PageManagerTree.Core_ResourceScriptRoute", "PageManagerTree/scripts/{resourceName}",
              new { controller = "EmbeddedResource", action = "Index", resourcePath = "Scripts" },
              new string[] { "MvcContrib.PortableAreas" });
            
            context.MapRoute(
                "PageManagerTree_default",
                "PageManagerTree/{controller}/{action}/{id}",
                new { action = "Module", controller = "Home", id = UrlParameter.Optional }
            );

            //var assemblyName = Assembly.GetAssembly(this.GetType()).FullName;


            //var generalModuleDefId = ModelServices.RegisterPortableAreaModule(AreaName, assemblyName, "PageManagerTree");
            //var moduleDefId = ModelServices.AddModuleToPortal(generalModuleDefId, 0);

            this.RegisterAreaEmbeddedResources();
            PortableAreaUtils.RegisterScripts(this, context, bus);
        }

    }
}
