using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Appleseed.Core.ApplicationBus;
using System.Reflection;
using MvcContrib.PortableAreas;
using System.Web.Routing;
using System.Web.Mvc;

namespace Appleseed.Core
{
    public static class PortableAreaUtils
    {

        public static void RegisterScripts(this PortableAreaRegistration portableArea, System.Web.Mvc.AreaRegistrationContext context, MvcContrib.PortableAreas.IApplicationBus bus)
        {
            bus.Send(new BusMessage { Message = portableArea.AreaName + " registered" });

            bus.Send(new DBScriptsMessage
            {
                AreaName = portableArea.AreaName,
                LastVersion = GetLastDBScriptVersion(portableArea),
                Scripts = GetScripts(portableArea)

            });
        }

        private static List<DBScriptDescriptor> GetScripts(PortableAreaRegistration portableArea)
        {
            string[] resources =  Assembly.GetAssembly(portableArea.GetType()).GetManifestResourceNames();

            var result = new List<DBScriptDescriptor>();

            foreach (var resource in resources.Where(d => d.Contains(".sql")))
            {
                var version = resource.Substring(resource.IndexOf("._") + 2, 11).Replace(".", "_");

                result.Add(new DBScriptDescriptor { Url = resource, Version = version });
            }

            return result;
        }

        private static string GetLastDBScriptVersion(PortableAreaRegistration portableArea)
        {
            string[] resources = Assembly.GetAssembly(portableArea.GetType()).GetManifestResourceNames();

            //eg: Appleseed.Core.DBScripts._20110413.01. Create_DBVersion_Table.sql

            var dbversions = resources.Where(d => d.Contains(".sql")).Select(d => d.Substring(d.IndexOf("._") + 2, 11));

            return dbversions.OrderBy(d => d).LastOrDefault();
        }

        public static void RegisterArea<T>(RouteCollection routes, object state) where T : AreaRegistration
        {
            AreaRegistration registration = (AreaRegistration)Activator.CreateInstance(typeof(T));
            AreaRegistrationContext context = new AreaRegistrationContext(registration.AreaName, routes, state);
            string tNamespace = registration.GetType().Namespace;
            if (tNamespace != null)
            {
                context.Namespaces.Add(tNamespace + ".*");
            }
            registration.RegisterArea(context);
        }

        public enum RegistrationState
        {
            Initializing = 0,
            Bootstrapping = 1
        }
    }
}