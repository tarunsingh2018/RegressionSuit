using Framework.MarketIT.Automation_Framework.Managers;
using Framework.MarketIT.Automation_Framework.Utilities.Factory;
using Framework.MarketIT.Automation_Framework.Extensions;
using OpenQA.Selenium;
using RegressionSuitAutomationPractice.Common;
using RegressionSuitAutomationPractice.TestSuit.Pages;
using System;
using TechTalk.SpecFlow;

namespace RegressionSuitAutomationPractice.TestSuit.StepDefinations
{
    [Binding]
    [Scope(Feature = "SignInPageFeature")]
    public class SignInPageSteps
    {
        private IWebDriver webDriver;
        private SignInPage _signInPage;

        public SignInPageSteps(TestContexts contexts)
        {
            
            this.webDriver = contexts.GetContextWebDriver();
        }

        [Given(@"the user has already loaded the application")]
        public void GivenTheUserHasAlreadyOpenedApplication()
        {
            //var signInPage = new SignInPage(webDriver,Hooks.logReporter);
            _signInPage = ObjectFactory.CreatePage<SignInPage>(webDriver);            
            webDriver.NavigateToUrl(_signInPage.FetchURL());
        }

        [Given(@"User enters (.*) and (.*) in Sign in Page")]
        public void GivenUserEntersAnd(string email, string pass)
        {
            //need to create Landing page POM but now i am hitting it directly
            webDriver.FindElement(By.PartialLinkText("Sign in")).Click();_signInPage.Pause(3000);

            _signInPage.InputSignLoginDetails(email, pass);
        }
        
        [When(@"user clicks (.*) button")]
        public void WhenUserClicksButton(string button)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"verify below validation messages")]
        public void ThenVerifyBelowValidationMessages()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
