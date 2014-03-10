using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Web;
using Appleseed.Framework.Settings.Cache;

namespace Appleseed.Framework.Settings
{
	/// <summary>
	/// Static class for 'safe' retrieval of AppSettings from web.config file. 
	/// All members return a default value. Some members validate the value found and 
	/// return the default value if found value is invalid. (jes1111)
	/// </summary>
	public sealed class Config
	{
		// isolates Config reader for testing purposes
		private static Reader config = new Reader(new ConfigReader());

		/// <summary>
		/// Constructor
		/// </summary>
		private Config()
		{
			//SetReader();
		}

		#region public static methods

		/// <summary>
		/// Sets reader for all Get... methods in this class
		/// </summary>
		/// <param name="reader">an instance of a Concrete Strategy Reader</param>
		public static void SetReader(Reader reader)
		{
			config = reader;
		}

		/// <summary>
		/// Converts string value to integer value, returning parsed signed integer. 
		/// Allows integer values between -999,999,999 and 999,999,999. Returns defaultValue if
		/// settingValue is not legitimate equivalent.
		/// </summary>
		/// <param name="settingValue">setting value</param>
		/// <param name="defaultValue">default value</param>
		/// <param name="allowNegative">allow or disallow negative integers</param>
		/// <returns>an integer value</returns>
		public static int GetIntegerFromString(bool allowNegative, string settingValue, int defaultValue)
		{
			const int minChar = 48;
			const int maxChar = 57;
			const int hyphenMinus = 45;
			int isNegative;

			if (settingValue != null && settingValue.Length != 0)
			{
				settingValue = settingValue.Trim();

				if (allowNegative && settingValue[0] == hyphenMinus)
					isNegative = 1;
				else
					isNegative = 0;

				int adjustedLength = settingValue.Length - isNegative;

				if (adjustedLength == 0 || adjustedLength > 9)
					return defaultValue;

				for (int i = isNegative; i < settingValue.Length; i++)
				{
					if (settingValue[i] > maxChar || settingValue[i] < minChar)
						return defaultValue; // return defaultValue
				}
				return Int32.Parse(settingValue); // return parsed int
			}
			return defaultValue; // return defaultValue
		}

		/// <summary>
		/// Gets value from configured Reader for specified key, returning parsed signed integer. 
		/// Allows integer values between -99,999,999 and 999,999,999. If key is not present, returns defaultValue.
		/// </summary>
		/// <param name="key">setting key</param>
		/// <param name="defaultValue">setting value</param>
		/// <param name="allowNegative">allow or disallow negative integers</param>
		/// <returns>an integer value</returns>
		public static int GetInteger(string key, int defaultValue, bool allowNegative)
		{
			return GetIntegerFromString(allowNegative, config.GetAppSetting(key), defaultValue);
		}

		/// <summary>
		/// Gets value from configured Reader for specified key, returning parsed integer. 
		/// Allows integer values between 0 and 999,999,999. If key is not present, returns defaultValue.
		/// </summary>
		/// <param name="key">setting key</param>
		/// <param name="defaultValue">setting value</param>
		/// <returns>an integer value</returns>
		public static int GetInteger(string key, int defaultValue)
		{
			return GetInteger(key, defaultValue, false);
		}

		/// <summary>
		/// Gets value from configyred Reader for specified key, returning string.
		/// If key is not present, returns defaultValue. Empty string is an allowable value.
		/// </summary>
		/// <param name="key">collection key</param>
		/// <param name="defaultValue">default value (can be empty string or null)</param>
		/// <returns>setting value or defaultValue</returns>
		public static string GetString(string key, string defaultValue)
		{
			return GetString(key, defaultValue, true);
		}

