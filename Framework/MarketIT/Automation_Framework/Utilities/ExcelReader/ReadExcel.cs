using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop;
using System.Configuration;

namespace Framework.MarketIT.Automation_Framework.Utilities.ExcelReader
{
    class ReadExcel
    {


        private string report =
                    "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\"\n" +
                    "        \"http://www.w3.org/TR/html4/loose.dtd\">\n" +
                    "<html>\n" +
                    "    <head></head>\n" +
                    "    <body>\n" +
                    "        <table align=\"left\" style=\"background-color: #eee\" width=\"50pc\">\n" +
                    "                            <tr height=\"15\" bgcolor=\"#48C9B0\" style=\"color:#000000; font-weight:bold;\">\n" +
                    "                                        <td colspan=\"4\" align=\"center\" style=\"padding-left:5px; background-color: #85C1E9\">Specflow, BDD based Testing Automation</td>\n" +
                    "                                    </tr>\n" +
                    "                            <tr height=\"12\" bgcolor=\"#D4E6F1\" style=\"color:#000000; font-weight:bold;\">\n" +
                    "                                        <th width=\"20pc\">Product</th>\n" +
                    "                                        <th width=\"10pc\">End Date</th>\n" +
                    "                                        <th width=\"10pc\">Completed %</th>\n" +
                    "                                        <th width=\"10pc\">In Progress</th>\n" +
                    "                                    </tr>\n\n"+
                    "                           <tr height=\"12\" bgcolor=\"#ffffff\" >\n" +
                    "                                        <td align=\"left\" style=\"padding-left:5px\">Pulse Tech - (435)</td>\n" +
                    "                                        <td align=\"left\">20-Nov</td>\n" +
                    "                                        <td align=\"left\">65</td>\n" +
                    "                                        <td align=\"left\">10</td>\n" +
                    "                            </tr>\n"+
                         "                           <tr height=\"12\" bgcolor=\"#ffffff\" >\n" +
                    "                                        <td align=\"left\" style=\"padding-left:5px\">Response</td>\n" +
                    "                                        <td align=\"left\">8-Dec</td>\n" +
                    "                                        <td align=\"left\">0</td>\n" +
                    "                                        <td align=\"left\">0</td>\n" +
                    "                            </tr>\n" +
                         "                           <tr height=\"12\" bgcolor=\"#ffffff\" >\n" +
                    "                                        <td align=\"left\" style=\"padding-left:5px\">Connect</td>\n" +
                    "                                        <td align=\"left\">20-Dec</td>\n" +
                    "                                        <td align=\"left\">0</td>\n" +
                    "                                        <td align=\"left\">0</td>\n" +
                    "                            </tr>\n" +
                    "                            </table>\n" +

                    "        <table align=\"left\" style=\"background-color: #eee\" width=\"90pc\">\n" +
                    "                            <tr height=\"15\" bgcolor=\"#16A085\" style=\"color:#000000; font-weight:bold;\">\n" +
                    "                                        <td colspan=\"8\" align=\"center\" style=\"padding-left:5px; background-color: #16A085\">TCR Automation Daily Status Report</td>\n" +
                    "                                    </tr>\n" +
                    "                            <tr height=\"12\" bgcolor=\"#ABEBC6\" style=\"color:#000000; font-weight:bold;\">\n" +
                    "                                        <th width=\"10pc\">Name</th>\n" +
                    "                                        <th width=\"30pc\">Stories</th>\n" +
                    "                                        <th width=\"12c\">Status</th>\n" +
                    "                                        <th width=\"10pc\">Start date</th>\n" +
                    "                                        <th width=\"10pc\">Estimated End Date</th>\n" +
                    "                                        <th width=\"10pc\">Actual End Date</th>\n" +
                    "                                        <th width=\"10pc\">Complete %</th>\n" +
                    "                                        <th width=\"8pc\">Impediments</th>\n" +
                    "                                    </tr>\n\n";
                    
               
      

        string report1;
       public  string ReadExcelFile()
        {

            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            string workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string projectDirectory = Directory.GetParent(Directory.GetParent(workingDirectory).ToString()).ToString();
            string sampleFilePath = projectDirectory + "\\Resources\\" + "TCRPlanReport.xlsx";

            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(sampleFilePath);
            Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;
            var state = Microsoft.Office.Interop.Excel.XlWindowState.xlMaximized;


            xlApp.Visible = true;
            xlWorksheet = xlWorkbook.ActiveSheet;

            int rowCount = xlWorksheet.UsedRange.Rows.Count;
            int colCount = xlWorksheet.UsedRange.Columns.Count;

          //  string d = xlRange.Cells[2, "C"].Value.ToString();
            StringBuilder sb = new StringBuilder(report);
            for (int i = 2; i <= 5; i++)
            {
                sb.Append("<tr height =\"12\" bgcolor=\"#ffffff\" >\n");
                for (int j = 1; j < colCount; j++)
                {
                    string cellValue = xlRange.Cells[i, j].Value.ToString();

                    if(cellValue.Contains("12:00:00") )
                    {
                        cellValue = cellValue.Split(' ')[0];
                    }

                    if(j==7)
                        report1 =  "         <td align=\"center\" style=\"padding-left:5px\">" + cellValue + "%</td>\n";
                    else
                        report1 = "         <td align=\"left\" style=\"padding-left:5px\">" + cellValue + "</td>\n";

                    sb.Append(report1);

                     
                }

                sb.Append(" </tr >\n");
            }
            
           // s1.Append(report1);
            xlWorkbook.Close();

            xlApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorksheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkbook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
            GC.Collect();

            //  string finalReport = string.Concat(report, report1);
            return sb.ToString();
        }
    }
}
