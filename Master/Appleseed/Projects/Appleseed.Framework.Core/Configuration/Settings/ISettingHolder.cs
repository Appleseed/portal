namespace Appleseed.Framework.Configuration.Settings
{
    using System.Collections.Generic;

    /// <summary>
    /// Setting Holder Interface
    /// </summary>
    /// <remarks></remarks>
    public interface ISettingHolder
    {
        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <remarks></remarks>
        IDictionary<string, ISettingItem> Settings { get; }

        /// <summary>
        /// Inserts or updates the setting.
        /// </summary>
        /// <param name="settingItem">The setting item.</param>
        /// <remarks></remarks>
        void Upsert(ISettingItem settingItem);
    }
}
