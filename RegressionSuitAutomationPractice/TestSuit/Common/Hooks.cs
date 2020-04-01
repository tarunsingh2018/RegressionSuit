using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using Framework.MarketIT.Automation_Framework.FunctionLibrary;
using Framework.MarketIT.Automation_Framework.Helpers;
using Framework.MarketIT.Automation_Framework.Managers;
using NUnit.Framework.Internal;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using TechTalk.SpecFlow;
using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Targets;

namespace RegressionSuitAutomationPractice.Common
{

    [Binding]
    public class Hooks
    {
        static TestContexts testContexts;
        public static ExtentTest scenario;
        public static ExtentTest logReporter;
        public static ExtentTest errorReporter;
        private static ExtentTest featureName;
        private static ExtentReports extent;
        private static NLog.Logger logger;
        private static GeneralUtilities generalUtilities;
        private static SendReport sendReport;
        private static CopyResult copyResultObj;

        private static int passed = 0;
        private static int totalStepsInScenario = 0;
        private static int failed = 0;
        private static int blocked = 0;
        private static int executionTime = 0;
        private static Stopwatch stopwatch;
        private static int stepPassed;
        private static int stepFailed;
        

        public Hooks(TestContexts contexts)
        {
            testContexts = contexts;
        }

        [BeforeTestRun]
        public static void InitializeReport()
        {
            //stopwatch = new Stopwatch();
            //stopwatch.Start();
            //generalUtilities = GeneralUtilities.GetInstance();
            //sendReport = new SendReport();
            //copyResultObj = new CopyResult();
            //SetUpNLogConfiguration();
            ////Initialize Extent report before test starts
            //string resultFilePath = generalUtilities.GetResultFilePath();
            //string resultFolder = generalUtilities.GetProjectResultDirectory();
            //Directory.Delete(resultFolder, true);
            //Directory.CreateDirectory(resultFolder);
            //var htmlReporter = new ExtentHtmlReporter(resultFilePath);
            //htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
            //htmlReporter.Config.DocumentTitle = "MarketIT Automation Test Report";
            //htmlReporter.Config.ReportName = "MarketIT Automation Test Report";

            ////Attach report to reporter
            extent = new ExtentReports();
            //extent.AttachReporter(htmlReporter);
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            //Create dynamic feature name
            featureName = extent.CreateTest<Feature>(FeatureContext.Current.FeatureInfo.Title);
        }

        [BeforeScenario]
        public static void Initialize()
        {
            //if (!stopwatch.IsRunning) stopwatch.Start();

            ////Create dynamic scenario name
            //scenario = featureName.CreateNode<Scenario>(ScenarioContext.Current.ScenarioInfo.Title);
        }

        [BeforeStep]
        public void BeforeStep()
        {
            //var stepType = ScenarioStepContext.Current.StepInfo.StepDefinitionType.ToString();
            //var stepVerbose = ScenarioStepContext.Current.StepInfo.Text;
            //if (stepType == "Given")
            //    logReporter = scenario.CreateNode<Given>(stepVerbose);
            //else if (stepType == "When")
            //    logReporter = scenario.CreateNode<When>(stepVerbose);
            //else if (stepType == "Then")
            //    logReporter = scenario.CreateNode<Then>(stepVerbose);
            //else if (stepType == "And")
            //    logReporter = scenario.CreateNode<And>(stepVerbose);
        }

        [AfterStep]
        public static void InsertReportingSteps()
        {
            var stepType = ScenarioStepContext.Current.StepInfo.StepDefinitionType.ToString();
            PropertyInfo pInfo = typeof(ScenarioContext).GetProperty("TestStatus", BindingFlags.Instance | BindingFlags.NonPublic);
            if (ScenarioContext.Current.TestError != null)
            {
                stepFailed = stepFailed + 1;
                string base64Image = generalUtilities.GetChromeScreenShot();
                string failureStack = ScenarioContext.Current.TestError.Message;
                string stackTrace = ScenarioContext.Current.TestError.StackTrace;
                Exception innerException = ScenarioContext.Current.TestError.InnerException;

                if (innerException != null)
                    failureStack = innerException.ToString();
                else if (!string.IsNullOrEmpty(stackTrace))
                    failureStack = stackTrace;

                logReporter.Fail(failureStack).AddScreenCaptureFromPath(base64Image);
            }
            else
            {
                stepPassed = stepPassed + 1;
            }

        }

