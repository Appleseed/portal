// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlGeographicProvider.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   SQL implementation of the
//   <code>
//   GeographicProvider
//   </code>
//   API
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Providers.Geographic
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;

    using Appleseed.Framework.Settings;

    /// <summary>
    /// SQL implementation of the 
    /// <code>
    /// GeographicProvider
    /// </code>
    /// API
    /// </summary>
    public class SqlGeographicProvider : GeographicProvider
    {
        #region Properties

        /// <summary>
        ///   Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        protected static SqlConnection ConnectionString
        {
            get
            {
                // TODO: WTF?
                SqlConnection result;
                try
                {
                    result = Config.SqlConnectionString;
                }
                catch
                {
                    // I'm in a test environment
                    result =
                        new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                }

                return result;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the administrative division's name in the specified language if available. It not, gets the default one.
        /// </summary>
        /// <param name="administrativeDivisionName"></param>
        /// <param name="c">a <code>System.Globalization.CultureInfo</code> describing the language we want the name for</param>
        /// <returns>
        /// A <code>string</code> containing the localized name.
        /// </returns>
        public override string GetAdministrativeDivisionName(string administrativeDivisionName, CultureInfo c)
        {
            var cacheKey = string.Format("ADMINISTRATIVEDIVISIONNAME_{0} - {1}", administrativeDivisionName, c.TwoLetterISOLanguageName);

            if (CurrentCache.Get(cacheKey) == null)
            {
                var result = this.GetLocalizedDisplayName("ADMINISTRATIVEDIVISIONNAME_" + administrativeDivisionName, c);

                if (string.IsNullOrEmpty(result))
                {
                    result = administrativeDivisionName;
                }

                CurrentCache.Insert(cacheKey, result);
            }

            return (string)CurrentCache.Get(cacheKey);
        }

        /// <summary>
        /// The get countries.
        /// </summary>
        public override IList<Country> GetCountries()
        {
            return GetCountriesCore(this.CountriesFilter, string.Empty, CountryFields.CountryID);
        }

        /// <summary>
        /// Gets the countries.
        /// </summary>
        /// <param name="sortBy">The sort by.</param>
        /// <returns></returns>
        public override IList<Country> GetCountries(CountryFields sortBy)
        {
            return GetCountriesCore(this.CountriesFilter, string.Empty, sortBy);
        }

        /// <summary>
        /// The get countries.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        public override IList<Country> GetCountries(string filter)
        {
            return GetCountriesCore(this.CountriesFilter, filter, CountryFields.CountryID);
        }

        /// <summary>
        /// The get countries.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="sortBy">
        /// The sort BY.
        /// </param>
        public override IList<Country> GetCountries(string filter, CountryFields sortBy)
        {
            return GetCountriesCore(this.CountriesFilter, filter, sortBy);
        }

        /// <summary>
        /// Returns a <code>Country</code> object
        /// </summary>
        /// <param name="countryId">The country's id</param>
        /// <returns>
        /// A <code>Country</code> object containing the country info, or null if the country doesn't exist
        /// </returns>
        /// <exception cref="CountryNotFoundException">If the country is not found</exception>
        public override Country GetCountry(string countryId)
        {
            Country result;

            var cmd = new SqlCommand
                {
                    CommandText =
                        "SELECT NeutralName, AdministrativeDivisionNeutralName FROM rb_Countries WHERE CountryID=@CountryID"
                };
            cmd.Parameters.Add("@CountryID", SqlDbType.NChar, 2).Value = countryId;

            cmd.Connection = ConnectionString;

            SqlDataReader reader = null;
            try
            {
                cmd.Connection.Open();

                reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    throw new CountryNotFoundException(
                        string.Format("The country with ID {0} was not found in the database", countryId));
                }

                result = new Country(countryId, reader.GetString(0), reader.GetString(1));
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                cmd.Connection.Close();
            }

            return result;
        }

        /// <summary>
        /// Gets the display name of the country.
        /// </summary>
        /// <param name="countryId">The country id.</param>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        public override string GetCountryDisplayName(string countryId, CultureInfo c)
        {
            var cacheKey = string.Format("COUNTRY_{0} - {1}", countryId, c.TwoLetterISOLanguageName);

            if (CurrentCache.Get(cacheKey) == null)
            {
                var result = this.GetLocalizedDisplayName("COUNTRY_" + countryId, c);

                if (string.IsNullOrEmpty(result))
                {
                    result = this.GetCountry(countryId).NeutralName;
                }

                CurrentCache.Insert(cacheKey, result);
            }

            return (string)CurrentCache.Get(cacheKey);
        }

        /// <summary>
        /// Returns a list of states for a specified country.
        /// </summary>
        /// <param name="countryId">The country code</param>
        /// <returns>
        /// The list of states for the specified country
        /// </returns>
        public override IList<State> GetCountryStates(string countryId)
        {
            IList<State> result = new List<State>();

            var cmd = new SqlCommand
                {
                    CommandText =
                        "SELECT  StateID, NeutralName FROM rb_States WHERE CountryID = @CountryID ORDER BY NeutralName"
                };
            cmd.Parameters.Add("@CountryID", SqlDbType.NChar, 2).Value = countryId;

            cmd.Connection = ConnectionString;

            SqlDataReader reader = null;
            try
            {
                cmd.Connection.Open();

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new State(reader.GetInt32(0), countryId, reader.GetString(1)));
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                cmd.Connection.Close();
            }

            return result;
        }

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <param name="stateId">The state id.</param>
        /// <returns></returns>
        public override State GetState(int stateId)
        {
            State result;

            var cmd = new SqlCommand
                {
                    CommandText = "SELECT CountryID, NeutralName FROM rb_States WHERE StateID=@StateID" 
                };
            cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = stateId;

            cmd.Connection = ConnectionString;

            SqlDataReader reader = null;
            try
            {
                cmd.Connection.Open();

                reader = cmd.ExecuteReader();

                if (!reader.Read())
                {
                    throw new StateNotFoundException(
                        string.Format("The state with ID {0} was not found in the database", stateId));
                }

                result = new State(stateId, reader.GetString(0), reader.GetString(1));
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                cmd.Connection.Close();
            }

            return result;
        }

        /// <summary>
        /// Gets the states's name in the specified language if available. It not, gets the default one.
        /// </summary>
        /// <param name="stateId"></param>
        /// <param name="c">a <code>System.Globalization.CultureInfo</code> describing the language we want the name for</param>
        /// <returns>
        /// A <code>string</code> containing the localized name.
        /// </returns>
        /// <exception cref="StateNotFoundException">If the state is not found</exception>
        public override string GetStateDisplayName(int stateId, CultureInfo c)
        {
            var cacheKey = string.Format("STATENAME_{0} - {1}", stateId, c.TwoLetterISOLanguageName);

            if (CurrentCache.Get(cacheKey) == null)
            {
                var result = this.GetLocalizedDisplayName("STATENAME_" + stateId, c);

                if (string.IsNullOrEmpty(result))
                {
                    result = this.GetState(stateId).NeutralName;
                }

                CurrentCache.Insert(cacheKey, result);
            }

            return (string)CurrentCache.Get(cacheKey);
        }

        /// <summary>
        /// The get unfiltered countries.
        /// </summary>
        /// <returns>
        /// </returns>
        public override IList<Country> GetUnfilteredCountries()
        {
            return GetCountriesCore(string.Empty, string.Empty, CountryFields.CountryID);
        }

        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="name">The friendly name of the provider.</param>
        /// <param name="config">A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.</param>
        /// <exception cref="T:System.ArgumentNullException">The name of the provider is null.</exception>
        ///   
        /// <exception cref="T:System.ArgumentException">The name of the provider has a length of zero.</exception>
        ///   
        /// <exception cref="T:System.InvalidOperationException">An attempt is made to call <see cref="M:System.Configuration.Provider.ProviderBase.Initialize(System.String,System.Collections.Specialized.NameValueCollection)"/> on a provider after the provider has already been initialized.</exception>
        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);

            // Initialize values from web.config.
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            this.CountriesFilter = this.GetConfigValue(config["countriesFilter"], string.Empty);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the countries core.
        /// </summary>
        /// <param name="configFilter">The config filter.</param>
        /// <param name="additionalFilter">The additional filter.</param>
        /// <param name="sortBy">The sort by.</param>
        /// <returns></returns>
        private static IList<Country> GetCountriesCore(
            string configFilter, string additionalFilter, CountryFields sortBy)
        {
            var result = new List<Country>();

            using (var cmd = new SqlCommand())
            {
                var filter = BuildCountriesFilter(configFilter, additionalFilter);

                cmd.CommandText = filter.Equals(string.Empty) ? "rb_GetCountries" : "rb_GetCountriesFiltered";

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Connection = ConnectionString;

                if (!filter.Equals(string.Empty))
                {
                    cmd.Parameters.Add("@Filter", SqlDbType.NVarChar, 10000).Value = filter;
                }

                cmd.Parameters.Add("@SortBy", SqlDbType.Int).Value = (int)sortBy;

                SqlDataReader reader = null;
                try
                {
                    cmd.Connection.Open();

                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result.Add(new Country(reader.GetString(0), reader.GetString(1), reader.GetString(2)));
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }

                    cmd.Connection.Close();
                }
            }

            if (sortBy == CountryFields.Name)
            {
                result.Sort(new CountryNameComparer());
            }

            return result;
        }

        /// <summary>
        /// A helper function to retrieve config values from the configuration file.
        /// </summary>
        /// <param name="configValue">The config value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The get config value.
        /// </returns>
        private string GetConfigValue(string configValue, string defaultValue)
        {
            return String.IsNullOrEmpty(configValue) ? defaultValue : configValue;
        }

        /// <summary>
        /// Gets the display name of the localized.
        /// </summary>
        /// <param name="textKey">The text key.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        private string GetLocalizedDisplayName(string textKey, CultureInfo culture)
        {
            var cmd = new SqlCommand
                {
                    CommandText =
                        "SELECT Description FROM rb_Localize WHERE Textkey=@TextKey AND CultureCode=@CultureCode"
                };
            cmd.Parameters.Add("@TextKey", SqlDbType.NVarChar, 255).Value = textKey;
            cmd.Parameters.Add("@CultureCode", SqlDbType.NVarChar, 2).Value = culture.TwoLetterISOLanguageName;
            cmd.Connection = ConnectionString;

            SqlDataReader reader = null;
            try
            {
                cmd.Connection.Open();

                reader = cmd.ExecuteReader();

                return reader.Read() ? reader.GetString(0) : null;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                cmd.Connection.Close();
            }
        }

        #endregion
    }
}