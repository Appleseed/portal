using System;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Appleseed.Framework;
using Appleseed.Framework.DataTypes;
using Appleseed.Framework.Web.UI.WebControls;
using System.Web.Security;
using System.Collections.Generic;

namespace Appleseed.Content.Web.Modules
{
    using System.Web.UI.WebControls;

    /// <summary>
    /// The IframeModule provides an IFRAME where you can set the
    /// source URL and the height of the frame using the settings system.
    /// Default height is 200px and URL is http://www.Appleseedportal.net
    /// Written by: Jakob Hansen, hansen3000@hotmail
    /// </summary>
    public partial class IframeModule : PortalModuleControl
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            
            //string strURL = Settings["URL"].ToString();
            string strURL = BuildUrlSetting();
            if (strURL.Contains("[[User.Email]]"))
            {
                strURL = strURL.Replace("[[User.Email]]", Appleseed.Framework.Site.Configuration.PortalSettings.CurrentUser.Identity.Email);
            }
            for (int i = 0; i < 10; i++) {
                var currentParameter = String.Format("[[p{0}]]", i);
                if (strURL.Contains(currentParameter)) {
                    strURL = strURL.Replace(currentParameter, Request.QueryString["p" + i.ToString()]);
                }
            }
            string height = Settings["Height"].ToString();
            string width = Settings["Width"].ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append("<iframe frameborder='0'");
            sb.Append(" src='");
            sb.Append(strURL);
            sb.Append("'");
            sb.Append(" width='");
            sb.Append(width);
            sb.Append("'");
            sb.Append(" height='");
            sb.Append(height);
            sb.Append("'");
            sb.Append(" title='");
            sb.Append(TitleText);
            sb.Append("'");
            sb.Append(">");
            sb.Append("</iframe>");

