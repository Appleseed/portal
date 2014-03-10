using System.Web.Mvc;
using MvcContrib.PortableAreas;
using Appleseed.Core.ApplicationBus;
using Appleseed.Core;

namespace Password {
    public class PasswordRegistration : MvcContrib.PortableAreas.PortableAreaRegistration {
        public override string AreaName {
            get {
                return "Password";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus) {
            context.MapRoute("Password_ResourceRoute", "Password/resource/{resourceName}",
               new { controller = "EmbeddedResource", action = "Index" },
               new string[] { "MvcContrib.PortableAreas" });

            context.MapRoute("Password_ResourceImageRoute", "Password/images/{resourceName}",
              new { controller = "EmbeddedResource", action = "Index", resourcePath = "images" },
              new string[] { "MvcContrib.PortableAreas" });

            context.MapRoute(
                "Password_default",
                "Password/{controller}/{action}/{id}",
                new { action = "Index", controller = "ForgotPassword", id = UrlParameter.Optional, area = AreaName }
            );

            this.RegisterAreaEmbeddedResources();
            PortableAreaUtils.RegisterScripts(this, context, bus);
        }

    }
}
