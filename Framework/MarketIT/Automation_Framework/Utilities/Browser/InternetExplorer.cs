using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.MarketIT.Automation_Framework.Utilities.Browser
{
    public class InternetExplorer
    {
        private static InternetExplorerDriver ieDriver = null;

        public static InternetExplorerDriver GetIeDriver()
        {
            return ieDriver;
        }

        public static void SetIeDriver(InternetExplorerDriver ieDriver)
        {
            InternetExplorer.ieDriver = ieDriver;
        }

        public static IWebDriver GetDriver()
        {
            if (GetIeDriver() == null)
            {
                InternetExplorerOptions options = new InternetExplorerOptions()
                {
                    EnableNativeEvents = true,
                    IgnoreZoomLevel = true
                };
                IWebDriver ieDriver = new InternetExplorerDriver(options);

                //options.AddAdditionalCapability("INTRODUCE_FLAKINESS_BY_IGNORING_SECURITY_DOMAINS", true);
                //options.AddAdditionalCapability("ENABLE_PERSISTENT_HOVERING", true);
                //options.AddAdditionalCapability("NATIVE_EVENTS", false);
                //capabilities.SetJavascriptEnabled(true);
                //ieDriver = new InternetExplorerDriver(capabilities);
                //sIWebDriver ieDriver = new InternetExplorerDriver();
            }

            return ieDriver;
        }

        //public static DesiredCapabilities getDC()
        //{
        //    DesiredCapabilities dc = DesiredCapabilities.internetExplorer();
        //    //dc.setBrowserName("internet explorer");
        //    //dc.setPlatform(SharedDriver.getPlatformObject());
        //    //dc.setVersion(SharedDriver.BROWSER_VERSION);
        //    //dc.setCapability(CapabilityType.TAKES_SCREENSHOT, true);

        //    return dc;
        //}
    }
}
