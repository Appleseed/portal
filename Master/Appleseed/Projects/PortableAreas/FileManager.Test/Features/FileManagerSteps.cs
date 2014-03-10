using System;
using FileManager.Test.Base;
using TechTalk.SpecFlow;

namespace FileManager.Test.Features
{
    [Binding]
    public class FileManagerSteps : FeatureBase
    {
        [Given(@"I have the Home Page open")]
        public void GivenIHaveTheHomePageOpen()
        {
            CurrentPage = PageBase.LoadIndexPage(CurrentDriver, Settings.CurrentSettings.BaseUrl);
        }

        [Then(@"I navigate to the FileManager")]
        public void ThenINavigateToTheFileManager()
        {
            CurrentPage = CurrentPage.NavigateToFileManager();
        }

        [Given(@"I navigate to the FileManager")]
        public void GivenINavigateToTheFileManager() {
            CurrentPage = CurrentPage.NavigateToFileManager();
        }

        [Given(@"Testing de que me agarra el background")]
        public void GivenTestingDeQueMeAgarraElBackground() {
            
        }



    }
}
