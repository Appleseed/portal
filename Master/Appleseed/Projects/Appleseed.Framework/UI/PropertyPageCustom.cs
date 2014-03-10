// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyPageCustom.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   PropertyPage_custom inherits from Appleseed.Framework.UI.PropertyPage <br />
//   Used for properties pages to display custom properties of a module<br />
//   Can be inherited
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Web.UI
{
    using System.Collections;

    using Appleseed.Framework.Site.Configuration;

    /// <summary>
    /// PropertyPage_custom inherits from Appleseed.Framework.UI.PropertyPage <br/>
    ///     Used for properties pages to display custom properties of a module<br/>
    ///     Can be inherited
    /// </summary>
    public class PropertyPageCustom : PropertyPage
    {
        #region Constants and Fields

        /// <summary>
        ///     The custom user settings.
        /// </summary>
        private Hashtable customUserSettings;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets current module settings
        /// </summary>
        /// <value>The custom user settings.</value>
        public Hashtable CustomUserSettings
        {
            get
            {
                if (this.customUserSettings == null)
                {
                    if (this.ModuleID > 0)
                    {
                        // Get settings from the database
                        this.customUserSettings = ModuleSettingsCustom.GetModuleUserSettings(
                            this.ModuleID, PortalSettings.CurrentUser.Identity.ProviderUserKey, this);
                    }
                    else
                    {
                        // Or provides an empty hashtable
                        this.customUserSettings = new Hashtable();
                    }
                }

                return this.customUserSettings;
            }
        }

        #endregion
    }
}