using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mail;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Helpers;
using Appleseed.Framework.Settings;
using Appleseed.Framework;
using Appleseed.Framework.Web.UI.WebControls;
using Appleseed.Framework.Data;
using Appleseed.Framework.Massive;
using System.Collections.Generic;

namespace Appleseed.Framework.Content.Data
{
    /// <summary>
    /// Class that encapsulates all data logic
    /// necessary to send newsletters.
    /// </summary>
    public class NewsletterDB
    {
        /// <summary>
        /// Get only the records with SendNewsletter enabled from the "Users" database table.
        /// </summary>
        /// <param name="portalID">The portal ID.</param>
        /// <returns></returns>
        public IList<dynamic> GetUsersNewsletter(int portalID) 
        {

            var model = new DynamicModel(connectionStringName: "ConnectionString");
            var users = model.Fetch(@"
                SELECT mem.UserId, prof.Name, mem.Email
                FROM aspnet_Membership mem INNER JOIN 
                     aspnet_CustomProfile prof ON mem.UserId = prof.UserId INNER JOIN
                     aspnet_Applications app ON mem.ApplicationId = app.ApplicationId INNER JOIN
                     rb_Portals por ON LOWER(por.PortalAlias) = app.LoweredApplicationName
                WHERE 
                     por.PortalID = @0 AND prof.SendNewsletter = 1
                     AND (NOT (mem.Email IN (SELECT Email FROM rb_BlackList WHERE PortalID = @0)))", portalID);
     
            // Return the list
            return users;
        }

        

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="From">From.</param>
        /// <param name="To">To.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Subject">The subject.</param>
        /// <param name="Body">The body.</param>
        /// <param name="Send">if set to <c>true</c> [send].</param>
        /// <param name="HtmlMode">if set to <c>true</c> [HTML mode].</param>
        /// <param name="breakLines">if set to <c>true</c> [break lines].</param>
        /// <returns></returns>
        public string SendMessage(string From, string To, string Name, string Subject, string Body, bool Send, bool HtmlMode, bool breakLines)
        {
			// Obtain PortalSettings from Current Context
			PortalSettings portalSettings = (PortalSettings) HttpContext.Current.Items["PortalSettings"];

			System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
        	//MailFormat format;
            
            //Interprets TAGS
            //{NAME} = UserName
            //{EMAIL} = UserEmail
            Body = Body.Replace("{NAME}" , Name);
            if (HtmlMode)
            {
				mail.IsBodyHtml = true;
                //format = MailFormat.Html;
                Body = Body.Replace("{EMAIL}", "<A Href=\"mailto:" + To + "\">" + To + "</A>");
			
                //This option is useful is you type the text and you want to send as html.
				if (breakLines)
					Body = Body.Replace("\n", "<br>");
            }
            else
            {
				mail.IsBodyHtml = false;
                //format = MailFormat.Text;
                Body = Body.Replace("{EMAIL}", To);

                //Break rows - must be the last
                Body = ((HTMLText) Body).GetBreakedText(78);
            }

            // Send only if true
            if (Send)
            {
				mail.From = new System.Net.Mail.MailAddress(From);
				mail.To.Add(To);
                mail.Subject = Subject;
                mail.Body = Body;
                mail.Priority = System.Net.Mail.MailPriority.Low;
				System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(Config.SmtpServer);
				smtp.Send(mail);
            }
            return Body;
        }
    }
}
