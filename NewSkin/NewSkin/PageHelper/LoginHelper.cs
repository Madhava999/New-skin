using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using NewSkin.PageHelper.Comm;

namespace NewSkin.PageHelper
{
    public class LoginHelper : DriverHelper
    {
        public LocatorReader locatorReader;

        public LoginHelper(IWebDriver idriver)
            : base(idriver)
        {
            locatorReader = new LocatorReader("Login2.xml");
        }

        public void EnterUserName(string username)
        {
            var locator = locatorReader.ReadLocator("Login/Username");
            SendKeys(locator, username);
        }

        public void EnterPassword(string password)
        {
            var locator = locatorReader.ReadLocator("Login/Password");
            SendKeys(locator, password);
        }

        public void ClickEnterButton()
        {
            var locator = locatorReader.ReadLocator("Login/Enter");
            Click(locator);
        }

        public void verifyErrorMessages(string msg)
        {
            GetWebDriver().PageSource.Contains(msg);
        }

        public void ClickUserIcon()
        {
            var locator = locatorReader.ReadLocator("Logout/UserIcon");

            WaitForElementPresent(locator, 20);
            MouseOver(locator);
            WaitForWorkAround(1000);
        }

        public void ClickLogOff()
        {
            var locator = locatorReader.ReadLocator("Logout/LogOff");
            Click(locator);
        }


        //Click to given xml node
        public void ClickElement(string XmlNode)
        {
            var locator = locatorReader.ReadLocator(XmlNode);
            Click(locator);
        }

        //Type into given xml node
        public void TypeText(string XmlNode, string text)
        {
            var locator = locatorReader.ReadLocator(XmlNode);
            SendKeys(locator, text);
        }

        //Verify text of given xml node
        public void VerifyText(string XmlNode, string text)
        {
            var locator = locatorReader.ReadLocator(XmlNode);
            var value = GetText(locator);
            Assert.IsTrue(value.Contains(text));
        }

        //Select From DropDown

        public void Select(string Field, string TrgValue)
        {
            var locator = locatorReader.ReadLocator(Field);
            SelectDropDown(locator, TrgValue);

        }

    }
}