using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.Util;

namespace NewSkin.Tests
{
    [TestClass]
    public class DigitalSignatures : BaseTest
    {
        //********** SHARE PDF **********
        private LocatorReader d;

        [TestInitialize]
        public void TestInitialize()
        {
            d = new LocatorReader("DigitalSignatures.xml");
            Browser = Pegasus.LoginCom("seloffice");
            Thread.Sleep(500);
        }

        private void ShareWindow()
        {
            Browser.Click(d, "clients-tab")
                .Wait(1)
                .Click(d, "brian-client")
                .Wait(1)
                .Click(d, "pdfs-tab")
                .Wait(1)
                .Click(d, "share-link");
        }

        private void ClearThenShare()
        {
            Browser.Click(d, "clients-tab")
                .Wait(1)
                .Click(d, "brian-client")
                .Wait(1)
                .Click(d, "file-share")
                .Wait(2);

            while (Browser.ElementsVisible(d, "file-delete"))
            {
                Browser.Click(d, "file-delete")
                    .Wait(3);
            }

            Browser.Click(d, "pdfs-tab")
                .Wait(1)
                .Click(d, "share-link");
        }

        [TestMethod]
        public void SharePDFLink()
        {
            ShareWindow();
            Thread.Sleep(1000);

            Assert.IsTrue(Browser.ElementsVisible(d, "share-window"));
        }

        [TestMethod]
        public void SharePDFSelectUser()
        {
            ShareWindow();
            Browser.Click(d, "owner-box")
                .Click(d, "share-button")
                .Wait(1);

            Assert.AreEqual("Please select atleast one user", Browser.AlertText);
        }

        [TestMethod]
        public void SharePDFNotify()
        {
            ShareWindow();
            Browser.Click(d, "notify")
                .Click(d, "share-button")
                .Wait(1);

            Assert.AreEqual("Are you sure you don't want to notify users?", Browser.AlertText);
        }

        [TestMethod]
        public void SharePDFCancelledStatus()
        {
            ClearThenShare();
            Browser.Click(d, "notify")
                .Click(d, "share-button")
                .Wait(1)
                .AlertAccept()
                .Wait(5)
                .Click(d, "share-link")
                .Click(d, "notify")
                .Click(d, "share-button")
                .Wait(1)
                .AlertAccept()
                .Wait(5);

            Browser.Click(d, "file-share")
                .Wait(2);

            Assert.IsTrue(Browser.ElementsVisible(d, "file-cancelled"));
        }

