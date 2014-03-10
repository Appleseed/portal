// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CookieUtil.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   This class manages cookies
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.BLL.Utils
{
    using System;
    using System.Threading;
    using System.Web;

    /// <summary>
    /// This class manages cookies
    /// </summary>
    internal sealed class CookieUtil
    {
        #region Constants and Fields

        /// <summary>
        /// The expire time (25 minutes).
        /// </summary>
        private static TimeSpan expire = new TimeSpan(0, 0, 25, 0);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cookie expiration.
        /// </summary>
        /// <value>The expiration.</value>
        public static TimeSpan Expiration
        {
            get
            {
                return expire;
            }

            set
            {
                Monitor.Enter(expire);
                expire = value;
                Monitor.Exit(expire);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add the cookie
        /// </summary>
        /// <param name="name">
        /// The cookie name.
        /// </param>
        /// <param name="value">
        /// The cookie value.
        /// </param>
        public static void Add(string name, object value)
        {
            // is it a string
            if (value is string)
            {
                AddImpl(name, (string)value);
            }
        }

        /// <summary>
        /// Add the cookie
        /// </summary>
        /// <param name="name">
        /// The cookie name.
        /// </param>
        /// <param name="value">
        /// The cookie value.
        /// </param>
        public static void Add(int name, object value)
        {
            // is it a string
            if (value != null && value is string)
            {
                AddImpl(name.ToString(), (string)value);
            }
        }

        /// <summary>
        /// Remove a cookie
        /// </summary>
        /// <param name="name">
        /// The cookie name.
        /// </param>
        public static void Remove(int name)
        {
            RemoveImpl(name.ToString());
        }

        /// <summary>
        /// Remove a Cookie
        /// </summary>
        /// <param name="name">
        /// The cookie name.
        /// </param>
        public static void Remove(string name)
        {
            RemoveImpl(name);
        }

        /// <summary>
        /// Retrieve a cookie
        /// </summary>
        /// <param name="name">
        /// The cookie name.
        /// </param>
        /// <returns>
        /// The retrieve.
        /// </returns>
        public static object Retrieve(int name)
        {
            return RetrieveImpl(name.ToString());
        }

        /// <summary>
        /// Retrieve a Cookie
        /// </summary>
        /// <param name="name">
        /// The cookie name.
        /// </param>
        /// <returns>
        /// The retrieve.
        /// </returns>
        public static object Retrieve(string name)
        {
            return RetrieveImpl(name);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Implementation of the add cookie
        /// </summary>
        /// <param name="name">The cookie name.</param>
        /// <param name="value">The value.</param>
        private static void AddImpl(string name, string value)
        {
            // create cookie
            var hcookie = new HttpCookie(name, value);
            SetCookie(ref hcookie);
        }

        /// <summary>
        /// Clear the cookie
        /// </summary>
        /// <param name="cookie">
        /// The cookie.
        /// </param>
        private static void ClearCookie(ref HttpCookie cookie)
        {
            cookie.Expires = new DateTime(1999, 10, 12);
            cookie.Value = null;

            // HttpContext.Current.Response.Cookies.Remove(cookie.Name);
        }

        /// <summary>
        /// Implemented the remove functionality
        /// </summary>
        /// <param name="name">
        /// The cookie name.
        /// </param>
        private static void RemoveImpl(string name)
        {
            var hcookie = HttpContext.Current.Response.Cookies[name];

            if (hcookie != null)
            {
                // clear the cookie
                ClearCookie(ref hcookie);
            }
        }

        /// <summary>
        /// Implemented the remove functionality
        /// </summary>
        /// <param name="name">
        /// The cookie name.
        /// </param>
        /// <returns>
        /// The retrieve impl.
        /// </returns>
        private static object RetrieveImpl(string name)
        {
            return HttpContext.Current.Request.Cookies[name];
        }

        /// <summary>
        /// Set cookie
        /// </summary>
        /// <param name="cookie">
        /// The cookie.
        /// </param>
        private static void SetCookie(ref HttpCookie cookie)
        {
            // expire in timespan
            cookie.Expires = DateTime.Now + expire;
            cookie.Path = GlobalInternalStrings.CookiePath;

            // see if cookie exists, otherwise create it
            if (HttpContext.Current.Response.Cookies[cookie.Name] != null)
            {
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
            else
            {
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        #endregion
    }
}