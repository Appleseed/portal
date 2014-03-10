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
using System.IO;

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
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="context"></param>
        public static void InsertAllScripts(Page page, HttpContext context)
        {
            if (!page.ClientScript.IsClientScriptBlockRegistered("allscripts")) {
                var scripts = GetBaseScripts(page);

                int index = 0;
                foreach (var script in scripts) {
                    HtmlGenericControl include = new HtmlGenericControl("script");
                    include.Attributes.Add("type", "text/javascript");
                    include.Attributes.Add("src", script as string);
                    page.Header.Controls.AddAt(index++, include);
                }

                var portalSettings = (PortalSettings)context.Items["PortalSettings"];

                if (portalSettings != null) {
                    var cssHref = page.ResolveUrl("~/Design/jqueryUI/" + portalSettings.PortalAlias + "/jquery-ui.custom.css");


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

                //HtmlGenericControl includeStyleTree = new HtmlGenericControl("link");
                //includeStyleTree.Attributes.Add("type", "text/css");
                //includeStyleTree.Attributes.Add("rel", "stylesheet");
                //includeStyleTree.Attributes.Add("href", HttpUrlBuilder.BuildUrl("~/aspnet_client/jQuery/jsTree/themes/default/style.css"));
                //page.Header.Controls.AddAt(index++, includeStyleTree);


                

                
                

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
            try {
                string filePath = HttpContext.Current.Server.MapPath("~/Scripts/Scripts.xml");
                if (File.Exists(filePath)) {
                    XDocument xml = XDocument.Load(filePath);
                    foreach (var s in xml.Descendants("scripts").DescendantNodes()) {
                        scripts += s.ToString() + Environment.NewLine;
                    }
                }
            } catch (Exception exc) {
                ErrorHandler.Publish(LogLevel.Debug, exc);
            }

            return scripts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ArrayList GetBaseScripts(Page page)
        {
            var scripts = new ArrayList();

            string min = "min.js";
            string js = "js";
            string ScriptsModeDebug = System.Configuration.ConfigurationManager.AppSettings.Get("ScriptsModeDebug");
            if (ScriptsModeDebug != null && ScriptsModeDebug.Equals("true")) {
                min = "js";
                js = "debug.js";
            }

            scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/jquery-1.8.3." + min));
            scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/jquery-ui-1.9.2."+min));
            scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/jquery.validate."+min));
            scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/jquery.validate.unobtrusive." + min));
            scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/jquery.bgiframe." + min));
            scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/jquery-ui-i18n.min.js"));
            scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/jquery.unobtrusive-ajax." + min));
            scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/jquery.jeditable.js"));

            scripts.Add(page.ResolveUrl("~/aspnet_client/jQuery/modernizr-2.6.2."+min));
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
    }
}