using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace NewSkin.Util
{
    /// <summary>
    ///     This class represents a Browser object backed by the Selenium webdriver.
    /// </summary>
    public class Browser
    {
        private int _waitTime;

        /// <summary>
        ///     Construct a new Browser object. It is currently hardcoded to use ChromeDriver()
        ///     since that is the only browser that PegasusCRM officially supports.
        /// </summary>
        public Browser()
        {
            WebDriver = new ChromeDriver();
        }

        public IWebDriver WebDriver { get; }
        public string Title => WebDriver.Title;

        /// <summary>
        ///     Set or get Selenium's ImplicitWait time in seconds. This allows Selenium
        ///     to wait up to this time for an element to become visible.
        /// </summary>
        public int ImplicitWait
        {
            get { return _waitTime; }
            set
            {
                _waitTime = value;
                WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(value));
            }
        }

        /// <summary>
        ///     The text in the alert box.
        /// </summary>
        public string AlertText => WebDriver.SwitchTo().Alert().Text;

        /// <summary>
        ///     Get the Actions object to perform action sequences.
        /// </summary>
        public Actions Sequence => new Actions(WebDriver);

        /// <summary>
        ///     Refresh the webpage.
        /// </summary>
        public Browser Refresh()
        {
            WebDriver.Navigate().Refresh();
            return this;
        }

        /// <summary>
        ///     Maximize the selenium browser window.
        /// </summary>
        /// <returns>The browser instance.</returns>
        public Browser Maximize()
        {
            WebDriver.Manage().Window.Maximize();
            return this;
        }

        /// <summary>
        ///     Accepts the alert dialog.
        /// </summary>
        /// <returns>The browser instance</returns>
        public Browser AlertAccept()
        {
            Wait(1);
            WebDriver.SwitchTo().Alert().Accept();
            return this;
        }

        /// <summary>
        ///     Dismisses the alert dialog.
        /// </summary>
        /// <returns>The Browser instance.</returns>
        public Browser AlertDismiss()
        {
            Wait(1);
            WebDriver.SwitchTo().Alert().Dismiss();
            return this;
        }

        /// <summary>
        ///     Quits the Selenium webdriver instance.
        /// </summary>
        public void Quit()
        {
            WebDriver.Quit();
        }

        /// <summary>
        ///     Navigates the Browser to the url given in the parameter.
        ///     The Browser object is returned for chainability.
        /// </summary>
        /// <param name="url">The url to navigate to.</param>
        /// <returns>The Browser object.</returns>
        public Browser GoToUrl(string url)
        {
            WebDriver.Navigate().GoToUrl(url);
            return this;
        }

        /// <summary>
        ///     Fill out a form specified by the By object with a specified value.
        /// </summary>
        /// <param name="by">The By instance to find the HTML element.</param>
        /// <param name="value">The value to fill the form with.</param>
        /// <returns>The Browser object.</returns>
        public Browser FillForm(By by, string value)
        {
            WebDriver.FindElement(by).SendKeys(value);
            return this;
        }

        public Browser FillForm(LocatorReader r, string node, string value)
        {
            return FillForm(r.Get(node), value);
        }

        /// <summary>
        ///     Clear a form specified by the By object.
        /// </summary>
        /// <param name="by">The By instance to find the HTML element.</param>
        /// <returns>The Browser object.</returns>
        public Browser ClearForm(By by)
        {
            WebDriver.FindElement(by).Clear();
            return this;
        }

        public Browser ClearForm(LocatorReader r, string node)
        {
            return ClearForm(r.Get(node));
        }

        /// <summary>
        ///     Fills out a form, replacing the existing contents.
        /// </summary>
        /// <param name="by">The By instance to find the HTML element.</param>
        /// <param name="value">The value to fill the form with.</param>
        /// <returns>The Browser instance.</returns>
        public Browser FillFormReplace(By by, string value)
        {
            return FillForm(by, Convert.ToString('\u0001') + value);
        }

        public Browser FillFormReplace(LocatorReader r, string node, string value)
        {
            return FillFormReplace(r.Get(node), value);
        }

        public Browser FillFormReplace(IWebElement e, string value)
        {
            e.SendKeys(Convert.ToString('\u0001') + value);
            return this;
        }

        /// <summary>
        ///     Click on a HTML element selected by the By object.
        /// </summary>
        /// <param name="by">The By object used to find an element.</param>
        /// <returns>The Browser object.</returns>
        public Browser Click(By by)
        {
            WebDriver.FindElement(by).Click();
            return Wait(1);
        }

        public Browser Click(LocatorReader r, string node)
        {
            return Click(r.Get(node));
        }

        /// <summary>
        ///     MouseOver a sequence of By objects in order.
        /// </summary>
        /// <param name="bys">A variadic list of By objects to mouse over.</param>
        /// <returns>The browser instance.</returns>
        public Browser MouseOver(params By[] bys)
        {
            var actions = new Actions(WebDriver);
            bys.Select(by => WebDriver.FindElement(by)).ToList()
                .ForEach(elem => actions.MoveToElement(elem));
            actions.Build().Perform();
            return this;
        }

        public Browser MouseOver(LocatorReader r, params string[] nodes)
        {
            return MouseOver(nodes.ToList().Select(e => r.Get(e)).ToArray());
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return WebDriver.FindElements(@by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(LocatorReader r, string node)
        {
            return FindElements(r.Get(node));
        }

        public IWebElement FindElement(By by)
        {
            return WebDriver.FindElement(@by);
        }

        public IWebElement FindElement(LocatorReader r, string node)
        {
            return FindElement(r.Get(node));
        }

        /// <summary>
        ///     Check whether a list of elements are visible.
        /// </summary>
        /// <param name="bys">The By objects used to select the elements.</param>
        /// <returns>Whether or not all the elements are visible.</returns>
        public bool ElementsVisible(params By[] bys)
        {
            return bys.ToList().Select(FindElements).ToList()
                .TrueForAll(es => es.Count > 0 && es.ToList().TrueForAll(e => e.Displayed));
        }

        public bool ElementsVisible(LocatorReader r, params string[] nodes)
        {
            return ElementsVisible(nodes.ToList().Select(e => r.Get(e)).ToArray());
        }

        internal object DropdownSelectByIndex(By by, string v)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///     Get the number of elements with a specific by selector.
        /// </summary>
        /// <param name="by">The by selector object.</param>
        /// <returns>The count of elements found.</returns>
        public int ElementCount(By by)
        {
            return FindElements(by).Count;
        }

        public int ElementCount(LocatorReader r, string node)
        {
            return ElementCount(r.Get(node));
        }

        public Browser DropdownSelectByValue(By by, string value)
        {
            var dropdown = new SelectElement(FindElement(by));
            dropdown.SelectByValue(value);
            return this;
        }

        public Browser DropdownSelectByValue(LocatorReader r, string node, string value)
        {
            return DropdownSelectByValue(r.Get(node), value);
        }

        public Browser DropdownSelectByText(By by, string text)
        {
            var dropdown = new SelectElement(FindElement(by));
            dropdown.SelectByText(text);
            return this;
        }

        public Browser DropdownSelectByText(LocatorReader r, string node, string text)
        {
            return DropdownSelectByText(r.Get(node), text);
        }

        /// <summary>
        ///     Select an option from the dropdown by its index.
        /// </summary>
        /// <param name="by">The by selector.</param>
        /// <param name="index">The index to use.</param>
        /// <returns>The browser instance.</returns>
        public Browser DropdownSelectByIndex(By by, int index)
        {
            new SelectElement(FindElement(by)).SelectByIndex(1);
            return this;
        }

        public Browser DropdownSelectByIndex(LocatorReader r, string node, int index)
        {
            return DropdownSelectByIndex(r.Get(node), index);
        }

        /// <summary>
        ///     Upload a file to the selected element.
        /// </summary>
        /// <param name="by">The by selector to find the input field.</param>
        /// <param name="filename">The name of the file in Resources/</param>
        /// <returns>The current Browser instance.</returns>
        public Browser UploadFile(By by, string filename)
        {
            FillForm(by, @Resources.GetPath(filename));
            return this;
        }

        /// <summary>
        ///     Upload a file to the selected element.
        /// </summary>
        /// <param name="r">The LocatorReader instance.</param>
        /// <param name="node">The node to get in the LocatorReader.</param>
        /// <param name="filename">The name of the file in Resources/</param>
        /// <returns>The current Browser instance.</returns>
        public Browser UploadFile(LocatorReader r, string node, string filename)
        {
            return UploadFile(r.Get(node), filename);
        }

        /// <summary>
        ///     Switch to iframe element.
        /// </summary>
        /// <param name="frameElement">The iframe element.</param>
        /// <returns>The driver</returns>
        public IWebDriver SwitchToFrame(IWebElement frameElement)
        {
            return WebDriver.SwitchTo().Frame(frameElement);
        }

        /// <summary>
        ///     Check if the text exists on the current page.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool TextExists(string text)
        {
            var selector = By.XPath($"//*[contains(text(), \"{text}\")]");
            var elements = FindElements(selector);
            return elements.Count > 0 && elements.Any(i => i.Displayed);
        }

        /// <summary>
        ///     Blocking wait with Thread.Sleep().
        /// </summary>
        /// <param name="seconds">The amount of time in seconds to wait.</param>
        /// <returns>The browser instance.</returns>
        public Browser Wait(int seconds)
        {
            Thread.Sleep(seconds*1000);
            return this;
        }
    }
}