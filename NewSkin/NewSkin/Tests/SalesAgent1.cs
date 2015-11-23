using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.Util;

namespace NewSkin.Tests
{
    [TestClass]
    public class SalesAgent1 : BaseTest
    {
        private LocatorReader s;
        Random rand = new Random();

        [TestInitialize]
        public void TestInitialize()
        {
            s = new LocatorReader("SalesAgent1.xml");
            // Browser = Pegasus.LoginCom("brianagent");
            Browser = Pegasus.LoginMyPeg("brianagent");
            Thread.Sleep(500);
        }

        //********** OPPORTUNITIES **********
        [TestMethod]
        public void ViewOpportunities()
        {
            Browser.Click(s, "opp.tab")
                .Wait(1);

            Assert.AreEqual("Opportunities", Browser.Title);
            Assert.IsTrue(Browser.ElementsVisible(s, "opp.table"));
        }

        [TestMethod]
        public void CreateOpportunity1()
        {
            var name1 = "Opportunity " + rand.Next(int.MaxValue);
            var name2 = "Company " + rand.Next(int.MaxValue);

            Browser.Click(s, "opp.tab")
                .Click(s, "opp.create.from-button")
                .FillForm(s, "opp.create.name", name1)
                .FillForm(s, "opp.create.comp", name2)
                .DropdownSelectByText(s, "opp.create.status", "New")
                .DropdownSelectByText(s, "opp.create.respons", "Brian Sales Agent")
                .Click(Common, "save-button")
                .Wait(2)
                .Click(s, "opp.tab")
                .Wait(2);

            Assert.IsTrue(Browser.TextExists(name1));
        }

        [TestMethod]
        public void CreateOpportunity2()
        {
            var name1 = "Opportunity " + rand.Next(int.MaxValue);
            var name2 = "Company " + rand.Next(int.MaxValue);

            Browser.MouseOver(s, "opp.tab")
                .Click(s, "opp.create.from-tab")
                .FillForm(s, "opp.create.name", name1)
                .FillForm(s, "opp.create.comp", name2)
                .DropdownSelectByText(s, "opp.create.status", "New")
                .DropdownSelectByText(s, "opp.create.respons", "Brian Sales Agent")
                .Click(Common, "save-button")
                .Wait(2)
                .Click(s, "opp.tab")
                .Wait(2);

            Assert.IsTrue(Browser.TextExists(name1));
        }

        [TestMethod]
        public void CreateOpportunityRequired()
        {
            Browser.Click(s, "opp.tab")
                .Click(s, "opp.create.from-button")
                .Wait(1)
                .Click(Common, "save-button");

            Assert.IsTrue(Browser.ElementCount(s, "required-message") == 4);
            Assert.AreEqual("This field is required.",
                Browser.FindElement(s, "required-message").Text);
        }

        [TestMethod]
        public void CreateOpportunityCancel()
        {
            Browser.Click(s, "opp.tab")
                .Wait(1)
                .Click(s, "opp.create.from-button")
                .Wait(1)
                .FillForm(s, "opp.create.name", "Cancel Opportunity")
                .Wait(1)
                .FillForm(s, "opp.create.comp", "Cancel Opportunity")
                .Wait(1)
                .Click(s, "cancel-button")
                .Wait(1);

            Assert.IsFalse(Browser.TextExists("Cancel Opportunity"));
        }

