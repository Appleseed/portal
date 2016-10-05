using System.Web.Mvc;
using MvcContrib.PortableAreas;
using System.Reflection;
using Appleseed.Core;
using System.Web.Routing;

namespace AppleseedProxy {
    public class AppleseedProxyRegistration : MvcContrib.PortableAreas.PortableAreaRegistration {
        public override string AreaName {
            get {
                return "ASProxy";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus) {
            context.MapRoute("ASProxy_ResourceRoute", "ASProxy/resource/{resourceName}",
               new { controller = "EmbeddedResource", action = "Index" },
               new string[] { "MvcContrib.PortableAreas" });

            context.MapRoute("ASProxy_ResourceImageRoute", "ASProxy/images/{resourceName}",
              new { controller = "EmbeddedResource", action = "Index", resourcePath = "images" },
              new string[] { "MvcContrib.PortableAreas" });

            context.MapRoute("ASProxy.Core_ResourceScriptRoute", "ASProxy/scripts/{resourceName}",
              new { controller = "EmbeddedResource", action = "Index", resourcePath = "Scripts" },
              new string[] { "MvcContrib.PortableAreas" });
            
            context.MapRoute(
                "ASProxy_default",
                "ASProxy/{controller}/{action}/{id}/{*path}",
                new { action = "Index", controller = "Proxy", id = UrlParameter.Optional }
            );

            //var route = new Route(
            //    "Proxy/{proxyId}/{*path}",
            //    new RouteValueDictionary { { "area", "Proxy" }, { "controller", "Proxy" }, { "action", "Index" } },
            //    new RouteValueDictionary(),
            //    new RouteValueDictionary { { "area", "Proxy" } },
            //    new MvcRouteHandler()
            //);
            //route.DataTokens.Add("IgnoreJSON", true);

            //return new[] {
            //    new RouteDescriptor {
            //        Priority = 5,
            //        Route = route
            //    }
            //};

            this.RegisterAreaEmbeddedResources();
            PortableAreaUtils.RegisterScripts(this, context, bus);
        }

    }
}
