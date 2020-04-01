using AventStack.ExtentReports;
using Framework.MarketIT.Automation_Framework.FunctionLibrary;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using OpenQA.Selenium.Support.PageObjects;

namespace Framework.MarketIT.Automation_Framework.Utilities.Common
{
    public class BasePage
    {
        IWebDriver driver;
        private static ExtentTest reporter;
        GeneralUtilities generalUtilities;

        public readonly int DEFAULT_TIMEOUT_IN_SECONDS              = 60;

        public void Initialize(IWebDriver webDriver)
        {
            if (webDriver == null) throw new ArgumentNullException("webDriver");

            driver = webDriver;            
            PageFactory.InitElements(webDriver, this);
        }

        public readonly string base64ImageTag                       = "<img style=\"width:100%;\" src={0}>";
        public static readonly string ELEMENT_ATTRIBUTE_VALUE       = "value";

        public readonly int DEFAULT_OVERLAY_TIMEOUT_IN_SECONDS      = 10;
        public readonly int ON_CLICK_DEFAULT_NEW_WINDOW_COUNT       = 2;
        public readonly int DEFAULT_TIMEOUT_30_SECONDS              = 30;
        public readonly int DEFAULT_TIMEOUT_FOR_ZERO_SECONDS        = 0;
        public readonly int DEFAULT_GET_TEXT_TIMEOUT_IN_SECONDS     = 20;
        public readonly int DEFAULT_NOT_FOUND_TIMEOUT_IN_SECONDS    = 5;
        public readonly int NOT_FOUND_SHORT_TIMEOUT_IN_SECONDS      = 2;
        public readonly int POLLING_IN_EVERY_ONE_SECOND             = 1;
        public readonly int POLLING_IN_EVERY_TWO_SECOND             = 2;
        public readonly int SELECT_OPTION_WAIT_TIME_IN_MS           = 1000;
        public readonly int HTTP_STATUS_OK                          = 200;
        public readonly int NUM_RETRY_ATTEMPT_WHEN_FAILED           = 2;

        public static readonly int WINDOWS_AUTH_WAIT_TIMEOUT_SEC    = 2;
        public static readonly long LOGIN_PAGE_DISPLAY_TIMEOUT_MS   = 5000;
        public static readonly int DEFAULT_HOURS_BACK               = 1;
        public static readonly int DEFAULT_ONE_DAY_AFTER            = 24;
        public static readonly string NUMBER_ZERO_STRING            = "0";
        public static readonly string NUMBER_ONE_STRING             = "1";
        public static readonly string NUMBER_TWO_STRING             = "2";

        public static readonly int NUMBER_ZERO_VALUE                = int.Parse(NUMBER_ZERO_STRING);
        public static readonly int NUMBER_ONE_VALUE                 = int.Parse(NUMBER_ONE_STRING);
        public static readonly int NUMBER_TWO_VALUE                 = int.Parse(NUMBER_TWO_STRING);
        public static readonly int TWELVE_HOUR_DAY                  = 12;
        public static readonly int TWENTYFOUR_HOUR_DAY              = 24;
        public static readonly int FIRST_INDEX                      = 1;

        public static readonly string DUMMY_TEXT                    = "dummy";
        public readonly string displayNoneStyleType                 = "display: none;";
        public readonly string visibilityStyleType                  = "visibility: hidden;";
        public readonly string hiddenClass                          = "ng-hide";

        public readonly string DATE_TIME_MAP_DATE_KEY               = "date";
        public readonly string DATE_TIME_MAP_TIME_KEY               = "time";
        public readonly string DISABLED_ATTRIBUTE                   = "disabled";
        private static readonly DateTime Jan1st1970                 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public BasePage()
        {
            
        }

        public BasePage(IWebDriver iWebDriver, ExtentTest pageReporter)
        {
            driver = iWebDriver;
            reporter = pageReporter;
            generalUtilities = GeneralUtilities.GetInstance();
        }

        public string NavigateToURL()
        {
            return GeneralUtilities.GetInstance().GetApplicationUrl();            
        }

        
        public IWebElement GetElement(By locator)
        {
            IWebElement webElement = GetClickableElementByExplicitWait(locator);
            return webElement;
        }

        // Return booleand resulet and check Element visible by IwebElement

        public bool IsElementDisplayByxpath(String xPathString)
        {
            bool result;

            if (driver.FindElement(By.XPath(xPathString)).Displayed)
            {
                LogInfo("Web Element Visible on DOM");
                result = true;
            }
            else
            {
                LogInfo("Fail to Display --  Web Element on DOM");
                
                result = false;
            }
            return result;
        }

        /**
        * Method to return located WebElement with explicit wait with locator as input
        * @param locator
        * @return WebElement
        */
        public IWebElement GetClickableElementByExplicitWait(By locator)
        {
            IWebElement webElement = null;
            try
            {
                WebDriverWait wait = WaitFor(4);
                wait.Until(d=>d.FindElement(locator).Displayed==true);
                //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
                webElement = driver.FindElement(locator);
                return webElement;
            }
            catch (Exception exp)
            {
                string locatorInfo = "Element with locator[" + locator.ToString() + "] not found\n";
                LogInfo(locatorInfo + exp.StackTrace);                
            }

            return webElement;
        }

        public bool VerifyIsTextPresent(string searchString,By containerElement=null)
        {
            WebDriverWait wait = WaitFor(3);
            try
            {
                if(containerElement==null)
                    wait.Until(d => d.FindElement(By.XPath("//html")).Text.Contains(searchString));
                else
                    wait.Until(d => d.FindElement(containerElement).Text.Contains(searchString));
                return true;
            }
            catch (Exception ex)
            {
                LogInfo(ex.StackTrace);
                return false;
            }
                        
        }

        /**
        * Method to return located WebElement with explicit wait with locator as input
        * @param locator
        * @return WebElement
        */
        public IWebElement GetClickableElementByExplicitWait(By locator, int timeOut)
        {
            IWebElement webElement = null;
            try
            {
                WebDriverWait wait = WaitFor(timeOut);
                webElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));

                return webElement;
            }
            catch (Exception exp)
            {
                string locatorInfo = "Element with locator[" + locator.ToString() + "] not found\n";
                LogInfo(locatorInfo + exp.StackTrace);                
            }

