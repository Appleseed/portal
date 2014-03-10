using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;

namespace UserManager.Controllers
{
    public class ResourceController : Controller
    {
        public ActionResult Index(string resourceName)
        {
            var contentType = GetContentType(resourceName);
            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            return this.File(resourceStream, contentType);
        }

        private static string GetContentType(string resourceName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(resourceName).ToLower();
            switch (ext)
            {
                case ".js":
                    return "text/javascript";
            }

            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }
    }
}