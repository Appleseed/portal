using System.Reflection;
using System.Web.Mvc;
using Appleseed.Core;
using Appleseed.Framework.Core.Model;
using MvcContrib.PortableAreas;

namespace FileManager {
    public class PortableAreaMvc3Registration : MvcContrib.PortableAreas.PortableAreaRegistration {
        public override string AreaName {
            get {
                return "FileManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus) {
            context.MapRoute("PortableAreaMvc3_ResourceRoute", "FileManager/resource/{resourceName}",
               new { controller = "EmbeddedResource", action = "Index" },
               new string[] { "MvcContrib.PortableAreas" });

            context.MapRoute("PortableAreaMvc3_ResourceImageRoute", "FileManager/images/{resourceName}",
              new { controller = "EmbeddedResource", action = "Index", resourcePath = "images" },
              new string[] { "MvcContrib.PortableAreas" });

            context.MapRoute(
                "PortableAreaMvc3_default",
                "FileManager/{controller}/{action}/{id}",
                new { action = "Index", controller = "Home", id = UrlParameter.Optional }
            );

            var assemblyName = Assembly.GetAssembly(this.GetType()).FullName;


            //var generalModuleDefId = ModelServices.RegisterPortableAreaModule(AreaName, assemblyName, "Home");
            //var modDef =  ModelServices.AddModuleToPortal(generalModuleDefId, 0);
            //ModelServices.AddModuleToPage(modDef, 155, "FileManager", false);

            this.RegisterAreaEmbeddedResources();
            PortableAreaUtils.RegisterScripts(this, context, bus);


        }

    }
}
