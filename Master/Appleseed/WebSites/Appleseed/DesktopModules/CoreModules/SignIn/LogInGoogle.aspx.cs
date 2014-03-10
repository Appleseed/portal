using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Web.Security;
using System.Security.Cryptography;
using Appleseed.Framework.Security;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.Extensions.UI;


namespace Appleseed.DesktopModules.CoreModules.SignIn {
    public partial class LogInGoogle : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

             // Metodo viejo que anda

             //if (Session["GoogleAppId"] != null && Session["GoogleAppSecret"] != null) {
             //try {
             //   string code = Request.QueryString["code"];
             //   string url = "https://accounts.google.com/o/oauth2/token";
             //   string appId = Session["GoogleAppId"] as string;
             //   string appSecret = Session["GoogleAppSecret"] as string;
             //   string redirectUrl = ConvertRelativeUrlToAbsoluteUrl(HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/SignIn/LogInGoogle.aspx"));
            
             //   StringBuilder postData = new StringBuilder();

             //   postData.Append("code=" + code + "&");
             //   postData.Append("client_id=" + appId + "&");
             //   postData.Append("client_secret=" + appSecret + "&");
             //   postData.Append("redirect_uri=" + redirectUrl + "&");
             //   postData.Append("grant_type=authorization_code");


             //   //ETC for all Form Elements

             //   // Now to Send Data.
             //   StreamWriter writer = null;

             //   HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
             //   request.Method = "POST";
             //   request.ContentType = "application/x-www-form-urlencoded";
             //   request.ContentLength = postData.ToString().Length;
             //   try {
             //       writer = new StreamWriter(request.GetRequestStream());
             //       writer.Write(postData.ToString());
             //   }
             //   finally {
             //       if (writer != null)
             //           writer.Close();
             //   }

             //   WebResponse response = request.GetResponse();
             //   StreamReader sr = new StreamReader(response.GetResponseStream());
             //   String json = sr.ReadToEnd().Trim(); 
                

             //   JavaScriptSerializer ser = new JavaScriptSerializer();
             //   var profileDic = ser.Deserialize<Dictionary<string, object>>(json);

             //   string access_token = (string)profileDic["access_token"];
             //   var refresh_token = profileDic["refresh_token"];

             //   int statusCode = -1;
             //   String strUrl = "https://www.google.com/m8/feeds/contacts/default/full?access_token=" + access_token;
             //   request = WebRequest.Create(strUrl) as HttpWebRequest;

                
             //       using (HttpWebResponse res = (HttpWebResponse)request.GetResponse() as HttpWebResponse) {
             //           statusCode = (int)res.StatusCode;
             //           using (Stream dataStream = res.GetResponseStream()) {
             //               var resp = XDocument.Load(dataStream);                            
             //               XNamespace atom = "http://www.w3.org/2005/Atom";
             //               var q = from c in resp.Root.Descendants(atom + "author")
             //                       select new {
             //                           Name = (string)c.Element(atom + "name"),
             //                           Email = (string)c.Element(atom + "email")
             //                       };
             //               string name = string.Empty;
             //               string email = string.Empty;
             //               foreach (var person in q) {
             //                   name = person.Name;
             //                   email = person.Email;
             //               }

             //               if (Membership.GetUser(email) == null){

             //                   Session["GoogleUserName"] = name;
             //                   Session["GoogleUserEmail"] = email;

             //                   Session["CameFromSocialNetwork"] = true;
             //                   Session["CameFromGoogleLogin"] = true;
             //                   Response.Redirect(HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Register/Register.aspx"));
             //               }
             //               else{
             //                   string urlHome = ConvertRelativeUrlToAbsoluteUrl("~");
             //                   Session["CameFromSocialNetwork"] = true;
             //                   PortalSecurity.SignOn(email, GeneratePasswordHash(email), false, urlHome);
             //               }

