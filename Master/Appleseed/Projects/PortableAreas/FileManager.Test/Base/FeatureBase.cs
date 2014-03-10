using OpenQA.Selenium.Remote;
using TechTalk.SpecFlow;

namespace FileManager.Test.Base {
    public class FeatureBase : TestFixtureBase {

        #region Properties for Readability

        /// <summary>
        /// Shortcut property to Settings.CurrentSettings.Defaults for readability
        /// </summary>
        //protected DefaultValues.DefaultValues Default { get { return Settings.CurrentSettings.Defaults; } }

        /// <summary>
        /// Sets the Current page to the specified value - provided to help readability
        /// </summary>
        protected PageBase NextPage { set { CurrentPage = value; } }

        #endregion

        protected PageBase CurrentPage {
            get { return (PageBase)ScenarioContext.Current["CurrentPage"]; }
            set { ScenarioContext.Current["CurrentPage"] = value; }
        }

        [BeforeScenario("UI")]
        public void BeforeScenario() {
            if (!ScenarioContext.Current.ContainsKey("CurrentDriver")) {
                TestSetup();
                ScenarioContext.Current.Add("CurrentDriver", CurrentDriver);
            }
            else {
                CurrentDriver = (RemoteWebDriver)ScenarioContext.Current["CurrentDriver"];
            }
        }

        [AfterScenario("UI")]
        public void AfterScenario() {
            if (ScenarioContext.Current.ContainsKey("CurrentDriver")) {
                TestTeardown();
                ScenarioContext.Current.Remove("CurrentDriver");
            }
        }
    }
}