        [AfterScenario]
        public static void AfterEachScenario()
        {
            //bool flag = true;
            //string scenarioStatus = scenario.Status.ToString().ToUpper();
            //var scenarioTitle = ScenarioContext.Current.ScenarioInfo.Title;
            //var featureTitle = FeatureContext.Current.FeatureInfo.Title;
            //totalStepsInScenario = sendReport.CountTotalStepsInScenario(featureTitle, scenarioTitle);
            //switch (scenarioStatus)
            //{
            //    case "PASS":
            //        passed = passed + 1;
            //        break;

            //    case "FAIL":
            //        failed = failed + 1;
            //        break;

            //    default:
            //        break;
            //}
            ////testContexts.GetWebDriverManager().CloseDriver();
            ///* for every scenario reporting in mail*/

            //executionTime = Convert.ToInt16(stopwatch.Elapsed.TotalSeconds);
            //copyResultObj.OrganiseResultDateWise();
            //int stepPassedPct = (stepPassed * 100) / (totalStepsInScenario);
            //sendReport.ProcessResultsForEmailReport(passed, failed, blocked, executionTime, scenarioTitle, featureTitle, stepPassedPct);
            //passed = 0;
            //failed = 0;
            //blocked = 0;
            //stepFailed = 0;
            //stepPassed = 0;
            //totalStepsInScenario = 0;
            //stopwatch.Restart();

        }



        [AfterFeature]
        public static void TearDown()
        {
            //testContexts.GetWebDriverManager().CloseDriver();

            /* executionTime = featureName.Model.RunDuration.Minutes;
             copyResultObj.OrganiseResultDateWise();
             sendReport.ProcessResultsForEmailReport(passed, failed, blocked, executionTime);*/
            passed = 0;
            failed = 0;
            blocked = 0;
            //stopwatch.Restart();
        }

        [AfterTestRun]
        public static void TearDownReport()
        {
            SetUpNLogConfiguration();
            sendReport.ProcessConsolidatedReport(passed, failed, blocked);
            extent.Flush();
            sendReport.Broadcast();
            ////-----------------------------------------------
            ////Post matrices call 
            //var configFileReader = FileReaderManager.GetInstance().GetConfigReader();
            ////  if (configFileReader.GetPostMatrices() == "false")
            //{
            //    string workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //    string projectDirectory = Directory.GetParent(Directory.GetParent(workingDirectory).ToString()).ToString();
            //    string jsonFile = projectDirectory + "\\Common\\" + "PostMetriceKeys.json";
            //    var reportLst = (BasicFileReporter)extent.StartedReporterList[0];

            //    var startDt = reportLst.StartTime.ToString("yyyy-MM-ddTHH:mm:ss");//, System.Globalization.CultureInfo.InvariantCulture);
            //    var endDt = reportLst.EndTime.ToString("yyyy-MM-ddTHH:mm:ss");//, System.Globalization.CultureInfo.InvariantCulture);

            //    // PostMetrice.Main(jsonFile, extent.Stats.ChildCountPass, extent.Stats.ChildCountFail, extent.Stats.ChildCountSkip, startDt, endDt);
            //}



            ////-----------------------------------------------
            LogManager.Shutdown();
        }

        public static void SetUpNLogConfiguration()
        {
            //Initialize Extent report before test starts
            string logFilePath = generalUtilities.GetLogFilePath();
            string internalLogFilePath = generalUtilities.GetInternalLogFilePath();
            var nlogConfig = new LoggingConfiguration();
            var fileTarget = new FileTarget("logfile")
            {
                FileName = logFilePath,
                Layout = "${longdate} ${level} ${message} ${exception}",
                DeleteOldFileOnStartup = true
            };
            nlogConfig.AddTarget(fileTarget);
            nlogConfig.AddRuleForAllLevels(fileTarget);

            var consoleTarget = new ColoredConsoleTarget("console")
            {
                Layout = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception}"
            };
            nlogConfig.AddTarget(consoleTarget);
            nlogConfig.AddRuleForAllLevels(consoleTarget);

            bool isInternalLogNeeded = FileReaderManager.GetInstance().GetConfigReader().IsInternalLoggingEnabled();
            if (isInternalLogNeeded)
            {
                InternalLogger.LogFile = internalLogFilePath;
                InternalLogger.LogWriter = new StringWriter();
                InternalLogger.LogLevel = LogLevel.Trace;
            }
            LogManager.Configuration = nlogConfig;
            LogManager.ThrowExceptions = true;
            logger = LogManager.GetCurrentClassLogger();
        }
    }


}

