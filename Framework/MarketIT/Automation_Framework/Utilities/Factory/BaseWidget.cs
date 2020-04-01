using OpenQA.Selenium;
using System;

namespace Framework.MarketIT.Automation_Framework.Utilities.Factory
{
    public class BaseWidget
    {
        protected IWebDriver WebDriver;
        
        public virtual void Initialize(IWebDriver webDriver)
        {
            if (webDriver == null) throw new ArgumentNullException("webDriver");
            WebDriver = webDriver;
            OpenQA.Selenium.Support.PageObjects.PageFactory.InitElements(webDriver, this);
        }
    }
}