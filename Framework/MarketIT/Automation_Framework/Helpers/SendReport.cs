
using Framework.MarketIT.Automation_Framework.DataProvider;
using Framework.MarketIT.Automation_Framework.FunctionLibrary;
using Framework.MarketIT.Automation_Framework.Managers;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;

//using TechTalk.SpecFlow;

namespace Framework.MarketIT.Automation_Framework.Helpers
{
    public class SendReport
    {
        GeneralUtilities generalUtilitiesObj;
        ConfigFileReader configFileReader;
        private static Logger logger;
        private string _reportHtmlSource;
        bool isThisTheFirstRun;
        private string _lastFeatureTitle;

        //TODO This need to be taken care of when tests are run in dedicated automation machines for report links
        private readonly string RUN_REPORT_FILE_DIR = "/index.html";
        private readonly string OVERALL_STATUS_PASS = "{0}pc Pass";
        private readonly string OVERALL_STATUS_PASSED = "PASS";
        private readonly string OVERALL_STATUS_PASS_BG_COLOR = "#b5d6a7";
        private readonly string OVERALL_STATUS_PASS_TEXT_COLOR = "green";
        private readonly string OVERALL_PASS_HEADER_TEXT_COLOR = "#00A658";
        private readonly string OVERALL_STATUS_FAIL_BG_COLOR = "#ff9a9a";
        private readonly string OVERALL_STATUS_FAIL_TEXT_COLOR = "red";
        private static readonly string HYPHEN_STRING = "-";
        private readonly string RUN_DIR_NAME = "Run" + HYPHEN_STRING;
        private readonly string SMTP_SERVER = "<smtp_server_name>";
        private readonly int SMTP_PORT = 25;
        private readonly string REPORT_MAIL_SUBJECT_PLACEHOLDER = "{0} Automation Report ({1} {2}) (Overall - {3}% {4})";
        private readonly string REPORT_MAIL_HEADER_PLACEHOLDER = "Test Run Status ({0} {1}) ({2}/{3} <span style=\"color:{4}\">{5}</span>)";
        private readonly string ALL_PASS_PERCENTAGE_BAR = "<td width=\"{0}pc\" bgcolor=\"green\">\n";
        private readonly string ALL_FAIL_PERCENTAGE_BAR = "<td width=\"{0}pc\" bgcolor=\"red\">\n";

        private readonly string REPORT_HTML_HEADER_TEMPLATE =
                     //"<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\"\n" +
                     //        "        \"http://www.w3.org/TR/html4/loose.dtd\">\n" +
                     //        "<html>\n" +
                     //        "    <head></head>\n" +
                     //        "    <body>\n" +

                     ////daily report
                     //"        <table align=\"center\" style=\"background-color: #eee\" width=\"90pc\">\n" +
                     //"                            <tr height=\"15\" bgcolor=\"gray\" style=\"color:#000000; font-weight:bold;\">\n" +
                     //"                                        <td colspan=\"8\" align=\"center\" style=\"padding-left:5px; background-color: #eee\">TCR Automation Daily Status Report</td>\n" +
                     //"                                    </tr>\n" +
                     //"                            <tr height=\"12\" bgcolor=\"gray\" style=\"color:#000000; font-weight:bold;\">\n" +
                     //"                                        <th width=\"10pc\">Name</th>\n" +
                     //"                                        <th width=\"30pc\">Stories</th>\n" +
                     //"                                        <th width=\"12c\">Status</th>\n" +
                     //"                                        <th width=\"10pc\">Start date</th>\n" +
                     //"                                        <th width=\"10pc\">Estimated End Date</th>\n" +
                     //"                                        <th width=\"10pc\">Actual End Date</th>\n" +
                     //"                                        <th width=\"10pc\">Complete %</th>\n" +
                     //"                                        <th width=\"8pc\">Impediments</th>\n" +
                     //"                                    </tr>\n" +
                     ////Abhishek
                     //"                            <tr height=\"12\" bgcolor=\"#ffffff\" >\n" +
                     //"                                        <td align=\"left\" style=\"padding-left:5px\">Abhishek</td>\n" +
                     //"                                        <td align=\"left\">bdd creation | induction module</td>\n" +
                     //"                                        <td align=\"left\">In progress</td>\n" +
                     //"                                        <td align=\"left\">6-aug</td>\n" +
                     //"                                        <td align=\"left\">14-aug</td>\n" +
                     //"                                        <td align=\"left\">14-aug</td>\n" +
                     //"                                        <td align=\"center\">30%</td>\n" +
                     //"                                        <td align=\"center\"></td>\n" +
                     //"                                    </tr>\n" +

