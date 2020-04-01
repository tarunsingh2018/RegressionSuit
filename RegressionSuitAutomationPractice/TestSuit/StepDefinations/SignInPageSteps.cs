using Framework.MarketIT.Automation_Framework.Managers;
using Framework.MarketIT.Automation_Framework.Utilities.Factory;
using Framework.MarketIT.Automation_Framework.Extensions;
using OpenQA.Selenium;
using RegressionSuitAutomationPractice.Common;
using RegressionSuitAutomationPractice.TestSuit.Pages;
using System;
using TechTalk.SpecFlow;
using Framework.MarketIT.Automation_Framework.Utilities.Common;
using NUnit.Framework;

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
            _signInPage.ClickOnSignInButton();
        }
        
        [Then(@"verify below error (.*) on the page")]
        public void ThenVerifyBelowErrorOnThePage(string errorMessage)
        {            
            Assert.Multiple(() =>
            {
                Assert.AreEqual(_signInPage.GetSignInValidationMessageHeaderText(), "There is 1 error");
                Assert.AreEqual(_signInPage.GetSignInValidationMessageText(), errorMessage);
            });
        }


    }
}
