using Framework.MarketIT.Automation_Framework.Managers;
using Framework.MarketIT.Automation_Framework.Utilities.Enum;
using NUnit.Framework;
using System;
using System.Configuration;
using System.IO;
using System.Xml;

namespace Framework.MarketIT.Automation_Framework.DataProvider
{
    public class ConfigFileReader
    {
        public string GetCurrentProjectDirectory()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            baseDir = Directory.GetParent(Directory.GetParent(baseDir).ToString()).FullName;
            baseDir = Directory.GetParent(baseDir).ToString();
            return baseDir;
        }

        public string GetConfigApplicationName()
        {
            return GetConfigValueFor("ApplicationName");
        }

        public string GetConfigApplicationLogoPath()
        {
            return GetConfigValueFor("ApplicationLogoPath");
        }

        public string GetConfigResultIisLocation()
        {
            return GetConfigValueFor("ResultIisLocation");
        }

        public string GetURL()
        {
            return ConfigurationManager.AppSettings["URL"];
        }

        public string GetUserName()
        {
            return GetConfigValueFor("UserName");
        }

        public string GetPassword()
        {
            return GetConfigValueFor("Password");
        }

        public int GetTestRunOnGrid()
        {
            int gridTestRun = 0;
            string gridRun = ConfigurationManager.AppSettings["RunTestsOnGrid"];
            if (!string.IsNullOrEmpty(gridRun))
            {
                gridTestRun = int.Parse(gridRun);
                if (gridTestRun == 1)
                    return gridTestRun;
            }

            return gridTestRun;
        }

        public bool IsReportDownloadPathCustomized()
        {
            bool isDownloadCustomized = false;
            string customReportPath = GetConfigValueFor("CustomReportDownload");
            if (!string.IsNullOrEmpty(customReportPath))
            {
                isDownloadCustomized = bool.Parse(customReportPath);
                return isDownloadCustomized;
            }

            return isDownloadCustomized;
        }

        public string GetCountry()
        {
            return GetConfigValueFor("Country");
        }

        public string GetEnvironment()
        {
            return GetConfigValueFor("Environment");
        }

        public string GetScreenshotFolderPath()
        {
            string scrnShotPath = GetConfigValueFor("ScreenShotPath");

            return GetCurrentProjectDirectory() + scrnShotPath;
        }

        /**
         * Method to retrieve the key value from the command prompt
         **/
        public string GetValueFromClp(string paramKey)
        {
            return TestContext.Parameters.Get(paramKey);
        }

        public string GetValueFromAppConfig(string configKey)
        {
            string configValue = "";
            try
            {
                configValue = ConfigurationManager.AppSettings[configKey];
            }
            catch (Exception)
            {
                return configValue;
            }

            return configValue;
        }

        public Browser GetTargetBrowser()
        {
            Browser testBrowser = Browser.CHROME;
            string browser = ConfigurationManager.AppSettings["Browser"];

            if (!string.IsNullOrEmpty(browser))
            {
                if (!testBrowser.ToString().Equals(browser.ToUpper()))
                {
                    string browserCode = GetEnumData(browser).ToUpper();
                    switch (browserCode)
                    {
                        case "CHROME":
                            testBrowser = Browser.CHROME;
                            break;
                        case "IE":
                            testBrowser = Browser.IE;
                            break;
                        case "EDGE":
                            testBrowser = Browser.EDGE;
                            break;
                        case "FIREFOX":
                            testBrowser = Browser.FIREFOX;
                            break;
                        default:
                            break;
                    }

                }
            }

            return testBrowser;
        }

        public static string GetEnumData(string testEnum)
        {
            string enumstring = null;
            foreach (string value in Enum.GetNames(typeof(Browser)))
            {
                if (value.Equals(testEnum))
                {
                    enumstring = value;
                    break;
                }

            }
            return enumstring;
        }

        public string GetConfigResultFile()
        {
            return GetConfigValueFor("ResultFile");
        }

        public string GetConfigResultDashboardFile()
        {
            return GetConfigValueFor("ResultDashboardFile");
        }

        public string GetConfigRunModule()
        {
            return GetConfigValueFor("RunModule");
        }

        public string GetConfigResultDirectory()
        {
            return GetConfigValueFor("ResultDirectory");
        }

