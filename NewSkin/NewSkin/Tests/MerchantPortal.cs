using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.Util;

namespace NewSkin.Tests
{
    [TestClass]
    public class MerchantPortal : BaseTest
    {
        private LocatorReader _m;

        [TestInitialize]
        public void TestInitialize()
        {
            _m = new LocatorReader("MerchantPortal.xml");
        }

        private void InitializeOffice()
        {
            Browser = Pegasus.Login("seloffice");
            Browser.ImplicitWait = 10;
        }

        private void InitializeMerchant()
        {
            Browser = Pegasus.Login("merchant");
            Browser.ImplicitWait = 10;
        }

        private void GoToClientsList()
        {
            InitializeOffice();
            Browser.ImplicitWait = 10;
            Browser.Click(Common, "sidebar.clients");
        }

        /// <summary>
        ///     This will go to a random client's iPage.
        /// </summary>
        private void OpenRandomClientsPage()
        {
            GoToClientsList();
            Browser.ImplicitWait = 10;
            var links = Browser.FindElements(_m, "clients.client-link");
            links[new Random().Next(links.Count)].Click();
        }

        private void CreateClient()
        {
            var merger = new LocatorReader("Merger.xml");
            var randomName = "Client " + new Random().Next(int.MaxValue);

            Browser.MouseOver(merger.Get("ClientsTab"))
                .Click(merger.Get("CreateClient"))
                .DropdownSelectByText(merger.Get("Status"), "New")
                .DropdownSelectByText(merger.Get("Responsibility"), "Test Test")
                .Click(merger.Get("CompanyDetailsTab"))
                .FillForm(merger.Get("CompanyDBAName"), randomName)
                .Click(merger.Get("Save"));
            Thread.Sleep(2000);
            Assert.AreEqual("Client saved successfully.",
                Browser.FindElement(Common.Get("flash-message")).Text);

            TestContext.Properties["client "] = randomName;
        }

        /// <summary>
        ///     Add an owner contact.
        ///     Prerequsite: be in the clients page.
        /// </summary>
        private void AddOwnerContacts()
        {
            Browser.ImplicitWait = 5;
            Browser.Click(_m, "clients.contacts.tab")
                .FillFormReplace(_m, "clients.contacts.owner-first-name", "Larry")
                .FillFormReplace(_m, "clients.contacts.owner-last-name", "Page")
                .FillFormReplace(_m, "clients.contacts.owner-email", "larrypage@google.com")
                .Click(Common, "save-button");
        }

        [TestMethod]
        public void TestClientUserPrompt()
        {
            OpenRandomClientsPage();

            Browser.ImplicitWait = 10;
            var clientUserExists = Browser.FindElements(_m, "clients.no-client-user-message").Count == 0;
            Browser.Click(_m, "clients.pdfs-tab")
                .Click(_m, "clients.pdfs.share-link");

            Thread.Sleep(2000);

            Assert.IsTrue(clientUserExists ^ Browser.FindElements(_m, "clients.pdfs.create-user-title").Count > 0);
        }

        /// <summary>
        ///     Create a merchant portal user login.
        /// </summary>
        private void CreateClientUser()
        {
            OpenRandomClientsPage();
            AddOwnerContacts();

            Browser.Click(_m, "clients.info-tab");

            Thread.Sleep(5000);
            // TODO: This part is broken for some reason; fix it later.
            Browser.Click(_m, "clients.manage-create-button");

            Thread.Sleep(500);
            Browser.Click(_m, "clients.add-user-button")
                .FillFormReplace(_m, "clients.password-field", "password")
                .Click(_m, "clients.notify-user-checkbox")
                .Click(_m, "clients.create-user-button")
                .AlertAccept();
        }

        [TestMethod]
        public void TestFileShare()
        {
            InitializeMerchant();
            Browser.Click(_m, "merchant.sidebar.fileshare");
            Assert.AreEqual("Files Share", Browser.Title);
        }

