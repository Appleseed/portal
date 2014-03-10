using System.Web.Mvc;
using MvcContrib.PortableAreas;
using Appleseed.Core.ApplicationBus;
using System;

namespace Appleseed.Core
{
    public class AppleseedCoreRegistration : MvcContrib.PortableAreas.PortableAreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Appleseed.Core";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus)
        {
            var currentState = (PortableAreaUtils.RegistrationState)Enum.Parse(typeof(PortableAreaUtils.RegistrationState), context.State.ToString());

            if (currentState == PortableAreaUtils.RegistrationState.Initializing) {
                context.MapRoute("Appleseed.Core_ResourceRoute", "Appleseed.Core/resource/{resourceName}",
                   new { controller = "EmbeddedResource", action = "Index" },
                   new string[] { "MvcContrib.PortableAreas" });

                context.MapRoute("Appleseed.Core_ResourceImageRoute", "Appleseed.Core/images/{resourceName}",
                  new { controller = "EmbeddedResource", action = "Index", resourcePath = "images" },
                  new string[] { "MvcContrib.PortableAreas" });

                context.MapRoute("Appleseed.Core_ResourceScriptRoute", "Appleseed.Core/scripts/{resourceName}",
                  new { controller = "EmbeddedResource", action = "Index", resourcePath = "Scripts" },
                  new string[] { "MvcContrib.PortableAreas" });

                context.MapRoute(
                    "Appleseed.Core_default",
                    "Appleseed.Core/{controller}/{action}/{id}",
                    new { action = "Index", controller = "Home", id = UrlParameter.Optional }
                );

                RegisterAreaEmbeddedResources();
                PortableAreaUtils.RegisterScripts(this, context, bus);
            }
        }
    }
}