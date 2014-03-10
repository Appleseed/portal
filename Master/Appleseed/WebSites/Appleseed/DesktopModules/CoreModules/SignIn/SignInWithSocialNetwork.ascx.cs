using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Appleseed.Framework.UI.WebControls;
using Appleseed.Framework;
using Facebook.Web;
using Facebook;

namespace Appleseed.DesktopModules.CoreModules.SignIn {
    public partial class SignInWithSocialNetwork : SignInControl {

        protected string TwitterLink;


        public SignInWithSocialNetwork() {
            var hideAutomatically = new SettingItem<bool, CheckBox>() {
                Value = true,
                EnglishName = "Hide automatically",
                Order = 20
            };
            this.BaseSettings.Add("SIGNIN_AUTOMATICALLYHIDE", hideAutomatically);
        }

        protected void Page_Load(object sender, EventArgs e) {
            bool hide = false;

            if (this.BaseSettings.ContainsKey("SIGNIN_AUTOMATICALLYHIDE") && !this.BaseSettings["SIGNIN_AUTOMATICALLYHIDE"].ToString().Equals(string.Empty)) {
                //if (this.Settings["SIGNIN_AUTOMATICALLYHIDE"] != null) 
                hide = bool.Parse(this.BaseSettings["SIGNIN_AUTOMATICALLYHIDE"].ToString());
            }

            if (hide && this.Request.IsAuthenticated) {
                this.Visible = false;
            }
            else {


                bool ShowSocial = false;
                if (PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_TWITTER_APP_ID") &&
                    !PortalSettings.CustomSettings["SITESETTINGS_TWITTER_APP_ID"].ToString().Equals(string.Empty) &&
                    PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_TWITTER_APP_SECRET") &&
                    !PortalSettings.CustomSettings["SITESETTINGS_TWITTER_APP_SECRET"].ToString().Equals(string.Empty)
                   )
                    ShowSocial = true;
                if (PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_FACEBOOK_APP_ID") &&
                    !PortalSettings.CustomSettings["SITESETTINGS_FACEBOOK_APP_ID"].ToString().Equals(string.Empty) &&
                    PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_FACEBOOK_APP_SECRET") &&
                    !PortalSettings.CustomSettings["SITESETTINGS_FACEBOOK_APP_SECRET"].ToString().Equals(string.Empty))
                    ShowSocial = true;
                if (PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_GOOGLE_LOGIN") &&
                    !PortalSettings.CustomSettings["SITESETTINGS_GOOGLE_LOGIN"].ToString().Equals(string.Empty) &&
                    bool.Parse(PortalSettings.CustomSettings["SITESETTINGS_GOOGLE_LOGIN"].ToString()))
                    ShowSocial = true;
                if (!ShowSocial)
                    SocialButtons.Visible = false;
            }
        }

        public override void Logoff() {
            var context = GetFacebookWebContext();
            if (context != null && context.IsAuthenticated()) {
                context.DeleteAuthCookie();

            }


            Session.RemoveAll();
        }

        internal FacebookWebContext GetFacebookWebContext() {
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
            }
            catch (Exception) {
                return null;
            }
        }

        public override Guid GuidID {
            get { return new Guid("{4AEF959B-FEFE-49EB-B364-9B5995552ED8}"); }
        }
    }
}