using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewSkin.Util;
using System;
using System.Threading;


namespace NewSkin.Tests
{
    /// <summary>
    ///     Summary description for Contacts
    /// </summary>
    [TestClass]
    public class Contacts : BaseTest
    {
        private LocatorReader _contacts;
        
        [TestInitialize]
        public void Initialize()
        {
            Browser = Pegasus.LoginCom("tseaa");
            _contacts = new LocatorReader("Contacts.xml");
        }

        [TestMethod]
        public void TestContactLink()
        {
            Browser.Click(_contacts.Get("ContactTab"))
            .Wait(2);
            Assert.AreEqual("Contacts", Browser.Title);
        }
        [TestMethod]
        public void TestCreateContactLink()
        {
            Browser.MouseOver(_contacts.Get("ContactTab"));
            Browser.Click(_contacts.Get("CreateContactLink"))
                .Wait(2);

            Assert.AreEqual("Create a Contact", Browser.Title);
        }
        [TestMethod]
        public void TestImportContactLink()
        {
            Browser.MouseOver(_contacts.Get("ContactTab"));
            Browser.Click(_contacts.Get("ImportContactLink"))
                .Wait(2);
            Assert.AreEqual("Contacts", Browser.Title);
        }
        [TestMethod]
        public void TestExportContactLink()
        {
            Browser.MouseOver(_contacts.Get("ContactTab"));
            Browser.Click(_contacts.Get("ExportContactLink"))
                .Wait(2);
            Assert.AreEqual("Contacts", Browser.Title);
        }
        [TestMethod]
        public void TestRecycleBinLink()
        {
            Browser.MouseOver(_contacts.Get("ContactTab"));
            Browser.Click(_contacts.Get("RecycleBinLink"))
                .Wait(2);
            Assert.AreEqual("Recycled Contacts", Browser.Title);
        }
        [TestMethod]
        public void CreateContact()
        {
            var randomName = "Lebron " + new Random().Next(int.MaxValue);
            var a = new Random();
            var rand = a.Next(int.MaxValue);
            var phoneNumber = "";
            for (var i = 0; i < 10; ++i)
            {
                phoneNumber += a.Next(10);
            }


            TestCreateContactLink();
            Browser.ImplicitWait = 10;
            Browser.FillForm(_contacts.Get("FirstNameField"), randomName)
                .FillForm(_contacts.Get("LastNameField"), "James"+rand)
                .FillForm(_contacts.Get("CompanyNameField"), "Cavs"+rand)
                .DropdownSelectByText(_contacts.Get("PhoneTypeField"), "Cell")
                .FillForm(_contacts.Get("PhoneNumberField"), phoneNumber)
                .FillForm(_contacts.Get("eAddressField"), "KingJames"+rand+"@yahoo.com")
                .FillForm(_contacts.Get("AddressLineField"), rand+"Main St")
                .FillForm(_contacts.Get("ZipCodeField"), "30033");
            Thread.Sleep(5000);
                Browser.Click(_contacts.Get("Save"));
                Assert.AreEqual("A Contact has been created. .",
                Browser.FindElement(Common.Get("flash-message")).Text);
            TestContext.Properties["LeadName"] = randomName;
        }

        [TestMethod]
        public void CancelContact()
        {
            TestCreateContactLink();
            Browser.ImplicitWait = 10;
            Browser.Click(_contacts.Get("Cancel"))
                .Wait(2);
            Assert.AreEqual("Contacts", Browser.Title);
        }
        [TestMethod]
        public void ExportContactAsCSV()
        {
            TestContactLink();
            Thread.Sleep(1000);
            Browser.Click(_contacts.Get("SelectAllContactsButton"))
                  .Click(_contacts.Get("ExportButton"));
            Thread.Sleep(1000);
            Browser.Click(_contacts.Get("ExportAsCSV"));
            Thread.Sleep(1000);
        }
        [TestMethod]
        public void ExportFirstLastName()
        {
            TestExportContactLink();
            Thread.Sleep(1000);
            Browser.Click(_contacts.Get("FirstNameExport"))
                .Click(_contacts.Get("LastNameExport"))
                .Click(_contacts.Get("Cancel"));
            Assert.AreEqual("Contacts", Browser.Title);

        }

