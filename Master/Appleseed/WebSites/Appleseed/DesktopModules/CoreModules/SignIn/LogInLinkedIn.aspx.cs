using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;
using Appleseed.Framework;
using System.Text;

using System.Xml.Serialization;
using System.IO;
using System.Xml;

using System.Web.Security;
using System.Security.Cryptography;


//using System.Xml.Serialization;

namespace Appleseed.DesktopModules.CoreModules.SignIn
{
    public partial class LogInLinkedIn : System.Web.UI.Page
    {
        static string token_secret = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["oauth_problem"] != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<script language='javascript'>");
                sb.Append("window.close();");
                sb.Append("</script>");
                Response.Write(sb.ToString());
                Response.End();
            }
            else if(Request["oauth_token"] == null)
            {
                
                var credentials = new OAuthCredentials
                {
                    CallbackUrl = ConvertRelativeUrlToAbsoluteUrl("~/DesktopModules/CoreModules/SignIn/LoginLinkedIn.aspx"),
                    ConsumerKey = Session["LinkedInAppId"] as string,
                    ConsumerSecret = Session["LinkedInAppSecret"] as string,
                    Verifier = "123456",
                    Type = OAuthType.RequestToken
                };

                var client = new RestClient { Authority = "https://api.linkedin.com/uas/oauth", Credentials = credentials };
                var request = new RestRequest { Path = "requestToken" };
                RestResponse response = client.Request(request);

                token = response.Content.Split('&').Where(s => s.StartsWith("oauth_token=")).Single().Split('=')[1];
                token_secret = response.Content.Split('&').Where(s => s.StartsWith("oauth_token_secret=")).Single().Split('=')[1];
                Response.Redirect("https://api.linkedin.com/uas/oauth/authorize?oauth_token=" + token);
            }
            else if (Request["oauth_verifier"] != null)
            {
                token = Request["oauth_token"];
                verifier = Request["oauth_verifier"];
                
                var credentials = new OAuthCredentials
                {
                    ConsumerKey = Session["LinkedInAppId"] as string,
                    ConsumerSecret = Session["LinkedInAppSecret"] as string,
                    Token = token,
                    TokenSecret = token_secret,
                    Verifier = verifier,
                    Type = OAuthType.AccessToken,
                    ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                    SignatureMethod = OAuthSignatureMethod.HmacSha1,
                    Version = "1.0"
                };

                var client = new RestClient { Authority = "https://api.linkedin.com/uas/oauth", Credentials = credentials, Method = WebMethod.Post };
                var request = new RestRequest { Path = "accessToken" };
                RestResponse response = client.Request(request);
                string content = response.Content;

                string accessToken = response.Content.Split('&').Where(s => s.StartsWith("oauth_token=")).Single().Split('=')[1];
                string accessTokenSecret = response.Content.Split('&').Where(s => s.StartsWith("oauth_token_secret=")).Single().Split('=')[1];
                var people = new LinkedInService(accessToken, accessTokenSecret).GetCurrentUser();
                var userName = "LinkedIn_" + people.id;
                string password = GeneratePasswordHash(userName);

                Session["CameFromSocialNetwork"] = true;
                
                StringBuilder sb = new StringBuilder();
                if (Membership.GetUser(userName) == null)
                {
                    //The user doesnt exists, needs to be registered
                    Session["LinkedInUserName"] = userName;
                    string urlRegister = ConvertRelativeUrlToAbsoluteUrl("~/DesktopModules/CoreModules/Register/Register.aspx");

                    sb.Append("<script language='javascript'>");
                    sb.Append("window.opener.location.href = '");
                    sb.Append(urlRegister);
                    sb.Append("';window.opener.focus();");
                    sb.Append("window.close();");
                    sb.Append("</script>");
                    Response.Write(sb.ToString());
                    Response.End();

                }
                else
                {
                    Session["CameFromSocialNetwork"] = true;
                    Session["UserName"] = userName;
                    var redirect = ConvertRelativeUrlToAbsoluteUrl(HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/SignIn/LoginIn.aspx"));
                    
                    sb.Append("<script language='javascript'>");
                    sb.Append("window.opener.location.href = '");
                    sb.Append(redirect);
                    sb.Append("';window.opener.focus();");
                    sb.Append("window.close();");
                    sb.Append("</script>");
                    Response.Write(sb.ToString());
                    Response.End();
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
            for (int i = 0; i < tmpHash.Length; i++)
            {
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

        string token = "";
        string verifier = "";
        
    }
    public partial class LinkedInService : System.Web.UI.Page
    {
        private const string URL_BASE = "http://api.linkedin.com/v1";
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }

        public LinkedInService(string accessToken, string accessTokenSecret)
        {
            this.AccessToken = accessToken;
            this.AccessTokenSecret = accessTokenSecret;
        }

        private OAuthCredentials AccessCredentials
        {
            get
            {
                return new OAuthCredentials
                {
                    Type = OAuthType.AccessToken,
                    SignatureMethod = OAuthSignatureMethod.HmacSha1,
                    ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                    ConsumerKey = Session["LinkedInAppId"] as string,
                    ConsumerSecret = Session["LinkedInAppSecret"] as string,
                    Token = AccessToken,
                    TokenSecret = AccessTokenSecret
                };
            }
        }

        private RestResponse GetResponse(string path)
        {
            var client = new RestClient()
            {
                Authority = URL_BASE,
                Credentials = AccessCredentials,
                Method = WebMethod.Get
            };

            var request = new RestRequest { Path = path };

            return client.Request(request);
        }

        public MvcLinkedInApplication.Person GetCurrentUser()
        {
            var response = GetResponse("people/~:(id,first-name,last-name) ");
            MemoryStream memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(response.Content));
            XmlSerializer deserializer = new XmlSerializer(typeof(MvcLinkedInApplication.Person));
            return (MvcLinkedInApplication.Person)deserializer.Deserialize(new StringReader(response.Content));
        }
    }
    public partial class MvcLinkedInApplication : System.Web.UI.Page
    {
        [Serializable, XmlRoot("person")]
        public class Person
        {
            [XmlElement("id")]
            public string id { get; set; }

            [XmlElement("first-name")]
            public string FirstName { get; set; }

            [XmlElement("last-name")]
            public string LastName { get; set; }

            [XmlElement("headline")]
            public string headLine { get; set; }

            [XmlElement("url")]
            public string ProfileUrl { get; set; }
        }
    }

}