		/// <summary>
		/// Gets value from configured reader for specified key, returning string.
		/// If key is not present, returns defaultValue. Boolean parameter controls whether
		/// empty string is an allowable value.
		/// </summary>
		/// <param name="key">collection key</param>
		/// <param name="defaultValue">default value (can be empty string or null)</param>
		/// <param name="allowEmpty">allow or disallow return of empty string from setting item</param>
		/// <returns>setting value or defaultValue</returns>
		public static string GetString(string key, string defaultValue, bool allowEmpty)
		{
			return GetStringInternal(allowEmpty, config.GetAppSetting(key), defaultValue);
		}

		/// <summary>
		/// Returns string value or defaultValue.
		/// </summary>
		/// <param name="allowEmpty">allow empty string as settingValue</param>
		/// <param name="settingValue">settingValue</param>
		/// <param name="defaultValue">defaultValue</param>
		/// <returns>settingValue or defaultValue</returns>
		private static string GetStringInternal(bool allowEmpty, string settingValue, string defaultValue)
		{
			if (settingValue != null)
			{
				if (settingValue.Length != 0 || allowEmpty)
					return settingValue;
			}
			return defaultValue;
		}

		/// <summary>
		/// Gets value from configured Reader for specified key, returning boolean.
		/// If key is not present, returns defaultValue. 
		/// </summary>
		/// <param name="key">collection key</param>
		/// <param name="defaultValue">default value</param>
		/// <returns>setting value or default value</returns>
		public static bool GetBoolean(string key, bool defaultValue)
		{
			return GetBooleanInternal(defaultValue, config.GetAppSetting(key));
		}

		/// <summary>
		/// Converts string value to boolean. Returns defaultValue if
		/// settingValue is not legitimate equivalent.
		/// </summary>
		/// <param name="settingValue">settingValue</param>
		/// <param name="defaultValue">default value</param>
		/// <returns>setting value or default value</returns>
		private static bool GetBooleanInternal(bool defaultValue, string settingValue)
		{
			if (settingValue != null)
			{
				if (string.Compare(settingValue, Boolean.TrueString, true, CultureInfo.InvariantCulture) == 0)
					return true;

				if (string.Compare(settingValue, Boolean.FalseString, true, CultureInfo.InvariantCulture) == 0)
					return false;

				settingValue = settingValue.Trim();

				if (string.Compare(settingValue, Boolean.TrueString, true, CultureInfo.InvariantCulture) == 0)
					return true;

				if (string.Compare(settingValue, Boolean.FalseString, true, CultureInfo.InvariantCulture) == 0)
					return false;

			}
			return defaultValue;
		}

		/// <summary>
		/// Gets value from configured Reader for specified key, returning HttpStatusCode enum.
		/// If key is not present or value is not equivalent to valid HttpStatusCode, returns defaultValue. 
		/// </summary>
		/// <param name="key">collection key</param>
		/// <param name="defaultValue">default value</param>
		/// <returns>setting value or default value</returns>
		public static HttpStatusCode GetHttpStatusCode(string key, HttpStatusCode defaultValue)
		{
			string settingValue;

			if (key != null && key.Length != 0)
				settingValue = config.GetAppSetting(key);
			else
				settingValue = null;

			if (settingValue != null)
			{
				if (Enum.IsDefined(typeof (HttpStatusCode), settingValue))
					return (HttpStatusCode) Enum.Parse(typeof (HttpStatusCode), settingValue, false);
			}
			return defaultValue;
		}

		/// <summary>
		/// Converts a string to a URL by trimming it, replacing '~' with Application Root and URL Encoding it
		/// </summary>
		/// <param name="item">The string to be converted</param>
		/// <returns>the converted string</returns>
		public static string ToUrl(string item)
		{
			item = item.Trim();

			if (item.StartsWith("~"))
				item = Path.ApplicationRootPath(item.TrimStart(new char[] {'~'}));

			return HttpUtility.UrlEncode(item);
		}

		#endregion

		#region public static members

		/// <summary>
		/// The default portal alias
		/// <br/>
		/// Default value: "Appleseed"</summary>
		public static string DefaultPortal
		{
			get { return GetString("DefaultPortal", "Appleseed", false).Trim().ToLower(CultureInfo.InvariantCulture); }
		}

