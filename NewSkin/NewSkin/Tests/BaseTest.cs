using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.Util;

namespace NewSkin.Tests
{
    /// <summary>
    ///     This is the base unit test class. It automatically declares the browser field
    ///     and closes it at the end of each test.
    /// </summary>
    [TestClass]
    public class BaseTest
    {
        protected Browser Browser;
        protected LocatorReader Common;

        /// <summary>
        ///     Gets or sets the test context which provides
        ///     information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void LoadCommon()
        {
            Common = new LocatorReader("Common.xml");
        }

        /// <summary>
        ///     Close the browser at the end of each test if we are not debugging.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            if (!Debugger.IsAttached) Browser.Quit();
        }

        /// <summary>
        ///     Go to the admin page portal from the office view.
        /// </summary>
        protected void GoToAdmin()
        {
            Browser.ImplicitWait = 10;
            Browser.Sequence
                .MoveToElement(Browser.FindElement((Common.Get("name-dropdown"))))
                .Click(Browser.FindElement(Common.Get("admin-link")))
                .Build().Perform();
        }

        /// <summary>
        ///     Go to the main page portal from the office view.
        /// </summary>
        protected void GoToMain()
        {
            Browser.Sequence
                .MoveToElement(Browser.FindElement((Common.Get("name-dropdown"))))
                .Click(Browser.FindElement(Common.Get("main-link")))
                .Build().Perform();
        }

        protected void Logout()
        {
            Browser.Sequence
                .MoveToElement(Browser.FindElement((Common.Get("name-dropdown"))))
                .Click(Browser.FindElement(Common.Get("logout-link")))
                .Build().Perform();
        }
    }
}