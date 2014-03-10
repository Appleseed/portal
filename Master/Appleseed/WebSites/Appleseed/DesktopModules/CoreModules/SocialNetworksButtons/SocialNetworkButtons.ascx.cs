using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Appleseed.Framework.UI.WebControls;
using Appleseed.Framework;
using Facebook;
using Twitterizer;
using Facebook.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;

namespace Appleseed.DesktopModules.CoreModules.SocialNetworksButtons {
    public partial class SocialNetworkButtons : SignInControl

    {

        protected string TwitterLink;


        

        protected void Page_Load(object sender, EventArgs e)
        {
            bool hide = false;
            
            

            if (this.Request.IsAuthenticated) {
                this.Visible = false;
            } else {
                try {
                    hide = true;
                    var facebookContext = GetFacebookWebContext();
                    if (facebookContext != null) {
                        appId.Value = PortalSettings.CustomSettings["SITESETTINGS_FACEBOOK_APP_ID"].ToString();
                        appidfacebook.Value = PortalSettings.CustomSettings["SITESETTINGS_FACEBOOK_APP_ID"].ToString();
                        hide = false;
                        if (facebookContext.IsAuthenticated()) {
                            //Here is were i check if the user login via facebook
                            FacebookSignInMethod();
                        }
                        
                    } else {
                        //TODO: ocultar boton y mostrar warning
                        loginfb_div.Visible = false;
                        ErrorHandler.Publish(LogLevel.Error, "Facebook settings are not correct");
                    }
                } catch (FacebookApiException ex) {
                    loginfb_div.Visible = false;
                    ErrorHandler.Publish(LogLevel.Error, Resources.Appleseed.FACEBOOK_ERROR, ex);
                }


                try {
                    var TwitterRequestToken = GetTwitterRequestToken();
                    if (TwitterRequestToken != null) {
                        hide = false;
                    } else {
                        //TODO: ocultar boton y mostrar warning
                        logintwit_div.Visible = false;
                        ErrorHandler.Publish(LogLevel.Error, "Twitter settings are not correct");
                    }
                } catch (TwitterizerException ex) {
                    logintwit_div.Visible = false;
                    ErrorHandler.Publish(LogLevel.Error, Resources.Appleseed.TWITTER_ERROR, ex);
                }

                if(this.PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_GOOGLE_LOGIN") &&
                this.PortalSettings.CustomSettings["SITESETTINGS_GOOGLE_LOGIN"].ToString().Length != 0 &&
                !bool.Parse(this.PortalSettings.CustomSettings["SITESETTINGS_GOOGLE_LOGIN"].ToString())) {
                    google_div.Visible = false;
                }
                else
                    hide = false;

                if ((this.PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_LINKEDIN_APP_ID") &&
                this.PortalSettings.CustomSettings["SITESETTINGS_LINKEDIN_APP_ID"].ToString().Length != 0)&&
                    (this.PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_LINKEDIN_APP_SECRET") &&
                this.PortalSettings.CustomSettings["SITESETTINGS_LINKEDIN_APP_SECRET"].ToString().Length != 0))
                {
                    hide = false;
                    Session["LinkedInAppId"] = PortalSettings.CustomSettings["SITESETTINGS_LINKEDIN_APP_ID"].ToString();
                    Session["LinkedInAppSecret"] = PortalSettings.CustomSettings["SITESETTINGS_LINKEDIN_APP_SECRET"].ToString();
                }
                else
                    linkedin_div.Visible = false;
            }
        }

        protected string getTwitterLink() { 
            var TwitterRequestToken = GetTwitterRequestToken();
            if (TwitterRequestToken != null) {
                Uri authenticationUri = OAuthUtility.BuildAuthorizationUri(TwitterRequestToken.Token, true);
                return authenticationUri.AbsoluteUri;
            }
            return null;
        }

        protected string getGoogleLink() {
            return HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/SignIn/LogInGoogle.aspx");
        }

        protected string getLinkedInLink()
        {
            return HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/SignIn/LoginLinkedIn.aspx");
        }

        private void UpdateProfile()
        {
            var client = new FacebookWebClient();
            dynamic me = client.Get("me");

            ProfileManager.Provider.ApplicationName = PortalSettings.PortalAlias;
            ProfileBase profile = ProfileBase.Create(me.email);
            profile.SetPropertyValue("Email", me.email);
            profile.SetPropertyValue("Name", me.name);
            try {
                profile.Save();
            } catch (Exception exc) {
                ErrorHandler.Publish(LogLevel.Error, "Error al salvar un perfil", exc);
            }
        }

