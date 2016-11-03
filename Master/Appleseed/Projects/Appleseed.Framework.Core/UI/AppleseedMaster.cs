using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Collections;
using Appleseed.Framework.Site.Configuration;
using System.Web.Mvc;
using System.Web;
using System.Xml.Linq;
using System;
using Appleseed.Framework;
using Appleseed.Framework.Design;
using System.IO;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace Appleseed
{
    /// <summary>
    /// 
    /// </summary>
    public class AppleseedMaster : System.Web.Mvc.ViewMasterPage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(System.EventArgs e)
        {
            InsertAllScripts(Page, Context);

            base.OnLoad(e);
        }

        /// <summary>
        /// load all scripts
        /// </summary>
        /// <param name="page">page</param>
        /// <param name="context">context</param>
        public static void InsertAllScripts(Page page, HttpContext context)
        {
            if (!page.ClientScript.IsClientScriptBlockRegistered("allscripts"))
            {


                var scripts = GetBaseScripts(page, context);
                string datetimestamp = DateTime.Now.ToString("ddMMyyyyHHmmss");
                int index = 0;

                #region
              
                if (ConfigurationManager.AppSettings["CSSLoadTop"] != null && bool.Parse(ConfigurationManager.AppSettings["CSSLoadTop"]))
                {
                    var psCSS = (PortalSettings)context.Items["PortalSettings"];

                    HtmlGenericControl includeDefaultCSS = new HtmlGenericControl("link");
                    includeDefaultCSS.Attributes.Add("type", "text/css");
                    includeDefaultCSS.Attributes.Add("rel", "stylesheet");
                    includeDefaultCSS.Attributes.Add("href", psCSS.GetCurrentTheme().CssFile);
                    page.Header.Controls.AddAt(index++, includeDefaultCSS);




                    //var portalSettings = (PortalSettings)context.Items["PortalSettings"];
                    if (psCSS != null)
                    {
                        var cssHref = page.ResolveUrl("~/Design/jqueryUI/" + psCSS.PortalAlias + "/jquery-ui.css");

                        HtmlGenericControl include = new HtmlGenericControl("link");
                        include.Attributes.Add("type", "text/css");
                        include.Attributes.Add("rel", "stylesheet");
                        include.Attributes.Add("href", cssHref);
                        page.Header.Controls.AddAt(index++, include);
                    }

                    if (psCSS != null)
                    {
                        var cssHref = page.ResolveUrl("~/Design/jqueryUI/" + psCSS.PortalAlias + "/jquery-ui-anant.css");

                        HtmlGenericControl include = new HtmlGenericControl("link");
                        include.Attributes.Add("type", "text/css");
                        include.Attributes.Add("rel", "stylesheet");
                        include.Attributes.Add("href", cssHref);
                        page.Header.Controls.AddAt(index++, include);
                    }

                    var multiselect = page.ResolveUrl("~/aspnet_client/jQuery/ui.multiselect.css");

                    HtmlGenericControl add = new HtmlGenericControl("link");
                    add.Attributes.Add("type", "text/css");
                    add.Attributes.Add("rel", "stylesheet");
                    add.Attributes.Add("href", multiselect);
                    page.Header.Controls.AddAt(index++, add);

                    var csshref = page.ResolveUrl("~/aspnet_client/CSSControlAdapters/HtmlEditStyle.css");

                    HtmlGenericControl Include = new HtmlGenericControl("link");
                    Include.Attributes.Add("type", "text/css");
                    Include.Attributes.Add("rel", "stylesheet");
                    Include.Attributes.Add("href", csshref);
                    page.Header.Controls.AddAt(index++, Include);

                    foreach (var script in scripts)
                    {
                        HtmlGenericControl include = new HtmlGenericControl("script");
                        include.Attributes.Add("type", "text/javascript");
                        include.Attributes.Add("src", script as string);
                        page.Header.Controls.AddAt(index++, include);
                    }

                }

                #endregion
                else
                {
                    foreach (var script in scripts)
                    {
                        HtmlGenericControl include = new HtmlGenericControl("script");
                        include.Attributes.Add("type", "text/javascript");
                        include.Attributes.Add("src", script as string);
                        page.Header.Controls.AddAt(index++, include);
                    }

                    var portalSettings = (PortalSettings)context.Items["PortalSettings"];

                    if (portalSettings != null)
                    {
                        var cssHref = page.ResolveUrl("~/Design/jqueryUI/" + portalSettings.PortalAlias + "/jquery-ui.css");

                        HtmlGenericControl include = new HtmlGenericControl("link");
                        include.Attributes.Add("type", "text/css");
                        include.Attributes.Add("rel", "stylesheet");
                        include.Attributes.Add("href", cssHref);
                        page.Header.Controls.AddAt(index++, include);
                    }

                    if (portalSettings != null)
                    {
                        var cssHref = page.ResolveUrl("~/Design/jqueryUI/" + portalSettings.PortalAlias + "/jquery-ui-anant.css");

                        HtmlGenericControl include = new HtmlGenericControl("link");
                        include.Attributes.Add("type", "text/css");
                        include.Attributes.Add("rel", "stylesheet");
                        include.Attributes.Add("href", cssHref);
                        page.Header.Controls.AddAt(index++, include);
                    }

                    var multiselect = page.ResolveUrl("~/aspnet_client/jQuery/ui.multiselect.css");

                    HtmlGenericControl add = new HtmlGenericControl("link");
                    add.Attributes.Add("type", "text/css");
                    add.Attributes.Add("rel", "stylesheet");
                    add.Attributes.Add("href", multiselect);
                    page.Header.Controls.AddAt(index++, add);

                    var csshref = page.ResolveUrl("~/aspnet_client/CSSControlAdapters/HtmlEditStyle.css");

                    HtmlGenericControl Include = new HtmlGenericControl("link");
                    Include.Attributes.Add("type", "text/css");
                    Include.Attributes.Add("rel", "stylesheet");
                    Include.Attributes.Add("href", csshref);
                    page.Header.Controls.AddAt(index++, Include);
                }
                var uiculture = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                var datepickerscript = "$(document).ready(function(){$.datepicker.setDefaults($.datepicker.regional['" + uiculture + "']);});";

                HtmlGenericControl includedp = new HtmlGenericControl("script");
                includedp.Attributes.Add("type", "text/javascript");
                includedp.InnerHtml = datepickerscript;
                page.Header.Controls.AddAt(index++, includedp);

                var accordion = "$(function() {$('.accordion').accordion({collapsible: true,active: false,alwaysOpen: false,});});";
                //"$(.accordion').accordion({collapsible: true,active: false,alwaysOpen: false,});";

                HtmlGenericControl includeacc = new HtmlGenericControl("script");
                includeacc.Attributes.Add("type", "text/javascript");
                includeacc.InnerHtml = accordion;
                page.Header.Controls.AddAt(index++, includeacc);

                var popupcss = page.ResolveUrl("~/aspnet_client/popupHelper/popup.css");

                HtmlGenericControl addpopupcss = new HtmlGenericControl("link");
                addpopupcss.Attributes.Add("type", "text/css");
                addpopupcss.Attributes.Add("rel", "stylesheet");
                addpopupcss.Attributes.Add("href", popupcss);
                page.Header.Controls.AddAt(index++, addpopupcss);

                string extraScripts = GetExtraScripts();
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), "allscripts", extraScripts, false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetExtraScripts()
        {
            string scripts = string.Empty;
            try
            {
                string filePath = HttpContext.Current.Server.MapPath("~/Scripts/Scripts.xml");
                if (File.Exists(filePath))
                {
                    XDocument xml = XDocument.Load(filePath);
                    foreach (var s in xml.Descendants("scripts").DescendantNodes())
                    {
                        scripts += s.ToString() + Environment.NewLine;
                    }
                }
            }
            catch (Exception exc)
            {
                ErrorHandler.Publish(LogLevel.Debug, exc);
            }

            return scripts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [History("Ashish.patel@haptix.biz", "2014/12/10", "Add key for loading user specific jQuery and jQueryUI")]
        public static ArrayList GetBaseScripts(Page page, HttpContext context)
        {
            var scripts = new ArrayList();

            string min = "min.js";
            string js = "js";
            string ScriptsModeDebug = System.Configuration.ConfigurationManager.AppSettings.Get("ScriptsModeDebug");
            if (ScriptsModeDebug != null && ScriptsModeDebug.Equals("true"))
            {
                min = "js";
                js = "debug.js";
            }

            // Set portalSettings
            var portalSettings = (PortalSettings)context.Items["PortalSettings"];

            // if user set the SITESETTINGS_JQUERY value then his own js will load other wise default js will load 
            // This is for jQuery
            if (portalSettings.CustomSettings.ContainsKey("SITESETTINGS_JQUERY") &&
                portalSettings.CustomSettings["SITESETTINGS_JQUERY"].Value != null)
                scripts.Add(page.ResolveUrl(portalSettings.CustomSettings["SITESETTINGS_JQUERY"].Value.ToString()));
            else
                scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/jquery-1.8.3." + min));

            // if user set the SITESETTINGS_JQUERY_UI value then his own js will load
            // This is for jQuery UI
            if (portalSettings.CustomSettings.ContainsKey("SITESETTINGS_JQUERY_UI") &&
                portalSettings.CustomSettings["SITESETTINGS_JQUERY_UI"].Value != null)
                scripts.Add(page.ResolveUrl(portalSettings.CustomSettings["SITESETTINGS_JQUERY_UI"].Value.ToString()));
            else
                scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/jquery-ui-1.12.1." + min));

            scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/jquery.validate." + min));
            scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/jquery.validate.unobtrusive." + min));
            scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/jquery.bgiframe." + min));
            scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/jquery-ui-i18n.min.js"));
            scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/jquery.unobtrusive-ajax." + min));
            scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/jquery.jeditable.js"));

            scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/modernizr-2.6.2." + min));
            scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/jquery.cookie.js"));

            scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/MicrosoftAjax." + js));
            scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/MicrosoftMvcAjax." + js));
            scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/MicrosoftMvcValidation." + js));

            scripts.Add(page.ResolveUrl("~/aspnet_client/js/DragNDrop.js"));
            scripts.Add(page.ResolveUrl("~/aspnet_client/js/browser_upgrade_notification.js"));
            scripts.Add(page.ResolveUrl("~/aspnet_client/js/AppleseedScripts.js"));

            scripts.Add(page.ResolveUrl("~/aspnet_client/CSSControlAdapters/AdapterUtils.js"));
            scripts.Add(page.ResolveUrl("~/aspnet_client/CSSControlAdapters/MenuAdapter.js"));
            scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/ui.multiselect.js"));
            scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/jQueryTabs.js"));

            //scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/jsTree/_lib/jquery.cookie.js"));
            //scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/jsTree/_lib/jquery.hotkeys.js"));
            //scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/jsTree/jquery.jstree.js"));
            //scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/jsTree/_docs/syntax/!script.js"));

            return scripts;
        }

        
        /// <summary>
        /// ModalChangeMaster=true
        /// </summary>
        public static bool IsModalChangeMaster
        {
            get
            {
                return HttpContext.Current.Request.QueryString["ModalChangeMaster"] == "true";
            }
        }
    }
}