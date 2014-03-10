// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Selector.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   This user control will render the current list of languages.
//   by José Viladiu
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI.WebControls
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.UI;

    using Appleseed.Framework.Settings;
    using Appleseed.Framework.Site.Configuration;

    /// <summary>
    /// This user control will render the current list of languages.
    ///     by José Viladiu
    /// </summary>
    [ToolboxData("<{0}:Selector runat='server'></{0}:Selector>")]
    [History("jviladiu@portalservices.net", "2004/06/15", "First Implementation Selector webcontrol for Appleseed")]
    public class Selector : LanguageSwitcher
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref = "Selector" /> class.
        /// </summary>
        public Selector()
        {
            this.ImagePath = Path.WebPathCombine(Path.ApplicationRoot, "aspnet_client/flags/");
            this.ChangeLanguageAction = LanguageSwitcherAction.PostBack;

            // Obtain PortalSettings from Current Context
            if (HttpContext.Current != null && HttpContext.Current.Items["PortalSettings"] != null)
            {
                var pS = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
                if (pS.CustomSettings != null)
                {
                    if (pS.CustomSettings["SITESETTINGS_LANGLIST"] != null)
                    {
                        this.LanguageListString = pS.CustomSettings["SITESETTINGS_LANGLIST"].ToString();
                    }

                    if (pS.CustomSettings["LANGUAGESWITCHER_CUSTOMFLAGS"] != null)
                    {
                        if (bool.Parse(pS.CustomSettings["LANGUAGESWITCHER_CUSTOMFLAGS"].ToString()))
                        {
                            this.ImagePath = pS.PortalFullPath + "/images/flags/";
                        }
                    }
                }
            }
            else
            {
                this.LanguageListString = "en-US";
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Examines/combines all the variables involved and sets
        /// CurrentUICulture and CurrentCulture.  instance containing the event data.
        /// </summary>
        /// <param name="e">Language switcher event arguments.</param>
        protected override void OnChangeLanguage(LanguageSwitcherEventArgs e)
        {
            if (this.Context == null)
            {
                return;
            }

            var mId = 0;
            if (this.Context.Request.Params["Mid"] != null)
            {
                mId = Int32.Parse(this.Context.Request.Params["Mid"]);
            }

            var tId = 0;
            if (this.Context.Request.Params["PageID"] != null)
            {
                tId = Int32.Parse(this.Context.Request.Params["PageID"]);
            }
            else if (this.Context.Request.Params["TabID"] != null)
            {
                tId = Int32.Parse(this.Context.Request.Params["TabID"]);
            }

            var auxUrl = this.Context.Request.Url.AbsolutePath;
            var auxApplication = this.Context.Request.ApplicationPath;
            var index = auxUrl.ToLower().IndexOf(auxApplication.ToLower());
            if (index != -1)
            {
                auxUrl = auxUrl.Substring(index + auxApplication.Length);
            }

            if (auxUrl.StartsWith("/"))
            {
                auxUrl = "~" + auxUrl;
            }
            else
            {
                auxUrl = "~/" + auxUrl;
            }

            var customParams =
                this.Context.Request.QueryString.Keys.Cast<string>().Where(
                    key =>
                    !key.ToLower().Equals("mid") && !key.ToLower().Equals("tabid") && !key.ToLower().Equals("lang")).
                    Aggregate(
                        string.Empty,
                        (current, key) => current + string.Format("&{0}={1}", key, this.Context.Request.Params[key]));

            var returnUrl = HttpUrlBuilder.BuildUrl(
                auxUrl, tId, mId, e.CultureItem.Culture, customParams, string.Empty, string.Empty);
            if (returnUrl.ToLower().IndexOf("lang") == -1)
            {
                customParams += "&Lang=" + e.CultureItem.Culture.Name;
                returnUrl = HttpUrlBuilder.BuildUrl(
                    auxUrl, tId, mId, e.CultureItem.Culture, customParams, string.Empty, string.Empty);
            }

            // System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(e.CultureItem
            // LanguageCultureItem lci = new LanguageCultureItem(e.CultureItem.Culture.Name, e.CultureItem.Culture.Name)
            SetCurrentLanguage(e.CultureItem);
            this.Context.Response.Redirect(returnUrl);
        }

        #endregion
    }
}