		/// <summary>
		/// Enables multiple databases (i.e. 1 per portal on a single codebase)
		/// <br/>
		/// Default value: false</summary>
		public static bool EnableMultiDbSupport
		{
			get { return GetBoolean("EnableMultiDbSupport", false); }
		}

		/// <summary>
		/// The database connection string - checks for EnableMultiDbSupport and returns the
		/// correct connection string for this portal.
		/// <br/>
		/// Default value: "server=localhost;database=Appleseed;uid=sa;pwd="</summary>
		public static string ConnectionString
		{
			get
			{
                // TODO: ENABLE Multi DB SUpport?
                
				string keyConnection = String.Concat(Portal.UniqueID, "_ConnectionString");
				string siteConnectionString;

				// check cache first
				if (!CurrentCache.Exists(keyConnection)) // not in cache
				{
					if (EnableMultiDbSupport)
						// look in web.config for key="[uniqueID]_ConnectionString", default to key="ConnectionString"
						siteConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
					else 
						// look in web.config for key="ConnectionString"
						siteConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
					// add to cache
					CurrentCache.Insert(keyConnection, siteConnectionString);
					// return the right connection string for this portal
					return siteConnectionString;
				}
				else // already cached
				{
					// return cached value
					return (string) CurrentCache.Get(keyConnection);
				}
			}
		}

        /// <summary>
        /// Returns a new SqlConnection object using current ConnectionString
        /// </summary>
        public static SqlConnection SqlConnectionString
        {
            get
            {
                SqlConnection myConnection = new SqlConnection();


                try
                {
                    myConnection.ConnectionString = ConnectionString;                  
                }
                catch (System.ArgumentException ex) //connectionstring not well formed
                {
                    //redirect to installer
                    ErrorHandler.Publish(LogLevel.Error, "Connection string not well formed. Redirecting to Install: " + ex.Message);
                    HttpContext.Current.Response.Redirect(InstallerRedirect);
                }
                return myConnection;
            }
        }


		/// <summary>
		/// Removes "www." when attempting to derive alias from hostname
		/// <br/>
		/// Default value: true</summary>
		public static bool RemoveDomainPrefixes
		{
            get { return GetBoolean("RemoveDomainPrefixes", true); }
		}

		/// <summary>
		/// Removes one- and two-part TLDs when attempting to derive alias from hostname
		/// <br/>
		/// Default value: true</summary>
		public static bool RemoveTLD
		{
			get { return GetBoolean("RemoveTLD", true); }
		}

        /// <summary>
        /// List of possible domain prefixes to use for removing when attempting to derive alias from hostname
        /// </summary>
        public static string DomainPrefixes
        {
            get { return GetString("DomainPrefixes", "www;local", false); }
        }

		/// <summary>
		/// List of possible second level domains to use for removing two-part TLDs when attempting to derive alias from hostname
		/// <br/>
		/// Default value: "aero;biz;com;coop;info;museum;name;net;org;pro;gov;edu;mil;int;co;ac;sch;nhs;police;mod;ltd;plc;me"</summary>
		public static string SecondLevelDomains
		{
			get { return GetString("SecondLevelDomains", "aero;biz;com;coop;info;museum;name;net;org;pro;gov;edu;mil;int;co;ac;sch;nhs;police;mod;ltd;plc;me", false); }
		}

		/// <summary>
		/// (Optional) Default PromoCode for Amazon module
		/// <br/>
		/// Default value: empty string</summary>
		public static string AmazonPromoCode
		{
			get { return GetString("AmazonPromoCode", string.Empty, true); }
		}

		/// <summary>
		/// (Optional) Default DevToken for Amazon module
		/// <br/>
		/// Default value: empty string</summary>
		public static string AmazonDevToken
		{
			get { return GetString("AmazonDevToken", string.Empty, true); }
		}

