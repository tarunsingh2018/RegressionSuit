using Framework.MarketIT.Automation_Framework.Managers;
using Framework.MarketIT.Automation_Framework.Utilities.Browser;
using NLog;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.MarketIT.Automation_Framework.FunctionLibrary
{
    public class GeneralUtilities
    {
        private static GeneralUtilities generalUtilities;
        private static Logger logger;
        private static readonly string BASE64_PREFIX        = "data:image/png;base64,";
        private static readonly string DEFAULT_DATE_FORMAT  = "dd-MM-yyyy";
        private static readonly string HYPHEN_STRING        = "-";
        private static readonly string RUN_DIR_NAME         = "Run" + HYPHEN_STRING;
        private static readonly int DEFAULT_RUN             = 0;

        private GeneralUtilities()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        public static GeneralUtilities GetInstance()
        {
            if (generalUtilities == null)
                generalUtilities = new GeneralUtilities();

            return generalUtilities;
        }

        /*
        * Method name : getCurrentProjectDirectory
        * Purpose     : Method returns the user home directory
        * @param      : None
        * @return     : returns String
        */
        public string GetCurrentProjectDirectory()
        {
            return FileReaderManager.GetInstance().GetConfigReader().GetCurrentProjectDirectory();
        }

        public string GetResultFilePath()
        {
            string projectResultFile = FileReaderManager.GetInstance().GetConfigReader().GetProjectResultFilePath();
            return GetCurrentProjectDirectory() + projectResultFile;
        }

        public string GetResultDashboardFilePath()
        {
            string projectResultDashboardFile = FileReaderManager.GetInstance().GetConfigReader().GetProjectResultDashboardFilePath();
            return GetCurrentProjectDirectory() + projectResultDashboardFile;
        }

        public string GetLogFilePath()
        {
            string projectLogFile = FileReaderManager.GetInstance().GetConfigReader().GetProjectLogFilePath();
            return GetCurrentProjectDirectory() + projectLogFile;
        }

        public string GetInternalLogFilePath()
        {
            string projectLogErrorFile = FileReaderManager.GetInstance().GetConfigReader().GetProjectInternalLogFilePath();
            return GetCurrentProjectDirectory() + projectLogErrorFile;
        }

        public string GetProjectResultDirectory()
        {
            string projectResultDirectory = FileReaderManager.GetInstance().GetConfigReader().GetConfigResultDirectory();
            return GetCurrentProjectDirectory() + projectResultDirectory;
        }

        public string GetApplicationUrl()
        {
            return FileReaderManager.GetInstance().GetConfigReader().GetURL();
        }

        /**
        * Method name : getScreenshot
        * Purpose     : Method capture the screenshot and save to provided location and return the path of the screenshot
        * @param      : screenshotFolderPath - location of screenshot to be saved
        *               fileName - name to be given to screenshot
        * @return     : screenImg  - location of screen Image that is Saved
        */
        public string GetScreenshot(IWebDriver driver, string screenshotFolderPath, string fileName)
        {
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            Screenshot screenshot = ts.GetScreenshot();
            string pth = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string finalpth = pth.Substring(0, pth.LastIndexOf("bin")) + "ErrorScreenshots\\" + fileName + ".png";
            string localpath = new Uri(finalpth).LocalPath;
            screenshot.SaveAsFile(localpath, ScreenshotImageFormat.Png);

            return localpath;
        }

        public string GetChromeScreenShot()
        {
            //Screenshot pageScreenShot = Chrome.GetChromeDriver().GetFullPageScreenshot();
            //string base64Image = BASE64_PREFIX + pageScreenShot.AsBase64EncodedString;

            return "abc";
        }

        /**
        * Method name : appendTimeStamp
        * Purpose     : Method appends the time stamp (with format: ddMMyyyyHHmmss) to the
        * text which is passed to it and returns the same text with time stamp
        * @param      : text
        * @return     : textWithTimestamp - A String value appended with time stamp
        */
        public string AppendTimeStamp(string text)
        {
            string textWithTimestamp = "";
            DateTime dt = DateTime.Now; // Or whatever
            string s = dt.ToString("yyyyMMddHHmmss");
            textWithTimestamp = text + s;

            return textWithTimestamp;
        }

        public void CreateFolder(string folderPath)
        {
            Directory.CreateDirectory(folderPath);
        }

        public string GetCurrentDateString()
        {
            DateTime dateTime = DateTime.Now;
            string dateString = dateTime.ToString(DEFAULT_DATE_FORMAT);

            return dateString;
        }

        public int GetNextTestRunNumber(string runDirectory)
        {
            int lastRunNumber = GetLastTestRunNumber(runDirectory);

            return lastRunNumber + 1;
        }

        public int GetLastTestRunNumber(string runDirectory)
        {
            List<int> allRunList = new List<int>();
            string[] runDirectories = Directory.GetDirectories(runDirectory);
            foreach (string directoryName in runDirectories)
            {
                if (directoryName.Contains(RUN_DIR_NAME))
                {
                    string[] runNumberSplit = directoryName.Split(new[] { RUN_DIR_NAME }, StringSplitOptions.None);
                    int runNumber = int.Parse(runNumberSplit[1]);
                    allRunList.Add(runNumber);
                }
            }

            if (allRunList.Count == 0)
            {
                return DEFAULT_RUN;
            }
            else
            {
                int lastRun = allRunList.Max();
                return lastRun;
            }
        }
    }
}
