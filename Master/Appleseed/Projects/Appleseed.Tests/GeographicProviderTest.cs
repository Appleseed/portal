using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Appleseed.Framework;
using System.Globalization;
using Appleseed.Framework.Providers.Geographic;

namespace Appleseed.Tests
{

    [TestFixture]
    public class GeographicProviderTest
    {

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            // Set up initial database environment for testing purposes
            TestHelper.TearDownDB();
            TestHelper.RecreateDBSchema();
        }

        [SetUp]
        public void Init()
        {
            GeographicProvider.Current.CountriesFilter = "AR,BR,CL,UY";
        }

        [Test]
        public void CountryCtorTest1()
        {
            try
            {
                Country c = new Country();
                Assert.AreEqual(string.Empty, c.CountryID);
                Assert.AreEqual(string.Empty, c.NeutralName);
                Assert.AreEqual(string.Empty, c.AdministrativeDivisionNeutralName);
            }
            catch (Exception ex)
            {

                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void CountryCtorTest2()
        {
            try
            {
                Country c = new Country("BR", "Brazil", "State");
                Assert.AreEqual("BR", c.CountryID);
                Assert.AreEqual("Brazil", c.NeutralName);
                Assert.AreEqual("State", c.AdministrativeDivisionNeutralName);
            }
            catch (Exception ex)
            {

                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void StateCtorTest1()
        {
            try
            {
                State s = new State();
                Assert.AreEqual(string.Empty, s.CountryID);
                Assert.AreEqual(string.Empty, s.NeutralName);
                Assert.AreEqual(0, s.StateID);
            }
            catch (Exception ex)
            {

                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void StateCtorTest2()
        {
            try
            {
                State s = new State(1000, "UY", "aName");
                Assert.AreEqual("UY", s.CountryID);
                Assert.AreEqual("aName", s.NeutralName);
                Assert.AreEqual(1000, s.StateID);
            }
            catch (Exception ex)
            {

                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void ProviderInstanciationTest()
        {
            try
            {
                GeographicProvider provider = GeographicProvider.Current;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void ProviderInitializeTest()
        {
            try
            {
                string filteredCountries = GeographicProvider.Current.CountriesFilter;
                Assert.AreEqual("AR,BR,CL,UY", filteredCountries);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetCountriesTest1()
        {
            try
            {
                IList<Country> countries = GeographicProvider.Current.GetCountries();
                Assert.AreEqual(countries.Count, 4);  // AR, BR, UY and CL
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetCountriesLocalizationTest1()
        {
            try
            {
                IList<Country> countries = GeographicProvider.Current.GetCountries();

                CultureInfo currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");

                Assert.AreEqual("Argentinien", countries[0].Name);
                Assert.AreEqual("Brasilien", countries[1].Name);
                Assert.AreEqual("Chile", countries[2].Name);
                Assert.AreEqual("Uruguay", countries[3].Name);

                // again so we can test cached names
                Assert.AreEqual("Argentinien", countries[0].Name);
                Assert.AreEqual("Brasilien", countries[1].Name);
                Assert.AreEqual("Chile", countries[2].Name);
                Assert.AreEqual("Uruguay", countries[3].Name);

                System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetSortedCountriesTest1()
        {
            try
            {
                IList<Country> countries = GeographicProvider.Current.GetCountries(CountryFields.CountryID);
                Assert.AreEqual(countries.Count, 4);  // AR, BR, UY and CL
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetSortedCountriesTest2()
        {
            try
            {
                IList<Country> countries = GeographicProvider.Current.GetCountries(CountryFields.CountryID);

                Assert.AreEqual("AR", countries[0].CountryID);
                Assert.AreEqual("BR", countries[1].CountryID);
                Assert.AreEqual("CL", countries[2].CountryID);
                Assert.AreEqual("UY", countries[3].CountryID);

                Assert.AreEqual("Argentina", countries[0].NeutralName);
                Assert.AreEqual("Brazil", countries[1].NeutralName);
                Assert.AreEqual("Chile", countries[2].NeutralName);
                Assert.AreEqual("Uruguay", countries[3].NeutralName);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetSortedCountriesTest3()
        {
            try
            {
                IList<Country> countries = GeographicProvider.Current.GetCountries(CountryFields.NeutralName);

                Assert.AreEqual("AR", countries[0].CountryID);
                Assert.AreEqual("BR", countries[1].CountryID);
                Assert.AreEqual("CL", countries[2].CountryID);
                Assert.AreEqual("UY", countries[3].CountryID);

                Assert.AreEqual("Argentina", countries[0].NeutralName);
                Assert.AreEqual("Brazil", countries[1].NeutralName);
                Assert.AreEqual("Chile", countries[2].NeutralName);
                Assert.AreEqual("Uruguay", countries[3].NeutralName);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetSortedCountriesTest4()
        {
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
                IList<Country> countries = GeographicProvider.Current.GetCountries(CountryFields.Name);

                Assert.AreEqual("AR", countries[0].CountryID);
                Assert.AreEqual("BR", countries[1].CountryID);
                Assert.AreEqual("CL", countries[2].CountryID);
                Assert.AreEqual("UY", countries[3].CountryID);

                Assert.AreEqual("Argentina", countries[0].NeutralName);
                Assert.AreEqual("Brazil", countries[1].NeutralName);
                Assert.AreEqual("Chile", countries[2].NeutralName);
                Assert.AreEqual("Uruguay", countries[3].NeutralName);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetSortedCountriesTest5()
        {
            try
            {
                IList<Country> countries = GeographicProvider.Current.GetCountries(CountryFields.None);

                Assert.AreEqual("AR", countries[0].CountryID);
                Assert.AreEqual("BR", countries[1].CountryID);
                Assert.AreEqual("CL", countries[2].CountryID);
                Assert.AreEqual("UY", countries[3].CountryID);

                Assert.AreEqual("Argentina", countries[0].NeutralName);
                Assert.AreEqual("Brazil", countries[1].NeutralName);
                Assert.AreEqual("Chile", countries[2].NeutralName);
                Assert.AreEqual("Uruguay", countries[3].NeutralName);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetCountriesFilteredTest1()
        {
            try
            {
                IList<Country> countries = GeographicProvider.Current.GetCountries("AR,UY");
                Assert.AreEqual(countries.Count, 2);  // AR, and UY 

                Assert.AreEqual("AR", countries[0].CountryID);
                Assert.AreEqual("UY", countries[1].CountryID);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetCountriesFilteredTest2()
        {
            try
            {
                IList<Country> countries = GeographicProvider.Current.GetCountries("LongFilter");
                Assert.Fail("Should've thrown an ArgumentException");
            }
            catch (ArgumentException)
            {
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetFilteredSortedCountriesTest1()
        {
            try
            {
                IList<Country> countries = GeographicProvider.Current.GetCountries("AR,UY", CountryFields.CountryID);
                Assert.AreEqual(countries.Count, 2);  // AR, UY 
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetFilteredSortedCountriesTest2()
        {
            try
            {
                IList<Country> countries = GeographicProvider.Current.GetCountries("AR,UY", CountryFields.CountryID);

                Assert.AreEqual("AR", countries[0].CountryID);
                Assert.AreEqual("UY", countries[1].CountryID);

                Assert.AreEqual("Argentina", countries[0].NeutralName);
                Assert.AreEqual("Uruguay", countries[1].NeutralName);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetFilteredSortedCountriesTest3()
        {
            try
            {
                IList<Country> countries = GeographicProvider.Current.GetCountries("AR,UY", CountryFields.NeutralName);

                Assert.AreEqual("AR", countries[0].CountryID);
                Assert.AreEqual("UY", countries[1].CountryID);

                Assert.AreEqual("Argentina", countries[0].NeutralName);
                Assert.AreEqual("Uruguay", countries[1].NeutralName);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetFilteredSortedCountriesTest4()
        {
            try
            {
                IList<Country> countries = GeographicProvider.Current.GetCountries("AR,UY", CountryFields.CountryID);

                Assert.AreEqual("AR", countries[0].CountryID);
                Assert.AreEqual("UY", countries[1].CountryID);

                Assert.AreEqual("Argentina", countries[0].NeutralName);
                Assert.AreEqual("Uruguay", countries[1].NeutralName);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetFilteredSortedCountriesTest5()
        {
            try
            {
                IList<Country> countries = GeographicProvider.Current.GetCountries("AR,UY", CountryFields.None);

                Assert.AreEqual("AR", countries[0].CountryID);
                Assert.AreEqual("UY", countries[1].CountryID);

                Assert.AreEqual("Argentina", countries[0].NeutralName);
                Assert.AreEqual("Uruguay", countries[1].NeutralName);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetSortedCountriesNoCountryFilterTest1()
        {
            try
            {
                GeographicProvider.Current.CountriesFilter = string.Empty;
                IList<Country> countries = GeographicProvider.Current.GetCountries(CountryFields.CountryID);
                Assert.AreEqual(countries.Count, 237);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetSortedCountriesNoCountryFilterTest2()
        {
            try
            {
                GeographicProvider.Current.CountriesFilter = string.Empty;
                IList<Country> countries = GeographicProvider.Current.GetCountries(CountryFields.CountryID);

                Assert.AreEqual("AD", countries[0].CountryID);
                Assert.AreEqual("AE", countries[1].CountryID);
                Assert.AreEqual("AF", countries[2].CountryID);
                Assert.AreEqual("ZW", countries[236].CountryID);

                Assert.AreEqual("Andorra", countries[0].NeutralName);
                Assert.AreEqual("United Arab Emirates", countries[1].NeutralName);
                Assert.AreEqual("Afghanistan", countries[2].NeutralName);
                Assert.AreEqual("Zimbabwe", countries[236].NeutralName);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetSortedCountriesNoCountryFilterTest3()
        {
            try
            {
                GeographicProvider.Current.CountriesFilter = string.Empty;
                IList<Country> countries = GeographicProvider.Current.GetCountries(CountryFields.NeutralName);

                Assert.AreEqual("AF", countries[0].CountryID);
                Assert.AreEqual("AL", countries[1].CountryID);
                Assert.AreEqual("ZM", countries[235].CountryID);
                Assert.AreEqual("ZW", countries[236].CountryID);

                Assert.AreEqual("Afghanistan", countries[0].NeutralName);
                Assert.AreEqual("Albania", countries[1].NeutralName);
                Assert.AreEqual("Zambia", countries[235].NeutralName);
                Assert.AreEqual("Zimbabwe", countries[236].NeutralName);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetSortedCountriesNoCountryFilterTest4()
        {
            try
            {
                GeographicProvider.Current.CountriesFilter = string.Empty;
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
                IList<Country> countries = GeographicProvider.Current.GetCountries(CountryFields.Name);

                Assert.AreEqual("AF", countries[0].CountryID);
                Assert.AreEqual("AL", countries[1].CountryID);
                Assert.AreEqual("ZM", countries[235].CountryID);
                Assert.AreEqual("ZW", countries[236].CountryID);

                Assert.AreEqual("Afghanistan", countries[0].NeutralName);
                Assert.AreEqual("Albania", countries[1].NeutralName);
                Assert.AreEqual("Zambia", countries[235].NeutralName);
                Assert.AreEqual("Zimbabwe", countries[236].NeutralName);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetSortedCountriesNoCountryFilterTest5()
        {
            try
            {
                GeographicProvider.Current.CountriesFilter = string.Empty;
                IList<Country> countries = GeographicProvider.Current.GetCountries(CountryFields.None);

                Assert.AreEqual("AD", countries[0].CountryID);
                Assert.AreEqual("AE", countries[1].CountryID);
                Assert.AreEqual("ZM", countries[235].CountryID);
                Assert.AreEqual("ZW", countries[236].CountryID);

                Assert.AreEqual("Andorra", countries[0].NeutralName);
                Assert.AreEqual("United Arab Emirates", countries[1].NeutralName);
                Assert.AreEqual("Zambia", countries[235].NeutralName);
                Assert.AreEqual("Zimbabwe", countries[236].NeutralName);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetCountriesFilteredNoCountryFilterTest1()
        {
            try
            {
                GeographicProvider.Current.CountriesFilter = string.Empty;
                IList<Country> countries = GeographicProvider.Current.GetCountries("AR,UY");
                Assert.AreEqual(countries.Count, 2);  // AR, and UY 

                Assert.AreEqual("AR", countries[0].CountryID);
                Assert.AreEqual("UY", countries[1].CountryID);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetCountriesFilteredNoCountryFilterTest2()
        {
            try
            {
                GeographicProvider.Current.CountriesFilter = string.Empty;
                IList<Country> countries = GeographicProvider.Current.GetCountries("LongFilter");
                Assert.Fail("Should've thrown an ArgumentException");
            }
            catch (ArgumentException)
            {
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetCountriesSortedNoCountryFilterTest1()
        {
            try
            {
                GeographicProvider.Current.CountriesFilter = string.Empty;
                IList<Country> countries = GeographicProvider.Current.GetCountries(CountryFields.CountryID);
                Assert.AreEqual(countries.Count, 237);  // AR, BR, UY and CL
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetFilteredSortedCountriesNoCountryFilterTest1()
        {
            try
            {
                GeographicProvider.Current.CountriesFilter = string.Empty;
                IList<Country> countries = GeographicProvider.Current.GetCountries("AR,UY", CountryFields.CountryID);
                Assert.AreEqual(countries.Count, 2);  // AR, UY 
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetFilteredSortedCountriesNoCountryFilterTest2()
        {
            try
            {
                GeographicProvider.Current.CountriesFilter = string.Empty;
                IList<Country> countries = GeographicProvider.Current.GetCountries("AR,UY", CountryFields.CountryID);

                Assert.AreEqual("AR", countries[0].CountryID);
                Assert.AreEqual("UY", countries[1].CountryID);

                Assert.AreEqual("Argentina", countries[0].NeutralName);
                Assert.AreEqual("Uruguay", countries[1].NeutralName);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetFilteredSortedCountriesNoCountryFilterTest3()
        {
            try
            {
                GeographicProvider.Current.CountriesFilter = string.Empty;
                IList<Country> countries = GeographicProvider.Current.GetCountries("AR,UY", CountryFields.NeutralName);

                Assert.AreEqual("AR", countries[0].CountryID);
                Assert.AreEqual("UY", countries[1].CountryID);

                Assert.AreEqual("Argentina", countries[0].NeutralName);
                Assert.AreEqual("Uruguay", countries[1].NeutralName);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetFilteredSortedCountriesNoCountryFilterTest4()
        {
            try
            {
                GeographicProvider.Current.CountriesFilter = string.Empty;
                IList<Country> countries = GeographicProvider.Current.GetCountries("AR,UY", CountryFields.CountryID);

                Assert.AreEqual("AR", countries[0].CountryID);
                Assert.AreEqual("UY", countries[1].CountryID);

                Assert.AreEqual("Argentina", countries[0].NeutralName);
                Assert.AreEqual("Uruguay", countries[1].NeutralName);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetFilteredSortedCountriesNoCountryFilterTest5()
        {
            try
            {
                GeographicProvider.Current.CountriesFilter = string.Empty;
                IList<Country> countries = GeographicProvider.Current.GetCountries("AR,UY", CountryFields.None);

                Assert.AreEqual("AR", countries[0].CountryID);
                Assert.AreEqual("UY", countries[1].CountryID);

                Assert.AreEqual("Argentina", countries[0].NeutralName);
                Assert.AreEqual("Uruguay", countries[1].NeutralName);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetUnfilteredCountriesTest1()
        {
            try
            {
                IList<Country> allCountries = GeographicProvider.Current.GetUnfilteredCountries();
                Assert.AreEqual(237, allCountries.Count);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetCountryStatesTest1()
        {
            try
            {
                IList<State> states = GeographicProvider.Current.GetCountryStates("AE");

                Assert.AreEqual(4, states.Count);

                foreach (State s in states)
                {
                    switch (s.StateID)
                    {
                        case 599:
                            Assert.AreEqual("Abu Dhabi", s.NeutralName);
                            Assert.AreEqual("AE", s.CountryID);
                            break;
                        case 2082:
                            Assert.AreEqual("Ash Shariqah", s.NeutralName);
                            Assert.AreEqual("AE", s.CountryID);
                            break;
                        case 9470:
                            Assert.AreEqual("Dubai", s.NeutralName);
                            Assert.AreEqual("AE", s.CountryID);
                            break;
                        case 9217877:
                            Assert.AreEqual("Al l'Ayn", s.NeutralName);
                            Assert.AreEqual("AE", s.CountryID);
                            break;
                        default:
                            Assert.Fail();
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetCountryStatesTest2()
        {
            try
            {
                IList<State> states = GeographicProvider.Current.GetCountryStates("PP");

                Assert.AreEqual(0, states.Count);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetCountryDisplayNameTest1()
        {
            string displayName = GeographicProvider.Current.GetCountryDisplayName("BR", new CultureInfo("es-ES"));
            Assert.AreEqual("Brasil", displayName);
        }

        [Test]
        public void GetCountryDisplayNameTest2()
        {
            try
            {
                string displayName = GeographicProvider.Current.GetCountryDisplayName("ZZ", new CultureInfo("es-ES"));
                Assert.Fail("ZZ isn't a CountryID");
            }
            catch (CountryNotFoundException)
            {
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetStateDisplayNameTest1()
        {
            string displayName = GeographicProvider.Current.GetStateDisplayName(1003, new CultureInfo("en-US"));
            Assert.AreEqual("Alabama", displayName);
        }

        [Test]
        public void GetStateDisplayNameTest2()
        {
            try
            {
                string displayName = GeographicProvider.Current.GetStateDisplayName(-40, new CultureInfo("en-US"));
                Assert.Fail("-40 is not a valid StateID");
            }
            catch (StateNotFoundException)
            {
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetAdministrativeDivisionNameTest1()
        {
            string displayName = GeographicProvider.Current.GetAdministrativeDivisionName("Department", new CultureInfo("es-ES"));
            Assert.AreEqual("Department", displayName);
        }

        [Test]
        public void GetCountryTest1()
        {
            Country c = GeographicProvider.Current.GetCountry("US");

            Assert.AreEqual("US", c.CountryID);
            Assert.AreEqual("United States", c.NeutralName);
        }

        [Test]
        public void GetCountryTest2()
        {
            Country c = GeographicProvider.Current.GetCountry("US");

            CultureInfo currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            Assert.AreEqual("Vereinigte Staaten von Amerika", c.Name);
            System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
        }

        [Test]
        public void GetCountryTest3()
        {
            try
            {
                Country c = GeographicProvider.Current.GetCountry("..");
                Assert.Fail("Country .. doesn't exist");
            }
            catch (CountryNotFoundException)
            {
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void GetStateTest1()
        {
            State state = GeographicProvider.Current.GetState(1003);
            Assert.AreEqual("Alabama", state.NeutralName);
            Assert.AreEqual(1003, state.StateID);
            Assert.AreEqual("US", state.CountryID);
        }

        [Test]
        public void GetStateTest2()
        {
            try
            {
                State c = GeographicProvider.Current.GetState(-100);
                Assert.Fail("State -100 doesn't exist");
            }
            catch (StateNotFoundException)
            {
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void CurrentCountryTest1()
        {
            Country actual = GeographicProvider.Current.CurrentCountry;
            Country expected = GeographicProvider.Current.GetCountry(RegionInfo.CurrentRegion.Name);

            Assert.AreEqual(expected, actual);
        }
    }
}
