using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PageManagerTree.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }

        public JsonResult TestValue() {
            return Json("testString");
        }

    }
}