                     "                                  </table>\n\n\n" +
                    "        \n\n\n<table width=\"100pc\">\n" +
                    "                           <tr>\n</tr>" +
                    "        </table>\n" +

                    "        <table style=\"background-color: #eee\" width=\"100pc\">\n" +
                    "            <tr>\n" +
                    "                <td style=\"padding:20px 20px 0; text-align:center;\">\n" +
                    "                    <table bgcolor=\"#00a658\" width=\"100pc\">\n" +
                    "                        <tr>\n" +
                    "                            <td style=\"padding:10px; text-align:center;\">\n" +
                    "                                <img src=\"{0}\">\n" +
                    "                            </td>\n" +
                    "                        </tr>\n" +
                    "                    </table>\n" +
                    "                </td>\n" +
                    "            </tr>\n" +
                    "            <tr>\n" +
                    "                <td style=\"padding:20px 20px 0; text-align:center;\">\n" +
                    "                    <table border=\"0\" cellpadding=\"10\" cellspacing=\"1\"  width=\"100pc\" style=\"font-family:arial; font-size:14px;\">\n" +
                    "                        <tr>\n" +
                    "                            <td>\n" +
                    "                                <h2 style=\"font-size:24px; font-family:arial; margin:0 0 20px; color:#00A658; text-align:center\">{1}</h2>\n";

        private readonly string REPORT_HTML_MODULE_TABLE_TEMPLATE =
                    "                        <table width=\"100pc\" cellpadding=\"0\">\n" +
                    "                                    <tr height=\"30\" bgcolor=\"#00a658\" style=\"color:#ffffff;\">\n" +
                    "                                        <th width=\"37pc\">Test Scenarios</th>\n" +
                    "                                        <th width=\"5pc\">Country</th>\n" +
                    "                                        <th width=\"6pc\">Passed</th>\n" +
                    "                                        <th width=\"6pc\">Failed</th>\n" +
                    "                                        <th width=\"6pc\">Skipped</th>\n" +
                    "                                        <th width=\"10pc\">Overall Status</th>\n" +
                    "                                        <th width=\"8pc\">Pass %</th>\n" +
                    "                                        <th width=\"8pc\">Time Taken (in seconds)</th>\n" +
                    "                                    </tr>\n";

        private readonly string REPORT_HTML_MODULE_ROW_TEMPLATE =
                    "                            <tr height=\"15\" bgcolor=\"#ffffff\" >\n" +
                    "                                        <td align=\"left\" style=\"padding-left:5px;word-wrap:break-word;\">{0}</td>\n" +
                    "                                        <td align=\"center\">{1}</td>\n" +
                    "                                        <td align=\"center\" style=\"color:green\"><b>{2}</b></td>\n" +
                    "                                        <td align=\"center\" style=\"color:red\"><b>{3}</b></td>\n" +
                    "                                        <td align=\"center\" style=\"color:orange\"><b>{4}</b></td>\n" +
                    //   "                                        <td align=\"center\" style=\"color: {6};\" bgcolor=\"{5}\">{8}</td>\n" +
                    "                                        <td>\n" +
                    "                                            <table width=\"100pc\" cellspacing=\"0\">\n" +
                    "                                                <tr height=\"12\">\n{5}  </tr>\n" +
                    //"                                                
                    "                                            </table>\n" +
                    "                                        </td>\n" +
                   "                                        <td align=\"center\">{8}</td>\n" +
                     "                                        <td align=\"center\">{9}</td>\n" +
                    //"                                        <td align=\"center\"><a href=\"{10}\" target=\"_blank\" title=\"{11}\">Click To View</a></td>\n" +
                    "                                    </tr>\n";

