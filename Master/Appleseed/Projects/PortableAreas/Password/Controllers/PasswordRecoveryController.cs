using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Appleseed.Framework;
using Password.Models;
using System.Web.Security;
using Appleseed.Framework.Providers.AppleseedMembershipProvider;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Helpers;

namespace Password.Controllers
{
    public class PasswordRecoveryController : Controller
    {
        //
        // GET: /PasswordRecovery/

        protected PortalSettings PortalSettings
        {
            get
            {
                return (PortalSettings)HttpContext.Items["PortalSettings"];
            }
        }

        public void GoHome()
        {
            Response.Redirect(HttpUrlBuilder.BuildUrl("~/"));
        }

        public ActionResult Index()
        {

            if (Request.IsAuthenticated)
            {
                Response.Redirect(HttpUrlBuilder.BuildUrl("~/"));
            }

            return View();
        }

        public ActionResult PasswordRecoveryIndex()
        {

            var model = new RecoveryModel();
            if (Request.QueryString["usr"] == null || Request.QueryString["tok"] == null)
            {
                model.message = General.GetString("CHANGE_PWD_INVALID_URL_ERROR", "You are not allowed to use this functionality witout the correct parameters.");
                model.error = true;
                return View("ErrorPasswordRecoveryIndex", model);
            }

            Guid userId;
            Guid token;

            if (!Guid.TryParse(Request.QueryString["usr"].ToString(), out userId) ||
                !Guid.TryParse(Request.QueryString["tok"].ToString(), out token))
            {
                model.message = General.GetString("CHANGE_PWD_INVALID_URL_ERROR", "You are not allowed to use this functionality witout the correct parameters.");
                model.error = true;
                return View("ErrorPasswordRecoveryIndex", model);
            }


            Membership.ApplicationName = this.PortalSettings.PortalAlias;
            var membership = (AppleseedMembershipProvider)Membership.Provider;
            if (!membership.VerifyTokenForUser(userId, token))
            {
                model.message = General.GetString("CHANGE_PWD_INVALID_TOKEN_ERROR", "The token is no longer valid.");
                model.error = true;
                return View("ErrorPasswordRecoveryIndex", model);
            }

            model.message = "Enter your new password below.";
            model.UserId = userId;
            model.token = token;

            return View(model);

        }

        public JsonResult savePassword(string pwd1, string pwd2, Guid userId, Guid token)
        {
            string message = "";
            if (pwd1 != pwd2 || string.IsNullOrEmpty(pwd1) || string.IsNullOrEmpty(pwd2))
            {
                message = General.GetString("CHANGE_PWD_NOT_SAME_TWICE_ERROR", "The second password entered is not the same as the first one. Please write them again.");
                return Json(new { ok = true, Message = message });

            }

            try
            {
                Membership.ApplicationName = this.PortalSettings.PortalAlias;
                var membership = (AppleseedMembershipProvider)Membership.Provider;
                var user = membership.GetUser(userId, false);
                if (!membership.ChangePassword(user.UserName, token, pwd1))
                {
                    throw new ApplicationException("Error while trying to change the user password");
                }

                message = "Success! Your password has been reset.";

                return Json(new { ok = true, Message = message });
            }
            catch (Exception ex)
            {
                //ShowError("CHANGE_PWD_UNEXPECTED_ERROR", "An error ocurred while trying to update your password.");
                LogHelper.Logger.Log(
                    LogLevel.Error,
                    string.Format(@"Error while trying to update the password for user with id '{0}'. Token used: '{1}'.", userId, token)
                    , ex);

                message = "Success! Your password has been reset.";
                return Json(new { ok = true, Message = message });


            }
        }

    }
}
