// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortalModuleControlCustom.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   A PortalModuleControl that supports CustomUserSettings for authenticated users. (Users can specify
//   settings for the module instance that will apply only to them when they interact with the module).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI.WebControls
{
    using System.Collections;
    using System.Web;
    using System.Web.UI.WebControls;

    using Appleseed.Framework.Settings.Cache;
    using Appleseed.Framework.Site.Configuration;

    /// <summary>
    /// A PortalModuleControl that supports CustomUserSettings for authenticated users. (Users can specify
    ///   settings for the module instance that will apply only to them when they interact with the module).
    /// </summary>
    public class PortalModuleControlCustom : PortalModuleControl
    {
        // provide a custom Hash table that will store user-specific settings for this instance
        // of the module.  
        #region Constants and Fields

        /// <summary>
        /// The custom user settings.
        /// </summary>
        protected Hashtable CustomUserSettings;

        /// <summary>
        /// The customize button.
        /// </summary>
        private ModuleButton customizeButton;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets Module Properties button
        /// </summary>
        /// <value>The customize button.</value>
        public ModuleButton CustomizeButton
        {
            get
            {
                if (this.customizeButton == null && HttpContext.Current != null)
                {
                    // check authority
                    if (this.HasCustomizeableSettings && PortalSettings.CurrentUser.Identity.IsOnline)
                    {
                        // create the button
                        this.customizeButton = new ModuleButton
                            {
                                Group = ModuleButton.ButtonGroup.Admin,
                                EnglishName = "Customize",
                                TranslationKey = "CUSTOMIZE",
                                Image = this.CurrentTheme.GetImage("Buttons_Properties", "Properties.gif")
                            };
                        if (this.PropertiesUrl.IndexOf("?") >= 0)
                        {
                            // Do not change if  the query string is present (shortcut patch)
                            // if ( this.ModuleID != this.OriginalModuleID ) // shortcut
                            this.customizeButton.HRef = this.PropertiesUrl;
                        }
                        else
                        {
                            this.customizeButton.HRef =
                                HttpUrlBuilder.BuildUrl(
                                    "~/DesktopModules/CoreModules/Admin/CustomPropertyPage.aspx", 
                                    this.PageID, 
                                    string.Format("mID={0}", this.ModuleID));
                        }

                        this.customizeButton.Target = this.PropertiesTarget;
                        this.customizeButton.RenderAs = this.ButtonsRenderAs;
                    }
                }

                return this.customizeButton;
            }
        }

        /// <summary>
        ///   Gets the customized user settings.
        /// </summary>
        /// <value>The customized user settings.</value>
        public Hashtable CustomizedUserSettings
        {
            get
            {
                if (this.CustomUserSettings != null)
                {
                    return this.CustomUserSettings;
                }

                var tempSettings = new Hashtable();

                // refresh this module's settings on every call in case they logged off, so it will
                // retrieve the 'default' settings from the database.
                // Invalidate cache
                CurrentCache.Remove(Key.ModuleSettings(this.ModuleID));

                // this._baseSettings = ModuleSettings.GetModuleSettings(this.ModuleID, this._baseSettings);
                foreach (string str in this.Settings.Keys)
                {
                    var thedefault = this.Settings[str] as SettingItem<string, TextBox>;
                    if (thedefault != null)
                    {
                        if (thedefault.Group == SettingItemGroup.CUSTOM_USER_SETTINGS)
                        {
                            // It's one we want to customize
                            tempSettings.Add(str, thedefault); // insert the 'default' value
                        }
                    }
                }

                // Now, replace the default settings with the custom settings for this user from the database.
                return ModuleSettingsCustom.GetModuleUserSettings(
                    this.ModuleConfiguration.ModuleID, 
                    PortalSettings.CurrentUser.Identity.ProviderUserKey, 
                    tempSettings);
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance has customizable settings.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has customizable settings; otherwise, <c>false</c>.
        /// </value>
        public bool HasCustomizeableSettings
        {
            get
            {
                return this.CustomizedUserSettings.Count > 0;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds the three public button lists
        /// </summary>
        protected override void BuildButtonLists()
        {
            if (this.CustomizeButton != null)
            {
                this.ButtonListAdmin.Add(this.CustomizeButton);
            }

            base.BuildButtonLists();
        }

        #endregion
    }
}