        [TestMethod]
        public void OpportunityImport()
        {
            Browser.Click(s, "opp.tab")
                .Click(s, "opp.import.import-button")
                .UploadFile(s, "opp.import.upload", "opportunitysamples.csv")
                .Click(s, "opp.import.import");

            Assert.AreEqual("4 Records Imported Successfully.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void OpportunityMerge()
        {
            Browser.Click(s, "opp.tab")
                .Wait(1)
                .Click(s, "opp.box1")
                .Click(s, "opp.box2")
                .Click(s, "opp.merge.button")
                .Wait(2)
                .Click(s, "opp.merge.primary")
                .Click(s, "opp.merge.merge")
                .Wait(1)
                .AlertAccept()
                .Wait(1);

            Assert.AreEqual("Merging Opportunity(s) Completed Successfully.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        //********** LEADS **********
        [TestMethod]
        public void ViewLeads()
        {
            Browser.Click(s, "lead.tab")
                .Wait(1);

            Assert.AreEqual("Leads", Browser.Title);
            Assert.IsTrue(Browser.ElementsVisible(s, "lead.table"));
        }

        [TestMethod]
        public void CreateLead1()
        {
            var name1 = "First" + rand.Next(int.MaxValue);
            var name2 = "Last" + rand.Next(int.MaxValue);
            var name3 = "Company " + rand.Next(int.MaxValue);

            Browser.Click(s, "lead.tab")
                .Click(s, "lead.create.from-button")
                .DropdownSelectByText(s, "lead.create.status", "New")
                .DropdownSelectByText(s, "lead.create.respons", "Brian Sales Agent")
                .Click(s, "lead.create.comp-detail")
                .FillForm(s, "lead.create.first", name1)
                .FillForm(s, "lead.create.last", name2)
                .FillForm(s, "lead.create.comp", name3)
                .Click(Common, "save-button")
                .Wait(2)
                .Click(s, "lead.tab")
                .Wait(2);

            Assert.IsTrue(Browser.TextExists(name3));
        }

        [TestMethod]
        public void CreateLead2()
        {
            var name1 = "First" + rand.Next(int.MaxValue);
            var name2 = "Last" + rand.Next(int.MaxValue);
            var name3 = "Company " + rand.Next(int.MaxValue);

            Browser.MouseOver(s, "lead.tab")
                .Click(s, "lead.create.from-tab")
                .DropdownSelectByText(s, "lead.create.status", "New")
                .DropdownSelectByText(s, "lead.create.respons", "Brian Sales Agent")
                .Click(s, "lead.create.comp-detail")
                .FillForm(s, "lead.create.first", name1)
                .FillForm(s, "lead.create.last", name2)
                .FillForm(s, "lead.create.comp", name3)
                .Click(Common, "save-button")
                .Wait(2)
                .Click(s, "lead.tab")
                .Wait(2);

            Assert.IsTrue(Browser.TextExists(name3));
        }

        [TestMethod]
        public void CreateLeadRequired()
        {
            Browser.Click(s, "lead.tab")
                .Wait(2)
                .Click(s, "lead.create.from-button")
                .Wait(1)
                .Click(Common, "save-button");

            Assert.IsTrue(Browser.ElementCount(s, "required-message") == 5);
            Assert.AreEqual("This field is required.",
                Browser.FindElement(s, "required-message").Text);
        }

        [TestMethod]
        public void CreateLeadCancel()
        {
            Browser.Click(s, "lead.tab")
                .Click(s, "lead.create.from-button")
                .DropdownSelectByText(s, "lead.create.status", "New")
                .DropdownSelectByText(s, "lead.create.respons", "Brian Sales Agent")
                .Click(s, "lead.create.comp-detail")
                .FillForm(s, "lead.create.first", "Cancel Lead")
                .FillForm(s, "lead.create.last", "Cancel Lead")
                .FillForm(s, "lead.create.comp", "Cancel Lead")
                .Click(s, "cancel-button");

            Assert.IsFalse(Browser.TextExists("Cancel Lead"));
        }

        [TestMethod]
        public void LeadImport()
        {
            Browser.Click(s, "lead.tab")
                .Wait(1)
                .Click(s, "lead.import.button")
                .Wait(1)
                .UploadFile(s, "lead.import.file", "leadsamples.csv")
                .Click(s, "lead.import.import");

            Assert.AreEqual("3 Records Imported Successfully.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        [TestMethod]
        public void LeadMerge()
        {
            Browser.Click(s, "lead.tab")
                .Click(s, "lead.box1")
                .Click(s, "lead.box2")
                .Click(s, "lead.merge.button")
                .Wait(2)
                .Click(s, "lead.merge.primary")
                .Click(s, "lead.merge.merge")
                .Wait(1)
                .AlertAccept()
                .Wait(1);

            Assert.AreEqual("Merging Lead(s) Completed Successfully.",
                Browser.FindElement(Common, "flash-message").Text);
        }

        //********** CLIENTS **********
        [TestMethod]
        public void ViewClients()
        {
            Browser.Click(s, "client.tab")
                .Wait(1);

            Assert.AreEqual("Clients", Browser.Title);
            Assert.IsTrue(Browser.ElementsVisible(s, "client.table"));
        }

        [TestMethod]
        public void CreateClient1()
        {
            var name = "Company " + rand.Next(int.MaxValue);

            Browser.Click(s, "client.tab")
                .Wait(1)
                .Click(s, "client.create.from-button")
                .DropdownSelectByText(s, "client.create.status", "New")
                .DropdownSelectByText(s, "client.create.respons", "Brian Sales Agent")
                .Click(s, "client.create.comp-detail")
                .FillForm(s, "client.create.comp", name)
                .Click(Common, "save-button")
                .Wait(2)
                .Click(s, "client.tab")
                .Wait(2);

            Assert.IsTrue(Browser.TextExists(name));
        }

        [TestMethod]
        public void CreateClient2()
        {
            var name = "Company " + rand.Next(int.MaxValue);

            Browser.MouseOver(s, "client.tab")
                .Click(s, "client.create.from-tab")
                .DropdownSelectByText(s, "client.create.status", "New")
                .DropdownSelectByText(s, "client.create.respons", "Brian Sales Agent")
                .Click(s, "client.create.comp-detail")
                .FillForm(s, "client.create.comp", name)
                .Click(Common, "save-button")
                .Wait(2)
                .Click(s, "client.tab")
                .Wait(2);

            Assert.IsTrue(Browser.TextExists(name));
        }

        [TestMethod]
        public void CreateClientRequired()
        {
            Browser.Click(s, "client.tab")
                .Wait(2)
                .Click(s, "client.create.from-button")
                .Wait(1)
                .Click(Common, "save-button");

            Assert.IsTrue(Browser.ElementCount(s, "required-message") == 3);
        }

        [TestMethod]
        public void CreateClientCancel()
        {
            Browser.MouseOver(s, "client.tab")
                .Click(s, "client.create.from-tab")
                .DropdownSelectByText(s, "client.create.status", "New")
                .DropdownSelectByText(s, "client.create.respons", "Brian Sales Agent")
                .Click(s, "client.create.comp-detail")
                .FillForm(s, "client.create.comp", "Cancel Client")
                .Click(s, "cancel-button");

            Assert.IsFalse(Browser.TextExists("Cancel Client"));
        }

        [TestMethod]
        public void ClientImport()
        {
            Browser.Click(s, "client.tab")
                .Wait(1)
                .Click(s, "client.import.button")
                .UploadFile(s, "client.import.file", "clientsamples.csv")
                .Click(s, "client.import.import")
                .DropdownSelectByText(s, "client.import.peg-field", "Company DBA Name")
                .Wait(1)
                .Click(s, "client.import.import2");

            Assert.IsTrue(Browser.FindElement(Common, "flash-message").Text
                .Contains("Records Imported Successfully."));
        }

        [TestMethod]
        public void ClientMerge()
        {
            Browser.Click(s, "client.tab")
                .Wait(1)
                .Click(s, "client.box1")
                .Click(s, "client.box2")
                .Click(s, "client.merge.button")
                .Wait(2)
                .Click(s, "client.merge.primary")
                .Click(s, "client.merge.merge")
                .Wait(1)
                .AlertAccept()
                .Wait(1);

            Assert.AreEqual("Merging Client(s) Completed Successfully.",
                Browser.FindElement(Common, "flash-message").Text);
        }
    }
}