             //           }
             //       }
             //   }
             //   catch (Exception ex) {
             //       ErrorHandler.Publish(LogLevel.Error, "Google error ocurred", ex);
             //       Response.Redirect(HttpUrlBuilder.BuildUrl(""));
             //   }

                
             //}
             //else{
             //   ErrorHandler.Publish(LogLevel.Error,"Google settings weren't loaded.");
             //   Response.Redirect(HttpUrlBuilder.BuildUrl(""));
             //}

            OpenIdRelyingParty Openid = new OpenIdRelyingParty();
            var openIdUrl = "http://www.google.com/accounts/o8/id";
            var response = Openid.GetResponse();
            if (response == null)
    	    {
              // User submitting Identifier
    	        Identifier id;
                if (Identifier.TryParse(openIdUrl, out id))
    	        {
    	            try
    	            {
    	                var request = Openid.CreateRequest(openIdUrl);
    	                var fetch = new FetchRequest();
    	                fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
    	                fetch.Attributes.AddRequired(WellKnownAttributes.Name.First);
    	                fetch.Attributes.AddRequired(WellKnownAttributes.Name.Last);
    	                request.AddExtension(fetch);
                        request.AddExtension(new UIRequest() {
                            Mode = UIModes.Popup
                        });
                        var b = new UriBuilder(Request.Url) { Query = "" };
                        request.RedirectToProvider();
    	            }
    	            catch (ProtocolException ex)
    	            {
                        ErrorHandler.Publish(LogLevel.Error,"Google Error", ex);
                        Response.Redirect(HttpUrlBuilder.BuildUrl("~/"));
    	            }
    	        }
    	        ErrorHandler.Publish(LogLevel.Error,"OpenID Error...invalid url. url='" + openIdUrl + "'");
                Response.Redirect(HttpUrlBuilder.BuildUrl("~/"));
    	    }
    	 
    	    // OpenID Provider sending assertion response
    	    switch (response.Status)
    	    {
    	        case AuthenticationStatus.Authenticated:
    	            var fetch = response.GetExtension<FetchResponse>();
    	            string firstName = "unknown";
    	            string lastName = "unknown";
    	            string email = "unknown";
    	            if(fetch!=null)
    	            {
    	                firstName = fetch.GetAttributeValue(WellKnownAttributes.Name.First);
    	                lastName = fetch.GetAttributeValue(WellKnownAttributes.Name.Last);
    	                email = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email);
    	            }
                    if (Membership.GetUser(email) == null) {

                        Session["GoogleUserFirstName"] = firstName;
                        Session["GoogleUserLastName"] = lastName;
                        Session["GoogleUserEmail"] = email;

                        Session["CameFromSocialNetwork"] = true;
                        Session["CameFromGoogleLogin"] = true;
                        var redirect = ConvertRelativeUrlToAbsoluteUrl(HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Register/Register.aspx"));
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
                    else {
                        
                        Session["CameFromSocialNetwork"] = true;
                        Session["UserName"] = email;
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
                    break;
    	            
    	        case AuthenticationStatus.Canceled:
    	            ErrorHandler.Publish(LogLevel.Info,"OpenID: Cancelled at provider.");
                    Response.Write ("<script language=javascript>close();</script>");
                    Response.End();
                    Response.Redirect(HttpUrlBuilder.BuildUrl("~/"));
                    break;
    	        case AuthenticationStatus.Failed:
                
                    ErrorHandler.Publish(LogLevel.Error, "OpenID Exception...",response.Exception);
    	            Response.Write ("<script language=javascript>close();</script>");
                    Response.End();
                    Response.Redirect(HttpUrlBuilder.BuildUrl("~/"));
                    break;
    	    }
            Response.Redirect(HttpUrlBuilder.BuildUrl("~/"));



    	}
            

        


        public string ConvertRelativeUrlToAbsoluteUrl(string relativeUrl) {

            if (Request.IsSecureConnection)

                return string.Format("https://{0}{1}", Request.Url.Host, Page.ResolveUrl(relativeUrl));

            else

                return string.Format("http://{0}{1}", Request.Url.Host, Page.ResolveUrl(relativeUrl));

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

        
    
    }
}