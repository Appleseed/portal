namespace Appleseed.Framework.Settings
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using System.Web;

    using Appleseed.Context;

    /// <summary>
    /// This class contains useful information for Extension, Module and Core Developers.
    /// </summary>
    public static class Portal
    {
        #region Constants and Fields

        /// <summary>
        /// The context.
        /// </summary>
        private static Context.Reader context = new Context.Reader(new WebContextReader());

        #endregion

        #region Constructors and Destructors

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the code version.
        /// </summary>
        /// <value>The code version.</value>
        public static int CodeVersion
        {
            get
            {
                return context.Current != null && context.Current.Application["CodeVersion"] != null
                           ? (int)context.Current.Application["CodeVersion"] : 0;
            }
        }

        /// <summary>
        ///     Gets Database connection
        /// </summary>
        /// <value>The connection string.</value>
        [Obsolete("Please use Appleseed.Framework.Settings.Config.ConnectionString")]
        public static string ConnectionString
        {
            get
            {
                return Config.ConnectionString;
            }
        }

        /// <summary>
        ///     Gets the page ID.
        /// </summary>
        /// <value>The page ID.</value>
        public static int PageID
        {
            get
            {
                string strPageId = null;

                return FindPageIdFromQueryString(context.Current.Request.QueryString, ref strPageId) ? Config.GetIntegerFromString(false, strPageId, 0) : 0;
            }
        }

        /// <summary>
        ///     Gets Smtp Server
        /// </summary>
        /// <value>The SMTP server.</value>
        [Obsolete("Please use Appleseed.Framework.Settings.Config.SmtpServer")]
        public static string SmtpServer
        {
            get
            {
                return Config.SmtpServer;
            }
        }

        /// <summary>
        ///     This static string fetches the site's alias either via querystring, cookie or domain and returns it
        /// </summary>
        /// <value>The unique ID.</value>
        public static string UniqueID
        {
            // new version - Jes1111 - 07/07/2005
            get
            {
                try
                {
                    if (context.Current.Items["PortalAlias"] == null)
                    {
                        // not already in context
                        var uniquePortalId = Config.DefaultPortal; // set default value

                        FindAlias(context.Current.Request, ref uniquePortalId); // will change uniquePortalID if it can

                        context.Current.Items.Add("PortalAlias", uniquePortalId); // add to context

                        return uniquePortalId; // return current value
                    }

                    // already in context
                    return (string)context.Current.Items["PortalAlias"]; // return from context
                }
                catch
                {
                    return "Appleseed";
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Finds the alias from cookies.
        /// </summary>
        /// <param name="cookies">
        /// The cookies.
        /// </param>
        /// <param name="alias">
        /// The alias.
        /// </param>
        /// <returns>
        /// The find alias from cookies.
        /// </returns>
        public static bool FindAliasFromCookies(HttpCookieCollection cookies, ref string alias)
        {
            var portalAliasCookie = cookies["PortalAlias"];
            if (portalAliasCookie != null)
            {
                var cookieValue = portalAliasCookie.Value.Trim().ToLower(CultureInfo.InvariantCulture);
                if (cookieValue.Length != 0)
                {
                    alias = cookieValue;
                    return true;
                }
                
                // ErrorHandler.Publish(Appleseed.Framework.LogLevel.Warn, "FindAliasFromCookies failed - PortalAlias found but value was empty.");
                return false;
            }
            
            return false;
        }

        /// <summary>
        /// Finds the alias from query string.
        /// </summary>
        /// <param name="queryString">
        /// The query string.
        /// </param>
        /// <param name="alias">
        /// The alias.
        /// </param>
        /// <returns>
        /// The find alias from query string.
        /// </returns>
        public static bool FindAliasFromQueryString(NameValueCollection queryString, ref string alias)
        {
            if (queryString != null)
            {
                if (queryString["Alias"] != null)
                {
                    var queryStringValues = queryString.GetValues("Alias");
                    var queryStringValue = string.Empty;

                    if (queryStringValues != null)
                    {
                        if (queryStringValues.Length > 0)
                        {
                            queryStringValue = queryStringValues[0].Trim().ToLower(CultureInfo.InvariantCulture);
                        }
                    }

                    if (queryStringValue.Length != 0)
                    {
                        alias = queryStringValue;
                        return true;
                    }
                    
                    // ErrorHandler.Publish(Appleseed.Framework.LogLevel.Warn, "FindAliasFromQueryString failed - Alias param found but value was empty.");
                    return false;
                }
                
                return false;
            }

            return false;
        }

        /// <summary>
        /// Finds the alias from URI.
        /// </summary>
        /// <param name="requestUri">
        /// The request URI.
        /// </param>
        /// <param name="alias">
        /// The alias.
        /// </param>
        /// <param name="defaultPortal">
        /// The default portal.
        /// </param>
        /// <param name="removeDomainPrefixes">
        /// The remove Domain Prefixes.
        /// </param>
        /// <param name="removeTld">
        /// if set to <c>true</c> [remove TLD].
        /// </param>
        /// <param name="secondLevelDomains">
        /// The second level domains.
        /// </param>
        /// <param name="domainPrefixes">
        /// The domain Prefixes.
        /// </param>
        /// <param name="forceFullRemoving">
        /// The force Full Removing.
        /// </param>
        /// <returns>
        /// The find alias from uri.
        /// </returns>
        public static bool FindAliasFromUri(
            Uri requestUri,
            ref string alias,
            string defaultPortal,
            bool removeDomainPrefixes,
            bool removeTld,
            string secondLevelDomains,
            string domainPrefixes,
            bool forceFullRemoving)
        {
            // if request is to localhost, return default portal 
            if (requestUri.IsLoopback)
            {
                alias = defaultPortal;

                return true;
            }

            if (requestUri.HostNameType == UriHostNameType.Dns)
            {
                // get it from hostname
                var hostDelim = new[] { '.' };

                // step 1: split hostname into parts
                var hostPartsList = new ArrayList(requestUri.Host.Split(hostDelim));
                var prefixes = new ArrayList(domainPrefixes.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                var gtoplevelDomains = new ArrayList(
                    secondLevelDomains.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));

                if (forceFullRemoving)
                {
                    // Saco todo 
                    alias = hostPartsList.Cast<string>().Where(
                        s =>
                        !((prefixes.Contains(s) && removeDomainPrefixes) || (gtoplevelDomains.Contains(s) && removeTld)))
                        .Aggregate(string.Empty, (current, s) => current + s);
                }
                else
                {
                    // step 2: do we need to remove "www"?
                    if (removeDomainPrefixes && prefixes.Contains(hostPartsList[0].ToString()))
                    {
                        hostPartsList.RemoveAt(0);
                    }

                    // step 3: do we need to remove TLD?
                    if (removeTld)
                    {
                        hostPartsList.Reverse();
                        if (hostPartsList.Count > 2 && hostPartsList[0].ToString().Length == 2)
                        {
                            // this is a ccTLD, so need to check if next segment is a pseudo-gTLD                            
                            if (gtoplevelDomains.Contains(hostPartsList[1].ToString()))
                            {
                                hostPartsList.RemoveRange(0, 2);
                            }
                            else
                            {
                                hostPartsList.RemoveAt(0);
                            }
                        }
                        else
                        {
                            hostPartsList.RemoveAt(0);
                        }

                        hostPartsList.Reverse();
                    }

                    // step 4: re-assemble the remaining parts
                    alias = string.Join(".", (string[])hostPartsList.ToArray(typeof(string)));
                }

                return true;
            }

            alias = defaultPortal;
            return true;
        }

        /// <summary>
        /// Finds the page id from query string.
        /// </summary>
        /// <param name="queryString">
        /// The query string.
        /// </param>
        /// <param name="pageId">
        /// The page ID.
        /// </param>
        /// <returns>
        /// The find page id from query string.
        /// </returns>
        public static bool FindPageIdFromQueryString(NameValueCollection queryString, ref string pageId)
        {
            string[] queryStringValues;

            // tabID = 240
            if (queryString != null)
            {
                if (queryString[GlobalInternalStrings.str_PageID] != null)
                {
                    queryStringValues = queryString.GetValues(GlobalInternalStrings.str_PageID);
                }
                else if (queryString[GlobalInternalStrings.str_TabID] != null)
                {
                    queryStringValues = queryString.GetValues(GlobalInternalStrings.str_TabID);
                }
                else
                {
                    return false;
                }

                var queryStringValue = string.Empty;

                if (queryStringValues != null && queryStringValues.Length > 0)
                {
                    queryStringValue = queryStringValues[0].Trim().ToLower(CultureInfo.InvariantCulture);
                }

                if (queryStringValue.Length != 0)
                {
                    pageId = queryStringValue;
                    return true;
                }

                // ErrorHandler.Publish(Appleseed.Framework.LogLevel.Warn, "FindPageIDFromQueryString failed - Alias param found but value was empty.");
                return false;
            }

            return false;
        }

        /// <summary>
        /// Sets reader for context in this class
        /// </summary>
        /// <param name="reader">
        /// an instance of a Concrete Strategy Reader
        /// </param>
        public static void SetReader(Context.Reader reader)
        {
            context = reader;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finds the alias.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="alias">
        /// The alias.
        /// </param>
        private static void FindAlias(HttpRequest request, ref string alias)
        {
            if (FindAliasFromQueryString(request.QueryString, ref alias))
            {
                return;
            }

            if (FindAliasFromCookies(request.Cookies, ref alias))
            {
                return;
            }

            FindAliasFromUri(
                request.Url,
                ref alias,
                Config.DefaultPortal,
                Config.RemoveDomainPrefixes,
                Config.RemoveTLD,
                Config.SecondLevelDomains,
                Config.DomainPrefixes,
                Config.ForceFullRemoving);
        }

        #endregion
    }
}