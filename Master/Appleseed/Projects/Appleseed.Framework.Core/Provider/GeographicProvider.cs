// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GeographicProvider.cs" company="--">
//   Copyright © -- 2010. All Rights Reserved.
// </copyright>
// <summary>
//   Defines the types of countries lists that can be retrieved using Country.GetCountries
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Providers.Geographic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Configuration.Provider;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Caching;

    using Appleseed.Framework.Helpers;
    using Appleseed.Framework.Provider;

    /// <summary>
    /// Defines the types of countries lists that can be retrieved using Country.GetCountries
    /// </summary>
    [Serializable]
    public enum CountryFields
    {
        /// <summary>
        ///   None
        /// </summary>
        None, 

        /// <summary>
        ///   NeutralName
        /// </summary>
        NeutralName, 

        /// <summary>
        ///   CountryID
        /// </summary>
        CountryID, 

        /// <summary>
        ///   Name
        /// </summary>
        Name
    }

    #region Country comparers

    /// <summary>
    /// The country name comparer.
    /// </summary>
    internal class CountryNameComparer : IComparer<Country>
    {
        #region Implemented Interfaces

        #region IComparer<Country>

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">
        /// The first object to compare.
        /// </param>
        /// <param name="y">
        /// The second object to compare.
        /// </param>
        /// <returns>
        /// Value Condition Less than zero x is less than y. Zero x equals y. Greater than zero x is greater than y.
        /// </returns>
        public int Compare(Country x, Country y)
        {
            var xName = x.Name;
            var yName = y.Name;
            return new CaseInsensitiveComparer(CultureInfo.CurrentUICulture).Compare(xName, yName);
        }

        #endregion

        #endregion
    }

    #endregion

    /// <summary>
    /// Geographic provider API
    /// </summary>
    public abstract class GeographicProvider : ProviderBase
    {
        #region Constants and Fields

        /// <summary>
        ///   The current cache.
        /// </summary>
        protected static Cache _currentCache;

        /// <summary>
        ///   Camel case. Must match web.config section name
        /// </summary>
        private const string ProviderType = "geographic";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "GeographicProvider" /> class.
        /// </summary>
        public GeographicProvider()
        {
            this.CountriesFilter = string.Empty;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Instances this instance.
        /// </summary>
        /// <returns></returns>
        public static GeographicProvider Current
        {
            get
            {
                return GetCurrentObject();
            }
        }

        private static GeographicProvider GetCurrentObject()
        {
            // Get the names of providers
            var config = ProviderConfiguration.GetProviderConfiguration(ProviderType);

            // Read specific configuration information for this provider
            var providerSettings = (ProviderSettings)config.Providers[config.DefaultProvider];

            // In the cache?
            var cacheKey = "Appleseed::Web::GeographicProvider::" + config.DefaultProvider;

            if (CurrentCache[cacheKey] == null)
            {
                // The assembly should be in \bin or GAC, so we simply need

                // to get an instance of the type
                try
                {
                    CurrentCache.Insert(
                        cacheKey, ProviderHelper.InstantiateProvider(providerSettings, typeof(GeographicProvider)));
                }
                catch (Exception e)
                {
                    throw new Exception("Unable to load provider", e);
                }
            }

            return (GeographicProvider)CurrentCache[cacheKey];
        }

        /// <summary>
        ///   Returns the a string of comma separated country IDs. This list is used when retrieving lists of countries.
        /// </summary>
        public string CountriesFilter { get; set; }

        /// <summary>
        ///   Get a Country objects that represents the country of the current thread
        /// </summary>
        public Country CurrentCountry
        {
            get
            {
                return Current.GetCountry(RegionInfo.CurrentRegion.Name);
            }
        }

        /// <summary>
        ///   Gets the current cache.
        /// </summary>
        /// <value>The current cache.</value>
        protected static Cache CurrentCache
        {
            get
            {
                if (_currentCache == null)
                {
                    if (HttpContext.Current != null)
                    {
                        _currentCache = HttpContext.Current.Cache;
                    }
                    else
                    {
                        // I'm in a test environment
                        var r = new HttpRuntime();
                        _currentCache = HttpRuntime.Cache;
                    }
                }

                return _currentCache;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the administrative division's name in the specified language if available. It not, gets the default one.
        /// </summary>
        /// <param name="administrativeDivisionName">
        /// The administrative Division Name.
        /// </param>
        /// <param name="c">
        /// a 
        ///   <code>
        /// System.Globalization.CultureInfo
        ///   </code>
        /// describing the language we want the name for
        /// </param>
        /// <returns>
        /// A 
        ///   <code>
        /// string
        ///   </code>
        /// containing the localized name.
        /// </returns>
        public abstract string GetAdministrativeDivisionName(string administrativeDivisionName, CultureInfo c);

        /// <summary>
        /// The get countries.
        /// </summary>
        /// <returns>
        /// </returns>
        public abstract IList<Country> GetCountries();

        /// <summary>
        /// The get countries.
        /// </summary>
        /// <param name="sortBy">
        /// The sort by.
        /// </param>
        /// <returns>
        /// </returns>
        public abstract IList<Country> GetCountries(CountryFields sortBy);

        /// <summary>
        /// The get countries.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <returns>
        /// </returns>
        public abstract IList<Country> GetCountries(string filter);

        /// <summary>
        /// The get countries.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="sortBy">
        /// The sort by.
        /// </param>
        /// <returns>
        /// </returns>
        public abstract IList<Country> GetCountries(string filter, CountryFields sortBy);

        /// <summary>
        /// Returns a 
        ///   <code>
        /// Country
        ///   </code>
        /// object
        /// </summary>
        /// <param name="countryId">
        /// The country's id
        /// </param>
        /// <returns>
        /// A 
        ///   <code>
        /// Country
        ///   </code>
        /// object containing the country info, or null if the country doesn't exist
        /// </returns>
        /// <exception cref="CountryNotFoundException">
        /// If the country is not found
        /// </exception>
        public abstract Country GetCountry(string countryId);

        /// <summary>
        /// Gets the country's name in the specified language if available. It not, gets the default one.
        /// </summary>
        /// <param name="countryId">
        /// The country's id
        /// </param>
        /// <param name="c">
        /// a 
        ///   <code>
        /// System.Globalization.CultureInfo
        ///   </code>
        /// describing the language we want the name for
        /// </param>
        /// <returns>
        /// A 
        ///   <code>
        /// string
        ///   </code>
        /// containing the localized name.
        /// </returns>
        /// <exception cref="CountryNotFoundException">
        /// If the country is not found
        /// </exception>
        public abstract string GetCountryDisplayName(string countryId, CultureInfo c);

        /// <summary>
        /// Returns a list of states for a specified country.
        /// </summary>
        /// <param name="countryId">
        /// The country code
        /// </param>
        /// <returns>
        /// The list of states for the specified country
        /// </returns>
        public abstract IList<State> GetCountryStates(string countryId);

        /// <summary>
        /// Returns a 
        ///   <code>
        /// State
        ///   </code>
        /// object
        /// </summary>
        /// <param name="stateId">
        /// The state's id
        /// </param>
        /// <returns>
        /// A 
        ///   <code>
        /// State
        ///   </code>
        /// object containing the State info, or null if the state doesn't exist
        /// </returns>
        /// <exception cref="StateNotFoundException">
        /// If the state is not found
        /// </exception>
        public abstract State GetState(int stateId);

        /// <summary>
        /// Gets the states's name in the specified language if available. It not, gets the default one.
        /// </summary>
        /// <param name="stateId">
        /// The state ID.
        /// </param>
        /// <param name="c">
        /// a 
        ///   <code>
        /// System.Globalization.CultureInfo
        ///   </code>
        /// describing the language we want the name for
        /// </param>
        /// <returns>
        /// A 
        ///   <code>
        /// string
        ///   </code>
        /// containing the localized name.
        /// </returns>
        /// <exception cref="StateNotFoundException">
        /// If the state is not found
        /// </exception>
        public abstract string GetStateDisplayName(int stateId, CultureInfo c);

        /// <summary>
        /// Gets the unfiltered countries.
        /// </summary>
        /// <returns>
        /// </returns>
        public abstract IList<Country> GetUnfilteredCountries();

        #endregion

        #region Methods

        /// <summary>
        /// Builds the countries filter.
        /// </summary>
        /// <param name="configFilter">
        /// The configuration filter.
        /// </param>
        /// <param name="additionalFilter">
        /// The additional filter.
        /// </param>
        /// <returns>
        /// a string of comma-separated country codes, representing the built filter
        /// </returns>
        protected static string BuildCountriesFilter(string configFilter, string additionalFilter)
        {
            var configFilterArray = configFilter.Trim().Split(',');
            var additionalFilterArray = additionalFilter.Trim().Split(',');
            string[] filterArray;

            foreach (var s in configFilterArray.Where(s => (!s.Equals(string.Empty)) && (s.Length != 2)))
            {
                // this is not a valid country code
                throw new ArgumentException(string.Format("{0} is not a valid country code", s));
            }

            foreach (var s in additionalFilterArray.Where(s => (!s.Equals(string.Empty)) && (s.Length != 2)))
            {
                // this is not a valid country code
                throw new ArgumentException(string.Format("{0} is not a valid country code", s));
            }

            if ((configFilterArray.Length > 1) && (additionalFilterArray.Length > 1))
            {
                filterArray = Utilities.IntersectArrays(configFilterArray, additionalFilterArray);
            }
            else if (configFilterArray.Length > 1)
            {
                filterArray = configFilterArray;
            }
            else if (additionalFilterArray.Length > 1)
            {
                filterArray = additionalFilterArray;
            }
            else
            {
                filterArray = new string[0];
            }

            var filter = filterArray.Aggregate(string.Empty, (current, s) => string.Format("{0}{1},", current, s));

            if (filter.EndsWith(","))
            {
                filter = filter.Substring(0, filter.Length - 1);
            }

            return filter;
        }

        #endregion
    }
}