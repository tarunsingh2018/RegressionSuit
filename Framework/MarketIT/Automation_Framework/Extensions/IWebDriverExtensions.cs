using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Framework.MarketIT.Automation_Framework.Extensions
{
    public static class IWebDriverExtensions
    {
        public static void NavigateToUrl(this IWebDriver webDriver, string url)
        {
            if (webDriver == null) throw new ArgumentNullException("webDriver");

            try
            {
                webDriver.Url = url;
            }
            catch (Exception)
            {
                webDriver.WaitForPageToLoad();
            }

            webDriver.CloseAlert();
            webDriver.WaitForPageToLoad();
        }

        public static void WaitForPageToLoad(this IWebDriver driver)
        {
            if (driver == null) throw new ArgumentNullException("driver");
            var timeout = new TimeSpan(0, 0, 0, 90);
            var wait = new WebDriverWait(driver, timeout);

            wait.Until(d =>
            {
                try
                {
                    var readyState = d.ExecuteScript<string>(
                        "if (document.readyState) return document.readyState;");
                    return readyState.ToLower() == "complete";
                }
                catch (InvalidOperationException e)
                {
                    //Window is no longer available
                    return e.Message.ToLower().Contains("unable to get browser");
                }
                catch (WebDriverException e)
                {
                    //Browser is no longer available
                    return e.Message.ToLower().Contains("unable to connect");
                }
            });
        }

        public static void CloseAlert(this IWebDriver webDriver)
        {
            if (webDriver == null) throw new ArgumentNullException("webDriver");
            try
            {
                // Wait for alert to show
                webDriver.WaitUntil(TimeSpan.FromSeconds(5), ExpectedConditions.AlertState(true));

                // Clear the alert
                var alert = webDriver.SwitchTo().Alert();
                alert.Accept();
            }
            catch
            {
                // Do nothing, if no nawaway
            }
        }
        public static void WaitUntil<TResult>(this IWebDriver webDriver, TimeSpan maxWaitTime,
                                Func<IWebDriver, TResult> condition)
        {
            if (webDriver == null) throw new ArgumentNullException("webDriver");
            var webDriverWait = new WebDriverWait(webDriver, maxWaitTime);
            webDriverWait.Until(condition);
        }
        public static object ExecuteScript(this IWebDriver webDriver, string script, params object[] arguments)
        {
            var jsExecutor = webDriver as IJavaScriptExecutor;
            if (jsExecutor == null)
                throw new ArgumentException("WebDriver must implement IJavaScriptExecutor", "webDriver");
            return jsExecutor.ExecuteScript(script, arguments);
        }

        public static T ExecuteScript<T>(this IWebDriver webDriver, string script, params object[] arguments)
        {
            var result = ExecuteScript(webDriver, script, arguments);
            var typeCastedResult = (T)result;
            if (typeCastedResult != null) return typeCastedResult;
            return (T)Convert.ChangeType(result, typeof(T));
        }

    }
}
