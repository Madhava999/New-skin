using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.Util;
using OpenQA.Selenium.Support.UI;

namespace NewSkin.Tests
{
    /// <summary>
    ///     Test cases for the corporate module.
    /// </summary>
    [TestClass]
    public class CorporatePortal : BaseTest
    {
        private LocatorReader _corp;

        [TestInitialize]
        public void Initialize()
        {
            Browser = Pegasus.LoginCom("selcorp");
            _corp = new LocatorReader("CorporatePortal.xml");
        }

        [TestMethod]
        public void OfficePage()
        {
            Browser.MouseOver(_corp.Get("OfficeTab"))
                .Click(_corp.Get("OfficeTab2")).Wait(5);
        }

        [TestMethod]
        public void CreateOffice()
        {
            var randomOfficeName = "Apple " + new Random().Next(int.MaxValue);
            var a = new Random();
            var rand = a.Next(1000000);

            OfficePage();
            Browser.ImplicitWait = 5;
            Thread.Sleep(2000);
            Browser.Click(_corp.Get("Create"))
                .FillForm(_corp.Get("Name"), randomOfficeName)
                .DropdownSelectByText(_corp.Get("AddressType"), "Office")
                .FillForm(_corp.Get("AddressLine1"), "123 Main St")
                .FillForm(_corp.Get("ZipCode"), "30033")
                .FillForm(_corp.Get("UserName"), "Test" + rand)
                .FillForm(_corp.Get("ZipCode"), "30033");
            Thread.Sleep(3000);
            Browser.Click(_corp.Get("AutoGeneratePassword"))
                .FillForm(_corp.Get("Password"), "password")
                .FillForm(_corp.Get("FirstName"), "Lebron")
                .FillForm(_corp.Get("LastName"), "James")
                .FillForm(_corp.Get("eAddress"), "test@yahoo.com")
                .Click(_corp.Get("Save"));
            Thread.Sleep(2000);
            Assert.AreEqual("Office created successfully.",
                Browser.FindElement(Common.Get("flash-message")).Text);

            TestContext.Properties["LeadName"] = randomOfficeName;
            Thread.Sleep(2000);
        }

