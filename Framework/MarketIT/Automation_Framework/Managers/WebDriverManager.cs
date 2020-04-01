using Framework.MarketIT.Automation_Framework.Utilities.Browser;
using Framework.MarketIT.Automation_Framework.Utilities.Enum;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Framework.MarketIT.Automation_Framework.Managers
{
    public class WebDriverManager
    {
        private IWebDriver driver;
        public static int runTestInGrid;
        private string url;
        private static Browser browserType;

        public WebDriverManager()
        {
            url = FileReaderManager.GetInstance().GetConfigReader().GetURL();
            browserType = FileReaderManager.GetInstance().GetConfigReader().GetTargetBrowser();
            runTestInGrid = FileReaderManager.GetInstance().GetConfigReader().GetTestRunOnGrid();
        }

        public IWebDriver GetDriver()
        {
            if (driver == null) driver = CreateLocalDriver();

            return driver;
        }

        private IWebDriver CreateLocalDriver()
        {
            switch (runTestInGrid)
            {
                case 0:
                    {
                        switch (browserType)
                        {
                            case Browser.CHROME:
                                driver = Chrome.GetDriver();
                                break;

                            case Browser.IE:
                                driver = InternetExplorer.GetDriver();
                                break;

                            case Browser.FIREFOX:
                                driver = new FirefoxDriver();
                                break;

                            default:
                                break;
                        }
                        break;
                    }
                case 1:
                    {
                        switch (browserType)
                        {
                            case Browser.CHROME:
                                //driver = new RemoteWebDriver(getGridHubUrl(), Chrome.getDC());
                                //driver = new Augmenter().augment(driver);
                                break;
                            case Browser.IE:
                                break;
                            case Browser.FIREFOX:
                                break;
                            default:
                                break;
                        }
                        break;
                    }
                default:
                    break;
            }

            driver.Manage().Window.Maximize();
            //driver.Url = url;

            return driver;
        }

        public void CloseDriver()
        {
            switch (browserType)
            {
                case Browser.CHROME:
                    Chrome.ResetChromeDriver();
                    break;
                default:
                    break;
            }

            driver.Close();
            driver.Quit();
        }
    }
}