        [TestMethod]
        public void TestSignature()
        {
            TestFileShare();
            Browser.Click(_m, "merchant.signature-button");
            Browser.ImplicitWait = 5;
            Assert.IsTrue(Browser.FindElement(_m, "merchant.signature-box").Displayed);
        }

        [TestMethod]
        public void TestViewDocument()
        {
            TestFileShare();
            Assert.IsTrue(Browser.ElementsVisible(_m, "merchant.view-document-button"));
        }

        [TestMethod]
        public void TestUploadDocuments()
        {
            TestFileShare();
            Browser.ImplicitWait = 5;
            Browser.Click(_m, "merchant.documents-required-button")
                .Click(_m, "merchant.upload-button");

            Assert.AreEqual("Documents uploaded successfully", Browser.AlertText);
        }

        [TestMethod]
        public void TestHomeFileAndTickets()
        {
            InitializeMerchant();
            Assert.IsTrue(Browser.ElementsVisible(_m, "merchant.file-share-header", "merchant.tickets-header"));
        }

        [TestMethod]
        public void TestTicketsLink()
        {
            InitializeMerchant();
            Browser.MouseOver(_m, "merchant.sidebar.tickets.self")
                .Click(_m, "merchant.sidebar.tickets.tickets");
            Assert.AreEqual("Tickets", Browser.Title);
        }

        [TestMethod]
        public void TestCreateTicket()
        {
            var name = "Ticket " + new Random().Next(int.MaxValue);
            CreateTicket(name);

            Assert.AreEqual("Ticket Created Successfully",
                Browser.FindElement(Common, "flash-message").Text);
        }

        private void CreateTicket(string name)
        {
            TestTicketsLink();
            Browser.Click(_m, "merchant.create-ticket-button");

            Browser.FillForm(_m, "merchant.ticket-subject", name)
                .Click(Common, "save-button");
        }

        [TestMethod]
        public void TestCreateTicketDraft()
        {
            var name = "Ticket " + new Random().Next(int.MaxValue);
            CreateTicketDraft(name);
            Assert.AreEqual("Ticket Saved as Draft Successfully",
                Browser.FindElement(Common, "flash-message").Text);
        }

        private void CreateTicketDraft(string name)
        {
            TestTicketsLink();
            Browser.Click(_m, "merchant.create-ticket-button");

            Browser.FillForm(_m, "merchant.ticket-subject", name)
                .Click(_m, "merchant.draft-button");
        }

        [TestMethod]
        public void TestDeleteTicket()
        {
            var name = "Ticket " + new Random().Next(int.MaxValue);
            CreateTicket(name);
            Browser.ImplicitWait = 5;
            Browser.Click(_m.Get("merchant.ticket-checkbox", name))
                .Click(_m, "merchant.delete-ticket-button")
                .AlertAccept();
            Thread.Sleep(2000);
            Assert.AreEqual("1 Deleted Successfully",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void TestHomeLink()
        {
            InitializeMerchant();
            Browser.Click(_m, "merchant.sidebar.home");
            Assert.AreEqual("Dashboard", Browser.Title);
        }

        [TestMethod]
        public void TestResumeDraft()
        {
            var name = "Ticket " + new Random().Next(int.MaxValue);
            CreateTicketDraft(name);

            Browser.ImplicitWait = 5;
            Browser.MouseOver(_m, "merchant.sidebar.tickets.self")
                .Click(_m, "merchant.sidebar.tickets.draft")
                .Click(_m.Get("merchant.ticket-name", name))
                .Click(Common, "save-button");

            Thread.Sleep(2500);
            Assert.AreEqual("Ticket Edited Successfully",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void TestViewMerchantInformation()
        {
            InitializeMerchant();
            Assert.IsTrue(Browser.ElementsVisible(_m, "merchant.info-header"));
        }
    }
}