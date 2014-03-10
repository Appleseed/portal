// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalResources.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Summary description for GlobalResources.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Appleseed.Framework.BLL.Utils
{
    using System;
    using System.Configuration;

    /// <summary>
    /// Encapsulate resources -- it can come from anywhere
    /// </summary>
    [Obsolete("use Appleseed.Framework.Settings.Config")]
    public class GlobalResources
    {
        // jes1111 - moved to GlobalInternalStrings
        // <summary>
        // /// non breakable html space character
        // /// </summary>
        // public  const string HTML_SPACE = "&nbsp;";
        #region Properties

        /// <summary>
        ///     Gets a value indicating whether the Portal supports Window Mgmt Functions/Controls
        /// </summary>
        /// <value><c>true</c> if [support window MGMT]; otherwise, <c>false</c>.</value>
        public static bool SupportWindowMgmt
        {
            get
            {
                return SafeBoolean("WindowMgmtControls", false);
            }
        }

        /// <summary>
        ///     Gets a value indicating whether we support the close button
        /// </summary>
        /// <value>
        ///     <c>true</c> if [support window MGMT close]; otherwise, <c>false</c>.
        /// </value>
        public static bool SupportWindowMgmtClose
        {
            get
            {
                return SafeBoolean("WindowMgmtWantClose", false);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Boolean Resource
        /// </summary>
        /// <param name="name">
        /// The name of the resource.
        /// </param>
        /// <param name="defaultRet">
        /// if set to <c>true</c> [default_ret].
        /// </param>
        /// <returns>
        /// The safe boolean.
        /// </returns>
        public static bool SafeBoolean(string name, bool defaultRet)
        {
            bool returnVal;
            return bool.TryParse(ConfigurationManager.AppSettings[name], out returnVal) ? returnVal : defaultRet;
        }

        /// <summary>
        /// Get Integer Resource
        /// </summary>
        /// <param name="name">
        /// The resource name.
        /// </param>
        /// <param name="defaultRet">
        /// The default_ret.
        /// </param>
        /// <returns>
        /// The safe int.
        /// </returns>
        public static int SafeInt(string name, int defaultRet)
        {
            int returnVal;
            return int.TryParse(ConfigurationManager.AppSettings[name], out returnVal) ? returnVal : defaultRet;
        }

        /// <summary>
        /// Get string Resource
        /// </summary>
        /// <param name="name">
        /// The resource name.
        /// </param>
        /// <param name="defaultRet">
        /// The default_ret.
        /// </param>
        /// <returns>
        /// The safe string.
        /// </returns>
        public static string SafeString(string name, string defaultRet)
        {
            var obj = ConfigurationManager.AppSettings[name];

            return obj ?? defaultRet;
        }

        #endregion
    }
}