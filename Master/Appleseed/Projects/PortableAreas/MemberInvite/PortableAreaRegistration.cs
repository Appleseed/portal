using System.Web.Mvc;
using MvcContrib.PortableAreas;
using System.Reflection;
using Appleseed.Core;
using System.Web.Routing;

namespace MemberInvite {
    public class AppleseedMemberInviteRegistration : MvcContrib.PortableAreas.PortableAreaRegistration {
        public override string AreaName {
            get {
                return "ASMemberInvite";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus) {
            context.MapRoute("ASMemberInvite_ResourceRoute", "ASMemberInvite/resource/{resourceName}",
               new { controller = "EmbeddedResource", action = "Index" },
               new string[] { "MvcContrib.PortableAreas" });

            context.MapRoute("ASMemberInvite_ResourceImageRoute", "ASMemberInvite/images/{resourceName}",
              new { controller = "EmbeddedResource", action = "Index", resourcePath = "images" },
              new string[] { "MvcContrib.PortableAreas" });

            context.MapRoute("ASMemberInvite.Core_ResourceScriptRoute", "ASMemberInvite/scripts/{resourceName}",
              new { controller = "EmbeddedResource", action = "Index", resourcePath = "Scripts" },
              new string[] { "MvcContrib.PortableAreas" });
            
            context.MapRoute(
                "ASMemberInvite_default",
                "ASMemberInvite/{controller}/{action}/{id}/{*path}",
                new { action = "Index", controller = "MemberInvite", id = UrlParameter.Optional }
            );

            this.RegisterAreaEmbeddedResources();
            PortableAreaUtils.RegisterScripts(this, context, bus);
        }

    }
}
