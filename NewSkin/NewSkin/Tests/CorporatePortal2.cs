using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.Util;

namespace NewSkin.Tests
{
    [TestClass]
    public class CorporatePortal2 : BaseTest
    {
        private LocatorReader _c;
        private Browser _client;

        [TestInitialize]
        public void TestInitalize()
        {
            _c = new LocatorReader("CorporatePortal2.xml");
            Browser = Pegasus.LoginCom("selcorp");
        }

        [TestCleanup]
        public void QuitClient()
        {
            if (!Debugger.IsAttached) _client?.Quit();
        }

        private void GoToRatesFees()
        {
            Browser.ImplicitWait = 10;
            Browser.MouseOver(_c, "sidebar.master-data.self")
                .Click(_c, "sidebar.master-data.rates-fees");
            Thread.Sleep(1000);
        }

        [TestMethod]
        public void TestRatesFees()
        {
            Browser.ImplicitWait = 10;
            var name = "Rates & Fees " + new Random().Next(int.MaxValue);
            GoToRatesFees();

            // Verify that rates & fees validate required input.
            Browser.Click(Common, "create-button")
                .Click(Common, "save-button");
            Assert.IsTrue(Browser.ElementsVisible(Common, "required-field-label"));

            // Create a new rates & fees.
            Browser.FillForm(_c, "rates-fees.name", name)
                .DropdownSelectByIndex(_c, "rates-fees.ptype", 1)
                .DropdownSelectByText(_c, "rates-fees.mtype", "Agent")
                .DropdownSelectByText(_c, "rates-fees.method", "Manually Swiped");
            Assert.IsFalse(Browser.ElementsVisible(Common, "required-field-label"));
            Browser.Click(Common, "save-button");

            Assert.AreEqual("The Rates is successfully created!!",
                Browser.FindElement(Common, "flash-message").Text);

            // Delete the rate.
            Browser.Click(_c.Get("rates-fees.delete-button", name));
            Thread.Sleep(1000);
            Browser.AlertAccept();
            Thread.Sleep(1000);
            Assert.AreEqual("The Rates is successfully deleted!!",
                Browser.FindElement(Common, "flash-message").Text);

            // Push to Offices.
            Browser.Click(_c, "push-offices-button");
            Thread.Sleep(1000);
            Browser.AlertAccept();
            Thread.Sleep(1000);
            Assert.AreEqual("Rates & Fees successfully pushed to offices.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void TestEditEmailTemplates()
        {
            Browser.ImplicitWait = 10;
            Browser.MouseOver(_c, "sidebar.system.self")
                .Click(_c, "sidebar.system.email");
            Thread.Sleep(1000);
            Browser.Click(_c, "email.edit-template")
                .Click(Common, "save-button");
            Assert.AreEqual("The email management has been saved",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void TestAuditTrails()
        {
            Browser.ImplicitWait = 10;
            Browser.MouseOver(_c, "sidebar.system.self")
                .Click(_c, "sidebar.system.audit");
            Thread.Sleep(2000);
            Browser.Click(_c, "audit.name")
                .Click(Common, "save-button");
            Assert.AreEqual("Options Saved.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void TestEditProfile()
        {
            Browser.ImplicitWait = 5;
            Browser.MouseOver(_c, "user-dropdown")
                .Click(_c, "profile-link")
                .Click(_c, "edit-profile")
                .Click(Common, "save-button");
            Assert.AreEqual("Your profile has been successfully updated",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void TestSettings()
        {
            Browser.ImplicitWait = 5;
            Browser.MouseOver(_c, "sidebar.system.self")
                .Click(_c, "sidebar.system.settings")
                .Click(Common, "save-button");
            Assert.AreEqual("Settings updated successfully",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void TestTabs()
        {
            Browser.ImplicitWait = 10;
            var name = "Tab " + new Random().Next(int.MaxValue);

            // Create a tab.
            Browser.MouseOver(_c, "sidebar.fdict.self")
                .Click(_c, "sidebar.fdict.tabs")
                .Click(Common, "create-button")
                .FillForm(_c, "tab-name", name)
                .Click(Common, "save-button");

            Assert.AreEqual("Tab Created Successfully",
                Browser.FindElement(Common, "flash-message").Text);

            // Open new browser in client page and see if tab is there.
            _client = Pegasus.LoginCom("seloffice");
            _client.Click(Common, "sidebar.clients")
                .Click(_c, "first-client");

            Assert.IsTrue(_client.ElementsVisible(_c.Get("tab-name", name)));

            // Edit a tab.
            var newName = name + " Edited";
            Browser.Click(_c.Get("edit-tab", name))
                .FillFormReplace(_c, "edit-tab-name", newName);
            Browser.FindElements(Common, "save-button")[1].Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Tab Updated Successfully",
                Browser.FindElement(Common, "flash-message").Text);

            // Refresh client and see if change is made.
            _client.Refresh();
            Assert.IsTrue(_client.ElementsVisible(_c.Get("tab-name", newName)));

            // Delete tab.
            Browser.Click(_c.Get("delete-tab", newName))
                .AlertAccept();
            Thread.Sleep(1000);
            Assert.AreEqual("Tab Deleted Sucessfully.",
                Browser.FindElement(Common, "flash-message").Text);

            // Refresh client and verify tab is gone.
            _client.Refresh();
            Assert.IsFalse(_client.ElementsVisible(_c.Get("tab-name", newName)));
        }

        [TestMethod]
        public void TestSections()
        {
            Browser.MouseOver(_c, "sidebar.fdict.self")
                .Click(_c, "sidebar.fdict.sections");

            // Create a new section.
            var name = "Section " + new Random().Next(int.MaxValue);
            Browser.Click(Common, "create-button")
                .FillForm(_c, "section-name", name);
            Thread.Sleep(1000);
            Browser.Click(Common, "save-button");
            Thread.Sleep(2000);
            Assert.AreEqual("Section Created Successfully", Browser.AlertText);
            Browser.AlertAccept();

            // Check if section appears for client.
            _client = Pegasus.LoginCom("seloffice");
            _client.Click(Common, "sidebar.clients")
                .Click(_c, "first-client");
            Assert.IsTrue(_client.TextExists(name));

            // Edit a section.
            var newName = name + " Edited";
            Browser.Click(_c.Get("edit-section", name))
                .FillFormReplace(_c, "edit-section-name", newName);
            Browser.FindElements(Common, "save-button")[1].Click();
            Thread.Sleep(2000);
            Browser.AlertAccept();
            Assert.IsTrue(Browser.TextExists(newName));

            // Test if change is reflected on client.
            _client.Refresh();
            Assert.IsTrue(_client.TextExists(newName));

            // Delete a section.
            Browser.Click(_c.Get("delete-section", newName))
                .AlertAccept();
            Thread.Sleep(1000);
            Assert.AreEqual("Section Deleted Sucessfully.",
                Browser.FindElement(Common, "flash-message").Text);

            _client.Refresh();
            Assert.IsFalse(_client.TextExists(newName));
        }

        [TestMethod]
        public void TestFields()
        {
            Browser.MouseOver(_c, "sidebar.fdict.self")
                .MouseOver(_c, "sidebar.fdict.fields.self")
                .Click(_c, "sidebar.fdict.fields.props")
                .DropdownSelectByText(_c, "field-module", "Clients")
                .Click(_c, "field-search")
                .Click(_c, "first-field")
                .Wait(2)
                .Click(Common, "save-button")
                .Wait(1);


            // There's a 500 internal error, and I'm not sure what
            // should happen if it works, so I can't verify that it works.
            // Just going to check that "Error" isn't on the page.
            Assert.IsFalse(Browser.TextExists("Error"));
        }

        [TestMethod]
        public void TestFieldGroups()
        {
            Browser.ImplicitWait = 5;
            Browser.MouseOver(_c, "sidebar.fdict.self")
                .Click(_c, "sidebar.fdict.field-group");

            var name = "Field Group " + new Random().Next(int.MaxValue);
            Browser.Click(Common, "create-button")
                .Wait(2)
                .FillForm(_c, "fgroup-name", name)
                .DropdownSelectByText(_c, "fgroup-module", "Clients")
                .DropdownSelectByIndex(_c, "fgroup-proc", 1)
                .Wait(2)
                .DropdownSelectByIndex(_c, "fgroup-tab", 1)
                .Wait(2)
                .DropdownSelectByIndex(_c, "fgroup-field", 1)
                .Click(_c, "fgroup-add-field")
                .Click(Common, "save-button")
                .Wait(2);

            Assert.AreEqual("Field Grouping Template Saved.",
                Browser.FindElement(Common, "flash-message").Text);
        }
    }
}