        [TestMethod]
        public void DeleteOffice()
        {
            CreateOffice();
            Thread.Sleep(2000);
            OfficePage();
            Browser.ImplicitWait = 5;
            Thread.Sleep(2000);
            Browser.Click(_corp.Get("SelectOffice", TestContext.Properties["LeadName"]))
                .Click(_corp.Get("DeleteOffice"))
                .Wait(2)
                .AlertAccept();
            Thread.Sleep(3000);
                Browser.Click(_corp.Get("Delete"));
            Thread.Sleep(2000);
            Assert.AreEqual("Office deleted successfully.",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }

        [TestMethod]
        public void SendEmailToOffice()
        {
            CreateOffice();
            OfficePage();
            Browser.ImplicitWait = 5;
            Thread.Sleep(5000);
            Browser.Click(_corp.Get("SelectOffice", TestContext.Properties["LeadName"]))
                .Click(_corp.Get("SendEmailOffice"))
                .FillForm(_corp.Get("EmailSubject"), "TestSubject")
                .Wait(3)
                .Click(_corp.Get("SendEmailButton"));
            Assert.AreEqual("Email Sent Successfully.",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }

        [TestMethod]
        public void AddNoteToOffice()
        {
            CreateOffice();
            Thread.Sleep(2000);
            OfficePage();
            Browser.ImplicitWait = 5;
            Browser.Click(_corp.Get("SelectOffice", TestContext.Properties["LeadName"]))
                .Wait(2)
                .Click(_corp.Get("AddNote"))
                .FillForm(_corp.Get("NoteSubjectName"), "TestSubject")
                .Wait(2)
                .Click(_corp.Get("Save3"));
            Assert.AreEqual("Note Created Successfully",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }

        [TestMethod]
        public void AddTaskToOffice()
        {
            CreateOffice();
            Thread.Sleep(2000);
            OfficePage();
            Browser.ImplicitWait = 5;
            Browser.Click(_corp.Get("SelectOffice", TestContext.Properties["LeadName"]))
                .Wait(2)
                .Click(_corp.Get("NewTask"));
            Thread.Sleep(2000);
            Browser.FillForm(_corp.Get("TaskSubject"), "TestSubject")
                .FillForm(_corp.Get("StartDate"), "07/25/2015")
                .Wait(2)
                .Click(_corp.Get("TaskSaveButton"));
            Assert.AreEqual("Task Created Successfully.",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }

        [TestMethod]
        public void AddDocumentToOffice()
        {
            CreateOffice();
            Thread.Sleep(2000);
            OfficePage();
            Browser.ImplicitWait = 5;
            Browser.Click(_corp.Get("SelectOffice", TestContext.Properties["LeadName"]))
                .Click(_corp.Get("AddDocument"));
            Thread.Sleep(2000);
            Browser.FillForm(_corp.Get("DocumentName"), "TestSubject")
                .UploadFile(_corp.Get("ChooseFile"), "leadsamples.csv")
                .Click(_corp.Get("DocumentSaveButton"));
            Assert.AreEqual("Documents successfully Added.",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }

        [TestMethod]
        public void CreateEmployee()
        {
            var a = new Random();
            var rand = a.Next(1000000);
            Browser.Click(_corp.Get("EmployeesTab"))
                .Click(_corp.Get("Create1"))
                .FillForm(_corp.Get("UserName"), "Test" + rand)
                .FillForm(_corp.Get("PrimaryEmail"), "Test@yahoo.com")
                .FillForm(_corp.Get("FirstName1"), "Bob")
                .FillForm(_corp.Get("LastName1"), "Jones")
                .Click(_corp.Get("CorporateAdminUser"))
                .FillForm(_corp.Get("PhoneNumber"), "5654565454")
                .FillForm(_corp.Get("eAddress1"), "Test@yahoo.com")
                .Click(_corp.Get("Save1"));
            Assert.AreEqual("Employee Created Successfully.",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }

        [TestMethod]
        public void EditEmployee()
        {
            var a = new Random();
            var rand = a.Next(1000000);
            Browser.Click(_corp.Get("EmployeesTab"));
            Thread.Sleep(2000);
            Browser.Click(_corp.Get("Edit"))
                .FillFormReplace(_corp.Get("LastName1"), "James" + rand)
                .Click(_corp.Get("Save1"));
            Assert.AreEqual("Employee Details successfully updated",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }

        [TestMethod]
        public void ExportUserList()
        {
            Browser.MouseOver(_corp.Get("OfficeTab"))
                .Click(_corp.Get("UserTab"))
                .Click(_corp.Get("Export"))
                .Click(_corp.Get("CSV"));
        }

        [TestMethod]
        public void MakeOfficeActiceOrInActice()
        {
            OfficePage();
            Browser.ImplicitWait = 5;
            Browser.Click(_corp.Get("ActiveInactive"));
            Thread.Sleep(2000);
            Browser.AlertAccept();
            Assert.AreEqual("Office is successfully deactivated",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }

        [TestMethod]
        public void ExportLookUpCodes()
        {
            Browser.MouseOver(_corp.Get("ResidualIncomeTab"))
                .MouseOver(_corp.Get("MasterData"))
                .Click(_corp.Get("OfficeLookupCode"))
                .Click(_corp.Get("Export1"));
        }

        [TestMethod]
        public void RevenueShareSetup()
        {
            Browser.MouseOver(_corp.Get("ResidualIncomeTab"))
                .MouseOver(_corp.Get("MasterData"))
                .Click(_corp.Get("RevenueShareSetup"));

            new SelectElement(Browser.FindElement(_corp, "ProcessorType")).SelectByIndex(1);
            Thread.Sleep(2000);
            Browser.FillForm(_corp.Get("RevenueSharePercentage"), "55")
                .Click(_corp.Get("ApplyToAll"))
                .Click(_corp.Get("Save2"));
            Assert.AreEqual("Master Revenue Share Created.",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }

        [TestMethod]
        public void ViewMerchants()
        {
            Browser.Click(_corp.Get("MerchantTab"));
            Thread.Sleep(2000);
            Browser.Click(_corp.Get("MerchantCompany"));
        }

        [TestMethod]
        public void EditExistingAvatar()
        {
            Browser.MouseOver(_corp.Get("SystemTab"))
                .Click(_corp.Get("Avatar"));
            Thread.Sleep(2000);
            Browser.Click(_corp.Get("EditAvatar"))
                .Click(_corp.Get("CreateTicket"))
                .Click(_corp.Get("Save1"));
            Assert.AreEqual("Avatar has been updated.",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }

        [TestMethod]
        public void CreateAvatar()
        {
            var a = new Random();
            var rand = a.Next(1000000);
            Browser.MouseOver(_corp.Get("SystemTab"))
                .Click(_corp.Get("Avatar"))
                .Click(_corp.Get("Create2"))
                .FillForm(_corp.Get("AvatarName"), "Test" + rand)
                .DropdownSelectByText(_corp.Get("UserType"), "Client")
                .Click(_corp.Get("Save1"));

            Assert.AreEqual("Avatar has been created.",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }

        [TestMethod]
        public void EditMerchantType()
        {
            var a = new Random();
            var rand = a.Next(100);
            Browser.MouseOver(_corp.Get("MasterData1"))
                .Click(_corp.Get("MerchantType"));
            Thread.Sleep(2000);
            Browser.Click(_corp.Get("CreateMerchant"))
                .FillForm(_corp.Get("MerchantTypeName"), "Test" + rand)
                .Click(_corp.Get("Save3"));
            Assert.AreEqual("The merchant type is successfully created!!",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }

        [TestMethod]
        public void EditPricingPlans()
        {
            var a = new Random();
            var rand = a.Next(100);
            Browser.MouseOver(_corp.Get("MasterData1"))
                .Click(_corp.Get("PricingPlans"));
            Thread.Sleep(2000);
            Browser.Click(_corp.Get("CreatePricingPlan"))
                .FillForm(_corp.Get("PricingPlanName"), "Test" + rand);
           
            new SelectElement(Browser.FindElement(_corp, "ProcessorNamePricing")).SelectByIndex(1);
            Browser.Click(_corp.Get("Save3"));
            Assert.AreEqual("The pricing plan is successfully created!!",
                Browser.FindElement(Common.Get("flash-message")).Text);
            Thread.Sleep(2000);
            Browser.Click(_corp.Get("PushToOffices"));
            Thread.Sleep(2000);
            Browser.AlertAccept();
            Thread.Sleep(2000);
            Assert.AreEqual("Pricing Plans successfully pushed to offices.",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }

        [TestMethod]
        public void CreateAmexRates()
        {
            var a = new Random();
            var rand = a.Next(10000);
            Browser.MouseOver(_corp.Get("MasterData1"))
                .Click(_corp.Get("AmexRates"));
            Thread.Sleep(2000);
            Browser.Click(_corp.Get("CreateAmexRate"))
                .FillForm(_corp.Get("MCCCode"), "T" + rand)
                .Click(_corp.Get("Save3"));
            Assert.AreEqual("The Amex Rates is successfully created!!",
                Browser.FindElement(Common.Get("flash-message")).Text);
            Thread.Sleep(2000);
            Browser.Click(_corp.Get("PushToOffices"));
            Thread.Sleep(2000);
            Browser.AlertAccept();
            Thread.Sleep(2000);
            Assert.AreEqual("Amex Rates successfully pushed to offices.",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }
        
        [TestMethod]
        public void EditOmahaAuthGrid()
        {
            var a = new Random();
            var rand = a.Next(100);
            Browser.MouseOver(_corp.Get("MasterData1"))
                .Click(_corp.Get("OmahaAuthGrids"));
            Thread.Sleep(2000);
            Browser.Click(_corp.Get("CreateOmahaAuth"));
            Thread.Sleep(2000);
            Browser.FillForm(_corp.Get("AuthGridId"), "" + rand)
               .FillForm(_corp.Get("MCPos"), "" + rand)
                .FillForm(_corp.Get("DiscPos"), "" + rand)
                .FillForm(_corp.Get("VoiceAuth"), "" + rand)
                .FillForm(_corp.Get("AVSVoice"), "" + rand)
                .FillForm(_corp.Get("VisaPos"), "" + rand)
                .FillForm(_corp.Get("AmexPos"), "" + rand)
                .FillForm(_corp.Get("JCBPos"), "" + rand)
                .FillForm(_corp.Get("AVSElect"), "" + rand)
                .FillForm(_corp.Get("ARU"), "" + rand)
                .Click(_corp.Get("Save3"));
            Assert.AreEqual("Corporate Master Omaha Auth Grid Created Successfully.",
                Browser.FindElement(Common.Get("flash-message")).Text);
            Thread.Sleep(2000);
            Browser.Click(_corp.Get("PushToOffices"));
            Thread.Sleep(1000);
            Browser.AlertAccept();
            Thread.Sleep(2000);
            Assert.AreEqual("Omaha Auth Grids successfully pushed to offices.",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }

        [TestMethod]
        public void EditOfficeUserLimit()
        {
            var a = new Random();
            var rand = a.Next(100);
            Browser.MouseOver(_corp.Get("MasterData1"))
                .Click(_corp.Get("UserLimit"));
            Thread.Sleep(2000);
            Browser.Click(_corp.Get("EditOfficeUserLimit"))
                .FillForm(_corp.Get("EditEmployees"), "" + rand)
                .Click(_corp.Get("Save4"));
            Assert.IsTrue(Browser.ElementsVisible(_corp.Get("EditOfficeUserLimit")));
        }

        [TestMethod]
        public void CreateLanguages()
        {
            var a = new Random();
            var rand = a.Next(100000);
            Browser.MouseOver(_corp.Get("MasterData1"))
                .Click(_corp.Get("Languages"));
            Thread.Sleep(2000);
            Browser.Click(_corp.Get("CreateLanguage"));
            Thread.Sleep(2000);
            Browser.FillForm(_corp.Get("LanguageName"), "test" + rand)
                .Click(_corp.Get("Save3"));
            Assert.AreEqual("Language Created Successfully",
                Browser.FindElement(Common.Get("flash-message")).Text);
            Browser.Click(_corp.Get("PushToOffices"));
            Thread.Sleep(2000);
            Browser.AlertAccept();
            Thread.Sleep(2000);
            Assert.AreEqual("Languges Successfully Pushed to Offices.",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }

        [TestMethod]
        public void AddPickList()
        {
            Browser.ImplicitWait = 5;
            var a = new Random();
            var rand = a.Next(100000);
            Browser.MouseOver(_corp.Get("SystemTab"))
                .Click(_corp.Get("PickList"));
            Thread.Sleep(2000);
            Browser.Click(_corp.Get("AddressType1"))
                .Click(_corp.Get("AddNewItem"));
            Thread.Sleep(2000);
            Browser.FillForm(_corp.Get("ItemName"), "test" + rand)
                .Click(_corp.Get("Save5"));
            Assert.IsTrue(Browser.FindElement(_corp, "Message").Displayed);
        }

        [TestMethod]
        public void InactivePickList()
        {
            Browser.ImplicitWait = 5;
            Browser.MouseOver(_corp.Get("SystemTab"))
                .Click(_corp.Get("PickList"));
            Thread.Sleep(2000);
            Browser.Click(_corp.Get("AddressType1"))
                .Wait(2)
                .Click(_corp.Get("InactivateButton"));
            Assert.AreNotEqual(_corp.Get("StatusText"),
                Browser.FindElement(_corp.Get("ActiveText")).Text);
        }
    }
}