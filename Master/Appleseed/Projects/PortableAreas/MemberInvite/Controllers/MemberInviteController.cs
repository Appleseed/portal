using Appleseed.Framework;
using Appleseed.Framework.Helpers;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Site.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace MemberInvite.Controllers
{
    public class MemberInviteController : Controller
    {
        [AcceptVerbs("GET", "HEAD", "POST", "PUT", "DELETE")]
        [ValidateInput(false)]
        public ActionResult Index()
        {
            ViewBag.PageId = this.PortalSettings.ActivePage.PageID;
            return View();
        }
        public ActionResult RenderView(int pid)
        {
            var portalSettings = Appleseed.Framework.Site.Configuration.PortalSettings.GetPortalSettings(pid, Config.DefaultPortal);
            ViewBag.ThemeCSS = portalSettings.GetCurrentTheme().CssFile;
            ViewBag.PageId = pid;
            return View();
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
        public JsonResult SendEmail(List<string> data, string pageid)
        {
            var message = string.Empty;

            foreach (var item in data)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    var pid = Convert.ToInt32(pageid);
                    var portalSettings = Appleseed.Framework.Site.Configuration.PortalSettings.GetPortalSettings(pid, Config.DefaultPortal);
                    var email = item.Split('#')[1];
                    string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/');
                    string tokenString = GetToken(pid);
                    string url = baseUrl + Appleseed.Framework.HttpUrlBuilder.BuildUrl(pid) + "?tkn=" + tokenString;
                    var mail = new MailMessage();

                    // we check the PortalSettings in order to get if it has an sender registered 
                    if (this.PortalSettings.CustomSettings["SITESETTINGS_ON_REGISTER_SEND_FROM"] != null)
                    {
                        var sf = this.PortalSettings.CustomSettings["SITESETTINGS_ON_REGISTER_SEND_FROM"];
                        var mailFrom = sf.ToString();
                        try
                        {
                            mail.From = new MailAddress(mailFrom);
                        }
                        catch
                        {
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

                    mail.To.Add(new MailAddress(email));
                    mail.Subject = "Invitation";
                    string emailHtml = string.Format("Signup : {0}", url);
                    mail.Body = emailHtml;
                    mail.IsBodyHtml = false;
                    using (var client = new SmtpClient())
                    {
                        try
                        {
                            client.Send(mail);
                            message = "Invitation has been sent successfully.";
                        }
                        catch (Exception exception)
                        {
                            message = string.Format("We can't send invites to {0}. There were problems while trying to do so.", email);
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
            }

            return Json(new { ok = true, Message = message });
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
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