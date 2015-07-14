// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingItemGroups.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   SettingItemGroups, used to sort and group site and module
//   settings in SettingsTable.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework
{
    /// <summary>
    /// SettingItemGroups, used to sort and group site and module
    ///   settings in SettingsTable.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public enum SettingItemGroup
    {
        /// <summary>
        /// The none.
        /// </summary>
        NONE = 0, 

        /// <summary>
        /// The theme layout settings.
        /// </summary>
        THEME_LAYOUT_SETTINGS = 1000, 

        /// <summary>
        /// The security user settings.
        /// </summary>
        SECURITY_USER_SETTINGS = 2000, 

        /// <summary>
        /// The culture settings.
        /// </summary>
        CULTURE_SETTINGS = 3000, 

        /// <summary>
        /// The button display settings.
        /// </summary>
        BUTTON_DISPLAY_SETTINGS = 6000, 

        /// <summary>
        /// The module special settings.
        /// </summary>
        MODULE_SPECIAL_SETTINGS = 7000, 

        /// <summary>
        /// The meta settings.
        /// </summary>
        META_SETTINGS = 8000, 

        /// <summary>
        /// The misc settings.
        /// </summary>
        MISC_SETTINGS = 9000, 

        /// <summary>
        /// The navigation settings.
        /// </summary>
        NAVIGATION_SETTINGS = 10000, 

        /// <summary>
        /// The custom user settings.
        /// </summary>
        CUSTOM_USER_SETTINGS = 15000, 

        /// <summary>
        ///   Module Data Filter (aka. MDF).
        /// </summary>
        MDF_SETTINGS = 20000,

        /// <summary>
        /// for user define css and js
        /// </summary>
        ADD_CSS_JAVASCRIPT = 25000

    }
}