        #region Properties

        public override Guid GuidID
        {
            get
            {
                return new Guid("{C1B682A7-E046-4915-AAD0-EDA96759A03C}");
            }
        }

        #endregion

        #region Facebook Method

        /// <summary>
        /// check if facebook settings were setting up from the portal settings, if not update the facebooksettings section of the web config file
        /// </summary>
        internal FacebookWebContext GetFacebookWebContext()
        {
            try {
                if (PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_FACEBOOK_APP_ID") &&
                    !PortalSettings.CustomSettings["SITESETTINGS_FACEBOOK_APP_ID"].ToString().Equals(string.Empty) &&
                    PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_FACEBOOK_APP_SECRET") &&
                    !PortalSettings.CustomSettings["SITESETTINGS_FACEBOOK_APP_SECRET"].ToString().Equals(string.Empty)) {
                    string appId = PortalSettings.CustomSettings["SITESETTINGS_FACEBOOK_APP_ID"].ToString();
                    var appSecret = PortalSettings.CustomSettings["SITESETTINGS_FACEBOOK_APP_SECRET"].ToString();

                    if (FacebookWebContext.Current.Settings != null) {
                        var facebookConfigurationSection = new FacebookConfigurationSection();
                        facebookConfigurationSection.AppId = appId;
                        facebookConfigurationSection.AppSecret = appSecret;
                        return new FacebookWebContext(facebookConfigurationSection);
                    }
                }

                return null;
            } catch (Exception) {
                return null;
            }
        }

        private void FacebookSignInMethod()
        {
            var client = new FacebookWebClient();
            dynamic me = client.Get("me");
            Session["CameFromSocialNetwork"] = true;
            if (Membership.GetUser(me.email) == null) {
                
                Session["FacebookUserName"] = me.email;
                Session["FacebookName"] = me.name;

                
                string urlRegister = ConvertRelativeUrlToAbsoluteUrl(HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/Register/Register.aspx"));
                Response.Redirect(urlRegister);
            } else
                //PortalSecurity.SignOn(me.email, GeneratePasswordHash(me.email as String));
                Session["UserName"] = me.email;
                Response.Redirect(HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/SignIn/LoginIn.aspx"));
            if (this.Settings["SIGNIN_AUTOMATICALLYHIDE"] != null) {
                bool hide = bool.Parse(this.Settings["SIGNIN_AUTOMATICALLYHIDE"].ToString());
                this.Visible = false;
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

        public override void Logoff()
        {
            var context = GetFacebookWebContext();
            if (context != null && context.IsAuthenticated()) {
               context.DeleteAuthCookie();

            }
            

            Session.RemoveAll();
        }

        #endregion Facebook Methods

        #region Twitter Method

        internal OAuthTokenResponse GetTwitterRequestToken()
        {
            try {
                if (PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_TWITTER_APP_ID") &&
                    !PortalSettings.CustomSettings["SITESETTINGS_TWITTER_APP_ID"].ToString().Equals(string.Empty) &&
                    PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_TWITTER_APP_SECRET") &&
                    !PortalSettings.CustomSettings["SITESETTINGS_TWITTER_APP_SECRET"].ToString().Equals(string.Empty)) {
                    string appId = PortalSettings.CustomSettings["SITESETTINGS_TWITTER_APP_ID"].ToString();
                    var appSecret = PortalSettings.CustomSettings["SITESETTINGS_TWITTER_APP_SECRET"].ToString();
                    
                    string server = ConvertRelativeUrlToAbsoluteUrl(HttpUrlBuilder.BuildUrl("~/DesktopModules/CoreModules/SignIn/LogInTweeter.aspx"));                        //ConvertRelativeUrlToAbsoluteUrl("~/DesktopModules/CoreModules/SignIn/LogInTweeter.aspx");
                    Session["TwitterAppId"] = appId;
                    Session["TwitterAppSecret"] = appSecret;

                    OAuthTokenResponse requestToken = OAuthUtility.GetRequestToken(appId, appSecret, server);

                    return requestToken;

                }

                return null;
            } catch (Exception) {
                return null;
            }
        }

        public string ConvertRelativeUrlToAbsoluteUrl(string relativeUrl)
        {

            if (Request.IsSecureConnection)

                return string.Format("https://{0}{1}", Request.Url.Host, relativeUrl);

            else

                return string.Format("http://{0}{1}", Request.Url.Host, relativeUrl);

        }

        #endregion
    }
}