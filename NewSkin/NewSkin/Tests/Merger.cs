using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.Util;
using OpenQA.Selenium;
using System;
using System.Threading;


namespace NewSkin.Tests
{
    /// <summary>
    ///     Summary description for Merger
    /// </summary>
    [TestClass]
    public class Merger : BaseTest
    {
        private LocatorReader _merger;

        [TestInitialize]
        public void Initialize()
        {
            Browser = Pegasus.LoginCom("tseaa");
            _merger = new LocatorReader("Merger.xml");
        }
        [TestMethod]
        public void CreateClient()
        {
            CreateClient(0);
        }

        private void CreateClient(int id)
        {
          
            var randomName = "Client " + new Random().Next(int.MaxValue);

            Browser.MouseOver(_merger.Get("ClientsTab"))
                .Click(_merger.Get("CreateClient"));
            Thread.Sleep(2000);
                Browser.DropdownSelectByText(_merger.Get("Status"), "New")
                 .DropdownSelectByText(_merger.Get("Responsibility"), "Test Test")
                 .Click(_merger.Get("CompanyDetailsTab"))
                 .FillForm(_merger.Get("CompanyDBAName"), randomName)
               // .FillForm(_merger.Get("Location"), "Atlanta")
                .Click(_merger.Get("Save"));
            Thread.Sleep(2000);
            Assert.AreEqual("Client saved successfully.",
                Browser.FindElement(Common.Get("flash-message")).Text);

            TestContext.Properties["client " + id] = randomName;
        }


        [TestMethod]
        public void TestClientsPage()
        {
            Browser.Click(_merger.Get("ClientsTab"));
            Assert.AreEqual("Clients", Browser.Title);
        }
        [TestMethod]
        public void TestMerge2ClientRecords()
        {
            CreateClient(1);
            CreateClient(2);
            TestClientsPage();

            Browser.ImplicitWait = 5;

            var xpath = "//tr[.//td[@title=\"{0}\"]]//input[@role=\"checkbox\"]";
            var checkbox1 = string.Format(xpath, TestContext.Properties["client 1"]);
            var checkbox2 = string.Format(xpath, TestContext.Properties["client 2"]);

            Browser.Click(By.XPath(checkbox1))
                .Click(By.XPath(checkbox2))
                .Click(_merger.Get("MergeRecordsButton"));
            Thread.Sleep(3000);
            Assert.IsTrue(Browser.ElementsVisible(_merger.Get("ClientMergeForm")));

        }
        [TestMethod]
        public void TestMerge1ClientRecord()
        {
            CreateClient(1);
            TestClientsPage();
            Browser.ImplicitWait = 5;
            Browser.Click(_merger.Get("ClientCheckbox", TestContext.Properties["client 1"]))
                .Click(_merger.Get("MergeRecordsButton"));
            Assert.AreEqual("Please select 2 or more clients you wish to merge", Browser.AlertText);
            Browser.AlertAccept();
        }
        [TestMethod]
        public void TestMerge0ClientRecord()
        {
            TestClientsPage();
            Browser.ImplicitWait = 5;
            Browser.Click(_merger.Get("MergeRecordsButton"));
            Assert.AreEqual("Please select 2 or more clients you wish to merge", Browser.AlertText);
            Browser.AlertAccept();

        }
        [TestMethod]
        public void TestMergeCancelButton()
        {
            CreateClient(1);
            CreateClient(2);
            TestClientsPage();

            Browser.ImplicitWait = 5;

            var xpath = "//tr[.//td[@title=\"{0}\"]]//input[@role=\"checkbox\"]";
            var checkbox1 = string.Format(xpath, TestContext.Properties["client 1"]);
            var checkbox2 = string.Format(xpath, TestContext.Properties["client 2"]);

            Browser.Click(By.XPath(checkbox1))
                .Click(By.XPath(checkbox2))
                .Click(_merger.Get("MergeRecordsButton"));
            Thread.Sleep(3000);
                Browser.Click(_merger.Get("CancelButton"));
            Assert.IsTrue((Browser.ElementsVisible(By.XPath(checkbox1)))&& Browser.ElementsVisible(By.XPath(checkbox2)));
        
        }

        [TestMethod]
        public void TestSelectPrimaryRecord()
        {
            TestMerge2ClientRecords();

            var name = (string)TestContext.Properties["client 1"];
            var xpath = "//div[contains(@class, 'box-pad') and .//h4[text()='{0}']]" +
                "//input[@type='radio']";

            var checkbox = By.XPath(string.Format(xpath, name));

            Browser.Click(checkbox)
                .Click(_merger.Get("MergeButton"));
            Assert.AreEqual("Records will be merged into the primary Record. This cannot be undone", Browser.AlertText);
            Browser.AlertAccept();
            Assert.AreEqual("Merging Client(s) Completed Successfully.",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }
        [TestMethod]
        public void CancelPrimaryRecord()
        {
            TestMerge2ClientRecords();

            var name = (string)TestContext.Properties["client 1"];
            var xpath = "//div[contains(@class, 'box-pad') and .//h4[text()='{0}']]" +
                "//input[@type='radio']";

            var checkbox = By.XPath(string.Format(xpath, name));

            Browser.Click(checkbox)
                .Click(_merger.Get("MergeButton"));
            Assert.AreEqual("Records will be merged into the primary Record. This cannot be undone", Browser.AlertText);
            Browser.AlertDismiss();
            Assert.IsTrue(Browser.ElementsVisible(_merger.Get("ClientMergeForm")));


        }
        [TestMethod]
        public void ViewMergedRecord()
        {
            TestSelectPrimaryRecord();
            Thread.Sleep(1000);

            var name = (string)TestContext.Properties["client 1"];
            var xpath = "//a[text()='{0}']";
            var link = string.Format(xpath, name);
            Browser.Click(By.XPath(link));
            Assert.AreEqual(name + " - Details", Browser.Title);
        }
        public void MergeRecordWithRateFees(int id)
        {
            var randomName = "Client " + new Random().Next(int.MaxValue);

            Browser.MouseOver(_merger.Get("ClientsTab"))
                .Click(_merger.Get("CreateClient"))
                .DropdownSelectByText(_merger.Get("Status"), "New")
                .DropdownSelectByText(_merger.Get("Responsibility"), "Test Test")
                .Click(_merger.Get("CompanyDetailsTab"))
                .FillForm(_merger.Get("CompanyDBAName"), randomName)
                .Click(_merger.Get("Save"));
            Thread.Sleep(2000);
            Assert.AreEqual("Client saved successfully.",
                Browser.FindElement(Common.Get("flash-message")).Text);

            TestContext.Properties["client " + id] = randomName;

            Browser.MouseOver(_merger.Get("ClientsTab"))
               .Click(_merger.Get("CreateClient"))
               .DropdownSelectByText(_merger.Get("Status"), "New")
               .DropdownSelectByText(_merger.Get("Responsibility"), "Test Test")
               .Click(_merger.Get("CompanyDetailsTab"))
               .FillForm(_merger.Get("CompanyDBAName"), randomName)
               .Click(_merger.Get("Save"));
            Thread.Sleep(2000);
            Assert.AreEqual("Client saved successfully.",
                Browser.FindElement(Common.Get("flash-message")).Text);

            TestContext.Properties["client " + id] = randomName;

        }


    }
}