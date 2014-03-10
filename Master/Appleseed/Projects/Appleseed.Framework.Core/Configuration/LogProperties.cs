// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogProperties.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The log code version property.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Logging
{
    using System.Web;

    using Appleseed.Framework.Settings;

    /// <summary>
    /// The log code version property.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class LogCodeVersionProperty
    {
        #region Public Methods

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override string ToString()
        {
            try
            {
                return Portal.CodeVersion.ToString();
            }
            catch
            {
                return "not available";
            }
        }

        #endregion
    }

    /// <summary>
    /// The log user name property.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class LogUserNameProperty
    {
        #region Public Methods

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override string ToString()
        {
            try
            {
                return HttpContext.Current.User.Identity.Name;
            }
            catch
            {
                return "not available";
            }
        }

        #endregion
    }

    /// <summary>
    /// The log rewritten url property.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class LogRewrittenUrlProperty
    {
        #region Public Methods

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override string ToString()
        {
            try
            {
                return HttpContext.Current == null
                           ? "not available"
                           : HttpContext.Current.Server.HtmlDecode(HttpContext.Current.Request.Url.ToString());
            }
            catch
            {
                return "not available";
            }
        }

        #endregion
    }

    /// <summary>
    /// The log user agent property.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class LogUserAgentProperty
    {
        #region Public Methods

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override string ToString()
        {
            try
            {
                return HttpContext.Current.Request.UserAgent;
            }
            catch
            {
                return "not available";
            }
        }

        #endregion
    }

    /// <summary>
    /// The log user languages property.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class LogUserLanguagesProperty
    {
        #region Public Methods

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override string ToString()
        {
            try
            {
                return string.Join(";", HttpContext.Current.Request.UserLanguages);
            }
            catch
            {
                return "not available";
            }
        }

        #endregion
    }

    /// <summary>
    /// The log user ip property.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class LogUserIpProperty
    {
        #region Public Methods

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override string ToString()
        {
            try
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            catch
            {
                return "not available";
            }
        }

        #endregion
    }

    /// <summary>
    /// The portal alias property.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class PortalAliasProperty
    {
        #region Public Methods

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public override string ToString()
        {
            try
            {
                return Portal.UniqueID;
            }
            catch
            {
                return "not available";
            }
        }

        #endregion
    }
}