using System;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using Selenium;

namespace NewSkin.PageHelper.Comm
{
    public abstract class DriverTestCase
    {
        private static bool _isExecutedFirst;
        private static string _oldfname;
        private static string _name;
        private readonly XMLParse _oWaXmlData = new XMLParse();
        private IWebDriver _driver;
        private ISelenium _selenium;
        private string _url;
        public string BrowserType;
        public DriverHelper DriverHelper;
        public LoginHelper LoginHelper;
        protected string LogoutUrl;
        public StringBuilder VerificationErrors;

        [TestInitialize]
        public void SetupTest()
        {
            CreateFolder();
            _oWaXmlData.LoadXML("../../Config/ApplicationSettings.xml");

            _url = _oWaXmlData.getNodeValue("settings/URL/application");
            LogoutUrl = _oWaXmlData.getNodeValue("settings/URL/logout");

            BrowserType = _oWaXmlData.getNodeValue("settings/browserdata/browser");

            if (BrowserType.ToLower().Equals("firefox") || BrowserType.ToLower().Equals("ff"))
            {
                _driver = new FirefoxDriver();
            }
            else if (BrowserType.ToLower().Equals("internet explorer") || BrowserType.ToLower().Equals("ie"))
            {
                _driver = new InternetExplorerDriver();
            }
            else if (BrowserType.ToLower().Equals("chrome"))
            {
                _driver = new ChromeDriver(@"C:\\Webdrivers");
            }
            else
            {
                _driver = new FirefoxDriver();
            }

            _driver.Manage().Window.Maximize();

            _selenium = new WebDriverBackedSelenium(_driver, _url);

            _driver.Navigate().GoToUrl(_url);
        }

        public IWebDriver GetWebDriver()
        {
            return _driver;
        }

        public ISelenium GetSelenium()
        {
            return _selenium;
        }

        [TestCleanup]
        public void TeardownTest()
        {
            _driver.Quit();
        }

        //Taking Screenshot
        public void TakeScreenshot(string saveLocation)
        {
            var location = GetPath() + _name + "\\" + saveLocation + ".png";
            var ssdriver = _driver as ITakesScreenshot;
            var screenshot = ssdriver.GetScreenshot();
            screenshot.SaveAsFile(location, ImageFormat.Png);
        }

        public string GetRandomNumber()
        {
            var number = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            return number;
        }

        // Verify the page title
        public void VerifyTitle(string title)
        {
            var containsTitle = false;

            for (var i = 0; i < 50 && !containsTitle; ++i)
            {
                containsTitle = GetWebDriver().Title.Contains(title);
                Thread.Sleep(100);
            }

            Assert.IsTrue(containsTitle);
        }

        // Refresh the current web page
        public void RefreshPage()
        {
            GetWebDriver().Navigate().Refresh();
        }

        // Login into the application
        public void Login(string userName, string password)
        {
            LoginHelper = new LoginHelper(GetWebDriver());

            LoginHelper.EnterUserName(userName);
            LoginHelper.EnterPassword(password);
            LoginHelper.ClickEnterButton();
        }

        // Logout from the application
        public void Logout()
        {
            LoginHelper = new LoginHelper(GetWebDriver());
            LoginHelper.ClickUserIcon();
            LoginHelper.WaitForWorkAround(2000);
            LoginHelper.ClickLogOff();
        }

        // Get path
        public string GetPath()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var ab = currentDirectory.Split(new[] {"bin"}, StringSplitOptions.None);
            var a = ab[0];
            var fPath = a + "Screenshots\\";
            return fPath;
        }

        // Create file _name
        public void CreateFolder()
        {
            if (_isExecutedFirst) return;

            _name = string.Format("{0:yyMMdd_hhmmss}", DateTime.Now);
            var fname = GetPath() + _name;
            _oldfname = fname;
            Directory.CreateDirectory(_oldfname);
            _isExecutedFirst = true;
        }

        public int RandomNumber(int n1, int n2)
        {
            var rnd = new Random();
            return rnd.Next(n1, n2);
        }
    }
}