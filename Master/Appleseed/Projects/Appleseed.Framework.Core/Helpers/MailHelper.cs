// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MailHelper.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   This class contains functions for mailing to
//   Appleseed users
//   Some changes by Rob Siera 4 dec 2004
//   modified again by Bill Forney 4 dec 2004
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Helpers
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using System.Security.Principal;
    using System.Web;

    using Appleseed.Framework.Data;
    using Appleseed.Framework.DataTypes;
    using Appleseed.Framework.Exceptions;
    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Site.Configuration;

    /// <summary>
    /// This class contains functions for mailing to 
    ///   Appleseed users
    ///   Some changes by Rob Siera 4 dec 2004
    ///   modified again by Bill Forney 4 dec 2004
    /// </summary>
    public class MailHelper
    {
        #region Public Methods

        /// <summary>
        /// It writes email address only, if JavaScript is enabled (needs a really user-agent),
        ///   if not, an address like meyert[at]geschichte.hu-berlin.de is returned.
        /// </summary>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <returns>
        /// The formatted email.
        /// </returns>
        public static string FormatEmail(string email)
        {
            var user = email.Substring(0, email.IndexOf('@'));
            var dom = email.Substring(email.IndexOf('@') + 1);
            return
                string.Format(
                    "<script language=\"javascript\">var name = \"{0}\"; var domain = \"{1}\"; document.write('<a href=\"mailto:' + name + String.fromCharCode(64) + domain + '\">' + name + String.fromCharCode(64) + domain + '</a>')</script><noscript>{2} at {3}</noscript>", 
                    user, 
                    dom, 
                    user, 
                    dom);
        }

        /// <summary>
        /// This function return's the email address of the current logged on user.
        ///   If its email-address is not valid or not found,
        ///   then an empty string is returned.
        /// </summary>
        /// <returns>
        /// Email address of current user.
        /// </returns>
        public static string GetCurrentUserEmailAddress()
        {
            return GetCurrentUserEmailAddress(string.Empty);
        }

        /// <summary>
        /// Gets the current user email address.
        /// </summary>
        /// <param name="validated">
        /// if set to <c>true</c> [validated].
        /// </param>
        /// <returns>
        /// A string value...
        /// </returns>
        public static string GetCurrentUserEmailAddress(bool validated)
        {
            return GetCurrentUserEmailAddress(string.Empty, validated);
        }

        /// <summary>
        /// This function return's the email address of the current logged on user.
        ///   If its email-address is not valid or not found,
        ///   then the Default address is returned
        /// </summary>
        /// <param name="defaultEmail">
        /// The default email.
        /// </param>
        /// <param name="validated">
        /// if set to <c>true</c> [validated].
        /// </param>
        /// <returns>
        /// Current user's email address.
        /// </returns>
        public static string GetCurrentUserEmailAddress(string defaultEmail, bool validated = true)
        {
            if (HttpContext.Current.User is WindowsPrincipal)
            {
                // windows user
                var eal = ADHelper.GetEmailAddresses(HttpContext.Current.User.Identity.Name);

                return eal.Count == 0 ? defaultEmail : (string)eal[0];
            }
            else
            {
                // Get the logged on email address from the context
                // string email = System.Web.HttpContext.Current.User.Identity.Name;
                var email = PortalSettings.CurrentUser.Identity.Email;

                if (!validated)
                {
                    return email;
                }

                // Check if its email address is valid
                var eal = new EmailAddressList();

                try
                {
                    eal.Add(email);
                    return email;
                }
                catch
                {
                    return defaultEmail;
                }
            }
        }

        /// <summary>
        /// Gets the email addresses in roles.
        /// </summary>
        /// <param name="roles">
        /// The roles.
        /// </param>
        /// <param name="portalId">
        /// The portal ID.
        /// </param>
        /// <returns>
        /// A string[] value...
        /// </returns>
        public static string[] GetEmailAddressesInRoles(string[] roles, int portalId)
        {
            if (Config.UseSingleUserBase)
            {
                portalId = 0;
            }

            if (HttpContext.Current.User is WindowsPrincipal)
            {
                var addresses = new List<string>();

                foreach (var t in
                    roles.Select(ADHelper.GetEmailAddresses).SelectMany(
                        ea => ea.Cast<string>().Where(t => !addresses.Contains(t))))
                {
                    addresses.Add(t);
                }

                return addresses.ToArray();
            }

            // No roles --> no email addresses
            if (roles.Length == 0)
            {
                return new string[0];
            }

            // Build the SQL select
            var adaptedRoles = new string[roles.Length];

            for (var i = 0; i < roles.Length; i++)
            {
                adaptedRoles[i] = roles[i].Replace("'", "''");
            }

            var delimitedRoleList = string.Format("N'{0}'", string.Join("', N'", adaptedRoles));
            var sql =
                string.Format(
                    "SELECT DISTINCT rb_Users.Email FROM rb_UserRoles INNER JOIN  rb_Users ON rb_UserRoles.UserID = rb_Users.UserID INNER JOIN  rb_Roles ON rb_UserRoles.RoleID = rb_Roles.RoleID WHERE (rb_Users.PortalID = {0})  AND (rb_Roles.RoleName IN ({1}))", 
                    portalId, 
                    delimitedRoleList);

            // Execute the SQL
            var eal = new EmailAddressList();
            var reader = DBHelper.GetDataReader(sql);

            try
            {
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        try
                        {
                            var email = reader.GetString(0);

                            if (email.Trim().Length != 0)
                            {
                                eal.Add(email);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
            finally
            {
                reader.Close();
            }

            // Return the result
            return (string[])eal.ToArray(typeof(string));
        }

        /// <summary>
        /// Sends an email to specified address.
        /// </summary>
        /// <param name="from">
        /// Email address from
        /// </param>
        /// <param name="sendTo">
        /// Email address to
        /// </param>
        /// <param name="subject">
        /// Email subject line
        /// </param>
        /// <param name="body">
        /// Email body content
        /// </param>
        /// <param name="cc">
        /// Email carbon copy to
        /// </param>
        /// <param name="bcc">
        /// Email blind carbon copy to
        /// </param>
        /// <param name="smtpServer">
        /// SMTP Server to send mail thru (optional, if not specified local machine is used)
        /// </param>
        public static void SendEMail(
            string from, string sendTo, string subject, string body, string cc, string bcc, string smtpServer)
        {
            List<string> attachmentFiles = null;
            SendEMail(from, sendTo, subject, body, attachmentFiles, cc, bcc, smtpServer);
        }

        /// <summary>
        /// Sends an email to specified address.
        /// </summary>
        /// <param name="from">
        /// Email address from
        /// </param>
        /// <param name="sendTo">
        /// Email address to
        /// </param>
        /// <param name="subject">
        /// Email subject line
        /// </param>
        /// <param name="body">
        /// Email body content
        /// </param>
        /// <param name="attachmentFile">
        /// Optional attachment file name
        /// </param>
        /// <param name="cc">
        /// Email carbon copy to
        /// </param>
        /// <param name="bcc">
        /// Email blind carbon copy to
        /// </param>
        /// <param name="smtpServer">
        /// SMTP Server to send mail thru (optional, if not specified local machine is used)
        /// </param>
        public static void SendEMail(
            string from, 
            string sendTo, 
            string subject, 
            string body, 
            string attachmentFile, 
            string cc, 
            string bcc, 
            string smtpServer)
        {
            var attachmentFiles = new List<string>();

            if (!string.IsNullOrEmpty(attachmentFile))
            {
                attachmentFiles.Add(attachmentFile);
            }
            else
            {
                attachmentFiles = null;
            }

            SendEMail(from, sendTo, subject, body, attachmentFiles, cc, bcc, smtpServer);
        }

        /// <summary>
        /// Sends an email to specified address.
        /// </summary>
        /// <param name="from">
        /// Email address from
        /// </param>
        /// <param name="sendTo">
        /// Email address to
        /// </param>
        /// <param name="subject">
        /// Email subject line
        /// </param>
        /// <param name="body">
        /// Email body content
        /// </param>
        /// <param name="attachmentFiles">
        /// Optional, list of attachment file names in form of an array list
        /// </param>
        /// <param name="cc">
        /// Email carbon copy to
        /// </param>
        /// <param name="bcc">
        /// Email blind carbon copy to
        /// </param>
        /// <param name="smtpServer">
        /// SMTP Server to send mail thru (optional, if not specified local machine is used)
        /// </param>
        /// <param name="bodyIsHtml">
        /// Optional, mail format (text/html) true if is html, false if text
        /// </param>
        public static void SendEMail(
            string from, 
            string sendTo, 
            string subject, 
            string body, 
            List<string> attachmentFiles, 
            string cc, 
            string bcc, 
            string smtpServer, 
            bool bodyIsHtml = false)
        {
            var mailMessage = new MailMessage();
            mailMessage.To.Add(sendTo);
            mailMessage.From = new MailAddress(from);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = bodyIsHtml;

            if (cc.Length != 0)
            {
                mailMessage.CC.Add(cc);
            }

            if (bcc.Length != 0)
            {
                mailMessage.Bcc.Add(bcc);
            }

            if (attachmentFiles != null)
            {
                foreach (var x in attachmentFiles.Where(File.Exists))
                {
                    mailMessage.Attachments.Add(new Attachment(x));
                }
            }

            if (smtpServer.Length == 0)
            {
                throw new AppleseedException("SMTPServer configuration error");
            }

            var smtp = new SmtpClient(smtpServer);
            smtp.SendCompleted += SmtpSendCompleted;
            smtp.SendAsync(mailMessage, null);
        }

        /// <summary>
        /// Sends an email to specified address.
        /// </summary>
        /// <param name="from">
        /// Email address from
        /// </param>
        /// <param name="sendTo">
        /// Email address to
        /// </param>
        /// <param name="subject">
        /// Email subject line
        /// </param>
        /// <param name="body">
        /// Email body content
        /// </param>
        /// <param name="attachmentFiles">
        /// Optional, list of attachment file names in form of an array list
        /// </param>
        /// <param name="cc">
        /// Email carbon copy to
        /// </param>
        /// <param name="bcc">
        /// Email blind carbon copy to
        /// </param>
        /// <param name="smtpServer">
        /// SMTP Server to send mail thru (optional, if not specified local machine is used)
        /// </param>
        public static void SendMailMultipleAttachments(
            string from, 
            string sendTo, 
            string subject, 
            string body, 
            List<string> attachmentFiles, 
            string cc, 
            string bcc, 
            string smtpServer)
        {
            SendEMail(from, sendTo, subject, body, attachmentFiles, cc, bcc, smtpServer);
        }

        /// <summary>
        /// Sends an email to specified address.
        /// </summary>
        /// <param name="from">
        /// Email address from
        /// </param>
        /// <param name="sendTo">
        /// Email address to
        /// </param>
        /// <param name="subject">
        /// Email subject line
        /// </param>
        /// <param name="body">
        /// Email body content
        /// </param>
        /// <param name="cc">
        /// Email carbon copy to
        /// </param>
        /// <param name="bcc">
        /// Email blind carbon copy to
        /// </param>
        /// <param name="smtpServer">
        /// SMTP Server to send mail thru (optional, if not specified local machine is used)
        /// </param>
        public static void SendMailNoAttachment(
            string from, string sendTo, string subject, string body, string cc, string bcc, string smtpServer)
        {
            SendEMail(from, sendTo, subject, body, cc, bcc, smtpServer);
        }

        /// <summary>
        /// Sends an email to specified address.
        /// </summary>
        /// <param name="from">
        /// Email address from
        /// </param>
        /// <param name="sendTo">
        /// Email address to
        /// </param>
        /// <param name="subject">
        /// Email subject line
        /// </param>
        /// <param name="body">
        /// Email body content
        /// </param>
        /// <param name="attachmentFile">
        /// Optional attachment file name
        /// </param>
        /// <param name="cc">
        /// Email carbon copy to
        /// </param>
        /// <param name="bcc">
        /// Email blind carbon copy to
        /// </param>
        /// <param name="smtpServer">
        /// SMTP Server to send mail thru (optional, if not specified local machine is used)
        /// </param>
        public static void SendMailOneAttachment(
            string from, 
            string sendTo, 
            string subject, 
            string body, 
            string attachmentFile, 
            string cc, 
            string bcc, 
            string smtpServer)
        {
            SendEMail(from, sendTo, subject, body, attachmentFile, cc, bcc, smtpServer);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the SendCompleted event of the SMTP control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.ComponentModel.AsyncCompletedEventArgs"/> instance containing the event data.
        /// </param>
        private static void SmtpSendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ErrorHandler.Publish(LogLevel.Error, e.Error);
            }
        }

        #endregion
    }
}