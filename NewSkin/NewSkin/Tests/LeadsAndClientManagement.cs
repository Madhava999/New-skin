using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.Util;
using System;
using System.Threading;
using OpenQA.Selenium;


namespace NewSkin.Tests
{
    /// <summary>
    ///     Summary description for Contacts
    /// </summary> b
    [TestClass]
    public class LeadClientManagement : BaseTest
    {
        private LocatorReader _management;

        [TestInitialize]
        public void Initialize()
        {
            Browser = Pegasus.LoginCom("tseaa");
            _management = new LocatorReader("LeadClientManagement.xml");
        }

        [TestMethod]
        public void  LeadsPage()
        {
            Browser.Click(_management.Get("Leads"));
            Assert.AreEqual("Leads", Browser.Title);
        }

        [TestMethod]
        public void CreateLead()
        {
            var randomCompanyName = "Apple Inc " + new Random().Next(int.MaxValue);

            var a = new System.Random();
            var rand = a.Next(10000);
            LeadsPage();
            Browser.Click(_management.Get("CreateButton"))
                .DropdownSelectByText(_management.Get("Status"), "In Process")
                .DropdownSelectByText(_management.Get("Responsibility"), "jimmy m")
                .Click(_management.Get("CompanyDetailsTab"))
                .FillForm(_management.Get("FirstName"), "Steve" + rand)
                .FillForm(_management.Get("LastName"), "Jobs" + rand)
                .FillForm(_management.Get("CompanyName"), randomCompanyName)
                .Click(_management.Get("Save"));
            Thread.Sleep(2000);
            Assert.AreEqual("Lead saved successfully. .",
                Browser.FindElement(Common.Get("flash-message")).Text);

            TestContext.Properties["LeadName"] = randomCompanyName;

        }

        [TestMethod]
        public void VerifyConvertingLead()
        {
            CreateLead();
            LeadsPage();
            Browser.ImplicitWait = 5;
            Browser.Click(_management.Get("SelectLead", TestContext.Properties["LeadName"]))
                   .Click(_management.Get("ConvertButton"));
            Thread.Sleep(2000);
            Assert.IsTrue(Browser.ElementsVisible(_management.Get("ConvertLeadPopUp")));
        }

        [TestMethod]
        public void ConvertLeadwithNotesandNoRecycle()
        {
            VerifyConvertingLead();
            Browser.Click(_management.Get("NoRecycleBin"))
                .Click(_management.Get("SaveButton"));
            Thread.Sleep(3000);
            Assert.AreEqual("Lead is converted successfully.",
               Browser.FindElement(Common.Get("flash-message")).Text);
        }

        [TestMethod]
        public void ConvertLeadwithNotesandYesRecycle()
        {
            VerifyConvertingLead();
            Browser.ImplicitWait = 5;
            Browser.Click(_management.Get("SaveButton"));
            Thread.Sleep(1000);
            Assert.AreEqual("Lead is converted and moved to recyclebin.",
               Browser.FindElement(Common.Get("flash-message")).Text);
        }

        [TestMethod]
        public void ImportLeads()
        {
            Browser.MouseOver(_management.Get("Leads"))
                .Click(_management.Get("ImportLeads"));
            Thread.Sleep(1000);
                Browser.UploadFile(_management.Get("ChooseFile"), "leadsamples.csv")
                .Click(_management.Get("ImportButton"));
            Thread.Sleep(2000);
            Browser.Click(_management.Get("Radio"))
                .Click(_management.Get("MergeButton"))
                .AlertAccept();
            Thread.Sleep(2000);
            Assert.AreEqual("Merging Lead(s) Completed Successfully.",
              Browser.FindElement(Common.Get("flash-message")).Text);
        }
        [TestMethod]
        public void ExportLeads()
        {
            Browser.MouseOver(_management.Get("Leads"))
                .Click(_management.Get("ExportLeads"))
                .Click(_management.Get("CheckAll"))
            .Click(_management.Get("ExportButton"));
            Thread.Sleep(3000);
        }
        [TestMethod]
        public void RestoreLead()
        {
            Browser.MouseOver(_management.Get("Leads"))
                .Click(_management.Get("RecycleBin"));
            Thread.Sleep(3000);
                Browser.Click(_management.Get("RestoreLead"));
            Assert.AreEqual("Lead Restored Successfully.",
             Browser.FindElement(Common.Get("flash-message")).Text);
        }
        //Cancel button is no longer there
       /* [TestMethod]
        public void CancelImport()
        {
            Browser.MouseOver(_management.Get("Leads"))
                .Click(_management.Get("ImportLeads"));
            Thread.Sleep(1000);
            Browser.UploadFile(_management.Get("ChooseFile"), "leadsamples.csv")
            .Click(_management.Get("ImportButton"));
            Thread.Sleep(2000);
            Browser.Click(_management.Get("CancelButton"));
            Assert.IsTrue(Browser.ElementsVisible(_management.Get("UploadImportFile")));
        }*/
        [TestMethod]
        public void CancelButtonConvertLead()
        {
            VerifyConvertingLead();
            Browser.Click(_management.Get("CloseButton"));
            Assert.IsTrue(Browser.ElementsVisible(_management.Get("Leads")));

        }
        [TestMethod]
        public void XButtonConvertLead()
        {
            VerifyConvertingLead();
            Browser.Click(_management.Get("XButton"));
            Assert.IsTrue(Browser.ElementsVisible(_management.Get("Leads")));

        }
        [TestMethod]
        public void DownloadCSVTemplate()
        {
            Browser.MouseOver(_management.Get("Leads"))
                .Click(_management.Get("ImportLeads"))
                .Click(_management.Get("DownloadTemplate"));
            Thread.Sleep(1000);
        }
        [TestMethod]
        public void SelectingOnePrimaryRecord()
        {
            Browser.MouseOver(_management.Get("Leads"))
               .Click(_management.Get("ImportLeads"));
            Thread.Sleep(1000);
            Browser.UploadFile(_management.Get("ChooseFile"), "leadsamples.csv")
            .Click(_management.Get("ImportButton"));
            Thread.Sleep(2000);
            Browser.Click(_management.Get("Radio"))
                .Click(_management.Get("SecondRadio"))
                .Click(_management.Get("MergeButton"))
                .AlertAccept();
            Thread.Sleep(2000);
            Assert.AreEqual("Merging Lead(s) Completed Successfully.",
              Browser.FindElement(Common.Get("flash-message")).Text);
        }
        [TestMethod]
        public void UploadNonCSVFile()
        {
            Browser.MouseOver(_management.Get("Leads"))
               .Click(_management.Get("ImportLeads"));
            Thread.Sleep(1000);
            Browser.UploadFile(_management.Get("ChooseFile"), "emptyfile")
            .Click(_management.Get("ImportButton"));
            Thread.Sleep(2000);
            Assert.AreEqual("Please upload csv files only", Browser.AlertText);
        }
        [TestMethod]
        public void CancelMerge()
        {
            Browser.MouseOver(_management.Get("Leads"))
                .Click(_management.Get("ImportLeads"));
            Thread.Sleep(1000);
            Browser.UploadFile(_management.Get("ChooseFile"), "leadsamples.csv")
            .Click(_management.Get("ImportButton"));
            Thread.Sleep(2000);
            Browser.Click(_management.Get("Radio"))
                .Click(_management.Get("CancelButton"));
            Assert.IsTrue(Browser.ElementsVisible(_management.Get("Leads")));
        }
    }
}
