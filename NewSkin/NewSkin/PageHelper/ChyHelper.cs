
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using NewSkin.PageHelper.Comm;
using System.Threading;
using System;

namespace NewSkin.PageHelper
{
    public class ChyHelper : DriverHelper
    {
        public LocatorReader locatorReader;

        public ChyHelper(IWebDriver idriver, string xmlFile)
            : base(idriver)
        {
            locatorReader = new LocatorReader(xmlFile);
        }

        /* All the methods needed to click buttons and navigate webpage*/

        //Type into given xml node (type info to be entered into Pergasus)
        public void TypeText(string Field, string text)
        {
            var locator = locatorReader.ReadLocator(Field);
            WaitForElementPresent(locator, 20);
            SendKeys(locator, text);
        }



        //Select by value (from drop down menu)
        public void Select(string xmlNode, string value)
        {
            var locator = locatorReader.ReadLocator(xmlNode);
            SelectDropDown(locator, value);
        }
        public void SelectByText(string xmlNode, string value)
        {
            var locator = locatorReader.ReadLocator(xmlNode);
            SelectDropDownByText(locator, value);
        }
        public void SelectByIndex(string xmlNode, int value)
        {
            var locator = locatorReader.ReadLocator(xmlNode);
            SelectDropDownByIndex(locator, value);
        }
        //Click buttons and check boxes

        public void ClickElement(string xmlNode)
        {
            var locator = locatorReader.ReadLocator(xmlNode);
            WaitForElementPresent(locator, 20);
            Click(locator);
            WaitForWorkAround(2000);
        }

        //Upload a file
        internal void Upload(string Field, string FileName)
        {
            var locator = locatorReader.ReadLocator(Field);
            WaitForElementVisible(locator, 20);
            GetWebDriver().FindElement(ByLocator(locator)).SendKeys(FileName);
        }


        //Hover over tab without clicking then click on subsequent menu item
        internal void MouseHover(string field)
        {

            var locator = locatorReader.ReadLocator(field);
            WaitForElementPresent(locator, 20);
            MouseOver(locator);
            WaitForWorkAround(2000);

        }


        public void RedirectToURL(string urlName)
        {
            GetWebDriver().Navigate().GoToUrl(urlName);
        }

        //Verify method Present method

        public void VerifyTrueOrFalse(System.Boolean flag, string text)
        {

          
            var result = GetWebDriver().PageSource.Contains(text);
            //  var locator = locatorReader.ReadLocator(Company);
            if (flag == true)
            {
                Assert.IsTrue(result, "Text String: " + text + " Not Found.");
                Thread.Sleep(3000);
            }
            else
            {
                Assert.IsFalse(result, "Text String: " + text + " Found.");
                Thread.Sleep(3000);
            }

        }
        public void VerifyNodeTextTrue(string XmlNode, string text)
        {
            var locator = locatorReader.ReadLocator(XmlNode);
            var nodeText = GetText(locator);
            
            Assert.IsTrue(nodeText.Contains(text));
            Thread.Sleep(3000);
            
            
        }
        public void VerifyNodeTextFalse(string XmlNode, string text)
        {
            var locator = locatorReader.ReadLocator(XmlNode);
            var nodeText = GetText(locator);
         
            Assert.IsFalse(nodeText.Contains(text));
            Thread.Sleep(3000);
         

        }
        public void VerifyNodeValue(string XmlNode, string text)
        {
            var locator = locatorReader.ReadLocator(XmlNode);
            var value = GetValue(locator);
            Assert.IsTrue(value.Contains(text));
        }
        // Method to verify text in page source
        public new void VerifyPageText(string text)
        {
            var result = GetWebDriver().PageSource.Contains(text);
            Assert.IsTrue(result, "Text String: " + text + " Not Found.");
            Thread.Sleep(3000);
        }
        public string GetTextOfNode(string xmlNode)
        {
            var locator = locatorReader.ReadLocator(xmlNode);
            string value = GetText(locator);
            return value;
        }
        public bool IsElementPresentLocator(string xmlNode)
        {

            var locator = locatorReader.ReadLocator(xmlNode);
            WaitForElementPresent(locator, 20);
            bool present = IsElementPresent(locator);
            return present;
        }
        public void DragAndDropElement(string source, string target)
        {
            var locator1 = locatorReader.ReadLocator(source);
            var locator2 = locatorReader.ReadLocator(target);
            DragAndDrop(locator1, locator2);

        }
        public void WaitUntilElementIsPresent(string xmlNode)
        {
            var locator = locatorReader.ReadLocator(xmlNode);
            WaitForElementPresent(locator, 20);
        }
        public void WaitUntilElementIsVisible(string xmlNode)
        {
            var locator = locatorReader.ReadLocator(xmlNode);
            WaitForElementVisible(locator, 20);
        }
        public void ClearTextBox(string xmlNode)
        {
            var locator = locatorReader.ReadLocator(xmlNode);
            WaitForElementVisible(locator, 20);
            ClearTextBoxValue(locator);
        }
        public bool CheckSelectOptionThere(string xmlNode, string text)
        {
            var locator = locatorReader.ReadLocator(xmlNode);
            try
            {
                SelectDropDownByText(locator, text);
            }

            catch
            {
                return false;
            }
            return true;
        }
        public void ImplicitWait(int waitTime)
        {

            GetWebDriver().Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(waitTime));
        }
        public void VerifyAnyNodeWithText(bool tOrF,string text)
        {
          
             int count=GetWebDriver().FindElements(By.XPath("//*[contains(text(),'" + text + "')]")).Count;
            if (count > 0 && tOrF == true)
                Assert.IsTrue(true);
            else if (count == 0 && tOrF == true)
                Assert.IsTrue(false);
            else if (count > 0 && tOrF == false)
                Assert.IsFalse(true);
            else if (count == 0 && tOrF == false)
                Assert.IsFalse(false);

        }
        public void PressEnter(string xmlNode)
        {
            var locator = locatorReader.ReadLocator(xmlNode);
            WaitForElementVisible(locator, 20);
            GetWebDriver().FindElement(ByLocator(locator)).SendKeys(Keys.Enter);
        }

    }
}