        private readonly string REPORT_HTML_MODULE_TABLE_END =
                    "                        </table>\n" +
                   "                        <table><tr height=\"30\"></tr></table>";

        private readonly string REPORT_HTML_FOOTER_TEMPLATE =
                    "                    </td>\n" +
                    "                        </tr>\n" +
                    "                        <tr>\n" +
                    "                            <td style=\"text-align:left; padding:8px 8px 10px;\">\n" +
                    "                                <img width=\"66\">\n" +
                    "                            </td>\n" +
                    "                        </tr>\n" +
                    "                    </table>\n" +
                    "                </td>\n" +
                    "            </tr>\n" +
                    "        </table>\n" +
                    "    </body>\n" +
                    "</html>";

        public SendReport()
        {
            generalUtilitiesObj = GeneralUtilities.GetInstance();
            configFileReader = FileReaderManager.GetInstance().GetConfigReader();
            logger = LogManager.GetCurrentClassLogger();
            isThisTheFirstRun = configFileReader.IsFirstInExecution();
        }

        public void SendEmail(string htmlSourceToEmbed, string subject, string toAddresses, string fromAddresses)
        {
            MailMessage mailMessage = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Port = SMTP_PORT;
            client.Host = SMTP_SERVER;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = true;
            mailMessage.From = new MailAddress(fromAddresses);
            //mailMessage.To.Add(new MailAddress(toAddresses));
            foreach (var address in toAddresses.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                mailMessage.To.Add(address);
            }

            string workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string projectDirectory = Directory.GetParent(Directory.GetParent(workingDirectory).ToString()).ToString();
            string testResultPath = projectDirectory + "\\TestResults\\";
            DirectoryInfo lastModifiedDirect = new DirectoryInfo(testResultPath).GetDirectories().OrderByDescending(d => d.LastWriteTimeUtc).First();
            string  execStatusPath = testResultPath + lastModifiedDirect + "\\stats.json";
            Attachment attachment = new Attachment(execStatusPath);
            
            mailMessage.Attachments.Add(attachment);

            mailMessage.Subject = subject;
            mailMessage.Body = htmlSourceToEmbed;
            mailMessage.IsBodyHtml = true;
            client.Send(mailMessage);
        }

        
        public void CreateStatsJsonOrModifyContentIfExist(string executionDay, int passed, int failed, int blocked)
        {
            int totalCases = passed + failed + blocked;
            JObject statsObject = new JObject();
            //statsObject.Add("passed", passed);
            statsObject.Add("total_passed", passed);
            statsObject.Add("total_failed", failed);
            statsObject.Add("total_blocked", blocked);            
            statsObject.Add("total_cases", totalCases);
            string resultDirectory = generalUtilitiesObj.GetProjectResultDirectory();
            string moduleJsonFile = resultDirectory + "\\" + executionDay + "\\stats.json";
            if (File.Exists(moduleJsonFile))
            {
                JObject jsonObject = ReadFileAndGetContentAsJson(executionDay);
                int lastPassCount = int.Parse(jsonObject.GetValue("total_passed").ToString());
                int lastTcCount = int.Parse(jsonObject.GetValue("total_cases").ToString());
                int lastFailCount = int.Parse(jsonObject.GetValue("total_failed").ToString());
                int totalPassed = lastPassCount + passed;
                int totalTestCases = lastTcCount + totalCases;
                int totalFailed = lastFailCount + failed;

                statsObject.Property("total_passed").Value = totalPassed;
                statsObject.Property("total_failed").Value = totalFailed;
                statsObject.Property("total_cases").Value = totalTestCases;
                
                File.Delete(moduleJsonFile);
            }

            File.AppendAllText(moduleJsonFile, statsObject.ToString());
        }

