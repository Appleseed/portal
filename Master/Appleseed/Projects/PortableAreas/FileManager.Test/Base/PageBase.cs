using System;
using FileManager.Test.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.PageObjects;

namespace FileManager.Test.Base {
    public abstract class PageBase : CommonBase {
        public string BaseUrl { get; set; }
        public virtual string DefaultTitle { get { return ""; } }

        protected TPage GetInstance<TPage>(RemoteWebDriver driver = null, string expectedTitle = "") where TPage : PageBase, new() {
            return GetInstance<TPage>(driver ?? Driver, BaseUrl, expectedTitle);
        }

        protected static TPage GetInstance<TPage>(RemoteWebDriver driver, string baseUrl, string expectedTitle = "") where TPage : PageBase, new() {
            var pageInstance = new TPage
                                   {
                Driver = driver,
                BaseUrl = baseUrl
            };
            PageFactory.InitElements(driver, pageInstance);

            if (string.IsNullOrWhiteSpace(expectedTitle)) expectedTitle = pageInstance.DefaultTitle;

            //wait up to 5s for an actual page title since Selenium no longer waits for page to load after 2.21
            new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(5))
                                            .Until(d => d.FindElement(By.TagName("body")));

            //AssertIsEqual(expectedTitle, driver.Title, "Page Title");

            return pageInstance;
        }

        /// <summary>
        /// Asserts that the current page is of the given type
        /// </summary>
        public void Is<TPage>() where TPage : PageBase, new() {
            if (!(this is TPage)) {
                throw new AssertionException(String.Format("Page Type Mismatch: Current page is not a '{0}'", typeof(TPage).Name));
            }
        }

        /// <summary>
        /// Inline cast to the given page type
        /// </summary>
        public TPage As<TPage>() where TPage : PageBase, new() {
            return (TPage)this;
        }

        public static IndexPage LoadIndexPage(RemoteWebDriver driver, string baseURL) {
            driver.Navigate().GoToUrl(baseURL.TrimEnd(new[] { '/' }) + IndexPage.URL);
            return GetInstance<IndexPage>(driver, baseURL, "");
        }

        public FileManagerPage NavigateToFileManager()
        {
            var url = Settings.CurrentSettings.BaseUrl.TrimEnd(new[] {'/'}) + FileManagerPage.URL;
            Driver.Navigate().GoToUrl(url);
            return GetInstance<FileManagerPage>(Driver,url);
        }

        [FindsBy(How = How.Id, Using = "Store")]
        IWebElement LinkStore { get; set; }

    }
}
