using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.Util;
using OpenQA.Selenium;

namespace NewSkin.Tests
{
    [TestClass]
    public class iFrame : BaseTest
    {
        private LocatorReader i;
        Random rand = new Random();

        //creates 2 iFrames to be used throughout tests
        [ClassInitialize]
        public static void setup(TestContext testContext)
        {
            Browser b = Pegasus.LoginCom("seloffice");
            LocatorReader c = new LocatorReader("Common.xml");
            LocatorReader i = new LocatorReader("iFrame.xml");
            Thread.Sleep(500);

            b.Sequence
                .MoveToElement(b.FindElement((c.Get("name-dropdown"))))
                .Click(b.FindElement(c.Get("admin-link")))
                .Build().Perform();
            Thread.Sleep(2000);

            b.MouseOver(i, "integration-tab")
                .Click(i, "iframe-link")
                .Wait(1)
                .Click(i, "create-button")
                .FillForm(i, "iframe-name", "iFrame 1")
                .FillForm(i, "username", "Email")
                .FillForm(i, "password", "Password")
                .FillForm(i, "login", "https://www.dropbox.com/login")
                .FillForm(i, "forgot", "https://www.dropbox.com/forgot?email_from_login=")
                .FillForm(i, "create", "https://www.dropbox.com/")
                .Click(i, "appear")
                .Click(i, "save-button");
            Thread.Sleep(2000);

            b.Click(i, "create-button")
                .FillForm(i, "iframe-name", "iFrame 2")
                .FillForm(i, "username", "Email")
                .FillForm(i, "password", "Password")
                .FillForm(i, "login", "https://www.dropbox.com/login")
                .FillForm(i, "forgot", "https://www.dropbox.com/forgot?email_from_login=")
                .FillForm(i, "create", "https://www.dropbox.com/")
                .Click(i, "appear")
                .Click(i, "save-button");
            Thread.Sleep(2000);

            b.Quit();
        }

        //deletes the 2 iFrames created
        [ClassCleanup]
        public static void cleanup()
        {
            Browser b = Pegasus.LoginCom("seloffice");
            LocatorReader c = new LocatorReader("Common.xml");
            LocatorReader i = new LocatorReader("iFrame.xml");
            Thread.Sleep(500);

            b.Sequence
                .MoveToElement(b.FindElement((c.Get("name-dropdown"))))
                .Click(b.FindElement(c.Get("admin-link")))
                .Build().Perform();
            Thread.Sleep(2000);

            b.MouseOver(i, "integration-tab")
                .Click(i, "iframe-link")
                .Wait(1)
                .Click(i, "delete")
                .Wait(1)
                .AlertAccept()
                .Wait(1);
            Thread.Sleep(2000);

            b.Click(i, "delete")
                .Wait(1)
                .AlertAccept()
                .Wait(1);
            Thread.Sleep(2000);

            b.Quit();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            i = new LocatorReader("iFrame.xml");
            Browser = Pegasus.LoginCom("seloffice");
            Thread.Sleep(500);
        }

        [TestMethod]
        public void iFramePage()
        {
            GoToAdmin();
            Thread.Sleep(2000);
            Browser.MouseOver(i, "integration-tab")
                .Click(i, "iframe-link")
                .Wait(1);

            Assert.AreEqual("Iframe Apps", Browser.Title);
        }

        [TestMethod]
        public void iFrameCreateButton()
        {
            iFramePage();
            Browser.Click(i, "create-button")
                .Wait(1);

            Assert.AreEqual("Create Iframe", Browser.Title);
        }

        [TestMethod]
        public void iFrameEditButton()
        {
            iFramePage();
            Browser.Click(i, "edit-button")
                .Wait(1);

            Assert.AreEqual("Edit Iframe", Browser.Title);
        }

        [TestMethod]
        public void iFrameEditSave()
        {
            iFrameEditButton();
            Browser.Click(i, "save-button");

            Assert.AreEqual("Iframe Updated Successfully.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void iFrameEditCancel()
        {
            var name = "iFrame " + rand.Next(int.MaxValue);

            iFrameEditButton();
            Browser.ClearForm(i, "iframe-name")
                .FillForm(i, "iframe-name", name)
                .Click(i, "cancel-button")
                .Wait(1);

            Assert.AreEqual("Iframe Apps", Browser.Title);
            Assert.AreNotEqual(name, Browser.FindElement(i, "first-entry-name").Text);
        }

        [TestMethod]
        public void iFrameConfirmDelete()
        {
            iFramePage();
            Browser.Click(i, "delete-button")
                .Wait(1);

            Assert.AreEqual("Are you sure you want to delete this iframe permanently?",
                Browser.AlertText);
        }

        [TestMethod]
        public void iFrameSearch()
        {
            iFramePage();
            Thread.Sleep(1000);
            Browser.FillForm(i, "search-name", "iFrame 1")
                .Wait(2);

            Assert.IsTrue(Browser.ElementCount(i, "iframe2") == 0);
        }

        [TestMethod]
        public void iFrameRefresh()
        {
            iFramePage();
            Thread.Sleep(1000);
            Browser.FillForm(i, "search-name", "iFrame 2")
                .Wait(2)
                .Click(i, "refresh-button")
                .Wait(2);

            Assert.IsTrue(Browser.ElementCount(i, "entries") > 1);
        }

        [TestMethod]
        public void iFrameLoginPage()
        {
            Browser.Click(i, "iframe1-tab");

            Assert.IsTrue(Browser.ElementsVisible(i, "iframe-login-page"));
        }

        [TestMethod]
        public void iFrameForgot()
        {
            Browser.Click(i, "iframe1-tab");

            IWebElement frame = Browser.FindElement(By.XPath("//html//body//iframe"));
            Browser.SwitchToFrame(frame);

            Browser.Click(i, "iframe-forgot");

            Assert.IsTrue(Browser.TextExists("Forgot your password?"));
        }

        [TestMethod]
        public void iFrameCreateAccount()
        {
            Browser.Click(i, "iframe1-tab");

            IWebElement frame = Browser.FindElement(By.XPath("//html//body//iframe"));
            Browser.SwitchToFrame(frame);

            Browser.Click(i, "iframe-create-account");

            Assert.IsTrue(Browser.TextExists("Sign up"));
        }

        [TestMethod]
        public void iFrameValidDomain()
        {
            iFrameCreateButton();
            Browser.FillForm(i, "login", "http://example.cmo")
                .Click(i, "save-button");

            Assert.IsTrue(Browser.ElementsVisible(i, "valid-domain"));
        }
    }
}