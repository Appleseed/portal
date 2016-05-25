// Created by John Mandia (john.mandia@whitelightsolutions.com)
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using Appleseed.Framework.Settings;
using System.Text;

namespace Appleseed.Framework.Web
{
	/// <summary>
	/// Summary description for Helper.
	/// </summary>
    [History("Ashish.patel@haptix.biz", "2014/12/24", "Changed Accessibility of GetUrlElements for caching")]
	public sealed class UrlBuilderHelper
	{
		/// <summary>
		///     ctor
		/// </summary>
		/// 
		/// <returns>
		///     A void value...
		/// </returns>
		private UrlBuilderHelper()
		{
		}

        /// <summary>
        /// Placeholder id
        /// </summary>
		public const string IsPlaceHolderID = "TabPlaceholder";

        /// <summary>
        /// tab link
        /// </summary>
		public const string TabLinkID = "TabLink";

        /// <summary>
        /// Page name
        /// </summary>
		public const string PageNameID = "UrlPageName";

        /// <summary>
        /// Url keyword id
        /// </summary>
		public const string UrlKeywordsID = "TabUrlKeyword";

        /// <summary>
        /// Page title 
        /// </summary>
        public const string PageTitleID = "UrlPageTitle";

        /// <summary>
        /// Get url elements query
        /// </summary>
        private static string GetUrlElementsQuery = @"SELECT ISNULL((SELECT SettingValue FROM rb_TabSettings WHERE TabID=@PageID AND SettingName = 'UrlPageName'),'') as PageSEOName,
                                                            ISNULL((SELECT PageName FROM rb_Pages WHERE PageID=@PageID ),'') as PageTitle, 
                                                            ISNULL((SELECT SettingValue FROM rb_TabSettings WHERE TabID=@PageID AND SettingName = 'TabUrlKeyword'),'') as Keywords,
                                                            ISNULL((SELECT SettingValue FROM rb_TabSettings WHERE TabID=@PageID AND SettingName = 'TabLink'),'') as ExternalLink,
                                                            ISNULL((SELECT SettingValue FROM rb_TabSettings WHERE TabID=@PageID AND SettingName = 'TabPlaceholder'),'') as IsPlaceHolder";

		/// <summary>
		/// Builds up a cache key for Url Elements/Properties
		/// </summary>
		/// <param name="pageID">The ID of the page for which you want to generate a url element cache key for</param>
		/// <param name="UrlElement">The Url element you are after (IsPlaceHolderID/TabLinkID/PageNameID/UrlKeywordsID) constants</param>
		/// <returns>A unique key</returns>
		private static string UrlElementCacheKey(int pageID, string UrlElement)
		{
			return string.Concat(SiteUniqueID.ToString(), pageID, UrlElement);
		}

        /// <summary>
        /// Builds up a cache key for Url Elements/Properties
        /// </summary>
        /// <param name="pageID">The ID of the page for which you want to generate a url elements cache key for</param>
        /// <returns>A unique key</returns>
        private static string UrlElementsCacheKey(int pageID)
        {
            return string.Concat(SiteUniqueID.ToString(), pageID);
        }


		/// <summary>
		/// Clears all cached url elements for a given page
		/// </summary>
		/// <param name="pageID"></param>
		public static void ClearUrlElements(int pageID)
		{
			Cache applicationCache = HttpContext.Current.Cache;

            string urlElementsCacheKey = UrlElementsCacheKey(pageID);

            if (applicationCache[urlElementsCacheKey] != null)
                applicationCache.Remove(urlElementsCacheKey);
		}

