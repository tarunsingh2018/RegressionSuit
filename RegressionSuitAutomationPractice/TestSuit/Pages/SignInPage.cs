using AventStack.ExtentReports;
using Framework.MarketIT.Automation_Framework.Utilities.Common;
using NLog;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegressionSuitAutomationPractice.TestSuit.Pages
{
    public class SignInPage : BasePage
    {
        //private static Logger logger;
        //public IWebDriver driver;
        //private ExtentTest logReporter;

        //public SignInPage(IWebDriver driver) : base(driver)
        //{
        //    logger = LogManager.GetCurrentClassLogger();
        //    //this.logReporter = reporter;
        //    this.driver = driver;
        //}

        //public SignInPage()
        //{}

        //Xpaths for Sign in Page
        By SignInButton = By.XPath("//*[@id='SubmitLogin']");
        By SignInPageErrorMessage = By.XPath("//div[@class='alert alert-danger']/ol/li");
        By SignInPageErrorHeader= By.XPath("//div[@class='alert alert-danger']/p");
        By SignInPageEmail = By.XPath("//*[@id='email']");
        By SignInPagePassword = By.XPath("//*[@id='passwd']");

        public void ClickOnSignInButton()
        {
            Click(SignInButton);
            Pause(1000);
        }

        public string FetchURL()
        {
            //logger.Info("Pace Login Page is open");
            return NavigateToURL();
        }

        public void InputSignLoginDetails(string email, string pass)
        {
             
            WriteInInputBox(SignInPageEmail, email.Equals("<blank>") ? string.Empty : email);
            WriteInInputBox(SignInPagePassword, pass.Equals("<blank>") ? string.Empty : pass);
        }

        public string GetSignInValidationMessageText()
        {
            return driver.FindElement(SignInPageErrorMessage).Text.Trim();
        }

        public string GetSignInValidationMessageHeaderText()
        {
            return driver.FindElement(SignInPageErrorHeader).Text.Trim();
        }
    }
}