        public string GetConfigLogDirectory()
        {
            return GetConfigValueFor("LogDirectory");
        }

        public string GetConfigLogFile()
        {
            return GetConfigValueFor("LogFile");
        }

        public string GetProjectLogFilePath()
        {
            string logDirectory = GetConfigValueFor("LogDirectory");
            string logFileName = GetConfigValueFor("LogFile");
            return logDirectory + logFileName;
        }

        public string GetProjectInternalLogFilePath()
        {
            string logDirectory = GetConfigValueFor("LogDirectory");
            string InternalLogFileName = GetConfigValueFor("InternalLogFile");
            return logDirectory + InternalLogFileName;
        }

        public string GetProjectResultFilePath()
        {
            string resultDirectory = GetConfigValueFor("ResultDirectory");
            string resultFileName = GetConfigValueFor("ResultFile");
            return resultDirectory + resultFileName;
        }

        public string GetProjectResultDashboardFilePath()
        {
            string resultDirectory = GetConfigValueFor("ResultDirectory");
            string resultFileName = GetConfigValueFor("ResultDashboardFile");
            return resultDirectory + resultFileName;
        }

        public string GetConfigEnvironment()
        {
            return GetConfigValueFor("Environment");
        }

        public string GetConfigCountry()
        {
            return GetConfigValueFor("Country");
        }

        public string GetConfigSendMailTo()
        {
            return GetConfigValueFor("SendMailTo");
        }

        public string GetConfigSendMailFrom()
        {
            return GetConfigValueFor("SendMailFrom");
        }

        public bool IsFirstInExecution()
        {
            bool IsItFirst = false;
            string IsFirstExecution = GetConfigValueFor("IsFirst");
            if (!string.IsNullOrEmpty(IsFirstExecution))
            {
                IsItFirst = bool.Parse(IsFirstExecution);
                return IsItFirst;
            }


            return IsItFirst;
        }

        public bool IsLastInExecution()
        {
            bool IsItLast = false;
            string IsLastExecution = GetConfigValueFor("IsLast");
            if (!string.IsNullOrEmpty(IsLastExecution))
            {
                IsItLast = bool.Parse(IsLastExecution);
                return IsItLast;
            }


            return IsItLast;
        }

        public bool IsExecutionMarkedForEndOfCountry()
        {
            bool IsItEndOfCountry = false;
            string IsItCountryEnd = GetConfigValueFor("IsEndOfCountry");
            if (!string.IsNullOrEmpty(IsItCountryEnd))
            {
                IsItEndOfCountry = bool.Parse(IsItCountryEnd);
                return IsItEndOfCountry;
            }


            return IsItEndOfCountry;
        }

        public bool IsInternalLoggingEnabled()
        {
            bool IsInternalLogNeeded = false;
            string IsInternalLoggingEnabled = GetConfigValueFor("IsInternalLoggingEnabled");
            if (!string.IsNullOrEmpty(IsInternalLoggingEnabled))
            {
                IsInternalLogNeeded = bool.Parse(IsInternalLoggingEnabled);
                return IsInternalLogNeeded;
            }


            return IsInternalLogNeeded;
        }

        public string GetEnvironmentUrlFromXml()
        {
            string envUrl = null;
            string currentDirPath = GetCurrentProjectDirectory();
            string fileName = currentDirPath + "/Environments.xml";
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(fileName);

            XmlNodeList xmlNodes = xDoc.SelectNodes("/Environments");
            foreach (XmlNode xmlNode in xmlNodes)
            {
                XmlElement element = (XmlElement)xmlNode;
                //hereapp.config should be send here
                envUrl = xmlNode[GetEnvironment()].InnerText;
                Console.WriteLine(envUrl);
            }
            return envUrl;
        }

        public string GetConfigValueFor(string configKey)
        {
            string keyValue = "";
            string paramValue = GetValueFromClp(configKey);
            string appConfigValue = GetValueFromAppConfig(configKey);
            if (!string.IsNullOrEmpty(paramValue))
                keyValue = paramValue;
            else if (!string.IsNullOrEmpty(appConfigValue))
                keyValue = appConfigValue;
            else
                throw new Exception("Could not find value for key[" + configKey + "] in command parameter or app.config file");

            return keyValue;
        }

        public string GetPostMatrices()
        {
            return GetConfigValueFor("PostMetriceRun");
        }
    }
}
