using Framework.MarketIT.Automation_Framework.FunctionLibrary;
using Framework.MarketIT.Automation_Framework.Managers;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.MarketIT.Automation_Framework.Helpers
{
    public class CopyResult
    {
        GeneralUtilities generalUtilitiesObj;
        private static Logger logger;
        private static readonly string HYPHEN_STRING = "-";
        private static readonly string RUN_DIR_NAME = "Run" + HYPHEN_STRING;

        public CopyResult()
        {
            generalUtilitiesObj = GeneralUtilities.GetInstance();
            logger = LogManager.GetCurrentClassLogger();
        }

        public void OrganiseResultDateWise()
        {
            var configFileReaderObj = FileReaderManager.GetInstance().GetConfigReader();
            string moduleName = configFileReaderObj.GetConfigRunModule();
            string resultFileName = configFileReaderObj.GetConfigResultFile();
            string resultDashboardFileName = configFileReaderObj.GetConfigResultDashboardFile();
            string resultDirectory = generalUtilitiesObj.GetProjectResultDirectory();
            string date = generalUtilitiesObj.GetCurrentDateString();
            string dateDir = resultDirectory + date;
            string moduleDir = dateDir + "\\" + moduleName;
            generalUtilitiesObj.CreateFolder(dateDir);
            generalUtilitiesObj.CreateFolder(moduleDir);
            int runNum = generalUtilitiesObj.GetNextTestRunNumber(moduleDir);
            string runDir = moduleDir + "\\" + RUN_DIR_NAME + runNum;
            generalUtilitiesObj.CreateFolder(runDir);
            string[] fileEntries = Directory.GetFiles(resultDirectory);
            foreach (string fileName in fileEntries)
            {
                string fileDestination = runDir + "\\" + resultFileName;
                string fileDashboardDestination = runDir + "\\" + resultDashboardFileName;
                if (fileName.Contains(resultFileName))
                    File.Copy(fileName, fileDestination, true);

                if (fileName.Contains(resultDashboardFileName))
                    File.Copy(fileName, fileDashboardDestination, true);
            }
        }
    }
}

