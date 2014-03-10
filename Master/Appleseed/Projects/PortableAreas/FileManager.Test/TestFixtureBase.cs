using System;
using System.Drawing.Imaging;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace FileManager.Test {
    public class TestFixtureBase {
        protected RemoteWebDriver CurrentDriver { get; set; }

        [SetUp]
        public void TestSetup() {
            FirefoxBinary fb;
            fb = !String.IsNullOrWhiteSpace(Settings.CurrentSettings.FirefoxBinaryPath) ? new FirefoxBinary(Settings.CurrentSettings.FirefoxBinaryPath) : new FirefoxBinary();
            CurrentDriver = new FirefoxDriver(fb, new FirefoxProfile());
        }



        [TearDown]
        public void TestTeardown() {
            try {
                if (TestContext.CurrentContext.Result.Status == TestStatus.Failed
                        && CurrentDriver is ITakesScreenshot) {
                    ((ITakesScreenshot)CurrentDriver).GetScreenshot().SaveAsFile(TestContext.CurrentContext.Test.FullName + ".jpg", ImageFormat.Jpeg);
                }
            }
            catch { }	// null ref exception occurs from accessing TestContext.CurrentContext.Result.Status property

            try {
                CurrentDriver.Quit();
            }
            catch { }
        }

    }
}