        public void CreateFileOrAppendContentIfExist(string reportHtmlSource, string executionDay)
        {
            string resultDirectory = generalUtilitiesObj.GetProjectResultDirectory();
            string moduleFile = resultDirectory + "\\" + executionDay + "\\modules.txt";
            File.AppendAllText(moduleFile, reportHtmlSource);
        }

        public void WriteFinalEmailHtml(string executionDate, string emailContent)
        {
            string resultDirectory = generalUtilitiesObj.GetProjectResultDirectory();
            string reportFile = resultDirectory + "\\" + executionDate + "\\emailableReport.html";
            if (File.Exists(reportFile))
                File.Delete(reportFile);
            File.AppendAllText(reportFile, emailContent);
        }

        public string GetFinalReportHtmlSource(string appLogoPath, string headerText, string executionDay)
        {
            string resultDirectory = generalUtilitiesObj.GetProjectResultDirectory();
            string moduleTextPath = resultDirectory + "\\" + executionDay + "\\modules.txt";
            string moduleTextContent = File.ReadAllText(moduleTextPath, Encoding.UTF8);
            string htmlHeader = string.Format(REPORT_HTML_HEADER_TEMPLATE, appLogoPath, headerText);
            //StringBuilder stringBuilder = new StringBuilder(htmlHeader);
            //ReadExcel objExcel = new ReadExcel();
            StringBuilder stringBuilder = new StringBuilder();// new StringBuilder(objExcel.ReadExcelFile());
            stringBuilder.Append(htmlHeader);
            stringBuilder.Append(moduleTextContent);
            stringBuilder.Append(REPORT_HTML_FOOTER_TEMPLATE);
            string finalHtmlSource = stringBuilder.ToString();
            finalHtmlSource = finalHtmlSource.Replace("pc", "%");

            return finalHtmlSource;
        }

        public JObject ReadFileAndGetContentAsJson(string executionDay)
        {
            string resultDirectory = generalUtilitiesObj.GetProjectResultDirectory();
            string moduleJson = resultDirectory + "\\" + executionDay + "\\stats.json";
            JObject jsonObject = JObject.Parse(File.ReadAllText(moduleJson));

            return jsonObject;
        }

        public void DeleteModuleRowFile(string executionDay)
        {
            string resultDirectory = generalUtilitiesObj.GetProjectResultDirectory();
            string moduleTextPath = resultDirectory + "\\" + executionDay + "\\modules.txt";
            if (File.Exists(moduleTextPath))
               File.Delete(moduleTextPath);
        }

        //public void DeleteModuleJsonFile(string executionDay)
        //{
        //    string resultDirectory = generalUtilitiesObj.GetProjectResultDirectory();
        //    string moduleJsonPath = resultDirectory + "\\" + executionDay + "\\stats.json";
        //    if (File.Exists(moduleJsonPath))
        //        //File.Delete(moduleJsonPath);
        //}

        //public void ProcessResultsForEmailReport(int passed, int failed, int blocked, int timeTakenInMinutes, String module)
        //{
        //    string resultIisLocation = configFileReader.GetConfigResultIisLocation();
        //    string environment = configFileReader.GetConfigEnvironment().ToUpper();
        //    string country = configFileReader.GetConfigCountry().ToUpper();
        //    //string module = configFileReader.GetConfigRunModule();
        //    string reportDir = generalUtilitiesObj.GetProjectResultDirectory();


        //    bool isThisTheLastRun = configFileReader.IsLastInExecution();
        //    //bool isThisTheFirstRun = configFileReader.IsFirstInExecution();
        //    bool isEndOfCountry = configFileReader.IsExecutionMarkedForEndOfCountry();

