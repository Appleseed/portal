using System.Web.Mvc;
using MvcContrib.PortableAreas;
using System.Reflection;
using Appleseed.Core;
using Appleseed.Framework.Core.Model;

//using Appleseed.Core.ApplicationBus;

namespace UserManager
{
    public class UserManagerRegistration : MvcContrib.PortableAreas.PortableAreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "UserManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus)
        {

            context.MapRoute("UserManager_ResourceRoute", "UserManager/resource/{resourceName}",
               new { controller = "EmbeddedResource", action = "Index" },
               new string[] { "MvcContrib.PortableAreas" });

            context.MapRoute("UserManager_ResourceImageRoute", "UserManager/images/{resourceName}",
              new { controller = "EmbeddedResource", action = "Index", resourcePath = "images" },
              new string[] { "MvcContrib.PortableAreas" });

            context.MapRoute(
                "UserManager_default",
                "UserManager/{controller}/{action}/{id}",
                new { action = "Index", controller = "Home", id = UrlParameter.Optional }
            );

            //var assemblyName = Assembly.GetAssembly(this.GetType()).FullName;

            //var generalModuleDefId = ModelServices.RegisterPortableAreaModule(AreaName, assemblyName, "UserManager");
            //var moduleDefId = ModelServices.AddModuleToPortal(generalModuleDefId, 0);
            //ModelServices.AddModuleToPage(moduleDefId, 180, "Available Packages", false);

            //generalModuleDefId = ModelServices.RegisterPortableAreaModule(AreaName, assemblyName, "Updates");
            //moduleDefId = ModelServices.AddModuleToPortal(generalModuleDefId, 0);
            //ModelServices.AddModuleToPage(moduleDefId, 180, "Package Updates", false);


            RegisterAreaEmbeddedResources();
            PortableAreaUtils.RegisterScripts(this, context, bus);

         


        }

    }
}
