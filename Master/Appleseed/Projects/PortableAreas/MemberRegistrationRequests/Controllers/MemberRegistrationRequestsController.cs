using Appleseed.Framework;
using Appleseed.Framework.Helpers;
using Appleseed.Framework.Providers.AppleseedMembershipProvider;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Users.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MemberRegistrationRequests.Controllers
{
    public class MemberRegistrationRequestsController : Controller
    {
        [AcceptVerbs("GET", "HEAD", "POST", "PUT", "DELETE")]
        [ValidateInput(false)]
        public ActionResult Index()
        {
            var users = ((Appleseed.Framework.Providers.AppleseedMembershipProvider.AppleseedMembershipProvider)Membership.Provider).GetAllUsers(this.PortalSettings.PortalAlias).Cast<AppleseedUser>().Where(user => !user.IsApproved);

            return View(users);
        }

        private string GetToken(int pageid)
        {
            string tokenString = string.Empty;
            var portalSettings = Appleseed.Framework.Site.Configuration.PortalSettings.GetPortalSettings(pageid, Config.DefaultPortal);
            if (portalSettings.ActivePage.CustomSettings["PAGE_TOKEN"] != null && portalSettings.ActivePage.CustomSettings["PAGE_TOKEN"].Value != null && !string.IsNullOrEmpty(portalSettings.ActivePage.CustomSettings["PAGE_TOKEN"].Value.ToString()))
            {
                tokenString = portalSettings.ActivePage.CustomSettings["PAGE_TOKEN"].Value.ToString();
            }
            else
            {
                while (true)
                {
                    Guid g = Guid.NewGuid();
                    tokenString = Convert.ToBase64String(g.ToByteArray());
                    tokenString = tokenString.Replace("=", "");
                    tokenString = tokenString.Replace("+", "");
                    Appleseed.Framework.Site.Data.PagesDB pagedb1 = new Appleseed.Framework.Site.Data.PagesDB();
                    var pid = pagedb1.GetPageIdByToken(tokenString);
                    if (pid <= 0 && !tokenString.Contains("/"))
                    {
                        Appleseed.Framework.Site.Configuration.PageSettings.UpdatePageSettings(portalSettings.ActivePage.PageID, "PAGE_TOKEN", tokenString);
                        break;
                    }
                }

            }

            return tokenString;
        }

        [AcceptVerbs("GET", "HEAD", "POST", "PUT", "DELETE")]
        [HttpPost]
        public JsonResult ApproveUser(string userid)
        {
            var message = "APPROVED";
            var user = ((Appleseed.Framework.Providers.AppleseedMembershipProvider.AppleseedMembershipProvider)Membership.Provider).GetUser(Guid.Parse(userid), false);
            if (user != null)
            {
                user.IsApproved = true;
                ((Appleseed.Framework.Providers.AppleseedMembershipProvider.AppleseedMembershipProvider)Membership.Provider).UpdateUser(user);
            }
            return Json(new { ok = true, Message = message });
        }

        protected PortalSettings PortalSettings
        {
            get
            {
                return (PortalSettings)HttpContext.Items["PortalSettings"];
            }
        }
    }
}