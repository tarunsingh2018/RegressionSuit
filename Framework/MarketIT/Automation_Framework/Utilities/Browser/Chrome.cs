using Framework.MarketIT.Automation_Framework.Managers;
using Framework.MarketIT.Automation_Framework.Utilities.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;

namespace Framework.MarketIT.Automation_Framework.Utilities.Browser
{
    public class Chrome
    {
        private static ChromeForFullScreenShots chromeDriver    = null;
        //private static ChromeDriver chromeDriver = null;
        public static string USER_DIR                           = FileReaderManager.GetInstance().GetConfigReader().GetCurrentProjectDirectory();
        
        public static ChromeForFullScreenShots GetChromeDriver()
        {
            return chromeDriver;
        }
        //public static ChromeDriver GetChromeDriver()
        //{
        //    if (!chromeDriver == null) return chromeDriver;
        //    return GetDriver();

        //}

        public static void ResetChromeDriver()
        {
            chromeDriver = null;
        }

        public static IWebDriver GetDriver()
        {
            if (GetChromeDriver() == null)
            {
                ChromeOptions options = new ChromeOptions();
                //if (CUSTOM_REPORT_DOWNLOAD == true)
                //{
                //    DirectoryInfo downloadFilepath = CreateDir();
                //    options.AddUserProfilePreference("download.default_directory", downloadFilepath);
                //    options.AddUserProfilePreference("download.prompt_for_download", false);
                //    options.AddUserProfilePreference("disable-popup-blocking", "true");
                //    //options.setExperimentalOption("prefs", chromePrefs);

                //}
                //options.AddArguments("--start-maximized");
                //options.AddArguments("disable-infobars");
                //options.AddAdditionalCapability("useAutomationExtension", false);
                //options.AddAdditionalCapability("elementScrollBehavior", 0);

                options.AddArguments("--disable-infobars");
                options.AddUserProfilePreference("download.default_directory", USER_DIR);
                options.AddUserProfilePreference("disable-popup-blocking", "true");
                //options.AddArguments(new List<string>() { "headless", "disable-gpu" });
                //options.AddArguments("--incognito");
                //chromeDriver = new ChromeDriver(options);
                chromeDriver = new ChromeForFullScreenShots(options);
                
            }

            return chromeDriver;
        }

        //public static DirectoryInfo CreateDir()
        //{
        //    SSRS_REPORTS_DOWNLOAD = Directory.CreateDirectory(SSRS_REPORTS_DIR);
        //    foreach (FileInfo file in SSRS_REPORTS_DOWNLOAD.EnumerateFiles())
        //    {
        //        file.Delete();
        //    }
        //    foreach (DirectoryInfo dir in SSRS_REPORTS_DOWNLOAD.EnumerateDirectories())
        //    {
        //        dir.Delete(true);
        //    }

        //    return SSRS_REPORTS_DOWNLOAD;
        //}
    }
}