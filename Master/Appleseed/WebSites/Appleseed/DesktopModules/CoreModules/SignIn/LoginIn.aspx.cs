using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Appleseed.Framework.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.UI.WebControls;


namespace Appleseed.DesktopModules.CoreModules.SignIn {


    public partial class LoginIn : System.Web.UI.Page {

        private string StringsPortalSettings = "PortalSettings";

        protected void Page_Load(object sender, EventArgs e) {

            var userName = Session["UserName"] as string;
            LogIn(userName);

        }

        public string GeneratePasswordHash(string thisPassword) {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] tmpSource;
            byte[] tmpHash;

            tmpSource = ASCIIEncoding.ASCII.GetBytes(thisPassword); // Turn password into byte array
            tmpHash = md5.ComputeHash(tmpSource);

            StringBuilder sOutput = new StringBuilder(tmpHash.Length);
            for (int i = 0; i < tmpHash.Length; i++) {
                sOutput.Append(tmpHash[i].ToString("X2"));  // X2 formats to hexadecimal
            }
            return sOutput.ToString();
        }

        private void LogIn(string userName) {
            var PortalSettings = (PortalSettings)HttpContext.Current.Items[StringsPortalSettings];
            var user = Membership.GetUser(userName);
            FormsAuthentication.SetAuthCookie(user.ToString(), false);
            var hck = HttpContext.Current.Response.Cookies["Appleseed_" + PortalSettings.PortalAlias.ToLower()];
            if (hck != null) {
                hck.Value = user.ToString(); // Fill all data: name + email + id
                hck.Path = "/";

                var minuteAdd = Config.CookieExpire;
                var time = DateTime.Now;
                var span = new TimeSpan(0, 0, minuteAdd, 0, 0);

                hck.Expires = time.Add(span);

                // 					}

            }


            // Redirect browser back to originating page
            Response.Redirect("~/");

        }


    }
}