		/// <summary>
		/// (Optional) Email address for webMaster (used in RSS feed)
		/// <br/>
		/// Default value: empty string</summary>
		public static string WebMaster
		{
			get { return GetString("WebMaster", string.Empty, true); }
		}

		/// <summary>
		/// (Optional) ConnectionString for PortalTemplates server
		/// <br/>
		/// Default value: empty string</summary>
		public static string PortalTemplatesConnectionString
		{
			get { return GetString("PortalTemplatesConnectionString", string.Empty, true); }
		}

		/// <summary>
		/// (Optional) Folder for Quotes files (used by Quote module)
		/// <br/>
		/// Default value: empty string</summary>
		public static string QuoteFileFolder
		{
			get { return GetString("QuoteFileFolder", string.Empty, true); }
		}

		/// <summary>
		/// Enables Scheduler
		/// <br/>
		/// Default value: false</summary>
		public static bool SchedulerEnable
		{
			get { return GetBoolean("SchedulerEnable", false); }
		}

		/// <summary>
		/// Enables WebCompile
		/// <br/>
		/// Default value: false</summary>
		public static bool EnableWebCompile
		{
			get { return GetBoolean("EnableWebCompile", false); }
		}

		/// <summary>
		/// Cache size for Scheduler
		/// <br/>
		/// Default value: 100</summary>
		public static int SchedulerCacheSize
		{
			get { return GetInteger("SchedulerCacheSize", 100); }
		}

		/// <summary>
		/// Period for Scheduler
		/// <br/>
		/// Default value: 60000</summary>
		public static int SchedulerPeriod
		{
			get { return GetInteger("SchedulerPeriod", 60000); }
		}

		/// <summary>
		/// SMTP Server for sending emails from modules, registration, etc.
		/// (Set value to empty string for cluster server support).
		/// <br/>
		/// Default value: "localhost"</summary>
		public static string SmtpServer
		{
			get { return GetString("SmtpServer", "localhost", true); }
		}

		/// <summary>
		/// Email "From" address for sending emails from modules, registration, etc.
		/// <br/>
		/// Default value: "portal@localhost.com"</summary>
		public static string EmailFrom
		{
			get { return GetString("EmailFrom", "portal@localhost.com", false); }
		}

		/// <summary>
		/// Email "Destinatary" address for sending emails on error case.
		/// <br/>
		/// Default value: "portal@localhost.com"</summary>
        public static string ErrorMailDestinatary
		{
            get { return GetString("ErrorMailDestinatary", "portal@localhost.com", false); }
		}
		/// <summary>
		/// Enables Password Encryption
		/// <br/>
		/// Default value: false</summary>
		public static bool EncryptPassword
		{
			get { return GetBoolean("EncryptPassword", false); }
		}

		/// <summary>
		/// URL for SmartError page
		/// <br/>
		/// Default value: "~/app_support/SmartError.aspx"</summary>
		public static string SmartErrorRedirect
		{
			get { return GetString("SmartErrorRedirect", "~/app_support/SmartError.aspx", false); }
		}

		/// <summary>
		/// URL for redirect when invalid Alias is detected in request
		/// <br/>
		/// Default value: "~/app_support/SmartError.aspx"</summary>
		public static string InvalidAliasRedirect
		{
			get { return GetString("InvalidAliasRedirect", "~/app_support/SmartError.aspx", false); }
		}

		/// <summary>
		/// URL for redirect when invalid PageID is detected in request
		/// <br/>
		/// Default value: "~/app_support/SmartError.aspx"</summary>
		public static string InvalidPageIdRedirect
		{
			get { return GetString("InvalidPageIdRedirect", "~/app_support/SmartError.aspx", false); }
		}

		/// <summary>
		/// Determines how 'wrong' a request can be, i.e. allow invalid PageID, etc.
		/// Returns an integer between 1 and 4.
		/// See http://support.Appleseedportal.net/confluence/display/DOX/Changes+in+Global.asax.cs
		/// <br/>
		/// Default value: 3</summary>
		public static int UrlToleranceLevel
		{
			get
			{
				int returnValue = GetInteger("UrlToleranceLevel", 3);
				return returnValue < 1 ? 1 : returnValue > 4 ? 4 : returnValue; // make sure number is between 1 and 4
			}
		}

