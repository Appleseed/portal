using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Appleseed.Framework.Security;

namespace FileManager.Controllers
{
    public class FileManagerViewFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // This method is executed before calling the action
            // and here you have access to the route data:
            //var mID = (int) filterContext.ActionParameters["mID"];
            var mID = Convert.ToInt32(filterContext.ActionParameters["mID"]);

            if (!PortalSecurity.HasViewPermissions(mID))
            {
                PortalSecurity.AccessDenied();
                filterContext.Result = new EmptyResult();
            }

            else
                base.OnActionExecuting(filterContext);
        }

    }

    public class FileManagerEditFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // This method is executed before calling the action
            // and here you have access to the route data:
            var mID = (int)filterContext.ActionParameters["mID"];

            if (!PortalSecurity.HasEditPermissions(mID))
            {
                PortalSecurity.AccessDenied();
                filterContext.Result = new EmptyResult();
            }
            else
                base.OnActionExecuting(filterContext);
        }

    }


}