		/// <summary>
		/// This method is used to retrieve a specific Url Property
		/// </summary>
		/// <param name="pageID">The ID of the page the Url belongs to</param>
		/// <param name="propertyID">The ID of the property you are interested in</param>
		/// <param name="cacheDuration">The number of minutes you want to cache this returned value for</param>
		/// <returns>A string value representing the property you are interested in</returns>
		public static string PageSpecificProperty(int pageID, string propertyID, double cacheDuration)
		{
			// Page 0 is shared across portals as a default setting (It doesn't have any real data associated with it so return defaults);
			if (pageID == 0)
			{
				if (propertyID == IsPlaceHolderID)
				{
					return "False";
				}
				else
				{
					return string.Empty;
				}
			}

			// get the unique cache key for the property requested
			string uniquePropertyCacheKey = UrlElementCacheKey(pageID, propertyID);

			// calling HttpContext.Current.Cache all the time incurs a small performance hit so get a reference to it once and reuse that for greater performance
			Cache applicationCache = HttpContext.Current.Cache;

			if (applicationCache[uniquePropertyCacheKey] == null)
			{
				string property = string.Empty;

				using (SqlConnection conn = new SqlConnection(SiteConnectionString))
				{
					try
					{
						// Open the connection
						conn.Open();

						using (SqlCommand cmd = new SqlCommand("SELECT SettingValue FROM rb_TabSettings WHERE TabID=" + pageID.ToString() + " AND SettingName = '" + propertyID + "'", conn))
						{
							// 1. Instantiate a new command above
							// 2. Call ExecuteNonQuery to send command
							property = (string) cmd.ExecuteScalar();
						}
					}

					catch
					{
						// TODO: Decide whether or not this should be logged. If it is a large site upgrading then it would quickly fill up a log file.
						// If there is no value in the database then it thows an error as it is expecting something.
						// This can happen with the initial setup or if no entries for a tab have been made
					}

					finally
					{
					}
				}

				// if null is returned always ensure that either a bool (if it is a TabPlaceholder) or an empty string is returned.
				if ((property == null) || (property.Length == 0))
				{
					// Check to make sure it is not a placeholder...if it is change it to false otherwise ensure that it's value is ""
					if (propertyID == IsPlaceHolderID)
						property = "False";

					else
						property = string.Empty;
				}

				else
				{
					// Just check to see that it is cleaned before caching it (i.e. removing illegal characters)
					// If this section grows too much I will clean it up into methods instead of using if else checks.
					if ((propertyID == PageNameID) || (propertyID == UrlKeywordsID))
					{
						// Replace any illegal characters such as space and special characters and replace it with "-"
						property = Regex.Replace(property, @"[^A-Za-z0-9]", "-");
						if (propertyID == PageNameID)
							property += ".aspx";
					}
				}

				// NOTE: Below you will see an implementation that has been commented out as it didn't seem to work well with the tabsetting cache dependency and always retrieved it again and again.
				//       If someone can figure out why it cant see the cached value please apply the fix and switch the implementation back as it is more ideal (would allow users to see their changes straight away)

				// If this changes it means that the tabsettings have changed which means the urlkeyword, tablink or placeholder status has changed

				// String[] dependencyKey = new String[1];
				// dependencyKey[0] = Appleseed.Framework.Settings.Cache.Key.TabSettings(pageID);
				// applicationCache.Insert(uniquePropertyCacheKey, property, new CacheDependency(null, dependencyKey));

				if (cacheDuration == 0)
				{
					applicationCache.Insert(uniquePropertyCacheKey, property);
				}
				else
				{
					applicationCache.Insert(uniquePropertyCacheKey, property, null, DateTime.Now.AddMinutes(cacheDuration), Cache.NoSlidingExpiration);
				}
				return property;
			}

			else
				return applicationCache[uniquePropertyCacheKey].ToString();
		}

        /// Ashish.patel@haptix.biz - 2014/12/24 - Changed Accessibility for caching
        /// <summary>
		/// This method is used to get all Url Elements in one go
		/// </summary>
		/// <param name="pageID">The ID of the page you are interested in</param>
		/// <param name="cacheDuration">The length of time these values should be cached once retrieved</param>
		public static UrlElements GetUrlElements(int pageID, double cacheDuration)
		{
            UrlElements urlElements = new UrlElements();

			// pageID 0 is a default page shared across portals with no real settings
			if (pageID == 0)
                return urlElements;

			string urlElementsCacheKey = UrlElementsCacheKey(pageID);

			// calling HttpContext.Current.Cache all the time incurs a small performance hit so get a reference to it once and reuse that for greater performance
			Cache applicationCache = HttpContext.Current.Cache;

			// if any values are null refetch
            if (applicationCache[urlElementsCacheKey] == null)
			{
				using (SqlConnection conn = new SqlConnection(SiteConnectionString))
				{
					try
					{
						// Open the connection
						conn.Open();

						using (SqlCommand cmd = new SqlCommand(GetUrlElementsQuery, conn))
						{
                            var parameterPageId = new SqlParameter("@PageID", SqlDbType.Int, 4) { Value = pageID };
                            cmd.Parameters.Add(parameterPageId);

							SqlDataReader pageElements = cmd.ExecuteReader(CommandBehavior.CloseConnection);
							if (pageElements.HasRows)
							{
								pageElements.Read();

                                urlElements.PageName = Convert.ToString(pageElements["PageSEOName"]);
                                if (!String.IsNullOrEmpty(urlElements.PageName))
                                    urlElements.PageName = CleanNoAlphanumerics(urlElements.PageName);

                                urlElements.PageTitle = Convert.ToString(pageElements["PageTitle"]);
                                if (!String.IsNullOrEmpty(urlElements.PageTitle))
                                    urlElements.PageTitle = CleanNoAlphanumerics(urlElements.PageTitle);

                                urlElements.UrlKeywords = Convert.ToString(pageElements["Keywords"]);
                                if (!String.IsNullOrEmpty(urlElements.UrlKeywords))
                                    urlElements.UrlKeywords = CleanNoAlphanumerics(urlElements.UrlKeywords);

                                urlElements.TabLink = Convert.ToString(pageElements["ExternalLink"]);
                                
								if (pageElements["IsPlaceHolder"].ToString() != String.Empty)
								{
									urlElements.IsPlaceHolder = bool.Parse(pageElements["IsPlaceHolder"].ToString());
								}
								// insert value in cache so it doesn't always try to retrieve it

								// NOTE: This is the tabsettings Cache Dependency approach see note above
								// applicationCache.Insert(isPlaceHolderKey, _isPlaceHolder.ToString(), new CacheDependency(null, dependencyKey));
								if (cacheDuration == 0)
								{
									applicationCache.Insert(urlElementsCacheKey, urlElements);
								}
								else
								{
									applicationCache.Insert(urlElementsCacheKey, urlElements, null, DateTime.Now.AddMinutes(cacheDuration), Cache.NoSlidingExpiration);
								}
							}
							// close the reader
							pageElements.Close();
						}
					}
					catch
					{
						// TODO: Decide whether or not this should be logged. If it is a large site upgrading then it would quickly fill up a log file.
						// If there is no value in the database then it thows an error as it is expecting something.
						// This can happen with the initial setup or if no entries for a tab have been made
					}

					finally
					{
					}
				}
			}
			else
			{
                urlElements = (UrlElements)applicationCache[urlElementsCacheKey];
			}
            return urlElements;
		}


