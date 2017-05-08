using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppleseedMemberRegistrationRequests
{
    public static class Extensions
    {
        public static string AppleseedMemberRegistrationRequestsResource(this UrlHelper urlHelper, string resourceName)
        {
            var areaName = (string)urlHelper.RequestContext.RouteData.DataTokens["area"];
            return urlHelper.Action("Index", "Resource", new { resourceName = resourceName, area = areaName });
        }
    }
}