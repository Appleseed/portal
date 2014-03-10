using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace FileManager.Test.Base {
    public abstract class CommonBase {
        public RemoteWebDriver Driver { get; set; }

        public void ClearAndType(IWebElement element, string value) {
            element.Clear();
            element.SendKeys(value);
        }

        public void WaitUpTo(int milliseconds, Func<bool> condition, string description) {
            int timeElapsed = 0;
            while (!condition() && timeElapsed < milliseconds) {
                Thread.Sleep(100);
                timeElapsed += 100;
            }

            if (timeElapsed >= milliseconds || !condition()) {
                throw new AssertionException("Timed out while waiting for: " + description);
            }
        }

        public static void AssertIsEqual(string expectedValue, string actualValue, string elementDescription) {
            if (expectedValue != actualValue) {
                throw new AssertionException(String.Format("AssertIsEqual Failed: '{0}' didn't match expectations. Expected [{1}], Actual [{2}]", elementDescription, expectedValue, actualValue));
            }
        }

        public static bool IsElementPresent(IWebElement element) {
            try {
                var b = element.Displayed;
                return true;
            }
            catch {
                return false;
            }
        }

        public static void AssertElementPresent(IWebElement element, string elementDescription) {
            if (!IsElementPresent(element))
                throw new AssertionException(String.Format("AssertElementPresent Failed: Could not find '{0}'", elementDescription));
        }

        public static bool IsElementPresent(ISearchContext context, By searchBy) {
            try {
                var b = context.FindElement(searchBy).Displayed;
                return true;
            }
            catch {
                return false;
            }
        }

        public static void AssertElementPresent(ISearchContext context, By searchBy, string elementDescription) {
            if (!IsElementPresent(context, searchBy))
                throw new AssertionException(String.Format("AssertElementPresent Failed: Could not find '{0}'", elementDescription));
        }

        public static void AssertElementsPresent(IWebElement[] elements, string elementDescription) {
            if (elements.Length == 0)
                throw new AssertionException(String.Format("AssertElementsPresent Failed: Could not find '{0}'", elementDescription));
        }

        public static void AssertElementText(IWebElement element, string expectedValue, string elementDescription) {
            AssertElementPresent(element, elementDescription);
            string actualtext = element.Text;
            if (actualtext != expectedValue) {
                throw new AssertionException(String.Format("AssertElementText Failed: Value for '{0}' did not match expectations. Expected: [{1}], Actual: [{2}]", elementDescription, expectedValue, actualtext));
            }
        }

    }
}
