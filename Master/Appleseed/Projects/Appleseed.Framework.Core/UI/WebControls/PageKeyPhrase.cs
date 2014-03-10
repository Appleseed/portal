using System;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using Appleseed.Framework.Site.Configuration;

namespace Appleseed.Framework.Web.UI.WebControls
{
    /// <summary>
    /// Created By john.mandia@whitelightsolutions.com - This control is used by the PageKeyPhrase Module.
    /// </summary>
    public class PageKeyPhrase : Label
    {
        // Obtain PortalSettings from Current Context
        private PortalSettings PortalSettings = (PortalSettings) HttpContext.Current.Items["PortalSettings"];
        private string _tabKeyPhrase;
        private string currentLanguage;

        /// <summary>
        /// Stores current Tab Key Phrase
        /// </summary>
        /// <value>The tab key phrase.</value>
        public string TabKeyPhrase
        {
            get
            {
                currentLanguage = "TabKeyPhrase_" + PortalSettings.PortalContentLanguage.Name.ToString();
                if (PortalSettings.PortalContentLanguage != CultureInfo.InvariantCulture
                    && PortalSettings.ActivePage.CustomSettings[currentLanguage] != null)
                {
                    _tabKeyPhrase =
                        (string) ViewState["TabKeyPhrase_" + PortalSettings.PortalContentLanguage.Name.ToString()];
                    if (_tabKeyPhrase != null)
                        return _tabKeyPhrase;
                    else
                    {
                        // Try to get this tab's key phrase
                        _tabKeyPhrase = PortalSettings.ActivePage.CustomSettings[currentLanguage].ToString();

                        if (_tabKeyPhrase == string.Empty && PortalSettings.CustomSettings != null)
                        {
                            _tabKeyPhrase = PortalSettings.ActivePage.CustomSettings["TabKeyPhrase"].ToString();

                            if (_tabKeyPhrase == string.Empty)
                                _tabKeyPhrase = PortalSettings.CustomSettings["SITESETTINGS_PAGE_KEY_PHRASE"].ToString();
                        }

                        return _tabKeyPhrase;
                    }
                }
                else
                {
                    _tabKeyPhrase = (string) ViewState["TabKeyPhrase"];
                    if (_tabKeyPhrase != null)
                        return _tabKeyPhrase;
                    else
                    {
                        if (PortalSettings.ActivePage.CustomSettings["TabKeyPhrase"] != null)
                        {
                            // Try to get this tab's key phrase
                            _tabKeyPhrase = PortalSettings.ActivePage.CustomSettings["TabKeyPhrase"].ToString();

                            if (_tabKeyPhrase == string.Empty && PortalSettings.CustomSettings != null)
                            {
                                _tabKeyPhrase = PortalSettings.CustomSettings["SITESETTINGS_PAGE_KEY_PHRASE"].ToString();
                            }

                            return _tabKeyPhrase;
                        }
                        return string.Empty;
                    }
                }
            }
            set
            {
                if (PortalSettings.PortalContentLanguage != CultureInfo.InvariantCulture)
                {
                    ViewState["TabKeyPhrase_" + PortalSettings.PortalContentLanguage.Name.ToString()] = value;
                }
                else
                {
                    ViewState["TabKeyPhrase"] = value;
                }
            }
        }

        /// <summary>
        /// Load event handler
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            Text = TabKeyPhrase;

            base.DataBind();
        }
    }
}