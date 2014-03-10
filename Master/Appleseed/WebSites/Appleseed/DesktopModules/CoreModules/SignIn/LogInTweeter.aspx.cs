using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;
using Twitterizer;
using System.Web.Security;


using System.Text;
using System.Text.RegularExpressions;  // This is for password validation
using System.Security.Cryptography;
using Appleseed.Framework.Security;
using Appleseed.Framework;
using System.Web.Profile;
using Appleseed.Framework.Settings;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework.Web.UI.WebControls;
using Appleseed.Framework.Web.UI;


namespace Appleseed.DesktopModules.CoreModules.SignIn
{
    public partial class LogInTweeter : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated) {
                try {

                    if (Session["TwitterAppId"] != null && Session["TwitterAppSecret"] != null) {

                        string consumerKey = Session["TwitterAppId"] as string;
                        string consumerSecret = Session["TwitterAppSecret"] as string;

                        OAuthTokenResponse accessTokenResponse = OAuthUtility.GetAccessToken(consumerKey, consumerSecret,
                                                                                                Request.QueryString["oauth_token"],
                                                                                                Request.QueryString["oauth_verifier"]);

                        Session["CameFromSocialNetwork"] = true;

                        string userName = "Twitter_" + accessTokenResponse.ScreenName;
                        string password = GeneratePasswordHash(userName);

                        if (Membership.GetUser(userName) == null) {
                            //The user doesnt exists, needs to be registered

                            Session["TwitterUserName"] = userName;                            
                            string urlRegister = ConvertRelativeUrlToAbsoluteUrl("~/DesktopModules/CoreModules/Register/Register.aspx");
                            
                            StringBuilder sb = new StringBuilder();
                            sb.Append("<script language='javascript'>");
                            sb.Append("window.opener.location.href = '");
                            sb.Append(urlRegister);
                            sb.Append("';window.opener.focus();");
                            sb.Append("window.close();");
                            sb.Append("</script>");
                            Response.Write(sb.ToString());
                            Response.End();


                        } else {

                            Session["CameFromSocialNetwork"] = true;
                            Session["UserName"] = userName;
                            var redirect = ConvertRelativeUrlToAbsoluteUrl(HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/SignIn/LoginIn.aspx"));
                            StringBuilder sb = new StringBuilder();
                            sb.Append("<script language='javascript'>");
                            sb.Append("window.opener.location.href = '");
                            sb.Append(redirect);
                            sb.Append("';window.opener.focus();");
                            sb.Append("window.close();");
                            sb.Append("</script>");
                            Response.Write(sb.ToString());
                            Response.End();
                            
                        }
                    } else {
                        ErrorHandler.Publish(LogLevel.Error, "TwitterSettings are not correct");
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<script language='javascript'>");                       
                        sb.Append("window.close();");
                        sb.Append("</script>");
                        Response.Write(sb.ToString());
                        Response.End();
                    }

                } catch (TwitterizerException ex) {

                    ErrorHandler.Publish(LogLevel.Error, ex);
                    string _redirectUrl = Config.SmartErrorRedirect;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script language='javascript'>");
                    sb.Append("window.close();");
                    sb.Append("</script>");
                    Response.Write(sb.ToString());
                    Response.End();
                    Response.Redirect(_redirectUrl);
                }
            }
        }

        public string GeneratePasswordHash(string thisPassword)
        {
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

        public string ConvertRelativeUrlToAbsoluteUrl(string relativeUrl)
        {

            if (Request.IsSecureConnection)

                return string.Format("https://{0}{1}", Request.Url.Host, Page.ResolveUrl(relativeUrl));

            else

                return string.Format("http://{0}{1}", Request.Url.Host, Page.ResolveUrl(relativeUrl));

        }
    }
}