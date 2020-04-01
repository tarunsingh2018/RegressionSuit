using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.MarketIT.Automation_Framework.Services
{
    public class PostMetrice
    {
        /**
        * Class which runs post integration test and sends test run metrics to "devopsmetricsservice" on staging/production
        * servers. It reads the extent report file and extracts relevant information from it and forms a valid post request
        * and finally sends it to the service.
        */
        #region PostMetriceConstant
                 
        private static string POST_STAGING_URL = "https://stg-devopsmetricsservice.azurewebsites.net/api/metrics/submit";
        private static string POST_PRODUCTION_URL = "https://devopsmetricsservice.azurewebsites.net/api/metrics/submit";

        private static string JSON_REQUEST_TEMPLATE = "[\n" +
                "            \"ApplicationId\": {0},\n" +
                "            \"AutomationProcessId\": {1},\n" +
                "            \"AutomationToolId\": {2},\n" +
                "            \"Description\": \"{3}\",\n" +
                "            \"Version\": \"{4}\",\n" +
                "            \"Cycle\": \"{5}\",\n" +
                "            \"Passed\": {6},\n" +
                "            \"Failed\": {7},\n" +
                "            \"Blocked\": {8},\n" +
                "            \"StartDateTime\": \"{9}\",\n" +
                "            \"EndDateTime\": \"{10}\",\n" +
                "            \"Metadata\": null\n" +
                "  ]  ";

        
        private static bool runOnStaging;
        private static Dictionary<string, object> objDic;

        #endregion

        public static void Main(string jsonFilePath, int passed, int failed, int skipped, string startTime, string endTime)
        {
            string jsonFromFile;

           // var startDt = DateTime.ParseExact(startTime, @"yyyy-MM-ddTHH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
           // var endDt = DateTime.ParseExact(endTime.ToString(), @"yyyy/MM/dd hh:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            if (File.Exists(jsonFilePath))
            {                
                using (var reader = new StreamReader(jsonFilePath))
                {
                    jsonFromFile = reader.ReadToEnd();
                }

                var postMetriceConstant = JsonConvert.DeserializeObject<PostMetriceConstant>(jsonFromFile);
                string filledJsonString = FilledRequestJson(postMetriceConstant, passed, failed, skipped, startTime, endTime);

                 var t = Task.Run(() => SendMetricsDataWith(jsonFromFile));
                 t.Wait();

                Console.WriteLine(t.Result);
            }
            else
            {
                Console.WriteLine("Json file path is not valid");
            }

           
            Console.ReadLine();
        }

        private static async Task<string> SendMetricsDataWith(string filledJsonString)
        {
            objDic = new Dictionary<string, object>();
            StringBuilder responseText = new StringBuilder();

            string uri;
            var response = string.Empty;

            try
            {
                if (runOnStaging)
                {
                    Console.WriteLine("Http Post Url: " + POST_STAGING_URL);
                    uri = POST_STAGING_URL;
                }
                else
                {
                    Console.WriteLine("Http Post Url: " + POST_PRODUCTION_URL);
                    uri = POST_PRODUCTION_URL;
                }


                HttpContent c = new StringContent(filledJsonString.ToString(), Encoding.UTF8, "application/json");
                using (var client = new HttpClient())
                {
                    HttpResponseMessage result = await client.PostAsync(uri, c);
                    if (result.IsSuccessStatusCode)
                    {
                        response = result.StatusCode.ToString();
                        Console.WriteLine("JSON has been sent with Response : " + result.StatusCode);
                        Console.WriteLine("JSON has been sent with Response : " + result.Content);
                    }
                    else
                    {
                        Console.WriteLine("Post scripts not exectued");
                    }
                }

            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }

            return response;
        }

        private static string FilledRequestJson(PostMetriceConstant constant, int passed, int failed, int skipped, string startTime, string endTime)
        {
            var template = string.Format(JSON_REQUEST_TEMPLATE, constant.applicationId, constant.automationProcessId, constant.automationToolId, constant.description,
                    constant.version, constant.cycle, passed, failed, skipped, startTime, endTime, constant.metaData);
            var x = template.Replace('[', '{');
            var y = x.Replace(']', '}');
            return y;
        }
    }
}


class PostMetriceConstant
{
    public int applicationId { get; set; }
    public int automationProcessId { get; set; }
    public int automationToolId { get; set; }
    public string description { get; set; }
    public string version { get; set; }
    public string cycle { get; set; }
    public string passed { get; set; }
    public string failed { get; set; }
    public string blocked { get; set; }
    public string startDateTime { get; set; }
    public string endDateTime { get; set; }
    public string metaData { get; set; }
}