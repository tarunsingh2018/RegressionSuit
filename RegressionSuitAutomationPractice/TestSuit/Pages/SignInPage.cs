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
        private static Logger logger;
        public IWebDriver driver;
        private ExtentTest logReporter;

        public SignInPage(IWebDriver driver, ExtentTest reporter) : base(driver, reporter)
        {
            logger = LogManager.GetCurrentClassLogger();
            this.logReporter = reporter;
            this.driver = driver;
        }

        public SignInPage()
        {}

        public string FetchURL()
        {
            //logger.Info("Pace Login Page is open");
            return NavigateToURL();            
            

        }

        internal void InputSignLoginDetails(string email, string pass)
        {
            throw new NotImplementedException();
        }
    }
}
