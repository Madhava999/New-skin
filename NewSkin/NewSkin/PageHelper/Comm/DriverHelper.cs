using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;



namespace NewSkin.PageHelper.Comm
{
    public abstract class DriverHelper
    {
        private readonly IWebDriver _driver;

        public DriverHelper(IWebDriver idriver)
        {
            _driver = idriver;
        }

        public IWebDriver GetWebDriver()
        {
            return _driver;
        }

        public By ByLocator(string locator)
        {
            By result = null;

            if (locator.StartsWith("//")|| locator.StartsWith("("))
            {
                result = By.XPath(locator);
            }
            
            else if (locator.StartsWith("xpath="))
            {
                result = By.XPath(locator.Replace("xpath=", ""));
            }
            else if (locator.StartsWith("css="))
            {
                result = By.CssSelector(locator.Replace("css=", ""));
            }
            else if (locator.StartsWith("#"))
            {
                result = By.Name(locator.Replace("#", ""));
            }
            else if (locator.StartsWith("link="))
            {
                result = By.LinkText(locator.Replace("link=", ""));
            }

            else
            {
                result = By.Id(locator);
            }

            return result;
        }

        public void SelectWindow(string title)
        {
            foreach (var item in _driver.WindowHandles.Where(item => _driver.SwitchTo().Window(item).Title.Equals(title)))
            {
                _driver.SwitchTo().Window(item);
                break;
            }
        }

        public void SelectWindowWithTitle(string title)
        {
            foreach (var item in _driver.WindowHandles.Where(item => _driver.SwitchTo().Window(item).Title.Contains(title)))
            {
                _driver.SwitchTo().Window(item);
                break;
            }
        }

        public void SwitchWindowWithSimilerTitle(string title, string Id)
        {
            foreach (var item in _driver.WindowHandles.Where(item => _driver.SwitchTo().Window(item).Title.Equals(title) && item != Id))
            {
                _driver.SwitchTo().Window(item);
                break;
            }
        }