        //    // This section will do cumulative calculation on the extracted information above for use in the report email
        //    int totalTestCases = passed + failed + blocked;
        //    int passPercentage = (passed * 100) / totalTestCases;
        //    int failPercentage = 100 - passPercentage;
        //    string overallStatusBar;
        //    if (passPercentage == 100)
        //    {
        //        overallStatusBar = string.Format(ALL_PASS_PERCENTAGE_BAR, "100");
        //    }
        //    else if (passPercentage == 0)
        //    {
        //        overallStatusBar = string.Format(ALL_FAIL_PERCENTAGE_BAR, "100");
        //    }
        //    else
        //    {
        //        string passTd = string.Format(ALL_PASS_PERCENTAGE_BAR, passPercentage);
        //        string failTd = string.Format(ALL_FAIL_PERCENTAGE_BAR, failPercentage);
        //        overallStatusBar = string.Concat(passTd, failTd);
        //    }

        //    // This section will create result link for viewing through the report
        //    string executionDay = generalUtilitiesObj.GetCurrentDateString();
        //    string resultDirectory = generalUtilitiesObj.GetProjectResultDirectory();
        //    string dateDir = resultDirectory + "\\" + executionDay;
        //    string moduleDir = dateDir + "\\" + module;
        //    int lastRun = generalUtilitiesObj.GetLastTestRunNumber(moduleDir);
        //    string iisResultLocation = resultIisLocation + "\\" + executionDay + "\\" + module + "\\" + RUN_DIR_NAME
        //            + lastRun + "\\" + RUN_REPORT_FILE_DIR;

        //    // This section will create different color coding for pass, failures etc.
        //    string backgroundColor = OVERALL_STATUS_PASS_BG_COLOR;
        //    string textColor = OVERALL_STATUS_PASS_TEXT_COLOR;
        //    string overallResult = string.Format(OVERALL_STATUS_PASS, passPercentage);
        //    if (failed > 0 || blocked > 0)
        //    {
        //        backgroundColor = OVERALL_STATUS_FAIL_BG_COLOR;
        //        textColor = OVERALL_STATUS_FAIL_TEXT_COLOR;
        //    }

        //    // This section will create multiple module row results and store it in a file so that the same can be used
        //    // while creating the readonly report
        //    string reportHtmlSource = string.Format(REPORT_HTML_MODULE_ROW_TEMPLATE, module, country, passed,
        //            failed, blocked, overallStatusBar, textColor, backgroundColor, overallResult, timeTakenInMinutes,
        //            iisResultLocation, iisResultLocation);

        //    // This section will add the table column's (like module, country etc) with the start of a country tag
        //    if (isThisTheFirstRun)
        //    {
        //        reportHtmlSource = string.Concat(REPORT_HTML_MODULE_TABLE_TEMPLATE, reportHtmlSource);
        //        isThisTheFirstRun = false;
        //    }
        //    // This section will close the table tag and add another table column depending on whether its the last tag
        //    // running or not
        //    if (isEndOfCountry)
        //    {
        //        if (isThisTheLastRun)
        //            reportHtmlSource = string.Concat(reportHtmlSource, REPORT_HTML_MODULE_TABLE_END);
        //        else
        //            reportHtmlSource = string.Concat(reportHtmlSource, REPORT_HTML_MODULE_TABLE_END, REPORT_HTML_MODULE_TABLE_TEMPLATE);
        //    }

        //    reportHtmlSource = reportHtmlSource.Replace("pc", "%");
        //    CreateFileOrAppendContentIfExist(reportHtmlSource, executionDay);
        //    CreateStatsJsonOrModifyContentIfExist(executionDay, passed, failed, blocked);
        //}


