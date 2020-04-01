using Framework.MarketIT.Automation_Framework.Utilities.Common;
using OpenQA.Selenium;

namespace RegressionSuitAutomationPractice.TestSuit.StepDefinations
{
    public class StepBase<TPage> where TPage : BasePage, new()
    {
        //protected readonly AppSettingsManager AppSettingsManager = new AppSettingsManager();

        //protected IWebDriver WebDriver
        //{
        //    get { return WebDriverContainer.Instance.WebDriver; }
        //}
        //    protected void NavigateToStartUrl(string url = null, bool cleanCookies = true)
        //    {
        //    // Load the page once so that all the cookies (including "domain" and "path" cookies are accessible)
        //    StartPage = ObjectFactory.CreatePage<TPage>(WebDriver);
        //    StartPage.NavigateToStartUrl();

        //    // Clear the cookies at this time to get a fresh start
        //    if (cleanCookies)
        //    {
        //        WebDriver.Manage().Cookies.DeleteAllCookies();
        //        // Clear the session storage to get the fresh session
        //        WebDriver.ClearSessionStorage();
        //        // Navigate this time again to start the test case
        //        StartPage.NavigateToStartUrl();
        //    }


        //    if (TestCaseData.ShouldPutUserInQueryString) LoadUserInfoFromPage(TestCaseData.User);
        //}
    }
}