		/// <summary>
		/// ApplicationPath, Application dependent relative Application Path.
		/// Base dir for all portal code
		/// Since it is common for all portals is declared as static
		/// </summary>
		public static string ApplicationPath
		{
			get { return Path.ApplicationRoot; }
		}

		/// <summary>
		///     Returns the current site's database connection string
		/// </summary>
		/// <value>
		///     <para>
		///         
		///     </para>
		/// </value>
		/// <remarks>
		///     
		/// </remarks>
		private static string SiteConnectionString
		{
			get { return Config.ConnectionString; }
		}

		/// <summary>
		/// This static string fetches the site's alias either via querystring, cookie or domain and returns it
		/// </summary>
		private static string SiteUniqueID
		{
			get { return Portal.UniqueID; }
		}

        /// <summary>
        /// Clean alpha numerics
        /// </summary>
        /// <param name="StringToClean">string to clean</param>
        /// <returns>result</returns>
        public static string CleanNoAlphanumerics(string StringToClean) {

            /*Regex r = new Regex("[A-Za-z0-9¡ƒ¿¬·‰‚‡…À» ÈÎÍËÕœŒÃÌÔÏÓ”÷“‘ÛˆÚÙ⁄‹€Ÿ˙¸˘˚«Á—Ò/\\s]");
            string aux = "";
            for (int i = 0; i < StringToClean.Length; i++) {
                if (r.IsMatch(StringToClean[i].ToString()))
                    aux += StringToClean[i];
            }

            StringToClean = aux;
            StringToClean = Regex.Replace(StringToClean, @"[¡ƒ¿¬]", "A");
            StringToClean = Regex.Replace(StringToClean, @"[·‰‚‡]", "a");
            StringToClean = Regex.Replace(StringToClean, @"[…À» ]", "E");
            StringToClean = Regex.Replace(StringToClean, @"[ÈÎÍË]", "e");
            StringToClean = Regex.Replace(StringToClean, @"[ÕœŒÃ]", "I");
            StringToClean = Regex.Replace(StringToClean, @"[ÌÔÏÓ]", "i");
            StringToClean = Regex.Replace(StringToClean, @"[”÷“‘]", "O");
            StringToClean = Regex.Replace(StringToClean, @"[ÛˆÚÙ]", "o");
            StringToClean = Regex.Replace(StringToClean, @"[⁄‹€Ÿ]", "U");
            StringToClean = Regex.Replace(StringToClean, @"[˙¸˘˚]", "u");
            StringToClean = Regex.Replace(StringToClean, @"[«]", "C");
            StringToClean = Regex.Replace(StringToClean, @"[Á]", "c");
            StringToClean = Regex.Replace(StringToClean, @"[—]", "N");
            StringToClean = Regex.Replace(StringToClean, @"[Ò]", "n");*/

            StringToClean = Regex.Replace(RemoveDiacritics(StringToClean), @"[^A-Za-z0-9/]", "-");
            StringToClean = Regex.Replace(StringToClean, @"-{2,}", "-");

            while (StringToClean.EndsWith("-")) {
                StringToClean = StringToClean.Remove(StringToClean.Length - 1);
            }

            return StringToClean;
        }

        // \p{Mn} or \p{Non_Spacing_Mark}: 
        //   a character intended to be combined with another 
        //   character without taking up extra space 
        //   (e.g. accents, umlauts, etc.). 
        private readonly static Regex nonSpacingMarkRegex =
            new Regex(@"\p{Mn}", RegexOptions.Compiled);

        private static string RemoveDiacritics(string text)
        {
            if (text == null)
                return string.Empty;

            var normalizedText =
                text.Normalize(NormalizationForm.FormD);

            return nonSpacingMarkRegex.Replace(normalizedText, string.Empty);
        }

	}
}