        public void ProcessResultsForEmailReport(int passed, int failed, int blocked, int timeTakenInSeconds,string scenarioTitle, string featureTitle,int stepPassedPct)
        {
            string resultIisLocation = configFileReader.GetConfigResultIisLocation();
            string environment = configFileReader.GetConfigEnvironment().ToUpper();
            string country = configFileReader.GetConfigCountry().ToUpper();
            string module = configFileReader.GetConfigRunModule();
            // string scenarioName= TestContext.CurrentContext.Test.Name;
            string scenarioName = scenarioTitle;
            string reportDir = generalUtilitiesObj.GetProjectResultDirectory();
            //if(_lastFeatureTitle.Equals(featureTitle))
            //{

            //}

            bool isThisTheLastRun = configFileReader.IsLastInExecution();
            //bool isThisTheFirstRun = configFileReader.IsFirstInExecution();
            bool isEndOfCountry = configFileReader.IsExecutionMarkedForEndOfCountry();

            // This section will do cumulative calculation on the extracted information above for use in the report email
            int totalTestCases = passed + failed + blocked;
            int passPercentage = (passed * 100) / totalTestCases;
            int failPercentage = 100 - passPercentage;
            string overallStatusBar;
            if (passPercentage == 100)
            {
                overallStatusBar = string.Format(ALL_PASS_PERCENTAGE_BAR, "100");
            }
            else if (passPercentage == 0)
            {
                overallStatusBar = string.Format(ALL_FAIL_PERCENTAGE_BAR, "100");
            }
            else
            {
                string passTd = string.Format(ALL_PASS_PERCENTAGE_BAR, passPercentage);
                string failTd = string.Format(ALL_FAIL_PERCENTAGE_BAR, failPercentage);
                overallStatusBar = string.Concat(passTd, failTd);
            }

            // This section will create result link for viewing through the report
            string executionDay = generalUtilitiesObj.GetCurrentDateString();
            string resultDirectory = generalUtilitiesObj.GetProjectResultDirectory();
            string dateDir = resultDirectory + "\\" + executionDay;
            string moduleDir = dateDir + "\\" + module;
            int lastRun = generalUtilitiesObj.GetLastTestRunNumber(moduleDir);
            string iisResultLocation = resultIisLocation + "\\" + executionDay + "\\" + module + "\\" + RUN_DIR_NAME
                    + lastRun + "\\" + RUN_REPORT_FILE_DIR;

            // This section will create different color coding for pass, failures etc.
            string backgroundColor = OVERALL_STATUS_PASS_BG_COLOR;
            string textColor = OVERALL_STATUS_PASS_TEXT_COLOR;
            string overallResult = string.Format(OVERALL_STATUS_PASS, stepPassedPct > 1? stepPassedPct : passPercentage);
            if (failed > 0 || blocked > 0)
            {
                backgroundColor = OVERALL_STATUS_FAIL_BG_COLOR;
                textColor = OVERALL_STATUS_FAIL_TEXT_COLOR;
            }

            // This section will create multiple module row results and store it in a file so that the same can be used
            // while creating the readonly report
            /*string reportHtmlSource = string.Format(REPORT_HTML_MODULE_ROW_TEMPLATE, module, country, passed,
                    failed, blocked, overallStatusBar, textColor, backgroundColor, overallResult, timeTakenInMinutes,
                    iisResultLocation, iisResultLocation);*/
            _reportHtmlSource = string.Format(REPORT_HTML_MODULE_ROW_TEMPLATE, scenarioName, country, passed,
                  failed, blocked, overallStatusBar, textColor, backgroundColor, overallResult, timeTakenInSeconds,
                  iisResultLocation, iisResultLocation);

            // This section will add the table column's (like module, country etc) with the start of a country tag
            if (isThisTheFirstRun)
            {
                _reportHtmlSource = string.Concat(REPORT_HTML_MODULE_TABLE_TEMPLATE, _reportHtmlSource);
                isThisTheFirstRun = false;
            }
            // This section will close the table tag and add another table column depending on whether its the last tag
            // running or not
            /*if (isEndOfCountry)
            {
                if (isThisTheLastRun)
                    reportHtmlSource = string.Concat(reportHtmlSource, REPORT_HTML_MODULE_TABLE_END);
                else
                    reportHtmlSource = string.Concat(reportHtmlSource, REPORT_HTML_MODULE_TABLE_END, REPORT_HTML_MODULE_TABLE_TEMPLATE);
            }*/

            _reportHtmlSource = _reportHtmlSource.Replace("pc", "%");
            CreateFileOrAppendContentIfExist(_reportHtmlSource, executionDay);
            CreateStatsJsonOrModifyContentIfExist(executionDay, passed, failed, blocked);
        }

