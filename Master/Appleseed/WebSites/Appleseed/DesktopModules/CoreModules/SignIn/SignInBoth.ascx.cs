using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Appleseed.Framework.Site.Configuration;
using Appleseed.Framework;

namespace Appleseed.DesktopModules.CoreModules.SignIn
{
    public partial class SignInBoth : Appleseed.Framework.UI.WebControls.SignInControl
    {

        public SignInBoth()
        {
            var hideAutomatically = new SettingItem<bool, CheckBox>() {
                Value = true,
                EnglishName = "Hide automatically",
                Order = 20
            };
            this.BaseSettings.Add("SIGNIN_AUTOMATICALLYHIDE", hideAutomatically);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            bool hide = false;

            if (this.BaseSettings.ContainsKey("SIGNIN_AUTOMATICALLYHIDE") && !this.BaseSettings["SIGNIN_AUTOMATICALLYHIDE"].ToString().Equals(string.Empty)) {
                //if (this.Settings["SIGNIN_AUTOMATICALLYHIDE"] != null) 
                hide = bool.Parse(this.BaseSettings["SIGNIN_AUTOMATICALLYHIDE"].ToString());
            }

            if (hide && this.Request.IsAuthenticated) {
                this.Visible = false;
            } else {

                if ((PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_TWITTER_APP_ID") &&
                    PortalSettings.CustomSettings["SITESETTINGS_TWITTER_APP_ID"].ToString().Equals(string.Empty) ||
                    PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_TWITTER_APP_SECRET") &&
                    PortalSettings.CustomSettings["SITESETTINGS_TWITTER_APP_SECRET"].ToString().Equals(string.Empty)) &&
                    (PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_FACEBOOK_APP_ID") &&
                    PortalSettings.CustomSettings["SITESETTINGS_FACEBOOK_APP_ID"].ToString().Equals(string.Empty) ||
                    PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_FACEBOOK_APP_SECRET") &&
                    PortalSettings.CustomSettings["SITESETTINGS_FACEBOOK_APP_SECRET"].ToString().Equals(string.Empty)) &&
                    PortalSettings.CustomSettings.ContainsKey("SITESETTINGS_GOOGLE_LOGIN") &&
                    PortalSettings.CustomSettings["SITESETTINGS_GOOGLE_LOGIN"].ToString().Length != 0 &&
                    !bool.Parse(PortalSettings.CustomSettings["SITESETTINGS_GOOGLE_LOGIN"].ToString())
                ) {

                    SocialNetwork.Visible = false;

                }
                labelSocialNetwork.Text = Resources.Appleseed.SIGNIN_SHOW_FACEBOOK_OPTIONS;
            }
        }

        #region Properties

        public override Guid GuidID
        {
            get
            {
                return new Guid("{A1783E61-C038-439C-BA35-7743D69580DA}");
            }
        }

        #endregion

        public override void Logoff()
        {
            this.SignInSocialNetwork1.Logoff();
        }
    }
}