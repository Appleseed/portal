namespace Appleseed.Framework.Settings.Cache
{
	/// <summary>
	/// This class return the cache keys used in Appleseed.
	/// </summary>
	public sealed class Key
	{
		/// <summary>
		///     ctor
		/// </summary>
		/// 
		/// <returns>
		///     A void value...
		/// </returns>
		private Key() {}

		/// <summary>
		/// This method allows you to create a custom cache key for a modules' specific settings
		/// </summary>
		/// <param name="moduleID">The ID of the module you wish to get the cache key for</param>
		/// <returns>The generated cache key for the module you specified</returns>
		public static string ModuleSettings(int moduleID)
		{
			return string.Concat(Portal.UniqueID, "_ModuleSettings_", moduleID.ToString());
		}

		/// <summary>
		/// This method allows you to retrieve the cache key used to store this portal's current settings in portal settings table
		/// </summary>
		/// <returns>Cache Key For Portal Settings</returns>
		public static string PortalSettings()
		{
			return string.Concat(Portal.UniqueID, "_PortalSettings");
		}

		/// <summary>
		/// This method allows you to retrieve the cache key used to store this portal's base settings i.e. Layouts Available, Themes Available etc
		/// </summary>
		/// <returns>Cache Key For Portal Base Settings</returns>
		public static string PortalBaseSettings()
		{
			return string.Concat(Portal.UniqueID, "_PortalBaseSettings");
		}

		/// <summary>
		/// This method allows you to create a custom cache key for a Tab's specific settings
		/// </summary>
		/// <param name="tabID">The ID of the tab you wish to get the cache key for</param>
		/// <returns>The generated cache key for the tab you specified</returns>
		public static string TabSettings(int tabID)
		{
			return string.Concat(Portal.UniqueID, "_TabSettings_", tabID.ToString());
		}

		/// <summary>
		/// This method allows you to create a custom cache key for Language Specific Tab Navigation
		/// </summary>
		/// <param name="tabID">The ID of the tab you wish to get the cache key for</param>
		/// <param name="language">The language you need it in</param>
		/// <returns>The generated cache key for the tab navigation you specified</returns>
		public static string TabNavigationSettings(int tabID, string language)
		{
			return string.Concat(Portal.UniqueID, "_TabNavigationSettings_", tabID.ToString(), language);
		}

		/// <summary>
		/// This method allows you to create a custom cache key for a list of languages used in portal
		/// </summary>
		/// <returns>The generated cache key</returns>
		public static string LanguageList()
		{
			return string.Concat(Portal.UniqueID, "_LanguageList");
		}

		/// <summary>
		/// This method allows you to create a custom cache key for a list of availables image menus in current layout
		/// </summary>
		/// <param name="currentLayout">The name of current layout</param>
		/// <returns>The generated cache key</returns>
		public static string ImageMenuList(string currentLayout)
		{
			return string.Concat(Portal.UniqueID, "_ImageMenuList_", currentLayout);
		}

		/// <summary>
		/// This method allows you to create a custom cache key for a list of availables layouts in pathname
		/// </summary>
		/// <param name="pathname">The name of path</param>
		/// <returns>The generated cache key</returns>
		public static string LayoutList(string pathname)
		{
			return string.Concat(Portal.UniqueID, "_LayoutList_", pathname);
		}

		/// <summary>
		/// This method allows you to create a custom cache key for a list of availables themes in pathname
		/// </summary>
		/// <param name="pathname">The name of path</param>
		/// <returns>The generated cache key</returns>
		public static string ThemeList(string pathname)
		{
			return string.Concat(Portal.UniqueID, "_ThemeList_", pathname);
		}

		/// <summary>
		/// This method allows you to create a custom cache key for current theme
		/// </summary>
		/// <param name="pathname">The name of path</param>
		/// <returns>The generated cache key</returns>
		public static string CurrentTheme(string pathname)
		{
			return string.Concat(Portal.UniqueID, "_CurrentTheme_", pathname);
		}



	}
}