		/// <summary>
		/// HTTP Status code to use when redirecting to Database Update page
		/// <br/>
		/// Default value: HttpStatusCode.ServiceUnavailable (503)</summary>
		public static HttpStatusCode DatabaseUpdateResponse
		{
			get { return GetHttpStatusCode("DatabaseUpdateResponse", HttpStatusCode.ServiceUnavailable); }
		}

		/// <summary>
		/// URL for redirect to Database Update page
		/// <br/>
		/// Default value: "~/Setup/Update.aspx"</summary>
        //public static string DatabaseUpdateRedirect
        //{
        //    get { return GetString("DatabaseUpdateRedirect", "~/Installer/Update.aspx", false); }
        //}

        /// <summary>
        /// URL for redirect to Installer page
        /// <br/>
        /// Default value: "~/Installer/default.aspx"</summary>
        public static string InstallerRedirect
        {
            get { return GetString("InstallerRedirect", "~/Installer/default.aspx", false); }
        }

		/// <summary>
		/// HTTP Status code to use when redirecting to Database Error Page
		/// <br/>
		/// Default value: HttpStatusCode.ServiceUnavailable (503)</summary>
		public static HttpStatusCode DatabaseErrorResponse
		{
			get { return GetHttpStatusCode("DatabaseErrorResponse", HttpStatusCode.ServiceUnavailable); }
		}

		/// <summary>
		/// URL for redirect on Database Error
		/// <br/>
		/// Default value: "~/app_support/GeneralError.html"</summary>
		public static string DatabaseErrorRedirect
		{
			get { return GetString("DatabaseErrorRedirect", "~/app_support/GeneralError.aspx", false); }
		}

		/// <summary>
		/// HTTP Status code to use when redirecting to Code Update page
		/// <br/>
		/// Default value: HttpStatusCode.ServiceUnavailable (503)</summary>
		public static HttpStatusCode CodeUpdateResponse
		{
			get { return GetHttpStatusCode("CodeUpdateResponse", HttpStatusCode.ServiceUnavailable); }
		}

		/// <summary>
		/// URL for redirect on Code Update
		/// <br/>
		/// Default value: "~/app_support/GeneralError.html"</summary>
		public static string CodeUpdateRedirect
		{
			get { return GetString("CodeUpdateRedirect", "~/app_support/GeneralError.aspx", false); }
		}

		/// <summary>
		/// HTTP Status code to use when redirecting on Critical Error
		/// <br/>
		/// Default value: HttpStatusCode.ServiceUnavailable (503)</summary>
		public static HttpStatusCode CriticalErrorResponse
		{
			get { return GetHttpStatusCode("CriticalErrorResponse", HttpStatusCode.ServiceUnavailable); }
		}

		/// <summary>
		/// URL for redirect on Critical Error
		/// <br/>
		/// Default value: "~/app_support/GeneralError.html"</summary>
		public static string CriticalErrorRedirect
		{
            get { return GetString("CriticalErrorRedirect", "~/app_support/GeneralError.aspx", false); }
		}

		/// <summary>
		/// HTTP Status code to use when redirecting on NoPortal Error
		/// <br/>
		/// Default value: HttpStatusCode.NotFound (404)</summary>
		public static HttpStatusCode NoPortalErrorResponse
		{
			get { return GetHttpStatusCode("NoPortalErrorResponse", HttpStatusCode.NotFound); }
		}

		/// <summary>
		/// URL for redirect on NoPortal Error
		/// <br/>
		/// Default value: "~/app_support/GeneralError.html"</summary>
		public static string NoPortalErrorRedirect
		{
			get { return GetString("NoPortalErrorRedirect", "~/app_support/smartError.aspx", false); }
		}

