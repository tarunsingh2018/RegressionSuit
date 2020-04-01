using Framework.MarketIT.Automation_Framework.Utilities.Common;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.MarketIT.Automation_Framework.Utilities.Factory
{
    public class ObjectFactory
    {
        //Generic object creation for pages
        public static T CreatePage<T>(IWebDriver webDriver) where T : BasePage, new()
        {
            var page = new T();
            page.Initialize(webDriver);
            return page;
        }

        //Generic object creation for components
        public static T CreateWidget<T>(IWebDriver webDriver, params object[] constructorArgs) where T : BaseWidget
        {
            var widget = Activator.CreateInstance(typeof(T), constructorArgs) as T;
            Debug.Assert(widget != null, "widget != null");
            widget.Initialize(webDriver);
            return widget;
        }
    }
}