            LiteralIframe.Text = sb.ToString();
        }

        private string BuildUrlSetting()
        {

            if(String.IsNullOrEmpty(Settings["alternativeURL"].ToString()))
            {
                return Settings["URL"].ToString();
            }

            // Checks if current url it's responding, if not, load the alternative url

            var cache = HttpRuntime.Cache;
            var correctStatusCode = "iframeUrlCacheResponseCorrectStatusCode";
            var errorStatusCode = "iframeUrlCacheResponseErrorStatusCode";
            if (cache.Get(correctStatusCode) != null)
            {
                return Settings["URL"].ToString();
            }
            if (cache.Get(errorStatusCode) != null)
            {
                return Settings["alternativeURL"].ToString();
            }

            // Check if the url it's responding

            var url = Settings["URL"].ToString();
            int timeout = 60000;
            try
            {
                timeout = int.Parse(Settings["timeToWaitForTimeOut"].ToString())*1000;
            }
            catch(Exception)
            {
                timeout = 60000;
            }

            try
            {

                var webRequest = WebRequest.Create(url) as HttpWebRequest;
                webRequest.UserAgent = HttpContext.Current.Request.UserAgent;
                webRequest.Method = WebRequestMethods.Http.Get;
                webRequest.Timeout = timeout;

                var responseData = webRequest.GetResponse();
                var statusCode = (int) ((HttpWebResponse) responseData).StatusCode;

                return addToCacheAndReturnUrl(statusCode < 400);

               
            }
            catch(Exception)
            {
                return addToCacheAndReturnUrl(false);
            }

           // return Settings["URL"].ToString();




            //string url = Settings["URL"].ToString();

            //Dictionary<string, string> parameters = new Dictionary<string, string>();
            //List<MailParametersDTO> customParameters = new List<MailParametersDTO>();

            //MembershipUser user = Membership.GetUser();
            //if (user != null) {
            //    MailParametersDTO userParameter = new MailParametersDTO();
            //    userParameter.Key = "User";
            //    userParameter.Value = (object)user;
            //    userParameter.AssemblyName = user.GetType().Assembly.GetName().ToString();
            //    customParameters.Add(userParameter);

            //    BulkMailManager mgr = new BulkMailManager();
            //    Dictionary<string, string> tempParameters = mgr.BuildParameters(url, customParameters);
            //    foreach (KeyValuePair<string, string> tempParam in tempParameters) {
            //        parameters.Add(tempParam.Key, tempParam.Value);
            //    }

            //    foreach (KeyValuePair<string, string> kvp in parameters) {
            //        url = url.Replace("[[" + kvp.Key + "]]", kvp.Value);
            //    }
            //}
            //return url;
        }

        public string addToCacheAndReturnUrl(bool correctRequest)
        {
            int time;
            var cache = HttpRuntime.Cache;
            string key;
            if(correctRequest)
            {
                key = "iframeUrlCacheResponseCorrectStatusCode";
                try
                {
                    time = int.Parse(Settings["timeToCacheCorrectAnswer"].ToString());
                }
                catch (Exception)
                {
                    time = 5;
                }
                cache.Add(key, true, null, DateTime.Now.AddMinutes(time), TimeSpan.Zero, CacheItemPriority.Normal, null);



                return Settings["URL"].ToString();
            }
            else
            {
                key = "iframeUrlCacheResponseErrorStatusCode";
                try
                {
                    time = int.Parse(Settings["timeToCacheAlternativeAnswer"].ToString());
                }
                catch (Exception)
                {
                    time = 5;
                }
                cache.Add(key, false, null, DateTime.Now.AddMinutes(time), TimeSpan.Zero, CacheItemPriority.Normal, null);

                return Settings["alternativeURL"].ToString();
            }

        }

        /// <summary>
        /// General module GUID
        /// </summary>
        /// <value></value>
        public override Guid GuidID
        {
            get { return new Guid("{2502DB18-B580-4F90-8CB4-C15E6E531005}"); }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public IframeModule()
        {
            // modified by Hongwei Shen
            SettingItemGroup group = SettingItemGroup.MODULE_SPECIAL_SETTINGS;
            int groupBase = (int) group;

            //MH:canged to support relativ url
            //SettingItem url = new SettingItem(new UrlDataType());
            var url = new SettingItem<string, TextBox>();
            url.Required = true;
            url.Group = group;
            url.Order = groupBase + 20; //1;
            url.Value = "http://www.Appleseedportal.net";
            this.BaseSettings.Add("URL", url);

            var alternativeurl = new SettingItem<string, TextBox>();
            alternativeurl.Required = false;
            alternativeurl.Group = group;
            alternativeurl.Order = groupBase + 21; //2;
            alternativeurl.Value = string.Empty;
            alternativeurl.EnglishName = "Alternative URL";
            this.BaseSettings.Add("alternativeURL", alternativeurl);

            var timeToCacheCorrectAnswer = new SettingItem<string, TextBox>();
            timeToCacheCorrectAnswer.Required = true;
            timeToCacheCorrectAnswer.Group = group;
            timeToCacheCorrectAnswer.Order = groupBase + 22; //3;
            timeToCacheCorrectAnswer.Value = "5";
            timeToCacheCorrectAnswer.EnglishName = "Time to cache url in minutes";
            this.BaseSettings.Add("timeToCacheCorrectAnswer", timeToCacheCorrectAnswer);

            var timeToCacheAlternativeAnswer = new SettingItem<string, TextBox>();
            timeToCacheAlternativeAnswer.Required = true;
            timeToCacheAlternativeAnswer.Group = group;
            timeToCacheAlternativeAnswer.Order = groupBase + 23; //3;
            timeToCacheAlternativeAnswer.Value = "5";
            timeToCacheAlternativeAnswer.EnglishName = "Time to cache alternative url in minutes";
            this.BaseSettings.Add("timeToCacheAlternativeAnswer", timeToCacheAlternativeAnswer);

            var timeToWaitForTimeOut = new SettingItem<string, TextBox>();
            timeToWaitForTimeOut.Required = true;
            timeToWaitForTimeOut.Group = group;
            timeToWaitForTimeOut.Order = groupBase + 24; //3;
            timeToWaitForTimeOut.Value = "60";
            timeToWaitForTimeOut.EnglishName = "Time to Wait for TimeOut in Seconds";
            this.BaseSettings.Add("timeToWaitForTimeOut", timeToWaitForTimeOut);

            //MH: added to support width values
            var width = new SettingItem<string, TextBox>();
            width.Required = true;
            width.Group = group;
            width.Order = groupBase + 25; //3;
            width.Value = "250";
            //width.MinValue = 1;
            //width.MaxValue = 2000;
            this.BaseSettings.Add("Width", width);

            //MH: changed to StringDataType to support  percent or pixel values
            //SettingItem width = new SettingItem<int, TextBox>();
            var height = new SettingItem<string, TextBox>();
            height.Required = true;
            height.Group = group;
            height.Order = groupBase + 30; //4;
            height.Value = "250";
            //height.MinValue = 1;
            //height.MaxValue = 2000;
            this.BaseSettings.Add("Height", height);

            

        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises Init event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);

            // Create a new Title the control
//			ModuleTitle = new DesktopModuleTitle();
            // Add title ad the very beginning of 
            // the control's controls collection
//			Controls.AddAt(0, ModuleTitle);

            base.OnInit(e);
        }

        #endregion
    }
}