		/// <summary>
		/// Locks all portals - redirects non-KeyHolders to LockRedirect page
		/// <br/>
		/// Default value: false</summary>
		public static bool LockAllPortals
		{
			get { return GetBoolean("LockAllPortals", false); }
		}

		/// <summary>
		/// Semi-colon delimited list of IP addresses which are "key holders" - key holders are
		/// allowed into a locked portal and are shown full error details on SmartError page
		/// <br/>
		/// Default value: "127.0.0.1"</summary>
		public static string LockKeyHolders
		{
			get { return GetString("LockKeyHolders", "127.0.0.1", false); }
		}

		/// <summary>
		/// HTTP Status code to use when redirecting to LockRedirect page
		/// <br/>
		/// Default value: HttpStatusCode.ServiceUnavailable (503)</summary>
		public static HttpStatusCode LockResponse
		{
			get { return GetHttpStatusCode("LockResponse", HttpStatusCode.ServiceUnavailable); }
		}

		/// <summary>
		/// URL for redirect on "All Portals Locked"
		/// <br/>
		/// Default value: "~/app_support/GeneralError.html"</summary>
		public static string LockRedirect
		{
			get { return GetString("LockRedirect", "~/app_support/GeneralError.aspx", false); }
		}

		/// <summary>
		/// Time in minutes for cookie expiration
		/// <br/>
		/// Default value: 60</summary>
		public static int CookieExpire
		{
			get { return GetInteger("CookieExpire", 60); }
		}

		/// <summary>
		/// If true, cookie will not renew itself, and force login 
		/// every 'CookieExpire' minutes
		/// <br/>
		/// Default value: false</summary>
		public static bool ForceExpire
		{
			get { return GetBoolean("ForceExpire", false); }
		}

		/// <summary>
		/// Enables check for correct file permissions for ASPNET user on application start
		/// <br/>
		/// Default value: false</summary>
		public static bool CheckForFilePermission
		{
			get { return GetBoolean("CheckForFilePermission", false); }
		}

		/// <summary>
		/// Enables use of proxy for Web Requests
		/// <br/>
		/// Default value: false</summary>
		public static bool UseProxyServerForServerWebRequests
		{
			get { return GetBoolean("UseProxyServerForServerWebRequests", false); }
		}

		/// <summary>
		/// Enables Window Management on modules (min, max, etc.)
		/// <br/>
		/// Default value: false</summary>
		public static bool WindowMgmtControls
		{
			get { return GetBoolean("WindowMgmtControls", false); }
		}

		/// <summary>
		/// Enables Window Management 'close' function on modules
		/// <br/>
		/// Default value: false</summary>
		public static bool WindowMgmtWantClose
		{
			get { return GetBoolean("WindowMgmtWantClose", false); }
		}

		/// <summary>
		/// Enables Monitoring
		/// <br/>
		/// Default value: false</summary>
		public static bool EnableMonitoring
		{
			get { return GetBoolean("EnableMonitoring", false); }
		}

		/// <summary>
		/// Enables use of DesktopPagesXml in PortalSettings (for navigation etc.)
		/// <br/>
		/// Default value: false</summary>
		public static bool PortalSettingDesktopPagesXml
		{
			get { return GetBoolean("PortalSettingDesktopPagesXml", false); }
		}

		/// <summary>
		/// (Optional) String to prefix all page titles
		/// <br/>
		/// Default value: empty string</summary>
		public static string PortalTitlePrefix
		{
			get { return GetString("PortalTitlePrefix", string.Empty); }
		}

		/// <summary>
		/// The folder in which portal data directories are located
		/// <br/>
		/// Default value: "Portals"</summary>
		public static string PortalsDirectory
		{
			//an empty directory is allowed
			get { return GetString("PortalsDirectory", "Portals", true); }
		}

