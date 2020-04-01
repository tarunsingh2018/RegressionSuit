using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.MarketIT.Automation_Framework.Managers
{
    public class TestContexts
    {
        private WebDriverManager driverManager;
  
        public TestContexts()
        {
            if (driverManager == null) driverManager = new WebDriverManager();         
        }
       
        public IWebDriver GetContextWebDriver()
        {
            return driverManager.GetDriver();
        }
        
        public WebDriverManager GetWebDriverManager()
        {
            return driverManager;
        }
    }
}
