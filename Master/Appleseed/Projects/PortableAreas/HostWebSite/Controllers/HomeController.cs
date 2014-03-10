using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HostWebSite.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "Instrucciones:";

            return View();
        }

        public ActionResult Amadeus()
        {
            return View();
        }

        public ActionResult CustomPackages()
        {
            return View();
        }
    }
}