		/// <summary>
		/// The secure directory for Ecommerce system
		/// <br/>
		/// Default value: "ECommerce/Secure"</summary>
		public static string PortalSecureDirectory
		{
			get { return GetString("PortalSecureDirectory", "ECommerce/Secure", false); }
		}

		/// <summary>
		/// ActiveDirectory Administrator Group
		/// <br/>
		/// Default value: "MyDomain\Administrators"</summary>
		public static string ADAdministratorGroup
		{
			get { return GetString("ADAdministratorGroup", @"MyDomain\Administrators", false); }
		}

		/// <summary>
		/// ActiveDirectory DNS
		/// <br/>
		/// Default value: "LDAP://DomainControllerName/DC=MyDomain, DC=com; WinNT://MyDomain"</summary>
		public static string ADdns
		{
			get { return GetString("ADdns", @"LDAP://DomainControllerName/DC=MyDomain, DC=com; WinNT://MyDomain", false); }
		}

		/// <summary>
		/// Default DOCTYPE for Zen pages
		/// <br/>
		/// Default value: "&lt;!DOCTYPE HTML PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN' &gt;"</summary>
		public static string DefaultDOCTYPE
		{
			get { return GetString("DefaultDOCTYPE", @"&lt;!DOCTYPE HTML PUBLIC '-//W3C//DTD HTML 4.01 Transitional//EN' &gt;", false); }
		}

		/// <summary>
		/// Script call for IE7, used for Zen
		/// <br/>
		/// Default value: "/aspnet_client/ie7-08a/ie7-standard-p.js"</summary>
		public static string Ie7Script
		{
			get { return GetString("Ie7Script", @"/aspnet_client/ie7-08a/ie7-standard-p.js", false); }
		}

		/// <summary>
		/// LDAP Administrator Group
		/// <br/>
		/// Default value: empty string</summary>
		public static string LDAPAdministratorGroup
		{
			get { return GetString("LDAPAdministratorGroup", string.Empty, true); }
		}

		/// <summary>
		/// LDAP Login
		/// <br/>
		/// Default value: empty string</summary>
		public static string LDAPLogin
		{
			get { return GetString("LDAPLogin", string.Empty, true); }
		}

		/// <summary>
		/// LDAP Server
		/// <br/>
		/// Default value: empty string</summary>
		public static string LDAPServer
		{
			get { return GetString("LDAPServer", string.Empty, true); }
		}

		/// <summary>
		/// LDAP Group
		/// <br/>
		/// Default value: empty string</summary>
		public static string LDAPGroup
		{
			get { return GetString("LDAPGroup", string.Empty, true); }
		}

		/// <summary>
		/// LDAP Contexts
		/// <br/>
		/// Default value: empty string</summary>
		public static string LDAPContexts
		{
			get { return GetString("LDAPContexts", string.Empty, true); }
		}

        /// <summary>
        /// Default language
        /// <br/>
        /// Default value: "en-US"</summary>
        public static string DefaultLanguage
        {
            get { return GetString("DefaultLanguage", "en-US", false); }
        }

        /// <summary>
        /// Default language list
        /// <br/>
        /// Default value: "en;es;ar;bg;ca;cs;da;de;el;en-GB;en-US;es-ES;es-MX;fr;fr-FR;he;hi;hr;is;it;ja;ko;nl;nl-BE;no;pl;pt;ru;sl;sv;tr;uk;vi;zh-CN;zh-TW;"</summary>
        public static string DefaultLanguageList
        {
            get { return GetString("DefaultLanguageList", "en;es;ar;bg;ca;cs;da;de;el;en-GB;en-US;es-ES;es-MX;fr;fr-FR;he;hi;hr;is;it;ja;ko;nl;nl-BE;no;pl;pt;ru;sl;sv;tr;uk;vi;zh-CN;zh-TW;", false); }
        }

		/// <summary>
		/// (Optional) Username for login to Update page
		/// <br/>
		/// Default value: empty string</summary>
		public static string UpdateUserName
		{
			get { return GetString("UpdateUserName", string.Empty, true); }
		}

