namespace Appleseed.Framework.Configuration.Settings
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Settings Manager
    /// </summary>
    /// <remarks>
    /// Used to load and save all settings from a centralized point.
    /// </remarks>
    public class Manager
    {
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="settingHolder">The setting holder.</param>
        /// <returns>An enumerable list of setting items.</returns>
        /// <remarks></remarks>
        public static IDictionary<string, ISettingItem> GetAll(ISettingHolder settingHolder)
        {
            return settingHolder.Settings;
        }

        /// <summary>
        /// Gets the specified setting name.
        /// </summary>
        /// <param name="settingHolder">The setting holder.</param>
        /// <param name="settingName">Name of the setting.</param>
        /// <returns>A setting item.</returns>
        /// <remarks></remarks>
        public static ISettingItem Get(ISettingHolder settingHolder, string settingName)
        {
            return settingHolder.Settings.ContainsKey(settingName) ? settingHolder.Settings[settingName] : null;
        }

        /// <summary>
        /// Gets the specified setting name.
        /// </summary>
        /// <typeparam name="T">The type of the setting to get.</typeparam>
        /// <param name="settingHolder">The setting holder.</param>
        /// <param name="settingName">Name of the setting.</param>
        /// <returns>A setting item of type T.</returns>
        /// <remarks></remarks>
        public static ISettingItem<T> Get<T>(ISettingHolder settingHolder, string settingName)
        {
            return settingHolder.Settings.ContainsKey(settingName) ? settingHolder.Settings[settingName] as ISettingItem<T> : null;
        }

        /// <summary>
        /// Sets the specified setting name.
        /// </summary>
        /// <param name="settingHolder">The setting holder.</param>
        /// <param name="settingItem">The setting item.</param>
        /// <remarks></remarks>
        public void Set(ISettingHolder settingHolder, ISettingItem settingItem)
        {
            settingHolder.Upsert(settingItem);
        }
    }
}
