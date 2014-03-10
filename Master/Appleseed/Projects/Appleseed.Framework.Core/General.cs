// --------------------------------------------------------------------------------------------------------------------
// <copyright file="General.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Static helper methods for one line calls
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework
{
    using System;
    using System.Web;

    /// <summary>
    /// Static helper methods for one line calls
    /// </summary>
    /// <remarks>
    /// <list type="string">
    /// <item>
    /// GetString
    /// </item>
    /// </list>
    /// </remarks>
    public static class General
    {
        #region Public Methods

        /// <summary>
        /// Get a resource string value
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// The get string.
        /// </returns>
        public static string GetString(string key)
        {
            return GetString(key, string.Empty);
        }

        /// <summary>
        /// Get a resource string value
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="o">The o.</param>
        /// <returns>
        /// The get string.
        /// </returns>
        public static string GetString(string key, string defaultValue, object o)
        {
            // TODO: What are objects passed around for?
            return GetString(key, defaultValue);
        }

        /// <summary>
        /// Get a resource string value
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The get string.
        /// </returns>
        public static string GetString(string key, string defaultValue)
        {
            if (HttpContext.Current == null)
            {
                var ne = new Exception("HttpContext.Current not an object");
                ErrorHandler.Publish(LogLevel.Warn, "Problem with Global Resources - could not get key: " + key, ne);
                return "<span class='error'>Could not get key: " + key + "</span>";
            }

            try
            {
                // TODO: Should we be using cached resource set per language?
#if DEBUG
                HttpContext.Current.Trace.Warn(string.Format("GetString({0})", key));
#endif

                // userCulture = Thread.CurrentThread.CurrentCulture.Name;
                var str = HttpContext.GetGlobalResourceObject("Appleseed", key);

                // string str = ((Appleseed.Framework.Web.UI.Page)System.Web.UI.Page).UserCultureSet.GetString(key);
                var ret = string.Empty;

                if (str != null)
                {
                    ret = str.ToString();
#if DEBUG
                    HttpContext.Current.Trace.Warn(
                        ret.Length > 0 ? "We got localized  version" : "Localized return empty, use default");

#endif
                }

                if (ret.Length == 0)
                {
                    return defaultValue;
                }

                HttpContext.Current.Trace.Warn("GetString  = " + ret);
                return ret;
            }
            catch (Exception ex)
            {
                ErrorHandler.Publish(LogLevel.Warn, "Problem with Global Resources - could not get key: " + key, ex);
                return defaultValue;
            }
        }

        #endregion
    }
}