        public bool IsElementPresent(string locator)
        {
            bool result;
            try
            {
                _driver.FindElement(ByLocator(locator));
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        public int CheckboxCount()
        {
            return _driver.FindElements(By.XPath("//input[@type='checkbox']")).Count();
        }

        public void WaitForElementPresent(string locator, int timeout)
        {
            for (var i = 0; i < timeout * 10; ++i)
            {
                if (IsElementPresent(locator))
                {
                    break;
                }

                try
                {
                    Thread.Sleep(100);
                }
                catch (Exception)
                {
                    //e.printStackTrace();
                }
            }
        }

        public void WaitForElementEnabled(string locator, int timeout)
        {
            for (var i = 0; i < timeout * 10; ++i)
            {
                if (IsElementPresent(locator) && _driver.FindElement(ByLocator(locator)).Enabled)
                {
                    break;
                }

                try
                {
                    Thread.Sleep(100);
                }
                catch (Exception)
                {
                    // Ignore exception.
                }
            }
        }

        public void WaitForElementNotEnabled(string locator, int timeout)
        {
            for (var i = 0; i < timeout * 10; ++i)
            {
                if (IsElementPresent(locator) && !_driver.FindElement(ByLocator(locator)).Enabled)
                {
                    break;
                }

                try
                {
                    Thread.Sleep(100);
                }
                catch (Exception)
                {
                    // Ignore exception.
                }
            }
        }

        public void WaitForElementVisible(string locator, int timeout)
        {
            for (var i = 0; i < timeout * 10; ++i)
            {
                if (IsElementPresent(locator) && _driver.FindElement(ByLocator(locator)).Displayed)
                {
                    break;
                }

                try
                {
                    Thread.Sleep(100);
                }
                catch (Exception)
                {
                    // Ignore exception.
                }
            }
        }

        public void WaitForElementNotVisible(string locator, int timeout)
        {
            for (var i = 0; i < timeout * 10; ++i)
            {
                if (IsElementPresent(locator) && !_driver.FindElement(ByLocator(locator)).Displayed)
                {
                    break;
                }

                try
                {
                    Thread.Sleep(100);
                }
                catch (Exception)
                {
                    // Ignore exception.
                }
            }
        }

        public void SendKeys(string locator, string value)
        {
            WaitForElementPresent(locator, 20);
            Assert.IsTrue(IsElementPresent(locator));
            var el = GetWebDriver().FindElement(ByLocator(locator));
            el.Clear();
            el.SendKeys(value);
        }

        public void Click(string locator)
        {
            WaitForElementPresent(locator, 20);
            Assert.IsTrue(IsElementPresent(locator));
            _driver.FindElement(ByLocator(locator)).Click();
        }

        public void SelectDropDown(string locator, string targetValue)
        {
            WaitForElementPresent(locator, 20);
            Assert.IsTrue(IsElementPresent(locator));

            var dropDownListBox = _driver.FindElement(ByLocator(locator));
            var clickThis = new SelectElement(dropDownListBox);
            clickThis.SelectByValue(targetValue);
        }

        public void SelectDropDownByIndex(string locator, int targetValue)
        {
            WaitForElementPresent(locator, 20);
            Assert.IsTrue(IsElementPresent(locator));

            var dropDownListBox = _driver.FindElement(ByLocator(locator));
            var clickThis = new SelectElement(dropDownListBox);
            clickThis.SelectByIndex(targetValue);
        }

        public string GetText(string locator)
        {
            var value = "";
            WaitForElementPresent(locator, 20);
            Assert.IsTrue(IsElementPresent(locator));
            var textval = _driver.FindElement(ByLocator(locator));
            value = textval.Text;
            return value;
        }

        public string GetTextFromNonVisibleElement(string locator)
        {
            var value = "";
            WaitForElementPresent(locator, 20);
            Assert.IsTrue(IsElementPresent(locator));
            var textval = _driver.FindElement(ByLocator(locator));
            value = ((IJavaScriptExecutor)_driver).ExecuteScript("return arguments[0].innerHTML", textval).ToString();
            return value;
        }

        public void SelectAndClosePopUp(string title)
        {
            foreach (var item in _driver.WindowHandles.Where(item => _driver.SwitchTo().Window(item).Title.Equals(title)))
            {
                _driver.SwitchTo().Window(item);
                _driver.Close();
                break;
            }
        }
        //Used to get the value of a specific element on the page
        public string GetValue(string locator)
        {
            var value = "";
            WaitForElementPresent(locator, 20);
            Assert.IsTrue(IsElementPresent(locator));
            var textval = _driver.FindElement(ByLocator(locator));
            value = textval.GetAttribute("value");
            return value;
        }

        public string GetAttributeValue2(string text)
        {
            return _driver.FindElement(
                    By.XPath("//select[@id='ctl00_ContentPlaceHolderBody_drpClass']/option[contains(text(), '" + text +
                             "')]")).GetAttribute("value");
        }

        public string GetAttributeValuep(string text, string id)
        {
            return _driver.FindElement(By.XPath("//select[@id='" + id + "']/option[contains(text(), '" + text + "')]"))
                    .GetAttribute("value");
        }

        public string GetAtrributeByXpath(string xpath, string attribute)
        {
            return _driver.FindElement(By.XPath(xpath)).GetAttribute(attribute);
        }

        public string GetAtrributeByLocator(string locator, string attribute)
        {
            return _driver.FindElement(ByLocator(locator)).GetAttribute(attribute);
        }

        public bool IsFieldDisabled(string locator, string attribute)
        {
            var bRetVal = false;

            try
            {
                bRetVal = Convert.ToBoolean(_driver.FindElement(ByLocator(locator)).GetAttribute(attribute));
            }
            catch (Exception)
            {
                // Ignore exception.
            }

            return bRetVal;
        }

        public void SelectDropDownByText(string locator, string targetText)
        {
            WaitForElementPresent(locator, 20);
            Assert.IsTrue(IsElementPresent(locator));

            var dropDownListBox = _driver.FindElement(ByLocator(locator));
            var clickThis = new SelectElement(dropDownListBox);

            clickThis.SelectByText(targetText);
        }

        // Clear Text box Value.
        public void ClearTextBoxValue(string locator)
        {
            WaitForElementPresent(locator, 20);
            Assert.IsTrue(IsElementPresent(locator));
            var el = GetWebDriver().FindElement(ByLocator(locator));
            el.Clear();
        }

        // Count number Of Rows.
        public int XpathCount(string xPath)
        {
            var count = _driver.FindElements(By.XPath(xPath)).Count;
            return count;
        }

        public void ClickViaJavaScript(string locator)
        {
            WaitForElementPresent(locator, 20);
            Assert.IsTrue(IsElementPresent(locator));
            var el = _driver.FindElement(ByLocator(locator));

            //OpenQA.Selenium.Interactions.Actions actions = new OpenQA.Selenium.Interactions.Actions(driver);
            //actions.MoveToElement(el).ClickAndHold();

            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", el);
        }

        /* dan - Not sure we need this or not
        //Is text Present
        public bool IsTextPresent(string locator, string sTextToFind)
        {
            bool bRetVal = false;
            IWebElement element = GetWebDriver().FindElement(ByLocator(locator));
            if (element.Text == sTextToFind)
            {
                bRetVal = true;
            }

            return bRetVal;
        }
        */

        public string GetIdByAtttribute(string locator)
        {
            var sRetVal = "";

            var we = _driver.FindElement(By.XPath(locator));
            sRetVal = we.GetAttribute("id");

            return sRetVal;
        }

        public void CloseSelectedWindow()
        {
            _driver.Close();
        }

        // Method to click on button using btn text
        public void ClickButtonText(string btnText)
        {
            var locator = "//span[@class='ui-button-text' and contains(text(), '" + btnText + "')]";
            WaitForElementPresent(locator, 20);
            Click(locator);
        }

        // Method to verify text in page source
        public void VerifyPageText(string text)
        {
            var result = GetWebDriver().PageSource.Contains(text);
            Assert.IsTrue(result, "Text String: " + text + "Not Found.");
        }

        // Wait
        public void WaitForWorkAround(int number)
        {
            Thread.Sleep(number);
        }

        public void ArrowDown(string locator)
        {
            Assert.IsTrue(IsElementPresent(locator));
            GetWebDriver().FindElement(ByLocator(locator)).SendKeys(Keys.ArrowDown);
        }

        public void MouseOver(string locator)
        {
            var el = GetWebDriver().FindElement(ByLocator(locator));

            var builder = new Actions(GetWebDriver());
            builder.MoveToElement(el).Build().Perform();
        }

        public void AcceptAlert()
        {
            WaitForWorkAround(2000);
            GetWebDriver().SwitchTo().Alert().Accept();
        }



        /*    public void drawSign()
             {
                 Actions builder = new Actions(GetWebDriver());
                 Action drawAction = builder.MoveToElement(//*[@id='clicknew']", 50, 100)
        
                     //signatureWebElement is the element that holds the signature element you have in the DOM
                           .ClickAndHold()
                           .moveByOffset(100, 50)
                           .moveByOffset(6, 7)
                           .release()
                           .build();
                  drawAction.Perform();    

             }    */


       public void DragAndDrop(string element,string target)
       {
            WaitForElementPresent(element, 20);
            Assert.IsTrue(IsElementPresent(element));
            WaitForElementPresent(target, 20);
            Assert.IsTrue(IsElementPresent(target));
            var el = GetWebDriver().FindElement(ByLocator(element));
            var tar = GetWebDriver().FindElement(ByLocator(target));
            var builder = new Actions(GetWebDriver());
            //maybe take out .Build
            builder.DragAndDrop(el,tar).Build().Perform();
            
        }

    }
}