        [TestMethod]
        public void SharePDF()
        {
            ShareWindow();
            Browser.Click(d, "notify")
                .Click(d, "share-button")
                .Wait(1)
                .AlertAccept()
                .Wait(5);

            Assert.AreEqual("Succesfully Generated Document and email sent.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void SharePDFCancel()
        {
            ShareWindow();
            Browser.Click(d, "cancel-share")
                .Wait(1);

            Assert.IsFalse(Browser.ElementsVisible(d, "share-window"));
        }
    }

    [TestClass]
    public class DigitalSignatures2 : BaseTest
    {
        //********** SIGN PDF **********
        private LocatorReader d;

        //clears all existing pdfs and sends a new one
        [ClassInitialize]
        public static void setup(TestContext testContext)
        {
            Browser b = Pegasus.LoginCom("seloffice");
            LocatorReader d = new LocatorReader("DigitalSignatures.xml");
            Thread.Sleep(500);

            b.Click(d, "clients-tab")
                .Wait(1)
                .Click(d, "Test1")
                .Wait(1)
                .Click(d, "file-share")
                .Wait(2);

            while (b.ElementsVisible(d, "file-delete"))
            {
                b.Click(d, "file-delete")
                    .Wait(3);
            }

            b.Click(d, "pdfs-tab")
                .Wait(1)
                .Click(d, "share-link")
                .Click(d, "sig-require")
                .Click(d, "notify")
                .Click(d, "share-button")
                .Wait(1)
                .AlertAccept()
                .Wait(5);

            b.Quit();
        }

        //clears all shared files after all tests
        [ClassCleanup]
        public static void cleanup()
        {
            Browser b = Pegasus.LoginCom("seloffice");
            LocatorReader d = new LocatorReader("DigitalSignatures.xml");
            Thread.Sleep(500);

            b.Click(d, "clients-tab")
                .Wait(1)
                .Click(d, "brian-client")
                .Wait(1)
                .Click(d, "file-share")
                .Wait(2)
                .Click(d, "file-delete")
                .Wait(1)
                .Quit();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            d = new LocatorReader("DigitalSignatures.xml");
            Browser = Pegasus.LoginCom("brianclient");
            Thread.Sleep(500);
        }

        private void SignWindow()
        {
            Browser
                .Click(d, "file-share-tab")
                .Wait(1)
                .Click(d, "sig-require-button")
                .Wait(1);
        }

        [TestMethod]
        public void SignLinkPresent()
        {
            Browser.Click(d, "file-share-tab")
                .Wait(2);

            Assert.IsTrue(Browser.ElementsVisible(d, "sig-require-button"));
        }

        [TestMethod]
        public void SignLink()
        {
            SignWindow();

            Assert.IsTrue(Browser.ElementsVisible(d, "sig-window"));
        }

        [TestMethod]
        public void CorrectDocumentName()
        {
            Browser.Click(d, "file-share-tab")
                .Wait(2);

            var docName = Browser.FindElement(d, "file-name").Text;

            Browser.Click(d, "sig-require-button");

            Assert.AreEqual("Click here to view the " + docName,
                Browser.FindElement(d, "view-doc").Text);
        }

        [TestMethod]
        public void AcceptTerms()
        {
            SignWindow();
            Browser.Click(d, "sign-button")
                .Wait(1);

            Assert.AreEqual("Please accept the terms and conditions "
                + "before signing Merchant Agreement.",
                Browser.AlertText);
        }

        [TestMethod]
        public void BlankSignature()
        {
            SignWindow();
            Browser.Click(d, "accept-terms")
                .Click(d, "sign-button")
                .Wait(1);

            Assert.AreEqual("Please Draw Signature", Browser.AlertText);
        }

        [TestMethod]
        public void ClickToBeginDraw()
        {
            SignWindow();
            Browser.Click(d, "accept-terms")
                .Click(d, "click-sign");

            Assert.IsFalse(Browser.ElementsVisible(d, "click-sign"));
        }

        [TestMethod]
        public void ClearSignature()
        {
            ClickToBeginDraw();
            Browser.Click(d, "sign-canvas")
                .Click(d, "clear-button");

            Assert.AreEqual("", Browser.FindElement(d, "sig").GetAttribute("value"));
        }

        [TestMethod]
        public void ClearSignatureSign()
        {
            ClickToBeginDraw();
            Browser.Click(d, "sign-canvas")
                .Click(d, "clear-button")
                .Click(d, "sign-button")
                .Wait(1);

            Assert.AreEqual("Please Draw Signature", Browser.AlertText);
        }

        [TestMethod]
        public void SignConfirm()
        {
            ClickToBeginDraw();
            Browser.Click(d, "sign-canvas")
                .Click(d, "sign-button")
                .Wait(1);

            Assert.AreEqual("Please confirm to proceed.", Browser.AlertText);
        }

        [TestMethod]
        public void CancelSign()
        {
            SignConfirm();
            Browser.AlertDismiss()
                .Click(d, "x")
                .Wait(1);

            Assert.IsFalse(Browser.ElementsVisible(d, "sig-window"));
            Assert.IsTrue(Browser.ElementsVisible(d, "sig-require-button"));
        }

        [TestMethod]
        public void SignPDF()
        {
            SignConfirm();
            Browser.AlertAccept();

            Assert.AreEqual("Successfully Completed Signed Process.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void SignedOnDateTime()
        {
            DateTime now = DateTime.Now;
            string date = now.ToString("MM-dd-yyyy");
            string time = now.ToString("hh:mmtt");

            Browser.Click(d, "file-share-tab")
                .Wait(3);

            Assert.AreEqual(date + " " + time,
                Browser.FindElement(d, "file-sign-time").Text);
        }

        [TestMethod]
        public void CompletedStatus()
        {
            Browser.Click(d, "file-share-tab")
                .Wait(3);

            Assert.AreEqual("Sign Completed",
                Browser.FindElement(d, "file-status").Text);
        }

        [TestMethod]
        public void CompletedStatusOffice()
        {
            SignPDF();
            Browser.Quit();
            Browser = Pegasus.LoginCom("seloffice");
            Thread.Sleep(500);

            Browser.Click(d, "clients-tab")
                .Wait(1)
                .Click(d, "brian-client")
                .Wait(1)
                .Click(d, "file-share")
                .Wait(5);

            Assert.AreEqual("Completed", Browser.FindElement(d, "file-status2").Text);
        }
    }
}