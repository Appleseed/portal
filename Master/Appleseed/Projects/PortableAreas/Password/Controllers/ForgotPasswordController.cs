using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Appleseed.Framework.Site.Configuration;
using System.Web.Security;
using Appleseed.Framework.Providers.AppleseedMembershipProvider;
using Appleseed.Framework;
using Appleseed.Framework.Settings;
using System.Net.Mail;
using Appleseed.Framework.Helpers;
using Password.Models;

namespace Password.Controllers
{
    public class ForgotPasswordController : Controller
    {
        //
        // GET: /ForgotPassword/

        public ActionResult Index() {
            if (Request.IsAuthenticated) {
                Response.Redirect(HttpUrlBuilder.BuildUrl("~/"));
            }          

            return View();

        }

        protected PortalSettings PortalSettings {
            get {
                return (PortalSettings)HttpContext.Items["PortalSettings"];
            }
        }

        public JsonResult sendPasswordToken(string email) {

            Membership.ApplicationName = this.PortalSettings.PortalAlias;
            var membership = (AppleseedMembershipProvider)Membership.Provider;

            // Obtain single row of User information
            var memberUser = membership.GetUser(email, false);

            var message = string.Empty;

            if (memberUser == null) {
                message = General.GetString(
                    "SIGNIN_PWD_MISSING_IN_DB", "The e-mail address you entered could not be found. Make sure you are using the e-mail address associated with your user.", this);
                return Json(new { ok = false, Message = message });
                //throw new Exception(message);
            }

            var userId = (Guid)(memberUser.ProviderUserKey ?? Guid.Empty);

            // generate Token for user
            var token = membership.CreateResetPasswordToken(userId);

            String uri = HttpUrlBuilder.BuildUrl("~/Password/PasswordRecovery");

            var changePasswordUrl = string.Concat(Request.Url.Host, uri,
                "?usr=",
                userId.ToString("N"),
                "&tok=",
                token.ToString("N"));

            var mail = new MailMessage();

            // we check the PortalSettings in order to get if it has an sender registered 
            if (this.PortalSettings.CustomSettings["SITESETTINGS_ON_REGISTER_SEND_FROM"] != null) {
                var sf = this.PortalSettings.CustomSettings["SITESETTINGS_ON_REGISTER_SEND_FROM"];
                var mailFrom = sf.ToString();
                try {
                    mail.From = new MailAddress(mailFrom);
                }
                catch {
                    // if the address is not well formed, a warning is logged.
                    LogHelper.Logger.Log(
                        LogLevel.Warn,
                        string.Format(
                            @"This is the current email address used as sender when someone want to retrieve his/her password: '{0}'. 
Is not well formed. Check the setting SITESETTINGS_ON_REGISTER_SEND_FROM of portal '{1}' in order to change this value (it's a portal setting).",
                            mailFrom,
                            this.PortalSettings.PortalAlias));
                }
            }

            // if there is not a correct email in the portalSettings, we use the default sender specified on the web.config file in the mailSettings tag.
            mail.To.Add(new MailAddress(email));
            mail.Subject = string.Format(
                "{0} - {1}",
                this.PortalSettings.PortalName,
                General.GetString("SIGNIN_PWD_LOST", "I lost my password", this));

            var viewToSend = EmailSubject(memberUser.UserName, changePasswordUrl, this.PortalSettings.PortalName) as ViewResult;

            StringResult sr = new StringResult();
            sr.ViewName = viewToSend.ViewName;
            sr.MasterName = viewToSend.MasterName;
            sr.ViewData = viewToSend.ViewData;
            sr.TempData = viewToSend.TempData;
            sr.ExecuteResult(this.ControllerContext);
            string emailHtml = sr.Html;


            mail.Body = emailHtml;
            mail.IsBodyHtml = false;

            using (var client = new SmtpClient()) {
                try {
                    client.Send(mail);
                    message = General.GetString(
                        "SIGNIN_PWD_WAS_SENT", "Your password was sent to the address you provided", this);
                    //this.Message.TextKey = "SIGNIN_PWD_WAS_SENT";

                    return Json(new { ok = true, Message = message });

                }
                catch (Exception exception) {
                    message = General.GetString(
                        "SIGNIN_SMTP_SENDING_PWD_MAIL_ERROR",
                        "We can't send you your password. There were problems while trying to do so.");
                    //this.Message.TextKey = "SIGNIN_SMTP_SENDING_PWD_MAIL_ERROR";
                    LogHelper.Logger.Log(
                        LogLevel.Error,
                        string.Format(
                            "Error while trying to send the password to '{0}'. Perhaps you should check your SMTP server configuration in the web.config.",
                            email),
                        exception);
                    return Json(new { ok = false, Message = message });
                    
                }
            }
        }

        public ActionResult EmailSubject(string userName, string url, string PortalName) {

            var model = new PasswordModel();
            model.UserName = userName;
            model.url = url;
            model.PortalName = PortalName;

            return View("EmailSubject", model);
        }

    }
}