        public void ProcessConsolidatedReport(int passed, int failed, int blocked)
        {
            string executionDay = generalUtilitiesObj.GetCurrentDateString();
            CreateStatsJsonOrModifyContentIfExist(executionDay, passed, failed, blocked);
        }

        public int CountTotalStepsInScenario(string featureTitle, string scenarioTitle)
        {
            int totalStepsInScenario = 0;
            bool flag = true;
            string workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string projectDirectory = Directory.GetParent(Directory.GetParent(workingDirectory).ToString()).ToString();
            //Expecting Featurefile name is same as feature title
            var featureFileLocation = projectDirectory +"\\Features\\" + featureTitle + ".feature";
            var abc = File.ReadAllLines(featureFileLocation);
            foreach (var row in abc)
            {
                if (!row.ToString().Contains(scenarioTitle) && flag == true)
                    continue;
                if (row.ToUpper().StartsWith("SCENARIO"))
                {
                    flag = false;
                    continue;
                }
                if ( row.StartsWith("\tGiven") || row.StartsWith("\tWhen") || row.StartsWith("\tAnd") || row.StartsWith("\tThen") || row.StartsWith("\tBut"))
                {   
                    totalStepsInScenario++;
                    continue;
                }                
                if (row.Contains("|")||row.Contains("#"))
                    continue;
                if (row.Contains(""))
                    break;
            }
            return totalStepsInScenario;
        }

        public void Broadcast()
        {
            string applicationName = configFileReader.GetConfigApplicationName();
            string applicationLogoPath = configFileReader.GetConfigApplicationLogoPath();
            string fromAddresses = configFileReader.GetConfigSendMailFrom();
            string toAddresses = configFileReader.GetConfigSendMailTo();
            string executionDay = generalUtilitiesObj.GetCurrentDateString();
            string environment = configFileReader.GetConfigEnvironment().ToUpper();
            _reportHtmlSource = string.Concat(_reportHtmlSource, REPORT_HTML_MODULE_TABLE_END);

            // This section will consolidate the readonly report and send using smtp
            JObject jsonObject = ReadFileAndGetContentAsJson(executionDay);
            int numPassedCount = int.Parse(jsonObject.GetValue("total_passed").ToString());
            int numTestCaseCount = int.Parse(jsonObject.GetValue("total_cases").ToString());
            dynamic percentagePassed = Math.Round(((float)numPassedCount / numTestCaseCount)*100,2);

            string subjectStatus = OVERALL_STATUS_PASSED;
            string statusColor = OVERALL_PASS_HEADER_TEXT_COLOR;
            if (numTestCaseCount != numPassedCount)
                statusColor = OVERALL_STATUS_FAIL_TEXT_COLOR;

            string mailSubject = string.Format(REPORT_MAIL_SUBJECT_PLACEHOLDER, applicationName, environment, executionDay,
                    percentagePassed, subjectStatus);
            string headerText = string.Format(REPORT_MAIL_HEADER_PLACEHOLDER, environment, executionDay, numPassedCount,
                    numTestCaseCount, statusColor, subjectStatus);
            string readonlyHtmlSource = GetFinalReportHtmlSource(applicationLogoPath, headerText, executionDay);

            SendEmail(readonlyHtmlSource, mailSubject, toAddresses, fromAddresses);

            WriteFinalEmailHtml(executionDay, readonlyHtmlSource);
            DeleteModuleRowFile(executionDay);
            //DeleteModuleJsonFile(executionDay);
        }
    }
}