        [TestMethod]
        public void UploadFile()
        {
            TestImportContactLink();
            Browser.ImplicitWait = 5;
            Browser.UploadFile(_contacts.Get("ChooseFile"), "contactsamples.csv")
                .Click(_contacts.Get("ImportButton"));
            Thread.Sleep(1000);
            Assert.AreEqual("Duplicate records has been found",
                Browser.FindElement(_contacts.Get("DuplicateRecordsMessage")).Text);

        }
        [TestMethod]
        public void RecycleBin()
        {
            CreateContact();
            TestContactLink();
            Browser.ImplicitWait = 5;
            Browser.Click(_contacts.Get("SelectContact", TestContext.Properties["LeadName"]))
                 .Click(_contacts.Get("RecyclebinButton"))
                 .Wait(2);
            Assert.AreEqual("Recycled Contacts", Browser.Title);
        }
        [TestMethod]
        public void UploadPDF()
        {
            TestImportContactLink();
            Browser.ImplicitWait = 5;
            Browser.UploadFile(_contacts.Get("ChooseFile"), "FNP_MPA.pdf")
               .Click(_contacts.Get("ImportButton"));
            Thread.Sleep(2000);
            Browser.AlertAccept();
            Assert.AreEqual("Contacts", Browser.Title);

        }
        [TestMethod]
        public void UploadExcel()
        {
            TestImportContactLink();
            Browser.ImplicitWait = 5;
            Browser.UploadFile(_contacts.Get("ChooseFile"), "ContactTestCases.xlsx")
               .Click(_contacts.Get("ImportButton"));
            Thread.Sleep(2000);
            Browser.AlertAccept();
            Assert.AreEqual("Contacts", Browser.Title);

        }
        [TestMethod]
        public void RecentlyAddedContacts()
        {
            TestContactLink();
            Browser.ImplicitWait = 5;
            Browser.MouseOver(_contacts.Get("MyContactsButton"))
                .Click(_contacts.Get("RecentlyAddedContacts"));
            Assert.AreEqual("Contacts", Browser.Title);
        }
        [TestMethod]
        public void EditContactIcon()
        {
            TestContactLink();
            Browser.ImplicitWait = 5;
            Browser.Click(_contacts.Get("EditContactButton"))
                .Wait(2);
            Assert.AreEqual("Edit Contact", Browser.Title);
        }
        [TestMethod]
        public void EditingContact()
        {
            var a = new System.Random();
            var rand = a.Next(10000);

            TestContactLink();
            Browser.ImplicitWait = 5;
            Browser.Click(_contacts.Get("EditContactButton"))
                .FillForm(_contacts.Get("FirstNameField"), "Michael" + rand)
                .FillForm(_contacts.Get("LastNameField"), "Jordan" + rand)
                .FillForm(_contacts.Get("CompanyNameField"), "Bulls" + rand)
                .DropdownSelectByText(_contacts.Get("PhoneTypeField"), "Cell");
            Thread.Sleep(1000);
            Browser.Click(_contacts.Get("Save"));
            Assert.AreEqual("Contact has been updated.",
                Browser.FindElement(Common.Get("flash-message")).Text);

        }
        [TestMethod]
        public void ExportContactAsExcel()
        {
            TestContactLink();
            Thread.Sleep(1000);
            Browser.Click(_contacts.Get("SelectAllContactsButton"))
                  .Click(_contacts.Get("ExportButton"));
            Thread.Sleep(1000);
            Browser.Click(_contacts.Get("ExportAsExcel"));
            Thread.Sleep(1000);
        }
        [TestMethod]
        public void CreateDuplicateContactSameName()
        {
            var a = new System.Random();
            var rand = a.Next(10000);
            var phoneNumber = "";
            for (var i = 0; i < 10; ++i)
            {
                phoneNumber += a.Next(10);
            }
            TestCreateContactLink();
            Browser.ImplicitWait = 10;
            Browser.FillForm(_contacts.Get("FirstNameField"), "Lebron27")
                .FillForm(_contacts.Get("LastNameField"), "James27")
                .FillForm(_contacts.Get("CompanyNameField"), "Cavs"+rand)
                .DropdownSelectByText(_contacts.Get("PhoneTypeField"), "Cell")
                .FillForm(_contacts.Get("PhoneNumberField"), phoneNumber)
                .FillForm(_contacts.Get("eAddressField"), "KingJames"+rand+"@yahoo.com")
                .FillForm(_contacts.Get("AddressLineField"), rand+"Main St")
                .FillForm(_contacts.Get("ZipCodeField"), "30033");
            Thread.Sleep(5000);
            Browser.Click(_contacts.Get("Save"));
            Thread.Sleep(3000);
            Assert.IsTrue(Browser.ElementsVisible(_contacts.Get("CreateDuplicateButton")));
        }
        [TestMethod]
        public void CreateDuplicateContactSameCompanyName()
        {
            var a = new System.Random();
            var rand = a.Next(10000);
            var phoneNumber = "";
            for (var i = 0; i < 10; ++i)
            {
                phoneNumber += a.Next(10);
            }
            TestCreateContactLink();
            Browser.ImplicitWait = 10;
            Browser.FillForm(_contacts.Get("FirstNameField"), "Lebron"+rand)
                .FillForm(_contacts.Get("LastNameField"), "James"+rand)
                .FillForm(_contacts.Get("CompanyNameField"), "Cavs27")
                .DropdownSelectByText(_contacts.Get("PhoneTypeField"), "Cell")
                .FillForm(_contacts.Get("PhoneNumberField"), phoneNumber)
                .FillForm(_contacts.Get("eAddressField"), "KingJames"+rand+"@yahoo.com")
                .FillForm(_contacts.Get("AddressLineField"), rand+"Main St")
                .FillForm(_contacts.Get("ZipCodeField"), "30033");
            Thread.Sleep(5000);
            Browser.Click(_contacts.Get("Save"));
            Thread.Sleep(3000);
            Assert.IsTrue(Browser.ElementsVisible(_contacts.Get("CreateDuplicateButton")));
        }
        [TestMethod]
        public void CreateDuplicateContactSamePhone()
        {
            var a = new System.Random();
            var rand = a.Next(10000);

            TestCreateContactLink();
            Browser.ImplicitWait = 10;
            Browser.FillForm(_contacts.Get("FirstNameField"), "Lebron"+rand)
                .FillForm(_contacts.Get("LastNameField"), "James"+rand)
                .FillForm(_contacts.Get("CompanyNameField"), "Cavs"+rand)
                .DropdownSelectByText(_contacts.Get("PhoneTypeField"), "Cell")
                .FillForm(_contacts.Get("PhoneNumberField"), "1111111111")
                .FillForm(_contacts.Get("eAddressField"), "KingJames"+rand+"@yahoo.com")
                .FillForm(_contacts.Get("AddressLineField"), rand+"Main St")
                .FillForm(_contacts.Get("ZipCodeField"), "30033");
            Thread.Sleep(5000);
            Browser.Click(_contacts.Get("Save"));
            Thread.Sleep(3000);
            Assert.IsTrue(Browser.ElementsVisible(_contacts.Get("CreateDuplicateButton")));
        }
        [TestMethod]
        public void CreateDuplicateContactSameEmail()
        {
            var a = new System.Random();
            var rand = a.Next(10000);
            var phoneNumber = "";
            for (var i = 0; i < 10; ++i)
            {
                phoneNumber += a.Next(10);
            }
            TestCreateContactLink();
            Browser.ImplicitWait = 10;
            Browser.FillForm(_contacts.Get("FirstNameField"), "Lebron" + rand)
                .FillForm(_contacts.Get("LastNameField"), "James" + rand)
                .FillForm(_contacts.Get("CompanyNameField"), "Cavs" + rand)
                .DropdownSelectByText(_contacts.Get("PhoneTypeField"), "Cell")
                .FillForm(_contacts.Get("PhoneNumberField"), phoneNumber)
                .FillForm(_contacts.Get("eAddressField"), "KingJames27@yahoo.com")
                .FillForm(_contacts.Get("AddressLineField"), rand+"Main St")
                .FillForm(_contacts.Get("ZipCodeField"), "30033");
            Thread.Sleep(5000);
            Browser.Click(_contacts.Get("Save"));
            Thread.Sleep(3000);
            Assert.AreEqual("A Contact has been created. .",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }
        [TestMethod]
        public void CreateDuplicateContactSameAddress()
        {
            var a = new Random();
            int rand = a.Next(1000, 1000000000);

            var phoneNumber = "";
            for (var i = 0; i < 10; ++i)
            {
                phoneNumber += a.Next(10);
            }

            TestCreateContactLink();
            Browser.ImplicitWait = 10;
            Browser.FillForm(_contacts.Get("FirstNameField"), "Lebron" + rand)
                .FillForm(_contacts.Get("LastNameField"), "James" + rand)
                .FillForm(_contacts.Get("CompanyNameField"), "Cavs" + rand)
                .DropdownSelectByText(_contacts.Get("PhoneTypeField"), "Cell")
                .FillForm(_contacts.Get("PhoneNumberField"), phoneNumber)
                .FillForm(_contacts.Get("eAddressField"), "KingJames"+rand+"@yahoo.com")
                .FillForm(_contacts.Get("AddressLineField"), "27 Main St")
                .FillForm(_contacts.Get("ZipCodeField"), "30033");
            Thread.Sleep(5000);
            Browser.Click(_contacts.Get("Save"));
            Thread.Sleep(3000);
            Assert.AreEqual("A Contact has been created. .",
                Browser.FindElement(Common.Get("flash-message")).Text);
        }

    }
}