            return webElement;
        }

        public WebDriverWait WaitFor(int timeOut)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut));
        }

        public WebDriverWait WaitFor()
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(DEFAULT_TIMEOUT_IN_SECONDS));
        }

        public void TakeScreenShot()
        {
            string base64Image = generalUtilities.GetChromeScreenShot();
            string imageTag = string.Format(base64ImageTag, base64Image);
            reporter.Log(Status.Info, imageTag);
        }

        public void LogInfo(string logInformation)
        {
            reporter.Log(Status.Info, logInformation);
        }

        public void Pause(int timeToPause)
        {
            Thread.Sleep(timeToPause);
        }

        /**
         * Method to refresh the current page
         */
        public void RefreshPage()
        {
            driver.Navigate().Refresh();
        }

        /**
         * Method to click on a WebElement
         * @param webElement
         */
        public void Click(IWebElement webElement)
        {
            webElement.Click();
        }

        /**
         * Method to double click on a WebElement
         * @param webElement
         */
        public void DoubleClick(IWebElement webElement)
        {
            Click(webElement);
            Click(webElement);
        }
        /**
         * Method to click on an element identified from a locator
         * @param locator
         */
        public void Click(By locator)
        {
            IWebElement element = GetElement(locator);
            Click(element);
            Pause(1000);
        }
        public void ClickByPartialLinkedText(string buttonCaption)
        {
            driver.FindElement(By.PartialLinkText(buttonCaption)).Click();
        }

        /**
        * Method to find an element and click on it
        * @param locator
        */
        public void findAndClick(By locator)
        {
            IWebElement element = GetElement(locator);
            element.Click();
        }

        /**
         * Method to find an element with findElements() and click on any one which is displayed. Useful in cases where we
         * have more than one element for a given locator and clicking on any one of the many is enough
         * @param locator
         */
        public void FindAndClickAny(By locator)
        {
            IList<IWebElement> elements = GetElements(locator);
            foreach (IWebElement element in elements)
            {
                if (element.Displayed)
                {
                    element.Click();
                    break;
                }
            }
        }

        /**
          * Method to find an element, scroll to it and click on it
          * @param locator
          */
        public void FindScrollToAndClick(By locator)
        {
            ScrollElementToView(locator);
            IWebElement element = GetElement(locator);
            element.Click();
        }

        /**
         * Method to click on one of the elements from a list of similar elements, scroll to it and click on it
         * @param locator
         */
        public void FindScrollToAndClickAny(By locator)
        {
            IList<IWebElement> elementList = GetElements(locator);
            foreach (IWebElement webElement in elementList)
            {
                ScrollElementToView(webElement);
                webElement.Click();
                break;
            }
        }

        /**
        * Method to scroll to an element and click on it
        * @param webElement
        */
        public void FindScrollToAndClick(IWebElement webElement)
        {
            ScrollElementToView(webElement);
            webElement.Click();
        }

        /**
        * Method to get WebElement's from a locator
        * @param locator
        * @return list<WebElement></WebElement>
        */
        public IList<IWebElement> GetElements(By locator)
        {
            IList<IWebElement> elements = driver.FindElements(locator);

            return elements;
        }
        
        ///**  // Shabana Need to fix below member method later 
        // * Method to get WebElement from a locator using findElements
        // * @param locator
        // * @return WebElement
        // */
        //public IWebElement GetElementWithoutExplicitWait(By locator)
        //{
        //    IWebElement webElement = driver.FindElements(locator).ToList().

        //        .Find() ->    stream().findFirst().orElseThrow(()-> {
        //        return new NoSuchElementException("Cannot locate an element using " + locator);
        //    });

        //    return webElement;
        //}

        /**
        * Method to get the attribute value of a passed attribute name of an element
        * @param locator
        * @param elementAttribute
        * @return String attribute value of the element
        */
        public string GetElementAttributeValue(By locator, string elementAttribute)
        {
            IWebElement webElement = GetElement(locator);

            return webElement.GetAttribute(elementAttribute);
        }

        /**
        * Method to get the attribute value of a passed attribute name of an element
        * @param webElement
        * @param elementAttribute
        * @return String - Element attribute value
        */
        public string GetElementAttributeValue(IWebElement webElement, string elementAttribute)
        {
            return webElement.GetAttribute(elementAttribute);
        }

        /**
        * Method to get WebElement inside another WebElement
        * @param parentWebElement
        * @param childXPath
        * @return WebElement
        */
        public IWebElement GetElementWithinElement(IWebElement parentWebElement, By childXPath)
        {
            IWebElement webElement = parentWebElement.FindElement(childXPath);

            return webElement;
        }

        /**
        * Method to get WebElements inside another WebElement
        * @param parentWebElement
        * @param childXPath
        * @return List<WebElement>
        */
        public IList<IWebElement> GetElementsWithinElement(IWebElement parentWebElement, By childXPath)
        {
            IList<IWebElement> webElementList = parentWebElement.FindElements(childXPath);

            return webElementList;
        }

        /**
         * Method to get WebElement from a locator with xpath and with explicit wait
         * @param locator
         * @return WebElement
         */
        public IWebElement GetElementByXpath(string locator)
        {
            By locatorXpath = By.XPath(locator);
            IWebElement webElement = GetElement(locatorXpath);

            if (webElement == null)
                LogInfo(string.Format(Assertions.ELEMENT_NOT_FOUND, Element.DEFAULT,
                        locator));

            return webElement;
        }
        //public static void WaitForPageUrlToChange(this IWebDriver driver, string destinationUrlSubstring)
        //{
        //    if (driver == null) throw new ArgumentNullException("driver");
        //    if (string.IsNullOrWhiteSpace(destinationUrlSubstring)) throw new ArgumentNullException("destinationUrlSubstring");
        //    var timeout = TimeSpan.FromSeconds(80);
        //    var wait = new WebDriverWait(driver, timeout);

        //    //driver.CloseAlert(); // Handle ajax errors / navaways before reading URL
        //    try
        //    {
        //        wait.Until(d => driver.Url.Contains(destinationUrlSubstring));
                
        //            // Handle ajax errors / navaways before reading URL
        //            //driver.CloseAlert();
        //            //return driver.Url.Contains(destinationUrlSubstring);
                
        //    }
        //    catch (Exception)
        //    {}

        //}  
       

        public IWebElement GetElementByPartialLinkText(string searchString)
        {
            By locatorPartialLinktext = By.PartialLinkText(searchString);
            IWebElement webElement = GetElement(locatorPartialLinktext);            
            if (webElement == null)
                LogInfo(string.Format(Assertions.ELEMENT_NOT_FOUND, Element.DEFAULT,
                        searchString));

            return webElement;
        }

        /**
         * Method to get a visible WebElement from a locator with xpath and with explicit wait. This method is helpful in
         * finding the visible element out of many, which has the same xpath but currently invisible
         * @param locator
         * @return WebElement
         */
        public IWebElement GetVisibleElementOutOfManyByXpath(string locator)
        {
            IWebElement webElement = null;
            By locatorXpath = By.XPath(locator);
            IList<IWebElement> webElementList = GetElements(locatorXpath);
            foreach (IWebElement element in webElementList)
            {
                if (element.Displayed)
                {
                    webElement = element;
                    break;
                }
            }

            if (webElement == null)
                LogInfo(string.Format(Assertions.ELEMENT_NOT_FOUND, Element.DEFAULT,
                        locator));

            return webElement;
        }

        /**
         * Method to get WebElement's from a locator with xpath
         * @param locator
         * @return list<WebElement></WebElement>
         */
        public IList<IWebElement> GetElementsByXpath(string locator)
        {
            By locatorXpath = By.XPath(locator);
            IList<IWebElement> elements = driver.FindElements(locatorXpath);

            return elements;
        }

        /**
         * Method to get WebElement's from a 'By' locator
         * @param locator
         * @return list<WebElement></WebElement>
         */
        public IList<IWebElement> GetElementsByXpath(By locator)
        {
            IList<IWebElement> elements = driver.FindElements(locator);

            return elements;
        }

        /**
        * Method to get WebElement from a locator with id
        * @param locator
        * @return WebElement
        */
        public IWebElement GetElementById(string locator)
        {
            By locatorId = By.Id(locator);
            IWebElement webElement = driver.FindElement(locatorId);
            return webElement;
        }

        /**
         * Method to get WebElement from a locator with css selector
         * @param locator
         * @return WebElement
         */
        public IWebElement GetElementByCssSelector(string locator)
        {
            IWebElement webElement = driver.FindElement(By.CssSelector(locator));
            return webElement;
        }

        /**
        * Method to write into input box with data
        * @param locator
        * @param data
        */
        public void WriteInInputBox(By locator, string data)
        {
            IWebElement element = GetClickableElementByExplicitWait(locator);
            //element.Clear();
            if (element.Text!=null|| element.Text=="")
            {
                element.Clear();
                element.SendKeys(data);
            }
            Pause(500);
            
        }

        /**
        * Method to write into input box with data and press enter
        * @param locator
        * @param data
        */
        public void WriteInInputBoxAndEnter(By locator, string data)
        {
            IWebElement inputWel = GetClickableElementByExplicitWait(locator);
            inputWel.Clear();
            inputWel.SendKeys(data);
            TakeViewPortScreenShot();
            inputWel.SendKeys(Keys.Enter);
        }

        /**
        * Method to write into input box with data and press tab
        * @param locator
        * @param data
        */
        public void WriteInInputBoxAndPressTab(By locator, string data)
        {
            WriteInInputBoxAndPressTab(locator, data, true);
        }

        /**
         * Method to write into input box with data, press tab and take screen shot of the entry based on input
         * @param locator
         * @param data
         * @param takeInputScreenShot
         */
        public void WriteInInputBoxAndPressTab(By locator, string data, bool takeInputScreenShot)
        {
            IWebElement inputWel = GetClickableElementByExplicitWait(locator);
            WriteInInputBoxAndPressTab(inputWel, data, takeInputScreenShot);
        }

        /**
         * Method to write into input box with data, press enter and take screen shot of the entry based on input
         * @param locator
         * @param data
         * @param takeInputScreenShot
         */
        public void WriteInInputBoxAndPressEnter(By locator, string data, bool takeInputScreenShot)
        {
            IWebElement inputWel = GetClickableElementByExplicitWait(locator);
            inputWel.Clear();
            inputWel.SendKeys(data);
            if (takeInputScreenShot)
                TakeViewPortScreenShot();
            inputWel.SendKeys(Keys.Enter);
        }

        /**
        * Method to write into input box with data and press escape
        * @param locator
        * @param data
        */
        public void WriteInInputBoxAndPressEscape(By locator, string data)
        {
            WriteInInputBoxAndPressEscape(locator, data, true);
        }

        /**
         * Method to write into input box with data, press escape and take screen shot of the entry based on input
         * @param locator
         * @param data
         * @param takeInputScreenShot
         */
        public void WriteInInputBoxAndPressEscape(By locator, string data, bool takeInputScreenShot)
        {
            IWebElement inputWel = GetClickableElementByExplicitWait(locator);
            inputWel.Clear();
            inputWel.SendKeys(data);
            if (takeInputScreenShot)
                TakeViewPortScreenShot();
            inputWel.SendKeys(Keys.Escape);
        }

        /**
         * Method to write into input box with data
         * @param inputWel - <WebElement></WebElement>
         * @param data
         */
        public void WriteInInputBox(IWebElement inputWel, string data)
        {
            inputWel.Clear();
            inputWel.SendKeys(data);
        }

        /**
        * Method to write into input box with data, press tab and take screen shot of the entry based on input
        * @param webElement
        * @param data
        * @param takeInputScreenShot
        */
        public void WriteInInputBoxAndPressTab(IWebElement webElement, string data, bool takeInputScreenShot)
        {
            webElement.Clear();
            webElement.SendKeys(data);
            if (takeInputScreenShot)
                TakeViewPortScreenShot();
            webElement.SendKeys(Keys.Tab);
        }

        /**
         * Method to write into input box with data with wait condition in finding element
         * @param locator
         * @param data
         */
        public void WaitAndWriteInInputBox(By locator, string data)
        {
            IWebElement webElement = GetLocatedElementByExplicitWait(locator);
            webElement.Clear();
            webElement.SendKeys(data);
        }

        /**
        * Method to clear input box data with wait condition in finding element
        * @param locator
        */
        public void ClearInputBox(By locator)
        {
            IWebElement webElement = GetLocatedElementByExplicitWait(locator);
            webElement.Clear();
        }

        /**
         * Method to clear input box data with wait condition in finding element
         * @param webElement
         */
        public void ClearInputBox(IWebElement webElement)
        {
            webElement.Clear();
        }

        /**
         * Method to get window handle of latest opened browser
         * @return
         */
        public string GetBrowserHandle()
        {
            IReadOnlyCollection<string> windowHandlesSet = driver.WindowHandles;
            string[] windowHandles = windowHandlesSet.ToArray<string>();
            return windowHandles[windowHandles.Length - 1];
        }

        /**
        * Method to get close all browsers except the parent one
        * @return
        */
        public void CloseAllOpenedBrowsers()
        {
            IReadOnlyCollection<string> windowHandlesSet = driver.WindowHandles;
            string[] windowHandles = windowHandlesSet.ToArray();
            string mainWindow = windowHandles[0];
            for (int i = 1; i < windowHandles.Length; i++)
            {
                driver.SwitchTo().Window(windowHandles[i]);
                driver.Close();
                driver.SwitchTo().Window(mainWindow);
            }
        }

        /**
         * Method to get total opened browser's
         * @return int browser count
         */
        public int GetBrowserCount()
        {
            int intBrowserCount = driver.WindowHandles.Count();

            return intBrowserCount;
        }

        /**
        * Method to get url of latest opened browser
        * @return String url
        */
        public string GetLatestBrowserURL()
        {
            string currWinHandle = driver.CurrentWindowHandle;
            string winHandle = GetBrowserHandle(); // switch the handle to the latest browser
            driver.SwitchTo().Window(winHandle);
            string urlOpened = driver.Url; // fetch the url
            driver.SwitchTo().Window(currWinHandle); // Switch back to the old browser which was in focus
            return urlOpened;
        }

        /**
         * Method to switch to latest opened browser
         */
        public void SwitchToLatestBrowser()
        {
            driver.SwitchTo().Window(GetBrowserHandle());
        }

        public string GetLatestWindow()
        {
            string winHandleBefore = driver.CurrentWindowHandle;

            return winHandleBefore;
        }

        public void SwitchToLatestWindow(string switchWindow)
        {
            driver.SwitchTo().Window(switchWindow);
        }

        /**
         * Method to verify url of opened window
         * @param urlExpected
         * @return bool true/false
         */
        public bool VerifyUrl(string urlExpected)
        {
            string winHandle = GetBrowserHandle();
            driver.SwitchTo().Window(winHandle);
            string urlOpened = driver.Url;
            driver.SwitchTo().DefaultContent();
            return (DUMMY_TEXT + urlOpened).Contains(urlExpected);
        }

        /**
        * Method to close the latest browser and move focus to previous browser
        */
        public void CloseLatestBrowser()
        {
            string winHandle = GetBrowserHandle();
            driver.SwitchTo().Window(winHandle);
            driver.Close();
            winHandle = GetBrowserHandle();
            driver.SwitchTo().Window(winHandle);
        }

        /**
        * Method to check text is present in the page source
        * @param textToVerify
        * @return bool true/false
        */
        public bool IsTextPresentOnPage(string textToVerify)
        {
            string pageSource = driver.PageSource;
            return pageSource.Contains(textToVerify);
        }

        /**
         * Method to ger page source
         * @return String
         */
        public string GetPageText()
        {
            return driver.PageSource;
        }

        /**
         * Method to get current page url
         * @return string
         */
        public string GetCurrentPageUrl()
        {
            return driver.Url;
        }

        /**
         * Method to select item in drop down with locator as input
         * @param itemToSelect
         * @param locator
         */
        public void SelectItemInDropDown(string itemToSelect, By locator)
        {
            Pause(1000);
            IWebElement select = driver.FindElement(locator);
            
                SelectElement dropdownList = new SelectElement(select);
                dropdownList.SelectByText(itemToSelect);
                Pause(1000);
            
            
        }

        /**
         * Method to select item with exact value as provided in drop down with locator as input
         * @param itemToSelect
         * @param locator
         */
        public void SelectValueItemInDropDown(string itemToSelect, By locator)
        {
            IWebElement select = driver.FindElement(locator);
            SelectElement dropdownList = new SelectElement(select);
            dropdownList.SelectByValue(itemToSelect);
        }

        /**
        * Method to select item in drop down with WebElement as input
        * @param itemToSelect
        * @param select
        */
        public void SelectItemInDropDown(string itemToSelect, IWebElement select)
        {
            SelectElement dropdownList = new SelectElement(select);
            dropdownList.SelectByText(itemToSelect);
        }

        /**
         * Method to select the first option from the select dropdown and log message if there are no options available
         * @param locator
         * @return bool true/false
         */
        public bool IsFirstAvailableOptionSelectedFromDropdown(By locator)
        {
            bool isOptionSelected = false;
            IWebElement select = GetElement(locator);
            SelectElement dropdownList = new SelectElement(select);
            IList<IWebElement> optionList = dropdownList.Options;
            foreach (IWebElement optionWel in optionList)
            {
                if (optionWel.Displayed)
                {
                    optionWel.Click();
                    isOptionSelected = true;
                    break;
                }
            }
            if (optionList.Count == 0)
            {
                LogInfo(string.Format(Assertions.SELECT_OPTIONS_NOT_FOUND, locator));                
            }

            return isOptionSelected;
        }

        /**
        * Method to select the first option from the select dropdown and log message if there are no options available
        * @param locator
        * @param optionText
        * @return bool true/false
        */
        public bool IsOptionWithGivenTextSelectedFromDropdown(By locator, string optionText)
        {
            bool isOptionSelected = false;
            IWebElement select = GetElement(locator);
            SelectElement dropdownList = new SelectElement(select);
            IList<IWebElement> optionList = dropdownList.Options;
            if (optionList.Count == 0)
                LogInfo(string.Format(Assertions.SELECT_OPTIONS_NOT_FOUND, locator));
            else
                isOptionSelected = IsOptionSelected(optionList, optionText);

            return isOptionSelected;
        }

        /**
         * Method to select item (containing provided text) in drop down with WebElement as input
         * @param textContaining
         * @param locator (By)
         */
        public void SelectItemInDropDownContaining(By locator, string textContaining)
        {
            IWebElement select = GetElement(locator);
            SelectElement dropdownList = new SelectElement(select);
            bool isSelected = IsOptionSelected(dropdownList.Options, textContaining);

            if (!isSelected)
                LogInfo("Could not select the option containing text: " + textContaining
                        + ", check if it exists");
        }
        public bool SelectItemInDropDownContaining(string textContaining, By locator)
        {
            IWebElement select = GetElement(locator);
            SelectElement dropdownList = new SelectElement(select);
            bool isSelected = IsOptionSelected(dropdownList.Options, textContaining);

            if (!isSelected)
                LogInfo("Could not select the option containing text: " + textContaining
                        + ", check if it exists");

            return isSelected;
        }
        public void SelectItemInDropDownContaining(By locator, int itemNoToSelect, string textContaining)
        {
            IWebElement select = GetElement(locator);
            SelectElement dropdownList = new SelectElement(select);
            bool isSelected = IsOptionSelected(dropdownList.Options, itemNoToSelect, textContaining);

            if (!isSelected)
                LogInfo("Could not select the option containing text: " + textContaining + ", check if it exists");
        }

        /**
         * Method to find out the number of options available in a select dropdown
         * @param locator
         * @return int - number of options
         */
        public int GetNumSelectOptionsInDropDown(By locator)
        {
            IWebElement select = GetElement(locator);
            SelectElement dropdownList = new SelectElement(select);
            IList<IWebElement> optionList = dropdownList.Options;

            return optionList.Count;
        }

        /**
         * Method to select item in drop down with item index
         * @param itemIndex
         * @param locator
         */
        public void SelectItemInDropDown(int itemIndex, By locator)
        {
            IWebElement select = driver.FindElement(locator);
            SelectElement dropdownList = new SelectElement(select);
            dropdownList.SelectByIndex(itemIndex);
        }

        /**
         * Method to check if an option is selected or not
         * @param options
         * @param itemToSelect
         * @return bool true/false
         */
        public bool IsOptionSelected(IList<IWebElement> options, string itemToSelect)
        {
            bool isValueFound = false;
            foreach (IWebElement option in options)
            {
                if (option.Text.Contains(itemToSelect))
                {
                    option.Click();
                    ElapseTimeAfterSelect();
                    isValueFound = true;
                    break;
                }
            }

            return isValueFound;
        }
        public bool IsOptionSelected(IList<IWebElement> options, int itemNoToSelect, string itemToSelect)
        {
            bool isValueFound = false;
            foreach (IWebElement option in options)
            {
                if (option.Text.Contains(itemToSelect))
                {
                    options.ElementAt(itemNoToSelect).Click();
                    ElapseTimeAfterSelect();
                    isValueFound = true;
                    break;
                }
            }

            return isValueFound;
        }
        public void ElapseTimeAfterSelect()
        {
            long t1, t2;
            t1 = t2 = CurrentTimeMillis();
            while (t1 - t2 < SELECT_OPTION_WAIT_TIME_IN_MS)
            {
                t1 = CurrentTimeMillis();
            }
        }

        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        /**
         * Method of get first selected item in a dropdown with locator as input
         * @param locator
         * @return String
         */
        public string GetSelectedItemFromDropDown(By locator)
        {
            IWebElement select = driver.FindElement(locator);
            SelectElement dropdownList = new SelectElement(select);
            return dropdownList.SelectedOption.Text;
        }

        /**
         * Method of get first selected item in a dropdown with WebElement as input
         * @param webElement
         * @return String
         */
        public string GetSelectedItemFromDropDown(IWebElement webElement)
        {
            SelectElement dropdownList = new SelectElement(webElement);
            return dropdownList.SelectedOption.Text;
        }

        /**
         * Method to get item count inside a WebElement
         * @param locator
         * @return int
         */
        public int GetElementCount(By locator)
        {
            int count = driver.FindElements(locator).Count;
            return count;
        }

        /**
         * Method to get text of a WebElement
         * @param element
         * @return String
         */
        public string GetElementText(IWebElement element)
        {
            return element.Text;
        }

        /**
         * Method to get text of an element by locator
         * @param locator
         * @return String
         */
        public string GetElementText(By locator)
        {
            IWebElement webElement = GetElement(locator);
            return GetElementText(webElement);
        }

        /**
         * Method to get value of an element by locator input
         * @param locator
         * @return String
         */
        public string GetElementValue(By locator)
        {
            IWebElement webElement = GetElement(locator);
            return GetElementValue(webElement);
        }

        /**
         * Method to get value of an element by WebElement input
         * @param element
         * @return String
         */
        public string GetElementValue(IWebElement element)
        {
            return element.GetAttribute(ELEMENT_ATTRIBUTE_VALUE);
        }

        /**
         * Method to get text of an element by locator, try catch used to return blank if exception occurs
         * @param locator
         * @return String element text or empty string
         */
        public string GetElementTextByHandlingException(By locator)
        {
            string elementText = "";
            try
            {
                IWebElement webElement = driver.FindElement(locator);
                elementText = webElement.Text;
            }
            catch (Exception exp)
            {
                LogInfo(exp.StackTrace);
                return elementText;
            }

            return elementText;
        }

        /**
         * Method to get text of an element by locator with JSE, try catch used to return blank if exception occurs
         * @param locator
         * @return String element text or empty string
         */
        public string GetElementTextWithJseByHandlingException(By locator)
        {
            string elementText = GetTextWithJse(locator);

            return elementText;
        }

        /**
        * Method to get the value with JavaScriptExecutor with locator input
        * @param locator
        * @return String value of the provided element
        */
        public string GetTextWithJse(By locator)
        {
            string elementText = "";
            try
            {
                IWebElement element = driver.FindElement(locator);
                IJavaScriptExecutor javascriptExecutor = (IJavaScriptExecutor)driver;
                elementText = (string)javascriptExecutor.ExecuteScript("return arguments[0].value", element);
            }
            catch (Exception exp)
            {
                LogInfo(exp.StackTrace);
            }

            return elementText;
        }

        /**
        * Method to get other sibling element from one element. It actually goes to the direct parent element and find the
        * sibling. The sibling to find needs to be passed as the parameter.
        * @param childLocator - locator for the element whose sibling need to be found
        * @param siblingXpath - the sibling xPath, which will change depending on the type i.e. for an input, label etc.
        * @return WebElement - Sibling element of the provided locator
        */
        public IWebElement GetOtherSiblingFromAGivenChild(By childLocator, By siblingXpath)
        {
            IWebElement parentWel = GetParentWebElement(childLocator);
            IWebElement siblingWel = GetElementWithinElement(parentWel, siblingXpath);

            return siblingWel;
        }

        public IWebElement GetOtherSiblingFromAGivenChild(IWebElement childElement, By siblingXpath)
        {
            IWebElement parentWel = GetParentWebElement(childElement);
            IWebElement siblingWel = GetElementWithinElement(parentWel, siblingXpath);

            return siblingWel;
        }

        /**
         * Method to get the parent element of the provided locator
         * @param childLocator
         * @return WebElement - Parent element
         */
        public IWebElement GetParentWebElement(By childLocator)
        {
            IWebElement childWel = GetElement(childLocator);
            IWebElement parentWel = childWel.FindElement(By.XPath(".."));

            return parentWel;
        }

        /**
         * Method to check if an element is present or not
         * @param locator
         * @return bool true/false
         */
        public bool IsElementPresent(By locator)
        {
            try
            {
                IWebElement element = GetLocatedElementByExplicitWait(locator);
                if (element != null)
                    return true;
            }
            catch (Exception nse)
            {
                string logMessage = string.Format(Assertions.ELEMENT_NOT_FOUND, Element.DEFAULT,
                        locator);
                LogInfo(logMessage);
                return false;
            }

            return false;
        }

        /**
         * Method to check if an element is present on page within zero seconds
         * @param by
         * @return bool true/false
         */
        public bool IsWebElementPresent(By by)
        {
            GetLocatedElementByExplicitWait(by, DEFAULT_TIMEOUT_FOR_ZERO_SECONDS);
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException e)
            {
                return false;
            }
        }

        //  /**
        //* To check, if the element is even present on the DOM. If it's not present, it certainly isn't visible.
        //* Then, if that returns true, I'll check the dimensions of the element:
        //* @param locator
        //* @return
        //*/
        //  public bool IsElementPresentOnDOMByElementSize(By locator)
        //  {
        //      bool isPresent = driver.FindElements(locator).Count > 0;

        //      if (isPresent)
        //      {
        //          Dimension dimension = GetElement(locator).Size(;
        //          return (dimension.getHeight() > 0 && dimension.getWidth() > 0);
        //      }

        //      return isPresent;
        //  }

        /**
         * Method to get hidden elements with style attribute display:none and set the value of a bool accordingly
         * @param locator
         * @return
         */
        public bool IsElementPresentOnDOMByCSSDisplayNone(By locator)
        {
            bool isEleVisible = false;
            //String elementStyle = getElementAttributeValue(locator, ElementAttribute.STYLE);
            string elementStyle = driver.FindElement(locator).GetAttribute(ElementAttribute.STYLE);
            isEleVisible = !(elementStyle.Equals(displayNoneStyleType) || elementStyle.Equals(visibilityStyleType));

            return isEleVisible;
        }

        /**
         * Method to check if an element is enabled or disabled by checking the 'disabled' value in class attribute
         * @param locator
         * @param toLogIfFalse
         * @return bool true/false
         */
        public bool IsElementDisabled(By locator, bool toLogIfFalse)
        {
            IWebElement webElement = GetElement(locator);
            bool isDisabled = IsElementDisabled(webElement, toLogIfFalse, locator);

            return isDisabled;
        }

        /**
         * Method to check if an element is enabled or disabled by checking the 'disabled' value in class attribute
         * @param webElement
         * @param toLogIfNot
         * @param locator
         * @return bool true/false
         */
        public bool IsElementDisabled(IWebElement webElement, bool toLogIfNot, By locator)
        {
            bool isDisabled = IsElementDisabled(webElement);

            if (!isDisabled && toLogIfNot)
                LogInfo(string.Format(Assertions.ELEMENT_IS_NOT_DISABLED, locator));

            return isDisabled;
        }

        /**
        * Method to check if an element is enabled or disabled by checking the 'disabled' value in class attribute
        * @param webElement
        * @return  bool true/false
        */
        public bool IsElementDisabled(IWebElement webElement)
        {
            bool isDisabled = false;
            if (webElement.Enabled)
            {
                string classValue = webElement.GetAttribute(ElementAttribute.CLASS);
                if (classValue.Contains(DISABLED_ATTRIBUTE))
                    isDisabled = true;
            }
            else
                isDisabled = true;

            return isDisabled;
        }

        /**
         * Method to check if an element is enabled or disabled by checking the 'disabled' value in class attribute. This
         * method is very specific to India because in some cases they just have the 'disabled' attribute with no value
         * @param toLogIfNot
         * @param locator
         * @return bool true/false
         */
        public bool IsElementDisabledForIndia(By locator, bool toLogIfNot)
        {
            bool isDisabled = false;
            IWebElement webElement = GetElement(locator);
            if (webElement.Enabled)
            {
                string disabledValue = webElement.GetAttribute(ElementAttribute.DISABLED);
                if (disabledValue != null && (disabledValue.Equals("true") || string.IsNullOrEmpty(disabledValue)))
                    isDisabled = true;
            }
            else
                isDisabled = true;

            if (!isDisabled && toLogIfNot)
                LogInfo(string.Format(Assertions.ELEMENT_IS_NOT_DISABLED, locator));

            return isDisabled;
        }

        /**
         * Method to find an element first with the locator and then verify if it's disabled
         * @param locator
         * @return boolean true/false
         */
        public bool IsElementPresentAndDisabled(By locator)
        {
            bool isPresentElementDisabled = false;
            IWebElement webElement = GetElement(locator);
            if (!webElement.Enabled)
                isPresentElementDisabled = true;

            return isPresentElementDisabled;
        }

        /**
         * Method to find an element first with the locator and then verify if it's disabled and log it if needed
         * @param locator
         * @return boolean true/false
         */
        public bool IsElementPresentAndDisabled(By locator, bool toLogIfFalse)
        {
            bool isPresentElementDisabled = false;
            IWebElement webElement = GetElement(locator);
            if (!webElement.Enabled)
                isPresentElementDisabled = true;

            if (!isPresentElementDisabled && toLogIfFalse)
                LogInfo(string.Format(Assertions.ELEMENT_IS_NOT_DISABLED, locator));

            return isPresentElementDisabled;
        }

        /**
         * Method to find an element first with the locator and then verify if it's enabled
         * @param locator
         * @return boolean true/false
         */
        public bool IsElementPresentAndEnabled(By locator)
        {
            bool isPresentElementEnabled = false;
            IWebElement webElement = GetElement(locator);
            if (webElement.Enabled)
                isPresentElementEnabled = true;

            return isPresentElementEnabled;
        }

        /**
         * Method to find an element first with the locator and then verify if it's enabled and log it if needed
         * @param locator
         * @param toLogIfFalse
         * @return boolean true/false
         */
        public bool IsElementPresentAndEnabled(By locator, bool toLogIfFalse)
        {
            bool isPresentElementEnabled = false;
            IWebElement webElement = GetElement(locator);
            if (webElement.Enabled)
                isPresentElementEnabled = true;

            if (!isPresentElementEnabled && toLogIfFalse)
                LogInfo(string.Format(Assertions.ELEMENT_IS_NOT_ENABLED, locator));

            return isPresentElementEnabled;
        }

        /**
         * Method to check if an element is absent or not
         * @param locator
         * @param timeOut
         * @return boolean true/false
         */
        public bool IsElementAbsent(By locator, TimeSpan timeOut)
        {
            return IsElementAbsent(locator, timeOut, true);
        }

        /**
         * Method to check if an element is absent or not and log message if required
         * @param locator
         * @param timeOut
         * @param logIfFailed
         * @return boolean true/false
         */
        public bool IsElementAbsent(By locator, TimeSpan timeOut, bool logIfFailed)
        {
            bool isAbsent = IsInvisibilityOfElementLocatedByUserDefinedExplicitWait(locator, timeOut, logIfFailed);
            if (!isAbsent && logIfFailed)
            {
                string logMessage = string.Format(Assertions.ELEMENT_FOUND, Element.DEFAULT,
                        locator);
                LogInfo(logMessage);
            }

            return isAbsent;
        }

        /**
        * Method to check invisibility of a WebElement with explicit wait with locator as input and timeout
        * @param locator
        * @param explicitTimeout
        * @return boolean true/false
        */
        public bool IsInvisibilityOfElementLocatedByUserDefinedExplicitWait(By locator, TimeSpan explicitTimeout,
                                                                               bool logIfFalse)
        {
            bool isInvisible = false;
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, explicitTimeout);
                isInvisible = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(locator));
            }
            catch (Exception exp)
            {
                if (logIfFalse)
                    LogInfo(exp.StackTrace);
            }

            return isInvisible;
        }

        /**
        * Method to check if an element is displayed or not
        * @param webElement
        * @return
        */
        public bool IsElementDisplayed(IWebElement webElement)
        {
            return webElement.Displayed;
        }

        /**
         * Method to check if an element is displayed or not
         * @param locator
         * @return boolean true/false
         */
        public bool IsElementDisplayed(By locator)
        {
            try
            {
                IWebElement element = GetLocatedElementByExplicitWait(locator);
                if (element != null && element.Displayed)
                    return true;
                else
                    LogInfo(string.Format(Assertions.ELEMENT_NOT_DISPLAYED, locator));
            }
            catch (Exception exp)
            {
                LogInfo(string.Format(Assertions.ELEMENT_NOT_FOUND, Element.DEFAULT,
                        locator));
                return false;
            }

            return false;
        }

        /**
         * Method to check if an element is clickable or not
         * @param locator
         * @return boolean true/false
         */
        public bool IsElementClickable(By locator)
        {
            try
            {
                IWebElement element = GetClickableElementByExplicitWait(locator);
                if (element != null)
                    return true;
            }
            catch (Exception nse)
            {
                LogInfo(nse.StackTrace);
                return false;
            }

            return false;
        }

        /**
        * Method to switch to an alert and accept it
        */
        public void SwitchToAlertAndAccept()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                alert.Accept();
            }
            catch (NoAlertPresentException e)
            {
                LogInfo("No alert present");
            }
            Pause(2000);
        }

        /**
        * Method to get the parent element of the provided locator
        * @param webElement
        * @return WebElement - Parent element
        */
        public IWebElement GetParentWebElement(IWebElement webElement)
        {
            IWebElement parentWel = webElement.FindElement(By.XPath(".."));

            return parentWel;
        }

        /**
        * Method to take screen shot of the view port only (not the whole page) and embed in the report file
        */
        public void TakeViewPortScreenShot()
        {
            string base64Data = ((ITakesScreenshot)driver).GetScreenshot().ToString();// OutputType.BASE64);
            string path = ("<img style=\"width:100%;\" src=data:image/png;base64," + base64Data + ">");
            reporter.AddScreenCaptureFromBase64String(path);//.addStepLog(path);
        }

        /**
         * Method to get WebElement's from a locator with xpath. The elements are found by using explicit waits
         * @param locator
         * @return list<WebElement></WebElement>
         */
        public IList<IWebElement> GetElementsByXpathByWait(string locator)
        {
            By locatorxpath = By.XPath(locator);
            IList<IWebElement> elements = GetListOrTableRows(locatorxpath);

            return elements;
        }

        /**
        * Method to get all table rows
        * @return List<WebElement>
        */
        public IList<IWebElement> GetListOrTableRows(By locator)
        {
            IList<IWebElement> tableList = new List<IWebElement>();
            bool isTablePopulated = AreEnoughRowsInTableOrItemsInListPresentCheckedByFluentWait(locator);
            if (isTablePopulated)
                tableList = GetElements(locator);
            else
                LogInfo(string.Format(Assertions.TR_NOT_GT, locator,
                        NUMBER_ZERO_STRING));

            return tableList;
        }

        /**
         * Method to verify if a table is loaded with enough values to proceed further
         * @param locator
         * @return bool true/false
         */
        public bool AreEnoughRowsInTableOrItemsInListPresentCheckedByFluentWait(By locator)
        {
            bool areEnoughRowsPresent = false;
            try
            {
                DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(driver);
                wait.Timeout = TimeSpan.FromSeconds(DEFAULT_GET_TEXT_TIMEOUT_IN_SECONDS);
                wait.PollingInterval = TimeSpan.FromSeconds(POLLING_IN_EVERY_ONE_SECOND);

                areEnoughRowsPresent = wait.Until<int>((welDev) =>
                {
                    return welDev.FindElements(locator).Count;
                }) > NUMBER_ZERO_VALUE;
            }
            catch (Exception exp)
            {
                LogInfo(exp.StackTrace);
            }

            return areEnoughRowsPresent;
        }

        /**
        * Method to get WebElement from a locator with xpath and without any waits
        * @param locator
        * @return WebElement
        */
        public IWebElement GetElementByXpathWithoutWait(string locator)
        {
            By locatorXpath = By.XPath(locator);
            IWebElement webElement = driver.FindElement(locatorXpath);

            if (webElement == null)
                LogInfo(string.Format(Assertions.ELEMENT_NOT_FOUND, Element.DEFAULT,
                        locator));

            return webElement;
        }

        /**
         * Method to get WebElement from a locator with xpath and with explicit wait
         * @param locator
         * @return WebElement
         */
        public IWebElement GetElementByXpathSuppressException(string locator)
        {
            IWebElement webElement;
            try
            {
                By locatorXpath = By.XPath(locator);
                webElement = GetElementByFluentWait(locatorXpath, NOT_FOUND_SHORT_TIMEOUT_IN_SECONDS);
            }
            catch (Exception exp)
            {
                return null;
            }

            return webElement;
        }

        /**
         * Method to return WebElement with fluent wait with locator and timeout as input
         * @param locator
         * @param timeOut
         * @return WebElement
         */
        public IWebElement GetElementByFluentWait(By locator, int timeOut)
        {
            IWebElement webElement = null;
            try
            {
                DefaultWait<IWebDriver> wait = new DefaultWait<IWebDriver>(driver);
                wait.Timeout = TimeSpan.FromSeconds(timeOut);
                wait.PollingInterval = TimeSpan.FromSeconds(POLLING_IN_EVERY_ONE_SECOND);
                wait.IgnoreExceptionTypes(typeof(NoSuchElementException));

                webElement = wait.Until<IWebElement>((webDriver) =>
                {
                    return webDriver.FindElement(locator);
                });

            }
            catch (Exception exp)
            {
                LogInfo(exp.StackTrace);
            }
            return webElement;
        }

        /**
         * Method to return clickable WebElement with explicit wait with locator as input
         * @param locator
         * @param locator
         * @return WebElement
         */
        public IWebElement GetClickableElementByExplicitWait(By locator, bool logIfFalse)
        {
            IWebElement webElement = null;
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(DEFAULT_TIMEOUT_IN_SECONDS));
                webElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
            }
            catch (Exception exp)
            {
                if (logIfFalse)
                    LogInfo(exp.StackTrace);
            }

            return webElement;
        }

        /**
         * Method to return clickable WebElement with explicit wait with locator as input without catching any error
         * @param locator
         * @return WebElement
         */
        public IWebElement GetClickableElementByExplicitWaitWithoutCatchingError(By locator)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(DEFAULT_TIMEOUT_IN_SECONDS));
            IWebElement webElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));

            return webElement;
        }

        /**
         * Method to return clickable WebElement with explicit wait with locator as input
         * @param elementsXpath
         * @return List<WebElement>
         */
        public IList<IWebElement> GetElementByXpathByExplicitWait(string elementsXpath)
        {
            IList<IWebElement> webElementList = new List<IWebElement>();
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(DEFAULT_TIMEOUT_IN_SECONDS));
                webElementList = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.PresenceOfAllElementsLocatedBy(
                        By.XPath(elementsXpath)));
            }
            catch (Exception exp)
            {
                LogInfo(exp.StackTrace);
            }

            return webElementList;
        }

        /**
        * Method to scroll to the given element to make it visible on screen
        * @param locator
        */
        public void ScrollElementToView(By locator)
        {
            try
            {
                IWebElement toScrollWel = driver.FindElement(locator);
                IJavaScriptExecutor javascriptExecutor = (IJavaScriptExecutor)driver;
                javascriptExecutor.ExecuteScript("arguments[0].scrollIntoView(true)", toScrollWel);
            }
            catch (Exception exp)
            {
                LogInfo(exp.StackTrace);
            }
        }

        /**
         * Method to scroll to the given element to make it visible on screen
         * @param webElement
         */
        public void ScrollElementToView(IWebElement webElement)
        {
            try
            {
                IJavaScriptExecutor javascriptExecutor = (IJavaScriptExecutor)driver;
                javascriptExecutor.ExecuteScript("arguments[0].scrollIntoView(true)", webElement);
            }
            catch (Exception exp)
            {
                LogInfo(exp.StackTrace);
            }
        }

        /**
        * Method to return located WebElement with explicit wait with locator as input
        * @param locator
        * @return WebElement
        */
        public IWebElement GetLocatedElementByExplicitWait(By locator)
        {
            IWebElement webElement = null;
            try
            {
                WebDriverWait wait = WaitFor();
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(locator));

                return driver.FindElement(locator);
            }
            catch (Exception exp)
            {
                LogInfo(exp.StackTrace);
            }

            return webElement;
        }

        /**
        * Method to return located WebElement with explicit wait with locator as input
        * @param locator
        * @return WebElement
        */
        public IWebElement GetLocatedElementByExplicitWait(By locator, int timeOut)
        {
            IWebElement webElement = null;
            try
            {
                WebDriverWait wait = WaitFor(timeOut);
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(locator));

                return driver.FindElement(locator);
            }
            catch (Exception exp)
            {
                LogInfo(exp.StackTrace);
            }

            return webElement;
        }

        //public WebDriverWait WaitFor(int timeOut)
        //{
        //    return new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut));
        //}

        //public WebDriverWait WaitFor()
        //{
        //    return new WebDriverWait(driver, TimeSpan.FromSeconds(DEFAULT_TIMEOUT_IN_SECONDS));
        //}

        public static string getFieldInfoData(object Obj, string expectedString)
        {
            FieldInfo[] fieldsValues = Obj.GetType().GetFields();
            string defaultString = "";
            foreach (FieldInfo info in fieldsValues)
            {
                if (expectedString.Equals(info.GetValue(Obj)))
                {
                    defaultString = info.GetValue(Obj).ToString();
                    break;
                }
            }
            return defaultString;
        }
    }
}