		/// <summary>
		/// (Optional) Password for login to Update page
		/// <br/>
		/// Default value: empty string</summary>
		public static string UpdatePassword
		{
			get { return GetString("UpdatePassword", string.Empty, true); }
		}

		/// <summary>
		/// (Optional) Address for Proxy Server to use for Web Requests
		/// <br/>
		/// Default value: "http://127.0.0.1"</summary>
		public static string ProxyServer
		{
			get { return GetString("ProxyServer", "http://127.0.0.1", false); }
		}

		/// <summary>
		/// (Optional) UserID for Proxy Server to use for Web Requests
		/// <br/>
		/// Default value: empty string</summary>
		public static string ProxyUserID
		{
			get { return GetString("ProxyUserID", string.Empty, true); }
		}

		/// <summary>
		/// (Optional) Password for Proxy Server to use for Web Requests
		/// <br/>
		/// Default value: empty string</summary>
		public static string ProxyPassword
		{
			get { return GetString("ProxyPassword", string.Empty, true); }
		}

		/// <summary>
		/// (Optional) Domain for Proxy Server to use for Web Requests
		/// <br/>
		/// Default value: empty string</summary>
		public static string ProxyDomain
		{
			get { return GetString("ProxyDomain", string.Empty, true); }
		}

		/// <summary>
		/// ActiveDirectory UserName
		/// <br/>
		/// Default value: empty string</summary>
		public static string ADUserName
		{
			get { return GetString("ADUserName", string.Empty, true); }
		}

		/// <summary>
		/// ActiveDirectory Password
		/// <br/>
		/// Default value: empty string</summary>
		public static string ADUserPassword
		{
			get { return GetString("ADUserPassword", string.Empty, true); }
		}

		/// <summary>
		/// Enables ActiveDirectory usage
		/// <br/>
		/// Default value: false</summary>
		public static bool EnableADUser
		{
			get { return GetBoolean("EnableADUser", false); }
		}

		/// <summary>
		/// Forces all portals to share Portal 0 user table
		/// <br/>
		/// Default value: false</summary>
		public static bool UseSingleUserBase
		{
			get { return GetBoolean("UseSingleUserBase", false); }
		}

		/// <summary>
		/// Sets default Module Cache time (in seconds) - value here overrides any (cacheable) modules with
		/// local setting of 0
		/// <br/>
		/// Default value: 0</summary>
		public static int ModuleOverrideCache
		{
			get { return GetInteger("ModuleOverrideCache", 0); }
		}

		/// <summary>
		/// (Optional) Specifies folder containing XSL files (used by XmlFeed module)
		/// <br/>
		/// Default value: empty string</summary>
		public static string XMLFeedXSLFolder
		{
			get { return GetString("XMLFeedXSLFolder", string.Empty, true); }
		}

		/// <summary>
		/// Using grouping tabs to display module settings or not
		/// <br/>
		/// Default value: false
		/// </summary>
		public static bool UseSettingsGroupingTabs
		{
			get { return GetBoolean("UseSettingsGroupingTabs", false); }
		}

		/// <summary>
		/// Width of SettingsTable contrl when using grouping tabs to 
		/// display module settings
		/// <br/>
		/// Default value: 600
		/// </summary>
		public static int SettingsGroupingWidth
		{
			get { return GetInteger("SettingsGroupingWidth", 600); }
		}

		/// <summary>
		/// Height of SettingsTable contrl when using grouping tabs to 
		/// display module settings
		/// <br/>
		/// Default value: 350
		/// </summary>
		public static int SettingsGroupingHeight
		{
			get { return GetInteger("SettingsGroupingHeight", 350); }
		}


        /// <summary>
        /// Indicates if all tokens in topleveldomains and/or secondleveldomains 
        /// must be removed from url.
        /// </summary>
        public static bool ForceFullRemoving
		{
            get { return GetBoolean("ForceFullRemoving", false); }
		}

		#endregion
	}
}
