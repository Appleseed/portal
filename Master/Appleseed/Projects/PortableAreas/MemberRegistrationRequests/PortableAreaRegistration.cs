using System.Web.Mvc;
using MvcContrib.PortableAreas;
using System.Reflection;
using Appleseed.Core;
using System.Web.Routing;

namespace MemberRegistrationRequests
{
    public class AppleseedMemberRegistrationRequestsRegistration : MvcContrib.PortableAreas.PortableAreaRegistration {
        public override string AreaName {
            get {
                return "ASMemberRegistrationRequests";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus) {
            context.MapRoute("ASMemberRegistrationRequests_ResourceRoute", "ASMemberRegistrationRequests/resource/{resourceName}",
               new { controller = "EmbeddedResource", action = "Index" },
               new string[] { "MvcContrib.PortableAreas" });

            context.MapRoute("ASMemberRegistrationRequests_ResourceImageRoute", "ASMemberRegistrationRequests/images/{resourceName}",
              new { controller = "EmbeddedResource", action = "Index", resourcePath = "images" },
              new string[] { "MvcContrib.PortableAreas" });

            context.MapRoute("ASMemberRegistrationRequests.Core_ResourceScriptRoute", "ASMemberRegistrationRequests/scripts/{resourceName}",
              new { controller = "EmbeddedResource", action = "Index", resourcePath = "Scripts" },
              new string[] { "MvcContrib.PortableAreas" });
            
            context.MapRoute(
                "ASMemberRegistrationRequests_default",
                "ASMemberRegistrationRequests/{controller}/{action}/{id}/{*path}",
                new { action = "Index", controller = "MemberRegistrationRequests", id = UrlParameter.Optional }
            );

            this.RegisterAreaEmbeddedResources();
            PortableAreaUtils.RegisterScripts(this, context, bus);
        }

    }
}
