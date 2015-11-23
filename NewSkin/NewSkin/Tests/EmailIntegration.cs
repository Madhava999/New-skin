using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.Util;
using OpenQA.Selenium;

namespace NewSkin.Tests
{
    [TestClass]
    public class EmailIntegration : BaseTest
    {
        private LocatorReader e;
        Random rand = new Random();

        [TestInitialize]
        public void TestInitialize()
        {
            e = new LocatorReader("EmailIntegration.xml");
            Browser = Pegasus.LoginCom("seloffice");
            Thread.Sleep(500);
        }

        private void GoToEmailAccounts()
        {
            Browser.MouseOver(e, "activities-tab")
                .MouseOver(e, "email-tab")
                .Click(e, "email-accounts")
                .Wait(1);
        }

        private void FillWithValid()
        {
            var name = "Email Account " + rand.Next(int.MaxValue);
            Browser.FillForm(e, "account-name", name)
                .FillForm(e, "username", "qaintern@pegasuscrm.net")
                .FillForm(e, "password", "Welcome132")
                .DropdownSelectByText(e, "provider", "Exchange")
                .FillForm(e, "server-address", "secure.emailsrvr.com")
                .DropdownSelectByText(e, "connect-type", "SSL")
                .FillForm(e, "smtp-address", "secure.emailsrvr.com")
                .DropdownSelectByText(e, "connect-type2", "SSL");
        }

        private void FillWithInvalid()
        {
            var name = "Email Account " + rand.Next(int.MaxValue);
            Browser.FillForm(e, "account-name", name)
                .FillForm(e, "username", "test@test.com")
                .FillForm(e, "password", "password")
                .DropdownSelectByText(e, "provider", "Exchange")
                .FillForm(e, "server-address", "secure.emailsrvr.com")
                .DropdownSelectByText(e, "connect-type", "SSL")
                .FillForm(e, "smtp-address", "secure.emailsrvr.com")
                .DropdownSelectByText(e, "connect-type2", "SSL");
        }

        private void CreateValidEmail()
        {
            GoToEmailAccounts();
            if (!Browser.ElementsVisible(e, "valid-email"))
            {
                Browser.Click(e, "create-account")
                    .FillForm(e, "account-name", "Valid Email")
                    .FillForm(e, "username", "qaintern@pegasuscrm.net")
                    .FillForm(e, "password", "Welcome132")
                    .DropdownSelectByText(e, "provider", "Exchange")
                    .FillForm(e, "server-address", "secure.emailsrvr.com")
                    .DropdownSelectByText(e, "connect-type", "SSL")
                    .FillForm(e, "smtp-address", "secure.emailsrvr.com")
                    .DropdownSelectByText(e, "connect-type2", "SSL")
                    .FillForm(e, "signature", "test signature")
                    .Click(Common, "save-button");
                Browser.ImplicitWait = 15;
                Assert.AreEqual("E-Mail account created successfully.",
                    Browser.FindElement(Common, "flash-message").Text);
            }
        }

        [TestMethod]
        public void ListEmailAccounts()
        {
            GoToEmailAccounts();

            Assert.AreEqual("E-Mail Accounts", Browser.Title);
        }

        [TestMethod]
        public void EmailAdd()
        {
            GoToEmailAccounts();
            Browser.Click(e, "create-account");
            FillWithValid();
            Browser.Click(Common, "save-button");

            Assert.AreEqual("E-Mail account created successfully.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void EmailProviderSupport()
        {
            GoToEmailAccounts();
            Browser.Click(e, "create-account")
                .Click(e, "provider");

            Assert.IsTrue(Browser.ElementsVisible(e,
                "exchange", "hotmail", "gmail1", "gmail2",
                "yahoo1", "yahoo2", "other1", "other2"));
        }

        [TestMethod]
        public void EmailValidIncoming()
        {
            GoToEmailAccounts();
            Browser.Click(e, "create-account");
            FillWithValid();
            Browser.Click(e, "incoming-check");
            Browser.ImplicitWait = 20;

            Assert.IsTrue(Browser.ElementsVisible(e, "connect-success"));
        }

        [TestMethod]
        public void EmailInvalidIncoming()
        {
            GoToEmailAccounts();
            Browser.Click(e, "create-account");
            FillWithInvalid();
            Browser.Click(e, "incoming-check");
            Browser.ImplicitWait = 20;

            Assert.IsTrue(Browser.ElementsVisible(e, "connect-fail"));
        }

        [TestMethod]
        public void EmailValidOutgoing()
        {
            GoToEmailAccounts();
            Browser.Click(e, "create-account");
            FillWithValid();
            Browser.Click(e, "outgoing-check");
            Browser.ImplicitWait = 20;

            Assert.IsTrue(Browser.ElementsVisible(e, "send-success"));
        }

        [TestMethod]
        public void EmailInvalidOutgoing()
        {
            GoToEmailAccounts();
            Browser.Click(e, "create-account");
            FillWithInvalid();
            Browser.Click(e, "outgoing-check");
            Browser.ImplicitWait = 20;

            Assert.IsTrue(Browser.ElementsVisible(e, "send-fail"));
        }

        [TestMethod]
        public void SendEmail()
        {
            CreateValidEmail();

            var name = "Test Email " + rand.Next(int.MaxValue);
            Browser.MouseOver(e, "activities-tab")
                .MouseOver(e, "email-tab")
                .Click(e, "compose")
                .DropdownSelectByText(e, "from", "Valid Email (qaintern@pegasuscrm.net)")
                .FillForm(e, "to", "qaintern@pegasuscrm.net")
                .FillForm(e, "subject", name)
                .FillForm(e, "body", "email body")
                .Wait(1)
                .Click(e, "send")
                .Wait(1);
            Assert.AreEqual("E-Mail Sent Successfully!!",
                Browser.FindElement(Common, "flash-message").Text);

            Browser.Click(e, "valid-email")
                .Wait(1)
                .Click(e, "sent-mail")
                .Wait(1);

            Assert.AreEqual("Subject: " + name, Browser.FindElement(e, "sent-subject").Text);
            IWebElement frame = Browser.FindElement(By.XPath("//html//body//iframe"));
            Browser.SwitchToFrame(frame);
            Assert.AreEqual("email body", Browser.FindElement(By.TagName("p")).Text);
        }

        [TestMethod]
        public void ViewEmails()
        {
            Browser.MouseOver(e, "activities-tab")
                .Click(e, "email-tab");

            Assert.IsTrue(Browser.ElementsVisible(e, "inbox"));
        }

        [TestMethod]
        public void SendToClient()
        {
            Browser.Click(Common, "sidebar.clients")
                .Click(e, "client-email")
                .Wait(1);

            Assert.AreEqual("Compose", Browser.Title);
            Assert.AreEqual("qaintern@pegasuscrm.net",
                Browser.FindElement(e, "to").GetAttribute("value"));
        }

        [TestMethod]
        public void EmailSignature()
        {
            SendEmail();

            Assert.AreEqual("test signature",
                Browser.FindElement(e, "sent-signature").Text);
        }
    }
}